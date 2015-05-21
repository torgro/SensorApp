using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


public class DataSamples
{
    public List<DataSamplePacket> List { get; set; }
    // Events and delegates
    public event LogEventEventHandler LogEvent;
    public delegate void LogEventEventHandler(LogDetail LogItem);

    public DataSamples()
    {
        this.List = new List<DataSamplePacket>();
    }

    public void AddPacket(DataSamplePacket packet)
    {
        packet.Id = this.List.Count;
        packet.TimeDate = DateTime.Now;
        packet.Time = DateTime.Now.ToLongTimeString();
        this.List.Add(packet);
    }

    public void AddPacket(GenericPacket GenericPack)
    {
        DataSamplePacket packet = GenericPack.ToDataSamplePacket();
        if(packet != null)
        {
            packet.Id = this.List.Count;
            packet.TimeDate = DateTime.Now;
            packet.Time = DateTime.Now.ToLongTimeString();
            this.List.Add(packet);
        }
        else
        {
            this.LogIt("Unable to add sample packet, ToDataSamplePacket returned null");
        }
    }

    //private void addPacketOLD(XbeeStruct.DataSampleStruct dsStruct)
    //{
    //    this.LogIt("Adding struct packet");
    //    DataSamplePacket ds = new DataSamplePacket();
    //    ds.API = XbeeBasePacket.XbeePacketType.DataSample;
    //    ds.CheckSum = dsStruct.CheckSum;
    //    ds.Id = this.List.Count;
    //    ds.Length = dsStruct.Length;
    //    //ds.Samples = dsStruct.SamplesAsHex;
    //    ds.SourceAdr16 = dsStruct.SourceAdr16;
    //    ds.SourceAdr64 = dsStruct.SourceAdr64;
    //    ds.TimeDate = DateTime.Now;
    //    ds.Time = DateTime.Now.ToLongTimeString();
    //    this.List.Add(ds);
    //}

    private void LogIt(string Str)
    {
        LogDetail log = new LogDetail();
        string calledby = new StackFrame(3, true).GetMethod().Name;
        string ClassName = this.GetType().FullName;
        log.ClassName = ClassName;
        log.Description = Str;
        log.Level = 0;
        log.Method = calledby;
        log.TimeDate = DateTime.Now;

        if (this.LogEvent != null)
        {
            this.LogEvent(log);
        }
    }
}

public class DataSamplePacket
{
    public DateTime TimeDate { get; set; }
    public string Time { get; set; }
    public int Id { get; set; }
    public XbeeBasePacket.XbeePacketType API { get; set; }
    public byte CheckSum { get; set; }
    public ushort Length { get; set; }
    public string Samples
    {
        get
        {
            string StringSample = string.Empty;
            foreach (byte singlebyte in this.DigitalSamples)
            {
                StringSample += Util.ConvertToBin(singlebyte);
            }
            return StringSample;
        }
    }
    //public string SourceAdr16 { get; set; }
    //public string SourceAdr64 { get; set; }
    public string SourceAdr16
    {
        get
        {
            string ShortMac = this.ShortAdr1.ToString("x2") + this.ShortAdr2.ToString("x2");
            return ShortMac.ToUpper();
        }
        set
        {
            if (value.Length == 4)
            {
                this.ShortAdr1 = (byte)Util.ConvertHexToInt(value.Substring(0, 2));
                this.ShortAdr2 = (byte)Util.ConvertHexToInt(value.Substring(2, 2));
            }
        }
    }
    public string SourceAdr64
    {
        get
        {
            string mac = this.Address1.ToString("x2") + this.Address2.ToString("x2") + this.Address3.ToString("x2") + this.Address4.ToString("x2");
            mac = mac + this.Address5.ToString("x2") + this.Address6.ToString("x2") + this.Address7.ToString("x2") + this.Address8.ToString("x2");
            return mac.ToUpper();
        }
        set
        {
            if (value.Length == 16)
            {
                this.Address1 = (byte)Util.ConvertHexToInt(value.Substring(0, 2));
                this.Address2 = (byte)Util.ConvertHexToInt(value.Substring(2, 2));
                this.Address3 = (byte)Util.ConvertHexToInt(value.Substring(4, 2));
                this.Address4 = (byte)Util.ConvertHexToInt(value.Substring(6, 2));
                this.Address5 = (byte)Util.ConvertHexToInt(value.Substring(8, 2));
                this.Address6 = (byte)Util.ConvertHexToInt(value.Substring(10, 2));
                this.Address7 = (byte)Util.ConvertHexToInt(value.Substring(12, 2));
                this.Address8 = (byte)Util.ConvertHexToInt(value.Substring(14, 2));
                this.mDestAdr64Bytes.Add(this.Address1);
                this.mDestAdr64Bytes.Add(this.Address2);
                this.mDestAdr64Bytes.Add(this.Address3);
                this.mDestAdr64Bytes.Add(this.Address4);
                this.mDestAdr64Bytes.Add(this.Address5);
                this.mDestAdr64Bytes.Add(this.Address6);
                this.mDestAdr64Bytes.Add(this.Address7);
                this.mDestAdr64Bytes.Add(this.Address8);

            }
            else
            {
                throw new Exception("Sourceaddress must be 16 chars long");
            }
        }

    }
    public List<byte> PacketBytes;
    public byte Delimitter;
    private byte Length0;
    private byte Address1;
    private byte Address2;
    private byte Address3;
    private byte Address4;
    private byte Address5;
    private byte Address6;
    private byte Address7;
    private byte Address8;
    private byte ShortAdr1;
    private byte ShortAdr2;
    private byte ReceiveOptions;
    private List<byte> DigitalMask;
    private byte AnalogMask;
    private List<byte> DigitalSamples;
    private byte NumberOfSamples;
    private List<byte> mAnalogSamples;
    private List<byte> mDestAdr64Bytes;
    public DataSamplePacket()
    {
        this.setDefaults();
    }

    public DataSamplePacket(byte[] bytes)
    {
        this.setDefaults();
        if (bytes[3] == 0x92)
        {
            //packet is datasample
            this.ParseBytes(bytes);
        }
    }

    private void setDefaults()
    {
        this.PacketBytes = new List<byte>();
        this.DigitalMask = new List<byte>();
        this.DigitalSamples = new List<byte>();
        this.mDestAdr64Bytes = new List<byte>();
        this.mAnalogSamples = new List<byte>();
    }

    private void ParseBytes(byte[] bytes)
    {
        this.PacketBytes.AddRange(bytes);
        this.Delimitter = bytes[0];
        this.Length0 = bytes[1];
        this.Length = bytes[2];
        this.API = (XbeeBasePacket.XbeePacketType)Enum.Parse(typeof(XbeeBasePacket.XbeePacketType), bytes[3].ToString());
        this.Address1 = bytes[4];
        this.Address2 = bytes[5];
        this.Address3 = bytes[6];
        this.Address4 = bytes[7];
        this.Address5 = bytes[8];
        this.Address6 = bytes[9];
        this.Address7 = bytes[10];
        this.Address8 = bytes[11];
        this.ShortAdr1= bytes[12];
        this.ShortAdr2 = bytes[13];
        this.ReceiveOptions = bytes[14];
        this.NumberOfSamples = bytes[15];
        this.DigitalMask.Add(bytes[16]);
        this.DigitalMask.Add(bytes[17]);
        this.AnalogMask = bytes[18];
        this.DigitalSamples.Add(bytes[19]);
        this.DigitalSamples.Add(bytes[20]);
        if(this.Length == 18)
        {
            
        }
        if (this.Length > 18)
        {
            byte[] analogSampleBytes = this.PacketBytes.Skip(21).Take(this.PacketBytes.Count -2).ToArray();
            this.mAnalogSamples.AddRange(analogSampleBytes);
        }
        this.CheckSum = this.PacketBytes.Last();
    }
}
