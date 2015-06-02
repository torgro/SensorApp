﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

public class RemoteCmdPackets
{
    public List<RemoteCmdPacket> List;
    //public List<RemoteCmdResponsType> ResponsTypeList;
    //events and delegates
    public event LogEventEventHandler LogEvent;
    public delegate void LogEventEventHandler(LogDetail LogItem);

    public RemoteCmdPackets()
    {
        this.List = new List<RemoteCmdPacket>();
        //this.ResponsTypeList = new List<RemoteCmdResponsType>();
    }

    public void AddPacket(GenericPacket GenPacket)
    {       		    
        RemoteCmdPacket cmd = GenPacket.ToRemoteCmdPacket();
        if(cmd != null)
        {
            cmd.Id = this.List.Count;
            cmd.TimeDate = DateTime.Now;
            this.List.Add(cmd);
        }
        else
	    {
            this.LogIt("Unable to add RemoteCmd packet, ToRemoteCmdPacket returned null");
	    }
    }
    
    virtual protected void OnLogEvent(LogDetail it)
    {
        if (LogEvent != null)
        {
            LogEvent(it);
        }
    }

    private void LogIt(string Str)
    {
        LogDetail log = new LogDetail();
        string calledby = new StackFrame(2, true).GetMethod().Name;
        string ClassName = this.GetType().FullName;
        log.ClassName = ClassName;
        log.Description = Str;
        log.Level = 0;
        log.Method = calledby;
        log.TimeDate = DateTime.Now;

        OnLogEvent(log);
        //if (LogEvent != null)
        //{
        //    LogEvent(log);
        //}
    }
}

public class RemoteCmdPacket
{
    public byte Delimiter;
    public int Id { get; set; }
    public DateTime TimeDate { get; set; }
    public XbeeBasePacket.XbeePacketDirection Direction { get; set; }
    public byte Length { get; set; }
    public XbeeBasePacket.XbeePacketType API { get; set; }
    public byte FrameID { get; set; }
    public string DestAdr64
    {
        get
        {
            string mac = this.mDestAddress1.ToString("x2") + this.mDestAddress2.ToString("x2") + this.mDestAddress3.ToString("x2") + this.mDestAddress4.ToString("x2");
            mac = mac + this.mDestAddress5.ToString("x2") + this.mDestAddress6.ToString("x2") + this.mDestAddress7.ToString("x2") + this.mDestAddress8.ToString("x2");
            return mac.ToUpper();
        }
        set
        {
            if (value.Length == 16)
            {
                this.mDestAddress1 = (byte)Util.ConvertHexToInt(value.Substring(0, 2));
                this.mDestAddress2 = (byte)Util.ConvertHexToInt(value.Substring(2, 2));
                this.mDestAddress3 = (byte)Util.ConvertHexToInt(value.Substring(4, 2));
                this.mDestAddress4 = (byte)Util.ConvertHexToInt(value.Substring(6, 2));
                this.mDestAddress5 = (byte)Util.ConvertHexToInt(value.Substring(8, 2));
                this.mDestAddress6 = (byte)Util.ConvertHexToInt(value.Substring(10, 2));
                this.mDestAddress7 = (byte)Util.ConvertHexToInt(value.Substring(12, 2));
                this.mDestAddress8 = (byte)Util.ConvertHexToInt(value.Substring(14, 2));
                this.mDestAdr64Bytes.Add(this.mDestAddress1);
                this.mDestAdr64Bytes.Add(this.mDestAddress2);
                this.mDestAdr64Bytes.Add(this.mDestAddress3);
                this.mDestAdr64Bytes.Add(this.mDestAddress4);
                this.mDestAdr64Bytes.Add(this.mDestAddress5);
                this.mDestAdr64Bytes.Add(this.mDestAddress6);
                this.mDestAdr64Bytes.Add(this.mDestAddress7);
                this.mDestAdr64Bytes.Add(this.mDestAddress8);

            }
            else
            {
                throw new Exception("Sourceaddress must be 16 chars long");
            }
        }

    }
    public string DestAdr16
    {
        get
        {
            string ShortMac = this.mDestShortAdr1.ToString("x2") + this.mDestShortAdr2.ToString("x2");
            return ShortMac;
        }
        set
        {
            if (value.Length == 4)
            {
                this.mDestShortAdr1 = (byte)Util.ConvertHexToInt(value.Substring(0, 2));
                this.mDestShortAdr2 = (byte)Util.ConvertHexToInt(value.Substring(2, 2));
            }
        }
    }
    public apiCmdOptions cmdOptions { get; set; }
    public string ATcmd
    {
        get
        {
            string cmd = new string(this.mATcmd);
            //cmd += Util.ConvertToHex(this.mATcmd[1]);
            return cmd;
        }
        set
        {
            if (value.Length == 2)
            {
                char[] chars = value.ToUpper().ToCharArray();
                this.mATcmd = chars;
            }
        }
    }
    public string cmdParameter 
    { 
        get
        {
            string param = this.mCmdParameter.ToString();
            if (this.API == XbeeBasePacket.XbeePacketType.RemoteCmd & this.mCmdParameter == 0x0)
            {
                return "";
            }
            return param;
        }
        set
        {
            this.mCmdParameter = Convert.ToByte(value);
        }
    }
    public string cmdRemoteData 
    {
        get
        {
            string RemoteCmd = string.Empty;//this.mCmdRemoteData.ToString();
            foreach (byte item in this.mCmdRemoteData)
            {
                RemoteCmd += Util.ConvertToHex(item) + " ";
            }
            if (this.API == XbeeBasePacket.XbeePacketType.RemoteCmd)
            {
                return "";
            }
            return RemoteCmd;
        }
        set
        {
            //fixme
            this.mCmdRemoteData.Add(Convert.ToByte(value));//Convert.ToByte(value);
        }
    }
    public string RemoteStatus 
    { 
        get
        {
            if (this.API == XbeeBasePacket.XbeePacketType.RemoteCmd)
            {
                return "";
            }
            return this.mRemoteStatus.ToString();
        }     
    }
    public byte CheckSum { get; set; }

    public DigitalMask digitalMaskValue;

    public List<byte> AllBytes;

    private List<byte> mDestAdr64Bytes;
    private RemoteCmdResponsStatus mRemoteStatus;
    private byte mCmdParameter;
    private byte Length0;
    private char[] mATcmd;
    private System.Collections.ArrayList mCmdRemoteData;
    private byte mDestAddress1;
    private byte mDestAddress2;
    private byte mDestAddress3;
    private byte mDestAddress4;
    private byte mDestAddress5;
    private byte mDestAddress6;
    private byte mDestAddress7;
    private byte mDestAddress8;
    private byte mDestShortAdr1;
    private byte mDestShortAdr2;

    // public byte CmdRespons { get; set; }
    //private byte DigitalPinHex = 0x44; 
    //public string PinName { get; set; }
    //public byte CmdData { get; set; } //old SLETTES

    public RemoteCmdPacket()
    {
        this.setDefaults();
    }

    public RemoteCmdPacket(byte[] bytes)
    {
        this.setDefaults();
        switch (bytes[3])
        {
            case 0x17:
            {
                this.ParseRemoteCMDBytes(bytes);
                break;
            }

            case 0x97:
            {
                this.ParseRemoteCmdResponsBytes(bytes);
                break;
            }
            
            default:
                break;
        }        
    }

    public RemoteCmdPacket(XbeeAPIpin Pin, String Address, byte FrameID)
    {
        this.setDefaults();
        this.GetPinStatus(Pin, Address, FrameID);
    }

    private void setDefaults()
    {
        //this.CmdData = 0xff;
        this.ATcmd = string.Empty;
        this.Length = 0x0;
        this.AllBytes = new List<byte>();
        this.Length0 = 0x0;
        //this.mATcmd = new byte [] {0x0,0x0};
        this.mDestAdr64Bytes = new List<byte>();
        this.mCmdRemoteData = new System.Collections.ArrayList();
    }

    public void CreatePinStatusPacket(XbeeAPIpin Pin, String Address, byte FrameID)
    {
        this.GetPinStatus(Pin,Address,FrameID); 
    }

    public void CreateSetPinTriggerPacket(bool EnableTrigger, XbeeAPIpin Pin, string Address, byte FrameID)
    {
        this.SetPinStatus(EnableTrigger, Pin, Address, FrameID);
    }

    public string ToHexString()
    {
        string str = string.Empty;
        if (this.AllBytes.Count > 0)
        {
            str = Util.ConvertByteArrayToHexString(this.AllBytes.ToArray());
        }
        return str;
    }

    public byte[] ToByteArray()
    {
        return this.AllBytes.ToArray();
    }

    public GenericPacket ToGenericPacket()
    {
        if (this.AllBytes.Count == 0)
        {
            return null;
        }
        GenericPacket Generic = new GenericPacket();
        Generic.API = (byte)this.API;
        Generic.Length = this.Length;
        Generic.APItype = this.API;
        Generic.CheckSum = this.CheckSum;
        Generic.Delimiter = this.Delimiter.ToString();
        Generic.Direction = this.Direction;
        Generic.FrameID = this.FrameID;
        Generic.SourceAddress = this.DestAdr64;
        Generic.PacketBytes = this.AllBytes;
        Generic.Hex = this.ToHexString();
        Generic.PacketBytes = this.AllBytes;
        return Generic;
    }

    private void UpdateAllBytes()
    {
        List<byte> list = new List<byte>();
        list.Add(this.Delimiter);  
        list.Add(this.Length0);        
        list.Add(this.Length);       
        list.Add((byte)this.API);        
        list.Add(this.FrameID);        
        list.AddRange(this.mDestAdr64Bytes);
        list.Add(this.mDestShortAdr1);
        list.Add(this.mDestShortAdr2);        
        list.Add((byte)this.cmdOptions);
        list.Add(Convert.ToByte(this.mATcmd[0]));
        list.Add(Convert.ToByte(this.mATcmd[1]));
        if (this.Length > 15)
        {
            list.Add(this.mCmdParameter);
        }
        //adding checksum;
        list.Add(0x0);
        int checksum = Util.ComputeChecksum(list.ToArray());
        this.CheckSum = (byte)checksum;
        list[list.Count - 1] = (byte)checksum;
        this.AllBytes = list;
    }

    public void GetBatteryLevel(string Adr, byte FrameID)
    {
        this.Delimiter = 0x7E;
        this.Length0 = 0x0;
        this.Length = 15;
        this.API = XbeeBasePacket.XbeePacketType.RemoteCmd;
        this.FrameID = FrameID;
        this.DestAdr64 = Adr;
        this.DestAdr16 = "FFFE";
        this.cmdOptions = apiCmdOptions.ApplyChanges;
        this.ATcmd = "IS";
        this.UpdateAllBytes();
    }
     
    private void GetPinStatus(XbeeAPIpin Pin, string Adr, byte FrameID)
    {
        this.Delimiter = 0x7E;        
        this.Length0 = 0x0;        
        this.Length = 15;        
        this.API = XbeeBasePacket.XbeePacketType.RemoteCmd;       
        this.FrameID = FrameID;       
        this.DestAdr64 = Adr;       
        this.DestAdr16 = "FFFE";       
        this.cmdOptions = apiCmdOptions.None;       
        this.ATcmd = Pin.ToString();
        this.UpdateAllBytes();
    }

    private void SetPinStatus(bool EnableTrigger, XbeeAPIpin Pin, string Adr, byte FrameID)
    {
        this.GetPinStatus(Pin, Adr, FrameID);
        this.CheckSum = 0x0;
        this.Length = 16;        
        this.cmdOptions = apiCmdOptions.ApplyChanges;        
        if(EnableTrigger)
        {
            this.mCmdParameter = 0x3;
        }
        else
        {
            this.mCmdParameter = 0x0;
        }
        this.UpdateAllBytes();
    }

    private void ParseRemoteCmdResponsBytes(byte[]bytes)
    {
        if (bytes[3] == 0x97)
        {
            this.Delimiter = bytes[0];
            this.Length0 = bytes[1];
            this.Length = bytes[2];
            this.API = (XbeeBasePacket.XbeePacketType)Enum.Parse(typeof(XbeeBasePacket.XbeePacketType), bytes[3].ToString());
            this.FrameID = bytes[4];
            this.mDestAddress1 = bytes[5];
            this.mDestAddress2 = bytes[6];
            this.mDestAddress3 = bytes[7];
            this.mDestAddress4 = bytes[8];
            this.mDestAddress5 = bytes[9];
            this.mDestAddress6 = bytes[10];
            this.mDestAddress7 = bytes[11];
            this.mDestAddress8 = bytes[12];
            this.mDestShortAdr1 = bytes[13];
            this.mDestShortAdr2 = bytes[14];
            List<char> chars = new List<char>();
            chars.Add((char)bytes[15]);
            chars.Add((char)bytes[16]);
            this.mATcmd = chars.ToArray();
            this.mRemoteStatus = (RemoteCmdResponsStatus)Enum.Parse(typeof(RemoteCmdResponsStatus), bytes[17].ToString());
            if (this.Length == 15) //0x0F
            {
                //this is a getter packet, no commanddata (commanddata)
                this.CheckSum = bytes[18];
            }
            if (this.Length >= 16) //0x10
            {
                //this is a setter packet, parameters are included (commanddata)

                //this.mCmdRemoteData = bytes[18];
                for (int i = 18; i < (bytes.Length - 1); i++)
                {
                    this.mCmdRemoteData.Add(bytes[i]);
                }
                int lastbyte = bytes.Length;
                this.CheckSum = bytes[bytes.Length - 1];
            }
            this.AllBytes.AddRange(bytes);  
            this.Direction = XbeeBasePacket.XbeePacketDirection.In;
        }
    }

    private void ParseRemoteCMDBytes(byte[] bytes)
    {
        if (bytes[3] == 0x17)
        {
            //this is a RemoteCmd packet 0x17
            this.Delimiter = bytes[0];
            this.Length0 = bytes[1];
            this.Length = bytes[2];
            this.API = (XbeeBasePacket.XbeePacketType)Enum.Parse(typeof(XbeeBasePacket.XbeePacketType), bytes[3].ToString());
            this.FrameID = bytes[4];
            this.mDestAddress1 = bytes[5];
            this.mDestAddress2 = bytes[6];
            this.mDestAddress3 = bytes[7];
            this.mDestAddress4 = bytes[8];
            this.mDestAddress5 = bytes[9];
            this.mDestAddress6 = bytes[10];
            this.mDestAddress7 = bytes[11];
            this.mDestAddress8 = bytes[12];
            this.mDestShortAdr1 = bytes[13];
            this.mDestShortAdr2 = bytes[14];
            this.cmdOptions = (apiCmdOptions)Enum.Parse(typeof(apiCmdOptions), bytes[15].ToString());
            List<char> chars = new List<char>();
            chars.Add((char)bytes[16]);
            chars.Add((char)bytes[17]);
            this.mATcmd = chars.ToArray();
            //this.ATcmd = Util.ConvertToHex(bytes[15]) + Util.ConvertToHex(bytes[16]);
            if (this.Length == 15) //0x0F
            {
                //this is a getter packet, no parametervalue (cmdData)
                this.CheckSum = bytes[18];
            }
            if (this.Length == 16) //0x10
            {
                //this is a setter packet, parameters are included (cmdData)
                this.mCmdParameter = bytes[18]; 
                this.CheckSum = bytes[19];
            }
            this.AllBytes.AddRange(bytes);
            this.Direction = XbeeBasePacket.XbeePacketDirection.Out;
        }
    }

    public enum apiCmdType : int
    {
        get = 0x0,
        set = 0x1        
    }
    public enum apiCmdOptions : byte
    {
        None = 0x0,
        DisableACK = 0x1,
        ApplyChanges = 0x2,
        ExtendedTimeout = 0x40
    }
    public enum XbeeAPIpin : byte
    {
        D0 = 20,
        D1 = 19,
        D2 = 18,
        D3 = 17
    }
    public enum XbeeAPIpinIndex : byte
    {
        D0 = 0x30,
        D1 = 0x31,
        D2 = 0x32,
        D3 = 0x33
    }
    public enum XbeePin : int
    {
        //INT is Module pinnumber, name is portnumber
        p2 = 4,
        p0 = 0,
        p1 = 7,
        d8 = 9,
        d4 = 11,
        d7 = 12,
        d9 = 13,
        d5 = 15,
        d6 = 16,
        d3 = 17,
        d2 = 18,
        d1 = 19,
        d0 = 20
    }
    public enum RemoteCmdResponsStatus : byte
    {
        OK = 0x0,
        Error = 0x1,
        InvalidCommand = 0x2,
        InvalidParameter = 0x3,
        TransmissionFailed = 0x4

    }

    //http://derekwill.com/2015/03/05/bit-processing-in-c/
    [Flags]
    public enum DigitalMask : short
    {
        None = 0,
        D0 = 1,     //00000000 00000001
        D1 = 2,     //00000000 00000010
        D2 = 4,     //00000000 00000100
        D3 = 8,     //00000000 00001000
        D4 = 16,    //00000000 00010000
        D5 = 32,    //00000000 00100000
        D6 = 64,    //00000000 01000000
        D7 = 128,   //00000000 10000000
        D8 = 256,   //00000001 00000000
        D9 = 512,   //00000010 00000000
        D10 = 1024, //00000100 00000000
        D11 = 2048, //00001000 00000000
        D12 = 4096  //00010000 00000000
                         //11000 00010000
    }

    [Flags]
    public enum AnalogMask
    {
        None = 0,
        A0 = 1,  //00000000 00000001
        A1 = 2,  //00000000 00000010
        A2 = 4,  //00000000 00000100
        A3 = 8   //00000000 00001000
    }

}
