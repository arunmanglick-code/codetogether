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

public partial class SequenceDigarams : BasePage
{
    #region Private Variables

    //private string pathName1 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"UML Cafe\\Class Diagrams\\Images\\DigitalDownaload.AccessSecurity.GIF";
    private string pathName0 = "~//UML Cafe//Sequence Diagrams//Images//General.FragmentOperators.GIF";

    private string pathName1 = "~//UML Cafe//Sequence Diagrams//Images//DigitalDownload.Preview1.GIF";
    private string pathName2 = "~//UML Cafe//Sequence Diagrams//Images//DigitalDownload.Preview2.GIF";
    private string pathName3 = "~//UML Cafe//Sequence Diagrams//Images//DigitalDownload.Preview3.GIF";

    private string pathName4 = "~//UML Cafe//Sequence Diagrams//Images//Tyrelink.CreateOrder.GIF";
    private string pathName5 = "~//UML Cafe//Sequence Diagrams//Images//Tyrelink.LoginProcess.GIF";
    private string pathName6 = "~//UML Cafe//Sequence Diagrams//Images//Tyrelink.ReviewOrder.GIF";
    private string pathName7 = "~//UML Cafe//Sequence Diagrams//Images//Tyrelink.Search.GIF";
    private string pathName8 = "~//UML Cafe//Sequence Diagrams//Images//Tyrelink.SubstituteTyre.GIF";
    private string pathName9 = "~//UML Cafe//Sequence Diagrams//Images//Tyrelink.UploadOrder.GIF";

    private string pathName10 = "~//UML Cafe//Sequence Diagrams//Images//Monetrics.AuditHistory.GIF";
    private string pathName11 = "~//UML Cafe//Sequence Diagrams//Images//Monetrics.PrintPreview.GIF";
    private string pathName12 = "~//UML Cafe//Sequence Diagrams//Images//Monetrics.SavePrompt.GIF";


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

    protected void lnkNotePad0_Click(object sender, EventArgs e)
    {
        SessionManager.DiagramPath = pathName0;
        Response.Redirect(Page.ResolveUrl("~/DrawPad.aspx"));

    }

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
    protected void lnkNotePad7_Click(object sender, EventArgs e)
    {
        SessionManager.DiagramPath = pathName7;
        Response.Redirect(Page.ResolveUrl("~/DrawPad.aspx"));

    }
    protected void lnkNotePad8_Click(object sender, EventArgs e)
    {
        SessionManager.DiagramPath = pathName8;
        Response.Redirect(Page.ResolveUrl("~/DrawPad.aspx"));

    }
    protected void lnkNotePad9_Click(object sender, EventArgs e)
    {
        SessionManager.DiagramPath = pathName9;
        Response.Redirect(Page.ResolveUrl("~/DrawPad.aspx"));

    }

    protected void lnkNotePad10_Click(object sender, EventArgs e)
    {
        SessionManager.DiagramPath = pathName10;
        Response.Redirect(Page.ResolveUrl("~/DrawPad.aspx"));

    }
    protected void lnkNotePad11_Click(object sender, EventArgs e)
    {
        SessionManager.DiagramPath = pathName11;
        Response.Redirect(Page.ResolveUrl("~/DrawPad.aspx"));

    }
    protected void lnkNotePad12_Click(object sender, EventArgs e)
    {
        SessionManager.DiagramPath = pathName12;
        Response.Redirect(Page.ResolveUrl("~/DrawPad.aspx"));

    }

    #endregion

}
