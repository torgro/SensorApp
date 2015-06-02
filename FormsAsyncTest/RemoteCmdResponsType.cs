using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    //public class RemoteCmdResponsType
    //{
    //    public ISpacket isPacket;
    //    public RemoteCmdResponsType() {}

    //    public void ParseRemoteCmdResponsPacket(RemoteCmdPacket pack)
    //    {
    //        switch (pack.ATcmd.ToString().ToUpper())
    //        {
    //            case "IS":
    //            {
    //                this.isPacket = new ISpacket();
    //                this.isPacket.ParseFullPacket(pack.ToGenericPacket());
    //                break;
    //            }
    //            default:
    //                break;
    //        }
    //    }
    //}
namespace RemoteCmdResponsType
{
    public class ISpacket
    {
        public int NumberOfSamples { get; set; }
        public RemoteCmdPacket.DigitalMask DigitalChannelMask { get; set; }
        public RemoteCmdPacket.AnalogMask AnalogChannelMask { get; set; }
        public RemoteCmdPacket.DigitalMask DigitalSamples { get; set; }
        public System.Collections.Generic.List<byte[]> AnalogSamples { get; set; }
        public string SourceAddress { get; set; }

        public ISpacket()
        {
            this.AnalogSamples = new List<byte[]>();
            this.SourceAddress = string.Empty;
        }

        private void ParseFullPacket(byte[] bytes)
        {
            if (bytes[3] != 0x97)
            {
                // this is not a RemoteCmdResponsISpacket
                return;
            }

            if (bytes[15] != 0x49 | bytes[16] != 0x53)
            {
                // command is not IS
                return;
            }

            if (bytes.Length >= 22)
            {
                //18 = Number of samples
                this.NumberOfSamples = bytes[18];
                //19 og 20 = DigitalMask
                if (bytes[19] > 0 || bytes[20] > 0)
                {
                    byte[] by = new byte[] { bytes[19], bytes[20] };
                    var digMask = BitConverter.ToInt32(by, 0);
                    this.DigitalChannelMask = (RemoteCmdPacket.DigitalMask)digMask;
                }
                else
                {
                    this.DigitalChannelMask = RemoteCmdPacket.DigitalMask.None;
                }                
                //21 = AnalogMask
                this.AnalogChannelMask = (RemoteCmdPacket.AnalogMask)bytes[21];
            }                                                 

            if (this.DigitalChannelMask != RemoteCmdPacket.DigitalMask.None)
            {
                if (bytes.Length > 23)
                {
                    //22 og 23 = DigitalSample (bit mask)
                    this.DigitalSamples = (RemoteCmdPacket.DigitalMask)BitConverter.ToInt32(new byte[] { bytes[22], bytes[23] }, 0);
                }
            }

            if (this.AnalogChannelMask != RemoteCmdPacket.AnalogMask.None)
            {
                switch (this.DigitalChannelMask)
                {
                    case RemoteCmdPacket.DigitalMask.None:                        
                        if (bytes.Length > 23)
                        {
                            //24 og 25 = AnalogSample1 (if analog mask is != 00
                            this.AnalogSamples.Add(new byte[] { bytes[22], bytes[23] });
                        }

                        if (bytes.Length > 25)
                        {
                            //26 og 27 = AnalogSample2 (if analog mask is != 00
                            this.AnalogSamples.Add(new byte[] { bytes[24], bytes[25] });
                        }

                        if (bytes.Length > 27)
                        {
                            //28 og 29 = AnalogSample3 (if analog mask is != 00
                            this.AnalogSamples.Add(new byte[] { bytes[26], bytes[27] });
                        }

                        if (bytes.Length > 29)
                        {
                            //30 og 31 = AnalogSample4 (if analog mask is != 00
                            this.AnalogSamples.Add(new byte[] { bytes[28], bytes[29] });
                        }
                        break;
                    default:
                        if (bytes.Length > 25)
                        {
                            //24 og 25 = AnalogSample1 (if analog mask is != 00
                            this.AnalogSamples.Add(new byte[] { bytes[24], bytes[25] });
                        }

                        if (bytes.Length > 27)
                        {
                            //26 og 27 = AnalogSample2 (if analog mask is != 00
                            this.AnalogSamples.Add(new byte[] { bytes[26], bytes[27] });
                        }

                        if (bytes.Length > 29)
                        {
                            //28 og 29 = AnalogSample3 (if analog mask is != 00
                            this.AnalogSamples.Add(new byte[] { bytes[28], bytes[29] });
                        }

                        if (bytes.Length > 31)
                        {
                            //30 og 31 = AnalogSample4 (if analog mask is != 00
                            this.AnalogSamples.Add(new byte[] { bytes[30], bytes[31] });
                        }
                        break;
                }                
            }
        }

        public void ParseFullPacket(GenericPacket packet)
        {
            this.ParseFullPacket(packet.PacketBytes.ToArray());
            if (this.NumberOfSamples > 0)
            {
                this.SourceAddress = packet.SourceAddress;
            }            
        }
    }
}
