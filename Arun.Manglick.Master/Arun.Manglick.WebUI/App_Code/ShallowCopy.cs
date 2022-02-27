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
    /// Summary description for ShallowCopy
    /// </summary>
    public class ShallowCopy
    {
        public static string CompanyName = "My Company";
        public int Age;
        public string EmployeeName;
        public CopyObject Salary;

        // In C# and VB.NET, shallow copy is done by the object method MemberwiseClone()
        public ShallowCopy MakeShallowCopy(ShallowCopy inputcls)
        {
            return inputcls.MemberwiseClone() as ShallowCopy;
        }
    }
}
