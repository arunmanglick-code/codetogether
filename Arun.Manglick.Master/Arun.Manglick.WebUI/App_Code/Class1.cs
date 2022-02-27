using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

namespace Arun.Manglick.UI
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class Class1
    {
        public Class1()
        {
        }

        public void Swap<T>(ref T x, ref T y)
        {
            T temp;  
            temp = x;
            x = y;
            y = temp;
        }



    }
}
