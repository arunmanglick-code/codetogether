using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Xml;
using System.Web.Script.Services;

namespace Arun.Manglick.UI
{

    /// <summary>
    /// Summary description for ComplexWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ComplexWebService : System.Web.Services.WebService
    {
        #region Private Variables

        private string _xmlString =
            @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                <message>
                    <content>
                        Welcome to the asynchronous communication layer world!
                    </content>
                </message>";

        #endregion

        #region Constructor

        public ComplexWebService()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        #endregion

        #region Web Methods

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string GetServerTime()
        {
            string serverTime =String.Format("The current time is {0}.", DateTime.Now);
            return serverTime;
        }

        [WebMethod]
        public string Add(int a, int b)
        {
            int addition = a + b;
            string result = String.Format("The addition result is {0}.", addition.ToString());
            return result;
        }

        [System.Web.Services.WebMethod]
        public string Sum(SumObject objSum)
        {
            try
            {
                int addition = objSum.Number1 + objSum.Number2;
                string result = String.Format("The addition result is {0}.", addition.ToString());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public XmlDocument GetXmlDocument()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(_xmlString);
            return xmlDoc;
        }

        // This method uses GET instead of POST.
        // For this reason its input parameters are sent by the client in the URL query string.
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string EchoStringAndDate(DateTime dt, string s)
        {
            return s + ":" + dt.ToString();
        }        

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml,XmlSerializeString = true)]
        public string GetString()
        {
            return "Hello World";
        }

        #endregion
    }
}

