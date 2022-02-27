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

public partial class SPSnippets : BasePage
{
    #region Private Variables

    private string pathName1 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"SQL Server Cafe\\SP Snippets\\MxSP.sql";
    private string pathName2 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"SQL Server Cafe\\SP Snippets\\XMLPacket.sql";
    private string pathName3 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"SQL Server Cafe\\SP Snippets\\ConvertFunction.sql";
    private string pathName4 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"SQL Server Cafe\\SP Snippets\\SearchEvent.sql";
    private string pathName5 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"SQL Server Cafe\\SP Snippets\\ShowEventTickets.sql";
    private string pathName6 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"SQL Server Cafe\\SP Snippets\\ShowEmployeeDetailsBanner.sql";
    private string pathName7 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"SQL Server Cafe\\SP Snippets\\CreateTable.sql";
    private string pathName8 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"SQL Server Cafe\\SP Snippets\\AlterTable.sql";
    private string pathName9 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"SQL Server Cafe\\SP Snippets\\InsertTable.sql";
    private string pathName10 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"SQL Server Cafe\\SP Snippets\\CreateProcedure1.sql";


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
        base.ReadTextStream(pathName1);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));

    }
    protected void lnkNotePad2_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(pathName2);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));

    }
    protected void lnkNotePad3_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(pathName3);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));

    }

    protected void lnkNotePad4_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(pathName4);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));

    }

    protected void lnkNotePad5_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(pathName5);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));

    }

    protected void lnkNotePad6_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(pathName6);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));

    }

    protected void lnkNotePad7_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(pathName7);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));

    }

    protected void lnkNotePad8_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(pathName8);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));

    }

    protected void lnkNotePad9_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(pathName9);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));

    }

    protected void lnkNotePad10_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(pathName10);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));

    }

    #endregion

}
