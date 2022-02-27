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

public partial class DynamicBoundNTemplateColumnsAndAuditing : BasePage
{

    #region Private Variables

    String XmlFileName1 = "~\\XML\\DynamicColumn.xml";
    String XmlFileName2 = "~\\XML\\Status.xml";

    // Non Grid Page
    public string userName = string.Empty;
    public string pageName = string.Empty;
    private const string AuditTrailPropertyName = "AuditTrailLog";
    private const string OldDto = "OldDTO";   // Session Key
    private const string AuditTrailData = "AuditTrailData"; // Temporary Session Key

    // Grid Page
    private const string Sequence = "Sequence";
    private const string DataKey = "CourseId";
    private const string Institution = "Institution";
    private const string Course = "Course";
    private const string Row = "Row";
    private const string GridTable = "SourceTable"; // Session Key
    private const string CopiedRow = "CopiedRow"; // Session Key
    private const string TextBox = "TextBox"; 
    private const string DropDownList = "DropDownList"; 

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["TCBC"] != null)
        {
            AddBoundColumn();
        }
        if (Session["TCTBDD"] != null)
        {
            AddTemplateColumnTextBox();
            AddTemplateColumnDropDown();
        }
        if (Session["TCBCTBDD"] != null)
        {
            AddBoundColumn();
            AddTemplateColumnTextBox();
            AddTemplateColumnDropDown();
        }
        BindGrid();
    }

    #endregion

    #region Private Methods

    private void BindGrid()
    {
        GetEmptyDataTable();
        GridView1.DataSource = Session[GridTable] as DataTable;
        GridView1.DataBind();
    }
    private void GetEmptyDataTable()
    {
        DataSet ds = null;
        DataTable dt = null;
        try
        {
            if (Session[GridTable] == null)
            {
                ds = new DataSet();
                ds.ReadXml(Server.MapPath(XmlFileName1));
                dt = ds.Tables[0];
                ds.Dispose();
                Session[GridTable] = dt;
            }            
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
    private DataTable GetGridDataSource()
    {
        DataRow row = null;        
        DataTable table = null;
        int count = 2;

        if (Session[GridTable] != null)
        {
            table = Session[GridTable] as DataTable;
            foreach (GridViewRow gridRow in GridView1.Rows)
            {
                count = 0; // Why 4.. Bcoz Only the 5th Column - 'Average' is dispayed in TextBox
                if (gridRow.RowType == DataControlRowType.DataRow)
                {
                    row = table.Rows[gridRow.RowIndex];
                    row[DataKey] = Convert.ToInt32(gridRow.Cells[0].Text);
                    row[Sequence] = Convert.ToInt32(gridRow.Cells[1].Text);

                    foreach (TableCell cell in gridRow.Cells)
                    {
                        foreach (Control ctrl in cell.Controls)
                        {
                            if (ctrl.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                            {
                                TextBox txtTemp = ctrl as TextBox;
                                if (row.ItemArray.Length > count)
                                {
                                    if (txtTemp.Text.Length > 0)
                                        row[count] = (object)txtTemp.Text;
                                }
                                break;
                            }
                            else if (ctrl.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                            {
                                DropDownList drpTemp = ctrl as DropDownList;
                                if (row.ItemArray.Length > count)
                                {
                                    row[count] = (object)drpTemp.SelectedItem.Value;
                                }
                                break;
                            }
                        }
                        count++;
                    }
                }
            }
        }
        return table;
    }

    #endregion

    #region Add Dynamic Columns

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
            templateField.ItemTemplate = new GridViewItemTemplate(DataControlRowType.DataRow, "Dynamic", "txt", "", "Dynamic");
            GridView1.Columns.Add(templateField);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    private void AddTemplateColumnTextBox(String columnName)
    {
        try
        {
            TemplateField templateField = new TemplateField();
            templateField.HeaderText = "Dynamic Template Column";
            templateField.ItemTemplate = new GridViewItemTemplate(DataControlRowType.DataRow, columnName, "txt", "", "Dynamic");
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
            templateField.ItemTemplate = new GridViewItemTemplate(DataControlRowType.DataRow, "Dynamic", "ddl", "", status);
            GridView1.Columns.Add(templateField);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    private void AddTemplateColumnDropDown(String columnName)
    {
        try
        {
            DataTable status = GetDropDownDataTable();
            TemplateField templateField = new TemplateField();
            templateField.HeaderText = "Dynamic Template Column";
            templateField.ItemTemplate = new GridViewItemTemplate(DataControlRowType.DataRow, columnName, "ddl", "", status);
            GridView1.Columns.Add(templateField);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void AddTemplateColumnInDataTable(String columnName, String value)
    {
        DataTable dt = Session[GridTable] as DataTable;
        dt.Columns.Add(new DataColumn(columnName));

        for (int i = 0; i < dt.Rows.Count; i++)
        {            
            dt.Rows[i][columnName] = value + i.ToString(); // optional
        }

        Session[GridTable] = dt;
    }

    #endregion

    #region Grid Update - Audit Methods

    /// <summary>
    /// Adjust the immediate up/down sequence
    /// </summary>
    /// <param name="index">int</param>
    /// <param name="sequenceKey">string</param>
    /// <param name="employeeTable">DataTable</param>
    /// <param name="direction">MoveRow Enum</param>
    /// <returns>DataTable</returns>
    /// <history created="Arun M"></history>
    /// <history date="Jan 09, 2007"></history>
    private DataTable MoveSequence(int index, string sequenceKey, DataTable employeeTable, MoveRow direction)
    {
        DataRow row = employeeTable.Rows[index];
        int sequence = index + 1;
        DataRow nextRow = null;
        DataRow prevRow = null;
        if (direction == MoveRow.MoveDown)
        {
            nextRow = employeeTable.Rows[index + 1];
            row[sequenceKey] = sequence + 1;
            nextRow[sequenceKey] = sequence;
        }
        else
        {
            prevRow = employeeTable.Rows[index - 1];
            row[sequenceKey] = sequence - 1;
            prevRow[sequenceKey] = sequence;
        }
        return employeeTable;
    }

    /// <summary>
    /// Reshuffle the complete sequence
    /// </summary>
    /// <param name="sequenceKey">string</param>
    /// <param name="employeeTable">DataTable</param>
    /// <returns>DataTable</returns>
    /// <history created="Arun M"></history>
    /// <history date="Jan 09, 2007"></history>
    private DataTable RearrangeSequence(string sequenceKey, DataTable employeeTable)
    {
        int sequence = 1;
        foreach (DataRow row in employeeTable.Rows)
        {
            row[sequenceKey] = sequence++;
        }
        return employeeTable;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private DataTable MoveRows(int index, MoveRow moveRow)
    {
        DataTable dTable = null;
        dTable = GetGridDataSource();
        dTable = MoveSequence(index, Sequence, dTable, moveRow);

        return dTable;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private DataTable AddRow()
    {
        DataTable dTable = null;
        dTable = GetGridDataSource();
        DataRow row = dTable.NewRow();
        row[DataKey] = -1;
        dTable.Rows.Add(row);
        dTable = RearrangeSequence(Sequence, dTable);
        Session[GridTable] = dTable;
        return dTable;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private DataTable InsertRow(int index)
    {
        DataTable dTable = null;
        dTable = GetGridDataSource();
        DataRow row = dTable.NewRow();
        row[DataKey] = -1;
        dTable.Rows.InsertAt(row, index);
        dTable = RearrangeSequence(Sequence, dTable);
        Session[GridTable] = dTable;
        return dTable;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private DataTable PasteRow(int index)
    {
        DataTable dTable = null;
        dTable = GetGridDataSource();
        dTable.Rows[index].ItemArray = (Session[CopiedRow] as DataRow).ItemArray;
        dTable.Rows[index][DataKey] = -1;
        dTable = RearrangeSequence(Sequence, dTable);
        Session[GridTable] = dTable;
        return dTable;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private DataTable DeleteRow(int index)
    {
        DataTable dTable = null;
        dTable = GetGridDataSource();
        dTable.Rows.Remove(dTable.Rows[index]);
        dTable = RearrangeSequence(Sequence, dTable);
        Session[GridTable] = dTable;
        return dTable;
    }

    #endregion

    #region Control Events - Add Columns Dynamically

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
        //AddBoundColumn();
        //Session["TCBC"] = true;
        //BindGrid(); 
    }    
    protected void btnAddTemplateColumn_Click(object sender, EventArgs e)
    {
        // First Add columns in Datatable and assign some value to them
        AddTemplateColumnInDataTable("DynamicColumn1","Fresh");
        AddTemplateColumnInDataTable("DynamicColumn2","N");

        AddTemplateColumnTextBox("DynamicColumn1");
        AddTemplateColumnDropDown("DynamicColumn2");
        Session["TCTBDD"] = true;
        BindGrid(); 
    }
    protected void btnAddBoundTemplateColumn_Click(object sender, EventArgs e)
    {
        //AddBoundColumn();
        //AddTemplateColumnTextBox();
        //AddTemplateColumnDropDown();
        //Session["TCBCTBDD"] = true;
        //BindGrid(); 
    }

    #endregion

    #region Control Events - Auditing

    protected void btnNewRow_Click(object sender, EventArgs e)
    {
        DataTable dTable = null;
        try
        {
            dTable = AddRow();
            BindGrid();
        }
        catch (Exception ex)
        {
            throw;
        }

    }
    protected void btnInsertRow_Click(object sender, EventArgs e)
    {
        DataTable dTable = null;
        string rowIndex = hiddenSelectRowId.Value;
        int index = 0;

        try
        {
            if (!String.IsNullOrEmpty(rowIndex))
            {
                index = Convert.ToInt32(rowIndex);
                dTable = InsertRow(index);
                BindGrid();
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void btnCopyRow_Click(object sender, EventArgs e)
    {
        DataTable dTable = null;
        string rowIndex = hiddenSelectRowId.Value;
        int index = 0;

        try
        {
            if (!String.IsNullOrEmpty(rowIndex))
            {
                dTable = GetGridDataSource();
                index = Convert.ToInt32(rowIndex);
                Session[CopiedRow] = dTable.Rows[index];
            }
        }
        catch (Exception ex)
        {
            throw;
        }     
    }
    protected void btnPasteRow_Click(object sender, EventArgs e)
    {
        DataTable dTable = null;
        string rowIndex = hiddenSelectRowId.Value;
        int index = 0;

        try
        {
            if (!String.IsNullOrEmpty(rowIndex))
            {
                dTable = GetGridDataSource();
                index = Convert.ToInt32(rowIndex);
                if (dTable.Rows[index]["CourseId"].ToString().Equals("-1"))
                {
                    dTable = PasteRow(index);
                    GenericAudit.AuditPaste(index, userName, pageName);
                    BindGrid();
                }
                lblError.Text = "Please Select a Blank Row";
            }
        }
        catch (Exception ex)
        {
            throw;
        }   
    }

    protected void btnDeleteRow_Click(object sender, EventArgs e)
    {
        DataTable dTable = null;
        string rowIndex = hiddenSelectRowId.Value;
        int index = 0;

        try
        {
            if (!String.IsNullOrEmpty(rowIndex))
            {
                index = Convert.ToInt32(rowIndex);
                dTable = DeleteRow(index);
                GenericAudit.AuditDelete(index, userName, pageName);
                BindGrid();
            }
        }
        catch (Exception ex)
        {
            throw;
        }          
    }

    #endregion

    #region Control Events - Grid Events

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        String strTemp = String.Empty;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList dropdown = e.Row.FindControl("DropDownList1") as DropDownList;
            if (dropdown != null)
            {
                dropdown.DataSource = GetDropDownDataTable();
                dropdown.DataTextField = "value";
                dropdown.DataValueField = "key";
                dropdown.DataBind();

                strTemp = DataBinder.Eval(e.Row.DataItem, "Grade").ToString();
                if (strTemp.Equals("A"))
                {
                    dropdown.SelectedIndex = 0;
                }
                else
                {
                    dropdown.SelectedIndex = 1;
                }
            }

            dropdown = e.Row.FindControl("ddlDynamicColumn2") as DropDownList;
            if (dropdown != null)
            {
                // ------------------------------------------------------------------------------------
                // Below bidning can be done here. But for now this has been taken care in 'GridViewItemTemplate.cs'
                // ------------------------------------------------------------------------------------
                //dropdown.DataSource = GetDropDownDataTable();
                //dropdown.DataTextField = "value";
                //dropdown.DataValueField = "key";
                //dropdown.DataBind();
                // ------------------------------------------------------------------------------------

                strTemp = DataBinder.Eval(e.Row.DataItem, "DynamicColumn2").ToString();
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
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int index = (GridView1.PageIndex * GridView1.PageSize) + e.Row.RowIndex + 1;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onclick", "SetSelectedRow('" + index.ToString() + "'); setMouseOverColor(this);");
        }
    }

    #endregion

}
