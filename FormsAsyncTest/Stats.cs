using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Stat
{
    public List<KeyValuePair<string,int>> StatParis { get; set; }

    public int BadPackets { get; set; }
    public int DataSample { get; set; }
    public int Filtered { get; set; }
    public int SensorsOnline { get; set; }
    public int SensorsOffline { get; set; }
    public int SensorsDisabled { get; set; }
    public int ActiveTasks { get; set; }
    public int Sensors { get; set; }
    public int ComPortInterrupts { get; set; }

    public Stat()
    {
        this.UpdateStats();
        //this.StatParis = new List<KeyValuePair<string, int>>();
        //this.StatParis.Add(this.AddSingle("BadPackets", this.BadPackets));
        //this.StatParis.Add(this.AddSingle("DataSample", this.DataSample));
        //this.StatParis.Add(this.AddSingle("Filtered", this.Filtered));
        //this.StatParis.Add(this.AddSingle("SensorsOnline", this.SensorsOnline));
        //this.StatParis.Add(this.AddSingle("SensorsDisabled", this.SensorsDisabled));
        //this.StatParis.Add(this.AddSingle("ActiveTasks", this.ActiveTasks));
        //this.StatParis.Add(this.AddSingle("Sensors", this.Sensors));
        //this.StatParis.Add(this.AddSingle("IRQ", this.ComPortInterrupts));
    }

    public void UpdateStats()
    {
        this.StatParis = new List<KeyValuePair<string, int>>();
        //this.StatParis.Add(this.AddSingle("BadPackets", this.BadPackets));
        this.StatParis.Add(this.AddSingle("DataSample", this.DataSample));
        this.StatParis.Add(this.AddSingle("Filtered", this.Filtered));
        this.StatParis.Add(this.AddSingle("SensorsOnline", this.SensorsOnline));
        this.StatParis.Add(this.AddSingle("SensorsOffline", this.SensorsOffline));
        this.StatParis.Add(this.AddSingle("ActiveTasks", this.ActiveTasks));
        this.StatParis.Add(this.AddSingle("Sensors", this.Sensors));
        this.StatParis.Add(this.AddSingle("IRQ", this.ComPortInterrupts));
    }

    //public Stat(Stats NewStats)
    //{
    //    this.StatParis = new List<KeyValuePair<string, int>>();
    //    this.AddStats(NewStats);
    //}

    //public void SetSsats()
    //{
    //    this.StatParis = new List<KeyValuePair<string, int>>();
        
    //}
    private KeyValuePair<string,int> AddSingle(string Key, int Value)
    {
        return new KeyValuePair<string, int>(Key, Value);
    }
}

public class Stats
{
    public string BadPackets { get; set; }
    public string DataSample { get; set; }
    public string Filtered { get; set; }
    public string SensorsOnline { get; set; }
    public string SensorsOffline { get; set; }
    public string SensorsDisabled { get; set; }
    public string SensorsPaused { get; set; }
    public string Sensors { get; set; }
    public string ComPortInterrupts { get; set; }

    public Stats()
    {
        this.BadPackets = "0";
        this.DataSample = "0";
        this.ComPortInterrupts = "0";
        this.Sensors = "0";
        this.SensorsDisabled = "0";
        this.SensorsOffline = "0";
        this.SensorsOnline = "0";
        this.SensorsPaused = "0";
        this.ComPortInterrupts = "0";
    }
}
