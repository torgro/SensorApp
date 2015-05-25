using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Diagnostics;

public class AzureBus
{
    //events and delegates
    public event LogEventEventHandler LogEvent;
    public delegate void LogEventEventHandler(LogDetail LogItem);

    const string QueueName = "sensors";
    private string nokkel = "";
    private string ConStr = "";
    private QueueClient queue;
    private MessagingFactory factory;

    public AzureBus()
    {
        //MessagingFactory factory = null;
        try
        {            
            string path = System.IO.Directory.GetCurrentDirectory() + @"\ServiceBusKey.txt";
            this.nokkel = System.IO.File.ReadAllText(path);
            this.ConStr = nokkel;
            //NamespaceManager namespaceClient = NamespaceManager.CreateFromConnectionString(this.ConStr);
            this.factory = MessagingFactory.CreateFromConnectionString(this.ConStr);
            this.queue = factory.CreateQueueClient(AzureBus.QueueName);
        }
        catch (Exception ex)
        {
            System.Windows.Forms.MessageBox.Show("Exception ServiceBus - " + ex.Message);
        }
    }

    public void SendMsg(BrokeredMessage msg)
    {
        if(this.queue != null)
        {
            this.LogIt("SendMsg - Sending message to queue");
            this.queue.BeginSend(msg, OnSendComplete, new Tuple<QueueClient, string>(queue, msg.MessageId));
        }
        else
        {
            this.LogIt("SendMsg - Unable to send msg, queue is null");
        }        
    }

    public void OnSendComplete(IAsyncResult result)
    {
        this.LogIt("OnSendComplete - Start");
        Tuple<QueueClient, string> stateInfo = (Tuple<QueueClient, string>)result.AsyncState;
        QueueClient queueClient = stateInfo.Item1;
        string messageId = stateInfo.Item2;
        this.LogIt("OnSendComplete - Message with id " + messageId + "was sent");
        try
        {
            this.LogIt("OnSendComplete - Ending send async");
            queueClient.EndSend(result);
        }
        catch (Exception ex)
        {
            this.LogIt("EXCEPTION - Unable to send message with id " + messageId + " " + ex.Message);
        }
    }

    public void SendDatasample(DataSamplePacket packet, MonitorDevice Device)
    {
        BrokeredMessage msg = new BrokeredMessage(Device.Name);
        msg.MessageId = packet.Id.ToString();
        msg.Properties.Add("time", DateTime.Now.ToLongTimeString());
        msg.Properties.Add("lat", Device.GPSlat);
        msg.Properties.Add("lng", Device.GPSlong);
        msg.Properties.Add("date", "D0");
       // msg.Properties.Add("Samples", packet.Samples);
        this.SendMsg(msg);
    }

    virtual protected void OnLogEvent(LogDetail it)
    {
        if (LogEvent != null)
        {
            LogEvent(it);
        }
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

        OnLogEvent(log);
    }
}

