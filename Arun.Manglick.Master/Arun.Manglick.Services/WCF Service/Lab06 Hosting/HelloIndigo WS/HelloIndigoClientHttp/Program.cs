using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloIndigoClientHttp.HIServiceReferenceLab6WSHttp;
namespace HelloIndigoClientHttp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (HelloIndigoServiceClient selfProxy = new HelloIndigoServiceClient())
            {
                string s = selfProxy.HelloIndigo();
                Console.WriteLine(s);

            }

            Console.WriteLine("Press <ENTER> to terminate Client.");
            Console.ReadLine();
        }
    }
}
