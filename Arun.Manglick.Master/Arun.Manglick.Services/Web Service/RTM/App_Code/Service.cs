using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;


namespace Arun.Manglick.WebService
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {
        #region Constructor
        public Service()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }
        #endregion

        #region Test Method
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        #endregion

        #region Overloading

        [WebMethod]
        public string TestOverloading()
        {
            return "Overloading";
        }

        [WebMethod(MessageName = "Overloaded")]
        public string TestOverloading(int i)
        {
            return "Overloading";
        }

        #endregion

    }
}
