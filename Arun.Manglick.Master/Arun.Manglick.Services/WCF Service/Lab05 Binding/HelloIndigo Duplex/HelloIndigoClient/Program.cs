using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;
using HelloIndigoClient.HIServiceReferenceLab5Duplex;

namespace HelloIndigoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client running on thread {0}", Thread.CurrentThread.GetHashCode());
            Console.WriteLine();

            HelloIndigoCallBack cb = new HelloIndigoCallBack();
            InstanceContext context = new InstanceContext(cb);

            using (HelloIndigoContractClient proxy = new HelloIndigoContractClient(context))
            {
                Console.WriteLine("Calling Service...");
                proxy.HelloIndigo("");
                Console.WriteLine("Press <ENTER> to terminate Client.");
                Console.ReadLine();
            }
        }
    }
}
