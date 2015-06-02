using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

    public class Pushbullet
    {
        private string key;
        private string httpadr = "https://api.pushbullet.com/v2/";
        private string pushUrl;
        private HttpClient client;
        private string authEncoded;

        public Pushbullet()
        {
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory() + @"\pushbulletkey.txt";
                this.key = System.IO.File.ReadAllText(path);
                this.client = new HttpClient();
                this.pushUrl = this.httpadr + "pushes";
                this.authEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(this.key + ":"));
                this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", this.authEncoded);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Eception in pushbullet" + ex.Message);
            }            
        }

        public void SendLink(string channel, string title, string body)
        {
            string pushtype = "link";
            //string deviceId = "ufexVNDsjz9biq5J7Y";//W510 device
            //channel_tag https://www.pushbullet.com/channel?tag=hjortefot
            var data = new KeyValuePairList<string,string>
			{
				//{"device_iden", deviceId},
				{"type", pushtype},
				{"title", "Alarm!"},
                {"body", "snublebluss snublebluss"},
                {"url", "http://hjortefot2.azurewebsites.net"},
                {"channel_tag", "hjortefot"}
			};

            HttpResponseMessage response;

            using (var httpcont = new FormUrlEncodedContent(data))
            {
                response = this.client.PostAsync(this.pushUrl, httpcont).Result;
            }

            string responsstring = null;

            if (response.IsSuccessStatusCode)
            {
                responsstring = response.Content.ReadAsStringAsync().Result;
                string v = response.Content.ReadAsStringAsync().Result;
            }

            if(responsstring != null)
            {
                dynamic obj = JObject.Parse(responsstring);                
            }
        }
    }

    public class KeyValuePairList<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly List<KeyValuePair<TKey, TValue>> _list = new List<KeyValuePair<TKey, TValue>>();

        internal List<KeyValuePair<TKey, TValue>> InnerList
        {
            get { return _list; }
        }

        public void Add(TKey key, TValue value)
        {
            InnerList.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
