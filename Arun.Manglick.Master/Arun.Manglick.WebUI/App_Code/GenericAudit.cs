using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Resources;
using System.Globalization;
using System.Reflection;

namespace Arun.Manglick.UI
{
    /// <summary>
    /// GenericAudit Class
    /// </summary>
    public sealed class GenericAudit
    {
        #region Private Variables

        private const string AuditActionsList = "AuditActionList";
        private const string Row = "Row";
        private const string ActionNumber = "ActionNumber"; // Denotes the name of one of the Column in AuditAction Table
        private const string NameMatchCriteriaKey = "NameMatchCriteriaKey";
        private const string FullNameMatchCriteriaKey = "FullNameMatchCriteriaKey";

        #endregion

        #region Constructor

        /// <summary>
        /// FxCop guideline - StaticHolderTypesShouldNotHaveConstructors
        /// </summary>
        /// <history created=”Arun M”></history>
        /// <history date=”Jan 10, 2007”></history>
        private GenericAudit()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        #region Private Static Methods

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
        private static DataTable MoveSequence(int index, string sequenceKey, DataTable employeeTable, MoveRow direction)
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
        private static DataTable RearrangeSequence(string sequenceKey, DataTable employeeTable)
        {
            int sequence = 1;
            foreach (DataRow row in employeeTable.Rows)
            {
                row[sequenceKey] = sequence++;
            }
            return employeeTable;
        }

        #endregion

        #region AuditTrail Methods

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
        /// <returns>AuditAction</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        private static AuditAction FormAuditAction(string userName, string sourceLabel, string action, int position, string gridColumn, string oldValue, string newValue, string pageName)
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

        /// <summary>
        /// Queues the AuditActions
        /// </summary>
        /// <param name="auditAction">AuditAction</param>
        /// <returns>void</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        private static void QueueAuditAction(AuditAction auditAction)
        {
            List<AuditAction> auditActionList = AuditActionList; //TODO
            auditActionList.Add(auditAction);
            AuditActionList = auditActionList;
        }

        /// <summary>
        /// Queues the AuditActions
        /// </summary>
        /// <returns>void</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 14, 2007"></history>
        private static void ClearAuditActionQueue()
        {
            if (AuditActionList.Count > 0)
            {
                AuditActionList.Clear();
            }
        }

        /// <summary>
        /// Logs all the AuditActions in a DataTable
        /// </summary>
        /// <param name="auditActionList">List items for the AuditAction</param>
        /// <returns>DataTable</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        private static DataTable LogAuditTrail(List<AuditAction> auditActionList)
        {
            DataTable dTable = GetEmptyAuditActionTable();
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
                            row[count] = auditAction.GetType().InvokeMember(pInfo.Name, BindingFlags.GetProperty, null, auditAction, null,CultureInfo.InvariantCulture).ToString();
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
        private static DataTable GetEmptyAuditActionTable()
        {
            AuditAction auditAction = new AuditAction();
            DataColumn column = null;
            DataTable dataTable = new DataTable();
            dataTable.Locale = CultureInfo.InvariantCulture;
            foreach (PropertyInfo pInfo in auditAction.GetType().GetProperties())
            {
                column = new DataColumn(pInfo.Name, pInfo.PropertyType);
                dataTable.Columns.Add(column);
            }
            return dataTable;
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// AuditActionList Property, keeps track of all the AuditActions in Session
        /// </summary>
        private static List<AuditAction> AuditActionList
        {
            get
            {
                List<AuditAction> auditActionList = null;
                if (HttpContext.Current.Session[AuditActionsList] == null)
                {
                    auditActionList = new List<AuditAction>();
                    HttpContext.Current.Session[AuditActionsList] = auditActionList;
                }
                else
                {
                    auditActionList = HttpContext.Current.Session[AuditActionsList] as List<AuditAction>;
                }
                return auditActionList;
            }
            set
            {
                HttpContext.Current.Session[AuditActionsList] = value;
            }
        }

        #endregion

        #endregion

        #region Public Static Methods

        #region Full Version Methods

        /// <summary>
        ///  Audit MoveUp
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="gridTable">DataTable</param>
        /// <param name="sequenceKey">String: ColumnName of the Sequence field</param>
        /// <param name="userName">string</param>
        /// <param name="pageName">string</param>
        /// <returns>DataTable</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        public static DataTable AuditMoveUp(int index, DataTable gridTable, string sequenceKey, string userName, string pageName)
        {
            DataTable dtable = gridTable;
            int sequence = index + 1;
            dtable = MoveSequence(index, sequenceKey, dtable, MoveRow.MoveUp);
            AuditAction auditAction = FormAuditAction(userName, Row, Resources.AuditActions.Up, sequence - 1, "-", sequence.ToString(), (sequence - 1).ToString(), pageName); ;
            QueueAuditAction(auditAction);
            return dtable;
        }

        /// <summary>
        ///  Audit MoveDown
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="gridTable">DataTable</param>
        /// <param name="sequenceKey">String: ColumnName of the Sequence field</param>
        /// <param name="userName">string</param>
        /// <param name="pageName">string</param>
        /// <returns>DataTable</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        public static DataTable AuditMoveDown(int index, DataTable gridTable, string sequenceKey, string userName, string pageName)
        {
            DataTable dtable = gridTable;
            int sequence = index + 1;
            dtable = MoveSequence(index, sequenceKey, dtable, MoveRow.MoveDown);
            AuditAction auditAction = FormAuditAction(userName, Row, Resources.AuditActions.Down, sequence + 1, "-", sequence.ToString(), (sequence + 1).ToString(), pageName);
            QueueAuditAction(auditAction);
            return dtable;
        }

        /// <summary>
        ///  Audit New
        /// </summary>
        /// <param name="gridTable">DataTable</param>
        /// <param name="dataKey">String: ColumnName of the Key field</param>
        /// <param name="sequenceKey">String: ColumnName of the Sequence field</param>
        /// <param name="userName">string</param>
        /// <param name="pageName">string</param>
        /// <returns>DataTable</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        public static DataTable AuditNew(DataTable gridTable, string dataKey, string sequenceKey, string userName, string pageName)
        {
            DataTable dtable = gridTable;
            DataRow row = dtable.NewRow();
            row[dataKey] = -1;
            dtable.Rows.Add(row);
            AuditAction auditAction = FormAuditAction(userName, Row, Resources.AuditActions.New, dtable.Rows.Count, "-", "-", "-", pageName);
            QueueAuditAction(auditAction);
            dtable = RearrangeSequence(sequenceKey, dtable);
            return dtable;
        }

        /// <summary>
        ///  Audit Insert
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="gridTable">DataTable</param>
        /// <param name="dataKey">String: ColumnName of the Key field</param>
        /// <param name="sequenceKey">String: ColumnName of the Sequence field</param>
        /// <param name="userName">string</param>
        /// <param name="pageName">string</param>
        /// <returns>DataTable</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        public static DataTable AuditInsert(int index, DataTable gridTable, string dataKey, string sequenceKey, string userName, string pageName)
        {
            DataTable dtable = gridTable;
            DataRow row = dtable.NewRow();
            int sequence = index + 1;
            row[dataKey] = -1;
            dtable.Rows.InsertAt(row, index);
            AuditAction auditAction = FormAuditAction(userName, Row, Resources.AuditActions.New, sequence, "-", "-", "-", pageName);
            QueueAuditAction(auditAction);
            dtable = RearrangeSequence(sequenceKey, dtable);
            return dtable;
        }

        /// <summary>
        /// Audit Paste
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="gridTable">DataTable</param>
        /// <param name="copiedRow">DataRow</param>
        /// <param name="dataKey">String: ColumnName of the Key field</param>
        /// <param name="sequenceKey">String: ColumnName of the sequecne field</param>
        /// <param name="userName">string</param>
        /// <param name="pageName">string</param>
        /// <returns>DataTable</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        public static DataTable AuditPaste(int index, DataTable gridTable, DataRow copiedRow, string dataKey, string sequenceKey, string userName, string pageName)
        {
            DataTable dtable = gridTable;
            dtable.Rows[index].ItemArray = copiedRow.ItemArray;
            int sequence = index + 1;
            dtable.Rows[index][dataKey] = -1;
            AuditAction auditAction = FormAuditAction(userName, Row, Resources.AuditActions.New, sequence, "-", "-", "-", pageName);
            QueueAuditAction(auditAction);
            dtable = RearrangeSequence(sequenceKey, dtable);
            return dtable;
        }

        /// <summary>
        /// Audit Delete
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="gridTable">DataTable</param>
        /// <param name="sequenceKey">String: ColumnName of the sequecne field</param>
        /// <param name="userName">string</param>
        /// <param name="pageName">string</param>
        /// <returns>DataTable</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        public static DataTable AuditDelete(int index, DataTable gridTable, string sequenceKey, string userName, string pageName)
        {
            DataTable dtable = gridTable;
            dtable.Rows.Remove(dtable.Rows[index]);
            int sequence = index + 1;
            AuditAction auditAction = FormAuditAction(userName, Row, Resources.AuditActions.Delete, sequence, "-", "-", "-", pageName);
            QueueAuditAction(auditAction);
            dtable = RearrangeSequence(sequenceKey, dtable);
            return dtable;
        }
        
        #endregion

        #region Action-Only Version Methods

        /// <summary>
        ///  Audit MoveUp
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="userName">string</param>
        /// <param name="pageName">string</param>
        /// <returns>void</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        public static void AuditMoveUp(int index, string userName, string pageName)
        {
            int sequence = index + 1;
            AuditAction auditAction = FormAuditAction(userName, Row, Resources.AuditActions.Up, sequence - 1, "-", sequence.ToString(), (sequence - 1).ToString(), pageName); ;
            QueueAuditAction(auditAction);
        }
        
        /// <summary>
        ///  Audit MoveDown
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="userName">string</param>
        /// <param name="pageName">string</param>
        /// <returns>void</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        public static void AuditMoveDown(int index, string userName, string pageName)
        {
            int sequence = index + 1;
            AuditAction auditAction = FormAuditAction(userName, Row, Resources.AuditActions.Down, sequence + 1, "-", sequence.ToString(), (sequence + 1).ToString(), pageName);
            QueueAuditAction(auditAction);
        }
        
        /// <summary>
        ///  Audit New
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="userName">string</param>
        /// <param name="pageName">string</param>
        /// <returns>void</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        public static void AuditNew(int index, string userName, string pageName)
        {
            int sequence = index;
            AuditAction auditAction = FormAuditAction(userName, Row, Resources.AuditActions.New, sequence, "-", "-", "-", pageName);
            QueueAuditAction(auditAction);
        }
        
        /// <summary>
        ///  Audit Insert
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="userName">string</param>
        /// <param name="pageName">string</param>
        /// <returns>void</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        public static void AuditInsert(int index, string userName, string pageName)
        {
            int sequence = index + 1;
            AuditAction auditAction = FormAuditAction(userName, Row, Resources.AuditActions.New, sequence, "-", "-", "-", pageName);
            QueueAuditAction(auditAction);
        }
        
        /// <summary>
        /// Audit Paste
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="userName">string</param>
        /// <param name="pageName">string</param>
        /// <returns>void</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        public static void AuditPaste(int index, string userName, string pageName)
        {
            int sequence = index + 1;
            AuditAction auditAction = FormAuditAction(userName, Row, Resources.AuditActions.New, sequence, "-", "-", "-", pageName);
            QueueAuditAction(auditAction);
        }

        /// <summary>
        /// Audit Delete
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="userName">string</param>
        /// <param name="pageName">string</param>void
        /// <returns>void</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        public static void AuditDelete(int index, string userName, string pageName)
        {
            int sequence = index + 1;
            AuditAction auditAction = FormAuditAction(userName, Row, Resources.AuditActions.Delete, sequence, "-", "-", "-", pageName);
            QueueAuditAction(auditAction);
        }

        #endregion
        
        #region Audit Page Methods

        /// <summary>
        /// Audits the complete page having no GridView Control
        /// </summary>
        /// <param name="oldDto">Page Level DTO</param>
        /// <param name="newDto">Page Level DTO</param>
        /// <param name="captionDto">Page Level Caption DTO</param>
        /// <param name="action">String: Actual Action</param>
        /// <param name="userName">string</param>
        /// <param name="pageName">string</param>
        /// <returns>IDto</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        public static IDto AuditPage(IDto oldDto, IDto newDto, ICaptionDto captionDto, string auditTrailPropertyName, string action, string userName, string pageName)
        {
            Type oldDtoType = oldDto.GetType();
            Type newDtoType = newDto.GetType();
            Type captionDtoType = captionDto.GetType();
            AuditAction auditAction = null;
            string propName = string.Empty;
            string oldValue = string.Empty;
            string newValue = string.Empty;
            string captionValue = string.Empty;
            string columnName = string.Empty;
            string headerText = string.Empty;

            try
            {
                #region Audit NonGrid Part
                
                foreach (PropertyInfo pInfo in oldDtoType.GetProperties())
                {
                    if (pInfo.PropertyType.IsValueType || pInfo.PropertyType.FullName.Equals("System.String"))
                    {
                        propName = pInfo.Name;
                        if (propName != NameMatchCriteriaKey && propName != FullNameMatchCriteriaKey)
                        {
                            oldValue = pInfo.GetValue(oldDto, null).ToString();
                            newValue = newDtoType.GetProperty(propName).GetValue(newDto, null).ToString();
                            captionValue = captionDtoType.GetProperty(propName).GetValue(captionDto, null).ToString();

                            if (oldValue != newValue)
                            {
                                auditAction = FormAuditAction(userName, captionValue, action, 0, String.Empty, oldValue, newValue, pageName);
                                QueueAuditAction(auditAction);
                            }
                        }
                    }
                }

                #endregion

                DataTable dTable = LogAuditTrail(AuditActionList);
                newDtoType.GetProperty(auditTrailPropertyName).SetValue(newDto, dTable, null);
                ClearAuditActionQueue();
                return newDto;
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Audits the complete page having Grid & Non Grid part
        /// </summary>
        /// <param name="oldDto">Page Level DTO</param>
        /// <param name="newDto">Page Level DTO</param>
        /// <param name="captionDto">Page Level Caption DTO</param>
        /// <param name="dataKey">String: ColumnName of the DataKey field</param>
        /// <param name="sequenceKey">String: ColumnName of the sequecne field</param>
        /// <param name="auditTrailPropertyName">String: Proeperty of the AuditTrail field in DTO</param>
        /// <param name="dirtyNonGrid">bool</param>
        /// <param name="dirtyGrid">bool</param>
        /// <param name="userName">string</param>
        /// <param name="pageName">string</param>
        /// <returns>IDto</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 09, 2007"></history>
        public static IDto AuditPage(IDto oldDto, IDto newDto, ICaptionDto captionDto, string dataKey, string sequenceKey, string auditTrailPropertyName, bool dirtyNonGrid, bool dirtyGrid, string userName, string pageName)
        {
            Type oldDtoType = oldDto.GetType();
            Type newDtoType = newDto.GetType();
            Type captionDtoType = captionDto.GetType();
            DataTable oldGridTable = null;
            DataTable newGridTable = null;
            DataRowCollection oldRowCollection = null;
            DataRowCollection newRowCollection = null;
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
                            if (propName != NameMatchCriteriaKey && propName != FullNameMatchCriteriaKey)
                            {
                                oldValue = pInfo.GetValue(oldDto, null).ToString();
                                newValue = newDtoType.GetProperty(propName).GetValue(newDto, null).ToString();
                                captionValue = captionDtoType.GetProperty(propName).GetValue(captionDto, null).ToString();

                                if (oldValue != newValue)
                                {
                                    auditAction = FormAuditAction(userName, captionValue, Resources.AuditActions.Change, 0, String.Empty, oldValue, newValue, pageName);
                                    QueueAuditAction(auditAction);
                                }
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
                                                auditAction = FormAuditAction(userName, Row, Resources.AuditActions.Update, Convert.ToInt32(newRow[sequenceKey]), headerText, oldRow[columnName].ToString(), newRow[columnName].ToString(), pageName);
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
                                    auditAction = FormAuditAction(userName, Row, Resources.AuditActions.NewUpdate, Convert.ToInt32(newRow[sequenceKey]), headerText, "-", newRow[columnName].ToString(), pageName);
                                    QueueAuditAction(auditAction);

                                }
                            }
                            #endregion
                        }
                    }
                }
                #endregion

                DataTable dTable = LogAuditTrail(AuditActionList);
                newDtoType.GetProperty(auditTrailPropertyName).SetValue(newDto, dTable, null);
                ClearAuditActionQueue();
                return newDto;
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Audits all the Actions.
        /// Required in case the Actions behaves like a Page Save. 
        /// For e.g. 'Delete' Action on Search Page
        /// </summary>
        /// <param name="newDto">IDto</param>
        /// <param name="auditTrailPropertyName">string</param>
        /// <returns>IDto</returns>
        /// <history created="Arun M"></history>
        /// <history date="Jan 14, 2007"></history>
        public static IDto AuditPage(IDto newDto, string auditTrailPropertyName)
        {
            DataTable dTable = LogAuditTrail(AuditActionList);
            Type newDtoType = newDto.GetType();
            newDtoType.GetProperty(auditTrailPropertyName).SetValue(newDto, dTable, null);
            ClearAuditActionQueue();
            return newDto;
        }

        #endregion

        #endregion
    }
}
