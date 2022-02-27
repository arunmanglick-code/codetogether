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
using Arun.Manglick.BL;

public partial class Reflection_ObjectToDataTable : System.Web.UI.Page
{
    #region Private Variables

    private const string AuditActionsList = "AuditActionList";
    private const string Row = "Row";
    private const string ActionNumber = "ActionNumber"; // Denotes the name of one of the Column in AuditAction Table

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ReflectionGrid.DataSource = LogAuditTrailProfile();
            ReflectionGrid.DataBind();
        }
    }

    #endregion

    #region Private Functions
    
    /// <summary>
    /// Logs all the AuditActions in a DataTable
    /// </summary>
    /// <param name="auditActionList">List items for the AuditAction</param>
    /// <returns>DataTable</returns>
    /// <history created="Arun M"></history>
    /// <history date="Jan 09, 2007"></history>
    private DataTable LogAuditTrail()
    {
        DataTable dTable = GetEmptyAuditActionTable();
        List<AuditAction> auditActionList = AuditActionList();
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

    /// <summary>
    /// Method to get the empty data table for audit trail
    /// </summary>
    /// <returns>DataTable</returns>
    /// <history created="Arun M"></history>
    /// <history date="Jan 09, 2007"></history>
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
    
    #region Profile Demo

    /// <summary>
    /// Logs all the AuditActions in a DataTable
    /// </summary>
    /// <param name="auditActionList">List items for the AuditAction</param>
    /// <returns>DataTable</returns>
    /// <history created="Arun M"></history>
    /// <history date="Jan 09, 2007"></history>
    private DataTable LogAuditTrailProfile()
    {
        DataTable dTable = GetEmptyAuditActionTableProfile();
        List<AuditAction> auditActionList = AuditActionList();
        DataRow row = null;
        int count = 0;

        if (auditActionList.Count > 0)
        {
            foreach (AuditAction auditAction in auditActionList)
            {
                row = dTable.NewRow();

                row[0] = count++;
                row[1] = auditAction.DateTime;
                row[2] = auditAction.User;
                row[3] = auditAction.SourceLabel;
                row[4] = auditAction.Action;
                row[5] = auditAction.Position;
                row[6] = auditAction.Column;
                row[7] = auditAction.NewValue;
                row[8] = auditAction.OldValue;
                row[9] = auditAction.Page;
                dTable.Rows.Add(row);
            }
        }
        return dTable;
    }

    /// <summary>
    /// Method to get the empty data table for audit trail
    /// </summary>
    /// <returns>DataTable</returns>
    /// <history created="Arun M"></history>
    /// <history date="Jan 09, 2007"></history>
    private DataTable GetEmptyAuditActionTableProfile()
    {
        DataColumn column = null;
        DataTable dataTable = new DataTable("AuditTrailData");
        column = new DataColumn("ActionNumber", System.Type.GetType("System.Int32"));
        dataTable.Columns.Add(column);
        column = new DataColumn("DateTime", System.Type.GetType("System.DateTime"));
        dataTable.Columns.Add(column);
        column = new DataColumn("User", System.Type.GetType("System.String"));
        dataTable.Columns.Add(column);
        column = new DataColumn("SourceLabel", System.Type.GetType("System.String"));
        dataTable.Columns.Add(column);
        column = new DataColumn("Action", System.Type.GetType("System.String"));
        dataTable.Columns.Add(column);
        column = new DataColumn("Position", System.Type.GetType("System.Int32"));
        dataTable.Columns.Add(column);
        column = new DataColumn("Column", System.Type.GetType("System.String"));
        dataTable.Columns.Add(column);
        column = new DataColumn("NewValue", System.Type.GetType("System.String"));
        dataTable.Columns.Add(column);
        column = new DataColumn("OldValue", System.Type.GetType("System.String"));
        dataTable.Columns.Add(column);
        column = new DataColumn("Page", System.Type.GetType("System.String"));
        dataTable.Columns.Add(column);


        return dataTable;
    }

    #endregion
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private List<AuditAction> AuditActionList()
    {
        List<AuditAction> auditActionList = null;
        if (Session[AuditActionsList] == null)
        {
            auditActionList = new List<AuditAction>();
            auditActionList.Add(FormAuditAction("AM", "Label1", "Insert", 1, "FirstName", "Old", "New", "TrialPage"));
            auditActionList.Add(FormAuditAction("PM", "Label2", "Update", 2, "FirstName", "Old", "New", "TrialPage"));
            auditActionList.Add(FormAuditAction("SM", "Label3", "Delete", 3, "FirstName", "Old", "New", "TrialPage"));
            auditActionList.Add(FormAuditAction("RM", "Label4", "Copy", 4, "FirstName", "Old", "New", "TrialPage"));
            auditActionList.Add(FormAuditAction("TM", "Label5", "Paste", 5, "FirstName", "Old", "New", "TrialPage"));
            HttpContext.Current.Session[AuditActionsList] = auditActionList;
        }
        else
        {
            auditActionList = Session[AuditActionsList] as List<AuditAction>;
        }
        return auditActionList;
    }

    /// <summary>
    ///  Method to form a AuditAction object
    /// </summary>
    /// <param name="userName">string</param>
    /// <param name="sourceLabel">string</param>
    /// <param name="action">string</param>
    /// <param name="position">int</param>
    /// <param name="gridColumn">string</param>
    /// <param name="oldValue">string</param>
    /// <param name="newValue">string</param>
    /// <param name="pageName">string</param>
    /// <returns>Dto.AuditAction</returns>
    /// <history created="Arun M"></history>
    /// <history date="Jan 09, 2007"></history>
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

    #endregion
}
