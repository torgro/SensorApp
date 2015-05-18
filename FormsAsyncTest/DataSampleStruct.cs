using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace XbeeStruct
{
    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct DataSampleStruct
    {
        [MarshalAs(UnmanagedType.U1)]
        //0
        public byte Delimiter;

        [MarshalAs(UnmanagedType.U1)]
        //1
        public byte mLength0;
        [MarshalAs(UnmanagedType.U1)]
        //2
        public byte mLength;

        [MarshalAs(UnmanagedType.U1)]
        //3
        public byte api;

        [MarshalAs(UnmanagedType.U1)]
        //4
        public byte SourceAdr1;

        [MarshalAs(UnmanagedType.U1)]
        //5
        public byte SourceAdr2;

        [MarshalAs(UnmanagedType.U1)]
        //6
        public byte SourceAdr3;

        [MarshalAs(UnmanagedType.U1)]
        //7
        public byte SourceAdr4;

        [MarshalAs(UnmanagedType.U1)]
        //8
        public byte SourceAdr5;

        [MarshalAs(UnmanagedType.U1)]
        //9
        public byte SourceAdr6;

        [MarshalAs(UnmanagedType.U1)]
        //10
        public byte SourceAdr7;

        [MarshalAs(UnmanagedType.U1)]
        //11
        public byte SourceAdr8;

        [MarshalAs(UnmanagedType.U1)]
        //12
        public byte SourceAdrShort1;

        [MarshalAs(UnmanagedType.U1)]
        //13
        public byte SourceAdrShort2;

        [MarshalAs(UnmanagedType.U1)]
        //14
        public ReceiveOption RcvOptions;

        [MarshalAs(UnmanagedType.U1)]
        //15
        public byte NumSamples;

        [MarshalAs(UnmanagedType.U1)]
        //16
        public byte DigitalMask1;

        [MarshalAs(UnmanagedType.U1)]
        //17
        public byte DigitalMask2;

        [MarshalAs(UnmanagedType.U1)]
        //18
        public byte AnalogMask;

        [MarshalAs(UnmanagedType.U1)]
        //'19
        public Byte Sample1;

        [MarshalAs(UnmanagedType.U1)]
        //'20
        public Byte Sample2;

        [MarshalAs(UnmanagedType.U1)]
        //'21
        public Byte CheckSum;

        //----------------
        //
        // END STRUCT
        //
        //----------------

        public string SamplesAsHex
        {
            get
            {
                string s = string.Empty;
                s += Util.ConvertToHex(this.Sample1);
                s += Util.ConvertToHex(this.Sample2);
                return s;
            }
        }

        public byte Length
        {
            get { return this.mLength; }
            set { this.mLength = value; }
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
                    throw new Exception("Sourceaddress must be 16 chars long");
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

        public string GetAsHex()
        {
            string hex = string.Empty;
            hex = Util.ConvertByteArrayToHexString(GetPacketAsBytes());
            return hex;
        } 

        public byte[] GetPacketAsBytes()
        {
            return Util.StructToBytes<XbeeStruct.DataSampleStruct>(this);         
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
        FromEndDeviceEncryptedBroadcast = 0x62
    }
}
