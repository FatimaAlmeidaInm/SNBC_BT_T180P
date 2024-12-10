using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace POSSDK_Pro_Demo
{
    public static class SampleBarcode
    {
        public static int Sample_PrintBarCode(int handle)
        {
            int result = 0;
            const string format = "BasicWidth=3|Height=60";
            LoadPOSSDKPro.PrintTextWrapper(handle, "UPC-A\n", "");
            const string data_UPCA = "123456789012";
            LoadPOSSDKPro.BarCodePrintWrapper(handle, LoadPOSSDKPro.BarCode_UPCA, data_UPCA, data_UPCA.Length, format);
            LoadPOSSDKPro.FeedLinesWrapper(handle, 1);

            LoadPOSSDKPro.PrintTextWrapper(handle, "UPC-E\n", "");
            const string data_UPCE = "023456789012";
            LoadPOSSDKPro.BarCodePrintWrapper(handle, LoadPOSSDKPro.BarCode_UPCE, data_UPCE, data_UPCE.Length, format);
            LoadPOSSDKPro.FeedLinesWrapper(handle, 1);

            LoadPOSSDKPro.PrintTextWrapper(handle, "EAN13\n", "");
            const string data_EAN13 = "3456789012345";
            LoadPOSSDKPro.BarCodePrintWrapper(handle, LoadPOSSDKPro.BarCode_EAN13, data_EAN13, data_EAN13.Length, format);
            LoadPOSSDKPro.FeedLinesWrapper(handle, 1);

            LoadPOSSDKPro.PrintTextWrapper(handle, "EAN8\n", "");
            const string data_EAN8 = "12345678";
            LoadPOSSDKPro.BarCodePrintWrapper(handle, LoadPOSSDKPro.BarCode_EAN8, data_EAN8, data_EAN8.Length, format);
            LoadPOSSDKPro.FeedLinesWrapper(handle, 1);

            LoadPOSSDKPro.PrintTextWrapper(handle, "CODE39\n", "");
            const string data_CODE39 = "01234ABCDE $%+-./";
            LoadPOSSDKPro.BarCodePrintWrapper(handle, LoadPOSSDKPro.BarCode_Code39, data_CODE39, data_CODE39.Length, "BasicWidth=2|Height=60");
            LoadPOSSDKPro.FeedLinesWrapper(handle, 1);

            LoadPOSSDKPro.PrintTextWrapper(handle, "ITF\n", "");
            const string data_ITF = "01234567891234";
            LoadPOSSDKPro.BarCodePrintWrapper(handle, LoadPOSSDKPro.BarCode_ITF, data_ITF, data_ITF.Length, format);
            LoadPOSSDKPro.FeedLinesWrapper(handle, 1);

            LoadPOSSDKPro.PrintTextWrapper(handle, "CODEBAR\n", "");
            const string data_CODEBAR = "A0123456789$+-./:D";
            LoadPOSSDKPro.BarCodePrintWrapper(handle, LoadPOSSDKPro.BarCode_Codebar, data_CODEBAR, data_CODEBAR.Length, "BasicWidth=2|Height=60");
            LoadPOSSDKPro.FeedLinesWrapper(handle, 1);

            LoadPOSSDKPro.PrintTextWrapper(handle, "CODE93\n", "");
            const string data_CODE93 = "01234ABCDE $%+-./";
            LoadPOSSDKPro.BarCodePrintWrapper(handle, LoadPOSSDKPro.BarCode_Code93, data_CODE93, data_CODE93.Length, format);
            LoadPOSSDKPro.FeedLinesWrapper(handle, 1);

            LoadPOSSDKPro.PrintTextWrapper(handle, "CODE128\n", "");
            const string data_CODE128 = "01234ABCDExyz";
            LoadPOSSDKPro.BarCodePrintWrapper(handle, LoadPOSSDKPro.BarCode_Code128, data_CODE128, data_CODE128.Length, format);
            LoadPOSSDKPro.FeedLinesWrapper(handle, 1);

            LoadPOSSDKPro.PrintTextWrapper(handle, "QR\n", "");
            const string data_QR = "0123456789ABCDEFGHIJKLmnopqrstuvwxyz汉字";
            LoadPOSSDKPro.BarCodePrintWrapper(handle, LoadPOSSDKPro.BarCode_QR, data_QR, data_QR.Length, "");
            LoadPOSSDKPro.FeedLinesWrapper(handle, 1);

            LoadPOSSDKPro.PrintTextWrapper(handle, "PDF417\n", "");
            const string data_PDF417 = "0123456789ABCDEFGHIJKLmnopqrstuvwxyz汉字";
            LoadPOSSDKPro.BarCodePrintWrapper(handle, LoadPOSSDKPro.BarCode_PDF417, data_PDF417, data_PDF417.Length, "");
            LoadPOSSDKPro.FeedLinesWrapper(handle, 1);

            LoadPOSSDKPro.PrintTextWrapper(handle, "MAXICODE\n", "");
            const string data_MAXICODE = "0123456789ABCDEFGHIJKLmnopqrstuvwxyz";
            LoadPOSSDKPro.BarCodePrintWrapper(handle, LoadPOSSDKPro.BarCode_MAXICODE, data_MAXICODE, data_MAXICODE.Length, "");
            LoadPOSSDKPro.FeedLinesWrapper(handle, 1);

            LoadPOSSDKPro.PrintTextWrapper(handle, "GS1\n", "");
            const string data_GS1 = "01234567891234";
            LoadPOSSDKPro.BarCodePrintWrapper(handle, LoadPOSSDKPro.BarCode_GS1, data_GS1, data_GS1.Length, "GS1Type=1");
            LoadPOSSDKPro.FeedLinesWrapper(handle, 1);

            result = LoadPOSSDKPro.PaperCutWrapper(handle, 0, 0);

            return result;
        }
    }
}
