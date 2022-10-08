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

        private void Form1_Load(object sender, EventArgs e)//�����load�׶�ִ��ָ��
        {
            Control.CheckForIllegalCrossThreadCalls = false;//ʹ�߳��ܹ������ؼ�
            if (IsAutoRun())//����Ƿ��ǿ����Զ����У���С������������
            {
                Form1.form1.WindowState = FormWindowState.Minimized;
                Form1.form1.ShowInTaskbar = false;
            }
            ReadDataTxt();//�������д����
        }

        public void PingCheck(object Ip)//ping���ؼ��
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
                        MessageBox.Show("Vpn���ơ��˻�������δ��д��");
                        return;
                    }

                    if (!VpnTextInputCheck())//���vpnҳ����д����д����ȷ���˳�
                    {
                        return;
                    }

                    VpnSave.PerformClick();//������水ť
                    VpnName.Enabled = false;
                    VpnAccount.Enabled = false;
                    VpnPassword.Enabled = false;
                    VpnAllLink.Enabled = false;
                    VpnOnlyPrivateLink.Enabled = false;
                    VpnNetGate.Enabled = false;
                    VpnNet.Enabled = false;
                    VpnSave.Enabled = false;
                    VpnConnect.Enabled = false;
                    VpnConnect.Text = "�����С���";
                    AutoConnectSmbSetting = false;
                    Dis_ConnectVpn.Text = "�����С���";
                    Dis_ConnectVpn.Enabled = false;

                    Thread NewPing = new Thread(new ParameterizedThreadStart(PingThenConnectVpn));
                    NewPing.IsBackground = true;
                    NewPing.Start(VpnNetGate.Text);//���������������鴫�ݽ�ȥ��
                }
            }
        }

        private void Form1_Shown_1(object sender, EventArgs e)//��������ļ��Ƿ����
        {
            //MessageBox.Show("��������ļ��Ƿ����");
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
            if (!File.Exists("C:\\Program Files\\SMB HelpER\\NonAdministratorExecuteCmd.exe"))//չ���Զ�ִ��cmd��exe
            {
                if (!System.IO.Directory.Exists("C:\\Program Files\\SMB HelpER"))
                {
                    System.IO.Directory.CreateDirectory("C:\\Program Files\\SMB HelpER");
                }
                System.Reflection.Assembly assembly = GetType().Assembly;
                Stream streamSmall = assembly.GetManifestResourceStream("SMB HelpER.Resources.NonAdministratorExecuteCmd.exe");
                //ע��ImageAreaSelector.Resources.template.xlsx����Դ·��
                // �����Դ·��д���˻᷵�ؿ�ֵ��ʹ��ʱ�ͻᱨ��δ�������������õ������ʵ�������쳣
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

            Thread PingCheckThred = new Thread(new ParameterizedThreadStart(PingCheck));//���سǴ���cmd�����ֹ��������
            PingCheckThred.IsBackground = true;
            PingCheckThred.Start(VpnNetGate.Text);//���������������鴫�ݽ�ȥ��

            AutoConnectSmbSettingCheck();//���smb�Զ����ӵ�checkbox

            Thread AutoSmb = new Thread(AutoConnectSmb);//�ظ�ִ�У����bool���ж��Ƿ�����smb
            AutoSmb.IsBackground = true;
            AutoSmb.Start();
            if (AutoRun)//�ǿ����Զ����еĻ�����С������������
            {
                //MessageBox.Show("");
                //
                Form1.form1.Hide();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)//ѡ��л�Ҳˢ������
        {
            ReadDataTxt();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)//��ֹ�رմ��ڣ���������
        {
            //ȡ��"�رմ���"�¼�
            e.Cancel = true; // ȡ���رմ��� 
            this.Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)//˫����ʾ������
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

        private void Show_Click(object sender, EventArgs e)//������̲˵���ʾҳ��
        {
            this.Visible = true;
            this.Activate();
            Form1.form1.Opacity = 100;
            Form1.form1.ShowInTaskbar = true;
            //Form1.form1.WindowState = FormWindowState.Maximized;
        }

        private void Close_Click(object sender, EventArgs e)//������̲˵��˳�
        {
            this.Hide();
            notifyIcon1.Text = "�˳��� Closing";
            contextMenuStrip1.Enabled = false;
            Form1.form1.ShowInTaskbar = false;
            Form1.form1.WindowState = FormWindowState.Minimized;
            Form1.form1.Opacity = 0;
            this.Visible = true;
            this.Activate();
            
            //this.Show();
            
            AutoConnectSmbSetting = false;
            Thread IfDisconnectT = new Thread(IfDisconnect);//�ر�ʱ�ж��Ƿ�Ͽ�����
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

        //////////////////////////////////////////////////////////////////////�ϣ��������ݣ��£�public����

        public static void CreateTxtFile(string Txt, string FileName)//��txt��д���ļ�
        {
                FileStream fs1 = new FileStream("C:\\Program Files\\SMB HelpER\\" + FileName, FileMode.Create, FileAccess.Write);//����д���ļ� 
                StreamWriter sw = new StreamWriter(fs1, Encoding.UTF8);
                sw.WriteLine(Txt);//��ʼд��ֵ
                sw.Close();
                fs1.Close();
        }

        private static void ReadDataTxt()//��дÿ��ҳ�������
        {
            //////////��дvpnҳ��
            if (File.Exists("C:\\Program Files\\SMB HelpER\\VpnData.txt"))//�ж��ļ�����
            {
                StreamReader VpnData = new StreamReader("C:\\Program Files\\SMB HelpER\\VpnData.txt", Encoding.UTF8);//��ȡ�ļ�
                int line = 0;
                string VpnNameData = "";
                string VpnAccountData = "";
                string VpnPasswordData = "";
                string VpnLinkTypeData = "";
                string VpnNetGateData = "";
                string VpnNetData = "";
                while (!VpnData.EndOfStream)//ѭ��
                {
                    string config = VpnData.ReadLine();
                    if (line == 0)//���������������+1
                    {
                        //MessageBox.Show(config);
                        VpnNameData = config;//��config�����ݴ��ݸ�setting
                                             //break;
                    }
                    else if (line == 1)//���������������+1
                    {
                        //MessageBox.Show(config);
                        VpnAccountData = config;//��config�����ݴ��ݸ�setting
                                                //break;
                    }
                    else if (line == 2)//���������������+1
                    {
                        //MessageBox.Show(config);
                        VpnPasswordData = config;//��config�����ݴ��ݸ�setting
                                                 //break;
                    }
                    else if (line == 3)//���������������+1
                    {
                        //MessageBox.Show(config);
                        VpnLinkTypeData = config;//��config�����ݴ��ݸ�setting
                                                 //break;
                    }
                    else if (line == 4)//���������������+1
                    {
                        //MessageBox.Show(config);
                        VpnNetGateData = config;//��config�����ݴ��ݸ�setting
                                                //break;
                    }
                    else if (config == "q615749669")//���������������+1
                    {
                        //MessageBox.Show(config);
                        //VpnNet = config;//��config�����ݴ��ݸ�setting
                        break;
                    }
                    else//���������������+1
                    {
                        //MessageBox.Show(config);
                        if (config != "")
                        {
                            VpnNetData += config + "\r\n";//��config�����ݴ��ݸ�setting
                        }
                    }
                    line++;//����һ��
                }
                //MessageBox.Show(VpnNameData);//����
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
                    form1.VpnNet.Text = VpnNetData.Substring(0, VpnNetData.Length - 2);//ȥ�����Ļ��з�
                }
                VpnData.Close();//�رս���
            }

            //////////��дsmbҳ��
            if (File.Exists("C:\\Program Files\\SMB HelpER\\SmbData.txt"))//�ж��ļ��Ƿ����
            {
                StreamReader SmbData = new StreamReader("C:\\Program Files\\SMB HelpER\\SmbData.txt", Encoding.UTF8);//��ȡ�ļ�
                int line = 0;
                string SmbUrlData = "";
                while (!SmbData.EndOfStream)//ѭ��
                {
                    string config = SmbData.ReadLine();
                    if (config == "q615749669")//���������������+1
                    {
                        //MessageBox.Show(config);
                        //VpnNet = config;//��config�����ݴ��ݸ�setting
                        break;
                    }
                    else//���������������+1
                    {
                        //MessageBox.Show(config);
                        if (config != "")
                        {
                            SmbUrlData += config + "\r\n";//��config�����ݴ��ݸ�setting
                        }
                    }
                    line++;//����һ��
                }
                //MessageBox.Show(VpnNameData);//����
                if (SmbUrlData.Length >= 2)
                {
                    form1.SmbUrl.Text = SmbUrlData.Substring(0, SmbUrlData.Length - 2);//ȥ�����Ļ��з�
                }
                SmbData.Close();//�رս���       
            }
            //////////ϵͳ����ҳ��
            if (File.Exists("C:\\Program Files\\SMB HelpER\\SystemData.txt"))//�ж��ļ��Ƿ����
            {
                StreamReader SystemData = new StreamReader("C:\\Program Files\\SMB HelpER\\SystemData.txt", Encoding.UTF8);//��ȡ�ļ�
                int line = 0;
                string VpnConnect = "";
                string VpnDisconnect = "";
                string SmbConnect = "";
                string SmbDisconnect = "";
                string StartWithWindows = "";
                while (!SystemData.EndOfStream)//ѭ��
                {
                    string config = SystemData.ReadLine();
                    if (line == 0)//���������������+1
                    {
                        //MessageBox.Show(config);
                        VpnConnect = config;//��config�����ݴ��ݸ�setting
                                            //break;
                    }
                    else if (line == 1)//���������������+1
                    {
                        //MessageBox.Show(config);
                        VpnDisconnect = config;//��config�����ݴ��ݸ�setting
                                               //break;
                    }
                    else if (line == 2)//���������������+1
                    {
                        //MessageBox.Show(config);
                        SmbConnect = config;//��config�����ݴ��ݸ�setting
                                            //break;
                    }
                    else if (line == 3)//���������������+1
                    {
                        //MessageBox.Show(config);
                        SmbDisconnect = config;//��config�����ݴ��ݸ�setting
                                               //break;
                    }
                    else if (line == 4)//���������������+1
                    {
                        //MessageBox.Show(config);
                        StartWithWindows = config;//��config�����ݴ��ݸ�setting
                                                  //break;
                    }
                    else if (config == "q615749669")//���������������+1
                    {
                        //MessageBox.Show(config);
                        //VpnNet = config;//��config�����ݴ��ݸ�setting
                        break;
                    }
                    line++;//����һ��
                }
                //MessageBox.Show(VpnNameData);//����
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
                SystemData.Close();//�رս���       
            }
        }

        //////////����cmd����

        public static string Cmd(object Command)
        {
            //Console.WriteLine("������Ҫִ�е�����:");
            string strInput = Command.ToString();
            Process p = new Process();

            //����Ҫ������Ӧ�ó���
            p.StartInfo.FileName = "cmd.exe";
            //�Ƿ�ʹ�ò���ϵͳshell����
            p.StartInfo.UseShellExecute = false;
            // �������Ե��ó����������Ϣ
            p.StartInfo.RedirectStandardInput = true;
            //�����Ϣ
            p.StartInfo.RedirectStandardOutput = true;
            // �������
            p.StartInfo.RedirectStandardError = true;
            //����ʾ���򴰿�
            p.StartInfo.CreateNoWindow = true;

            //��������
            p.Start();

            //��cmd���ڷ���������Ϣ
            p.StandardInput.WriteLine(strInput + "&exit");

            p.StandardInput.AutoFlush = true;

            //��ȡ�����Ϣ
            string strOuput = p.StandardOutput.ReadToEnd();
            //�ȴ�����ִ�����˳�����
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
            if (Cmd("ping " + Ip.ToString() + " -n 2 -w 500").Contains("TTL"))//pingĿ��ip�����Σ�1�볬ʱ
            {
                //MessageBox.Show("Ping " + Ip.ToString() + " succeed.");
                VpnConnect.Text = "�Ͽ�";
                Dis_ConnectVpn.Text = "�Ͽ� VPN";
                VpnNetGate.Enabled = false;

                return true;//ping�ɹ�
            }
            //MessageBox.Show("Ping " + Ip.ToString() + " failed.");
            VpnConnect.Text = "����";
            Dis_ConnectVpn.Text = "���� VPN";
            return false;//pingʧ��
        }

        /////////////////////////////////////////////////////////////////////////////////�ϣ�public���£�vpnҳ��

        private void VpnSave_Click(object sender, EventArgs e)//����д��txt��ֵ
        {
            if (!VpnTextInputCheck())//���vpnҳ����д����д����ȷ���˳�
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

        private void VpnAllLink_CheckedChanged(object sender, EventArgs e)//ȫ�ֺͽ��������л�
        {
            VpnOnlyPrivateLink.Checked = !VpnAllLink.Checked;
            if (VpnAllLink.Checked == true)
            {
                VpnNet.Enabled = false;
                if (VpnConnect.Text == "�Ͽ�")
                {
                    VpnAllLink.Enabled = false;
                    VpnOnlyPrivateLink.Enabled = false ;
                    string Command = "route change 0.0.0.0 mask 0.0.0.0 " + VpnNetGate.Text;

                    Thread SwitchRouteCommand = new Thread(new ParameterizedThreadStart(SwitchRoute));//���سǴ���cmd�����ֹ��������
                    SwitchRouteCommand.IsBackground = true;
                    SwitchRouteCommand.Start(Command);//���������������鴫�ݽ�ȥ��
                }
            }
            else
            {
                if(VpnConnect.Text == "����")
                {
                    VpnNet.Enabled = true;
                }
                else
                {
                    VpnAllLink.Enabled = false;
                    VpnOnlyPrivateLink.Enabled = false;
                    string Command = "ipconfig /renew&route delete 0.0.0.0 mask 0.0.0.0 " + VpnNetGate.Text;

                    Thread SwitchRouteCommand = new Thread(new ParameterizedThreadStart(SwitchRoute));//���سǴ���cmd�����ֹ��������
                    SwitchRouteCommand.IsBackground = true;
                    SwitchRouteCommand.Start(Command);//���������������鴫�ݽ�ȥ��
                }           
            }
        }

        private void VpnOnlyPrivateLink_CheckedChanged(object sender, EventArgs e)//ȫ�ֺͽ��������л�
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
            MessageBox.Show("ֻ�������λ�ip��ַ����ʹ��Ĭ������255.255.255.0��Ĭ������Ϊ�Ϸ��������������ء�\r\n���磺192.168.1.0" +
                "\r\n�Զ������������� ���λ�IP/����/����\r\n���磺172.22.6.24/255.255.0.0/172.22.22.1");
        }

        /////////////////////////////////////////////////////////////////////////////////////�ϣ�vpnҳ�棻��vpn����ҳ��

        public bool VpnTextInputCheck()//���ҳ�����غ���������
        {
            if(!VpnTextInputCheckIp(VpnNetGate.Text, "NetGate"))//������ص�ip��ַ����
            {
                return false;
            }
            if (!VpnTextInputCheckNet(VpnNet.Text))//���·��textbox������
            {
                return false;
            }
            return true;
        }

        public bool VpnTextInputCheckNet(string Net)//���·��textbox������
        {
            if (Net == null || Net == "") //�հ׷�����ȷ
            { return true; }

            while (Net.Substring(Net.Length - 1, 1) == "\r"|| Net.Substring(Net.Length - 1, 1) == "\n")//�������ǻ��з���ɾ��
            {
                Net = Net.Remove(Net.Length-1,1);
                //MessageBox.Show(Net);
            }
            VpnNet.Text= Net;
            
            string A= Net+"\r\n";//��·��textbox���ÿ�в��ȥ���
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

        public bool VpnTextInputCheckNetLine(string Net)//���·��textbox������
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
            
            if (LineNumber == 0)//����д������
            {
                if (VpnTextInputCheckIp(Net, "")) { return true; } else { return false; }
            }
            if (Line1 < 2 || Line2 - Line1 < 2 || Net.Length - Line2 < 2)
            {
                MessageBox.Show("����ת��������д����");
                return false;
            }
            //MessageBox.Show(Net.Substring(0, Line1 - 1) + Net.Substring(Line1, Line2 - 1-Line1)+ Net.Substring(Line2, Net.Length- Line2));
            if (LineNumber == 2)//��д��Ĭ�����Ρ����������
            {
                if (VpnTextInputCheckIp(Net.Substring(0, Line1 - 1), "") && VpnTextInputCheckMask(Net.Substring(Line1, Line2 - 1-Line1), "")
                    && VpnTextInputCheckIp(Net.Substring(Line2, Net.Length - Line2), ""))
                { return true; }
                else { return false; }
            }
            else
            {
                MessageBox.Show("����ת��������д����");
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

        public bool VpnTextInputCheckIp(string IpAdress, string Box)//���ip��ַ����
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
                        MessageBox.Show("������д����");
                    }
                    else
                    {
                        MessageBox.Show("����ת��������д����");
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
                    MessageBox.Show("������д����");
                }
                else
                {
                    MessageBox.Show("����ת��������д����");
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
                    MessageBox.Show("������д����");
                }
                else
                {
                    MessageBox.Show("����ת��������д����");
                }
                return false;
            }
            //MessageBox.Show(IpAdress.Substring(Dot3, IpAdress.Length - Dot3));
            int Ip4 = int.Parse(IpAdress.Substring(Dot3, IpAdress.Length - Dot3));
            if(Ip1<0|| Ip1>255|| Ip2 < 0 || Ip2 > 255 || Ip3 < 0 || Ip3 > 255 || Ip4 < 0 || Ip4 > 255)
            {
                if (Box == "NetGate")
                {
                    MessageBox.Show("������д����");
                }
                else
                {
                    MessageBox.Show("����ת��������д����");
                }
                return false;
            }
            return true;
        }

        private void VpnConnect_Click(object sender, EventArgs e)//������� / �Ͽ�
        {
            if (VpnName.Text == "" || VpnAccount.Text == "" || VpnPassword.Text == "")
            {
                MessageBox.Show("Vpn���ơ��˻�������δ��д��");
                return;
            }

            if (!VpnTextInputCheck())//���vpnҳ����д����д����ȷ���˳�
            {
                return;
            }

            if (VpnConnect.Text == "����")
            {
                if (VpnNet.Text == null || VpnNet.Text == "")
                {
                    if (MessageBox.Show("δ��д·�����ã���ֻ����VPN��������·�ɣ����Ӻ�Ĭ��ֻת��VPN����\r\nע�⣺���SMB����Ⱦ�������Դ" +
                        "��VPN����ͬһ���Σ���Ҫ����������Դ������д���·�", "��ʾ��", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        return;
                    }

                }
            }

            VpnSave.PerformClick();//������水ť
            VpnName.Enabled = false;
            VpnAccount.Enabled = false;
            VpnPassword.Enabled = false;
            VpnAllLink.Enabled = false;
            VpnOnlyPrivateLink.Enabled = false;
            VpnNetGate.Enabled = false;
            VpnNet.Enabled = false;
            VpnSave.Enabled = false;
            VpnConnect.Enabled = false;
            VpnConnect.Text = "�����С���";
            AutoConnectSmbSetting = false;
            Dis_ConnectVpn.Enabled = false;
            Dis_ConnectVpn.Text = "�����С���";

            Thread NewPing = new Thread(new ParameterizedThreadStart(PingThenConnectVpn));
            NewPing.IsBackground = true;
            NewPing.Start(VpnNetGate.Text);//���������������鴫�ݽ�ȥ��
        }

        public void PingThenConnectVpn(object Ip)
        {
            string Command = "";
            if (Ping(Ip.ToString()))
            {
                //MessageBox.Show("pingͨ��������VPN�����Ͽ�");
                Command = "rasdial " + VpnName.Text + " /disconnect";
            }
            else
            {
                //MessageBox.Show("ping��ͨ��δ���ӣ�������");                
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

        public void ChangeRoute(string IP)//���Ӹ���·�ɱ�
        {
            
            if (Ping(IP))//��pingͨ����·�ɱ�
            {
                string Command2 = "";
                if (VpnOnlyPrivateLink.Checked == true)//ִ��routeָ��ģ��
                {
                    if (VpnNet.Text == null || VpnNet.Text == "")//��������·�ɿ��ǿյ�
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

                    
                        string Route = VpnNet.Text + "\r\n";//���в�ÿ��ִ��Routeָ��
                        int LineNumberA = 0;
                        int LineNumberB = 0;
                        for (int i = 0; i < VpnNet.Text.Length + 1; i++)//���ֲ��һ��з����л��оͰ���һ�е�ִ����
                            {
                            if (Route.Substring(0, 1) == "\r\n" || Route.Substring(0, 1) == "\r" || Route.Substring(0, 1) == "\n")
                            {
                                LineNumberB = i;
                                string RouteLine = VpnNet.Text.Substring(LineNumberA, LineNumberB - LineNumberA) + "/";//��ȡÿһ��
                                LineNumberA = LineNumberB + 1;
                                //MessageBox.Show(RouteLine);
                                int RouteLineNumber = 0;
                                string RouteLine1 = RouteLine;
                                for (int j = 0; j < RouteLine.Length; j++)//�ж�/�ĸ���
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
            else//ping��ͨ����ȡ���ظ�Ĭ��·�ɱ�
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

        //////////////////////////////////////////////////////////////////////////////////////�ϣ�vpn����ҳ�棻�£�smbҳ��

        private void SmbSave_Click(object sender, EventArgs e)//smbҳ�������水ť
        {
            if (!SmbUrlCheck())//���url
            {
                return;
            }

            string txt =  SmbUrl.Text + "\r\n" + "q615749669";//�����ַ���
            //MessageBox.Show(txt);
            CreateTxtFile(txt, "SmbData.txt");//�����ļ�
        }

        public bool SmbUrlCheck()//���smb url
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
            for (int i = 0; i < SmbUrl.Text.Length + 1; i++)//ÿ�в���ֱ���
            {
                if (UrlCheck.Substring(0, 1) == "\r\n" || UrlCheck.Substring(0, 1) == "\n" || UrlCheck.Substring(0, 1) == "\r")
                {
                    EnterB = i;
                    string SmbLine = SmbUrl.Text.Substring(EnterA, EnterB - EnterA);
                    //MessageBox.Show(SmbLine);
                    if (!SmbUrlCheckLine(SmbLine))//���ÿ��
                    {
                        MessageBox.Show("SMB URL ��д����");
                        return false;
                    }
                    EnterA = EnterB + 1;
                }
                UrlCheck = UrlCheck.Remove(0, 1);
            }
            return true;
        }

        public bool SmbUrlCheckLine(string Url)//���ÿ��
        {
            if (Url.Length < 13)//ÿ�г���С��13ֱ��false
            {
                return false;
            }

            string LineCheck = Url;//��sml url���ip�����
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

            if (Regex.IsMatch(Url.Substring(0, 1), @"^[A-Za-z]+$") && Url.Substring(1, 4) == ": \\\\" && VpnTextInputCheckIp(Ip, "Smb"))//�����������ĸ����Ӣ��ð�� �ո� б��б�ܡ���ip�Ϸ���
            {
                return true;
            }
            return false;
        }

        private void SmbConnect_Click(object sender, EventArgs e)//���smb���Ӱ�ť
        {
            SmbConnect.Text = "�����С���";
            SmbDisconnect.Text = "�����С���";
            ConnectSmb.Text = "�����С���";
            DisconnectSmb.Text = "�����С���";
            SmbDisconnect.Enabled = false;
            SmbConnect.Enabled = false;
            SmbUrl.Enabled = false;
            SmbSave.Enabled = false;
            AutoConnectSmbSetting = false;
            ConnectSmb.Enabled = false;
            DisconnectSmb.Enabled=false;

            if (!SmbUrlCheck())//���url
            {
                SmbConnect.Text = "����";
                SmbDisconnect.Text = "�Ͽ�";
                ConnectSmb.Text = "���� SMB";
                DisconnectSmb.Text = "�Ͽ� SMB";
                SmbDisconnect.Enabled = true;
                SmbConnect.Enabled = true;
                ConnectSmb.Enabled = true;
                DisconnectSmb.Enabled = true;
                return;
            }

            SmbSave.PerformClick();

            if (SmbUrl.Text == "" || SmbUrl.Text == null)
            {
                SmbConnect.Text = "����";
                SmbDisconnect.Text = "�Ͽ�";
                ConnectSmb.Text = "���� SMB";
                DisconnectSmb.Text = "�Ͽ� SMB";
                SmbDisconnect.Enabled = true;
                SmbConnect.Enabled = true;
                SmbUrl.Enabled = true;
                SmbSave.Enabled = true;
                ConnectSmb.Enabled = true;
                DisconnectSmb.Enabled = true;
                return;
            }

            Thread NewPing = new Thread(new ParameterizedThreadStart(PingThenConnectSmb));//��pingͨ���߳�����net useָ��
            NewPing.IsBackground = true;
            NewPing.Start("Connect");//���������������鴫�ݽ�ȥ��
        }

        public void PingThenConnectSmb(object ConnectOrDis)////���سǣ���pingͨ������ָ��д��txt��Ȼ����һ���û�Ȩ�޵�c#����ִ����
        {
            string UrlCheck = SmbUrl.Text + "\r\n";
            int EnterA = 0;
            int EnterB = 0;
            string Command = "";
            for (int i = 0; i < SmbUrl.Text.Length + 1; i++)//ÿ�в���ֱ���
            {
                if (UrlCheck.Substring(0, 1) == "\r\n" || UrlCheck.Substring(0, 1) == "\n" || UrlCheck.Substring(0, 1) == "\r")
                {
                    EnterB = i;
                    string SmbLine = SmbUrl.Text.Substring(EnterA, EnterB - EnterA);
                    //MessageBox.Show(SmbLine);

                    if (ConnectOrDis.ToString() == "Connect"|| ConnectOrDis.ToString() == "AutoConnect")//����cmdָ��
                    {
                        //���ӵĻ��ȶ�pingһ��
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
                                        MessageBox.Show("�����޷�Pingͨ������");
                                    }
                                    SmbConnect.Text = "����";
                                    SmbDisconnect.Text = "�Ͽ�";
                                    ConnectSmb.Text = "���� SMB";
                                    DisconnectSmb.Text = "�Ͽ� SMB";
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
                        //����cmd����
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
            Process.Start("explorer.exe", ExecuteCmdExePath);//ʹ��explorer�������У����û�Ȩ������

            SmbConnect.Text = "����";
            SmbDisconnect.Text = "�Ͽ�";
            ConnectSmb.Text = "���� SMB";
            DisconnectSmb.Text = "�Ͽ� SMB";
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

        //////////////////////////////////////////////////////////////////////////////////////�ϣ�smbҳ�棻�£�ϵͳҳ��

        private void SystemSave_Click(object sender, EventArgs e)//���ϵͳҳ��ı��水ť
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

        private void SmbDisconnect_Click(object sender, EventArgs e)//���smb�Ͽ���ť
        {
            SmbConnect.Text = "�����С���";
            SmbDisconnect.Text = "�����С���";
            ConnectSmb.Text = "�����С���";
            DisconnectSmb.Text = "�����С���";
            SmbDisconnect.Enabled = false;
            SmbConnect.Enabled = false;
            SmbUrl.Enabled = false;
            SmbSave.Enabled = false;
            ConnectSmb.Enabled = false;
            DisconnectSmb.Enabled = false;


            if (!SmbUrlCheck())//���url
            {
                SmbConnect.Text = "����";
                SmbDisconnect.Text = "�Ͽ�";
                ConnectSmb.Text = "���� SMB";
                DisconnectSmb.Text = "�Ͽ� SMB";
                SmbDisconnect.Enabled = true;
                SmbConnect.Enabled = true;
                ConnectSmb.Enabled = true;
                DisconnectSmb.Enabled = true;
                return;
            }

            SmbSave.PerformClick();

            if (SmbUrl.Text == "" || SmbUrl.Text == null)
            {
                SmbConnect.Text = "����";
                SmbDisconnect.Text = "�Ͽ�";
                ConnectSmb.Text = "���� SMB";
                DisconnectSmb.Text = "�Ͽ� SMB";
                SmbDisconnect.Enabled = true;
                SmbConnect.Enabled = true;
                SmbUrl.Enabled = true;
                SmbSave.Enabled = true;
                ConnectSmb.Enabled = true;
                DisconnectSmb.Enabled = true;
                return;
            }

            Thread NewPing = new Thread(new ParameterizedThreadStart(PingThenConnectSmb));//��pingͨ���߳�����net useָ��
            NewPing.IsBackground = true;
            NewPing.Start("Disconnect");//���������������鴫�ݽ�ȥ��
        }

        private void SystemReset_Click(object sender, EventArgs e)//����ָ����ð�ť
        {
            SystemVpnAutoConnect.Checked=false;
            SystemVpnAutoDisconnect.Checked = true;
            SystemSmbAutoConnect.Checked = false;
            SystemSmbAutoDisconnect.Checked = true;
            SystemStartWithWindows.Checked = false;
        }

        public void AutoConnectSmb()//�Զ�����smb
        {
            Thread.Sleep(5000);
            while (true)
            {
                while (AutoConnectSmbSetting && SmbSave.Enabled)
                {
                    SmbConnect.Text = "�����С���";
                    SmbDisconnect.Text = "�����С���";
                    SmbDisconnect.Enabled = false;
                    SmbConnect.Enabled = false;
                    SmbUrl.Enabled = false;
                    SmbSave.Enabled = false;
                    AutoConnectSmbSetting = false;

                    if (!SmbUrlCheck())//���url
                    {
                        SmbConnect.Text = "����";
                        SmbConnect.Enabled = true;
                        return;
                    }

                    SmbSave.PerformClick();

                    if (SmbUrl.Text == "" || SmbUrl.Text == null)
                    {
                        SmbConnect.Text = "����";
                        SmbDisconnect.Text = "�Ͽ�";
                        SmbDisconnect.Enabled = true;
                        SmbConnect.Enabled = true;
                        SmbUrl.Enabled = true;
                        SmbSave.Enabled = true;
                        return;
                    }

                    Thread NewPing = new Thread(new ParameterizedThreadStart(PingThenConnectSmb));//��pingͨ���߳�����net useָ��
                    NewPing.IsBackground = true;
                    NewPing.Start("AutoConnect");//���������������鴫�ݽ�ȥ��
                    Thread.Sleep(8000);
                }
                Thread.Sleep(8000);
            }
        }

        public void AutoConnectSmbSettingCheck()//���smb�Զ����ӵ�checkbox������public bool
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

        public void IfDisconnect()//�ر����ʱvpn��smb���Ͽ�
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

            Thread CloseSoftwareT = new Thread(CloseSoftware);//�ȴ�ָ��ִ���꣬�˳�����
            CloseSoftwareT.IsBackground = true;
            CloseSoftwareT.Start();
        }

        public bool IsAutoRun()//�������ж��Ƿ��ǿ����Զ�����
        {
            //string strFilePath = Application.ExecutablePath;
            //string strFileName = System.IO.Path.GetFileName(strFilePath);

            //if (Form1.IsAutoRun(strFilePath + " -autostart", strFileName))
            //{
            string[] strArgs = Environment.GetCommandLineArgs();
            if (strArgs.Length >= 2 && strArgs[1].Equals("-autorun"))
            {
                //MessageBox.Show("�Զ�");
                AutoRun = true;
                return true;
            }
            else
            {
                //MessageBox.Show("�ֶ�");
                AutoRun = false;
                return false;
            }
            //}
            //else
            //{
            //MessageBox.Show("�ֶ�");
            //}
        }

        private void SystemClear_Click(object sender, EventArgs e)//���������ú�ע����رճ���ť
        {
            Microsoft.Win32.RegistryKey rk2 = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run");
            rk2.DeleteValue("SMB_HelpER", false);
            rk2.Close();//ɾ��ע���

            Directory.Delete("C:\\Program Files\\SMB HelpER",true);//ɾ�������ļ��ļ���
        }

        private void SystemShareOut_Click(object sender, EventArgs e)//����������ð�ť
        {
            FolderBrowserDialog dilog = new FolderBrowserDialog();//��ѡ��洢�ļ���
            dilog.Description = "��ѡ���ļ���";
            if (dilog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string pSaveFilePath = dilog.SelectedPath + "\\SMB_HelpER_Backup_Q615749669.zip";

            try//�ȳ���ɾ����ֹ����ѹ���ļ�ʱ����
            {
                File.Delete(pSaveFilePath);
            }
            catch (Exception ex)
            {

            }
            ZipFile.CreateFromDirectory("C:\\Program Files\\SMB HelpER", pSaveFilePath);
        }

        private void SystemShareIn_Click(object sender, EventArgs e)//������������ļ���ť
        {
            OpenFileDialog fileDialog = new OpenFileDialog();//���ļ�
            fileDialog.Multiselect = false;//��ѡ
            fileDialog.Title = "��ѡ���ļ�";
            fileDialog.Filter = "zip�ļ�|SMB_HelpER_Backup_Q615749669.zip";
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
            ////////��ѹ����ʼ
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
                //MessageBox.Show("������û����ʾ");
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
                //MessageBox.Show("������û����ʾ");
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
                //MessageBox.Show("������û����ʾ");
            }
            tabControl1.SelectedTab = Smb;
            SmbDisconnect.PerformClick();
            this.Hide();
        }

        //////////////////////////////////////////////////////////////////////////

    }
}