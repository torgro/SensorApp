using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

public class AutoTasks
{
    public List<AutoTask> List;
    // Events and delegates
    public event LogEventEventHandler LogEvent;
    public delegate void LogEventEventHandler(LogDetail LogItem);

    public AutoTasks()
    {
        this.LogIt("New instance of class");
        this.List = new List<AutoTask>();
    }

    public void addTask(AutoTask SingleTask)
    {
        this.LogIt("START");
        if(SingleTask.StartAt > DateTime.Now)
        {
            this.LogIt("Adding task with starttime " + SingleTask.StartAt.ToString());
            SingleTask.Id = this.List.Count;
            this.List.Add(SingleTask);
        }
        else
        {
            this.LogIt("Unable to add task, StartAt is in the past (expired)!");
        }
    }

    public List<AutoTask> GetPendingTasks()
    {
        System.Collections.Generic.IEnumerable<AutoTask> q =
            from it in this.List.AsEnumerable()
            where it.Executed == false
            select it;

        return q.ToList<AutoTask>();
    }

    public int GetPendingTaskCount()
    {
        return this.GetPendingTasks().Count;
    }

    public async Task<List<AutoTask>> GetPendingTasksAsync()
    {
        List<AutoTask> list = await Task.Run(() => this.GetPendingTasks());
        return list;
    }

    public async Task<bool> RunPendingTasksAsync()
    {
        bool Executed = false;
        foreach (AutoTask item in await this.GetPendingTasksAsync())
        {
            Executed = await item.RunTaskAsync();
        }
        return Executed;
    }

    private void LogIt(string Str)
    {
        LogDetail log = new LogDetail();
        string calledby = new StackFrame(1, true).GetMethod().Name;
        var tore = new StackFrame();
        string ClassName = this.GetType().FullName;
        log.ClassName = ClassName;
        log.Description = Str;
        log.Level = 0;
        log.Method = calledby;
        log.TimeDate = DateTime.Now;

        if (this.LogEvent != null)
        {
            this.LogEvent(log);
        }
    }

}

public class AutoTask
{
    public DateTime StartAt { get; set; }
    public AutoTaskType TaskType { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public string PropertyName { get; set; }
    public int ObjectID { get; set; }
    public byte[] bytes;// { get; set; }
    public bool Executed { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public int Runcount { get; set; }
    //events and delegates
    public event LogEventEventHandler LogEvent;
    public delegate void LogEventEventHandler(LogDetail LogItem);

    private XbeeCOM mSerial;
    private MonitorDevices mDevices;

    public AutoTask(XbeeCOM serial, MonitorDevices Devices)
    {
        this.LogIt("Creating new task");
        this.mSerial = serial;
        this.mDevices = Devices;
        this.Executed = false;
    }

    public async Task<bool> RunTaskAsync()
    {
        bool ReturnValue = false;
        string f = "RunTaskAsync";
        this.LogIt("Checking task with id " + this.Id + " and name " + this.Name,f);
        if (this.StartAt <= DateTime.Now)
        {
            this.LogIt("STARTING task",f);
            this.Runcount++;
            switch (this.TaskType)
            {
                case AutoTaskType.ATcommand:
                    this.LogIt("ATcommand task",f);
                    if (this.bytes != null)
                    {
                        this.LogIt("Running async write serial",f);
                        if (this.mSerial.IsOpen)
                        {
                            await this.mSerial.WriteAsync(this.bytes);
                            ReturnValue = true;
                        }
                        else
                        {
                            this.LogIt("Serialport is not open!",f);
                        }                        
                    }
                    else
                    {
                        this.LogIt("Warning, task had no bytes to write", f);
                    }                    
                    break;
                case AutoTaskType.Device:
                    this.LogIt("Device task", f);
                    MonitorDevice CurrentDevice; 
                    if(this.ObjectID > 0) 
                    {
                        //CurrentDevice = await this.mDevices.GetDeviceAsync(this.ObjectID);
                        CurrentDevice = this.mDevices.GetSingleDevice(this.ObjectID);
                        if (CurrentDevice != null)
                        {
                            //MonitorDevice theDevice = DeviceList[0];
                            CurrentDevice.SetProperty(this.PropertyName, this.NewValue);
                        }
                        else
                        {
                            this.LogIt("Warning, unable to find device with id " + this.ObjectID);
                            return false;
                        }
                        ReturnValue = true;
                    }
                    else
                    {
                        this.LogIt("Warning, no objectID was provided", f);
                    }
                    break;
                default:
                    this.LogIt("Warning, other task not defined");
                    break;
            }
        }
        this.LogIt("END, returning" + ReturnValue.ToString(), f);
        if(this.Runcount > 5)
        {
            this.LogIt("Unable to execute task, setting status to executed = true", f);
            this.Executed = true;
        }
        if (ReturnValue == true) { this.Executed = true; }
        return ReturnValue;
    }

    private void LogIt(string LogThis, string CalledBy)
    {
        LogDetail log = new LogDetail();
        string ClassName = this.GetType().FullName;
        log.ClassName = ClassName;
        log.Description = LogThis;
        log.Level = 0;
        log.Method = CalledBy;
        log.TimeDate = DateTime.Now;
        this.LogIt(log);
    }

    private void LogIt(LogDetail it)
    {
        if (this.LogEvent != null)
        {
            this.LogEvent(it);
        }
    }

    private void LogIt(string Str)
    {
        LogDetail log = new LogDetail();
        string calledby = new StackFrame(3, true).GetMethod().Name;
        var tore = new StackFrame();
        string ClassName = this.GetType().FullName;
        log.ClassName = ClassName;
        log.Description = Str;
        log.Level = 0;
        log.Method = calledby;
        log.TimeDate = DateTime.Now;
        this.LogIt(log);
    }

    public enum AutoTaskType
    {
        ATcommand = 0,
        Device = 2
    }
}
