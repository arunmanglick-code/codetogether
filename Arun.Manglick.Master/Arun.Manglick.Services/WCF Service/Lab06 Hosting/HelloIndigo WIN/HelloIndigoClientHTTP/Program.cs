using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloIndigoClientHTTP.HIServiceReferenceLab6WINHttp;

namespace HelloIndigoClientHTTP
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
