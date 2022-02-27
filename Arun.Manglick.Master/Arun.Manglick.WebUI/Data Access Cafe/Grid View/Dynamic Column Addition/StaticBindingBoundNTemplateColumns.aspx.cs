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

public partial class StaticBindingBoundNTemplateColumns : System.Web.UI.Page
{

    #region Private Variables

    string FileName = "GridView.xls";
    String XmlFileName1 = "~\\XML\\DynamicColumn.xml";
    String XmlFileName2 = "~\\XML\\Grade.xml";
    String XmlFileName3 = "~\\XML\\Status.xml";

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = GetEmptyDataTable();
            Session["MyTable"] = dt;
            GridView1.DataSource = dt;
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

    private DataTable GetStatusDropDownDataTable()
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

    private DataTable GetGradeDropDownDataTable()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath(XmlFileName3));
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
            DropDownList dropdown = e.Row.FindControl("ddlGrade") as DropDownList;
            if (dropdown != null)
            {
                dropdown.DataSource = GetGradeDropDownDataTable();
                dropdown.DataTextField = "value";
                dropdown.DataValueField = "key";
                dropdown.DataBind();
            }

            dropdown = e.Row.FindControl("ddlStatus") as DropDownList;
            if (dropdown != null)
            {
                dropdown.DataSource = GetStatusDropDownDataTable();
                dropdown.DataTextField = "value";
                dropdown.DataValueField = "key";
                dropdown.DataBind();
            }
        }       
    }
    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        int index = 0;
        String controlValue = String.Empty;
        DataTable dt = Session["MyTable"] as DataTable;

        foreach (GridViewRow gRow in GridView1.Rows)
        {
            foreach (TableCell cell in gRow.Cells)
            {
                DropDownList gradeList = cell.FindControl("ddlGrade") as DropDownList;
                if (gradeList != null)
                {
                    controlValue = dt.Rows[index]["Grade"].ToString();
                    gradeList.SelectedIndex = gradeList.Items.IndexOf(gradeList.Items.FindByValue(controlValue.ToString()));
                }

                DropDownList statusList = cell.FindControl("ddlStatus") as DropDownList;
                if (statusList != null)
                {
                    controlValue = dt.Rows[index]["Status"].ToString();
                    statusList.SelectedIndex = statusList.Items.IndexOf(statusList.Items.FindByValue(controlValue.ToString()));
                }
            }

            index++;
        }

    }

    #endregion    
}
