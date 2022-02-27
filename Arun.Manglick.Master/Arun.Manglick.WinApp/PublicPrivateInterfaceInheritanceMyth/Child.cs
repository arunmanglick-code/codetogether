using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.PublicPrivateInterfaceInheritanceMyth
{
    class ChildClass : ParentClass, MyInterface
    {
        #region MyInterface Members

        string MyInterface.GetMessage()
        {
            return "Child Explicit";
        }

        #endregion
    }


}
