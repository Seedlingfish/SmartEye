using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SmartEye_Demo
{
    public class UDPsocket_Rec
    {
        public IPEndPoint ipep;
        Socket newsock;
        public byte[] buffer;
        int recv;
        Dialog n_dialog;

        public UDPsocket_Rec(Dialog dialog)
        {
            this.n_dialog = dialog;
        }

        public void startRec()
        {
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
            buffer = new byte[1024];

            newsock.Bind(ipep);

            //得到客户机IP
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender);
            recv = newsock.ReceiveFrom(buffer, ref Remote);

            Console.WriteLine("Message received from {0}: ", Remote.ToString());
            Console.WriteLine(BitConverter.ToString(buffer, 0, recv));


            while (true)
            {

                try
                {
                    string msg = Encoding.ASCII.GetString(buffer, 0, recv);
                    JObject jo = JObject.Parse(msg);
                    //获得控制箱是否接管的状态
                    string type = jo["Event"].ToString();
                    int stat = Convert.ToInt32(jo["status"]);
                    string puid = jo["puid"].ToString();
                    //根据接收的状态改变tsp通道状态
                    ChangeState(puid, stat);
                    Console.WriteLine(Remote.ToString() + msg);
                    buffer = new byte[1024];
                    recv = newsock.ReceiveFrom(buffer, ref Remote);               
           
                }
                catch (SocketException e)
                {
                    MessageBox.Show("异常：超时" + e.Message);
                    //SetText("异常：超时" + e.Message);
                }
            }


        }

        //根据接收的状态改变tsp通道状态
        public void ChangeState(string puid,int stat)
        {
            foreach (OneDialog tspDialog in n_dialog.m_tspDialogs)
            {
                if (tspDialog.pu.id == puid)
                {
                    tspDialog.state=stat;
                }
            }
        }
    }
}
