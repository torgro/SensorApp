using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace XbeeStruct
{
    public struct RemoteCmdStruckt
    {
        [MarshalAs(UnmanagedType.U1)]
        //0
        public byte Delimiter;

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        //1 og 2
        //public char[] mLength;
        [MarshalAs(UnmanagedType.U1)]
        //1
        public byte mLength0;
        [MarshalAs(UnmanagedType.U1)]
        //2
        public byte mLength;
        [MarshalAs(UnmanagedType.U1)]
        //3
        public byte API;

        [MarshalAs(UnmanagedType.U1)]
        //4
        public byte FrameID;

        [MarshalAs(UnmanagedType.U1)]
        //5
        public byte SourceAdr1;

        [MarshalAs(UnmanagedType.U1)]
        //6
        public byte SourceAdr2;

        [MarshalAs(UnmanagedType.U1)]
        //7
        public byte SourceAdr3;

        [MarshalAs(UnmanagedType.U1)]
        //8
        public byte SourceAdr4;

        [MarshalAs(UnmanagedType.U1)]
        //9
        public byte SourceAdr5;

        [MarshalAs(UnmanagedType.U1)]
        //10
        public byte SourceAdr6;

        [MarshalAs(UnmanagedType.U1)]
        //11
        public byte SourceAdr7;

        [MarshalAs(UnmanagedType.U1)]
        //12
        public byte SourceAdr8;

        [MarshalAs(UnmanagedType.U1)]
        //13
        public byte SourceAdrShort1;

        [MarshalAs(UnmanagedType.U1)]
        //14
        public byte SourceAdrShort2;

        [MarshalAs(UnmanagedType.U1)]
        //15
        public byte CmdOptions;

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] MarshalAsAttribute
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        //16 17
        public char[] mATCmd;

        [MarshalAs(UnmanagedType.U1)]
        //18 is null if getting value, has value if setting register value
        public byte CmdData;

        [MarshalAs(UnmanagedType.U1)]
        //19 
        public byte Checksum;

        //private byte[] ATCmdData;
        public void ATCmdData(byte[] bytes)
        {
            
        }

        //public void ATCmd(string cmd)
        //{
        //    this.mATCmd = cmd.ToCharArray();
        //}
        ////public static ArrayList packetbytes { get; set; }

        ////public const int DataPayLoadStartAtIndex = 16;
        ////public const path Direction = path.Ut;

        ////public const byte FrameTypeId = 0x17;
        public byte Length
        {
            get { return this.mLength; }//{ return Util.GetUshortFromLittleEndian(this.mLength); }
            set
            {
                this.mLength = value;
                //char first = (char)(0);
                //char second = (char)(int.Parse(value));
                //List<char> list = new List<char>();

                //list.Add(first);
                //list.Add(second);
                //this.mLength = list.ToArray();
            }
        }

        public string DestAdr64
        {
            get
            {
                string mac = this.SourceAdr1.ToString("x2") + this.SourceAdr2.ToString("x2") + this.SourceAdr3.ToString("x2") + this.SourceAdr4.ToString("x2");
                mac = mac + this.SourceAdr5.ToString("x2") + this.SourceAdr6.ToString("x2") + this.SourceAdr7.ToString("x2") + this.SourceAdr8.ToString("x2");
                return mac.ToUpper();
            }
            set
            {
                if (value.Length == 16)
                {
                    this.SourceAdr1 = (byte)Util.ConvertHexToInt(value.Substring(0, 2));
                    this.SourceAdr2 = (byte)Util.ConvertHexToInt(value.Substring(2, 2));
                    this.SourceAdr3 = (byte)Util.ConvertHexToInt(value.Substring(4, 2));
                    this.SourceAdr4 = (byte)Util.ConvertHexToInt(value.Substring(6, 2));
                    this.SourceAdr5 = (byte)Util.ConvertHexToInt(value.Substring(8, 2));
                    this.SourceAdr6 = (byte)Util.ConvertHexToInt(value.Substring(10, 2));
                    this.SourceAdr7 = (byte)Util.ConvertHexToInt(value.Substring(12, 2));
                    this.SourceAdr8 = (byte)Util.ConvertHexToInt(value.Substring(14, 2));
                }
                else
                {
                    throw new Exception("Sourceaddress must be 8 chars long");
                }

            }
        }

        public string ATcmd
        {
            get
            {
                return new string(this.mATCmd);
            }
            set
            {
                char[] chars = value.ToUpper().ToCharArray();
                this.mATCmd = chars;
            }
        }

        public string DestAdr16
        {
            get
            {
                string shortAdr = this.SourceAdrShort1.ToString("x2") + this.SourceAdrShort2.ToString("x2");
                return shortAdr;
            }
            set
            {
                this.SourceAdrShort1 = (byte)Util.ConvertHexToInt(value.Substring(0, 2));
                this.SourceAdrShort2 = (byte)Util.ConvertHexToInt(value.Substring(2, 2));
            }
        }

        ////public byte[] GetPacketAsBytes()
        ////{
        ////    List<byte> bytelist = new List<byte>();
        ////    byte[] header = Util.StructToBytes(this);
        ////    foreach (byte b in header)
        ////    {
        ////        bytelist.Add(b);
        ////    }
        ////    //bytelist.RemoveAt(bytelist.Count - 1)
        ////    return bytelist.ToArray();
        ////}

        //public void AddBytes(byte[] bytes)
        //{
        //    //not needed in this struct, is it handled by a constant size of 2 in the mashalling structure/declaration
        //    // ONLY HERE FOR COMBABILITY WITH OBJECT CALLS and set checksum
        //    this.CmdData = 0;
        //    this.Checksum = bytes[bytes.Length - 1];
        //}

    }
}
