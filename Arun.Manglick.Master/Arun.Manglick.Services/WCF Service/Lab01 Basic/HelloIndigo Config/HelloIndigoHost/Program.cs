using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using HelloIndigo.Lab1.Config;

namespace HelloIndigoHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost selfHost = null;
            try
            {
                selfHost = new ServiceHost(typeof(HelloIndigoService));
                selfHost.Open();
                Console.WriteLine("Service is Ready. Go & Run Client..");
                Console.WriteLine("Press <Enter> to terminate service");
                Console.WriteLine();
                Console.ReadLine();
                selfHost.Close();
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine("An exception occurred: {0}", ex.Message);
                selfHost.Abort();
            }
        }


    }
}
