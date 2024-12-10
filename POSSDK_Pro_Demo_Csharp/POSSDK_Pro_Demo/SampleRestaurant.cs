using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace POSSDK_Pro_Demo
{
    public static class SampleRestaurant
    {
        // 正常状态
        const string normalStatus = "Normal";
        const string format = "Bold=1|HScale=2|VScale=2";
        // 中文收据小票样张
        public static int Sample_Restaurant(int handle, out string errorStatus)
        {
            errorStatus = "";
            int result = 0;
            int nStatus = 0;
            string status;

            // check status
            nStatus = LoadPOSSDKPro.QueryStatusWrapper(handle, out status);
            if (!status.Equals(normalStatus, StringComparison.Ordinal))
            {
                errorStatus = status;
                return nStatus;
            }

            const string lineBetwen = "------------------------------------------------\n";

            LoadPOSSDKPro.PrintTextWrapper(handle, lineBetwen, "Bold=0|Align=0|HScale=1|VScale=1|Underline=0|Italics=0|Font=FontA");
            LoadPOSSDKPro.PrintTextWrapper(handle, "2020年04月10日09：48\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, lineBetwen, "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "#20美团外卖\n", "Bold=1|Align=1|HScale=2|VScale=2");
            LoadPOSSDKPro.PrintTextWrapper(handle, "切尔西ChelseaKitchen\n", "Bold=0|HScale=1|VScale=1");
            LoadPOSSDKPro.PrintTextWrapper(handle, "在线支付(已支付)\n", format);
            LoadPOSSDKPro.PrintTextWrapper(handle, "订单号：5415221202244734\n", "Bold=0|HScale=1|VScale=1");
            LoadPOSSDKPro.PrintTextWrapper(handle, "下单时间：2021-04-10 10：00：00\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "---------------------1号口袋--------------------\n", "Align=0");
            LoadPOSSDKPro.PrintTextWrapper(handle, "意大利茄汁意面X1                            32.9\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "7寸浓香芝士披萨X1                           34.9\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "葡式蛋挞2个装X1                                9\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "9寸培根土豆披萨X1                           54.9\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "9寸芝士加量X1                                 10\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "---------------------其他-----------------------\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "[满100.0元减40.0元]\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "[减配送费3.0元]\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "餐盒费：7\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "[赠送惠尔康茶饮 X 1]:\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, lineBetwen, "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "原价：￥141.7  \n", "Align=2");
            LoadPOSSDKPro.PrintTextWrapper(handle, "实付：￥107.7 \n", format);
            LoadPOSSDKPro.PrintTextWrapper(handle, lineBetwen, "Bold=0|Align=0|HScale=1|VScale=1");
            LoadPOSSDKPro.PrintTextWrapper(handle, "通鑫学生公寓A5-2\n", format);
            LoadPOSSDKPro.PrintTextWrapper(handle, "号（A5-107）\n", "Bold=0");
            LoadPOSSDKPro.PrintTextWrapper(handle, "131****0501\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "苏（先生）\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, lineBetwen, "HScale=1|VScale=1");
   
            result = LoadPOSSDKPro.PaperCutWrapper(handle, 0, 0);

            // check status
            nStatus = LoadPOSSDKPro.QueryStatusWrapper(handle, out status);
            if (!status.Equals(normalStatus, StringComparison.Ordinal))
            {
                errorStatus = status;
                return nStatus;
            }

            return result;
        }

        // 英文收据小票样张
        public static int Sample_Restaurant_En(int handle, out string errorStatus)
        {
            errorStatus = "";
            int result = 0;
            int nStatus = 0;
            string status;

            // check status
            nStatus = LoadPOSSDKPro.QueryStatusWrapper(handle, out status);
            if (!status.Equals(normalStatus, StringComparison.Ordinal))
            {
                errorStatus = status;
                return nStatus;
            }

            LoadPOSSDKPro.PrintTextWrapper(handle, "XxxxXxxx\n", "Bold=1|Align=1|HScale=1|VScale=1|Underline=0|Italics=0|Font=FontA");
            LoadPOSSDKPro.PrintTextWrapper(handle, "201 East 31st St.\n", "Bold=0");
            LoadPOSSDKPro.PrintTextWrapper(handle, "New York, NY 10000\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "0344590786\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "Server: Kristen                       Station: 7\n", "Align=0");
            LoadPOSSDKPro.PrintTextWrapper(handle, "------------------------------------------------\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "Order #: 123401                          Dine In\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "Table: L6                               Guest: 2\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "------------------------------------------------\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "1 Lamb Embuchado.                          12.00\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "1 NY Strip 6oz                             18.00\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "1 Mozzarella Flatbread                     10.00\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "1 Mahan                                     5.00\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "Bar Subtotal:                               0.00\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "Food Subtotal:                             45.00\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "Tax:                                        3.99\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "                                        ========\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "TOTAL:            $49.00\n", format);
            LoadPOSSDKPro.PrintTextWrapper(handle, "\n", "Bold=0|HScale=1|VScale=1");
            LoadPOSSDKPro.PrintTextWrapper(handle, ">> Ticket #: 11 <<\n", "Align=1");
            LoadPOSSDKPro.PrintTextWrapper(handle, "4/23/2019 7:03:24 PM\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "**********************************************\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "Join our mailing list for exclusive offers\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "www.XxxxXxxx.com\n", "Underline=1");
            LoadPOSSDKPro.PrintTextWrapper(handle, "\n", "Underline=0");
            LoadPOSSDKPro.PrintTextWrapper(handle, "15% Gratuity = $6.75\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "18% Gratuity = $8.10\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "20% Gratuity = $9.00\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "22% Gratuity = $9.90\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "**********************************************\n", "");
            LoadPOSSDKPro.PrintTextWrapper(handle, "Join Us For Our $5 Happy Hour Daily 5-8pm\n", "");

            result = LoadPOSSDKPro.PaperCutWrapper(handle, 0, 0);

            // check status
            nStatus = LoadPOSSDKPro.QueryStatusWrapper(handle, out status);
            if (!status.Equals(normalStatus, StringComparison.Ordinal))
            {
                errorStatus = status;
                return nStatus;
            }

            return result;
        }
    }
}
