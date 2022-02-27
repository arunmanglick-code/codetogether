using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.MultiInheritanceMyth
{
    class MyClass : InterfaceA,InterfaceB
    {        
        #region InterfaceA Members

        public string GetMessage()
        {
            return "InterfaceA";
        }

        #endregion

        #region InterfaceB Members

        string InterfaceB.GetMessage()
        {
            return "InterfaceB";
        }

        #endregion
    }
}
