using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tests
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Events
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Thread th = new Thread(mp4video);
                th.IsBackground = true;
                th.Start();
            }
            catch
            {
                MessageBox.Show("Current video in progress.");
            }
        }
        #endregion

        #region Methods

        public void mp4video()
        {
            HttpWebRequest req = WebRequest.CreateHttp("https://helloacm.com/api/video/?cached&lang=en&hash=4944cfe42e791f8ce75c0ce1a710ccc6&video=" + textBox1.Text);
            req.Method = "GET";
            req.Host = "helloacm.com";
            req.Accept = "*/*";
            req.Headers.Add("Origin", "https://weibomiaopai.com");
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.84 Safari/537.36";
            req.KeepAlive = true;
            req.Referer = "https://weibomiaopai.com/online-video-downloader/youtube";
            req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            req.Headers.Add("Accept-Language", "en-US,en;q=0.9");

            using (var streamReader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                string response = streamReader.ReadToEnd();
                var obj = JObject.Parse(response);
                var url = (string)obj["url"];

                this.Invoke(new Action(() =>
                {
                    textBox2.Text = (url);
                }));
            }
        }

        #endregion

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Video file|*.mp4";
                sfd.ShowDialog();

                using (var client = new WebClient())
                {
                    client.DownloadFileAsync(new Uri(textBox2.Text), sfd.FileName);
                    client.DownloadProgressChanged += Client_DownloadProgressChanged;
                    client.DownloadFileCompleted += Client_DownloadFileCompleted;
                }
            }
            catch
            {

            }
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            progressBar1.Value = 0;
            textBox1.Text = "";
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Minimum = 0;
            double receive = double.Parse(e.BytesReceived.ToString());
            double total = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = receive / total * 100;
            progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
        }
    }
}
