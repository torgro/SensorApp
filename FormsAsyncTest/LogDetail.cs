using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LogDetail
{
    public string Description { get; set; }
    public int Level { get; set; }
    public string ClassName { get; set; }
    public string Method { get; set; }
    public DateTime TimeDate { get; set; }
    public int id { get; set; }

    public LogDetail()
    {
    }

    public LogDetail(string description)
    {
        this.Description = description;
    }
}
