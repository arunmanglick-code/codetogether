using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.PublicPrivateInterfaceInheritanceMyth
{
    class ParentClass : MyInterface
    {
        #region Explicit

        string MyInterface.GetMessage()
        {
            return "Explicit";
        }

        #endregion
    }


}
