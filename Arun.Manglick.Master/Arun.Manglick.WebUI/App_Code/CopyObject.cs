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

namespace Arun.Manglick.UI
{
    /// <summary>
    /// Note: The classes to be cloned (Deep Copy) must be flagged as [Serializable].
    /// Therefore the CopyObject is declared as 'Serializable'
    /// </summary>
    [Serializable]
    public class CopyObject
    {
        // Note: The classes to be cloned (Deep Copy) must be flagged as [Serializable]. Therefore the CopyObject must be declared as 'Serializable'

        public int Salary;
        public CopyObject(int salary)
        {
            this.Salary = salary;
        }
    }
}
