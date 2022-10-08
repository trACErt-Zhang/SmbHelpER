namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.VpnName = new System.Windows.Forms.TextBox();
            this.VpnAccount = new System.Windows.Forms.TextBox();
            this.VpnPassword = new System.Windows.Forms.TextBox();
            this.VpnOnlyPrivateLink = new System.Windows.Forms.CheckBox();
            this.VpnAllLink = new System.Windows.Forms.CheckBox();
            this.SmbUrl = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Vpn = new System.Windows.Forms.TabPage();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.VpnNet = new System.Windows.Forms.RichTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.VpnNetGate = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.VpnSave = new System.Windows.Forms.Button();
            this.VpnConnect = new System.Windows.Forms.Button();
            this.Smb = new System.Windows.Forms.TabPage();
            this.SmbDisconnect = new System.Windows.Forms.Button();
            this.SmbConnect = new System.Windows.Forms.Button();
            this.SmbSave = new System.Windows.Forms.Button();
            this.Software = new System.Windows.Forms.TabPage();
            this.SystemShareIn = new System.Windows.Forms.Button();
            this.SystemShareOut = new System.Windows.Forms.Button();
            this.SystemReset = new System.Windows.Forms.Button();
            this.SystemClear = new System.Windows.Forms.Button();
            this.SystemStartWithWindows = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SystemSave = new System.Windows.Forms.Button();
            this.SystemSmbAutoDisconnect = new System.Windows.Forms.CheckBox();
            this.SystemSmbAutoConnect = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SystemVpnAutoDisconnect = new System.Windows.Forms.CheckBox();
            this.SystemVpnAutoConnect = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Dis_ConnectVpn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ConnectSmb = new System.Windows.Forms.ToolStripMenuItem();
            this.DisconnectSmb = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Show = new System.Windows.Forms.ToolStripMenuItem();
            this.Close = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.Vpn.SuspendLayout();
            this.Smb.SuspendLayout();
            this.Software.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "vpn连接名称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(144, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "vpn账号：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(144, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "vpn密码：";
            // 
            // VpnName
            // 
            this.VpnName.Location = new System.Drawing.Point(226, 11);
            this.VpnName.Name = "VpnName";
            this.VpnName.Size = new System.Drawing.Size(100, 23);
            this.VpnName.TabIndex = 3;
            // 
            // VpnAccount
            // 
            this.VpnAccount.Location = new System.Drawing.Point(226, 40);
            this.VpnAccount.Name = "VpnAccount";
            this.VpnAccount.Size = new System.Drawing.Size(100, 23);
            this.VpnAccount.TabIndex = 4;
            // 
            // VpnPassword
            // 
            this.VpnPassword.Location = new System.Drawing.Point(226, 69);
            this.VpnPassword.Name = "VpnPassword";
            this.VpnPassword.Size = new System.Drawing.Size(100, 23);
            this.VpnPassword.TabIndex = 5;
            // 
            // VpnOnlyPrivateLink
            // 
            this.VpnOnlyPrivateLink.AutoSize = true;
            this.VpnOnlyPrivateLink.Checked = true;
            this.VpnOnlyPrivateLink.CheckState = System.Windows.Forms.CheckState.Checked;
            this.VpnOnlyPrivateLink.Location = new System.Drawing.Point(144, 98);
            this.VpnOnlyPrivateLink.Name = "VpnOnlyPrivateLink";
            this.VpnOnlyPrivateLink.Size = new System.Drawing.Size(148, 21);
            this.VpnOnlyPrivateLink.TabIndex = 6;
            this.VpnOnlyPrivateLink.Text = "仅VPN网段和特定流量";
            this.VpnOnlyPrivateLink.UseVisualStyleBackColor = true;
            this.VpnOnlyPrivateLink.CheckedChanged += new System.EventHandler(this.VpnOnlyPrivateLink_CheckedChanged);
            // 
            // VpnAllLink
            // 
            this.VpnAllLink.AutoSize = true;
            this.VpnAllLink.Location = new System.Drawing.Point(30, 98);
            this.VpnAllLink.Name = "VpnAllLink";
            this.VpnAllLink.Size = new System.Drawing.Size(75, 21);
            this.VpnAllLink.TabIndex = 7;
            this.VpnAllLink.Text = "全部流量";
            this.VpnAllLink.UseVisualStyleBackColor = true;
            this.VpnAllLink.CheckedChanged += new System.EventHandler(this.VpnAllLink_CheckedChanged);
            // 
            // SmbUrl
            // 
            this.SmbUrl.Location = new System.Drawing.Point(6, 142);
            this.SmbUrl.Name = "SmbUrl";
            this.SmbUrl.Size = new System.Drawing.Size(320, 150);
            this.SmbUrl.TabIndex = 8;
            this.SmbUrl.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(308, 136);
            this.label4.TabIndex = 9;
            this.label4.Text = "SMB URL （回车分隔 英文冒号）\r\n【分配盘符】【空格】【URL】\r\n例：z: \\\\192.168.1.4\\share\r\n-----------------" +
    "-----------------------\r\n注意：使用此软件前请先在计算机内映射一次SMB，以\r\n保存用户名和密码。Windows映射SMB同时只能使用一" +
    "\r\n个账户，请确保下方列表中使用的账户均是之前在计算机\r\n中连接过的账户。";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Vpn);
            this.tabControl1.Controls.Add(this.Smb);
            this.tabControl1.Controls.Add(this.Software);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(340, 357);
            this.tabControl1.TabIndex = 10;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // Vpn
            // 
            this.Vpn.Controls.Add(this.linkLabel1);
            this.Vpn.Controls.Add(this.VpnNet);
            this.Vpn.Controls.Add(this.label12);
            this.Vpn.Controls.Add(this.VpnNetGate);
            this.Vpn.Controls.Add(this.label11);
            this.Vpn.Controls.Add(this.VpnSave);
            this.Vpn.Controls.Add(this.VpnConnect);
            this.Vpn.Controls.Add(this.label1);
            this.Vpn.Controls.Add(this.VpnName);
            this.Vpn.Controls.Add(this.label2);
            this.Vpn.Controls.Add(this.VpnAllLink);
            this.Vpn.Controls.Add(this.label3);
            this.Vpn.Controls.Add(this.VpnOnlyPrivateLink);
            this.Vpn.Controls.Add(this.VpnAccount);
            this.Vpn.Controls.Add(this.VpnPassword);
            this.Vpn.Location = new System.Drawing.Point(4, 26);
            this.Vpn.Name = "Vpn";
            this.Vpn.Padding = new System.Windows.Forms.Padding(3);
            this.Vpn.Size = new System.Drawing.Size(332, 327);
            this.Vpn.TabIndex = 0;
            this.Vpn.Text = "VPN";
            this.Vpn.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(243, 155);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(20, 17);
            this.linkLabel1.TabIndex = 15;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "例";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // VpnNet
            // 
            this.VpnNet.Location = new System.Drawing.Point(6, 177);
            this.VpnNet.Name = "VpnNet";
            this.VpnNet.Size = new System.Drawing.Size(320, 115);
            this.VpnNet.TabIndex = 14;
            this.VpnNet.Text = "";
            this.VpnNet.WordWrap = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 155);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(233, 17);
            this.label12.TabIndex = 13;
            this.label12.Text = "路由设置：网段ip/掩码/网关（回车分隔）";
            // 
            // VpnNetGate
            // 
            this.VpnNetGate.Location = new System.Drawing.Point(226, 123);
            this.VpnNetGate.Name = "VpnNetGate";
            this.VpnNetGate.Size = new System.Drawing.Size(100, 23);
            this.VpnNetGate.TabIndex = 12;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(30, 126);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(179, 17);
            this.label11.TabIndex = 11;
            this.label11.Text = "VPN网关，也是ping检测网关：";
            // 
            // VpnSave
            // 
            this.VpnSave.Location = new System.Drawing.Point(6, 298);
            this.VpnSave.Name = "VpnSave";
            this.VpnSave.Size = new System.Drawing.Size(94, 23);
            this.VpnSave.TabIndex = 10;
            this.VpnSave.Text = "保存";
            this.VpnSave.UseVisualStyleBackColor = true;
            this.VpnSave.Click += new System.EventHandler(this.VpnSave_Click);
            // 
            // VpnConnect
            // 
            this.VpnConnect.Location = new System.Drawing.Point(232, 298);
            this.VpnConnect.Name = "VpnConnect";
            this.VpnConnect.Size = new System.Drawing.Size(94, 23);
            this.VpnConnect.TabIndex = 9;
            this.VpnConnect.Text = "连接";
            this.VpnConnect.UseVisualStyleBackColor = true;
            this.VpnConnect.Click += new System.EventHandler(this.VpnConnect_Click);
            // 
            // Smb
            // 
            this.Smb.Controls.Add(this.SmbDisconnect);
            this.Smb.Controls.Add(this.SmbConnect);
            this.Smb.Controls.Add(this.SmbSave);
            this.Smb.Controls.Add(this.label4);
            this.Smb.Controls.Add(this.SmbUrl);
            this.Smb.Location = new System.Drawing.Point(4, 26);
            this.Smb.Name = "Smb";
            this.Smb.Padding = new System.Windows.Forms.Padding(3);
            this.Smb.Size = new System.Drawing.Size(332, 327);
            this.Smb.TabIndex = 1;
            this.Smb.Text = "SMB";
            this.Smb.UseVisualStyleBackColor = true;
            // 
            // SmbDisconnect
            // 
            this.SmbDisconnect.Location = new System.Drawing.Point(132, 298);
            this.SmbDisconnect.Name = "SmbDisconnect";
            this.SmbDisconnect.Size = new System.Drawing.Size(94, 23);
            this.SmbDisconnect.TabIndex = 12;
            this.SmbDisconnect.Text = "断开";
            this.SmbDisconnect.UseVisualStyleBackColor = true;
            this.SmbDisconnect.Click += new System.EventHandler(this.SmbDisconnect_Click);
            // 
            // SmbConnect
            // 
            this.SmbConnect.Location = new System.Drawing.Point(232, 298);
            this.SmbConnect.Name = "SmbConnect";
            this.SmbConnect.Size = new System.Drawing.Size(94, 23);
            this.SmbConnect.TabIndex = 11;
            this.SmbConnect.Text = "连接";
            this.SmbConnect.UseVisualStyleBackColor = true;
            this.SmbConnect.Click += new System.EventHandler(this.SmbConnect_Click);
            // 
            // SmbSave
            // 
            this.SmbSave.Location = new System.Drawing.Point(6, 298);
            this.SmbSave.Name = "SmbSave";
            this.SmbSave.Size = new System.Drawing.Size(94, 23);
            this.SmbSave.TabIndex = 10;
            this.SmbSave.Text = "保存";
            this.SmbSave.UseVisualStyleBackColor = true;
            this.SmbSave.Click += new System.EventHandler(this.SmbSave_Click);
            // 
            // Software
            // 
            this.Software.Controls.Add(this.SystemShareIn);
            this.Software.Controls.Add(this.SystemShareOut);
            this.Software.Controls.Add(this.SystemReset);
            this.Software.Controls.Add(this.SystemClear);
            this.Software.Controls.Add(this.SystemStartWithWindows);
            this.Software.Controls.Add(this.label10);
            this.Software.Controls.Add(this.SystemSave);
            this.Software.Controls.Add(this.SystemSmbAutoDisconnect);
            this.Software.Controls.Add(this.SystemSmbAutoConnect);
            this.Software.Controls.Add(this.label9);
            this.Software.Controls.Add(this.SystemVpnAutoDisconnect);
            this.Software.Controls.Add(this.SystemVpnAutoConnect);
            this.Software.Controls.Add(this.label8);
            this.Software.Controls.Add(this.label7);
            this.Software.Location = new System.Drawing.Point(4, 26);
            this.Software.Name = "Software";
            this.Software.Size = new System.Drawing.Size(332, 327);
            this.Software.TabIndex = 2;
            this.Software.Text = "说明与系统设置";
            this.Software.UseVisualStyleBackColor = true;
            // 
            // SystemShareIn
            // 
            this.SystemShareIn.Location = new System.Drawing.Point(103, 301);
            this.SystemShareIn.Name = "SystemShareIn";
            this.SystemShareIn.Size = new System.Drawing.Size(94, 23);
            this.SystemShareIn.TabIndex = 13;
            this.SystemShareIn.Text = "导入 配置文件";
            this.SystemShareIn.UseVisualStyleBackColor = true;
            this.SystemShareIn.Click += new System.EventHandler(this.SystemShareIn_Click);
            // 
            // SystemShareOut
            // 
            this.SystemShareOut.Location = new System.Drawing.Point(3, 301);
            this.SystemShareOut.Name = "SystemShareOut";
            this.SystemShareOut.Size = new System.Drawing.Size(94, 23);
            this.SystemShareOut.TabIndex = 12;
            this.SystemShareOut.Text = "导出 配置文件";
            this.SystemShareOut.UseVisualStyleBackColor = true;
            this.SystemShareOut.Click += new System.EventHandler(this.SystemShareOut_Click);
            // 
            // SystemReset
            // 
            this.SystemReset.Location = new System.Drawing.Point(235, 177);
            this.SystemReset.Name = "SystemReset";
            this.SystemReset.Size = new System.Drawing.Size(94, 23);
            this.SystemReset.TabIndex = 11;
            this.SystemReset.Text = "恢复设置";
            this.SystemReset.UseVisualStyleBackColor = true;
            this.SystemReset.Click += new System.EventHandler(this.SystemReset_Click);
            // 
            // SystemClear
            // 
            this.SystemClear.Location = new System.Drawing.Point(3, 177);
            this.SystemClear.Name = "SystemClear";
            this.SystemClear.Size = new System.Drawing.Size(197, 23);
            this.SystemClear.TabIndex = 10;
            this.SystemClear.Text = "清除配置文件和注册表并关闭软件";
            this.SystemClear.UseVisualStyleBackColor = true;
            this.SystemClear.Click += new System.EventHandler(this.SystemClear_Click);
            // 
            // SystemStartWithWindows
            // 
            this.SystemStartWithWindows.AutoSize = true;
            this.SystemStartWithWindows.Location = new System.Drawing.Point(56, 267);
            this.SystemStartWithWindows.Name = "SystemStartWithWindows";
            this.SystemStartWithWindows.Size = new System.Drawing.Size(99, 21);
            this.SystemStartWithWindows.TabIndex = 9;
            this.SystemStartWithWindows.Text = "开机自动启动";
            this.SystemStartWithWindows.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 268);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 17);
            this.label10.TabIndex = 8;
            this.label10.Text = "软件：";
            // 
            // SystemSave
            // 
            this.SystemSave.Location = new System.Drawing.Point(235, 301);
            this.SystemSave.Name = "SystemSave";
            this.SystemSave.Size = new System.Drawing.Size(94, 23);
            this.SystemSave.TabIndex = 7;
            this.SystemSave.Text = "保存";
            this.SystemSave.UseVisualStyleBackColor = true;
            this.SystemSave.Click += new System.EventHandler(this.SystemSave_Click);
            // 
            // SystemSmbAutoDisconnect
            // 
            this.SystemSmbAutoDisconnect.AutoSize = true;
            this.SystemSmbAutoDisconnect.Checked = true;
            this.SystemSmbAutoDisconnect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SystemSmbAutoDisconnect.Location = new System.Drawing.Point(196, 240);
            this.SystemSmbAutoDisconnect.Name = "SystemSmbAutoDisconnect";
            this.SystemSmbAutoDisconnect.Size = new System.Drawing.Size(123, 21);
            this.SystemSmbAutoDisconnect.TabIndex = 6;
            this.SystemSmbAutoDisconnect.Text = "关闭软件自动断开";
            this.SystemSmbAutoDisconnect.UseVisualStyleBackColor = true;
            // 
            // SystemSmbAutoConnect
            // 
            this.SystemSmbAutoConnect.AutoSize = true;
            this.SystemSmbAutoConnect.Location = new System.Drawing.Point(56, 240);
            this.SystemSmbAutoConnect.Name = "SystemSmbAutoConnect";
            this.SystemSmbAutoConnect.Size = new System.Drawing.Size(113, 21);
            this.SystemSmbAutoConnect.TabIndex = 5;
            this.SystemSmbAutoConnect.Text = "ping通自动连接";
            this.SystemSmbAutoConnect.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 241);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 17);
            this.label9.TabIndex = 4;
            this.label9.Text = "SMB：";
            // 
            // SystemVpnAutoDisconnect
            // 
            this.SystemVpnAutoDisconnect.AutoSize = true;
            this.SystemVpnAutoDisconnect.Checked = true;
            this.SystemVpnAutoDisconnect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SystemVpnAutoDisconnect.Location = new System.Drawing.Point(196, 213);
            this.SystemVpnAutoDisconnect.Name = "SystemVpnAutoDisconnect";
            this.SystemVpnAutoDisconnect.Size = new System.Drawing.Size(123, 21);
            this.SystemVpnAutoDisconnect.TabIndex = 3;
            this.SystemVpnAutoDisconnect.Text = "关闭软件自动断开";
            this.SystemVpnAutoDisconnect.UseVisualStyleBackColor = true;
            // 
            // SystemVpnAutoConnect
            // 
            this.SystemVpnAutoConnect.AutoSize = true;
            this.SystemVpnAutoConnect.Location = new System.Drawing.Point(56, 213);
            this.SystemVpnAutoConnect.Name = "SystemVpnAutoConnect";
            this.SystemVpnAutoConnect.Size = new System.Drawing.Size(123, 21);
            this.SystemVpnAutoConnect.TabIndex = 2;
            this.SystemVpnAutoConnect.Text = "打开软件自动连接";
            this.SystemVpnAutoConnect.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 214);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "VPN：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(323, 153);
            this.label7.TabIndex = 0;
            this.label7.Text = "说明：\r\n此软件不是代理软件。此软件的功能是自动打开和关闭Win-\r\ndows内置的vpn连接，自动连接和断开映射的smb网络驱\r\n动器。\r\n\r\n使用：\r\n1. " +
    "使用Windows内置VPN创建连接并能连接上。\r\n2. 控制面板-网络连接-右键vpn连接-属性-网络-\r\n双击ipv4-高级-取消勾选【在远程网络上使用默认" +
    "网关】";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "SMB HelpER";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Dis_ConnectVpn,
            this.toolStripSeparator2,
            this.ConnectSmb,
            this.DisconnectSmb,
            this.toolStripSeparator1,
            this.Show,
            this.Close});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(167, 126);
            // 
            // Dis_ConnectVpn
            // 
            this.Dis_ConnectVpn.Name = "Dis_ConnectVpn";
            this.Dis_ConnectVpn.Size = new System.Drawing.Size(166, 22);
            this.Dis_ConnectVpn.Text = "连接 / 断开 VPN";
            this.Dis_ConnectVpn.Click += new System.EventHandler(this.Dis_ConnectVpn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(163, 6);
            // 
            // ConnectSmb
            // 
            this.ConnectSmb.Name = "ConnectSmb";
            this.ConnectSmb.Size = new System.Drawing.Size(166, 22);
            this.ConnectSmb.Text = "连接 SMB";
            this.ConnectSmb.Click += new System.EventHandler(this.ConnectSmb_Click);
            // 
            // DisconnectSmb
            // 
            this.DisconnectSmb.Name = "DisconnectSmb";
            this.DisconnectSmb.Size = new System.Drawing.Size(166, 22);
            this.DisconnectSmb.Text = "断开 SMB";
            this.DisconnectSmb.Click += new System.EventHandler(this.DisconnectSmb_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // Show
            // 
            this.Show.Name = "Show";
            this.Show.Size = new System.Drawing.Size(166, 22);
            this.Show.Text = "主界面";
            this.Show.Click += new System.EventHandler(this.Show_Click);
            // 
            // Close
            // 
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(166, 22);
            this.Close.Text = "退出";
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 381);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "SMB HelpER q615749669 v1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown_1);
            this.tabControl1.ResumeLayout(false);
            this.Vpn.ResumeLayout(false);
            this.Vpn.PerformLayout();
            this.Smb.ResumeLayout(false);
            this.Smb.PerformLayout();
            this.Software.ResumeLayout(false);
            this.Software.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox VpnAccount;
        private TextBox VpnPassword;
        private CheckBox VpnOnlyPrivateLink;
        private CheckBox VpnAllLink;
        private RichTextBox SmbUrl;
        private Label label4;
        private TabPage Vpn;
        private Button VpnSave;
        private TabPage Smb;
        private TabPage Software;
        private Button SmbConnect;
        private Button SmbSave;
        private Button SystemReset;
        private Button SystemClear;
        private CheckBox SystemStartWithWindows;
        private Label label10;
        private Button SystemSave;
        private CheckBox SystemSmbAutoDisconnect;
        private CheckBox SystemSmbAutoConnect;
        private Label label9;
        private CheckBox SystemVpnAutoDisconnect;
        private CheckBox SystemVpnAutoConnect;
        private Label label8;
        private Label label7;
        private TextBox VpnNetGate;
        private Label label11;
        private RichTextBox VpnNet;
        private Label label12;
        private NotifyIcon notifyIcon1;
        private TextBox VpnName;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem ConnectSmb;
        private ToolStripMenuItem DisconnectSmb;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem Show;
        private ToolStripMenuItem Close;
        public Button VpnConnect;
        private LinkLabel linkLabel1;
        private Button SmbDisconnect;
        private TabControl tabControl1;
        private Button SystemShareOut;
        private Button SystemShareIn;
        private ToolStripMenuItem Dis_ConnectVpn;
        private ToolStripSeparator toolStripSeparator2;
    }
}