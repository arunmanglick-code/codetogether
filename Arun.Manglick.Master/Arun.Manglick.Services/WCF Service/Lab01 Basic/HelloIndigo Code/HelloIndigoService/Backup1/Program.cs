using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using HelloIndigo.Lab1.Code;

namespace HelloIndigoHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://localhost:8000/HelloIndigoCode");
            ServiceHost selfHost = new ServiceHost(typeof(HelloIndigoService), baseAddress);
            try
            {
                selfHost.AddServiceEndpoint(typeof(IHelloIndigoService), new BasicHttpBinding(), "HelloIndigoService");
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                selfHost.Description.Behaviors.Add(smb);

                selfHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

                selfHost.Open();
                Console.WriteLine("Service is Ready");
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
