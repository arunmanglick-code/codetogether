#region File History

/******************************File History***************************
 * File Name        : message_details.aspx.cs
 * Author           : 
 * Created Date     : 
 * Purpose          : Shows Details of Selected Message and also gives option to forward it.
 *                  : 
 *                  :
 * *********************File Modification History*********************
 * Date(mm-dd-yyyy) Developer Reason of Modification
 * ------------------------------------------------------------------- 
 * 10-26-2006       Abhijeetk   Message Forward Option Added.
 * 10-30-2006       Abhijeetk   If Time is negative then show '0'.
 *                              Show 8 digits for MRN Field.
 * 10-31-2006       Abhijeetk   Do not Show Forward button for Group Admin.
 * 11-18-2006       Swapnil K   Show Forward button for Group Admin, Provide link for Forwarded / Original Message in Activity log.
 * 04-01-2007       Indrajeet K Added Grid to show test result for the MessageID
 *                  Method Added: getMessageTestResults(), GetDummyDataForTestResult
 *                  Method Modified: Page_Load()
 * 02-21-2007       Swapnil K   Modified all functions for Veriphy Lab.
 * 02-22-2007       Swapnil K   Removed Readback functionality as it was not applicable in Veriphy Lab
 * 06-04-2007       IAK         Added filter for Result level Reactive NonReactive for Test results grid
 *                              Modified Functions: dgTestResults_ItemDataBound
 * 06-21-2007       IAK         Access rights for messageDetails grid  link
 *                              DOB Enhancement
 * 06-29-2007       IAK         Message Details: Reply link column changes
 * 07-02-2007       IAK         Defect Solved: 1219, 1233
 * 07-06-2007       IAK         Defect Solved: 1240, DOB/MRN Show logic change
 * 09-06-2007       SSK         Show the link for Primary / Backup message in the Activity Log for the message. 
 * 11-19-2007       IAK         Added Created Column in message detail grid
 * 01-28-2007       SBT         Added "Message Forward" Button and functionality.
 * 02-17-2008       IAK         Resend All Notification functionaliy
 * 02-17-2008       IAK         'All Notitfication' option in resend Notitfication combo need to be show only if devices are more than 1
 * 02-20-2008       IAK         Notitfication combo option 'Resend All Notification' Text replace with 'All' 
 * 02-20-2008       IAK         Mark as Received link url modified
 * 02-26-2008       IAK         Resend All Notification Functionality Change: Only the send messages to devices, to which previosly messages are sent, not all devices in combo
 * 02-26-2008       IAK         Resend All Notification : If no device available for resent All, show message.
 * 03-03-2008       IAK         Message Note: New column createdBySystem added
 * 03-06-2008       SSK         Message Forwarding: Show speaker icon in Message Note grid if the Forward reason is recorded through VUI.
 * 03-06-2008       IAK         Message Note: insertMessageNote() parameter NoteType passed
 * 13-03-2008       Suhas       Code review fixes
 * 19-03-2008       IAK         Forward button link to forward page thr javascript
 * 31-03-2008       IAK         Result level accept alphanumeric not only numeric
 * 04-04-2008       IAK         Defect 2981: Invalid Received By speaker icon appears in Msg Details screen
 * 25-04-2008       Suhas       Defect # 3061 - Alignment of VUI patient name speaker icon
 * 28-04-2008       Suhas       Defect # 3089 - Message Notes Speaker icon text
 * 08-05-2008       Suhas       Defect #2900 - Fixed.
 * 09-05-2008       Suhas       Defect #3115 - Speaker Icon in message note.
 * 22-05-2008       Suhas       Defect #3179 - Dataloss warning popup implementation.
 * 12 Jun 2008      Prerak      Migration of AJAX Atlas to AJAX RTM 1.0
 * 08-08-2008       IAK         FR: Show Year in date
 * 09-19-2008       Prerak      CR #232 Message Details Refresh 
 * 09-20-2008       Prerak      CodeReview Finding Fixed
 * 20-09-2008       Raju G      Modified Page to display popup on device link in Activity log section
 * 09-06-2008       Prerak      Avaoid adding duplicate resend notification devices 
 *                              at the time of autorefresh and adding notes
 * 09-30-2008       Prerak      Autorefresh appicable only for Activity log and Message detail
 * * 3 Oct 2008  - RajuG -      Display comment in Message Note grid for lab messages
 * 22-10-2008   RG        Modified to handle Non System Recipient
 * 10-23-08         RG          Modified to not diaplay link if NotificationText is not stored in Delivered Notification table
 * 11-04-2008       SNK         Modified getNotificationsAvailable method for not displaying SMSWebLink device for Radiology message
 * 11-05-2008       Suhas       Defect 4069 - Fixed
 * 11-11-2008       RG          Defect 4117 - Fixed
 * 11-13-2008       RG          Display Unprofiled Clinician (Non-system recipient) name with labelled as “Unprofiled Clinician”.
 * 11-19-2008       RG          Modified to display seconds also
 * 12-08-2008		SSK			#4271 - Considered ServerDate for exact outstanding time calculation
 * 01-08-2009       Suhas       TTP Defect 304 - Removed Manual Resend for Declined message is Fixed.
 * ------------------------------------------------------------------- */
#endregion

using System;
using System.Collections;
using System.Configuration;
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
using Vocada.Veriphy;
using DataAccess = Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using Vocada.CSTools.DataAccess;
using Vocada.VoiceLink.Utilities;
using System.Text;
using System.Web.UI.WebControls.WebParts;


namespace Vocada.CSTools
{
    /// <summary>
    /// Summary description for MessageDetails.
    /// Message Details contains complete details about selected Message and replies.
    /// Message Details contains information about Activity Log related to selected messages
    /// Message Details contains Message Notes for selected message.
    /// </summary>
    public partial class messageDetails : System.Web.UI.Page
    {
        #region Class Variables
        private DataTable dtMsgDetails = new DataTable();
        protected string strUserSettings = "NO";
        private int numAddRows = 0;
        private int numNotes = 0;
        private int messageID = 0;
        private int groupID = 0;
        private int timeZoneID = 4;
        private int isLabMessage = 0;
        private int departmentMessage = 0;
        private int activityWrappedRowCnt = 0;
        protected string ParaIsDeptMsg = "";
        protected string ParaIsLabMsg = "";
        #endregion Class Variables

        #region Constants
        private const string GROUP_ID = "GroupID";
        private const string TIMEZONE_ID = "TimeZoneID";
        private const string ESC_STAGE = "MessageEscStage";
        private const string DELIVERED_STATUS = "DELIVERED";
        private const string CONFIGKEY_FAXOUTPUTURL = "FaxOutputbaseURL";
        #endregion Constants


        #region Control's events and Public Methods
        /// <summary>
        /// Page Load function for Message Details gets information for selected messages
        /// Loads Activity History for selected message
        /// Notifications Events available for selected messages
        /// Message Notes avaialble for selected messages
        /// Sets dynamic height for datagrids used for Message Details,Activity Log,Message Note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (Request["IsDeptMsg"] != null)
                    ParaIsDeptMsg = Request["IsDeptMsg"].ToString();
                if (Request["IsLabMsg"] != null)
                    ParaIsLabMsg = Request["IsLabMsg"].ToString();
                StringBuilder sbScript = new StringBuilder();
                sbScript.Append("<script language=JavaScript>");
                sbScript.Append("var tbAuthorClientID = '" + tbAuthor.ClientID + "';");
                sbScript.Append("var textChangedClientID = '" + textChanged.ClientID + "';");
                sbScript.Append("var tbNoteClientID = '" + tbNote.ClientID + "';");
                sbScript.Append("</script>");
                this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());

                tbNote.Attributes.Add("onchange", "JavaScript:return CheckMaxLength('" + tbNote.ClientID + "','" + tbNote.MaxLength + "');");
                tbNote.Attributes.Add("onblur", "JavaScript:return CheckMaxLength('" + tbNote.ClientID + "','" + tbNote.MaxLength + "');");
                tbNote.Attributes.Add("onkeyup", "JavaScript:return CheckMaxLength('" + tbNote.ClientID + "','" + tbNote.MaxLength + "');");
                tbNote.Attributes.Add("onkeydown", "JavaScript:return CheckMaxLength('" + tbNote.ClientID + "','" + tbNote.MaxLength + "');");
                tbAuthor.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
                tbNote.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
                btnForward.Attributes.Add("onclick", "JavaScript:goForward('message_forward.aspx?MessageID=" + Request["MessageID"] + "&IsDeptMsg=" + Request["IsDeptMsg"] + "&IsLabMsg=" + Request["IsLabMsg"] + "');");
                //lnkBacktoMessage.PostBackUrl = "message_list.aspx";

                btnAddNote.Attributes.Add("onclick", "return checkNote();");

                //btnForward.Attributes.Add("onclick", "location.href= 'message_forward.aspx?MessageID=" + Request["MessageID"] + "&IsDeptMsg=" + Request["IsDeptMsg"] + "&IsLabMsg=" + Request["IsLabMsg"] + "';");            
                if (Session[SessionConstants.LOGGED_USER_ID] == null)
                    Response.Redirect(Vocada.CSTools.Utils.GetReturnURL("default.aspx", "message_center.aspx", this.Page.ClientQueryString));

                ///*ZNK - 2006/11/29 - User Seetings for View Text/Graphics*/
                //if (Session[SessionConstants.USER_SETTINGS] != null)
                //    strUserSettings = Session[SessionConstants.USER_SETTINGS].ToString();
                strUserSettings = "NO";
                if (Request["MessageID"] == null || Request["MessageID"].Length == 0 || Request["IsDeptMsg"] == null || Request["IsDeptMsg"].Length == 0 || Request["IsLabMsg"] == null || Request["IsLabMsg"].Length == 0)
                {
                    Response.Redirect(Vocada.CSTools.Utils.GetReturnURL("default.aspx", "message_center.aspx", this.Page.ClientQueryString));
                }
                else
                {
                    messageID = int.Parse(Request["MessageID"].ToString());
                    departmentMessage = int.Parse(Request["IsDeptMsg"].ToString());
                    isLabMessage = int.Parse(Request["IsLabMsg"].ToString());
                }
                if (string.Compare(strUserSettings, "YES") == 0)
                {
                    ((HyperLinkColumn)(dgMessage.Columns[8])).DataTextFormatString = "Play";
                    ((HyperLinkColumn)(dgMessage.Columns[12])).DataTextFormatString = "Play Message";
                }

                if (Session[SessionConstants.LOGGED_USER_NAME] != null)
                {
                    tbAuthor.Text = Session[SessionConstants.LOGGED_USER_NAME].ToString();
                }

                //validateUserAgainstMessage();
                getMessageDetails();
                if (!Page.IsPostBack)
                {
                    ViewState["dtMsgDetailsCount"] = 1;
                    ViewState["numAddRows"] = 1;
                    ViewState["dtTestResltCount"] = 1;
                    if (isLabMessage == 1)
                        getMessageTestResults();

                    getMessageActivityHistory();
                    getNotificationsAvailable();
                    getMessageNotes();
                    setdatagridHeight();
                    //UpdatePanelMessageList.Update();
                }
                else
                {
                    if (ViewState[GROUP_ID] != null)
                        groupID = int.Parse(ViewState[GROUP_ID].ToString());
                    else
                        groupID = 0;

                    if (ViewState[TIMEZONE_ID] != null)
                        timeZoneID = int.Parse(ViewState[TIMEZONE_ID].ToString());
                    else
                        timeZoneID = 0;

                }


                //if(roleID == (int)UserRole.Specialist || roleID == (int)UserRole.GroupAdministrator)
                if (isLabMessage == 0)
                {
                    hideColumnsForLab();
                }
                if (Request["returnTo"] != null && Request["returnTo"].Length > 0)
                {
                    switch (Request["returnTo"])
                    {
                        case "1":
                            hlnkBackTo.Text = "Back To Messages";
                            hlnkBackTo.NavigateUrl = "~/message_center.aspx";
                            break;
                        case "2":
                            hlnkBackTo.Text = "Back To Device Notification Errors";
                            hlnkBackTo.NavigateUrl = "~/device_notification_error.aspx?GroupID=" + groupID;
                            break;
                    }

                }

                Session[SessionConstants.CURRENT_TAB] = "MsgCenter";
                Session[SessionConstants.CURRENT_PAGE] = "message_details.aspx?MessageID=" + Request["MessageID"] + "IsDeptMsg" + Request["IsDeptMsg"] + "IsLabMsg" + Request["IsLabMsg"] + "&returnTo=" + Request["returnTo"];
                Session[SessionConstants.CURRENT_INNER_TAB] = "MessageDetails";
                setdatagridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.Page_Load:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                //if(Request["UpdateMessage"] != null)
                //    lblUpdateMessage.Text = Request["UpdateMessage"].ToString();
            }
        }

        /// <summary>
        /// This function is for Resending Notification
        /// This function insert Resend information into Notification history and Notes
        /// This function calls stored procedure "insertMessageNote"
        /// This function calls Notifier.ResendMessageNotification, which sends Notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResendNotification_Click(object sender, System.EventArgs e)
        {
            string message = "";
            string requestPara = this.Page.ClientQueryString;

            try
            {
                if (ddlNotifications.SelectedValue == "-1")
                    message = ResendAllNotifications();
                else
                    message = ResendNotification();
            }
            catch (Exception ex)
            {
                message = "Error while resending notification. Try again later.";
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.btnResendNotification_Click:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                ScriptManager.RegisterStartupScript(UpdatePanelMessageList,UpdatePanelMessageList.GetType(), "ResendNotification", "RefreshPage('" + requestPara + "','" + message + "');", true);
            }
        }

        /// <summary>
        /// Resend Selected Notification 
        /// </summary>
        /// <returns></returns>
        private string ResendNotification()
        {
            string message = "";
            string sUrl = "";
            string requestPara = this.Page.ClientQueryString;
            MessageDetails objMessageDetails = null;
            NotifierServiceProxy notifierProxy = null;
            try
            {
                sUrl = System.Configuration.ConfigurationManager.AppSettings["VoiceLinkR2.com.vocada.voicelink1.Reference"];
                int resendMessageID = int.Parse(Request["MessageID"].ToString());
                int notificationID = int.Parse(ddlNotifications.SelectedItem.Value);
                notifierProxy = new NotifierServiceProxy(sUrl);
                int notify;
                if (Request["IsLabMsg"] == "0")
                {
                    if (Request["IsDeptMsg"] == "1")
                        notify = notifierProxy.ResendDepartmentMessageNotification(resendMessageID, notificationID);
                    else
                        notify = notifierProxy.ResendMessageNotification(resendMessageID, notificationID);
                }
                else
                    notify = notifierProxy.ResendLabMessageNotification(resendMessageID, notificationID);

                //Suucessfully sent msg
                if (notify == 0)
                {
                    string author = tbAuthor.Text;
                    string note = "Author Initiated a Manual Resend @ " + DateTime.Now.ToShortTimeString();
                    objMessageDetails = new MessageDetails();
                    int result = objMessageDetails.InsertMessageNote(messageID, departmentMessage, author, note, groupID, isLabMessage, MessageNoteType.Default);

                    message = "Notification sent successfully.";
                    objMessageDetails = null;
                }
                else
                {
                    message = "Error while resending notification. Try again later.";
                }
                return message;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.ResendNotification:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                message = "Error while resending notification. Try again later.";
                return message;
            }
            finally
            {
                objMessageDetails = null;
                notifierProxy = null;
            }
        }


        /// <summary>
        /// Resend All Notifications listed in device combo box. 
        /// </summary>
        /// <returns></returns>
        private string ResendAllNotifications()
        {
            string message = "";
            string requestPara = this.Page.ClientQueryString;
            string errorMessage = "";
            int notify = 0;
            string sUrl = "";
            int messageID = 0;
            bool isLabMessage = false;
            bool isDeptMessage = false;
            int[] notificationList = null;
            int[] notificationSendList = null;
            MessageDetails objMessageDetails = null;
            NotifierServiceProxy notifierProxy = null;
            try
            {
                sUrl = System.Configuration.ConfigurationManager.AppSettings["VoiceLinkR2.com.vocada.voicelink1.Reference"];
                messageID = int.Parse(Request["MessageID"].ToString());
                isLabMessage = Request["IsLabMsg"] == "0" ? false : true;
                isDeptMessage = Request["IsDeptMsg"] == "0" ? false : true;
                notificationList = new int[ddlNotifications.Items.Count];
                notifierProxy = new NotifierServiceProxy(sUrl);
                int notificationDeviceCount = 0;
                int totalDevices = ddlNotifications.Items.Count;
                int escStage = int.Parse(ViewState[ESC_STAGE].ToString());
                //Load all notification 
                for (int currentDevice = 0; currentDevice < totalDevices; currentDevice++)
                {
                    if (ddlNotifications.Items[currentDevice].Value != "-1")
                    {
                        if (
                            (escStage == 1 && ddlNotifications.Items[currentDevice].Text.StartsWith("Primary - ", StringComparison.OrdinalIgnoreCase)) ||
                            (escStage == 2 && (ddlNotifications.Items[currentDevice].Text.StartsWith("Primary - ", StringComparison.OrdinalIgnoreCase) || ddlNotifications.Items[currentDevice].Text.StartsWith("Backup - ", StringComparison.OrdinalIgnoreCase))) ||
                            (escStage == 3 && (ddlNotifications.Items[currentDevice].Text.StartsWith("Primary - ", StringComparison.OrdinalIgnoreCase) || ddlNotifications.Items[currentDevice].Text.StartsWith("Backup - ", StringComparison.OrdinalIgnoreCase) || ddlNotifications.Items[currentDevice].Text.StartsWith("Fail-Safe - ", StringComparison.OrdinalIgnoreCase) || ddlNotifications.Items[currentDevice].Text.StartsWith("FailSafe - ", StringComparison.OrdinalIgnoreCase)))
                            )
                        {
                            notificationList[currentDevice] = int.Parse(ddlNotifications.Items[currentDevice].Value);
                            notificationDeviceCount++;
                        }
                    }
                }

                if (notificationDeviceCount > 0)
                {
                    int currentIndex = 0;
                    notificationSendList = new int[notificationDeviceCount];
                    for (int deviceNum = 0; deviceNum < notificationList.Length; deviceNum++)
                    {
                        if (notificationList[deviceNum] != 0)
                        {
                            notificationSendList[currentIndex] = notificationList[deviceNum];
                            currentIndex++;
                        }
                    }
                }

                if (notificationSendList == null)
                    message = "No notifications are sent.";

                notify = notifierProxy.ResendAllNotifications(messageID, notificationSendList, isLabMessage, isDeptMessage, out errorMessage);
                if (notify == 0)
                {
                    string author = tbAuthor.Text;
                    string note = "Author Initiated a Manual Resend to All @ " + DateTime.Now.ToShortTimeString();
                    objMessageDetails = new MessageDetails();
                    if (isLabMessage)
                        objMessageDetails.InsertMessageNote(messageID, departmentMessage, author, note, groupID, 1, MessageNoteType.Default);
                    else
                        objMessageDetails.InsertMessageNote(messageID, departmentMessage, author, note, groupID, 0, MessageNoteType.Default);

                    message = "Notifications sent successfully.";
                }
                else
                {
                    int notificationValue;
                    if (errorMessage.Trim().Length > 0)
                    {
                        char[] split = { ',' };
                        string[] notificationErrorList = errorMessage.Split(split);

                        for (int notitcationCnt = 0; notitcationCnt < notificationList.Length; notitcationCnt++)
                        {
                            if (notificationErrorList[notitcationCnt].Length > 0)
                            {
                                if (message.Length == 0)
                                    message = "Error while resending notifications for following devices.\\n";
                                message += ddlNotifications.Items.FindByValue(notificationErrorList[notitcationCnt]).Text + "\\n";
                            }
                        }
                        if (message.Length > 0)
                        {
                            message += "Try again later.\\n";
                        }
                    }

                }
                return message;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.ResendAllNotification:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                message = "Error while resending notifications. Try again later.";
                return message;
            }
            finally
            {
                objMessageDetails = null;
                notifierProxy = null;
                notificationList = null;
            }

        }


        /// <summary>
        /// This function insert Note into datagrid for Message Note
        /// This function calls stored procedure "insertMessageNote"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddNote_Click(object sender, System.EventArgs e)
        {
            try
            {
                string author = tbAuthor.Text;
                string note = tbNote.Text;
                textChanged.Value = "false";
                MessageDetails objMessageDetails = new MessageDetails();
                //int roleID = int.Parse(Session[SessionConstants.ROLE_ID].ToString());
                int result = objMessageDetails.InsertMessageNote(messageID, departmentMessage, author, note, groupID, isLabMessage, MessageNoteType.Default);
                objMessageDetails = null;
                // Zeeshan : Atlas Implementation (Note : Commented Response.Redirect below)
                tbNote.Text = string.Empty;
                getMessageNotes();
                setdatagridHeight();
                //UpdatePanelMessageList.Update();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.btnAddNote_Click:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
            }
        }
        /// <summary>
        /// This function Refersh the activity log and message details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnRefresh_Click(object sender, System.EventArgs e)
        {
            try
            {
                getMessageDetails();
                getMessageActivityHistory();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.lbtnRefresh_Click:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This function is to set dynamic height of datagrid of Message Details, Activity Log and Message Note
        /// </summary>
        private void setdatagridHeight()
        {
            //Set height for Message Details grid
            int nheight = 50;
            int totalRows = Convert.ToInt16(ViewState["dtMsgDetailsCount"].ToString()) + Convert.ToInt16(ViewState["numAddRows"].ToString());

            //int totalRows = dtMsgDetails.Rows.Count + numAddRows;
            //If no. of records in this grid are less than 5
            if (totalRows < 5)
            {
                if (totalRows == 0)
                {
                    nheight = 25;
                }
                else
                {
                    nheight = (totalRows + 2) * 25;
                }
            }
            else
            {
                nheight = 5 * 25;
            }

            //Set height for Message Notes Grid 
            int nNotesHeight = 26;

            //If no. of records in this grid are less than 5
            //nNotesHeight = (numNotes * 21) + 25;
            nNotesHeight = (Convert.ToInt16(ViewState["dtTestResltCount"].ToString()) * 21) + 25;

            //Set height for Test Results Grid             
            int nTestHeight = 26;

            //If no. of records in this grid are less than 5
            if (dgTestResults.Items.Count > 0)
            {
                nTestHeight = dgTestResults.Items.Count * 21 + 25;
            }

            int nActivityLog = 26;
            if (dgActivityLog.Items.Count > 0)
            {
                nActivityLog = dgActivityLog.Items.Count * 21 + 25;

                for (int rowCnt = 0; rowCnt < activityWrappedRowCnt; rowCnt++)
                    nActivityLog = nActivityLog + 15;

            }

            string uId = this.UniqueID;
            string newUid = uId.Replace(":", "_");
            StringBuilder acScript = new StringBuilder();
            acScript.Append("<script type=\"text/javascript\">");
            acScript.AppendFormat("document.getElementById(" + '"' + "MessageDiv" + '"' + ").style.height='" + (nheight + 4) + "';");
            acScript.AppendFormat("document.getElementById(" + '"' + "divNotes" + '"' + ").style.height='" + (nNotesHeight + 1) + "';");
            //acScript.AppendFormat("document.getElementById(" + '"' + "ActivityDiv" + '"' + ").style.height='" + (nActivityLog + 1) + "';");

            if (isLabMessage == 1)
            {
                acScript.AppendFormat("document.getElementById(" + '"' + TestResultDiv.ClientID + '"' + ").style.height='" + (nTestHeight + 1) + "';");
                int dgTestResultsHeight = dgTestResults.Items.Count * 21;
            }
            acScript.Append("</script>");
            ScriptManager.RegisterStartupScript(UpdatePanelMessageList,UpdatePanelMessageList.GetType(), newUid, acScript.ToString(), false);

        }

        /// <summary>
        /// This function get Message information about selected message
        /// This function calls stored procedure "VOC_VLR_GetMessageDetails"
        /// This function binds data into datagrid dgMessage
        /// </summary>
        /// <param name="cnx"></param>
        /// <param name="messageID"></param>
        private void getMessageDetails()
        {
            MessageDetails objLabDetails = null;
            try
            {
                objLabDetails = new MessageDetails();
                dtMsgDetails = objLabDetails.GetMessageDetails(messageID, departmentMessage, isLabMessage);
                ViewState["dtMsgDetailsCount"] = dtMsgDetails.Rows.Count;
                if (dtMsgDetails.Rows.Count > 0)
                {
                    groupID = int.Parse(dtMsgDetails.Rows[0]["GroupID"].ToString());
                    timeZoneID = int.Parse(dtMsgDetails.Rows[0]["GroupTimeZoneID"].ToString());
                    ViewState[GROUP_ID] = groupID;
                    ViewState[TIMEZONE_ID] = timeZoneID;
                    ViewState[ESC_STAGE] = int.Parse(dtMsgDetails.Rows[0]["MessageEscStage"].ToString());
                }
                addRowsForForwardedMessages(messageID, ref dtMsgDetails, groupID);
                dgMessage.DataSource = dtMsgDetails.DefaultView;
                dgMessage.DataBind();
                //setAccessMsgDetails();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.getMessageDetails:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                objLabDetails = null;
            }
        }

        /// <summary>
        /// This method hides or changes the title of column for radiology.
        /// </summary>
        private void hideColumnsForLab()
        {
            dgMessage.Columns[6].Visible = false;
            LabTestDiv.Visible = false;
        }

        /// <summary>
        /// This function get Message information about selected message
        /// This function calls stored procedure "VOC_VLR_GetMessageDetails"
        /// This function binds data into datagrid dgMessage
        /// </summary>
        /// <param name="cnx"></param>
        /// <param name="messageID"></param>
        private void addRowsForForwardedMessages(int messageID, ref DataTable dtMsgDetails, int groupID)
        {
            MessageDetails objMessageDetails = new MessageDetails();
            DataTableReader reader = null;
            try
            {
                DataTable dtFwdMessages = objMessageDetails.GetForwardedMessages(messageID, departmentMessage, isLabMessage);
                reader = dtFwdMessages.CreateDataReader();
                while (reader.Read())
                {
                    DataRow dgRow = dtMsgDetails.NewRow();
                    dgRow["MessageID"] = reader["FMessageID"];
                    dgRow["CreatedOn_UsersTime"] = ((DateTime)reader["ForwardCreatedOn_UsersTime"]);
                    dgRow["GroupName"] = dtMsgDetails.Rows[0].ItemArray[2];
                    dgRow["IsDepartmentMessage"] = departmentMessage;
                    dgRow["Accession"] = reader["Accession"];
                    dgRow[3] = 0;
                    dgRow[4] = 0;
                    dgRow[5] = 0;
                    dgRow[10] = 0;
                    dgRow["CreatedOn"] = DateTime.MinValue;
                    dgRow["LastEscalationNotifyAt"] = DateTime.MinValue;
                    dgRow["BackupNotifyStarted"] = false;
                    dgRow[16] = 0;
                    dgRow[17] = 0;
                    dgRow[18] = 0;
                    dgRow[22] = 0;
                    dgRow["FindingID"] = 0;
                    dgRow["FindingDescription"] = "";
                    StringBuilder sbOrderingClinician = new StringBuilder();
                    sbOrderingClinician.Append("Forwarded to ");
                    sbOrderingClinician.Append(reader["ForwardedTo"].ToString());
                    sbOrderingClinician.Append(" by ");
                    sbOrderingClinician.Append(reader["ForwardedBY"].ToString());
                    dgRow["RecipientDisplayName"] = sbOrderingClinician.ToString();
                    dgRow["RecipientID"] = 0;
                    dgRow["MessageReplyID"] = 0;
                    dgRow["RoomBedID"] = 0;
                    dgRow["IsDocumented"] = 0;
                    dgRow["RequireReadback"] = false;
                    dgRow["PatientVoiceURL"] = ""; //patient name
                    dgRow["MessageCreatedBySystem"] = "";
                    if (dtMsgDetails.Columns.Contains("DirectoryID"))
                        dgRow["DirectoryID"] = 0;
                    dgRow["ReplyCreatedOn_Server"] = System.DBNull.Value;
                    if (dtMsgDetails.Columns.Contains("ReadbackCreatedOn_Server"))
                        dgRow["ReadbackCreatedOn_Server"] = System.DBNull.Value;
                    dgRow["AgentCreatedOn"] = DateTime.MinValue;
                    dgRow["CurrentDate"] = DateTime.Now;
                    if (reader.IsDBNull(reader.GetOrdinal("ForwardReadOn_UsersTime")))
                    {
                        DateTime createdOn = (DateTime)reader["ForwardCreatedOn_UsersTime"];
                        DateTime calCreatedOn = new DateTime(createdOn.Year, createdOn.Month, createdOn.Day, createdOn.Hour, createdOn.Minute, createdOn.Second);
                        DateTime timeZoneNow = Utils.GetUsersTimeOnTimeZone(timeZoneID, DateTime.Now);
                        DateTime calTimeZoneNow = new DateTime(timeZoneNow.Year, timeZoneNow.Month, timeZoneNow.Day, timeZoneNow.Hour, timeZoneNow.Minute, timeZoneNow.Second);

                        TimeSpan diff = calTimeZoneNow.Subtract(calCreatedOn);
      
                        int days = 0;
                        int hours = 0;
                        int minutes = 0;
                        int seconds = 0;

                        if (diff.Days > 0)
                            days = diff.Days;
                        if (diff.Hours > 0)
                            hours = diff.Hours;
                        if (diff.Minutes > 0)
                            minutes = diff.Minutes;
                        if (diff.Seconds > 0)
                            seconds = diff.Seconds;

                        if (seconds < 0)
                            seconds = 0;
                        if (minutes < 0)
                            minutes = 0;
                        if (hours < 0)
                            hours = 0;
                        if (days < 0)
                            days = 0;
                        
                        dgRow["SpecialistAffiliation"] = "Open: " + days + "d " + hours + "h " + minutes + "m " + seconds + "s ";

                    }  // end message not read...

                        // Message Read 
                    else
                    {
                        dgRow["ReadOn"] = (DateTime)reader["ForwardReadOn_UsersTime"];
                        dgRow["ReadOn_UsersTime"] = (DateTime)reader["ForwardReadOn_UsersTime"];
                        dgRow["ReadBy"] = reader["ReadBy"];
                        dgRow["ReadComment"] = reader["ReadComment"];
                        dgRow["SpecialistAffiliation"] = "Received " + ((DateTime)reader["ForwardReadOn_UsersTime"]).ToString("MM/dd/yyyy hh:mm:sstt");

                    }
                    dgRow["RoleID"] = 0;
                    dgRow["directoryID"] = 0;
                    if (dtMsgDetails.Columns.Contains("MessageEscStage"))
                        dgRow["MessageEscStage"] = 1;

                    dtMsgDetails.Rows.Add(dgRow);
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.addRowsForForwardedMessages:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                objMessageDetails = null;
            }
        }

        /// <summary>
        /// This function gets Notification Events related to selected Message
        /// This function calls stored procedure "getNotificationsForRPResend"
        /// </summary>
        private void getNotificationsAvailable()
        {
            try
            {
                MessageDetails objMessageDetails = new MessageDetails();
                ArrayList alNotif = objMessageDetails.GetNotificationsAvailable(messageID, departmentMessage, isLabMessage, groupID);

                foreach (LabUnitObject objNotif in alNotif)
                {
                    ListItem liNotif = new ListItem();
                    if (!(ParaIsLabMsg == "0" && objNotif.FieldName.Contains("SMSWebLink")))
                    {
                        liNotif.Text = objNotif.FieldName;
                        liNotif.Value = objNotif.FieldID.ToString();
                        ddlNotifications.Items.Add(liNotif);
                    }
                }

                if (ddlNotifications.Items.Count < 1)
                {
                    btnResendNotification.Enabled = false;
                }
                else
                {
                    btnResendNotification.Enabled = true;

                    if (ddlNotifications.Items.Count > 1)
                    {
                        ListItem lstResendAll = new ListItem();
                        lstResendAll.Text = "All";
                        lstResendAll.Value = "-1";
                        ddlNotifications.Items.Add(lstResendAll);
                    }
                }
                objMessageDetails = null;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.getNotificationsAvailable:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        /// <summary>
        /// This function is get all information about Message Notes history for selected Messages
        /// This function calls stored procedure "getMessageNotes"
        /// </summary>
        /// <param name="cnx"></param>
        /// <param name="messageID"></param>
        private void getMessageNotes()
        {
            try
            {
                MessageDetails objMessageDetails = new MessageDetails();
                DataTable dtNotes = objMessageDetails.GetMessageNotes(messageID, departmentMessage, isLabMessage);
                dgMessageNotes.DataSource = dtNotes.DefaultView;
                dgMessageNotes.DataBind();
                objMessageDetails = null;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.getMessageNotes:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;

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
            this.dgMessage.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgMessage_ItemDataBound);
            this.dgActivityLog.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgActivityLog_ItemDataBound);
            this.dgMessageNotes.ItemDataBound += new DataGridItemEventHandler(dgMessageNotes_ItemDataBound);
        }
        /// <summary>
        /// This function calls DataBind() dynamically for datagrid dgMessageNotes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgMessageNotes_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    numNotes++;
                    ViewState["dtTestResltCount"] = numNotes;
                    DataRowView data = e.Item.DataItem as DataRowView;
                    if (data["Note"] != System.DBNull.Value)
                    {
                        string note = (string)data["Note"];

                        if (note.StartsWith("http") && note.EndsWith(".wav"))
                        {
                            string fwdMessageText = "Reason for Forward";
                            if(isLabMessage == 1)
                                fwdMessageText = "Comment for Forward";
                            if (string.Compare(strUserSettings, "YES") == 0)
                                e.Item.Cells[3].Text = "<a href=" + note + ">Play</a>&nbsp;&nbsp;" + fwdMessageText;
                            else
                                e.Item.Cells[3].Text = "<a href=" + note + "><img src=img/ic_play_msg.gif border=0></a>&nbsp;&nbsp;" + fwdMessageText;
                        }
                        else
                            e.Item.Cells[3].Text = data["Note"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.dgMessageNotes_ItemDataBound:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        protected void dgTestResults_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
            {
                DataRowView data = e.Item.DataItem as DataRowView;

                if (string.Compare(strUserSettings, "YES") == 0)
                {
                    if (data["NewFinding"] != System.DBNull.Value && Convert.ToBoolean(data["NewFinding"]))
                    {
                        e.Item.Cells[3].Text = "Yes";
                    }
                }
                else
                {
                    if (data["NewFinding"] != System.DBNull.Value && Convert.ToBoolean(data["NewFinding"]))
                    {
                        e.Item.Cells[3].Text = "<image src=img/true.gif border=0>";
                    }
                }

                if (data["ResultTypeID"] != System.DBNull.Value && Convert.ToInt32(data["ResultTypeID"]) == 2)
                {
                    if (data["ResultLevel"] != System.DBNull.Value && data["ResultLevel"].ToString() == "1")
                        e.Item.Cells[1].Text = "Positive";
                    else
                        e.Item.Cells[1].Text = "Negative";
                }
                else if (data["ResultTypeID"] != System.DBNull.Value && Convert.ToInt32(data["ResultTypeID"]) == 4)
                {
                    if (data["ResultLevel"] != System.DBNull.Value && data["ResultLevel"].ToString() == "1")
                        e.Item.Cells[1].Text = "Reactive";
                    else
                        e.Item.Cells[1].Text = "Non-Reactive";
                }
                else if (data["ResultTypeID"] != System.DBNull.Value && Convert.ToInt32(data["ResultTypeID"]) == 1)
                {
                    if (data["MeasurementName"] != System.DBNull.Value && data["MeasurementName"].ToString().Length > 0)
                        e.Item.Cells[1].Text = data["ResultLevel"].ToString() + " (" + data["MeasurementName"].ToString() + ")";
                    else
                        e.Item.Cells[1].Text = data["ResultLevel"].ToString();
                }
                else if (data["ResultTypeID"] != System.DBNull.Value && Convert.ToInt32(data["ResultTypeID"]) == 3 && data["ResultLevel"] != System.DBNull.Value && data["ResultLevel"].ToString() == "0")
                    e.Item.Cells[1].Text = " ";


            }
        }
        #endregion

        /// <summary>
        /// This function calls DataBind() dynamically for datagrid dgMessage
        /// This function checks for Reply Messages if any and add it to Message list
        /// This function sets alternate items color same for reply messages if any
        /// This function wrap the columns Note, Status and MRN if string is too long
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgMessage_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
            try
            {
            
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView data = e.Item.DataItem as DataRowView;

                    bool forwarded = false;                    
                    bool isCreatedByAgent = false;

                    int recepientType = -1;
                    int maxDataDisplayLenght = 10;

                    (e.Item.Cells[14].Controls[0] as HyperLink).NavigateUrl = "./mark_message_received.aspx?MessageID=" + data["MessageID"].ToString() + "&IsDeptMsg=" + data["IsDepartmentMessage"].ToString() + "&IsLabMsg=" + isLabMessage.ToString() + "&GroupID=" + data["GroupID"].ToString() + "&returnTo=" + Request["returnTo"];
                    (e.Item.Cells[15].Controls[0] as HyperLink).NavigateUrl = "./mark_message_received.aspx?MessageID=" + Request["MessageID"].ToString() + "&MessageReplyID=" + data["MessageID"].ToString() + "&IsDeptMsg=" + data["IsDepartmentMessage"].ToString() + "&IsLabMsg=" + isLabMessage.ToString() + "&GroupID=" + data["GroupID"].ToString() + "&returnTo=" + Request["returnTo"];

                    if (data["Accession"] != System.DBNull.Value)
                    {
                        if (data["Accession"].ToString().Length > maxDataDisplayLenght)
                        {
                            e.Item.Cells[9].Text = data["Accession"].ToString().Substring(0, maxDataDisplayLenght) + "...";
                            e.Item.Cells[9].ToolTip = data["Accession"].ToString();
                        }
                        else
                        {
                            e.Item.Cells[9].Text = data["Accession"].ToString();
                            e.Item.Cells[9].ToolTip = data["Accession"].ToString();
                        }
                    }

                    if (data["SpecialistDisplayName"] != null)
                    {
                        string createdByName = data["SpecialistDisplayName"].ToString();
                        int agentIndex = createdByName.IndexOf("Agent Name:");
                        if (agentIndex > 0)
                        {
                            StringBuilder sbSpecialistName = new StringBuilder();
                            sbSpecialistName.Append(createdByName.Substring(0, agentIndex));
                            sbSpecialistName.Append("<br>");
                            sbSpecialistName.Append("<br>");
                            sbSpecialistName.Append(createdByName.Substring(agentIndex, createdByName.Length - agentIndex));                            
                            createdByName = sbSpecialistName.ToString();
                            isCreatedByAgent = true;
                            e.Item.Cells[4].Text = e.Item.Cells[4].Text + "<br><br>" + String.Format("{0:MM/dd/yyyy hh:mm:sstt} ", Convert.ToDateTime(data["AgentCreatedOn_UsersTime"]));
                        }

                        e.Item.Cells[3].Text = createdByName.Trim();
                    }

                    if (data["RecipientTypeID"] != null)
                        recepientType = Convert.ToInt32(data["RecipientTypeID"]);

                    // ReferringPhysician name / link display
                    if (data[12].ToString().Trim().StartsWith("Forwarded"))
                        forwarded = true;

                    bool isDocumented = false;
                    if (data["IsDocumented"] != System.DBNull.Value)
                        isDocumented = Convert.ToBoolean(data["IsDocumented"]);

                    // If message is not forwarded then only show the name Of Ordering Clinician with hyperlink.
                    if (!forwarded)
                    {
                        StringBuilder sbOrderingClinician = new StringBuilder();
                        int recipientID = Convert.ToInt32(data["RecipientID"]);
                        if (recepientType == (int)MessageInfo.RecepientType.BedNumber && data["NurseID"] != System.DBNull.Value)
                        {
                            //sbOrderingClinician.Append("<a href='./edit_nurse.aspx?NurseID=");
                        }
                        else if (recepientType == (int)MessageInfo.RecepientType.UnitName || recepientType == (int)MessageInfo.RecepientType.BedNumber)
                        {
                            //sbOrderingClinician.Append("<a href='./unit_device_preference.aspx?unitProfileID=");
                        }
                        else if (recepientType == (int)MessageInfo.RecepientType.Department)
                        {
                            //sbOrderingClinician.Append("<a href='./dept_devices.aspx?departmentID=");
                        }
                        else
                        {
                           
                            if (userInfo.RoleId != UserRoles.SupportLevel1.GetHashCode() && recipientID > 0)
                            {
                                sbOrderingClinician.Append("<a href='./edit_oc.aspx?ReferringPhysicianID=");
                                sbOrderingClinician.Append(data["RecipientID"].ToString());
                                sbOrderingClinician.Append("' onclick=");
                                sbOrderingClinician.Append('"');
                                sbOrderingClinician.Append("if(window.parent.frames[0].document.getElementById('Directory') != null) { window.parent.frames[0].document.getElementById('Directory').click();} if(window.parent.frames[0].document.getElementById('DownloadBnW') != null) { window.parent.frames[0].document.getElementById('DownloadBnW').click();}");
                                sbOrderingClinician.Append('"');
                                sbOrderingClinician.Append(">");
                            }

                        }
                        string recipientName = data["RecipientDisplayName"].ToString();                        
                        if (recipientName.Length > 25)
                        {
                            recipientName = recipientName.Substring(0, 25) + "...";
                        }
                        recipientName = (recipientID <= 0) ? "Unprofiled Clinician: " + recipientName : recipientName;
                        sbOrderingClinician.Append(recipientName);

                        if (recepientType == (int)MessageInfo.RecepientType.BedNumber && int.Parse(data["NurseID"].ToString()) != 0)
                        {
                            sbOrderingClinician.Append(" (Nurse)");
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

                        sbOrderingClinician.Append("PassCode To Access: ");
                        sbOrderingClinician.Append(data["PassCode1"]);
                        if (data["CallInNumber"] != DBNull.Value && data["CallInNumber"].ToString() != "")
                        {
                            numAddRows++;
                            ViewState["numAddRows"] = numAddRows;
                            sbOrderingClinician.Append("<br>");
                            sbOrderingClinician.Append("<br>");
                            sbOrderingClinician.Append("Call in # " + Utils.expandPhoneNumber(data["CallInNumber"].ToString()));
                        }

                        /*ZNK - 08-12-2006 : Change for conditionally checking for redirecting to Directory Tab */
                        e.Item.Cells[5].Text = sbOrderingClinician.ToString();
                        if (data["RecipientDisplayName"].ToString().Length > 25)
                            e.Item.Cells[5].ToolTip = data["RecipientDisplayName"].ToString();
                    }

                    if (data["ImpressionVoiceURL"] == System.DBNull.Value || data["ImpressionVoiceURL"].ToString().Length == 0)
                    {
                        e.Item.Cells[12].Text = "";
                    }
                    int replyID = 0;

                    if ((!forwarded) && data["MessageReplyID"] != System.DBNull.Value) // To check original message with reply
                    {
                        replyID = Convert.ToInt32(data["MessageReplyID"]);
                    }

                    // Patient VoiceURL Display
                    if (data["PatientVoiceURL"] != System.DBNull.Value)
                    {
                        string patient = (string)data["PatientVoiceURL"];
                        if (patient.StartsWith("http") || patient.StartsWith("https"))
                        {
                            if (string.Compare(strUserSettings, "YES") == 0)
                                e.Item.Cells[8].Text = "<a href=" + patient + ">Play</a>";
                            else
                                e.Item.Cells[8].Text = "<center><a href=" + patient + "><img src=img/ic_play_msg.gif border=0></a></center>";
                        }
                        else  // patient name, not a patient recording...
                        {
                            if (patient.Length > 25)
                            {
                                patient = patient.Substring(0, 25) + "...";
                                e.Item.Cells[8].ToolTip = (string)data["PatientVoiceURL"];
                            }
                            e.Item.Cells[8].Text = patient;
                        }
                    }
                    if (e.Item.ItemType != ListItemType.AlternatingItem)
                    {
                        if (data["DOB"].ToString() != "")
                        {
                            e.Item.Cells[11].Text = Convert.ToDateTime(data["DOB"].ToString()).ToShortDateString();
                            e.Item.Cells[11].ToolTip = Convert.ToDateTime(data["DOB"].ToString()).ToShortDateString();
                        }
                        else
                        {
                            e.Item.Cells[11].Text = "";
                            dgMessage.Columns[11].Visible = false;
                        }
                    }
                    if (e.Item.ItemType != ListItemType.AlternatingItem)
                    {
                        // MRN Display
                        if (data["MRN"] != System.DBNull.Value)
                        {
                            string strMRN = data["MRN"].ToString();
                            if (strMRN.Length > 0)
                            {
                                if (strMRN.Contains("http") || strMRN.Contains("HTTP"))
                                {
                                    StringBuilder sbMRNUrl = new StringBuilder();
                                    sbMRNUrl.Append("<a href=");
                                    sbMRNUrl.Append(strMRN);
                                    sbMRNUrl.Append("><img src=img/ic_play_msg.gif border=0></a>");
                                    e.Item.Cells[10].Text = sbMRNUrl.ToString();
                                }
                                else
                                {
                                    e.Item.Cells[10].Text = (strMRN.Length > 9) ? strMRN.Substring(0, 8) + ".." : strMRN;
                                    e.Item.Cells[10].ToolTip = strMRN;
                                }
                            }
                            else
                            {
                                dgMessage.Columns[10].Visible = false;
                            }
                        }
                    }
                    int days = 0;
                    int hours = 0;
                    int minutes = 0;
                    int seconds = 0;

                    // Message Not Read 
                    if (data["ReadOn"] == System.DBNull.Value)
                    {
                        StringBuilder sbStatus = new StringBuilder();

                        if (string.Compare(strUserSettings, "YES") == 0)
                            e.Item.Cells[0].Text = "";
                        else
                            e.Item.Cells[0].Text = "<img src=img/ic_ballon_green.gif border=0>";

                        DateTime createdOn = (DateTime)data["CreatedOn"];
                        DateTime calCreatedOn = new DateTime(createdOn.Year, createdOn.Month, createdOn.Day, createdOn.Hour, createdOn.Minute, createdOn.Second);
                        //DateTime timeZoneNow = Utils.GetUsersTimeOnTimeZone(timeZoneID, DateTime.Now);
                        //DateTime calTimeZoneNow = new DateTime(timeZoneNow.Year, timeZoneNow.Month, timeZoneNow.Day, timeZoneNow.Hour, timeZoneNow.Minute, timeZoneNow.Second);
                        DateTime currentDate = (DateTime)data["CurrentDate"];
                        DateTime calTimeZoneNow = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, currentDate.Second);

                        //Check for With Agent timings
                        if (isCreatedByAgent)
                        {
                            DateTime agentCreatedOn = (DateTime)data["AgentCreatedOn"];
                            DateTime calAgentCreatedOn = new DateTime(agentCreatedOn.Year, agentCreatedOn.Month, agentCreatedOn.Day, agentCreatedOn.Hour, agentCreatedOn.Minute, agentCreatedOn.Second);

                            TimeSpan withLASpan = calAgentCreatedOn.Subtract(calCreatedOn);

                            if (withLASpan.Days > 0)
                                days = withLASpan.Days;
                            if (withLASpan.Hours > 0)
                                hours = withLASpan.Hours;
                            if (withLASpan.Minutes > 0)
                                minutes = withLASpan.Minutes;
                            if (withLASpan.Seconds > 0)
                                seconds = withLASpan.Seconds;

                            if (seconds < 0)
                                seconds = 0;
                            if (minutes < 0)
                                minutes = 0;
                            if (hours < 0)
                                hours = 0;
                            if (days < 0)
                                days = 0;

                            sbStatus.Append("With Agent: ");
                            sbStatus.Append(days);
                            sbStatus.Append("d ");
                            sbStatus.Append(hours);
                            sbStatus.Append("h ");
                            sbStatus.Append(minutes);
                            sbStatus.Append("m ");
                            sbStatus.Append(seconds);
                            sbStatus.Append("s ");
                            sbStatus.Append("<br /><br />");

                            days = hours = minutes = seconds = 0;

                            TimeSpan InVeriphySpan = calTimeZoneNow.Subtract(calAgentCreatedOn);

                            if (InVeriphySpan.Days > 0)
                                days = InVeriphySpan.Days;
                            if (InVeriphySpan.Hours > 0)
                                hours = InVeriphySpan.Hours;
                            if (InVeriphySpan.Minutes > 0)
                                minutes = InVeriphySpan.Minutes;
                            if (InVeriphySpan.Seconds > 0)
                                seconds = InVeriphySpan.Seconds;

                            if (seconds < 0)
                                seconds = 0;
                            if (minutes < 0)
                                minutes = 0;
                            if (hours < 0)
                                hours = 0;
                            if (days < 0)
                                days = 0;

                            sbStatus.Append("In Veriphy: ");
                            sbStatus.Append(days);
                            sbStatus.Append("d ");
                            sbStatus.Append(hours);
                            sbStatus.Append("h ");
                            sbStatus.Append(minutes);
                            sbStatus.Append("m ");
                            sbStatus.Append(seconds);
                            sbStatus.Append("s ");
                            sbStatus.Append("<br /><br />");

                            days = hours = minutes = seconds = 0;
                        }

                        TimeSpan diff = calTimeZoneNow.Subtract(calCreatedOn);

                        if (diff.Days > 0)
                            days = diff.Days;
                        if (diff.Hours > 0)
                            hours = diff.Hours;
                        if (diff.Minutes > 0)
                            minutes = diff.Minutes;
                        if (diff.Seconds > 0)
                            seconds = diff.Seconds;

                        if (seconds < 0)
                            seconds = 0;
                        if (minutes < 0)
                            minutes = 0;
                        if (hours < 0)
                            hours = 0;
                        if (days < 0)
                            days = 0;                       

                        
                        sbStatus.Append("Open: ");
                        sbStatus.Append(days);
                        sbStatus.Append("d ");
                        sbStatus.Append(hours);
                        sbStatus.Append("h ");
                        sbStatus.Append(minutes);
                        sbStatus.Append("m ");
                        sbStatus.Append(seconds);
                        sbStatus.Append("s ");

                        e.Item.Cells[13].Text = sbStatus.ToString();


                        if (((data["EscalationsComplete"] != System.DBNull.Value) && (Convert.ToBoolean(data["EscalationsComplete"]))) || ((data["ComplianceEscalationComplete"] != System.DBNull.Value) && (Convert.ToBoolean(data["ComplianceEscalationComplete"]))))
                        {
                            if (string.Compare(strUserSettings, "YES") == 0)
                                e.Item.Cells[0].Text = "<b>O</b>";
                            else
                                e.Item.Cells[0].Text = "<img src=img/ic_baloon_red.gif border=0>";
                        }
                        (dgMessage.Columns[16] as HyperLinkColumn).Visible = false;
                    }  // end message not read...

                        // Message Read 
                    else
                    {
                        trNotification.Visible = false;
                        if (string.Compare(strUserSettings, "YES") == 0)
                            e.Item.Cells[0].Text = "x";
                        else
                            e.Item.Cells[0].Text = "<img src=img/ic_baloon_gray.gif border=0>";

                        e.Item.Cells[15].Text = "";

                        StringBuilder sbStatus = new StringBuilder();

                        //Check for With Agent timings
                        if (isCreatedByAgent)
                        {
                            DateTime createdOn = (DateTime)data["CreatedOn"];
                            DateTime calCreatedOn = new DateTime(createdOn.Year, createdOn.Month, createdOn.Day, createdOn.Hour, createdOn.Minute, createdOn.Second);                          
                            DateTime agentCreatedOn = (DateTime)data["AgentCreatedOn"];
                            DateTime calAgentCreatedOn = new DateTime(agentCreatedOn.Year, agentCreatedOn.Month, agentCreatedOn.Day, agentCreatedOn.Hour, agentCreatedOn.Minute, agentCreatedOn.Second);

                            TimeSpan withLASpan = calAgentCreatedOn.Subtract(calCreatedOn);
                     

                            if (withLASpan.Days > 0)
                                days = withLASpan.Days;
                            if (withLASpan.Hours > 0)
                                hours = withLASpan.Hours;
                            if (withLASpan.Minutes > 0)
                                minutes = withLASpan.Minutes;                            
                            if (withLASpan.Seconds > 0)
                                seconds = withLASpan.Seconds;

                            if (seconds < 0)
                                seconds = 0;
                            if (minutes < 0)
                                minutes = 0;
                            if (hours < 0)
                                hours = 0;
                            if (days < 0)
                                days = 0;

                            sbStatus.Append("With Agent: ");
                            sbStatus.Append(days);
                            sbStatus.Append("d ");
                            sbStatus.Append(hours);
                            sbStatus.Append("h ");
                            sbStatus.Append(minutes);
                            sbStatus.Append("m ");
                            sbStatus.Append(seconds);
                            sbStatus.Append("s ");
                            sbStatus.Append("<br /><br />");

                            days = hours = minutes = seconds = 0;

                            DateTime readOn = (DateTime)data["ReadOn"];

                            if (!agentCreatedOn.Equals(readOn))
                            {

                                DateTime calReadOn = new DateTime(readOn.Year, readOn.Month, readOn.Day, readOn.Hour, readOn.Minute, readOn.Second);

                                TimeSpan InVeriphySpan = calReadOn.Subtract(calAgentCreatedOn);

                                if (InVeriphySpan.Days > 0)
                                    days = InVeriphySpan.Days;
                                if (InVeriphySpan.Hours > 0)
                                    hours = InVeriphySpan.Hours;
                                if (InVeriphySpan.Minutes > 0)
                                    minutes = InVeriphySpan.Minutes;
                                if (InVeriphySpan.Seconds > 0)
                                    seconds = InVeriphySpan.Seconds;

                                if (seconds < 0)
                                    seconds = 0;
                                if (minutes < 0)
                                    minutes = 0;
                                if (hours < 0)
                                    hours = 0;
                                if (days < 0)
                                    days = 0;

                                sbStatus.Append("In Veriphy: ");
                                sbStatus.Append(days);
                                sbStatus.Append("d ");
                                sbStatus.Append(hours);
                                sbStatus.Append("h ");
                                sbStatus.Append(minutes);
                                sbStatus.Append("m ");
                                sbStatus.Append(seconds);
                                sbStatus.Append("s ");
                                sbStatus.Append("<br /><br />");

                                days = hours = minutes = seconds = 0;
                 
                            }

                        }

                        if (data["ReadOn_UsersTime"] != System.DBNull.Value)
                             sbStatus.Append("Received " + ((DateTime)data["ReadOn_UsersTime"]).ToString("MM/dd/yyyy hh:mm:sstt"));
                        if (data["ReadBy"] != System.DBNull.Value && data["ReadBy"].ToString().Trim().Length > 0)
                             sbStatus.Append(e.Item.Cells[13].Text + " by " + data["ReadBy"].ToString());

                         e.Item.Cells[13].Text = sbStatus.ToString();

                        if (data["ReceivedByVoiceURL"] != System.DBNull.Value && data["ReceivedByVoiceURL"].ToString().Length > 0)
                        {
                            StringBuilder sbReceivedBy = new StringBuilder();
                            //sbReceivedBy.Append(e.Item.Cells[13].Text);
                            sbReceivedBy.Append("&nbsp;<a href=");
                            sbReceivedBy.Append(data["ReceivedByVoiceURL"]);
                            if (string.Compare(strUserSettings, "YES") == 0)
                            {
                                sbReceivedBy.Append(">Play</a>");
                            }
                            else
                            {
                                sbReceivedBy.Append("><img src=img/ic_play_msg.gif border=0></a>");
                            }
                            e.Item.Cells[16].Text = sbReceivedBy.ToString();
                        }
                        else
                        {
                            if (!forwarded)
                                dgMessage.Columns[16].Visible = false;
                        }
                        if (!forwarded)
                        {
                            (dgMessage.Columns[14] as HyperLinkColumn).Visible = false;
                            (dgMessage.Columns[15] as HyperLinkColumn).Visible = false;
                        }
                    }

                    if (isDocumented)
                    {
                        trNotification.Visible = false;
                        e.Item.Cells[0].Text = "<image src=img/ic_baloon_yellow.gif border=0>";
                        if (strUserSettings == "YES")
                            e.Item.Cells[0].Text = "<b>D</b>";
                    }
                    if (data["DeclinedAt"] != System.DBNull.Value)
                    {
                        trNotification.Visible = false;
                        // message was declined, put decline message + graphic to decline voice over file
                        e.Item.Cells[13].Text += "<br>Declined at " + ((DateTime)data["DeclinedAt"]).ToString("MM/dd/yyyy hh:mm:sstt");

                        if (data["DeclinedMessageVoiceURL"] != System.DBNull.Value)
                        {

                            if (string.Compare(strUserSettings, "YES") == 0)
                            {
                                StringBuilder sbUserSettings = new StringBuilder();
                                sbUserSettings.Append(e.Item.Cells[13].Text);
                                sbUserSettings.Append("&nbsp; <a href=");
                                sbUserSettings.Append(data["DeclinedMessageVoiceURL"]);
                                sbUserSettings.Append(">Play Message</a>");

                                e.Item.Cells[13].Text = sbUserSettings.ToString();
                            }
                            else
                            {
                                StringBuilder sbNoUserSettings = new StringBuilder();
                                sbNoUserSettings.Append(e.Item.Cells[13].Text);
                                sbNoUserSettings.Append("&nbsp; <a href=");
                                sbNoUserSettings.Append(data["DeclinedMessageVoiceURL"]);
                                sbNoUserSettings.Append("><img src=img/ic_play_msg.gif border=0></a>");
                                e.Item.Cells[13].Text = sbNoUserSettings.ToString();
                            }

                        }
                        numAddRows++;
                        ViewState["numAddRows"] = numAddRows;
                    }

                    //starting of Reply / Readback part

                    int readbackID = 0;

                    if ((!forwarded) && data["ReadbackID"] != System.DBNull.Value) // For original message with readback.
                    {
                        readbackID = Convert.ToInt32(data["ReadbackID"].ToString());
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

                    if (data["ReplyVoiceURL"] != System.DBNull.Value)
                    {
                        if (string.Compare(strUserSettings, "YES") == 0)
                        {
                            StringBuilder sbUserSettings = new StringBuilder();
                            sbUserSettings.Append("<a href='ListenToReply.aspx?ReplyID=");
                            sbUserSettings.Append(data["MessageReplyID"]);
                            sbUserSettings.Append("&SubscriberID=");
                            sbUserSettings.Append(data["SubscriberID"]);
                            sbUserSettings.Append("&ReplyURL=");
                            sbUserSettings.Append(data["ReplyVoiceURL"]);
                            sbUserSettings.Append("'>Play Message</a>");

                            e.Item.Cells[12].Text = sbUserSettings.ToString();
                        }
                        else
                        {
                            StringBuilder sbNoUserSettings = new StringBuilder();
                            sbNoUserSettings.Append("<a href='ListenToReply.aspx?ReplyID=");
                            sbNoUserSettings.Append(data["MessageReplyID"]);
                            sbNoUserSettings.Append("&SubscriberID=");
                            sbNoUserSettings.Append(data["SubscriberID"]);
                            sbNoUserSettings.Append("&ReplyURL=");
                            sbNoUserSettings.Append(data["ReplyVoiceURL"]);
                            sbNoUserSettings.Append("'><img src=img/ic_play_msg.gif border=0></a>");

                            e.Item.Cells[12].Text = sbNoUserSettings.ToString();
                        }
                    }


                    // If the message has Reply Or Readback
                    if (replyID > 0 || readbackID > 0)
                    {
                        //Show Green icon for Reply Or Readback Open otherwise Gray.
                        if ((replyID > 0 && replyReadOn == DateTime.MinValue) || (readbackID > 0 && readbackReadOn == DateTime.MinValue))
                        {
                            //green
                            //ZNK 12-12-2006 Black and White - Change in Message Status Icon for Text view
                            if (string.Compare(strUserSettings, "YES") == 0)
                            {
                                if (replyID > 0 && replyReadOn == DateTime.MinValue)
                                    e.Item.Cells[0].Text = "<br><br>" + "r";
                                else if (readbackID > 0 && readbackReadOn == DateTime.MinValue)
                                    e.Item.Cells[0].Text = "<br><br>" + "R";
                            }
                            else
                            {
                                e.Item.Cells[0].Text = "<image src=img/ic_ballon_green.gif border=0>";
                                if (replyID > 0 || readbackID > 0)
                                    e.Item.Cells[0].Text = e.Item.Cells[0].Text + "<br><br>" + "<image src=img/icon_reply.gif border=0>";
                            }
                        }
                        else
                        {
                            //gray    
                            //ZNK 12-12-2006 Black and White - Change in Message Status Icon for Text view
                            if (string.Compare(strUserSettings, "YES") == 0)
                            {
                                e.Item.Cells[0].Text = "x";
                                if (replyID > 0)
                                    e.Item.Cells[0].Text = e.Item.Cells[0].Text + "<br><br>" + "r";
                                else if (readbackID > 0)
                                    e.Item.Cells[0].Text = e.Item.Cells[0].Text + "<br><br>" + "R";
                            }
                            else
                            {
                                e.Item.Cells[0].Text = "<image src=img/ic_baloon_gray.gif border=0>";
                                if (replyID > 0 || readbackID > 0)
                                    e.Item.Cells[0].Text = e.Item.Cells[0].Text + "<br><br>" + "<image src=img/icon_reply.gif border=0>";
                            }
                        }

                        numAddRows++;
                        ViewState["numAddRows"] = numAddRows;
                        // This is reply details so setting other columns data to blank.

                        //If message has Reply
                        if (replyID > 0)
                        {
                            bool isMessageRecorded = true;
                            if (data["ImpressionVoiceURL"] == System.DBNull.Value || data["ImpressionVoiceURL"].ToString().Length == 0)
                            {
                                isMessageRecorded = false;
                            }

                            e.Item.Cells[4].Text = e.Item.Cells[4].Text + "<br><br>" + String.Format("{0:MM/dd/yyyy hh:mm:sstt} ", Convert.ToDateTime(data["ReplyCreatedOn"]));
                            if (string.Compare(strUserSettings, "YES") == 0)
                            {
                                StringBuilder sbUserSettings = new StringBuilder();
                                if (isMessageRecorded)
                                {
                                    sbUserSettings.Append("<a href=");
                                    sbUserSettings.Append(data["ImpressionVoiceURL"]);
                                    sbUserSettings.Append(">Play Message</a>");
                                }
                                sbUserSettings.Append("<br><br>");
                                sbUserSettings.Append("<a href=");
                                sbUserSettings.Append(data["ReplyVoiceURL"]);
                                sbUserSettings.Append(">Play Message</a>");
                                e.Item.Cells[12].Text = sbUserSettings.ToString();
                            }
                            else
                            {
                                StringBuilder sbNoUserSettings = new StringBuilder();
                                if (isMessageRecorded)
                                {
                                    sbNoUserSettings.Append("<a href=");
                                    sbNoUserSettings.Append(data["ImpressionVoiceURL"]);
                                    sbNoUserSettings.Append("><img src=img/ic_play_msg.gif border=0></a>");
                                }
                                sbNoUserSettings.Append("<br><br>");
                                sbNoUserSettings.Append("<a href=");
                                sbNoUserSettings.Append(data["ReplyVoiceURL"]);
                                sbNoUserSettings.Append("><img src=img/ic_play_msg.gif border=0></a>");
                                e.Item.Cells[12].Text = sbNoUserSettings.ToString();
                            }
                            string replyscreatedOn = "";
                            if (data["ReplyCreatedOn_Server"] != System.DBNull.Value)
                            {
                                replyscreatedOn = data["ReplyCreatedOn_Server"].ToString();
                            }

                            DateTime replycreatedOn = DateTime.MinValue;
                            if (replyscreatedOn.Length > 0)
                            {
                                replycreatedOn = Convert.ToDateTime(replyscreatedOn);
                            }
                            //[21.12.2006, Nitin] Gettimezone for readback and reply
                            DateTime calReplyCreatedOn = new DateTime(replycreatedOn.Year, replycreatedOn.Month, replycreatedOn.Day, replycreatedOn.Hour, replycreatedOn.Minute, replycreatedOn.Second);                                                                          
                            //DateTime timeZoneNow = Utils.GetUsersTimeOnTimeZone(timeZoneID, DateTime.Now);
                            //DateTime calTimeZoneNow = new DateTime(timeZoneNow.Year, timeZoneNow.Month, timeZoneNow.Day, timeZoneNow.Hour, timeZoneNow.Minute, timeZoneNow.Second);
                            DateTime currentDate = (DateTime)data["CurrentDate"];
                            DateTime calTimeZoneNow = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, currentDate.Second);

                            TimeSpan replydiff = calTimeZoneNow.Subtract(calReplyCreatedOn);


                            //Reply Closed
                            if (replyReadOn != DateTime.MinValue)
                            {

                                if (data["ReadBy"] != System.DBNull.Value)
                                {
                                    e.Item.Cells[13].Text = e.Item.Cells[13].Text + "<BR> <BR> Reply Received " + ((DateTime)data["RpReadOn_UsersTime"]).ToString("MM/dd/yyyy hh:mm:sstt");
                                    numAddRows += 2;
                                }

                                if (data["ReplyReadBy"] != System.DBNull.Value)
                                    e.Item.Cells[13].Text = e.Item.Cells[13].Text + " by " + data["ReplyReadBy"].ToString();

                            }

                            //Reply Open
                            else
                            {
                                minutes = 0;
                                hours = 0;
                                days = 0;
                                seconds = 0;


                                if (replydiff.Days > 0)
                                    days = replydiff.Days;
                                if (replydiff.Hours > 0)
                                    hours = replydiff.Hours;
                                if (replydiff.Minutes > 0)
                                    minutes = replydiff.Minutes;
                                if (replydiff.Seconds > 0)
                                    seconds = replydiff.Seconds;


                                StringBuilder sbReplyOpen = new StringBuilder();
                                sbReplyOpen.Append(e.Item.Cells[13].Text);
                                sbReplyOpen.Append("<br><br>");
                                sbReplyOpen.Append(" Reply Open ");
                                sbReplyOpen.Append(days);
                                sbReplyOpen.Append("d ");
                                sbReplyOpen.Append(hours);
                                sbReplyOpen.Append("h ");
                                sbReplyOpen.Append(minutes);
                                sbReplyOpen.Append("m ");
                                sbReplyOpen.Append(seconds);
                                sbReplyOpen.Append("s ");

                                e.Item.Cells[13].Text = sbReplyOpen.ToString();
                                e.Item.Cells[15].Text = "<a href=mark_message_received.aspx?MessageID=" + Request["MessageID"].ToString() + "&MessageReplyID=" + replyID.ToString() + "&IsDeptMsg=" + data["IsDepartmentMessage"].ToString() + "&IsLabMsg=" + isLabMessage.ToString() + "&GroupID=" + data["GroupID"].ToString() + "&returnTo=" + Request["returnTo"] + "> Mark Received </a>";
                                (dgMessage.Columns[14] as HyperLinkColumn).Visible = false;
                                (dgMessage.Columns[15] as HyperLinkColumn).Visible = true;
                                numAddRows += 2;
                            }

                            e.Item.Cells[5].Text = e.Item.Cells[5].Text + "<br>" + " -Reply";
                        }
                        else
                        {
                            if (data["ReadbackCreatedOn"] != System.DBNull.Value)
                            {
                                e.Item.Cells[4].Text = e.Item.Cells[4].Text + "<br><br>" + String.Format("{0:MM/dd/yyyy hh:mm:sstt} ", Convert.ToDateTime(data["ReadbackCreatedOn"]));
                            }
                            if (data["ImpressionVoiceURL"] != System.DBNull.Value)
                            {
                                if (string.Compare(strUserSettings, "YES") == 0)
                                    e.Item.Cells[12].Text = "<a href=" + data["ImpressionVoiceURL"].ToString() + ">Play Message</a>";
                                else
                                    e.Item.Cells[12].Text = "<a href=" + data["ImpressionVoiceURL"].ToString() + "><img src=img/ic_play_msg.gif border=0></a>";
                            }
                            if (data["ReadbackVoiceURL"] != System.DBNull.Value)
                            {
                                if (string.Compare(strUserSettings, "YES") == 0)
                                {
                                    StringBuilder sbUserSettings = new StringBuilder();
                                    sbUserSettings.Append(e.Item.Cells[12].Text);
                                    sbUserSettings.Append("<br><br>");
                                    sbUserSettings.Append("<a href=");
                                    sbUserSettings.Append(data["ReadbackVoiceURL"]);
                                    sbUserSettings.Append(">Play Message</a>");
                                    e.Item.Cells[12].Text = sbUserSettings.ToString();
                                }
                                else
                                {
                                    StringBuilder sbNoUserSettings = new StringBuilder();
                                    sbNoUserSettings.Append(e.Item.Cells[12].Text);
                                    sbNoUserSettings.Append("<br><br>");
                                    sbNoUserSettings.Append("<a href=");
                                    sbNoUserSettings.Append(data["ReadbackVoiceURL"]);
                                    sbNoUserSettings.Append("><img src=img/ic_play_msg.gif border=0></a>");
                                    e.Item.Cells[12].Text = sbNoUserSettings.ToString();
                                }
                            }

                            string strReadbackCreatedOn = "";
                            if (data["ReadbackCreatedOn_Server"] != System.DBNull.Value)
                            {
                                strReadbackCreatedOn = data["ReadbackCreatedOn_Server"].ToString();
                            }
                            DateTime readbackCreatedOn = DateTime.MinValue;
                            if (strReadbackCreatedOn.Length > 0)
                            {
                                readbackCreatedOn = Convert.ToDateTime(strReadbackCreatedOn);
                            }
                            //[21.12.2006, Nitin] Gettimezone for readback and reply
                            //TimeSpan readbackDiff = DateTime.Now.Subtract(readbackCreatedOn);
                            DateTime calReadbackCreatedOn = new DateTime(readbackCreatedOn.Year, readbackCreatedOn.Month, readbackCreatedOn.Day,
                                                   readbackCreatedOn.Hour, readbackCreatedOn.Minute, readbackCreatedOn.Second);
                  
                            //DateTime timeZoneNow = Utils.GetUsersTimeOnTimeZone(timeZoneID, DateTime.Now);
                            //DateTime calTimeZoneNow = new DateTime(timeZoneNow.Year, timeZoneNow.Month, timeZoneNow.Day,
                            //                                                    timeZoneNow.Hour, timeZoneNow.Minute, timeZoneNow.Second);
                            DateTime currentDate = (DateTime)data["CurrentDate"];
                            DateTime calTimeZoneNow = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, currentDate.Second);
                            TimeSpan readbackDiff = calTimeZoneNow.Subtract(calReadbackCreatedOn);



                            //If readback is accepted / rejected.
                            if (readbackReadOn != DateTime.MinValue)
                            {
                                string strStatus = "";
                                if (Convert.ToBoolean(data["Accepted"]))
                                {
                                    strStatus = "Accepted";
                                }
                                else
                                {
                                    strStatus = "Rejected";
                                }
                                StringBuilder sbStatus = new StringBuilder();
                                sbStatus.Append(e.Item.Cells[13].Text);
                                sbStatus.Append("<br><br>");
                                sbStatus.Append(" Readback ");
                                sbStatus.Append(strStatus);

                                e.Item.Cells[13].Text = sbStatus.ToString();
                                numAddRows += 2;
                            }
                            //For readback Open
                            else
                            {
                                minutes = 0;
                                hours = 0;
                                days = 0;
                                seconds = 0;

                                if (readbackDiff.Days > 0)
                                    days = readbackDiff.Days;
                                if (readbackDiff.Hours > 0)
                                    hours = readbackDiff.Hours;
                                if (readbackDiff.Minutes > 0)
                                    minutes = readbackDiff.Minutes;
                                if (readbackDiff.Seconds > 0)
                                    seconds = readbackDiff.Seconds;


                                StringBuilder sbStatus = new StringBuilder();
                                sbStatus.Append(e.Item.Cells[13].Text);
                                sbStatus.Append("<br><br>");
                                sbStatus.Append(days);
                                sbStatus.Append("d ");
                                sbStatus.Append(hours);
                                sbStatus.Append("h ");
                                sbStatus.Append(minutes);
                                sbStatus.Append("m ");
                                sbStatus.Append(seconds);
                                sbStatus.Append("s ");
                                e.Item.Cells[13].Text = sbStatus.ToString();
                                numAddRows += 2;
                                StringBuilder sbURL = null;
                                if (data["ReadbackVoiceURL"] != System.DBNull.Value)
                                {
                                    sbURL = new StringBuilder();
                                    sbURL.Append(data["ReadbackVoiceURL"].ToString());
                                    sbURL.Append("$");
                                    sbURL.Append(readbackID);
                                    sbURL.Append("$");
                                    sbURL.Append(data["MessageID"].ToString());
                                    sbURL.Append("$");
                                    sbURL.Append(data["IsDepartmentMessage"].ToString());
                                }

                                if (data["ImpressionVoiceURL"] != System.DBNull.Value)
                                {
                                    if (string.Compare(strUserSettings, "YES") == 0)
                                    {
                                        StringBuilder sbWithUserSettings = new StringBuilder();
                                        sbWithUserSettings.Append("<a href=");
                                        sbWithUserSettings.Append(data["ImpressionVoiceURL"].ToString());
                                        sbWithUserSettings.Append(">Play Message</a>");
                                        sbWithUserSettings.Append("<br><br>");
                                        sbWithUserSettings.Append("<a href=");
                                        sbWithUserSettings.Append(data["ReadbackVoiceURL"].ToString());
                                        sbWithUserSettings.Append(" onclick=Start('");
                                        sbWithUserSettings.Append(sbURL.ToString());
                                        sbWithUserSettings.Append("');>Play Message</a>");
                                        e.Item.Cells[12].Text = sbWithUserSettings.ToString();
                                    }
                                    else
                                    {
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
                                        e.Item.Cells[12].Text = sbWithoutUserSettings.ToString();
                                    }
                                }

                            }

                            e.Item.Cells[5].Text = e.Item.Cells[5].Text + "<br>" + " -Readback";
                            e.Item.Cells[14].Text = "";
                        }

                        if (e.Item.ItemIndex > 0)
                            e.Item.BackColor = dgMessage.Items[e.Item.ItemIndex - 1].BackColor;
                    }

                    if (forwarded)
                    {
                        e.Item.Cells[0].Text = "";
                        e.Item.Cells[4].Text = "";
                        e.Item.Cells[12].Text = "";
                        e.Item.Cells[13].Text = "";
                        e.Item.Cells[14].Text = "";
                    }

                    //End of reply part
                }
            }

            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.dgMessage_ItemDataBound:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }

        }

        /// <summary>
        /// This function gets History details for Message Activity Log
        /// This function calls stored procedure for "getNotificationHistoryForMessage"
        /// </summary>
        /// <param name="messageID">integer Type</param>
        private void getMessageActivityHistory()
        {
            try
            {
                MessageDetails objMessageDetails = new MessageDetails();
                DataTable dtNotifyHistory = objMessageDetails.GetMessageActivityHistory(messageID, departmentMessage, isLabMessage, groupID);
                dgActivityLog.DataSource = dtNotifyHistory.DefaultView;
                dgActivityLog.DataBind();
                // ZNK 12-12-2006 : In 'Text' view DeviceImage Column replaced with DeviceDescription
                if (string.Compare(strUserSettings, "YES") == 0)
                {
                    dgActivityLog.Columns[5].Visible = false;
                    dgActivityLog.Columns[6].Visible = true;
                }
                objMessageDetails = null;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.getMessageActivityHistory:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }

        }

        /// <summary>
        /// This function gets Test Results for specified messageID
        /// This function calls stored procedure for "VOC_VL_getMessageTestResults"
        /// </summary>
        private void getMessageTestResults()
        {
            try
            {
                MessageDetails objMsgDetails = new MessageDetails();
                DataTable dtTestReslt = objMsgDetails.GetMessageTestResults(messageID);
                ViewState["dtTestResltCount"] = dtTestReslt.Rows.Count;
                dgTestResults.DataSource = dtTestReslt.DefaultView;
                dgTestResults.DataBind();
                objMsgDetails = null;

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.getMessageTestResults:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        /// <summary>
        /// This function Ensure that the messageID is valid for the requesting user/group
        /// This function calls stored procedure "validateUserForMessage"
        /// </summary>
        private void validateUserAgainstMessage()
        {
            MessageDetails objMsgDetails = new MessageDetails();
            if (!objMsgDetails.ValidateUserAgainstMessage(messageID, departmentMessage, isLabMessage))
                Response.Redirect("./default.aspx");
            objMsgDetails = null;
        }

        /// <summary>
        /// This function calls DataBind() for dynamic dgActivityLog bind.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgActivityLog_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            try
            {
                bool rowAdded = false;
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView data = (DataRowView)e.Item.DataItem;
                    string eventDescription = data["EventDescription"].ToString();
                    if (eventDescription.Equals("Message Read")
                            ||
                            eventDescription.Equals("Reply Created")
                            ||
                            eventDescription.Equals("Message Created")
                        )
                    {
                        e.Item.Cells[2].Font.Bold = false;
                        e.Item.Cells[4].Text = "";
                    }

                    if (eventDescription.Equals("Message Forwarded"))
                    {
                        int deptMessage = 0;
                        string linkedMsgID = data["MessageForwardID"].ToString();
                        if (linkedMsgID.Length == 0)
                        {
                            linkedMsgID = data["DeptMessageForwardID"].ToString();
                            deptMessage = 1;
                        }
                        StringBuilder sbURL = new StringBuilder();
                        sbURL.Append("<a href=");
                        if (linkedMsgID.Length == 0)
                            sbURL.Append("#");
                        else
                        {
                            sbURL.Append("message_details.aspx?MessageID=");
                            sbURL.Append(linkedMsgID);
                            sbURL.Append("&IsDeptMsg=");
                            sbURL.Append(deptMessage);
                            sbURL.Append("&IsLabMsg=");
                            sbURL.Append(isLabMessage.ToString());
                            sbURL.Append("&returnTo=");
                            sbURL.Append(Request["returnTo"]);
                        }
                        sbURL.Append(">Message Forwarded</a>");
                        e.Item.Cells[2].Text = sbURL.ToString();
                    }

                    if (eventDescription.Equals("Original Message"))
                    {
                        int deptMessage = 0;
                        string linkedMsgID = data["OriginalMessageID"].ToString();
                        if (linkedMsgID.Length == 0)
                        {
                            linkedMsgID = data["DeptOriginalMessageID"].ToString();
                            deptMessage = 1;
                        }

                        StringBuilder sbURL = new StringBuilder();
                        sbURL.Append("<a href=");
                        sbURL.Append("message_details.aspx?MessageID=");
                        sbURL.Append(linkedMsgID);
                        sbURL.Append("&IsDeptMsg=");
                        sbURL.Append(deptMessage);
                        sbURL.Append("&IsLabMsg=");
                        sbURL.Append(isLabMessage.ToString());
                        sbURL.Append("&returnTo=");
                        sbURL.Append(Request["returnTo"]);
                        sbURL.Append(">Original Message</a>");
                        e.Item.Cells[2].Text = sbURL.ToString();
                    }

                    /*SSK  -  2007/06/09 - Use Cc as backup - Provide link to view Primary and Backup Message in the activity log*/
                    if (eventDescription.Equals("Primary Message"))
                    {
                        int deptMessage = 0;
                        string linkedMsgID = data["PrimaryMessageID"].ToString();
                        if (linkedMsgID.Length == 0)
                        {
                            linkedMsgID = data["DeptPrimaryMessageID"].ToString();
                            deptMessage = 1;
                        }
                        StringBuilder sbURL = new StringBuilder();
                        sbURL.Append("<a href=");
                        if (linkedMsgID.Length == 0)
                            sbURL.Append("#");
                        else
                        {
                            sbURL.Append("message_details.aspx?MessageID=");
                            sbURL.Append(linkedMsgID);
                            sbURL.Append("&IsDeptMsg=");
                            sbURL.Append(deptMessage);
                            sbURL.Append("&IsLabMsg=");
                            sbURL.Append(isLabMessage.ToString());
                            sbURL.Append("&returnTo=");
                            sbURL.Append(Request["returnTo"]);
                        }
                        sbURL.Append(">Primary Message</a>");
                        e.Item.Cells[2].Text = sbURL.ToString();
                    }

                    if (eventDescription.Equals("Backup Message"))
                    {
                        int deptMessage = 0;
                        string linkedMsgID = data["BackupMessageID"].ToString();
                        if (linkedMsgID.Length == 0)
                        {
                            linkedMsgID = data["DeptBackupMessageID"].ToString();
                            deptMessage = 1;
                        }
                        StringBuilder sbURL = new StringBuilder();
                        sbURL.Append("<a href=");
                        sbURL.Append("message_details.aspx?MessageID=");
                        sbURL.Append(linkedMsgID);
                        sbURL.Append("&IsDeptMsg=");
                        sbURL.Append(deptMessage);
                        sbURL.Append("&IsLabMsg=");
                        sbURL.Append(isLabMessage.ToString());
                        sbURL.Append("&returnTo=");
                        sbURL.Append(Request["returnTo"]);
                        sbURL.Append(">Backup Message</a>");
                        e.Item.Cells[2].Text = sbURL.ToString();
                    }

                    if (eventDescription.StartsWith("Message closed"))
                    {
                        e.Item.Cells[2].Text = eventDescription;
                    }

                    if (e.Item.Cells[5].Text != "" && e.Item.Cells[5].Text != null && e.Item.Cells[5].Text != "&nbsp;")
                    {
                        //Device Icon
                        string notificationStatus = e.Item.Cells[7].Text;

                        //Check if Message is Delivered and NotificationText is stored
                        string strDeliveredNotificationID = (data["DeliveredNotificationID"] == null) ? string.Empty : data["DeliveredNotificationID"].ToString();
                        int deliveredNotificationID = 0;
                        if (strDeliveredNotificationID != null && strDeliveredNotificationID.Trim().Length > 0)
                            deliveredNotificationID = Convert.ToInt32(strDeliveredNotificationID);

                        if (notificationStatus.ToUpper().Equals(DELIVERED_STATUS) && deliveredNotificationID > 0)                        
                        {
                            //Delivered Notifications

                            string notificationHistoryID = e.Item.Cells[8].Text;
                            string deviceIDText = e.Item.Cells[9].Text;
                            int messageType = 1; //Rad

                            //Based on Querystring parameter set the message Type
                            while (true)
                            {
                                if (ParaIsDeptMsg.Equals("1"))
                                {
                                    messageType = 2; //Department
                                    break;
                                }

                                if (ParaIsLabMsg.Equals("1"))
                                {
                                    messageType = 3;//Lab
                                    break;
                                }
                                messageType = 1;//Rad
                                break;
                            }

                            if (deviceIDText.Equals("4"))
                            {
                                //Fax
                                string fileURL = ConfigurationManager.AppSettings[CONFIGKEY_FAXOUTPUTURL];
                                MessageDetails objMessageDetails = new MessageDetails();
                                //Get the Actual message Details
                                DataTable dtActualMessage = objMessageDetails.GetActualNotificationMessage(Convert.ToInt32(notificationHistoryID), messageType);
                                if (dtActualMessage.Columns.Contains("FaxTemplateURL") && dtActualMessage.Rows.Count > 0)
                                {
                                    fileURL += dtActualMessage.Rows[0]["FaxTemplateURL"].ToString();
                                }
                                e.Item.Cells[5].Text = "<a href='" + fileURL + "' target='_blank'><img src='" + e.Item.Cells[5].Text + "' border='0'/></a>";
                                dtActualMessage.Dispose();
                            }
                            else
                            {                                
                                //Email,Pager, SMS etc
                                e.Item.Cells[5].Text = "<a href='javascript:showActualMessagePopup(" + notificationHistoryID + ","+ messageType.ToString() +");'><img src='" + e.Item.Cells[5].Text + "' border='0'/></a>";
                            }
                        }
                        else
                        {
                            e.Item.Cells[5].Text = "<img src='" + e.Item.Cells[5].Text + "'>";
                        }
                    }

                    if (eventDescription.Length > 60)
                    {
                        activityWrappedRowCnt++;
                        rowAdded = true;
                    }

                    if (data["FailureDescription"].ToString().Length > 1)
                    {
                        e.Item.Cells[7].Text += ": " + data["FailureDescription"].ToString();
                        if (!rowAdded)
                            activityWrappedRowCnt++;
                    }
                }
            }

            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageDetails.dgActivityLog_ItemDataBound:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        #endregion


    }
}
