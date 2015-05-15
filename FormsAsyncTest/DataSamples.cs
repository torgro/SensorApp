using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DataSamples
{
    List<DataSamplePacket> List { get; set; }

    public DataSamples()
    {
        this.List = new List<DataSamplePacket>();
    }

    public void addPacket(XbeeStruct.DataSampleStruct dsStruct)
    {
        DataSamplePacket ds = new DataSamplePacket();
        ds.API = dsStruct.GetXbeePacketType();
        ds.CheckSum = dsStruct.CheckSum;
        ds.Id = this.List.Count;
        ds.Length = dsStruct.length;
        ds.Samples = dsStruct.Samples;
        ds.SourceAdr16 = dsStruct.SourceAdr16;
        ds.SourceAdr64 = dsStruct.SourceAdr64;
        ds.TimeDate = DateTime.Now;
        ds.Time = DateTime.Now.ToLongTimeString();
        this.List.Add(ds);
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
