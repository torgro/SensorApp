using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Threading;

namespace FormsAsyncTest
{
    public partial class Form1 : Form
    {
        public delegate void delegateXbeeTest(string msg);
        private XbeeCOM serial = new XbeeCOM();
        private MonitorDevices Devices = new MonitorDevices();
        private XbeeBasePacket Packet = new XbeeBasePacket();
        private GenericPackets GenericPackets = new GenericPackets();
        private Logging Logs = new Logging();
        private Stat Statistics = new Stat();
        private int IRC = 0;
        private System.Windows.Forms.Timer ticks;
        private GridViewMode CurrentGridView = GridViewMode.Log;

        public Form1()
        {
            InitializeComponent();
           
            //this.man.XbeeTest += this.InboundXbeeTestEvent;
            this.serial.LogEvent += this.logit;
            this.Packet.LogEvent += this.logit;
            this.Devices.LogEvent += this.logit;
            this.Statistics.AddStats(new Stats());
            this.data_Stats.SuspendLayout();
            this.data_Stats.DataSource = this.Statistics.StatParis;
            this.data_Stats.ResumeLayout();
            this.ticks = new System.Windows.Forms.Timer();
            this.ticks.Interval = 3000;
            this.ticks.Enabled = true;
            this.ticks.Tick += this.Ticks_Elapsed;
            MonitorDevice mon = new MonitorDevice();
            mon.Enabled = true;
            mon.MAC = "0013A20040A1D8CE";
            mon.GPSlat = "59,664874";
            mon.GPSlong = "6,444361";
            mon.Name = "bøen";
            this.Devices.AddDevice(mon);
        //    this.tics = new System.Timers.Timer(3000);
        //    this.tics.Elapsed += this.Ticks_Elapsed;
        //    this.tics.SynchronizingObject = this;
        //    this.tics.Enabled = true;
        }
        private async void Ticks_Elapsed(object Sender, EventArgs e)
        {
            this.logit(new LogDetail("EVENT_Updating statistics"));
            this.IRC++;
            await this.UpdateStats();
            this.UpdateStatsView();
        }

        //private async Task<bool> UpdateStats()
        private async Task<bool> UpdateStats()
        {         
            bool GetNewStats = await Task.Run(() =>
            {
                bool ReturnValue = true;                
                Stats stats = new Stats();
                stats.ComPortInterrupts = this.IRC.ToString();
                stats.Packets = this.GenericPackets.Packets.Count.ToString();
                Statistics.AddStats(stats);
                return ReturnValue;
            });
            
            return true;
        }

        private void UpdateStatsView()
        {
            this.data_Stats.SuspendLayout();
            this.data_Stats.DataSource = this.Statistics.StatParis;
            this.data_Stats.ResumeLayout();
        }

        private async Task<bool> UpdateStatsViewAsync()
        {
            return await Task.Run(() => 
                {
                    bool retValue = true;
                    this.data_Stats.Refresh();
                    return retValue;
                });
        }

        private void logit(LogDetail it)
        {
            this.textBox3.AppendText(it.TimeDate + " - " + it.Description + " Method: " + it.Method);
            this.textBox3.AppendText(Environment.NewLine);
            this.Logs.AddItem(it);
            if(this.CurrentGridView == GridViewMode.Log)
            {
                this.UpdateDGV();
            }
        }

        private void UpdateDGV()
        {
            try
            {
                this.data_main.SuspendLayout();
                switch (this.CurrentGridView)
                {
                    case GridViewMode.Log:                       
                        this.data_main.DataSource = this.Logs.LogItems.ToList<LogDetail>();
                        this.UpdateFirstDisplayedScrollingRowIndex(this.data_main, this.Logs.LogItems.Count -5);                                            
                        break;
                    case GridViewMode.Device:
                        this.logit(new LogDetail("view is Device"));
                        this.data_main.DataSource = this.Devices.List;
                        this.UpdateFirstDisplayedScrollingRowIndex(this.data_main, this.Devices.List.Count);         
                        break;
                    case GridViewMode.DataSample:
                        this.logit(new LogDetail("view is DataSample"));
                        this.UpdateFirstDisplayedScrollingRowIndex(this.data_main, this.Logs.LogItems.Count);                                 
                        break;
                    case GridViewMode.RemoteCommand:
                        this.logit(new LogDetail("view is RemoteCommand"));
                        break;
                    case GridViewMode.AllPackets:
                        this.logit(new LogDetail("view is AllPackets"));
                        this.data_main.DataSource = this.GenericPackets.Packets.ToList<GenericPacket>();
                        break;
                    default:
                        break;
                }                
                this.data_main.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                this.data_main.ResumeLayout();
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                this.logit(new LogDetail("EXCEPTION: " + ex.Message));
            }            
        }

        private void PacketInterpreter(GenericPacket GenericPack)
        {
            try
            {
                switch (GenericPack.APItype)
                {
                    case XbeeBasePacket.XbeePacketType.TransmitRequest:
                        break;
                    case XbeeBasePacket.XbeePacketType.RemoteCmdRespons:
                        break;
                    case XbeeBasePacket.XbeePacketType.DataSample:
                        XbeeStruct.DataSampleStruct datasample = Util.BytesToStructure<XbeeStruct.DataSampleStruct>(GenericPack.PacketBytes.ToArray().Take(GenericPack.PacketBytes.Count).ToArray());
                        datasample.AddBytes(GenericPack.PacketBytes.ToArray());
                        break;
                    case XbeeBasePacket.XbeePacketType.RemoteCmd:
                        break;
                    case XbeeBasePacket.XbeePacketType.ReceivePacket:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void UpdateFirstDisplayedScrollingRowIndex(DataGridView DataGridObject, int RowIndex)
        {
            if (DataGridObject != null)
            {
                if (RowIndex > 5)
                {
                    DataGridObject.FirstDisplayedScrollingRowIndex = RowIndex;
                }
            }
        }
        
        private void InboundXbeeTestEvent(string msg)
        {
            //this.textBox3.AppendText(msg);
            //this.textBox3.AppendText(Environment.NewLine);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "";
            this.logit(new LogDetail("button1_clicked"));
            this.InboundXbeeTestEvent("button1_clicked");
            this.button1.Enabled = false;
            Task tt = doWork();
                       
            try
            {
                await tt;
            }
            catch (Exception ex)
            {
                this.textBox2.Text = String.Format("Exception {0}", ex.Message);
            }

            if (tt.Exception != null)
            {
                //this.textBox2.Text = "Exception" + tt.Exception.Message;
            }
            else
            {
                this.textBox2.Text = "click finished";
            }
            this.button1.Enabled = true;
            
        } 
 
        private async Task doWork()
        {
            await Task.Delay(2000);
            throw new Exception("Something happened."); 
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            //await man.PreEventProcessAsync("msgss");
            //this.serial.TestLogEvent("this is test");
            //this.serial.Open(); { 0x7d, 0x31,0x33,0x5e }
            this.button2.Enabled = false;
            //
            //string SampleOn = "7E 00 12 92 00 13 A2 00 40 A1 D8 CE FF FE C1 01 00 01 00 00 01 70";
            string SampleOn = "7E 00 12 92 00 7D 33 A2 00 40 A1 D8 CE FF FE C1 01 00 01 00 00 00 71";
            XbeeBasePacket xbee = new XbeeBasePacket();
            
            await Packet.AddByte(SampleOn);
            this.button2.Enabled = true;
            string hex = Packet.GetPacketAsHex();
            this.GenericPackets.AddGenericPacket(Packet.PacketBytes.ToArray());
            var tore = "";
            this.UpdateDGV();
            this.PacketInterpreter(new GenericPacket(Packet.PacketBytes.ToArray()));
            //byte[] bytes = { 0x7d, 0x31,0x7d, 0x33,0x7d,0x5e,0xff };
            //byte[] NeedEscapingbytes = { 0x11, 0x13, 0x7e, 0xff };  
            //List<byte> l = new List<byte>();
            //l.AddRange(NeedEscapingbytes);
            //Util.UnEscapeUartBytes(l);
            //Util.EscapeUartBytes(l);
        }

        public enum GridViewMode
        {
            Log = 0,
            Device = 1,
            AllPackets = 2,
            DataSample = 3,
            RemoteCommand = 4
        }

        private void btn_logs_Click(object sender, EventArgs e)
        {
            this.btn_logs.Enabled = false;
            this.CurrentGridView = GridViewMode.Log;
            this.UpdateDGV();
            this.btn_logs.Enabled = true;
        }

        private void btn_device_Click(object sender, EventArgs e)
        {
            this.btn_device.Enabled = false;
            this.CurrentGridView = GridViewMode.Device;
            this.UpdateDGV();
            this.btn_device.Enabled = true;
        }

        private void btn_AllPackets_Click(object sender, EventArgs e)
        {
            this.btn_AllPackets.Enabled = false;
            this.CurrentGridView = GridViewMode.AllPackets;
            this.UpdateDGV();
            this.btn_AllPackets.Enabled = true;
        }

        private async void btn_ManPacket_Click(object sender, EventArgs e)
        {
            this.Packet = new XbeeBasePacket();
            await Packet.AddByte(this.textBox1.Text);
            this.GenericPackets.AddGenericPacket(this.Packet.PacketBytes.ToArray());
            this.UpdateDGV();
        }          
    }

    public class manager
    {
        public event XbeeHEXeEventHandler XbeeHEXx;
        public event XbeeTestEventHandler XbeeTest;
        public delegate void XbeeTestEventHandler(string h);
        public delegate void XbeeHEXeEventHandler(byte[] ByteArray);
        public manager() 
        {
            
        }
        public async Task<int> GetNumber(int number1, int numberMax)
        {            
            bool yalla = await StopForABit();
            double val = (double)number1 / numberMax;
            val = val * 100;
            int ret = int.Parse(val.ToString());
            return ret;
            
        } 
         
        private async Task<bool> StopForABit()
        {
            await Task.Delay(50);
            bool bol = true;
            return bol;
        }

        private void xbeeTest_Event(string themsg)
        {
            if(XbeeTest != null)
            {
                XbeeTest(themsg);
               
            }            
        }

        public async Task<bool> PreEventProcessAsync(string value)
        {
            await this.PreeventProcess("something");
            this.xbeeTest_Event("after running await in PreEventProcessAsync");
            return true;
        }

        private async Task<string> PreeventProcess(string Value)
        {
            string RetValue = "";
            await Task.Delay(1000);
            RetValue = Value + " added by preEventprocess";
            return RetValue;
        }

       
    }
}
