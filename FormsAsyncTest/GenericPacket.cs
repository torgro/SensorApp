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
            this.Packets.Add(packet);
        }    
    }
}


public class GenericPacket
{
    public string Delimiter { get; set; }
    public int PacketLength { get; set; }
    public byte API { get; set; }
    public int FrameID { get; set; }
    public byte CheckSum { get; set; }
    public List<byte> PacketBytes { get; set; }
    public string Hex { get; set; }
    public string SourceAddress { get; set; }

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
        this.PacketLength = (PacketOfBytes[1] + PacketOfBytes[2]);
        this.API = PacketOfBytes[3];
        this.FrameID = PacketOfBytes[4];
        int StartIndexAddress = this.GetSourceAddressIndex();
        for (int i = StartIndexAddress; i < (StartIndexAddress + 8); i++)
        {
            this.SourceAddress += Util.ConvertToHex(PacketOfBytes[i]);
        }
        //this.SourceAddress += Util.ConvertToHex(PacketOfBytes[5]);
        //this.SourceAddress += Util.ConvertToHex(PacketOfBytes[6]);
        //this.SourceAddress += Util.ConvertToHex(PacketOfBytes[7]);
        //this.SourceAddress += Util.ConvertToHex(PacketOfBytes[8]);
        //this.SourceAddress += Util.ConvertToHex(PacketOfBytes[9]);
        //this.SourceAddress += Util.ConvertToHex(PacketOfBytes[10]);
        //this.SourceAddress += Util.ConvertToHex(PacketOfBytes[11]);
        this.CheckSum = PacketOfBytes[PacketOfBytes.Length -1];
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

