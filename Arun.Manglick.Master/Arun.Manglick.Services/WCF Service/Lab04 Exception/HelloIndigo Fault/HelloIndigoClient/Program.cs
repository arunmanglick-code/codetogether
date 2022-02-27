using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using HelloIndigoClient.HIServiceReferenceFault;

namespace HelloIndigoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            HelloIndigoServiceClient proxy = new HelloIndigoServiceClient();
            
            #region Simple Fault

            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Calling proxy.ThrowSimpleFault()");
                Console.WriteLine("");
                Console.ResetColor();

                proxy.ThrowSimpleFault();

            }
            catch (FaultException fe)
            {
                Console.WriteLine(fe.GetType().ToString());
                Console.WriteLine("ERROR: {0}", fe.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().ToString());
                Console.WriteLine("Normal ERROR: {0}", ex.Message);
            }

            Console.WriteLine();
            #endregion

            #region Fault Exception
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Calling proxy.ThrowFaultException()");
                Console.ResetColor();

                proxy.ThrowFaultException();
            }
            catch (FaultException fe)
            {
                Console.WriteLine(fe.GetType().ToString());
                Console.WriteLine("ERROR: {0}", fe.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().ToString());
                Console.WriteLine("Normal ERROR:  {0}", ex.Message);
            }
            Console.WriteLine();

            #endregion
            
            #region Message Fault
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Calling proxy.ThrowMessageFault()");
                Console.ResetColor();

                proxy.ThrowMessageFault();

            }
            catch (FaultException fe)
            {
                Console.WriteLine(fe.GetType().ToString());
                Console.WriteLine("ERROR: {0}", fe.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().ToString());
                Console.WriteLine("Normal ERROR:  {0}", ex.Message);
            }

            Console.WriteLine();

            #endregion

            proxy.Close();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Press <ENTER> to terminate Client.");
            Console.ReadLine();
        }
    }
}
