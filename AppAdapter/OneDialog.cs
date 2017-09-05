using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartEye_Demo
{
    /// <summary>
    /// 用于本地存储一个Dialog对话
    /// </summary>
    public class OneDialog
    {
        public Panel panel;         // 用于显示预览的窗口
        public IntPtr dialogHandle; // BVCU SDK返回的对话句柄
        public Pu pu;               // 对话所属的Pu设备
        public int channelNo;       // 对话所在的通道
        public DateTime timeStamp;  // 对话当前显示帧的时间戳
        public bool recording;      // 是否正在录像
        public byte[] sendData;     // 对应dialog发送给设备的数据
        public int state;//机器人状态
    
        public OneDialog()
        {
            dialogHandle = IntPtr.Zero;

            //byte[] sndData = Enumerable.Repeat((byte)0x00, 55).ToArray();
            //sndData[0] = 0x55;
            //sndData[53] = 0xAA;
            //sndData[54] = 0xAA;


            sendData = FrameFactory.Create("").ByteArray;
            //state = 2;

            //sendData = Encoding.UTF8.GetString(sndData);
            //sendData = sndData;
            //sndMsg.Replace("-", "");
           
        }
    }
}
