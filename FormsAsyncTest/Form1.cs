using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Threading;
using System.Diagnostics;

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
        private DataSamples Datasample = new DataSamples();
        private RemoteCmdPackets RemoteCommandPackets = new RemoteCmdPackets();
        private AutoTasks Tasks = new AutoTasks();
        private int IRC = 0;
        private System.Windows.Forms.Timer ticks;
        private GridViewMode CurrentGridView = GridViewMode.Log;

        public Form1()
        {
            InitializeComponent();
           
            this.serial.LogEvent += this.logit;
            this.Packet.LogEvent += this.logit;
            this.Devices.LogEvent += this.logit;            
            this.Tasks.LogEvent += this.logit;
            this.RemoteCommandPackets.LogEvent += this.logit;
            this.Statistics.AddStats(new Stats());
            this.data_Stats.SuspendLayout();
            this.data_Stats.DataSource = this.Statistics.StatParis;
            this.data_Stats.ResumeLayout();
            this.ticks = new System.Windows.Forms.Timer();
            this.ticks.Interval = 10000;
            //this.textBox3.AppendText("Ticks is disabled!!");
            this.textBox3.AppendText(Environment.NewLine);
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

        //private async Task<bool> UpdateStats()
        private void UpdateStats()
        {
            Stats stats = new Stats();
            stats.ComPortInterrupts = this.IRC.ToString();
            stats.Packets = this.GenericPackets.Packets.Count.ToString();
            stats.Sensors = this.Devices.List.Count.ToString();
            Statistics.AddStats(stats);
        }

        private async Task<bool> UpdateStatsAsync()
        {
            await Task.Run(() => this.UpdateStats());
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

        #region InboundEventsDelegates
        private void logit(LogDetail it)
        {
            //this.textBox3.AppendText(it.TimeDate + " - " + it.Description + " Method: " + it.Method);
            //this.textBox3.AppendText(Environment.NewLine);
            this.Logs.AddItem(it);
            if (this.CurrentGridView == GridViewMode.Log)
            {
                this.UpdateDGV();
            }
            this.data_main.Refresh();
        }

        private void logit(string Description)
        {
            LogDetail it = new LogDetail();
            it.Description = Description;
            it.time = DateTime.Now.ToLongTimeString();
            it.TimeDate = DateTime.Now;
            it.Method = new StackFrame(1, true).GetMethod().Name;
            it.ClassName = "form1";
            this.logit(it);
        }

        private async void Ticks_Elapsed(object Sender, EventArgs e)
        {
            this.logit("Ticks_Elapsed start");
            //foreach (AutoTask item in this.Tasks.List)
            //{
            //    this.logit("Running item " + item.Id.ToString());
            //    bool x = await item.RunTaskAsync();
            //    if(item.Executed)
            //    {
            //        this.logit("removing event for task with id " + item.Id.ToString());
            //        item.LogEvent -= this.logit;
            //    }
            //}
            this.IRC++;
            await Tasks.RunPendingTasksAsync();
            await Task.Run(() => UpdateStats());
            //this.UpdateStats();
            //this.UpdateStatsView();
            this.UpdateStatsView();
        }
        #endregion

        private async void UpdateDGVasync()
        {
            await Task.Run(() => this.UpdateDGV());
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
                        this.logit("view is Device");
                        this.data_main.DataSource = this.Devices.List.ToList<MonitorDevice>();
                        //this.UpdateFirstDisplayedScrollingRowIndex(this.data_main, this.Devices.List.Count);         
                        break;
                    case GridViewMode.DataSample:
                        this.logit("view is DataSample");
                        this.data_main.DataSource = this.Datasample.List.ToList<DataSamplePacket>();
                        //this.UpdateFirstDisplayedScrollingRowIndex(this.data_main, this.Datasample.List.Count);                                 
                        break;
                    case GridViewMode.RemoteCommand:
                        this.logit("view is RemoteCommand");
                        this.data_main.DataSource = this.RemoteCommandPackets.List.ToList<RemoteCmdPacket>();
                        break;
                    case GridViewMode.AllPackets:
                        this.logit("view is AllPackets");
                        this.data_main.DataSource = this.GenericPackets.Packets.ToList<GenericPacket>();
                        break;
                    case GridViewMode.Tasks:
                        this.logit("View is Tasks");
                        this.data_main.DataSource = this.Tasks.List.ToList<AutoTask>();
                        break;
                    default:
                        this.logit("Help mummy, should NOT SEE THIS");
                        break;
                }                
                this.data_main.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                this.data_main.ResumeLayout();
                this.data_main.Refresh();
                //bool x = await UpdateStats();
                this.UpdateStats();
                this.UpdateStatsView();
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
                this.GenericPackets.AddGenericPacket(GenericPack);
                switch (GenericPack.APItype)
                {
                    case XbeeBasePacket.XbeePacketType.TransmitRequest:
                        throw new NotImplementedException("TransmitRequest packet not implemented!");    
                        //break;

                    case XbeeBasePacket.XbeePacketType.RemoteCmdRespons:
                        XbeeStruct.RemoteCmdResponsStruct RemoteResponsStruct = Util.BytesToStructure<XbeeStruct.RemoteCmdResponsStruct>(GenericPack.PacketBytes.ToArray());
                        this.RemoteCommandPackets.AddPacket(RemoteResponsStruct);
                        break;

                    case XbeeBasePacket.XbeePacketType.DataSample:
                        this.logit("Updating datasample object");
                        XbeeStruct.DataSampleStruct datasample = Util.BytesToStructure<XbeeStruct.DataSampleStruct>(GenericPack.PacketBytes.ToArray());
                        if(datasample.SamplesAsHex.Contains("1"))
                        {
                            this.logit("Creating auto task to enable PIN");
                            this.SetDevicePin(true, datasample.SourceAdr64, 1);
                            this.SetDeviceBattery(1, "BatteryLevel", 10, 1, 1);
                        }
                        this.Datasample.addPacket(datasample);
                        break;

                    case XbeeBasePacket.XbeePacketType.RemoteCmd:
                        XbeeStruct.RemoteCmdStruckt RemoteStruct = Util.BytesToStructure<XbeeStruct.RemoteCmdStruckt>(GenericPack.PacketBytes.ToArray());
                        this.RemoteCommandPackets.AddPacket(RemoteStruct);
                        break;

                    case XbeeBasePacket.XbeePacketType.ReceivePacket:
                        throw new NotImplementedException("ReceivePacket not implemented!");
                        //break;
                    default:
                        throw new NotImplementedException("unknown packet not implemented!");
                        //break;
                }
                this.UpdateDGV();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in PacketInterpreter!" + ex.Message);
                //throw;
            }
        }

        private void SetDevicePin(bool Enabled, string Address, int PauseMinutes)
        {
            RemoteCmdPacket newpacket = new RemoteCmdPacket();
            AutoTask newtask = new AutoTask(this.serial,this.Devices);
            newtask.Name = "test packet";
            newtask.ObjectID = 1;
            newtask.TaskType = AutoTask.AutoTaskType.ATcommand;
            newtask.bytes = newpacket.SetPinStatus(RemoteCmdPacket.XbeeAPIpin.D0, Address, 1, Enabled).GetPacketAsBytes();
            newtask.StartAt = DateTime.Now.AddMinutes(PauseMinutes);
            newtask.LogEvent += this.logit;
            this.Tasks.addTask(newtask);
        }

        private void DisableDeviceIRsensor(RemoteCmdPacket.XbeeAPIpin pin, string Address, int TimeoutInMinutes)
        {
            this.logit("Creating a packet to stop triggers for device " + Address);
            RemoteCmdPacket packet = new RemoteCmdPacket();
            int frameid = this.Devices.GetDeviceID(Address);
            packet.SetPinStatus(pin, Address, (byte)frameid, false);
            this.serial.Write(packet.AllBytes.ToArray());
            this.PacketInterpreter(new GenericPacket(packet.AllBytes.ToArray()));
            this.SetDevicePin(true, Address, TimeoutInMinutes);
        }

        private void SetDeviceBattery(int ObjectID, string PropName, byte newValue, byte OldValue, int PauseMinutes)
        {
            AutoTask t = new AutoTask(this.serial,this.Devices);
            t.Name = "testdevice update";
            t.ObjectID = ObjectID;
            t.OldValue = OldValue;
            t.NewValue = newValue;
            t.PropertyName = PropName;
            t.StartAt = DateTime.Now.AddMinutes(PauseMinutes);
            t.LogEvent += this.logit;
            t.TaskType = AutoTask.AutoTaskType.Device;
            this.Tasks.addTask(t);
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
        
        
        #region Form_Events
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

        private async void button2_Click(object sender, EventArgs e)
        {
            //await man.PreEventProcessAsync("msgss");
            //this.serial.TestLogEvent("this is test");
            //this.serial.Open(); { 0x7d, 0x31,0x33,0x5e }
            this.button2.Enabled = false;
            //
            //string SampleOn = "7E 00 12 92 00 13 A2 00 40 A1 D8 CE FF FE C1 01 00 01 00 00 01 70";
            //string SampleOn = "7E 00 12 92 00 7D 33 A2 00 40 A1 D8 CE FF FE C1 01 00 01 00 00 00 71";
            //XbeeBasePacket xbee = new XbeeBasePacket();
            this.textBox3.AppendText("setting battery value to 2");
            this.Devices.List[0].SetProperty("BatteryLevel", 2);
            this.textBox3.AppendText(Environment.NewLine);
            this.textBox3.AppendText("current view is " + this.CurrentGridView);
            this.UpdateDGV();
            //await Packet.AddByte(SampleOn);
            this.button2.Enabled = true;
            //string hex = Packet.GetPacketAsHex();
            //this.GenericPackets.AddGenericPacket(Packet.PacketBytes.ToArray());
            //var tore = "";

            //this.PacketInterpreter(new GenericPacket(Packet.PacketBytes.ToArray()));
            //this.UpdateDGV();
            //byte[] bytes = { 0x7d, 0x31,0x7d, 0x33,0x7d,0x5e,0xff };
            //byte[] NeedEscapingbytes = { 0x11, 0x13, 0x7e, 0xff };  
            //List<byte> l = new List<byte>();
            //l.AddRange(NeedEscapingbytes);
            //Util.UnEscapeUartBytes(l);
            //Util.EscapeUartBytes(l);
        }

        private void btn_logs_Click(object sender, EventArgs e)
        {
            this.btn_logs.Enabled = false;
            this.CurrentGridView = GridViewMode.Log;
            this.UpdateDGV();
            this.btn_logs.Enabled = true;
        }

        private void btn_tasks_Click(object sender, EventArgs e)
        {
            this.btn_tasks.Enabled = false;
            this.CurrentGridView = GridViewMode.Tasks;
            this.UpdateDGV();
            this.btn_tasks.Enabled = true;
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

        private void btn_ManPacket_Click(object sender, EventArgs e)
        {
            this.Packet = new XbeeBasePacket();
            //7E 00 12 92 00 13 A2 00 40 A1 D8 CE FF FE C1 01 00 01 00 00 01 70 - Datasample high
            //
            Packet.AddByte(this.textBox1.Text);
            this.PacketInterpreter(new GenericPacket(Packet.PacketBytes.ToArray()));
            //this.UpdateDGV();
        }

        private void btn_TestSample_Click(object sender, EventArgs e)
        {
            string SampleTriggerHigh = "7E 00 12 92 00 13 A2 00 40 A1 D8 CE FF FE C1 01 00 01 00 00 01 70";
            XbeeBasePacket packet = new XbeeBasePacket();
            packet.AddByte(SampleTriggerHigh);
            this.PacketInterpreter(new GenericPacket(packet.PacketBytes.ToArray()));
        }

        private void btn_DataSample_Click(object sender, EventArgs e)
        {
            this.btn_DataSample.Enabled = false;
            this.CurrentGridView = GridViewMode.DataSample;
            this.UpdateDGV();
            this.btn_DataSample.Enabled = true;
        }

        private void btn_testcmd_Click(object sender, EventArgs e)
        {
            //string DnullOn = "7E 00 10 17 01 00 13 A2 00 40 A1 D8 CE FF FF 02 44 30 03 34";
            //XbeeBasePacket xbee = new XbeeBasePacket(DnullOn);
            //XbeeStruct.RemoteCmdStruckt remoteCmd = Util.BytesToStructure<XbeeStruct.RemoteCmdStruckt>(xbee.PacketBytes.ToArray());
            //RemoteCmdPackets packets = new RemoteCmdPackets();

            RemoteCmdPacket packet = new RemoteCmdPacket();
            byte[] PinStatusBytes = packet.GetPinStatusPacket(RemoteCmdPacket.XbeeAPIpin.D3, "00 13 A2 00 40 A1 D8 CE".Replace(" ", ""), 1);
            XbeeStruct.RemoteCmdStruckt statuspin = Util.BytesToStructure<XbeeStruct.RemoteCmdStruckt>(PinStatusBytes);
            this.PacketInterpreter(new GenericPacket(statuspin.GetPacketAsBytes()));
            //packets.AddPacket(statuspin);
            XbeeBasePacket xbee = new XbeeBasePacket(PinStatusBytes);

            string toree = xbee.GetPacketAsHex();
            this.textBox3.AppendText(toree);
            toree = Util.ConvertByteArrayToHexString(statuspin.GetPacketAsBytes());
            this.textBox3.AppendText(Environment.NewLine);
            this.textBox3.AppendText(toree);

            packet = new RemoteCmdPacket();
            XbeeStruct.RemoteCmdStruckt setpin = packet.SetPinStatus(RemoteCmdPacket.XbeeAPIpin.D0, "00 13 A2 00 40 A1 D8 CE".Replace(" ", ""), 1, true);
            this.PacketInterpreter(new GenericPacket(setpin.GetPacketAsBytes()));
            this.textBox3.AppendText(Environment.NewLine);
            toree = Util.ConvertByteArrayToHexString(setpin.GetPacketAsBytes());
            this.textBox3.AppendText(toree);
        }

        private void btn_RemoteCmd_Click(object sender, EventArgs e)
        {
            this.btn_RemoteCmd.Enabled = false;
            this.CurrentGridView = GridViewMode.RemoteCommand;
            this.UpdateDGV();
            this.btn_RemoteCmd.Enabled = true;
        }          
        #endregion
        
        #region Enums
            public enum GridViewMode
                {
                    Log = 0,
                    Device = 1,
                    AllPackets = 2,
                    DataSample = 3,
                    RemoteCommand = 4,
                    Tasks = 5
                }
        #endregion
        
        #region test
        private async Task doWork()
        {
            await Task.Delay(2000);
            throw new Exception("Something happened.");
        }

        private void InboundXbeeTestEvent(string msg)
        {
            //this.textBox3.AppendText(msg);
            //this.textBox3.AppendText(Environment.NewLine);
        }
        #endregion

        

        
    }

    public class manager
    {
        //public event XbeeHEXeEventHandler XbeeHEXx;
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
