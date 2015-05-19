using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Diagnostics;
//using System.IO;
using System.IO.Ports;

public class XbeeCOM
{ 
    private System.IO.Ports.SerialPort SerialPort1;
    public bool IsOpen = false;
    public int DataReceivedCounter = 0;
    public string port { get; set; }
    public int BaudRate { get; set; }
    public bool RtsEnable { get; set; }
    public event LogEventEventHandler LogEvent;
    public delegate void LogEventEventHandler(LogDetail str);
    public event XbeeHEXEventHandler XbeeHEX;
    public delegate void XbeeHEXEventHandler(byte[] ByteArray);

    public XbeeCOM()
    {
        this.BaudRate = 9600;
    }

    public XbeeCOM(string COMport)
    {
        this.port = COMport;
    }

    public void TestLogEvent(string msg)
    {
        this.LogIt(msg);
    }
    private void initPort()
    {
        this.LogIt("Running initPort in xbeecom");
        this.SerialPort1 = new SerialPort();
        if (this.port == null)
        {
            this.port = "com13";
        }
        var _with1 = this.SerialPort1;
        _with1.ParityReplace = 0x3b;
        _with1.PortName = this.port;
        _with1.BaudRate = this.BaudRate;
        _with1.Parity = System.IO.Ports.Parity.None;
        _with1.StopBits = System.IO.Ports.StopBits.One;
        _with1.Handshake = System.IO.Ports.Handshake.None;
        _with1.RtsEnable = this.RtsEnable;
        _with1.DataBits = 8;
        _with1.ReceivedBytesThreshold = 1;
        //.NewLine = vbCr
        _with1.ReadTimeout = 10000;
        //.Encoding = System.Text.Encoding.GetEncoding("Windows-1252")
    }

    public void Open()
    {
        try
        {
            this.LogIt("Current port is '" + this.port + "'");
            if (this.SerialPort1 == null)
            {
                this.initPort();
            }
            this.SerialPort1.Open();
            this.LogIt("Serialport is OPEN, using port " + this.port.ToString());
            this.LogIt(string.Format("BaudRate is {0}", this.BaudRate.ToString()));
            this.IsOpen = true;

            this.SerialPort1.DataReceived += new SerialDataReceivedEventHandler(this.DataReceivedHandler);

        }
        catch (Exception ex)
        {
            this.LogIt(ex.Message);
        }
    }

    public void Write(byte[] b)
    {
        try
        {
            if (this.SerialPort1 == null)
            {
                this.initPort();
            }
            if (this.SerialPort1.IsOpen == false)
            {
                this.SerialPort1.Open();
                this.IsOpen = true;
            }
            List<byte> list = Util.EscapeUartBytes(b);
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (byte bbyte in list)
            {
                sb.Append(Util.ConvertToHex(bbyte));
                sb.Append(" ");
            }
            char[] trimWhite = {' '};
            this.LogIt("Sending this hex thingy '" + sb.ToString().TrimEnd(trimWhite) + "'");

            this.LogIt("Writing " + list.Count + " bytes to the serialport");
            this.SerialPort1.Write(list.ToArray(), 0, list.Count);
            this.LogIt("Writing done");
        }
        catch (Exception ex)
        {
            this.LogIt(ex.Message);
        }
    }

    public async Task<bool> WriteAsync(byte[] bytes)
    {
        return await Task<bool>.Run(() =>
            {
                this.Write(bytes);
                return true;
            });
    }
    public void close()
    {
        if (this.SerialPort1 != null)
        {
            if (this.SerialPort1.IsOpen)
            {
                this.LogIt("Serial port is open, closing " + this.port);
                this.SerialPort1.Close();
                this.LogIt("Removing handler");
                this.SerialPort1.DataReceived -= this.DataReceivedHandler;
                this.SerialPort1.Dispose();
                this.SerialPort1 = null;                
            }
            else
            {
                this.LogIt("Serial port is closed");
            }
        }
        else
        {
            this.LogIt("Serialport1 is null");
        }
        this.IsOpen = false;
    }

    private void LogIt(string Str)
    {
        string calledby = new StackFrame(1, true).GetMethod().Name;
        LogDetail it = new LogDetail();
        it.ClassName = this.GetType().FullName;
        it.Description = Str;
        it.TimeDate = DateTime.Now;
        it.Level = 0;
        it.Method = calledby;

        if (LogEvent != null)
        {
            LogEvent(it);
        }
    }

    private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        this.DataReceivedCounter += 1;
        int length = sp.BytesToRead;
        this.LogIt("Data frame Received, length: " + length.ToString());
        byte[] buff = new byte[length];
        sp.Read(buff, 0, length);
        if (XbeeHEX != null)
        {
            this.LogIt("Raising event on Data frame received!");
            XbeeHEX(buff);
        }
    }
}

