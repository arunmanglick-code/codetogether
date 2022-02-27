#region File History

/******************************File History***************************
 * File Name        : group_preferences.aspx.cs
 * Author           : Prerak Shah.
 * Created Date     : 18-07-2007
 * Purpose          : This Class will set the preferemces for group.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification
 *  11-30-2007  Prerak - Update for warning message. 
 *  11-30-2007  Prerak - Update method btnSave_Click,  getGroupPreferences, registerJavascriptVariables remove field archive Messages For Days
 * 12-07-2007   IAK    - Defect 2408 fixed
 * 12-11-2007   Prerak - Updated for OCFaxTemplateURL, CTTemplateURL, and UnitTemplateURL
 * 12-12-2007   Prerak - Rename CTFaxTemplateURL to GroupTemplateURL, Remove UnitFaxTemplateURL. 
 *                       After update as doc file save that file to .html format.   
 * 13-12-2007 - Prerak - Columns added CTFaxTemplateURL and UnitFaxTemplateURL
 * 17-12-2007 - Prerak - Change to store physical path in DB insted of url for Customized fax template.
 * 17-12-2007 - Prerak - Added File Icon hyper link to open existing template.
 * 18-12-2007 - Prerak - Use Default Template link added for OC, CT, GROUP, and UNIT Fax template.
 * 18-12-2007 - Prerak - Navigate URL for OC, CT, Group, Unit Fax Template is removed on click of use default link.
 * 02-06-2008 - IAK -    CR- TAP Pager implementation
 * 04-08-2008 - Prerak - Allow SMS Text Message With Web Link implementation 
 * 04-08-2008 - Prerak - Allow VUI Message Forwarding implementation 
 * 04-17-2008 - Prerak - Remove Allow sms text message with weblink 
 * 04-17-2008 - Prerak - Set Default = On for MessageForwardingAlert and ForwardedMessaageClosedAlert
 * 05-07-2008   Suhas    Defect 2979: Auto Tab issue.
 * 05-13-2008 - Prerak - Defect #3134 page error fixed
 * 30-05-2008 - Suhas  - Added Allow Send to Agent flag.
 * 12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 * 14 Jul 2008 - Prerak - Updated for RequirePatientNameInPagerAndSMS and RequirePatientNameInEmail 
 * 28 Aug 2008 - IAK    - Added DS option
 * 09 Sep 2008 - RajuG  - Removed the code for Upload and save fax template (After Implementation of Custom Notification Module)
 * 3 Oct 2008  - RajuG - Added one more preference For Forwarded Lab Message Readback
 * 17 Nov 2008 - JNK   - Defect #2735 - Update Group Preferences page refresh issue.
 * -----------------------------------------------------------------------------------------------------------------------------------------
 */
#endregion

#region using

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Vocada.CSTools.Common;
using Vocada.CSTools.DataAccess;
using System.Text;
using Vocada.VoiceLink.Utilities;

#endregion

namespace Vocada.CSTools
{
    public partial class group_preferences : System.Web.UI.Page
    {
        #region Private Members

        //Constants to hold Viewstate Key name for Current Insititute ID
        private const string INSTITUTE_ID = "InstID";
        
        #endregion

        #region Events

        /// <summary>
        /// Handle the Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
            {
                Response.Redirect(Utils.GetReturnURL("default.aspx", "group_preferences.aspx", this.Page.ClientQueryString));
            }
            
            try
            {
                this.btnSave.Attributes.Add("onclick", "javascript:if (checkRequired()){document.getElementById('" + hdnTextChanged.ClientID + "').value = 'false';}else {return false;}");
                
                registerJavascriptVariables();
                hlinkFindings.NavigateUrl = "./group_maintenance_findings.aspx?GroupID=" + Convert.ToInt32(Request["GroupID"]);
                if (!IsPostBack)
                {
                    ViewState["GroupID"]  = Convert.ToInt32(Request["Groupid"].ToString());
                    txtGroupID.Text = ViewState["GroupID"].ToString();
                    getGroupPreferences(Convert.ToInt32(ViewState["GroupID"].ToString()));
                    hdnTextChanged.Value = "false";
                }
                hlinkGroupMaintenance.NavigateUrl = "./group_maintenance.aspx?GroupID=" + ViewState["GroupID"]; ;  
                Session[SessionConstants.CURRENT_TAB] = "GroupMonitor";
                Session[SessionConstants.CURRENT_INNER_TAB] = "GroupPreferences";
                Session[SessionConstants.CURRENT_PAGE] = "group_preferences.aspx";

                this.Form.DefaultButton = this.btnSave.UniqueID;
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("group_preferences - page_load", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }
      
        /// <summary>
        /// Handle the Saave Group Preferences event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Group objGroup = null;
            GroupInformation objGroupInfo = null;
            string url = "";
            try
            {

                if (Session[SessionConstants.USER_ID] != null)
                {
                    objGroupInfo = new GroupInformation();
                    objGroupInfo.GroupID = Convert.ToInt32(ViewState["GroupID"]);
                    if (txtMsgActiveDays.Text.Length > 0)
                        objGroupInfo.MessageActiveForDays = Convert.ToInt32(txtMsgActiveDays.Text);
                    if (txtOverdueTh.Text.Length > 0)
                        objGroupInfo.OverdueThreshold = Convert.ToInt32(txtOverdueTh.Text);
                    objGroupInfo.RequireRPAcceptance = rblReqRPAcceptance.SelectedValue == "1" ? true : false;
                    objGroupInfo.RequireAccession = rblReqAccession.SelectedValue == "1" ? true : false;
                    objGroupInfo.RequirePatientInitials = rblReqPatientInitials.SelectedValue == "1" ? true : false;
                    objGroupInfo.RequireMRN = rblRequire.Items[0].Selected ? true : false;
                    objGroupInfo.RequireDOBIdentifier = rblRequire.Items[1].Selected ? true : false;
                    objGroupInfo.RequireNameCapture = rblReqNameCapture.SelectedValue == "1" ? true : false;
                    objGroupInfo.AllowDownload = rblAllowDownload.SelectedValue == "1" ? true : false;
                    objGroupInfo.ClosePrimaryAndBackupMessages = rblClosePBkupMsg.SelectedValue == "1" ? true : false;
                    objGroupInfo.AllowAlphanumericMRN = rblAllowAlphaNumaricMRN.SelectedValue == "1" ? true : false;
                    objGroupInfo.UseCcAsBackup = rblUseCcAsBackup.SelectedValue == "1" ? true : false;
                    objGroupInfo.VUIErrors = rblVuiErrors.SelectedValue == "1" ? true : false;
                    objGroupInfo.DirectoryTabOnDesktop = rblDirectoryTab.SelectedValue == "1" ? true : false;
                    //objGroupInfo.MessageForwardingAlert = rblMsgForwardingAlert.SelectedValue == "1" ? true : false;
                    //objGroupInfo.ForwardedMessageClosedAlert = rblForwardedMsgClosed.SelectedValue == "1" ? true : false;
                    objGroupInfo.EnableDirectorySynchronization = rblEnableDirectorySync.SelectedValue == "1" ? true : false;
                    if (rblGroupType.SelectedValue == "1")
                    {
                        objGroupInfo.AllowVUIMsgForwarding = rblVuiMsgForward.SelectedValue == "1" ? true : false;
                        objGroupInfo.RequireReadbackForFwdLabMsg = rblReadbackForFWDLabMsg.SelectedValue == "1" ? true : false;
                    }
                    objGroupInfo.OCFaxTemplate = url;
                    objGroupInfo.GroupFaxTemplate = url;
                    objGroupInfo.UnitFaxTemplate = url;
                    objGroupInfo.CTFaxTemplate = url;
                    objGroupInfo.PagerTAP800Number = txtPagerTAP800No1.Text.Trim() + txtPagerTAP800No2.Text.Trim() + txtPagerTAP800No3.Text.Trim();
                    objGroupInfo.AllowSendToAgent = rdlAllowSendToAgent.SelectedValue == "1" ? true : false;
                    objGroupInfo.RequirePatientNameInPagerAndSMS = rlstPagerSms.SelectedValue == "1" ? true : false;
                    objGroupInfo.RequirePatientNameInEmail = rlstEmail.SelectedValue == "1" ? true : false;                   
                    objGroup = new Group();
                    objGroup.UpdateGroupPreferences(objGroupInfo);
                    hdnTextChanged.Value = "false";
                    ScriptManager.RegisterStartupScript(upnlGroupPreferences, upnlGroupPreferences.GetType(), "GroupPreferences", "enabledControl();alert('Group Preferences Updated Successfully.');Navigate(" + ViewState[INSTITUTE_ID] + ");", true);
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("Group Preferences - lnkSubmitUpload_Click", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                objGroupInfo = null;
                objGroup = null;
            }

        }     

        #endregion

        #region Private Methods

        /// <summary>
        /// This method fills the group preferences for a perticular group.
        /// </summary>
        /// <param name="groupid">int groupid</param>
        private void getGroupPreferences(int groupid)
        {
            Group objGroup = new Group();
            DataTable dtGroupInfo = new DataTable();
            try
            {
                dtGroupInfo = objGroup.GetGroupPreferences(groupid);

                if (dtGroupInfo.Rows.Count > 0)
                {
                    DataRow drGroupPref ;//= new DataRow();
                    drGroupPref = dtGroupInfo.Rows[0];

                    txtGroupName.Text = drGroupPref["GroupName"] == DBNull.Value ? "" : drGroupPref["GroupName"].ToString();
                    txtMsgActiveDays.Text =  drGroupPref["MessageActiveForDays"] == DBNull.Value ? "14" : drGroupPref["MessageActiveForDays"].ToString();
                    txtOverdueTh.Text = drGroupPref["OverdueThreshold"] == DBNull.Value ? "0" : drGroupPref["OverdueThreshold"].ToString();
                    rblReqRPAcceptance.SelectedValue = drGroupPref["RequireRPAcceptance"]== DBNull.Value ? "0" :  Convert.ToInt32(drGroupPref["RequireRPAcceptance"]).ToString();
                    rblReqAccession.SelectedValue = drGroupPref["RequireAccession"]== DBNull.Value ? "0" : Convert.ToInt32(drGroupPref["RequireAccession"]).ToString();
                    rblReqPatientInitials.SelectedValue =drGroupPref["RequirePatientInitials"] == DBNull.Value? "0" : Convert.ToInt32(drGroupPref["RequirePatientInitials"]).ToString();
                    rblAllowDownload.SelectedValue = drGroupPref["AllowDownload"] == DBNull.Value ? "0" : Convert.ToInt32(drGroupPref["AllowDownload"]).ToString();
                    if (drGroupPref["RequireMRN"].ToString().Length > 0)
                    {
                        rblRequire.Items[0].Selected = Convert.ToBoolean(drGroupPref["RequireMRN"]);
                    }
                    if (drGroupPref["RequireDOBIdentifier"].ToString().Length > 0)
                    {
                        rblRequire.Items[1].Selected = Convert.ToBoolean(drGroupPref["RequireDOBIdentifier"]);
                    }
                    if (rblRequire.Items[0].Selected == false && rblRequire.Items[1].Selected == false)
                    {
                        rblRequire.Items[2].Selected = true;
                    }
                    rblClosePBkupMsg.SelectedValue = drGroupPref["ClosePrimaryAndBackupMessages"]==DBNull.Value? "0" : Convert.ToInt32(drGroupPref["ClosePrimaryAndBackupMessages"]).ToString();
                    rblReqNameCapture.SelectedValue = drGroupPref["RequireNameCapture"] == DBNull.Value ? "0" : Convert.ToInt32(drGroupPref["RequireNameCapture"]).ToString();
                    rblAllowAlphaNumaricMRN.SelectedValue = drGroupPref["AllowAlphaNumericMRN"]==DBNull.Value ?"0" : Convert.ToInt32(drGroupPref["AllowAlphaNumericMRN"]).ToString();
                    rblUseCcAsBackup.SelectedValue = drGroupPref["UseCCAsBackup"]==DBNull.Value ? "0" : Convert.ToInt32(drGroupPref["UseCCAsBackup"]).ToString();
                    rblVuiErrors.SelectedValue = drGroupPref["VUIErrors"] == DBNull.Value ? "1" : Convert.ToInt32(drGroupPref["VUIErrors"]).ToString();
                    rblDirectoryTab.SelectedValue = drGroupPref["DirectoryTabOnDesktop"]== DBNull.Value ? "1" : Convert.ToInt32(drGroupPref["DirectoryTabOnDesktop"]).ToString();
                    rblGroupType.SelectedValue = drGroupPref["GroupType"] == DBNull.Value ? "0" : Convert.ToInt32(drGroupPref["GroupType"]).ToString();
                   // rblMsgForwardingAlert.SelectedValue = drGroupPref["MsgForwardingAlert"] == DBNull.Value ? "1" : Convert.ToInt32(drGroupPref["MsgForwardingAlert"]).ToString();
                    //rblForwardedMsgClosed.SelectedValue = drGroupPref["ForwardedMsgClosedAlert"] == DBNull.Value ? "1" : Convert.ToInt32(drGroupPref["ForwardedMsgClosedAlert"]).ToString();
                    rdlAllowSendToAgent.SelectedValue = drGroupPref["AllowSendToAgent"] == DBNull.Value ? "1" : Convert.ToInt32(drGroupPref["AllowSendToAgent"]).ToString();
                    rlstPagerSms.SelectedValue = drGroupPref["RequirePatientNameInPagerAndSMS"] == DBNull.Value ? "0" : Convert.ToInt32(drGroupPref["RequirePatientNameInPagerAndSMS"]).ToString();
                    rlstEmail.SelectedValue = drGroupPref["RequirePatientNameInEmail"] == DBNull.Value ? "0" : Convert.ToInt32(drGroupPref["RequirePatientNameInEmail"]).ToString();
                    rblEnableDirectorySync.SelectedValue = drGroupPref["EnableDirectorySynchronization"] == DBNull.Value ? "0" : Convert.ToInt32(drGroupPref["EnableDirectorySynchronization"]).ToString();

                    if (rblGroupType.SelectedValue == "1")
                    {
                        rblVuiMsgForward.SelectedValue = drGroupPref["AllowVUIMsgForwarding"] == DBNull.Value ? "0" : Convert.ToInt32(drGroupPref["AllowVUIMsgForwarding"]).ToString();
                        rblReadbackForFWDLabMsg.SelectedValue = drGroupPref["RequireReadbackForFwdLabMsg"] == DBNull.Value ? "0" : Convert.ToInt32(drGroupPref["RequireReadbackForFwdLabMsg"]).ToString(); 
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(upnlGroupPreferences, upnlGroupPreferences.GetType(), "setVisibility", "setVisibility();", true);
                    }                    
                   
                    string tap800No = drGroupPref["TAP800Number"] == DBNull.Value ? "" : drGroupPref["TAP800Number"].ToString().Trim();
                    tap800No = Utils.flattenPhoneNumber(tap800No);
                    if (tap800No.Length >= 10)
                    {
                        txtPagerTAP800No1.Text = tap800No.Substring(0, 3);
                        txtPagerTAP800No2.Text = tap800No.Substring(3, 3);
                        txtPagerTAP800No3.Text = tap800No.Substring(6, 4);
                    }

                    ViewState[INSTITUTE_ID] = drGroupPref["InstitutionId"] == DBNull.Value ? "" : drGroupPref["InstitutionId"];
                    ScriptManager.RegisterStartupScript(upnlGroupPreferences, upnlGroupPreferences.GetType(), "GroupPreferences", "enabledControl();", true);
                    hdnTextChanged.Value = "false";
                        
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("group_preferences - getGroupPreferences", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                objGroup = null;
            }
        }

        /// <summary>
        /// Register JS variables, client side button click events
        /// </summary>
        private void registerJavascriptVariables()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("var hdnTextChangedClientID = '" + hdnTextChanged.ClientID + "';");
            ClientScript.RegisterStartupScript(Page.GetType(), "scriptDeviceClientIDs", sbScript.ToString(), true);

            txtMsgActiveDays.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtOverdueTh.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtGroupID.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            rblRequire.Attributes.Add("onclick", "JavaScript:document.getElementById('" + hdnTextChanged.ClientID + "').value='true';return enabledControl();");
            txtPagerTAP800No1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtPagerTAP800No2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtPagerTAP800No3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");

            txtPagerTAP800No1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPagerTAP800No2.ClientID + "').focus()";
            txtPagerTAP800No2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPagerTAP800No3.ClientID + "').focus()";

            rblAllowAlphaNumaricMRN.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblAllowDownload.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblClosePBkupMsg.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblReqAccession.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblReqNameCapture.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblReqPatientInitials.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblReqRPAcceptance.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblDirectoryTab.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblUseCcAsBackup.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            txtPagerTAP800No1.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtPagerTAP800No2.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtPagerTAP800No3.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            rblVuiMsgForward.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            //rblMsgForwardingAlert.Attributes.Add("onclick", "JavaScript:return TextChanged();");
           //rblForwardedMsgClosed.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rdlAllowSendToAgent.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblEnableDirectorySync.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblReadbackForFWDLabMsg.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            
            StringBuilder sbScript1 = new StringBuilder();
            sbScript1.Append("<script language=JavaScript>");
            sbScript1.Append("var rblRequireClientID = '" + rblRequire.ClientID + "';");
            sbScript1.Append("var rblAllowAlphaNumaricMRNClientID = '" + rblAllowAlphaNumaricMRN.ClientID + "';");
            sbScript1.Append("var lblAlphaNumaricMRNClientID = '" + lblAlphaNumaricMRN.ClientID + "';");
            sbScript1.Append("var txtMsgActiveDaysClientID = '" + txtMsgActiveDays.ClientID + "';");
            sbScript1.Append("var txtOverdueThClientID = '" + txtOverdueTh.ClientID + "';");
            sbScript1.Append("var txtPagerTAP800No1ClientID = '" + txtPagerTAP800No1.ClientID + "';");
            sbScript1.Append("var txtPagerTAP800No2ClientID = '" + txtPagerTAP800No2.ClientID + "';");
            sbScript1.Append("var txtPagerTAP800No3ClientID = '" + txtPagerTAP800No3.ClientID + "';");
            sbScript1.Append("var lblVuiMsgForwardClientID = '" + lblVuiMsgForward.ClientID + "';");
            sbScript1.Append("var rblVuiMsgForwardClientID = '" + rblVuiMsgForward.ClientID + "';");
            sbScript1.Append("var rblEnableDirectorySyncClientID = '" + rblEnableDirectorySync.ClientID + "';");
            sbScript1.Append("var lblRequireReadbackForFWDLabMsgClientID = '" + lblRequireReadbackForFWDLabMsg.ClientID + "';");
            sbScript1.Append("var rblReadbackForFWDLabMsgClientID = '" + rblReadbackForFWDLabMsg.ClientID + "';");
            sbScript1.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript1.ToString());
        }  

        #endregion

    }
}
