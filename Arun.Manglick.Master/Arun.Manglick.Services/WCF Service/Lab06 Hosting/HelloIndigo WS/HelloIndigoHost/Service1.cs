using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using HelloIndigo.Lab6.WS;

namespace HelloIndigoHost
{
    public partial class HelloIndigoHost : ServiceBase
    {
        ServiceHost selfHost = null;

        public HelloIndigoHost()
        {
            InitializeComponent();
            this.ServiceName = "HelloIndigo WCFHost WindowsService";
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                selfHost = new ServiceHost(typeof(HelloIndigoService));
                selfHost.Faulted += new EventHandler(selfHost_Faulted);
                selfHost.Open();

                string baseAddresses = "";
                foreach (Uri address in selfHost.BaseAddresses)
                {
                    baseAddresses += " " + address.AbsoluteUri;
                }
                string s = String.Format("{0} Started & Running at {1}", this.ServiceName, baseAddresses);
                this.EventLog.WriteEntry(s, EventLogEntryType.Information);
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine("An exception occurred: {0}", ex.Message);
                selfHost.Abort();
            }
        }

        void selfHost_Faulted(object sender, EventArgs e)
        {
            string s = String.Format("{0} has faulted, notify administrators of this problem", this.ServiceName);
            this.EventLog.WriteEntry(s, EventLogEntryType.Error);
        }

        protected override void OnStop()
        {
            if (selfHost != null)
            {
                selfHost.Close();
                string s = String.Format("{0} stopped", this.ServiceName);
                this.EventLog.WriteEntry(s, EventLogEntryType.Information);
            }

            selfHost = null;
        }

        protected void LogMessage(String message)
        {
            if (!EventLog.SourceExists("Event Logger"))
            {
                EventLog.CreateEventSource("Event Logger", "Application");
            }

            EventLog log = new EventLog();

            log.Source = "Event Logger";
            log.WriteEntry(message, EventLogEntryType.Information);

        }
    }
}
