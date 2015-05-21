﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace XbeeStruct
{
    public struct RemoteCmdStrucktX
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

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        //16 17
        public char[] mATCmd;

        [MarshalAs(UnmanagedType.U1)]
        //18 is null if getting value, has value if setting register value
        public byte CmdData;

        [MarshalAs(UnmanagedType.U1)]
        //19 
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

        //public byte[] GetPacketAsBytes()
        //{
        //    List<byte> bytes = new List<byte>();
        //    bytes.AddRange(Util.StructToBytes<XbeeStruct.RemoteCmdStruckt>(this));
        //    // if getting value, CmdData should be empty(REMOVED) and last byte should be checksum
        //    if (this.Length == 15)
        //    {
        //        if (this.Checksum == 0)
        //        {
        //            bytes.RemoveAt(bytes.Count - 1);
        //        }
        //        else
        //        {
        //            if (this.CmdData != 0)
        //            {
        //                bytes.RemoveAt(bytes.Count - 2);
        //            }          
        //        }                      
        //    }
        //    return bytes.ToArray();
        //}
    }
}
