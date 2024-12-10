using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace POSSDK_Pro_Demo
{
    public static class LoadPOSSDKPro
    {
        // POSSDKPro.dll及其依赖库所在路径
        public const string POSSDK_Pro_Lib_Path = "..\\..\\..\\..\\..\\Libs\\x86\\";
        public const string POSSDK_Pro_Dll_Path = POSSDK_Pro_Lib_Path + "POSSDK_Pro.dll";

        /*
         * 
         *  @~Chinese
         *  @brief 枚举设备
         * 
         *  @param [in]    portType    端口类型。
         *  @param [out]   deviceInfo   设备信息，每个设备名称结束符为@，此结束符不作为枚举的设备名称信息，只用于划分多个设备。
         *                     如deviceInfo可能是“USBAPI|192@USBCLASS|BTP-U80(U)1@”、“192.168.1.251@ 192.168.1.249@”，用户可通过‘@’字符进行拆分           
         *  @param [in]    deviceInfoLen    设备信息长度，需与deviceInfo大小一致。
         *  @return
         *  成功：≥0，表示枚举到的设备数量，以上面deviceInfo返回值是“USBAPI|192@USBCLASS|BTP-U80@”为例，
         *       因为这里有“USBAPI|192”和“USBCLASS|BTP-U80”两个设备，则返回值为2
         *  失败：错误码
         * 
         *  @~English
         *  @brief Enumerate devices
         * 
         *  @param [in]    portType    Port Type。
         *  @param [out]   deviceInfo  The buffer of name list each name terminator with @, this terminator is not used as 
         * 					the name information for the enumeration. It is only used to divide multiple name devices. Such 
         * 					as "deviceInfo" maybe "USBAPI | 192@USBCLASS|BTP - U80(U)1@"、"192.168.1.251@ 192.168.1.249@"，
         * 					Users can split it by character"@" .
         *  @param [in]    deviceInfoLen    The length of the device information must be the same as the size of the deviceInfo.
         *  @return
         *  Success:≥0,indicates the number of devices enumerated,Take the above deviceInfo return value as
         * 			"USBAPI|192@USBCLASS|BTP-U80@"as an example,Because there are two devices"USBAPI|192" and "USBCLASS|BTP-U80", 
         * 			the return value is 2.
         *  Failure: error code
         * 
         */
        [DllImport(POSSDK_Pro_Dll_Path)]
        extern private static int EnumDevice(int portType, StringBuilder deviceInfo, int deviceInfoLen);
        public static int EnumDeviceWrapper(int portType, out string deviceInfo, int deviceInfoLen)
        {
            deviceInfo = "";
            StringBuilder enumList = new StringBuilder(deviceInfoLen);
            int nRet = EnumDevice(portType, enumList, deviceInfoLen);
            if (nRet >= 0)
            {
                deviceInfo = enumList.ToString();
            }
            return nRet;
        }

        /*
        * 
        *  @~Chinese
        *  @brief 打开端口
        * 
        *  @param [in]   modelName    打印机型号名称，例如“BTP-R880NP”。
        *  @param [in]   portInfo     端口信息，如  "COM1|19200"、"USB"、"USBAPI|912"或
        *                             "USBCLASS | BTP - S80(U)1"或"192.168.1.251"
        *  @return
        *  成功：≥0的设备句柄
        *  失败：错误码
        * 
        *  @~English
        *  @brief Open Printer
        * 
        *  @param [in]   modelName    Printer model name, for example "BTP-N80".
        *  @param [in]   portInfo     port information,such as  "COM1|19200","USB","USBAPI|912" or
        *                             "USBCLASS | BTP - S80(U)1" or "192.168.1.251"
        *  @return
        *  Success: ≥ 0 device handle
        *  Failure: error code
        * 
        */
        [DllImport(POSSDK_Pro_Dll_Path)]
        extern private static int OpenPrinter(string modelName, string portInfo);
        public static int OpenPrinterWrapper(string modelName, string portInfo)
        {
            int nRet = OpenPrinter(modelName, portInfo);
            return nRet;
        }

        /*
         * 
         *  @~Chinese
         *  @brief 关闭端口
         * 
         *  @param [in]    devHandle   设备句柄，即OpenPrinter的有效返回值，当只有一台设备时可以传0。
         *                             
         *  @return
         *  成功：0
         *  失败：错误码
         * 
         * 	*  @~English
         *  @brief Close Printer
         * 
         *  @param [in]    devHandle   Device handle. The effective return value of OpenPrinter. 
         *  							  When there is only one device, it can be 0.
         *                             
         *  @return
         *  Success: 0
         *  Failure: error code
         * 
         */
        [DllImport(POSSDK_Pro_Dll_Path)]
        extern private static int ClosePrinter(int devHandle);
        public static int ClosePrinterWrapper(int devHandle)
        {
            int nRet = ClosePrinter(devHandle);
            return nRet;
        }

        /*
         * 
         *  @~Chinese
         *  @brief 文本打印
         * 
         *  @param [in]   devHandle   设备句柄，即OpenPrinter的有效返回值，当只有一台设备时可以传0。
         *  @param [in]   content     打印内容
         *  @param [in]   font     该参数为组合，如“Font=FontA|Bold=1| Italics =1|Underline=1”。
         *                     也可以为NULL或空字符串，若未设置过字体风格，Font字体默认为FontA，其他默认为不加粗、
         *                     左对齐、横向放大、无下划线、非斜体。字体在设置为FontA或者FontB时，HScale和VScale的
         *                     取值范围为1-6，其他情况取值范围为15-170。若之前已设置过字体风格，则延续上次设置。
         *              字段含义	   字段名称	    属性值
         *               加粗		Bold	    1 / 0（加粗 / 不加粗）
         * 				 对齐		Align		0 / 1 / 2（左对齐 / 居中 / 右对齐）
         * 				 横向放大	HScale		1 - 6（TTF为15 - 170）
         * 				 纵向放大	VScale		1 - 6（TTF为15 - 170）
         * 				 下划线		Underline	1 / 0（下划线 / 无）
         * 				 倾斜		Italics		1 / 0（斜体 / 无）
         * 				 字体		Font		详见下部内部字体
         * 
         * 							内部字体		备注
         * 							FontA		12 * 24内部字体 / （或其他语言的标准宋体字）
         * 							FontB		9 * 17内部字体
         * 							楷体			TrueType字体
         * 							华文宋体		TrueType字体
         * 							…			其他TrueType字体
         * 	说明
         * 		1.	TrueType字体不支持单行多种字体风格。
         * 		2.	TrueType字体HScale和VScale大于等于15，小于170, 为字号，推荐使用比例1:2，例如“HScale = 15 | VScale = 30”。
         * 		3.	TrueType字体不支持对齐、斜体、下划线。
         * 		4.	调用PrintText接口，数据结束时需添加\n换行符, 或者调用FeedLines接口才会打印。
         *  @return
         *  成功：0
         *  失败：错误码
         * 
         *  @~English
         *  @brief Text Print
         * 
         *  @param [in]   devHandle   Device handle. The effective return value of OpenPrinter. 
         * 							  When there is only one device, it can be 0.
         *  @param [in]   content     print contents
         *  @param [in]   font     This parameter is a combination, such as ”Font=FontA|Bold=1| 
         * 						   Italics =1|Underline=1”, It can also be null or empty string. 
         * 						   If font style has not been set, font is fontA by default, and 
         * 						   others are non bold, left aligned, horizontal enlarged, non underlined 
         * 						   and non italicized by default. When the font is set to FontA or FontB, 
         * 						   the value range of HScale and VScale is 1-6, and the value range of other 
         * 						   cases is 15-170.
         * 						   If the font style has been set before, the last setting will continue.
         * 
         * 						   Field meaning	Field name	Attribute value
         * 						   Bold				Bold		1 / 0(Bold / unbold)
         * 						   alignment		Align		0 / 1 / 2(Align left / Center / Align right)
         * 						   Horizontal 		HScale		1 - 6(TTF is 15 - 170)
         * 						   Vertical 		VScale		1 - 6(TTF is 15 - 170)
         * 						   Underline		Underline	1 / 0(Underline / none)
         * 						   Italics			Italics		1 / 0(Italics / none)
         * 						   Font				Font		See the internal fonts in the table below for details
         * 							
         * 							Internal font	Remarks
         * 							FontA			12 * 24 Internal font / (Or standard Song font in other languages)
         * 							FontB			9 * 17 Internal font
         * 							Regular			script	TrueType fonts
         * 							stsong			TrueType  fonts
         * 							…				Other TrueType fonts
         *  @return
         *  Success: 0
         *  Failure: error code
         * 
         */
        [DllImport(POSSDK_Pro_Dll_Path)]
        extern private static int PrintText(int devHandle, string content, string font);
        public static int PrintTextWrapper(int devHandle, string content, string font)
        {
            int nRet = PrintText(devHandle, content, font);
            return nRet;
        }

        /*
           * 
           *  @~Chinese
           *  @brief 切纸
           * 
           *  @param [in]   devHandle		设备句柄，即OpenPrinter的有效返回值，当只有一台设备时可以传0。
           *  @param [in]   cutMode		全切 1，半切 0
           *  @param [in]   distance		切纸前走纸距离,取值范围（0~255），单位：点
           * 
           * 	说明
           * 		1.	切纸情况与具体机型有关，部分机型仅支持半切或者全切
           * 		2.	该接口确保切纸时，将打印内容送出切刀之后，切纸不会切在打印内容上。
           * 		3.	由于各打印机打印头到切刀之间距离不同，出纸模块结构不同等原因，distance参数实际生效值范围不为0 - 255，
           * 			需要根据实际情况进行调整。
           *  @return
           *  成功：0
           *  失败：错误码
           * 
           *  @~Chinese
           *  @brief Cut Paper
           * 
           *  @param [in]   devHandle		Device handle, which is the effective return value of openprinter, 
           * 								can be passed to 0 when there is only one device.
           *  @param [in]   cutMode		Full cut 1, half cut 0
           *  @param [in]   distance		Distance before paper cutting, value range (0 ~ 255), unit: point
           * 
           * 	Description
           * 		1.	The paper cutting situation is related to the specific model, some models only 
           * 			support half cutting or full cutting
           * 		2.	The interface ensures that the paper will not be cut on the printed content 
           * 			after the printed content is sent out of the cutter.
           * 		3.	Due to the different distance between the printer head and the cutter and 
           * 			the different structure of the paper output module, the actual effective value range 
           * 			of the distance parameter is not 0-255, which needs to be adjusted according to the 
           * 			actual situation.
           *  @return
           *  Success: 0
           *  Failure: error code
           * 
           */
        [DllImport(POSSDK_Pro_Dll_Path)]
        extern private static int PaperCut(int devHandle, int cutMode, int distance);
        public static int PaperCutWrapper(int devHandle, int cutMode, int distance)
        {
            int nRet = PaperCut(devHandle, cutMode, distance);
            return nRet;
        }

        /*
         * 
         *  @~Chinese
         *  @brief 走纸
         * 
         *  @param [in]   devHandle		设备句柄，即OpenPrinter的有效返回值，当只有一台设备时可以传0。
         *  @param [in]   line			进纸行数，取值范围（0~255）单位：行
         * 
         * 	说明：
         * 		打印打印机缓冲区内数据并走纸line行。
         *  @return
         *  成功：0
         *  失败：错误码
         * 
         *  @~English
         *  @brief Feed Paper
         * 
         *  @param [in]   devHandle		Device handle, which is the effective return value of openprinter, 
         * 								can be passed to 0 when there is only one device.
         *  @param [in]   line			Number of feed lines, value range (0 ~ 255) unit: Lines
         * 
         * 	Description:
         * 		Print the data in the printer buffer and feed the paper line
         *  @return
         *  Success: 0
         *  Failure: error code
         * 
         */
        [DllImport(POSSDK_Pro_Dll_Path)]
        extern private static int FeedLines(int devHandle, int line);
        public static int FeedLinesWrapper(int devHandle, int line)
        {
            int nRet = FeedLines(devHandle, line);
            return nRet;
        }

        /*
	    * 
	    *  @~Chinese
	    *  @brief 状态查询
	    * 
	    *  @param [in]    devHandle	设备句柄，即OpenPrinter的有效返回值，当只有一台设备时可以传0。
	    *  @param [out]   status		状态为字符串叠加，例如：“PaperOut|CoverOpen”
	    * 					字符串			含义			值
	    * 					Normal			正常			1
	    * 					PaperOut		缺纸			2
	    * 					PaperNearEnd	纸将尽		4
	    * 					CoverOpen		上盖抬起		8
	    * 					HeadOverheated	打印头过热	16
	    * 					CutterError		切刀错		32
	    * 	说明
	    * 		1.	需要传入status数组长度≥512
	    * 		2.	支持返回值解析状态，如上表中值。
	    * 
	    *  @return
	    *  成功：成功：≥0的状态码，可按位解析状态，且与status返回值含义保持一致
	    *  失败：错误码
	    * 
	    *  @~English
	    *  @brief Query Status
	    * 
	    *  @param [in]    devHandle	Device handle, which is the effective return value of openprinter, 
	    * 								can be passed to 0 when there is only one device.
	    *  @param [out]   status		The status is character string overlay, for example:“PaperOut|CoverOpen”
	    * 					
	    * 								Character		string		Meaning	Value
	    * 								Normal			Normal				1
	    * 								PaperOut		Paper Out			2
	    * 								PaperNearEnd	Paper Near End		4
	    * 								CoverOpen		Cover Open			8
	    * 								HeadOverheated	Head Over heated	16
	    * 								CutterError		Cutter Error		32
	    * 	Description
	    * 		1.	The length of the passed in status array should be greater than or equal to 512
	    * 		2.	Support return value parsing status, as shown in the table above.
	    * 
	    *  @return
	    *  Success: Status code ≥ 0, The status can be parsed bit by bit, and the meaning is consistent 
	    * 			 with the status return value
	    *  Failure: error code
	    * 
	    */
        [DllImport(POSSDK_Pro_Dll_Path)]
        extern private static int QueryStatus(int devHandle, StringBuilder status);
        public static int QueryStatusWrapper(int devHandle, out string status)
        {
            const int MAXSTATUSLEN = 128;
            StringBuilder StrBuildStatus = new StringBuilder(MAXSTATUSLEN);
            int nStatus = QueryStatus(devHandle, StrBuildStatus);
            status = StrBuildStatus.ToString();
            return nStatus;
        }

        /*
	    * 
	    *  @~Chinese
	    *  @brief 条码打印
	    * 
	    *  @param [in]   devHandle		设备句柄，即OpenPrinter的有效返回值，当只有一台设备时可以传0。
	    *  @param [in]   type			条码类型,如下：
	    * 								有效值	条码类型
	    * 								1		UPC - A
	    * 								2		UPC - E
	    * 								3		JAN13(EAN13)
	    * 								4		JAN8(EAN8)
	    * 								5		CODE39
	    * 								6		ITF
	    * 								7		CODEBAR
	    * 								8		CODE93
	    * 								9		CODE128
	    * 								10		QRCODE
	    * 								11		PDF417
	    * 								12		MAXICODE
	    * 								13		GS1
	    * 
	    *  @param [in]   data		条码数据
	    *  @param [in]   dataLen	条码数据长度
	    *  @param [in]    format：条码格式，如“BasicWidth=1|Height=50|HriPos=0”,大小写不限，以符号‘|’分隔。
	    * 					注：当format传入NULL或者空字符串，按照默认值执行。
	    * 	format具体各属性如下：
	    *  {
	    * 		BasicWidth: 条码基本元素宽度，
	    * 					一维码：有效范围1 - 6，默认是2
	    * 					二维码：有效范围QR 1 - 10，默认5；PDF417 1 - 7, 默认2
	    *      Height: 一维码：条码高度（单位：点），有效范围1-255，默认是50,GS1高度，有效范围2 - 250，默认是50		
	    * 				二维码：条码元素高度（单位：点），有效范围2 - 25，默认10
	    *      HriPos：条码字符位置，仅一维条码有效，默认是2
	    * 					有效值		位置
	    * 					0			不打印
	    * 					1			只在条码上方打印
	    * 					2			只在条码下方打印
	    * 					3			条码上、下方都打印
	    * 		HriFont：条码字符字体类型，仅一维条码有效，默认是0
	    * 					有效值		位置
	    * 					0			标准ASCII
	    * 					1			压缩ASCII
	    * 		X：水平方向离左边界的距离（单位：点），默认 - 1
	    * 					有效值		对齐方式
	    * 					- 1			居左
	    * 					- 2			居右
	    * 					- 3			居中
	    * 					≥0			离左边界距离（点）
	    *  以下几项仅对QR码起作用：
	    * 		LanguageMode : 语言模式 0：汉字 1：日文，默认为0
	    * 		ErrorCorrect：纠错等级, 有效范围QRCode : 0 - 3 : 0代表L级别, 1代表M级别, 2代表Q级别 3代表H级别。默认为L级别
	    * 	以下几项仅PDF417起作用：
	    * 		Rows：条码的行数，有效范围3 - 90，默认是5
	    * 		Cols : 条码的列数，有效范围1 - 30，默认是5
	    * 		ScaleH：条码外观比高度，有效范围1 - 10，默认是2
	    * 		ScaleV：条码外观比宽度，有效范围1 - 100，默认是10
	    * 		ErrorCorrect：纠错等级, 等级越高条码容量越大, 有效范围PDF417 : 0 - 8，默认为3
	    * 	以下几项仅GS1起作用：
	    * 		GS1条码是独立条码或者复合条码，通过编码数据中是否存在数据分隔符“ | ”区分，存在“ | ”为复合码，否则是独立的
	    * 	DataBar条码。“ | ”之前的是复合码中的DataBar条码数据，“ | ”之后的是复合码中2D条码的数据。
	    * 		GS1Type:  GS1条码类型及字符集，有效范围1-7，默认为1,详见帮助文档
	    * 		BasicHeight: 复合码中2D条码符号基本元素高度点数，有效范围1-10点，默认是3
	    * 		SegmentNum：每行条码符号段数，只有条码类型是扩展层排型时需要设置此参数。有效范围4 - 20，默认是6
	    * 		SepHeight：分隔符高度点数，条码类型是DataBar复合码或者独立的DataBar层排型、全向层排型、扩展层排型需要设置此参数。有效范围1 - 10点，默认是1
	    *  	HriType：注释字符内容，有效范围1 - 4，默认是1,详见帮助文档
	    * 		AI：是否应用AI（应用标识符）：0表示不应用AI；1表示应用AI，默认是0
	    *  }
	    *  @return
	    *  成功：0
	    *  失败：错误码
	    * 
	    *  @~English
	    *  @brief Barcode Printing
	    * 
	    *  @param [in]   devHandle		Device handle, which is the effective return value of openprinter, 
	    * 								can be passed to 0 when there is only one device.
	    *  @param [in]   type			Barcode Type,as below:
	    * 							Valid Value	Barcode Type
	    * 								1		UPC - A
	    * 								2		UPC - E
	    * 								3		JAN13(EAN13)
	    * 								4		JAN8(EAN8)
	    * 								5		CODE39
	    * 								6		ITF
	    * 								7		CODEBAR
	    * 								8		CODE93
	    * 								9		CODE128
	    * 								10		QRCODE
	    * 								11		PDF417
	    * 								12		MAXICODE
	    * 								13		GS1
	    * 
	    *  @param [in]   data		Barcode data
	    *  @param [in]   dataLen	Barcode data length
	    *  @param [in]   format    Bar code format, Such as "BasicWidth=1|Height=50|HriPos=0", the case 
	    //is not limited, separated by the symbol ‘|‘.
	    * 	The specific attributes of format are as follows:
	    *  {
	    * 		BasicWidth: The width of the basic element of the barcode,
	    * 					One dimensional code: valid range 1-6, default is 2
	    * 					QR Code: effective range QR 1-10, default 5; PDF417 1-7, default 2
	    *      Height: One dimensional code: bar code height (unit: point), valid range 1-255, default is 50
	    * 				GS1 height, valid range 2-250, default is 50
	    * 				QR Code: bar code element height (unit: point), valid range 2-25, default is 10
	    *      HriPos：Barcode character position, only one-dimensional barcode is valid, the default is 2
	    * 					Value		Position
	    * 					0			No printing
	    * 					1			Print only above the barcode
	    * 					2			Print only below the barcode
	    * 					3			Print both above and below the barcode
	    * 		HriFont：Barcode character font type, only one-dimensional barcode is valid, the default is 0
	    * 					Value		Position
	    * 					0			Standard ASCII
	    * 					1			Compression ASCII
	    * 		X：The horizontal distance from the left boundary (unit: point), default -1
	    * 					Value		Alignment
	    * 					- 1			Left-aligning 
	    * 					- 2			Right-aligning
	    * 					- 3			Center-aligning 
	    * 					≥0			Distance from left boundary (point)
	    *  The following items only work on QR Code:
	    * 		LanguageMode:Language mode 0: Chinese character 1: Japanese, default is 0
	    * 		ErrorCorrect:Error correction level, effective range, QRCode:0-3 : 0 for level L, 
	    * 					 1 for level M, 2 for level Q, and 3 for level H. The default is L level
	    *  The following only work on PDF417:
	    * 		Rows: The number of lines of barcode, valid range 3-90, default is 5
	    * 		Cols: The number of bar code columns, the effective range is 1-30, the default is 5
	    * 		ScaleH: Bar code appearance ratio height, valid range 1 - 10, default is 2
	    * 		ScaleV: Bar code appearance ratio width, valid range 1-100, default is 10
	    * 		ErrorCorrect: Error correction level: the higher the level is, the larger the barcode capacity is. 
	    * 					  The effective range is PDF417:0-8, which is 3 by default
	    * 	The following only work on GS1:
	    * 		GS1Type: GS1 barcode type and character, valid range 1-7, default to 1
	    * 		BasicHeight: The height points of basic elements of 2D barcode symbol in compound code, 
	    * 					 the effective range is 1-10 points, the default is 3
	    * 		SegmentNum: The number of segments of bar code symbol in each line. This parameter needs to be set 
	    * 					only when the bar code type is extended layer type. The valid range is 4-20, and the default is 6
	    * 		SepHeight: Separator height points, bar code type is DataBar composite code or independent DataBar 
	    * 				   layer type, omni-directional layer type, extended layer type, you need to set this parameter. 
	    * 				   The valid range is 1-10 points. The default value is 1
	    *  	HriType: Comment character content, valid range 1-4, default is 1
	    * 		AI:Whether to apply AI (application identifier): 0 means not to apply AI; 1 means to apply AI, the default is 0
	    *  }
	    *  @return
	    *  Success: 0
	    *  Failure: error code
	    * 
	    */
        [DllImport(POSSDK_Pro_Dll_Path)]
        extern private static int BarCodePrint(int devHandle, int type, string data, int dataLen, string format);
        public static int BarCodePrintWrapper(int devHandle, int type, string data, int dataLen, string format)
        {
            int nRet = BarCodePrint(devHandle, type, data, dataLen, format);
            return nRet;
        }

        /*
	    * 
	    *  @~Chinese
	    *  @brief 图片打印
	    * 
	    *  @param [in]   devHandle		设备句柄，即OpenPrinter的有效返回值，当只有一台设备时可以传0。
	    *  @param [in]   Image			图片文件路径。
	    *  @param [in]   format		图片处理格式，如“x=0|y=0|Width=0|Height=0|Scale=50|Zoom=1|Dither=0|Threshold=-1”,大小写不限，以符号‘|’分隔。
	    * 					注：当format传入NULL或者空字符串，按照默认值执行。
	    * 	format具体各属性如下：
	    *  {
	    * 		x: 位图打印横坐标，默认0，有效值范围为-3-255，单位为点。-1：居左对齐；-2：居中对齐；-3：居右对齐。
	    * 		y: 位图打印纵坐标，仅页模式下生效，默认0，有效值范围为0-255，单位为点。
	    *         Width，Height：图像放缩到的长宽像素数，无上限，下限为0，默认为0；
	    *                                    取值0时按图像原长宽进行打印；
	    *                                    两个值都不设置时，按默认值0进行处理；
	    *                                    仅设置一个时，按原图与设置值的比例进行放缩。
	    *         Scale： 放缩比例，单位为%，无上限，下限为0，默认为100；
	    *                    取值0时，按100处理，进行原图打印；
	    *                    优先级低于width，height参数，当width、height设置时，此参数不生效；
	    * 	    Zoom：放缩算法参数，默认值3；
	    *                     取值1：立方插值法；
	    *                     取值2：双线性插值法；
	    *                  	取值3：临近插值法。
	    *         Dither：二值抖动算法方式，默认0。
	    *                       0：固定阈值法；
	    *                       1：BayerM3
	    *                       2：BayerM4
	    *                       3：误差扩散法
	    *          Threshold：二值抖动算法固定阈值法阈值参数，仅dither取值0时生效，默认-1，参数范围-1-255。设置-1时使用自动阈值
	    * 
	    *  }
	    *  @return
	    *  成功：0
	    *  失败：错误码
	    * 
	    *  @~English
	    *  @brief Print Image file
	    * 
	    *  @param [in]   devHandle		Device handle, which is the effective return value of openprinter, 
	    * 								can be passed to 0 when there is only one device.
	    *  @param [in]   Image			Image file path, can be relative or absolute path.
	    * 
	    *  @param [in]   format		This parameter is the format string of the image processing  mode, 
	    * 								such as "x=0|y=0|Width=0|Height=0|Scale=50|Zoom=1|Dither=0|Threshold=-1".
	    * 								It can also be an empty string. When it is an empty string, the default value is used.
	    * 								
	    *  The meaning of each parameter, the value range and the default value are as follows.
	    *  {
	    * 		x:	The horizontal offset, the value range is -3 to 255, and the unit is point. The default value is 0.
	    * 			Among them, -1 means left-aligned, -2 means center-aligned, and -3   means right-aligned. 
	    * 			This alignment is only valid in row mode.
	    * 		y:	Vertical offset, the value range is 0 to 255, and the unit is point.  The default value is 0. 
	    * 			This parameter is only valid in page mode.
	    *      Width，Height:	The final print length and width of the image,the      minimum value is 0, no upper limit, 
	    * 						and the unit is pixel. The default value is 0. 
	    *                      When the value is 0, it is processed according to the actual length and width of the picture. 
	    *                      When only one of the two parameters is set, the scaling process is performed according to the
	    * 						ratio of the set value to the actual value of  the picture.
	    *      Scale:	Image zoom ratio, the minimum value is 0, no upper limit, the unit is %. The default value is 0. 
	    *              When the value is 0, it is processed as 100 and the original image is  printed. 
	    *              The priority is lower than Width and Height. When a value in Width and Height is set, 
	    * 				this setting will not take effect.
	    * 	    Zoom:	Image zooming mode, the value ranges from 1 to 3, and the    default value is 3. 
	    *                    Value 1: Cubic interpolation method;
	    *                    Value 2: Bilinear interpolation method; 
	    *                    Value 3: Proximity interpolation method.
	    *         Dither:	Binary dithering algorithm, the value ranges from 0 to 3, and the default value is 0. 
	    *                       Value 0: fixed threshold method; 
	    *                       Value 1: BayerM3; 
	    *                       Value 2: BayerM4; 
	    *                       Value 3: Error diffusion method.
	    *          Threshold:	The threshold parameter of the fixed threshold method, the  value range is -1 to 255, and the default value is -1. 
	    * 						It takes effect only when Dither is set to 0. 
	    * 						The value - 1 is to use the automatic threshold.。
	    *  }
	    *  @return
	    *  Success: 0
	    *  Failure: error code
	    * 
	    */
        [DllImport(POSSDK_Pro_Dll_Path)]
        extern private static int PrintImageFile(int devHandle, string Image, string format);
        public static int PrintImageFileWrapper(int devHandle, string Image, string format)
        {
            int nRet = PrintImageFile(devHandle, Image, format);
            return nRet;
        }

        /*
	    * 
	    *  @~Chinese
	    *  @brief 已下载图片打印
	    * 
	    *  @param [in]   devHandle	设备句柄，即OpenPrinter的有效返回值，当只有一台设备时可以传0。
	    *  @param [in]   Type		图片下载方式。
	    *                                        0-RAM位图
	    *                                        1-Flash位图
	    *  @param [in]   ImageID	图片下载序号。
	    *                                        RAM位图取值范围1-8
	    *                                        Flash位图取值范围1-255
	    *  @param [in]   format：条码格式，如“x=0|y=0”,大小写不限，以符号‘|’分隔。
	    * 					注：当format传入NULL或者空字符串，按照默认值执行。
	    * 		x: 位图打印横坐标，默认0，有效值范围为-3-255，单位为点。-1：居左对齐；-2：居中对齐；-3：居右对齐。
	    * 		y: 位图打印纵坐标，仅页模式下生效，默认0，有效值范围为0-255，单位为点。
	    *  }
	    *  @return
	    *  成功：0
	    *  失败：错误码
	    * 
	    *  @~English
	    *  @brief Print the downloaded image
	    * 
	    *  @param [in]   devHandle		Device handle, which is the effective return value of openprinter, 
	    * 								can be passed to 0 when there is only one device.
	    * 
	    *  @param [in]   Type		Image download method. 0-RAM,1-Flash
	    * 
	    *  @param [in]   ImageID	The serial number of the picture when downloading, 
	    * 							the value range of RAM is 1-8, and the value range of Flash is 1-255.
	    * 
	    *  @param [in]   format	The image has been processed when downloading. Only the print coordinates 
	    * 							are set here, such as "x=-1|y=0". It can also be an empty string, 
	    * 							and the default is an empty string. Value, the meaning of each parameter, 
	    * 							value range and default value are as follows.
	    * 		x:	The horizontal offset, the value range is -3 to 255, and the unit is point. The default value is 0. 
	    * 			Among them, -1 means left - aligned, -2 means center - aligned, and -3 means  right - aligned.
	    * 			This alignment is only valid in row mode.
	    * 		y:	Vertical offset, the value range is 0 to 255, and the unit is point. The default value is 0. 
	    * 			This parameter is only valid in page mode.
	    *  }
	    *  @return
	    *  Success: 0
	    *  Failure: error code
	    * 
	    */
        [DllImport(POSSDK_Pro_Dll_Path)]
        extern private static int PrintDownloadedImage(int devHandle, int Type, int ImageID, string format);
        public static int PrintDownloadedImageWrapper(int devHandle, int Type, int ImageID, string format)
        {
            int nRet = PrintDownloadedImage(devHandle, Type, ImageID, format);
            return nRet;
        }

        /*
         * 
         *  @~Chinese
         *  @brief 下载图片
         * 
         *  @param [in]   devHandle		设备句柄，即OpenPrinter的有效返回值，当只有一台设备时可以传0。
         *  @param [in]   Type		图片下载方式。
         *                                        0-RAM位图
         *                                        1-Flash位图
         *  @param [in]   ImageList 	下载图片路径列表。仅支持路径，且至少需要有一个路径值
         *                                        以“|”分隔，例如：“./aaa.jpg|C:\\S.PNG”。
         *                                        Flash位图取值范围1-255
         *   @param [in]	format，具体参数参见PrintImageFile接口的format参数描述，此接口中无x, y参数，且会对ImageList中所有图片都进行统一处理。
         * 
         *  }
         *  @return
         *  成功：0
         *  失败：错误码
         * 
         *  @~English
         *  @brief Download images to printer
         * 
         *  @param [in]   devHandle		Device handle, which is the effective return value of openprinter, 
         * 								can be passed to 0 when there is only one device.
         *  @param [in]   Type			Image download method. 0-RAM,1-Flash
         * 
         *  @param [in]   ImageList 	The list of download image file paths can be relative or absolute paths. 
         * 								At least one path value is required, and the paths are directly separated by "|". 
         * 								For example: "./image/RAM1.bmp|./image/RAM2.bmp|./image/RAM3.bmp".
         * 
         *   @param [in]  format		The specific parameters are described in the format parameter description of
         * 								the printimagefile interface, which is no x, y, and all the images in the 
         * 								imagelist are processed uniformly.
         *  }
         *  @return
         *  Success: 0
         *  Failure: error code
         * 
         */
        [DllImport(POSSDK_Pro_Dll_Path)]
        extern private static int DownloadImage(int devHandle, int Type, string ImageList, string format);
        public static int DownloadImageWrapper(int devHandle, int Type, string ImageList, string format)
        {
            int nRet = DownloadImage(devHandle, Type, ImageList, format);
            return nRet;
        }

        [DllImport(POSSDK_Pro_Dll_Path)]
        extern private static int PrintContent(int devHandle, int Type, string content);
        public static int PrintContentWrapper(int devHandle, int Type, string content)
        {
            int nRet = PrintContent(devHandle, Type, content);
            return nRet;
        }

        //条码类型
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

        //下载图像位置
        public const int IMAGE_TYPE_RAM = 0;
        public const int IMAGE_TYPE_FLASH = 1;

        //返回值
        public const int SUCCESS = 0;
    }
}
