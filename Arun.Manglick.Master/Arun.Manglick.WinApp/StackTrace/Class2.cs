using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.StackTrace
{
    class Class2
    {
        public static void ThrowClass2()
        {            
            try
            {
                Class3.ThrowClass3();
            }
            catch (System.Exception ex)
            {
                // throw ex; // Avoid it,otherwise you'll losse the Stack Trace
                throw;
            }
        }
    }
}
