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

public partial class GridViewSnippets : BasePage
{
    #region Private Variables

    private string pathName1 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Code Snippets\\GridView Snippets\\LoopThruGridColumnsOrCells.txt";
    private string pathName2 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Code Snippets\\GridView Snippets\\LoopThruAllControlsInGridView.txt";
    private string pathName3 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Code Snippets\\GridView Snippets\\EventHandlersforControlsContainedinGridVeiwRow.txt";
    private string pathName4 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Code Snippets\\GridView Snippets\\Sorting.txt";
    private string pathName5 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"SQL Server Cafe\\SP Snippets\\ShowEventTickets.sql";
    private string pathName6 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"SQL Server Cafe\\SP Snippets\\ShowEmployeeDetailsBanner.sql";

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
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

    #endregion

}
