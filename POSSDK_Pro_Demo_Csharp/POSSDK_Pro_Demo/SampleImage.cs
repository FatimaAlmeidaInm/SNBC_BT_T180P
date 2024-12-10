using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace POSSDK_Pro_Demo
{
    public static class SampleImage
    {
        private const string imgPath = "../../../../../resource/";
        private const string lineBetwen = "=====================\n";
       
        public static int Sample_PrintImage(int handle)
        {
            int result = 0;

            const string format = "x=-2|Dither=3";
            // Alignment and offset
            const string alginFileName = imgPath + "01.bmp";
            LoadPOSSDKPro.PrintTextWrapper(handle, lineBetwen, "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "Alignment and offset, x = -1\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, alginFileName, "x=-1");

            LoadPOSSDKPro.PrintTextWrapper(handle, "Alignment and offset, x = 50\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, alginFileName, "x=50");

            LoadPOSSDKPro.PrintTextWrapper(handle, "Alignment and offset, x = -2\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, alginFileName, "x=-2");

            LoadPOSSDKPro.PrintTextWrapper(handle, "Alignment and offset, x = -3\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, alginFileName, "x=-3");


            // Zoom
            LoadPOSSDKPro.PrintTextWrapper(handle, lineBetwen, "");
            const string zoomFileName = imgPath + "01.bmp";
            LoadPOSSDKPro.PrintTextWrapper(handle, "Zoom, Scale = 20\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, zoomFileName, "x=-2|Scale=20");

            LoadPOSSDKPro.PrintTextWrapper(handle, "Zoom, Scale = 50\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, zoomFileName, "x=-2|Scale=50");

            LoadPOSSDKPro.PrintTextWrapper(handle, "Zoom, Scale = 100\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, zoomFileName, "x=-2|Scale=100");

            LoadPOSSDKPro.PrintTextWrapper(handle, "Zoom, Scale = 200\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, zoomFileName, "x=-2|Scale=200");

            LoadPOSSDKPro.PrintTextWrapper(handle, "Zoom, Width = 100, Height = 200\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, zoomFileName, "x=-2|Width=100|Height=200");

            LoadPOSSDKPro.PrintTextWrapper(handle, "Zoom, Width = 200, Height = 100\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, zoomFileName, "x=-2|Width=200|Height=100");


            // Dither
            LoadPOSSDKPro.PrintTextWrapper(handle, lineBetwen, "");
            const string DitherFileName = imgPath + "dither.jpg";
            LoadPOSSDKPro.PrintTextWrapper(handle, "Dither, Dither = 0, Threshold = -1\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, DitherFileName, "x=-2|Dither=0|Threshold=-1");

            LoadPOSSDKPro.PrintTextWrapper(handle, "Dither, Dither = 0, Threshold = 100\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, DitherFileName, "x=-2|Dither=0|Threshold=100");

            LoadPOSSDKPro.PrintTextWrapper(handle, "Dither, Dither = 1\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, DitherFileName, "x=-2|Dither=1");

            LoadPOSSDKPro.PrintTextWrapper(handle, "Dither, Dither = 2\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, DitherFileName, "x=-2|Dither=2");

            LoadPOSSDKPro.PrintTextWrapper(handle, "Dither, Dither = 3\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, DitherFileName, format);


            // Type
            LoadPOSSDKPro.PrintTextWrapper(handle, lineBetwen, "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "PNG\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, imgPath + "1.png", format);

            LoadPOSSDKPro.PrintTextWrapper(handle, "JPEG\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, imgPath + "2.jpg", format);

            LoadPOSSDKPro.PrintTextWrapper(handle, "TIF\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, imgPath + "3.tif", format);

            LoadPOSSDKPro.PrintTextWrapper(handle, "GIF\n", "");
            LoadPOSSDKPro.PrintImageFileWrapper(handle, imgPath + "4.gif", format);

            LoadPOSSDKPro.PaperCutWrapper(handle, 0, 0);

            result = DownLoadAndPrintImage(handle);

            return result;
        }

        private static int DownLoadAndPrintImage(int handle)
        {
            int result = 0;

            // RAM
            LoadPOSSDKPro.PrintTextWrapper(handle, lineBetwen, "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "DownLoadImage RAM\n", "");
            const string ramimageList = imgPath + "RAM1.bmp" + "|" + imgPath + "RAM2.bmp" + "|" + imgPath + "RAM3.bmp";
            result = LoadPOSSDKPro.DownloadImageWrapper(handle, LoadPOSSDKPro.IMAGE_TYPE_RAM, ramimageList, "");
            if (result == LoadPOSSDKPro.SUCCESS)
            {
                int printRAMImageID = 1;
                LoadPOSSDKPro.PrintDownloadedImageWrapper(handle, LoadPOSSDKPro.IMAGE_TYPE_RAM, printRAMImageID++, "X=-2");
                LoadPOSSDKPro.PrintDownloadedImageWrapper(handle, LoadPOSSDKPro.IMAGE_TYPE_RAM, printRAMImageID++, "X=-2");
                LoadPOSSDKPro.PrintDownloadedImageWrapper(handle, LoadPOSSDKPro.IMAGE_TYPE_RAM, printRAMImageID, "X=-2");
            }
            else
            {
                string error = "DownLoadImage failed. error = " + result + "\n";
                LoadPOSSDKPro.PrintTextWrapper(handle, error, "");
            }

            // Flash
            LoadPOSSDKPro.PrintTextWrapper(handle, lineBetwen, "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "DownLoadImage Flash\n", "");
            const string FlashimageList = imgPath + "Flash1.bmp" + "|" + imgPath + "Flash2.bmp" + "|" + imgPath + "Flash3.bmp";
            result = LoadPOSSDKPro.DownloadImageWrapper(handle, LoadPOSSDKPro.IMAGE_TYPE_FLASH, FlashimageList, "");
            if (result == LoadPOSSDKPro.SUCCESS)
            {
                int printFlashImageID = 1;
                LoadPOSSDKPro.PrintDownloadedImageWrapper(handle, LoadPOSSDKPro.IMAGE_TYPE_FLASH, printFlashImageID++, "X=-2");
                LoadPOSSDKPro.PrintDownloadedImageWrapper(handle, LoadPOSSDKPro.IMAGE_TYPE_FLASH, printFlashImageID++, "X=-2");
                LoadPOSSDKPro.PrintDownloadedImageWrapper(handle, LoadPOSSDKPro.IMAGE_TYPE_FLASH, printFlashImageID, "X=-2");
            }
            else
            {
                string error = "DownLoadImage failed. error = " + result + "\n";
                LoadPOSSDKPro.PrintTextWrapper(handle, error, "");
            }
            return result;
        }
    }
}
