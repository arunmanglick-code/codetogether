using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1.InheritanceMyth
{
    class MinorBase : MajorBase
    {
        public string name;
        public string GetMinorMessage()
        {
            return "Minor Base";
        }       
    }
}
