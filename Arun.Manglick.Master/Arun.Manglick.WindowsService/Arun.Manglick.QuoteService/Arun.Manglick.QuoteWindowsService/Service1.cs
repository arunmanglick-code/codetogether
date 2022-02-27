using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Arun.Manglick.BusinessProvider;

namespace Arun.Manglick.QuoteWindowsService
{
    public partial class Service1 : ServiceBase
    {
        private QuoteServer quoteServer;

        public Service1()
        {
            InitializeComponent();
        }
               

        protected override void OnStart(string[] args)
        {
            quoteServer = new QuoteServer(@"c:\ProCSharp\Services\quotes.txt",
                5678);
            quoteServer.Start();
        }
        protected override void OnStop()
        {
            quoteServer.Stop();
        }
        protected override void OnPause()
        {
            quoteServer.Suspend();
        }
        protected override void OnContinue()
        {
            quoteServer.Resume();
        }
        protected override void OnShutdown()
        {
            OnStop();
        }

        public const int commandRefresh = 128;
        protected override void OnCustomCommand(int command)
        {
            switch (command)
            {
                case commandRefresh:
                    quoteServer.RefreshQuotes();
                    break;
                default:
                    break;
            }
        }
    }
}
