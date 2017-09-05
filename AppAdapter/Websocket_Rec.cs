using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.IO;
using SuperWebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace SmartEye_Demo
{
    public class Websocket_Rec
    {
        WebSocketServer server = new WebSocketServer();
        WebSocketSession session = null;

        Dictionary<string, Queue<MainWinForm.RsSlData>[]> m_pu_rsDatas;
        Dialog m_dialog;


        public Websocket_Rec(Dictionary<string, Queue<MainWinForm.RsSlData>[]> pu_rsDatas,Dialog dialog)
        {
            this.m_pu_rsDatas = pu_rsDatas;
            this.m_dialog = dialog;
        }

        struct RecData_BC
        {
            public string PuId { get; set; }
            public string Event { get; set; }
            public string CMD { get; set; }
            public int Data { get; set; }
        }

        public struct RsAlDatas
        {
            public Queue<MainWinForm.RsSlData> CO;
            public Queue<MainWinForm.RsSlData> CO2;
            public Queue<MainWinForm.RsSlData> O2;
            public Queue<MainWinForm.RsSlData> H2S;
        }

        public struct SndData_BC
        {
            public string Event;
            public RsAlDatas data;
            public int status;
        }

 

        public void startListen()
        {
            if (server.Setup("192.168.0.116", 8087))
            {
                //开始监听
                if (server.Start())
                {
                    server.SessionClosed += new SuperSocket.SocketBase.SessionHandler<WebSocketSession, SuperSocket.SocketBase.CloseReason>(server_SessionClosed);
                    server.NewSessionConnected += new SuperSocket.SocketBase.SessionHandler<WebSocketSession>(server_NewSessionConnected);
                    server.NewMessageReceived += new SuperSocket.SocketBase.SessionHandler<WebSocketSession, string>(server_NewMessageReceived);
                }
                else
                {
                    MessageBox.Show("监听失败");
                }
            }
            else
            {
                MessageBox.Show("创建WS服务器失败");
            }

            MessageBox.Show("开始监听");

            //测试
            //MessageBox.Show(m_pu_rsDatas["TBG"].ElementAt(0)[0].ToString());
        }

        void server_NewMessageReceived(WebSocketSession session, string value)
        {
            ShowMessage(session.RemoteEndPoint + "发送来数据" + value);

            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(value);
            RecData_BC recData_BC = (RecData_BC)serializer.Deserialize(new JsonTextReader(sr), typeof(RecData_BC));

            switch (recData_BC.Event)
            {
                case "CTL":
                    foreach (OneDialog tspDialog in m_dialog.m_tspDialogs)
                    {
                        if (tspDialog.pu.id == recData_BC.PuId)
                        {
                            Frame_aNCI anci = null;
                            anci = new Frame_aNCI((aNciCmdEnum)(aNciCmdEnum)Enum.Parse(typeof(aNciCmdEnum), recData_BC.CMD), recData_BC.Data);
                            FrameFactory.Load(recData_BC.PuId, anci);
                            tspDialog.sendData = FrameFactory.Create(recData_BC.PuId).ByteArray;
                            //session.Send(Encoding.UTF8.GetString(tspDialog.sendData));
                        }
                    }
                    
                    break;

                case "REQ":
                    if (m_pu_rsDatas.ContainsKey(recData_BC.PuId))
                    {
                        RsAlDatas rsAlDatas = new RsAlDatas
                        {
                            CO = m_pu_rsDatas[recData_BC.PuId][0],
                            CO2 = m_pu_rsDatas[recData_BC.PuId][1],
                            O2 = m_pu_rsDatas[recData_BC.PuId][2],
                            H2S = m_pu_rsDatas[recData_BC.PuId][3]

                        };

                        SndData_BC sndData = new SndData_BC
                        {
                            Event = "DATA",
                            data = rsAlDatas,
                            status = OnlineState(recData_BC.PuId)
                        };

                        JsonSerializer serializer_w = new JsonSerializer();
                        StringWriter sw = new StringWriter();
                        serializer_w.Serialize(new JsonTextWriter(sw), sndData);
                        session.Send(sw.GetStringBuilder().ToString());
                    }

                    break;
                default:
                    break;
            }

            //测试
            //session.Send(m_pu_rsDatas["TBGY"].ElementAt(0)[0].ToString());
           
        }

        //查找设备状态，如果查找到对应puid在线的TSP通道，则读取TSP通道状态，如果未查到，则返回离线
        public int OnlineState(string puid)
        {
            foreach (OneDialog tspDialog in m_dialog.m_tspDialogs)
            {
                if (tspDialog.pu.id == puid)
                {
                    return tspDialog.state;
                }
            }
            return 2;
        }

       

        void server_NewSessionConnected(WebSocketSession session)
        {
            this.session = session;
            ShowMessage(session.RemoteEndPoint + "有的会话接入");
        }

        void server_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            ShowMessage(session.RemoteEndPoint + "关闭会话");
        }

        public void ShowMessage(String msg)
        {
            //Action action = () =>
            //{
            //    textBox1.Text += msg + Environment.NewLine;
            //};
            //this.Invoke(action);

            MessageBox.Show(msg);
        }

    }
}
