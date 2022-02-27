#region File History

/******************************File History***************************
 * File Name        : group_maintenance_findings.aspx.cs
 * Author           : 
 * Created Date     : 
 * Purpose          : This Class will provide Group findings, and details about group notifications.
 *                  : 
 * *********************File Modification History*********************
 * Date(mm-dd-yyyy) Developer Reason of Modification
 * 12-07-2007   IAK     Defect 2407 fixed
 * 12-12-2007   IAK     Added document only and active column
 * 18-12-2007   Prerak  iteration 17 Code review fixes review details 36-39 fixed.
 * 12 Jun 2008  Prerak  Migration of AJAX Atlas to AJAX RTM 1.0
 *                      Removed Iframe and Merged Notification Step1, Step2  
 * 11 Aug 2008  Prerak  CR #255 Conformation pouup for add/edit devices
 * 09 Sep 2008  Prerak  Sharepoint defect #558 "The user can't enter the correct email gateway 
 *                      for a clinical team or unit" fixed
 * 10 Sep 2008  Prerak  if only number is chaged email gateway updated automatically.
 * 14 Oct 2008  Prerak  Live Agent "SendToAgent" flag for powerscribe users.
 * 31 Oct 2008  Sheetal Remove Pager validations
 * 13 Nov 2008  Zeeshan Defect #3164 - User clicks on Edit for a device at the bottom of the list they are forced to scroll to find the device they selected.
 * 18 Nov 2008  Prerak  Defect #4165 Fixed
 * 18-Nov-2008  IAK     Defect #3113 Fixed
 * 20-Nov-2008  IAK     Defect #3113, #4165 Fixed: Handled blank values
 * 11-24-2008   SD      Defect #4225 fixed
 * 12-19-2008   GB      Added Default column and changed the column order as per TTP #244 and #231.  
 * 01-05-2009       RG          FR 282 Changes
 * 01-13-2009       RG      Suggestion from Fred
 * ------------------------------------------------------------------- 
 */
#endregion

#region Imports

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
using System.Web.UI.HtmlControls;
using Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.Utilities;

#endregion

namespace Vocada.CSTools
{
	/// <summary>
	/// This page class is for Group Maintenance i.e. Finding out which Group the user belongs to and what is his role assigned.
    /// If he is a group administrator then who are the users coming under him and what role they play.
    /// What is there contact nos and Address informations. 
    /// Preference Settings for that group will be set by group administrator.
	/// </summary>
	public partial class group_maintenance_findings : System.Web.UI.Page
    {
        #region Private Variable
        private const string GROUP_ID = "GroupID";
        private const string TAP_800_NUM = "TAP800Num";

        //Constants for Toggle Button Name
        private const string SHOWDETAILS_BUTTONNAME = "Assign Event";
        private const string HIDEDETAILS_BUTTONNAME = "Hide Event Details";
        #endregion

        #region Events

        /// <summary>
        /// This function Loads all User Group information when page loads. Fill all then findings for that particular groups.
        /// This function generated height of datagrid dynamically depending on number of records available into datagrid.
        /// QueryStringtoIframe is fuction which helps frames to load the particular page for Group Notifications Devices and Notification Events to Devices for that group 
        /// Page_Load takes care of error handling and Generating error log file whenever any unexpected error will be caught.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                if (Session[SessionConstants.USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
                    Response.Redirect(Utils.GetReturnURL("default.aspx", "group_maintenance_findings.aspx", this.Page.ClientQueryString));
                UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
                this.grdFindings.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdFindings_ItemDataBound);

                registerJavascriptVariables();
                if (!Page.IsPostBack)
                {
                    if (Request["GroupID"] != null)
                    {
                        ViewState["GroupID"] = Request["GroupID"].ToString();
                        getGroupName();
                        fillGroupFindings();
                        generateDataGridHeight();
                        //Step1
                        setTAP800Number();
                        fillDevices();
                        fillGroupDevices();
                        getCellPhoneCarriers();
                        getPagerCarriers();
                        //End Step1
                        //Step 2
                        fillEventDDL(cmbEvents);
                        fillFindingDDL(cmbFindings);
                        //End Step2
                        QueryStringtoIframe();
                    }
                }
                //Step1
                txtDeviceAddress.Attributes.Add("onclick", "RemoveDeviceLabel();");

                if (cmbDevices.Items.Count > 0 && cmbDevices.SelectedItem.Text.Equals("Email"))
                    txtGateway.Attributes.Add("onclick", "RemoveGatewayLabel();");

                if (int.Parse(cmbDevices.SelectedItem.Value) == NotificationDevice.pagerPartner.GetHashCode() || int.Parse(cmbDevices.SelectedItem.Value) == NotificationDevice.PagerNumSkyTel.GetHashCode() || int.Parse(cmbDevices.SelectedItem.Value) == NotificationDevice.PagerNumUSA.GetHashCode() || int.Parse(cmbDevices.SelectedItem.Value) == NotificationDevice.PagerNumRegular.GetHashCode())
                {
                    txtDeviceAddress.Attributes.Add("onkeyPress", "JavaScript:return PagerValidationWithSpace('" + txtDeviceAddress.ClientID + "');");
                }
                else
                {
                    txtDeviceAddress.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes();");
                }
                //End Step1
                if (userInfo.RoleId == UserRoles.SupportLevel2.GetHashCode())
                {
                    lnkAddFindings.Visible = false;
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("group_maintenance_findings - Page_Load: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                generateStep1DataGridHeight();
            }
        }

        /// <summary>
        /// This is Dynamically binding data to datagrid "grdFindings" where we can set changes which performing dynamic binding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdFindings_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            string dayMode = "am";
            int hour = 0;
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView data = e.Item.DataItem as DataRowView;
                    //if (e.Item.Cells[9].Text[0].CompareTo('0') == 0) //SendOTNAt
                    //    e.Item.Cells[9].Text = "-";

                    if (!bool.Parse(e.Item.Cells[10].Text)) //"Embargo"
                    {

                        e.Item.Cells[11].Text = "-";
                        e.Item.Cells[12].Text = "-";
                        e.Item.Cells[13].Text = "-";
                    }
                    else // there is an embargo period, make sure times represented properly...
                    {
                        hour = int.Parse(e.Item.Cells[11].Text);//"EmbargoStartHour"
                        if (hour > 12)
                        {
                            dayMode = "pm";
                            hour -= 12;
                        }
                        else if (hour == 12)
                        {
                            dayMode = "noon";
                        }
                        e.Item.Cells[11].Text = hour.ToString() + ":00 " + dayMode;

                        dayMode = "am";
                        hour = int.Parse(e.Item.Cells[12].Text);//"EmbargoEndHour"
                        if (hour > 12)
                        {
                            dayMode = "pm";
                            hour -= 12;
                        }
                        else if (hour == 12)
                        {
                            dayMode = "noon";
                        }
                        e.Item.Cells[12].Text = hour.ToString() + ":00 " + dayMode;
                    }
                    if (bool.Parse(e.Item.Cells[14].Text)) //"Require Readback"
                    {
                        e.Item.Cells[14].Text = "<image src=img/ic_tick.gif border=0>";
                    }
                    else
                    {
                        e.Item.Cells[14].Text = "-";
                    }

                    string findingURL = (data["FindingVoiceOverURL"] == DBNull.Value ? "" : (string)(data["FindingVoiceOverURL"]));
                    if (findingURL.StartsWith("http") || findingURL.StartsWith("https"))
                    {
                        e.Item.Cells[4].Text = "<a href=" + findingURL + "><img src=img/ic_play_msg.gif border=0></a>";
                    }
                    else
                    {
                        e.Item.Cells[4].Text = "";
                    }
                    bool isDocumented = Convert.ToBoolean(data["isDocumented"]);
                    bool isActive = Convert.ToBoolean(data["isFindingActive"]);
                    bool isDefault = Convert.ToBoolean(data["IsDefault"]);
                    if (isDocumented)
                    {
                        ((System.Web.UI.WebControls.Image)e.Item.FindControl("imgDocumented")).ImageUrl = "img/True.GIF";
                    }
                    else
                    {
                        ((System.Web.UI.WebControls.Image)e.Item.FindControl("imgDocumented")).ImageUrl = "img/False.GIF";
                    }

                    if (isActive)
                    {
                        ((System.Web.UI.WebControls.Image)e.Item.FindControl("imgActive")).ImageUrl = "img/True.GIF";
                    }
                    else
                    {
                        ((System.Web.UI.WebControls.Image)e.Item.FindControl("imgActive")).ImageUrl = "img/False.GIF";
                    }

                    if (isDefault)
                    {
                        ((System.Web.UI.WebControls.Image)e.Item.FindControl("imgDefault")).ImageUrl = "img/True.GIF";
                    }
                    else
                    {
                        ((System.Web.UI.WebControls.Image)e.Item.FindControl("imgDefault")).ImageUrl = "img/False.GIF";
                    }

                    /* // Is Finding Active - (Soft Delete Finding)
                    bool isActive = (bool)data["IsFindingActive"];
                    ((CheckBox)(e.Item.Cells[15].FindControl("chkIsActive"))).Checked = isActive;
                    ((LinkButton)(e.Item.Cells[17].Controls[0])).Enabled = isActive;
                    */
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("group_maintenance_findings - grdFindings_ItemDataBound: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }


        /// <summary>
        /// Item Command event for findings grid control.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdFindings_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                if (string.Compare(e.CommandName, "Edit") == 0)
                {
                    Response.Redirect("add_findings.aspx?Mode=Edit&FindingID=" + e.Item.Cells[0].Text);
                    return;
                }
                            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("group_maintenance_findings - grdFindings_DeleteCommand: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// Redirect page to add findings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAddFindings_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("./add_findings.aspx?GroupID=" + ViewState["GroupID"].ToString() + "&institutionID=" + ViewState["InstituteID"].ToString());
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("group_maintenance_findings - generateDataGridHeight: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// Redirect page to Group Preferences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkGroupPreferences_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("./group_preferences.aspx?GroupID=" + ViewState["GroupID"].ToString());
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("group_maintenance_findings - generateDataGridHeight: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// Redirect page to Group Maintenance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkGroupMaintenance_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("./group_maintenance.aspx?GroupID=" + ViewState["GroupID"].ToString());
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("group_maintenance_findings - generateDataGridHeight: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        #region Step1

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
                if (txtDeviceAddress.Text.Trim().Length == 0)
                {
                    txtDeviceAddress.Text = hdnDeviceLabel.Value;
                    return;
                }

                if (cmbCarrier.Items.Count > 0 && cmbCarrier.SelectedItem.Value.Equals("-1"))
                    return;

                generateGatewayAddress();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - txtDeviceAddress_TextChanged: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// This function is to Add Devices into Device list for logged in user.
        /// This function calls stored procedure "insertGroupDevice" for groupId
        /// After adding devices into datagrid it refresh Group maintenance page for effect to be shown immediately.
        /// Exception handling part takes care if any error occured while adding devices for groupID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddDevice_Click(object sender, System.EventArgs e)
        {
            Device device = null;
            string deviceAddress;
            string gateway = null;
            string carrier = null;
            
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
                        acRegScript.AppendFormat("document.getElementById(" + '"' + GroupFindingsDevicesDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                        acRegScript.AppendFormat("alert('Please enter valid pager number');");
                        acRegScript.Append("</script>");
                        ScriptManager.RegisterClientScriptBlock(upnlStep1, upnlStep1.GetType(), "Register", acRegScript.ToString(), false);
                        generateStep1DataGridHeight();
                        
                        return;
                    }

                    if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerTAP || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerTAPA)
                    {
                        if ((Utils.RegExNumericMatch(txtGateway.Text.Trim())) == false)
                        {
                            //"Alphanumeric characters in pager tap number"
                            StringBuilder acRegScript = new StringBuilder();
                            acRegScript.Append("<script type=\"text/javascript\">");
                            acRegScript.AppendFormat("document.getElementById(" + '"' + GroupFindingsDevicesDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                            acRegScript.AppendFormat("alert('Please enter valid pager number');");
                            acRegScript.Append("</script>");
                            ScriptManager.RegisterClientScriptBlock(upnlStep1, upnlStep1.GetType(), "Register", acRegScript.ToString(), false);
                            generateStep1DataGridHeight();
                            
                            return;
                        }
                    }
                }


                if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.EMail)
                {
                    if ((Utils.RegExMatch(txtGateway.Text.Trim())) == false)
                    {
                        //"Email format incorrect"
                        StringBuilder acRegScript = new StringBuilder();
                        acRegScript.Append("<script type=\"text/javascript\">");
                        acRegScript.AppendFormat("document.getElementById(" + '"' + GroupFindingsDevicesDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                        acRegScript.AppendFormat("alert('Please enter valid Email ID');");
                        acRegScript.Append("</script>");
                        ScriptManager.RegisterClientScriptBlock(upnlStep1,upnlStep1.GetType(), "Register", acRegScript.ToString(),false);
                        generateStep1DataGridHeight();
                        return;
                    }

                }
                else if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerTAP || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerTAPA)
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
                        acRegScript.AppendFormat("document.getElementById(" + '"' + GroupFindingsDevicesDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                        acRegScript.AppendFormat("alert('" + error + "');");
                        acRegScript.Append("</script>");
                        ScriptManager.RegisterClientScriptBlock(upnlStep1,upnlStep1.GetType(),"Register", acRegScript.ToString(),false);
                        generateStep1DataGridHeight();
                        return;
                    }
                }

                else
                {
                    string deviceAdd = txtDeviceAddress.Text.Trim();
                    if (int.Parse(cmbDevices.SelectedItem.Value) != (int)NotificationDevice.EMail)
                    {
                        string error = "";
                        if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.Fax || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.SMS || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.SMS_WebLink)
                        {
                            if (deviceAdd.Length != 10)
                            {
                                error = "Please enter valid Number\\n";
                            }
                        }
                        if (int.Parse(cmbDevices.SelectedItem.Value) != (int)NotificationDevice.PagerAlpha && int.Parse(cmbDevices.SelectedItem.Value) != (int)NotificationDevice.pagerPartner && int.Parse(cmbDevices.SelectedItem.Value) != (int)NotificationDevice.PagerNumRegular && int.Parse(cmbDevices.SelectedItem.Value) != (int)NotificationDevice.PagerNumSkyTel && int.Parse(cmbDevices.SelectedItem.Value) != (int)NotificationDevice.PagerNumUSA)
                        {
                            if (!Utils.isNumericValue(deviceAdd))
                            {
                                error = "Please enter valid Number\\n";
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
                            acRegScript.AppendFormat("document.getElementById(" + '"' + GroupFindingsDevicesDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
                            acRegScript.AppendFormat("alert('" + error + "');");
                            acRegScript.Append("</script>");
                            ScriptManager.RegisterClientScriptBlock(upnlStep1,upnlStep1.GetType(),"Register", acRegScript.ToString(),false);
                            generateStep1DataGridHeight();
                            return;
                        }

                    }
                }
                deviceAddress = txtDeviceAddress.Text.Trim();
                if (cmbDevices.Items.Count > 0 && cmbDevices.SelectedItem.Text.Trim().Equals("Email"))
                    deviceAddress = txtGateway.Text.Trim();

                if (!(cmbDevices.Items.Count > 0 && cmbDevices.SelectedItem.Text.Trim().Equals("Email")))
                    gateway = txtGateway.Text.Trim();

                if (cmbCarrier.Items.Count > 0 && cmbCarrier.SelectedIndex > -1 && cmbCarrier.Visible)
                    carrier = cmbCarrier.SelectedItem.Text.Trim();

                int eventID = (cmbEvents.Visible) ? Convert.ToInt32(cmbEvents.SelectedValue) : 0;
                int findingID = (cmbFindings.Visible) ? Convert.ToInt32(cmbFindings.SelectedValue) : 0;

                device = new Device();
                device.InsertGroupDevice(int.Parse(ViewState[GROUP_ID].ToString()),
                                         int.Parse(cmbDevices.SelectedItem.Value), deviceAddress, gateway, carrier,eventID,findingID);
                                
                
                fillGroupDevices();
                cmbDevices.SelectedValue = "-1";
                setLabelsAndInputBoxes(-1);
                txtDeviceAddress.Text = "";                

                String regScript = "";

                //regScript += "document.getElementById(" + '"' + GroupFindingsDevicesDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';";

                regScript += "alert('Device has been added');";
                ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "RefreshParent", regScript, true);
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - btnAddDevice_Click: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                device = null;
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
                cmbFindings.Visible = isShow;
                cmbEvents.SelectedValue = "-1";
                cmbFindings.SelectedValue = "-1";
                lblEvents.Visible = isShow;
                lblFindings.Visible = isShow;
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
                generateStep1DataGridHeight();
            }
        }

        /// <summary>
        /// This function is to Select Phone Carrier from list for Device list for groupID
        /// This function also calls generateGatewayAddress method to update the Gateway for that particular Phone carrier.
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
                generateStep1DataGridHeight();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - cmbCarrier_SelectedIndexChanged: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// This function is to setLabelsAndInputBoxes for changing values of Lables and Input boxes when Device index are changed 
        /// Exeption hanlding created error log when any error occured.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbDevices_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (grdDevices.EditItemIndex == -1)
                {
                    lblDeviceAlreadyExistsStep1.Text = "";
                    int deviceID = int.Parse(cmbDevices.SelectedItem.Value);
                    setLabelsAndInputBoxes(deviceID);
                    if (deviceID == NotificationDevice.SMS.GetHashCode() || deviceID == NotificationDevice.PagerAlpha.GetHashCode())  // cell or pager.
                        generateGatewayAddress();
                    else
                        cmbCarrier.Items.Clear();

                    if (cmbDevices.Items.Count > 0)
                    {
                        if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerTAP || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerTAPA)
                        {
                            txtDeviceAddress.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes();");
                            //txtDeviceAddress.MaxLength = 6;
                        }
                        else if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.pagerPartner || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerNumSkyTel || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerNumUSA || int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerNumRegular)
                        {
                            txtDeviceAddress.Attributes.Add("onkeyPress", "JavaScript:return PagerValidationWithSpace('" + txtDeviceAddress.ClientID + "');");
                            //txtDeviceAddress.MaxLength = 100;
                        }
                        else if (int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.PagerAlpha)
                        {
                            txtDeviceAddress.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes();");
                            //txtDeviceAddress.MaxLength = 100;
                        }
                        else if (!(int.Parse(cmbDevices.SelectedItem.Value) == (int)NotificationDevice.EMail))
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
                    generateStep1DataGridHeight();
                }
                else
                {
                    generateStep1DataGridHeight();

                    if (grdDevices.EditItemIndex != -1)
                        addLinkToGridInEditMode(grdDevices.Items[grdDevices.EditItemIndex]);     

                    cmbDevices.SelectedValue = "-1";
     
                }

                
                    
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - cmbDevices_SelectedIndexChanged: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// This function call the stored procedure "VOC_VLR_deleteGroupDevice" to delete the Device from Device list of users.
        /// if Device ready to delete Already Exists into Notification Events then its not let you delete,  instead it gives you message that this device associated with notification events.
        /// First delete them and you can delete device list.
        /// If Deletion of Device is Complete it refresh the whole frame again to refresh it self.
        /// Execption handling is to take care if any unexpected error occured.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDevices_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            Device device = null;

            try
            {
                device = new Device();
                int rowsUpdated = device.DeleteGroupDevice(int.Parse(e.Item.Cells[0].Text.Trim()),int.Parse(e.Item.Cells[11].Text.Trim()));
                grdDevices.EditItemIndex = -1;
                fillGroupDevices();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - btnDeviceDelete_Click: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                device = null;               
            }
        }

        /// <summary>
        /// 
        /// This function use for Editing records of Devices datagrid.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDevices_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                grdDevices.EditItemIndex = e.Item.ItemIndex;
                lblDeviceAlreadyExistsStep1.Text = "";
                fillGroupDevices();
                TextBox tbName = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[0].FindControl("txtgrdDeviceName")));
                TextBox tbEmailGateway = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[3].FindControl("txtgrdGateway")));
                TextBox tbAddress = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[1].FindControl("txtgrdDeviceAddress")));
                DropDownList dlEvent = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[9].FindControl("dlistGridEvents")));
                DropDownList dlFinding = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[10].FindControl("dlistGridFindings")));
                string findingID = grdDevices.Items[e.Item.ItemIndex].Cells[7].Text.Trim();
                string eventID = grdDevices.Items[e.Item.ItemIndex].Cells[8].Text.Trim();           

                int deviceType = int.Parse(grdDevices.Items[e.Item.ItemIndex].Cells[6].Text.Trim());

                ViewState[Constants.DEVICE_ADDRESS] = tbAddress.Text.Trim();
                ViewState[Constants.EMAIL_GATEWAY] = tbEmailGateway.Text.Trim();

                if (deviceType == (int)NotificationDevice.PagerTAP || deviceType == (int)NotificationDevice.PagerTAPA)
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
                
                fillEventDDL(dlEvent);
                fillFindingDDL(dlFinding);

                dlEvent.SelectedValue = eventID.ToString();
                dlFinding.SelectedValue = findingID.ToString();
                setLabelsAndInputBoxes(-1);
                cmbDevices.SelectedValue = "-1";
                hdnIsAddClicked.Value = "false";
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - grdDevices_EditCommand: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// This function is to Cancel Editing on Devices Datagrid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDevices_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                grdDevices.EditItemIndex = -1;
                lblDeviceAlreadyExistsStep1.Text = "";
                hdnIsAddClicked.Value = "false";
                fillGroupDevices();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - grdDevices_CancelCommand: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// This function Update Records into Device datagrid.
        /// This function calls stored procedure "updateGroupDevice" to update record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDevices_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            Device device = null;
            string isAddClicked = hdnIsAddClicked.Value;

            try
            {
                if (!validateRecord(e))
                {
                    addLinkToGridInEditMode(grdDevices.Items[grdDevices.EditItemIndex]);
                    return;
                }
                device = new Device();
                
                TextBox deviceNameTextBox = (TextBox)e.Item.Cells[1].Controls[1];
                TextBox deviceAddressTextBox = (TextBox)e.Item.Cells[2].Controls[1];
                TextBox gatewayTextBox = (TextBox)e.Item.Cells[4].Controls[1];
                DropDownList cmbGrdEvents = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[9].FindControl("dlistGridEvents")));
                DropDownList cmbGrdFindings = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[10].FindControl("dlistGridFindings")));              
                
           
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

                //Set DeptNotifyEventID property value   
                int eventID = Convert.ToInt32(cmbGrdEvents.SelectedValue);                

                //Set FindingID property value   
                int findingID = Convert.ToInt32(cmbGrdFindings.SelectedValue);
              
                //Set GroupNotificationID
                int groupNotificationID = Convert.ToInt32(grdDevices.Items[e.Item.ItemIndex].Cells[11].Text.Trim());

                //Set Carrier
                string strCarrier = e.Item.Cells[3].Text.Trim();
                
                string regScript = "";

                if (isAddClicked == "true")
                {
                    device.InsertGroupDevice(int.Parse(ViewState[GROUP_ID].ToString()),
                                 int.Parse(e.Item.Cells[6].Text.Trim()), deviceAddressTextBox.Text.Trim(), gatewayTextBox.Text.Trim(), strCarrier, eventID, findingID);
                    grdDevices.EditItemIndex = -1;
                    fillGroupDevices();
                    regScript = "Device has been added.";
                    ViewState[Constants.DEVICE_ADDRESS] = null;
                    ViewState[Constants.EMAIL_GATEWAY] = null;
                }
                else
                {
                    int result = device.UpdateGroupDevice(int.Parse(e.Item.Cells[0].Text.Trim()), deviceNameTextBox.Text.Trim(), deviceAddressTextBox.Text.Trim(), gatewayTextBox.Text.Trim(), Convert.ToInt32(grdDevices.DataKeys[e.Item.ItemIndex]), eventID, findingID, groupNotificationID);

                    if (result == -1)
                    {
                        regScript = "Device name already exists.";
                        addLinkToGridInEditMode(grdDevices.Items[grdDevices.EditItemIndex]);
                        ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "DeviceExists", "alert('" + regScript + "');", true);
                        return;
                    }
                    else
                    {
                        lblDeviceAlreadyExistsStep1.Text = "";
                        grdDevices.EditItemIndex = -1;
                        fillGroupDevices();
                        regScript = "Device has been updated.";
                        ViewState[Constants.DEVICE_ADDRESS] = null;
                        ViewState[Constants.EMAIL_GATEWAY] = null;
                    }
                }               
                
                ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "DeviceExists", "alert('" + regScript + "');", true);                
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - grdDevices_UpdateCommand: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                device = null;
                hdnIsAddClicked.Value = "false";                
                generateStep1DataGridHeight();
            }
        }

        protected void grdDevices_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
            {
                DataRowView data = (DataRowView)e.Item.DataItem;
                // Reference the Delete button
                ((LinkButton)(e.Item.FindControl("DeleteButton"))).OnClientClick = "return confirm('Are you sure you want to delete?');";
                Label gridFindingLabel = ((Label)(e.Item.Cells[10].FindControl("lblGridDeviceFinding")));
                Label gridEventLabel = ((Label)(e.Item.Cells[9].FindControl("lblGridDeviceEvent")));

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

                    if ((int)data.Row["GroupNotifyEventID"] == -1)
                    {
                        gridEventLabel.Text = "All Events";
                        gridEventLabel.ToolTip = "All Events";
                    }
                    else
                    {
                        gridEventLabel.Text = data.Row["EventDescription"].ToString();
                        gridEventLabel.ToolTip = data.Row["EventDescription"].ToString();
                    }
                }
                else
                    addLinkToGridInEditMode(e.Item);
            }
        }

        #endregion Step1
       
        #endregion Events

        #region Private Methods

        /// <summary>
        /// This void functions is to Call Notification Device page i.e. "GMFNotificationStep1.aspx" into frame called "ifrmGMFNotificationStep1"
        /// and Notification Device Events page i.e. "GMFNotificationStep2.aspx" into frame called "ifrmGMFNotificationStep1" part.
        /// This function simply loads these pages on demand.
        /// </summary>
        public void QueryStringtoIframe()
        {            
            lnkGroupPreferences.Attributes.Add("OnClientClick", "./group_preferences.aspx?GroupID=" + ViewState["GroupID"].ToString());
            lnkGroupMaintenance.Attributes.Add("OnClientClick", "./group_maintenance.aspx?GroupID=" + ViewState["GroupID"].ToString());
        }

        /// <summary>
        /// This function is to fill Groups Finding into datagrid "grdFindings". 
        /// This function handles data handling and error log file when any error occured while filling up grdFindings
        /// </summary>
        /// <param name="cnx"></param>
		private void fillGroupFindings()
		{
            Group group = null;
            DataTable dtGroupFindings = null;
            try
            {
                int groupID = Convert.ToInt32(ViewState["GroupID"].ToString());
                group = new Group();
                dtGroupFindings = group.GetGroupFindings(groupID);
                grdFindings.DataSource = dtGroupFindings.DefaultView;
                grdFindings.DataBind();
            }
            finally
            {
                group = null;
                dtGroupFindings = null;
            }
		}

        /// <summary>
        /// This function is to fill Groups Finding into datagrid "grdFindings". 
        /// This function handles data handling and error log file when any error occured while filling up grdFindings
        /// </summary>
        /// <param name="cnx"></param>
        private void getGroupName()
		{
            Group group = null;
            DataTable dtGroupInfo = null;
            try
            {
                int groupID = Convert.ToInt32(ViewState["GroupID"].ToString());
                string groupName = "";
                group = new Group();
                dtGroupInfo = group.GetGroupInformationByGroupID(groupID);
                groupName = dtGroupInfo.Rows[0]["GroupName"].ToString();
                ViewState["InstituteID"] = dtGroupInfo.Rows[0]["InstitutionID"].ToString();
                lblFindingsTitle.Text = "Findings and Notifications - " + groupName;
            }
            finally
            {
                group = null;
                dtGroupInfo = null;
            }
		}

        private void registerJavascriptVariables()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var cmbDevicesClientID = '" + cmbDevices.ClientID + "';");
            sbScript.Append("var txtGatewayClientID = '" + txtGateway.ClientID + "';");
            sbScript.Append("var txtDeviceAddressClientID = '" + txtDeviceAddress.ClientID + "';");
            sbScript.Append("var cmbCarrierClientID = '" + cmbCarrier.ClientID + "';");
            sbScript.Append("var hdnDeviceLabelClientID = '" + hdnDeviceLabel.ClientID + "';");
            sbScript.Append("var hdnGatewayLabelClientID = '" + hdnGatewayLabel.ClientID + "';");
            sbScript.Append("var hiddenScrollPos = '" + scrollPos.ClientID + "';");
            sbScript.Append("var hdnIsAddClickedClientID = '" + hdnIsAddClicked.ClientID + "';");
            sbScript.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());
        }
        
        /// <summary>
        /// This function is used to create dynamic height for DataGrids.
        /// It counts the no of items on datagrid then sets the height of datagrid accordingly.
        /// </summary>
		private void generateDataGridHeight()
        {
            try
            {
                int findingsGridHeight = 20;
                int gridRowHeight = 21;
                int gridHeaderHeight = 25;
                int maxGridRows = 4;

                if(grdFindings.Items.Count <= maxGridRows)
                {
                    if(grdFindings.Items.Count == 0)
                        findingsGridHeight = gridHeaderHeight;
                    else
                        findingsGridHeight = (grdFindings.Items.Count * gridRowHeight) + gridHeaderHeight;
                }
                else
                {
                    findingsGridHeight = (maxGridRows * gridRowHeight) + gridHeaderHeight;
                }
                
                string newUid = this.UniqueID.Replace(":", "_");
                string scriptBlock = "<script type=\"text/javascript\">";
                scriptBlock += "document.getElementById(" + '"' + "GroupFindingsDiv" + '"' + ").style.height='" + (findingsGridHeight + 10) + "';</script>";
                ScriptManager.RegisterStartupScript(upnlFindings, upnlFindings.GetType(),newUid, scriptBlock,false);
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("group_maintenance_findings - generateDataGridHeight: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        #region Step1
        /// <summary>
        /// This function will fill the list of device for logged in user groupID
        /// If there is no record exists then label will show that.
        /// This functions will set dynamic height of Device datagrid depending on records available for device.
        /// if Exception occurs then that will be caught in catch block and create error log file.
        /// </summary>
        private void fillGroupDevices()
        {
            Device device = null;
            DataTable dtDevices = null;
            try
            {
                device = new Device();
                dtDevices = device.GetGroupDevices(int.Parse(ViewState[GROUP_ID].ToString()));
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
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - fillGroupDevices: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                device = null;
                dtDevices = null;
                generateStep1DataGridHeight();
            }
        }

        /// <summary>
        /// This function is to fill all the list of Devices into Drop down list box.
        /// Exception handling is done in this function which create error log file.
        /// </summary>
        /// <param name="cnx">Connection String</param>
        private void fillDevices()
        {
            Device device = null;
            DataTable dtDevices = null;
            try
            {
                device = new Device();
                dtDevices = device.GetAllDevices();
                cmbDevices.DataSource = dtDevices.DefaultView;
                cmbDevices.DataBind();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - fillDevices: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                device = null;
                dtDevices = null;

                cmbDevices.Items.Remove(cmbDevices.Items.FindByValue("14"));
                cmbDevices.Items.Remove(cmbDevices.Items.FindByValue("12"));
                cmbDevices.Items.Remove(cmbDevices.Items.FindByValue("11"));
                ListItem listItem = new ListItem("-- Select Device To Add --", "-1");
                cmbDevices.Items.Add(listItem);
                cmbDevices.Items.FindByValue("-1").Selected = true;
            }
        }

        /// <summary>
        /// This function is to fill all the list of Phone Carriers into Drop down list box.
        /// Exception handling is done in this function which create error log file.
        /// </summary>
        /// <param name="cnx">Connection String</param>
        private void getCellPhoneCarriers()
        {
            Device device = null;
            DataTable dtCellCarriers = null;
            try
            {
                device = new Device();
                dtCellCarriers = device.GetCellPhoneCarriers();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - getCellPhoneCarriers: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                ViewState["CellPhoneCarriers"] = dtCellCarriers;
                dtCellCarriers = null;
                device = null;
            }
        }

        /// <summary>
        /// This function is to fill all the list of Pager Carriers into Drop down list box.
        /// Exception handling is done in this function which create error log file.
        /// </summary>
        /// <param name="cnx">Connection String</param>
        private void getPagerCarriers()
        {
            Device device = null;
            DataTable dtPagerCarriers = null;
            try
            {
                device = new Device();
                dtPagerCarriers = device.GetPagerCarriers();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - getPagerCarriers: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                ViewState["PagerCarriers"] = dtPagerCarriers;
                device = null;
                dtPagerCarriers = null;
            }
        }

        /// <summary>
        /// This function sets dynamic height for Datagrid grdDevices depening no of records available
        /// Exception handling part is to see if any error occured which setting dynamic height.
        /// </summary>
        private void generateStep1DataGridHeight()
        {
            try
            {
                string scriptBlock = "<script type=\"text/javascript\">";
                scriptBlock += "if(document.getElementById(" + '"' + GroupFindingsDevicesDiv.ClientID + '"' + ") != null){document.getElementById(" + '"' + GroupFindingsDevicesDiv.ClientID + '"' + ").style.height=setHeightOfGrid('" + grdDevices.ClientID + "','" + 60 + "', '168');}";
                scriptBlock += "document.getElementById(" + '"' + GroupFindingsDevicesDiv.ClientID + '"' + ").scrollTop=document.getElementById('" + scrollPos.ClientID + "').value;</script>";
                ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "SetHeight", scriptBlock, false);
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - generateStep1DataGridHeight: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// This function is to Generate Gateway Address for selected Device from device list.
        /// This function hanldes if any error occures while setting Gateway address.
        /// </summary>
        private void generateGatewayAddress()
        {
            DataTable dtcarriers = null;
            DataRow[] carrierRows = null;
            try
            {
                int deviceID = int.Parse(cmbDevices.SelectedItem.Value);
                IEnumerator iEnumerator;

                switch (deviceID)
                {
                    case (int)NotificationDevice.SMS: // Cell Phones
                        txtGateway.Text = txtDeviceAddress.Text.Trim() + "@";
                        dtcarriers = (DataTable)ViewState["CellPhoneCarriers"];
                        carrierRows = dtcarriers.Select("CarrierID='" + cmbCarrier.SelectedItem.Value+"'"); // should have 1
                        iEnumerator = carrierRows.GetEnumerator();
                        if (iEnumerator.MoveNext())
                        {
                            DataRow currentCarrier = (DataRow)iEnumerator.Current;
                            txtGateway.Text += currentCarrier["CarrierEmail"];
                        }
                        break;
                    case (int)NotificationDevice.PagerAlpha: // Pagers
                        txtGateway.Text = txtDeviceAddress.Text.Trim() + "@";
                        dtcarriers = (DataTable)ViewState["PagerCarriers"];
                        carrierRows = dtcarriers.Select("CarrierID='" + cmbCarrier.SelectedItem.Value+"'"); // should have 1
                        iEnumerator = carrierRows.GetEnumerator();
                        if (iEnumerator.MoveNext())
                        {
                            DataRow currentCarrier = (DataRow)iEnumerator.Current;
                            txtGateway.Text = txtDeviceAddress.Text.Trim() + "@" + currentCarrier["CarrierEmail"].ToString();
                        }
                        break;
                    case (int)NotificationDevice.PagerTAP:
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
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - generateGatewayAddress: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                dtcarriers = null;
                carrierRows = null;
            }
        }

        /// <summary>
        /// This function to set Set Labels and Values for Input boxes for selected Device.
        /// Execption handling sets any error occures while process.
        /// </summary>
        /// <param name="deviceID">Integer Type</param>
        private void setLabelsAndInputBoxes(int deviceID)
        {
            ListItem listItem = null;
            try
            {
                listItem = new ListItem("-- Select Carrier", "-1");
                
                cmbEvents.Visible = false;
                lblEvents.Visible = false;
                cmbFindings.Visible = false;
                lblFindings.Visible = false;
                cmbEvents.SelectedValue = "-1";
                cmbFindings.SelectedValue = "-1";
                btnShowHideDetails.Text = SHOWDETAILS_BUTTONNAME;
                btnShowHideDetails.Visible = true;

                switch (deviceID)
                {
                    case (int)NotificationDevice.EMail:  // Email
                        txtGateway.Text = "Enter Email Address";
                        txtDeviceAddress.Visible = false;
                        lblNumAddress.Visible = false;
                        txtDeviceAddress.Text = "";
                        txtDeviceAddress.Width = Unit.Pixel(250);
                        txtDeviceAddress.AutoPostBack = false;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtGateway.Visible = true;
                        lblEmailGateway.Visible = true;
                        lblEmailGateway.Text = "Number / Address";
                        btnAddDevice.Visible = true;
                        break;
                    case (int)NotificationDevice.SMS:  // SMS/Cell
                        txtDeviceAddress.Text = "Enter Cell # (numbers only)";
                        txtDeviceAddress.MaxLength = 10;
                        txtDeviceAddress.Visible = true;
                        lblNumAddress.Visible = true;
                        txtDeviceAddress.Width = Unit.Pixel(175);
                        cmbCarrier.Visible = true;
                        lblCarrier.Visible = true;
                        txtGateway.Visible = true;
                        lblEmailGateway.Visible = true;
                        lblEmailGateway.Text = "Email Gateway";
                        btnAddDevice.Visible = true;
                        cmbCarrier.DataSource = (DataTable)ViewState["CellPhoneCarriers"];
                        cmbCarrier.DataBind();

                        cmbCarrier.Items.Add(listItem);
                        cmbCarrier.Items.FindByValue("-1").Selected = true;
                        break;
                   case (int)NotificationDevice.PagerAlpha:  // Pager - Alpha
                        txtDeviceAddress.Text = "Enter Pager # (numbers only)";
                        txtDeviceAddress.Visible = true;
                        lblNumAddress.Visible = true;
                        txtDeviceAddress.Width = Unit.Pixel(175);
                        cmbCarrier.Visible = true;
                        lblCarrier.Visible = true;
                        txtGateway.Visible = true;
                        lblEmailGateway.Visible = true;
                        lblEmailGateway.Text = "Email Gateway";
                        btnAddDevice.Visible = true;
                        cmbCarrier.DataSource = (DataTable)ViewState["PagerCarriers"];
                        cmbCarrier.DataBind();

                        cmbCarrier.Items.Add(listItem);
                        cmbCarrier.Items.FindByValue("-1").Selected = true;
                        break;
                    case (int)NotificationDevice.PagerNumRegular:
                    case (int)NotificationDevice.PagerNumSkyTel:
                    case (int)NotificationDevice.PagerNumUSA:
                        txtDeviceAddress.Text = "Enter Pager # + PIN (numbers only)";
                        txtDeviceAddress.Width = Unit.Pixel(250);
                        txtDeviceAddress.Visible = true;
                        lblNumAddress.Visible = true;
                        txtDeviceAddress.AutoPostBack = false;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtGateway.Visible = false;
                        lblEmailGateway.Visible = false;
                        txtGateway.Text = "";
                        btnAddDevice.Visible = true;
                        break;
                    case (int)NotificationDevice.Fax:  // Fax
                        txtDeviceAddress.Text = "Enter Fax # (numbers only)";
                        txtDeviceAddress.Visible = true;
                        lblNumAddress.Visible = true;
                        txtDeviceAddress.MaxLength = 10;
                        txtDeviceAddress.Width = Unit.Pixel(175);
                        txtDeviceAddress.AutoPostBack = false;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtGateway.Visible = false;
                        lblEmailGateway.Visible = false;
                        txtGateway.Text = "";
                        btnAddDevice.Visible = true;
                        break;

                    case (int)NotificationDevice.pagerPartner:
                        txtDeviceAddress.Text = "Enter Pager # + PIN (numbers only)";
                        txtDeviceAddress.Width = Unit.Pixel(250);
                        txtDeviceAddress.Visible = true;
                        lblNumAddress.Visible = true;
                        txtDeviceAddress.AutoPostBack = false;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtGateway.Visible = false;
                        lblEmailGateway.Visible = false;
                        txtGateway.Text = "";
                        btnAddDevice.Visible = true;
                        break;

                    case (int)NotificationDevice.OutboundCallCI:  // Outbound Call with Callback Instructions
                        txtDeviceAddress.Text = "Enter Outbound Phone Call number (numbers only)";
                        txtDeviceAddress.Visible = true;
                        lblNumAddress.Visible = true;
                        txtDeviceAddress.Width = Unit.Pixel(255);
                        txtDeviceAddress.AutoPostBack = false;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtGateway.Visible = false;
                        lblEmailGateway.Visible = false;
                        txtGateway.Text = "";
                        btnAddDevice.Visible = true;
                        break;

                    case -1:
                        txtDeviceAddress.Visible = false;
                        lblNumAddress.Visible = false;
                        txtDeviceAddress.Text = "";
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtGateway.Visible = false;
                        lblEmailGateway.Visible = false;
                        txtGateway.Text = "";
                        btnAddDevice.Visible = false;
                        btnShowHideDetails.Visible = false;
                        lblEvents.Visible = false;
                        lblFindingsTitle.Visible = false;
                        break;
                    case (int)NotificationDevice.PagerTAP:  // Pager TAP device
                    case (int)NotificationDevice.PagerTAPA:
                        txtDeviceAddress.Text = "Enter PIN number (numbers only)";
                        //txtDeviceAddress.MaxLength = 6;
                        txtDeviceAddress.Visible = true;
                        lblNumAddress.Visible = true;
                        txtDeviceAddress.Width = Unit.Pixel(175);
                        txtDeviceAddress.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
                        txtDeviceAddress.AutoPostBack = false;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        if (ViewState[TAP_800_NUM].ToString().Length == 0)
                            txtGateway.Text = "Enter TAP 800 number (numbers only)";
                        else
                            txtGateway.Text = ViewState[TAP_800_NUM].ToString();
                        //txtGateway.MaxLength = 10;
                        txtGateway.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
                        txtGateway.Attributes.Add("onclick", "RemoveGatewayLabel();");
                        txtGateway.Visible = true;
                        lblEmailGateway.Visible = true;
                        lblEmailGateway.Text = "Email Gateway";
                        btnAddDevice.Visible = true;
                        break;
                    default:
                        txtDeviceAddress.Visible = false;
                        lblNumAddress.Visible = false;
                        txtDeviceAddress.Text = "";
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtGateway.Visible = false;
                        lblEmailGateway.Visible = false;
                        txtGateway.Text = "";
                        btnAddDevice.Visible = false;
                        cmbEvents.Visible = false;
                        cmbFindings.Visible = false;
                        btnShowHideDetails.Visible = false;
                        lblEvents.Visible = false;
                        lblFindingsTitle.Visible = false;
                        break;
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep1 - setLabelsAndInputBoxes: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                listItem = null;
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
            int deviceType = int.Parse(grdDevices.Items[e.Item.ItemIndex].Cells[6].Text.Trim());
            TextBox gridDeviceTypeTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[1].FindControl("txtgrdDeviceName")));
            TextBox gridDeviceNumberTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[2].FindControl("txtgrdDeviceAddress")));
            TextBox gridEmailGatewayTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[4].FindControl("txtgrdGateway")));

            DropDownList dlEvent = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[5].FindControl("dlistGridEvents")));
            DropDownList dlFinding = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[6].FindControl("dlistGridFindings")));
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
                case (int)NotificationDevice.OutboundCallCB:
                case (int)NotificationDevice.OutboundCallRS:
                case (int)NotificationDevice.OutboundCallCI:

                    if (gridDeviceNumberTxt.Text.Trim().Length == 0)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter Phone Number\\n";
                    }
                    else if (!Utils.isNumericValue(gridDeviceNumberTxt.Text.Trim()))
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
                case (int)NotificationDevice.OutboundCallAS:
                    if (gridDeviceNumberTxt.Text.Trim().Length == 0)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter Phone Number\\n";
                    }
                    else if (!Utils.isNumericValue(gridDeviceNumberTxt.Text.Trim()))
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
                generateStep1DataGridHeight();      
                ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "Grid_Alert", "alert('" + errorMessage + "');", true);
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
                dtGroupInfo = group.GetGroupPreferences(int.Parse(Request[GROUP_ID]));

                if (dtGroupInfo.Rows.Count > 0)
                {
                    string tap800No = Utils.flattenPhoneNumber(dtGroupInfo.Rows[0]["TAP800Number"].ToString().Trim());
                    ViewState[TAP_800_NUM] = tap800No;
                }
            }
            catch (Exception ex)
            {
                if (ViewState["SubscriberID"] != null)
                    Tracer.GetLogger().LogInfoEvent("GroupMaintenance_Finding_notificationStep1.deleteSubscriberDevice:: Exception occured for User ID - " + ViewState["SubscriberID"].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState["SubscriberID"]));
                throw ex;
            }
            finally
            {
                group = null;
                dtGroupInfo = null;
            }
        }

        /// <summary>
        /// This function is to calculate dynamic height for grdDevices datagrid
        /// </summary>
        private int getDataGridHeight()
        {
            try
            {
        
                int devicesGridHeight = 28;//Grid height for grdDevices
                int headerHeigth = 31;
                int rowHeight = 21;
                int maxRows = 4;
                if (grdDevices.Items.Count <= maxRows)
                {
                    if (grdDevices.Items.Count != 0)
                        devicesGridHeight = (grdDevices.Items.Count * rowHeight) + headerHeigth;//Grid height for grdDevices
                }
                else
                {
                    devicesGridHeight = (maxRows * rowHeight) + headerHeigth;//Grid height for grdDevices
                }


                return devicesGridHeight + 1;
            }
            catch (Exception ex)
            {
                if (ViewState["SubscriberID"] != null)
                    Tracer.GetLogger().LogInfoEvent("GroupMaintenance_Finding_notificationStep1.generateStep1DataGridHeight:: Exception occured for User ID - " + ViewState["SubscriberID"].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState["SubscriberID"]));
                throw ex;
            }
        }

        /// <summary>
        /// This function is to fill Events available into Notification events drop down list.
        /// This function takes care of Exception handling during this function call.
        /// </summary>
        private void fillEventDDL(DropDownList objDropdown)
        {
            Device device = null;
            DataTable dtNotificationEvents = null;
            try
            {
                device = new Device();
                dtNotificationEvents = device.GetGroupNotificationTypes();
                objDropdown.DataSource = dtNotificationEvents;
                objDropdown.DataBind();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep2 - fillEventDDL: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                device = null;
                dtNotificationEvents = null;
                ListItem listItem = new ListItem("All Events", "-1");
                objDropdown.Items.Add(listItem);
                objDropdown.Items.FindByValue("-1").Selected = true;

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
        /// This function fills Findings into drop down list.
        /// if error occured while filling findings into drop down it is handled into catch block.
        /// </summary>
        private void fillFindingDDL(DropDownList objDropdown)
        {
            Device device = null;
            DataTable dtFindings = null;
            try
            {
                device = new Device();
                dtFindings = device.GetFindingsForGroup(Convert.ToInt32(ViewState[GROUP_ID].ToString()));
                objDropdown.DataSource = dtFindings;
                objDropdown.DataBind();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("gmf_notificationStep2 - fillFindingDDL: ", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                device = null;
                dtFindings = null;

                ListItem listItem = new ListItem("All Findings", "-1");
                objDropdown.Items.Add(listItem);
                objDropdown.Items.FindByValue("-1").Selected = true;

                if (!objDropdown.ID.ToUpper().Equals("CMBFINDINGS"))
                {
                    ListItem objLi1 = new ListItem();
                    objLi1.Text = "-- None --";
                    objLi1.Value = "0";
                    objDropdown.Items.Insert(objDropdown.Items.Count, objLi1);
                }
            }
        }

        /// <summary>
        /// Add 'Add' in grid for edit mode
        /// </summary>
        /// <param name="item"></param>
        private void addLinkToGridInEditMode(DataGridItem item)
        {

            LinkButton lbUpdate = (item.Cells[12].Controls[0]) as LinkButton;
            string lnkButID = lbUpdate.ClientID.Replace("_", "$");

            HtmlAnchor objAddLink = new HtmlAnchor();
            objAddLink.InnerHtml = "Add&nbsp;New";
            objAddLink.ID = "lnkAdd";
            objAddLink.Name = "lnkAdd";
            objAddLink.CausesValidation = false;
            string script = "javascript:__doPostBack('" + lnkButID + "','');";
            objAddLink.HRef = script;
            objAddLink.Attributes.Add("onclick", "javascript:return AddRecordFromGrid();");

            item.Cells[12].Controls.AddAt(2, objAddLink);
            item.Cells[12].Controls.AddAt(3, new LiteralControl("&nbsp;"));

        }

        #endregion Step1

        #endregion Private Methods

    }
}
