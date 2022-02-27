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

/// <summary>
/// Summary description for ExtensionSupportHandler
/// </summary>
public class ExtensionSupportHandler : IHttpHandler
{
    public ExtensionSupportHandler()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region IHttpHandler Members

    public bool IsReusable
    {
        get { return true; }
    }

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Write("<b>Hi! You have a support for .arun Extension</b>");
    }

    #endregion
}
