using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

public class MonitorDevices
{
    public List<MonitorDevice> List { get; set; }
    public List<TreeNode> TreeNodes { get; set; }
    public event LogEventEventHandler LogEvent;
    public delegate void LogEventEventHandler(LogDetail LogItem);

    public MonitorDevices() 
    {
        this.List = new List<MonitorDevice>();
        this.TreeNodes = new List<TreeNode>();
        TreeNode node = new TreeNode();
        node.Name = "AllSensors";
        node.Text = "All Sensors";
        this.TreeNodes.Add(node);
    }

    private void AddNewDevice(MonitorDevice Device)
    {
        List<MonitorDevice> ListMatch = this.GetDevice(Device.MAC);
        if (ListMatch.Count > 0)
        {
            this.LogIt("Monitor with MAC " + Device.MAC + " exists, NOT adding!");
        }
        else
        {
            this.LogIt("Monitor with MAC " + Device.MAC + " not found adding it!");
            Device.Enabled = true;
            this.List.Add(Device);
        }       
    }

    public void AddDevice(string MAC)
    {
        this.LogIt("Adding device with MAC " + MAC);
        MonitorDevice device = new MonitorDevice();
        device.DeviceID = this.List.Count;
        device.MAC = MAC;
        device.Enabled = true;
        this.AddNewDevice(device);
    }
    public void AddDevice(MonitorDevice dev)
    {
        dev.DeviceID = this.List.Count;
        this.AddNewDevice(dev);
    }

    private void LogIt(string Str)
    {
        LogDetail log = new LogDetail();
        string calledby = new StackFrame(2, true).GetMethod().Name;
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

    public List<MonitorDevice> GetDevice(int ById)
	{
        System.Collections.Generic.IEnumerable<MonitorDevice> q = 
        from it in this.List.AsEnumerable()
        select it;
        return q.ToList<MonitorDevice>();
	}

    public List<MonitorDevice> GetDevice(string MAC)
	{
        System.Collections.Generic.IEnumerable<MonitorDevice> q =
        from it in this.List
        where it.MAC.ToLower() == MAC.ToLower()
        select it;

        return q.ToList<MonitorDevice>(); 
	}

}

public class MonitorDevice
{

    public string Name { get; set; }
    public int DeviceID { get; set; }
    public string Location { get; set; }
    public string GPSlat { get; set; }
    public string GPSlong { get; set; }
    public bool Online { get; set; }
    public int NumberOfSensors { get; set; }
    public string LastStatus { get; set; }
    public string Logs { get; set; }
    public MonitorHeading Heading { get; set; }
    public long BatteryLevel { get; set; }
    public string MAC { get; set; }
    public string FlightPlan { get; set; }
    public bool Enabled { get; set; }
    public List<XbeeBasePacket> Packets { get; set; }
    public bool Sensor1Detect { get; set; }
    public bool Sensor2Detect { get; set; }

    public MonitorDevice()
    {
        this.Packets = new List<XbeeBasePacket>();
        this.Online = true;
        this.Enabled = true;
        this.Location = string.Empty;
        this.Name = string.Empty;
        this.LastStatus = string.Empty;
        this.Logs = string.Empty;
        this.BatteryLevel = 0;
        this.DeviceID = 0;
        this.GPSlat = string.Empty;
        this.GPSlong = string.Empty;
        this.Heading = MonitorHeading.North;
        this.NumberOfSensors = 2;
        this.Sensor1Detect = false;
        this.Sensor2Detect = false;
    }
}

public enum MonitorHeading
{
    North = 0,
    West = 1,
    South = 2,
    East = 3
}
