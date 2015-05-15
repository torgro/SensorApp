using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Stat
{
    public List<KeyValuePair<string,string>> StatParis { get; set; }

    public Stat()
    {
        this.StatParis = new List<KeyValuePair<string, string>>();
    }

    public Stat(Stats NewStats)
    {
        this.StatParis = new List<KeyValuePair<string, string>>();
        this.AddStats(NewStats);
    }

    public void AddStats(Stats NewStats)
    {
        this.StatParis = new List<KeyValuePair<string, string>>();
        this.StatParis.Add(this.AddSingle("BadPackets", NewStats.BadPackets));
        this.StatParis.Add(this.AddSingle("Packets", NewStats.Packets));
        this.StatParis.Add(this.AddSingle("SensorsOnline", NewStats.SensorsOnline));
        this.StatParis.Add(this.AddSingle("SensorsDisabled", NewStats.SensorsDisabled));
        this.StatParis.Add(this.AddSingle("SensorsPaused", NewStats.SensorsPaused));
        this.StatParis.Add(this.AddSingle("Sensors", NewStats.Sensors));
        this.StatParis.Add(this.AddSingle("IRQ", NewStats.ComPortInterrupts));
    }
    private KeyValuePair<string,string> AddSingle(string Key, string Value)
    {
        return new KeyValuePair<string, string>(Key, Value);
    }
}

public class Stats
{
    public string BadPackets { get; set; }
    public string Packets { get; set; }
    public string SensorsOnline { get; set; }
    public string SensorsOffline { get; set; }
    public string SensorsDisabled { get; set; }
    public string SensorsPaused { get; set; }
    public string Sensors { get; set; }
    public string ComPortInterrupts { get; set; }

    public Stats()
    {
        this.BadPackets = "0";
        this.Packets = "0";
        this.ComPortInterrupts = "0";
        this.Sensors = "0";
        this.SensorsDisabled = "0";
        this.SensorsOffline = "0";
        this.SensorsOnline = "0";
        this.SensorsPaused = "0";
        this.ComPortInterrupts = "0";
    }
}
