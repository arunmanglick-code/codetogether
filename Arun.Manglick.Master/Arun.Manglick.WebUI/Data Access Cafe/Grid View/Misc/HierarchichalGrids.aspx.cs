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

public partial class TemplatePage : System.Web.UI.Page
{
    #region Private Variables

    private DataSet ds = new DataSet();

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            BindDetailReport();
        }
        catch (Exception ex)
        {
            string str = ex.Message;
        }
    }

    #endregion
    
    #region Private Methods

    private void BindDetailReport()
    {
        DataTable table1 = GetEmptyDataTable1();
        DataTable table2 = GetEmptyDataTable2();
        DataTable table3 = GetEmptyDataTable3();

        table1.TableName = "Table1";
        table2.TableName = "Table2";
        table3.TableName = "Table3";

        ds.Tables.AddRange(new DataTable[] { table1, table2, table3 });

        GridView1.DataSource = ds.Tables;
        GridView1.DataBind();
    }

    private DataTable GetEmptyDataTable1()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath("~\\XML\\AuditXML.xml"));
            DataTable dt = ds.Tables[0];
            ds.Dispose();
            return dt.Copy();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    private DataTable GetEmptyDataTable2()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath("~\\XML\\Table2.xml"));
            DataTable dt = ds.Tables[0];
            
            ds.Dispose();
            return dt.Copy();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    private DataTable GetEmptyDataTable3()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath("~\\XML\\Table3.xml"));
            DataTable dt = ds.Tables[0];
            ds.Dispose();
            return dt.Copy();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region Control Events

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView innerGrid = e.Row.FindControl("GridView2") as GridView;
            innerGrid.DataSource = ds.Tables[e.Row.RowIndex];
            innerGrid.DataBind();
            
            HtmlImage plus = e.Row.FindControl("imgPlus") as HtmlImage;
            HtmlImage minus = e.Row.FindControl("imgMinus") as HtmlImage;

            plus.Attributes.Add("onclick", "ShowGrid('" + innerGrid.ClientID + "','" + plus.ClientID + "','" + minus.ClientID + "');");
            minus.Attributes.Add("onclick", "HideGrid('" + innerGrid.ClientID + "','" + plus.ClientID + "','" + minus.ClientID + "');");

        }

    }

    #endregion
}
