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

public partial class StaticBoundNTemplateColumns : System.Web.UI.Page
{

    #region Private Variables

    string FileName = "GridView.xls";
    String XmlFileName1 = "~\\XML\\DynamicColumn.xml";
    String XmlFileName2 = "~\\XML\\Status.xml";

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridView1.DataSource = GetEmptyDataTable();
            GridView1.DataBind();
        }
    }

    #endregion

    #region Private Methods

    private DataTable GetEmptyDataTable()
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

    protected void btnSimple_Click(object sender, EventArgs e)
    {
        // No Need to Rebind on Simple Postback, as the changed state is automatically maintained when 'EnableViewState="True"' for GridView
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        // Code to rebind must be placed in Page_Load
        // Firing the BindGrid here in Button Click event handler will not server the purpose, irrespective of 'EnableViewState= True / False' for GridView

        GridView1.DataSource = GetEmptyDataTable();
        GridView1.DataBind();
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList dropdown = e.Row.FindControl("DropDownList2") as DropDownList;
            if (dropdown != null)
            {
                dropdown.DataSource = GetDropDownDataTable();
                dropdown.DataTextField = "value";
                dropdown.DataValueField = "key";
                dropdown.DataBind();
            }
        }       
    }

    #endregion

}
