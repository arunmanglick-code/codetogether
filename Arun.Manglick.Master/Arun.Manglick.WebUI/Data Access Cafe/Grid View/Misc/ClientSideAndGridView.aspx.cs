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
using System.IO;
using System.Globalization;

public partial class ClientSideAndGridView : System.Web.UI.Page
{
    #region Private Variables

    private String FileName = "GridView.xls";
    private String XmlFileName = "~\\XML\\MoveSelection.xml";

    private const String UnAssignedObjects = "UnAssignedObjects";
    private const String AssignedObjects = "AssignedObjects";

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindUnAssignGrid();
        }
    }

    #endregion

    #region Private Methods

    private DataTable GetEmptyDataTable()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath(XmlFileName));
            DataTable dt = ds.Tables[0];
            ds.Dispose();
            return dt;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    
   
    private void BindUnAssignGrid()
    {
        if (Session[UnAssignedObjects] == null)
        {
            Session[UnAssignedObjects] = GetEmptyDataTable();
        }
        
        GridView1.DataSource = Session[UnAssignedObjects] as DataTable;
        GridView1.DataBind();
    }
    
    private Int32[] GetSelectedRows()
    {
        char[] chr = { ',' };
        String[] selList = hdfRowId.Value.Split(chr);
        Int32[] selectedItems = new Int32[selList.Length];
        for (int iCnt = 0; iCnt < selList.Length; iCnt++)
        {
            selectedItems[iCnt] = Convert.ToInt32(selList[iCnt], CultureInfo.InvariantCulture);
        }
        Array.Sort(selectedItems);
        return selectedItems;
    }

    #endregion

    #region Control Events

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        int index = -1;
        index = e.Row.RowIndex + 1;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes.Add("onclick", "ChangeRowColor(this)");
            e.Row.Attributes.Add("onclick", "ChangeRowColor(this); StoreSelectedObjRow('" + index.ToString() + "');");
        }
    }

     
    
    protected void btnPostBack_Click(object sender, EventArgs e)
    {
        if (GridView1.Rows.Count == 0)
        {
            Response.Write("Hello");
        }
    }

    
    #endregion

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int index = -1;
        index = e.Row.RowIndex + 1;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes.Add("onclick", "ChangeRowColor(this)");
            //e.Row.Attributes.Add("onclick", "ChangeRowColor(this); StoreSelectedObjRow('" + index.ToString() + "');");
        }
    }
}
