
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Runtime.InteropServices;
using System.Text;


namespace KioskController.Hardware.ReceiptPrinter.SNBC
{
    public partial class BT
    {

        //"C:\Users\FátimaAlmeida\source\repos\BT\lib"
        // Importando a função externa que enumera os dispositivos
        [DllImport("Printer_BT\\POSSDK_Pro.dll", CharSet = CharSet.Ansi)]
        public static extern int EnumDevice(int portType, StringBuilder deviceInfo, int deviceInfoLen);

        // Importação de StartPrinter da DLL POSSDK_Pro
        [DllImport("Printer_BT\\POSSDK_Pro.dll", CharSet = CharSet.Ansi)]
        public static extern int StartPrinter(string modelName, string portInfo); 

        // Importação de OpenPrinter da DLL POSSDK_Pro
        [DllImport("Printer_BT\\POSSDK_Pro.dll", CharSet = CharSet.Ansi)]
        public static extern int OpenPrinter(string modelName, string portInfo);

        // Importação de ClosePrinter da DLL POSSDK_Pro
        [DllImport("Printer_BT\\POSSDK_Pro.dll", CharSet = CharSet.Ansi)]
        public static extern int ClosePrinter(int devHandle);

        // Importação de PrintContent da DLL POSSDK_Pro
        [DllImport("Printer_BT\\POSSDK_Pro.dll", CharSet = CharSet.Ansi, EntryPoint = "PrintText")]
        public static extern int PrintContent(int devHandle, string content, string font);

        // Importação de PaperCut da DLL POSSDK_Pro
        [DllImport("Printer_BT\\POSSDK_Pro.dll", CharSet = CharSet.Ansi)]
        public static extern int PaperCut(int devHandle, int mode, int option);

        // Importação de FeedLines da DLL POSSDK_Pro
        [DllImport("Printer_BT\\POSSDK_Pro.dll", CharSet = CharSet.Ansi)]
        public static extern int FeedLines(int devHandle, int line);

        // Importação de QueryStatus da DLL POSSDK_Pro
        [DllImport("Printer_BT\\POSSDK_Pro.dll", CharSet = CharSet.Ansi)]
        public static extern int QueryStatus(int devHandle, StringBuilder status);

        // Importação de BarCodePrint da DLL POSSDK_Pro
        [DllImport("Printer_BT\\POSSDK_Pro.dll", CharSet = CharSet.Ansi)]
        public static extern int BarCodePrint(int devHandle, int type, string data, int dataLen, string format);

        // Importação de PrintDownloadedImage da DLL POSSDK_Pro
        [DllImport("Printer_BT\\POSSDK_Pro.dll", CharSet = CharSet.Ansi)]
        public static extern int PrintDownloadedImage(int devHandle, int Type, int ImageID, string format);

        // Importação de DownloadImage da DLL POSSDK_Pro
        [DllImport("Printer_BT\\POSSDK_Pro.dll", CharSet = CharSet.Ansi)]
        public static extern int DownloadImage(int devHandle, int Type, string ImageList, string format);

        // Importação de PrintImageFile da DLL POSSDK_Pro
        [DllImport("Printer_BT\\POSSDK_Pro.dll", CharSet = CharSet.Ansi)]
        public static extern int PrintImageFile(int devHandle, string Image, string format);

        [DllImport("Printer_BT\\POSSDK_Pro.dll", CharSet = CharSet.Ansi)]
        public static extern int SendCommand(int devHandle, byte[] command, int length);

       








        public enum BT_x
        {
            Min = 0,      // Default Value
            Max = 65535
        }

        public enum BT_y
        {
            Min = 0,      // Default Value
            Max =65535
        }

        public enum BT_Alignment
        {
            left = 0,     // Default Value
            centre = 1,
            right =2
        }

        public  enum BT_Width
        {
            Min = 0,
            Max = 2000
        }

        public enum BT_Height
        {
            Min = 0,
            Max = 2000
        }

        public enum BT_Zoom_Scale
        {
           Value = 0,
        }

        public enum BT_Zoom_Type
        {
            nearest = 0,
            bilinear = 1,
            bicubic = 2,
        }

        public enum Dither
        {
            threshold,
            bayer_m3,
            bayer_m4_algo,
            error_difuse
        }

        public enum BT_Threshold
        {
            Min = 0,
            Max = 255
        }



        public enum BT_Content
        {
            Text,
            Image,
            Font
        }

        public enum BT_Font
        {
            font_A = 0, // Fonte A (12x24)
            font_B = 1  // Fonte B (9x17)
        }
        
        public enum BT_TextScale
        {
            Min = 1,        // Minimum scale (horizontal or vertical)
            Max = 6         // Maximum scale for internal fonts (or 15-170 for TrueType fonts)
        }
        public enum BT_TextStyle
        {
            Normal = 0,     // Normal (non-bold, non-italicized, non-underlined)
            Italic = 1,     // Italics

        }
        public enum BT_Bold
        {
            Unbold = 0,
            Bold = 1       
        }

        public enum BT_Underline
        {
             noUnderline = 0,
             underline = 1,

        }

        public enum BT_Reverse
        {
            noReverse = 0,
            Reverse = 1
        }

        public enum BT_Italics
        {
            noItalics = 0,  // Value Default
            Italics = 1,
        }
        public enum BT_Zoom
        {
            Nearest = 1,    // Value Default
            Bilinear = 2,
            Bicubic = 3,
        }

        public enum BT_FeedLine
        {
            Feed = 1,       // Alimenta o papel antes de cortar
            NoFeed = 0      // Corta o papel imediatamente
        }

        public enum BT_Lines
        {
            Min = 0,
            Max = 255
        }

        // Enumeração para o tipo de corte de papel
        public enum BT_PaperCutMode
        {
            FullCut = 1,    // Corte completo
            PartialCut = 0  // Corte parcial
        }
        public enum BT_Feedline
        {
            Min = 1,    // Alimentação de 0 linhas
            Max = 255  
        }

        public enum BT_CommunicationError
        {
            
            COMMU_ERR_OPEN_PORT = -1,   // Falha ao abrir a porta
            COMMU_ERR_WRITE_PORT = -2,  // Falha ao escrever na porta
            COMMU_ERR_READ_PORT = -3,   // Falha ao ler da porta
            COMMU_ERR_TIME_OUT = -4,    // Tempo de leitura/escrita excedido
            COMMU_ERR_CLOSE_PORT = -5,  // Falha ao fechar a porta
            COMMU_ERR_PORT_NOT_SUPPORT = -6,  // Porta não suportada
            COMMU_ERR_PORT_REATCH_MAXNUM = -7, // O número de portas abertas atingiu o máximo
            COMMU_ERR_NO_ONE_DEVICE = -8,      // O número de dispositivos não é único
            COMMU_ERR_OTHER = -9         // Outros erros de comunicação
        }
        public enum BT_ParameterError
        {
            PARA_ERR_TEXT_X = -17,       // Erro no parâmetro de formato de texto (X)
            PARA_ERR_TEXT_Y = -18,       // Erro no parâmetro de formato de texto (Y)
            PARA_ERR_TEXT_LINESPACE = -19, // Erro no parâmetro de espaçamento de linhas
            PARA_ERR_FORMAT = -20,        // Erro de formato de parâmetro
            PARA_ERR_ITEM_ILLEGAL = -21,  // Valor de parâmetro ilegal
            PARA_ERR_FILE_NOT_EXIST = -22, // Arquivo não encontrado ou falha ao abrir
            PARA_ERR_TEXT_BOLD = -23,     // Erro no parâmetro de formato de texto (BOLD)
            PARA_ERR_TEXT_ALIGNMENT = -24, // Erro no parâmetro de alinhamento de texto
            PARA_ERR_TEXT_HORMAGNIFY = -25, // Erro no parâmetro de magnificação horizontal
            PARA_ERR_TEXT_VERMAGNIFY = -26, // Erro no parâmetro de magnificação vertical
            PARA_ERR_TEXT_UNDERLINE = -27, // Erro no parâmetro de sublinhado
            PARA_ERR_TEXT_ITALICS = -28,  // Erro no parâmetro de itálico
            PARA_ERR_TEXT_FONT = -29,     // Erro no parâmetro de fonte de texto
            PARA_ERR_BAR_TYPE = -30,      // Erro no tipo de código de barras
            PARA_ERR_BAR_DATA = -31,      // Erro nos dados do código de barras
            PARA_ERR_BAR_DATALEN = -32,   // Erro no comprimento dos dados do código de barras
            PARA_ERR_BAR_BASICWIDTH = -33, // Erro no parâmetro do código de barras (largura do elemento básico)
            PARA_ERR_BAR_HEIGHT = -34,    // Erro no parâmetro do código de barras (altura)
            PARA_ERR_BAR_HRIPOS = -35,    // Erro no parâmetro de posição do texto no código de barras
            PARA_ERR_BAR_HRIFONT = -36,   // Erro no parâmetro do tipo de fonte do código de barras
            PARA_ERR_BAR_X = -37,         // Erro na distância horizontal do código de barras
            PARA_ERR_BAR_Y = -38,         // Erro na distância vertical do código de barras
            PARA_ERR_BAR_ERRORCORRECT = -39, // Erro no nível de correção de erro do código de barras
            PARA_ERR_BAR_ROWS = -40,      // Erro no número de linhas do código de barras
            PARA_ERR_BAR_COLS = -41,      // Erro no número de colunas do código de barras
            PARA_ERR_BAR_SCALEH = -42,    // Erro na altura da aparência do código de barras
            PARA_ERR_BAR_SCALEV = -43,    // Erro na largura da aparência do código de barras
            PARA_ERR_BAR_SYMBOLTYPE = -44, // Erro no tipo de símbolo do código de barras
            PARA_ERR_BAR_LANGUAGE_MODE = -45, // Erro no modo de linguagem do código de barras
            PARA_ERR_BAR_GS1TYPE = -46,   // Erro no tipo de código de barras GS1
            PARA_ERR_BAR_BASICHEIGHT = -47, // Erro na altura básica do código de barras GS1
            PARA_ERR_BAR_SEGMENTNUM = -48, // Erro no número de segmentos de código de barras GS1
            PARA_ERR_BAR_SEPHEIGHT = -49, // Erro na altura do separador GS1
            PARA_ERR_BAR_HRITYPE = -50,   // Erro no tipo de caractere de comentário GS1
            PARA_ERR_BAR_AI = -51,        // Erro no identificador de aplicação GS1
            PARA_ERR_ARRAY_TOO_SMALL = -52, // O array fornecido é muito pequeno
            PARA_ERR_PROPERTY_NAME = -53, // Erro no nome da propriedade no formato de string
            PARA_ERR_BMP_TYPE = -60,      // Erro no parâmetro do tipo de imagem BMP
            PARA_ERR_BMP_IMAGE = -61,     // Erro no caminho do arquivo da imagem BMP
            PARA_ERR_BMP_ID = -62,        // Erro no ID da imagem BMP
            PARA_ERR_BMP_X = -63,         // Erro na coordenada X da imagem BMP
            PARA_ERR_BMP_Y = -64,         // Erro na coordenada Y da imagem BMP
            PARA_ERR_BMP_WIDTH = -65,     // Erro na largura da imagem BMP
            PARA_ERR_BMP_HEIGHT = -66,    // Erro na altura da imagem BMP
            PARA_ERR_BMP_SCALE = -67,     // Erro na escala da imagem BMP
            PARA_ERR_BMP_ZOOM_PARAM = -68, // Erro no tipo de zoom da imagem BMP
            PARA_ERR_BMP_DITHER = -69,    // Erro na dithering da imagem BMP
            PARA_ERR_BMP_THRESHOLD = -70, // Erro no valor de threshold da imagem BMP
            PARA_ERR_BMP_FILEFORMAT = -71, // Erro no formato do arquivo de imagem BMP
            PARA_ERR_BMP_TOOBIG = -72     // A imagem é muito grande
        }
        public enum BT_BarcodeType
        {
            UPC_A = 1,       // UPC-A (0-9, 11-12 caracteres)
            UPC_E = 2,       // UPC-E (0-9, 11-12 ou 6-8 caracteres)
            JAN13 = 3,       // JAN13 (EAN13) (0-9, 12-13 caracteres)
            JAN8 = 4,        // JAN8 (EAN8) (0-9, 7-8 caracteres)
            CODE39 = 5,      // Code39 (0-9, A-Z, $% +-./, 1-255 caracteres)
            ITF = 6,         // ITF (0-9, 1-255 caracteres)
            CODEBAR = 7,     // Codebar (0-9, A-D, $+-./:, 1-255 caracteres)
            CODE93 = 8,      // Code93 (ASCII 0-127, 1-255 caracteres)
            CODE128 = 9,     // Code128 (ASCII 0-127, 2-255 caracteres)
            QRCODE = 10,     // QR Code (0x00-0xFF, caracteres chineses, 1-928 caracteres)
            PDF417 = 11,     // PDF417 (0x00-0xFF, caracteres chineses, 1-3600 caracteres)
            MAXICODE = 12,   // MaxiCode (0-127 ASCII e 128-255 ASCII estendido, 1-93 caracteres)
            GS1 = 13         // GS1 (Consulte a propriedade GS1Type para detalhes)
        }



    }
}
