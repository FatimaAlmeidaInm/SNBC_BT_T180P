using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace POSSDK_Pro_Demo
{
    public static class SampleXML
    {
        // std mode
        public static int Sample_XML(int handle)
        {
            int result = 0;

            string xmlStr = "<pos-print>" +
                "<image align='center'>../../../../../resource/SAMPLELOGO.jpg</image>" +
                "<text align='left' bold='false' scale-h='1' scale-v='1' italics='1' font='font_a'>Ginger A   Store   #12345REG   #02&#xA;</text>" +
                "<text>Date:2006-04-09 	       Time:18:30&#xA;</text>" +
                "<text>-----------------------------------------&#xA;</text>" +
                "<text>Item         Qty             Amount&#xA;</text>" +
                "<text>Apple	      1               $1.00&#xA;</text>" +
                "<text>Orange	      2               $5.00&#xA;</text>" +
                "<text>Brush	      1               $3.8&#xA;</text>" +
                "<text>Beer	      2               $8.00&#xA;</text>" +
                "<text>-----------------------------------------&#xA;</text>" +
                "<text>Subtotal:&#xA;</text>" +
                "<text>Tax:&#xA;</text>" +
                "<text scale-h='2' scale-v='2'>Total:       $17.80&#xA;</text>" +
                "<text scale-h='1' scale-v='1'>-----------------------------------------&#xA;</text>" +
                "<text align='center'>Customer signature&#xA;</text>" +
                "<text>Thank you&#xA;</text>" +
                "<text>Welcome next time&#xA;</text>" +
                "<symbol align='center' type='pdf417'>12345REG</symbol>" +
                "<cut/>" +
                "</pos-print>";

            result = LoadPOSSDKPro.PrintContentWrapper(handle, 0, xmlStr);

            return result;
        }

        // page mode
        public static int Sample_XML_PageMode(int handle)
        {
            int result = 0;
            
            string xmlStr = "<pos-print>" +
                "<page width='600' height='300' x='0' y='0'>" +
                "<image x='60' y='40'>../../../../../resource/SAMPLELOGO.jpg</image>" +
                "<text x='20' y='120' bold='true'>==============================&#xA;</text>" +
                "<text x='20' bold='true'>POS Printer Page Mode Sample.&#xA;</text>" +
                "<text x='60' bold='false' underline='true'>Sample Text UnderLine.&#xA;</text>" +
                "<text x='20' underline='false'>==============================&#xA;</text>" +
                "<feed/>" +
                "<text x='20' italics='false' scale-h='2' scale-v='2'>Thank you for your use.</text>" +
                "<symbol type='qr' x='400' y='200' width='7'>SAMPLE PAGE MODE</symbol>" +
                "</page>" +
                "<cut/>" +
                "</pos-print>";

            result = LoadPOSSDKPro.PrintContentWrapper(handle, 0, xmlStr);

            return result;
        }
    }
}
