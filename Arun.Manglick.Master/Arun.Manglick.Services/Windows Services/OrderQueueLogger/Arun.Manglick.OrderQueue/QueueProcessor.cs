using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Arun.Manglick.DataManager;
using Arun.Manglick.DataAccess;
using Arun.Manglick.Utilities;

namespace Arun.Manglick.OrderQueue
{
    class QueueProcessor
    {
        /// <summary>
        /// Method to process order queue items
        /// This method gets the Orders from queue and starts a new ThreadPool thread for each item
        /// </summary>
        public void ProcessOrderQueue()
        {
            int setCount = 100;
            NWCTMOrderQueueManager queueMgr = new NWCTMOrderQueueManager();
            List<CTMOrder> queueItems = null;
            if (setCount > 0)
            {
                lock (queueMgr)
                {
                    queueItems = queueMgr.GetOrders(setCount);
                }
            }

            foreach (CTMOrder order in queueItems)
            {
                ThreadPoolHelper.QueueUserWorkItem(order.OrderId,new WaitCallback(ThreadProc));
            }

            if (queueItems.Count > 0)
            {
                // have to wait here or the background threads in the thread
                // pool would not run before the main thread exits.
                bool working = true;
                int workerThreads = 0;
                int completionPortThreads = 0;
                int maxWorkerThreads = 0;
                int maxCompletionPortThreads = 0;
                // get max threads in the pool
                System.Threading.ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);
                while (working)
                {
                    // get available threads
                    System.Threading.ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
                    if (workerThreads == maxWorkerThreads)
                    {
                        // allow to quit
                        working = false;
                    }
                    else
                    {
                        // sleep before checking again
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
        }

        static void ThreadProc(Object orderInfo)
        {
            if (!EventLog.SourceExists("OrderQueue"))
            {
                EventLog.CreateEventSource("OrderQueue", "Application");
            }

            NWCTMOrderQueueManager queueMgr = new NWCTMOrderQueueManager();
            queueMgr.MarkComplete(Convert.ToInt32(orderInfo));

            EventLog log = new EventLog();
            String message = "Order Id: " + Convert.ToInt32(orderInfo) + " Processed";
            log.Source = "OrderQueue";
            log.WriteEntry(message, EventLogEntryType.Information);
        }
        
        public void ReleaseOrderQueue()
        {

        }
    }
}
