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

public partial class DynamicBoundNTemplateColumns : BasePage
{

    #region Private Variables

    String XmlFileName1 = "~\\XML\\DynamicColumn.xml";
    String XmlFileName2 = "~\\XML\\Status.xml";

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["TCBC"] != null)
        {
            AddBoundColumn();
        }
        if (Session["TCTB"] != null)
        {
            AddTemplateColumnTextBox();
        }
        if (Session["TCDD"] != null)
        {
            AddTemplateColumnDropDown();
        }
        BindGrid();
    }

    #endregion

    #region Private Methods

    private void BindGrid()
    {
        GridView1.DataSource = GetEmptyDataTable();
        GridView1.DataBind();
    }
    private DataTable GetEmptyDataTable()
    {
        DataSet ds = null;
        DataTable dt = null;
        try
        {
            if (Session["SourceTable"] == null)
            {
                ds = new DataSet();
                ds.ReadXml(Server.MapPath(XmlFileName1));
                dt = ds.Tables[0];
                ds.Dispose();
                Session["SourceTable"] = dt;
            }
            return Session["SourceTable"] as DataTable;
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

    private void AddBoundColumn()
    {
        BoundField boundField = new BoundField();
        boundField.HeaderText = "Dynamic Sequence";
        boundField.DataField = "Sequence";
        boundField.HtmlEncode = false;
        GridView1.Columns.Add(boundField);
    }
    private void AddTemplateColumnTextBox()
    {
        try
        {            
            TemplateField templateField = new TemplateField();
            templateField.HeaderText = "Dynamic Template Column";
            templateField.ItemTemplate = new GridViewItemTemplate(DataControlRowType.DataRow, "DynamicSequence", "txt", "", "Dynamic");
            GridView1.Columns.Add(templateField);
        }
        catch (Exception ex)
        {            
            throw;
        }
    }
    private void AddTemplateColumnDropDown()
    {
        try
        {
            DataTable status = GetDropDownDataTable();
            TemplateField templateField = new TemplateField();
            templateField.HeaderText = "Dynamic Template Column";
            templateField.ItemTemplate = new GridViewItemTemplate(DataControlRowType.DataRow, "DynamicSequence", "ddl", "", status);
            GridView1.Columns.Add(templateField);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private DataTable ShowData()
    {
        DataSet ds = null;
        DataTable dt = null;
        try
        {
            return null;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    #endregion

    #region Control Events

    protected void btnSimple_Click(object sender, EventArgs e)
    {
        // Required to Rebind on Simple Postback also, as the changed state is not automatically maintained when 'EnableViewState="False"' for GridView
    }    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        // Code to rebind must be placed in Page_Load
        // Firing the BindGrid here in Button Click event handler will not server the purpose, irrespective of 'EnableViewState= True / False' for GridView
    }

    protected void btnAddBoundColumn_Click(object sender, EventArgs e)
    {
        AddBoundColumn();
        Session["TCBC"] = true;
        BindGrid(); // This is required to take the newly added columns in effect. As a consequence it will clear the changed state.
    }    
    protected void btnAddTemplateColumn_Click(object sender, EventArgs e)
    {
        AddTemplateColumnTextBox();
        Session["TCTB"] = true;
        BindGrid(); // This is required to take the newly added columns in effect. As a consequence it will clear the changed state.
    }
    protected void btnAddTemplateColumnDropDown_Click(object sender, EventArgs e)
    {
        AddTemplateColumnDropDown();
        Session["TCDD"] = true;
        BindGrid(); // This is required to take the newly added columns in effect. As a consequence it will clear the changed state.
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList dropdown = e.Row.FindControl("DropDownList1") as DropDownList;
            if (dropdown != null)
            {
                dropdown.DataSource = GetDropDownDataTable();
                dropdown.DataTextField = "value";
                dropdown.DataValueField = "key";
                dropdown.DataBind();

                string strTemp = DataBinder.Eval(e.Row.DataItem, "Grade").ToString();
                if (strTemp.Equals("A"))
                {
                    dropdown.SelectedIndex = 0;
                }
                else
                {
                    dropdown.SelectedIndex = 1;
                }
            }
        }
    }

    #endregion

}
