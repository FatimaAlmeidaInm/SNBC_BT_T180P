using System.Runtime.InteropServices;
using System.Text;
using System;


namespace Printer_SNBC_BT
{
    public static class SNBC_BT

    {
        [DllImport("POSSDK_Pro.dll")] 
        public static extern int EnumDevice(int portType, StringBuilder deviceInfo, int deviceInfoLen);
        
        [DllImport("POSSDK_Pro.dll")] 
        public static extern int OpenPrinter(string modelName, string portInfo);

        [DllImport("POSSDK_Pro.dll")] 
        public static extern int ClosePrinter(int devHandle);

        [DllImport("POSSDK_Pro.dll")]
        public static extern int PrintReset(int devHandle);

        [DllImport("POSSDK_Pro.dll")]
        public static extern int PrintText(int devHandle, string content, string font);

        [DllImport("POSSDK_Pro.dll")]
        public static extern int PaperCut(int devHandle, int cutMode, int distance);

        [DllImport("POSSDK_Pro.dll")] 
        public static extern int FeedLines(int devHandle, int line);

        [DllImport("POSSDK_Pro.dll")]
        public static extern int QueryStatus(int devHandle, StringBuilder status);

        [DllImport("POSSDK_Pro.dll")]
        public static extern int BarCodePrint(int devHandle, int type, string data, int dataLen, string format);

        [DllImport("POSSDK_Pro.dll")]
        public static extern int PrintImageFile(int devHandle, string Image, string format);

        [DllImport("POSSDK_Pro.dll")]
        public static extern int PrintDownloadedImage(int devHandle, int Type, int ImageID, string format);
        
        [DllImport("POSSDK_Pro.dll")]
        public static extern int DownloadImage(int devHandle, int Type, string ImageList, string format);

        [DllImport("POSSDK_Pro.dll")]
        private static extern int PrintContent(int devHandle, int Type, string content);

        [DllImport("POSSDK_Pro.dll")]
        public static extern int BankDeposit(int devHandle, string content, string font);

        [DllImport("POSSDK_Pro.dll")]
        public static extern int TotalVault(int devHandle, string content, string font);

        [DllImport("POSSDK_Pro.dll")]
        public static extern int PeriodSummary(int devHandle, string content, string font);

        [DllImport("POSSDK_Pro.dll")]
        public static extern int PeriodTransactions(int devHandle, string content, string font);

        [DllImport("POSSDK_Pro.dll")]
        public static extern int PeriodOpening(int devHandle, string content, string font);

        [DllImport("POSSDK_Pro.dll")]
        public static extern int PeriodClosing(int devHandle, string content, string font);

        public const int BarCode_UPCA = 1;
        public const int BarCode_UPCE = 2;
        public const int BarCode_EAN13 = 3;
        public const int BarCode_EAN8 = 4;
        public const int BarCode_Code39 = 5;
        public const int BarCode_ITF = 6;
        public const int BarCode_Codebar = 7;
        public const int BarCode_Code93 = 8;
        public const int BarCode_Code128 = 9;
        public const int BarCode_QR = 10;
        public const int BarCode_PDF417 = 11;
        public const int BarCode_MAXICODE = 12;
        public const int BarCode_GS1 = 13;

        public const int IMAGE_TYPE_RAM = 0;
        public const int IMAGE_TYPE_FLASH = 1;



        /// <summary>
        /// Enumeração de códigos de erro do SDK
        /// </summary>
        public enum SdkErrorCode
        {
            // Sucesso
            SUCCESS = 0,

            // Erros de Comunicação
            COMMU_ERR_OPEN_PORT = -1,
            COMMU_ERR_WRITE_PORT = -2,
            COMMU_ERR_READ_PORT = -3,
            COMMU_ERR_TIME_OUT = -4,
            COMMU_ERR_CLOSE_PORT = -5,
            COMMU_ERR_PORT_NOT_SUPPORT = -6,
            COMMU_ERR_PORT_REACH_MAXNUM = -7,
            COMMU_ERR_NO_ONE_DEVICE = -8,

            // Erros de Parâmetros
            PARA_ERR_TEXT_X = -17,
            PARA_ERR_TEXT_Y = -18,
            PARA_ERR_TEXT_LINESPACE = -19,
            PARA_ERR_TEXT_BOLD = -23,
            PARA_ERR_TEXT_ALIGNMENT = -24,
            PARA_ERR_TEXT_HORMAGNIFY = -25,
            PARA_ERR_TEXT_UNDERLINE = -27,
            PARA_ERR_TEXT_FONT = -29,

            PARA_ERR_BAR_TYPE = -30,
            PARA_ERR_BAR_DATA = -31,
            PARA_ERR_BAR_DATALEN = -32,
            PARA_ERR_BAR_HEIGHT = -34,

            PARA_ERR_BMP_TYPE = -60,
            PARA_ERR_BMP_IMAGE = -61,
            PARA_ERR_BMP_WIDTH = -65,
            PARA_ERR_BMP_HEIGHT = -66,

            // Erros Gerais
            ERR_LOAD_RESOURCE_FAIL = -80,
            ERR_FUNC_NOT_SUPPORT = -81,
            ERR_INVALID_HANDLE = -82,
            ERR_PORT_ENUM_DEVICE_FAILED = -84,

            // Processamento de Imagem
            ERR_IMG_CONVERSION_BMP = -140,
            ERR_IMG_DITHER = -145,
            ERR_IMG_ZOOM = -146
        }
        

    }


    


}





