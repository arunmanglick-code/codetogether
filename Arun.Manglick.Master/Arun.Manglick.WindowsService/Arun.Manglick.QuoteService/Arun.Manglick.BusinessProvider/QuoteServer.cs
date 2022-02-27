using System;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Arun.Manglick.BusinessProvider
{
    public class QuoteServer : System.ComponentModel.Component
    {
        private TcpListener listener;
        private int port;
        private string filename;
        private StringCollection quotes;
        private Random random;
        private Thread listenerThread;

        private int requestsPerSec;
        private int bytesPerSec;

        private System.Diagnostics.PerformanceCounter performanceCounterRequestsPerSec;
        private System.Diagnostics.PerformanceCounter performanceCounterBytesSentTotal;
        private System.Diagnostics.PerformanceCounter performanceCounterBytesSentPerSec;
        private System.Timers.Timer timer;
        private System.Diagnostics.EventLog eventLog;
        private System.Diagnostics.PerformanceCounter performanceCounterRequestsTotal;


        public QuoteServer()
            : this("quotes.txt")
        {
        }
        public QuoteServer(string filename)
            : this(filename, 7890)
        {
        }
        public QuoteServer(string filename, int port)
        {
            this.filename = filename;
            this.port = port;

            //InitializeComponent();
        }

        protected void ReadQuotes()
        {
            quotes = new StringCollection();
            Stream stream = File.OpenRead(filename);
            StreamReader streamReader = new StreamReader(stream);
            string quote;
            while ((quote = streamReader.ReadLine()) != null)
            {
                quotes.Add(quote);
            }
            streamReader.Close();
            stream.Close();
            random = new Random();
        }
        protected string GetRandomQuoteOfTheDay()
        {
            int index = random.Next(0, quotes.Count);
            return quotes[index];
        }
        public void Start()
        {
            ReadQuotes();
            listenerThread = new Thread(
                new ThreadStart(this.Listener));
            listenerThread.Start();
        }
        protected void Listener()
        {
            try
            {
                IPAddress ipAddress = Dns.Resolve("localhost").AddressList[0];
                listener = new TcpListener(ipAddress, port);
                listener.Start();
                while (true)
                {
                    Socket socket = listener.AcceptSocket();
                    string message = GetRandomQuoteOfTheDay();
                    UnicodeEncoding encoder = new UnicodeEncoding();
                    byte[] buffer = encoder.GetBytes(message);
                    socket.Send(buffer, buffer.Length, 0);
                    socket.Close();

                    performanceCounterRequestsTotal.Increment();
                    performanceCounterBytesSentTotal.IncrementBy(buffer.Length);

                    requestsPerSec++;
                    bytesPerSec += buffer.Length;
                }
            }
            catch (SocketException e)
            {
                string message = "Quote Server failed in Listener: "
                    + e.Message;
                eventLog.WriteEntry(message, EventLogEntryType.Error);
            }
        }
        public void Stop()
        {
            listener.Stop();
        }
        public void Suspend()
        {
            listenerThread.Suspend();
        }
        public void Resume()
        {
            listenerThread.Resume();
        }

        private void InitializeComponent()
        {
            this.performanceCounterRequestsPerSec = new System.Diagnostics.PerformanceCounter();
            this.performanceCounterBytesSentTotal = new System.Diagnostics.PerformanceCounter();
            this.performanceCounterBytesSentPerSec = new System.Diagnostics.PerformanceCounter();
            this.performanceCounterRequestsTotal = new System.Diagnostics.PerformanceCounter();
            this.timer = new System.Timers.Timer();
            this.eventLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounterRequestsPerSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounterBytesSentTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounterBytesSentPerSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounterRequestsTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).BeginInit();
            // 
            // performanceCounterRequestsPerSec
            // 
            this.performanceCounterRequestsPerSec.CategoryName = "Quote Service Counts";
            this.performanceCounterRequestsPerSec.CounterName = "# of Requests / sec";
            this.performanceCounterRequestsPerSec.MachineName = "NAGELC";
            this.performanceCounterRequestsPerSec.ReadOnly = false;
            // 
            // performanceCounterBytesSentTotal
            // 
            this.performanceCounterBytesSentTotal.CategoryName = "Quote Service Counts";
            this.performanceCounterBytesSentTotal.CounterName = "# of Bytes sent";
            this.performanceCounterBytesSentTotal.MachineName = "NAGELC";
            this.performanceCounterBytesSentTotal.ReadOnly = false;
            // 
            // performanceCounterBytesSentPerSec
            // 
            this.performanceCounterBytesSentPerSec.CategoryName = "Quote Service Counts";
            this.performanceCounterBytesSentPerSec.CounterName = "# of Bytes sent / sec";
            this.performanceCounterBytesSentPerSec.MachineName = "NAGELC";
            this.performanceCounterBytesSentPerSec.ReadOnly = false;
            // 
            // performanceCounterRequestsTotal
            // 
            this.performanceCounterRequestsTotal.CategoryName = "Quote Service Counts";
            this.performanceCounterRequestsTotal.CounterName = "# of Requests";
            this.performanceCounterRequestsTotal.MachineName = "NAGELC";
            this.performanceCounterRequestsTotal.ReadOnly = false;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 1000;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            // 
            // eventLog
            // 
            this.eventLog.Log = "Application";
            this.eventLog.Source = "QuoteService";
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounterRequestsPerSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounterBytesSentTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounterBytesSentPerSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounterRequestsTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).EndInit();

        }

        public void RefreshQuotes()
        {
            ReadQuotes();
        }

        private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            performanceCounterBytesSentPerSec.RawValue = bytesPerSec;
            performanceCounterRequestsPerSec.RawValue = requestsPerSec;
            bytesPerSec = 0;
            requestsPerSec = 0;

        }
    }
}
