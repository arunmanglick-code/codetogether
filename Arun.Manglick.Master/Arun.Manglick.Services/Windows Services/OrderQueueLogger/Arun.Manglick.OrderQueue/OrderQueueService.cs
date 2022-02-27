using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Arun.Manglick.OrderQueue
{
    partial class OrderQueueService : ServiceBase
    {
        public OrderQueueService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.timer1.Enabled = true;
            this.LogMessage("OrderQueue Service Started");

        }

        protected override void OnStop()
        {
            this.timer1.Enabled = false;
            this.LogMessage("OrderQueue Service Stoped");
        }

        void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.LogMessage("Timer Clicked At " + System.DateTime.Now.ToShortTimeString());
            QueueProcessor objProcessor = null;

            try
            {
                objProcessor = new QueueProcessor();
                objProcessor.ProcessOrderQueue();
            }
            catch (Exception ex)
            {
                if (objProcessor != null)
                {
                    objProcessor.ReleaseOrderQueue();
                    objProcessor = null;
                }

                this.LogMessage("Error When Timer Clicked At " + System.DateTime.Now.ToShortTimeString());
            }
        }

        protected void LogMessage(String message)
        {
            if (!EventLog.SourceExists("OrderQueue"))
            {
                EventLog.CreateEventSource("OrderQueue", "Application");
            }

            EventLog log = new EventLog();

            log.Source = "OrderQueue";
            log.WriteEntry(message, EventLogEntryType.Information);

        }
    }
}
