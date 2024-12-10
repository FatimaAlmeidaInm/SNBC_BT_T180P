using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSSDK_Pro_Demo
{

    public partial class POSSDK_Pro_Demo : Form
    {
        // demo依赖配置文件路径
        public const string POSSDK_Pro_DemoIni_Path = LoadPOSSDKPro.POSSDK_Pro_Lib_Path + "POSSDK_Pro_Demo_Config.ini";

        // POSSDKPro设备handle
        private int m_printerHandle;

        public POSSDK_Pro_Demo()
        {
            InitializeComponent();
            GetModelAndIdFromIni();
            InitUi();
        }

        // UI初始化
        public void InitUi()
        {
            // serial port
            string[] serialPortNumber = { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6",
                "COM7", "COM8", "COM9", "COM10", "COM11", "COM12" };
            foreach (string it in serialPortNumber)
            {
                comboBox2.Items.Add(it);
            }
            // 默认COM1
            comboBox2.SelectedIndex = 0;

            // baudrate
            string[] serialPortBaudrate = { "2400", "4800", "9600", "19200", "38400", "57600", "115200" };
            foreach (string it in serialPortBaudrate)
            {
                comboBox3.Items.Add(it);
            }
            // 默认115200
            comboBox3.SelectedIndex = comboBox3.Items.Count - 1;

            //控件默认状态
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            button1.Enabled = false;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            //默认选中USB
            radioButton2.Checked = true;
        }

        // 从INI中获取支持机型
        public void GetModelAndIdFromIni()
        {
            string IniDir = System.Environment.CurrentDirectory + "\\" + POSSDK_Pro_DemoIni_Path;
            // 获取支持的打印机型
            string printerList = "";
            MyGetPrivateProfileString("Model", "List", "", out printerList, IniDir);
            List<string> lPrinterList = printerList.Split(',').ToList();

            foreach (string it in lPrinterList)
            {
                comboBox1.Items.Add(it);
            }
            comboBox1.SelectedIndex = 0;
        }

        // USB枚举
        private void button1_Click(object sender, EventArgs e)
        {
            //设备下拉列表清除
            comboBox4.Items.Clear();

            const int PORT_USB = 1;
            string enumList = "";
            int nRet = SampleApi.Sample_EnumDevice(PORT_USB, out enumList);
            if(nRet < 0)
            {
                return;
            }

            //枚举结果以@进行拆分
            string[] deviceList = enumList.Split('@');
            foreach (string it in deviceList)
            {
                const int usblen = 3;
                // 排除空及不以USB开头的异常情况
                if (string.IsNullOrEmpty(it) || !it.Substring(0, usblen).Equals("USB", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                comboBox4.Items.Add(it);
            }
            if(comboBox4.Items.Count != 0)
            {
                comboBox4.SelectedIndex = 0;
            }
        }

        //网口枚举
        private void button2_Click(object sender, EventArgs e)
        {
            //清除
            comboBox5.Items.Clear();

            const int PORT_Net = 2;
            string enumList = "";
            int nRet = SampleApi.Sample_EnumDevice(PORT_Net, out enumList);
            if (nRet < 0)
            {
                return;
            }

            //枚举结果以@进行拆分
            string[] deviceList = enumList.Split('@');
            foreach (string it in deviceList)
            {
                //排除为空的异常情况
                if (string.IsNullOrEmpty(it))
                {
                    continue;
                }
                comboBox5.Items.Add(it);
            }
            if (comboBox5.Items.Count != 0)
            {
                comboBox5.SelectedIndex = 0;
            }
        }

        //连接
        private void button3_Click(object sender, EventArgs e)
        {
            int nRet = 0;
            if (button3.Text.Equals("Connect Printer", StringComparison.OrdinalIgnoreCase))
            {
                // 获取机型名
                string strModelName = comboBox1.Text;
                // 组装连接信息
                string strPortInfo = "";
                if(radioButton1.Checked)
                {
                    // COM
                    strPortInfo = comboBox2.Text + "|" + comboBox3.Text;
                }
                else if(radioButton2.Checked)
                {
                    strPortInfo = comboBox4.Text;
                    if(string.IsNullOrEmpty(strPortInfo))
                    {
                        strPortInfo = "USB";
                    }
                }
                else if(radioButton3.Checked)
                {
                    strPortInfo = comboBox5.Text;
                }
                else
                {
                    strPortInfo = "USB";
                }

                nRet = SampleApi.Sample_Open(strModelName, strPortInfo);
                if (nRet < 0)
                {
                    MessageBox.Show("Connect error. [" + nRet + "]");
                    return;
                }
                // 打开成功保存handle
                m_printerHandle = nRet;
                button3.Text = "Disconnect Printer";
            }
            else
            {
                nRet = SampleApi.Sample_Close(m_printerHandle);
                if (nRet < 0)
                {
                    MessageBox.Show("Disconnect error. [" + nRet + "]");
                    return;
                }
                m_printerHandle = -1;
                button3.Text = "Connect Printer";
            }
        }

        //打印收据小票样张
        private void button4_Click(object sender, EventArgs e)
        {
            string errorStatus = "";
            int nRet = 0;
            //打印中文样张
            nRet = SampleRestaurant.Sample_Restaurant(m_printerHandle, out errorStatus);
            if (nRet != 0)
            {
                MessageBox.Show("Restaurant Print error.\nErrorCode [" + nRet + "]\n" + "Printer Status [ " + errorStatus + " ]\n");
                return;
            }
            //打印英文样张
            nRet = SampleRestaurant.Sample_Restaurant_En(m_printerHandle, out errorStatus);
            if (nRet != 0)
            {
                MessageBox.Show("Restaurant_En error.\nErrorCode [" + nRet + "]\n" + "Printer Status [ " + errorStatus + " ]\n");
            }

            nRet = SampleXML.Sample_XML(m_printerHandle);
            if (nRet != 0)
            {
                MessageBox.Show("Restaurant Print error.\nErrorCode [" + nRet + "]\n");
                return;
            }
            //打印英文样张
            nRet = SampleXML.Sample_XML_PageMode(m_printerHandle);
            if (nRet != 0)
            {
                MessageBox.Show("Restaurant_En error.\nErrorCode [" + nRet + "]\n");
            }
        }

        //打印条码样张
        private void button5_Click(object sender, EventArgs e)
        {
            int nRet = SampleBarcode.Sample_PrintBarCode(m_printerHandle);
            if (nRet != 0)
            {
                MessageBox.Show("PrintBarCode error.\nErrorCode [" + nRet + "]\n");
            }
        }

        //打印图片样张
        private void button6_Click(object sender, EventArgs e)
        {
            int nRet = SampleImage.Sample_PrintImage(m_printerHandle);
            if (nRet != 0)
            {
                MessageBox.Show("PrintImage error.\nErrorCode [" + nRet + "]\n");
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //跟随单选状态变化
            comboBox2.Enabled = radioButton1.Checked;
            comboBox3.Enabled = radioButton1.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //跟随单选状态变化
            button1.Enabled = radioButton2.Checked;
            comboBox4.Enabled = radioButton2.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            //跟随单选状态变化
            comboBox5.Enabled = radioButton3.Checked;
            button2.Enabled = radioButton3.Checked;
        }

        private void button3_TextChanged(object sender, EventArgs e)
        {
            button4.Enabled = !button4.Enabled;
            button5.Enabled = !button5.Enabled;
            button6.Enabled = !button6.Enabled;
        }

        /// 为INI文件中指定的节点取得字符串
        /// </summary>
        /// <param name="lpAppName">欲在其中查找关键字的节点名称</param>
        /// <param name="lpKeyName">欲获取的项名</param>
        /// <param name="lpDefault">指定的项没有找到时返回的默认值</param>
        /// <param name="lpReturnedString">指定一个字串缓冲区，长度至少为nSize</param>
        /// <param name="nSize">指定装载到lpReturnedString缓冲区的最大字符数量</param>
        /// <param name="lpFileName">INI文件完整路径</param>
        /// <returns>复制到lpReturnedString缓冲区的字节数量，其中不包括那些NULL中止字符</returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            int nSize,
            string lpFileName);
        public static int MyGetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, out string lpReturnedString, string lpFileName)
        {
            const int INI_MAX_SIZE = 1024;
            StringBuilder temp = new StringBuilder(INI_MAX_SIZE);
            int ret = GetPrivateProfileString(lpAppName, lpKeyName, lpDefault, temp, INI_MAX_SIZE, lpFileName);
            lpReturnedString = temp.ToString();
            return ret;
        }
    }
}
