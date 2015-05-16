using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Logging
{
    public List<LogDetail> LogItems;
    public int Loglevel;
    public Logging()
    {
        this.Loglevel = 0;
        this.LogItems = new List<LogDetail>();
    }

    public void AddItem(string Description, int Level, string ClassName, string Method)
    {
        LogDetail it = new LogDetail();
        it.Description = Description;
        it.Level = Level;
        it.ClassName = ClassName;
        it.Method = Method;
        it.TimeDate = DateTime.Now;
        it.id = this.LogItems.Count;
        it.time = it.TimeDate.ToLongTimeString();
        this.LogItems.Add(it);
    }
    public void AddItem(LogDetail it)
    {
        it.id = this.LogItems.Count;
        it.TimeDate = DateTime.Now;
        it.time = it.TimeDate.ToLongTimeString();
        this.LogItems.Add(it);
    }
    public void ClearList()
    {
        this.LogItems = new List<LogDetail>();
    }
}

public class LogDetail
{
    public int id { get; set; }
    public DateTime TimeDate { get; set; }
    public string time { get; set; }
    public string ClassName { get; set; }
    public string Method { get; set; }
    public string Description { get; set; }
    public int Level { get; set; }

    public LogDetail()
    {
    }

    public LogDetail(string description)
    {
        this.Description = description;
    }
}