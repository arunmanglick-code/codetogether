using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;


namespace Arun.Manglick.EventLogger
{
    partial class EventLogger : ServiceBase
    {
        public EventLogger()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.timer1.Enabled = true;
            this.LogMessage("Event Logger Service Started");

        }

        protected override void OnStop()
        {
            this.timer1.Enabled = false;
            this.LogMessage("Event Logger Service Stoped");

        }

        void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.LogMessage("Timer Clicked At " + System.DateTime.Now.ToShortTimeString());
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
