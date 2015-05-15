using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


class XbeeBasePacket
{
    public List<byte[]> PacketByteList { get; set; }
    public List<byte> PacketBytes { get; set; }
    public bool Vaild { get; set; }
    public bool Dropped { get; set; }
    public int ListLength { get; set; }
    public int PacketLength { get; set; }
    public bool EscapeCharFlag { get; set; }
    public XbeePacketType PacketType { get; set; }
    public int EscapeCharCount { get; set; }
    // Events and delegates
    public event LogEventEventHandler LogEvent;
    public delegate void LogEventEventHandler(LogDetail LogItem);
    public event VaildPacketEventHandler VaildPacket;
    public delegate void VaildPacketEventHandler(string hex);

    public XbeeBasePacket()
    {
        this.PacketByteList = new List<byte[]>();
        this.PacketBytes = new List<byte>();
        this.Vaild = false;
        this.ListLength = 0;
        this.PacketLength = 0;
        this.EscapeCharCount = 0;
    }

    public XbeeBasePacket(byte[] PacketBytes)
    {
        foreach (byte b in PacketBytes) 
        {
            this.PacketBytes.Add(b);
            string s = Util.ConvertToHex(b);
        }

        this.ListLength = this.PacketBytes.Count;
        if (this.PacketBytes.Count >= 3) 
        {
            this.PacketLength = this.PacketBytes[1] + this.PacketBytes[2];
            if (this.PacketLength == (this.ListLength - 5)) 
            {
                this.Vaild = true;
                this.PacketType = (XbeePacketType)Enum.Parse(typeof(XbeePacketType), this.PacketBytes[3].ToString());
            }
        }

    }

    public string GetPacketAsHex()
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte byten in this.PacketBytes)
        {
            sb.Append(Util.ConvertToHex(byten));
            sb.Append(" ");
        }
        char[] WhiteSpace = { ' ' };
        return sb.ToString().TrimEnd(WhiteSpace);
    }

    public XbeeBasePacket(string hex)
    {
        char[] SplitSpace = { ' ' };
        foreach (string s in hex.Split(SplitSpace))
        {
            int numb = Util.ConvertHexToInt(s);
            PacketBytes.Add((byte)numb);
        }
        this.ListLength = this.PacketBytes.Count;
        this.PacketLength = this.PacketBytes[1] + this.PacketBytes[2];
        if (this.PacketLength == (this.ListLength - 5)) 
        {
            this.Vaild = true;
            this.PacketType = (XbeePacketType)Enum.Parse(typeof(XbeePacketType), this.PacketBytes[3].ToString());
        }
    }

    private void ResetPacket()
    {
        this.Vaild = false;
        this.PacketLength = 0;
        this.ListLength = 0;
        this.PacketBytes = new List<byte>();
        this.EscapeCharFlag = false;
        this.EscapeCharCount = 0;
    }

    public async Task<bool> AddByte(byte thebyte)
    {
        if (this.Vaild == true) {
            this.LogIt("WARNING - Resetting packet");
            this.ResetPacket();
        }
        if (this.EscapeCharFlag == true) {
            this.EscapeCharFlag = false;
            this.LogIt("EscapeFlag was TRUE, now false");
            thebyte = (byte)(thebyte ^ 0x20); //&H20
            this.LogIt("thebyte:" + thebyte.ToString());
        }
        //check for escapechar
        if (thebyte == 0x7D & this.EscapeCharFlag == false) {
            this.LogIt("received escapechar, setting flag to TRUE");
            this.EscapeCharCount += 1;
            this.EscapeCharFlag = true;
        }
        else
        {
            this.PacketBytes.Add(thebyte);
        }
                
        if (this.PacketBytes[0] == 0x7E) {
            //we have a valid packet start byte
        } else {
            this.LogIt("Error - Delimmiter is:" + Util.ConvertToHex(thebyte));
            //something is wrong, invalid packet, reset (should notify about dropping bytes)
            this.LogIt("Dropping frame, invalid packet/frame due to delimiter error");
            //TODO - create badpacket log/list with reason (checksumERROR, delimmiter error, length error)
            return false;
        }
        string ByteAsHex = Util.ConvertToHex(thebyte);
       
        this.ListLength = this.PacketBytes.Count;
        if (this.PacketBytes.Count >= 3) 
        {
            this.PacketLength = this.PacketBytes[1] + this.PacketBytes[2];
            if (this.PacketLength == (this.ListLength - this.EscapeCharCount - 4)) {
                int intCalculatedChecksum = Util.ComputeChecksum(this.PacketBytes.ToArray());
                int packetCheckSum = Convert.ToInt32(this.PacketBytes[this.PacketBytes.Count - 1]);
                //if checksum match then raise event Vaildpacket
                if (intCalculatedChecksum == packetCheckSum) 
                {
                    this.LogIt("Checksum OK");
                    this.Vaild = true;
                    this.LogIt("Valid packet received");
                    this.XbeeBasePacket_Vaild();
                } 
                else 
                {
                    this.LogIt("BAD checksum, resetting/dropping");
                    //this.ResetPacket();
                    this.Dropped = true;                    
                }
            }
        }
        return true;
    }

    public void XbeeBasePacket_Vaild()
    {
        if (VaildPacket != null)
        {
            // Raise ValidPacket event
            VaildPacket(this.GetPacketAsHex());
        }
        else
        {
            this.LogIt("Warning unable to send event since ValidPacket is null");
        }
    }

    public void AddByte(byte[] ArrayOfbytes)
    {
        foreach (byte byten in ArrayOfbytes)
        {
            this.AddByte(byten);
        }
    }

    public async Task<bool> AddByte(string PacketAsHex)
    {
        char[] Split = { ' ' };

        if(PacketAsHex.Contains(" "))
        {
            foreach (string hex in PacketAsHex.Split(Split))
            {
                await this.AddByte(Util.ConvertHexToByte(hex));
            }
        }
        return true;
    }

    private void LogIt(string Str)
    {
        LogDetail log = new LogDetail();
        string calledby = new StackFrame(4, true).GetMethod().Name;
        string ClassName = this.GetType().FullName;
        log.ClassName = ClassName;
        log.Description = Str;
        log.Level = 0;
        log.Method = calledby;
        log.TimeDate = DateTime.Now;

        if (LogEvent != null)
        {
            LogEvent(log);
        }
    }

    public enum XbeePacketType
    {
        TransmitRequest = 0x10,
        RemoteCmdRespons = 0x97,
        DataSample = 0x92,
        RemoteCmd = 0x17,
        ReceivePacket = 0x90
    }        
}

