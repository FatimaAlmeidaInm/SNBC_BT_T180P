using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;



namespace KioskController.Hardware.ReceiptPrinter.SNBC
{
    public partial class BT : ReceiptPrinter
    {
        // STATUS
        #region  
        public override string PrinterName => "BT_180P";


        private const int STATUS_START_DELAY = 5000;        // Status check loop start in msec
        private const int STATUS_CHECK_WAIT = 5000;         // Status check loop in msec
        private const int STATUS_RESET_WAIT = 5000;         // Msec between reset free and init
        private const int STATUS_REOPEN_WAIT = 15000;       // Msec between resets
        private const int FREE_THREAD_WAIT = 2500;          // Msec wait thread cleaning before forced stop
        private const int INI_MAX_SIZE = 1024;              // maximum allowed size for read buffers or content storage
        private bool FEED_LOW_ERROR = true;                 // Paper feed before a "low paper warn" sould be reported as an error
        private bool CRASH = false;                         // Crash unit not inhibit
        #endregion
        // PROPERTIES
        #region
        StringBuilder _deviceInfo = new StringBuilder();

        private string _format = string.Empty;
        private string _bold = string.Empty;
        private string _alignment = string.Empty;
        private string _font = "Bold=0|HScale=1|VScale=1";
        private string _underLine = string.Empty;
        private string _reverse = string.Empty;
        private string _italics = string.Empty;
        private string _width = string.Empty;

        private string _lines = string.Empty;

        private string _serialNumber = String.Empty;
        private string _portInfo = string.Empty;

        private bool BT_open;
        private bool BT_cut;
        private bool BT_start;
        private bool BT_status;
        private int _status = 0;
        private Thread BT_statusThread;

        private static int _devHandle = 0;

        private const string _modelName = "BTP-U80 PLUS";
        private const string images_path = ".\\Resources\\Images\\";
        private const string image_3 = "bfa_receipt.bmp";

        private string timeNow = DateTime.Now.ToString("HH:mm:ss");
        private string dateNow = DateTime.Now.ToString("yyyy-MM-dd");

        #endregion
        public override bool Init(bool firstTime)
        {
            // First Init -> Copy Dlls
            if (firstTime)
            {
                string dir = "SNBC";
                Log.File(PREFIX, dir);
                Utils.DirFilesCopy(dir, "BT_DLL", true);
            }

            // Init
            return Init(firstTime, false);
        }
        private bool Init(bool firstTime, bool reset)
        {
            // Reset
            _ready = false;

            // Log
            if (App.Internal) Log.Show(PREFIX, "SNBC.BT (" + firstTime + ", " + reset + ")");


            // Config: [configs] -> False: ReceiptPrinterStatus + Init finish
            if (!SetupConfig(firstTime))
            {
                ReceiptPrinterStatus = new ReceiptPrinterStatus(ReceiptPrinterCode.ERROR_CONFIG);
                return true;
            }

            //Crash check -> False: ReceiptPrinterStatus + Init finish
            if (CRASH && (App.CrashCount >= App.CRASH_UNIT_INHIBIT))
            {
                ReceiptPrinterStatus = new ReceiptPrinterStatus(ReceiptPrinterCode.ERROR_CONFIG, "Crash inhibit (" + App.CrashCount + ")");
                return true;
            }

            if (!Start())
            {
                ReceiptPrinterStatus = new ReceiptPrinterStatus(ReceiptPrinterCode.ERROR_CALL, "Start failed");

            }

            // Init actions
            if (!reset)
            {
                // Check status -> False: Retry init
                if (!CheckStatus(true))
                {
                    Free(reset);
                    return false;
                }

                // Status check loop start
                try
                {
                    BT_statusThread = Utils.ThreadStartE(() =>
                    {
                        BT_status = true;
                        CheckStatus_Thread();
                    });
                }
                catch (Exception e)
                {
                    ReceiptPrinterStatus = new ReceiptPrinterStatus(ReceiptPrinterCode.ERROR_CALL, "Status listner failed", e);
                    Free(reset);
                    return false;
                }
            }

            // Ready
            _ready = true;

            // Log status
            SetupStatus(null);

            // Init finish
            return true;
        }

        public override Dictionary<string, string> Info(bool doRead, bool textOnly)    // doRead used
        {
            Dictionary<string, string> info = null;
            Utils.Add(ref info, "Status", StatusDetail + ((Active && !Ready) ? " (not ready)" : null));
            Utils.Add(ref info, "Unit", _Config.Path);
            Utils.Add(ref info, "Config", KioskUtils.InfoSend_Css(_Config.Config, "style_narrow", textOnly));
            Utils.Add(ref info, "SerialNumber", _Config.Path);

            return info;
        }

        // SETUP
        // TODO: override SetupConfig

        // UNIT
        // TODO: override Clear()

        public override bool Free()
        {
            return Free(false);
        }
        private bool Free(bool reset)
        {
            try
            {

                Log.Show(PREFIX, "Impressora desconectada com sucesso.");
                return true;
            }

            catch (Exception ex)
            {
                Log.Show(PREFIX, "Erro ao liberar a impressora: " + ex.Message);
                return false;
            }


        }
        // PRINT
        public override bool Start()
        {
            try
            {
                // Enumera dispositivos conectados
                StringBuilder enumList = new StringBuilder(INI_MAX_SIZE);
                int enumResult = EnumDevice(1, enumList, INI_MAX_SIZE);  // Certifique-se de que o tipo de conexão (1 para USB, por exemplo) esteja correto

                if (enumResult < 1) // Verifica se a enumeração encontrou dispositivos
                {
                    Log.Show(PREFIX, $"Erro ao enumerar dispositivos. Código de erro: {enumResult}");
                    return false;
                }

                // Obtém informações do dispositivo (por exemplo, porta e modelo)
                string[] devices = enumList.ToString().Split('@');
                if (devices.Length < 1 || string.IsNullOrWhiteSpace(devices[0]))
                {
                    Log.Show(PREFIX, "Nenhum dispositivo válido encontrado na enumeração.");
                    return false;
                }
                _portInfo = devices[0]; // Assume que o primeiro dispositivo na lista é o desejado

                // Tenta abrir a conexão com a impressora
                int result = OpenPrinter(_modelName, _portInfo);
                if (result != 0) // Verifica se a conexão foi bem-sucedida
                {
                    Log.Show(PREFIX, $"Erro ao conectar com a impressora. Código de erro: {result}");
                    return false;
                }

                // Verifica o status da impressora
                StringBuilder status = new StringBuilder(128);
                result = QueryStatus(_devHandle, status);
                if (result != 0)
                {
                    Log.Show(PREFIX, $"Erro ao consultar o status da impressora. Código de erro: {result}");
                    return false;
                }

                // Verifica se a impressora está pronta
                string statusText = status.ToString();
                if (!statusText.Contains("Ready"))
                {
                    Log.Show(PREFIX, $"A impressora não está pronta. Status: {statusText}");
                    return false;
                }

                Log.Show(PREFIX, "Impressora inicializada com sucesso e está pronta para uso.");
                return true; // Inicialização bem-sucedida
            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, $"Erro inesperado ao iniciar a impressora: {ex.Message}");
                return false;
            }
        }

        public override void Finish()
        {
            try
            {
                // Verifica se o handle da impressora é válido antes de tentar finalizar
                if (_devHandle == 0)
                {
                    Log.Show(PREFIX, "Erro: A impressora não está inicializada ou já foi finalizada.");
                    return;
                }

                // Realiza o corte de papel, se necessário
                if (BT_cut)
                {
                    int cutResult = PaperCut(_devHandle, 0, 0); // 0,0 como parâmetros para corte total
                    if (cutResult != 0) // Verifica se o corte foi bem-sucedido
                    {
                        Log.Show(PREFIX, $"Erro ao realizar o corte de papel. Código de erro: {cutResult}");
                    }
                    BT_cut = false; // Reseta o estado de corte
                }

                // Fecha a conexão com a impressora
                int closeResult = ClosePrinter(_devHandle);
                if (closeResult != 0) // 0 indica sucesso
                {
                    Log.Show(PREFIX, $"Erro ao finalizar a conexão com a impressora. Código de erro: {closeResult}");
                }
                else
                {
                    Log.Show(PREFIX, "Conexão com a impressora finalizada com sucesso.");
                }

                // Reseta variáveis de estado
                _devHandle = -1; // Define como inválido após o fechamento
                BT_start = false; // Marca o estado de inicialização como falso
            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, $"Erro inesperado ao finalizar a impressora: {ex.Message}");
            }
        }

        // TEXT SETUP ON CURRENT POSITION 
        public override void SetBold(bool on)
        {
            if (on)
            {
                _bold = $"Bold={BT_Bold.Bold}"; // Define o valor como "1" para negrito ativado
            }
            else
            {
                _bold = $"Bold={BT_Bold.Unbold}"; // Define o valor como "0" para negrito desativado
            }
        }
        public override void SetUnderline(bool on)
        {

            if (on)
            {
                _underLine = $"UnderLine={BT_Underline.underline}"; // Define o valor como "1" para underline ativado
            }
            else
            {
                _underLine = $"UnderLine={BT_Underline.noUnderline}"; // Define o valor como "0" para underline desativado
            }

        }
        public override void SetReverse(bool on)
        {

            if (on)
            {
                _reverse = $"Reverse={BT_Reverse.Reverse}"; // Define o valor como "1" para reverse ativado
            }
            else
            {
                _reverse = $"Reverse={BT_Reverse.noReverse}"; // Define o valor como "0" para reverse desativado
            }
        }

        public override void SetCharacterWidth(CharacterWidth width)
        {
            switch (width)
            {
                case CharacterWidth.WIDTH_1:

                    Log.Show(PREFIX, "Largura do caractere definida para Normal.");
                    break;

                case CharacterWidth.WIDTH_2:

                    Log.Show(PREFIX, "Largura do caractere definida para Dupla.");
                    break;

                case CharacterWidth.WIDTH_3:

                    Log.Show(PREFIX, "Largura do caractere definida para Tripla.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(width), width, "Altura de caracter não suportada.");
            }

        }
        private bool ConfigurePrinter()
        {
            try
            {
                // Comando de configuração principal
                byte[] configCommand = { 0x1B, 0x73, 0x42, 0x45, 0x92, 0x9A, 0x03, 0x00, 0x6D };

                // Comando de configuração de encoding (exemplo para Windows-1252)
                byte[] encodingCommand = { 0x1B, 0x74, 0x10 }; // 0x10 configura Windows-1252 em algumas impressoras ESC/POS

                // Verifica se o handle da impressora é válido
                if (_devHandle <= 0)
                {
                    Log.Show(PREFIX, "Erro: A impressora não está inicializada.");
                    return false;
                }

                // Envia o comando de configuração principal
                int configResult = SendCommand(_devHandle, configCommand, configCommand.Length);
                if (configResult != 0) // Verifica se houve erro
                {
                    Log.Show(PREFIX, $"Erro ao enviar o comando de configuração principal. Código de erro: {configResult}");
                    return false;
                }
                Log.Show(PREFIX, "Comando de configuração principal enviado com sucesso.");

                // Envia o comando de configuração de encoding
                int encodingResult = SendCommand(_devHandle, encodingCommand, encodingCommand.Length);
                if (encodingResult != 0) // Verifica se houve erro
                {
                    Log.Show(PREFIX, $"Erro ao enviar o comando de configuração de encoding. Código de erro: {encodingResult}");
                    return false;
                }
                Log.Show(PREFIX, "Comando de configuração de encoding enviado com sucesso.");

                return true; // Ambos os comandos foram enviados com sucesso
            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, $"Erro ao configurar a impressora: {ex.Message}");
                return false;
            }
        }


        // TEXT SETUP ON BEGINING OF LINE
        public override void SelectCharacterHeight(CharacterHeight height)
        {

            switch (height)
            {
                case CharacterHeight.HEIGHT_1:

                    Console.WriteLine("Altura do caractere definida para Normal.");
                    break;

                case CharacterHeight.HEIGHT_2:

                    Console.WriteLine("Altura do caractere definida para Dupla.");
                    break;

                case CharacterHeight.HEIGHT_3:

                    Console.WriteLine("Altura do caractere definida para Tripla.");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(height), height, "Altura de caracter não suportada.");
            }

        }
        public override void SelectCharacterSize(CharacterWidth width, CharacterHeight height)
        {
            switch (width)
            {
                case CharacterWidth.WIDTH_1:
                    break;

                case CharacterWidth.WIDTH_2:
                    break;

                case CharacterWidth.WIDTH_3:
                    break;
            }

            switch (height)
            {
                case CharacterHeight.HEIGHT_1:
                    break;

                case CharacterHeight.HEIGHT_2:
                    break;

                case CharacterHeight.HEIGHT_3:
                    break;
            }
        }
        public override void SelectFontSmall(bool on)
        {
            try
            {
                // Escolhe a fonte com base no valor de 'on'
                BT_Font selectedFont = on ? BT_Font.font_B : BT_Font.font_A;

                // Aplica a configuração da fonte
                Log.Show(PREFIX, on
                    ? "Fonte pequena ativada (9x17)."
                    : "Fonte padrão ativada (12x24).");

                // Comando necessário para configurar a impressora, se aplicável

                string fontCommand = $"Font={selectedFont}";
                int result = PrintContent(_devHandle, "", _font);
                if (result != 0)
                {
                    Log.Show(PREFIX, $"Erro ao aplicar a fonte. Código de erro: {result}");
                }
            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, "Erro ao selecionar a fonte: " + ex.Message);
            }
        }
        // Método para enviar o comando de fonte à impressora

        public override void SelectJustification(ReceiptPrinter.Justification justification)
        {
            try
            {

                switch (justification)
                {
                    case Justification.LEFT:
                        Console.WriteLine("Texto justificado à esquerda");
                        break;
                    case Justification.CENTER:
                        Console.WriteLine("Texto justificado ao centro");
                        break;
                    case Justification.RIGHT:
                        Console.WriteLine("Texto justificado à direita");
                        break;

                }

                Log.Show(PREFIX, $"Justificação selecionada: {justification}");
            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, "Erro ao selecionar justificação: " + ex.Message);
            }
        }
        public override void SelectLineFeed(byte feed)
        {
            try
            {
                int result = PrintContent(_devHandle, "", _font + "Bold=0|HScale=1|VScale=1");
                if (result != 0)
                {
                    Log.Show(PREFIX, "Avanço de linhas com sucesso.");
                }
            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, "Erro ao executar avanço de linhas: " + ex.Message);
            }
        }
        public override void SelectCharacterSpacing(byte spacing)
        {
            /*try
            {
   
                // Envia o comando para a impressora
                int result = PrintContent(_devHandle, "", _font);

                if (result != 0)
                {
                    Console.WriteLine($"Erro ao definir espaçamento entre caracteres. Código de erro: {result}");
                   
                }
                else
                {
                    Console.WriteLine($"Espaçamento entre caracteres definido para {spacing}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao definir espaçamento entre caracteres: " + ex.Message);

            }*/
        }

        // RESET ALL TEXT SETTINGS
        public override void ResetText()
        {
            try
            {
                // Verifica se o handle da impressora é válido
                if (_devHandle < 0)
                {
                    Log.Show(PREFIX, "Erro: Impressora não inicializada.");
                    return;
                }

                Log.Show(PREFIX, "Resetando a impressora...");

                // Comando ESC/POS para reset
                byte[] resetCommand = { 0x1B, 0x40 };

                // Envia o comando para a impressora
                int result = SendCommand(_devHandle, resetCommand, resetCommand.Length);

                if (result == 0) // 0 indica sucesso
                {
                    Log.Show(PREFIX, "Impressora resetada com sucesso.");
                }
                else
                {
                    Log.Show(PREFIX, $"Erro ao enviar comando de reset. Código de erro: {result}");
                }
            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, $"Erro ao resetar a impressora: {ex.Message}");
            }
        }

        private void ResetTextSettings()
        {
            try
            {
                // Verifica se a impressora está inicializada
                if (_devHandle <= 0)
                {
                    Log.Show(PREFIX, "Erro: Impressora não inicializada.");
                    return;
                }
                // Define a largura e altura padrão dos caracteres
                SelectCharacterSize(CharacterWidth.WIDTH_1, CharacterHeight.HEIGHT_1);

                SetBold(false);
                SetUnderline(false);
                SetReverse(false);
                SelectFontSmall(false); // Supondo que `false` indica a fonte padrão maior

                Log.Show(PREFIX, "Configurações de texto redefinidas para os valores padrão.");

            }

            catch (Exception ex)
            {
                Log.Show(PREFIX, "Erro ao redefinir as configurações de texto: " + ex.Message);

            }
        }

        // PRINT
        private int CommandPrinter(int devHandle, byte[] command, int length)
        {
            try
            {
                // Validação se o comando é válido
                if (command == null || command.Length == 0)
                {
                    Log.Show(PREFIX, "Comando inválido. O comando não pode ser nulo ou vazio.");
                    return -1; // Retorna -1 para indicar erro de comando inválido
                }

                // Envia o comando para a impressora
                int result = SendCommand(devHandle, command, length);

                // Verifica o resultado da operação
                if (result == 0) // 0 indica sucesso
                {
                    Log.Show(PREFIX, $"Comando enviado com sucesso: {BitConverter.ToString(command)}");
                }
                else
                {
                    Log.Show(PREFIX, $"Erro ao enviar comando. Código de erro: {result}");
                }

                return result; // Retorna o código de sucesso ou erro
            }
            catch (Exception ex)
            {
                // Caso ocorra alguma exceção, loga a mensagem de erro
                Log.Show(PREFIX, $"Erro ao enviar comando: {ex.Message}");
                return -1; // Retorna -1 em caso de erro
            }
        }

        private int EncodingCommand(int devHandle)
        {


            byte[] encodingCommand = { 0x1B, 0x74, 0x10 };
            int result = SendCommand(_devHandle, encodingCommand, encodingCommand.Length);
            if (result == 0)
            {
                Log.Show(PREFIX, "Comando de encoding aplicado com sucesso.");
            }
            else
            {
                Log.Show(PREFIX, $"Erro ao aplicar o comando de encoding. Código de erro: {result}");
            }
            return result;

        }



        public override void PrintText(string text)
        {
            try
            {
                // Verifica se o texto é válido antes de continuar
                if (string.IsNullOrWhiteSpace(text))
                {
                    Log.Show(PREFIX, "Texto vazio ou nulo não pode ser impresso.");
                    return; // Se o texto for vazio ou nulo, retorna sem tentar imprimir
                }

                // Comando de configuração principal
                byte[] configCommand = { 0x1B, 0x74, 0x42, 0x45, 0x92, 0x9A, 0x03, 0x10, 0x6D };

                // Comando de configuração de encoding (exemplo para Windows-1252)
                byte[] encodingCommand = { 0x1B, 0x74, 0x10, 0x6D }; // 0x10 configura Windows-1252 em algumas impressoras ESC/POS

                // Verifica se o handle da impressora é válido
                if (_devHandle <= 0)
                {
                    Log.Show(PREFIX, "Erro: A impressora não está inicializada.");

                }

                // Envia o comando de encoding se a inicialização foi bem-sucedida
                int configResult = SendCommand(_devHandle, configCommand, configCommand.Length);
                if (configResult != 0)
                {
                    Log.Show(PREFIX, $"Erro ao aplicar comando de encoding. Código de erro: {configResult}");
                    return;
                }
                int encodingResult = SendCommand(_devHandle, encodingCommand, encodingCommand.Length);
                if (encodingResult != 0)
                {
                    Log.Show(PREFIX, $"Erro ao aplicar comando de encoding. Código de erro: {encodingResult}");
                    return;
                }

                // Envia o texto para impressão
                int printResult = PrintContent(_devHandle, text, _font);
                if (printResult != 0)
                {
                    Log.Show(PREFIX, "Erro ao imprimir o texto. Código de erro: " + printResult);
                    return; // Se houver erro na impressão, retorna sem tentar cortar o papel
                }

                Log.Show(PREFIX, "Texto impresso com sucesso.");

                // Realiza o corte do papel após imprimir
                int cutResult = PaperCut(_devHandle, 0, 0);
                if (cutResult != 0)
                {
                    Log.Show(PREFIX, $"Erro ao realizar o corte do papel. Código de erro: {cutResult}");
                }
                else
                {
                    Log.Show(PREFIX, "Corte de papel realizado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, $"Erro ao imprimir o texto: {ex.Message}");
            }
        }
        public override void PrintTextLine(string text)
        {
            try
            {
                // Verifica se o texto é válido antes de realizar qualquer operação
                if (string.IsNullOrWhiteSpace(text))
                {
                    Log.Show(PREFIX, "Texto vazio ou nulo não pode ser impresso.");
                    return;
                }

                int printResult = PrintContent(_devHandle, text, _font);
                if (printResult != 0)  // 0 indica sucesso
                {
                    Log.Show(PREFIX, $"Erro ao imprimir o texto. Código de erro: {printResult}");
                    return;  // Retorna caso a impressão falhe
                }


                // Alimenta algumas linhas após a impressão (por exemplo, 1 linhas)
                int feedResult = FeedLines(_devHandle, 1);
                if (feedResult != 0)
                {
                    Log.Show(PREFIX, $"Erro ao alimentar linhas. Código de erro: {feedResult}");
                }


            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, $"Erro ao imprimir linha de texto: {ex.Message}");
            }
        }
        public override void PrintLine()
        {
            try
            {

                string line = new string('-', 47); // Ajuste o número de caracteres conforme a largura da impressora
                int result = PrintContent(_devHandle, line + "\n", ""); // Adiciona uma quebra de linha

                if (result != 0) // 0 indica sucesso
                {
                    Log.Show(PREFIX, "Linha impressa com sucesso.");
                }

            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, $"Erro ao imprimir linha: {ex.Message}");
            }
        }
        public override void PrintLine(byte lines)
        {
            try
            {
                string emptyLines = new string('\n', lines);  // Gera as linhas a serem impressas (linhas vazias)
                int result = PrintContent(_devHandle, emptyLines, ""); // Envia as linhas para a impressora

                if (result != 0) // 0 indica sucesso
                {
                    Log.Show(PREFIX, $"{lines} linhas impressas com sucesso.");
                }

            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, $"Erro ao imprimir as linhas: {ex.Message}");
            }
        }
        public override void PrintFeed(byte height)
        {
            try
            {
                string Feed = new string('\n', height); //  avanço
                int result = PrintContent(_devHandle, Feed, "");

                if (result != 0) //  0 indica sucesso
                {
                    Log.Show(PREFIX, $"Avanço de {height} unidades realizado com sucesso.");
                }

            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, $"Erro ao realizar o avanço do papel: {ex.Message}");
            }
        }
        public override void PrintRuler(byte height, bool dash)
        {
            try
            {
                int result = PrintContent(_devHandle, "_", _font + "Bold=0|HScale=1|VScale=1");

                if (result != 0)
                {
                    Log.Show(PREFIX, "Régua encontrada.");
                }
            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, "Erro ao imprimir a régua: " + ex.Message);
            }
        }
        public override bool PrintBmp(string file)
        {
            try
            {
                int result = PrintImageFile(_devHandle, images_path + image_3, "x=-3|Scale=100");

                if (result != 0)
                {
                    return true; // Se a imagem foi enviada corretamente para a impressora, retornar true
                }
                else
                {
                    Log.Show(PREFIX, "Arquivo não encontrado.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Tratar possíveis erros
                Log.Show(PREFIX, "Erro ao imprimir o arquivo BMP: " + ex.Message);
                return false;
            }
        }

        //SETTINGS
        public override int GetLineCharacters()
        {
            return GetLineCharacters(CharacterWidth.WIDTH_2);
        }
        public override int GetLineCharacters(CharacterWidth width)
        {
            try
            {
                int maxLineWidth = 80;  // Tamanho máximo da linha (exemplo, 80 caracteres)
                int characterWidth = 0; // Determina a largura do caractere com base no tipo de CharacterWidth

                switch (width)
                {
                    case CharacterWidth.WIDTH_1:
                        characterWidth = 1; // Caracter estreito ocupa 1 unidade de largura
                        break;
                    case CharacterWidth.WIDTH_2:
                        characterWidth = 2; // Caracter normal ocupa 2 unidades de largura
                        break;
                    case CharacterWidth.WIDTH_3:
                        characterWidth = 3; // Caracter largo ocupa 3 unidades de largura
                        break;

                }

                // Calcula o número de caracteres que cabem na linha, dado o tipo de caractere
                return maxLineWidth / characterWidth;
            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, "Erro ao calcular caracteres por linha: " + ex.Message);
                return 0;  // Retorna 0 em caso de erro
            }
        }

        //PAGE
        public override bool Page()
        {
            try
            {
                Log.Show(PREFIX, "Nova página iniciada com sucesso.");
                return true;
            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, "Erro ao iniciar nova página: " + ex.Message);
                return false;
            }
        }


        // STATUS
        public override void New(UnitConfig config)
        {
            base.New(config);
            ReceiptPrinterStatus = ReceiptPrinterStatus.OK_INIT;
        }
        private int QueryStatus()
        {
            Log.Show(PREFIX, "Status da consulta...");
            StringBuilder status = new StringBuilder();
            int nRet = QueryStatus(0, status);

            if (nRet == 1)
            {
                Log.Show(PREFIX, "1 - Estado Normal. ");
            }

            if (nRet == 2)
            {
                Log.Show(PREFIX, "2 - Sem papel. ");
            }

            if (nRet == 4)
            {
                Log.Show(PREFIX, "4 - Papel perto do fim. ");
            }

            if (nRet == 8)
            {
                Log.Show(PREFIX, "8 - Tampa aberta. ");
            }

            if (nRet == 16)
            {
                Log.Show(PREFIX, "16 - Cabeça aquecida. ");
            }

            if (nRet == 32)
            {
                Log.Show(PREFIX, "32 - Erro no corte de papel. ");
            }
            else
            {
                Log.Show(PREFIX, $"Código de erro: {nRet}");
            }
            return nRet;
        }
        private void CheckStatus_Thread()
        {

            // Start sleep
            Utils.Sleep(STATUS_START_DELAY);

            // Loop
            while (!App.IsExit && BT_status)
            {
                // Reset or !Ready
                if (_reset || !_ready)
                {
                    Free(true);
                    Utils.Wait(() => !App.IsExit, STATUS_RESET_WAIT);
                    if (App.IsExit) break;

                    Init(false, true);
                    if (!_ready)
                    {
                        Log.File(PREFIX, "Reset unit retry");
                        Utils.Wait(() => !App.IsExit, STATUS_REOPEN_WAIT);
                    }
                    else
                    {
                        Utils.Sleep(500);
                    }
                    if (App.IsExit) break;
                }

                // Ready
                if (_ready)
                {
                    bool reset = _reset;

                    // Check status -> False: Reset
                    if (!CheckStatus(false))
                    {
                        Utils.Wait(() => !App.IsExit, STATUS_REOPEN_WAIT);
                        if (!_reset)
                        {
                            Log.Debug(PREFIX, "Reset unit on status read error");
                            _reset = true;
                        }
                        else
                        {
                            Log.File(PREFIX, "Reset unit retry on status read error");
                        }
                    }
                    else if (_reset && reset)
                    {
                        Log.Debug(PREFIX, "Reset ok");
                        _reset = false;
                    }
                }

                // Loop sleep
                Utils.Wait(() => !App.IsExit, STATUS_CHECK_WAIT);
            }
        }
        private bool CheckStatus(bool init)
        {
            ReceiptPrinterStatus status = ReceiptPrinterStatus.OK;
            StringBuilder statusBuffer = new StringBuilder(256);
            ReceiptPrinterStatus resultStatus;

            try
            {
                int result = QueryStatus(_devHandle, statusBuffer);  // Consulta o status usando a função QueryStatus da DLL POSSDK_Pro
                if (result == 0) // // Verifica se a chamada foi bem-sucedida e avalia o status da impressora
                {
                    string statusMessage = statusBuffer.ToString();

                    // Mapeia o status de retorno para códigos internos de status
                    if (statusMessage.Contains("PAPER_OK"))
                        resultStatus = new ReceiptPrinterStatus(ReceiptPrinterCode.OK, "Papel OK");
                    else if (statusMessage.Contains("WARN_PAPER"))
                        resultStatus = new ReceiptPrinterStatus(ReceiptPrinterCode.WARN_PAPER, "Papel baixo");
                    else if (statusMessage.Contains("ERROR_PAPER"))
                        resultStatus = new ReceiptPrinterStatus(ReceiptPrinterCode.ERROR_PAPER, "Erro de papel");
                    else
                        resultStatus = new ReceiptPrinterStatus(ReceiptPrinterCode.ERROR_CALL, "Status desconhecido");
                }
                else
                {
                    // Define o status como erro se a chamada QueryStatus falhar
                    resultStatus = new ReceiptPrinterStatus(ReceiptPrinterCode.ERROR_CALL, "Falha ao consultar status");
                }
            }
            catch (Exception ex)
            {
                // Captura exceções e define o status como erro de chamada
                resultStatus = new ReceiptPrinterStatus(ReceiptPrinterCode.ERROR_CALL, $"Erro na consulta: {ex.Message}");
            }

            // Check if paper low switches to paper error
            if ((status.Code == ReceiptPrinterCode.WARN_PAPER) && FEED_LOW_ERROR)
            {
                status = new ReceiptPrinterStatus(ReceiptPrinterCode.ERROR_PAPER);
            }

            // Set status
            StatusStatus = status;

            // Call error -> Reset
            return status.Code != ReceiptPrinterCode.ERROR_CALL;
        }

        private ReceiptPrinterStatus _StatusStatus = ReceiptPrinterStatus.OK;
        public ReceiptPrinterStatus StatusStatus
        {
            get { return _StatusStatus; }
            protected set
            {
                if (StatusStatus.IsChange(value))
                {
                    _StatusStatus = value;
                    SetupStatus(value);
                }
            }
        }
        protected void SetupStatus(ReceiptPrinterStatus subStatus)
        {
            ReceiptPrinterStatus status = ReceiptPrinterStatus.OK;

            // Merge status
            status = status.Merge(StatusStatus);

            // Merge init status (+ printer name)
            status = status.Merge(_ready ? new ReceiptPrinterStatus(ReceiptPrinterCode.OK) : ReceiptPrinterStatus.OK_INIT);

            // Log substatus if different of status
            if (status.IsChange(subStatus))
            {
                subStatus.Log(PREFIX, true);
            }

            // Set status
            ReceiptPrinterStatus = status;
        }

        private bool OpenDeviceDoor()
        {
            try
            {
                // Comando para abrir a porta
                byte[] openDoorCommand = { 0x1B, 0x63, 0x46, 0x00 };

                // Envia o comando
                int result = SendCommand(_devHandle, openDoorCommand, openDoorCommand.Length);

                // Verifica o resultado do comando
                if (result < 0) // 0 indica sucesso
                {
                    Log.Show(PREFIX, "Comando para abrir a porta enviado com sucesso.");
                    return true;
                }
                else
                {
                    Log.Show(PREFIX, $"Erro ao enviar o comando para abrir a porta. Código de erro: {result}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Show(PREFIX, $"Erro ao tentar abrir a porta: {ex.Message}");
                return false;
            }
        }



    }
}





