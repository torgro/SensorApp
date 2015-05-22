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

    const string QueueName = "IssueTrackingQueue";
    private string EndPoint = "Endpoint=sb://ardupilotbus.servicebus.windows.net/;SharedAccessKeyName=root;SharedAccessKey=";
    private string nokkel = "44 4d 68 32 76 7a 37 74 55 4e 58 49 56 44 64 35 57 35 43 44 66 54 6b 69 4b 46 33 68 55 74 55 65 64 5a 4b 65 41 69 4c 43 78 6c 55 3d";
    private string ConStr = "";
    private QueueClient queue;
    private MessagingFactory factory;

    public AzureBus()
    {
        //MessagingFactory factory = null;
        try
        {
            this.ConStr = EndPoint + nokkel;
            //NamespaceManager namespaceClient = NamespaceManager.CreateFromConnectionString(this.ConStr);
            this.factory = MessagingFactory.CreateFromConnectionString(this.ConStr);
            this.queue = factory.CreateQueueClient(AzureBus.QueueName);

        }
        catch (Exception)
        {
            
            throw;
        }
    }

    public void SendMsg(BrokeredMessage msg)
    {
        this.queue.BeginSend(msg, OnSendComplete, new Tuple<QueueClient, string>(queue, msg.MessageId));

        //return true;
    }

    public void OnSendComplete(IAsyncResult result)
    {
        Tuple<QueueClient, string> stateInfo = (Tuple<QueueClient, string>)result.AsyncState;
        QueueClient queueClient = stateInfo.Item1;
        string messageId = stateInfo.Item2;

        try
        {
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
        msg.Properties.Add("Sender", packet.SourceAdr64);
        msg.Properties.Add("GPSlat", Device.GPSlat);
        msg.Properties.Add("GPSlong", Device.GPSlong);
        msg.Properties.Add("Pin", "D0");
        msg.Properties.Add("Samples", packet.Samples);
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

