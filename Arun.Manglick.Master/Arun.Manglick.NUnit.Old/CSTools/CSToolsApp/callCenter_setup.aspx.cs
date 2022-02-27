#region File History

/******************************File History***************************
 * File Name        : add_callcenter.aspx.cs
 * Author           : Suhas Tarihalkar.
 * Created Date     : 30 May 07
 * Purpose          : This class is responsible for taking user input for call center information and preferences
 *                  : Also it gives facility to add/edit/remove reasons for selected call centers.
 *                  : 

 * *********************File Modification History*********************

 * * Date(dd-mm-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 *  12 Jun 2008 - Prerak    Migration of AJAX Atlas to AJAX RTM 1.0
 *  17 Jul 2008 - SSK       Remove Message closed alert option and added Manually closed confirmation popup option, fixed #3502
 *  19-09-2008  - Suhas     Removeing Alert Functionality
 *  23-09-2008  - SSK       Add institutionid for Agent
 *  25-09-2008  - Suhas     Device Notifications Implementation
 *  26-09-2008  - Suhas     Removed Group and Finding drop down
 * 31-Oct-2008  - Sheetal   Remove Pager validations
 * 14-Nov-2008  - Sheetal   Fixed Defect 3310
 * 17-Nov-2008  - Prerak    CR -> "Back to Call center List" link on call center setup page 
 *                          navigate to to add Call center page and populate call center list.
 * 18-Nov-2008  - Prerak    Defect #4165 Fixed
 * 20-Nov-2008  - IAK       Defect #3113, #4165 Fixed: Handled blank values
 * 20-Nov-2008  - SD        Replaced Call center by Agent Team
 * 11-25-2008   - SD        Defect #4225 fixed
 * 15-11-2008   - Suhas     Defect #4298 - Fixed.
 * 01-05-2009       RG      FR 282 Changes
 * 01-13-2009       RG      Suggestion from Fred
 * ------------------------------------------------------------------- 
 */
#endregion

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.Utilities;

namespace Vocada.CSTools
{
    public partial class callCenter_setup : System.Web.UI.Page
    {
        #region Private Variable
        private int userID;
        private int callCenterID = 0;
        private int institutionID = 0;
        private string callCenterName = "";
        private DataTable dtGridDeviceNotifications;
        private static int rowNo = 1;

        //Constants for Toggle Button Name
        private const string SHOWDETAILS_BUTTONNAME = "Assign Event";
        private const string HIDEDETAILS_BUTTONNAME = "Hide Event Details";
        #endregion

        #region Page Events

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
                Response.Redirect(Utils.GetReturnURL("default.aspx", "add_callcenter.aspx", ""));
            userID = (Session[SessionConstants.USER_ID] != null && Session[SessionConstants.USER_ID].ToString().Length > 0) ? Convert.ToInt32(Session[SessionConstants.USER_ID].ToString()) : 0;
            try
            {
                registerJavascriptVariables();

                Session[SessionConstants.CURRENT_PAGE] = "callCenter_setup.aspx";
                Session[SessionConstants.CURRENT_TAB] = "CallCenter";
                Session[SessionConstants.CURRENT_INNER_TAB] = "CallCenterSetup";

                callCenterID = Convert.ToInt32(Request["CallCenterID"].ToString());
                callCenterName = Request["CallCenterName"].ToString();
                institutionID = Convert.ToInt32(Request["InstitutionID"].ToString());
                lblCCInfoLine.Text = callCenterName;
                ViewState["CallCenterID"] = callCenterID;
                
                lnkAddAgent.NavigateUrl = "add_CC_agent.aspx?CallCenterID=" + callCenterID + "&CallCenterName=" + callCenterName + "&InstitutionID=" + institutionID;
                lnkLable.NavigateUrl = "./add_callcenter.aspx?InstitutionID=" + institutionID;
                if (Session[SessionConstants.DT_NOTIFICATION] == null)
                    dtGridDeviceNotifications = new DataTable();
                else
                    dtGridDeviceNotifications = (DataTable)Session[SessionConstants.DT_NOTIFICATION];

                if (!IsPostBack)
                {
                    populateGroups();
                    populateCallCenterInfo();
                    if (Session["Direction"] == null)
                        Session["Direction"] = " ASC";

                    //Device Notifiation changes
                    dtGridDeviceNotifications.Rows.Clear();
                    fillDevices();
                    getCarriers();
                    fillAgentDevice();
                    fillAgentEvent(cmbEvents);
                    resetControls();
                    //generateDataGridHeight("PageLoad");
                }
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("callCenter_setup - Page_Load", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            CallCenterInformation objCallCenterInformation = null;
            CallCenterPreferences objCallCenterPreferences = null;
            string setfocus = "";
            string errorMessage = "";
            try
            {
                string phoneNumber = txtPrimaryPhone1.Text.Trim() + txtPrimaryPhone2.Text.Trim() + txtPrimaryPhone3.Text.Trim();
                if (phoneNumber.Length < 10)
                {
                    errorMessage = "You must enter valid phone Number for Notifications\\n";
                    setfocus = txtPrimaryPhone1.ClientID;
                }
                if (!Utils.isNumericValue(txtLockedMsgTimeout.Text))
                {
                    errorMessage = errorMessage + "You must enter valid Locked Timeout value\\n";
                    setfocus = txtLockedMsgTimeout.ClientID;
                }
                if (!Utils.isNumericValue(txtAutoLogout.Text))
                {
                    errorMessage = errorMessage + "You must enter valid Autologout value\\n";
                    setfocus = txtAutoLogout.ClientID;
                }
                if (errorMessage.Length > 0)
                {
                    generateDataGridHeight("Error");
                    ScriptManager.RegisterClientScriptBlock(upnlContact, upnlContact.GetType(), "Navigate", "<script type=\'text/javascript\'>alert('" + errorMessage + "');document.getElementById('" + setfocus + "').focus();</script>", false);
                    return;
                }
                objCallCenterInformation = new CallCenterInformation();
                objCallCenterPreferences = new CallCenterPreferences();

                objCallCenterInformation.CallCenterID = Convert.ToInt32(ViewState["CallCenterID"].ToString());
                objCallCenterInformation.ContactName = txtContactName.Text.Trim();
                objCallCenterInformation.ContactPhone = txtPrimaryPhone1.Text.Trim() + txtPrimaryPhone2.Text.Trim() + txtPrimaryPhone3.Text.Trim();
                objCallCenterInformation.Email = txtEmail.Text.Trim();
                objCallCenterInformation.Fax = txtPrimaryFax1.Text.Trim() + txtPrimaryFax2.Text.Trim() + txtPrimaryFax3.Text.Trim();
                objCallCenterInformation.PagerNumber = txtPrimaryPager1.Text.Trim() + txtPrimaryPager2.Text.Trim() + txtPrimaryPager3.Text.Trim();
                objCallCenterInformation.AlternateContactName = txtAlternateContact.Text.Trim();
                objCallCenterInformation.AlternatePhone = txtAlternatePhone1.Text.Trim() + txtAlternatePhone2.Text.Trim() + txtAlternatePhone3.Text.Trim();
                objCallCenterInformation.IsActive = chkActive.Checked;

                if (!txtLockedMsgTimeout.Text.Trim().Equals(""))
                    objCallCenterPreferences.LockedMessageTimeout = txtLockedMsgTimeout.Text.Trim();
                else
                    objCallCenterPreferences.LockedMessageTimeout = null;
                if (!txtAutoLogout.Text.Trim().Equals(""))
                    objCallCenterPreferences.AutoLogout = txtAutoLogout.Text.Trim();
                else
                    objCallCenterPreferences.AutoLogout = null;

                //objCallCenterPreferences.ConfirmationManuallyClosedPopup = rlstConformationManuallyClosedPopup.SelectedValue == "1" ? true : false;
                //objCallCenterPreferences.ConfirmationSendPopup = rlstConfirmationSenPopup.SelectedValue == "1" ? true : false;
                //objCallCenterPreferences.ConfirmationDocPopup = rlstConformationDocPopup.SelectedValue == "1" ? true : false;
                //objCallCenterPreferences.ConfirmationConnectPopup = rlstConfirmationConnectPopup.SelectedValue == "1" ? true : false;

                objCallCenterInformation.objCallCenterPreferences = objCallCenterPreferences;

                updateCallCenterData(objCallCenterInformation);

                hdnTextChanged.Value = "false";
                ScriptManager.RegisterStartupScript(upnlContact, upnlContact.GetType(), "EditCallCenter", "alert('Agent Team information updated successfully.');", true);

            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("callCenter_setup - btnAdd_Click", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
            finally
            {
                objCallCenterInformation = null;
                objCallCenterPreferences = null;
                populateGroups();
                //populateReasons();
                populateCallCenterInfo();
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbDeviceType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmbDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int deviceID;

            try
            {
                if (clstGroupList.Items.Count > 0)
                {
                    if (grdDevices.EditItemIndex == -1)
                    {
                        //fillGroups();
                        //fillFinding();

                        deviceID = Convert.ToInt32(cmbDeviceType.SelectedItem.Value);
                        setLabelsAndInputBoxes(deviceID);
                        if (cmbDeviceType.Items.Count > 0)
                        {
                            if (Convert.ToInt32(cmbDeviceType.SelectedItem.Value) == NotificationDevice.PagerTAP.GetHashCode() || Convert.ToInt32(cmbDeviceType.SelectedItem.Value) == NotificationDevice.PagerTAPA.GetHashCode())
                            {
                                txtNumAddress.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes(hdnTextChangedClientID);");
                                //txtNumAddress.MaxLength = 6;
                                txtEmailGateway.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes(hdnTextChangedClientID);");
                                //txtEmailGateway.MaxLength = 10;
                            }
                            else if ((Convert.ToInt32(cmbDeviceType.SelectedItem.Value) == NotificationDevice.PagerNumSkyTel.GetHashCode() || Convert.ToInt32(cmbDeviceType.SelectedItem.Value) == NotificationDevice.PagerNumUSA.GetHashCode() || Convert.ToInt32(cmbDeviceType.SelectedItem.Value) == NotificationDevice.PagerNumRegular.GetHashCode() || Convert.ToInt32(cmbDeviceType.SelectedItem.Value) == NotificationDevice.pagerPartner.GetHashCode()))
                            {
                                txtNumAddress.Attributes.Add("onkeyPress", "JavaScript:return PagerValidationWithSpace('" + txtNumAddress.ClientID + "');");
                                //txtNumAddress.MaxLength = 100;
                            }
                            else if (Convert.ToInt32(cmbDeviceType.SelectedItem.Value) == NotificationDevice.PagerAlpha.GetHashCode())
                            {
                                txtNumAddress.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes(hdnTextChangedClientID);");
                                //txtNumAddress.MaxLength = 100;
                            }
                            else if (Convert.ToInt32(cmbDeviceType.SelectedItem.Value) != NotificationDevice.EMail.GetHashCode())
                            {
                                txtNumAddress.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes(hdnTextChangedClientID);");
                                txtNumAddress.MaxLength = 10;
                            }
                            else
                            {
                                txtNumAddress.Attributes.Add("onkeyPress", "return true");
                                txtNumAddress.Attributes.Add("onchange", "return true");
                                txtNumAddress.MaxLength = 100;

                                txtEmailGateway.Attributes.Add("onkeypress", "JavaScript:return true;");
                                txtEmailGateway.MaxLength = 100;
                            }
                        }
                        if (deviceID == NotificationDevice.SMS.GetHashCode() || deviceID == NotificationDevice.PagerAlpha.GetHashCode())  // cell or pager.
                            generateGatewayAddress();
                        generateDataGridHeight("DeviceType");
                    }
                    else
                    {
                        generateDataGridHeight("Edit");
                        cmbDeviceType.SelectedValue = "-1";
                    }
                }
                else
                {
                    generateDataGridHeight("DeviceType");
                    cmbDeviceType.SelectedValue = "-1";
                    string script = "alert('No Groups Available.')";
                    ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "Alert", script, true);
                }

                if (grdDevices.EditItemIndex != -1)
                    addLinkToGridInEditMode(grdDevices.Items[grdDevices.EditItemIndex]);
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID].ToString().Length > 0)
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.cmbDeviceType_SelectedIndexChanged:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbCarrier control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmbCarrier_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cmbCarrier.SelectedItem.Value.Equals("-1"))
            {
                txtEmailGateway.Text= "";
                return;
            }

            generateGatewayAddress();
            generateDataGridHeight("cmbcarrie");
        }

        /// <summary>
        /// Handles the Click event of the btnAddDevice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnAddDevice_Click(object sender, System.EventArgs e)
        {
            try
            {
                addAgentNotificationDevices();
                hdnTextChanged.Value = "true";
                hdnGridChanged.Value = "false";
                generateDataGridHeight("Add");
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("edit_oc.btnAddDevice_Click", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
            }
        }

        /// <summary>
        /// Handles the UpdateCommand event of the grdDevices control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        protected void grdDevices_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            hdnTextChanged.Value = "true";
            hdnGridChanged.Value = "false";
            string isAddClicked = hdnIsAddClicked.Value;

            try
            {
                if (!validateRecord(e))
                {
                    addLinkToGridInEditMode(grdDevices.Items[grdDevices.EditItemIndex]);
                    return;
                }

                TextBox da = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[1].FindControl("txtGridDeviceNumber")));
                TextBox gw = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[4].FindControl("txtGridEmailGateway")));
                TextBox gridInitialPauseTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[7].FindControl("txtGridInitialPause")));
                DropDownList cmbGrdEvents = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[5].FindControl("dlistGridEvents")));
                string deviceName = "";
                string deviceAddress = "";
                string strGridGateway = "";
                decimal initialPause;
                string strGridCarrier = "";
                string script = "";
                //Set DeviceName property value   
                deviceName = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[0].FindControl("txtGridDeviceType"))).Text.Trim();

                //Set DeviceAddress property value   
                deviceAddress = da.Text.Trim();

                //Device ID
                int deviceID = Convert.ToInt32(e.Item.Cells[10].Text.Trim());


                if (deviceName.StartsWith("PagerA_") || deviceName.StartsWith("SMS_"))
                {
                    string oldDeviceAddress = ViewState[Constants.DEVICE_ADDRESS].ToString();
                    string oldEmailGateway = ViewState[Constants.EMAIL_GATEWAY].ToString();
                    if (da.Text.Trim() != oldDeviceAddress && gw.Text.Trim() == oldEmailGateway)
                    {
                        int index = oldEmailGateway.IndexOf("@");
                        if (index > -1)
                        {
                            string oldGatewaydeviceNum = oldEmailGateway.Substring(0, oldEmailGateway.IndexOf("@"));

                            if (oldDeviceAddress == oldGatewaydeviceNum)
                            {
                                string deviceAdd = gw.Text.Trim().Substring(0, gw.Text.Trim().IndexOf("@"));
                                if (deviceAdd.Length > 0)
                                    gw.Text = gw.Text.Trim().Replace(deviceAdd, da.Text.Trim());
                                else
                                    gw.Text = da.Text.Trim() + gw.Text.Trim();
                            }
                        }
                    }

                }

                if (gw.Text.Trim().Length > 0)
                {
                    strGridGateway = gw.Text.Trim();
                }
                else
                {
                    strGridGateway = "";
                }

                strGridCarrier = ((Label)(grdDevices.Items[e.Item.ItemIndex].Cells[3].FindControl("lblGridCarrier"))).Text.Trim();

                //Set Carrier property value   
                if (strGridCarrier.Length < 0)
                    strGridCarrier = "";


                //Set DeptNotifyEventID property value   
                int rpNotifyEventID = Convert.ToInt32(cmbGrdEvents.SelectedValue);
                string strEvent = (rpNotifyEventID == 0) ? "" :cmbGrdEvents.SelectedItem.Text.Trim();

                //Set DeptDeviceID property value   
                int rowID = Convert.ToInt32(grdDevices.DataKeys[e.Item.ItemIndex]);

                if (gridInitialPauseTxt.Visible == true)
                    initialPause = Convert.ToInt32(gridInitialPauseTxt.Text.Trim());
                else
                    initialPause = 0;

                if (isAddClicked == "false")
                {
                    //int introwno = e.Item.ItemIndex;
                    DataRow[] editrow = dtGridDeviceNotifications.Select("DeviceName = '" + deviceName + "' AND RowID <>" + rowID);

                    if (editrow.GetLength(0) == 0)
                    {
                        DataRow[] number = dtGridDeviceNotifications.Select("DeviceName = '" + hdnOldDeviceName.Value + "'");
                        int introwno = dtGridDeviceNotifications.Rows.IndexOf(number[0]);
                        string flagModified = "Changed";
                        if (introwno >= 0)
                        {
                            dtGridDeviceNotifications.Rows[introwno].BeginEdit();
                            dtGridDeviceNotifications.Rows[introwno]["DeviceName"] = deviceName.Trim();
                            dtGridDeviceNotifications.Rows[introwno]["DeviceAddress"] = deviceAddress.Trim();
                            dtGridDeviceNotifications.Rows[introwno]["Gateway"] = strGridGateway.Trim();
                            dtGridDeviceNotifications.Rows[introwno]["Carrier"] = strGridCarrier;
                            dtGridDeviceNotifications.Rows[introwno]["EventDescription"] = strEvent;
                            dtGridDeviceNotifications.Rows[introwno]["AgentNotifyEventID"] = rpNotifyEventID;
                            dtGridDeviceNotifications.Rows[introwno]["FindingDescription"] = "";
                            dtGridDeviceNotifications.Rows[introwno]["FindingID"] = -1;
                            dtGridDeviceNotifications.Rows[introwno]["InitialPause"] = initialPause;
                            dtGridDeviceNotifications.Rows[introwno]["FlagModified"] = flagModified;
                            dtGridDeviceNotifications.Rows[introwno].EndEdit();
                            dtGridDeviceNotifications.Rows[introwno].AcceptChanges();
                        }
                        Session[SessionConstants.DT_NOTIFICATION] = dtGridDeviceNotifications;
                        grdDevices.EditItemIndex = -1;
                        bindDevicesForAgent();
                        script = "Device has been updated.";
                    }
                    else
                    {
                        script = "Device Name already exists.";
                        addLinkToGridInEditMode(grdDevices.Items[grdDevices.EditItemIndex]);
                    }
                }
                else
                {

                    rowNo++;
                    string flagModified = "UnChanged";
                    string flagRowType = "New";
                    int callCenterID = Convert.ToInt32(ViewState["CallCenterID"].ToString());
                    DataRow dtrow = dtGridDeviceNotifications.NewRow();
                    dtrow["DeviceName"] = generateDeviceName(deviceID);
                    dtrow["DeviceAddress"] = deviceAddress.Trim();
                    dtrow["Gateway"] = strGridGateway.Trim();
                    dtrow["Carrier"] = strGridCarrier;
                    dtrow["GroupName"] = "";
                    dtrow["EventDescription"] = strEvent;
                    dtrow["FindingDescription"] = "";
                    dtrow["InitialPause"] = initialPause;
                    dtrow["DeviceID"] = deviceID;
                    dtrow["GroupID"] = -1;
                    dtrow["AgentNotificationID"] = 0;
                    dtrow["AgentNotifyEventID"] = rpNotifyEventID;
                    dtrow["FindingID"] = -1;
                    dtrow["RowID"] = rowNo;
                    dtrow["FlagRowType"] = flagRowType;
                    dtrow["FlagModified"] = flagModified;
                    dtrow["CallCenterID"] = callCenterID;                    
                    dtGridDeviceNotifications.Rows.Add(dtrow);
                    Session[SessionConstants.DT_NOTIFICATION] = dtGridDeviceNotifications;
                    grdDevices.EditItemIndex = -1;
                    bindDevicesForAgent();
                    script = "Device has been added.";
                }

                generateDataGridHeight("updatedevice");
                ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "deviceExists", "alert('" + script + "');SetPostbackVarFalse();", true);

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.grdDevices_UpdateCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;

            }
            finally
            {
                hdnIsAddClicked.Value = "false";

            }

        }

        /// <summary>
        /// Handles the ItemDataBound event of the grdDevices control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
        protected void grdDevices_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
                {
                    DataRowView data = (DataRowView)e.Item.DataItem;
                    Label gridFindingLabel = ((Label)(e.Item.Cells[6].FindControl("lblGridDeviceFinding")));
                    Label gridGroupLabel = ((Label)(e.Item.Cells[1].FindControl("lblGridGroup")));
                    Label gridInitialPauseLabel = ((Label)(e.Item.Cells[7].FindControl("lblGridInitialPause")));
                    if (e.Item.ItemType != ListItemType.EditItem)
                    {
                        if ((int)data.Row["FindingID"] == -1)
                        {
                            gridFindingLabel.Text = "All Findings";
                            gridFindingLabel.ToolTip = "All Findings";
                        }
                        else
                        {
                            gridFindingLabel.Text = data.Row["FindingDescription"].ToString();
                            gridFindingLabel.ToolTip = data.Row["FindingDescription"].ToString();
                        }
                    }
                    if (data.Row["GroupID"].ToString() == "" || (int)data.Row["GroupID"] == -1)
                    {
                        if (Convert.ToInt16(data.Row["DeviceID"].ToString()) == NotificationDevice.SMS_WebLink.GetHashCode())
                        {
                            gridGroupLabel.Text = "All Lab Groups";
                            gridGroupLabel.ToolTip = "All Lab Groups";
                        }
                        else
                        {
                            gridGroupLabel.Text = "All Groups";
                            gridGroupLabel.ToolTip = "All Groups";
                        }
                        e.Item.Cells[11].Text = "-1";
                    }
                    else
                    {
                        gridGroupLabel.Text = data.Row["GroupName"].ToString();
                        gridGroupLabel.ToolTip = data.Row["GroupName"].ToString();
                        e.Item.Cells[11].Text = data.Row["GroupID"].ToString();
                    }
                    if (gridInitialPauseLabel != null)
                    {
                        if (data.Row["InitialPause"].ToString() == "0")
                        {
                            gridInitialPauseLabel.Text = "";
                        }
                    }
                    e.Item.Attributes.Add("onclick", "return SetPostbackVarTrue();");
                }
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    DataRowView data = (DataRowView)e.Item.DataItem;
                    TextBox gridInitialPausetxt = ((TextBox)(e.Item.Cells[7].FindControl("txtGridInitialPause")));
                    if (data.Row["InitialPause"].ToString() == "0")
                    {
                        gridInitialPausetxt.Text = "";
                        gridInitialPausetxt.Visible = false;
                    }
                    addLinkToGridInEditMode(e.Item);
                }

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.grdDevices_ItemDataBound:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
        }

        /// <summary>
        /// Handles the EditCommand event of the grdDevices control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        protected void grdDevices_EditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                int intGridEditItemIndex = e.Item.ItemIndex;                
                string deviceName = ((Label)(grdDevices.Items[intGridEditItemIndex].Cells[1].FindControl("lblGridDeviceNumber"))).Text.Trim();
                string deviceGatway = ((Label)(grdDevices.Items[intGridEditItemIndex].Cells[4].FindControl("lblGridDeviceEmailGateway"))).Text.Trim();                
                string deviceType = ((Label)(grdDevices.Items[e.Item.ItemIndex].Cells[0].FindControl("lblGridDeviceType"))).Text.Trim();
                string strFindingText = "";
                if (grdDevices.Items[intGridEditItemIndex].Cells[6].FindControl("lblGridDeviceFinding") != null)
                    strFindingText = ((Label)(grdDevices.Items[intGridEditItemIndex].Cells[6].FindControl("lblGridDeviceFinding"))).Text.Trim();
                string findingID = e.Item.Cells[12].Text.Trim();
                string strEventID = e.Item.Cells[13].Text.Trim();
                grdDevices.EditItemIndex = intGridEditItemIndex;
                editDeviceGrid(intGridEditItemIndex, strEventID, findingID, deviceName, deviceGatway, int.Parse(grdDevices.Items[intGridEditItemIndex].Cells[11].Text.Trim()));
                hdnOldDeviceName.Value = deviceType;
                resetControls();
                hdnTextChanged.Value = "true";
                hdnGridChanged.Value = "false";
                hdnIsAddClicked.Value = "false";
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.grdDevices_EditCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
        }

        /// <summary>
        /// Handles the CancelCommand event of the grdDevices control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        protected void grdDevices_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                hdnTextChanged.Value = "true";
                hdnGridChanged.Value = "false";
                grdDevices.EditItemIndex = -1;
                hdnIsAddClicked.Value = "false";
                bindDevicesForAgent();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.grdDevices_CancelCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
        }

        /// <summary>
        /// Handles the DeleteCommand event of the grdDevices control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        protected void grdDevices_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                hdnTextChanged.Value = "true";
                hdnGridChanged.Value = "false";
                string deviceName = "";               
                if (grdDevices.EditItemIndex == -1)
                {
                    Label gridDeviceNameLabel;
                    gridDeviceNameLabel = ((Label)(e.Item.Cells[0].FindControl("lblGridDeviceType")));
                    deviceName = gridDeviceNameLabel.Text.Trim();
                }
                else
                {
                    TextBox gridDevicenameText;
                    gridDevicenameText = (TextBox)(e.Item.Cells[0].FindControl("txtGridDeviceType"));
                    deviceName = gridDevicenameText.Text.Trim();
                }
                
                DataRow[] deleteRow = dtGridDeviceNotifications.Select("DeviceName = '" + deviceName + "'");
                int rowid = dtGridDeviceNotifications.Rows.IndexOf(deleteRow[0]);
                dtGridDeviceNotifications.Rows[rowid].BeginEdit();
                dtGridDeviceNotifications.Rows[rowid]["FlagModified"] = "Deleted";
                dtGridDeviceNotifications.Rows[rowid].EndEdit();
                dtGridDeviceNotifications.Rows[rowid].AcceptChanges();
                Session[SessionConstants.DT_NOTIFICATION] = dtGridDeviceNotifications;
                grdDevices.EditItemIndex = -1;
                bindDevicesForAgent();
                ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "Delete", "alert('Device has been deleted');", true);

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_oc.grdDevices_DeleteCommand", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        /// <summary>
        /// Handles the ItemCreated event of the grdDevices control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
        protected void grdDevices_ItemCreated(object source, DataGridItemEventArgs e)
        {
            try
            {
                string script = "javascript:";
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    if (e.Item.Cells[0].Controls[1] is TextBox)
                    {
                        LinkButton lbUpdate = (e.Item.Cells[8].Controls[0]) as LinkButton;
                        LinkButton lbCancel = (e.Item.Cells[8].Controls[2]) as LinkButton;
                        lbUpdate.OnClientClick = script + "return ChangeFlagforGrid();";
                        lbCancel.OnClientClick = script + "return ChangeFlagforGrid();";
                    }
                }
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lbtnEdit = (e.Item.Cells[8].Controls[0]) as LinkButton;
                    script += "return ChangeFlagforGrid();"; ;
                    lbtnEdit.OnClientClick = script;
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("edit_oc.btnAddDevice_Click", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
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
                bool isShow = (btnName.Equals(SHOWDETAILS_BUTTONNAME.ToUpper())) ? true : false;
                btnShowHideDetails.Text = (isShow) ? HIDEDETAILS_BUTTONNAME : SHOWDETAILS_BUTTONNAME;
                cmbEvents.Visible = isShow;
                if (cmbEvents.Items.Count > 0)
                    cmbEvents.SelectedIndex = 0;
                lblEvents.Visible = isShow;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("edit_oc.btnShowHideDetails_Click", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                generateDataGridHeight("binding");                
            }
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the call center data.
        /// </summary>
        /// <param name="objCallCenterInformation">The obj call center information.</param>
        private void updateCallCenterData(CallCenterInformation objCallCenterInformation)
        {
            AgentCallCenter objAgentCallCenter = null;
            grdDevices.EditItemIndex = -1;
            try
            {
                objAgentCallCenter = new AgentCallCenter();

                int itemCount = clstGroupList.Items.Count;
                string groupIDCollection = "";
                if (itemCount > 0)
                {
                    for (int i = 0; i < itemCount; i++)
                    {
                        if (clstGroupList.Items[i].Selected)
                        {
                            groupIDCollection = groupIDCollection + clstGroupList.Items[i].Value + ",";
                        }
                    }
                    if (groupIDCollection.Length <= 0)
                    {
                        groupIDCollection = "-1";
                    }
                    else
                    {
                        groupIDCollection = groupIDCollection.Substring(0, groupIDCollection.Length - 1);
                    }
                }
                objAgentCallCenter.UpdateCallCenterInformation(objCallCenterInformation, groupIDCollection);
                saveDeviceNotifications();
                generateDataGridHeight("Save");
            }
            finally
            {
                objAgentCallCenter = null;
            }
        }

        /// <summary>
        /// Populates the call center info.
        /// </summary>
        private void populateCallCenterInfo()
        {
            AgentCallCenter objAgentCallCenter = null;
            DataTable dtCallCenterInfo = null;
            try
            {
                objAgentCallCenter = new AgentCallCenter();
                dtCallCenterInfo = new DataTable();

                if (ViewState["CallCenterID"] != null)
                {
                    callCenterID = Convert.ToInt32(ViewState["CallCenterID"].ToString());
                    dtCallCenterInfo = objAgentCallCenter.GetCallCenterInformation(callCenterID);

                    if (dtCallCenterInfo.Rows.Count > 0)
                    {
                        DataRow drCallCenterInfo = dtCallCenterInfo.Rows[0];

                        if (drCallCenterInfo["ContactName"] != System.DBNull.Value)
                        {
                            txtContactName.Text = drCallCenterInfo["ContactName"].ToString().Trim();
                        }
                        else
                        {
                            txtContactName.Text = "";
                        }
                        chkActive.Checked = Convert.ToBoolean(drCallCenterInfo["IsActive"].ToString());
                        string contactPhone = Utils.flattenPhoneNumber(drCallCenterInfo["ContactPhone"].ToString().Trim());
                        if (contactPhone.Length >= 10)
                        {
                            txtPrimaryPhone1.Text = contactPhone.Substring(0, 3);
                            txtPrimaryPhone2.Text = contactPhone.Substring(3, 3);
                            txtPrimaryPhone3.Text = contactPhone.Substring(6, 4);
                        }
                        else
                        {
                            txtPrimaryPhone1.Text = "";
                            txtPrimaryPhone2.Text = "";
                            txtPrimaryPhone3.Text = "";
                        }
                        if (drCallCenterInfo["Email"] != System.DBNull.Value)
                            txtEmail.Text = drCallCenterInfo["Email"].ToString().Trim();
                        else
                            txtEmail.Text = "";

                        string fax = Utils.flattenPhoneNumber(drCallCenterInfo["Fax"].ToString().Trim());
                        if (fax.Length >= 10)
                        {
                            txtPrimaryFax1.Text = fax.Substring(0, 3);
                            txtPrimaryFax2.Text = fax.Substring(3, 3);
                            txtPrimaryFax3.Text = fax.Substring(6, 4);
                        }
                        else
                        {
                            txtPrimaryFax1.Text = "";
                            txtPrimaryFax2.Text = "";
                            txtPrimaryFax3.Text = "";

                        }

                        string pagerNumber = Utils.flattenPhoneNumber(drCallCenterInfo["PagerNumber"].ToString().Trim());
                        if (pagerNumber.Length >= 10)
                        {
                            txtPrimaryPager1.Text = pagerNumber.Substring(0, 3);
                            txtPrimaryPager2.Text = pagerNumber.Substring(3, 3);
                            txtPrimaryPager3.Text = pagerNumber.Substring(6, 4);
                        }
                        else
                        {
                            txtPrimaryPager1.Text = "";
                            txtPrimaryPager2.Text = "";
                            txtPrimaryPager3.Text = "";
                        }
                        if (drCallCenterInfo["AlternateContactName"] != System.DBNull.Value)
                            txtAlternateContact.Text = drCallCenterInfo["AlternateContactName"].ToString().Trim();
                        else
                            txtAlternateContact.Text = "";

                        string alternatePhone = Utils.flattenPhoneNumber(drCallCenterInfo["AlternatePhone"].ToString().Trim());
                        if (alternatePhone.Length >= 10)
                        {
                            txtAlternatePhone1.Text = alternatePhone.Substring(0, 3);
                            txtAlternatePhone2.Text = alternatePhone.Substring(3, 3);
                            txtAlternatePhone3.Text = alternatePhone.Substring(6, 4);
                        }
                        else
                        {
                            txtAlternatePhone1.Text = "";
                            txtAlternatePhone2.Text = "";
                            txtAlternatePhone3.Text = "";

                        }

                        //preferences

                        if (drCallCenterInfo["LockedMessageTimeout"] != System.DBNull.Value)
                        {
                            if (Convert.ToInt16(drCallCenterInfo["LockedMessageTimeout"].ToString()) != -1)

                                txtLockedMsgTimeout.Text = drCallCenterInfo["LockedMessageTimeout"].ToString().Trim();
                            else
                                txtLockedMsgTimeout.Text = "";
                        }
                        else
                        {
                            txtLockedMsgTimeout.Text = "";
                        }
                        if (drCallCenterInfo["AutoLogout"] != System.DBNull.Value)
                        {
                            if (Convert.ToInt16(drCallCenterInfo["AutoLogout"].ToString()) != -1)
                                txtAutoLogout.Text = drCallCenterInfo["AutoLogout"].ToString().Trim();
                            else
                                txtAutoLogout.Text = "";
                        }
                        else
                            txtAutoLogout.Text = "";

                        //if (drCallCenterInfo["ConfirmationManuallyClosedPopup"].ToString().Length > 0)
                        //    rlstConformationManuallyClosedPopup.SelectedValue = Convert.ToInt32(drCallCenterInfo["ConfirmationManuallyClosedPopup"]).ToString();
                        //else
                        //    rlstConformationManuallyClosedPopup.SelectedValue = "0";

                        //if (drCallCenterInfo["ConfirmationSendPopup"].ToString().Length > 0)
                        //    rlstConfirmationSenPopup.SelectedValue = Convert.ToInt32(drCallCenterInfo["ConfirmationSendPopup"]).ToString();
                        //else
                        //    rlstConfirmationSenPopup.SelectedValue = "0";

                        //if (drCallCenterInfo["ConfirmationDocPopup"].ToString().Length > 0)
                        //    rlstConformationDocPopup.SelectedValue = Convert.ToInt32(drCallCenterInfo["ConfirmationDocPopup"]).ToString();
                        //else
                        //    rlstConformationDocPopup.SelectedValue = "0";

                        //if (drCallCenterInfo["ConfirmationConnectPopup"].ToString().Length > 0)
                        //    rlstConfirmationConnectPopup.SelectedValue = Convert.ToInt32(drCallCenterInfo["ConfirmationConnectPopup"]).ToString();
                        //else
                        //    rlstConfirmationConnectPopup.SelectedValue = "0";

                    }
                }
            }
            finally
            {
                objAgentCallCenter = null;
            }
        }

        /// <summary>
        /// Populates the groups.
        /// </summary>
        private void populateGroups()
        {
            AgentCallCenter objAgentCallCenter = null;
            DataView dvGroups = null;
            DataTable dtGroups = null;
            DataTable dtAssignedGroups = null;
            try
            {
                objAgentCallCenter = new AgentCallCenter();
                int callCenterID = 0;

                if (ViewState["CallCenterID"] != null)
                {
                    callCenterID = Convert.ToInt32(ViewState["CallCenterID"].ToString());
                }
                lblNoRecords.Visible = false;

                dtGroups = objAgentCallCenter.GetAllGroups(callCenterID);
                dvGroups = dtGroups.DefaultView;
                clstGroupList.DataSource = dtGroups;
                clstGroupList.DataTextField = "GroupName";
                clstGroupList.DataValueField = "GroupID";
                clstGroupList.DataBind();

                dtAssignedGroups = objAgentCallCenter.GetAllAssignedGroups(callCenterID);
                int count = dtAssignedGroups.Rows.Count;

                if (dtGroups.Rows.Count > 0)
                {
                    int itemCount = 0;
                    int[] str = new int[dtAssignedGroups.Rows.Count];
                    foreach (DataRow dr in dtAssignedGroups.Rows)
                    {
                        str[itemCount] = Convert.ToInt32(dr["GroupID"].ToString());
                        itemCount++;
                    }
                    int groupItemCount = clstGroupList.Items.Count;

                    for (int index = 0; index < groupItemCount; index++)
                    {
                        int result = Array.IndexOf(str, Convert.ToInt32(clstGroupList.Items[index].Value));
                        if (result >= 0)
                        {
                            clstGroupList.Items[index].Selected = true;
                        }
                    }
                }
                else
                {
                    lblNoRecords.Visible = true;
                    Div1.Visible = false;
                }
            }
            finally
            {
                objAgentCallCenter = null;
                dtGroups = null;
                dvGroups = null;
                dtAssignedGroups = null;
            }
        }

        /// <summary>
        /// Register JS variables, client side button click events
        /// </summary>
        private void registerJavascriptVariables()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var hdnTextChangedClientID = '" + hdnTextChanged.ClientID + "';");
            sbScript.Append("var txtEmailClientID = '" + txtEmail.ClientID + "';");
            sbScript.Append("var txtAutoLogoutClientID = '" + txtAutoLogout.ClientID + "';");
            sbScript.Append("var txtLockedMsgTimeoutClientID = '" + txtLockedMsgTimeout.ClientID + "';");
            sbScript.Append("var txtPrimaryPhone1ClientID = '" + txtPrimaryPhone1.ClientID + "';");
            sbScript.Append("var txtPrimaryPhone2ClientID = '" + txtPrimaryPhone2.ClientID + "';");
            sbScript.Append("var txtPrimaryPhone3ClientID = '" + txtPrimaryPhone3.ClientID + "';");
            sbScript.Append("var txtPrimaryFax1ClientID = '" + txtPrimaryFax1.ClientID + "';");
            sbScript.Append("var txtPrimaryFax2ClientID = '" + txtPrimaryFax2.ClientID + "';");
            sbScript.Append("var txtPrimaryFax3ClientID = '" + txtPrimaryFax3.ClientID + "';");
            sbScript.Append("var txtPrimaryPager1ClientID = '" + txtPrimaryPager1.ClientID + "';");
            sbScript.Append("var txtPrimaryPager2ClientID = '" + txtPrimaryPager2.ClientID + "';");
            sbScript.Append("var txtPrimaryPager3ClientID = '" + txtPrimaryPager3.ClientID + "';");
            sbScript.Append("var txtAlternatePhone1ClientID = '" + txtAlternatePhone1.ClientID + "';");
            sbScript.Append("var txtAlternatePhone2ClientID = '" + txtAlternatePhone2.ClientID + "';");
            sbScript.Append("var txtAlternatePhone3ClientID = '" + txtAlternatePhone3.ClientID + "';");
            sbScript.Append("var txtContactNameClientID = '" + txtContactName.ClientID + "';");

            sbScript.Append("var txtInitialPauseClientID = '" + txtInitialPause.ClientID + "';");
            sbScript.Append("var hidDeviceLabelClientID = '" + hidDeviceLabel.ClientID + "';");
            sbScript.Append("var hidGatewayLabelClientID = '" + hidGatewayLabel.ClientID + "';");
            sbScript.Append("var hdnGridChangedClientID = '" + hdnGridChanged.ClientID + "';");
            sbScript.Append("var hiddenScrollPos = '" + scrollPos.ClientID + "';");
            sbScript.Append("var hdnIsAddClickedClientID = '" + hdnIsAddClicked.ClientID + "';");

            sbScript.Append("</script>");
            ClientScript.RegisterStartupScript(Page.GetType(), "scriptClientIDs", sbScript.ToString());
            string institutionID;
            if (Request["InstitutionID"] != null) 
              institutionID = Request["InstitutionID"].ToString();
            else
               institutionID = ""; 
                
            string backScript = "Javascript:Navigate('add_callcenter.aspx?InstitutionID=" + institutionID + "');";
           
            btnCancel.Attributes.Add("onclick", backScript);

            txtPrimaryPhone1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryPhone2.ClientID + "').focus();";
            txtPrimaryPhone2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryPhone3.ClientID + "').focus();";
            txtPrimaryPhone1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStrokes();");
            txtPrimaryPhone2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStrokes();");
            txtPrimaryPhone3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStrokes();");

            txtPrimaryFax1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryFax2.ClientID + "').focus();";
            txtPrimaryFax2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryFax3.ClientID + "').focus();";
            txtPrimaryFax1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStrokes();");
            txtPrimaryFax2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStrokes();");
            txtPrimaryFax3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStrokes();");

            txtPrimaryPager1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryPager2.ClientID + "').focus();";
            txtPrimaryPager2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryPager3.ClientID + "').focus();";
            txtPrimaryPager1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStrokes();");
            txtPrimaryPager2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStrokes();");
            txtPrimaryPager3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStrokes();");

            txtAlternatePhone1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtAlternatePhone2.ClientID + "').focus();";
            txtAlternatePhone2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtAlternatePhone3.ClientID + "').focus();";
            txtAlternatePhone1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStrokes();");
            txtAlternatePhone2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStrokes();");
            txtAlternatePhone3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStrokes();");

            txtLockedMsgTimeout.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStrokes();");
            txtAutoLogout.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStrokes();");

            txtPrimaryPhone1.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtPrimaryPhone2.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtPrimaryPhone3.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtPrimaryFax1.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtPrimaryFax2.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtPrimaryFax3.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtPrimaryPager1.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtPrimaryPager2.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtPrimaryPager3.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtAlternatePhone1.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtAlternatePhone2.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtAlternatePhone3.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtLockedMsgTimeout.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtAutoLogout.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtContactName.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtAlternateContact.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtEmail.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            chkActive.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            //rlstConformationManuallyClosedPopup.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            //rlstConfirmationSenPopup.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            //rlstConformationDocPopup.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            //rlstConfirmationConnectPopup.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            btnAdd.Attributes.Add("onclick", "return checkRequired();");

            //Device Notifications
            /* To prevent user to leave page without saving changes.*/
            btnAddDevice.Attributes.Add("onclick", "return validateDevices('" + cmbDeviceType.ClientID + "','" + txtNumAddress.ClientID + "','" + cmbCarrier.ClientID + "','" + txtEmailGateway.ClientID + "','" + txtInitialPause.ClientID + "');");
            txtNumAddress.Attributes.Add("onclick", "RemoveDeviceLabel('" + txtNumAddress.ClientID + "','" + hidDeviceLabel.ClientID + "');");
            txtInitialPause.Attributes.Add("onclick", "RemoveInitialPauseLabel('" + txtInitialPause.ClientID + "','" + hidInitPauseLabel.ClientID + "');");

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
            objAddLink.ID = "lnkAddNew";
            objAddLink.Name = "lnkAddNew";
            objAddLink.CausesValidation = false;
            string script = "javascript:__doPostBack('" + lnkButID + "','');";
            objAddLink.HRef = script;
            objAddLink.Attributes.Add("onclick", "javascript:return AddRecordFromGrid();");

            item.Cells[8].Controls.AddAt(2, objAddLink);
            item.Cells[8].Controls.AddAt(3, new LiteralControl("&nbsp;"));

        }


        #region Device Notifications

        /// <summary>
        /// Fills the agent device.
        /// </summary>
        private void fillAgentDevice()
        {
            AgentCallCenter objAgentCallCenter = null;
            try
            {
                objAgentCallCenter = new AgentCallCenter();
                DataTable dtAgentDevice = objAgentCallCenter.GetAgentDevices();
                DataRow dr = dtAgentDevice.NewRow();
                dr[0] = "-1";
                dr[1] = "-- Select Device To Add --";
                dtAgentDevice.Rows.InsertAt(dr, dtAgentDevice.Rows.Count);
                cmbDeviceType.DataSource = dtAgentDevice.DefaultView;
                cmbDeviceType.DataBind();
                cmbDeviceType.Items.FindByValue("-1").Selected = true;
                dtAgentDevice = null;
                cmbDeviceType.SelectedValue = "-1";
            }
            finally
            {
                objAgentCallCenter = null;
            }
        }

        /// <summary>
        /// Fills the agent event.
        /// </summary>
        private void fillAgentEvent(DropDownList objDropdown)
        {
            AgentCallCenter objAgentCallCenter = null;
            DataTable dtNotifyEvents = null;
            try
            {
                objAgentCallCenter = new AgentCallCenter();
                dtNotifyEvents = objAgentCallCenter.GetAgentNotifyEvents();
                objDropdown.DataSource = dtNotifyEvents.DefaultView;
                objDropdown.DataBind();
            }
            finally
            {
                dtNotifyEvents = null;
                objAgentCallCenter = null;

                if (!objDropdown.ID.ToUpper().Equals("CMBEVENTS"))
                {
                    ListItem objLi1 = new ListItem();
                    objLi1.Text = "-- None --";
                    objLi1.Value = "0";
                    objDropdown.Items.Insert(objDropdown.Items.Count, objLi1);
                }  

            }
        }

        /// <summary>
        /// Binds the devices for agent.
        /// </summary>
        private void bindDevicesForAgent()
        {

            DataView dvADevices = new DataView(dtGridDeviceNotifications);
            //Filter dataview with only Unchanged, Changed devices
            dvADevices.RowFilter = "FlagModified in ('Unchanged', 'Changed')"; //Filter Deleted Rows
            dvADevices.Sort = "EventDescription, DeviceName, GroupName,FindingDescription ASC";
            grdDevices.DataSource = dvADevices;
            grdDevices.DataBind();
            generateDataGridHeight("dataBind");

            if (dvADevices.Count > 0)
                lblNoRecordsStep1.Visible = false;
            else
                lblNoRecordsStep1.Visible = true;
        }

        /// <summary>
        /// Fills the devices.
        /// </summary>
        private void fillDevices()
        {
            AgentCallCenter objAgentCallCenter = null;
            DataTable dtDevice = null;
            try
            {
                objAgentCallCenter = new AgentCallCenter();
                dtDevice = objAgentCallCenter.GetLiveAgentDevice(callCenterID);

                dtGridDeviceNotifications = dtDevice.Copy();
         
                dtGridDeviceNotifications.Columns["DeviceName"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["DeviceAddress"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["Gateway"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["Carrier"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["GroupName"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["EventDescription"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["FindingDescription"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["InitialPause"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["DeviceID"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["GroupID"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["AgentNotificationID"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["AgentNotifyEventID"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["FindingID"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["RowID"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["CallCenterID"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["FlagRowType"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["FlagModified"].ReadOnly = false;

                Session[SessionConstants.DT_NOTIFICATION] = dtGridDeviceNotifications;
                rowNo = dtGridDeviceNotifications.Rows.Count;
                bindDevicesForAgent();
            }
            finally
            {
                objAgentCallCenter = null;
                dtDevice = null;
            }
        }

        /// <summary>
        /// Gets the carriers.
        /// </summary>
        private void getCarriers()
        {
            DataSet dsCellCarrier = null;
            DataSet dsPagerCarrier = null;
            try
            {
                OrderingClinician objOC = new OrderingClinician();
                dsCellCarrier = objOC.GetCellPhoneCarriers();
                if (dsCellCarrier != null)
                {
                    Session["CellPhoneCarriers"] = dsCellCarrier;
                }

                dsPagerCarrier = objOC.GetPagerCarriers();
                if (dsPagerCarrier != null)
                {
                    Session["PagerCarriers"] = dsPagerCarrier;
                }
                objOC = null;
            }
            finally
            {
                if (dsCellCarrier != null)
                {
                    dsCellCarrier.Dispose();
                    dsCellCarrier = null;
                }
                if (dsPagerCarrier != null)
                {
                    dsPagerCarrier.Dispose();
                    dsPagerCarrier = null;
                }
            }
        }

        /// <summary>
        /// Adds the agent notification devices.
        /// </summary>
        private void addAgentNotificationDevices()
        {
            try
            {
                if (validateDevices())
                {
                    //Set DeviceAddress property value            
                    string deviceAdd = txtNumAddress.Text.Trim();
                    string gateway = "";
                    string carrier = "";
                    decimal initialPause = 0;
                    string script = "";
                    if (cmbDeviceType.Items.Count > 0 && cmbDeviceType.SelectedItem.Text.Equals("Email"))
                    {
                        deviceAdd = txtEmailGateway.Text.Trim();
                    }

                    //Set Gateway property value            
                    if (int.Parse(cmbDeviceType.SelectedValue) == (int)NotificationDevice.PagerTAP || int.Parse(cmbDeviceType.SelectedValue) == (int)NotificationDevice.PagerTAPA) //TAP DEVICE
                    {
                        gateway = txtEmailGateway.Text.Trim();
                    }
                    else if ((!(cmbDeviceType.Items.Count > 0 && cmbDeviceType.SelectedItem.Text.Equals("Email"))) && txtEmailGateway.Visible == true && txtEmailGateway.Text.Trim().Length > 0)
                    {
                        gateway = txtEmailGateway.Text.Trim();
                    }
                    else
                    {
                        gateway = "";
                    }

                    //Set Carrier property value            
                    if (cmbCarrier.Visible && cmbCarrier.Items.Count > 0)
                    {
                        carrier = cmbCarrier.SelectedItem.Text;
                    }
                    else
                    {
                        carrier = "";
                    }

                    //Set InitialPauseTime property value
                    if (txtInitialPause.Visible == true)
                    {
                        initialPause = Convert.ToDecimal(txtInitialPause.Text.Trim());
                    }
                    else
                    {
                        initialPause = 0;
                    }

                    rowNo++;
                    string flagModified = "UnChanged";
                    string flagRowType = "New";
                    DataRow dtrow = dtGridDeviceNotifications.NewRow();
                    dtrow["DeviceName"] = generateDeviceName((Convert.ToInt32(cmbDeviceType.SelectedValue)));
                    dtrow["DeviceAddress"] = deviceAdd;
                    dtrow["Gateway"] = gateway;
                    dtrow["Carrier"] = carrier;
                    dtrow["GroupName"] = "";
                    dtrow["EventDescription"] = (cmbEvents.Visible)? cmbEvents.SelectedItem.Text : "";
                    dtrow["FindingDescription"] = "";
                    dtrow["InitialPause"] = initialPause;
                    dtrow["DeviceID"] = Convert.ToInt32(cmbDeviceType.SelectedValue);
                    dtrow["GroupID"] = -1;
                    dtrow["AgentNotificationID"] = 0;
                    dtrow["AgentNotifyEventID"] = (cmbEvents.Visible) ? Convert.ToInt32(cmbEvents.SelectedValue) : 0;
                    dtrow["FindingID"] = -1;
                    dtrow["RowID"] = rowNo;
                    dtrow["FlagRowType"] = flagRowType;
                    dtrow["FlagModified"] = flagModified;
                    dtrow["CallCenterID"] = callCenterID;

                    script = "alert('Device has been added')";
                    dtGridDeviceNotifications.Rows.Add(dtrow);

                    Session[SessionConstants.DT_NOTIFICATION] = dtGridDeviceNotifications;
                    bindDevicesForAgent();
                    resetControls();

                    ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "Alert", script, true);
                }
                else
                {
                    generateDataGridHeight("Validate");
                }

            }
            finally
            {
            }
        }

        /// <summary>
        /// Generates the name of the device.
        /// </summary>
        /// <returns></returns>
        private string generateDeviceName(int deviceType)
        {
            string deviceShortName = "";
            if (deviceType != -1)
            {
                OrderingClinician objOC = new OrderingClinician();
                int deviceID = deviceType;

                deviceShortName = objOC.GetDeviceShortDescription(deviceID);
                string expression = "DeviceName like '" + deviceShortName + "%'";

                DataRow[] existingDevices = dtGridDeviceNotifications.Select(expression);
                int count = existingDevices.GetLength(0);
                count++;

                while (true)
                {
                    DataRow[] newrow = dtGridDeviceNotifications.Select("DeviceName = '" + deviceShortName + "_" + count.ToString() + "'");

                    if (newrow.GetLength(0) == 0)
                        break;
                    else
                        count++;
                }

                deviceShortName += "_" + count.ToString();

                return deviceShortName;

            }
            return deviceShortName;
        }

        /// <summary>
        /// Edits the device grid.
        /// </summary>
        /// <param name="intGridEditItemIndex">Index of the int grid edit item.</param>
        /// <param name="strEventText">The STR event text.</param>
        /// <param name="findingID">The finding ID.</param>
        /// <param name="deviceName">Name of the device.</param>
        /// <param name="deviceGatway">The device gatway.</param>
        /// <param name="groupID">The group ID.</param>
        private void editDeviceGrid(int intGridEditItemIndex, string strEventID, string findingID, string deviceName, string deviceGatway, int groupID)
        {
            try
            {
                bindDevicesForAgent();

                TextBox tbDevice = ((TextBox)(grdDevices.Items[intGridEditItemIndex].Cells[1].FindControl("txtGridDeviceNumber")));
                TextBox tbEmailGateway = ((TextBox)(grdDevices.Items[intGridEditItemIndex].Cells[4].FindControl("txtGridEmailGateway")));
                DropDownList dlEvent = ((DropDownList)(grdDevices.Items[intGridEditItemIndex].Cells[5].FindControl("dlistGridEvents")));
                DropDownList dlFinding = ((DropDownList)(grdDevices.Items[intGridEditItemIndex].Cells[6].FindControl("dlistGridFindings")));
                TextBox gridInitialPauseTxt = ((TextBox)(grdDevices.Items[intGridEditItemIndex].Cells[7].FindControl("txtGridInitialPause")));

                int deviceType = int.Parse(grdDevices.Items[intGridEditItemIndex].Cells[10].Text.Trim());

                ViewState[Constants.DEVICE_ADDRESS] = tbDevice.Text.Trim();
                ViewState[Constants.EMAIL_GATEWAY] = tbEmailGateway.Text.Trim();

                if (deviceType == (int)NotificationDevice.PagerTAP || deviceType == (int)NotificationDevice.PagerTAPA)
                {
                    tbDevice.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
                    //tbDevice.MaxLength = 6;
                    tbEmailGateway.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
                    //tbEmailGateway.MaxLength = 10;
                }
                else if (deviceType == (int)NotificationDevice.PagerNumSkyTel || deviceType == (int)NotificationDevice.PagerNumUSA || deviceType == (int)NotificationDevice.PagerNumRegular || deviceType == (int)NotificationDevice.pagerPartner)
                {
                    tbDevice.Attributes.Add("onkeyPress", "JavaScript:return PagerValidationWithSpace('" + tbDevice.ClientID + "');");
                    //tbDevice.MaxLength = 100;
                }
                else if (deviceType == (int)NotificationDevice.PagerAlpha)
                {
                    tbDevice.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes(hdnTextChangedClientID);");
                    //tbDevice.MaxLength = 100;
                }
                else if (deviceType != (int)NotificationDevice.EMail)
                {
                    tbDevice.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes(hdnTextChangedClientID);");
                    tbDevice.MaxLength = 10;
                }
                else
                {
                    tbDevice.Attributes.Add("onkeyPress", "return true");
                    tbDevice.Attributes.Add("onchange", "return true");
                    tbDevice.MaxLength = 100;
                }

                if (deviceType == NotificationDevice.PagerTAP.GetHashCode() || deviceType == NotificationDevice.PagerTAPA.GetHashCode())
                {
                    txtEmailGateway.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes(hdnTextChangedClientID);");
                    //txtEmailGateway.MaxLength = 10;
                }
                else
                {
                    txtEmailGateway.Attributes.Add("onkeypress", "JavaScript:return true;");
                    txtEmailGateway.MaxLength = 100;
                }

                fillAgentEvent(dlEvent);

                if (deviceName.Length == 0)
                {
                    tbDevice.Visible = false;
                    tbEmailGateway.Visible = false;
                }
                else if (deviceGatway.Length == 0)
                {
                    tbEmailGateway.Visible = false;
                }

                if (gridInitialPauseTxt.Text.Trim().Length <= 0)
                {
                    gridInitialPauseTxt.Visible = false;
                }
                else
                {
                    gridInitialPauseTxt.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStrokeOrDecimalpoint();");
                    gridInitialPauseTxt.Attributes.Add("onchange", "JavaScript:return isNumericKeyStrokeOrDecimalpoint();");
                }

                dlEvent.SelectedValue = strEventID;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("edit_rp.editDeviceGrid:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
        }

        /// <summary>
        /// Validates the devices.
        /// </summary>
        /// <returns></returns>
        private bool validateDevices()
        {
            string focus = "";
            if (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerAlpha || (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerNumRegular) ||
                    int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerNumSkyTel || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerNumUSA || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.pagerPartner || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerTAP
                     || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerTAPA)
            {

                if ((Utils.RegExNumericMatch(txtNumAddress.Text.Trim())) == false)
                {
                    //"Alphanumeric characters in pager number"
                    StringBuilder acRegScript = new StringBuilder();
                    acRegScript.Append("<script type=\"text/javascript\">");
                    acRegScript.AppendFormat("document.getElementById(" + '"' + OCDeviceDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                    acRegScript.AppendFormat("alert('Please enter valid pager number');");
                    acRegScript.Append("</script>");
                    ScriptManager.RegisterClientScriptBlock(upnlStep1, upnlStep1.GetType(), "Register", acRegScript.ToString(), false);
                    generateDataGridHeight("invalidDevice");                    
                    return false;
                }

                if (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerTAP || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerTAPA)
                {
                    if ((Utils.RegExNumericMatch(txtEmailGateway.Text.Trim())) == false)
                    {
                        //"Alphanumeric characters in pager number"
                        StringBuilder acRegScript = new StringBuilder();
                        acRegScript.Append("<script type=\"text/javascript\">");
                        acRegScript.AppendFormat("document.getElementById(" + '"' + OCDeviceDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                        acRegScript.AppendFormat("alert('Please enter valid pager number');");
                        acRegScript.Append("</script>");
                        ScriptManager.RegisterClientScriptBlock(upnlStep1, upnlStep1.GetType(), "Register", acRegScript.ToString(), false);
                        generateDataGridHeight("invalidDevice");
                        return false;
                    }
                }
            }

            if (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.EMail)
            {
                if ((Utils.RegExMatch(txtEmailGateway.Text.Trim())) == false)
                {
                    //"Email format incorrect"
                    StringBuilder acRegScript = new StringBuilder();
                    acRegScript.Append("<script type=\"text/javascript\">");
                    acRegScript.AppendFormat("document.getElementById(" + '"' + OCDeviceDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                    acRegScript.AppendFormat("alert('Please enter valid Email Address');");
                    acRegScript.Append("document.getElementById('" + txtEmailGateway.ClientID + "').focus();");
                    acRegScript.Append("</script>");
                    generateDataGridHeight("invalidDevice");
                    ScriptManager.RegisterClientScriptBlock(upnlStep1, upnlStep1.GetType(), "Register", acRegScript.ToString(), false);
                    return false;
                }

            }
            else if (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerTAP || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerTAPA)
            {
                string pin = txtNumAddress.Text.Trim();
                string tap800num = txtEmailGateway.Text.Trim();
                string error = "";
                string taptext = "Enter TAP 800 number (numbers only)";
                string pintext = "Enter PIN number (numbers only)";
                //if (!Utils.isNumericValue(pin.Trim()))
                if ((pin.Trim() == "") || (pin.Trim() == pintext.Trim()))
                {
                    error = "Please enter PIN Number." + @"\n";
                    focus = txtNumAddress.ClientID;
                }
                //if (!Utils.isNumericValue(tap800num.Trim()))
                if ((tap800num.Trim() == "") || (tap800num.Trim() == taptext.Trim()))
                {
                    error += "Please enter TAP 800 Number.";
                    if (focus == "")
                        focus = txtEmailGateway.ClientID;
                }

                if (error.Length > 0)
                {
                    StringBuilder acRegScript = new StringBuilder();
                    acRegScript.Append("<script type=\"text/javascript\">");
                    acRegScript.AppendFormat("document.getElementById(" + '"' + OCDeviceDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                    //acRegScript.AppendFormat("alert('Please enter valid Email Address');");
                    acRegScript.AppendFormat("alert('" + error + "');");
                    acRegScript.Append("document.getElementById('" + focus + "').focus();");
                    acRegScript.Append("</script>");
                    ScriptManager.RegisterClientScriptBlock(upnlStep1, upnlStep1.GetType(), "Register1", acRegScript.ToString(), false);
                    generateDataGridHeight("validatedevice1");
                    return false;
                }
            }
            else
            {
                string deviceAdd = txtNumAddress.Text.Trim();
                if (int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.EMail)
                {
                    string error = "";
                    if (int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.PagerAlpha && int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.PagerNumRegular && int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.PagerNumSkyTel && int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.PagerNumUSA && int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.pagerPartner)
                    {
                        if (!Utils.isNumericValue(deviceAdd))
                        {
                            error = "Please enter valid Number\\n";
                            focus = txtNumAddress.ClientID;
                        }
                    }

                    if (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.Fax || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.SMS || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.SMS_WebLink)
                    {
                        if (deviceAdd.Length != 10)
                        {
                            error += "Please enter valid Number\\n";
                            focus = txtNumAddress.ClientID;
                        }
                    }

                    if (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerAlpha || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.SMS)
                    {
                        if (txtEmailGateway.Text.Trim().Length == 0)
                        {
                            error += "Please enter Email Gateway Address\\n";
                            if (focus == "")
                                focus = txtEmailGateway.ClientID;
                        }
                        else if ((Utils.RegExMatch(txtEmailGateway.Text.Trim())) == false)
                        {
                            error += "Please enter valid Email Address\\n";
                            focus = txtEmailGateway.ClientID;
                        }
                    }

                    if (error.Length > 0)
                    {
                        StringBuilder acRegScript = new StringBuilder();
                        acRegScript.Append("<script type=\"text/javascript\">");
                        acRegScript.AppendFormat("document.getElementById(" + '"' + OCDeviceDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                        acRegScript.AppendFormat("alert('" + error + "');");
                        acRegScript.Append("document.getElementById('" + focus + "').focus();");
                        acRegScript.Append("</script>");
                        ScriptManager.RegisterClientScriptBlock(upnlStep1, upnlStep1.GetType(), "Register", acRegScript.ToString(), false);
                        generateDataGridHeight("validatedevice2");
                        return false;
                    }

                }
            }
            return true;
        }

        /// <summary>
        /// Gets the height of the data grid.
        /// </summary>
        /// <returns></returns>
        private int getDataGridHeight()
        {
            int devicesGridHeight = 20;
            int gridItemHeight = 25;
            int gridHeaderHeight = 26;
            int maxRows = 4;

            if (grdDevices.Items.Count < maxRows)
            {
                if (grdDevices.Items.Count <= 1)
                    devicesGridHeight = (grdDevices.Items.Count * gridItemHeight) + gridHeaderHeight + 10;
                else
                    devicesGridHeight = (grdDevices.Items.Count * gridItemHeight) + gridHeaderHeight;
            }
            else
            {
                devicesGridHeight = (maxRows * gridItemHeight) + gridHeaderHeight;
            }

            return devicesGridHeight + 1;
        }

        /// <summary>
        /// Generates the gateway address.
        /// </summary>
        private void generateGatewayAddress()
        {
            try
            {
                int deviceID = Convert.ToInt32(cmbDeviceType.SelectedItem.Value);
                DataSet carriers;
                DataRow[] rows;
                IEnumerator e;
                switch (deviceID)
                {
                    case (int)NotificationDevice.SMS: // Cell Phones
                        txtEmailGateway.Text = txtNumAddress.Text.Trim() + "@";
                        carriers = (DataSet)Session["CellPhoneCarriers"];
                        rows = carriers.Tables[0].Select("CarrierID='" + cmbCarrier.SelectedItem.Value+"'"); // should have 1
                        e = rows.GetEnumerator();
                        if (e.MoveNext())
                        {
                            DataRow row = (DataRow)e.Current;
                            txtEmailGateway.Text += row["CarrierEmail"];
                        }
                        break;
                    case (int)NotificationDevice.PagerAlpha: // Pagers
                        carriers = (DataSet)Session["PagerCarriers"];
                        rows = carriers.Tables[0].Select("CarrierID='" + cmbCarrier.SelectedItem.Value+"'"); // should have 1
                        e = rows.GetEnumerator();
                        if (e.MoveNext())
                        {
                            DataRow row = (DataRow)e.Current;
                            txtEmailGateway.Text = txtNumAddress.Text.Trim() + "@" + row["Email"].ToString();
                        }
                        break;
                    default:
                        txtEmailGateway.Text = "";
                        break;
                }

            }
            finally
            {
            }
        }

        /// <summary>
        /// Sets the labels and input boxes.
        /// </summary>
        /// <param name="deviceID">The device ID.</param>
        private void setLabelsAndInputBoxes(int deviceID)
        {
            try
            {
                ListItem li = new ListItem("-- Select Carrier", "-1");
                DataSet ds = new DataSet();
                DataView dv = new DataView();

                cmbEvents.Visible = false;
                lblEvents.Visible = false;
                if (cmbEvents.Items.Count > 0)
                    cmbEvents.SelectedIndex = 0;
                btnShowHideDetails.Text = SHOWDETAILS_BUTTONNAME;
                btnShowHideDetails.Visible = true;


                switch (deviceID)
                {
                    case (int)NotificationDevice.SelectAll:
                        resetControls();
                        break;

                    case (int)NotificationDevice.EMail:  // Email
                        txtEmailGateway.Text = "Enter Email Address";
                        txtNumAddress.Visible = false;
                        txtNumAddress.Text = "";
                        lblNumAddress.Visible = false;
                        txtEmailGateway.Width = Unit.Pixel(250);
                        txtEmailGateway.AutoPostBack = false;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtEmailGateway.Visible = true;                       
                        lblEmailGateway.Visible = true;
                        lblEmailGateway.Text = "Number / Address";
                        btnAddDevice.Visible = true;                        
                        txtInitialPause.Visible = false;
                        txtInitialPause.Text = "";
                        lblInitialPause.Visible = false;
                        txtEmailGateway.Attributes.Add("onclick", "RemoveGatewayLabel('" + txtEmailGateway.ClientID + "','" + hidGatewayLabel.ClientID + "');");
                        break;

                    case (int)NotificationDevice.SMS:  // SMS/Cell
                        txtNumAddress.Text = "Enter Cell # (numbers only)";
                        txtNumAddress.Visible = true;
                        lblNumAddress.Visible = true;
                        txtNumAddress.Width = Unit.Pixel(150);
                        cmbCarrier.Visible = true;
                        lblCarrier.Visible = true;
                        txtEmailGateway.Visible = true;
                        txtEmailGateway.Text = "Enter Email Gateway";
                        txtEmailGateway.Width = Unit.Pixel(120);
                        lblEmailGateway.Visible = true;
                        lblEmailGateway.Text = "Email Gateway";
                        btnAddDevice.Visible = true;
                        ds = (DataSet)Session["CellPhoneCarriers"];
                        dv = ds.Tables[0].DefaultView;
                        cmbCarrier.DataTextField = "CarrierDescription";
                        cmbCarrier.DataValueField = "CarrierID";
                        cmbCarrier.DataSource = dv;
                        cmbCarrier.DataBind();
                        cmbCarrier.Items.Add(li);
                        cmbCarrier.SelectedValue = "-1";                        
                        txtInitialPause.Visible = false;
                        txtInitialPause.Text = "";
                        lblInitialPause.Visible = false;
                        break;

                    case (int)NotificationDevice.PagerAlpha: // Pager - Alpha
                        txtNumAddress.Text = "Enter Pager # (numbers only)";
                        txtNumAddress.Visible = true;
                        lblNumAddress.Visible = true;
                        txtNumAddress.Width = Unit.Pixel(140);
                        cmbCarrier.Visible = true;
                        lblCarrier.Visible = true;
                        txtEmailGateway.Visible = true;
                        lblEmailGateway.Visible = true;
                        lblEmailGateway.Text = "Email Gateway";
                        txtEmailGateway.Text = "Enter Email Gateway";
                        txtEmailGateway.Width = Unit.Pixel(120);
                        btnAddDevice.Visible = true;
                        ds = (DataSet)Session["PagerCarriers"];
                        dv = ds.Tables[0].DefaultView;
                        cmbCarrier.DataTextField = "CarrierDescription";
                        cmbCarrier.DataValueField = "CarrierID";
                        cmbCarrier.DataSource = dv;
                        cmbCarrier.DataBind();
                        cmbCarrier.Items.Add(li);
                        cmbCarrier.SelectedValue = "-1";
                        txtInitialPause.Visible = false;
                        lblInitialPause.Visible = false;
                        break;

                    case (int)NotificationDevice.PagerNumRegular:
                    case (int)NotificationDevice.PagerNumSkyTel:
                    case (int)NotificationDevice.PagerNumUSA:
                        txtNumAddress.Text = "Enter Pager # + PIN (numbers only)";
                        txtNumAddress.Width = Unit.Pixel(250);
                        txtNumAddress.Visible = true;
                        txtNumAddress.AutoPostBack = false;
                        lblNumAddress.Visible = true;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtEmailGateway.Visible = false;
                        txtEmailGateway.Text = "";
                        lblEmailGateway.Visible = false;
                        btnAddDevice.Visible = true;
                        txtInitialPause.Visible = false;
                        txtInitialPause.Text = "";
                        lblInitialPause.Visible = false;
                        break;

                    case (int)NotificationDevice.Fax:  // Fax
                        txtNumAddress.Text = "Enter Fax # (numbers only)";
                        txtNumAddress.Visible = true;
                        txtNumAddress.Width = Unit.Pixel(175);
                        txtNumAddress.AutoPostBack = false;
                        lblNumAddress.Visible = true;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtEmailGateway.Visible = false;
                        txtEmailGateway.Text = "";
                        lblEmailGateway.Visible = false;
                        btnAddDevice.Visible = true;                        
                        txtInitialPause.Visible = false;
                        txtInitialPause.Text = "";
                        lblInitialPause.Visible = false;
                        break;

                    case (int)NotificationDevice.pagerPartner:
                        txtNumAddress.Text = "Enter Pager # + PIN (numbers only)";
                        txtNumAddress.Width = Unit.Pixel(250);
                        txtNumAddress.Visible = true;
                        txtNumAddress.AutoPostBack = false;
                        lblNumAddress.Visible = true;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtEmailGateway.Visible = false;
                        txtEmailGateway.Text = "";
                        lblEmailGateway.Visible = false;
                        btnAddDevice.Visible = true;                        
                        txtInitialPause.Visible = false;
                        txtInitialPause.Text = "";
                        lblInitialPause.Visible = false;
                        break;

                    case (int)NotificationDevice.OutboundCallCB:  // Outbound Phone Call with Callback Option
                    case (int)NotificationDevice.OutboundCallRS:  // Outbound Phone Call with Lab Result
                    case (int)NotificationDevice.OutboundCallCI:  // Outbound Phone Call with Callback Instructions
                        txtNumAddress.Text = "Enter Outbound Phone Call number (numbers only)";
                        txtNumAddress.MaxLength = 10;
                        txtNumAddress.Visible = true;
                        txtNumAddress.Width = Unit.Pixel(175);
                        txtNumAddress.AutoPostBack = false;
                        lblNumAddress.Visible = true;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtEmailGateway.Visible = false;
                        txtEmailGateway.Text = "";
                        lblEmailGateway.Visible = false;
                        btnAddDevice.Visible = true;                        
                        txtInitialPause.Visible = false;
                        txtInitialPause.Text = "";
                        lblInitialPause.Visible = false;
                        break;
                    case (int)NotificationDevice.OutboundCallAS:  // Outbound Phone Call for Answering Service
                        txtNumAddress.Text = "Enter Outbound Phone Call number (numbers only)";
                        txtNumAddress.MaxLength = 10;
                        txtNumAddress.Visible = true;
                        txtNumAddress.Width = Unit.Pixel(175);
                        txtNumAddress.AutoPostBack = false;
                        lblNumAddress.Visible = true;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtEmailGateway.Visible = false;
                        txtEmailGateway.Text = "";
                        lblEmailGateway.Visible = false;
                        btnAddDevice.Visible = true;                        
                        lblInitialPause.Visible = true;
                        txtInitialPause.Visible = true;
                        txtInitialPause.Text = "Value between 1 to 30.99";
                        //txtInitialPause.Width = Unit.Pixel(185);
                        break;
                    case (int)NotificationDevice.PagerTAP:  // Pager TAP device
                    case (int)NotificationDevice.PagerTAPA:  // Pager TAP device
                        txtNumAddress.Text = "Enter PIN number (numbers only)";
                        //txtNumAddress.MaxLength = 6;
                        txtNumAddress.Visible = true;
                        txtNumAddress.Width = Unit.Pixel(175);
                        txtNumAddress.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes(hdnTextChangedClientID);");
                        txtNumAddress.AutoPostBack = false;
                        lblNumAddress.Visible = true;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtEmailGateway.Text = "Enter TAP 800 number (numbers only)";
                        //txtEmailGateway.MaxLength = 10;
                        txtEmailGateway.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes(hdnTextChangedClientID);");
                        txtEmailGateway.Attributes.Add("onclick", "RemoveGatewayLabel('" + txtEmailGateway.ClientID + "','" + hidGatewayLabel.ClientID + "');");
                        txtEmailGateway.Visible = true;
                        lblEmailGateway.Visible = true;
                        lblEmailGateway.Text = "Email Gateway";
                        btnAddDevice.Visible = true;                        
                        txtInitialPause.Visible = false;
                        txtInitialPause.Text = "";
                        lblInitialPause.Visible = false;
                        break;
                    default:
                        txtNumAddress.Visible = false;
                        lblNumAddress.Visible = false;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtEmailGateway.Visible = false;
                        txtEmailGateway.Text = "";
                        lblEmailGateway.Visible = false;
                        cmbEvents.Visible = false;
                        txtInitialPause.Visible = false;
                        txtInitialPause.Text = "";
                        lblEvents.Visible = false;
                        btnAddDevice.Visible = false;
                        lblInitialPause.Visible = false;
                        break;
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// Resets the controls.
        /// </summary>
        private void resetControls()
        {
            if (cmbDeviceType.Items.Count > 0)
                cmbDeviceType.SelectedValue = "-1";

            txtNumAddress.Text = string.Empty;
            txtNumAddress.Visible = false;
            lblNumAddress.Visible = false;

            txtEmailGateway.Text = string.Empty;
            txtEmailGateway.Visible = false;
            lblEmailGateway.Visible = false;

            cmbCarrier.Visible = false;
            cmbCarrier.SelectedValue = "-1";
            lblCarrier.Visible = false;

            cmbEvents.Visible = false;
            if (cmbEvents.Items.Count > 0)
                cmbEvents.SelectedIndex = 0;
            lblEvents.Visible = false;

            txtInitialPause.Visible = false;
            lblInitialPause.Visible = false;
            btnAddDevice.Visible = false;
            btnShowHideDetails.Visible = false;
        }

        // <summary>
        /// This function is to set dynamic height of data grid
        /// </summary>
        private void generateDataGridHeight(string key)
        {
            int nDevicesGridHeight = 20;
            int gridItemHeight = 25;
            int gridHeaderHeight = 26;
            int maxRows = 4;

            if (grdDevices.Items.Count < maxRows)
            {
                if (grdDevices.Items.Count <= 1)
                    nDevicesGridHeight = (grdDevices.Items.Count * gridItemHeight) + gridHeaderHeight + 10;
                else
                    nDevicesGridHeight = (grdDevices.Items.Count * gridItemHeight) + gridHeaderHeight;
            }
            else
            {
                nDevicesGridHeight = (maxRows * gridItemHeight) + gridHeaderHeight;
            }

            string script = "<script type=\"text/javascript\">";
            script += "document.getElementById(" + '"' + OCDeviceDiv.ClientID + '"' + ").style.height='" + (nDevicesGridHeight + 1) + "';";
            script += "document.getElementById(" + '"' + OCDeviceDiv.ClientID + '"' + ").scrollTop=document.getElementById('" + scrollPos.ClientID + "').value;</script>";
            ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), key, script, false);

        }

        /// <summary>
        /// Saves the device notifications.
        /// </summary>
        private void saveDeviceNotifications()
        {
            AgentCallCenter objAgentCallCenter = null;
            if (dtGridDeviceNotifications.Rows.Count > 0)
            {
                try
                {
                    objAgentCallCenter = new AgentCallCenter();
                    int numOfDevices = dtGridDeviceNotifications.Rows.Count;
                    for (int countDevices = 0; countDevices < numOfDevices; countDevices++)
                    {
                        LiveAgentDevicesInfo objDevice = new LiveAgentDevicesInfo();
                        int deviceID = 0;
                        string flageRowType;
                        string flageModified;
                        objDevice.CallCenterID = callCenterID;
                        objDevice.DeviceID = dtGridDeviceNotifications.Rows[countDevices]["DeviceID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["DeviceID"]);
                        objDevice.DeviceAddress = dtGridDeviceNotifications.Rows[countDevices]["DeviceAddress"] == DBNull.Value ? "" : dtGridDeviceNotifications.Rows[countDevices]["DeviceAddress"].ToString();
                        objDevice.DeviceName = dtGridDeviceNotifications.Rows[countDevices]["DeviceName"] == DBNull.Value ? "" : dtGridDeviceNotifications.Rows[countDevices]["DeviceName"].ToString();
                        objDevice.Gateway = dtGridDeviceNotifications.Rows[countDevices]["Gateway"] == DBNull.Value ? "" : dtGridDeviceNotifications.Rows[countDevices]["Gateway"].ToString();
                        objDevice.Carrier = dtGridDeviceNotifications.Rows[countDevices]["Carrier"] == DBNull.Value ? "" : dtGridDeviceNotifications.Rows[countDevices]["Carrier"].ToString();
                        objDevice.InitialPauseTime = dtGridDeviceNotifications.Rows[countDevices]["InitialPause"] == DBNull.Value ? "-1" : dtGridDeviceNotifications.Rows[countDevices]["InitialPause"].ToString();
                        objDevice.GroupID = dtGridDeviceNotifications.Rows[countDevices]["GroupID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["GroupID"]);
                        objDevice.FindingID = dtGridDeviceNotifications.Rows[countDevices]["FindingID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["FindingID"]);
                        objDevice.AgentNotifyEventID = dtGridDeviceNotifications.Rows[countDevices]["AgentNotifyEventID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["AgentNotifyEventID"]);
                        objDevice.AgentDeviceID = dtGridDeviceNotifications.Rows[countDevices]["AgentDeviceID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["AgentDeviceID"]);
                        objDevice.AgentNotificationID = dtGridDeviceNotifications.Rows[countDevices]["AgentNotificationID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["AgentNotificationID"]);

                        if (objDevice.InitialPauseTime == "0")
                            objDevice.InitialPauseTime = "-1";

                        flageRowType = dtGridDeviceNotifications.Rows[countDevices]["FlagRowType"].ToString();
                        flageModified = dtGridDeviceNotifications.Rows[countDevices]["FlagModified"].ToString();

                        if (flageRowType == "New") // New Device added
                        {
                            if (flageModified != "Deleted")
                            {
                                deviceID = objAgentCallCenter.InsertUpdateAgentDevice(objDevice, 0);// get new OCdeviceID which is added recently.
                            }
                        }
                        else // If Existing Devices
                        {

                            if (flageModified == "Deleted") // If device is deleted.
                            {
                                objAgentCallCenter.InsertUpdateAgentDevice(objDevice, 2);
                            }
                            else if (flageModified == "Changed") // if device is modified
                            {
                                deviceID = objAgentCallCenter.InsertUpdateAgentDevice(objDevice, 1); //update deviece & event.
                            }
                        }
                        objDevice = null;
                    }
                }
                finally
                {
                    dtGridDeviceNotifications.Rows.Clear();
                    fillDevices();
                    objAgentCallCenter = null;
                }
            }
        }

        /// <summary>
        /// Validates the record.
        /// </summary>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        private bool validateRecord(DataGridCommandEventArgs e)
        {
            bool returnVal = true;
            string errorMessage = "";
            int deviceType = int.Parse(grdDevices.Items[e.Item.ItemIndex].Cells[10].Text);
            TextBox gridDeviceTypeTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[0].FindControl("txtGridDeviceType")));
            TextBox gridDeviceNumberTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[1].FindControl("txtGridDeviceNumber")));
            TextBox gridEmailGatewayTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[4].FindControl("txtGridEmailGateway")));
            TextBox gridInitialPauseTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[7].FindControl("txtGridInitialPause")));

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

            if (gridDeviceTypeTxt.Text.Trim().Length == 0  && isAddClicked == "false")
            {
                errorMessage = "Please enter NotificationDevice Type.\\n";

                gridDeviceTypeTxt.Focus();
            }

            switch (deviceType)
            {
                case (int)NotificationDevice.EMail:
                    if (!Utils.RegExMatch(gridDeviceNumberTxt.Text.Trim()))
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
                    if (gridDeviceNumberTxt.Text.Trim().Length == 0)
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
                case (int)NotificationDevice.Fax:
                    if (gridDeviceNumberTxt.Text.Trim().Length == 0)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter Phone Number\\n";
                    }
                    else if (!Utils.isNumericValue(gridDeviceNumberTxt.Text.Trim()) || gridDeviceNumberTxt.Text.Trim().Length != 10)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter valid Phone Number\\n";
                    }
                    break;
                case (int)NotificationDevice.SMS:
                    if (gridDeviceNumberTxt.Text.Trim().Length == 0)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter Phone Number\\n";
                    }
                    else if ((int)gridDeviceNumberTxt.Text.Trim().Length != 10)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter valid Phone Number\\n";
                    }

                    if (!Utils.RegExMatch(gridEmailGatewayTxt.Text.Trim()))
                    {
                        if (errorMessage.Length == 0)
                            gridEmailGatewayTxt.Focus();
                        errorMessage += "Please enter valid E-mail ID.\\n";
                    }
                    break;

                case (int)NotificationDevice.PagerAlpha:
                    if (gridDeviceNumberTxt.Text.Trim().Length == 0)
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
                        errorMessage += "Please enter valid Pager Number\\n";
                    }
                    
                    if (!Utils.RegExMatch(gridEmailGatewayTxt.Text.Trim()))
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
                    
                    break;
            }
            if (errorMessage.Length > 0)
            {
                generateDataGridHeight("databind");
                ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "Grid_Alert", "alert('" + errorMessage + "');", true);
                returnVal = false;
            }
            return returnVal;
        }

        #endregion

        #endregion
    }
}