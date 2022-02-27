using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Resources;
using System.Threading;
using System.Globalization;
using System.IO;
using Arun.Manglick.UI;


public partial class GenericAuditTrail : BasePage
{
    #region Private Variables

    // Non Grid Page
    public string userName = string.Empty;
    public string pageName = string.Empty;
    private const string AuditTrailPropertyName = "AuditTrailLog";
    private const string OldDto = "OldDTO";   // Session Key
    private const string AuditTrailData = "AuditTrailData"; // Temporary Session Key

    // Grid Page
    private const string Sequence = "Sequence";
    private const string DataKey = "CourseId";
    private const string Row = "Row";
    private const string GridTable = "GridTable"; // Session Key
    private const string CopiedRow = "CopiedRow"; // Session Key

    #endregion

    #region Page Events

    /// <summary>
    /// Page load event for the Audit Trail
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 05, 2007"></history>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userName = User.Identity.Name;
            pageName = Page.Title;

            if (!Page.IsPostBack)
            {
                BindPage();
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Save button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 06, 2007"></history>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Employee employeeDto = null;
        EmployeeCaption employeeCaptionDto = null;
        try
        {
            employeeDto = new Employee();
            employeeCaptionDto = new EmployeeCaption();

            employeeDto = FillNonGridDTO(employeeDto);
            employeeDto = FillGridDTO(employeeDto);

            employeeCaptionDto = FillLabelCaptionDTO(employeeCaptionDto);
            employeeCaptionDto = FillHeaderTextDTO(employeeCaptionDto);

            bool dirtyNonGrid = true;
            bool dirtyGrid = true;
            //employeeDto = GenericAudit.AuditPage(Session[OldDto] as Employee, employeeDto, employeeCaptionDto, DataKey, Sequence, AuditTrailPropertyName, dirtyNonGrid, dirtyGrid, userName, pageName) as Employee;
            //Services.Employee.SaveEmployeeData(employeeDto);

            Session[AuditTrailData] = employeeDto.AuditTrailLog; // Temporary - To show the report immediately in a seperate Page.
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region Page Level Methods

    /// <summary>
    /// Method to bind data source to the grid view
    /// </summary>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 05, 2007"></history>
    private void BindPage()
    {
        int employeeId = 2;
        Employee employeeDto = Employee.GetEmployeeData();
        if (employeeDto != null)
        {
            txtEmployeeId.Text = employeeDto.Id.ToString();
            txtFirstName.Text = employeeDto.FirstName;
            txtLastName.Text = employeeDto.LastName;
            txtAge.Text = employeeDto.Age.ToString();
            Session[OldDto] = employeeDto;
        }
    }
       
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Employee FillNonGridDTO(Employee employeeDto)
    {
        if (!String.IsNullOrEmpty(txtEmployeeId.Text))
            employeeDto.Id = Convert.ToInt32(txtEmployeeId.Text);
        if (!String.IsNullOrEmpty(txtFirstName.Text))
            employeeDto.FirstName = txtFirstName.Text;
        if (!String.IsNullOrEmpty(txtLastName.Text))
            employeeDto.LastName = txtLastName.Text;
        if (!String.IsNullOrEmpty(txtAge.Text))
            employeeDto.Age = Convert.ToInt32(txtAge.Text);

        return employeeDto;
    }  
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private EmployeeCaption FillLabelCaptionDTO(EmployeeCaption employeeCaptionDto)
    {
        //String fileName = Request.AppRelativeCurrentExecutionFilePath.Substring(Request.AppRelativeCurrentExecutionFilePath.IndexOf("/") + 1);
        //String countryLanguageCode = Thread.CurrentThread.CurrentUICulture.Name;
        //String translateResourceFilePath = "App_LocalResources//" + fileName + "." + countryLanguageCode + ".resx";

        //if (!File.Exists(Server.MapPath(translateResourceFilePath)))
        //{
        //    translateResourceFilePath = "App_LocalResources//" + fileName + ".resx";
        //}

        //if (File.Exists(Server.MapPath(translateResourceFilePath)))
        //{
        //    ResXResourceReader resxReader = new ResXResourceReader(Server.MapPath(translateResourceFilePath));

        //    foreach (DictionaryEntry resxItem in resxReader)
        //    {
        //        if (resxItem.Value.ToString().Equals(lblEmployeeId.Text, StringComparison.CurrentCulture))
        //        {
        //            employeeCaptionDto.Id = resxItem.Key.ToString();
        //        }
        //        else if (resxItem.Value.ToString().Equals(lblFirstName.Text, StringComparison.CurrentCulture))
        //        {
        //            employeeCaptionDto.FirstName = resxItem.Key.ToString();
        //        }
        //        else if (resxItem.Value.ToString().Equals(lblLastName.Text, StringComparison.CurrentCulture))
        //        {
        //            employeeCaptionDto.LastName = resxItem.Key.ToString();
        //        }
        //        else if (resxItem.Value.ToString().Equals(lblAge.Text, StringComparison.CurrentCulture))
        //        {
        //            employeeCaptionDto.Age = resxItem.Key.ToString();
        //        }
        //    }
        //    return employeeCaptionDto;
        //}        
        
        return null;
    }

    #endregion

    #region Grid Events & Methods

    #region Required Events

    /// <summary>
    /// GridView row data bound event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 05, 2007"></history>
    protected void grdAuditTrail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int index = (grdAuditTrail.PageIndex * grdAuditTrail.PageSize) + e.Row.RowIndex + 1;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onclick", "SetSelectedRow('" + index.ToString() + "'); setMouseOverColor(this);");
        }
    }

    /// <summary>
    /// GridView databound event to disable up and down buttons.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 07, 2007"></history>
    protected void grdAuditTrail_DataBound(object sender, EventArgs e)
    {
        GridView gridView = sender as GridView;
        GridViewRow topRow = null;
        GridViewRow lastRow = null;
        ImageButton button = null;

        try
        {
            topRow = gridView.Rows[0];
            lastRow = gridView.Rows[gridView.Rows.Count - 1];
            button = topRow.Cells[topRow.Cells.Count - 1].FindControl("btnMoveUp") as ImageButton;
            button.Enabled = false;
            button = lastRow.Cells[topRow.Cells.Count - 1].FindControl("btnMoveDown") as ImageButton;
            button.Enabled = false;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// GridView sorting event to disable up and down buttons.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 07, 2007"></history>
    protected void grdAuditTrail_Sorting(object sender, GridViewSortEventArgs e)
    {
        grdAuditTrail.DataBind();
    }

    /// <summary>
    /// GridView page index changed event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 07, 2007"></history>
    protected void grdAuditTrail_PageIndexChanged(object sender, EventArgs e)
    {
        grdAuditTrail.DataBind();
    }
    
    #endregion

    #region Audit Events

    /// <summary>
    /// Move Up button click event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 06, 2007"></history>
    protected void btnMoveUp_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow row = null;
        DataTable dTable = null;
        int index = 0;
        try
        {
            row = (sender as ImageButton).Parent.Parent as GridViewRow;
            index=row.RowIndex + (grdAuditTrail.PageIndex * grdAuditTrail.PageSize);
            dTable = MoveRows(index, MoveRow.MoveUp);
            GenericAudit.AuditMoveUp(index, userName, pageName);
            BindGrid(dTable);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Move Down button click event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 06, 2007"></history>
    protected void btnMoveDown_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow row = null;
        DataTable dTable = null;
        int index = 0;
        try
        {
            row = (sender as ImageButton).Parent.Parent as GridViewRow;
            index = row.RowIndex + (grdAuditTrail.PageIndex * grdAuditTrail.PageSize);
            dTable = MoveRows(index, MoveRow.MoveDown);
            GenericAudit.AuditMoveDown(index, userName, pageName); ;
            BindGrid(dTable);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Add row button click event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 06, 2007"></history>
    protected void btnAddRow_Click(object sender, EventArgs e)
    {
        DataTable dTable = null;
        try
        {
            dTable = AddRow();
            GenericAudit.AuditNew(dTable.Rows.Count, userName, pageName);
            BindGrid(dTable);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Insert row button click event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 06, 2007"></history>
    protected void btnInsertRow_Click(object sender, EventArgs e)
    {        
        DataTable dTable=null;
        string rowIndex = hiddenSelectRowId.Value;
        int index = 0;

        try
        {
            if (! String.IsNullOrEmpty(rowIndex))
            {
                index = Convert.ToInt32(rowIndex);
                dTable = InsertRow(index);
                GenericAudit.AuditInsert(index, userName, pageName);
                BindGrid(dTable);
            }            
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Copy row button click event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 06, 2007"></history>
    protected void btnCopyRow_Click(object sender, EventArgs e)
    {
        DataTable dTable=null;
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

    /// <summary>
    /// Paste row button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 06, 2007"></history>
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
                    BindGrid(dTable);
                }
                lblNameError.Text = "Please Select a Blank Row";                
            }
        }
        catch (Exception ex)
        {
            throw;
        }       
    }

    /// <summary>
    /// Delete row button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 06, 2007"></history>
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
                BindGrid(dTable);
            }
        }
        catch (Exception ex)
        {
            throw;
        }               
    }

    #endregion

    #region Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dTable"></param>
    private void BindGrid(DataTable dTable)
    {
        Session[GridTable] = dTable;
        grdAuditTrail.DataBind();
    }

    /// <summary>
    /// Method to save grid view data
    /// </summary>
    /// <returns>DataTable</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Dec 13, 2007"></history>
    private DataTable GetGridDataSource()
    {
        DataRow row = null;
        DataTable table = null;
        int count = 2;
        int index = 0;

        if (Session[GridTable] != null)
        {
            table = Session[GridTable] as DataTable;
            foreach (GridViewRow gridRow in grdAuditTrail.Rows)
            {
                count = 2;
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
                                count++;
                                break;
                            }
                        }
                    }
                }
            }
        }
        return table;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Employee FillGridDTO(Employee employeeDto)
    {
        employeeDto.Profile = GetGridDataSource();
        return employeeDto;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private EmployeeCaption FillHeaderTextDTO(EmployeeCaption employeeCaptionDto)
    {
        //String fileName = Request.AppRelativeCurrentExecutionFilePath.Substring(Request.AppRelativeCurrentExecutionFilePath.IndexOf("/") + 1);
        //String countryLanguageCode = Thread.CurrentThread.CurrentUICulture.Name;
        //String translateResourceFilePath = "App_LocalResources//" + fileName + "." + countryLanguageCode + ".resx";

        //if (!File.Exists(Server.MapPath(translateResourceFilePath)))
        //{
        //    translateResourceFilePath = "App_LocalResources//" + fileName + ".resx";
        //}

        //if (File.Exists(Server.MapPath(translateResourceFilePath)))
        //{
        //    ResXResourceReader resxReader = new ResXResourceReader(Server.MapPath(translateResourceFilePath));

        //    foreach (DictionaryEntry resxItem in resxReader)
        //    {
        //        if (resxItem.Value.ToString().Equals((grdAuditTrail.HeaderRow.Cells[2].Controls[0] as LinkButton).Text, StringComparison.CurrentCulture))
        //        {
        //            employeeCaptionDto.Profile.Add(resxItem.Key.ToString(), "YearOfPassing");
        //        }
        //        if (resxItem.Value.ToString().Equals((grdAuditTrail.HeaderRow.Cells[3].Controls[0] as LinkButton).Text, StringComparison.CurrentCulture))
        //        {
        //            employeeCaptionDto.Profile.Add(resxItem.Key.ToString(), "Institution");
        //        }
        //        if (resxItem.Value.ToString().Equals((grdAuditTrail.HeaderRow.Cells[4].Controls[0] as LinkButton).Text, StringComparison.CurrentCulture))
        //        {
        //            employeeCaptionDto.Profile.Add(resxItem.Key.ToString(), "Course");
        //        }
        //        if (resxItem.Value.ToString().Equals((grdAuditTrail.HeaderRow.Cells[5].Controls[0] as LinkButton).Text, StringComparison.CurrentCulture))
        //        {
        //            employeeCaptionDto.Profile.Add(resxItem.Key.ToString(), "Average");
        //        }
        //    }
        //    return employeeCaptionDto;
        //}

        return null;
    }
    
    #endregion

    #region Grid Update Methods

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
    private DataTable MoveRows(int index,MoveRow moveRow)
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
        return dTable;
    }
    
    #endregion

    #endregion
}
