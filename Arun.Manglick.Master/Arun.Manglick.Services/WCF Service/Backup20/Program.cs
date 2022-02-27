using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using HelloIndigoClient.HIServiceReferenceUnCaught;

namespace HelloIndigoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            HelloIndigoServiceClient proxy = new HelloIndigoServiceClient();

            string res = proxy.GoodOperation();
            Console.WriteLine(res);
            Console.WriteLine();

            #region Two - Way

            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Calling proxy.ThrowException()");
                Console.WriteLine("");
                Console.ResetColor();

                proxy.ThrowException();

                System.Console.WriteLine("SUCCESS: proxy.ThrowException");

            }
            catch (CommunicationException communicationException)
            {

                Console.WriteLine(communicationException.GetType().ToString());
                Console.WriteLine("Communication ERROR: {0}", communicationException.Message);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.GetType().ToString());
                Console.WriteLine("ERROR: {0}", ex.Message);
            }

            Console.WriteLine();
            #endregion 
            
            #region One - Way

            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Calling proxy.ThrowExceptionOneWay()");
                Console.ResetColor();

                proxy.ThrowExceptionOneWay();
                System.Console.WriteLine("SUCCESS: proxy.ThrowExceptionOneWay");

            }
            catch (CommunicationException communicationException)
            {

                Console.WriteLine(communicationException.GetType().ToString());
                Console.WriteLine("Communication ERROR: {0}", communicationException.Message);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.GetType().ToString());
                Console.WriteLine("ERROR:  {0}", ex.Message);
            }

            #endregion

            Console.WriteLine();
            string res1 = proxy.GoodOperation();
            Console.WriteLine(res1);
            Console.WriteLine();

            proxy.Close();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Press <ENTER> to terminate Client.");
            Console.ReadLine();
        }
    }
}

