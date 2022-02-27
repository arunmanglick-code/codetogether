#region File History

/******************************File History***************************
 * File Name        : custom_notification.aspx.cs
 * Author           : Raju G
 * Created Date     : 26-Aug-2008
 * Purpose          : To provide UI to add new Custom Notification, modify or delete existing Custom Notification.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification
 * 24 Sep 2008  Prerak  FR-> Reset, Cancel and Save button options
 * 26 Sep 2008  Prerak  FR-> Variable Insertion UI
 * 04 Oct 2008  Prerak  FR-> Forwarded Messag Type option
 * ------------------------------------------------------------------- 

 */
#endregion

#region using

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vocada.CSTools.Common;
using Vocada.CSTools.DataAccess;
using Vocada.VoiceLink.Utilities;
using System.Drawing;
using System.Text;

#endregion

namespace Vocada.CSTools
{
    public partial class custom_notifications : System.Web.UI.Page
    {
        #region Private Variables

        private int userID = 0;

        #endregion

        #region Constants

        /// <summary>
        /// Constants for device notes to be displayed
        /// </summary>
        private const string SMSPAGER_DEVICENOTES = "* Not all SMS/Pager devices support 250 characters. Do not exceed your device max character limit.";
        private const string EMAIL_DEVICENOTES = "* Do not disclose confidential information as it will be automatically included in each notification.";
        private const string FAX_DEVICENOTES = "* Fax template file size should not be greater than 1MB.";

        /// <summary>
        /// Constants for grid variables
        /// </summary>
        private const string FIELD_LIST = "FieldList";
        private const string SELECTED_GRID_ITEM = "SelectedItem";
        private const string TEMPLATE_ID = "NotificationTemplateID";
        private const int GRID_ADJUSTMENT_VALUE = 263;


        /// <summary>
        /// These constants stores name of view state which store value of 
        /// grid sort by / direction and default sort by / direction
        /// </summary>
        private const string GRID_SORT_BY = "Grid_Sort_By";
        private const string GRID_SORT_DIRECTION = "Grid_Sort_Direction";
        private const string GRID_DEFAULT_SORTBY = "DeviceDescription";
        private const string GRID_DEFAULT_SORTDIRECTION = "ASC";
        private const string OLD_SUBJECT = "OldSubject";
        private const string OLD_BODY = "OLDBODY";

        /// <summary>
        /// Constants for Config Key Names
        /// </summary>
        private const string CONFIGKEY_FAXTEMPLATEDIRECTORY = "FaxTemplateDirectory";
        private const string CONFIGKEY_FAXTEMPLATEBASEURL = "FaxTemplatebaseURL";

        /// <summary>
        /// Constants for Template file Prefix Name
        /// </summary>
        private const string TEMPLATEFILEPREFIX_OC = "OCFaxTemplate";
        private const string TEMPLATEFILEPREFIX_RC = "RCFaxTemplate";
        private const string TEMPLATEFILEPREFIX_GROUP = "GroupFaxTemplate";
        private const string TEMPLATEFILEPREFIX_UNIT = "UnitFaxTemplate";
        private const string TEMPLATEFILEPREFIX_CT = "CTFaxTemplate";
        #endregion

        #region Events

        /// <summary>
        /// Load and Display the Default Custom Notifications UI for the login Rad / Lab Group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {            
            try
                {
                    if (Session[SessionConstants.USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
                        Response.Redirect(Utils.GetReturnURL("default.aspx", "custom_notifications.aspx", this.Page.ClientQueryString));
                                    
                    registerJavascriptVariables();
                    hdnTextboxID.Value = "";
                    if (!Page.IsPostBack)
                    {
                        //Apply the default sorting
                        ViewState[GRID_SORT_BY] = GRID_DEFAULT_SORTBY;
                        ViewState[GRID_SORT_DIRECTION] = GRID_DEFAULT_SORTDIRECTION;

                        userID = (Session[SessionConstants.USER_ID] != null && Session[SessionConstants.USER_ID].ToString().Length > 0) ? Convert.ToInt32(Session[SessionConstants.USER_ID].ToString()) : 0;

                        fillInstitution();
                        fillCustomNotificationListGrid(-1);
                        fillNotificationTemplateFieldsGrid();
                       
                    }
                    Session[SessionConstants.CURRENT_TAB] = "Tools";
                    Session[SessionConstants.CURRENT_INNER_TAB] = "Custom_Notifications";
                    Session[SessionConstants.CURRENT_PAGE] = "custom_notifications.aspx";
                  
                }
                catch (Exception ex)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.Page_Load", userID.ToString(), ex.Message, ex.StackTrace), userID);                    
                }
        }

        /// <summary>
        /// Hide Fax / General Template UI so that upload control can render first time
        /// To resolve problem for first time upload
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (!Page.IsPostBack)
                displayUIForSelectedDevice(-1);
            base.OnPreRenderComplete(e);
        }

        /// <summary>
        /// Handle institution dropdown selected index changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (String.Compare(cmbInstitution.SelectedValue, "-1") == 0)
                {
                    //Reset the Device and Events DDL
                    resetDDLItems(cmbGroup);
                }
                else
                {
                    //Fill the Groups DDL
                    fillGroups(Convert.ToInt32(cmbInstitution.SelectedValue));
                }
                ViewState[TEMPLATE_ID] = null;
                ViewState[OLD_SUBJECT] = null;
                ViewState[OLD_BODY] = null;
                resetDDLItems(cmbRecipientTypes);
                resetDDLItems(cmbDevices);
                resetDDLItems(cmbEvents);
                fillCustomNotificationListGrid(-1);

                //Display UI for No device selected
                displayUIForSelectedDevice(-1);
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.cmbRecipientTypes_SelectedIndexChanged", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
        }

        /// <summary>
        /// Handle Group dropdown selected index changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (String.Compare(cmbGroup.SelectedValue, "-1") == 0)
                {
                    fillCustomNotificationListGrid(-1);
                    resetDDLItems(cmbRecipientTypes);
                }
                else
                {
                    //Fill the Device and Events DDL
                    fillNotificationRecipientTypesDDL();
                    fillCustomNotificationListGrid(Convert.ToInt32(cmbGroup.SelectedValue));
                }
                ViewState[TEMPLATE_ID] = null;
                ViewState[OLD_SUBJECT] = null;
                ViewState[OLD_BODY] = null;
                //Reset the Device and Events DDL
                resetDDLItems(cmbDevices);
                resetDDLItems(cmbEvents);
                //Display UI for No device selected
                displayUIForSelectedDevice(-1);
                generateDataGridHeight("devices");
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.cmbRecipientTypes_SelectedIndexChanged", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
        }

        /// <summary>
        /// Fill or reset Devices and Event DDL based on selected recipient
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbRecipientTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (String.Compare(cmbRecipientTypes.SelectedValue, "-1") == 0)
                {
                    //Reset the Device and Events DDL
                    resetDDLItems(cmbDevices);
                    resetDDLItems(cmbEvents);
                    //Display UI for No device selected
                    displayUIForSelectedDevice(-1);
                }
                else
                {
                    //Fill the Device and Events DDL
                    fillNotificationRecipientDevicesDDL(Convert.ToInt32(cmbRecipientTypes.SelectedValue), Convert.ToInt32(cmbGroup.SelectedValue));
                    fillNotificationRecipientEventsDDL(Convert.ToInt32(cmbRecipientTypes.SelectedValue));
                }
                generateDataGridHeight("devices");
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.cmbRecipientTypes_SelectedIndexChanged", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
        }

        /// <summary>
        /// Change the Layout of Notification Template based on Device Selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Display UI based on Selected Device
                displayUIForSelectedDevice(Convert.ToInt32(cmbDevices.SelectedValue));

                //Populate the Default Notification Templates
                populateDefaultNotificationTemplate();
                generateDataGridHeight("Device");
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.cmbDevices_SelectedIndexChanged", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
        }

        /// <summary>
        /// Populate the default Notification Templates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Populate the Default Notification Templates
                populateDefaultNotificationTemplate();
                generateDataGridHeight("Events");
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.cmbEvents_SelectedIndexChanged", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
        }

        /// <summary>
        /// Add / Update General Templates (Pager, SMS and Email)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveGeneralTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                NotificationTemplateInfo objNotificationTemplateInfo;
                NotificationTemplate objNotificationTemplate;
                int retValue = 0;
                bool success = false;
                bool overwrite = false;
                string subjectNotInFields = parseFields(txtSubject.Text.Trim());
                string bodyNotInFields = parseFields(txtBody.Text.Trim());
                if (subjectNotInFields.Length > 0 || bodyNotInFields.Length > 0)
                {
                    generateDataGridHeight("addedit");
                    string errMessage = "Following code(s) are invalid:";
                    if (subjectNotInFields.Length > 0)
                        errMessage += "\\n\\nSubject:\\n" + subjectNotInFields;
                    if (bodyNotInFields.Length > 0)
                        errMessage += "\\n\\nBody:\\n" + bodyNotInFields;
                    ScriptManager.RegisterStartupScript(upnlTemplates, upnlTemplates.GetType(), "Error", "<script type=\'text/javascript\'>alert('" + errMessage + "');</script>", false);
                }
                else
                {
                    objNotificationTemplateInfo = new NotificationTemplateInfo();
                    objNotificationTemplateInfo.GroupID = Convert.ToInt32(cmbGroup.SelectedValue);
                    objNotificationTemplateInfo.RecipientID = Convert.ToInt32(cmbRecipientTypes.SelectedValue);
                    objNotificationTemplateInfo.DeviceID = Convert.ToInt32(cmbDevices.SelectedValue);
                    objNotificationTemplateInfo.EventID = Convert.ToInt32(cmbEvents.SelectedValue);
                    objNotificationTemplateInfo.MessageSendType = Convert.ToInt32(cmbMsgType.SelectedValue);
                    objNotificationTemplateInfo.SubjectText = txtSubject.Text.Trim();
                    objNotificationTemplateInfo.BodyText = txtBody.Text.Trim();
                    objNotificationTemplate = new NotificationTemplate();
                    if (ViewState[TEMPLATE_ID] == null)
                        retValue = objNotificationTemplate.AddCustomNotificationTemplate(objNotificationTemplateInfo);
                    else
                    {
                        objNotificationTemplateInfo.NotificationTemplateID = Convert.ToInt32(ViewState[TEMPLATE_ID]);
                        overwrite = Convert.ToBoolean(hdnOverWrite.Value);
                        success = objNotificationTemplate.UpdateCustomNotificationTemplate(objNotificationTemplateInfo, overwrite);
                        if (success)
                        {
                            ViewState[TEMPLATE_ID] = null;
                            ViewState[OLD_SUBJECT] = null;
                            ViewState[OLD_BODY] = null;
                        }
                        else
                            retValue = -1;
                    }
                    if (retValue == -1)
                    {
                        generateDataGridHeight("edit");
                        ScriptManager.RegisterStartupScript(upnlTemplates, upnlTemplates.GetType(), "Error", "<script type=\'text/javascript\'>alert('Custom Template already exist for this selection.');</script>", false);
                    }
                    else
                    {
                        resetControls();
                        displayUIForSelectedDevice(-1);
                        hdnDataChanged.Value = "false";
                        hdnOverWrite.Value = "false";
                        hdnIsEdit.Value = "false";
                        hdnOtherMsgTypeExists.Value  = "false";
                        fillCustomNotificationListGrid(Convert.ToInt32(cmbGroup.SelectedValue));
                        upnlCustomNotifications.Update();
                        
                        //generateDataGridHeight("add");
                    }
                }
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.dlistDevices_SelectedIndexChanged", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
        }

        /// <summary>
        /// Set the default template for the fax - Use Default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnUseDefault_Click(object sender, EventArgs e)
        {
            try
            {
                NotificationTemplateInfo objNotificationTemplateInfo;
                NotificationTemplate objNotificationTemplate;
                objNotificationTemplateInfo = new NotificationTemplateInfo();
                objNotificationTemplateInfo.GroupID = Convert.ToInt32(cmbGroup.SelectedValue);
                objNotificationTemplateInfo.RecipientID = Convert.ToInt32(cmbRecipientTypes.SelectedValue);
                objNotificationTemplateInfo.DeviceID = Convert.ToInt32(cmbDevices.SelectedValue);
                objNotificationTemplateInfo.EventID = Convert.ToInt32(cmbEvents.SelectedValue);
                objNotificationTemplateInfo.MessageSendType = Convert.ToInt32(cmbMsgType.SelectedValue);
                string backupFileName = string.Empty;
                if (ViewState[TEMPLATE_ID] != null && ViewState[TEMPLATE_ID].ToString().Trim().Length != 0)
                {
                    objNotificationTemplateInfo.FaxTemplateURL = string.Empty;
                    int index = lnkFaxUrl.NavigateUrl.LastIndexOf("/") + 1;
                    backupFileName = lnkFaxUrl.NavigateUrl.Substring(index);
                    backupFileName = (backupFileName.Equals("&nbsp;")) ? string.Empty : backupFileName;
                    objNotificationTemplate = new NotificationTemplate();
                    //Update Fax Template
                    objNotificationTemplateInfo.NotificationTemplateID = Convert.ToInt32(ViewState[TEMPLATE_ID].ToString());
                    bool overwrite = Convert.ToBoolean(hdnOverWrite.Value);
                    //Update the Notification Template
                    bool isUpdated = objNotificationTemplate.UpdateCustomNotificationTemplate(objNotificationTemplateInfo, overwrite);

                    if (isUpdated)
                    {
                        //if (backupFileName.Trim().Length > 0)
                        //{
                        //    deleteCustomFaxTemplateFile(backupFileName);
                        //}
                        ViewState[TEMPLATE_ID] = null;
                        ViewState[OLD_SUBJECT] = null;
                        ViewState[OLD_BODY] = null;
                        resetControls();
                        fillCustomNotificationListGrid(Convert.ToInt32(cmbGroup.SelectedValue));
                        displayUIForSelectedDevice(-1);
                        upnlCustomNotifications.Update();
                        hdnDataChanged.Value = "false";
                        hdnOverWrite.Value = "false";
                        hdnIsEdit.Value = "false";
                        hdnOtherMsgTypeExists.Value = "false";
                    }
                }
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.lbtnUseDefault_Click", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
        }

        /// <summary>
        /// Add the fax template 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveFaxTemplate_Click(object sender, EventArgs e)
        {
            try
            {

                NotificationTemplateInfo objNotificationTemplateInfo;
                NotificationTemplate objNotificationTemplate;
                objNotificationTemplateInfo = new NotificationTemplateInfo();
                objNotificationTemplateInfo.GroupID = Convert.ToInt32(cmbGroup.SelectedValue);
                objNotificationTemplateInfo.RecipientID = Convert.ToInt32(cmbRecipientTypes.SelectedValue);
                objNotificationTemplateInfo.DeviceID = Convert.ToInt32(cmbDevices.SelectedValue);
                objNotificationTemplateInfo.EventID = Convert.ToInt32(cmbEvents.SelectedValue);
                objNotificationTemplateInfo.MessageSendType = Convert.ToInt32(cmbMsgType.SelectedValue);
                bool overwrite = false;
                string url = string.Empty;
                string directory = "";
                string filename = "";
                if (ViewState[TEMPLATE_ID] != null && ViewState[TEMPLATE_ID].ToString().Trim().Length != 0)
                {
                    //Edit - Fax Template 
                    //Need to be Uploaded - means if we have uploaded any new file
                    //or Previous uploaded custom template is attached with hyperlink
                    if (lbtnUseDefault.Visible || flupdCTFaxTemplate.HasFile)
                        url = upload();
                }
                else
                {
                    //Add Fax Template  
                    if (flupdCTFaxTemplate.HasFile)
                        url = upload();
                }
                directory = ConfigurationManager.AppSettings[CONFIGKEY_FAXTEMPLATEDIRECTORY];
                filename = directory + url;
                objNotificationTemplateInfo.FaxTemplateURL = url;
                objNotificationTemplate = new NotificationTemplate();
                int retValue = 0;
                if (ViewState[TEMPLATE_ID] != null && ViewState[TEMPLATE_ID].ToString().Trim().Length != 0)
                {
                    //Update Fax Template
                    objNotificationTemplateInfo.NotificationTemplateID = Convert.ToInt32(ViewState[TEMPLATE_ID].ToString());
                    if (ifFileExist(filename))
                    {
                        //Update the Notification Template
                        overwrite = Convert.ToBoolean(hdnOverWrite.Value);
                        bool isUpdated = objNotificationTemplate.UpdateCustomNotificationTemplate(objNotificationTemplateInfo,overwrite);
                        if (isUpdated)
                        {
                            saveFile(directory, url);
                            ViewState[TEMPLATE_ID] = null;
                            ViewState[OLD_SUBJECT] = null;
                            ViewState[OLD_BODY] = null;
                        }
                        else
                            retValue = -1;
                    }
                    else
                    {
                        saveFile(directory, url);
                        overwrite = Convert.ToBoolean(hdnOverWrite.Value);
                        bool isUpdated = objNotificationTemplate.UpdateCustomNotificationTemplate(objNotificationTemplateInfo,overwrite);
                        if (isUpdated)
                        {
                            ViewState[TEMPLATE_ID] = null;
                            ViewState[OLD_SUBJECT] = null;
                            ViewState[OLD_BODY] = null;
                        }
                        else
                            retValue = -1;
                    }
                }
                else
                {
                    //Add Fax Template
                    if (ifFileExist(filename))
                    {
                        retValue = objNotificationTemplate.AddCustomNotificationTemplate(objNotificationTemplateInfo);
                        if (retValue > 0)
                            saveFile(directory, url);
                    }
                    else
                    {
                        saveFile(directory, url);
                        retValue = objNotificationTemplate.AddCustomNotificationTemplate(objNotificationTemplateInfo);
                    }
                }
                if (retValue == -1)
                {
                    generateDataGridHeight("edit");
                    ScriptManager.RegisterStartupScript(upnlTemplates, upnlTemplates.GetType(), "Error", "<script type=\'text/javascript\'>alert('Custom Template already exist for this selection.');</script>", false);
                }
                else
                {
                    resetControls();
                    fillCustomNotificationListGrid(Convert.ToInt32(cmbGroup.SelectedValue));
                    displayUIForSelectedDevice(-1);
                    upnlCustomNotifications.Update();
                    hdnDataChanged.Value = "false";
                    hdnOverWrite.Value = "false";
                    hdnIsEdit.Value = "false";
                    hdnOtherMsgTypeExists.Value = "false";
                }
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.lbtnAddEditFax_Click", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Handle the edit command of the grid
        /// Get the values of the template from Grid rows and
        /// Populate the Edit Custom Notification Template UI
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdCustomNotifications_EditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                ViewState[TEMPLATE_ID] = e.Item.Cells[0].Text;
                hdnIsEdit.Value  = "true";
                cmbRecipientTypes.SelectedValue = e.Item.Cells[4].Text;
                fillNotificationRecipientDevicesDDL(int.Parse(cmbRecipientTypes.SelectedValue),int.Parse(cmbGroup.SelectedValue));
                fillNotificationRecipientEventsDDL(int.Parse(cmbRecipientTypes.SelectedValue));
                cmbDevices.SelectedValue = e.Item.Cells[2].Text;
                displayUIForSelectedDevice(int.Parse(cmbDevices.SelectedValue));

                int recipientID = Convert.ToInt32(cmbRecipientTypes.SelectedValue);
                int eventID = Convert.ToInt32(e.Item.Cells[6].Text);
                bool escalation = Convert.ToBoolean(e.Item.Cells[13].Text);
                
                NotificationRecipient targetRecipient = (NotificationRecipient)recipientID;
                if ( (targetRecipient == NotificationRecipient.OrderingClinician ||
                     targetRecipient == NotificationRecipient.ClinicalTeam ||
                     targetRecipient == NotificationRecipient.Unit) && escalation && 
                     (eventID == 1 || eventID == 2)) //Primary Backup Escalation
                {
                    eventID = eventID + 3;
                    cmbEvents.SelectedValue = eventID.ToString();
                }                
                else
                    cmbEvents.SelectedValue = e.Item.Cells[6].Text;

                if (int.Parse(cmbDevices.SelectedValue) != NotificationDevice.Fax.GetHashCode())
                {
                    string subjectText = e.Item.Cells[10].Text;
                    txtSubject.Text = (subjectText.Equals("&nbsp;")) ? string.Empty : subjectText;
                    txtBody.Text = e.Item.Cells[11].Text;
                    ViewState[OLD_SUBJECT] = txtSubject.Text;
                    ViewState[OLD_BODY] = txtBody.Text;
                }
                else
                {

                    string url = ConfigurationManager.AppSettings[CONFIGKEY_FAXTEMPLATEBASEURL];
                    string fileName = e.Item.Cells[12].Text.Trim();
                    fileName = (fileName.Equals("&nbsp;")) ? string.Empty : fileName;
                    if (fileName.Length == 0)
                    {
                        //Default Template 
                        lbtnUseDefault.Visible = false;
                        //Populate the Default Notification Templates
                        populateDefaultNotificationTemplate();
                    }
                    else
                    {
                        //Custom Template 
                        url += fileName;
                        lnkFaxUrl.NavigateUrl = url;
                        lbtnUseDefault.Visible = true;
                    }
                }
                cmbMsgType.SelectedValue = e.Item.Cells[8].Text;
                hdnOverWrite.Value = "false";
                hdnOldMsgType.Value = cmbMsgType.SelectedValue;
                hdnOtherMsgTypeExists.Value = Convert.ToBoolean(e.Item.Cells[16].Text).ToString();
                //Page.SetFocus(e.Item.ClientID);

                e.Item.BackColor = Color.FromName("#ffffcc");

                ViewState[SELECTED_GRID_ITEM] = e.Item.ClientID;
                generateDataGridHeight("edit");

            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.grdCustomNotifications_EditCommand", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
        }

        /// <summary>
        /// Handle the Delete Command of the grid
        /// Delete the Notification and update the grid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdCustomNotifications_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                int templateID = int.Parse(e.Item.Cells[0].Text);
                string fileName = e.Item.Cells[12].Text.Trim();
                fileName = (fileName.Equals("&nbsp;")) ? string.Empty : fileName;
                NotificationTemplate objNotificationTemplate = new NotificationTemplate();
                bool isDeleted = objNotificationTemplate.DeleteCustomNotificationTemplate(templateID);
                //if (isDeleted && fileName.Trim().Length > 0)
                //{
                //    deleteCustomFaxTemplateFile(fileName);
                //}
                hdnDataChanged.Value = "false";
                fillCustomNotificationListGrid(Convert.ToInt32(cmbGroup.SelectedValue));
                resetControls();
                ViewState[TEMPLATE_ID] = null;
                ViewState[OLD_SUBJECT] = null;
                ViewState[OLD_BODY] = null;
                displayUIForSelectedDevice(-1);
                upnlCustomNotifications.Update();
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.grdCustomNotifications_DeleteCommand", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
        }

        /// <summary>
        /// Update the grid links to ensure data has not been changed before
        /// proceeding with other events
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdCustomNotifications_OnItemCreated(object source, DataGridItemEventArgs e)
        {
            string editScript = "javascript:";
            string deleteScript = "javascript:";
            LinkButton lbtnEdit = null;
            LinkButton lbtnDelete = null;

            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    lbtnEdit = (e.Item.Cells[14].Controls[1]) as LinkButton;
                    lbtnDelete = (e.Item.Cells[15].Controls[1]) as LinkButton;

                    editScript += "if(someDataChanged()){";
                    deleteScript += "if(ConformBeforeDelete()){";
                    if (e.Item.ItemIndex + 2 < 10)
                    {
                        editScript += "__doPostBack('ctl00$ContentPlaceHolder1$grdCustomNotifications$ctl0" + (e.Item.ItemIndex + 2) + "$lnkButEdit', '');";
                        deleteScript += "__doPostBack('ctl00$ContentPlaceHolder1$grdCustomNotifications$ctl0" + (e.Item.ItemIndex + 2) + "$lnkButDelete', '');";
                    }
                    else
                    {
                        editScript += "__doPostBack('ctl00$ContentPlaceHolder1$grdCustomNotifications$ctl" + (e.Item.ItemIndex + 2) + "$lnkButEdit', '');";
                        deleteScript += "__doPostBack('ctl00$ContentPlaceHolder1$grdCustomNotifications$ctl" + (e.Item.ItemIndex + 2) + "$lnkButDelete', '');";
                    }

                    lbtnEdit.OnClientClick = editScript + "return false;} else {return false;}";
                    lbtnDelete.OnClientClick = deleteScript + "return false;} else {return false;}";
                }
                else if (e.Item.ItemType == ListItemType.Header && e.Item.Cells[1].Controls.Count > 0)
                {
                    ((e.Item.Cells[1].Controls[0]) as LinkButton).OnClientClick += "javascript:if(someDataChanged()){__doPostBack('ctl00$ContentPlaceHolder1$grdCustomNotifications$ctl01$ctl00', '');return false;}else{return false;}";
                    ((e.Item.Cells[3].Controls[0]) as LinkButton).OnClientClick += "javascript:if(someDataChanged()){__doPostBack('ctl00$ContentPlaceHolder1$grdCustomNotifications$ctl01$ctl01', '');return false;}else{return false;}";
                    ((e.Item.Cells[5].Controls[0]) as LinkButton).OnClientClick += "javascript:if(someDataChanged()){__doPostBack('ctl00$ContentPlaceHolder1$grdCustomNotifications$ctl01$ctl02', '');return false;}else{return false;}";
                }
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.grdCustomNotifications_OnItemCreated", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
            finally
            {
                lbtnEdit = null;
                lbtnDelete = null;
            }
        }

        /// <summary>
        ///  This event sorts the grid with selected column
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdCustomNotifications_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            try
            {
                if (ViewState[GRID_SORT_BY] == null)
                {
                    ViewState[GRID_SORT_BY] = e.SortExpression;
                    ViewState[GRID_SORT_DIRECTION] = GRID_DEFAULT_SORTDIRECTION;
                }
                else
                {
                    //If user has selected same column for sorting then sort order should be in reverse order
                    if (string.Compare(ViewState[GRID_SORT_BY].ToString(), e.SortExpression) == 0)
                    {
                        if (string.Compare(ViewState[GRID_SORT_DIRECTION].ToString(), "ASC") == 0)
                        {
                            ViewState[GRID_SORT_DIRECTION] = "DESC";
                        }
                        else
                        {
                            ViewState[GRID_SORT_DIRECTION] = "ASC";
                        }
                    }
                    else
                    {
                        // If user has selected different sort order than previous then sort order should be Ascending
                        ViewState[GRID_SORT_BY] = e.SortExpression;
                        ViewState[GRID_SORT_DIRECTION] = GRID_DEFAULT_SORTDIRECTION;
                    }
                }

                //Populate Notification Templates Grid for login rad / lab group                
                fillCustomNotificationListGrid(Convert.ToInt32(cmbGroup.SelectedValue));
                upnlCustomNotifications.Update();

            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.grdCustomNotifications_SortCommand", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
        }

        /// <summary>
        /// Handle the reset General template UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {

            if (ViewState[TEMPLATE_ID] != null)
            {
                //Edit Mode 
                txtBody.Text = ViewState[OLD_BODY] != null ? ViewState[OLD_BODY].ToString() : "";
                txtSubject.Text = ViewState[OLD_SUBJECT] != null ? ViewState[OLD_SUBJECT].ToString() : "";
                hdnDataChanged.Value = "false";
                cmbMsgType.SelectedValue = hdnOldMsgType.Value;
            }
            else
            {
                //Add Mode  - Populate default Notification Template
                populateDefaultNotificationTemplate();
            }
        }
        /// <summary>
        /// Handle the Cancel functionality of General template UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {

            resetControls();
            ViewState[TEMPLATE_ID] = null;
            ViewState[OLD_SUBJECT] = null;
            ViewState[OLD_BODY] = null;
            displayUIForSelectedDevice(-1);
            hdnDataChanged.Value = "false";
            hdnTextboxID.Value = "";
            hdnDataChanged.Value = "false";
            hdnIsEdit.Value = "false";
            hdnOtherMsgTypeExists.Value = "false";
            generateDataGridHeight("Cencel");
            upnlCustomNotifications.Update();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// This method fills the institution combo box
        /// </summary>
        private void fillInstitution()
        {
            DataTable dtInstitution = new DataTable();
            dtInstitution = Utility.GetInstitutionList();
            cmbInstitution.DataSource = dtInstitution;
            cmbInstitution.DataBind();

            ListItem li = new ListItem("<Select>", "-1");
            cmbInstitution.Items.Add(li);
            cmbInstitution.Items.FindByValue("-1").Selected = true;

            //Resets the other dropdowns
            resetDDLItems(cmbGroup);
            resetDDLItems(cmbRecipientTypes);
            resetDDLItems(cmbDevices);
            resetDDLItems(cmbEvents);
        }

        /// <summary>
        /// This method fills the group combo box
        /// </summary>
        private void fillGroups(int instituteID)
        {
            DataTable dtGroups = new DataTable();
            dtGroups = Utility.GetGroups(instituteID);
            cmbGroup.DataSource = dtGroups;
            cmbGroup.DataBind();

            ListItem li = new ListItem("<Select>", "-1");
            cmbGroup.Items.Add(li);
            cmbGroup.Items.FindByValue("-1").Selected = true;

            //Resets the other dropdowns and grids
            resetDDLItems(cmbRecipientTypes);
            resetDDLItems(cmbDevices);
            resetDDLItems(cmbEvents);
            fillCustomNotificationListGrid(-1);
        }

        /// <summary>
        /// This function is used to fill Recipient Type DDL
        /// It called Business Layer Class NotificationTemplate - GetNotificationRecipientTypes()
        /// to get the Notification Recipient Type list
        /// </summary>
        private void fillNotificationRecipientTypesDDL()
        {
            NotificationTemplate objNotificattionTemplate = null;
            try
            {
                objNotificattionTemplate = new NotificationTemplate();
                using (DataTable dtRecipientTypes = objNotificattionTemplate.GetNotificationRecipientTypes())
                {
                    cmbRecipientTypes.DataTextField = "Recipient";
                    cmbRecipientTypes.DataValueField = "RecipientID";
                    cmbRecipientTypes.DataSource = dtRecipientTypes;
                    cmbRecipientTypes.DataBind();
                    ListItem liDefaultItem = new ListItem("<Select>", "-1");
                    cmbRecipientTypes.Items.Add(liDefaultItem);
                    cmbRecipientTypes.Items.FindByValue("-1").Selected = true;
                }
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.fillNotificationRecipientTypeDDL", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
            finally
            {
                objNotificattionTemplate = null;
            }
        }

        /// <summary>
        /// This function is used to populate devices for the target recipient
        /// It called Business Layer Class Notification Template - GetNotificationRecipientDevices() method
        /// to get the devices list for the target recipient
        /// </summary>
        /// <param name="recipientID">ID of target Recipient Type</param>
        private void fillNotificationRecipientDevicesDDL(int recipientID,int groupID)
        {
            NotificationTemplate objNotificattionTemplate = null;
            try
            {
                objNotificattionTemplate = new NotificationTemplate();
                using (DataTable dtDevices = objNotificattionTemplate.GetNotificationRecipientDevices(recipientID,groupID))
                {

                    cmbDevices.DataTextField = "DeviceDescription";
                    cmbDevices.DataValueField = "DeviceID";
                    cmbDevices.DataSource = dtDevices;
                    cmbDevices.DataBind();
                    ListItem liDefaultItem = new ListItem("<Select>", "-1");
                    cmbDevices.Items.Add(liDefaultItem);
                    cmbDevices.Items.FindByValue("-1").Selected = true;
                    //Display UI for No device selected
                    displayUIForSelectedDevice(-1);
                }
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.fillNotificationRecipientDeviceDDL", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
            finally
            {
                objNotificattionTemplate = null;
            }
        }

        /// <summary>
        /// This function is used to populate Events DDL for the target recipient
        /// It called Business Layer class Notification template - GetNotificationRecipientEvents() method
        /// to get the events list for the target recipient
        /// </summary>
        /// <param name="recipientID">ID of the target recipient</param>
        private void fillNotificationRecipientEventsDDL(int recipientID)
        {
            NotificationTemplate objNotificattionTemplate = null;
            try
            {
                objNotificattionTemplate = new NotificationTemplate();
                using (DataTable dtEvents = objNotificattionTemplate.GetNotificationRecipientEvents(recipientID))
                {
                    cmbEvents.DataTextField = "EventDescription";
                    cmbEvents.DataValueField = "EventID";
                    cmbEvents.DataSource = dtEvents;
                    cmbEvents.DataBind();

                    //To Insert Primary and Backup Escalation option for 
                    //OC, CT and Unit Recipient
                    NotificationRecipient targetRecipient = (NotificationRecipient)recipientID;
                    if (targetRecipient == NotificationRecipient.OrderingClinician ||
                        targetRecipient == NotificationRecipient.ClinicalTeam ||
                        targetRecipient == NotificationRecipient.Unit)
                    {
                        ListItem liPEscalationItem = new ListItem("Primary Escalation", "4");
                        ListItem liBEscalationItem = new ListItem("Backup Escalation", "5");
                        cmbEvents.Items.Insert(1, liPEscalationItem);
                        cmbEvents.Items.Insert(3, liBEscalationItem);
                    }
                    ListItem liDefaultItem = new ListItem("<Select>", "-1");
                    cmbEvents.Items.Add(liDefaultItem);
                    cmbEvents.Items.FindByValue("-1").Selected = true;
                }
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.fillNotificationRecipientEventDDL", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
            finally
            {
                objNotificattionTemplate = null;
            }
        }

        /// <summary>
        /// Reset the target dropdown with default item
        /// </summary>
        /// <param name="targetDropdown"></param>
        private void resetDDLItems(DropDownList targetDropdown)
        {
            targetDropdown.Items.Clear();
            ListItem liDefaultItem = new ListItem("<Select>", "-1");
            targetDropdown.Items.Add(liDefaultItem);
            targetDropdown.Items.FindByValue("-1").Selected = true;
        }

        /// <summary>
        /// This function is used to display UI for Selected Device
        /// </summary>
        /// <param name="selectedDeviceID">ID of selected device</param>
        private void displayUIForSelectedDevice(int selectedDeviceID)
        {
            divFaxTemplate.Visible = false;
            divGeneralTemplate.Visible = false;
            lblDeviceNotes.Text = string.Empty;
            txtSubject.Enabled = true;

            bool isEnabled = (ViewState[TEMPLATE_ID] == null);
            cmbDevices.Enabled = isEnabled;
            cmbEvents.Enabled = isEnabled;
            cmbRecipientTypes.Enabled = isEnabled;

            if (selectedDeviceID <= -1)
            {
                upnlTemplates.Update();
                return;
            }
            NotificationDevice selectedDeviceEnum = (NotificationDevice)selectedDeviceID;
            switch (selectedDeviceEnum)
            {
                case NotificationDevice.EMail:
                    divGeneralTemplate.Visible = true;
                    lblDeviceNotes.Text = EMAIL_DEVICENOTES;
                    lblBodyTextMaxLength.Visible = false;
                    break;
                case NotificationDevice.Fax:
                    divFaxTemplate.Visible = true;
                    lblDeviceNotes.Text = FAX_DEVICENOTES;
                    lbtnUseDefault.Visible = false;
                    break;
                case NotificationDevice.PagerNumRegular:
                    txtSubject.Enabled = false;
                    divGeneralTemplate.Visible = true;
                    lblDeviceNotes.Text = SMSPAGER_DEVICENOTES;
                    lblBodyTextMaxLength.Visible = true;
                    break;
                default:
                    divGeneralTemplate.Visible = true;
                    lblDeviceNotes.Text = SMSPAGER_DEVICENOTES;
                    lblBodyTextMaxLength.Visible = true;
                    break;
            }

            upnlTemplates.Update();
            //generateDataGridHeight("devices");
        }

        /// <summary>
        /// This function is used to populate grid of Notification Template Fields
        /// It called Business Layer function NotificationTemplate - GetNotificationTemplateFieldList()
        /// to get the Template fields
        /// </summary>        
        private void fillNotificationTemplateFieldsGrid()
        {
            NotificationTemplate objNotificattionTemplate = null;
            try
            {
                objNotificattionTemplate = new NotificationTemplate();
                using (DataTable dtNotificationTemplateFields = objNotificattionTemplate.GetNotificationTemplateFieldList())
                {
                    string strFieldList = ",";
                    grdNotificationTemplateFields.DataSource = dtNotificationTemplateFields;
                    grdNotificationTemplateFields.DataBind();
                    int count = dtNotificationTemplateFields.Rows.Count;
                    for (int cnt = 0; cnt < count; cnt++)
                    {
                        strFieldList += dtNotificationTemplateFields.Rows[cnt]["Code"].ToString() + ",";
                    }
                    ViewState[FIELD_LIST] = strFieldList;
                }
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.fillNotificationTemplateFieldsGrid", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
            finally
            {
                objNotificattionTemplate = null;
            }
        }

        /// <summary>
        /// This function is used to populate Custom Notification List Grid for the login rad / lab group
        /// It calls Business Layer method NotificationTemplate - GetCustomNotificationTemplateList() to
        /// get the list of custom notification templates for login rad / lab group
        /// </summary>
        /// <param name="groupID">ID of the login rad / lab group</param>
        private void fillCustomNotificationListGrid(int groupID)
        {
            NotificationTemplate objNotificattionTemplate = null;
            try
            {
                objNotificattionTemplate = new NotificationTemplate();
                using (DataTable dtNotificationTemplates = objNotificattionTemplate.GetCustomNotificationTemplateList(groupID))
                {
                    grdCustomNotifications.AllowSorting = (dtNotificationTemplates.Rows.Count > 1);
                    DataView dvNotificationTemplates = dtNotificationTemplates.DefaultView;
                    if (ViewState[GRID_SORT_BY] != null && ViewState[GRID_SORT_DIRECTION] != null)
                        dvNotificationTemplates.Sort = ViewState[GRID_SORT_BY].ToString() + " " + ViewState[GRID_SORT_DIRECTION].ToString();
                    grdCustomNotifications.DataSource = dvNotificationTemplates;
                    grdCustomNotifications.DataBind();
                    generateDataGridHeight("Bind");
                }
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.fillCustomNotificationListGrid", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
            finally
            {
                objNotificattionTemplate = null;
            }
        }

        /// <summary>
        /// This Method parse subject text and Body text for the valid fields.
        /// </summary>
        /// <param name="parsestring"></param>
        /// <returns></returns>
        private string parseFields(string parsestring)
        {
            char[] delimiterChars = { '[' };
            string notFieldValue = "";
            string strFieldList = ""; ;
            string[] arrFields = parsestring.Split(delimiterChars);
            if (ViewState[FIELD_LIST] != null)
                strFieldList = ViewState[FIELD_LIST].ToString();
            foreach (string field in arrFields)
            {
                if (field.IndexOf(']') >= 0)
                {
                    string token = field.Substring(0, field.IndexOf(']'));
                    int index = strFieldList.IndexOf(",[" + token + "],");
                    if (index == -1)
                        notFieldValue += "[" + token + "], ";
                }
            }
            if (notFieldValue.Length > 0)
                notFieldValue = notFieldValue.Substring(0, notFieldValue.Length - 2);
            return notFieldValue;

        }

        /// <summary>
        /// Check for Directory and upload file over there
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        private string upload()
        {
            string url = "";
            try
            {
                if (flupdCTFaxTemplate.HasFile)
                {
                    int recipientID = Convert.ToInt32(cmbRecipientTypes.SelectedValue);
                    int groupID = Convert.ToInt32(cmbGroup.SelectedValue);
                    int deviceID = Convert.ToInt32(cmbDevices.SelectedValue);
                    int EventID = Convert.ToInt32(cmbEvents.SelectedValue);
                    int MsgSendType = Convert.ToInt32(cmbMsgType.SelectedValue);
                    string fileName = flupdCTFaxTemplate.FileName;
                    string directory = ConfigurationManager.AppSettings[CONFIGKEY_FAXTEMPLATEDIRECTORY];

                    if (!System.IO.Directory.Exists(directory))
                        System.IO.Directory.CreateDirectory(directory);

                    switch (recipientID)
                    {
                        case (int)NotificationRecipient.OrderingClinician:
                            url = TEMPLATEFILEPREFIX_OC + "_" + groupID + "_" + EventID + "_" + MsgSendType + fileName.Substring(fileName.Length - 4);
                            break;
                        case (int)NotificationRecipient.Group:
                            url = TEMPLATEFILEPREFIX_GROUP + "_" + groupID + "_" + EventID + "_" + MsgSendType + fileName.Substring(fileName.Length - 4);
                            break;
                        case (int)NotificationRecipient.ReportingClinician:
                            url = TEMPLATEFILEPREFIX_RC + "_" + groupID + "_" + EventID + "_" + MsgSendType + fileName.Substring(fileName.Length - 4);
                            break;
                        case (int)NotificationRecipient.ClinicalTeam:
                            url = TEMPLATEFILEPREFIX_CT + "_" + groupID + "_" + EventID + "_" + MsgSendType + fileName.Substring(fileName.Length - 4);
                            break;
                        case (int)NotificationRecipient.Unit:
                            url = TEMPLATEFILEPREFIX_UNIT + "_" + groupID + "_" + EventID + "_" + MsgSendType + fileName.Substring(fileName.Length - 4);
                            break;
                    }
                    return url;
                }
                else
                {
                    if (lnkFaxUrl.NavigateUrl != "")
                    {
                        int index = lnkFaxUrl.NavigateUrl.LastIndexOf("/") + 1;
                        url = lnkFaxUrl.NavigateUrl.Substring(index);

                    }
                }
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.upload", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
            return url;
        }

        /// <summary>
        /// Reset controls to initial state
        /// </summary>
        private void resetControls()
        {
            cmbRecipientTypes.SelectedValue = "-1";
            resetDDLItems(cmbEvents);
            resetDDLItems(cmbDevices);
            txtBody.Text = "";
            txtSubject.Text = "";
            cmbMsgType.SelectedValue = "-1";
        }

        /// <summary>
        /// This function is to set dynamic height of data grid
        /// </summary>
        private void generateDataGridHeight(string key)
        {
            string script = "<script type=\"text/javascript\">" + Utils.getGridResizeScript(CustomNotificationDiv.ClientID, grdCustomNotifications.ClientID, GRID_ADJUSTMENT_VALUE) + "</script>";
            ScriptManager.RegisterStartupScript(upnlCustomNotifications, upnlCustomNotifications.GetType(), "SetHeight", script, false);
        }

        /// <summary>
        /// Register JavaScript variables and assigned the event to controls
        /// </summary>
        private void registerJavascriptVariables()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var hdnDataChangedClientID = '" + hdnDataChanged.ClientID + "';");
            sbScript.Append("var txtSubjectClientID = '" + txtSubject.ClientID + "';");
            sbScript.Append("var txtBodyClientID = '" + txtBody.ClientID + "';");
            sbScript.Append("var flupdCTFaxTemplateClientID = '" + flupdCTFaxTemplate.ClientID + "';");
            sbScript.Append("var cmbDevicesClientID = '" + cmbDevices.ClientID + "';");
            sbScript.Append("var cmbEventsClientID = '" + cmbEvents.ClientID + "';");
            sbScript.Append("var cmbRecipientTypesClientID = '" + cmbRecipientTypes.ClientID + "';");
            sbScript.Append("var btnSaveFaxTemplateClientID = '" + btnSaveFaxTemplate.ClientID + "';");
            sbScript.Append("var btnSaveGeneralTemplateClientID = '" + btnSaveGeneralTemplate.ClientID + "';");
            sbScript.Append("var btnResetClientID = '" + btnReset.ClientID + "';");
            sbScript.Append("var hdnTextboxIDClientID = '" + hdnTextboxID.ClientID + "';");
            sbScript.Append("var hdnOldMsgTypeClientID = '" + hdnOldMsgType.ClientID + "';");
            sbScript.Append("var hdnOtherMsgTypeExistsClientID = '" + hdnOtherMsgTypeExists.ClientID + "';");
            sbScript.Append("var hdnOverWriteClientID = '" + hdnOverWrite.ClientID + "';");
            sbScript.Append("var hdnIsEditClientID = '" + hdnIsEdit.ClientID + "';");
            sbScript.Append("var cmbMsgTypeClientID = '" + cmbMsgType.ClientID + "';");
            sbScript.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());

            lbtnUseDefault.Attributes.Add("onclick", "javascript:return ValidateUIForm(true)");
            btnSaveGeneralTemplate.Attributes.Add("onclick", "javascript:return ValidateUIForm(false)");
            btnSaveFaxTemplate.Attributes.Add("onclick", "javascript:return ValidateUIForm(true)");
            cmbDevices.Attributes.Add("onclick", "javascript:dataChanged();");
            cmbEvents.Attributes.Add("onclick", "javascript:dataChanged();");
            cmbRecipientTypes.Attributes.Add("onclick", "javascript:dataChanged();");
            txtSubject.Attributes.Add("onchange", "javascript:dataChanged();");
            txtBody.Attributes.Add("onchange", "javascript:checkBodyLength(this);");
            txtBody.Attributes.Add("onkeyup", "javascript:checkBodyLength(this);");
            cmbMsgType.Attributes.Add("onclick", "javascript:dataChanged();");
            flupdCTFaxTemplate.Attributes.Add("onchange", "javascript:dataChanged();");
            txtSubject.Attributes.Add("onClick", "javascript:getTextboxID('" + txtSubject.ClientID + "');");
            txtBody.Attributes.Add("onClick", "javascript:getTextboxID('" + txtBody.ClientID + "');");
        }

        /// <summary>
        /// Populate Default Notification Template        
        /// </summary>
        private void populateDefaultNotificationTemplate()
        {
            int groupID = Convert.ToInt32(cmbGroup.SelectedValue);
            int recipientID = Convert.ToInt32(cmbRecipientTypes.SelectedValue);
            int deviceID = Convert.ToInt32(cmbDevices.SelectedValue);
            int eventID = Convert.ToInt32(cmbEvents.SelectedValue);

            //No Device Selected
            if (deviceID == -1)
            {
                return;
            }

            NotificationDevice selectedDevice = (NotificationDevice)deviceID;
            if (selectedDevice == NotificationDevice.Fax)
            {
                //Fax Template 
                if (eventID == -1) //No Event Selected
                {
                    lnkFaxUrl.NavigateUrl = "";
                }
                else
                {
                    //Fetch and populate fax template
                    string url = ConfigurationManager.AppSettings[CONFIGKEY_FAXTEMPLATEBASEURL];
                    NotificationTemplate objNotification = new NotificationTemplate();
                    NotificationTemplateInfo objInfo = objNotification.GetDefaultNotificationTemplate(groupID, recipientID, deviceID, eventID);
                    lnkFaxUrl.NavigateUrl = url + objInfo.FaxTemplateURL;
                }
            }
            else
            {
                //Fax Template
                if (eventID == -1) //No Event Selected
                {
                    txtSubject.Text = txtBody.Text = "";
                }
                else
                {
                    NotificationTemplate objNotification = new NotificationTemplate();
                    NotificationTemplateInfo objInfo = objNotification.GetDefaultNotificationTemplate(groupID, recipientID, deviceID, eventID);

                    //Fetch and Populate General template
                    txtSubject.Text = objInfo.SubjectText;
                    txtBody.Text = objInfo.BodyText;
                }
            }
        }

        /// <summary>
        /// Delete the Fax template files from folder 
        /// physically
        /// </summary>
        /// <param name="fileName"></param>
        private void deleteCustomFaxTemplateFile(string fileName)
        {
            string directoryPath = ConfigurationManager.AppSettings[CONFIGKEY_FAXTEMPLATEDIRECTORY];
            try
            {
                string filePath = directoryPath + "\\" + fileName;
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.deleteCustomFaxTemplateFile", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
        }
        /// <summary>
        /// This Function checks whether fax template file is existis or not
        /// </summary>
        /// <param name="file">full path and name of file</param>
        /// <returns>returns true if exists otherwise false</returns>
        private bool ifFileExist(string file)
        {
            bool retVal = false;
            try
            {
                if (flupdCTFaxTemplate.HasFile)
                {
                    retVal = System.IO.File.Exists(file);
                }
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.ifFileExist()", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
            return retVal;
        }
        /// <summary>
        /// This function check for FaxTemplate directory and upload file into that directory.
        /// </summary>
        /// <param name="directory">Directory path</param>
        /// <param name="url">file name</param>
        private void saveFile(string directory, string url)
        {
            try
            {
                if (flupdCTFaxTemplate.HasFile)
                {
                    if (!System.IO.Directory.Exists(directory))
                        System.IO.Directory.CreateDirectory(directory);
                    directory += @"\" + url;
                    flupdCTFaxTemplate.SaveAs(directory);
                }
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("custom_notifications.saveFile()", userID.ToString(), ex.Message, ex.StackTrace), userID);
                throw ex;
            }
        }
        #endregion
    }
}