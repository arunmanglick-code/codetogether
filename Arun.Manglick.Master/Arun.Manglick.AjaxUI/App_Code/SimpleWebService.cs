using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

namespace Arun.Manglick.UI
{
    /// <summary>
    /// Summary description for SimpleWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SimpleWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string EchoInput(String input)
        {
            string inputString = Server.HtmlEncode(input);
            if (!String.IsNullOrEmpty(inputString))
            {
                return String.Format("You entered {0}. The current time is {1}.", inputString, DateTime.Now);
            }
            else
            {
                return "The input string was null or empty.";
            }
        }

    }
}

