using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Timers;
using System.Threading;
using System.Collections.Generic;

namespace SmartEye_Demo
{
    public partial class MainWinForm : Form
    {
        #region 属性[部分]
        /// <summary>
        /// 视频浏览panel
        /// </summary>
        ArrayList m_videoPanels;
        private MySqlConnection conn;
        /// <summary>
        /// SDK
        /// </summary>
        public BVCUSdkOperator m_sdkOperator;
        public System.Timers.Timer sndData_timer;
        public Websocket_Rec websocket_Rec;

        //存储pu对应的传感器数据
        Dictionary<string, Queue<short[]>> pu_rsDatas;
        short[] rsdata_Now;
        Queue<short[]> RSdatas_Now;

        #endregion 属性[部分]


        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWinForm()
        {
            m_videoPanels = new ArrayList();

            //存储pu对应的传感器数据
            pu_rsDatas = new Dictionary<string, Queue<short[]>>();

            InitializeComponent();

            devideScreen();

            

            m_sdkOperator = new BVCUSdkOperator(this);

            //Websocket服务器监听,并传递传感器数据和dialog
            websocket_Rec = new Websocket_Rec(pu_rsDatas,m_sdkOperator.Dialog);

            m_getPuList = new GetPuListDel(procGetPuList);//设置获得设备列表后的响应,初始化treeview

            m_capturePath = "";
            m_recordPath = "";

            m_activePanelBorder = new RectBorder(panelVideo, Color.Red);
            Panel panel = m_videoPanels[0] as Panel;
            m_activePanelBorder.show(panel.Location, panel.Width, panel.Height, ACTIVE_PANEL_BORDER_WIDTH);
            m_activePanel = panel;
            RecordPath = "E:\\PIPE_DATA\\TEST";

            //定时器
            sndData_timer = new System.Timers.Timer(1000);
            sndData_timer.Elapsed += new ElapsedEventHandler(sndMsg_TSP);
            sndData_timer.AutoReset = true;
            sndData_timer.Enabled = false;

            
            //连接数据库
            if (conn != null)
                conn.Close();

            string connStr = "server=127.0.0.1;user id=root; password=TBG244; database=controlbox; pooling=false";
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error connecting to the server: " + ex.Message);
            }
            /*test
            gsSrData = new gsData();
            gsSrData.CH4 = 23;
            gsSrData.CO = 24;
            gsSrData.CO2 = 25;
            gsSrData.H2S = 26;
            gsSrData.NH3 = 27;
            gsSrData.O2 = 28;

            string sql1 = string.Format(@"insert into gassensordata(DataTime,CO,CO2,H2S,NH3) values('{0}', '{1}', '{2}', '{3}', '{4}')", DateTime.Now, gsSrData.CO, gsSrData.CO2, gsSrData.CO2, gsSrData.NH3);
            MySqlCommand mycmd = new MySqlCommand(sql1, conn);
            mycmd.ExecuteNonQuery(); 
             * */
        }


       


        #region 浏览窗口绘制相关

        /// <summary>
        /// 划分窗口为2X2的4窗口结构
        /// </summary>
        const int VIDEO_PANEL_COUNT = 4;

        /// <summary>
        /// 划分窗口为2X2的4窗口结构
        /// </summary>
        void devideScreen()
        {
            for (int i = 0; i < VIDEO_PANEL_COUNT; i++)
            {
                Panel panel = new Panel();
                panel.BorderStyle = BorderStyle.Fixed3D;
                panel.Click += new System.EventHandler(panel_Click);
                panel.MouseUp += new MouseEventHandler(panel_MouseUp);
                m_videoPanels.Add(panel);
                panelVideo.Controls.Add(panel);
           


            }
            locateResizeVideoPanel();
        }

        /// <summary>
        /// 确定位置
        /// </summary>
        void locateResizeVideoPanel()
        {
            for (int i = 0; i < VIDEO_PANEL_COUNT; i++)
            {
                Panel panel = m_videoPanels[i] as Panel;
                panel.Width = panelVideo.Width / 2;
                panel.Height = panelVideo.Height / 2;
                switch (i)
                {
                    case 0:
                        panel.Location = new Point(0, 0);
                        break;
                    case 1:
                        panel.Location = new Point(panelVideo.Width / 2, 0);
                        break;
                    case 2:
                        panel.Location = new Point(0, panelVideo.Height / 2);
                        break;
                    case 3:
                        panel.Location = new Point(panelVideo.Width / 2, panelVideo.Height / 2);
                        break;
                }
            }
        }

        /// <summary>
        /// 绘制鼠标选中的当前窗口边框
        /// </summary>
        RectBorder m_activePanelBorder;

        const int ACTIVE_PANEL_BORDER_WIDTH = 3;//边框的宽度
        Panel m_activePanel;

        /// <summary>
        /// 响应小视屏窗口点击事件, 引用位置：this.devideScreen()
        /// </summary>
        void panel_Click(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            m_activePanelBorder.show(panel.Location, panel.Width, panel.Height, ACTIVE_PANEL_BORDER_WIDTH);
            m_sdkOperator.Dialog.setVolume(m_activePanel, 0);
            m_activePanel = panel;
            m_sdkOperator.Dialog.setVolume(m_activePanel, 50);
        }

        /// <summary>
        /// 猜测：小视屏窗口的红线绘制，引用位置：m_activePanelBorder = new RectBorder(panelVideo, Color.Red);
        /// </summary>
        class RectBorder
        {
            PictureBox[] lines;
            public RectBorder(Control parent, Color color)
            {
                lines = new PictureBox[4];
                for (int i = 0; i < 4; i++)
                {
                    lines[i] = new PictureBox();
                    lines[i].BackColor = color;
                    parent.Controls.Add(lines[i]);
                    lines[i].BringToFront();
                }
            }
            public void show(Point location, int width, int height, int lineWidth)
            {
                lines[0].Height = lineWidth;
                lines[0].Width = width;
                lines[0].Location = location;
                lines[1].Height = height;
                lines[1].Width = lineWidth;
                lines[1].Location = new Point(location.X + width - lineWidth, location.Y);
                lines[2].Height = lineWidth;
                lines[2].Width = width;
                lines[2].Location = new Point(location.X, location.Y + height - lineWidth);
                lines[3].Height = height;
                lines[3].Width = lineWidth;
                lines[3].Location = location;
            }
        }


        /// <summary>
        /// 窗口大小改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e3"></param>
        private void Form1_Resize(object sender, EventArgs e)
        {
            try
            {
                locateResizeVideoPanel();
                m_activePanelBorder.show(m_activePanel.Location, m_activePanel.Width, m_activePanel.Height, ACTIVE_PANEL_BORDER_WIDTH);
                m_sdkOperator.Dialog.OnResizeDialog();
            }
            catch
            { }
        }


        #endregion 浏览窗口绘制相关




        #region 刷新设备列表

        /// <summary>
        /// 获取Pu列表后在消息回调中刷新界面上的资源树结构
        /// </summary>
        delegate void GetPuListDel();
        GetPuListDel m_getPuList;

        //当前设备是否连接通道
        bool is_on_dialog = false;

        /// <summary>
        /// 获得设备列表，更新Form中设备TreeView的内容，被this.getPuList()调用
        /// </summary>
        void procGetPuList()
        {
            //添加session节点
            treeViewResList.Nodes.Clear();
            TreeNode session = new TreeNode(m_sdkOperator.Session.Name);
            treeViewResList.Nodes.Add(session);

            for (int i = 0; i < m_sdkOperator.Session.PuList.Count; i++)
            {
                //添加设备节点
                Pu pu = (Pu)m_sdkOperator.Session.PuList[i];

                //当前设备是否连接通道
                foreach (OneDialog dlg in m_sdkOperator.Dialog.m_dialogs)
                {
                    if (dlg.pu == pu)
                    {
                        is_on_dialog = true;
                        break;
                    }
                }

                //m_sdkOperator.Dialog.m_dialogs;


                TreeNode puNode = new TreeNode();
                puNode.Name = pu.id;
                if (pu.puName.Length == 0)
                    puNode.Text = pu.id;
                else
                    puNode.Text = pu.puName;

                session.Nodes.Add(puNode);
                bool online = false;
                //添加通道节点
                foreach (Channel channl in pu.channelList)
                {
                    TreeNode channelNode = new TreeNode(channl.channelName);
                    if (channl.online)
                    {
                        //。。。。。。。。。。后添加。。。。。。。。
                        if (!is_on_dialog)
                        {
                            #region

                            //获取通道号
                            int channelNo = pu.getChannelNo(channelNode.Text);


                            //如果通道在线，判断是否为TSP通道，如果为TSP通道，则打开TSP通道
                            if (channelNo >= BVCU.BVCU_SUBDEV_INDEXMAJOR_MIN_TSP && channelNo < BVCU.BVCU_SUBDEV_INDEXMAJOR_MAX_TSP)
                            {
                                //打开Tsp通道
                                if (m_sdkOperator.Dialog.openTspDialog(pu, channelNo) == 0)
                                {
                                    //向listview中添加设备和其对应的数据长度列
                                    ListViewItem item = new ListViewItem();
                                    item.Text = pu.id;
                                    item.Tag = channelNo;
                                    ListViewItem.ListViewSubItem TspData = new ListViewItem.ListViewSubItem();
                                    ListViewItem.ListViewSubItem len = new ListViewItem.ListViewSubItem();
                                    item.SubItems.AddRange(new ListViewItem.ListViewSubItem[] { TspData, len });

                                    listViewGPSData.Items.Add(item);



                                }
                            }
                            else if (channelNo >= BVCU.BVCU_SUBDEV_INDEXMAJOR_MIN_CHANNEL && channelNo < BVCU.BVCU_SUBDEV_INDEXMAJOR_MAX_CHANNEL)
                            {
                                if (m_sdkOperator.Dialog.Count == 4)
                                {
                                    //MessageBox.Show("窗口已全部占用");
                                    //return;
                                }
                                else
                                {
                                    foreach (Panel panel in m_videoPanels)
                                    {
                                        if (panel.ContextMenuStrip == null)
                                        {
                                            panel.ContextMenuStrip = contextMenuStripVideo;
                                            Console.WriteLine("Open dialog pu " + pu.id + " channel " + channelNo);

                                            //打开浏览*******************************************************************
                                            m_sdkOperator.Dialog.openBrowse(pu, channelNo, panel);
                                            //录像
                                            // RecordPath = "E:\\PIPE_DATA\\TEST";
                                            Thread.Sleep(10000);
                                            m_sdkOperator.Dialog.record(panel);

                                            /*
                                            Player p = new Player();
                                            p.Show();

                                            m_sdkOperator.Dialog.PreviewVideo(pu, channelNo, p.tbPlay);
                                            */
                                            break;
                                        }
                                    }

                                    Thread.Sleep(10000);
                                }
                            }

                            #endregion
                        }

                        channelNode.ForeColor = Color.Blue;
                        online = true;
                    }
                    else
                    {
                        channelNode.ForeColor = Color.Gray;
                    }
                    puNode.Nodes.Add(channelNode);
                }
                /*for (int t = 0; t < pu.channelList.Count; t++)
                {
                    Channel channl = pu.channelList[i] as Channel;
                    TreeNode channelNode = new TreeNode(channl.channelName);
                    if (channl.online)
                    {
                        channelNode.ForeColor = Color.Blue;
                        online = true;
                    }
                    else
                    {
                        channelNode.ForeColor = Color.Gray;
                    }
                    puNode.Nodes.Add(channelNode);
                }*/

                if (online)
                {
                    puNode.ForeColor = Color.Blue;
                }
                else
                {
                    puNode.ForeColor = Color.Gray;
                }

                is_on_dialog = false;
            }

            //打开TSP通道后，向所有打开通道发送数据请求信息
            sndData_timer.Enabled = true;
        }

        /// <summary>
        /// 获得设备列表
        /// </summary>
        public void getPuList()
        {
            if (treeViewResList.IsHandleCreated)
            {
                treeViewResList.BeginInvoke(m_getPuList);
            }
        }

        #endregion 刷新设备列表


        #region 节点浏览有关事件
        /// <summary>
        /// 双击界面上的资源树打开对话
        /// </summary>
        const int TREE_LEVEL_SESSION = 0;
        const int TREE_LEVEL_PU = 1;
        const int TREE_LEVEL_CHANNEL = 2;

        /// <summary>
        /// 点击设备列表中的节点时发生
        /// </summary>
        private void treeViewResList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //点击通道时发生，视频通道
            if (e.Node.Level == TREE_LEVEL_CHANNEL)
            {
                Pu pu = m_sdkOperator.Session.getPu(e.Node.Parent.Name);
                int channelNo = pu.getChannelNo(e.Node.Text);
                if (channelNo >= BVCU.BVCU_SUBDEV_INDEXMAJOR_MIN_CHANNEL && channelNo < BVCU.BVCU_SUBDEV_INDEXMAJOR_MAX_CHANNEL)
                {
                    if (m_sdkOperator.Dialog.Count == VIDEO_PANEL_COUNT)
                    {
                        MessageBox.Show("窗口已全部占用");
                        return;
                    }
                    foreach (Panel panel in m_videoPanels)
                    {
                        if (panel.ContextMenuStrip == null)
                        {
                            panel.ContextMenuStrip = contextMenuStripVideo;
                            Console.WriteLine("Open dialog pu " + pu.id + " channel " + channelNo);

                            //打开浏览*******************************************************************
                            m_sdkOperator.Dialog.openBrowse(pu, channelNo, panel);

                            /*
                            Player p = new Player();
                            p.Show();

                            m_sdkOperator.Dialog.PreviewVideo(pu, channelNo, p.tbPlay);
                            */
                            break;
                        }
                    }
                }
                else if (channelNo >= BVCU.BVCU_SUBDEV_INDEXMAJOR_MIN_TSP && channelNo < BVCU.BVCU_SUBDEV_INDEXMAJOR_MAX_TSP)
                {
                    //打开Tsp通道
                    if (m_sdkOperator.Dialog.openTspDialog(pu, channelNo) == 0)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = pu.id;
                        item.Tag = channelNo;
                        ListViewItem.ListViewSubItem TspData = new ListViewItem.ListViewSubItem();
                        ListViewItem.ListViewSubItem len = new ListViewItem.ListViewSubItem();
                        item.SubItems.AddRange(new ListViewItem.ListViewSubItem[] { TspData, len });

                        listViewGPSData.Items.Add(item);
                    }
                }
               
            }//点击通道时发生 end
        }

        /// <summary>
        /// 右键关闭对话菜单
        /// </summary>
        private void toolStripMenuItemClosePreview_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            Panel panel = (menu.GetCurrentParent() as ContextMenuStrip).SourceControl as Panel;
            panel.ContextMenuStrip = null;

            //关闭录像
            m_sdkOperator.Dialog.record(panel);

            m_sdkOperator.Dialog.closeBrowse(panel);
        }

        /// <summary>
        /// 右键截图菜单
        /// </summary>
        private void toolStripMenuItemSnapshot_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            Panel panel = (menu.GetCurrentParent() as ContextMenuStrip).SourceControl as Panel;

            m_sdkOperator.Dialog.capture(panel);
        }

        /// <summary>
        /// 右键录像菜单
        /// </summary>
        private void toolStripMenuItemRecord_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            Panel panel = (menu.GetCurrentParent() as ContextMenuStrip).SourceControl as Panel;

            m_sdkOperator.Dialog.record(panel);
        }

        /// <summary>
        /// 视频浏览框 右键弹起事件响应
        /// </summary>
        void panel_MouseUp(object sender, MouseEventArgs e)
        {
            Panel panel = sender as Panel;
            if (m_sdkOperator.Dialog.Recording(panel))
            {
                toolStripMenuItemRecord.Checked = true;
            }
            else
            {
                toolStripMenuItemRecord.Checked = false;
            }
        }

        #endregion 浏览有关事件


        #region 串口发送数据

        /// <summary>
        ///  串口发送数据
        /// </summary>
        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            this.m_sdkOperator.Dialog.SendTspData(this.tbInputMsg.Text);
            //this.tbInputMsg.Text = "";
        }

        //向所有TSP通道发送请求数据
        public void sndMsg_TSP(object sender, EventArgs e)
        {
           

            this.m_sdkOperator.Dialog.SendTspData();
        }

        #endregion 串口发送数据


        #region 其他事件

        /// <summary>
        /// 窗口关闭前释放所有资源
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Panel panel in m_videoPanels)
            {
                if (panel.ContextMenuStrip != null)
                {
                    //关闭录像
                    m_sdkOperator.Dialog.record(panel);
                }
            }

            sndData_timer.Enabled = false;

            m_sdkOperator.Session.logout();
            m_sdkOperator.Dispose();
            conn.Dispose();
            conn.Close();
        }

        #endregion 其他事件


        #region 配置截图录像的存储路径


        /// <summary>
        /// 配置截图及录像保存路径
        /// </summary>
        string m_capturePath;
        public string CapturePath
        {
            get
            {
                return m_capturePath;
            }
            private set
            {
                m_capturePath = value;
                labelCapturePath.Text = value;
            }
        }

        /// <summary>
        /// 配置截图及录像保存路径
        /// </summary>
        private void buttonCapturePath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CapturePath = folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>
        /// 配置截图及录像保存路径
        /// </summary>
        string m_recordPath;
        public string RecordPath
        {
            get
            {
                return m_recordPath;
            }
            private set
            {
                m_recordPath = value;
                labelRecordPath.Text = value;
            }
        }

        /// <summary>
        /// 配置截图及录像保存路径
        /// </summary>
        private void buttonRecordPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                RecordPath = folderBrowserDialog.SelectedPath;
            }
        }

        #endregion 配置截图录像的存储路径


        #region 云台控制
            

      

       

        #endregion 云台控制



        #region GpsAndTsp

        string RecStr = "";
        string RecAll = "";

        /// <summary>
        /// 获得Tsp数据
        /// </summary>
        public void onGetTspData(string puId, int iChannelNum, string pTspData, int len)
        {
            //遍历listview
            foreach (ListViewItem item in listViewGPSData.Items)
            {
                if (item.Text == puId)
                {
                    //串口数据显示
                    {
                        DateTime dataNow = DateTime.Now;
                        if (pTspData.Length < len)
                            return;
                        string msg = string.Format("{0}-{1} MSG:{2} [{3}:{4}:{5}]\r\n", puId, iChannelNum, pTspData.Substring(0, len), dataNow.Hour, dataNow.Minute, dataNow.Second);
                        //this.tbTSPData.AppendText(msg);
                        //this.tbTSPData.ScrollToCaret();

                       

                        //log中记录接收数据
                        LogHelper.LogHelper.RecordLog(2, msg);

                        RecStr = pTspData.Substring(0, len);

                        if (RecStr[0] == '5')
                        {
                            RecAll = RecStr;
                        }
                        else
                        {
                            RecAll += RecStr;
                        }

                        

                        if (RecAll.Length >=240)
                        {
                            //处理数据
                            byte[] RecData = Encoding.UTF8.GetBytes(RecAll);
                            byte[] nci = new byte[64];
                            Array.Copy(RecData, 176, nci, 0, 64);
                            //获取传感器数据，异常数据存入数据库，否则存入内存数组，100条，等待客户端请求
                            setNCIData(nci,puId); 


                        }
                    }
                    //如果通道号相同，则显示数据
                    if (item.Tag.ToString() == iChannelNum.ToString())
                    {
                        ListViewItem.ListViewSubItem TspData = new ListViewItem.ListViewSubItem();
                        TspData.Text = RecAll;//pTspData;
                        item.SubItems[1] = TspData;
                        ListViewItem.ListViewSubItem length = new ListViewItem.ListViewSubItem();
                        length.Text = RecAll.Length.ToString();
                        item.SubItems[2] = length;
                        break;
                    }
                }
            }
        }
        public struct gsData
        {
            public short CO ;
            public short CO2;
            public short H2S;
            public short NH3;
            public short CH4;
            public short O2;
        }
        public gsData gsSrData = new gsData();

        //获取传感器数据，存入数据库
        public void setNCIData(byte[] nci,string puid)
        {
            byte[,] nciOrder = new byte[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    nciOrder[i, j] = nci[i * 8 + j];
                }
            }

            for (int i = 0; i < 8; i++)
            {
                if (nciOrder[i, 0] == 0x0e && nciOrder[i, 1] == 0x01)
                {
                    gsSrData.CO = 0;
                    gsSrData.CH4 = 0;
                    gsSrData.CO2 = 0;
                    gsSrData.H2S = 0;
                    gsSrData.O2 = 0;
                    gsSrData.NH3 = 0;

                    if (nciOrder[i, 2] == 0x01)
                    {
                        //get value of H2S content 
                        gsSrData.H2S = bytes2short(new byte[] { nciOrder[i, 7], nciOrder[i, 6] });

                        //get value of CO content 
                        gsSrData.CO = bytes2short(new byte[] { nciOrder[i, 5], nciOrder[i, 4] });

                    }

                    if (nciOrder[i, 2] == 0x02)
                    {
                        //get value of O2 content 
                        gsSrData.O2 = bytes2short(new byte[] { nciOrder[i, 7], nciOrder[i, 6] });

                        //get value of NH3 content 
                        gsSrData.NH3 = bytes2short(new byte[] { nciOrder[i, 5], nciOrder[i, 4] });

                    }

                    if (nciOrder[i, 2] == 0x03)
                    {
                        gsSrData.CH4 = bytes2short(new byte[] { nciOrder[i, 7], nciOrder[i, 6] });
                        gsSrData.CO2 = bytes2short(new byte[] { nciOrder[i, 5], nciOrder[i, 4] });

                    }

                    //Queue<short[]> rsdata_Now = new Queue<short[]>();
                    rsdata_Now=new short[]{gsSrData.CO, gsSrData.CO2, gsSrData.H2S, gsSrData.NH3};

                    if (pu_rsDatas.ContainsKey(puid))
                    {
                        if (pu_rsDatas[puid].Count >= 100)
                        {
                            pu_rsDatas[puid].Dequeue();
                        }
                        pu_rsDatas[puid].Enqueue(rsdata_Now);
                    }
                    else
                    {
                        RSdatas_Now = new Queue<short[]>();
                        RSdatas_Now.Enqueue(rsdata_Now);
                        pu_rsDatas.Add(puid, RSdatas_Now);
                    }

                   
                    //存入数据库
                    //string sql1 = string.Format(@"insert into gassensordata(DataTime,CO,CO2,H2S,NH3) values('{0}', '{1}', '{2}', '{3}', '{4}')", DateTime.Now, gsSrData.CO, gsSrData.CO2, gsSrData.H2S, gsSrData.NH3);
                    //MySqlCommand mycmd = new MySqlCommand(sql1, conn);
                    //mycmd.ExecuteNonQuery();              
               

                }

                //测试
                //gsSrData.CO = 1;
                //gsSrData.CH4 = 0;
                //gsSrData.CO2 = 2;
                //gsSrData.H2S = 3;
                //gsSrData.O2 = 0;
                //gsSrData.NH3 = 0;

                //string sql1 = string.Format(@"insert into gassensordata(DataTime,CO,CO2,H2S,NH3) values('{0}', '{1}', '{2}', '{3}', '{4}')", DateTime.Now, gsSrData.CO, gsSrData.CO2, gsSrData.CO2, gsSrData.NH3);
                //MySqlCommand mycmd = new MySqlCommand(sql1, conn);
                //mycmd.ExecuteNonQuery(); 
            }

        }

        /**
     * 字节数组转短整数
     * @param bytes
     * @return short
     */
        private short bytes2short(byte[] IxByteArr)
        {
            short s = (short)(IxByteArr[1] & 0xFF);
            s |= (short)((IxByteArr[0] << 8) & 0xFF00);
            return (s);
        }




        #endregion Gps

        //使发送数据按钮可用
        delegate void SendTspMsg(bool isCanSendMsg);
        private SendTspMsg senTspMsg = null;
        public void TspStatusChange_OnEvent(bool isHaveOpen)
        {
            if (null == this.senTspMsg)
            {
                this.senTspMsg = new SendTspMsg(this.SenTspMsg_OnEvent);
            }

            this.BeginInvoke(this.senTspMsg, new object[] { isHaveOpen });
        }


        private void SenTspMsg_OnEvent(bool isCanSendMsg)
        {
            this.btnSendMsg.Enabled = isCanSendMsg;
        }

        int g_channelNo;
        Pu g_pu;
        private void treeViewResList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (e.Node.Level == TREE_LEVEL_CHANNEL)
                {
                    Pu pu = m_sdkOperator.Session.getPu(e.Node.Parent.Name);
                    int ichannelNo = pu.getChannelNo(e.Node.Text);
                    g_pu = pu;
                    g_channelNo = ichannelNo;
                    if (ichannelNo >= BVCU.BVCU_SUBDEV_INDEXMAJOR_MIN_CHANNEL && ichannelNo < BVCU.BVCU_SUBDEV_INDEXMAJOR_MAX_CHANNEL)
                    {
                        contextMenuStripTalkOnly.Show(Control.MousePosition.X,
                            Control.MousePosition.Y);
                    }
                }
            }
        }

        // 打开对讲
        private void ToolStripMenuItemTalkOnly_Click(object sender, EventArgs e)
        {
            if (m_sdkOperator.Dialog.Count == VIDEO_PANEL_COUNT)
            {
                MessageBox.Show("窗口已全部占用");
                return;
            }
            foreach (Panel panel in m_videoPanels)
            {
                if (panel.ContextMenuStrip == null)
                {
                    panel.ContextMenuStrip = contextMenuStripVideo;
                    Console.WriteLine("Open dialog pu for talk" + g_pu.id + " channel " + g_channelNo);
                    m_sdkOperator.Dialog.openTalkOnly(g_pu, g_channelNo, panel);
                    break;
                }
            }
        }

        public void clearTreeNodes()
        {
            treeViewResList.Nodes.Clear();
            if (listViewGPSData.Items.Count > 0)
                listViewGPSData.Items.Clear();
        }

        public void ClearlistViewGPSData(string puid, int channelNo)
        {
            for (int i = 0; i < listViewGPSData.Items.Count; ++i)
            {
                ListViewItem item = listViewGPSData.Items[i];
                if (puid.Equals(item.Text, StringComparison.CurrentCultureIgnoreCase) && 
                    (int)item.Tag == channelNo)
                {
                    listViewGPSData.Items.Remove(item);
                }
            }
        }

 
        //登录
        private void login_btn_Click(object sender, EventArgs e)
        {
            m_sdkOperator.Session.login(textBoxIp.Text, int.Parse(textBoxPort.Text), textBoxUsrName.Text, textBoxPsw.Text);

            //测试
            //Queue<short[]> data=new Queue<short[]>();
            //short[] data1 = new short[]{ 22, 11, 22, 44 };
            //data.Enqueue(data1);

            //pu_rsDatas.Add("TBG", data);

            //开启新线程，监听客户端请求
            Thread rec_Brequest = new System.Threading.Thread(websocket_Rec.startListen);
            rec_Brequest.Start();


            //测试
            //data = new Queue<short[]>();
            //data1 = new short[] { 33, 44, 55, 66 };
            //data.Enqueue(data1);
            //pu_rsDatas.Add("TBGY", data);
        }
    }
}
