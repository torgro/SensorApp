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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace FormsAsyncTest
{
    public partial class Form1 : Form
    {
        public delegate void delegateXbeeTest(string msg);
        private delegate void delegateDGVHandler();
        private XbeeCOM serial = new XbeeCOM();
        private MonitorDevices Devices = new MonitorDevices();
        private XbeeBasePacket Packet = new XbeeBasePacket();
        private GenericPackets GenericPackets = new GenericPackets();
        private Logging Logs = new Logging();
        private Stat Statistics = new Stat();
        private DataSamples Datasample = new DataSamples();
        private RemoteCmdPackets RemoteCommandPackets = new RemoteCmdPackets();
        private AutoTasks Tasks = new AutoTasks();
        private AzureBus Azure;
        private AzureStorage Storage;
        
        private System.Windows.Forms.Timer ticks;
        private GridViewMode CurrentGridView = GridViewMode.Log;
  
        //settings
        private string ComPort;

        public Form1()
        {
            InitializeComponent();
            this.ComPort = Properties.Settings.Default.ComPort;
            this.serial.LogEvent += this.logit;
            this.serial.XbeeHEX += this.On_SerialBytes;
            this.Packet.LogEvent += this.logit;
            this.Packet.VaildPacket += this.On_ValidHexPacket;
            this.Devices.LogEvent += this.logit;            
            this.Tasks.LogEvent += this.logit;
            this.RemoteCommandPackets.LogEvent += this.logit;
            this.Azure = new AzureBus();
            this.Azure.LogEvent += this.logit;
            this.Storage = new AzureStorage();
            this.Storage.LogEvent += this.logit;
            this.data_Stats.SuspendLayout();
            this.data_Stats.DataSource = this.Statistics.StatParis;
            this.data_Stats.ResumeLayout();
            this.ticks = new System.Windows.Forms.Timer();
            this.ticks.Interval = 20000;
            this.textBox3.AppendText("Ticks is disabled!!");
            this.textBox3.AppendText(Environment.NewLine);
            this.ticks.Enabled = true;
            this.ticks.Tick += this.Ticks_Elapsed;
            MonitorDevice mon = new MonitorDevice();
            mon.Enabled = true;
            mon.MAC = "0013A20040A1D8CE";
            mon.GPSlat = "59.664874";
            mon.GPSlong = "6.444361";
            mon.Name = "bøen";
            mon.PartitionKey = "device";
            this.Devices.AddDevice(mon);
            this.UpdateStats();
            this.UpdateStatsView();        
            
        }              

        private void UpdateStats()
        {            
            this.Statistics.DataSample = this.Datasample.List.Count;
            this.Statistics.Sensors = this.Devices.List.Count;
            this.Statistics.SensorsOnline = this.Devices.GetOnlineDeviceCount(true);
            this.Statistics.Filtered = this.GenericPackets.GetFilteredCount();
            this.Statistics.ActiveTasks = this.Tasks.GetPendingTaskCount();
            this.Statistics.SensorsOffline = this.Devices.GetOnlineDeviceCount(false);
           
            this.Statistics.UpdateStats();            
        }

        private void UpdateStatsView()
        {
            this.data_Stats.SuspendLayout();
            this.data_Stats.DataSource = this.Statistics.StatParis;
            this.data_Stats.ResumeLayout();
        }

        private async Task<bool> UpdateStatsAsync()
        {
            await Task.Run(() => this.UpdateStats());
            return true;
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

        private void On_ValidHexPacket(byte[] bytes)
        {
            this.logit("Valid bytes packet ");
            this.Statistics.ComPortInterrupts++;
            this.PacketInterpreter(new GenericPacket(bytes));
        }

        private void On_SerialBytes(byte[] bytes)
        {
            this.Packet.AddByte(bytes);
        }
        
        private void logit(LogDetail it)
        {
            this.Logs.AddItem(it);
           
            if(this.data_main.InvokeRequired)
            {
                var update = new delegateDGVHandler(() => this.UpdateDGV());
                this.Invoke(update);
            }
            else
            {
                if (this.CurrentGridView == GridViewMode.Log)
                {
                    this.UpdateDGV();
                }
                this.data_main.Refresh();
            }
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
            await Tasks.RunPendingTasksAsync();
            await Task.Run(() => UpdateStats());
            this.UpdateStats();            
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
                        if(this.data_main.InvokeRequired)
                        {
                            this.logit("invoke required in DGV");
                        }
                        else
                        {
                            this.data_main.DataSource = this.Logs.LogItems.ToList<LogDetail>();
                            this.UpdateFirstDisplayedScrollingRowIndex(this.data_main, this.Logs.LogItems.Count -5);   
                        }
                                                               
                        break;
                    case GridViewMode.Device:                        
                        this.data_main.DataSource = this.Devices.List.ToList<MonitorDevice>();                            
                        break;
                    case GridViewMode.DataSample:                        
                        this.data_main.DataSource = this.Datasample.List.ToList<DataSamplePacket>();                                                       
                        break;
                    case GridViewMode.RemoteCommand:                       
                        this.data_main.DataSource = this.RemoteCommandPackets.List.ToList<RemoteCmdPacket>();
                        break;
                    case GridViewMode.AllPackets:                        
                        this.data_main.DataSource = this.GenericPackets.Packets.ToList<GenericPacket>();
                        break;
                    case GridViewMode.Tasks:                        
                        this.data_main.DataSource = this.Tasks.List.ToList<AutoTask>();
                        break;
                    default:
                        this.logit("Help mummy, should NOT SEE THIS");
                        break;
                }                
                this.data_main.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                this.data_main.ResumeLayout();
                this.data_main.Refresh();
                this.UpdateStats();
                this.UpdateStatsView();
            }
            catch (Exception ex)
            {                
                this.logit(new LogDetail("EXCEPTION: " + ex.Message));
            }            
        }

        private bool PacketFilter(GenericPacket GenericPacket)
        {
            bool Proceed = false;
            MonitorDevice device = this.Devices.GetSingleDevice(GenericPacket.SourceAddress);
            if (GenericPacket.APItype != XbeeBasePacket.XbeePacketType.DataSample)
            {
                this.logit("Packet is not datasample, allow packet");
                Proceed = true;
                return Proceed;
            }
            if (device == null)
            {
                this.logit("Unable to filter packet, device with mac " + GenericPacket.SourceAddress + " does not exist");
                Proceed = true;
                return Proceed;
            }

            if (device.Online == true)
            {
                this.logit("Device is online");
                Proceed = true;
            }

            return Proceed;
        }

        private void PacketInterpreter(GenericPacket GenericPack)
        {
            try
            {
                this.GenericPackets.AddGenericPacket(GenericPack);
                MonitorDevice NewDevice = this.Devices.GetSingleDevice(GenericPack.SourceAddress);
                if(NewDevice == null)
                {
                    this.logit("Create new device not found in database");
                    this.Devices.AddDevice(GenericPack.SourceAddress);
                }
                switch (GenericPack.APItype)
                {
                    case XbeeBasePacket.XbeePacketType.TransmitRequest:
                        this.logit("WARNING - TransmitRequest packet not implemented!");
                        break;
                    case XbeeBasePacket.XbeePacketType.RemoteCmdRespons:
                       this.RemoteCommandPackets.AddPacket(GenericPack);
                        break;

                    case XbeeBasePacket.XbeePacketType.DataSample:                       
                        if(this.PacketFilter(GenericPack) == true)
                        {                            
                            DataSamplePacket datasample = GenericPack.ToDataSamplePacket();
                            if (datasample == null)
                            {
                                this.logit("WARNING - ToDataSamplePacket returned an null packet!");
                            }
                            else
                            {
                                if (datasample.Samples.Contains("1"))
                                {
                                    this.logit("Creating auto task to enable PIN");
                                    this.DisableDeviceIRsensor(RemoteCmdPacket.XbeeAPIpin.D0, datasample.SourceAdr64);
                                    MonitorDevice dev = this.Devices.GetSingleDevice(datasample.SourceAdr64);
                                    this.SetDevicePin(true, RemoteCmdPacket.XbeeAPIpin.D0, datasample.SourceAdr64, dev.TimeOutMinutes);
                                    this.Datasample.AddPacket(datasample);
                                    if (dev.GPSlat.Length > 0)
                                    {
                                        this.logit("Sending sample to azure service bus");
                                        this.Azure.SendDatasample(datasample, dev);
                                    }
                                    else
                                    {
                                        this.logit("not sending to azure, missing GPS coordinates for device " + dev.MAC);
                                    }
                                    
                                }
                                else
                                {
                                    this.logit("WARNING - A datasample slipped through the filter that was not high (sample does not contain 1");
                                }
                            }                            
                        }
                        else
                        {                            
                            this.GenericPackets.Packets.Last().Filtered = true;
                            this.logit("Datasample packet was filtered out");
                        }                                      
                        break;

                    case XbeeBasePacket.XbeePacketType.RemoteCmd:
                        this.RemoteCommandPackets.AddPacket(GenericPack);
                        break;

                    case XbeeBasePacket.XbeePacketType.ReceivePacket:
                        this.logit("ReceivePacket not implemented!");
                        break;
                    default:
                        this.logit("unknown packet not implemented!");
                        break;
                }

                if(this.data_main.InvokeRequired)
                {
                    var update = new delegateDGVHandler(() => this.UpdateDGV());
                    this.Invoke(update);
                }
                else
                {
                    this.UpdateDGV();
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in PacketInterpreter!" + ex.Message);                
            }
        }

        private void SetDeviceOfflineStatus(bool EnableOnline, string Address, int pauseMinutes)
        {
            AutoTask SetOnline = new AutoTask(this.serial,this.Devices);
            int ObjectIdInt = this.Devices.GetDeviceID(Address);
            this.SetDevicePropertyTask(ObjectIdInt, "Online", EnableOnline.ToString(), "", pauseMinutes);
        }

        private void SetDevicePin(bool Enabled, RemoteCmdPacket.XbeeAPIpin pin, string Address, int PauseMinutes)
        {
            this.logit("Creating task to enable/disable pin");
            RemoteCmdPacket newpacket = new RemoteCmdPacket();
            AutoTask newtask = new AutoTask(this.serial, this.Devices);
            int ObjectIdInt = this.Devices.GetDeviceID(Address);
            newtask.Name = "Task for pin" + pin.ToString();
            newtask.ObjectID = ObjectIdInt;
            newtask.TaskType = AutoTask.AutoTaskType.ATcommand;
            newtask.NewValue = Enabled.ToString();
            newpacket.CreateSetPinTriggerPacket(Enabled, pin, Address, (byte)ObjectIdInt);
            newtask.bytes = newpacket.ToByteArray();

            newtask.StartAt = DateTime.Now.AddMinutes(PauseMinutes);
            newtask.LogEvent += this.logit;
            this.Tasks.addTask(newtask);
        }

        private void DisableDeviceIRsensor(RemoteCmdPacket.XbeeAPIpin pin, string Address)
        {
            this.logit("Creating a packet to stop triggers for device " + Address);
            RemoteCmdPacket packet = new RemoteCmdPacket();
            int frameid = this.Devices.GetDeviceID(Address);
            MonitorDevice Device = this.Devices.GetSingleDevice(frameid);
            
            if (Device == null)
            {
                this.logit("Unable to find device with ID " + frameid.ToString());
                return;
            }
            Device.Online = false;
            this.logit("Creating remotecommand packet to disable pin" + pin.ToString());
            packet.CreateSetPinTriggerPacket(false, pin, Address, (byte)frameid);
            
            byte[] bytes = packet.ToByteArray();
            this.logit("Adding packet to list");
            this.PacketInterpreter(new GenericPacket(bytes));
            this.logit("Writing bytes[] to serial");
            this.serial.Write(bytes);
            
            //// Set device offline one minute after reenable D0-pin trigger
            this.SetDeviceOfflineStatus(true, Address, Device.TimeOutMinutes + 1);
            this.logit("Creating AutoTask to enable pin D0 in " + Device.TimeOutMinutes.ToString() + " minutes");
            //this.SetDevicePin(true, Address, Device.TimeOutMinutes);            
        }

        private void SetDevicePropertyTask(int ObjectID, string PropName, string newValue, string OldValue, int PauseMinutes)
        {
            AutoTask t = new AutoTask(this.serial,this.Devices);
            t.Name = "Set " + PropName + " to value '" + newValue + "'(" + OldValue + ")";
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

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (this.btn_Connect.Text == "Connect")
                {                    
                    this.serial.port = Properties.Settings.Default.ComPort;
                    this.serial.Open();
                    if (this.serial.IsOpen)
                    {
                        this.btn_Connect.Text = "Disconnect";
                    }
                    else
                    {
                        this.logit("Unable to open COM-port");
                    }
                }
                else
                {
                    this.btn_Connect.Text = "Connect";
                    this.serial.close();
                }
                this.btn_Connect.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in btn_Connect" + ex.Message);
            }                    
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

        private void btn_DataSample_Click(object sender, EventArgs e)
        {
            this.btn_DataSample.Enabled = false;
            this.CurrentGridView = GridViewMode.DataSample;
            this.UpdateDGV();
            this.btn_DataSample.Enabled = true;
        }
        
        private void btn_RemoteCmd_Click(object sender, EventArgs e)
        {
            this.btn_RemoteCmd.Enabled = false;
            this.CurrentGridView = GridViewMode.RemoteCommand;
            this.UpdateDGV();
            this.btn_RemoteCmd.Enabled = true;
        }

        private void btn_EnableD0_Click(object sender, EventArgs e)
        {
            if (this.data_main.SelectedCells.Count > 0)
            {
                int RowIndex = this.data_main.SelectedCells[0].RowIndex;
                DataGridViewRow row = this.data_main.Rows[RowIndex];
                if (typeof(MonitorDevice) == row.DataBoundItem.GetType())
                {
                    MonitorDevice dev = (MonitorDevice)row.DataBoundItem;
                    string mac = dev.MAC;                    
                    this.ToggleD0trigger(dev.DeviceID, dev.MAC, true);
                }
            }
        }

        private void btn_DisableD0_Click(object sender, EventArgs e)
        {
            if (this.data_main.SelectedCells.Count > 0)
            {
                int RowIndex = this.data_main.SelectedCells[0].RowIndex;
                DataGridViewRow row = this.data_main.Rows[RowIndex];
                if (typeof(MonitorDevice) == row.DataBoundItem.GetType())
                {
                    MonitorDevice dev = (MonitorDevice)row.DataBoundItem;
                    string mac = dev.MAC;
                    //this.SetDevicePin(false, mac, 1);
                    this.ToggleD0trigger(dev.DeviceID, dev.MAC, false);
                }
            }
        }

        private void ToggleD0trigger(int DeviceID, string Address, bool EnableTrigger)
        {
            this.logit("Toggle D0 pin for device " + Address + " setting enabled to " + EnableTrigger.ToString());
            RemoteCmdPacket packet = new RemoteCmdPacket();
            packet.CreateSetPinTriggerPacket(EnableTrigger, RemoteCmdPacket.XbeeAPIpin.D0, Address, (byte)DeviceID);
            string hex = packet.ToHexString();
            this.textBox3.AppendText(Environment.NewLine);
            this.textBox3.AppendText(hex);
            this.PacketInterpreter(packet.ToGenericPacket());
            this.serial.Write(packet.ToByteArray());
        }

        private void GetD0Status(int DeviceID, String Adr)
        {
            RemoteCmdPacket packet = new RemoteCmdPacket();
            packet.CreatePinStatusPacket(RemoteCmdPacket.XbeeAPIpin.D0, Adr, (byte)DeviceID);

            byte[] bytes = packet.ToByteArray();
            string hex = packet.ToHexString();
            this.textBox3.Text = hex;            
            this.PacketInterpreter(packet.ToGenericPacket());
            this.serial.Write(bytes);
        }

        private void btn_StatusD0_Click(object sender, EventArgs e)
        {
            if (this.data_main.SelectedCells.Count > 0)
            {
                int RowIndex = this.data_main.SelectedCells[0].RowIndex;
                DataGridViewRow row = this.data_main.Rows[RowIndex];
                if (typeof(MonitorDevice) == row.DataBoundItem.GetType())
                {
                    MonitorDevice dev = (MonitorDevice)row.DataBoundItem;
                    string mac = dev.MAC;                   
                    this.GetD0Status(dev.DeviceID, dev.MAC);
                }
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            //await man.PreEventProcessAsync("msgss");
            //this.serial.TestLogEvent("this is test");
            //this.serial.Open(); { 0x7d, 0x31,0x33,0x5e }
            this.button2.Enabled = false;
            //
            //string SampleOn = "7E 00 12 92 00 13 A2 00 40 A1 D8 CE FF FE C1 01 00 01 00 00 01 70";
            //string SampleOn = "7E 00 12 92 00 7D 33 A2 00 40 A1 D8 CE FF FE C1 01 00 01 00 00 00 71";
            //XbeeBasePacket xbee = new XbeeBasePacket();

            string SampleOn = "7E 00 12 92 00 13 A2 00 40 A1 D8 CE FF FE C1 01 00 01 00 00 01 70";
            string RemoteCmdRespons = "7E 00 0F 97 01 00 13 A2 00 40 A1 D8 CE FF FE 44 30 00 BA";
            XbeeBasePacket xbee = new XbeeBasePacket(RemoteCmdRespons);
            byte[] bytes = xbee.PacketBytes.ToArray();
            //DataSamplePacket Triggered = new DataSamplePacket(bytes);
            this.textBox1.Text = (new GenericPacket(bytes)).ToHexString();
            this.PacketInterpreter(new GenericPacket(bytes));
            //RemoteCmdPacket on = new RemoteCmdPacket();
            //string shouldbeempty = on.ToHexString();
            //on.CreateSetPinTriggerPacket(true, RemoteCmdPacket.XbeeAPIpin.D0, "00 13 A2 00 40 A1 D8 CE".Replace(" ", ""), 16);
            //string hex = on.ToHexString();
            //RemoteCmdPacket off = new RemoteCmdPacket();
            //off.CreateSetPinTriggerPacket(false, RemoteCmdPacket.XbeeAPIpin.D0, "00 13 A2 00 40 A1 D8 CE".Replace(" ", ""), 16);
            //string hexoff = off.ToHexString();
            //this.PacketInterpreter(new GenericPacket(on.ToByteArray()));

            //this.textBox3.AppendText("setting device to disabled");
            //this.Devices.List[0].SetProperty("Online", "false");
            //this.textBox3.AppendText(Environment.NewLine);
            //this.textBox3.AppendText("current view is " + this.CurrentGridView);
            //this.UpdateDGV();
            //this.data_main.Refresh();

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

        private void btn_testcmd_Click(object sender, EventArgs e)
        {
            //string DnullOn = "7E 00 10 17 01 00 13 A2 00 40 A1 D8 CE FF FE 02 44 30 03 34";
            //XbeeBasePacket xbee = new XbeeBasePacket(DnullOn);
            //XbeeStruct.RemoteCmdStruckt remoteCmd = Util.BytesToStructure<XbeeStruct.RemoteCmdStruckt>(xbee.PacketBytes.ToArray());
            //RemoteCmdPackets packets = new RemoteCmdPackets();

            RemoteCmdPacket packet = new RemoteCmdPacket();
            //byte[] PinStatusBytes = packet.GetPinStatusPacket(RemoteCmdPacket.XbeeAPIpin.D3, "00 13 A2 00 40 A1 D8 CE".Replace(" ", ""), 1);
            //XbeeStruct.RemoteCmdStruckt statuspin = Util.BytesToStructure<XbeeStruct.RemoteCmdStruckt>(PinStatusBytes);
            //this.PacketInterpreter(new GenericPacket(statuspin.GetPacketAsBytes()));
            ////packets.AddPacket(statuspin);
            //XbeeBasePacket xbee = new XbeeBasePacket(PinStatusBytes);

            //string toree = xbee.GetPacketAsHex();
            //this.textBox3.AppendText(toree);
            //toree = Util.ConvertByteArrayToHexString(statuspin.GetPacketAsBytes());
            //this.textBox3.AppendText(Environment.NewLine);
            //this.textBox3.AppendText(toree);

            //packet = new RemoteCmdPacket();
            //XbeeStruct.RemoteCmdStruckt setpin = packet.SetPinStatus(RemoteCmdPacket.XbeeAPIpin.D0, "00 13 A2 00 40 A1 D8 CE".Replace(" ", ""), 1, true);
            //this.PacketInterpreter(new GenericPacket(setpin.GetPacketAsBytes()));
            //this.textBox3.AppendText(Environment.NewLine);
            //toree = Util.ConvertByteArrayToHexString(setpin.GetPacketAsBytes());
            //this.textBox3.AppendText(toree);
        }

        private void btn_TestSample_Click(object sender, EventArgs e)
        {
            string SampleTriggerHigh = "7E 00 12 92 00 13 A2 00 40 A1 D8 CE FF FE C1 01 00 01 00 00 01 70";
            XbeeBasePacket packet = new XbeeBasePacket();
            packet.AddByte(SampleTriggerHigh);
            this.PacketInterpreter(new GenericPacket(packet.PacketBytes.ToArray()));
        }

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

        private void btn_TestAzure_Click(object sender, EventArgs e)
        {
            this.logit("Creating or getting table");
            this.Storage.GetOrCreateTable("Sensors");
            this.logit("updating table recoreds");
            List<MonitorDevice> llist = this.Storage.GetAzureTableAll<MonitorDevice>("device");
            this.Storage.InsertorReplaceEntity<MonitorDevice>(this.Devices.List[0]);
            this.logit("done");
        }
        
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
