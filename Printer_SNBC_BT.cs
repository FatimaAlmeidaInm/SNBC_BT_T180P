using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using static Printer_SNBC_BT.SNBC_BT;
using System;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using static System.Net.WebRequestMethods;
using static System.Net.Mime.MediaTypeNames;


namespace Printer_SNBC_BT   // SNBC BT-T180PRO
{
    internal class Printer_SNBC_BT
    {
        #region Especificações 

        private const string _portType = "USB";
        private const string _serialPort = "COM1";
        private const string _portInfo = "USBAPI|936";
        private const string _testDevName = "USBAPI|936";
        private const string _modelName = "BTP-U80 PLUS";
        private const string _deviceInfo = "192@USBCLASS|BTP";
        
        

        static int devHandle = 0;
        const string _dataLen = null;
        private const int PORT_USB = 1;  // PortType USB
        private const int PORT_NET = 2;  // PortType NET
        private const int _usblen = 3;
        private const int _portNumber = 19200;
        private const int _deviceInfoLen = 912;
        private const int INI_MAX_SIZE = 1024;

        private const string image_1 = "bfa_receipt.bmp";
        private const string content = "text|image|font";
        private const string images_path = "C:\\Dev\\assets\\";

        private const string font = "fontA,fontB,fontC";
        private const string _fontA = "Bold=0|HScale=1|VScale=1";
        private const string _fontB = "Bold=1|HScale=1|VScale=1";
        private const string _fontC = "Bold=0|HScale=2|VScale=2";

        private const string _date1 = "dd/MM";
        private const string _time1 = "hh:mm";
        private const string _timeNow = "hh:mm:ss";
        private const string _dateNow = "dd/MM/yyyy";

        private const string _emptyLine = "\n";
        private const string _underline = "________________________________________________\n";

        private const int cutMode = 0;
        private const int distance = 1;

        
        #endregion

        static void Help()
        {
            Console.WriteLine("Ajuda:");
            Console.WriteLine("F1 - EnumDevice");    // Enumerar Dispositivo 
            Console.WriteLine("F2 - OpenPrinter");   // Conectar/Abrir Impressora, 
            Console.WriteLine("F3 - ClosePrinter");  // Desconectar/Fechar impressora, 
            Console.WriteLine("F4 - PrintText");     // Imprimir Texto,
            Console.WriteLine("F5 - PaperCut");      // Cortar Papel, 
            Console.WriteLine("F6 - Feed Lines");    // Alimentar Papel
            Console.WriteLine("F7 - QueryStatus");   // Consultar Status
            Console.WriteLine("F8 - BarCodePrint");  // Imprimir Código de Barras
            Console.WriteLine("F9 - PrintDownloadedImage");    // Imprimir Imagem Baixada
            Console.WriteLine("F10 - DownloadImage");          // Baixar Imagem
            Console.WriteLine("F11 - PrintContent");           // imprimir Conteúdo
            Console.WriteLine("R - PrintReset");               // Reiniciar impressora
            Console.WriteLine("D - BankDeposit");              // Imprimir Depósito Bancário
            Console.WriteLine("V - TotalVault");               // Imprimir total de Cofre
            Console.WriteLine("S - PeriodSummary");            // Imprimir Resumo do Periodo
            Console.WriteLine("T - PeriodTransactions");       // Imprimir Transações do Periodo
            Console.WriteLine("O - PeriodOpening");            // Imprimir Abertura do Periodo
            Console.WriteLine("C - PeriodClosing");            // Imprimir Fecho do Periodo
            Console.WriteLine("F12 - Help");                   // Mostrar Ajuda
            Console.WriteLine("Esc - exit the program");       // Sair do Programa

        }
        public static void Main()
        {
            Console.WriteLine("SNBC BT-T180PRO");
            Help();
            bool loop = true;
            while (loop)
            {
                Console.Write("\n> ");
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();
                switch (key.Key)
                {
                    case ConsoleKey.F1: BT_EnumDevice(); break;

                    case ConsoleKey.F2: BT_OpenPrinter(); break;

                    case ConsoleKey.F3: BT_ClosePrinter(); break;

                    case ConsoleKey.F4: BT_PrintText(devHandle, content, font); break;

                    case ConsoleKey.F5: BT_PaperCut(devHandle, cutMode, distance); break;

                    case ConsoleKey.F6: BT_FeedLines(20); break;

                    case ConsoleKey.F7: BT_QueryStatus(); break;

                    //case ConsoleKey.F8: BT_BarCodePrint(); break;

                    //case ConsoleKey.F9: BT_PrintDownloadedImage(); break;

                    //case ConsoleKey.F10: BT_DownloadImage(); break;

                    //case ConsoleKey.F11: BT_PrintContent(); break;

                    case ConsoleKey.F12: Help(); break;

                    case ConsoleKey.R: BT_PrintReset(); break;

                    case ConsoleKey.D: BT_BankDeposit(devHandle); break;

                    case ConsoleKey.V: BT_TotalVault(devHandle); break;

                    case ConsoleKey.S: BT_PeriodSummary(devHandle); break;

                    case ConsoleKey.T: BT_PeriodTransactions(devHandle); break;

                    case ConsoleKey.O: BT_PeriodOpening(devHandle); break;

                    case ConsoleKey.C: BT_PeriodClosing(devHandle); break;

                    case ConsoleKey.Escape: loop = false; break;

                    default:
                        Console.WriteLine("Comando não reconhecido.");
                        break;

                }


            }


        }
        public static void BT_EnumDevice()
        {
            Console.WriteLine("Enumerando dispositivos de impressora...");

            try
            {
                // Define the buffer for enumerated device info
                const int DEVICE_INFO_LEN = 1024; // Example size, adjust as needed
                StringBuilder enumList = new StringBuilder(DEVICE_INFO_LEN);

                // Call EnumDevice to enumerate devices
                int nRet = EnumDevice(PORT_USB, enumList, INI_MAX_SIZE);

                if (nRet >= 0) // Assuming non-negative values indicate success
                {
                    string deviceInfo = enumList.ToString();
                    Console.WriteLine("Dispositivo(s) enumerado(s) com sucesso:");
                    Console.WriteLine(deviceInfo);
                }
                else
                {
                    Console.WriteLine($"Erro ao enumerar dispositivos. Código de erro: {nRet}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enumerar dispositivos: {ex.Message}");
            }
        }
        public static void BT_OpenPrinter()
        {
            Console.WriteLine("Abrindo a porta da impressora...");

            int nRet = OpenPrinter(_modelName, _portInfo);
            if (nRet >= 0)
            {
                devHandle = nRet;
                Console.WriteLine("Impressora ligada e conectada com sucesso. Handle: " + devHandle);
            }
            else
            {
                Console.WriteLine("Erro ao enumerar dispositivos: Código de erro " + nRet);
            }

        }
        public static void BT_ClosePrinter()
        {
            Console.WriteLine("A fechar porta da impressora...");

            int nRet = ClosePrinter(devHandle);
            if (nRet >= 0)
            {
                Console.WriteLine("Porta fechada");
            }
            else
            {
                Console.WriteLine("Erro ao enumerar dispositivos: Código de erro " + nRet);
            }

        }
        public static void BT_PrintText(int devHandle, string content, string font)
        {
            Console.WriteLine("Iniciando a impressão de texto...");

            try
            {
                if (devHandle <= 0)
                {
                    Console.WriteLine("Erro: O handle da impressora é inválido ou a impressora não está conectada.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(content))
                {
                    Console.WriteLine("Erro: O conteúdo do texto a ser impresso está vazio.");
                    return;
                }

                // PrintText is assumed to send content to the printer
                int nRet = PrintText(devHandle, content, font);

                if (nRet == 0) // Assuming 0 indicates success
                {
                    Console.WriteLine("Texto enviado para a impressora com sucesso.");
                }
                else
                {
                    Console.WriteLine($"Erro ao imprimir texto. Código de erro: {nRet}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao tentar imprimir texto: {ex.Message}");
            }
        }
        public static void BT_PaperCut(int devHandle, int cutMode, int option)
        {
            Console.WriteLine("A Cortar papel...");

            try
            {
                // Passing cutMode directly for flexibility; assuming it corresponds to `mode`
                int nRet = PaperCut(devHandle, cutMode, option); // Modify parameters as needed
                if (nRet >= 0)
                {
                    Console.WriteLine("Corte realizado com sucesso.");
                }
                else
                {
                    Console.WriteLine("Erro ao realizar o corte. Código de erro: " + nRet);
                }
            }
          
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu uma exceção ao tentar cortar o papel: " + ex.Message);
            }
        }
        public static void BT_FeedLines(int number)
        {
            int nRet = FeedLines(0, 4);
            if (nRet >= 0)
            {
                Console.WriteLine("Talão vazio ");
            }
        }
        public static void BT_QueryStatus()
        {
            const int MAXSTATUSLEN = 128;
            StringBuilder strBuildStatus = new StringBuilder(MAXSTATUSLEN);

            // Check if the device handle is valid
            if (devHandle <= 0)
            {
                Console.WriteLine("Erro: O handle da impressora é inválido.");
                return;
            }

            try
            {
                // Query the status of the printer
                int nStatus = QueryStatus(devHandle, strBuildStatus);

                if (nStatus == 0) // Assuming 0 indicates success
                {
                    string statusMessage = strBuildStatus.ToString();
                    Console.WriteLine("Status da impressora obtido com sucesso: " + statusMessage);

                    // Analyze the status codes
                    int statusCode = int.Parse(statusMessage); // Assuming the status is returned as a numeric string

                    if ((statusCode & 1) != 0)
                    {
                        Console.WriteLine("1 - Estado Normal.");
                    }
                    if ((statusCode & 2) != 0)
                    {
                        Console.WriteLine("2 - Sem papel.");
                    }
                    if ((statusCode & 4) != 0)
                    {
                        Console.WriteLine("4 - Papel perto do fim.");
                    }
                    if ((statusCode & 8) != 0)
                    {
                        Console.WriteLine("8 - Tampa aberta.");
                    }
                    if ((statusCode & 16) != 0)
                    {
                        Console.WriteLine("16 - Cabeça aquecida.");
                    }
                    if ((statusCode & 32) != 0)
                    {
                        Console.WriteLine("32 - Erro no cortador.");
                    }
                }
                else
                {
                    Console.WriteLine($"Erro ao consultar o status da impressora. Código de erro: {nStatus}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao consultar o status da impressora: {ex.Message}");
            }
        }


        //public static void BT_PrintDownloadedImage()

        //public static void BT_DownloadImage()

        //public static void BT_PrintImageFile()

        //public static void BT_PrintContent()

        //public static void BT_Help()

        public static void BT_PrintReset()
        {
            Console.WriteLine("Reset...");
            try
            {
                int nRet = PrintReset(); // Attempt to reset the printer
                if (nRet >= 0)
                {
                    Console.WriteLine("Impressora reiniciada com sucesso.");
                }
                else
                {
                    Console.WriteLine("Erro ao reiniciar a impressora. Código de erro: " + nRet);
                }
            }
           
            catch (EntryPointNotFoundException)
            {
                Console.WriteLine("Erro: A função PrintReset não foi encontrada no POSSDK_Pro.dll.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu uma exceção ao tentar reiniciar a impressora: " + ex.Message);
            }

        }
        private static int PrintReset()
        {
            throw new NotImplementedException();
        }
        public static void BT_BankDeposit(int devHandle)
        {
            string TimeNow = DateTime.Now.ToString(_timeNow);
            string DateNow = DateTime.Now.ToString(_dateNow);

            _ = SNBC_BT.PrintImageFile(devHandle, images_path + image_1, "x=-3|Scale=100");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");

            _ = SNBC_BT.PrintText(devHandle, "   Deposito Bancario  \n", _fontC);
            _ = SNBC_BT.PrintText(devHandle, "\x1B\x40", "");     // Envia comando ESC @ para resetar configurações

            _ = SNBC_BT.PrintText(devHandle, _underline, "");
            _ = SNBC_BT.PrintText(devHandle, "                             " + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                                        " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "Kiosk                                           \n", "");
            _ = SNBC_BT.PrintText(devHandle, "N.º do periodo:                                 \n", "");
            _ = SNBC_BT.PrintText(devHandle, "N.º Operacao: 12164962                          \n", "");
            _ = SNBC_BT.PrintText(devHandle, "N.º Transacao: 212164962                        \n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");
            _ = SNBC_BT.PrintText(devHandle, "\x1D\x42\x01", ""); // Ativa o modo invertido
            _ = SNBC_BT.PrintText(devHandle, " Deposito                                       \n", "");
            _ = SNBC_BT.PrintText(devHandle, "\x1D\x42\x00", ""); // Desativa o modo invertido
            _ = SNBC_BT.PrintText(devHandle, "\x1B\x40", "");     // Envia comando ESC @ para resetar configurações

            _ = SNBC_BT.PrintText(devHandle, "Titular: Joao Pedro Martins                     \n", "");
            _ = SNBC_BT.PrintText(devHandle, "IBAN: 2245 8625 6632 1422 22222                 \n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");
            _ = SNBC_BT.PrintText(devHandle, "Montante: 6.000,00 kz                          \n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");
            _ = SNBC_BT.PrintText(devHandle, _underline, "");
            _ = SNBC_BT.PrintText(devHandle, "        A sua seguranca passa pelo BFA          \n", "");
            _ = SNBC_BT.PrintText(devHandle, "          Nao partilhe os seus dados.           \n", "");
            _ = SNBC_BT.PrintText(devHandle, "     Linha de atendimento BFA: 923 120 120      \n", "");
            _ = SNBC_BT.PrintText(devHandle, "                  www.bfa.ao                    \n", "");

            _ = SNBC_BT.PaperCut(devHandle, 0, 0);


        }
        public static void BT_TotalVault(int devHandle)
        {
            string TimeNow = DateTime.Now.ToString(_timeNow);
            string DateNow = DateTime.Now.ToString(_dateNow);

            _ = SNBC_BT.PrintImageFile(devHandle, images_path + image_1, "x=-3|Scale=100");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");
            _ = SNBC_BT.PrintText(devHandle, "   Total do Cofre  \n", _fontC);
            _ = SNBC_BT.PrintText(devHandle, "\x1B\x40", "");     // Envia comando ESC @ para resetar configurações
            _ = SNBC_BT.PrintText(devHandle, _underline, "");

            _ = SNBC_BT.PrintText(devHandle, "                             " + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                                        " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "Kiosk                                           \n", "");
            _ = SNBC_BT.PrintText(devHandle, "(Local)                                         \n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");
            _ = SNBC_BT.PrintText(devHandle, "Periodo: 6                                      \n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");
            _ = SNBC_BT.PrintText(devHandle, "\x1D\x42\x01", ""); // Ativa o modo invertido
            _ = SNBC_BT.PrintText(devHandle, " Notas no Reciclador                            \n", "");
            _ = SNBC_BT.PrintText(devHandle, "\x1D\x42\x00", ""); // Desativa o modo invertido
            _ = SNBC_BT.PrintText(devHandle, "\x1B\x40", "");     // Envia comando ESC @ para resetar configurações
            _ = SNBC_BT.PrintText(devHandle, "200kz x-2..............................-400,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "500kz x-2............................-1.000,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "1.000kz x-2..........................-2.000,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "................................................\n", "");
            _ = SNBC_BT.PrintText(devHandle, "Total = -6...........................-3.400,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");

            _ = SNBC_BT.PrintText(devHandle, "\x1D\x42\x01", ""); // Ativa o modo invertido
            _ = SNBC_BT.PrintText(devHandle, " Notas no Cofre                                 \n", "");
            _ = SNBC_BT.PrintText(devHandle, "\x1D\x42\x00", ""); // Desativa o modo invertido
            _ = SNBC_BT.PrintText(devHandle, "\x1B\x40", "");     // Envia comando ESC @ para resetar configurações
            _ = SNBC_BT.PrintText(devHandle, "200kz x9..............................1.800,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "500kz x2..............................1.000,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "1.000kz x15..........................15.000,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "2.000kz x7...........................14.000,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "................................................\n", "");
            _ = SNBC_BT.PrintText(devHandle, "Total = 33...........................31.800,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");
            _ = SNBC_BT.PrintText(devHandle, _underline, "");
            _ = SNBC_BT.PrintText(devHandle, "Total Numerario                     28.400,00 kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "        A sua seguranca passa pelo BFA          \n", "");
            _ = SNBC_BT.PrintText(devHandle, "          Nao partilhe os seus dados.           \n", "");
            _ = SNBC_BT.PrintText(devHandle, "     Linha de atendimento BFA: 923 120 120      \n", "");
            _ = SNBC_BT.PrintText(devHandle, "                  www.bfa.ao                    \n", "");

            _ = SNBC_BT.PaperCut(devHandle, 0, 0);


        }
        public static void BT_PeriodSummary(int devHandle)
        {
            string TimeNow = DateTime.Now.ToString(_timeNow);
            string DateNow = DateTime.Now.ToString(_dateNow);

            _ = SNBC_BT.PrintImageFile(devHandle, images_path + image_1, "x=-3|Scale=100");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");
            _ = SNBC_BT.PrintText(devHandle, "   Resumo do Periodo  \n", _fontC);
            _ = SNBC_BT.PrintText(devHandle, "\x1B\x40", "");     // Envia comando ESC @ para resetar configurações
            _ = SNBC_BT.PrintText(devHandle, _underline, "");

            _ = SNBC_BT.PrintText(devHandle, "                             " + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                                        " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "Kiosk                                           \n", "");
            _ = SNBC_BT.PrintText(devHandle, "(Local)                                         \n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");

            _ = SNBC_BT.PrintText(devHandle, "Periodo: 6                                      \n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");

            _ = SNBC_BT.PrintText(devHandle, "Pag. Numerario                      26.400,00 kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "Notas de credito                     2.000,00 kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, _underline, "");

            _ = SNBC_BT.PrintText(devHandle, "Total Numerario                     28.400,00 kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");

            _ = SNBC_BT.PrintText(devHandle, "Pag. Numerario                                  \n", "");
            _ = SNBC_BT.PrintText(devHandle, _underline, "");

            _ = SNBC_BT.PrintText(devHandle, "Deposito Bancario                   26.400,00 kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");

            _ = SNBC_BT.PrintText(devHandle, "Bloqueio de pagamentos                          \n", "");
            _ = SNBC_BT.PrintText(devHandle, _underline, "");
            _ = SNBC_BT.PrintText(devHandle, "" + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "           " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                   :                            \n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "           " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                   :                            \n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "           " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                   :                            \n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "           " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                   :                            \n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "           " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                   :                            \n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "           " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                   :                            \n", "");
            _ = SNBC_BT.PrintText(devHandle, _underline, "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");

            _ = SNBC_BT.PrintText(devHandle, "Acoes de pagamentos                             \n", "");
            _ = SNBC_BT.PrintText(devHandle, _underline, "");

            _ = SNBC_BT.PrintText(devHandle, "" + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "           " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                                                \n", "");
            _ = SNBC_BT.PrintText(devHandle, "RESET: Status: Ok                               \n", "");
            _ = SNBC_BT.PrintText(devHandle, "        A sua seguranca passa pelo BFA          \n", "");
            _ = SNBC_BT.PrintText(devHandle, "          Nao partilhe os seus dados.           \n", "");
            _ = SNBC_BT.PrintText(devHandle, "     Linha de atendimento BFA: 923 120 120      \n", "");
            _ = SNBC_BT.PrintText(devHandle, "                  www.bfa.ao                    \n", "");
            _ = SNBC_BT.PrintText(devHandle, _underline, "");

            _ = SNBC_BT.PaperCut(devHandle, 0, 0);
        }
        public static void BT_PeriodTransactions(int devHandle)
        {
            string DateNow = DateTime.Now.ToString(_dateNow);
            string TimeNow = DateTime.Now.ToString(_timeNow);
            string Date1 = DateTime.Now.ToString(_date1);
            string Time1 = DateTime.Now.ToString(_time1);

            _ = SNBC_BT.PrintImageFile(devHandle, images_path + image_1, "x=-3|Scale=100");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");

            _ = SNBC_BT.PrintText(devHandle, " Transacoes de periodo \n", _fontC);
            _ = SNBC_BT.PrintText(devHandle, "\x1B\x40", "");     // Envia comando ESC @ para resetar configurações
            _ = SNBC_BT.PrintText(devHandle, _underline, "");

            _ = SNBC_BT.PrintText(devHandle, "                             " + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                                        " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "Kiosk                                           \n", "");
            _ = SNBC_BT.PrintText(devHandle, "(Local)                                         \n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");

            _ = SNBC_BT.PrintText(devHandle, "Periodo: 6                                      \n", _fontA);
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");


            _ = SNBC_BT.PrintText(devHandle, "\x1D\x42\x01", ""); // Ativa o modo invertido
            _ = SNBC_BT.PrintText(devHandle, " Pag. Numerario                                \n", _fontA);
            _ = SNBC_BT.PrintText(devHandle, "\x1D\x42\x00", ""); // Desativa o modo invertido
            _ = SNBC_BT.PrintText(devHandle, "\x1B\x40", "");     // Envia comando ESC @ para resetar configurações

            _ = SNBC_BT.PrintText(devHandle, "data  hora  ref.  operacao          valor      \n", "");
            _ = SNBC_BT.PrintText(devHandle, "...............................................\n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + Date1, "");
            _ = SNBC_BT.PrintText(devHandle, "      " + Time1, "");
            _ = SNBC_BT.PrintText(devHandle, "            AO*66 Deposito Bancario 5.000,00 kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + Date1, "");
            _ = SNBC_BT.PrintText(devHandle, "      " + Time1, "");
            _ = SNBC_BT.PrintText(devHandle, "            AO*55 Deposito Bancario 5.000,00 kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + Date1, "");
            _ = SNBC_BT.PrintText(devHandle, "      " + Time1, "");
            _ = SNBC_BT.PrintText(devHandle, "            AO*53 Deposito Bancario 5.000,00 kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + Date1, "");
            _ = SNBC_BT.PrintText(devHandle, "      " + Time1, "");
            _ = SNBC_BT.PrintText(devHandle, "            AO*55 Deposito Bancario 5.000,00 kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + Date1, "");
            _ = SNBC_BT.PrintText(devHandle, "      " + Time1, "");
            _ = SNBC_BT.PrintText(devHandle, "            AO*22 Deposito Bancario 5.000,00 kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + Date1, "");
            _ = SNBC_BT.PrintText(devHandle, "      " + Time1, "");
            _ = SNBC_BT.PrintText(devHandle, "            AO*66 Deposito Bancario 5.000,00 kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + Date1, "");
            _ = SNBC_BT.PrintText(devHandle, "      " + Time1, "");
            _ = SNBC_BT.PrintText(devHandle, "            AO*55 Deposito Bancario 5.000,00 kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + Date1, "");
            _ = SNBC_BT.PrintText(devHandle, "      " + Time1, "");
            _ = SNBC_BT.PrintText(devHandle, "            AO*55 Deposito Bancario 5.000,00 kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, _underline, "");
            _ = SNBC_BT.PrintText(devHandle, "Total = 8............................26.400,00kz\n", _fontA);
            _ = SNBC_BT.PrintText(devHandle, "        A sua seguranca passa pelo BFA          \n", "");
            _ = SNBC_BT.PrintText(devHandle, "          Nao partilhe os seus dados.           \n", "");
            _ = SNBC_BT.PrintText(devHandle, "     Linha de atendimento BFA: 923 120 120      \n", "");
            _ = SNBC_BT.PrintText(devHandle, "                  www.bfa.ao                    \n", "");

            _ = SNBC_BT.PaperCut(devHandle, 0, 0);
        }
        public static void BT_PeriodOpening(int devHandle)
        {
            string DateNow = DateTime.Now.ToString(_dateNow);
            string TimeNow = DateTime.Now.ToString(_timeNow);

            _ = SNBC_BT.PrintImageFile(devHandle, images_path + image_1, "x=-3|Scale=100");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");


            _ = SNBC_BT.PrintText(devHandle, " Abertura de periodo   \n", _fontC);
            _ = SNBC_BT.PrintText(devHandle, "\x1B\x40", "");     // Envia comando ESC @ para resetar configurações
            _ = SNBC_BT.PrintText(devHandle, _underline, "");

            _ = SNBC_BT.PrintText(devHandle, "                             " + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                                        " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "Kiosk                                           \n", "");
            _ = SNBC_BT.PrintText(devHandle, "(Local)                                         \n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");

            _ = SNBC_BT.PrintText(devHandle, "Periodo: 7                                      \n", _fontA);
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");


            _ = SNBC_BT.PrintText(devHandle, "\x1D\x42\x01", ""); // Ativa o modo invertido
            _ = SNBC_BT.PrintText(devHandle, "                     Vazio                      \n", _fontA);
            _ = SNBC_BT.PrintText(devHandle, "\x1D\x42\x00", ""); // Desativa o modo invertido
            _ = SNBC_BT.PrintText(devHandle, "\x1B\x40", "");     // Envia comando ESC @ para resetar configurações
            _ = SNBC_BT.PrintText(devHandle, "        A sua seguranca passa pelo BFA          \n", "");
            _ = SNBC_BT.PrintText(devHandle, "          Nao partilhe os seus dados.           \n", "");
            _ = SNBC_BT.PrintText(devHandle, "     Linha de atendimento BFA: 923 120 120      \n", "");
            _ = SNBC_BT.PrintText(devHandle, "                  www.bfa.ao                    \n", "");

            _ = SNBC_BT.PaperCut(devHandle, 0, 0);
        }
        public static void BT_PeriodClosing(int devHandle)
        {
            string DateNow = DateTime.Now.ToString(_dateNow);
            string TimeNow = DateTime.Now.ToString(_timeNow);

            _ = SNBC_BT.PrintImageFile(devHandle, images_path + image_1, "x=-3|Scale=100");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");


            _ = SNBC_BT.PrintText(devHandle, " Fecho de periodo   \n", _fontC);
            _ = SNBC_BT.PrintText(devHandle, "\x1B\x40", "");     // Envia comando ESC @ para resetar configurações
            _ = SNBC_BT.PrintText(devHandle, _underline, "");

            _ = SNBC_BT.PrintText(devHandle, "                            " + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                                       " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "Kiosk                                          \n", "");
            _ = SNBC_BT.PrintText(devHandle, "(Local)                                        \n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");

            _ = SNBC_BT.PrintText(devHandle, "Periodo: 6                                     \n", _fontA);
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");
            _ = SNBC_BT.PrintText(devHandle, "\x1B\x40", "");     // Envia comando ESC @ para resetar configurações

            _ = SNBC_BT.PrintText(devHandle, "\x1D\x42\x01", ""); // Ativa o modo invertido
            _ = SNBC_BT.PrintText(devHandle, " Notas no Reciclador                           \n", "");
            _ = SNBC_BT.PrintText(devHandle, "\x1D\x42\x00", ""); // Desativa o modo invertido
            _ = SNBC_BT.PrintText(devHandle, "\x1B\x40", "");     // Envia comando ESC @ para resetar configurações
            _ = SNBC_BT.PrintText(devHandle, "200kz x-2..............................-400,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "500kz x-2............................-1.000,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "1.000kz x-2..........................-2.000,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "................................................\n", "");
            _ = SNBC_BT.PrintText(devHandle, "Total = -6...........................-3.400,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");

            _ = SNBC_BT.PrintText(devHandle, "\x1D\x42\x01", ""); // Ativa o modo invertido
            _ = SNBC_BT.PrintText(devHandle, " Notas no Cofre                                 \n", "");
            _ = SNBC_BT.PrintText(devHandle, "\x1D\x42\x00", ""); // Desativa o modo invertido
            _ = SNBC_BT.PrintText(devHandle, "\x1B\x40", "");     // Envia comando ESC @ para resetar configurações
            _ = SNBC_BT.PrintText(devHandle, "200kz x9..............................1.800,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "500kz x2..............................1.000,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "1.000kz x15..........................15.000,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "2.000kz x7...........................14.000,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, "................................................\n", "");
            _ = SNBC_BT.PrintText(devHandle, "Total  = 33..........................31.800,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");
            _ = SNBC_BT.PrintText(devHandle, "Total Numerario......................31.800,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, _emptyLine, "");
            _ = SNBC_BT.PrintText(devHandle, "Bloqueios de pagamentos .............31.800,00kz\n", "");
            _ = SNBC_BT.PrintText(devHandle, _underline, "");

            _ = SNBC_BT.PrintText(devHandle, "" + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "           " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                      Fronted @240930144057    \n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "           " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                      ok (40s)                 \n", "");

            _ = SNBC_BT.PrintText(devHandle, "" + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "           " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                      Manutenção [(Local)]     \n", "");
            _ = SNBC_BT.PrintText(devHandle, "" + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "           " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                      ok (2s)                  \n", "");
            _ = SNBC_BT.PrintText(devHandle, _underline, "");

            _ = SNBC_BT.PrintText(devHandle, "Acoes de pagamentos                            \n", "");
            _ = SNBC_BT.PrintText(devHandle, _underline, "");
            _ = SNBC_BT.PrintText(devHandle, "" + DateNow, "");
            _ = SNBC_BT.PrintText(devHandle, "           " + TimeNow, "");
            _ = SNBC_BT.PrintText(devHandle, "                                                \n", "");
            _ = SNBC_BT.PrintText(devHandle, "RESET: Status: Ok                               \n", "");
            _ = SNBC_BT.PrintText(devHandle, _underline, "");
            _ = SNBC_BT.PrintText(devHandle, "        A sua seguranca passa pelo BFA          \n", "");
            _ = SNBC_BT.PrintText(devHandle, "          Nao partilhe os seus dados.           \n", "");
            _ = SNBC_BT.PrintText(devHandle, "     Linha de atendimento BFA: 923 120 120      \n", "");

            _ = SNBC_BT.PaperCut(devHandle, 0, 0);
        }

    }

}