#region File History

/******************************File History***************************
 * File Name        : vuiErrorReport.aspx.cs
 * Author           : Indrajeet K
 * Created Date     : 27-Sep-2007
 * Purpose          : View VUI Error Log
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 03-10-2007   IAK     Defect Solved: 1951, 1950, 1948, 1945, 1943, 1942, 1940, 1939 
 * 31-10-2007  Prerak   Added Institution nmae lable and access rights settings. 
 * 28-11-2007   IAK     Modified function setDatagridHeight()
 * 06-12-2007  Prerak   Modified for Inserting top row at institution and group combo 
 * 20-12-2007  IAK      Defect 2456 Add <info> filter
 * 12 Jun 2008 Prerak   Migration of AJAX Atlas to AJAX RTM 1.0
 * 13-01-2009   Rashmi  Defect # 10 - [CSTools:VUI Errors] The VUI Errors page is not dislaying any records for any condition.
 * ------------------------------------------------------------------- 
 */
#endregion File History


using System;
using System.IO;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Vocada.CSTools.Common;
using Vocada.CSTools.DataAccess;
using Vocada.VoiceLink.Utilities;

namespace Vocada.CSTools
{
    public partial class vuiErrorReport : System.Web.UI.Page
    {
        #region Class Variables
        public int divHeight = 0;
        public string strUserSettings = "NO";
        public string logFolder = "";
        public string sortOrder = "Desc";
        public string sortBy = "LogDateTime";
        DataGridItem lastItem = null;
        string lastTrace;
        #endregion Class Variables

        #region Constants
        public const string SORT_ORDER = "SortOrder";
        public const string SORT_BY = "SortBy";

        #endregion Constants

        #region Events

        /// <summary>
        /// Load initial Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
            {
                Response.Redirect(Utils.GetReturnURL("default.aspx", "vuiErrorReport.aspx", this.Page.ClientQueryString));
            }
            UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo; 
            try
            {
                registerJavascriptVariables();
                if (!Page.IsPostBack)
                {
                    Session[SessionConstants.WEEK_NUMBER] = null;
                    Session[SessionConstants.SHOWMESSAGES] = null;
                    Session[SessionConstants.STATUS] = null;
                    Session[SessionConstants.GROUP] = null;
                    //Set default sort order for log to Desc
                    ViewState[SORT_ORDER] = "Desc";
                    if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
                    {
                        hdnIsSystemAdmin.Value = "0";
                        cmbInstitution.Visible = false;
                        lblInstName.Visible = true;
                        lblInstName.Text = userInfo.InstitutionName; 
                        populateGroups(userInfo.InstitutionID);

                        //btnOCGrammar.Enabled = false;
                    }
                    else
                    {
                        hdnIsSystemAdmin.Value = "1";
                        cmbInstitution.Visible = true; ;
                        lblInstName.Visible = false;
                        populateInstitutions();
                        //if (Session[SessionConstants.INSTITUTION_ID] != null)
                        //{
                        //    cmbInstitution.SelectedValue = (Session[SessionConstants.INSTITUTION_ID].ToString());
                        //    populateGroups(Convert.ToInt32(Session[SessionConstants.INSTITUTION_ID]));
                        //}
                    }
                    populateErrorLog();
                }
                Session[SessionConstants.CURRENT_TAB] = "Tools";
                Session["CurrentInnerTab"] = "VUIError";

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("vuiErrorReport.Page_Load() ", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        /// <summary>
        /// Get Error log for selected Group, subscriber for given interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGO_Click(object sender, EventArgs e)
        {
            try
            {
                populateErrorLog();
                btnGO.Enabled = true;
            }
            catch (Exception ex)
            {
                

                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("vuiErrorReport.btnGO_Click() ", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                if (ex.GetType().FullName.Equals("System.IO.DirectoryNotFoundException"))
                {
                    ex = new ApplicationException("Log file directory does not exists. Please contact administrator.");
                }
                throw ex;
            }
        }

        /// <summary>
        /// Attach javascript to view error Stack trace. 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdErrorLog_ItemDataBound(object source, DataGridItemEventArgs e)
        {
            DataRowView data = null;
            string traceDesc = "";
            HyperLink link = null;
            try
            {
                
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    
                    //Get current data row
                    data = e.Item.DataItem as DataRowView;
                    traceDesc = Convert.ToString(data["TraceLogDescription"].ToString().Trim());
                    traceDesc = traceDesc.Replace("'", "");
                    data["ShortLogDescription"] = Convert.ToString(data["ShortLogDescription"].ToString().Trim()).Replace("'", "");

                    lastTrace = traceDesc.Trim();
                    lastItem = e.Item;

                    //Set Trace to image
                    link = e.Item.Cells[3].Controls[1] as HyperLink;
                    link.Attributes.Add("onmouseover", "viewTrace(this, '" + traceDesc.Trim() + "', 'false');");
                    link.Attributes.Add("onmouseout", "hideTrace();");

                    if (strUserSettings == "YES")
                    {
                        link.Text = "Details";
                    }
                    else
                    {
                        link.ImageUrl = "img/ic_add.gif";
                    }

                    if (traceDesc.Trim().Length == 0)
                    {
                        link.ImageUrl = "img/ic_add_gray.gif";
                    }
                    else
                    {
                        link.Style.Add("cursor", "pointer");
                    }
                }
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    if (lastItem != null)
                    {
                        //Set Trace to image
                        link = lastItem.Cells[3].Controls[1] as HyperLink;
                        link.Attributes.Add("onmouseover", "viewTrace(this, '" + lastTrace + "', 'true');");
                        link.Attributes.Add("onmouseout", "hideTrace();");
                    }
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("vuiErrorReport.grdErrorLog_ItemDataBound() ", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                data = null;
                link = null;
            }
        }

        /// <summary>
        /// Sort Error Log on Date
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdErrorLog_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            try
            {
                //Toggle Sort oder
                if (ViewState[SORT_ORDER].ToString().CompareTo("Desc") == 0)
                {
                    ViewState[SORT_ORDER] = "Asc";
                    sortOrder = "Asc";
                }
                else
                {
                    ViewState[SORT_ORDER] = "Desc";
                    sortOrder = "Desc";
                }
                sortBy = e.SortExpression;
                populateErrorLog();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("vuiErrorReport.grdErrorLog_SortCommand() ", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        /// <summary>
        /// Fill Group as per selected Institution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateGroups(int.Parse(cmbInstitution.SelectedValue));
                btnGO.Enabled = false;
                ScriptManager.RegisterClientScriptBlock(upnlVUI, upnlVUI.GetType(), "enableGo", "EnableVUIErrorGoButton();", true);
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("vuiErrorReport.cmbInstitution_SelectedIndexChanged() ", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        /// <summary>
        /// Fill Subscribers as per Group Selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {                
                populateSubscribers();
                btnGO.Enabled = false;
                ScriptManager.RegisterClientScriptBlock(upnlVUI, upnlVUI.GetType(), "enableGobtn", "EnableVUIErrorGoButton();", true);
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("vuiErrorReport.cmbGroup_SelectedIndexChanged() ", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }
        
        #endregion Events

        #region Private Methods

        /// <summary>
        /// Register JS variables, client side button click events
        /// </summary>
        private void registerJavascriptVariables()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var cmbInstitutionClientID = '" + cmbInstitution.ClientID + "';");
            sbScript.Append("var cmbSubscriberClientID = '" + cmbSubscriber.ClientID + "';");
            sbScript.Append("var cmbGroupClientID = '" + cmbGroup.ClientID + "';");
            sbScript.Append("var cmbSubscriberClientID = '" + cmbSubscriber.ClientID + "';");
            sbScript.Append("var txtStartDateClientID = '" + txtStartDate.ClientID + "';");
            sbScript.Append("var txtEndDateClientID = '" + txtEndDate.ClientID + "';");
            sbScript.Append("var anchFromDateClientID = '" + anchFromDate.ClientID + "';");
            sbScript.Append("var anchToDateClientID = '" + anchToDate.ClientID + "';");
            sbScript.Append("var btnGOClientID = '" + btnGO.ClientID + "';");
            sbScript.Append("var divTraceClientID = '" + divTrace.ClientID + "';");
            sbScript.Append("var lblTraceClientID = '" + lblTrace.ClientID + "';");
            sbScript.Append("var divLogGridClientID = '" + divLogGrid.ClientID + "';");
            sbScript.Append("var hdnIsSystemAdminClientID = '" + hdnIsSystemAdmin.ClientID + "';");

            sbScript.Append("document.getElementById(divLogGridClientID).style.border=0;");
            
            sbScript.Append("EnableVUIErrorGoButton();</script>");
            ClientScript.RegisterStartupScript(Page.GetType(),"scriptClientIDs",sbScript.ToString());

            //this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());

           
            cmbSubscriber.Attributes.Add("onChange", "EnableVUIErrorGoButton();");

            txtStartDate.Attributes.Add("onChange", "EnableVUIErrorGoButton();");
            txtStartDate.Attributes.Add("onKeyDown", "EnableVUIErrorGoButton();");
            txtStartDate.Attributes.Add("onKeyPress", "EnableVUIErrorGoButton();");
            txtStartDate.Attributes.Add("onKeyUp", "EnableVUIErrorGoButton();");
            
            txtEndDate.Attributes.Add("onChange", "EnableVUIErrorGoButton();");
            txtEndDate.Attributes.Add("onKeyDown", "EnableVUIErrorGoButton();");
            txtEndDate.Attributes.Add("onKeyPress", "EnableVUIErrorGoButton();");
            txtEndDate.Attributes.Add("onKeyUp", "EnableVUIErrorGoButton();");

            btnGO.Attributes.Add("onClick", "return Validate();");

            anchFromDate.Attributes.Add("onClick",
             String.Format(@"javascript:calVUILogDate.select(document.all['{0}'],document.all['{1}'],'MM/dd/yyyy')",
                                            txtStartDate.ClientID,
                                            anchFromDate.ClientID
                                            ));

            anchToDate.Attributes.Add("onClick",
            String.Format(@"javascript:calVUILogDate.select(document.all['{0}'],document.all['{1}'],'MM/dd/yyyy');return false;",
                                           txtEndDate.ClientID,
                                           anchToDate.ClientID
                                           ));
        }

        /// <summary>
        /// Fill Institution combo box
        /// </summary>
        private void populateInstitutions()
        {
            DataTable dtInstitutions = null;            
            try
            {
                //Get list of institutions
                dtInstitutions = Utility.GetInstitutionList();

                ////Add additional row at top
                //drInstitution = dtInstitutions.NewRow();
                //drInstitution[1] = "-- Select Institution --"; 
                //drInstitution[0] = 0;
                //dtInstitutions.Rows.InsertAt(drInstitution, 0);

                //Bind the datasource to combo
                cmbInstitution.Items.Clear();
                cmbInstitution.DataSource = dtInstitutions;
                cmbInstitution.DataTextField = "InstitutionName";
                cmbInstitution.DataValueField = "InstitutionID";
                cmbInstitution.DataBind();
                cmbInstitution.Items.Insert(0, new ListItem("-- Select Institution --", "0"));

                cmbInstitution.SelectedValue = "0";

                //Popualte Groups with selected Institution
                populateGroups(int.Parse(cmbInstitution.SelectedValue));
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("vuiErrorReport.populateInstitutions() ", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                dtInstitutions = null;                
            }
        }

        /// <summary>
        /// Fill Group combo box for selected Institution
        /// </summary>
        private void populateGroups(int institutionID)
        {
            DataTable dtGroups = null;            
            try
            {
                //Get list of institutions
                //int institutionID = int.Parse(cmbInstitution.SelectedValue);
               
                dtGroups = Utility.GetGroups(institutionID);                

                //Bind the datasource to combo
                cmbGroup.Items.Clear();
                cmbGroup.DataSource = dtGroups;
                cmbGroup.DataTextField = "GroupName";
                cmbGroup.DataValueField = "GroupID";
                cmbGroup.DataBind();
                cmbGroup.Items.Insert(0, new ListItem("-- Select Group --", "0"));                
                cmbGroup.SelectedValue = "0";

                populateSubscribers();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("vuiErrorReport.populateGroups() ", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                dtGroups = null;                
            }
        }

        /// <summary>
        /// Fill Subscriber combo box with selected Group
        /// </summary>
        private void populateSubscribers()
        {
            DataTable dtSubscribers = null;            
            Group group = null;
            int groupID;
            try
            {
                group = new Group();

                //Get list of institutions
                groupID = int.Parse(cmbGroup.SelectedValue);
                if (groupID == 0)
                    groupID = -1;
                int activeOnly = 0;
                dtSubscribers = group.GetGroupUsers(groupID, activeOnly);

                //Bind the datasource to combo
                DataView dvSubscribers = dtSubscribers.DefaultView;
                dvSubscribers.Sort = "DisplayName ASC";
                cmbSubscriber.Items.Clear();
                cmbSubscriber.DataSource = dvSubscribers;
                cmbSubscriber.DataTextField = "DisplayName";
                cmbSubscriber.DataValueField = "SubscriberID";
                cmbSubscriber.DataBind();
                cmbSubscriber.Items.Insert(0, new ListItem("-- Select Subscriber --", "0"));
                if (dtSubscribers.Rows.Count > 0)
                    cmbSubscriber.Items.Insert(1, new ListItem("All Subscriber", "-1"));
                cmbSubscriber.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("vuiErrorReport.populateSubscribers() ", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                dtSubscribers = null;                
                group = null;
            }
        }

        /// <summary>
        /// Populate Error log in grid for selection
        /// </summary>
        private void populateErrorLog()
        {

            DataTable dtLog = null;
            DataView dvLogView = null;
            try
            {
                dtLog = new DataTable();   //Create Log Table
                dtLog.Columns.Add(new DataColumn("SubscriberName", System.Type.GetType("System.String")));
                dtLog.Columns.Add(new DataColumn("LogDateTime", System.Type.GetType("System.DateTime")));
                dtLog.Columns.Add(new DataColumn("ShortLogDescription", System.Type.GetType("System.String")));
                dtLog.Columns.Add(new DataColumn("TraceLogDescription", System.Type.GetType("System.String")));

                if (IsPostBack)
                {
                    GetLogData(dtLog);         //Get logs in to datatable
                }

                if (dtLog.Rows.Count > 1)
                    grdErrorLog.AllowSorting = true;
                else
                    grdErrorLog.AllowSorting = false;

                //Sort it
                dvLogView = dtLog.DefaultView;
                dvLogView.Sort = sortBy + " " + sortOrder;
                grdErrorLog.DataSource = dvLogView;
                //If no record remove sort expression of datetime
                if (dtLog.Rows.Count == 0)
                {
                    grdErrorLog.Columns[1].SortExpression = "";
                    grdErrorLog.Columns[0].SortExpression = "";
                }
                else
                {
                    grdErrorLog.Columns[1].SortExpression = "LogDateTime";
                    grdErrorLog.Columns[0].SortExpression = "SubscriberName";
                }
                grdErrorLog.DataBind();
                if (Page.IsPostBack)
                {
                    if (dtLog.Rows.Count > 0)
                    {
                        lblNoRecordFound.Text = "";
                    }
                    else
                    {
                        lblNoRecordFound.Text = "No Records Available";
                    }
                }
                setDatagridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("vuiErrorReport.populateErrorLog() ", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                dtLog = null;
                dvLogView = null;
            }
        }

        /// <summary>
        /// Get Log Data in table
        /// </summary>
        /// <returns></returns>
        private void GetLogData(DataTable dtLogTable)
        {            
            DateTime startDateTime = DateTime.Now;
            DateTime endDateTime = DateTime.Now;
            DateTime dirCreateDate;
            DirectoryInfo dirInfo;
            Group group = null;             
            DataTable dtGroupInfo = null;
            string subscriberID = string.Empty;
            string groupName = string.Empty;
            string instId = string.Empty;
            string[] arrDir;
            string[] arrInnerDir;                        
            string dateDirName;
            string grpName_InstId = string.Empty;
            
            try
            {
                subscriberID = cmbSubscriber.SelectedValue.Trim() == "-1" ? "" : cmbSubscriber.SelectedValue.Trim();
                startDateTime = Convert.ToDateTime(txtStartDate.Text);
                endDateTime = Convert.ToDateTime(txtEndDate.Text);
                instId = cmbInstitution.SelectedValue;               

                //Get Group info to know the group Type( Lab/Rad)
                group = new Group();
                dtGroupInfo = group.GetGroupInformationByGroupID(Convert.ToInt32(cmbGroup.SelectedValue));
                
                //Get Log Folder Path.
                if (dtGroupInfo.Rows.Count > 0)
                {
                    groupName = dtGroupInfo.Rows[0]["GroupName"].ToString();
                    grpName_InstId = groupName + "_" + instId;
                    if (dtGroupInfo.Rows[0]["GroupType"].ToString() == "True")
                    {
                        logFolder = ConfigurationManager.AppSettings["VUILogLabFolder"];
                    }
                    else
                    {
                        logFolder = ConfigurationManager.AppSettings["VUILogRadFolder"];
                    }
                }
                       
                //Get Top Child directories. These are date-wise folders.
                arrDir = System.IO.Directory.GetDirectories(logFolder);                                                
                
                //Loop thru date-wise folders
                for (int cntr = 0; cntr < arrDir.Length; cntr++)  // Date folders
                {
                    dirInfo = new DirectoryInfo(arrDir[cntr]);
                    dateDirName = dirInfo.Name;
                    dirCreateDate = Convert.ToDateTime(dateDirName);

                    //If the folder satisfies the date criteria, then scan it further.
                    if (dirCreateDate.CompareTo(startDateTime) >= 0 && dirCreateDate.CompareTo(endDateTime) <= 0)
                    {
                        // look for the group and instt. selected in search criteria.                  
                        arrInnerDir = System.IO.Directory.GetDirectories(arrDir[cntr], grpName_InstId, SearchOption.TopDirectoryOnly);

                        for (int cnt = 0; cnt < arrInnerDir.Length; cnt++)        //
                        {
                            dirInfo = new DirectoryInfo(arrInnerDir[cnt]);

                            //If the directory belongs to the group and inst selected. Get all the logs.
                            if (dirInfo.Name.CompareTo(grpName_InstId) == 0)
                            {
                                //get all the files in this folder for the selected subscriber if any.                                
                                readLogFiles(dirInfo.FullName, dtLogTable, dateDirName, SearchOption.AllDirectories,subscriberID );
                            }
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("vuiErrorReport.GetLogData() ", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {                               
                dtLogTable = null;                
                group = null;
                dtGroupInfo = null;                
            }
        }

        /// <summary>
        /// Get all the files in this folder & subfolders for the selected subscriber if any.
        /// these are the files for inbound, ob calls & default log.
        /// Reads all log files in top directory & sub directory, and updates log datatable.
        /// </summary>
        /// <param name="dirPath">Path of the top directory</param>
        /// <param name="dtLog">DataTable</param>
        /// <param name="folderName">Name of the Top directory to scan files in.</param>
        /// <param name="searchOption">searchOption to search in Top directory or subdirectries.</param>
        /// <param name="subscriberId">subscriberId</param>
        private void readLogFiles(string dirPath, DataTable dtLog, string folderName, SearchOption searchOption, string subscriberId)
        {
            DataRow drLog = null;
            FileStream fileStream;
            StreamReader streamReader;            
            ListItem lstSubscriber = null;
            string dtlogDateTime;
            string currentLine = string.Empty;
            string[] arrFiles;            
            string trace = string.Empty;
            string fileSubscriberName = string.Empty;
            string fileSubscriberId = string.Empty;            

            try
            {
                arrFiles = System.IO.Directory.GetFiles(dirPath, "*" + subscriberId + ".log", searchOption);

                for (int k = 0; k < arrFiles.Length; k++)       //read all files.
                {                    
                    fileSubscriberId = "";
                    fileSubscriberName = "";
                                        
                    if (!arrFiles[k].Contains("_default"))
                    {
                        fileSubscriberId = arrFiles[k].Substring(arrFiles[k].LastIndexOf("_")+1);   //subscriberId from file name.
                        fileSubscriberId = fileSubscriberId.Remove(fileSubscriberId.LastIndexOf("."));
                        lstSubscriber = cmbSubscriber.Items.FindByValue(fileSubscriberId);          // subscriber name from dropdown list.
                        if (lstSubscriber != null)
                        {
                            fileSubscriberName = lstSubscriber.Text.Trim();
                        }
                    }
                    //get the info log entries from the log and populate data table.                
                    fileStream = new FileStream(arrFiles[k], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                    // Create an instance of streamReader that can read characters from the FileStream.
                    streamReader = new StreamReader(fileStream);

                    // While not at the end of the file, read lines from the file.
                    while (streamReader.Peek() > -1)
                    {
                        currentLine = streamReader.ReadLine();

                        if (currentLine.Contains("<E> --"))
                        {
                            if (drLog != null)
                            {
                                dtLog.Rows.Add(drLog);
                                drLog = null;
                            }
                            currentLine = currentLine.Remove(0, 7);
                            dtlogDateTime = currentLine.Substring(0, currentLine.IndexOf("--"));
                            currentLine = currentLine.Remove(0, currentLine.IndexOf("--") + 2);
                            drLog = dtLog.NewRow();             //add to datatable;
                            drLog["LogDateTime"] = folderName + " " + dtlogDateTime;
                            drLog["SubscriberName"] = fileSubscriberName;
                            drLog["ShortLogDescription"] = Server.HtmlEncode(currentLine);
                            trace = "";
                        }
                        else if (!currentLine.Contains("<I> --")) //Trace information.
                        {
                            trace = trace + Server.HtmlEncode(currentLine);
                            drLog["TraceLogDescription"] = trace;
                        }
                    }
                    if (drLog != null)
                    {
                        dtLog.Rows.Add(drLog);
                        drLog = null;
                    }
                    streamReader.Close();
                    fileStream.Close();
                }
            }
            finally
            {
                streamReader = null;
                fileStream = null;
                drLog = null;
                arrFiles = null;
            }
        }

        /// <summary>
        /// This method will set the height of datagrid dynamically accordingly the current rowcount of datagrid,
        /// each time when the page posts back. 
        /// </summary>
        private void setDatagridHeight()
        {
            string script = "<script type=\"text/javascript\">";
            script += "if(document.getElementById(" + '"' + divLogGrid.ClientID + '"' + ") != null){document.getElementById(" + '"' + divLogGrid.ClientID + '"' + ").style.height=setHeightOfGrid('" + grdErrorLog.ClientID + "','" + 50 + "');}</script>";
            ScriptManager.RegisterStartupScript(upnlVUI, upnlVUI.GetType(), "setHeight", script, false);
        }

        #endregion Private Methods

    }
}