namespace SmartEye_Demo
{
    partial class MainWinForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.login_btn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPsw = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxUsrName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxIp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonCapturePath = new System.Windows.Forms.Button();
            this.buttonRecordPath = new System.Windows.Forms.Button();
            this.labelCapturePath = new System.Windows.Forms.Label();
            this.labelRecordPath = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.listViewGPSData = new System.Windows.Forms.ListView();
            this.columnHeaderPuName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLng = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbInputMsg = new System.Windows.Forms.TextBox();
            this.treeViewResList = new System.Windows.Forms.TreeView();
            this.panelVideo = new System.Windows.Forms.Panel();
            this.contextMenuStripVideo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemClosePreview = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.contextMenuStripTalkOnly = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemTalkOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.panelLeft.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.contextMenuStripVideo.SuspendLayout();
            this.contextMenuStripTalkOnly.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.tabControl);
            this.panelLeft.Controls.Add(this.treeViewResList);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(268, 509);
            this.panelLeft.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage4);
            this.tabControl.Location = new System.Drawing.Point(3, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(262, 275);
            this.tabControl.TabIndex = 5;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.login_btn);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.textBoxPsw);
            this.tabPage2.Controls.Add(this.textBoxPort);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.textBoxUsrName);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.textBoxIp);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(254, 249);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "登录";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // login_btn
            // 
            this.login_btn.Location = new System.Drawing.Point(44, 154);
            this.login_btn.Name = "login_btn";
            this.login_btn.Size = new System.Drawing.Size(75, 23);
            this.login_btn.TabIndex = 16;
            this.login_btn.Text = "登录";
            this.login_btn.UseVisualStyleBackColor = true;
            this.login_btn.Click += new System.EventHandler(this.login_btn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "密码";
            // 
            // textBoxPsw
            // 
            this.textBoxPsw.Location = new System.Drawing.Point(54, 73);
            this.textBoxPsw.Name = "textBoxPsw";
            this.textBoxPsw.PasswordChar = '*';
            this.textBoxPsw.Size = new System.Drawing.Size(100, 21);
            this.textBoxPsw.TabIndex = 14;
            this.textBoxPsw.Text = "besovideo";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(54, 107);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(51, 21);
            this.textBoxPort.TabIndex = 13;
            this.textBoxPort.Text = "9701";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "Port";
            // 
            // textBoxUsrName
            // 
            this.textBoxUsrName.Location = new System.Drawing.Point(54, 46);
            this.textBoxUsrName.Name = "textBoxUsrName";
            this.textBoxUsrName.Size = new System.Drawing.Size(100, 21);
            this.textBoxUsrName.TabIndex = 11;
            this.textBoxUsrName.Text = "besovideo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "用户名";
            // 
            // textBoxIp
            // 
            this.textBoxIp.Location = new System.Drawing.Point(54, 19);
            this.textBoxIp.Name = "textBoxIp";
            this.textBoxIp.Size = new System.Drawing.Size(100, 21);
            this.textBoxIp.TabIndex = 9;
            this.textBoxIp.Text = "61.191.27.18";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "IP";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonCapturePath);
            this.tabPage1.Controls.Add(this.buttonRecordPath);
            this.tabPage1.Controls.Add(this.labelCapturePath);
            this.tabPage1.Controls.Add(this.labelRecordPath);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(254, 249);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "常规";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonCapturePath
            // 
            this.buttonCapturePath.Location = new System.Drawing.Point(85, 31);
            this.buttonCapturePath.Name = "buttonCapturePath";
            this.buttonCapturePath.Size = new System.Drawing.Size(83, 23);
            this.buttonCapturePath.TabIndex = 2;
            this.buttonCapturePath.Text = "截图路径...";
            this.buttonCapturePath.UseVisualStyleBackColor = true;
            this.buttonCapturePath.Click += new System.EventHandler(this.buttonCapturePath_Click);
            // 
            // buttonRecordPath
            // 
            this.buttonRecordPath.Location = new System.Drawing.Point(85, 77);
            this.buttonRecordPath.Name = "buttonRecordPath";
            this.buttonRecordPath.Size = new System.Drawing.Size(83, 23);
            this.buttonRecordPath.TabIndex = 1;
            this.buttonRecordPath.Text = "录像路径...";
            this.buttonRecordPath.UseVisualStyleBackColor = true;
            this.buttonRecordPath.Click += new System.EventHandler(this.buttonRecordPath_Click);
            // 
            // labelCapturePath
            // 
            this.labelCapturePath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCapturePath.Location = new System.Drawing.Point(6, 10);
            this.labelCapturePath.Name = "labelCapturePath";
            this.labelCapturePath.Size = new System.Drawing.Size(162, 18);
            this.labelCapturePath.TabIndex = 3;
            // 
            // labelRecordPath
            // 
            this.labelRecordPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelRecordPath.Location = new System.Drawing.Point(6, 57);
            this.labelRecordPath.Name = "labelRecordPath";
            this.labelRecordPath.Size = new System.Drawing.Size(162, 17);
            this.labelRecordPath.TabIndex = 4;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnSendMsg);
            this.tabPage4.Controls.Add(this.listViewGPSData);
            this.tabPage4.Controls.Add(this.tbInputMsg);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(254, 249);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "GPS/串口";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Enabled = false;
            this.btnSendMsg.Location = new System.Drawing.Point(199, 221);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(52, 23);
            this.btnSendMsg.TabIndex = 0;
            this.btnSendMsg.Text = "send";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // listViewGPSData
            // 
            this.listViewGPSData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewGPSData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPuName,
            this.columnHeaderLat,
            this.columnHeaderLng});
            this.listViewGPSData.FullRowSelect = true;
            this.listViewGPSData.GridLines = true;
            this.listViewGPSData.Location = new System.Drawing.Point(0, 0);
            this.listViewGPSData.Name = "listViewGPSData";
            this.listViewGPSData.Size = new System.Drawing.Size(254, 170);
            this.listViewGPSData.TabIndex = 0;
            this.listViewGPSData.UseCompatibleStateImageBehavior = false;
            this.listViewGPSData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderPuName
            // 
            this.columnHeaderPuName.Text = "设备名";
            this.columnHeaderPuName.Width = 68;
            // 
            // columnHeaderLat
            // 
            this.columnHeaderLat.Text = "Tsp";
            this.columnHeaderLat.Width = 140;
            // 
            // columnHeaderLng
            // 
            this.columnHeaderLng.Text = "Len";
            this.columnHeaderLng.Width = 92;
            // 
            // tbInputMsg
            // 
            this.tbInputMsg.Location = new System.Drawing.Point(3, 176);
            this.tbInputMsg.Multiline = true;
            this.tbInputMsg.Name = "tbInputMsg";
            this.tbInputMsg.Size = new System.Drawing.Size(190, 68);
            this.tbInputMsg.TabIndex = 0;
            // 
            // treeViewResList
            // 
            this.treeViewResList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewResList.ItemHeight = 16;
            this.treeViewResList.Location = new System.Drawing.Point(3, 293);
            this.treeViewResList.Name = "treeViewResList";
            this.treeViewResList.Size = new System.Drawing.Size(262, 213);
            this.treeViewResList.TabIndex = 0;
            this.treeViewResList.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewResList_NodeMouseClick);
            this.treeViewResList.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewResList_NodeMouseDoubleClick);
            // 
            // panelVideo
            // 
            this.panelVideo.BackColor = System.Drawing.Color.Black;
            this.panelVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelVideo.Location = new System.Drawing.Point(268, 0);
            this.panelVideo.Name = "panelVideo";
            this.panelVideo.Size = new System.Drawing.Size(523, 509);
            this.panelVideo.TabIndex = 1;
            // 
            // contextMenuStripVideo
            // 
            this.contextMenuStripVideo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemClosePreview,
            this.toolStripMenuItemSnapshot,
            this.toolStripMenuItemRecord});
            this.contextMenuStripVideo.Name = "contextMenuStripVideo";
            this.contextMenuStripVideo.Size = new System.Drawing.Size(125, 70);
            // 
            // toolStripMenuItemClosePreview
            // 
            this.toolStripMenuItemClosePreview.Name = "toolStripMenuItemClosePreview";
            this.toolStripMenuItemClosePreview.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemClosePreview.Text = "关闭预览";
            this.toolStripMenuItemClosePreview.Click += new System.EventHandler(this.toolStripMenuItemClosePreview_Click);
            // 
            // toolStripMenuItemSnapshot
            // 
            this.toolStripMenuItemSnapshot.Name = "toolStripMenuItemSnapshot";
            this.toolStripMenuItemSnapshot.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemSnapshot.Text = "截图";
            this.toolStripMenuItemSnapshot.Click += new System.EventHandler(this.toolStripMenuItemSnapshot_Click);
            // 
            // toolStripMenuItemRecord
            // 
            this.toolStripMenuItemRecord.Name = "toolStripMenuItemRecord";
            this.toolStripMenuItemRecord.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemRecord.Text = "录像";
            this.toolStripMenuItemRecord.Click += new System.EventHandler(this.toolStripMenuItemRecord_Click);
            // 
            // contextMenuStripTalkOnly
            // 
            this.contextMenuStripTalkOnly.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemTalkOnly});
            this.contextMenuStripTalkOnly.Name = "contextMenuStripTalkOnly";
            this.contextMenuStripTalkOnly.Size = new System.Drawing.Size(125, 26);
            // 
            // ToolStripMenuItemTalkOnly
            // 
            this.ToolStripMenuItemTalkOnly.Name = "ToolStripMenuItemTalkOnly";
            this.ToolStripMenuItemTalkOnly.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItemTalkOnly.Text = "打开对讲";
            this.ToolStripMenuItemTalkOnly.Click += new System.EventHandler(this.ToolStripMenuItemTalkOnly_Click);
            // 
            // MainWinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 509);
            this.Controls.Add(this.panelVideo);
            this.Controls.Add(this.panelLeft);
            this.Name = "MainWinForm";
            this.Text = "WinFormDemo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panelLeft.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.contextMenuStripVideo.ResumeLayout(false);
            this.contextMenuStripTalkOnly.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelVideo;
        private System.Windows.Forms.TreeView treeViewResList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripVideo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemClosePreview;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSnapshot;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRecord;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTalkOnly;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemTalkOnly;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button buttonCapturePath;
        private System.Windows.Forms.Button buttonRecordPath;
        private System.Windows.Forms.Label labelCapturePath;
        private System.Windows.Forms.Label labelRecordPath;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.ListView listViewGPSData;
        private System.Windows.Forms.ColumnHeader columnHeaderPuName;
        private System.Windows.Forms.ColumnHeader columnHeaderLat;
        private System.Windows.Forms.ColumnHeader columnHeaderLng;
        private System.Windows.Forms.TextBox tbInputMsg;
        private System.Windows.Forms.Button login_btn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPsw;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxUsrName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxIp;
        private System.Windows.Forms.Label label1;
    }
}

