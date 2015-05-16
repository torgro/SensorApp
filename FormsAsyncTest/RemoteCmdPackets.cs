using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RemoteCmdPackets
{
    public List<RemoteCmdPacket> List;

    public RemoteCmdPackets()
    {
        this.List = new List<RemoteCmdPacket>();
    }

    public void AddPacket(XbeeStruct.RemoteCmdStruckt StructPacket)
    {
        RemoteCmdPacket packet = new RemoteCmdPacket();
        packet.TimeDate = DateTime.Now;
        packet.Id = this.List.Count;
        packet.CheckSum = StructPacket.Checksum;
        packet.API = XbeeBasePacket.XbeePacketType.RemoteCmd;
        string test = StructPacket.mLength.ToString();
        packet.Length = StructPacket.Length;
        if (packet.Length == 15)
        {
            packet.CheckSum = StructPacket.CmdData;
            packet.CmdData = 0;
        }
        packet.ATcmd = StructPacket.ATcmd;
        packet.CmdData = StructPacket.CmdData;
        packet.cmdOptions = StructPacket.CmdOptions;
        packet.FrameID = StructPacket.FrameID;
        packet.DestAdr16 = StructPacket.DestAdr16;
        packet.DestAdr64 = StructPacket.DestAdr64;
        this.List.Add(packet);
    }
}

public class RemoteCmdPacket
{
    public DateTime TimeDate { get; set; }
    public int Id { get; set; }
    public XbeeBasePacket.XbeePacketType API { get; set; }
    public byte FrameID { get; set; }
    public byte CheckSum { get; set; }
    public byte Length { get; set; }
    public string ATcmd { get; set; }
    public string PinName { get; set; }
    public byte cmdOptions { get; set; }
    public byte CmdData { get; set; }
    public string DestAdr16 { get; set; }
    public string DestAdr64 { get; set; }
    public XbeeBasePacket.XbeePacketDirection Direction { get; set; }
    private byte DigitalPinHex = 0x44;
    public List<byte> AllBytes;
    public string PacketAsHex;

    public RemoteCmdPacket()
    {
        this.setDefaults();
    }

    private void setDefaults()
    {
        this.CmdData = 0xff;
        this.ATcmd = string.Empty;
        this.Length = 0x0;
    } 

    public byte[] GetPinStatusPacket(XbeeAPIpin Pin, String DestinationAddress, byte FrameID)
    {
        XbeeStruct.RemoteCmdStruckt cmd = new XbeeStruct.RemoteCmdStruckt();
        cmd.ATcmd = Pin.ToString();
        cmd.DestAdr64 = DestinationAddress;
        cmd.FrameID = FrameID;
        cmd.DestAdr16 = "FFFE";
        cmd.CmdOptions = (byte)apiCmdOptions.None;
        cmd.API = (byte)XbeeBasePacket.XbeePacketType.RemoteCmd;
        cmd.Delimiter = 0x7E;
        cmd.Length = 15;
        byte[] bytes = Util.StructToBytes<XbeeStruct.RemoteCmdStruckt>(cmd);
        cmd.Checksum = (byte)Util.ComputeChecksum(bytes);
        bytes = Util.StructToBytes<XbeeStruct.RemoteCmdStruckt>(cmd);
        List<byte> list = new List<byte>();
        list.AddRange(bytes);
        list.RemoveAt(18);
        return list.ToArray<byte>();
    }

    public XbeeStruct.RemoteCmdStruckt SetPinStatus(XbeeAPIpin Pin, String DestinationAddress, byte FrameID, bool EnablePin)
    {
        XbeeStruct.RemoteCmdStruckt cmd = new XbeeStruct.RemoteCmdStruckt();
        cmd.ATcmd = Pin.ToString();
        cmd.DestAdr64 = DestinationAddress;
        cmd.FrameID = FrameID;
        cmd.DestAdr16 = "FFFE";
        cmd.CmdOptions = (byte)apiCmdOptions.None;
        cmd.API = (byte)XbeeBasePacket.XbeePacketType.RemoteCmd;
        cmd.Delimiter = 0x7E;
        cmd.Length = 16;
        if (EnablePin == true)
        {
            cmd.CmdData = 0x33;
        }
        byte[] bytes = Util.StructToBytes<XbeeStruct.RemoteCmdStruckt>(cmd);
        cmd.Checksum = (byte)Util.ComputeChecksum(bytes);
        bytes = Util.StructToBytes<XbeeStruct.RemoteCmdStruckt>(cmd);
        string hex = Util.ConvertByteArrayToHexString(bytes);
        return cmd;
    }

    private ushort CalcStructLength(byte[] ArrayOfBytes)
    {
        ushort ReturnLength = 0;
        ReturnLength = (ushort)(ArrayOfBytes.Length - 4);
        return ReturnLength;
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
}
