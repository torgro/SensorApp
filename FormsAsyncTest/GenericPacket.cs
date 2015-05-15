using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GenericPackets
{
    public List<GenericPacket> Packets;

    public GenericPackets() 
    {
        this.Packets = new List<GenericPacket>();
    }

    public void AddGenericPacket(byte[] PacketOfBytes)
    {
        GenericPacket packet = new GenericPacket(PacketOfBytes);
        if (packet.PacketBytes.Count > 0)
        {
            packet.ID = this.Packets.Count;
            packet.Time = DateTime.Now;
            this.Packets.Add(packet);
            
        }    
    }
}


public class GenericPacket
{
    public int ID { get; set; }
    public DateTime Time { get; set; }
    public string Delimiter;
    public int Length { get; set; }
    public byte API { get; set; }
    public int FrameID { get; set; }
    public byte CheckSum { get; set; }
    public List<byte> PacketBytes { get; set; }
    public string Hex { get; set; }
    public string SourceAddress { get; set; }
    public XbeeBasePacket.XbeePacketType APItype { get; set; }
    public XbeeBasePacket.XbeePacketDirection Direction { get; set; }

    public GenericPacket()
    {
        this.PacketBytes = new List<byte>();
    }

    public GenericPacket(byte[] PacketOfBytes)
    {
        this.PacketBytes = new List<byte>();
        if (PacketOfBytes.Length == 0)
        {
            return;
        }
        
        this.PacketBytes.AddRange(PacketOfBytes);
        XbeeBasePacket packet = new XbeeBasePacket(PacketOfBytes);
        this.Hex = packet.GetPacketAsHex();
        this.Delimiter = Util.ConvertToHex(PacketOfBytes[0]);
        this.Length = (PacketOfBytes[1] + PacketOfBytes[2]);
        this.API = PacketOfBytes[3];
        this.FrameID = PacketOfBytes[4];
        int StartIndexAddress = this.GetSourceAddressIndex();
        for (int i = StartIndexAddress; i < (StartIndexAddress + 8); i++)
        {
            this.SourceAddress += Util.ConvertToHex(PacketOfBytes[i]);
        }
        this.CheckSum = PacketOfBytes[PacketOfBytes.Length -1];
        this.APItype = (XbeeBasePacket.XbeePacketType)Enum.Parse(typeof(XbeeBasePacket.XbeePacketType), this.API.ToString());
        this.Direction = GetPacketDirection(this.APItype);
    }

    private XbeeBasePacket.XbeePacketDirection GetPacketDirection(XbeeBasePacket.XbeePacketType APItype)
    {
        XbeeBasePacket.XbeePacketDirection Direction;
        switch (APItype)
        {
            case XbeeBasePacket.XbeePacketType.TransmitRequest:
                Direction = XbeeBasePacket.XbeePacketDirection.In;
                break;
            case XbeeBasePacket.XbeePacketType.RemoteCmdRespons:
                Direction = XbeeBasePacket.XbeePacketDirection.In;
                break;
            case XbeeBasePacket.XbeePacketType.DataSample:
                Direction = XbeeBasePacket.XbeePacketDirection.In;
                break;
            case XbeeBasePacket.XbeePacketType.RemoteCmd:
                Direction = XbeeBasePacket.XbeePacketDirection.Out;
                break;
            case XbeeBasePacket.XbeePacketType.ReceivePacket:
                Direction = XbeeBasePacket.XbeePacketDirection.In;
                break;
            default:
                Direction = XbeeBasePacket.XbeePacketDirection.In;
                break;
        }
        return Direction;
    }
    
    private int GetSourceAddressIndex()
    {
        int returnInt = 5;
        if (this.API == 0x92)
        {
            returnInt = 4;
        }
        return returnInt;
    }
}

