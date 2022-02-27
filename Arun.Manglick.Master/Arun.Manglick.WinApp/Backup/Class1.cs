using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1
{
    interface Test
    {
        string  MyMethod();
    }

    interface AnotherTest : Test
    {
        string YourMethod();
    }
    class Class1 : AnotherTest
    {

        #region test2 Members

        public string YourMethod()
        {
            return "Your Method";
        }

        #endregion

        #region test Members

        public string MyMethod()
        {
            return "My Method";
        }

        public void MyMethod1()
        {
            int i = 1;
            int I = 3;
            try
            {
                i = 2;
            }
            catch (Exception ex)
            {
                throw;
            }
            i = 3;
        }

        #endregion
    }
}
