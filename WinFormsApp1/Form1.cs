using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using Application = System.Windows.Forms.Application;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public static Form1 form1;
        public Form1()
        {
            InitializeComponent();
            form1 = this;
        }

        public bool AutoConnectSmbSetting;
        public bool AutoRun;

        private void Form1_Load(object sender, EventArgs e)//打开软件load阶段执行指令
        {
            Control.CheckForIllegalCrossThreadCalls = false;//使线程能够操作控件
            if (IsAutoRun())//检查是否是开机自动运行，最小化启动并隐藏
            {
                Form1.form1.WindowState = FormWindowState.Minimized;
                Form1.form1.ShowInTaskbar = false;
            }
            ReadDataTxt();//打开软件填写内容
        }

        public void PingCheck(object Ip)//ping网关检查
        {
            VpnName.Enabled = false;
            VpnAccount.Enabled = false;
            VpnPassword.Enabled = false;
            VpnAllLink.Enabled = false;
            VpnOnlyPrivateLink.Enabled = false;
            VpnNetGate.Enabled = false;
            VpnNet.Enabled = false;
            VpnSave.Enabled = false;
            VpnConnect.Enabled = false;
            if (Ping(Ip))
            {
                ChangeRoute(Ip.ToString());
                VpnConnect.Enabled = true;
                VpnAllLink.Enabled = true;
                VpnOnlyPrivateLink.Enabled = true;
            }
            else
            {
                VpnName.Enabled = true;
                VpnAccount.Enabled = true;
                VpnPassword.Enabled = true;
                VpnNet.Enabled = true;
                VpnSave.Enabled = true;
                VpnNetGate.Enabled = true;
                VpnConnect.Enabled = true;
                VpnAllLink.Enabled = true;
                VpnOnlyPrivateLink.Enabled = true;
                if (SystemVpnAutoConnect.Checked == true)
                {
                    if (VpnName.Text == "" || VpnAccount.Text == "" || VpnPassword.Text == "")
                    {
                        MessageBox.Show("Vpn名称、账户、密码未填写。");
                        return;
                    }

                    if (!VpnTextInputCheck())//检查vpn页面填写，填写不正确就退出
                    {
                        return;
                    }

                    VpnSave.PerformClick();//点击保存按钮
                    VpnName.Enabled = false;
                    VpnAccount.Enabled = false;
                    VpnPassword.Enabled = false;
                    VpnAllLink.Enabled = false;
                    VpnOnlyPrivateLink.Enabled = false;
                    VpnNetGate.Enabled = false;
                    VpnNet.Enabled = false;
                    VpnSave.Enabled = false;
                    VpnConnect.Enabled = false;
                    VpnConnect.Text = "处理中……";
                    AutoConnectSmbSetting = false;
                    Dis_ConnectVpn.Text = "处理中……";
                    Dis_ConnectVpn.Enabled = false;

                    Thread NewPing = new Thread(new ParameterizedThreadStart(PingThenConnectVpn));
                    NewPing.IsBackground = true;
                    NewPing.Start(VpnNetGate.Text);//多参数可又组成数组传递进去。
                }
            }
        }

        private void Form1_Shown_1(object sender, EventArgs e)//检查三个文件是否存在
        {
            //MessageBox.Show("检测配置文件是否存在");
            //if (!File.Exists("C:\\Program Files\\SMB HelpER\\VpnData.txt"))
            //{
                //tabControl1.SelectedTab = Vpn;
                //VpnSave.PerformClick();
            //}
            tabControl1.SelectedTab = Smb;
            //if (!File.Exists("C:\\Program Files\\SMB HelpER\\SmbData.txt"))
            //{
                //SmbSave.PerformClick();
            //}
            tabControl1.SelectedTab = Software;
            //if (!File.Exists("C:\\Program Files\\SMB HelpER\\SystemData.txt"))
            //{
                //tabControl1.SelectedTab = Software;
                //SystemSave.PerformClick();
            //}
            tabControl1.SelectedTab = Vpn;
            if (!File.Exists("C:\\Program Files\\SMB HelpER\\NonAdministratorExecuteCmd.exe"))//展开自动执行cmd的exe
            {
                if (!System.IO.Directory.Exists("C:\\Program Files\\SMB HelpER"))
                {
                    System.IO.Directory.CreateDirectory("C:\\Program Files\\SMB HelpER");
                }
                System.Reflection.Assembly assembly = GetType().Assembly;
                Stream streamSmall = assembly.GetManifestResourceStream("SMB HelpER.Resources.NonAdministratorExecuteCmd.exe");
                //注：ImageAreaSelector.Resources.template.xlsx是资源路径
                // 如果资源路径写错了会返回空值，使用时就会报“未将对象引用设置到对象的实例”的异常
                int length = (int)streamSmall.Length;
                byte[] bs = new byte[length];
                streamSmall.Read(bs, 0, length);
                File.WriteAllBytes("C:\\Program Files\\SMB HelpER\\NonAdministratorExecuteCmd.exe", bs);
                streamSmall.Close();
            }
            //if (!File.Exists("C:\\Program Files\\SMB HelpER\\DisconnectAllSmb.bat"))
            //{
                //string DisconnectAllSmbBat = "@echo off\r\nstart cmd /k \"net use * /delete /y&&exit\"";
                //CreateTxtFile(DisconnectAllSmbBat, "DisconnectAllSmb.bat");
            //}

            Thread PingCheckThred = new Thread(new ParameterizedThreadStart(PingCheck));//新县城处理cmd命令防止主程序卡死
            PingCheckThred.IsBackground = true;
            PingCheckThred.Start(VpnNetGate.Text);//多参数可又组成数组传递进去。

            AutoConnectSmbSettingCheck();//检查smb自动连接的checkbox

            Thread AutoSmb = new Thread(AutoConnectSmb);//重复执行，检查bool，判断是否连接smb
            AutoSmb.IsBackground = true;
            AutoSmb.Start();
            if (AutoRun)//是开机自动运行的话就最小化启动并隐藏
            {
                //MessageBox.Show("");
                //
                Form1.form1.Hide();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)//选项卡切换也刷新内容
        {
            ReadDataTxt();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)//阻止关闭窗口，托盘运行
        {
            //取消"关闭窗口"事件
            e.Cancel = true; // 取消关闭窗体 
            this.Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)//双击显示和隐藏
        {
            if (this.Visible)
            {
                this.Hide();
            }
            else
            {
                this.Visible = true;
                this.Activate();
                Form1.form1.Opacity = 100;
                Form1.form1.ShowInTaskbar = true;
                //Form1.form1.WindowState=FormWindowState.Maximized;
            }
        }

        private void Show_Click(object sender, EventArgs e)//点击托盘菜单显示页面
        {
            this.Visible = true;
            this.Activate();
            Form1.form1.Opacity = 100;
            Form1.form1.ShowInTaskbar = true;
            //Form1.form1.WindowState = FormWindowState.Maximized;
        }

        private void Close_Click(object sender, EventArgs e)//点击托盘菜单退出
        {
            this.Hide();
            notifyIcon1.Text = "退出中 Closing";
            contextMenuStrip1.Enabled = false;
            Form1.form1.ShowInTaskbar = false;
            Form1.form1.WindowState = FormWindowState.Minimized;
            Form1.form1.Opacity = 0;
            this.Visible = true;
            this.Activate();
            
            //this.Show();
            
            AutoConnectSmbSetting = false;
            Thread IfDisconnectT = new Thread(IfDisconnect);//关闭时判断是否断开连接
            IfDisconnectT.IsBackground = true;
            IfDisconnectT.Start();
        }

        public void CloseSoftware()
        {
            while (true)
            {
                if (VpnConnect.Enabled&& SmbConnect.Enabled)
                {
                    this.notifyIcon1.Visible = false;                    
                    System.Environment.Exit(System.Environment.ExitCode);
                }
                Thread.Sleep(500);
            }
        }

        //////////////////////////////////////////////////////////////////////上：窗体内容；下：public引用

        public static void CreateTxtFile(string Txt, string FileName)//向txt中写入文件
        {
                FileStream fs1 = new FileStream("C:\\Program Files\\SMB HelpER\\" + FileName, FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1, Encoding.UTF8);
                sw.WriteLine(Txt);//开始写入值
                sw.Close();
                fs1.Close();
        }

        private static void ReadDataTxt()//填写每个页面的内容
        {
            //////////填写vpn页面
            if (File.Exists("C:\\Program Files\\SMB HelpER\\VpnData.txt"))//判断文件存在
            {
                StreamReader VpnData = new StreamReader("C:\\Program Files\\SMB HelpER\\VpnData.txt", Encoding.UTF8);//读取文件
                int line = 0;
                string VpnNameData = "";
                string VpnAccountData = "";
                string VpnPasswordData = "";
                string VpnLinkTypeData = "";
                string VpnNetGateData = "";
                string VpnNetData = "";
                while (!VpnData.EndOfStream)//循环
                {
                    string config = VpnData.ReadLine();
                    if (line == 0)//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        VpnNameData = config;//把config的内容传递给setting
                                             //break;
                    }
                    else if (line == 1)//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        VpnAccountData = config;//把config的内容传递给setting
                                                //break;
                    }
                    else if (line == 2)//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        VpnPasswordData = config;//把config的内容传递给setting
                                                 //break;
                    }
                    else if (line == 3)//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        VpnLinkTypeData = config;//把config的内容传递给setting
                                                 //break;
                    }
                    else if (line == 4)//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        VpnNetGateData = config;//把config的内容传递给setting
                                                //break;
                    }
                    else if (config == "q615749669")//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        //VpnNet = config;//把config的内容传递给setting
                        break;
                    }
                    else//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        if (config != "")
                        {
                            VpnNetData += config + "\r\n";//把config的内容传递给setting
                        }
                    }
                    line++;//读下一行
                }
                //MessageBox.Show(VpnNameData);//调试
                form1.VpnName.Text = VpnNameData;
                form1.VpnAccount.Text = VpnAccountData;
                form1.VpnPassword.Text = VpnPasswordData;
                if (VpnLinkTypeData == "OnlyPrivateLink")
                {
                    form1.VpnOnlyPrivateLink.Checked = true;
                }
                else
                {
                    form1.VpnAllLink.Checked = true;
                }
                form1.VpnNetGate.Text = VpnNetGateData;
                if (VpnNetData.Length >= 2)
                {
                    form1.VpnNet.Text = VpnNetData.Substring(0, VpnNetData.Length - 2);//去掉最后的换行符
                }
                VpnData.Close();//关闭进程
            }

            //////////填写smb页面
            if (File.Exists("C:\\Program Files\\SMB HelpER\\SmbData.txt"))//判断文件是否存在
            {
                StreamReader SmbData = new StreamReader("C:\\Program Files\\SMB HelpER\\SmbData.txt", Encoding.UTF8);//读取文件
                int line = 0;
                string SmbUrlData = "";
                while (!SmbData.EndOfStream)//循环
                {
                    string config = SmbData.ReadLine();
                    if (config == "q615749669")//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        //VpnNet = config;//把config的内容传递给setting
                        break;
                    }
                    else//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        if (config != "")
                        {
                            SmbUrlData += config + "\r\n";//把config的内容传递给setting
                        }
                    }
                    line++;//读下一行
                }
                //MessageBox.Show(VpnNameData);//调试
                if (SmbUrlData.Length >= 2)
                {
                    form1.SmbUrl.Text = SmbUrlData.Substring(0, SmbUrlData.Length - 2);//去掉最后的换行符
                }
                SmbData.Close();//关闭进程       
            }
            //////////系统设置页面
            if (File.Exists("C:\\Program Files\\SMB HelpER\\SystemData.txt"))//判断文件是否存在
            {
                StreamReader SystemData = new StreamReader("C:\\Program Files\\SMB HelpER\\SystemData.txt", Encoding.UTF8);//读取文件
                int line = 0;
                string VpnConnect = "";
                string VpnDisconnect = "";
                string SmbConnect = "";
                string SmbDisconnect = "";
                string StartWithWindows = "";
                while (!SystemData.EndOfStream)//循环
                {
                    string config = SystemData.ReadLine();
                    if (line == 0)//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        VpnConnect = config;//把config的内容传递给setting
                                            //break;
                    }
                    else if (line == 1)//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        VpnDisconnect = config;//把config的内容传递给setting
                                               //break;
                    }
                    else if (line == 2)//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        SmbConnect = config;//把config的内容传递给setting
                                            //break;
                    }
                    else if (line == 3)//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        SmbDisconnect = config;//把config的内容传递给setting
                                               //break;
                    }
                    else if (line == 4)//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        StartWithWindows = config;//把config的内容传递给setting
                                                  //break;
                    }
                    else if (config == "q615749669")//读到的是这个数字+1
                    {
                        //MessageBox.Show(config);
                        //VpnNet = config;//把config的内容传递给setting
                        break;
                    }
                    line++;//读下一行
                }
                //MessageBox.Show(VpnNameData);//调试
                if (VpnConnect == "Auto")//1
                {
                    form1.SystemVpnAutoConnect.Checked = true;
                }
                else
                {
                    form1.SystemVpnAutoConnect.Checked = false;
                }
                if (VpnDisconnect == "Auto")//2
                {
                    form1.SystemVpnAutoDisconnect.Checked = true;
                }
                else
                {
                    form1.SystemVpnAutoDisconnect.Checked = false;
                }
                if (SmbConnect == "Auto")//3
                {
                    form1.SystemSmbAutoConnect.Checked = true;
                }
                else
                {
                    form1.SystemSmbAutoConnect.Checked = false;
                }
                if (SmbDisconnect == "Auto")//4
                {
                    form1.SystemSmbAutoDisconnect.Checked = true;
                }
                else
                {
                    form1.SystemSmbAutoDisconnect.Checked = false;
                }
                if (StartWithWindows == "True")//5
                {
                    form1.SystemStartWithWindows.Checked = true;
                }
                else
                {
                    form1.SystemStartWithWindows.Checked = false;
                }
                SystemData.Close();//关闭进程       
            }
        }

        //////////输入cmd命令

        public static string Cmd(object Command)
        {
            //Console.WriteLine("请输入要执行的命令:");
            string strInput = Command.ToString();
            Process p = new Process();

            //设置要启动的应用程序
            p.StartInfo.FileName = "cmd.exe";
            //是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;
            //输出信息
            p.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            p.StartInfo.CreateNoWindow = true;

            //启动程序
            p.Start();

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(strInput + "&exit");

            p.StandardInput.AutoFlush = true;

            //获取输出信息
            string strOuput = p.StandardOutput.ReadToEnd();
            //等待程序执行完退出进程
            p.WaitForExit();
            p.Close();

            //MessageBox.Show(strOuput);
            //Console.WriteLine(strOuput);

            //Console.ReadKey();
            return strOuput;
        }

        public bool Ping(object Ip)//PingIP
        {
            //MessageBox.Show(Cmd("ping " + Ip.ToString() + " -n 2 -w 1000"));
            if (Cmd("ping " + Ip.ToString() + " -n 2 -w 500").Contains("TTL"))//ping目标ip，两次，1秒超时
            {
                //MessageBox.Show("Ping " + Ip.ToString() + " succeed.");
                VpnConnect.Text = "断开";
                Dis_ConnectVpn.Text = "断开 VPN";
                VpnNetGate.Enabled = false;

                return true;//ping成功
            }
            //MessageBox.Show("Ping " + Ip.ToString() + " failed.");
            VpnConnect.Text = "连接";
            Dis_ConnectVpn.Text = "连接 VPN";
            return false;//ping失败
        }

        /////////////////////////////////////////////////////////////////////////////////上：public；下：vpn页面

        private void VpnSave_Click(object sender, EventArgs e)//传递写入txt的值
        {
            if (!VpnTextInputCheck())//检查vpn页面填写，填写不正确就退出
            {
                return;
            }

            string VpnLinkType = "";
            if (VpnAllLink.Checked == true)
            {
                VpnLinkType = "AllLink";
            }
            if (VpnOnlyPrivateLink.Checked == true)
            {
                VpnLinkType = "OnlyPrivateLink";
            }
            string txt = VpnName.Text + "\r\n" + VpnAccount.Text + "\r\n" + VpnPassword.Text + "\r\n" + VpnLinkType + "\r\n"
                + VpnNetGate.Text + "\r\n" + VpnNet.Text + "\r\n" + "q615749669";
            //MessageBox.Show(txt);
            CreateTxtFile(txt, "VpnData.txt");
        }

        private void VpnAllLink_CheckedChanged(object sender, EventArgs e)//全局和仅局域网切换
        {
            VpnOnlyPrivateLink.Checked = !VpnAllLink.Checked;
            if (VpnAllLink.Checked == true)
            {
                VpnNet.Enabled = false;
                if (VpnConnect.Text == "断开")
                {
                    VpnAllLink.Enabled = false;
                    VpnOnlyPrivateLink.Enabled = false ;
                    string Command = "route change 0.0.0.0 mask 0.0.0.0 " + VpnNetGate.Text;

                    Thread SwitchRouteCommand = new Thread(new ParameterizedThreadStart(SwitchRoute));//新县城处理cmd命令防止主程序卡死
                    SwitchRouteCommand.IsBackground = true;
                    SwitchRouteCommand.Start(Command);//多参数可又组成数组传递进去。
                }
            }
            else
            {
                if(VpnConnect.Text == "连接")
                {
                    VpnNet.Enabled = true;
                }
                else
                {
                    VpnAllLink.Enabled = false;
                    VpnOnlyPrivateLink.Enabled = false;
                    string Command = "ipconfig /renew&route delete 0.0.0.0 mask 0.0.0.0 " + VpnNetGate.Text;

                    Thread SwitchRouteCommand = new Thread(new ParameterizedThreadStart(SwitchRoute));//新县城处理cmd命令防止主程序卡死
                    SwitchRouteCommand.IsBackground = true;
                    SwitchRouteCommand.Start(Command);//多参数可又组成数组传递进去。
                }           
            }
        }

        private void VpnOnlyPrivateLink_CheckedChanged(object sender, EventArgs e)//全局和仅局域网切换
        {
            VpnAllLink.Checked = !VpnOnlyPrivateLink.Checked;
        }

        public void SwitchRoute(object Txt1)
        {
            Cmd(Txt1.ToString());
            VpnAllLink.Enabled = true;
            VpnOnlyPrivateLink.Enabled = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("只输入网段或ip地址，将使用默认掩码255.255.255.0，默认网关为上方输入框填入的网关。\r\n例如：192.168.1.0" +
                "\r\n自定义设置请输入 网段或IP/掩码/网关\r\n例如：172.22.6.24/255.255.0.0/172.22.22.1");
        }

        /////////////////////////////////////////////////////////////////////////////////////上：vpn页面；下vpn连接页面

        public bool VpnTextInputCheck()//检查页面网关和网段输入
        {
            if(!VpnTextInputCheckIp(VpnNetGate.Text, "NetGate"))//检查网关的ip地址输入
            {
                return false;
            }
            if (!VpnTextInputCheckNet(VpnNet.Text))//检查路由textbox的输入
            {
                return false;
            }
            return true;
        }

        public bool VpnTextInputCheckNet(string Net)//检查路由textbox的输入
        {
            if (Net == null || Net == "") //空白返回正确
            { return true; }

            while (Net.Substring(Net.Length - 1, 1) == "\r"|| Net.Substring(Net.Length - 1, 1) == "\n")//如果最后是换行符就删掉
            {
                Net = Net.Remove(Net.Length-1,1);
                //MessageBox.Show(Net);
            }
            VpnNet.Text= Net;
            
            string A= Net+"\r\n";//把路由textbox里的每行拆分去检查
            int EnterKeyNumer = 0;
            for (int i = 0; i < Net.Length+1; i++)
            {
                //MessageBox.Show(A.Substring(0, 1));
                if (A.Substring(0, 1) == "\r"|| A.Substring(0, 1) == "\n")
                {
                    string NetLine= Net.Substring(EnterKeyNumer, i-EnterKeyNumer);
                    //MessageBox.Show(NetLine);
                    EnterKeyNumer = i+1;
                    if (!VpnTextInputCheckNetLine(NetLine))
                    {
                        return false;
                    }
                }
                A=A.Remove(0, 1);
            }
            return true;
        }

        public bool VpnTextInputCheckNetLine(string Net)//检查路由textbox的输入
        {
            int LineNumber = 0;
            int Line1 = 0;
            int Line2 = 0;
            string A = Net;
            for (int i = 0; i < A.Length; i++)
            {
                if (Net.Substring(0, 1) == "/")
                {
                    LineNumber++;
                    if (Line1 == 0)
                    {
                        Line1 = i + 1;
                    }
                    else
                    {
                        Line2 = i + 1;
                    }
                }
                Net = Net.Remove(0, 1);
            }
            Net = A;
            //MessageBox.Show(LineNumber.ToString());
            
            if (LineNumber == 0)//仅填写了网段
            {
                if (VpnTextInputCheckIp(Net, "")) { return true; } else { return false; }
            }
            if (Line1 < 2 || Line2 - Line1 < 2 || Net.Length - Line2 < 2)
            {
                MessageBox.Show("流量转发网段填写错误");
                return false;
            }
            //MessageBox.Show(Net.Substring(0, Line1 - 1) + Net.Substring(Line1, Line2 - 1-Line1)+ Net.Substring(Line2, Net.Length- Line2));
            if (LineNumber == 2)//填写了默认网段、掩码和网关
            {
                if (VpnTextInputCheckIp(Net.Substring(0, Line1 - 1), "") && VpnTextInputCheckMask(Net.Substring(Line1, Line2 - 1-Line1), "")
                    && VpnTextInputCheckIp(Net.Substring(Line2, Net.Length - Line2), ""))
                { return true; }
                else { return false; }
            }
            else
            {
                MessageBox.Show("流量转发网段填写错误");
                return false;
            }
        }

        public bool VpnTextInputCheckMask(string MaskAdress, string Box)
        {
            if(MaskAdress == "255.255.255.255" || MaskAdress == "255.255.255.0" || MaskAdress == "255.255.0.0" || MaskAdress == "255.0.0.0")
            {

            }
            else
            {
                return false;
            }
            return true;
        }

        public bool VpnTextInputCheckIp(string IpAdress, string Box)//检查ip地址输入
        {
            int DotNumber = 0;
            int Dot1 = 0;
            int Dot2 = 0;
            int Dot3 = 0;
            string A = IpAdress;

            for (int i = 0; i < A.Length; i++)
            {
                if (IpAdress.Substring(0, 1) != "0"&& IpAdress.Substring(0, 1) != "1" && IpAdress.Substring(0, 1) != "2" && 
                    IpAdress.Substring(0, 1) != "3" && IpAdress.Substring(0, 1) != "4" && IpAdress.Substring(0, 1) != "5" && 
                    IpAdress.Substring(0, 1) != "6" && IpAdress.Substring(0, 1) != "7" && IpAdress.Substring(0, 1) != "8" && 
                    IpAdress.Substring(0, 1) != "9" && IpAdress.Substring(0, 1) != "." )
                {
                    if (Box == "NetGate")
                    {
                        MessageBox.Show("网关填写错误");
                    }
                    else
                    {
                        MessageBox.Show("流量转发网段填写错误");
                    }
                    return false;
                }
                if (IpAdress.Substring(0, 1) == ".")
                {
                    DotNumber++;
                    if (Dot1 == 0)
                    {
                        Dot1 = i + 1;
                    }
                    else
                    {
                        if (Dot2 == 0)
                        {
                            Dot2 = i + 1;
                        }
                        else { Dot3 = i + 1; }
                    }
                }
                IpAdress = IpAdress.Remove(0, 1);
            }
            IpAdress = A;
            //MessageBox.Show(Dot1.ToString() + Dot2.ToString() + Dot3.ToString());
            if (Dot1 < 2 || Dot1 > 4 || Dot2 - Dot1 < 2||Dot2-Dot1>4|| Dot3 - Dot2 < 2 || Dot2 - Dot1 > 4 || IpAdress.Length-Dot3>3|| DotNumber != 3)
            {
                if (Box == "NetGate")
                {
                    MessageBox.Show("网关填写错误");
                }
                else
                {
                    MessageBox.Show("流量转发网段填写错误");
                }
                return false;
            }
            int Ip1 = int.Parse(IpAdress.Substring(0, Dot1 - 1));
            int Ip2 = int.Parse(IpAdress.Substring(Dot1, Dot2 - 1 - Dot1));
            int Ip3 = int.Parse(IpAdress.Substring(Dot2, Dot3 - 1 - Dot2));
            if (IpAdress.Substring(IpAdress.Length - 1,1)== ".")
            {
                if (Box == "NetGate")
                {
                    MessageBox.Show("网关填写错误");
                }
                else
                {
                    MessageBox.Show("流量转发网段填写错误");
                }
                return false;
            }
            //MessageBox.Show(IpAdress.Substring(Dot3, IpAdress.Length - Dot3));
            int Ip4 = int.Parse(IpAdress.Substring(Dot3, IpAdress.Length - Dot3));
            if(Ip1<0|| Ip1>255|| Ip2 < 0 || Ip2 > 255 || Ip3 < 0 || Ip3 > 255 || Ip4 < 0 || Ip4 > 255)
            {
                if (Box == "NetGate")
                {
                    MessageBox.Show("网关填写错误");
                }
                else
                {
                    MessageBox.Show("流量转发网段填写错误");
                }
                return false;
            }
            return true;
        }

        private void VpnConnect_Click(object sender, EventArgs e)//点击连接 / 断开
        {
            if (VpnName.Text == "" || VpnAccount.Text == "" || VpnPassword.Text == "")
            {
                MessageBox.Show("Vpn名称、账户、密码未填写。");
                return;
            }

            if (!VpnTextInputCheck())//检查vpn页面填写，填写不正确就退出
            {
                return;
            }

            if (VpnConnect.Text == "连接")
            {
                if (VpnNet.Text == null || VpnNet.Text == "")
                {
                    if (MessageBox.Show("未填写路由设置，将只连接VPN，不更改路由，连接后默认只转发VPN网段\r\n注意：如果SMB共享等局域网资源" +
                        "与VPN不在同一网段，需要将局域网资源网段填写在下方", "提示：", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        return;
                    }

                }
            }

            VpnSave.PerformClick();//点击保存按钮
            VpnName.Enabled = false;
            VpnAccount.Enabled = false;
            VpnPassword.Enabled = false;
            VpnAllLink.Enabled = false;
            VpnOnlyPrivateLink.Enabled = false;
            VpnNetGate.Enabled = false;
            VpnNet.Enabled = false;
            VpnSave.Enabled = false;
            VpnConnect.Enabled = false;
            VpnConnect.Text = "处理中……";
            AutoConnectSmbSetting = false;
            Dis_ConnectVpn.Enabled = false;
            Dis_ConnectVpn.Text = "处理中……";

            Thread NewPing = new Thread(new ParameterizedThreadStart(PingThenConnectVpn));
            NewPing.IsBackground = true;
            NewPing.Start(VpnNetGate.Text);//多参数可又组成数组传递进去。
        }

        public void PingThenConnectVpn(object Ip)
        {
            string Command = "";
            if (Ping(Ip.ToString()))
            {
                //MessageBox.Show("ping通，已连接VPN，将断开");
                Command = "rasdial " + VpnName.Text + " /disconnect";
            }
            else
            {
                //MessageBox.Show("ping不通，未连接，将连接");                
                Command = "rasdial " + VpnName.Text + " " + VpnAccount.Text + " " + VpnPassword.Text;
            }
            Cmd(Command);
            ChangeRoute(Ip.ToString());

            VpnConnect.Enabled = true;
            VpnAllLink.Enabled = true;
            VpnOnlyPrivateLink.Enabled = true;
            Dis_ConnectVpn.Enabled  = true;
            AutoConnectSmbSettingCheck();
        }

        public void ChangeRoute(string IP)//连接更改路由表
        {
            
            if (Ping(IP))//能ping通，改路由表
            {
                string Command2 = "";
                if (VpnOnlyPrivateLink.Checked == true)//执行route指令模块
                {
                    if (VpnNet.Text == null || VpnNet.Text == "")//如果下面的路由框是空的
                    {
                        //string Range = VpnNetGate.Text;
                        //while (Range.Substring(Range.Length - 1, 1) != ".")
                        //{
                        //    Range = Range.Remove(Range.Length - 1, 1);
                        //}
                        //Command2 = "route add " + Range + "0 mask 255.255.255.0 " + VpnNetGate.Text;
                        //Cmd(Command2);
                    }
                    else
                    {

                    
                        string Route = VpnNet.Text + "\r\n";//分行并每行执行Route指令
                        int LineNumberA = 0;
                        int LineNumberB = 0;
                        for (int i = 0; i < VpnNet.Text.Length + 1; i++)//逐字查找换行符，有换行就把那一行的执行了
                            {
                            if (Route.Substring(0, 1) == "\r\n" || Route.Substring(0, 1) == "\r" || Route.Substring(0, 1) == "\n")
                            {
                                LineNumberB = i;
                                string RouteLine = VpnNet.Text.Substring(LineNumberA, LineNumberB - LineNumberA) + "/";//截取每一行
                                LineNumberA = LineNumberB + 1;
                                //MessageBox.Show(RouteLine);
                                int RouteLineNumber = 0;
                                string RouteLine1 = RouteLine;
                                for (int j = 0; j < RouteLine.Length; j++)//判断/的个数
                                {
                                    if (RouteLine1.Substring(0, 1) == "/")
                                    {
                                        RouteLineNumber++;
                                    }
                                    RouteLine1 = RouteLine1.Remove(0, 1);
                                }   
                                if (RouteLineNumber == 1)
                                {
                                    Command2 = "route add " + RouteLine.Substring(0, RouteLine.Length - 1) + " mask 255.255.255.0 " + VpnNetGate.Text;
                                    //MessageBox.Show(Command2);
                                    Cmd(Command2);
                                }
                                else
                                {
                                    string RouteLineIp = "";
                                    string RouteLineMask = "";
                                    string RouteLineGate = "";
                                    string RouteLine2 = RouteLine;
                                    int RouteLineNumberA = 0;
                                    for (int k = 0; k < RouteLine.Length; k++)
                                    {
                                        if (RouteLine2.Substring(0, 1) == "/")
                                        {
                                            if (RouteLineIp == "")
                                            {
                                                RouteLineIp = RouteLine.Substring(RouteLineNumberA, k - RouteLineNumberA);
                                            }
                                            else
                                            {
                                                if (RouteLineMask == "")
                                                {
                                                    RouteLineMask = RouteLine.Substring(RouteLineNumberA, k - RouteLineNumberA);
                                                }
                                                else
                                                {
                                                    RouteLineGate = RouteLine.Substring(RouteLineNumberA, k - RouteLineNumberA);
                                                }
                                            }
                                            RouteLineNumberA = k + 1;
                                        }
                                        Command2 = "route add " + RouteLineIp + " mask " + RouteLineMask + " " + RouteLineGate;
                                        RouteLine2 = RouteLine2.Remove(0, 1);
                                    }
                                    //MessageBox.Show(Command2);
                                    Cmd(Command2);
                                }
                            }
                            Route = Route.Remove(0, 1);
                        }
                    }
                }
                else
                {
                    Command2 = "route change 0.0.0.0 mask 0.0.0.0 " + VpnNetGate.Text;
                    Cmd(Command2);
                }
                VpnNet.Enabled = false;
                VpnNetGate.Enabled = false;
                VpnName.Enabled = false;
                VpnAccount.Enabled = false;
                VpnPassword.Enabled = false;
                VpnSave.Enabled = false;
            }
            else//ping不通，获取网关改默认路由表
            {
                string Command = "ipconfig /renew";
                Cmd(Command);
                if (VpnAllLink.Checked == false)
                {
                    VpnNet.Enabled = true;
                }
                VpnNetGate.Enabled = true;
                VpnName.Enabled = true;
                VpnAccount.Enabled = true;
                VpnPassword.Enabled = true;
                VpnSave.Enabled = true;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////上：vpn连接页面；下：smb页面

        private void SmbSave_Click(object sender, EventArgs e)//smb页面点击保存按钮
        {
            if (!SmbUrlCheck())//检查url
            {
                return;
            }

            string txt =  SmbUrl.Text + "\r\n" + "q615749669";//创建字符串
            //MessageBox.Show(txt);
            CreateTxtFile(txt, "SmbData.txt");//保存文件
        }

        public bool SmbUrlCheck()//检查smb url
        {
            if (SmbUrl.Text == "" || SmbUrl.Text == null)
            {
                return true;
            }

            while (SmbUrl.Text.Substring (SmbUrl.Text.Length -1,1)=="\r"|| SmbUrl.Text.Substring(SmbUrl.Text.Length - 1, 1) == "\n" || SmbUrl.Text.Substring(SmbUrl.Text.Length - 1, 1) == "\r\n")
            {
                SmbUrl.Text= SmbUrl.Text.Remove(SmbUrl.Text.Length-1, 1);
                if (SmbUrl.Text.Length == 0)
                {
                    break;
                }
            }
            string UrlCheck = SmbUrl.Text + "\r\n";
            int EnterA = 0;
            int EnterB = 0;
            for (int i = 0; i < SmbUrl.Text.Length + 1; i++)//每行拆出分别检查
            {
                if (UrlCheck.Substring(0, 1) == "\r\n" || UrlCheck.Substring(0, 1) == "\n" || UrlCheck.Substring(0, 1) == "\r")
                {
                    EnterB = i;
                    string SmbLine = SmbUrl.Text.Substring(EnterA, EnterB - EnterA);
                    //MessageBox.Show(SmbLine);
                    if (!SmbUrlCheckLine(SmbLine))//检查每行
                    {
                        MessageBox.Show("SMB URL 填写错误。");
                        return false;
                    }
                    EnterA = EnterB + 1;
                }
                UrlCheck = UrlCheck.Remove(0, 1);
            }
            return true;
        }

        public bool SmbUrlCheckLine(string Url)//检查每行
        {
            if (Url.Length < 13)//每行长度小于13直接false
            {
                return false;
            }

            string LineCheck = Url;//把sml url里的ip拆出来
            int LineCount = 0;
            int LineNumber2 = 0;
            int LineNumber3 = 0;
            for(int i = 0; i < Url.Length; i++)
            {
                if(LineCheck.Substring(0, 1) == "\\")
                {
                    LineCount++;
                    if (LineCount == 2)
                    {
                        LineNumber2 = i + 1;
                    }
                    if(LineCount == 3)
                    {
                        LineNumber3 = i;
                    }
                }
                LineCheck=LineCheck.Remove(0, 1);
            }
            if (LineCount < 3)
            {
                return false;
            }
            string Ip=Url.Substring(LineNumber2, LineNumber3 - LineNumber2);

            if (Regex.IsMatch(Url.Substring(0, 1), @"^[A-Za-z]+$") && Url.Substring(1, 4) == ": \\\\" && VpnTextInputCheckIp(Ip, "Smb"))//检查条件【字母】【英文冒号 空格 斜杠斜杠】、ip合法性
            {
                return true;
            }
            return false;
        }

        private void SmbConnect_Click(object sender, EventArgs e)//点击smb连接按钮
        {
            SmbConnect.Text = "处理中……";
            SmbDisconnect.Text = "处理中……";
            ConnectSmb.Text = "处理中……";
            DisconnectSmb.Text = "处理中……";
            SmbDisconnect.Enabled = false;
            SmbConnect.Enabled = false;
            SmbUrl.Enabled = false;
            SmbSave.Enabled = false;
            AutoConnectSmbSetting = false;
            ConnectSmb.Enabled = false;
            DisconnectSmb.Enabled=false;

            if (!SmbUrlCheck())//检查url
            {
                SmbConnect.Text = "连接";
                SmbDisconnect.Text = "断开";
                ConnectSmb.Text = "连接 SMB";
                DisconnectSmb.Text = "断开 SMB";
                SmbDisconnect.Enabled = true;
                SmbConnect.Enabled = true;
                ConnectSmb.Enabled = true;
                DisconnectSmb.Enabled = true;
                return;
            }

            SmbSave.PerformClick();

            if (SmbUrl.Text == "" || SmbUrl.Text == null)
            {
                SmbConnect.Text = "连接";
                SmbDisconnect.Text = "断开";
                ConnectSmb.Text = "连接 SMB";
                DisconnectSmb.Text = "断开 SMB";
                SmbDisconnect.Enabled = true;
                SmbConnect.Enabled = true;
                SmbUrl.Enabled = true;
                SmbSave.Enabled = true;
                ConnectSmb.Enabled = true;
                DisconnectSmb.Enabled = true;
                return;
            }

            Thread NewPing = new Thread(new ParameterizedThreadStart(PingThenConnectSmb));//能ping通，线程输入net use指令
            NewPing.IsBackground = true;
            NewPing.Start("Connect");//多参数可又组成数组传递进去。
        }

        public void PingThenConnectSmb(object ConnectOrDis)////新县城：能ping通，把总指令写成txt，然后用一个用户权限的c#程序执行他
        {
            string UrlCheck = SmbUrl.Text + "\r\n";
            int EnterA = 0;
            int EnterB = 0;
            string Command = "";
            for (int i = 0; i < SmbUrl.Text.Length + 1; i++)//每行拆出分别检查
            {
                if (UrlCheck.Substring(0, 1) == "\r\n" || UrlCheck.Substring(0, 1) == "\n" || UrlCheck.Substring(0, 1) == "\r")
                {
                    EnterB = i;
                    string SmbLine = SmbUrl.Text.Substring(EnterA, EnterB - EnterA);
                    //MessageBox.Show(SmbLine);

                    if (ConnectOrDis.ToString() == "Connect"|| ConnectOrDis.ToString() == "AutoConnect")//生成cmd指令
                    {
                        //连接的话先都ping一遍
                        int LineLength = SmbLine.Length;
                        string PingIp = SmbLine.Remove(0, 5);
                        for (int j = 0; j < LineLength; j++)
                        {
                            if (PingIp.Substring(0, 1) == "\\")
                            {
                                //MessageBox.Show(SmbLine.Remove(0, 5).Substring(0, j));
                                if (!Ping(SmbLine.Remove(0, 5).Substring(0, j)))
                                {
                                    if(ConnectOrDis.ToString() == "Connect")
                                    {
                                        MessageBox.Show("存在无法Ping通的主机");
                                    }
                                    SmbConnect.Text = "连接";
                                    SmbDisconnect.Text = "断开";
                                    ConnectSmb.Text = "连接 SMB";
                                    DisconnectSmb.Text = "断开 SMB";
                                    SmbDisconnect.Enabled = true;
                                    SmbConnect.Enabled = true;
                                    SmbUrl.Enabled = true;
                                    SmbSave.Enabled = true;
                                    ConnectSmb.Enabled = true;
                                    DisconnectSmb.Enabled = true;
                                    AutoConnectSmbSettingCheck();
                                    return;
                                }
                                break;
                            }
                            PingIp = PingIp.Remove(0, 1);
                        }
                        //生成cmd命令
                        Command = Command + "net use " + SmbLine.Substring(0, 1) + ": /delete\r\nnet use " + SmbLine+ " /persistent:no\r\n";
                    }
                    else
                    {
                        Command = Command +"net use " + SmbLine.Substring(0, 1) + ": /delete\r\n";
                    }

                    EnterA = EnterB + 1;
                }
                UrlCheck = UrlCheck.Remove(0, 1);
            }
            //MessageBox.Show(Command);
            Command = Command.Remove(Command.Length-2, 2);
            
            CreateTxtFile(Command, "ExeCmd.txt");

            string ExecuteCmdExePath = "C:\\Program Files\\SMB HelpER\\NonAdministratorExecuteCmd.exe";
            Process.Start("explorer.exe", ExecuteCmdExePath);//使用explorer代理运行，以用户权限启动

            SmbConnect.Text = "连接";
            SmbDisconnect.Text = "断开";
            ConnectSmb.Text = "连接 SMB";
            DisconnectSmb.Text = "断开 SMB";
            SmbDisconnect.Enabled = true;
            SmbConnect.Enabled = true;
            ConnectSmb.Enabled = true;
            DisconnectSmb.Enabled = true;

            if (ConnectOrDis.ToString() == "Disconnect")
            {
                SmbUrl.Enabled = true;
                SmbSave.Enabled = true;
                SystemSmbAutoConnect.Checked = false;
                tabControl1.SelectedTab = Software;
                SystemSave.PerformClick();
                tabControl1.SelectedTab = Smb;
            }        
        }

        //////////////////////////////////////////////////////////////////////////////////////上：smb页面；下：系统页面

        private void SystemSave_Click(object sender, EventArgs e)//点击系统页面的保存按钮
        {
            //MessageBox.Show("");
            string VpnConnect = "Auto";
            string VpnDisconnect = "Auto";
            string SmbConnect = "Auto";
            string SmbDisconnect = "Auto";
            string StartWithWindows = "False";
            if (SystemVpnAutoConnect.Checked == false)
            {
                VpnConnect = "Manual";
            }
            if (SystemVpnAutoDisconnect.Checked == false)
            {
                VpnDisconnect = "Manual";
            }
            if (SystemSmbAutoConnect.Checked == false)
            {
                SmbConnect = "Manual";
            }
            if (SystemSmbAutoDisconnect.Checked == false)
            {
                SmbDisconnect = "Manual";
            }
            if (SystemStartWithWindows.Checked == true)
            {
                StartWithWindows = "True";
            }
            string txt = VpnConnect + "\r\n" + VpnDisconnect + "\r\n" + SmbConnect + "\r\n" + SmbDisconnect 
                + "\r\n" + StartWithWindows + "\r\n" + "q615749669";
            //MessageBox.Show(txt);
            CreateTxtFile(txt, "SystemData.txt");
            AutoConnectSmbSettingCheck();
            if (SystemStartWithWindows.Checked)
            {
                string path = System.Windows.Forms.Application.ExecutablePath;
                Microsoft.Win32.RegistryKey rk2 = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run");
                rk2.SetValue("SMB_HelpER", @"""" + path + @""""+ " -autorun");
                rk2.Close();
            }
            else
            {
                Microsoft.Win32.RegistryKey rk2 = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run");
                rk2.DeleteValue("SMB_HelpER", false);
                rk2.Close();
            }
        }

        private void SmbDisconnect_Click(object sender, EventArgs e)//点击smb断开按钮
        {
            SmbConnect.Text = "处理中……";
            SmbDisconnect.Text = "处理中……";
            ConnectSmb.Text = "处理中……";
            DisconnectSmb.Text = "处理中……";
            SmbDisconnect.Enabled = false;
            SmbConnect.Enabled = false;
            SmbUrl.Enabled = false;
            SmbSave.Enabled = false;
            ConnectSmb.Enabled = false;
            DisconnectSmb.Enabled = false;


            if (!SmbUrlCheck())//检查url
            {
                SmbConnect.Text = "连接";
                SmbDisconnect.Text = "断开";
                ConnectSmb.Text = "连接 SMB";
                DisconnectSmb.Text = "断开 SMB";
                SmbDisconnect.Enabled = true;
                SmbConnect.Enabled = true;
                ConnectSmb.Enabled = true;
                DisconnectSmb.Enabled = true;
                return;
            }

            SmbSave.PerformClick();

            if (SmbUrl.Text == "" || SmbUrl.Text == null)
            {
                SmbConnect.Text = "连接";
                SmbDisconnect.Text = "断开";
                ConnectSmb.Text = "连接 SMB";
                DisconnectSmb.Text = "断开 SMB";
                SmbDisconnect.Enabled = true;
                SmbConnect.Enabled = true;
                SmbUrl.Enabled = true;
                SmbSave.Enabled = true;
                ConnectSmb.Enabled = true;
                DisconnectSmb.Enabled = true;
                return;
            }

            Thread NewPing = new Thread(new ParameterizedThreadStart(PingThenConnectSmb));//能ping通，线程输入net use指令
            NewPing.IsBackground = true;
            NewPing.Start("Disconnect");//多参数可又组成数组传递进去。
        }

        private void SystemReset_Click(object sender, EventArgs e)//点击恢复设置按钮
        {
            SystemVpnAutoConnect.Checked=false;
            SystemVpnAutoDisconnect.Checked = true;
            SystemSmbAutoConnect.Checked = false;
            SystemSmbAutoDisconnect.Checked = true;
            SystemStartWithWindows.Checked = false;
        }

        public void AutoConnectSmb()//自动连接smb
        {
            Thread.Sleep(5000);
            while (true)
            {
                while (AutoConnectSmbSetting && SmbSave.Enabled)
                {
                    SmbConnect.Text = "处理中……";
                    SmbDisconnect.Text = "处理中……";
                    SmbDisconnect.Enabled = false;
                    SmbConnect.Enabled = false;
                    SmbUrl.Enabled = false;
                    SmbSave.Enabled = false;
                    AutoConnectSmbSetting = false;

                    if (!SmbUrlCheck())//检查url
                    {
                        SmbConnect.Text = "连接";
                        SmbConnect.Enabled = true;
                        return;
                    }

                    SmbSave.PerformClick();

                    if (SmbUrl.Text == "" || SmbUrl.Text == null)
                    {
                        SmbConnect.Text = "连接";
                        SmbDisconnect.Text = "断开";
                        SmbDisconnect.Enabled = true;
                        SmbConnect.Enabled = true;
                        SmbUrl.Enabled = true;
                        SmbSave.Enabled = true;
                        return;
                    }

                    Thread NewPing = new Thread(new ParameterizedThreadStart(PingThenConnectSmb));//能ping通，线程输入net use指令
                    NewPing.IsBackground = true;
                    NewPing.Start("AutoConnect");//多参数可又组成数组传递进去。
                    Thread.Sleep(8000);
                }
                Thread.Sleep(8000);
            }
        }

        public void AutoConnectSmbSettingCheck()//检查smb自动连接的checkbox并更改public bool
        {
            if (SystemSmbAutoConnect.Checked == true)
            {
                AutoConnectSmbSetting = true;
            }
            else
            {
                AutoConnectSmbSetting = false;
            }
        }

        public void IfDisconnect()//关闭软件时vpn和smb都断开
        {
            tabControl1.SelectedTab = Smb;
            if (SystemSmbAutoDisconnect.Checked)
            {

                while (!SmbDisconnect.Enabled) { }
                SmbDisconnect.PerformClick();
            }
            tabControl1.SelectedTab = Vpn;
            if (SystemVpnAutoDisconnect.Checked)
            {
                if (!VpnSave.Enabled)
                {
                    while (!VpnConnect.Enabled) { }
                    VpnConnect.PerformClick();
                }
            }

            Thread CloseSoftwareT = new Thread(CloseSoftware);//等待指令执行完，退出程序
            CloseSoftwareT.IsBackground = true;
            CloseSoftwareT.Start();
        }

        public bool IsAutoRun()//检查参数判断是否是开机自动启动
        {
            //string strFilePath = Application.ExecutablePath;
            //string strFileName = System.IO.Path.GetFileName(strFilePath);

            //if (Form1.IsAutoRun(strFilePath + " -autostart", strFileName))
            //{
            string[] strArgs = Environment.GetCommandLineArgs();
            if (strArgs.Length >= 2 && strArgs[1].Equals("-autorun"))
            {
                //MessageBox.Show("自动");
                AutoRun = true;
                return true;
            }
            else
            {
                //MessageBox.Show("手动");
                AutoRun = false;
                return false;
            }
            //}
            //else
            //{
            //MessageBox.Show("手动");
            //}
        }

        private void SystemClear_Click(object sender, EventArgs e)//点击清除配置和注册表，关闭程序按钮
        {
            Microsoft.Win32.RegistryKey rk2 = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run");
            rk2.DeleteValue("SMB_HelpER", false);
            rk2.Close();//删除注册表

            Directory.Delete("C:\\Program Files\\SMB HelpER",true);//删除配置文件文件夹
        }

        private void SystemShareOut_Click(object sender, EventArgs e)//点击导出配置按钮
        {
            FolderBrowserDialog dilog = new FolderBrowserDialog();//先选择存储文件夹
            dilog.Description = "请选择文件夹";
            if (dilog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string pSaveFilePath = dilog.SelectedPath + "\\SMB_HelpER_Backup_Q615749669.zip";

            try//先尝试删除防止创建压缩文件时报错
            {
                File.Delete(pSaveFilePath);
            }
            catch (Exception ex)
            {

            }
            ZipFile.CreateFromDirectory("C:\\Program Files\\SMB HelpER", pSaveFilePath);
        }

        private void SystemShareIn_Click(object sender, EventArgs e)//点击导入配置文件按钮
        {
            OpenFileDialog fileDialog = new OpenFileDialog();//打开文件
            fileDialog.Multiselect = false;//多选
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "zip文件|SMB_HelpER_Backup_Q615749669.zip";
            if (fileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            if (!System.IO.Directory.Exists("C:\\Program Files\\SMB HelpER"))
            {
                System.IO.Directory.CreateDirectory("C:\\Program Files\\SMB HelpER");
            }
            else
            {
                System.IO.Directory.Delete("C:\\Program Files\\SMB HelpER");
                System.IO.Directory.CreateDirectory("C:\\Program Files\\SMB HelpER");
            }
            ////////解压缩开始
            ZipFile.ExtractToDirectory(fileDialog.FileName, "C:\\Program Files\\SMB HelpER");
            //MessageBox.Show("");
        }

        private void Dis_ConnectVpn_Click(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                Form1.form1.ShowInTaskbar = false;
                Form1.form1.WindowState = FormWindowState.Minimized;
                Form1.form1.Opacity = 0;
                this.Visible = true;
                this.Activate();
                //MessageBox.Show("主界面没有显示");
            }
            tabControl1.SelectedTab = Vpn;
            VpnConnect.PerformClick();
            this.Hide();
        }

        private void ConnectSmb_Click(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                Form1.form1.ShowInTaskbar = false;
                Form1.form1.WindowState = FormWindowState.Minimized;
                Form1.form1.Opacity = 0;
                this.Visible = true;
                this.Activate();
                //MessageBox.Show("主界面没有显示");
            }
            tabControl1.SelectedTab = Smb;
            SmbConnect.PerformClick();
            this.Hide();
        }

        private void DisconnectSmb_Click(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                Form1.form1.ShowInTaskbar = false;
                Form1.form1.WindowState = FormWindowState.Minimized;
                Form1.form1.Opacity = 0;
                this.Visible = true;
                this.Activate();
                //MessageBox.Show("主界面没有显示");
            }
            tabControl1.SelectedTab = Smb;
            SmbDisconnect.PerformClick();
            this.Hide();
        }

        //////////////////////////////////////////////////////////////////////////

    }
}