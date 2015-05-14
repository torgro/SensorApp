using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Threading;

namespace FormsAsyncTest
{
    public partial class Form1 : Form
    {
        public delegate void delegateXbeeTest(string msg);
        private manager man;
        private XbeeCOM serial = new XbeeCOM();
        public Form1()
        {
            InitializeComponent();
           
            //this.man.XbeeTest += this.InboundXbeeTestEvent;
            this.serial.LogEvent += this.logit;            
        }

        private void logit(LogDetail it)
        {
            this.textBox3.AppendText(it.Description + " Method: " + it.Method);
            this.textBox3.AppendText(Environment.NewLine);
        }

        private void InboundXbeeTestEvent(string msg)
        {
            this.textBox3.AppendText(msg);
            this.textBox3.AppendText(Environment.NewLine);
        }

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
 
        private async Task doWork()
        {
            await Task.Delay(2000);
            throw new Exception("Something happened."); 
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            //await man.PreEventProcessAsync("msgss");
            //this.serial.TestLogEvent("this is test");
            //this.serial.Open(); { 0x7d, 0x31,0x33,0x5e }
            this.button2.Enabled = false;
            string SampleOn = "7E 00 12 92 00 7D 33 A2 00 40 A1 D9 17 FF FE C1 01 00 01 00 00 01 26";
            XbeeBasePacket xbee = new XbeeBasePacket();
            xbee.LogEvent += this.InboundXbeeTestEvent;
            await xbee.AddByte(SampleOn);
            this.button2.Enabled = true;
            //byte[] bytes = { 0x7d, 0x31,0x7d, 0x33,0x7d,0x5e,0xff };
            //byte[] NeedEscapingbytes = { 0x11, 0x13, 0x7e, 0xff };  
            //List<byte> l = new List<byte>();
            //l.AddRange(NeedEscapingbytes);
            //Util.UnEscapeUartBytes(l);
            //Util.EscapeUartBytes(l);
        }
    }

    public class manager
    {
        public event XbeeHEXeEventHandler XbeeHEXx;
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
