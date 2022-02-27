using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.StackTrace
{
    class Class3
    {
        public static void ThrowClass3()
        {
            int i = 5;
            int j = 0;
            int res;
            try
            {
                res = i / j;
            }
            catch (System.Exception ex)
            {
                //throw;
            }
        }
    }
}
