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

public partial class GridRowMultiSelectionAndMovement : System.Web.UI.Page
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
            BindAssignGrid();
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

    private void MoveItem(bool allItems)
    {
        DataTable unAssignTable = null;
        DataTable assignTable = null;
        Int32[] selectedRows = null;
        DataRow row = null;
        DataRow unassignedRow = null;
        int index = 0;
        int count = 0;
        bool flag = true;

        unAssignTable = Session[UnAssignedObjects] as DataTable;
        if (Session[AssignedObjects] != null)
        {
            assignTable = Session[AssignedObjects] as DataTable;
            if (assignTable.Rows.Count == 0)
            {
                assignTable = unAssignTable.Clone();
            }

            if (unAssignTable != null && assignTable != null)
            {
                #region Accumulate Selected Rows
                if (allItems)
                {
                    selectedRows = new Int32[unAssignTable.Rows.Count];
                    for (int iCnt = 1; iCnt <= unAssignTable.Rows.Count; iCnt++)
                    {
                        selectedRows[iCnt - 1] = iCnt;
                    }
                }
                else
                {
                    selectedRows = GetSelectedRows();
                }
                #endregion

                #region Move Selected Rows
                count = selectedRows.Length - 1;
                for (int iCnt = count; iCnt >= 0; iCnt--)
                {
                    index = selectedRows[iCnt] - 1;
                    row = assignTable.NewRow();
                    unassignedRow = unAssignTable.Rows[index];
                    if (unassignedRow != null)
                    {
                        row.ItemArray = unassignedRow.ItemArray;
                        assignTable.Rows.Add(row);
                        unAssignTable.Rows.RemoveAt(index);
                    }
                }
                #endregion
            }

            Session[AssignedObjects] = assignTable;
            Session[UnAssignedObjects] = unAssignTable;
            BindUnAssignGrid();
            BindAssignGrid();
            hdfRowId.Value = String.Empty;
        }        
    }

    private void RemoveItems(bool allItems)
    {
        DataTable unAssignTable = null;
        DataTable assignTable = null;
        DataRow removedRow = null;
        Int32[] selectedRows = null;
        DataRow row = null;
        int index = 0;
        int count = 0;

        unAssignTable = Session[UnAssignedObjects] as DataTable;
        if (Session[AssignedObjects] != null)
        {
            assignTable = Session[AssignedObjects] as DataTable;
            if (unAssignTable != null && assignTable != null)
            {
                #region Accumulate Selected Rows
                if (allItems)
                {
                    selectedRows = new Int32[assignTable.Rows.Count];
                    for (int iCnt = 1; iCnt <= assignTable.Rows.Count; iCnt++)
                    {
                        selectedRows[iCnt - 1] = iCnt;
                    }
                }
                else
                {
                    selectedRows = GetSelectedRows();
                }
                #endregion

                #region Move Selected Rows
                count = selectedRows.Length - 1;
                for (int iCnt = count; iCnt >= 0; iCnt--)
                {
                    index = selectedRows[iCnt] - 1;
                    row = unAssignTable.NewRow();
                    removedRow = assignTable.Rows[index];
                    if (removedRow != null)
                    {
                        row.ItemArray = removedRow.ItemArray;
                        unAssignTable.Rows.Add(row);
                        assignTable.Rows.RemoveAt(index);
                    }
                }
                #endregion
            }

            Session[AssignedObjects] = assignTable;
            BindAssignGrid();
            BindUnAssignGrid();
            hdfRowId.Value = String.Empty;
        }
    }

    private void BindAssignGrid()
    {
        if (Session[AssignedObjects] != null)
        {
            GridView2.DataSource = Session[AssignedObjects] as DataTable;
            GridView2.DataBind();
        }
        else
        {
            Session[AssignedObjects] = new DataTable();
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
            e.Row.Attributes.Add("onclick", "SetSelectedObjRow('" 
                                                                + index.ToString() + "','"
                                                                + hdfRowId.ClientID + "','" 
                                                                + hdfLastSelectedRow.ClientID + "','"
                                                                + hdfCurrentGrid.ClientID + "','" 
                                                                + GridView2.ClientID + "','"
                                                                + GridView1.ClientID + "');");
        }
    }

    protected void GridView2_RowCreated(object sender, GridViewRowEventArgs e)
    {
        int index = -1;
        index = e.Row.RowIndex + 1;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes.Add("onclick", "ChangeRowColor(this)");
            e.Row.Attributes.Add("onclick", "SetSelectedObjRow('"
                                                                + index.ToString() + "','"
                                                                + hdfRowId.ClientID + "','"
                                                                + hdfLastSelectedRow.ClientID + "','"
                                                                + hdfCurrentGrid.ClientID + "','"
                                                                + GridView1.ClientID + "','"
                                                                + GridView2.ClientID + "');");
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
    }

    protected void btnMove_Click(object sender, EventArgs e)
    {
        MoveItem(false);
    }

    protected void btnMoveAll_Click(object sender, EventArgs e)
    {
        MoveItem(true);
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(hdfRowId.Value))
            {
                RemoveItems(false);
            }            
        }
        catch
        {
            throw;
        }
    }

    protected void btnRemoveAll_Click(object sender, EventArgs e)
    {
        RemoveItems(true);
    }
       
    #endregion
}
