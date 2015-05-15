using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace XbeeStruct
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct DataSampleStruct
    {

        [MarshalAs(UnmanagedType.U1)]
        //0
        private byte mDelimiter;

        [MarshalAs(UnmanagedType.U2)]
        //1
        private ushort mLength;
        //<MarshalAs(UnmanagedType.U1)> _
        //Private mLength2 As Byte '1

        [MarshalAs(UnmanagedType.U1)]
        //3
        private byte mapi;

        [MarshalAs(UnmanagedType.U1)]
        //4
        private byte SourceAdr1;

        [MarshalAs(UnmanagedType.U1)]
        //5
        private byte SourceAdr2;

        [MarshalAs(UnmanagedType.U1)]
        //6
        private byte SourceAdr3;

        [MarshalAs(UnmanagedType.U1)]
        //7
        private byte SourceAdr4;

        [MarshalAs(UnmanagedType.U1)]
        //8
        private byte SourceAdr5;

        [MarshalAs(UnmanagedType.U1)]
        //9
        private byte SourceAdr6;

        [MarshalAs(UnmanagedType.U1)]
        //10
        private byte SourceAdr7;

        [MarshalAs(UnmanagedType.U1)]
        //11
        private byte SourceAdr8;

        [MarshalAs(UnmanagedType.U1)]
        //12
        private byte SourceAdrShort1;

        [MarshalAs(UnmanagedType.U1)]
        //13
        private byte SourceAdrShort2;

        [MarshalAs(UnmanagedType.U1)]
        //14
        private ReceiveOption mRcvOptions;

        [MarshalAs(UnmanagedType.U1)]
        //15
        private byte mNumSamples;

        [MarshalAs(UnmanagedType.U1)]
        //16
        private byte mDigitalMask1;

        [MarshalAs(UnmanagedType.U1)]
        //17
        private byte mDigitalMask2;

        [MarshalAs(UnmanagedType.U1)]
        //18
        private byte mAnalogMask;

        //Public Property Samples As List(Of Byte)

        //<MarshalAs(UnmanagedType.U1)> _
        //Private Sample1 As Byte '19

        //<MarshalAs(UnmanagedType.U1)> _
        //Private sample2 As Byte '20

        //<MarshalAs(UnmanagedType.U1)> _
        //Private CheckSum As Byte '21
        public byte CheckSum { get; set; }

        public byte API
        {
            get { return mapi; }
            set { mapi = value; }
        }

        public string Samples
        {
            get
            {
                string s = string.Empty;
                foreach (byte b in mSamples)
                {
                    s += Util.ConvertToHex(b);
                }
                return s;
            }
        }

        public ushort length
        {
            get { return Util.GetUshortFromLittleEndian(this.mLength); }
            set { this.mLength = Util.GetUshortFromLittleEndian(value); }
        }

        public string SourceAdr64
        {
            get
            {
                string mac = this.SourceAdr1.ToString("x2") + this.SourceAdr2.ToString("x2") + this.SourceAdr3.ToString("x2") + this.SourceAdr4.ToString("x2");
                mac = mac + this.SourceAdr5.ToString("x2") + this.SourceAdr6.ToString("x2") + this.SourceAdr7.ToString("x2") + this.SourceAdr8.ToString("x2");
                return mac.ToUpper();
            }
            set
            {
                if (value.Length == 8)
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

        public string SourceAdr16
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

        public byte GetApiType()
        {
            byte api = 0x92;
            return api;
        }

        private static List<byte> mSamples = new List<byte>();
        public static System.Collections.ArrayList packetbytes { get; set; }
        public const int DataPayLoadStartAtIndex = 20;
        //public const path Direction = path.Inn;

        public const byte FrameTypeId = 0x92;
        public void AddBytes(byte[] bytes)
        {
            for (int i = DataPayLoadStartAtIndex; i <= bytes.Length - 2; i += 1)
            {
                mSamples.Add(bytes[i]);
            }
            this.CheckSum = bytes[bytes.Length - 1];
            //packetbytes.ToArray()
        }

        public string GetSampleAsHex()
        {
            string hex = string.Empty;
            foreach (byte b in mSamples)
            {
                hex += Util.ConvertToHex(b);
            }
            return hex;
        }

        public byte[] GetPacketAsBytes()
        {
            List<byte> bytelist = new List<byte>();
            byte[] header = Util.StructToBytes(this);
            foreach (byte b in header)
            {
                bytelist.Add(b);
            }
            bytelist.RemoveAt(bytelist.Count - 1);
            foreach (byte b in mSamples)
            {
                bytelist.Add(b);
            }
            bytelist.Add(this.CheckSum);
            return bytelist.ToArray();
        }

    }

    public enum ReceiveOption : byte
    {
        Empty = 0x0,
        PacketAcknowledged = 0x1,
        Broadcast = 0x2,
        Encrypted = 0x20,
        EncryptedPacketAcknowledged = 0x21,
        EncryptedBroadcast = 0x22,
        FromEndDevice = 0x40,
        FromEndDevicePacketAcknowledged = 0x41,
        FromEndDeviceBroadcast = 0x42,
        FromEndDeviceEncrypted = 0x60,
        FromEndDeviceEncryptedPacketAcknowledged = 0x61,
        FromEndDeviceEncryptedBroadcast = 0x62,
        aaa = 0xc1
    }
}
