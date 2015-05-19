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

    public void addPacket(XbeeStruct.DataSampleStruct dsStruct)
    {
        this.LogIt("Adding struct packet");
        DataSamplePacket ds = new DataSamplePacket();
        ds.API = XbeeBasePacket.XbeePacketType.DataSample;
        ds.CheckSum = dsStruct.CheckSum;
        ds.Id = this.List.Count;
        ds.Length = dsStruct.Length;
        ds.Samples = dsStruct.SamplesAsHex;
        ds.SourceAdr16 = dsStruct.SourceAdr16;
        ds.SourceAdr64 = dsStruct.SourceAdr64;
        ds.TimeDate = DateTime.Now;
        ds.Time = DateTime.Now.ToLongTimeString();
        this.List.Add(ds);
    }

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
    public string Samples { get; set; }
    public string SourceAdr16 { get; set; }
    public string SourceAdr64 { get; set; }

    public DataSamplePacket()
    {
    }
}
