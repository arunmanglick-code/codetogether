using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Resources;
using Arun.Manglick.BL;

public partial class Reflection_ObjectToDataTable : System.Web.UI.Page
{
    #region Private Variables

    private const string AuditActionsList = "AuditActionList";
    private const string Row = "Row";
    private const string ActionNumber = "ActionNumber"; // Denotes the name of one of the Column in AuditAction Table
    private const string Sequence = "Sequence";
    private const string DataKey = "CourseId";
    private const string AuditTrailPropertyName = "AuditTrailLog";
    private const string GridTable = "GridTable";
    public string userName = string.Empty;
    public string pageName = string.Empty;

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindPage();
        }
    }
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
            employeeDto = AuditPageNew(Session["OldDto"] as Employee, employeeDto, employeeCaptionDto, DataKey, Sequence, AuditTrailPropertyName, dirtyNonGrid, dirtyGrid, userName, pageName) as Employee;

            //Session["AuditTrailData"] = employeeDto.AuditTrailLog; // Temporary - To show the report immediately in a seperate Page.
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region Page Level Methods

    private void BindPage()
    {
        Employee employeeDto = GetEmployeeData();
        if (employeeDto != null)
        {
            txtEmployeeId.Text = employeeDto.Id.ToString();
            txtFirstName.Text = employeeDto.FirstName;
            txtLastName.Text = employeeDto.LastName;
            Session["OldDto"] = employeeDto;
        }
    }
    private static Employee GetEmployeeData()
    {
        Employee employee = null;

        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(HttpContext.Current.Server.MapPath("~\\XML\\AuditXML.xml"));

            employee = new Employee();
            employee.Id = 2;
            employee.FirstName = "John";
            employee.LastName = "Deer";
            employee.Profile = ds.Tables[0];
        }
        catch (Exception ex)
        {
            throw;
        }
        return employee;
    }

    private Employee FillNonGridDTO(Employee employeeDto)
    {
        if (!String.IsNullOrEmpty(txtEmployeeId.Text))
            employeeDto.Id = Convert.ToInt32(txtEmployeeId.Text);
        if (!String.IsNullOrEmpty(txtFirstName.Text))
            employeeDto.FirstName = txtFirstName.Text;
        if (!String.IsNullOrEmpty(txtLastName.Text))
            employeeDto.LastName = txtLastName.Text;

        return employeeDto;
    }
    private EmployeeCaption FillLabelCaptionDTO(EmployeeCaption employeeCaptionDto)
    {
        employeeCaptionDto.Id = "lblEmployeeId";
        employeeCaptionDto.FirstName = "lblFirstName";
        employeeCaptionDto.LastName = "lblLastName";

        return employeeCaptionDto;
    }

    private Employee FillGridDTO(Employee employeeDto)
    {
        employeeDto.Profile = GetGridDataSource();
        return employeeDto;
    }
    private EmployeeCaption FillHeaderTextDTO(EmployeeCaption employeeCaptionDto)
    {
        StringDictionary header = new StringDictionary();
        header.Add("YearOfPassing", "YearOfPassing");
        header.Add("Institution", "Institution");
        header.Add("Course", "Course");

        employeeCaptionDto.Profile = header;
        return employeeCaptionDto;
    }

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

    public Employee AuditPage(Employee oldDto, Employee newDto, EmployeeCaption captionDto, string dataKey, string sequenceKey, string auditTrailPropertyName, bool dirtyNonGrid, bool dirtyGrid, string userName, string pageName)
    {
        Type oldDtoType = oldDto.GetType();
        Type newDtoType = newDto.GetType();
        Type captionDtoType = captionDto.GetType();
        DataTable oldGridTable = null;
        DataTable newGridTable = null;
        DataRowCollection oldRowCollection = null;
        DataRow[] rowArray = null;
        DataRow newRow = null;
        AuditAction auditAction = null;
        StringDictionary gridHeader = null;
        string propName = string.Empty;
        string oldValue = string.Empty;
        string newValue = string.Empty;
        string captionValue = string.Empty;
        string columnName = string.Empty;
        string headerText = string.Empty;

        try
        {
            #region Audit NonGrid Part

            if (dirtyNonGrid)
            {
                foreach (PropertyInfo pInfo in oldDtoType.GetProperties())
                {
                    if (pInfo.PropertyType.IsValueType || pInfo.PropertyType.FullName.Equals("System.String"))
                    {
                        propName = pInfo.Name;
                        oldValue = pInfo.GetValue(oldDto, null).ToString();
                        newValue = newDtoType.GetProperty(propName).GetValue(newDto, null).ToString();
                        captionValue = captionDtoType.GetProperty(propName).GetValue(captionDto, null).ToString();

                        if (oldValue != newValue)
                        {
                            auditAction = FormAuditAction(userName, captionValue, "Change", 0, String.Empty, oldValue, newValue, pageName);
                            QueueAuditAction(auditAction);
                        }
                    }
                }
            }
            #endregion

            #region Audit Update & Insert in Grid

            if (dirtyGrid)
            {
                foreach (PropertyInfo pInfo in oldDtoType.GetProperties())
                {
                    if (!pInfo.Name.Equals(auditTrailPropertyName) && pInfo.PropertyType.FullName.Equals("System.Data.DataTable")) //TODO:
                    {
                        oldGridTable = pInfo.GetValue(oldDto, null) as DataTable;
                        newGridTable = newDtoType.GetProperty(pInfo.Name).GetValue(newDto, null) as DataTable;
                        DataTable dt = oldDto.GetType().InvokeMember(auditTrailPropertyName, BindingFlags.GetProperty, null, oldDto, null) as DataTable;
                        oldRowCollection = oldGridTable.Rows;

                        #region Audit Updates Grid
                        gridHeader = captionDtoType.GetProperty("Profile").GetValue(captionDto, null) as StringDictionary;
                        foreach (DataRow oldRow in oldRowCollection)
                        {
                            rowArray = (newGridTable.Select(dataKey + "='" + oldRow[dataKey].ToString() + "'")) as DataRow[]; //TODO:
                            if (rowArray.Length > 0)
                            {
                                newRow = rowArray[0];
                                if (newRow != null)
                                {
                                    foreach (DictionaryEntry dic in gridHeader) //TODO
                                    {
                                        headerText = dic.Key.ToString();
                                        columnName = dic.Value.ToString();
                                        if (newRow[columnName].ToString() != oldRow[columnName].ToString())
                                        {
                                            auditAction = FormAuditAction(userName, Row, "Update", Convert.ToInt32(newRow[sequenceKey]), headerText, oldRow[columnName].ToString(), newRow[columnName].ToString(), pageName);
                                            QueueAuditAction(auditAction);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Audit Inserts Grid
                        rowArray = (newGridTable.Select(dataKey + "= -1")) as DataRow[];//TODO:
                        for (int i = 0; i < rowArray.Length; i++)
                        {
                            newRow = rowArray[i];
                            foreach (DictionaryEntry dic in gridHeader)
                            {
                                headerText = dic.Key.ToString();
                                columnName = dic.Value.ToString();
                                auditAction = FormAuditAction(userName, Row, "NewUpdat", Convert.ToInt32(newRow[sequenceKey]), headerText, "-", newRow[columnName].ToString(), pageName);
                                QueueAuditAction(auditAction);

                            }
                        }
                        #endregion
                    }
                }
            }
            #endregion

            DataTable dTable = LogAuditTrail();
            newDtoType.GetProperty(auditTrailPropertyName).SetValue(newDto, dTable, null);
            ClearAuditActionQueue();
            return newDto;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public Employee AuditPageNew(Employee oldDto, Employee newDto, EmployeeCaption captionDto, string dataKey, string sequenceKey, string auditTrailPropertyName, bool dirtyNonGrid, bool dirtyGrid, string userName, string pageName)
    {
        DataRowCollection oldRowCollection = null;
        DataRow[] rowArray = null;
        DataRow newRow = null;
        AuditAction auditAction = null;
        string oldValue = string.Empty;
        string newValue = string.Empty;
        string captionValue = string.Empty;
        string columnName = string.Empty;
        string headerText = string.Empty;

        try
        {
            #region Audit NonGrid Part

            if (dirtyNonGrid)
            {
                if (!oldDto.FirstName.Equals(newDto.FirstName))
                {
                    auditAction = FormAuditAction(userName, captionDto.FirstName, "Change", 0, String.Empty, oldDto.FirstName, newDto.FirstName, pageName);
                    QueueAuditAction(auditAction);
                }

                if (!oldDto.LastName.Equals(newDto.LastName))
                {
                    auditAction = FormAuditAction(userName, captionDto.LastName, "Change", 0, String.Empty, oldDto.LastName, newDto.LastName, pageName);
                    QueueAuditAction(auditAction);
                }
            }
            #endregion

            #region Audit Update & Insert in Grid

            if (dirtyGrid)
            {               
                #region Audit Updates Grid

                oldRowCollection = oldDto.Profile.Rows;
                foreach (DataRow oldRow in oldRowCollection)
                {
                    rowArray = (newDto.Profile.Select("CourseId =" + oldRow["CourseId"].ToString())) as DataRow[];
                    if (rowArray.Length > 0)
                    {
                        newRow = rowArray[0];
                        if (newRow != null)
                        {
                            if (newRow["YearOfPassing"].ToString() != oldRow["YearOfPassing"].ToString())
                            {
                                headerText = "YearOfPassing";
                                columnName = "YearOfPassing";
                                auditAction = FormAuditAction(userName, Row, "Update", Convert.ToInt32(newRow[sequenceKey]), headerText, oldRow[columnName].ToString(), newRow[columnName].ToString(), pageName);
                                QueueAuditAction(auditAction);
                            }
                            if (newRow["Institution"].ToString() != oldRow["Institution"].ToString())
                            {
                                headerText = "Institution";
                                columnName = "Institution";
                                auditAction = FormAuditAction(userName, Row, "Update", Convert.ToInt32(newRow[sequenceKey]), headerText, oldRow[columnName].ToString(), newRow[columnName].ToString(), pageName);
                                QueueAuditAction(auditAction);
                            }
                            if (newRow["Course"].ToString() != oldRow["Course"].ToString())
                            {
                                headerText = "Course";
                                columnName = "Course";
                                auditAction = FormAuditAction(userName, Row, "Update", Convert.ToInt32(newRow[sequenceKey]), headerText, oldRow[columnName].ToString(), newRow[columnName].ToString(), pageName);
                                QueueAuditAction(auditAction);
                            }
                        }
                    }
                }
                #endregion

                #region Audit Inserts Grid
                rowArray = (newDto.Profile.Select("CourseId = -1")) as DataRow[];

                for (int i = 0; i < rowArray.Length; i++)
                {
                    newRow = rowArray[i];

                    auditAction = FormAuditAction(userName, Row, "NewUpdate", Convert.ToInt32(newRow[sequenceKey]), headerText, "-", newRow["YearOfPassing"].ToString(), pageName);
                    QueueAuditAction(auditAction);

                    auditAction = FormAuditAction(userName, Row, "NewUpdate", Convert.ToInt32(newRow[sequenceKey]), headerText, "-", newRow["Institution"].ToString(), pageName);
                    QueueAuditAction(auditAction);

                    auditAction = FormAuditAction(userName, Row, "NewUpdate", Convert.ToInt32(newRow[sequenceKey]), headerText, "-", newRow["Course"].ToString(), pageName);
                    QueueAuditAction(auditAction);
                }
                #endregion
            }
            #endregion

            DataTable dTable = LogAuditTrail();
            //newDto.AuditTrailLog = dTable;
            ClearAuditActionQueue();
            return newDto;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region Private Audit Functions

    private DataTable LogAuditTrail()
    {
        DataTable dTable = GetEmptyAuditActionTable();
        List<AuditAction> auditActionList = AuditActionList;
        DataRow row = null;
        int count = 0;

        if (auditActionList.Count > 0)
        {
            foreach (AuditAction auditAction in auditActionList)
            {
                row = dTable.NewRow();
                foreach (PropertyInfo pInfo in auditAction.GetType().GetProperties())
                {
                    if (pInfo.Name.Equals(ActionNumber))
                    {
                        row[count] = dTable.Rows.Count + 1;
                    }
                    else
                    {
                        row[count] = auditAction.GetType().InvokeMember(pInfo.Name, BindingFlags.GetProperty, null, auditAction, null).ToString();
                    }
                    count++;
                }

                dTable.Rows.Add(row);
                count = 0;
            }
        }
        return dTable;
    }
    private DataTable GetEmptyAuditActionTable()
    {
        AuditAction auditAction = new AuditAction();
        DataColumn column = null;
        DataTable dataTable = new DataTable();
        foreach (PropertyInfo pInfo in auditAction.GetType().GetProperties())
        {
            column = new DataColumn(pInfo.Name, pInfo.PropertyType);
            dataTable.Columns.Add(column);
        }
        return dataTable;
    }

    private void QueueAuditAction(AuditAction auditAction)
    {
        List<AuditAction> auditActionList = AuditActionList; //TODO
        auditActionList.Add(auditAction);
        AuditActionList = auditActionList;
    }
    private List<AuditAction> AuditActionList
    {
        get
        {
            List<AuditAction> auditActionList = null;
            if (Session[AuditActionsList] == null)
            {
                auditActionList = new List<AuditAction>();
                Session[AuditActionsList] = auditActionList;
            }
            else
            {
                auditActionList = Session[AuditActionsList] as List<AuditAction>;
            }
            return auditActionList;
        }
        set
        {
            HttpContext.Current.Session[AuditActionsList] = value;
        }
    }

    private AuditAction FormAuditAction(string userName, string sourceLabel, string action, int position, string gridColumn, string oldValue, string newValue, string pageName)
    {
        AuditAction auditAction = new AuditAction();
        auditAction.ActionNumber = 1;
        auditAction.DateTime = DateTime.Now;
        auditAction.User = userName;
        auditAction.SourceLabel = sourceLabel;
        auditAction.Action = action;
        auditAction.Position = position;
        auditAction.Column = gridColumn;
        auditAction.OldValue = oldValue;
        auditAction.NewValue = newValue;
        auditAction.Page = pageName;

        return auditAction;
    }
    private void ClearAuditActionQueue()
    {
        if (AuditActionList.Count > 0)
        {
            AuditActionList.Clear();
        }
    }

    #endregion
}
