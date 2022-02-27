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

    string FileName = "GridView.xls";
    String XmlFileName1 = "~\\XML\\SPList.xml";
    String XmlFileName2 = "~\\XML\\Status.xml";


    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridView1.DataSource = GetSPDataTable();
            GridView1.DataBind();
        }
    }

    #endregion

    #region Private Methods

    private DataTable GetSPDataTable()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath(XmlFileName1));
            DataTable dt = ds.Tables[0];
            ds.Dispose();
            return dt;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private DataTable GetDropDownDataTable()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath(XmlFileName2));
            DataTable dt = ds.Tables[0];
            ds.Dispose();
            return dt;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtDesc = e.Row.FindControl("txtDesc") as TextBox;
            BulletedList list = e.Row.FindControl("BulletedList1") as BulletedList;

            if (e.Row.DataItem != null)
            {
                String strDesc = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
                String[] desc = strDesc.Split(new char[] { ':' });

                list.DataSource = desc;
            }
        }
    }

    protected void SPShowHandler(object sender, EventArgs e)
    {
        try
        {
            GridViewRow selectedRow = null;
            String path = "";
            LinkButton button;

            button = (sender) as LinkButton;
            selectedRow = button.Parent.Parent as GridViewRow;
            Label lblPath = selectedRow.FindControl("lblPath") as Label;
            if (lblPath != null)
            {
                path = lblPath.Text;
                path = AppDomain.CurrentDomain.BaseDirectory.ToString() + path;

                base.ReadTextStream(path);
                Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));
            }
        }
        catch (Exception objException)
        {
           
        }
    }
    #endregion

}
