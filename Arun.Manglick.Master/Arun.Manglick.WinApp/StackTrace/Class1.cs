using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.StackTrace
{
    class Class1
    {
        public static void ThrowClass1()
        {
            try
            {
                Class2.ThrowClass2();
            }
            catch (System.Exception ex)
            {
                // throw ex; // Avoid it,otherwise you'll losse the Stack Trace
                throw;
            }
        }
    }
}
