using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SmartEye_Demo
{

    /// <summary>
    /// 协议帧工厂
    /// 提供设备号，获取待发送的协议帧
    /// 2017年8月22日 11:53:42
    /// </summary>
    public class FrameFactory
    {
        /// <summary>
        /// 设备指令列表
        /// </summary>
        private static Dictionary<string, Queue<Frame_aNCI>> PuaNCIList = new Dictionary<string, Queue<Frame_aNCI>>();

        /// <summary>
        /// 装载需要发送的指令
        /// </summary>
        /// <param name="PuId"></param>
        /// <param name="anci"></param>
        public static void Load(string PuId, Frame_aNCI anci)
        {
            /* 设备指令列表的维护可能要单独做，这里只是提供一个简单的示例 */
            if (!PuaNCIList.Keys.Contains(PuId))
            {
                PuaNCIList.Add(PuId, new Queue<Frame_aNCI>());
            }
            PuaNCIList[PuId].Enqueue(anci);
        }

        /// <summary>
        /// 通过设备号获取对应设备的待发送指令
        /// </summary>
        /// <param name="PuId">设备号</param>
        /// <returns>待发送协议帧</returns>
        public static S_Frame Create(string PuId)
        {
            S_FrameBuilder sb = new S_FrameBuilder();
            if (PuaNCIList.Keys.Contains(PuId) && PuaNCIList[PuId].Count > 0)
            {
                sb.AddaNci(PuaNCIList[PuId].Dequeue());
            }
            return sb.GetFrame();
        }
    }


    /// <summary>
    /// 协议帧父类
    /// 提供CRC校验
    /// 2017年2月22日 16:26:43
    /// </summary>
    public class Frame
    {
        #region CRC校验
        public static byte[] short2bytes(short s)
        {
            byte[] bytes = new byte[2];
            for (int i = 1; i >= 0; i--)
            {
                bytes[i] = (byte)(s % 256);
                s >>= 8;
            }
            return bytes;
        }
        public static short bytes2short(byte[] bytes)
        {
            short s = (short)(bytes[1] & 0xFF);
            s |= Convert.ToInt16((bytes[0] << 8) & 0xFF00);

            return s;
        }
        public static byte[] crc16Bytes(byte[] data)
        {
            return short2bytes(crc16Short(data));
        }
        public static short crc16Short(byte[] data)
        {
            return getCrc(data);
        }
        public static int[] crcTable = new int[]{
    		0x0000, 0xC0C1, 0xC181, 0x0140, 0xC301, 0x03C0, 0x0280, 0xC241,
    		0xC601, 0x06C0, 0x0780, 0xC741, 0x0500, 0xC5C1, 0xC481, 0x0440,
    		0xCC01, 0x0CC0, 0x0D80, 0xCD41, 0x0F00, 0xCFC1, 0xCE81, 0x0E40,
    		0x0A00, 0xCAC1, 0xCB81, 0x0B40, 0xC901, 0x09C0, 0x0880, 0xC841,
    		0xD801, 0x18C0, 0x1980, 0xD941, 0x1B00, 0xDBC1, 0xDA81, 0x1A40,
    		0x1E00, 0xDEC1, 0xDF81, 0x1F40, 0xDD01, 0x1DC0, 0x1C80, 0xDC41,
    		0x1400, 0xD4C1, 0xD581, 0x1540, 0xD701, 0x17C0, 0x1680, 0xD641,
    		0xD201, 0x12C0, 0x1380, 0xD341, 0x1100, 0xD1C1, 0xD081, 0x1040,
    		0xF001, 0x30C0, 0x3180, 0xF141, 0x3300, 0xF3C1, 0xF281, 0x3240,
    		0x3600, 0xF6C1, 0xF781, 0x3740, 0xF501, 0x35C0, 0x3480, 0xF441,
    		0x3C00, 0xFCC1, 0xFD81, 0x3D40, 0xFF01, 0x3FC0, 0x3E80, 0xFE41,
    		0xFA01, 0x3AC0, 0x3B80, 0xFB41, 0x3900, 0xF9C1, 0xF881, 0x3840,
    		0x2800, 0xE8C1, 0xE981, 0x2940, 0xEB01, 0x2BC0, 0x2A80, 0xEA41,
    		0xEE01, 0x2EC0, 0x2F80, 0xEF41, 0x2D00, 0xEDC1, 0xEC81, 0x2C40,
    		0xE401, 0x24C0, 0x2580, 0xE541, 0x2700, 0xE7C1, 0xE681, 0x2640,
    		0x2200, 0xE2C1, 0xE381, 0x2340, 0xE101, 0x21C0, 0x2080, 0xE041,
    		0xA001, 0x60C0, 0x6180, 0xA141, 0x6300, 0xA3C1, 0xA281, 0x6240,
    		0x6600, 0xA6C1, 0xA781, 0x6740, 0xA501, 0x65C0, 0x6480, 0xA441,
    		0x6C00, 0xACC1, 0xAD81, 0x6D40, 0xAF01, 0x6FC0, 0x6E80, 0xAE41,
    		0xAA01, 0x6AC0, 0x6B80, 0xAB41, 0x6900, 0xA9C1, 0xA881, 0x6840,
    		0x7800, 0xB8C1, 0xB981, 0x7940, 0xBB01, 0x7BC0, 0x7A80, 0xBA41,
    		0xBE01, 0x7EC0, 0x7F80, 0xBF41, 0x7D00, 0xBDC1, 0xBC81, 0x7C40,
    		0xB401, 0x74C0, 0x7580, 0xB541, 0x7700, 0xB7C1, 0xB681, 0x7640,
    		0x7200, 0xB2C1, 0xB381, 0x7340, 0xB101, 0x71C0, 0x7080, 0xB041,
    		0x5000, 0x90C1, 0x9181, 0x5140, 0x9301, 0x53C0, 0x5280, 0x9241,
    		0x9601, 0x56C0, 0x5780, 0x9741, 0x5500, 0x95C1, 0x9481, 0x5440,
    		0x9C01, 0x5CC0, 0x5D80, 0x9D41, 0x5F00, 0x9FC1, 0x9E81, 0x5E40,
    		0x5A00, 0x9AC1, 0x9B81, 0x5B40, 0x9901, 0x59C0, 0x5880, 0x9841,
    		0x8801, 0x48C0, 0x4980, 0x8941, 0x4B00, 0x8BC1, 0x8A81, 0x4A40,
    		0x4E00, 0x8EC1, 0x8F81, 0x4F40, 0x8D01, 0x4DC0, 0x4C80, 0x8C41,
    		0x4400, 0x84C1, 0x8581, 0x4540, 0x8701, 0x47C0, 0x4680, 0x8641,
    		0x8201, 0x42C0, 0x4380, 0x8341, 0x4100, 0x81C1, 0x8081, 0x4040
    	};

        public static int gPloy = 0x1021; //  
        public static short getCrcOfByte(int aByte)
        {
            int value = aByte << 8;

            for (int count = 7; count >= 0; count--)
            {
                if ((value & 0x8000) != 0)
                {
                    value = (value << 1) ^ gPloy;
                }
                else
                {
                    value = value << 1; //        
                }
            }
            value = value & 0xFFFF; // 
            return (short)value;
        }
        public static short getCrc(byte[] data)
        {
            int crc = 0xffff;
            int length = data.Length;
            for (int i = 0; i < length; i++)
            {
                crc = (crc >> 8) ^ crcTable[(crc ^ data[i]) & 0xff];
            }
            return (short)crc;
        }
        #endregion
    }
    /// <summary>
    /// 协议帧类
    /// 产生发送帧对象
    /// </summary>
    public class S_Frame : Frame
    {
        public Byte[] ByteArray = new Byte[52];

        public Byte Head
        {
            get { return ByteArray[0]; }
            set { ByteArray[0] = value; }
        }

        public Byte Addr
        {
            get { return ByteArray[1]; }
            set { ByteArray[1] = value; }
        }

        public Byte Append
        {
            get { return ByteArray[2]; }
            set { ByteArray[2] = value; }
        }

        public Byte Nck
        {
            get { return ByteArray[3]; }
            set { ByteArray[3] = value; }
        }

        public Byte[] DataBuf
        {
            get { return ByteArray.Skip(4).Take(44).ToArray(); }
            set { value.CopyTo(ByteArray, 4); }
        }

        public Byte[] aDI
        {
            get { return ByteArray.Skip(4).Take(4).ToArray(); }
            set { value.CopyTo(ByteArray, 4); }
        }

        public Byte[] aDO
        {
            get { return ByteArray.Skip(8).Take(8).ToArray(); }
            set { value.CopyTo(ByteArray, 8); }
        }

        public Frame_aNCI[] aNci
        {
            get
            {
                Frame_aNCI[] aNiArray = new Frame_aNCI[4];
                for (int i = 0; i < 4; i++)
                {
                    aNiArray[i] = new Frame_aNCI();
                    ByteArray.Skip(16 + i * 8).Take(8).ToArray().CopyTo(aNiArray[i].ByteArray, 0);
                }
                return aNiArray;
            }
            set
            {
                for (int i = 0; i < 4; i++)
                {
                    value[i].ByteArray.CopyTo(ByteArray, 16 + i * 8);
                }
            }
        }
        //索引器
        public Frame_aNCI this[int index]
        {
            get
            {
                if (index < 4)
                {
                    Frame_aNCI _aNci = new Frame_aNCI();
                    ByteArray.Skip(16 + index * 8).Take(8).ToArray().CopyTo(_aNci.ByteArray, 0);
                    return _aNci;//直接输出一个引用类型行吗？//不行
                }
                return null;
            }
            set
            {
                if (index < 4)
                {
                    value.ByteArray.CopyTo(ByteArray, 16 + index * 8);
                }
            }
        }

        private Byte[] Crc
        {
            get
            {
                return ByteArray.Skip(48).Take(2).ToArray();
            }
            set
            {
                ByteArray[48] = value[1];
                ByteArray[49] = value[0];
            }
        }

        public Byte[] Temp
        {
            get { return ByteArray.Skip(50).Take(2).ToArray(); }
            set { value.CopyTo(ByteArray, 50); }
        }

        public S_Frame()
        {
            Head = 0x55;
            Addr = 0;
            Append = 0;
            Nck = 0;
            Temp = new Byte[] { 0xAA, 0xAA };
            SetCrc();
        }

        /// <summary>
        /// 由Nci[]创建一个发送帧
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="append"></param>
        /// <param name="nck"></param>
        /// <param name="aNciArray"></param>
        public S_Frame(Byte addr, Byte append, Byte nck, Frame_aNCI[] aNciArray)
        {
            Head = 0x55;
            Addr = addr;
            Append = append;
            Nck = nck;
            aNci = aNciArray;
            Crc = crc16Bytes(ByteArray.Take(48).ToArray());
            Temp = new Byte[] { 0xAA, 0xAA };
        }
        /// <summary>
        /// 无指令构造函数
        /// 默认Nci[]为空
        /// 每次设置数据后需要调用check
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="append"></param>
        /// <param name="nck"></param>
        public S_Frame(Byte addr, Byte append, Byte nck)
        {
            Head = 0x55;
            Addr = addr;
            Append = append;
            Nck = nck;
            Crc = crc16Bytes(ByteArray.Take(48).ToArray());
            Temp = new Byte[] { 0xAA, 0xAA };
        }

        public S_Frame(Byte[] byteArray)
        {
            byteArray.CopyTo(ByteArray, 0);
            Head = 0x55;
            Temp = new Byte[] { 0xAA, 0xAA };
            Crc = crc16Bytes(ByteArray.Take(48).ToArray());

        }
        public void SetCrc()
        {
            Crc = crc16Bytes(ByteArray.Take(48).ToArray());
        }

        public bool CheckCrc()
        {
            Byte[] _crc = crc16Bytes(ByteArray.Take(48).ToArray());
            if (_crc[0] == ByteArray[49] && _crc[1] == ByteArray[48])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    /// <summary>
    /// 协议帧类
    /// 产生接收帧对象
    /// </summary>
    public class R_Frame : Frame
    {
        public Byte[] ByteArray = new Byte[244];

        public Byte Head
        {
            get { return ByteArray[0]; }
            set { ByteArray[0] = value; }
        }

        public Byte Addr
        {
            get { return ByteArray[1]; }
            set { ByteArray[1] = value; }
        }

        public Byte Append
        {
            get { return ByteArray[2]; }
            set { ByteArray[2] = value; }
        }

        public Byte Nck
        {
            get { return ByteArray[3]; }
            set { ByteArray[3] = value; }
        }

        public Byte[] aDI
        {
            get { return ByteArray.Skip(4).Take(4).ToArray(); }
            set { value.CopyTo(ByteArray, 4); }
        }

        public Byte[] aDO
        {
            get { return ByteArray.Skip(8).Take(8).ToArray(); }
            set { value.CopyTo(ByteArray, 8); }
        }

        public Byte[] aRbtCmuInfo
        {
            get { return ByteArray.Skip(16).Take(20).ToArray(); }
            set { value.CopyTo(ByteArray, 16); }
        }

        public Byte[] unRbtState
        {
            get { return ByteArray.Skip(36).Take(60).ToArray(); }
            set { value.CopyTo(ByteArray, 36); }
        }

        public Byte[] unSnrData
        {
            get { return ByteArray.Skip(96).Take(40).ToArray(); }
            set { value.CopyTo(ByteArray, 96); }
        }

        public Byte[] unPipeData
        {
            get { return ByteArray.Skip(136).Take(40).ToArray(); }
            set { value.CopyTo(ByteArray, 136); }
        }

        public Frame_aNCI[] aNci
        {
            get
            {
                Frame_aNCI[] aNiArray = new Frame_aNCI[8];
                for (int i = 0; i < 8; i++)
                {
                    aNiArray[i] = new Frame_aNCI();
                    ByteArray.Skip(176 + i * 8).Take(8).ToArray().CopyTo(aNiArray[i].ByteArray, 0);
                }
                return aNiArray;
            }
            set
            {
                for (int i = 0; i < 8; i++)
                {
                    value[i].ByteArray.CopyTo(ByteArray, 176 + i * 8);
                }
            }
        }

        //索引器
        public Frame_aNCI this[int index]
        {
            get
            {
                if (index < 8)
                {
                    return aNci[index];
                }
                return null;
            }
            set
            {
                if (index < 8)
                {
                    aNci[index] = value;
                }
            }
        }

        private Byte[] Crc
        {
            get
            {
                return ByteArray.Skip(240).Take(2).ToArray();
            }
            set
            {
                ByteArray[240] = value[1];
                ByteArray[241] = value[0];
            }
        }

        public Byte[] Temp
        {
            get { return ByteArray.Skip(242).Take(2).ToArray(); }
            set { value.CopyTo(ByteArray, 242); }
        }

        public R_Frame()
        {
            Head = 0x55;
            Addr = 0x00;
            Append = 0x00;
            Nck = 0x00;
            Temp = new Byte[] { 0xAA, 0xAA };
        }

        public R_Frame(Byte[] byteArray)
        {
            byteArray.CopyTo(ByteArray, 0);
            Head = 0x55;
            Temp = new Byte[] { 0xAA, 0xAA };
        }

        public bool CheckCrc()
        {
            Byte[] _crc = crc16Bytes(ByteArray.Take(240).ToArray());
            if (_crc[0] == ByteArray[241] && _crc[1] == ByteArray[240])
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetCrc()
        {
            Crc = crc16Bytes(ByteArray.Take(240).ToArray());
        }

        public void CopyTo(R_Frame frame)
        {
            this.ByteArray.CopyTo(frame.ByteArray, 0);
        }

    }
    /// <summary>
    /// 协议帧类
    /// 产生aNci对象，用于初始化S_Frame
    /// </summary>
    public class Frame_aNCI
    {
        public Byte[] ByteArray = new Byte[8];

        public Byte uCmd
        {
            get { return ByteArray[0]; }
            set { ByteArray[0] = value; }
        }

        public Byte uSub
        {
            get { return ByteArray[1]; }
            set { ByteArray[1] = value; }
        }

        public Byte uExt1
        {
            get { return ByteArray[2]; }
            set { ByteArray[2] = value; }
        }

        public Byte uExt2
        {
            get { return ByteArray[3]; }
            set { ByteArray[3] = value; }
        }

        public int iData
        {
            get
            {
                //大端小端
                //Byte[] tem = new Byte[4];
                //tem[0] = ByteArray[7];
                //tem[1] = ByteArray[6];
                //tem[2] = ByteArray[5];
                //tem[3] = ByteArray[4];
                return System.BitConverter.ToInt32(ByteArray, 4);
            }
            set
            {
                //Byte[] tem = System.BitConverter.GetBytes(value);
                //ByteArray[7] = tem[0];
                //ByteArray[6] = tem[1];
                //ByteArray[5] = tem[2];
                //ByteArray[4] = tem[3];
                System.BitConverter.GetBytes(value).CopyTo(ByteArray, 4);
            }
        }

        public Frame_aNCI()
        {

        }

        public Frame_aNCI(aNciCmdEnum cmd, int idata)
        {
            switch (cmd)
            {
                case aNciCmdEnum.ROBOT_MOVE:
                    uCmd = 0x01;
                    uSub = 0x00;
                    uExt1 = 0x00;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.ROBOT_ROTATE:
                    uCmd = 0x02;
                    uSub = 0x00;
                    uExt1 = 0x00;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.LIFT:
                    uCmd = 0x03;
                    uSub = 0x00;
                    uExt1 = 0x00;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.HOLDER_PITCH:
                    uCmd = 0x04;
                    uSub = 0x00;
                    uExt1 = 0x00;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.HOLDER_ROTATE:
                    uCmd = 0x05;
                    uSub = 0x00;
                    uExt1 = 0x00;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.HOLDER_RESET:
                    uCmd = 0x06;
                    uSub = 0x00;
                    uExt1 = 0x00;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.CAMERA_SWITCH:
                    uCmd = 0x07;
                    uSub = 0x00;
                    uExt1 = 0x00;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.CAMERA_FOCUS:
                    uCmd = 0x08;
                    uSub = 0x00;
                    uExt1 = 0x00;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.CAMERA_ZOOM:
                    uCmd = 0x09;
                    uSub = 0x00;
                    uExt1 = 0x00;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.BRIGHT_LEVEL:
                    uCmd = 0x0A;
                    uSub = 0x00;
                    uExt1 = 0x00;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.REMOTE_OPERATE:
                    uCmd = 0x0B;
                    uSub = 0x00;
                    uExt1 = 0x00;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.HOLDER_COMPENSATION:
                    uCmd = 0x0C;
                    uSub = 0x00;
                    uExt1 = 0x00;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.ADDR_CHANNEL:
                    uCmd = 0x0D;
                    uSub = 0x00;
                    uExt1 = 0x00;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.SENSOR_READGAS:
                    uCmd = 0x0E;
                    uSub = 0x01;
                    uExt1 = 0x01;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.SENSOR_WRITEGAS:
                    uCmd = 0x0E;
                    uSub = 0x01;
                    uExt1 = 0x02;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.SENSOR_READLASER:
                    uCmd = 0x0E;
                    uSub = 0x02;
                    uExt1 = 0x01;
                    uExt2 = 0x00;
                    break;
                case aNciCmdEnum.SENSOR_WRITELASER:
                    uCmd = 0x0E;
                    uSub = 0x02;
                    uExt1 = 0x02;
                    uExt2 = 0x00;
                    break;

                //...

            }
            iData = idata;
        }
    }

    /// <summary>
    /// aNci操作指令枚举
    /// </summary>
    public enum aNciCmdEnum
    {
        [Description("爬行器前进")]
        ROBOT_MOVE = 0,
        [Description("爬行器转弯")]
        ROBOT_ROTATE = 1,
        [Description("举升杆")]
        LIFT = 2,
        [Description("云台俯仰")]
        HOLDER_PITCH = 3,
        [Description("云台旋转")]
        HOLDER_ROTATE = 4,
        [Description("云台一键复位")]
        HOLDER_RESET = 5,
        [Description("切换摄像头")]
        CAMERA_SWITCH = 6,
        [Description("摄像头聚焦")]
        CAMERA_FOCUS = 7,
        [Description("摄像头变倍")]
        CAMERA_ZOOM = 8,
        [Description("照明亮度调节")]
        BRIGHT_LEVEL = 9,
        [Description("远程操作")]
        REMOTE_OPERATE = 10,
        [Description("云台补偿")]
        HOLDER_COMPENSATION = 11,
        [Description("地址、信道")]
        ADDR_CHANNEL = 12,
        [Description("读气体传感器")]
        SENSOR_READGAS = 13,
        [Description("写气体传感器")]
        SENSOR_WRITEGAS = 14,
        [Description("读激光传感器")]
        SENSOR_READLASER = 15,
        [Description("写激光传感器")]
        SENSOR_WRITELASER = 16
    }


    /// <summary>
    /// S_Frame协议帧辅助创建类
    /// 建造者模式
    /// 版本 V1.0.1
    /// 修改历史：
    /// 添加AddaNci
    /// 添加GetFrameByteArray
    /// </summary>
    public class S_FrameBuilder
    {
        private S_Frame frame;
        private int aNciCnt = 0;

        public S_FrameBuilder()
        {
            frame = new S_Frame();
        }

        /// <summary>
        /// 向新建的发送帧里面添加anci指令
        /// </summary>
        /// <param name="aNci"></param>
        public void AddaNci(Frame_aNCI aNci)
        {
            if (aNciCnt > 3)
            {
                throw new Exception("overcount aNci");
            }
            else
            {
                frame[aNciCnt] = aNci;
                aNciCnt++;
                foreach (byte data in aNci.ByteArray)
                {
                    //Common.LogHelper.INFO(data.ToString("X2"));

                }
            }
        }

        /// <summary>
        /// 获取协议帧
        /// </summary>
        /// <returns></returns>
        public S_Frame GetFrame()
        {
            frame.SetCrc();
            return frame;
        }

        /// <summary>
        /// 获取协议帧字节数组
        /// </summary>
        /// <returns></returns>
        public Byte[] GetFrameByteArray()
        {
            frame.SetCrc();
            return frame.ByteArray;
        }
    }

    //public class R_FrameBuilder
    //{
    //    private R_Frame frame;
    //    Robot Robot;
    //    public R_FrameBuilder(Robot robot)
    //    {
    //        frame = new R_Frame();
    //        Robot = robot;
    //    }

    //    public void SetLeftWheelPuase(short pause)
    //    {
    //        BitConverter.GetBytes(pause).CopyTo(frame.ByteArray, 77);
    //    }

    //    public void SetRightWheelPuase(short pause)
    //    {
    //        BitConverter.GetBytes(pause).CopyTo(frame.ByteArray, 79);
    //    }

    //    public void SetGPS()
    //    {
    //        if (Robot.GPSLatitude > 0)
    //        {
    //            frame.ByteArray[65] = (Byte)'S';
    //        }
    //        else
    //        {
    //            frame.ByteArray[65] = (Byte)'N';
    //        }
    //        BitConverter.GetBytes(Robot.GPSLatitude).CopyTo(frame.ByteArray, 61);

    //        if (Robot.GPSLngitude > 0)
    //        {
    //            frame.ByteArray[70] = (Byte)'E';
    //        }
    //        else
    //        {
    //            frame.ByteArray[70] = (Byte)'W';
    //        }
    //        BitConverter.GetBytes(Robot.GPSLngitude).CopyTo(frame.ByteArray, 66);

    //    }

    //    public void SetNRFState(bool state)
    //    {
    //        if (state)
    //        {
    //            frame.ByteArray[4] |= 0x01;
    //            frame.ByteArray[4] &= 0xfd;
    //        }
    //        else
    //        {
    //            frame.ByteArray[4] |= 0x02;
    //            frame.ByteArray[4] &= 0xfe;
    //        }

    //    }

    //    public R_Frame GetFrame()
    //    {
    //        #region 帧头
    //        frame.Head = 0x55;
    //        frame.Addr = 0;
    //        frame.Append = 0;
    //        frame.Nck = 0;
    //        #endregion
    //        #region aDI
    //        if (Robot.NRFState)
    //        {
    //            frame.ByteArray[4] |= 0x01;
    //            frame.ByteArray[4] &= 0xFD;
    //        }
    //        else
    //        {
    //            frame.ByteArray[4] |= 0x02;
    //            frame.ByteArray[4] &= 0xFE;
    //        }

    //        if (Robot.NRFSendErr)
    //            frame.ByteArray[4] |= 0x04;
    //        else
    //            frame.ByteArray[4] &= 0xFB;

    //        if (Robot.NRFRecErr)
    //            frame.ByteArray[4] |= 0x08;
    //        else
    //            frame.ByteArray[4] &= 0xF7;

    //        if (Robot.GPSState)
    //            frame.ByteArray[4] |= 0x10;
    //        else
    //            frame.ByteArray[4] &= 0xEF;
    //        #endregion
    //        #region aDO
    //        frame.ByteArray[9] = (Byte)Robot.CameraSwitch;//获取摄像头方向
    //        #endregion
    //        #region aRbtCmuInfo

    //        #endregion
    //        #region unRbtState
    //        BitConverter.GetBytes((short)Robot.MoveDistance).CopyTo(frame.ByteArray, 36);
    //        BitConverter.GetBytes((short)Robot.MoveSpeed).CopyTo(frame.ByteArray, 38);
    //        BitConverter.GetBytes((short)Robot.BodyRoll).CopyTo(frame.ByteArray, 40);
    //        BitConverter.GetBytes((short)Robot.BodyPitch).CopyTo(frame.ByteArray, 42);
    //        BitConverter.GetBytes((short)Robot.BodyYaw).CopyTo(frame.ByteArray, 44);
    //        BitConverter.GetBytes((short)Robot.HolderLift).CopyTo(frame.ByteArray, 46);
    //        BitConverter.GetBytes((short)Robot.HolderRotate).CopyTo(frame.ByteArray, 48);
    //        BitConverter.GetBytes((short)Robot.HolderPitch).CopyTo(frame.ByteArray, 50);
    //        BitConverter.GetBytes((short)Robot.HolderAtmos).CopyTo(frame.ByteArray, 52);
    //        BitConverter.GetBytes((short)Robot.HolderTemp).CopyTo(frame.ByteArray, 54);
    //        BitConverter.GetBytes((short)Robot.HolderHumidity).CopyTo(frame.ByteArray, 56);
    //        BitConverter.GetBytes((byte)Robot.CameraZoom).CopyTo(frame.ByteArray, 58);
    //        BitConverter.GetBytes((short)Robot.BrightLevel).CopyTo(frame.ByteArray, 59);

    //        BitConverter.GetBytes((float)Math.Abs(Robot.GPSLatitude)).CopyTo(frame.ByteArray, 61);
    //        if (Robot.GPSLatitude > 0)
    //            frame.ByteArray[65] = (byte)'S';
    //        else
    //            frame.ByteArray[65] = (byte)'N';
    //        BitConverter.GetBytes((float)Math.Abs(Robot.GPSLngitude)).CopyTo(frame.ByteArray, 66);
    //        if (Robot.GPSLngitude > 0)
    //            frame.ByteArray[70] = (byte)'W';
    //        else
    //            frame.ByteArray[70] = (byte)'E';

    //        BitConverter.GetBytes((short)Robot.BatLevel).CopyTo(frame.ByteArray, 71);
    //        BitConverter.GetBytes((short)Robot.BodyAtmos).CopyTo(frame.ByteArray, 73);
    //        BitConverter.GetBytes((short)Robot.BodyTemp).CopyTo(frame.ByteArray, 75);
    //        #endregion
    //        #region unSnrData

    //        #endregion
    //        #region unPipeData

    //        #endregion
    //        #region aNci

    //        #endregion

    //        //SetLeftWheelPuase(robot.LeftPause);
    //        //SetRightWheelPuase(robot.RightPause);

    //        frame.SetCrc();

    //        return frame;
    //    }

    //    public Byte[] GetFrameByteArray()
    //    {
    //        GetFrame();
    //        frame.SetCrc();
    //        return frame.ByteArray;
    //    }

    //}
}
