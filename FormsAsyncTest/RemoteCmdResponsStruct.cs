﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace XbeeStruct
{
    public struct RemoteCmdResponsStruct
	{
		[MarshalAs(UnmanagedType.U1)]
		//0
		private byte Delimiter;

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
		private byte SourceAdr1;

		[MarshalAs(UnmanagedType.U1)]
		//6
		private byte SourceAdr2;

		[MarshalAs(UnmanagedType.U1)]
		//7
		private byte SourceAdr3;

		[MarshalAs(UnmanagedType.U1)]
		//8
		private byte SourceAdr4;

		[MarshalAs(UnmanagedType.U1)]
		//9
		private byte SourceAdr5;

		[MarshalAs(UnmanagedType.U1)]
		//10
		private byte SourceAdr6;

		[MarshalAs(UnmanagedType.U1)]
		//11
		private byte SourceAdr7;

		[MarshalAs(UnmanagedType.U1)]
		//12
		private byte SourceAdr8;

		[MarshalAs(UnmanagedType.U1)]
		//13
		private byte SourceAdrShort1;

		[MarshalAs(UnmanagedType.U1)]
		//14
		private byte SourceAdrShort2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		//15 16
		private char[] mATCmd;

		[MarshalAs(UnmanagedType.U1)]
		//17
		public RemoteCmdResponsStatus RemStatus;

		[MarshalAs(UnmanagedType.U1)]
        //18
		public byte Checksum;
		
        //----------------
        //
        // END STRUCT
        //
        //----------------

		public byte Length
        {
            get { return this.mLength; }
            set { this.mLength = value; }           
        }

		public string SourceAdr
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

		public string shortAdr
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
            return Util.StructToBytes<XbeeStruct.RemoteCmdResponsStruct>(this);         
        }
	}
    public enum RemoteCmdResponsStatus : byte
    {
        OK = 0x0,
        Error = 0x1,
        InvalidCommand = 0x2,
        InvalidParameter = 0x3,
        TransmissionFailed = 0x4
    }
}

