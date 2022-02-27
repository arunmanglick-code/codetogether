#region File History

/******************************File History***************************
 * File Name        : message_list.aspx.cs
 * Author           : Prerak Shah
 * Created Date     : 24-Aug-2007
 * Purpose          : Display the open / closed messages for Particular user / Group.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 28-11-2007   IAK     Modified function setDataGridHeight()
 * 18-12-2007   IAK     implemented CR- Show'Embargoed' in msg status
 * 18-12-2007   IAK     Removed postback on group and message status
 * 28-12-2007   Prerak - Modified for Advance Search
 * 07-01-2008   Prerak - Modified for clear search link visibility, Added message loasing icon
 * 16-01-2008   Prerak - Defect# 2564 Stage column should not "Primary" when message is closed fixed
 * 16-01-2008   Prerak - Added Javascript pageload(2) on Previous, Next, Top button 
 * 18-01-2008   Prerak - Message Center SP Integration.
 * 30-01-2008   IAK   -  CR- MRN/DOB below patient name.
 * 14-02-2008   IAK   -  Handle null value for Stage Column.
 * 19-02-2008   IAK   -  Sort Order remain even after user returns to message center from other pages.
 * 19-02-2008   IAK   -   Removed Group Combo and From Date selection criteria
 *                        As per Fred Suggestion - Mail Saturday, February 16, 2008 2:01 AM
 * 25-02-2008   IAK   -   Defect 2799
 * 26-02-2008   IAK   -   Grid ViewState removed - Brijesh's Change
 * 27-02-2008   IAK   -   Removed Commented Code
 * 27-02-2008   IAK   -   'Not Sent (NS)' Functionality for Stage Column added
 * 27-02-2008   IAK   -   Message Created stage need to be 'NS'
 * 06-03-2008   IAK   -   If advance search for preselected group, then disable group and msg type combo
 * 13-03-2008   IAK   -   CR-Stage: NS: Only if receipent does not have device attach and message sent to him/her
 *                        Sort on Status Column  
 * 14-04-2008   IAK   -   CR: By default sorting on Status column 
 * 16-04-2008   SBT   -   Stage Column changes. Message state checking is done from database. 
 * 06-05-2008   Suhas     Defect # 2820 - Fixed.
 * 12 Jun 2008  Prerak -  Migration of AJAX Atlas to AJAX RTM 1.0
 * 20-06-2008   NDM       Stage column not updated correctly.
 * 30.06.2008   Suhas     CR# 256 - Support Monitoring Report Implementation.
 * 22-10-2008   RG        Modified to handle Non System Recipient
 * 12-15-2008   ZK        Defect #3155 Fixed - Animation Icon on click of links on Message Header
 * 12-31-2008   ZK        Known Defect- Index out of bound (Grid Header) for single record
 * ------------------------------------------------------------------- 
 */
#endregion

#region Using block
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.DataAccess;
using Vocada.VoiceLink.Utilities;
#endregion

namespace Vocada.CSTools
{
    /// <summary>
    /// This class will load the default page, this is the page/ screen shown to the user by default or when he logs in,
    /// It will load the Messages for that user in the datagrid showing him 2 checkboxes "Show Group Calls" and "Show All Messages"
    /// for normal user, and only 1 checkbox "Show All Messages" to Administrator. 
    /// </summary>
    public partial class message_center : System.Web.UI.Page
    {
        #region Page Variables
        private bool b_flg = false;
        private bool alternateItemFlag = false;
        private readonly string format = "MM/dd/yyyy hh:mm:sstt";
        private int numberOfMessages;
        private int visibleRows;
        private DataTable dtBlank = null;
        protected string strUserSettings = "NO";
        private const string RECIPIENT_NAME = "RecipientDisplayName";
        private const string CREATED_ON = "CreatedOn_UsersTime";
        private const string STATUS = "MessageStatusDateTime";
        private const string NOTE = "CS_Note";
        private const string FAILURES = "CS_Failures";
        private const string STARTS = "CS_Starts";
        private const int ROLE_SPECIALIST = 1;
        protected int nheight = 50;
        protected const string DT = "BlankDataTable";
        private string strDate = " ";
        private string FROM = "From";
        private int instId = -1;
        private bool isSystemAdmin = true;
        private int groupID = -1;
        
        #endregion

         #region Events
         /// <summary>
         /// This method will call fillForm method to load the MessageList for selected preference.
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void Page_Load(object sender, System.EventArgs e)
         {
             try
             {                                  
                 if(Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
                     Response.Redirect(Vocada.CSTools.Utils.GetReturnURL("default.aspx", "message_center.aspx", this.Page.ClientQueryString));

                 if (Session[SessionConstants.USER_SETTINGS] != null)
                     strUserSettings = Session[SessionConstants.USER_SETTINGS].ToString();
                 UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
                 if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
                 {
                     isSystemAdmin = false;
                     instId = userInfo.InstitutionID;
                 }
                 else
                 {
                     isSystemAdmin = true;
                 }
                 instId = userInfo.InstitutionID;
                 registerJavascriptVariables();

                 lbtnClear.Attributes.Add("onClick", "PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
                 dgOutstandingCalls.ItemStyle.BackColor = Color.FromArgb(202, 234, 253);
                 dgOutstandingCalls.AlternatingItemStyle.BackColor = Color.FromArgb(255, 255, 255);
                 nheight = (ViewState["nHeight"] == null ? 0 : Convert.ToInt32(ViewState["nHeight"]));
                 setGroupForMessages();
                
                 if (!Page.IsPostBack)
                 {
                     /*Sorting Message Center, default page*/
                     if (Session[SessionConstants.SHOW_All] == null)
                     {   Session[SessionConstants.SHOW_All] = true;
                         Session[SessionConstants.SORT_ON] = null;
                     }
                     if (Session[SessionConstants.WEEK_NUMBER] == null)
                     {
                         Session[SessionConstants.WEEK_NUMBER] = 1;
                         Session[SessionConstants.FROM_DATE] = FROM; 
                     }
                     
                     enabledControl();
                     if (Session[SessionConstants.SHOWMESSAGES] != null)
                         ddlistShowMessages.SelectedIndex = Convert.ToInt32(Session[SessionConstants.SHOWMESSAGES]);
                     else
                         Session[SessionConstants.SHOWMESSAGES] = ddlistShowMessages.SelectedIndex;
                     if (Session[SessionConstants.STATUS] != null)
                         cmbMsgStatus.SelectedIndex = Convert.ToInt32(Session[SessionConstants.STATUS]);
                     else
                         Session[SessionConstants.STATUS] = cmbMsgStatus.SelectedIndex;
                     if (Session[SessionConstants.SORT_ON] == null)
                         Session[SessionConstants.SORT_ON] = STATUS + " DESC";
                     calculateDate();
                     fillForm();
                     lbtnClear.Visible = true;
                     if (Session[SessionConstants.SEARCH_CRITERIA] == null)
                     {
                         enabledControl();
                         lbtnClear.Visible = false;
                     }
                 }
                 else if(Request["__eventargument"] == "load")
                 {
                     if ((Convert.ToInt32(Session[SessionConstants.SHOWMESSAGES]) == ddlistShowMessages.SelectedIndex) && (Convert.ToInt32(Session[SessionConstants.STATUS]) == cmbMsgStatus.SelectedIndex))
                     {
                         calculateDate();
                         fillForm();
                         lbtnClear.Visible = true;
                         if (Session[SessionConstants.SEARCH_CRITERIA] == null)
                         {
                             enabledControl();
                             lbtnClear.Visible = false;
                         }
                     }
                 }
                 else if (System.Text.RegularExpressions.Regex.IsMatch(Request["__EVENTTARGET"].ToString(), "dgOutstandingCalls"))
                 {
                     dgOutstandingCalls.DataSource = null;
                     dgOutstandingCalls.DataSource = new DataTable();
                     dgOutstandingCalls.DataBind();
                 }
                 this.Form.DefaultButton = this.btnGo.UniqueID;
                 
                if (Convert.ToInt16(cmbMsgStatus.SelectedValue) == MessageStatus.SupportMonitoringReport.GetHashCode())
                {
                    btnPrevious.Enabled = false;
                    btnNext.Enabled = false;
                    btnTop.Enabled = false;
                    btnPrevious.CssClass = "FrmbuttonDisabled";
                    btnNext.CssClass = "FrmbuttonDisabled";
                    btnTop.CssClass = "FrmbuttonDisabled";
                }                 
                
                 Session[SessionConstants.CURRENT_TAB] = "MsgCenter";
                 Session[SessionConstants.CURRENT_PAGE] = "message_center.aspx";
             }
             catch (Exception ex)
             {
                 if (Session[SessionConstants.USER_ID] != null)
                 {
                     Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("MessageCenter.Page_Load", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                 }
                 throw;
             }
             finally
             {

             }
         }
   
         /// <summary>
         /// Sort the message list as per selection in the reverse order of previous default is Ascending.
         /// </summary>
         /// <param name="source"></param>
         /// <param name="e"></param>
         protected void dgOutstandingCalls_SortCommand(object source, DataGridSortCommandEventArgs e)
         {
             try
             {                 
                 numberOfMessages = 0;
                 visibleRows = 0;
                 DataTable dt;
                 
                 dt = getMessagesInDataTable();
                 
                 numberOfMessages = dt.Rows.Count;
                 visibleRows = numberOfMessages;
                 DataView dvsrtOutstandingCalls = new DataView(dt);
                 string sortcolumn = string.Empty;
                 if (ViewState["ColumnName"] != null)
                     sortcolumn = ViewState["ColumnName"].ToString();
                 string sortDirection = string.Empty;
                 if (ViewState["Direction"] != null)
                     sortDirection = ViewState["Direction"].ToString();
                 if (sortcolumn == e.SortExpression.ToString() && sortDirection == "ASC")
                 {
                     dvsrtOutstandingCalls.Sort = e.SortExpression + " DESC";
                     ViewState["Direction"] = "DESC";
                 }
                 else if (sortcolumn == e.SortExpression.ToString() && sortDirection == "DESC")
                 {
                     dvsrtOutstandingCalls.Sort = e.SortExpression + " ASC";
                     ViewState["Direction"] = "ASC";
                 }
                 else
                 {
                     sortcolumn = e.SortExpression.ToString();
                     if (sortcolumn == NOTE || sortcolumn == FAILURES || sortcolumn == STARTS)
                     {
                         dvsrtOutstandingCalls.Sort = e.SortExpression + " DESC";
                         ViewState["Direction"] = "DESC";
                     }
                     else
                     {
                         dvsrtOutstandingCalls.Sort = e.SortExpression + " ASC";
                         ViewState["Direction"] = "ASC";
                     }
                     ViewState["ColumnName"] = e.SortExpression.ToString();
                 }
                 /*Sorting On Message Centre*/
                 if (sortcolumn != RECIPIENT_NAME)
                 {
                     dvsrtOutstandingCalls.Sort = dvsrtOutstandingCalls.Sort + ", " + RECIPIENT_NAME + " " + ViewState["Direction"];
                 }
                 else
                 {
                     dvsrtOutstandingCalls.Sort = dvsrtOutstandingCalls.Sort + ", " + CREATED_ON + " " + ViewState["Direction"];
                 }
                 if (cmbMsgStatus.SelectedValue == "0" || cmbMsgStatus.SelectedValue == "3")
                     dgOutstandingCalls.Columns[11].SortExpression = "MessageStatusDateTime";
                 else
                     dgOutstandingCalls.Columns[11].SortExpression = "";

                 Search objSearch = null;
                 objSearch = Session[SessionConstants.SEARCH_CRITERIA] as Search;
                 if (objSearch != null)
                 {
                     if (objSearch.GroupType == 1)
                         hideColumnsForLab(true);
                     else
                         hideColumnsForLab(false);
                 }
                 else
                 {
                     if (ddlistShowMessages.SelectedIndex == 1)//for lab
                         hideColumnsForLab(true);
                     else if (ddlistShowMessages.SelectedIndex == 0) // for radiology
                         hideColumnsForLab(false);
                 }
                if (Convert.ToInt16(cmbMsgStatus.SelectedValue) == MessageStatus.SupportMonitoringReport.GetHashCode())
                    hideColumnForSupportMonitoring(false);
                else
                    hideColumnForSupportMonitoring(true);

                 /*Sorting Message Center, default page*/
                 Session[SessionConstants.SORT_ON] = dvsrtOutstandingCalls.Sort;
                 dgOutstandingCalls.DataSource = null;
                 dgOutstandingCalls.DataSource = dvsrtOutstandingCalls;
                 setdatagridHeight();
                 dgOutstandingCalls.DataBind();
             }
             catch (Exception ex)
             {
                 if (Session[SessionConstants.USER_ID] != null)
                 {
                     Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("MessageCenter.getRadiologyMessages", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                 }
                 throw;
             }
             finally
             {
                 if (strUserSettings != "YES")
                     ScriptManager.RegisterStartupScript(upnlGridData, upnlGridData.GetType(), "GridLoad", "<script type=\'text/javascript\'>PageLoading('1', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');</script>", false);
             }
         }

        /// <summary>
        /// Handles the Click event of the btnGo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>        
         protected void btnGo_Click(object sender, EventArgs e)
         {
             //Page.RegisterClientScriptBlock("PageLoad", "<script type=\'text/javascript\'>PageLoad('2');</script>");
             Session[SessionConstants.WEEK_NUMBER] = 1;
             Session[SessionConstants.STATUS] = cmbMsgStatus.SelectedIndex;
             Session[SessionConstants.SHOWMESSAGES] = ddlistShowMessages.SelectedIndex;    
             calculateDate();
             fillForm();
             lbtnClear.Visible = true;
             if (Session[SessionConstants.SEARCH_CRITERIA] == null)
             {
                 enabledControl();
                 lbtnClear.Visible = false;
             }
            if (Convert.ToInt16(cmbMsgStatus.SelectedValue) == MessageStatus.SupportMonitoringReport.GetHashCode())
            {
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
                btnTop.Enabled = false;
                btnPrevious.CssClass = "FrmbuttonDisabled";
                btnNext.CssClass = "FrmbuttonDisabled";
                btnTop.CssClass = "FrmbuttonDisabled";
            }             
             
         }

        /// <summary>
        /// This event will bind each element from the resultset to the respective cell of datagrid, it will set the alternate style color programmatically
        /// to override the color of reply message programmatically.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgOutstandingCalls_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            //Setting alternate style color programmatically bcos we need to override the color
            //of reply message programmatically.
            bool itemFlag = false;
            string stage = "";
            string[] arrEvent = new string[2];
            int eventId = 0;

            if (e.Item.ItemType.ToString() != "Header" && alternateItemFlag == false)
            {
                if (b_flg)
                    b_flg = false;
                else
                    b_flg = true;
            }

            //Status with received messages

           
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView data = e.Item.DataItem as DataRowView;
                UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;

                string strValStatus = "";
                string strStatusDetails = "";
                string readbackStatus = "";
                int recepientType = -1;
                string isLabmsg = "0";
                arrEvent[0] = "0";
                arrEvent[1] = "";
                if(data["IsLabMessage"].ToString() == "0")
                    isLabmsg = "0";
                else
                    isLabmsg = "1";

                if (data["IsLabMessage"].ToString().Length > 1 && bool.Parse(data["IsLabMessage"].ToString()))
                    isLabmsg = "1";
                if (data["IsLabMessage"].ToString().Length > 1 && bool.Parse(data["IsLabMessage"].ToString()) == false)
                    isLabmsg = "0";
                (e.Item.Cells[12].Controls[0] as HyperLink).NavigateUrl = "./message_details.aspx?MessageID=" + data["MessageID"].ToString() + "&IsDeptMsg=" + data["IsDepartmentMessage"].ToString() + "&IsLabMsg=" + isLabmsg + "&returnTo=1";
                if (data["RecipientTypeID"] != null)
                    recepientType = Convert.ToInt32(data["RecipientTypeID"]);
                DateTime readOn = DateTime.MinValue;
                if (data["ReadOn"] != null)
                {
                    string sReadOn = data["ReadOn"].ToString();
                    if (sReadOn.Length > 0)
                        readOn = Convert.ToDateTime(sReadOn);
                }
                int replyId = 0;
                int readBackID = 0;
                if (data["MessageReplyID"] != System.DBNull.Value)
                {
                    replyId = Convert.ToInt32(data["MessageReplyID"].ToString());
                }
                if (data["ReadbackID"] != System.DBNull.Value)
                {
                    readBackID = Convert.ToInt32(data["ReadbackID"].ToString());
                }
                DateTime replyReadOn = DateTime.MinValue;
                if (data["ReplyReadOn"] != null)
                {
                    string sReplyReadOn = data["ReplyReadOn"].ToString();
                    if (sReplyReadOn.Length > 0)
                        replyReadOn = Convert.ToDateTime(sReplyReadOn);
                }
                DateTime readbackReadOn = DateTime.MinValue;
                if (data["AcceptRejectOn"] != null)
                {
                    string sReadbackReadOn = data["AcceptRejectOn"].ToString();
                    if (sReadbackReadOn.Length > 0)
                        readbackReadOn = Convert.ToDateTime(sReadbackReadOn);
                }
              

                bool isDocumented = false;
                if (data["IsDocumented"] != System.DBNull.Value)
                    isDocumented = Convert.ToBoolean(data["IsDocumented"]);

                string unitOCName = e.Item.Cells[7].Text;
                string patient = (string)data["PatientVoiceURL"];

                if (patient.StartsWith("http") || patient.StartsWith("https"))
                {
                   
                        e.Item.Cells[8].Text = "<a href=" + patient + "><img src=img/ic_play_msg.gif border=0></a>";
                        if (data["DOBorMRN"].ToString().Length > 0)
                        {
                            e.Item.Cells[8].Text += "</br>" + data["DOBorMRN"].ToString();
                        }
                }
                else  // patient name, not a patient recording...
                {
                    if (patient.Length > 25)
                    {
                        patient = patient.Substring(0, 25) + "...";
                        e.Item.Cells[8].ToolTip = (string)data["PatientVoiceURL"];
                    }
                    e.Item.Cells[8].Text = patient;
                    if (data["DOBorMRN"].ToString().Length > 0)
                    {
                        e.Item.Cells[8].Text += "</br>" + data["DOBorMRN"].ToString();
                    }
                }
                string finding = (string)data["FindingDescription"];

                StringBuilder sbOrderingClinician = new StringBuilder();
                int recipientID = Convert.ToInt32(data["RecipientID"]);
                if (recepientType == (int)MessageInfo.RecepientType.OrderingClinician && recipientID > 0)
                {
                    if (userInfo.RoleId != UserRoles.SupportLevel1.GetHashCode())
                    {
                        sbOrderingClinician.Append("<a href='./edit_oc.aspx?ReferringPhysicianID=");
                        sbOrderingClinician.Append(data["RecipientID"].ToString());
                        sbOrderingClinician.Append("' onclick=");
                        sbOrderingClinician.Append('"');
                        sbOrderingClinician.Append("if(window.parent.frames[0].document.getElementById('Directory') != null) { window.parent.frames[0].document.getElementById('Directory').click();} if(window.parent.frames[0].document.getElementById('DirectoryBnW') != null) { window.parent.frames[0].document.getElementById('DirectoryBnW').click();} if(window.parent.frames[0].document.getElementById('DirectoryBnWAlt') != null) { window.parent.frames[0].document.getElementById('DirectoryBnWAlt').click();}");
                        sbOrderingClinician.Append('"');
                        sbOrderingClinician.Append("target='_self'>");
                    }
                }
                string recipientName = data["RecipientDisplayName"].ToString();
                if (recipientName.Length > 25)
                {
                    recipientName = recipientName.Substring(0, 25) + "...";
                }
                sbOrderingClinician.Append(recipientName);
                if (recepientType == (int)MessageInfo.RecepientType.BedNumber && data["NurseID"] != System.DBNull.Value)
                {
                    if (data["NurseID"].ToString().Length > 0 && int.Parse(data["NurseID"].ToString()) != 0)
                        sbOrderingClinician.Append(" (Nurse)");
                    else
                        sbOrderingClinician.Append(" (Unit)");
                }
                else if (recepientType == (int)MessageInfo.RecepientType.UnitName || recepientType == (int)MessageInfo.RecepientType.BedNumber)
                {
                    sbOrderingClinician.Append(" (Unit)");
                }
                else if (recepientType == (int)MessageInfo.RecepientType.Department)
                {
                    sbOrderingClinician.Append(" (Clinical Team)");
                }
                else
                {
                    sbOrderingClinician.Append(" (OC)");
                    if (userInfo.RoleId != UserRoles.SupportLevel1.GetHashCode() && recipientID > 0)
                    {
                        sbOrderingClinician.Append("</a>");
                    }
                }

                if (data["RecipientDisplayName"] != System.DBNull.Value)
                {
                    sbOrderingClinician.Append("<br>");
                }
                sbOrderingClinician.Append("#");
                sbOrderingClinician.Append(data["PassCode1"]);

                e.Item.Cells[6].Text = sbOrderingClinician.ToString();
                if (data["RecipientDisplayName"].ToString().Length > 30)
                    e.Item.Cells[6].ToolTip = data["RecipientDisplayName"].ToString();

                if (readOn == DateTime.MinValue)  // message not read yet, flag accordingly.
                {
                    // EscalationsComplete or ComplianceEscalationComplete ==1
                    if (data["EscalationsComplete"] != null)
                    {
                        if ((Convert.ToBoolean(data["EscalationsComplete"])) || (Convert.ToBoolean(data["ComplianceEscalationComplete"])))
                        {
                            //red
                            e.Item.Cells[0].Text = "<image src=img/ic_baloon_red.gif border=0>";
                        }
                        else
                        {
                            //green
                            e.Item.Cells[0].Text = "<image src=img/ic_ballon_green.gif border=0  >";
                        }
                    }
                }
                else
                {
                    //if MessageReplyID > 0 and readon null
                    if (replyId > 0 && readOn == DateTime.MinValue)
                    {
                        //green
                        e.Item.Cells[0].Text = "<image src=img/ic_ballon_green.gif border=0  >";
                    }
                    else
                    {
                        //gray
                        e.Item.Cells[0].Text = "<image src=img/ic_baloon_gray.gif border=0  >";
                    }
                }

                if (data["ImpressionVoiceURL"] == System.DBNull.Value || data["ImpressionVoiceURL"].ToString().Length == 0)
                {
                    e.Item.Cells[10].Text = "";
                }

                if (isDocumented)
                {
                    e.Item.Cells[0].Text = "<image src=img/ic_baloon_yellow.gif border=0  >";
                }
                /*Sorting On Message Centre*/
                if (replyId > 0 || readBackID > 0)
                {
                    if (e.Item.Visible)
                        visibleRows++;
                    if ((replyId > 0 && replyReadOn == DateTime.MinValue) || (readBackID > 0 && readbackReadOn == DateTime.MinValue))
                    {
                        //green
                        e.Item.Cells[0].Text = "<image src=img/ic_ballon_green.gif border=0  >" + "<br><br>" + "<image src=img/icon_reply.gif border=0  >";
                    }
                    else
                    {
                        //gray
 
                        e.Item.Cells[0].Text = "<image src=img/ic_baloon_gray.gif border=0  > ";
                            if (replyId > 0 || readBackID > 0)
                                e.Item.Cells[0].Text = e.Item.Cells[0].Text + "<br><br>" + "<image src=img/icon_reply.gif border=0  >";
                    }

                    //If message has reply. 
                    if (replyId > 0)
                    {
                        e.Item.Cells[5].Text = e.Item.Cells[5].Text + "<br><br>" + String.Format("{0:MM/dd/yyyy hh:mmtt} ", Convert.ToDateTime(data["ReplyCreatedOn"]));

                        bool isMessageRecorded = true;
                        if (data["ImpressionVoiceURL"] == System.DBNull.Value || data["ImpressionVoiceURL"].ToString().Length == 0)
                        {
                            isMessageRecorded = false;
                        }
                      
                            StringBuilder sbWithoutUserSettings = new StringBuilder();
                            if (isMessageRecorded)
                            {
                                sbWithoutUserSettings.Append("<a href=");
                                sbWithoutUserSettings.Append(data["ImpressionVoiceURL"].ToString());
                                sbWithoutUserSettings.Append("><img src=img/ic_play_msg.gif border=0></a>");
                            }
                            sbWithoutUserSettings.Append("<br><br>");
                            sbWithoutUserSettings.Append("<a href=");
                            sbWithoutUserSettings.Append(data["ReplyVoiceURL"].ToString());
                            sbWithoutUserSettings.Append("><img src=img/ic_play_msg.gif border=0></a>");

                            e.Item.Cells[10].Text = sbWithoutUserSettings.ToString();
                            e.Item.Cells[6].Text = e.Item.Cells[6].Text + "<br>" + " -Reply";

                    }

                    else
                    {
                        e.Item.Cells[5].Text = e.Item.Cells[5].Text + "<br><br>" + String.Format("{0:MM/dd/yyyy hh:mmtt} ", Convert.ToDateTime(data["ReadbackCreatedOn"]));
                        if (readbackReadOn != DateTime.MinValue)
                        {
                                StringBuilder sbWithoutUserSettings = new StringBuilder();
                                sbWithoutUserSettings.Append("<a href=");
                                sbWithoutUserSettings.Append(data["ImpressionVoiceURL"].ToString());
                                sbWithoutUserSettings.Append("><img src=img/ic_play_msg.gif border=0></a>");
                                sbWithoutUserSettings.Append("<br><br>");
                                sbWithoutUserSettings.Append("<a href=");
                                sbWithoutUserSettings.Append(data["ReadbackVoiceURL"].ToString());
                                sbWithoutUserSettings.Append("><img src=img/ic_play_msg.gif border=0></a>");

                                e.Item.Cells[10].Text = sbWithoutUserSettings.ToString();
                        }
                        else
                        {
                            StringBuilder sbURL = new StringBuilder();
                            sbURL.Append(data["ReadbackVoiceURL"].ToString());
                            sbURL.Append("$");
                            sbURL.Append(readBackID);
                            sbURL.Append("$");
                            sbURL.Append(data["MessageID"].ToString());
                            sbURL.Append("$");
                            sbURL.Append(data["IsDepartmentMessage"].ToString());
                            sbURL.Append("$");
                            sbURL.Append(groupID.ToString());

                          
                                StringBuilder sbWithoutUserSettings = new StringBuilder();
                                sbWithoutUserSettings.Append("<a href=");
                                sbWithoutUserSettings.Append(data["ImpressionVoiceURL"].ToString());
                                sbWithoutUserSettings.Append("><img src=img/ic_play_msg.gif border=0></a>");
                                sbWithoutUserSettings.Append("<br><br>");
                                sbWithoutUserSettings.Append("<a href=");
                                sbWithoutUserSettings.Append(data["ReadbackVoiceURL"].ToString());
                                sbWithoutUserSettings.Append("><img src=img/ic_play_msg.gif border=0  onclick=Start('");
                                sbWithoutUserSettings.Append(sbURL.ToString());
                                sbWithoutUserSettings.Append("');></a>");
                                e.Item.Cells[10].Text = sbWithoutUserSettings.ToString();
                          
                        }

                        e.Item.Cells[6].Text = e.Item.Cells[6].Text + "<br>" + " -Readback";
                    }
                }

                if (data["Stage"] != null && data["Stage"].ToString().Length != 0)
                    eventId = Convert.ToInt32(data["Stage"]);
                else
                    eventId = 0;
                eventId = (eventId == 0 ? 39 : eventId -1);
                int notificationStatusID = -1;
                int newEventID;
                if (data["SecondStage"] != null && data["SecondStage"].ToString().Length != 0)
                    newEventID = Convert.ToInt32(data["SecondStage"]);
                else
                    newEventID = 0;
                newEventID = (newEventID == 0 ? 39 : newEventID - 1);

                if (eventId != 33 && eventId != 30)
                {
                    /*** Suhas T. 16/04/2008 - this change is for stage column in the message center grid **/
                    /*** For Primary,Backup,Compliacne and failsafe - messages state checking is done from database and not from notification history */
                    if (eventId == 16 | newEventID == 16) //Compliance notification embargo
                    {
                        eventId = 26;
                    }
                    if ((eventId == 15 && newEventID == 16) | (eventId == 15 && newEventID == 26)) //Compliance Escalation Time Passed - No Notifications Specified
                    {
                        eventId = 26;
                    }
                    if (eventId != 26 && eventId != 6 && eventId != 7 && eventId != 8 && eventId != 9 && readOn == DateTime.MinValue)
                    {
                        if (Convert.ToInt16(data["MessageState"].ToString()) == 4 && (newEventID == 14 || newEventID == 15 || newEventID == 16 || newEventID == 17 || newEventID == 18)) // Compliance
                        {
                            eventId = 14;
                        }
                        else if (Convert.ToInt16(data["MessageState"].ToString()) == 3 && (newEventID == 19 || newEventID == 21 || newEventID == 20 || newEventID == 20 || newEventID == 28 || newEventID == 29 || newEventID == 30 || newEventID == 31 || newEventID == 32 || newEventID == 22 || newEventID == 23 || newEventID == 24 || newEventID == 25)) // Fail-Safe
                        {
                            //eventId = 19;
                            if (newEventID == 20)
                                eventId = 20;
                        }
                        else if (Convert.ToInt16(data["MessageState"].ToString()) == 2 && (newEventID == 22 || newEventID == 23 || newEventID == 24 || newEventID == 25 || newEventID == 28 || newEventID == 29 || newEventID == 30 || newEventID == 31 || newEventID == 32)) // Backup
                        {
                            eventId = 22;
                        }
                        else if (Convert.ToInt16(data["MessageState"].ToString()) == 1 && (newEventID == 28 || newEventID == 29 || newEventID == 30 || newEventID == 31 || newEventID == 32)) // Primary
                        {
                            eventId = 28;
                        }
                    }

                    e.Item.Cells[14].Text = Utils.GetEventDescription(eventId);
                }
                else
                {
                    e.Item.Cells[14].Text = "NS";
                    e.Item.Cells[14].Style.Add("color", "red");
                }

                alternateItemFlag = false;

            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                if (dgOutstandingCalls.Items.Count > 1)
                {
                    LinkButton lnkGroupName = (LinkButton)(e.Item.Cells[3].Controls[0]);
                    LinkButton lnkCreatedBy = (LinkButton)(e.Item.Cells[4].Controls[0]);
                    LinkButton lnkCreatedOn = (LinkButton)(e.Item.Cells[5].Controls[0]);
                    LinkButton lnkCreatedFor = (LinkButton)(e.Item.Cells[6].Controls[0]);
                    LinkButton lnkBed = (LinkButton)(e.Item.Cells[7].Controls[0]);
                    LinkButton lnkFinding = (LinkButton)(e.Item.Cells[9].Controls[0]);
                    LinkButton lnkStatus = (LinkButton)(e.Item.Cells[11].Controls[0]);
                    LinkButton lnkNote = (LinkButton)(e.Item.Cells[13].Controls[0]);
                    LinkButton lnkStage = (LinkButton)(e.Item.Cells[14].Controls[0]);
                    LinkButton lnkFailures = (LinkButton)(e.Item.Cells[15].Controls[0]);
                    LinkButton lnkStarts = (LinkButton)(e.Item.Cells[16].Controls[0]);

                    lnkGroupName.Attributes.Add("onclick", "javascript:PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
                    lnkCreatedBy.Attributes.Add("onclick", "javascript:PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
                    lnkCreatedOn.Attributes.Add("onclick", "javascript:PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
                    lnkCreatedFor.Attributes.Add("onclick", "javascript:PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
                    lnkBed.Attributes.Add("onclick", "javascript:PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
                    lnkFinding.Attributes.Add("onclick", "javascript:PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
                    lnkStatus.Attributes.Add("onclick", "javascript:PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
                    lnkNote.Attributes.Add("onclick", "javascript:PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
                    lnkStage.Attributes.Add("onclick", "javascript:PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
                    lnkFailures.Attributes.Add("onclick", "javascript:PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
                    lnkStarts.Attributes.Add("onclick", "javascript:PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
                }
            }
        }

        /// <summary>
        /// Handles the ItemCreated event of the dgOutstandingCalls control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
        protected void dgOutstandingCalls_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                int i = 0;
                if (e.Item.Cells[6].Controls.Count > 0)
                {
                    LinkButton lblRecipient = (e.Item.Cells[6].Controls[0]) as LinkButton;
                    lblRecipient.Text = "Recipients";
                    Label lblText = new Label();
                    lblText.Text = "</br> #PassCode";
                    e.Item.Cells[6].Controls.Add(lblText);
                }
                if (e.Item.Cells[8].Controls.Count > 0)
                {
                    LinkButton lblRecipient = (e.Item.Cells[8].Controls[0]) as LinkButton;
                    lblRecipient.Text = "Patient";
                    Label lblText = new Label();
                    lblText.Text = "</br> MRN/DOB";
                    e.Item.Cells[8].Controls.Add(lblText);
                }
            }
        }
        /// <summary>
        /// Handles the Click event of the btnPrevious control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[SessionConstants.WEEK_NUMBER]) <= 4)
            {
                Session[SessionConstants.WEEK_NUMBER] = Convert.ToInt32(Session[SessionConstants.WEEK_NUMBER]) + 1;
                calculateDate();
                fillForm();
                enabledControl();
            }
            
        }
        /// <summary>
        /// Handles the Click event of the btnNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[SessionConstants.WEEK_NUMBER]) >= 1)
            {
                Session[SessionConstants.WEEK_NUMBER] = Convert.ToInt32(Session[SessionConstants.WEEK_NUMBER]) - 1;
                calculateDate();
                fillForm();
                enabledControl();
            }
           
        }
        /// <summary>
        /// Handles the Click event of the btnTop control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnTop_Click(object sender, EventArgs e)
        {
            Session[SessionConstants.WEEK_NUMBER] = 1;
            calculateDate();
            fillForm();
            enabledControl();
        }
        /// <summary>
        /// Handles the Click event of the lbtnClear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            Session[SessionConstants.SEARCH_CRITERIA] = null;
            btnGo_Click(sender, e);
        }
         #endregion

         #region Private Methods
       
        /// <summary>
        /// This method will be called whenever page loads, or any combobox selection is changed.
        /// It will call getUserConfigurationData(), getSubscriberInformation() and getRadiologyMessages() methods
        /// to fetch the dataset for any change in selection, and will store those results in the global variables.
        /// </summary>
        private void fillForm()
        {
            //SubscriberInfo objSubscriberInfo = null;
            MessageCenter objMsgCenter = null;
            try
            {  
                objMsgCenter = new MessageCenter();
                if (Session[SessionConstants.SEARCH_CRITERIA] != null)
                {
                    searchMessages();
                    controlEnabled(false);

                }
                else
                {    
                    if (Convert.ToInt16(cmbMsgStatus.SelectedValue) == MessageStatus.SupportMonitoringReport.GetHashCode())
                        hideColumnForSupportMonitoring(false);
                    else
                        hideColumnForSupportMonitoring(true);
                        
                    if(ddlistShowMessages.SelectedIndex == 1)//for lab
                    {
                        hideColumnsForLab(true);
                        getLabMessages();
                    }
                    else if (ddlistShowMessages.SelectedIndex == 0) // for radiology
                    {
                        hideColumnsForLab(false);
                        getRadiologyMessages();
                    }
                    else // For both option
                    {
                        hideColumnsForLab(true);
                        getBothMessages();
                    }
                    controlEnabled(true);
                }
                ScriptManager.RegisterStartupScript(parentPanel, parentPanel.GetType(), "PageLoading", "<script type=\'text/javascript\'>PageLoading('1', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');</script>", false);

                //Setting the height of datagrid dynamically.
                setdatagridHeight();
            }
            catch(Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("MessageCenter.FillForm", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw;
            }

            finally
            {
                objMsgCenter = null;
            }

        }

        /// <summary>
        /// This method hides or changes the title of column for radiology.
        /// </summary>
        private void hideColumnsForLab(bool flgShow)
        {
            if (Convert.ToInt16(cmbMsgStatus.SelectedValue) == MessageStatus.SupportMonitoringReport.GetHashCode())
            {
                dgOutstandingCalls.Columns[7].Visible = false;
            }
            else
            {
                dgOutstandingCalls.Columns[7].Visible = flgShow;
            }
        }

        /// <summary>
        /// Hides the column for support monitoring.
        /// </summary>
        /// <param name="status">if set to <c>true</c> [status].</param>
        private void hideColumnForSupportMonitoring(bool status)
        {
            dgOutstandingCalls.Columns[7].Visible = status;
            dgOutstandingCalls.Columns[8].Visible = status;
            dgOutstandingCalls.Columns[10].Visible = status;
            dgOutstandingCalls.Columns[11].Visible = status;
            dgOutstandingCalls.Columns[13].Visible = status;
            dgOutstandingCalls.Columns[14].Visible = status;
            dgOutstandingCalls.Columns[15].Visible = status;
            dgOutstandingCalls.Columns[16].Visible = status;
            dgOutstandingCalls.Columns[17].Visible = (status)? false:true;
        }

        /// <summary>
        /// This method must get called whenever the page loads/postback, this will set the datagrid height
        /// dynamically as per the number of records returned.
        /// </summary>
        private void setdatagridHeight()
        {
            string script = "<script type=\"text/javascript\">";
            script += "if(document.getElementById(" + '"' + "MessageDiv" + '"' + ") != null){document.getElementById(" + '"' + "MessageDiv" + '"' + ").style.height=setHeightOfGrid('" + dgOutstandingCalls.ClientID + "','" + 60 + "');}</script>";
            ScriptManager.RegisterStartupScript(parentPanel, parentPanel.GetType(),"SetHeight", script,false);
        }

        /// <summary>
        /// Get Data to load into Grid
        /// </summary>
        /// <returns></returns>
        private DataTable getMessagesInDataTable()
        {
            DataTable dtMessages = null;
            MessageCenter objMsgCenter = null;
            try
            {
                objMsgCenter = new MessageCenter();
                int institutionID;
                int msgStatus = Convert.ToInt32(cmbMsgStatus.SelectedValue);
                int weekNumber = Convert.ToInt32(Session[SessionConstants.WEEK_NUMBER]);
                string fromDate = Session[SessionConstants.FROM_DATE].ToString().ToUpper() ;


                if (isSystemAdmin)
                    institutionID = -1;
                else
                    institutionID = instId;

                if (fromDate == "" || fromDate == FROM.ToUpper() )
                    fromDate = "";

                if (Session[SessionConstants.SEARCH_CRITERIA] != null)
                {
                    Search objSerch = Session[SessionConstants.SEARCH_CRITERIA] as Search;
                    if (objSerch.GroupType == 0)
                        dtMessages = objMsgCenter.SearchMessagesForRadiology(institutionID, objSerch);
                    else
                        dtMessages = objMsgCenter.SearchMessagesForLab(institutionID, objSerch);
                    objSerch = null;
                }
                else
                {
                    string msgFor = "RAD";
                    if (ddlistShowMessages.SelectedIndex == 0)
                        msgFor = "RAD";
                    else if (ddlistShowMessages.SelectedIndex == 1)
                        msgFor = "LAB";
                    else
                        msgFor = "BOTH";
                    dtMessages = objMsgCenter.GetMessages(groupID, msgStatus, weekNumber, fromDate, institutionID, msgFor);
                }
                numberOfMessages = dtMessages.DefaultView.Table.Rows.Count;
                if(numberOfMessages == 0)
                    divNoMessageWarningLabel.Visible = true;
                else
                    divNoMessageWarningLabel.Visible = false;
                return dtMessages;
            }
            catch(Exception ex)
            {
                if(Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("MessageCenter.getMessagesInDataTable", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw;
            }
            finally
            {
                dtMessages = null;
                objMsgCenter = null;
            }
        }

        /// <summary>
        /// Gets the messages for all the units to which Unit admin or charge nurse belongs to.
        /// </summary>
        private void getLabMessages()
        {
            DataTable dtLabMsgs = null;
            try
            {
                numberOfMessages = 0;
                visibleRows = 0;

                dtLabMsgs = getMessagesInDataTable();
                numberOfMessages = dtLabMsgs.Rows.Count;
                visibleRows = numberOfMessages;
                
                dtBlank = dtLabMsgs.Clone();
                Session[DT] = dtBlank; 
                
                DataView dvOutstandingCalls = new DataView(dtLabMsgs);

                /*Sorting Message Center, default page*/
                if (Session[SessionConstants.SORT_ON] != null)
                {
                    if (Session[SessionConstants.SORT_ON].ToString().Substring(0, Session[SessionConstants.SORT_ON].ToString().IndexOf(" ")) == STATUS && (cmbMsgStatus.SelectedValue == "1" || cmbMsgStatus.SelectedValue == "2"))
                        dvOutstandingCalls.Sort = CREATED_ON + " DESC";
                    else
                        dvOutstandingCalls.Sort = Session[SessionConstants.SORT_ON].ToString();

                }
                dgOutstandingCalls.DataSource = null;
                dgOutstandingCalls.DataSource = dvOutstandingCalls;

                if (numberOfMessages <= 1)
                {
                    dgOutstandingCalls.AllowSorting = false;
                }
                else
                {
                    dgOutstandingCalls.AllowSorting = true;
                    if (cmbMsgStatus.SelectedValue == "0" || cmbMsgStatus.SelectedValue == "3")
                        dgOutstandingCalls.Columns[11].SortExpression = "MessageStatusDateTime";
                    else
                        dgOutstandingCalls.Columns[11].SortExpression = "";
                }
     
                dgOutstandingCalls.DataBind();
                
                StringBuilder strMessage = new StringBuilder();
                if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 0)
                {
                    strMessage.Append("  Open");
                }
                else if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 1)
                {
                    strMessage.Append("  Recently Closed");
                }
                else if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 2)
                {
                    strMessage.Append("  Documented");
                }
                else if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 3)
                {
                    strMessage.Append("  Embargoed");
                }
                else if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 4)
                {
                    strMessage.Append("  All");
                }

                if (numberOfMessages == 1 || numberOfMessages == 0)
                {
                    strMessage.Append(" Message");
                }
                else
                {
                    strMessage.Append(" Messages");
                }
                if (Convert.ToInt16(cmbMsgStatus.SelectedValue) == MessageStatus.SupportMonitoringReport.GetHashCode())
                {
                    labelOutgoing.Text = numberOfMessages.ToString() + strMessage; 
                }
                else
                {
                    labelOutgoing.Text = numberOfMessages.ToString() + strMessage + strDate;
                }
            }

            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("MessageCenter.getLabMessages", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
                finally
                {
                    dtLabMsgs = null;
                }
        }

        /// <summary>
        /// This method will fire "voc_vlr_getRecentMessagesForSubscriber" proc, it will return the resultset which will be
        /// bound to the datagrid, it will store the total rows affected in a string which will be assigned to label that 
        /// displays the total rows.
        /// </summary>
        /// <param name="cnx"></param>
        private void getRadiologyMessages()
        {         
            try
            {
                numberOfMessages = 0;
                visibleRows = 0;

                DataTable dtRadiology = getMessagesInDataTable();
                numberOfMessages = dtRadiology.Rows.Count;                
                visibleRows = numberOfMessages;
                DataView dvOutstandingCalls = new DataView(dtRadiology);
                
                dtBlank = dtRadiology.Clone();
                Session[DT] = dtBlank; 
                
                /*Sorting Message Center, default page*/
                if (Session[SessionConstants.SORT_ON] != null)
                {
                    if (Session[SessionConstants.SORT_ON].ToString().Substring(0, Session[SessionConstants.SORT_ON].ToString().IndexOf(" ")) == STATUS && (cmbMsgStatus.SelectedValue == "1" || cmbMsgStatus.SelectedValue == "2"))
                        dvOutstandingCalls.Sort = CREATED_ON + " DESC";
                    else
                        dvOutstandingCalls.Sort = Session[SessionConstants.SORT_ON].ToString();

                }
                dgOutstandingCalls.DataSource = null;
                dgOutstandingCalls.DataSource = dvOutstandingCalls;

                if (numberOfMessages <= 1)
                    dgOutstandingCalls.AllowSorting = false;
                else
                {
                    dgOutstandingCalls.AllowSorting = true;
                    if (cmbMsgStatus.SelectedValue == "0" || cmbMsgStatus.SelectedValue == "3")
                        dgOutstandingCalls.Columns[11].SortExpression = "MessageStatusDateTime";
                    else
                        dgOutstandingCalls.Columns[11].SortExpression = "";
                }

                dgOutstandingCalls.DataBind();
                
                
                StringBuilder strMessage = new StringBuilder();
                if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 0)
                {
                    strMessage.Append("  Open");
                }
                else if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 1)
                {
                    strMessage.Append("  Recently Closed");
                }
                else if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 2)
                {
                    strMessage.Append("  Documented"); 
                }
                else if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 3)
                {
                    strMessage.Append("  Embargoed");
                }
                else if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 4)
                {
                    strMessage.Append("  All");
                }
                if (numberOfMessages == 1 || numberOfMessages == 0)
                {
                    strMessage.Append(" Message");
                }
                else
                {
                    strMessage.Append(" Messages");
                }
                if (Convert.ToInt16(cmbMsgStatus.SelectedValue) == MessageStatus.SupportMonitoringReport.GetHashCode())
                {
                    labelOutgoing.Text = numberOfMessages.ToString() + strMessage;
                }
                else
                {
                    labelOutgoing.Text = numberOfMessages.ToString() + strMessage + strDate;
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID]  != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("MessageCenter.getRadiologyMessages", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw;
            }

        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgOutstandingCalls.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgOutstandingCalls_ItemDataBound);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        
        #endregion
        /// <summary>
        /// This method decrements the value of numberOfMessages and visibleRows as the record is not shown for the particular condition
        /// it also reverse the value of flag so as to show the color of the row for alternating item.
        /// </summary>
        private void hideRecord()
        {
            if(numberOfMessages > 0)
                numberOfMessages--;
            if(visibleRows > 0)
                visibleRows--;
            if(b_flg == true)
                b_flg = false;
            else
                b_flg = true; 
        }
       
        /// <summary>
        /// This method featch data for Both option. 
        /// </summary>
        private void getBothMessages()
        {
            DataTable dtBothMsgs = null;
            DataView dvOutstandingCalls = null;
            try
            {
                numberOfMessages = 0;
                visibleRows = 0;

                dtBothMsgs = getMessagesInDataTable();

                dtBlank = dtBothMsgs.Clone();
                Session[DT] = dtBlank;

                numberOfMessages = dtBothMsgs.Rows.Count;
                visibleRows = numberOfMessages;
                dvOutstandingCalls = new DataView(dtBothMsgs);

                /*Sorting Message Center, default page*/
                if (Session[SessionConstants.SORT_ON] != null)
                {
                    if (Session[SessionConstants.SORT_ON].ToString().Substring(0, Session[SessionConstants.SORT_ON].ToString().IndexOf(" ")) == STATUS && (cmbMsgStatus.SelectedValue == "1" || cmbMsgStatus.SelectedValue == "2"))
                        dvOutstandingCalls.Sort = CREATED_ON + " DESC";
                    else
                        dvOutstandingCalls.Sort = Session[SessionConstants.SORT_ON].ToString();

                }
                dgOutstandingCalls.DataSource = null;
                dgOutstandingCalls.DataSource = dvOutstandingCalls;

                if (numberOfMessages <= 1)
                    dgOutstandingCalls.AllowSorting = false;
                else
                {
                    dgOutstandingCalls.AllowSorting = true;
                    if (cmbMsgStatus.SelectedValue == "0" || cmbMsgStatus.SelectedValue == "3")
                        dgOutstandingCalls.Columns[11].SortExpression = "MessageStatusDateTime";
                    else
                        dgOutstandingCalls.Columns[11].SortExpression = "";
                }

                dgOutstandingCalls.DataBind();

                StringBuilder strMessage = new StringBuilder();
                if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 0)
                {
                    strMessage.Append("  Open");
                }
                else if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 1)
                {
                    strMessage.Append("  Recently Closed");
                }
                else if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 2)
                {
                    strMessage.Append("  Documented");
                }
                else if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 3)
                {
                    strMessage.Append("  Embargoed");
                }
                else if (Convert.ToInt32(cmbMsgStatus.SelectedValue) == 4)
                {
                    strMessage.Append("  All");
                }
                if (numberOfMessages == 1 || numberOfMessages == 0)
                {
                    strMessage.Append(" Message");
                }
                else
                {
                    strMessage.Append(" Messages");
                }
                if (Convert.ToInt16(cmbMsgStatus.SelectedValue) == MessageStatus.SupportMonitoringReport.GetHashCode())
                {
                    labelOutgoing.Text = numberOfMessages.ToString() + strMessage;
                }
                else
                {
                    labelOutgoing.Text = numberOfMessages.ToString() + strMessage + strDate;
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("MessageCenter.getBothMessages", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw;
            }
            finally
            {
                dvOutstandingCalls = null;
                dtBothMsgs = null;
            }
        }

        private void enabledControl()
        {
            if (Convert.ToInt32(Session[SessionConstants.WEEK_NUMBER]) >= 4)
            {
                btnPrevious.Enabled = false;
                btnNext.Enabled = true;
                btnTop.Enabled = true;
                btnPrevious.CssClass = "FrmbuttonDisabled";
                btnNext.CssClass = "Frmbutton";
                btnTop.CssClass = "Frmbutton";
            }
            else if (Convert.ToInt32(Session[SessionConstants.WEEK_NUMBER]) == 1)
            {
                btnPrevious.Enabled = true;
                btnNext.Enabled = false;
                btnTop.Enabled = false;
                btnPrevious.CssClass = "Frmbutton";
                btnNext.CssClass = "FrmbuttonDisabled";
                btnTop.CssClass = "FrmbuttonDisabled";
            }
            else
            {
                btnPrevious.Enabled = true;
                btnNext.Enabled = true;
                btnTop.Enabled = true;
                btnPrevious.CssClass = "Frmbutton";
                btnNext.CssClass = "Frmbutton";
                btnTop.CssClass = "Frmbutton";
            }
        }
        /// <summary>
        /// Calculate From Date and To date for which messages is shown
        /// </summary>
        private void calculateDate()
        {
            int week;
            string todate;
            string fromdate;
            DateTime today;

            week = Convert.ToInt32(Session[SessionConstants.WEEK_NUMBER]);

            switch (week)
            {
                case 1:
                    today = DateTime.Today;
                    fromdate = today.AddDays(-6).ToString();
                    todate = today.ToString();
                    strDate = " for " + fromdate.Substring(0, fromdate.Length - 12) + " to " + todate.Substring(0, todate.Length - 12);
                    break;
                case 2:
                    today = DateTime.Today;
                    fromdate = today.AddDays(-13).ToString();
                    todate = today.AddDays(-7).ToString();
                    strDate = " for " + fromdate.Substring(0, fromdate.Length - 12) + " to " + todate.Substring(0, todate.Length - 12);

                    break;
                case 3:
                    today = DateTime.Today;
                    fromdate = today.AddDays(-20).ToString();
                    todate = today.AddDays(-14).ToString();
                    strDate = " for " + fromdate.Substring(0, fromdate.Length - 12) + " to " + todate.Substring(0, todate.Length - 12);

                    break;
                case 4:
                    today = DateTime.Today;
                    fromdate = today.AddDays(-30).ToString();
                    todate = today.AddDays(-21).ToString();
                    strDate = " for " + fromdate.Substring(0, fromdate.Length - 12) + " to " + todate.Substring(0, todate.Length - 12);
                    break;
            }
        }

        /// <summary>
        /// Registers the javascript variables.
        /// </summary>
        private void registerJavascriptVariables()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var btnGoClientID = '" + btnGo.ClientID + "';");
            sbScript.Append("var btnNextClientID = '" + btnNext.ClientID + "';");
            sbScript.Append("var btnPreviousClientID = '" + btnPrevious.ClientID + "';");
            sbScript.Append("var btnTopClientID = '" + btnTop.ClientID + "';");
            sbScript.Append("</script>"); 
            this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());
            btnPrevious.Attributes.Add("onClick", "PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
            btnTop.Attributes.Add("onClick", "PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
            btnNext.Attributes.Add("onClick", "PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
            btnGo.Attributes.Add("onClick", "PageLoading('2', 'ctl00_tdMessageList', 'ctl00_tdLoading', '91px');");
            cmbMsgStatus.Attributes.Add("onChange", "Javascript:visibleNavigationButtons(false);");
            ddlistShowMessages.Attributes.Add("onChange", "Javascript:visibleNavigationButtons(false);");
            
            
        }

        /// <summary>
        /// This Method Gets the messges for Advance Search
        /// </summary>
        private void searchMessages()
        {
            try
            {
                numberOfMessages = 0;
                visibleRows = 0;
                DataTable dtRadMsgs = null;
                Search objSearch = null;
                objSearch = Session[SessionConstants.SEARCH_CRITERIA] as Search;  

                dtRadMsgs = getMessagesInDataTable();

                dtBlank = dtRadMsgs.Clone();
                Session[DT] = dtBlank;

                numberOfMessages = dtRadMsgs.Rows.Count;
                visibleRows = numberOfMessages;
                DataView dvOutstandingCalls = new DataView(dtRadMsgs);

                /*Sorting Message Center, default page*/
                if (Session[SessionConstants.SORT_ON] != null)
                {
                    if (Session[SessionConstants.SORT_ON].ToString().Substring(0, Session[SessionConstants.SORT_ON].ToString().IndexOf(" ")) == STATUS && (cmbMsgStatus.SelectedValue == "1" || cmbMsgStatus.SelectedValue == "2"))
                        dvOutstandingCalls.Sort = CREATED_ON + " DESC";
                    else
                        dvOutstandingCalls.Sort = Session[SessionConstants.SORT_ON].ToString();

                }
                dgOutstandingCalls.DataSource = null;
                dgOutstandingCalls.DataSource = dvOutstandingCalls;

                if (numberOfMessages <= 1)
                    dgOutstandingCalls.AllowSorting = false;
                else
                {
                    dgOutstandingCalls.AllowSorting = true;
                    if (cmbMsgStatus.SelectedValue == "0" || cmbMsgStatus.SelectedValue == "3")
                        dgOutstandingCalls.Columns[11].SortExpression = "MessageStatusDateTime";
                    else
                        dgOutstandingCalls.Columns[11].SortExpression = "";
                }

                dgOutstandingCalls.DataBind();

                //setdatagridHeight();
                string strMessage = "";

                if (objSearch.MessageStatus  == 0)
                    strMessage += " Open";
                else if (objSearch.MessageStatus == 1)
                    strMessage += " Closed";
                else if (objSearch.MessageStatus == 2)
                    strMessage += " Documented";
                else                                
                    strMessage += "  All";
                
                if (numberOfMessages == 1 || numberOfMessages == 0)
                    strMessage += " Message";
                else
                    strMessage += " Messages";
                
                if (Convert.ToInt16(cmbMsgStatus.SelectedValue) == MessageStatus.SupportMonitoringReport.GetHashCode())
                {
                    labelOutgoing.Text = numberOfMessages.ToString() + strMessage;
                }
                else
                {
                    labelOutgoing.Text = numberOfMessages.ToString() + strMessage + " Searched";
                }
                
                if (objSearch.GroupType == 1)
                    hideColumnsForLab(true);
                else
                    hideColumnsForLab(false);
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("MessageCenter.searchMessages", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw;
            }
        }

        /// <summary>
        /// This Method Enabled - Disabled Control
        /// </summary>
        /// <param name="flg"></param>
        private void controlEnabled(bool flg)
        {
            cmbMsgStatus.Enabled = flg;
            if (groupID == -1)
                ddlistShowMessages.Enabled = flg;
            else
                ddlistShowMessages.Enabled = false;
            btnGo.Enabled = flg;
            btnPrevious.Enabled = flg;
            btnNext.Enabled = flg;
            btnTop.Enabled = flg;

            if (!flg)
            {
                btnGo.CssClass = "FrmbuttonDisabled";
                btnPrevious.CssClass = "FrmbuttonDisabled";
                btnNext.CssClass = "FrmbuttonDisabled";
                btnTop.CssClass = "FrmbuttonDisabled";
            }
            else
            {
                btnPrevious.CssClass = "Frmbutton";
                btnNext.CssClass = "Frmbutton";
                btnTop.CssClass = "Frmbutton";
                btnGo.CssClass = "Frmbutton";
            }
        }

        /// <summary>
        /// Setting if any group messages are viewing thr Group Maintenance
        /// </summary>
        private void setGroupForMessages()
        {
            Group group = null;
            DataTable dtGroupInfo = null;
            
            if (Request["groupID"] != null && Request["groupID"].Length > 0)
            {
                groupID = int.Parse(Request["groupID"]);
                if (ViewState["ForGroup"] == null || (ViewState["ForGroup"] != null && int.Parse(ViewState["ForGroup"].ToString()) != groupID))
                {
                    group = new Group();
                    dtGroupInfo = group.GetGroupInformationByGroupID(groupID);
                    if (dtGroupInfo.Rows.Count > 0)
                    {
                        if (string.Compare(dtGroupInfo.Rows[0]["GroupType"].ToString(), "true", true) == 0)
                        {
                            ddlistShowMessages.SelectedValue = "1";
                            Session[SessionConstants.SHOWMESSAGES] = 1;
                        }
                        else
                        {
                            ddlistShowMessages.SelectedValue = "0";
                            Session[SessionConstants.SHOWMESSAGES] = 0;
                        }
                    }
                    cmbMsgStatus.SelectedValue = "0";
                    Session[SessionConstants.STATUS] = 0;
                    group = null;
                    dtGroupInfo = null;
                }
                ViewState["ForGroup"] = groupID;
                hlinkSearch.Attributes.Add("onClick", "Javascript:OpenAdvanceSearch('" + groupID + "', '" + ddlistShowMessages.SelectedValue + "');");
            }
        }
        #endregion
    }
}
