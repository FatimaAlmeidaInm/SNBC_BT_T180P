using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace POSSDK_Pro_Demo
{
    public static class SampleApi
    {
        public static int Sample_EnumDevice(int type, out string deviceList)
        {
            const int MAXSIZE = 1024;
            return LoadPOSSDKPro.EnumDeviceWrapper(type, out deviceList, MAXSIZE);
        }
        public static int Sample_Open(string model, string portInfo)
        {
            return LoadPOSSDKPro.OpenPrinterWrapper(model, portInfo);
        }

        public static int Sample_Close(int handle)
        {
            return LoadPOSSDKPro.ClosePrinterWrapper(handle);
        }
    }


}
