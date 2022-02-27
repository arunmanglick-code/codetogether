using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Arun.Manglick.UI;

public partial class ClassDigarams : BasePage
{
    #region Private Variables

    //private string pathName1 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"UML Cafe\\Class Diagrams\\Images\\DigitalDownaload.AccessSecurity.GIF";
    private string pathName1 = "~//UML Cafe//Class Diagrams//Images//DigitalDownload.AccessSecurity.GIF";
    private string pathName2 = "~//UML Cafe//Class Diagrams//Images//DigitalDownload.BasicFramework.GIF";
    private string pathName3 = "~//UML Cafe//Class Diagrams//Images//DigitalDownload.AdminAspect.GIF";
    private string pathName4 = "~//UML Cafe//Class Diagrams//Images//DigitalDownload.CoreFunctionality.GIF";
    private string pathName5 = "~//UML Cafe//Class Diagrams//Images//DigitalDownload.BasicFramework.GIF";
    private string pathName6 = "~//UML Cafe//Class Diagrams//Images//DigitalDownload.BasicFramework.GIF";


    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #endregion

    #region Private Methods
    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void lnkNotePad1_Click(object sender, EventArgs e)
    {
        SessionManager.DiagramPath = pathName1;
        Response.Redirect(Page.ResolveUrl("~/DrawPad.aspx"));

    }
    protected void lnkNotePad2_Click(object sender, EventArgs e)
    {
        SessionManager.DiagramPath = pathName2;
        Response.Redirect(Page.ResolveUrl("~/DrawPad.aspx"));

    }
    protected void lnkNotePad3_Click(object sender, EventArgs e)
    {
        SessionManager.DiagramPath = pathName3;
        Response.Redirect(Page.ResolveUrl("~/DrawPad.aspx"));

    }

    protected void lnkNotePad4_Click(object sender, EventArgs e)
    {
        SessionManager.DiagramPath = pathName4;
        Response.Redirect(Page.ResolveUrl("~/DrawPad.aspx"));

    }

    protected void lnkNotePad5_Click(object sender, EventArgs e)
    {
        SessionManager.DiagramPath = pathName5;
        Response.Redirect(Page.ResolveUrl("~/DrawPad.aspx"));

    }

    protected void lnkNotePad6_Click(object sender, EventArgs e)
    {
        SessionManager.DiagramPath = pathName6;
        Response.Redirect(Page.ResolveUrl("~/DrawPad.aspx"));

    }

    #endregion

}
