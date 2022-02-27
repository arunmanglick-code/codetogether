#region File History

/******************************File History***************************
 * File Name        : mark_message_received.aspx.cs
 * Author           : 
 * Created Date     : 
 * Purpose          : Shows Details of Selected Open Message and alllow to Mark it as Received.
 *                  : 
 *                  :
 * *********************File Modification History*********************
 * Date(mm-dd-yyyy) Developer Reason of Modification
 * ------------------------------------------------------------------- 
 *  02-21-2007      Swapnil     Modified all functions for Veriphy Lab.
 *  03-01-2007      IAK         Modified method btnMarkReceived_Click() Added roleid parameter to GetSubscriberInformation() method to retrive Subscriber Information
 *  02-21-2007      Swapnil K   Separated business logics functions into another level.Veriphy lab changes.
 *  06-09-2007      SSK         Close the Primary / BackupMessage if one of the Message is closed and Group Setting for Close both Message is ON.
 *  07-18-2007      IAK         Changes w.r.t Department integration   
 *  01-08-2008      Prerak      Added Curent Tab to session. 
 *  01-11-2008      IAK         ViewState Added for messageID, isdept, and islab for reply message flow
 *  08-05-2008      Suhas       Defect #2900 - Fixed.
 *  12 Jun 2008     Prerak      Migration of AJAX Atlas to AJAX RTM 1.0
 *  08-08-2008      IAK         FR: Show Year in date
 *  12-02-2008 - SSK    - Defect #4270 Close primary or backup message if other is closed. 
 * ------------------------------------------------------------------- */
#endregion
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using Vocada.VoiceLink.Utilities;
using Vocada.CSTools.Common;
using Vocada.CSTools.DataAccess;

namespace Vocada.CSTools
{
    /// <summary>
    /// Summary description for MarkMessageReceived.
    /// Mark as Received page makes message as Read.
    /// The Status of Message will be update.
    /// Reason for changing status of Message will be taken as a input.
    /// </summary>
    public partial class MarkMessageReceived : System.Web.UI.Page
    {
        #region Private Fields

        private int messageID = 0;
        private int isDeptMessage = 0;
        private int isLabMessage = 0;
        private int groupID = 0;
        private int messageReplyID = 0;

        private const string DATE_FORMAT = "MM/dd/yyyy hh:mmtt";
        #endregion

        #region Protected Fields
        protected string strUserSettings = "NO";
        #endregion

        #region Private Methods
        /// <summary>
        /// This function is called while page load occurs.
        /// This function fills Message Detail information, which Message will be marked as 'Message Read' and set Reason for that Message ID
        /// This function calls stored procedure "getMessage"
        /// </summary>
        private void getMessageDetails()
        {
            DataTable dt = null;
            DataRow dr = null;
            try
            {
                MessageDetails objMessageDetails = new MessageDetails();
                dt = objMessageDetails.GetMessageDetails(messageID, isDeptMessage, isLabMessage);
                dr = dt.Rows[0];
                int recepientType = -1;
                if (dr["RecipientTypeID"] != null)
                    recepientType = Convert.ToInt32(dr["RecipientTypeID"]);

                ViewState["MessageID"] = dr["MessageID"].ToString();
                lblMessageType.Text = "Outgoing Message";
                lblOn.Text = ((DateTime)dr["CreatedOn"]).ToString(DATE_FORMAT);

                int subscriberRoleID = Convert.ToInt32(dr["RoleID"]);
                string roleDesc = " (Lab Technician)";
                
                if (subscriberRoleID == (int)UserRoles.Specialist)
                {
                    roleDesc = " (Specialist)";
                }
                else if (subscriberRoleID == (int)UserRoles.GroupAdministrator)
                {
                    roleDesc = " (Group Administrator)";
                }

                lblFrom.Text = (string)dr["SpecialistDisplayName"] + roleDesc;
                string recipientName = dr["RecipientDisplayName"].ToString();
                if (recipientName.Length > 25)
                {
                    recipientName = recipientName.Substring(0, 25) + "...";
                    lblTo.ToolTip = (string)dr["RecipientDisplayName"];
                }
                lblTo.Text = recipientName;

                if (recepientType == (int)MessageInfo.RecepientType.BedNumber && dr["NurseID"] != System.DBNull.Value)
                {
                    lblTo.Text += " (Nurse)";
                }
                else if (recepientType == (int)MessageInfo.RecepientType.UnitName || recepientType == (int)MessageInfo.RecepientType.BedNumber)
                {
                    lblTo.Text += " (Unit)";
                }
                else if (recepientType == (int)MessageInfo.RecepientType.Department)
                {
                    lblTo.Text += " (Clinical Team)";
                }
                else
                    lblTo.Text += " (Ordering Clinician)";

                lblFinding.Text = (string)dr["FindingDescription"];
                objMessageDetails = null;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("MarkMessageReceived.getMessageDetails:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
            }
            finally
            {
                dr = null;
                dt = null;
            }
        }

        /// <summary>
        /// This function is called while page load occurs.
        /// This function fills Message Detail Reply information, which Message Reply will be marked as 'Message Read' and set Reason for that Message Reply ID
        /// This function calls stored procedure "getMessageReply"
        /// </summary>
        private void getMessageReplyDetails()
        {
            try
            {
                LabMarkAsReceived objLabMarkReceived = new LabMarkAsReceived();
                DataTable dtMessageReply = objLabMarkReceived.GetMessageReplyDetails(messageReplyID, isDeptMessage, isLabMessage);
                DataRow drReplyDetails = dtMessageReply.Rows[0];
                int recepientType = -1;
                if (drReplyDetails["RecipientTypeID"] != null)
                    recepientType = Convert.ToInt32(drReplyDetails["RecipientTypeID"]);
                ViewState["MessageID"] = drReplyDetails["MessageID"].ToString();
                ViewState["MessageReplyID"] = messageReplyID;
                lblMessageType.Text = "Reply";
                lblOn.Text = ((DateTime)drReplyDetails["CreatedOn"]).ToString(DATE_FORMAT);
                string roleText = string.Empty;
                if (recepientType == (int)MessageInfo.RecepientType.BedNumber && drReplyDetails["NurseID"] != System.DBNull.Value)
                {
                    roleText = " (Nurse)";
                }
                else if (recepientType == (int)MessageInfo.RecepientType.UnitName || recepientType == (int)MessageInfo.RecepientType.BedNumber)
                {
                    roleText = " (Unit)";
                }
                else
                    roleText = " (Ordering Clinician)";


                int subscriberRoleID = Convert.ToInt32(drReplyDetails["RoleID"]);
                string subscriberRoleDesc = " (Lab Technician)";
                
                if (subscriberRoleID == (int)UserRoles.Specialist)
                {
                    subscriberRoleDesc = " (Specialist)";
                }
                else if (subscriberRoleID == (int)UserRoles.GroupAdministrator)
                {
                    subscriberRoleDesc = " (Group Administrator)";
                }

                if ((bool)drReplyDetails["IsReplyFromSpecialist"])
                {
                    lblFrom.Text = (string)drReplyDetails["SpecialistDisplayName"] + subscriberRoleDesc;
                    lblTo.Text = (string)drReplyDetails["RecipientDisplayName"] + roleText;
                }
                else
                {
                    lblFrom.Text = (string)drReplyDetails["RecipientDisplayName"] + roleText;
                    lblTo.Text = (string)drReplyDetails["SpecialistDisplayName"] + subscriberRoleDesc;
                }

                lblFinding.Text = (string)drReplyDetails["FindingDescription"];
                drReplyDetails = null;
                dtMessageReply = null;
                objLabMarkReceived = null;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("MarkMessageReceived.getMessageReplyDetails:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
        }
        #endregion

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

        }
        #endregion

        #region Control's Event
        /// <summary>
        /// This function sends back to Message details page and cancel Marking Message as Read.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, System.EventArgs e)
        {

            string strQuery = "?MessageID=" + ViewState["MessageID"].ToString() + "&IsDeptMsg=" + ViewState["isDeptMessage"].ToString() + "&IsLabMsg=" + ViewState["isLabMessage"].ToString() + "&returnTo=" + Request["returnTo"];
            ScriptManager.RegisterClientScriptBlock(UpdatePanelMarkMessageReceived,UpdatePanelMarkMessageReceived.GetType(),"Navigate To Message Detail", "<script type=\'text/javascript\'>Navigate('" + strQuery + "');</script>",false);
            
            //Response.Redirect(strQuery);
        } 

        /// <summary>
        /// This function Click Mark Message and Reply as Read. This function validates for required field during updation.
        /// This function calls stored procedure "getSubscriberInfoBySubscriberID"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMarkReceived_Click(object sender, System.EventArgs e)
        {

            if (txtReason.Text.Length < 2)
            {
                lblError.Text = "You must enter a reason why you are marking the message as received";
                return;
            }
            DataTable dtGroupInfo = null;
            Group group = null; 
            try
            {
                string readBy = "";
                string userName = "";
                StringBuilder sbReadBy = new StringBuilder();
                sbReadBy.Append(Session[SessionConstants.LOGGED_USER_NAME].ToString());
                readBy = sbReadBy.ToString();
                userName = Session[SessionConstants.LOGGED_USER_ID].ToString();

                bool isReplyOpen = false;
                int orgMessageID = 0;
                if (ViewState["MessageReplyID"] != null)    // top-level message marked as received
                {
                    isReplyOpen = true;
                    orgMessageID = messageReplyID;
                }
                else
                {
                    orgMessageID = messageID;
                }

                LabMarkAsReceived objMarkAsReced = new LabMarkAsReceived();

                /*SSK  -  2007/07/09 - Use Cc as Backup functionality - 1.Check the ClosePrimaryAndBackupMessages Group Preference setting for the Logged in User Group
                                                                        2. If the Setting is ON call the CloseBackupMessage of MarkAsReceived to close the related message */
                group = new Group();
                dtGroupInfo = group.GetGroupInformationByGroupID(groupID);

                bool isBackupMessage = false;
                if (dtGroupInfo.Rows.Count > 0)
                {
                    isBackupMessage = Convert.ToBoolean(dtGroupInfo.Rows[0]["ClosePrimaryAndBackupMessages"].ToString());
                }
                int result = objMarkAsReced.MarkMessageReceived(orgMessageID, isDeptMessage, isReplyOpen, userName, readBy, txtReason.Text.Trim(), false, isLabMessage, groupID);

                if (isBackupMessage && !isReplyOpen)
                    result = objMarkAsReced.CloseBackupMessage(orgMessageID, isDeptMessage, isReplyOpen, userName, readBy, txtReason.Text.Trim(), false, isLabMessage, groupID);
                textChanged.Value = "false";
                objMarkAsReced = null;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("MarkMessageReceived.btnMarkReceived_Click:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
            finally
            {
                dtGroupInfo = null;
                group = null;
                string strQuery = "?MessageID=" + ViewState["MessageID"].ToString() + "&IsDeptMsg=" + ViewState["isDeptMessage"].ToString() + "&IsLabMsg=" + ViewState["isLabMessage"].ToString() + "&returnTo=" + Request["returnTo"];
                ScriptManager.RegisterClientScriptBlock(UpdatePanelMarkMessageReceived,UpdatePanelMarkMessageReceived.GetType(), "Navigate To Message Detail", "<script type=\'text/javascript\'>Navigate('" + strQuery + "');</script>",false );
            
                //Page.RegisterClientScriptBlock("Navigate To Message Detail", "<script type=\'text/javascript\'>Navigate('" + ViewState["MessageID"].ToString() + "', '" + Request.QueryString["IsDeptMsg"] + "');</script>");
                //Response.Redirect("./message_details.aspx?MessageID=" + ViewState["MessageID"].ToString());
            }
        }

        /// <summary>
        /// This is Page Load method while page loads to show.
        /// This function shows forms to fill for making message as Read.
        /// This functions set information of Message ID or Message Reply ID which status is going to change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if(Session[SessionConstants.LOGGED_USER_ID] == null)
                    Response.Redirect(Vocada.CSTools.Utils.GetReturnURL("default.aspx", "message_center.aspx", this.Page.ClientQueryString));

                Session[SessionConstants.CURRENT_TAB] = "MsgCenter";

                if (Session[SessionConstants.USER_SETTINGS] != null)
                    strUserSettings = Session[SessionConstants.USER_SETTINGS].ToString();
                if (strUserSettings == "YES")
                    lblWarning.ForeColor = Color.Black;

                StringBuilder sbScript = new StringBuilder();
                sbScript.Append("<script language=JavaScript>");
                sbScript.Append("var textChangedClientID = '" + textChanged.ClientID + "';");
                sbScript.Append("</script>");
                this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());

                //if (Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.ROLE_ID] == null || Session[SessionConstants.SUBSCRIBER_INFO] == null)
                //    Response.Redirect(Utils.GetReturnURL("default.aspx", "mark_message_received.aspx", this.Page.ClientQueryString));
                txtReason.Attributes.Add("onchange", "JavaScript:return CheckMaxLength('" + txtReason.ClientID + "','" + txtReason.MaxLength + "');");
                txtReason.Attributes.Add("onblur", "JavaScript:return CheckMaxLength('" + txtReason.ClientID + "','" + txtReason.MaxLength + "');");
                txtReason.Attributes.Add("onkeyup", "JavaScript:return CheckMaxLength('" + txtReason.ClientID + "','" + txtReason.MaxLength + "');");
                txtReason.Attributes.Add("onkeydown", "JavaScript:return CheckMaxLength('" + txtReason.ClientID + "','" + txtReason.MaxLength + "');");
                txtReason.Attributes.Add("onchange", "JavaScript:UpdateProfile();");

                if (Request["MessageReplyID"] == null) // top level message
                {
                    if (Request.QueryString["MessageID"] == null || Request.QueryString["MessageID"].Length == 0 ||
                        Request.QueryString["IsDeptMsg"] == null || Request.QueryString["IsDeptMsg"].Length == 0 ||
                        Request.QueryString["IsLabMsg"] == null || Request.QueryString["IsLabMsg"].Length == 0 ||
                        Request.QueryString["GroupID"] == null || Request.QueryString["GroupID"].Length == 0)
                        Response.Redirect("./default.aspx");

                    messageID = int.Parse(Request["MessageID"].ToString());
                    isLabMessage = int.Parse(Request["IsLabMsg"].ToString());
                    isDeptMessage = int.Parse(Request.QueryString["IsDeptMsg"]);
                    groupID = int.Parse(Request.QueryString["GroupID"]);
                    ViewState["MessageID"] = messageID.ToString();
                    ViewState["isDeptMessage"] = isDeptMessage.ToString();
                    ViewState["isLabMessage"] = isLabMessage.ToString();
                    getMessageDetails();
                    Session[SessionConstants.CURRENT_PAGE] = "mark_message_received.aspx?MessageID=" + Request["MessageID"] + "&IsDeptMsg=" + Request["IsDeptMsg"];
                }
                else  // reply message
                {
                    
                    if (Request.QueryString["MessageReplyID"] == null || Request.QueryString["MessageReplyID"].Length == 0 ||
                        Request.QueryString["IsDeptMsg"] == null || Request.QueryString["IsDeptMsg"].Length == 0)
                        Response.Redirect("./default.aspx");

                    messageID = int.Parse(Request["MessageID"].ToString());
                    messageReplyID = int.Parse(Request["MessageReplyID"].ToString());
                    isLabMessage = int.Parse(Request["IsLabMsg"].ToString());
                    isDeptMessage = int.Parse(Request.QueryString["IsDeptMsg"]);
                    groupID = int.Parse(Request.QueryString["GroupID"]);
                    ViewState["MessageID"] = messageID.ToString();
                    ViewState["isDeptMessage"] = isDeptMessage.ToString();
                    ViewState["isLabMessage"] = isLabMessage.ToString();
                    
                    getMessageReplyDetails();
                    Session[SessionConstants.CURRENT_TAB] = "MsgCenter";
                    Session[SessionConstants.CURRENT_INNER_TAB] = "MessageReceived";
                    Session[SessionConstants.CURRENT_PAGE] = "mark_message_received.aspx?MessageReplyID=" + Request["MessageReplyID"] + "&IsDeptMsg=" + Request["IsDeptMsg"];
                }

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("MarkMessageReceived.getMessageDetails:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
        }
        #endregion
    }
}
