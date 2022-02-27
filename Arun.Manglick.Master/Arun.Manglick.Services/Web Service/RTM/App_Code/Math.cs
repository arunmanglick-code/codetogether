using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Arun.Manglick.WebService
{

    /// <summary>
    /// Summary description for Math
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Math : System.Web.Services.WebService
    {
        #region Constructor

        public Math()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        #endregion
        
        #region Test Method

        [WebMethod]
        public int Addition(int x, int y)
        {
            return x + y;
        }

        [WebMethod]
        public float Subtract(float i, float j)
        {
            return i - j;
        }

        #endregion

    }
}

