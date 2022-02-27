#region File History

/******************************File History***************************
 * File Name        : user_profile.aspx.cs
 * Author           : ZNK.
 * Created Date     : 20 November 07
 * Purpose          : Display Subscriber information and allow to edit.

 * *********************File Modification History*********************

 * * Date(dd-mm-yyyy) Developer Reason of Modification

  * ------------------------------------------------------------------- 
 *  03-12-2007 - IAK    -  UI allignemnt, Role change then logic change, validation added, update profile logic few changes
 *  18-12-2007 - Prerak -  iteration 17 Code review fixes  
 *  08-01-2008 - IAK    -  Defect 2546
 *  09-01-2008 - IAK    -  Defect 2549, 2547
 *  09-01-2007 - IAK    -  User Crendtial validation added.
 *  11-01-2007 - IAK    -  Defect 2555.
 *  08-02-2008 - Suhas  -  Defect # 2722, 2723, 2724 - Fixed
 *  25-02-2008 - IAK    -  Lable Changed of message open for last # days.
 *  05-07-2008   Suhas     Defect 2979: Auto Tab issue. 
 *  12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 *                         Removed Iframe and Merged Notification Step1, Step2, Step3 
 *  19 Jun 2008 - Prerak - Defect #3311 - Confirmation Popup and Warning Popups are not coming up Fixed
 *  20 Jun 2008 - Prerak - UI Issue solved 
 *  05 Aug 2008 - IAK    - Replaced Vocada with Nuance in error message
 *  11 Aug 2008 - Prerak - CR #255 Conformation pouup for add/edit devices
 *  09 Sep 2008 - Prerak - Sharepoint defect #558 "The user can't enter the correct email gateway 
 *                         for a clinical team or unit" fixed
 * 10 Sep 2008  - Prerak - if only number is chaged email gateway updated automatically.
 * 13 Nov 2008  - Zeeshan- Defect #3164 - User clicks on Edit for a device at the bottom of the list they are forced to scroll to find the device they selected.
 * 14 Nov 2008  - Zeeshan- Defect #4154 - Unknown Error
 * 14 Nov 2008  - IAK    - Defect #3593
 * 11 Nov 2008  - ZNK    - Defect - Error on assign After-on-Notification when no devices present
 * 17-Nov-2008  - IAK    - Defect #3113 Fixed.
 * 18-Nov-2008  - Prerak - Defect #4165 Fixed
 * 18-Nov-2008  - IAK    - Defect #3113 Fixed
 * 20-Nov-2008  - IAK    - Defect #3113, #4165 Fixed: Handled blank values
 * 11-25-2008     SD     - Defect #4225 fixed
 * 09-01-2009   - GB     - Changes made for FR #282
 * * * ------------------------------------------------------------------- 
 
 */
#endregion

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.Utilities;
using VoiceLinkR2;

namespace Vocada.CSTools
{

    /// <summary>
    /// The purpose of this class is to provide the functions to fetch and edit the profile information
    /// of the logged in user.
    /// Edit Profile, Add Device, Add Notifications and Sending overdue messages are features of the profile page.
    /// Additionally logged in user can set Configuration setting for No of days and sending options for overdue message.
    /// </summary>
    public partial class user_profile : System.Web.UI.Page
    {
        #region Constants

        private const string GROUP_MAINTAINANCE = "GroupMaintenance";
        private const string TAP_800_NUM = "Tap800Num";
        private const string GROUP_ID = "GroupID";
        private const string SUBSCRIBER_ID = "SubscriberID";
        private const string SHOWDETAILS_BUTTONNAME = "Assign Event";
        private const string HIDEDETAILS_BUTTONNAME = "Hide Event Details";
        #endregion Constants

        #region Private Variables
        private int subscriberID;
        private int numberOfAfterHoursNotifications;
        private int numberOfDevices;
        private int numberOfNotifications;
        private bool isProfileSaved = false;
        private int credentialStatus = 0;
        private bool NoDelete = false;

        #endregion Private Variables


        #region Protected Fields
        protected string strUserSettings = "NO";
        protected string strAccess = "YES";
        #endregion

        #region Events
        /// <summary>
        /// This function is for filling User information, load report settings,
        /// User Configuration settings for logged in users.
        /// Exception handling has been taken care of in catch block.
        /// This is page load function for Notification step2 which fills logged in user Notification Events information, devices into drop down list for adding new devices
        /// Generating dynamic height of datagrid for Notification Events , fill all list of Devices for logged in user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Session[SessionConstants.USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
                Response.Redirect(Utils.GetReturnURL("default.aspx", "user_profile.aspx", this.Page.ClientQueryString));

            Session[SessionConstants.CURRENT_TAB] = "GroupMonitor";
            Session[SessionConstants.CURRENT_INNER_TAB] = "UseProfile";
            Session[SessionConstants.CURRENT_PAGE] = "user_profile.aspx";

            this.Form.DefaultButton = btnDefault.UniqueID;


            if (ViewState["RoleID"] == null)
            {
                ViewState["RoleID"] = Request["RoleID"];
                hdnUserRole.Value = Request["RoleID"];
            }
            else
            {
                hdnUserRole.Value = ViewState["RoleID"].ToString();
            }


            setUserGroup();
            //Load all javascripts
            loadJavaScript();
            try
            {
                if (Request["SubscriberID"] != null)
                {
                    subscriberID = Convert.ToInt32(Request["SubscriberID"].ToString());
                    ViewState["SubscriberID"] = subscriberID.ToString();
                    if (!Page.IsPostBack)
                    {
                        Page.SmartNavigation = true;
                        fillRoleDDL();
                        fillUserInformation();
                        fillSubscriberReportSettings();
                        getCellPhoneCarriers();
                        getPagerCarriers();
                        fillUserConfigurationInformation();
                        /* Remove Information profile(Notification Step 1,2 & 3)*/
                        //QueryStringtoIframe();
                        txtFirstName.Focus();
                        if (!(Convert.ToInt32(ViewState["RoleID"].ToString()) == UserRole.GroupAdmin.GetHashCode() ||
                        Convert.ToInt32(ViewState["RoleID"].ToString()) == UserRole.LabGroupAdmin.GetHashCode()))
                        {
                            //Step1
                            setTAP800Number();
                            fillDevices();
                            fillDeviceDDL();
                            getCellPhoneCarriers();
                            getPagerCarriers();
                            lblDeviceAlreadyExists.Text = "";
                            //end step1

                            //step2
                            fillFindings(cmbFinding);
                            fillEvents(cmbEvent);
                            //end step2

                            //step3
                            fillAfterHoursFindings();
                            fillSubscriberAfterHoursDeviceOptions();
                            fillAfterHoursNotifications();
                            //end Step3
                        }
                    }
                    //step1
                    if (!(Convert.ToInt32(ViewState["RoleID"].ToString()) == UserRole.GroupAdmin.GetHashCode() ||
                        Convert.ToInt32(ViewState["RoleID"].ToString()) == UserRole.LabGroupAdmin.GetHashCode()))
                    {
                        txtDeviceAddress.Attributes.Add("onclick", "RemoveDeviceLabel();");

                        if (cmbDevices.Items.Count > 0 && cmbDevices.SelectedItem.Text.Equals("Email"))
                            txtGateway.Attributes.Add("onclick", "RemoveGatewayLabel();");

                        if (int.Parse(cmbDevices.SelectedItem.Value) == NotificationDevice.pagerPartner.GetHashCode() || int.Parse(cmbDevices.SelectedItem.Value) == NotificationDevice.PagerNumSkyTel.GetHashCode() || int.Parse(cmbDevices.SelectedItem.Value) == NotificationDevice.PagerNumUSA.GetHashCode() || int.Parse(cmbDevices.SelectedItem.Value) == NotificationDevice.PagerNumRegular.GetHashCode())
                        {
                            txtDeviceAddress.Attributes.Add("onkeyPress", "JavaScript:return PagerValidationWithSpace('" + txtDeviceAddress.ClientID + "');");
                            //txtDeviceAddress.MaxLength = 100;
                        }
                        else if (cmbDevices.Items.Count > 0 && int.Parse(cmbDevices.SelectedItem.Value) == NotificationDevice.EMail.GetHashCode())
                        {
                            txtDeviceAddress.Attributes.Remove("onkeydown");
                            txtDeviceAddress.Attributes.Remove("onchange");
                            txtDeviceAddress.Attributes.Remove("onkeyPress");
                            txtDeviceAddress.MaxLength = 100;
                        }
                        else
                        {
                            txtDeviceAddress.Attributes.Add("onkeyPress", "JavaScript:return isNumericKey();");
                            //txtDeviceAddress.MaxLength = 10;
                        }
                        //end step1
                    }
                    lblUpdateMessage.Text = "";
                    /*Remove Information profile(Notification Step 1,2 & 3),group maintenance*/
                    if (Convert.ToInt32(ViewState["RoleID"].ToString()) == UserRole.GroupAdmin.GetHashCode() ||
                        Convert.ToInt32(ViewState["RoleID"].ToString()) == UserRole.LabGroupAdmin.GetHashCode())
                    {
                        divNotification.Visible = false;
                        legendNotifation.InnerText = "Configuration Settings";
                    }
                    //get Called in case page navigation
                    if (Request.Form["__EVENTTARGET"] != null)
                    {
                        if (Request.Form["__EVENTTARGET"].ToString().CompareTo("btnUpdateProfileClientID") == 0)
                        {
                            btnUpdateProfile_Click(sender, e);
                        }
                        if (Request.Form["__EVENTTARGET"].ToString().CompareTo("btnConfirmClientID") == 0)
                        {
                            btnConfirm_Click(sender, e);
                        }
                        if (Request.Form["__EVENTTARGET"].ToString().CompareTo("btnUpdateconfigClientID") == 0)
                        {
                            btnaddUserConfig_Click(sender, e);
                        }
                    }
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_subscriber - Page_Load", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                ScriptManager.RegisterStartupScript(UpdatePanelMessageList, UpdatePanelMessageList.GetType(), "Warning", "<script type=\"text/javascript\">checkReports();</script>", false);
            }

        }

        protected void btnDefault_Click(object sender, System.EventArgs e)
        {
            btnGeneratePassword.Enabled = false;
            ScriptManager.RegisterStartupScript(upnlGroupAffiliation, upnlGroupAffiliation.GetType(), "Warning", "<script type=\"text/javascript\">DisableGenerateButton('" + btnGeneratePassword.ClientID + "');</script>", false);
        }
        /// <summary>
        /// This function is used to Update the profile information for logged in user.
        /// This function calls stored procedure "updateSubscriberInfo" to update information.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateProfile_Click(object sender, System.EventArgs e)
        {
            try
            {
                /*Remove Information profile, group maintenance*/
                int roleID = int.Parse(ddlRole.Items[ddlRole.SelectedIndex].Value);

                /*Validation if the page is postback to save unsaved changes.*/
                string message = validateBeforeSave();

                if (message.Length != 0)
                {
                    //If any error
                    ScriptManager.RegisterClientScriptBlock(upnlGroupAffiliation, upnlGroupAffiliation.GetType(), "Warning4", "alert('" + message + "');", true);
                    return;
                }
                credentialStatus = 0;
                //Update the user profile
                if (ConfirmUpdateRecord())
                {
                    ScriptManager.RegisterClientScriptBlock(upnlGroupAffiliation, upnlGroupAffiliation.GetType(), "Warning1", "alert('Profile Updated Successfully');document.getElementById('" + btnUpdateProfile.ClientID + "').focus();", true);
                }

                if (credentialStatus == -1)
                {
                    txtLoginID.Focus();
                    ScriptManager.RegisterClientScriptBlock(upnlGroupAffiliation, upnlGroupAffiliation.GetType(), "Warning2", "alert('- Please enter different login id. This login id is already used in other group.');document.getElementById('" + txtLoginID.ClientID + "').focus();", true);
                }
                else if (credentialStatus == -2)
                {
                    txtPassword.Focus();
                    ScriptManager.RegisterClientScriptBlock(upnlGroupAffiliation, upnlGroupAffiliation.GetType(), "Warning3", "alert('- Please enter different PIN, This PIN is already used by other subscriber.');document.getElementById('" + txtPassword.ClientID + "').focus();", true);
                }

                //If profile updated then update user report settings
                if (hdnSaveCalled.Value == "true")
                {
                    if (txtEmail.Text.Trim().Length == 0)
                    {
                        cbEmailReports.Checked = false;
                    }
                    int faxLength = txtFaxAreaCode.Text.Trim().Length + txtFaxPrefix.Text.Trim().Length + txtFaxNNNN.Text.Trim().Length;
                    if (faxLength == 0)
                    {
                        cbFaxReports.Checked = false;
                    }

                    if (cbFaxReports.Checked || cbEmailReports.Checked)
                    {
                        bool isDaySelected = cbMonday.Checked || cbTuesday.Checked || cbWednesday.Checked || cbThursday.Checked || cbFriday.Checked || cbSaturday.Checked || cbSunday.Checked;
                        if (!isDaySelected)
                        {
                            cbFaxReports.Checked = cbEmailReports.Checked = false;
                        }
                    }
                    hdnProfileSaved.Value = "true";
                    if (credentialStatus == 0)
                        btnUpdateRptSettings_Click(sender, e);
                }
                if (roleID != UserRole.LabGroupAdmin.GetHashCode() || roleID != UserRole.GroupAdmin.GetHashCode())
                {
                    divNotification.Visible = true;
                    legendNotifation.InnerText = "Notifications";
                }
                //If role get changed then update the frame
                //if (string.Compare(Request["RoleID"], ViewState["RoleID"].ToString()) != 0)
                //    QueryStringtoIframe();

            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_subscriber - btnUpdateProfile_Click", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// This function is to control events when Confirm for Deleting Device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, System.EventArgs e)
        {
            Device device = null;
            try
            {
                /*Remove Information profile, group maintenance*/
                if (txtConfirm.Value == "1")
                {
                    device = new Device();
                    device.DeleteDevices(int.Parse(ViewState["SubscriberID"].ToString()));
                    credentialStatus = 0;
                    ConfirmUpdateRecord();
                    if (credentialStatus == -1)
                        ScriptManager.RegisterClientScriptBlock(upnlGroupAffiliation, upnlGroupAffiliation.GetType(), "Warning", "alert('- Please enter different login id. This login id is already used in other group.');document.getElementById('" + txtLoginID.ClientID + "').focus();", true);
                    else if (credentialStatus == -2)
                        ScriptManager.RegisterClientScriptBlock(upnlGroupAffiliation, upnlGroupAffiliation.GetType(), "Warning", "alert('- Please enter different PIN, This PIN is already used by other subscriber.');document.getElementById('" + txtPassword.ClientID + "').focus();", true);

                    divNotification.Visible = false;
                    legendNotifation.InnerText = "Configuration Settings";
                }
                else if (txtConfirm.Value == "0")
                {
                    Response.Redirect("user_profile.aspx?SubscriberID=" + ViewState["SubscriberID"] + "&RoleID=" + ViewState["RoleID"].ToString() + ";");
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("user_profile - btnConfirm_Click", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                device = null;
            }
        }

        /// <summary>
        /// This function is to control events when selected Role is changed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRole_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (int.Parse(ddlRole.Items[ddlRole.SelectedIndex].Value) == UserRole.Radiologists.GetHashCode())
                {
                    //txtSpecialty.Enabled = true;
                    txtSpecialty.BackColor = System.Drawing.Color.White;
                    //txtAffiliation.Enabled = true;
                    txtAffiliation.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    deactivateFields(false);
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("user_profile - ddlRole_SelectedIndexChanged", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// This function is to generate unique PIN randomly for logged in user.
        /// This function calls stored procedure "getUniquePassword"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGeneratePassword_Click(object sender, System.EventArgs e)
        {
            try
            {
                Subscriber objSubscriber = new Subscriber();
                txtPassword.Text = objSubscriber.GeneratePin(Convert.ToInt32(ViewState["GroupID"].ToString()));
                txtChanged.Value = "true";
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_subscriber - btnGeneratePassword_Click", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                ScriptManager.RegisterClientScriptBlock(upnlGroupAffiliation, upnlGroupAffiliation.GetType(), "Warning", "<script type=\"text/javascript\">alert('Error Generating Password.');</script>", false);
            }
            finally
            {
                ScriptManager.RegisterClientScriptBlock(upnlGroupAffiliation, upnlGroupAffiliation.GetType(), "Warning", "<script type=\"text/javascript\">checkReports();document.getElementById(txtFirstNameClientID).focus();</script>", false);
            }
        }

        /// <summary>
        /// This function is for Update Overdue Reports settings
        /// This function calls stored procedure "updateSubscriberReportSetting"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateRptSettings_Click(object sender, System.EventArgs e)
        {
            bool reportson = false;
            bool error = false;
            string deviceName = "";
            string deviceNumber = "";
            bool isEmailValid = false;
            bool isFaxValid = false;
            string fax = txtFaxAreaCode.Text.Trim() + txtFaxPrefix.Text.Trim() + txtFaxNNNN.Text.Trim();
            Subscriber objSubscriber = null;
            try
            {
                //If control comes after saving profile
                if (string.Compare(hdnProfileSaved.Value, "true") != 0 && string.Compare(txtChanged.Value, "true") == 0)
                {
                    //Validate Profile Values
                    string errorMessage = validateBeforeSave();
                    if (errorMessage.Length == 0)
                    {
                        ConfirmUpdateRecord();

                        isEmailValid = (txtEmail.Text.Trim().Length > 0) ? true : false;
                        isFaxValid = (fax.Length > 0) ? true : false;

                        ViewState["PrimaryEmail"] = txtEmail.Text.Trim();
                        ViewState["fax"] = fax;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "ReportUpdated", "<script type=\"text/javascript\">alert('" + errorMessage + "');document.getElementById('" + txtFaxAreaCode.ClientID + "').focus();</script>", false);
                        return;
                    }
                }
                else
                {
                    isEmailValid = ((ViewState["PrimaryEmail"] != null) && (ViewState["PrimaryEmail"].ToString().Length > 0)) ? true : false;
                    isFaxValid = ((ViewState["fax"] != null) && (ViewState["fax"].ToString().Length > 0)) ? true : false;
                }


                if (cbEmailReports.Checked)
                {
                    if (!isEmailValid)
                    {
                        deviceName = "Email";
                        deviceNumber = "Email ID";
                        error = true;
                    }
                }
                if (cbFaxReports.Checked)
                {
                    if (!isFaxValid)
                    {
                        if (deviceName.Length > 0)
                            deviceName += " & fax";
                        else
                            deviceName = "fax";

                        if (deviceNumber.Length > 0)
                            deviceNumber += " & fax number";
                        else
                            deviceNumber = "fax number";
                        error = true;
                    }
                }
                //If any Error return with appropriate message
                if (error)
                {
                    string focusON = "";
                    if (deviceName.Contains("Email"))
                        focusON = "document.getElementById('" + txtEmail.ClientID + "').focus();";
                    else
                        focusON = "document.getElementById('" + txtFaxAreaCode.ClientID + "').focus();";
                    ScriptManager.RegisterClientScriptBlock(upnlMsgConfig, upnlMsgConfig.GetType(), "ReportUpdated", "<script type=\"text/javascript\">alert('To send Outstanding Messages Reports by " + deviceName + ", you must have " + deviceNumber + " into Profile section.');" + focusON + "</script>", false);
                    return;
                }

                //If all Well Save Record.
                reportson = true;
                int reportOnDays = 0;
                if (cbEmailReports.Checked || cbFaxReports.Checked)
                {
                    if (cbMonday.Checked)
                        reportOnDays |= ReportDay.Monday.GetHashCode();
                    if (cbTuesday.Checked)
                        reportOnDays |= ReportDay.Tuesday.GetHashCode();
                    if (cbWednesday.Checked)
                        reportOnDays |= ReportDay.Wednesday.GetHashCode();
                    if (cbThursday.Checked)
                        reportOnDays |= ReportDay.Thursday.GetHashCode();
                    if (cbFriday.Checked)
                        reportOnDays |= ReportDay.Friday.GetHashCode();
                    if (cbSaturday.Checked)
                        reportOnDays |= ReportDay.Saturday.GetHashCode();
                    if (cbSunday.Checked)
                        reportOnDays |= ReportDay.Sunday.GetHashCode();
                }

                objSubscriber = new Subscriber();
                objSubscriber.updateReportSetting(int.Parse(ViewState["SubscriberID"].ToString()),
                    reportOnDays, int.Parse(ddlReportTime.SelectedItem.Value),
                    cbEmailReports.Checked, cbFaxReports.Checked);

                if (hdnSaveCalled.Value != "true")
                {
                    ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "ReportUpdated", "<script type=\"text/javascript\">checkReports();alert('Reports Updated Successfully.');document.getElementById('" + btnUpdateRptSettings.ClientID + "').focus();</script>", false);
                }
                fillUserInformation();
                btnSendOverdueRpt.Enabled = true;

            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("user_profile - btnUpdateRptSettings_Click", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                txtChanged.Value = "false";
                ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "setVarFalse", "document.getElementById('" + txtChanged.ClientID + "').value= 'false';checkReports();", true);
                objSubscriber = null;
            }
        }

        /// <summary>
        /// This function is to send overdue reports.
        /// This function calls VoiceLinkR1Reports to send overdue message to logged in users.
        /// if Any required field is required to send report then validation is taken care in this function
        /// If sending overdue report failed then this function gives message that sending report is failed try again later.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSendOverdueRpt_Click(object sender, System.EventArgs e)
        {
            VoiceLinkR1Reports voiceLinkR1Reports = null;
            try
            {
                voiceLinkR1Reports = new VoiceLinkR1Reports();
                if (cbEmailReports.Checked && (cbFaxReports.Checked))
                {
                    if ((ViewState["PrimaryEmail"] != null) && (ViewState["PrimaryEmail"].ToString().Length > 0) && (ViewState["fax"] != null) && (ViewState["fax"].ToString().Length > 0))
                    {
                        voiceLinkR1Reports.sendOverdueReport(int.Parse(ViewState["SubscriberID"].ToString()));
                        ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "OverdueMessage", "<script type=\"text/javascript\">alert('Report Sent Successfully');</script>", false);
                    }
                    else if ((ViewState["PrimaryEmail"] != null) && (ViewState["PrimaryEmail"].ToString().Length > 0) && (ViewState["fax"].ToString().Length == 0))// && (ViewState["fax"].ToString().Length == 0))
                    {
                        ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "OverdueMessage", "<script type=\"text/javascript\">alert('Fax number in Profile Section should not be blank.');document.getElementById('" + txtFaxAreaCode.ClientID + "').focus();</script>", false);
                    }
                    else if ((ViewState["PrimaryEmail"].ToString().Length == 0) && (ViewState["fax"] != null) && (ViewState["fax"].ToString().Length > 0))
                    {
                        ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "OverdueMessage", "<script type=\"text/javascript\">alert('Email Address in Profile Section should not be blank.');document.getElementById('" + txtEmail.ClientID + "').focus();</script>", false);
                    }
                    else if ((ViewState["PrimaryEmail"] == null) && (ViewState["fax"] == null))
                    {
                        ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "OverdueMessage", "<script type=\"text/javascript\">alert('Email Address and Fax in Profile Section should not be blank.');document.getElementById('" + txtFaxAreaCode.ClientID + "').focus();</script>", false);
                    }
                    else if ((ViewState["PrimaryEmail"].ToString().Length == 0) && (ViewState["fax"] == null))
                    {
                        ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "OverdueMessage", "<script type=\"text/javascript\">alert('Email Address and Fax in Profile Section should not be blank.');document.getElementById('" + txtFaxAreaCode.ClientID + "').focus();</script>", false);
                    }
                    else if ((ViewState["PrimaryEmail"].ToString().Length == 0) && (ViewState["fax"].ToString().Length == 0))
                    {
                        ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "OverdueMessage", "<script type=\"text/javascript\">alert('Email Address and Fax in Profile Section should not be blank.');document.getElementById('" + txtFaxAreaCode.ClientID + "').focus();</script>", false);
                    }
                }
                else if (cbEmailReports.Checked && (!cbFaxReports.Checked))
                {
                    if ((ViewState["PrimaryEmail"] != null) && (ViewState["PrimaryEmail"].ToString().Length > 0))
                    {
                        voiceLinkR1Reports.sendOverdueReport(int.Parse(ViewState["SubscriberID"].ToString()));
                        ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "OverdueMessage", "<script type=\"text/javascript\">alert('Report Sent Successfully');</script>", false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "OverdueMessage", "<script type=\"text/javascript\">alert('Email Address in Profile Section should not be blank.');document.getElementById('" + txtEmail.ClientID + "').focus();</script>", false);
                    }
                }
                else if ((!cbEmailReports.Checked) && (cbFaxReports.Checked))
                {
                    if ((ViewState["fax"] != null) && (ViewState["fax"].ToString().Length > 0))
                    {
                        voiceLinkR1Reports.sendOverdueReport(int.Parse(ViewState["SubscriberID"].ToString()));
                        ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "OverdueMessage", "<script type=\"text/javascript\">alert('Report Sent Successfully');</script>", false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "OverdueMessage", "<script type=\"text/javascript\">alert('Fax number in Profile Section should not be blank.');document.getElementById('" + txtFaxAreaCode.ClientID + "').focus();</script>", false);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "OverdueMessage", "<script type=\"text/javascript\">checkReports();alert('Select Send Report By.');</script>", false);
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("user_profile - btnSendOverdueRpt_Click", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "OverdueMessage", "<script type=\"text/javascript\">alert('Error Sending Report.');</script>", false);
            }
            finally
            {
                voiceLinkR1Reports = null;
                ScriptManager.RegisterClientScriptBlock(upnlOutMessages, upnlOutMessages.GetType(), "Warning", "<script type=\"text/javascript\">checkReports();</script>", false);
            }
        }

        /// <summary>
        /// This function Updates User Configuration settings.
        /// This function calls stored procedure "VOC_VLR_updateUserConfigurationDataForSubscriber"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnaddUserConfig_Click(object sender, EventArgs e)
        {
            Subscriber userProfile = null;
            try
            {
                userProfile = new Subscriber();
                int numOfDays = int.Parse(txtNoofDays.Text);

                if (numOfDays > 0 && numOfDays < 31)
                {
                    userProfile.UpdateUserConfigurationInfo(int.Parse(ViewState["SubscriberID"].ToString()), int.Parse(txtNoofDays.Text));
                    ScriptManager.RegisterClientScriptBlock(upnlMsgConfig, upnlMsgConfig.GetType(), "ConfigUpdated", "<script type=\"text/javascript\">alert('Configuration Updated Successfully.');ChangeFlag();</script>", false);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(upnlMsgConfig, upnlMsgConfig.GetType(), "ConfigUpdated", "<script type=\"text/javascript\">alert('Show all Closed messages since last days should  be between 1 and 30.');</script>", false);
                }

            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("user_profile - dgAfterHoursNotifications_DeleteCommand", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
            }
            finally
            {
                userProfile = null;
            }
        }

        #region Step1
        /// <summary>
        /// clear label lblDeviceAlreadyExists while text change event of Devices combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbDevices_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblDeviceAlreadyExists.Text = "";
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - cmbDevices_TextChanged", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// This function control the events when Device drop down list selection changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbDevices_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (grdDevices.EditItemIndex == -1)
                {
                    lblDeviceAlreadyExists.Text = "";
                    int deviceID = int.Parse(cmbDevices.SelectedItem.Value);
                    setLabelsAndInputBoxes(deviceID);
                    if (deviceID == NotificationDevice.SMS.GetHashCode() || deviceID == NotificationDevice.PagerAlpha.GetHashCode())  // cell or pager.
                        generateGatewayAddress();

                    if (cmbDevices.Items.Count > 0)
                    {
                        if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerTAP || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerTAPA)
                        {
                            txtDeviceAddress.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes();");
                            txtGateway.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes();");
                            //txtDeviceAddress.MaxLength = 6;
                            //txtGateway.MaxLength = 10;
                            txtGateway.Visible = true;
                        }
                        else if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerNumRegular || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.pagerPartner || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerNumSkyTel || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerNumUSA)
                        {
                            txtDeviceAddress.Attributes.Add("onkeyPress", "JavaScript:return PagerValidationWithSpace('" + txtDeviceAddress.ClientID + "');");
                            //txtDeviceAddress.MaxLength = 100;
                        }
                        else if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerAlpha)
                        {
                            txtDeviceAddress.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes();");
                            //txtDeviceAddress.MaxLength = 100;
                        }
                        else if (int.Parse(cmbDevices.SelectedItem.Value) != (int)NotificationDevice.EMail)
                        {
                            txtDeviceAddress.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes();");
                            txtDeviceAddress.MaxLength = 10;
                        }
                        else
                        {
                            txtDeviceAddress.Attributes.Add("onkeyPress", "return true");
                            txtDeviceAddress.Attributes.Add("onchange", "return true");
                            txtDeviceAddress.MaxLength = 100;
                        }

                        if (int.Parse(cmbDevices.SelectedItem.Value) == NotificationDevice.PagerTAP.GetHashCode() || int.Parse(cmbDevices.SelectedItem.Value) == NotificationDevice.PagerTAPA.GetHashCode())
                        {
                            txtGateway.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
                            //txtGateway.MaxLength = 10;
                        }
                        else
                        {
                            txtGateway.Attributes.Add("onkeypress", "JavaScript:return true;");
                            txtGateway.MaxLength = 100;
                        }
                    }
                }
                else
                {
                    cmbDevices.SelectedValue = "-1";
                    addLinkToGridInEditMode(grdDevices.Items[grdDevices.EditItemIndex]);
                }

                generateDataGridHeight();

            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - cmbDevices_SelectedIndexChanged", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// This method check if the dllCarrier is selected or not if selected then 
        /// generates the GatewayAddress.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtDeviceAddress_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (cmbCarrier.Items.Count > 0 && cmbCarrier.SelectedItem.Value.Equals("-1"))
                    return;

                generateGatewayAddress();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - txtDeviceAddress_TextChanged", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// This function is to delete records from dgDevice datagrid.
        /// After deleting records it refreshes profile page so that effect will reflect immediate into profile page.
        /// IF deleting records is associated into Notification Events then this function will show message.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDevices_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                int rowsUpdated = deleteSubscriberDevice(int.Parse(e.Item.Cells[0].Text.Trim()), int.Parse(e.Item.Cells[1].Text.Trim()));

                if (rowsUpdated == 1)
                {
                    NoDelete = true;
                    grdDevices.SelectedIndex = -1;
                }
                grdDevices.EditItemIndex = -1;
                fillDevices();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - grdDevices_DeleteCommand", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "Warning", "<script language='javascript'>alert('Error while Deleting Subscriber Notification Device.');</script>", false);
            }
            finally
            {

                if (NoDelete == true)
                {
                    lblDeviceAlreadyExists.Text = "Warning: This device is associated with notification events (Step 2). You must first delete notification in Step2 before you delete this device.";
                }
                else
                {
                    lblDeviceAlreadyExists.Text = "";
                    fillSubscriberAfterHoursDeviceOptions();
                    generateStep3DataGridHeight();
                    upnlStep3.Update();
                }
            }
        }

        /// <summary>
        /// generate GateWay Address for Cell Phone carriers & Pager Carriers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCarrier_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (cmbCarrier.SelectedItem.Value.Equals("-1"))
                {
                    txtGateway.Text = "";
                    return;
                }
                generateGatewayAddress();
                generateDataGridHeight();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - cmbCarrier_SelectedIndexChanged", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// This function is to call edit command for datagrid grdDevices
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDevices_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                lblDeviceAlreadyExists.Text = "";

                grdDevices.EditItemIndex = e.Item.ItemIndex;
                fillDevices();

                TextBox tbName = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[2].FindControl("txtDeviceName")));
                TextBox tbEmailGateway = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[5].FindControl("txtGateway")));
                TextBox tbAddress = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[3].FindControl("txtDeviceAddress")));
                int deviceType = int.Parse(grdDevices.Items[e.Item.ItemIndex].Cells[11].Text);
                DropDownList dlEvent = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[6].FindControl("dlistGridEvents")));
                DropDownList dlFinding = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[7].FindControl("dlistGridFindings")));
                string eventID = grdDevices.Items[e.Item.ItemIndex].Cells[12].Text.Trim();
                string findingID = grdDevices.Items[e.Item.ItemIndex].Cells[13].Text.Trim();

                ViewState[Constants.DEVICE_ADDRESS] = tbAddress.Text.Trim();
                ViewState[Constants.EMAIL_GATEWAY] = tbEmailGateway.Text.Trim();

                if (deviceType == NotificationDevice.PagerTAP.GetHashCode() || deviceType == NotificationDevice.PagerTAPA.GetHashCode())
                {
                    tbAddress.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
                    //tbAddress.MaxLength = 6;
                    tbEmailGateway.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
                    //tbEmailGateway.MaxLength = 10;
                }
                else if (deviceType == (int)NotificationDevice.pagerPartner || deviceType == (int)NotificationDevice.PagerNumSkyTel || deviceType == (int)NotificationDevice.PagerNumUSA || deviceType == (int)NotificationDevice.PagerNumRegular)
                {
                    tbAddress.Attributes.Add("onkeyPress", "JavaScript:return PagerValidationWithSpace('" + tbAddress.ClientID + "');");
                    //tbAddress.MaxLength = 100;
                }
                else if (deviceType == (int)NotificationDevice.PagerAlpha)
                {
                    tbAddress.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
                    //tbAddress.MaxLength = 100;
                }
                else if (deviceType != (int)NotificationDevice.EMail)
                {
                    tbAddress.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
                    tbAddress.MaxLength = 10;
                    tbEmailGateway.Attributes.Add("onkeypress", "JavaScript:return true;");
                    tbEmailGateway.MaxLength = 100;
                }
                else
                {
                    tbAddress.Attributes.Add("onkeypress", "JavaScript:return true;");
                    tbAddress.MaxLength = 100;
                    tbEmailGateway.Attributes.Add("onkeypress", "JavaScript:return true;");
                    tbEmailGateway.MaxLength = 100;
                }

                if (tbEmailGateway.Text.Trim().Length <= 0)
                {
                    tbEmailGateway.Visible = false;
                }

                fillEvents(dlEvent);
                dlEvent.SelectedValue = eventID;

                fillFindings(dlFinding);
                dlFinding.SelectedValue = findingID;

                resetControls();
                hdnIsAddClicked.Value = "false";

                if (tbName != null)
                    ScriptManager.GetCurrent(this).SetFocus(tbName.ClientID);
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - grdDevices_EditCommand", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// This function calls DataBind() function for datagrid dgNotification to bind it dynamically
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDevices_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView data = (DataRowView)e.Item.DataItem;
                    if (e.Item.ItemType != ListItemType.EditItem)
                    {
                        if ((int)data.Row["SubscriberNotifyEventID"] == -1)
                        {
                            ((Label)(e.Item.Cells[6].FindControl("lblGridDeviceEvent"))).Text = "All Events";
                        }
                        if ((int)data.Row["FindingID"] == -1)
                        {
                            ((Label)(e.Item.Cells[7].FindControl("lblGridDeviceFinding"))).Text = "All Findings";
                        }
                    }
                }

                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    addLinkToGridInEditMode(e.Item);
                }
            }
            catch (Exception ex)
            {
                if (ViewState["SubscriberID"] != null)
                    Tracer.GetLogger().LogInfoEvent("profile_notificationStep1.grdDevices_ItemDataBound:: Exception occured for User ID - " + ViewState["SubscriberID"].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState["SubscriberID"]));
                throw ex;
            }
        }

        /// <summary>
        /// change grid mode from edit to view mode 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDevices_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                lblDeviceAlreadyExists.Text = "";
                grdDevices.EditItemIndex = -1;
                hdnIsAddClicked.Value = "false";
                fillDevices();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - grdDevices_CancelCommand", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "RefreshParent", "document.location.reload(true); ", true);
                //if (ViewState["SubscriberID"] != null)
                //    ClientScript.RegisterStartupScript(this.GetType(), "RefreshParent", "RefreshParent('" + ViewState["SubscriberID"].ToString() + "');", true);
            }
        }

        /// <summary>
        /// This function Update records of grdDevices datgrid.
        /// For updation this function calls stored procedure "updateSubscriberDevice"
        /// After updating this function refresh this page so that newly updated records will be available into profie page
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDevices_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            Device device = null;
            bool error = false;
            string regScript = "";
            string isAddClicked = hdnIsAddClicked.Value;
            try
            {
                if (!validateRecord(e))
                {
                    addLinkToGridInEditMode(grdDevices.Items[grdDevices.EditItemIndex]);
                    error = true;
                    return;
                }
                /*NKV  -  2006/08/25 - Phase 2 Iteration 2 - Editing a notification device ID*/
                TextBox deviceNameTextBox = (TextBox)e.Item.Cells[2].Controls[1];
                TextBox deviceAddressTextBox = (TextBox)e.Item.Cells[3].Controls[1];
                TextBox gatewayTextBox = (TextBox)e.Item.Cells[5].Controls[1];
                DropDownList cmbGrdEvents = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[6].FindControl("dlistGridEvents")));
                DropDownList cmbGrdFindings = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[7].FindControl("dlistGridFindings")));

                if (deviceNameTextBox.Text.Trim().StartsWith("PagerA_") || deviceNameTextBox.Text.Trim().StartsWith("SMS_"))
                {
                    string oldDeviceAddress = ViewState[Constants.DEVICE_ADDRESS].ToString();
                    string oldEmailGateway = ViewState[Constants.EMAIL_GATEWAY].ToString();
                    if (deviceAddressTextBox.Text.Trim() != oldDeviceAddress && gatewayTextBox.Text.Trim() == oldEmailGateway)
                    {
                        int index = oldEmailGateway.IndexOf("@");
                        if (index > -1)
                        {
                            string oldGatewaydeviceNum = oldEmailGateway.Substring(0, oldEmailGateway.IndexOf("@"));

                            if (oldDeviceAddress == oldGatewaydeviceNum)
                            {
                                string deviceAdd = gatewayTextBox.Text.Trim().Substring(0, gatewayTextBox.Text.Trim().IndexOf("@"));
                                if (deviceAdd.Length > 0)
                                    gatewayTextBox.Text = gatewayTextBox.Text.Trim().Replace(deviceAdd, deviceAddressTextBox.Text.Trim());
                                else
                                    gatewayTextBox.Text = deviceAddressTextBox.Text.Trim() + gatewayTextBox.Text.Trim();
                            }
                        }
                    }

                }
                device = new Device();
                if (isAddClicked == "false")
                {
                    int result = device.UpdateSubscriberDevice(int.Parse(e.Item.Cells[0].Text.Trim()), deviceNameTextBox.Text.Trim(),
                                                    deviceAddressTextBox.Text.Trim(), gatewayTextBox.Text.Trim(), Convert.ToInt32(grdDevices.DataKeys[e.Item.ItemIndex]),
                                                    Convert.ToInt32(cmbGrdEvents.SelectedValue), Convert.ToInt32(cmbGrdFindings.SelectedValue), int.Parse(e.Item.Cells[1].Text.Trim()));

                    if (result == -1)
                    {
                        regScript = "Device name already exists.";
                        addLinkToGridInEditMode(grdDevices.Items[grdDevices.EditItemIndex]);
                    }
                    else
                    {
                        regScript += "Device has been updated";
                        grdDevices.EditItemIndex = -1;
                        fillDevices();
                        ViewState[Constants.DEVICE_ADDRESS] = null;
                        ViewState[Constants.EMAIL_GATEWAY] = null;
                    }
                }
                else
                {
                    device.InsertSubscriberDevice(int.Parse(ViewState["SubscriberID"].ToString()), Convert.ToInt32(e.Item.Cells[11].Text.Trim()),
                                 deviceAddressTextBox.Text.Trim(), gatewayTextBox.Text.Trim(), e.Item.Cells[4].Text.Trim().ToString(),
                                 Convert.ToInt32(cmbGrdEvents.SelectedValue), Convert.ToInt32(cmbGrdFindings.SelectedValue));
                    grdDevices.EditItemIndex = -1;
                    fillDevices();
                    ViewState[Constants.DEVICE_ADDRESS] = null;
                    ViewState[Constants.EMAIL_GATEWAY] = null;
                    generateStep3DataGridHeight();
                    upnlStep3.Update();
                    regScript += "document.getElementById(" + '"' + ProfileDevicesDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';";
                    regScript += "alert('Device has been added');";
                    ScriptManager.RegisterStartupScript(upnlStep3, upnlStep3.GetType(), "RefreshParent", regScript, true);
                }

            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - grdDevices_UpdateCommand", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "Warning", "alert('Error while Updating Subscriber Notification Device.');", true);
            }
            finally
            {
                hdnIsAddClicked.Value = "false";
                device = null;
                generateDataGridHeight();
                if (!error)
                {
                    fillAfterHoursNotifications();
                    fillSubscriberAfterHoursDeviceOptions();
                    generateStep3DataGridHeight();
                    upnlStep3.Update();
                    ScriptManager.RegisterStartupScript(upnlStep3, upnlStep3.GetType(), "DeviceExists", "alert('" + regScript + "');", true);
                }
            }
        }

        /// <summary>
        /// This Event is used to Show / Hide Notification details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowHideDetails_Click(object sender, System.EventArgs e)
        {
            try
            {
                string btnName = btnShowHideDetails.Text.ToUpper();
                bool showBtn = (btnName.Equals(SHOWDETAILS_BUTTONNAME.ToUpper())) ? true : false;
                btnShowHideDetails.Text = (showBtn) ? HIDEDETAILS_BUTTONNAME : SHOWDETAILS_BUTTONNAME;
                cmbEvent.Visible = showBtn;
                cmbFinding.Visible = showBtn;
                cmbEvent.SelectedValue = "-1";
                cmbFinding.SelectedValue = "-1";
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - btnShowHideDetails_Click", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                generateDataGridHeight();
            }
        }

        /// <summary>
        /// This function add new Devices into device list for grdDevices datagrid.
        /// This function call stored procedure "insertSubscriberDevice"
        /// After adding new device into list this function refresh profile page so that newly
        /// added record will exist on immediate effect into profile page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddDevice_Click(object sender, System.EventArgs e)
        {
            Device device = null;
            string carrier = null;
            string deviceAddress;
            string gateway = null;
            bool success = false;
            try
            {

                if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerAlpha || (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerNumRegular) ||
                    int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerNumSkyTel || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerNumUSA || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.pagerPartner || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerTAP
                     || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerTAPA)
                {

                    if ((Utils.RegExNumericMatch(txtDeviceAddress.Text.Trim())) == false)
                    {
                        //"Alphanumeric characters in pager number"
                        StringBuilder acRegScript = new StringBuilder();
                        acRegScript.Append("<script type=\"text/javascript\">");
                        acRegScript.AppendFormat("document.getElementById(" + '"' + ProfileDevicesDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                        acRegScript.AppendFormat("alert('Please enter valid pager number');");
                        acRegScript.Append("</script>");
                        ScriptManager.RegisterClientScriptBlock(upnlStep1, upnlStep1.GetType(), "Register", acRegScript.ToString(), false);
                        generateDataGridHeight();
                        return;
                    }

                    if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerTAP || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerTAPA)
                    {
                        if ((Utils.RegExNumericMatch(txtGateway.Text.Trim())) == false)
                        {
                            //"Alphanumeric characters in pager tap number"
                            StringBuilder acRegScript = new StringBuilder();
                            acRegScript.Append("<script type=\"text/javascript\">");
                            acRegScript.AppendFormat("document.getElementById(" + '"' + ProfileDevicesDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                            acRegScript.AppendFormat("alert('Please enter valid pager number');");
                            acRegScript.Append("</script>");
                            ScriptManager.RegisterClientScriptBlock(upnlStep1, upnlStep1.GetType(), "Register", acRegScript.ToString(), false);
                            generateDataGridHeight();
                            return;
                        }
                    }
                }

                if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.EMail)
                {
                    if ((Utils.RegExMatch(txtDeviceAddress.Text.Trim())) == false)
                    {
                        //"Email format incorrect"
                        StringBuilder acRegScript = new StringBuilder();
                        acRegScript.Append("<script type=\"text/javascript\">");
                        acRegScript.AppendFormat("document.getElementById(" + '"' + ProfileDevicesDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                        acRegScript.AppendFormat("alert('Please enter valid Email ID.');");
                        acRegScript.Append("</script>");
                        ScriptManager.RegisterClientScriptBlock(upnlStep1, upnlStep1.GetType(), "Register", acRegScript.ToString(), false);
                        generateDataGridHeight();
                        return;
                    }

                }
                else if (int.Parse(cmbDevices.SelectedItem.Value) == NotificationDevice.PagerTAP.GetHashCode() || int.Parse(cmbDevices.SelectedItem.Value) == NotificationDevice.PagerTAPA.GetHashCode())
                {
                    string pin = txtDeviceAddress.Text.Trim();
                    string tap800num = txtGateway.Text.Trim();
                    string error = "";
                    string taptext = "Enter TAP 800 number (numbers only)";
                    string pintext = "Enter PIN number (numbers only)";
                    //if (!Utils.isNumericValue(pin.Trim()))
                    if ((pin.Trim() == "") || (pin.Trim() == pintext.Trim()))
                    {
                        error = "Please enter PIN Number." + @"\n";
                    }
                    //if (!Utils.isNumericValue(tap800num.Trim()))
                    if ((tap800num.Trim() == "") || (tap800num.Trim() == taptext.Trim()))
                    {
                        error += "Please enter TAP 800 Number.";
                    }

                    if (error.Length > 0)
                    {
                        StringBuilder acRegScript = new StringBuilder();
                        acRegScript.Append("<script type=\"text/javascript\">");
                        acRegScript.AppendFormat("document.getElementById(" + '"' + ProfileDevicesDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                        acRegScript.AppendFormat("alert('" + error + "');");
                        acRegScript.Append("</script>");
                        ScriptManager.RegisterClientScriptBlock(upnlStep1, upnlStep1.GetType(), "Register", acRegScript.ToString(), false);
                        generateDataGridHeight();
                        return;
                    }
                }

                else
                {
                    string deviceAdd = txtDeviceAddress.Text.Trim();
                    if (int.Parse(cmbDevices.SelectedItem.Value) != (int)NotificationDevice.EMail)
                    {
                        string error = "";
                        if (int.Parse(cmbDevices.SelectedItem.Value) != (int)NotificationDevice.PagerAlpha && int.Parse(cmbDevices.SelectedItem.Value) != (int)NotificationDevice.pagerPartner && int.Parse(cmbDevices.SelectedItem.Value) != (int)NotificationDevice.PagerNumRegular && int.Parse(cmbDevices.SelectedItem.Value) != (int)NotificationDevice.PagerNumSkyTel && int.Parse(cmbDevices.SelectedItem.Value) != (int)NotificationDevice.PagerNumUSA)
                        {
                            if (!Utils.isNumericValue(deviceAdd))
                            {
                                error = "Please enter valid Number\\n";
                            }
                        }
                        if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.SMS || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.SMS_WebLink || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.Fax)
                        {
                            if (deviceAdd.Length != 10)
                            {
                                error += "Please enter valid Number\\n";
                            }
                        }
                        if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerAlpha || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.SMS)
                        {
                            if (txtGateway.Text.Trim().Length == 0)
                            {
                                error += "Please enter Email Gateway Address\\n";
                            }
                            else if ((Utils.RegExMatch(txtGateway.Text.Trim())) == false)
                            {
                                error += "Please enter valid Email ID\\n";
                            }
                        }

                        if (error.Length > 0)
                        {
                            StringBuilder acRegScript = new StringBuilder();
                            acRegScript.Append("<script type=\"text/javascript\">");
                            acRegScript.AppendFormat("document.getElementById(" + '"' + ProfileDevicesDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                            acRegScript.AppendFormat("alert('" + error + "');");
                            acRegScript.Append("</script>");
                            ScriptManager.RegisterClientScriptBlock(upnlStep1, upnlStep1.GetType(), "Register", acRegScript.ToString(), false);
                            generateDataGridHeight();
                            return;
                        }
                    }
                }
                deviceAddress = txtDeviceAddress.Text.Trim();
                if (cmbDevices.Items.Count > 0 && cmbDevices.SelectedItem.Text.Trim().Equals("Email"))
                    deviceAddress = txtGateway.Text.Trim();

                if (!(cmbDevices.Items.Count > 0 && cmbDevices.SelectedItem.Text.Trim().Equals("Email")))
                    gateway = txtGateway.Text.Trim();

                if (cmbCarrier.Items.Count > 0 && cmbCarrier.SelectedItem.Text.Trim() != "-- Select Carrier")
                    carrier = cmbCarrier.SelectedItem.Text.Trim();

                device = new Device();
                device.InsertSubscriberDevice(int.Parse(ViewState["SubscriberID"].ToString()), int.Parse(cmbDevices.SelectedItem.Value),
                                                 txtDeviceAddress.Text.Trim(), txtGateway.Text.Trim(), carrier,
                                                 (cmbEvent.Visible) ? Convert.ToInt32(cmbEvent.SelectedValue) : 0,
                                                 (cmbFinding.Visible) ? Convert.ToInt32(cmbFinding.SelectedValue) : 0);

                success = true;
                fillDevices();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - btnAddDevice_Click", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "Warning", "<script language='javascript'>alert('Error while Adding Subscriber Notification Device.');</script>", false);
            }
            finally
            {
                device = null;
                lblNoRecordsStep1.Visible = false;
                if (ViewState["SubscriberID"] != null)
                {
                    if (success)
                    {
                        setLabelsAndInputBoxes(-1);
                        fillDevices();

                        fillSubscriberAfterHoursDeviceOptions();
                        generateStep3DataGridHeight();
                        upnlStep3.Update();
                        string regScript = "";
                        regScript += "document.getElementById(" + '"' + ProfileDevicesDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';";
                        regScript += "alert('Device has been added');";
                        ScriptManager.RegisterStartupScript(upnlStep3, upnlStep3.GetType(), "RefreshParent", regScript, true);


                    }
                }
            }
        }

        #endregion Step1

        #region Step3
        /// <summary>
        /// This function is to add Notification Events for Fill After Hours.
        /// This function Refersh itself after adding new record into datagrid grdAfterHoursNotifications
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddAH_Click(object sender, System.EventArgs e)
        {
            Device device = null;
            bool retValue = true;
            try
            {
                if (cmbAHDevice.Items.Count > 0)
                {
                    device = new Device();
                    device.InsertSubscriberAHNotification(int.Parse(ViewState[SUBSCRIBER_ID].ToString()), int.Parse(cmbAHDevice.SelectedItem.Value),
                                                          int.Parse(cmbAHFindings.SelectedItem.Value), int.Parse(cmbAHStartHour.SelectedItem.Value),
                                                          int.Parse(cmbAHEndHour.SelectedItem.Value));
                }
                else
                    retValue = false;

                fillAfterHoursNotifications();
            }
            catch (Exception ex)
            {
                if (ViewState[SUBSCRIBER_ID] != null)
                    Tracer.GetLogger().LogInfoEvent("profile_notificationStep3.btnAddAH_Click:: Exception occured for User ID - " + ViewState[SUBSCRIBER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState[SUBSCRIBER_ID]));
                ScriptManager.RegisterStartupScript(upnlStep3, upnlStep3.GetType(), "Warning", "<script language='javascript'>alert('Error while Adding After Hours Notification.');</script>", false);
            }
            finally
            {
                device = null;
                if (retValue)
                {
                    string regScript = "";
                    regScript = "alert('Device has been added');";
                    ScriptManager.RegisterStartupScript(upnlStep3, upnlStep3.GetType(), "RefreshParent", regScript, true);
                }
                //Response.Redirect("profile_notificationStep3.aspx?SubscriberID=" + Request[SUBSCRIBER_ID].ToString());
            }
        }

        /// <summary>
        /// This function deletes records from datagrid grdAfterHoursNotifications
        /// This function calls stored procedure "deleteSubscriberAfterHoursNotification"
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdAfterHoursNotifications_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            Device device = null;
            try
            {
                device = new Device();
                device.DeleteAfterHoursNotification(int.Parse(e.Item.Cells[0].Text));
                fillAfterHoursNotifications();
            }
            catch (Exception ex)
            {
                if (ViewState[SUBSCRIBER_ID] != null)
                    Tracer.GetLogger().LogInfoEvent("profile_notificationStep3.grdAfterHoursNotifications_DeleteCommand:: Exception occured for User ID - " + ViewState[SUBSCRIBER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState[SUBSCRIBER_ID]));
                ScriptManager.RegisterStartupScript(upnlStep3, upnlStep3.GetType(), "Warning", "<script language='javascript'>alert('Error while Deleting After Hours Notification.');</script>", false);
            }
            finally
            {
                device = null;
                //Response.Redirect("profile_notificationStep3.aspx?SubscriberID=" + Request[SUBSCRIBER_ID].ToString());
            }
        }

        /// <summary>
        /// This function calls DataBind() function for grdAfterHoursNotifications dynamically
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAfterHoursNotifications_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.Cells[2].Text == "&nbsp;")
                        e.Item.Cells[2].Text = "All Findings";

                    if (int.Parse(e.Item.Cells[3].Text) == 12)
                        e.Item.Cells[3].Text = "12 Noon";
                    else if (int.Parse(e.Item.Cells[3].Text) == 0)
                        e.Item.Cells[3].Text = "12 Midnight";
                    else if (int.Parse(e.Item.Cells[3].Text) > 12)
                        e.Item.Cells[3].Text = (int.Parse(e.Item.Cells[3].Text) - 12) + " pm";
                    else
                        e.Item.Cells[3].Text += " am";

                    if (int.Parse(e.Item.Cells[4].Text) == 12)
                        e.Item.Cells[4].Text = "12 Noon";
                    else if (int.Parse(e.Item.Cells[4].Text) == 0)
                        e.Item.Cells[4].Text = "12 Midnight";
                    else if (int.Parse(e.Item.Cells[4].Text) > 12)
                        e.Item.Cells[4].Text = (int.Parse(e.Item.Cells[4].Text) - 12) + " pm";
                    else
                        e.Item.Cells[4].Text += " am";
                }
            }
            catch (Exception ex)
            {
                if (ViewState[SUBSCRIBER_ID] != null)
                    Tracer.GetLogger().LogInfoEvent("profile_notificationStep3.grdAfterHoursNotifications_ItemDataBound:: Exception occured for User ID - " + ViewState[SUBSCRIBER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState[SUBSCRIBER_ID]));
                ScriptManager.RegisterStartupScript(upnlStep3, upnlStep3.GetType(), "Warning", "<script language='javascript'>alert('Error while Deleting After Hours Notification.');</script>", false);
            }
        }

        #endregion Step3
        #endregion Events

        #region Private Methods
        /// <summary>
        /// Load JavaScript for page
        /// </summary>
        private void loadJavaScript()
        {
            //Create JS Variables
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var txtChangedClientID = '" + txtChanged.ClientID + "';");
            sbScript.Append("var ddlReportTimeClientID = '" + ddlReportTime.ClientID + "';");
            sbScript.Append("var cbEmailReportsClientID = '" + cbEmailReports.ClientID + "';");
            sbScript.Append("var cbFaxReportsClientID = '" + cbFaxReports.ClientID + "';");
            sbScript.Append("var cbMondayClientID = '" + cbMonday.ClientID + "';");
            sbScript.Append("var cbTuesdayClientID = '" + cbTuesday.ClientID + "';");
            sbScript.Append("var cbWednesdayClientID = '" + cbWednesday.ClientID + "';");
            sbScript.Append("var cbThursdayClientID = '" + cbThursday.ClientID + "';");
            sbScript.Append("var cbFridayClientID = '" + cbFriday.ClientID + "';");
            sbScript.Append("var cbSaturdayClientID = '" + cbSaturday.ClientID + "';");
            sbScript.Append("var cbSundayClientID = '" + cbSunday.ClientID + "';");
            sbScript.Append("var txtEmailClientID = '" + txtEmail.ClientID + "';");
            sbScript.Append("var txtFaxAreaCodeClientID = '" + txtFaxAreaCode.ClientID + "';");
            sbScript.Append("var txtFaxNNNNClientID = '" + txtFaxNNNN.ClientID + "';");
            sbScript.Append("var txtFaxPrefixClientID = '" + txtFaxPrefix.ClientID + "';");
            sbScript.Append("var txtValidateMessageClientID = '" + txtValidateMessage.ClientID + "';");
            sbScript.Append("var hdnSaveCalledClientID = '" + hdnSaveCalled.ClientID + "';");
            sbScript.Append("var hdnProfileSavedClientID = '" + hdnProfileSaved.ClientID + "';");
            sbScript.Append("var hdnUserRoleClientID = '" + hdnUserRole.ClientID + "';");
            sbScript.Append("var txtEmailClientID = '" + txtEmail.ClientID + "';");
            sbScript.Append("var txtPasswordClientID = '" + txtPassword.ClientID + "';");
            sbScript.Append("var txtConfirmClientID = '" + txtConfirm.ClientID + "';");
            sbScript.Append("var btnConfirmClientID = '" + btnConfirm.ClientID + "';");
            sbScript.Append("var txtFirstNameClientID = '" + txtFirstName.ClientID + "';");
            sbScript.Append("var txtLoginIdClientID = '" + txtLoginID.ClientID + "';");
            sbScript.Append("var ddlRoleClientID = '" + ddlRole.ClientID + "';");

            sbScript.Append("var txtPhone1ClientID = '" + txtPrimaryPhoneAreaCode.ClientID + "';");
            sbScript.Append("var txtPhone2ClientID = '" + txtPrimaryPhonePrefix.ClientID + "';");
            sbScript.Append("var txtPhone3ClientID = '" + txtPrimaryPhoneNNNN.ClientID + "';");

            sbScript.Append("var txtFax1ClientID = '" + txtFaxAreaCode.ClientID + "';");
            sbScript.Append("var txtFax2ClientID = '" + txtFaxPrefix.ClientID + "';");
            sbScript.Append("var txtFax3ClientID = '" + txtFaxNNNN.ClientID + "';");

            sbScript.Append("var btnUpdateProfileClientID = '" + btnUpdateProfile.ClientID + "';");
            sbScript.Append("var btnGeneratePasswordClientID = '" + btnGeneratePassword.ClientID + "';");
            sbScript.Append("DisableGenerateButton(btnGeneratePasswordClientID);");
            sbScript.Append("enableSpecialistInfo('" + ddlRole.ClientID + "','" + txtAffiliation.ClientID + "','" + txtSpecialty.ClientID + "','false');");
            sbScript.Append("var textChangedClientID = '" + txtChanged.ClientID + "';");

            sbScript.Append("var cmbDevicesClientID = '" + cmbDevices.ClientID + "';");
            sbScript.Append("var txtDeviceAddressClientID = '" + txtDeviceAddress.ClientID + "';");
            sbScript.Append("var txtGatewayClientID = '" + txtGateway.ClientID + "';");
            sbScript.Append("var hidDeviceLabelClientID = '" + hidDeviceLabel.ClientID + "';");
            sbScript.Append("var hidGatewayLabelClientID = '" + hidGatewayLabel.ClientID + "';");
            sbScript.Append("var cmbCarrierClientID = '" + cmbCarrier.ClientID + "';");
            sbScript.Append("var hiddenScrollPos = '" + scrollPos.ClientID + "';");
            sbScript.Append("var hdnIsAddClickedClientID = '" + hdnIsAddClicked.ClientID + "';");

            sbScript.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());

            //Set flag on change data in profile section
            txtFirstName.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtSpecialty.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtLastName.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtAffiliation.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtNickname.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtPrimaryPhonePrefix.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtPrimaryPhoneAreaCode.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtPrimaryPhoneNNNN.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtEmail.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtFaxPrefix.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtFaxAreaCode.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtFaxNNNN.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtLoginID.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtPassword.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            btnGeneratePassword.Attributes.Add("onclick", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");

            ddlRole.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');enableSpecialistInfo('" + ddlRole.ClientID + "','" + txtAffiliation.ClientID + "','" + txtSpecialty.ClientID + "','true');");

            txtPassword.Attributes.Add("onFocus", "JavaScript:EnableGenerateButton('" + btnGeneratePassword.ClientID + "');");

            txtFirstName.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            txtSpecialty.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            txtLastName.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            txtAffiliation.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            txtNickname.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            txtPrimaryPhonePrefix.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            txtPrimaryPhoneAreaCode.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            txtPrimaryPhoneNNNN.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            txtEmail.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            txtFaxPrefix.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            txtFaxAreaCode.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            txtFaxNNNN.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            txtLoginID.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");

            cbActive.Attributes.Add("onclick", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            ddlRole.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            btnDefault.Attributes.Add("onfocus", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            btnDefault.Attributes.Add("onblur", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");
            btnDefault.Attributes.Add("onclick", "JavaScript:DisableGenerateButton('" + btnGeneratePassword.ClientID + "');");

            //txtLoginID.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStroke();");
            txtPassword.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
            txtPrimaryPhoneAreaCode.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStroke();");
            txtPrimaryPhonePrefix.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStroke();");
            txtPrimaryPhoneNNNN.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStroke();");
            txtNoofDays.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStroke();");
            txtFaxAreaCode.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStroke();");
            txtFaxPrefix.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStroke();");
            txtFaxNNNN.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStroke();");
            btnUpdateRptSettings.Attributes.Add("onClick", "JavaScript:return ValidateMRDetails();");
            btnUpdateProfile.Attributes.Add("onClick", "JavaScript:ChangeFlag();Button_SaveCalled();");
            cbEmailReports.Attributes.Add("onClick", "JavaScript:ValidateMessageReportDetails();");
            cbFaxReports.Attributes.Add("onClick", "JavaScript:ValidateMessageReportDetails();");

            txtPrimaryPhoneAreaCode.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryPhonePrefix.ClientID + "').focus()";
            txtPrimaryPhonePrefix.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryPhoneNNNN.ClientID + "').focus()";
            txtPrimaryPhoneNNNN.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + txtFaxAreaCode.ClientID + "').focus()";

            txtFaxAreaCode.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtFaxPrefix.ClientID + "').focus()";
            txtFaxPrefix.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtFaxNNNN.ClientID + "').focus()";
            txtFaxNNNN.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + txtLoginID.ClientID + "').focus()";


        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            base.OnInit(e);
        }

        #endregion

        /// <summary>
        /// This function is to Refresh the the pages load into frames pages.
        /// </summary>
        //public void QueryStringtoIframe()
        //{
        //    if (ViewState["SubscriberID"] != null)
        //    {
        //        notificationStep1.Attributes["src"] = "profile_notificationStep1.aspx?SubscriberID=" + HttpUtility.UrlEncode(ViewState["SubscriberID"].ToString());
        //        notificationStep2.Attributes["src"] = "profile_notificationStep2.aspx?SubscriberID=" + HttpUtility.UrlEncode(ViewState["SubscriberID"].ToString());
        //        notificationStep3.Attributes["src"] = "profile_notificationStep3.aspx?SubscriberID=" + HttpUtility.UrlEncode(ViewState["SubscriberID"].ToString());
        //    }
        //}

        /// <summary>
        /// This function is to fill Role list into drop down list for logged in users.
        /// This function calls stored procedure "getRoles"
        /// </summary>
        /// <param name="cnx">Connection String</param>
        private void fillRoleDDL()
        {
            Subscriber userProfile = null;
            try
            {
                userProfile = new Subscriber();
                if (strAccess == "NO")
                {
                    ddlRole.DataSource = userProfile.GetUserRoles(false);
                }
                else
                {
                    ddlRole.DataSource = userProfile.GetUserRoles(true);
                }
                ddlRole.DataBind();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("user_profile - dgAfterHoursNotifications_DeleteCommand", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                userProfile = null;
            }
        }

        /// <summary>
        /// This function fills user information for logged in user into form.
        /// This function calls stored procedure "getSubscriberInfoBySubscriberID"
        /// If any error occured while getting requried information then error handling done in catch block
        /// </summary>
        /// <param name="cnx"></param>
        private void fillUserInformation()
        {
            Subscriber userProfile = null;
            DataTable dtUserInfo = null;
            try
            {
                userProfile = new Subscriber();

                dtUserInfo = userProfile.GetUserInfo(int.Parse(ViewState["SubscriberID"].ToString()), int.Parse(ViewState["RoleID"].ToString()));

                if (dtUserInfo.Rows.Count > 0)
                {
                    if (dtUserInfo.Rows[0]["LastUpdated"] == null)
                    {
                        lblLastUpdated.Text = "";
                    }
                    else
                    {
                        if (dtUserInfo.Rows[0]["LastUpdated"].ToString().Length > 0)
                        {
                            lblLastUpdated.Text = ((DateTime)dtUserInfo.Rows[0]["LastUpdated"]).ToShortDateString();
                        }
                        else
                        {
                            lblLastUpdated.Text = "";
                        }
                    }

                    cbActive.Checked = (bool)dtUserInfo.Rows[0]["Active"];
                    lblGroupName.Text = dtUserInfo.Rows[0]["GroupName"].ToString();
                    txtFirstName.Text = dtUserInfo.Rows[0]["FirstName"].ToString();
                    txtNickname.Text = dtUserInfo.Rows[0]["Nickname"].ToString();
                    txtLastName.Text = dtUserInfo.Rows[0]["LastName"].ToString();
                    txtLoginID.Text = dtUserInfo.Rows[0]["LoginID"].ToString().Trim();
                    txtPassword.Text = dtUserInfo.Rows[0]["Password"].ToString().Trim();
                    ViewState["Password"] = dtUserInfo.Rows[0]["Password"].ToString();
                    txtEmail.Text = dtUserInfo.Rows[0]["PrimaryEmail"].ToString();
                    ViewState["PrimaryEmail"] = dtUserInfo.Rows[0]["PrimaryEmail"].ToString();
                    ViewState["GroupID"] = dtUserInfo.Rows[0]["GroupID"].ToString();

                    btnCancelProfileChanges.Attributes.Add("onclick", "JavaScript:cancelClick('group_maintenance.aspx?GroupID=" + ViewState["GroupID"].ToString() + "');");
                    btnCancelRptSettings.Attributes.Add("onclick", "JavaScript:cancelClick('group_maintenance.aspx?GroupID=" + ViewState["GroupID"].ToString() + "');");

                    string primaryPhone = Utils.flattenPhoneNumber(dtUserInfo.Rows[0]["PrimaryPhone"].ToString());
                    if (primaryPhone.Length == 10)  // only if we have a valid 10 digit phone number stored..
                    {
                        txtPrimaryPhoneAreaCode.Text = primaryPhone.Substring(0, 3);
                        txtPrimaryPhonePrefix.Text = primaryPhone.Substring(3, 3);
                        txtPrimaryPhoneNNNN.Text = primaryPhone.Substring(6, 4);
                    }

                    string fax = Utils.flattenPhoneNumber(dtUserInfo.Rows[0]["Fax"].ToString());
                    if (fax.Length == 10)  // only if we have a valid 10 digit phone number stored..
                    {
                        txtFaxAreaCode.Text = fax.Substring(0, 3);
                        txtFaxPrefix.Text = fax.Substring(3, 3);
                        txtFaxNNNN.Text = fax.Substring(6, 4);

                    }
                    ViewState["fax"] = txtFaxAreaCode.Text + txtFaxPrefix.Text + txtFaxNNNN.Text;
                    ViewState["RoleID"] = dtUserInfo.Rows[0]["RoleID"].ToString();
                    hdnUserRole.Value = ViewState["RoleID"].ToString();
                    ddlRole.Items.FindByValue(dtUserInfo.Rows[0]["RoleID"].ToString()).Selected = true;

                    if ((int.Parse(ViewState["RoleID"].ToString()) == UserRole.LabTechnician.GetHashCode()) ||
                        (int.Parse(ViewState["RoleID"].ToString()) == UserRole.Radiologists.GetHashCode()))
                    {
                        if (dtUserInfo.Rows[0]["SpecialistID"] != null && dtUserInfo.Rows[0]["SpecialistID"].ToString().Length > 0)  // only if specialist info exists...
                        {
                            ViewState["SpecialistID"] = (int)dtUserInfo.Rows[0]["SpecialistID"];
                        }
                        txtSpecialty.Text = dtUserInfo.Rows[0]["Specialty"].ToString();
                        txtAffiliation.Text = dtUserInfo.Rows[0]["Affiliation"].ToString();
                        //txtSpecialty.Enabled = true;
                        //txtAffiliation.Enabled = true;
                    }
                    else
                    {
                        //txtSpecialty.Enabled = false;
                        //txtAffiliation.Enabled = false;
                    }
                }
                else
                {
                    lblError.Text = "User Information Not Available, Please Call Nuance";
                    deactivateFields(true);
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("user_profile - dgAfterHoursNotifications_DeleteCommand", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                userProfile = null;
                dtUserInfo = null;
            }
        }

        /// <summary>
        /// This function is to fill Report Setting information for logged in users.
        /// This function calls stored procedure "getSubscriberReportSettings"
        /// </summary>
        /// <param name="cnx"></param>
        private void fillSubscriberReportSettings()
        {
            Subscriber userProfile = null;
            DataTable dtReportSettings = null;
            try
            {
                userProfile = new Subscriber();

                dtReportSettings = userProfile.GetReportSettings(int.Parse(ViewState["SubscriberID"].ToString()));

                if (dtReportSettings.Rows.Count > 0)
                {
                    int reportOnDays = (int)dtReportSettings.Rows[0]["ReportOnDays"];
                    cbMonday.Checked = (reportOnDays & ReportDay.Monday.GetHashCode()) == ReportDay.Monday.GetHashCode();
                    cbTuesday.Checked = (reportOnDays & ReportDay.Tuesday.GetHashCode()) == ReportDay.Tuesday.GetHashCode();
                    cbWednesday.Checked = (reportOnDays & ReportDay.Wednesday.GetHashCode()) == ReportDay.Wednesday.GetHashCode();
                    cbThursday.Checked = (reportOnDays & ReportDay.Thursday.GetHashCode()) == ReportDay.Thursday.GetHashCode();
                    cbFriday.Checked = (reportOnDays & ReportDay.Friday.GetHashCode()) == ReportDay.Friday.GetHashCode();
                    cbSaturday.Checked = (reportOnDays & ReportDay.Saturday.GetHashCode()) == ReportDay.Saturday.GetHashCode();
                    cbSunday.Checked = (reportOnDays & ReportDay.Sunday.GetHashCode()) == ReportDay.Sunday.GetHashCode();

                    int reportAtHour = (int)dtReportSettings.Rows[0]["ReportAtHour"];
                    ddlReportTime.SelectedIndex = ddlReportTime.Items.IndexOf(ddlReportTime.Items.FindByValue(reportAtHour.ToString()));
                    cbEmailReports.Checked = (bool)dtReportSettings.Rows[0]["ReportViaEmail"];
                    cbFaxReports.Checked = (bool)dtReportSettings.Rows[0]["ReportViaFax"];
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("user_profile - fillSubscriberReportSettings", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                dtReportSettings = null;
                userProfile = null;
            }
        }

        /// <summary>
        /// This Method contains the no of control which need to be disabled on a particular situation
        /// </summary>
        /// <param name="allFields"></param>
        private void deactivateFields(bool allFields)
        {
            if (allFields)
            {
                txtEmail.Enabled = false;
                txtFirstName.Enabled = false;
                txtLastName.Enabled = false;
                txtLoginID.Enabled = false;
                ddlRole.Enabled = false;
                txtPassword.Enabled = false;
                txtNickname.Enabled = false;
            }
            txtSpecialty.Text = "";
            //txtSpecialty.Enabled = false;
            txtSpecialty.BackColor = System.Drawing.Color.Gray;
            txtAffiliation.Text = "";
            //txtAffiliation.Enabled = false;
            txtAffiliation.BackColor = System.Drawing.Color.Gray;
        }

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <returns></returns>
        private bool ConfirmUpdateRecord()
        {
            int roleID = int.Parse(ddlRole.Items[ddlRole.SelectedIndex].Value);
            string strPassword = "";
            Subscriber subscriber = null;
            Subscriber userProfile = null;

            try
            {

                subscriber = new Subscriber();
                SubscriberInformation subscriberInfo = subscriber.GetSubscriberInformation(txtLoginID.Text.Trim(), txtPassword.Text.Trim());

                userProfile = new Subscriber();
                strPassword = txtPassword.Text;

                //Update user profile
                int returnVal = userProfile.UpdateUserInfo(int.Parse(ViewState["SubscriberID"].ToString()),
                                            roleID, cbActive.Checked, txtFirstName.Text, txtNickname.Text,
                                            txtLastName.Text, txtLoginID.Text.Trim(), strPassword.Trim(), txtEmail.Text,
                                            txtPrimaryPhoneAreaCode.Text + txtPrimaryPhonePrefix.Text + txtPrimaryPhoneNNNN.Text,
                                            txtFaxAreaCode.Text + txtFaxPrefix.Text + txtFaxNNNN.Text, "0", "0");

                if (returnVal <= 0)
                {
                    credentialStatus = returnVal;
                    return false;
                }

                //if (roleID == Constants.ROLE_SPECIALIST || roleID == Constants.ROLE_LAB_TECHNICIAN)
                if (roleID == UserRole.Radiologists.GetHashCode() || roleID == UserRole.LabTechnician.GetHashCode())
                {
                    userProfile = new Subscriber();
                    userProfile.InsertSpecialistInfo(int.Parse(ViewState["SubscriberID"].ToString()), txtSpecialty.Text, txtAffiliation.Text);

                }
                if ((Convert.ToInt32(ViewState["RoleID"].ToString()) != UserRole.GroupAdmin.GetHashCode() && roleID == UserRole.GroupAdmin.GetHashCode()) ||
                    (Convert.ToInt32(ViewState["RoleID"].ToString()) != UserRole.LabGroupAdmin.GetHashCode() && roleID == UserRole.LabGroupAdmin.GetHashCode()))
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "Warning", "ConfirmOKCancel();", true);
                    /*Remove Information profile, group maintenance*/
                    if (txtConfirm.Value == "1")
                    {
                        Device device = new Device();
                        device.DeleteDevices(int.Parse(ViewState["SubscriberID"].ToString()));

                        divNotification.Visible = false;
                        legendNotifation.InnerText = "Configuration Settings";
                        txtConfirm.Value = "0";
                    }
                }
                ViewState["RoleID"] = roleID;
                hdnUserRole.Value = roleID.ToString();

                if (string.Compare(ViewState["RoleID"].ToString(), Request.QueryString["RoleID"]) != 0)
                {
                    //?SubscriberID=4259&RoleID=8
                    string newUid = this.UniqueID.Replace(":", "_");
                    string acScript = "window.location.href='user_profile.aspx?SubscriberID=" + ViewState["SubscriberID"].ToString() + "&RoleID=" + roleID + "';";
                    ScriptManager.RegisterClientScriptBlock(upnlGroupAffiliation, upnlGroupAffiliation.GetType(), newUid, acScript, true);
                }


                fillUserInformation();
                txtChanged.Value = "false";
                hdnProfileSaved.Value = "true";
                isProfileSaved = true;
                return true;
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("user_profile - fillSubscriberReportSettings", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                ScriptManager.RegisterClientScriptBlock(upnlGroupAffiliation, upnlGroupAffiliation.GetType(), "Warning6", "<script type=\"text/javascript\">alert('Error While Updating Profile.');</script>", false);
                hdnProfileSaved.Value = "false";
                return false;
            }
            finally
            {
                subscriber = null;
                userProfile = null;
            }
        }

        /// <summary>
        /// This function is to load Cell phone carriers list into data table
        /// This function calls stored procedure "getCellPhoneCarriers"
        /// </summary>
        private void getCellPhoneCarriers()
        {
            Device device = null;
            try
            {
                device = new Device();
                DataTable dtCarrier = device.GetCellPhoneCarriers();
                ViewState["CellPhoneCarriers"] = dtCarrier;
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("user_profile - fillSubscriberReportSettings", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                device = null;
            }
        }

        /// <summary>
        /// This function is to load Pager Carriers list into data table
        /// This function calls stored procedure "getPagerCarriers"
        /// </summary>
        private void getPagerCarriers()
        {
            Device device = null;
            try
            {
                device = new Device();
                DataTable dtPager = device.GetPagerCarriers();
                ViewState["PagerCarriers"] = dtPager;
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("user_profile - getPagerCarriers", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                device = null;
            }
        }

        /// <summary>
        /// This function is to fill user configuration information into logged in user profile.
        /// This function calls stored procedure "VOC_VLR_getUserConfigurationDataForSubscriber"
        /// </summary>
        /// <param name="cnx">Connection String</param>
        private void fillUserConfigurationInformation()
        {
            lblmessage.Text = "";
            Subscriber userProfile = null;
            DataTable dtUserConfigInfo = null;
            try
            {
                userProfile = new Subscriber();
                dtUserConfigInfo = userProfile.GetUserConfigurationInfo(int.Parse(ViewState["SubscriberID"].ToString()));

                if (dtUserConfigInfo.Rows.Count > 0)
                {
                    txtNoofDays.Text = dtUserConfigInfo.Rows[0]["numberOfDays"].ToString();
                    btnaddUserConfig.Text = "Update";
                }
                else
                {
                    int numOfDays = 5; //Default
                    userProfile.InsertUserConfigurationInfo(int.Parse(ViewState["SubscriberID"].ToString()), numOfDays);
                    txtNoofDays.Text = "5";
                    btnaddUserConfig.Text = "Update";
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("user_profile - fillUserConfigurationInformation", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                userProfile = null;
                dtUserConfigInfo = null;
            }
        }

        /// <summary>
        /// Resets all control to their default values.
        /// </summary>
        private void resetControls()
        {
            if (cmbDevices.Items.Count > 0)
                cmbDevices.SelectedValue = "-1";

            txtDeviceAddress.Text = string.Empty;
            txtDeviceAddress.Visible = false;

            cmbCarrier.Visible = false;
            cmbCarrier.SelectedValue = "-1";

            txtGateway.Text = string.Empty;
            txtGateway.Visible = false;

            cmbEvent.Visible = false;
            cmbEvent.SelectedValue = "-1";

            cmbFinding.Visible = false;
            cmbFinding.SelectedValue = "-1";

            btnAddDevice.Visible = false;
            btnShowHideDetails.Visible = false;
        }

        /// <summary>
        /// To validate if directly doPostback for Save is called from javascript e.g in case of
        /// Onunload event, if user didn't save the changes.
        /// </summary>
        /// <returns></returns>
        private string validateBeforeSave()
        {
            StringBuilder message = new StringBuilder();
            bool validated = true;
            if (txtFirstName.Text.Trim().Length == 0)
            {
                message.Append("You Must Enter A First Name");
                validated = false;
            }
            if (txtLastName.Text.Trim().Length == 0)
            {
                if (message.Length != 0)
                {
                    message.Append("#");
                }
                message.Append("You Must Enter A Last Name");
                validated = false;
            }

            //Validation for Phone
            if (txtPrimaryPhonePrefix.Text.Trim().Length != 3 && txtPrimaryPhonePrefix.Text.Trim().Length != 0)
            {
                if (message.Length != 0)
                {
                    message.Append("#");
                }
                message.Append("Please enter valid Phone prefix");
                validated = false;
            }
            if (txtPrimaryPhoneAreaCode.Text.Trim().Length != 3 && txtPrimaryPhoneAreaCode.Text.Trim().Length != 0)
            {
                if (message.Length != 0)
                {
                    message.Append("#");
                }
                message.Append("Please enter valid Phone Area Code");
                validated = false;
            }
            if (txtPrimaryPhoneNNNN.Text.Trim().Length != 4 && txtPrimaryPhoneNNNN.Text.Trim().Length != 0)
            {
                if (message.Length != 0)
                {
                    message.Append("#");
                }
                message.Append("Please enter valid Phone Extension");
                validated = false;
            }

            //Validation for Fax.
            if (txtFaxPrefix.Text.Trim().Length != 3 && txtFaxPrefix.Text.Trim().Length != 0)
            {
                if (message.Length != 0)
                {
                    message.Append("#");
                }
                message.Append("Please enter valid Fax prefix");
                validated = false;
            }
            if (txtFaxAreaCode.Text.Trim().Length != 3 && txtFaxAreaCode.Text.Trim().Length != 0)
            {
                if (message.Length != 0)
                {
                    message.Append("#");
                }
                message.Append("Please enter valid Fax Area Code");
                validated = false;
            }
            if (txtFaxNNNN.Text.Trim().Length != 4 && txtFaxAreaCode.Text.Trim().Length != 0)
            {
                if (message.Length != 0)
                {
                    message.Append("#");
                }
                message.Append("Please enter valid Fax Extension");
                validated = false;
            }
            if (txtEmail.Text.Trim().Length != 0)
            {
                if (!(txtEmail.Text.Contains("@") && txtEmail.Text.Contains(".")))
                {
                    if (message.Length != 0)
                    {
                        message.Append("#");
                    }
                    message.Append("Email format incorrect");
                    validated = false;
                }
            }
            return message.ToString();
        }

        /// <summary>
        /// This method will alert the error passed as parameter to the user.
        /// </summary>
        /// <param name="message"></param>
        private void alertErrorToUser(string message)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanelMessageList, UpdatePanelMessageList.GetType(), "Validationalert", "<script type=\"text/javascript\">showMessage('" + message + "');</script>", false);
        }

        private void setUserGroup()
        {
            if (ViewState["RoleID"] != null)
            {
                int roleID = int.Parse(ViewState["RoleID"].ToString());
                if (roleID == UserRole.LabTechnician.GetHashCode() || roleID == UserRole.LabGroupAdmin.GetHashCode())
                    strAccess = "YES";
                else
                    strAccess = "NO";
            }
        }

        #region Step1_1
        /// <summary>
        /// This function is to generate GateWay Address for Cell Phone carriers & Pager Carriers
        /// </summary>
        private void generateGatewayAddress()
        {
            DataTable dtCarriers = null;
            DataRow[] drCarriers = null;
            IEnumerator enmCarrier;
            try
            {
                int deviceID = int.Parse(cmbDevices.SelectedItem.Value);

                switch (deviceID)
                {
                    case 2: // Cell Phones
                        txtGateway.Text = txtDeviceAddress.Text.Trim() + "@";
                        dtCarriers = (DataTable)ViewState["CellPhoneCarriers"];
                        drCarriers = dtCarriers.Select("CarrierID='" + cmbCarrier.SelectedItem.Value + "'"); // should have 1
                        enmCarrier = drCarriers.GetEnumerator();
                        if (enmCarrier.MoveNext())
                        {
                            DataRow drCarrier = (DataRow)enmCarrier.Current;
                            txtGateway.Text += drCarrier["CarrierEmail"];
                        }
                        break;
                    case 3: // Pagers
                        txtGateway.Text = txtDeviceAddress.Text.Trim() + "@";
                        dtCarriers = (DataTable)ViewState["PagerCarriers"];
                        drCarriers = dtCarriers.Select("CarrierID='" + cmbCarrier.SelectedItem.Value + "'"); // should have 1
                        enmCarrier = drCarriers.GetEnumerator();
                        if (enmCarrier.MoveNext())
                        {
                            DataRow drCarrier = (DataRow)enmCarrier.Current;
                            txtGateway.Text = txtDeviceAddress.Text.Trim() + "@" + drCarrier["CarrierEmail"].ToString();
                        }
                        break;
                    case 15: // Pager Tap
                        break;
                    default:
                        txtGateway.Text = "";
                        break;
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - generateGatewayAddress", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                dtCarriers = null;
                drCarriers = null;
            }
        }

        /// <summary>
        /// This function sets Label and Input boxes for selected Device for logged in user
        /// </summary>
        /// <param name="deviceID">Integer Type</param>
        private void setLabelsAndInputBoxes(int deviceID)
        {
            ListItem itemCarrier = null;
            try
            {
                itemCarrier = new ListItem("-- Select Carrier", "-1");
                cmbEvent.Visible = false;
                cmbFinding.Visible = false;
                btnShowHideDetails.Text = SHOWDETAILS_BUTTONNAME;
                btnShowHideDetails.Visible = true;

                switch (deviceID)
                {
                    case (int)NotificationDevice.SelectAll:
                        resetControls();
                        break;

                    case (int)NotificationDevice.EMail:  // Email
                        txtDeviceAddress.Text = "Enter Email Address";
                        txtDeviceAddress.Visible = true;
                        txtDeviceAddress.Width = Unit.Pixel(250);
                        txtDeviceAddress.AutoPostBack = false;

                        cmbCarrier.Visible = false;
                        txtGateway.Visible = false;
                        txtGateway.Text = "";
                        btnAddDevice.Visible = true;
                        break;
                    case (int)NotificationDevice.SMS:  // SMS/Cell
                        txtDeviceAddress.Text = "Enter Cell # (numbers only)";
                        txtDeviceAddress.Visible = true;
                        txtDeviceAddress.Width = Unit.Pixel(175);
                        cmbCarrier.Visible = true;
                        txtGateway.Visible = true;
                        btnAddDevice.Visible = true;
                        cmbCarrier.DataSource = (DataTable)ViewState["CellPhoneCarriers"];
                        cmbCarrier.DataBind();

                        cmbCarrier.Items.Add(itemCarrier);
                        cmbCarrier.Items.FindByValue("-1").Selected = true;
                        break;
                    case (int)NotificationDevice.PagerAlpha:  // Pager - Alpha
                        txtDeviceAddress.Text = "Enter Pager # (numbers only)";
                        txtDeviceAddress.Visible = true;
                        txtDeviceAddress.Width = Unit.Pixel(175);
                        cmbCarrier.Visible = true;
                        txtGateway.Visible = true;
                        btnAddDevice.Visible = true;
                        cmbCarrier.DataSource = (DataTable)ViewState["PagerCarriers"];
                        cmbCarrier.DataBind();

                        cmbCarrier.Items.Add(itemCarrier);
                        cmbCarrier.Items.FindByValue("-1").Selected = true;
                        break;
                    case (int)NotificationDevice.Fax:  // Fax
                        txtDeviceAddress.Text = "Enter Fax # (numbers only)";
                        txtDeviceAddress.Visible = true;
                        txtDeviceAddress.Width = Unit.Pixel(175);
                        txtDeviceAddress.AutoPostBack = false;
                        cmbCarrier.Visible = false;
                        txtGateway.Visible = false;
                        btnAddDevice.Visible = true;
                        txtGateway.Text = "";
                        break;
                    case (int)NotificationDevice.PagerNumRegular:
                    case (int)NotificationDevice.PagerNumSkyTel:
                    case (int)NotificationDevice.PagerNumUSA:
                    case (int)NotificationDevice.pagerPartner:
                        txtDeviceAddress.Text = "Enter Pager # + PIN (numbers only)";
                        txtDeviceAddress.Width = Unit.Pixel(250);
                        txtDeviceAddress.Visible = true;
                        txtDeviceAddress.AutoPostBack = false;
                        cmbCarrier.Visible = false;
                        txtGateway.Visible = false;
                        btnAddDevice.Visible = true;
                        txtGateway.Text = "";
                        break;
                    case (int)NotificationDevice.PagerTAP:
                    case (int)NotificationDevice.PagerTAPA:
                        txtDeviceAddress.Text = "Enter PIN number (numbers only)";
                        txtDeviceAddress.Width = Unit.Pixel(255);
                        txtDeviceAddress.Visible = true;
                        txtDeviceAddress.AutoPostBack = false;
                        txtGateway.Visible = true;
                        cmbCarrier.Visible = false;
                        btnAddDevice.Visible = true;
                        if (ViewState[TAP_800_NUM].ToString().Length == 0)
                            txtGateway.Text = "Enter TAP 800 number (numbers only)";
                        else
                            txtGateway.Text = ViewState[TAP_800_NUM].ToString();
                        txtGateway.Width = Unit.Pixel(255);
                        txtDeviceAddress.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
                        txtGateway.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
                        txtGateway.Attributes.Add("onclick", "RemoveGatewayLabel();");
                        break;
                    default:
                        txtDeviceAddress.Visible = false;
                        cmbCarrier.Visible = false;
                        txtGateway.Visible = false;
                        btnAddDevice.Visible = false;
                        cmbFinding.Visible = false;
                        cmbEvent.Visible = false;
                        txtGateway.Text = "";
                        txtDeviceAddress.Text = "";
                        break;

                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - setLabelsAndInputBoxes", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                itemCarrier = null;
            }
        }
        /// <summary>
        /// This function is to calculate dynamic height for grdDevices datagrid
        /// </summary>
        private int getDataGridHeight()
        {
            try
            {
                int devicesGridHeight = 20;
                int rowHeight = 25;
                int headerHeight = 23;
                int maxRows = 4;

                if (grdDevices.Items.Count <= 4)
                {

                    if (grdDevices.Items.Count == 0)
                        devicesGridHeight = headerHeight;
                    else
                        devicesGridHeight = (grdDevices.Items.Count * rowHeight) + headerHeight;
                }
                else
                {
                    devicesGridHeight = (maxRows * rowHeight) + headerHeight;
                }

                return devicesGridHeight + 10;
            }
            catch (Exception ex)
            {
                if (ViewState["SubscriberID"] != null)
                    Tracer.GetLogger().LogInfoEvent("profile_notificationStep1.generateDataGridHeight:: Exception occured for User ID - " + ViewState["SubscriberID"].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState["SubscriberID"]));
                throw ex;
            }
        }

        /// <summary>
        /// This function is to generate dynamic height for grdDevices datagrid
        /// </summary>
        private void generateDataGridHeight()
        {
            try
            {
                int devicesGridHeight = 20;
                int rowHeight = 25;
                int headerHeight = 23;
                int maxRows = 4;

                if (grdDevices.Items.Count <= 4)
                {

                    if (grdDevices.Items.Count == 0)
                        devicesGridHeight = headerHeight;
                    else
                        devicesGridHeight = (grdDevices.Items.Count * rowHeight) + headerHeight;
                }
                else
                {
                    devicesGridHeight = (maxRows * rowHeight) + headerHeight;
                }

                string scriptBlock = "<script type=\"text/javascript\">";
                scriptBlock += "document.getElementById(" + '"' + ProfileDevicesDiv.ClientID + '"' + ").style.height='" + (devicesGridHeight + 10) + "';";
                scriptBlock += "document.getElementById(" + '"' + ProfileDevicesDiv.ClientID + '"' + ").scrollTop=document.getElementById('" + scrollPos.ClientID + "').value;</script>";
                ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "SetHeight", scriptBlock, false);
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - generateDataGridHeight", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// This function is to cancel Edit in grdDevices datagrid
        /// </summary>
        private void cancelDeviceEdit()
        {
            grdDevices.EditItemIndex = -1;
            fillDevices();
        }

        /// <summary>
        /// This function is to fill record for grdDevices datagrid for Logged in user
        /// This function calls stored procedure "getSubscriberDevices"
        /// </summary>
        /// <param name="cnx">Connection String</param>
        private void fillDevices()
        {
            Device device = null;
            DataTable dtDevices = null;
            try
            {
                //TODO:int.Parse(ViewState["SubscriberID"].ToString()

                device = new Device();
                dtDevices = device.GetSubscriberDevices(int.Parse(ViewState["SubscriberID"].ToString()));
                grdDevices.DataSource = dtDevices.DefaultView;
                grdDevices.DataBind();
                if (grdDevices.Items.Count < 1)
                {
                    lblNoRecordsStep1.Visible = true;
                }
                else
                {
                    lblNoRecordsStep1.Visible = false;
                }

            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - fillDevices", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                device = null;
                dtDevices = null;
                generateDataGridHeight();
            }
        }

        /// <summary>
        /// This function is to fill  available Device list into drop down list 
        /// </summary>
        /// <param name="cnx"></param>
        private void fillDeviceDDL()
        {
            Device device = null;
            DataTable dtDevices = null;
            try
            {
                device = new Device();
                dtDevices = device.GetAllDevices();

                foreach (DataRow dr in dtDevices.Rows)
                {
                    if (Convert.ToInt32(dr["DeviceID"]) == NotificationDevice.OutboundCallCB.GetHashCode() || Convert.ToInt32(dr["DeviceID"]) == NotificationDevice.OutboundCallRS.GetHashCode() || Convert.ToInt32(dr["DeviceID"]) == NotificationDevice.OutboundCallCI.GetHashCode() || Convert.ToInt32(dr["DeviceID"]) == NotificationDevice.OutboundCallAS.GetHashCode())
                    {
                        dr.Delete();
                    }
                }
                dtDevices.AcceptChanges();

                cmbDevices.DataSource = dtDevices.DefaultView;
                cmbDevices.DataBind();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - fillDeviceDDL", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                ListItem itemDevice = new ListItem("-- Select Device To Add", "-1");
                cmbDevices.Items.Add(itemDevice);
                cmbDevices.Items.FindByValue("-1").Selected = true;
                itemDevice = null;
            }
        }

        /// <summary>
        /// Delete Device notifications
        /// </summary>
        /// <param name="subscriberNotificationID"></param>
        private void deleteDeviceNotifications(int subscriberNotificationID)
        {
            Device device = null;
            try
            {
                device = new Device();
                device.DeleteDeviceNotification(subscriberNotificationID);
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - deleteDeviceNotifications", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                device = null;
            }
        }

        /// <summary>
        /// Delete Device assigned to subscriber
        /// </summary>
        /// <param name="subscriberDeviceID"></param>
        private int deleteSubscriberDevice(int subscriberDeviceID, int subscriberNotificationID)
        {
            Device device = null;
            int rowsAffected = 0;
            try
            {
                device = new Device();
                rowsAffected = device.DeleteSubscriberDevice(subscriberDeviceID, subscriberNotificationID);
                return rowsAffected;
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("profile_notificationStep1 - deleteSubscriberDevice", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                device = null;
            }
        }

        /// <summary>
        /// Check E-Mail format
        /// </summary>
        /// <param name="strItem"></param>
        /// <returns></returns>
        private bool RegExMatch(string strItem)
        {
            //string strPattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|" +
            //  @"0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" +
            //  @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";;
            string strPattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

            System.Text.RegularExpressions.Match objMatch =
                  System.Text.RegularExpressions.Regex.Match(strItem, strPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return objMatch.Success;
        }

        /// <summary>
        /// Check given value is numeric one.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private bool isNumericValue(string val)
        {
            try
            {
                long returnVal = long.Parse(val);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        /// <summary>
        /// Validate record before update
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool validateRecord(DataGridCommandEventArgs e)
        {
            bool returnVal = true;
            string errorMessage = "";

            int deviceType = int.Parse(grdDevices.Items[e.Item.ItemIndex].Cells[11].Text);
            TextBox gridDeviceTypeTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[2].FindControl("txtDeviceName")));
            TextBox gridDeviceNumberTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[3].FindControl("txtDeviceAddress")));
            TextBox gridEmailGatewayTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[5].FindControl("txtGateway")));

            DropDownList dlEvent = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[6].FindControl("dlistGridEvents")));
            DropDownList dlFinding = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[7].FindControl("dlistGridFindings")));
            int selectedEventID = Convert.ToInt32(dlEvent.SelectedValue);
            int selectedFindingID = Convert.ToInt32(dlFinding.SelectedValue);

            string isAddClicked = hdnIsAddClicked.Value;

            //1	Email
            //2	SMS (Cell)
            //3	Pager - Alpha
            //4	Fax
            //5	Pager - Numeric - Regular
            //6	Pager - Numeric - SkyTel Type
            //7	Pager - Numeric - PageUSA Type
            //8	PCMonitor
            //9	PCMonitor
            //10	Pager - Partners Paging

            if (gridDeviceTypeTxt.Text.Trim().Length == 0 && isAddClicked == "false")
            {
                errorMessage = "Please enter Device Type.\\n";

                gridDeviceTypeTxt.Focus();
            }

            //Validation for all Notification details or not
            if (!(selectedEventID == 0 && selectedFindingID == 0))
            {
                if (!((selectedEventID == -1 || selectedEventID > 0) && (selectedFindingID == -1 || selectedFindingID > 0)))
                {
                    errorMessage += "Either select all notification details (Event and Finding) or none.\\n";
                }
            }

            switch (deviceType)
            {
                case (int)NotificationDevice.EMail:
                    if (!RegExMatch(gridDeviceNumberTxt.Text.Trim()))
                    {
                        if (errorMessage.Length == 0)
                            gridDeviceNumberTxt.Focus();
                        errorMessage += "Please enter valid E-mail ID.\\n";
                    }
                    break;

                case (int)NotificationDevice.PagerNumRegular:
                case (int)NotificationDevice.PagerNumSkyTel:
                case (int)NotificationDevice.PagerNumUSA:
                case (int)NotificationDevice.pagerPartner:
                    if ((int)gridDeviceNumberTxt.Text.Trim().Length == 0)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter Phone Number\\n";
                    }
                    if ((Utils.RegExNumericMatch(gridDeviceNumberTxt.Text.Trim())) == false)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter valid Phone Number\\n";
                    }
                    break;
                case 8:
                case 9:
                case (int)NotificationDevice.Fax:
                case (int)NotificationDevice.OutboundCallRS:
                case (int)NotificationDevice.OutboundCallCI:

                    if ((int)gridDeviceNumberTxt.Text.Trim().Length == 0)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter Phone Number\\n";
                    }
                    else if (!isNumericValue(gridDeviceNumberTxt.Text.Trim()))
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter valid Phone Number\\n";
                    }
                    else
                    {
                        if (gridDeviceNumberTxt.Text.Trim().Length != 10)
                        {
                            if (errorMessage.Length == 0)
                            {
                                gridDeviceNumberTxt.Focus();
                            }
                            errorMessage += "Please enter valid Phone Number\\n";
                        }
                    }
                    break;
                case (int)NotificationDevice.SMS:
                    if ((int)gridDeviceNumberTxt.Text.Trim().Length == 0)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter Phone Number\\n";
                    }
                    else if (!isNumericValue(gridDeviceNumberTxt.Text.Trim()) || (int)gridDeviceNumberTxt.Text.Trim().Length != 10)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter valid Phone Number\\n";
                    }

                    if (!RegExMatch(gridEmailGatewayTxt.Text.Trim()))
                    {
                        if (errorMessage.Length == 0)
                            gridEmailGatewayTxt.Focus();
                        errorMessage += "Please enter valid E-mail ID.\\n";
                    }
                    break;

                case (int)NotificationDevice.PagerAlpha:
                    if ((int)gridDeviceNumberTxt.Text.Trim().Length == 0)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter Phone Number\\n";
                    }
                    //else if (!isNumericValue(gridDeviceNumberTxt.Text.Trim()))
                    //{
                    //    if (errorMessage.Length == 0)
                    //    {
                    //        gridDeviceNumberTxt.Focus();
                    //    }
                    //    errorMessage += "Please enter valid Phone Number\\n";
                    //}
                    if ((Utils.RegExNumericMatch(gridDeviceNumberTxt.Text.Trim())) == false)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter valid Pager Number\\n";
                    }

                    if (!RegExMatch(gridEmailGatewayTxt.Text.Trim()))
                    {
                        if (errorMessage.Length == 0)
                            gridEmailGatewayTxt.Focus();
                        errorMessage += "Please enter valid E-mail ID.\\n";
                    }
                    break;
                case (int)NotificationDevice.PagerTAP:
                case (int)NotificationDevice.PagerTAPA:
                    if (gridDeviceNumberTxt.Text.Trim().Length == 0)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter PIN Number.\\n";
                    }

                    if ((Utils.RegExNumericMatch(gridDeviceNumberTxt.Text.Trim())) == false)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter valid PIN Number\\n";
                    }

                    //else if (!Utils.isNumericValue(gridDeviceNumberTxt.Text.Trim()) || ((int)gridDeviceNumberTxt.Text.Length < 4))
                    //{
                    //    if (errorMessage.Length == 0)
                    //    {
                    //        gridDeviceNumberTxt.Focus();
                    //    }
                    //    errorMessage += "Pin must be 4 - 6 digits.\\n";
                    //}
                    if (gridEmailGatewayTxt.Text.Trim().Length == 0)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridEmailGatewayTxt.Focus();
                        }
                        errorMessage += "Please enter TAP 800 Number.\\n";
                    }

                    if ((Utils.RegExNumericMatch(gridEmailGatewayTxt.Text.Trim())) == false)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridEmailGatewayTxt.Focus();
                        }
                        errorMessage += "Please enter valid TAP 800 Number\\n";
                    }

                    //else if (!Utils.isNumericValue(gridEmailGatewayTxt.Text.Trim()) || ((int)gridEmailGatewayTxt.Text.Length < 10))
                    //{
                    //    if (errorMessage.Length == 0)
                    //    {
                    //        gridEmailGatewayTxt.Focus();
                    //    }
                    //    errorMessage += "TAP 800 Number must be 10 digits.\\n";
                    //}
                    break;
            }
            if (errorMessage.Length > 0)
            {
                ScriptManager.RegisterClientScriptBlock(upnlStep1, upnlStep1.GetType(), "Grid_Alert", "<script type=\'text/javascript\'>" + "document.getElementById(" + '"' + ProfileDevicesDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';" + "alert('" + errorMessage + "');</script>", false);
                returnVal = false;
            }
            return returnVal;
        }
        /// <summary>
        /// Set Pager TAP Number in viewstate TAP_800_NUM
        /// </summary>
        private void setTAP800Number()
        {
            Group group = null;
            DataTable dtGroupInfo = null;
            try
            {
                ViewState[TAP_800_NUM] = "";
                group = new Group();

                dtGroupInfo = group.GetGroupInformation(int.Parse(Request["SubscriberID"]));

                if (dtGroupInfo.Rows.Count > 0)
                {
                    string tap800No = Utils.flattenPhoneNumber(dtGroupInfo.Rows[0]["TAP800Number"].ToString().Trim());
                    ViewState[TAP_800_NUM] = tap800No;
                }
            }
            catch (Exception ex)
            {
                if (ViewState["SubscriberID"] != null)
                    Tracer.GetLogger().LogInfoEvent("profile_notificationStep1.deleteSubscriberDevice:: Exception occured for User ID - " + ViewState["SubscriberID"].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState["SubscriberID"]));
                throw ex;
            }
            finally
            {
                group = null;
                dtGroupInfo = null;
            }
        }

        #endregion Step1_1

        #region Step2_2

        /// <summary>
        /// This function fills Notification Events into drop down list.
        /// </summary>
        private void fillEvents(DropDownList objDropdown)
        {
            Device device = null;
            DataTable dtEvents = null;
            try
            {
                device = new Device();
                dtEvents = device.GetAllNotificationTypes();
                objDropdown.DataSource = dtEvents;
                objDropdown.DataBind();
            }
            catch (Exception ex)
            {
                if (ViewState["SubscriberID"] != null)
                    Tracer.GetLogger().LogInfoEvent("profile_notificationStep2.fillEvents:: Exception occured for User ID - " + ViewState["SubscriberID"].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState["SubscriberID"]));
                throw ex;
            }
            finally
            {
                device = null;
                dtEvents = null;
                ListItem listItem = new ListItem("All Events", "-1");
                objDropdown.Items.Add(listItem);
                objDropdown.Items.FindByValue("-1").Selected = true;
                if (!objDropdown.ID.ToUpper().Equals("CMBEVENT"))
                {
                    ListItem objLi1 = new ListItem();
                    objLi1.Text = "-- None --";
                    objLi1.Value = "0";
                    objDropdown.Items.Insert(objDropdown.Items.Count, objLi1);
                }
            }
        }

        /// <summary>
        /// This function is to fill Findings into drop down list.
        /// </summary>
        private void fillFindings(DropDownList objFiding)
        {
            Device device = null;
            DataTable dtEvents = null;
            try
            {
                device = new Device();
                dtEvents = device.GetFindingsForSubscriber(Convert.ToInt32(ViewState["SubscriberID"].ToString()));
                objFiding.DataSource = dtEvents;
                objFiding.DataBind();
            }
            catch (Exception ex)
            {
                if (ViewState["SubscriberID"] != null)
                    Tracer.GetLogger().LogInfoEvent("profile_notificationStep2.fillFindings:: Exception occured for User ID - " + ViewState["SubscriberID"].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState["SubscriberID"]));
                throw ex;
            }
            finally
            {
                device = null;
                dtEvents = null;

                ListItem listItem = new ListItem("All Findings", "-1");
                objFiding.Items.Add(listItem);
                objFiding.Items.FindByValue("-1").Selected = true;
                if (!objFiding.ID.ToUpper().Equals("CMBFINDING"))
                {
                    ListItem objLi1 = new ListItem();
                    objLi1.Text = "-- None --";
                    objLi1.Value = "0";
                    objFiding.Items.Insert(objFiding.Items.Count, objLi1);
                }
            }
        }

        #endregion Step2_2

        #region Step3_3
        /// <summary>
        /// This function is to generate dynamic height of datagrid grdAfterHoursNotifications
        /// </summary>
        private void generateStep3DataGridHeight()
        {
            try
            {
                int afterHrsGridHeight = 28;
                int headerHeight = 25;
                int rowHeight = 25;
                int maxRows = 4;
                if (grdAfterHoursNotifications.Items.Count <= maxRows)
                {
                    if (grdAfterHoursNotifications.Items.Count != 0)
                        afterHrsGridHeight = (grdAfterHoursNotifications.Items.Count * rowHeight) + headerHeight + 4;
                }
                else
                {
                    afterHrsGridHeight = (maxRows * rowHeight) + headerHeight; ;
                }
                string scriptBlock = "<script type=\"text/javascript\">";
                scriptBlock += "document.getElementById(" + '"' + ProfileAfterHrsDiv.ClientID + '"' + ").style.height='" + (afterHrsGridHeight + 1) + "';</script>";
                ScriptManager.RegisterStartupScript(upnlStep3, upnlStep3.GetType(), "SetHeight3", scriptBlock, false);
            }
            catch (Exception ex)
            {
                if (ViewState[SUBSCRIBER_ID] != null)
                    Tracer.GetLogger().LogInfoEvent("profile_notificationStep3.generateStep3DataGridHeight:: Exception occured for User ID - " + ViewState[SUBSCRIBER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState[SUBSCRIBER_ID]));
                throw ex;
            }
        }

        /// <summary>
        /// Add 'Add' in grid for edit mode
        /// </summary>
        /// <param name="item"></param>
        private void addLinkToGridInEditMode(DataGridItem item)
        {

            LinkButton lbUpdate = (item.Cells[8].Controls[0]) as LinkButton;
            string lnkButID = lbUpdate.ClientID.Replace("_", "$");

            HtmlAnchor objAddLink = new HtmlAnchor();
            objAddLink.InnerHtml = "Add&nbsp;New";
            objAddLink.ID = "lnkAdd";
            objAddLink.Name = "lnkAdd";
            objAddLink.CausesValidation = false;
            string script = "javascript:__doPostBack('" + lnkButID + "','');";
            objAddLink.HRef = script;
            objAddLink.Attributes.Add("onclick", "javascript:return AddRecordFromGrid();");

            item.Cells[8].Controls.AddAt(2, objAddLink);
            item.Cells[8].Controls.AddAt(3, new LiteralControl("&nbsp;"));

        }

        /// <summary>
        /// This function fills Findings into drop down for Fills after hours notification 
        /// </summary>
        private void fillAfterHoursFindings()
        {
            Device device = null;
            DataTable dtFindings = null;
            try
            {
                device = new Device();
                dtFindings = device.GetFindingsForSubscriber(Convert.ToInt32(ViewState[SUBSCRIBER_ID].ToString()));
                cmbAHFindings.DataSource = dtFindings;
                cmbAHFindings.DataBind();
            }
            catch (Exception ex)
            {
                if (ViewState[SUBSCRIBER_ID] != null)
                    Tracer.GetLogger().LogInfoEvent("profile_notificationStep3.fillAfterHoursFindings:: Exception occured for User ID - " + ViewState[SUBSCRIBER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState[SUBSCRIBER_ID]));
                throw ex;
            }
            finally
            {
                device = null;
                dtFindings = null;
                ListItem listItem = new ListItem("All Findings", "-1");
                cmbAHFindings.Items.Add(listItem);
                cmbAHFindings.Items.FindByValue("-1").Selected = true;
            }
        }

        /// <summary>
        /// This function is to fill records into datagrid grdAfterHoursNotifications for logged in user
        /// if there is no records availble then it will show "No Record Exists"
        /// </summary>
        private void fillAfterHoursNotifications()
        {
            Device device = null;
            DataTable dtAfterHoursNotifications = null;
            try
            {
                device = new Device();
                dtAfterHoursNotifications = device.GetAHNotificationsForSubscriber(int.Parse(ViewState[SUBSCRIBER_ID].ToString()));
                grdAfterHoursNotifications.DataSource = dtAfterHoursNotifications;
                grdAfterHoursNotifications.DataBind();
                if (grdAfterHoursNotifications.Items.Count < 1)
                {
                    lblNoRecordsStep3.Visible = true;
                }
                else
                {
                    lblNoRecordsStep3.Visible = false;
                }
            }
            catch (Exception ex)
            {
                if (ViewState[SUBSCRIBER_ID] != null)
                    Tracer.GetLogger().LogInfoEvent("profile_notificationStep3.fillAfterHoursNotifications:: Exception occured for User ID - " + ViewState[SUBSCRIBER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState[SUBSCRIBER_ID]));
                throw ex;
            }
            finally
            {
                device = null;
                dtAfterHoursNotifications = null;
                generateStep3DataGridHeight();
            }
        }

        /// <summary>
        /// This function is to fill Devices for logged in user into drop down list  for After hour notification events
        /// </summary>
        private void fillSubscriberAfterHoursDeviceOptions()
        {
            Device device = null;
            DataTable dtDevices = null;
            try
            {
                device = new Device();
                dtDevices = device.GetSubscriberDevicesForAfterHours(int.Parse(ViewState[SUBSCRIBER_ID].ToString()));
                cmbAHDevice.DataSource = dtDevices;
                cmbAHDevice.DataBind();
            }
            catch (Exception ex)
            {
                if (ViewState[SUBSCRIBER_ID] != null)
                    Tracer.GetLogger().LogInfoEvent("profile_notificationStep3.fillSubscriberAfterHoursDeviceOptions:: Exception occured for User ID - " + ViewState[SUBSCRIBER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState[SUBSCRIBER_ID]));
                throw ex;
            }
            finally
            {
                device = null;
                dtDevices = null;
            }
        }

        #endregion Step3_3
        #endregion Private Methods
    }
}