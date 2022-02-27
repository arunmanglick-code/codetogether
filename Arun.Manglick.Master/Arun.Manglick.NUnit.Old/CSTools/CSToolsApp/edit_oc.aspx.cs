#region File History

/******************************File History***************************
 * File Name        : edit_rp.aspx.cs
 * Author           : 
 * Created Date     : 
 * Purpose          : Change Details of RP
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 * 02-27-2007       IAK      Added logic for alert while navigating from one page to other                                 
 * 06-21-2007       IAK      Enhnacement 28, 29 30
 * 06-25-2007       IAK      Department and Resident field added, change in save routine.
 * 07-08-2007       IAK      Resident field removed, start and end date is added. Department assignment logic changed.
 * 08-14-2007       IAK      First, last name not allow to enter numeric values   
 * 01-28-2008       SBT      Added the login in the Cancel and Save Button if this page is called from ADD OC module.
 * 03-20-2008       SSK      Added fields for "PIN for Message Retrieval".
 * 05-07-2008       Suhas    Defect 2979: Auto Tab issue.
 * 12 Jun 2008      Prerak   Migration of AJAX Atlas to AJAX RTM 1.0
 *                           Removed Iframe and Merged Notification Step1, Step2, Step3 
 * 19 Jun 2008      Prerak   Defect# 3306 fixed - Notifications: Incorrect Email ID can be saved
 * 20 Jun 2008      Prerak   UI Issue solved     
 * 07 Aug 2008      Prerak   CR #200 OC Profile layout implemented.
 * 07 Aug 2008      Prerak   FR -> Add Cell Phone to OC Profile page implemented.
 * 08 Aug 2008      Prerak   Removed Static datatable for CR#200     
 * 08-08-2008       IAK      Defect 3581
 * 08-19-2008       Suhas    FR#271 SMS with Weblink device Group population 
 * 08-20-2008       Prerak   Defect #3636 -> change email id from error message to email address
 * 08-28-2008       Suhas    Defect # 3659 two warning popups if entered Email Gateways Address is not valid - Fixed
 * 09-09-2008       Prerak   Sharepoint defect #558 "The user can't enter the correct email gateway 
 *                           for a clinical team or unit" fixed
 * 09-10-2008       Prerak   if only number is chaged email gateway updated automatically.
 * 09-20-2008       Prerak   CR #273 Save button display in OC Profile
 * 31-10-2008       sheetal  Remove Pager validations
 * 11-03-2008       Prerak   Defect #4052 Fixed
 * 11-12-2008       Jeeshan  Defect #3660 fixed - System navigates to Technical Error Page if one device is deleted while other is in Edit Mode
 * 11-13-2008       Prerak   Defect #3637 OC Profile Save performance issue   
 * 11-13-2008       Zeeshan  Defect #3164 - User clicks on Edit for a device at the bottom of the list they are forced to scroll to find the device they selected.
 * 11-14-2008       IAK      Defect #3310
 * 11-18-2008       Prerak   Defect #4165 Fixed
 * 20-Nov-2008      IAK      Defect #3113, #4165 Fixed: Handled blank values
 * 11-24-2008       SD       Defect #4225 fixed
 * 11-28-2008       GB       Added clinical team phone to fix TTP #161
 * 12-01-2008       Prerak   Defect #4228 Fixed   
 * 12-03-2008       Prerak   Defect #4281 [CSTools: Edit OC Page]: Devices deletion issue. 
 * 01-05-2009       RG          FR 282 Changes
 * 01-08-2009       RG       Delete approach need to be modified
 * 01-13-2009       RG      Suggestion from Fred
 * 01-15-2009       GB       FR #152 - Clinical team assignment
 * 12-18-2008       ZNK      CR-Multiple External ID support.
 * 01-20-2009       SD       FR 237 implemented
 * ----------------------------------------------------------------------- */
#endregion

#region Using
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
using Vocada.VoiceLink.DataAccess;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.Utilities;
#endregion

namespace Vocada.CSTools
{
    /// <summary>
    /// The purpose of this class is to fetch information for Ordering Clinician for editing
    /// and saving the edited information to the Veriphy database.
    /// </summary>
    public partial class edit_oc : System.Web.UI.Page
    {
        #region Page Variables
        private Hashtable hashParams = new Hashtable();
        int departmentAssignmentID = 0;

        public string TextChanged
        {
            get
            {
                return textChanged.Value;
            }
        }

        private const int ROLE_SPECIALIST = 1;
        private const int DEVICE_FAX = 4;
        private const int DEVICE_PAGER_NUM = 5;
        private const int DEVICE_PAGER_NUMA = 6;
        private const int REPORT_ON_MONDAY = 2;
        private const int REPORT_ON_TUESDAY = 4;
        private const int REPORT_ON_WEDNESDAY = 8;
        private const int REPORT_ON_THURSDAY = 16;
        private const int REPORT_ON_FRIDAY = 32;
        private const int REPORT_ON_SATURDAY = 64;
        private const int REPORT_ON_SUNDAY = 128;
        private int numberOfDevices;
        private bool NoDelete = false;
        private int numberOfNotifications;
        private int numberOfAfterHoursNotifications;

        //Constants for Toggle Button Name
        private const string SHOWDETAILS_BUTTONNAME = "Assign Event";
        private const string HIDEDETAILS_BUTTONNAME = "Hide Event Details";
        private const int GRID_ADJUSTMENT_VALUE = -120;
        private const int GRID_MAX_SIZE = 120;

        #endregion

        #region Protected Fields
        protected string strUserSettings = "NO";
        private const string OCID = "ReferringPhysicianID";
        private const string DEPARTMENT = "Departments";

        string subscriberID;
        protected DataTable dtGridDeviceNotifications;
        protected DataTable dtGridAfterHours;
        protected static int RowNo = 1;
        protected static int AfterHourRowNo = 1;

        protected DataTable dtIdTypes = null;
        protected DataTable dtIdTypesInfo = null;
        #endregion
       
        #region Page_Load
        /// <summary>
        /// This function sets all the Logged in user information, Cell Phone Carrier list, Pager Carrier list and  Frames edit_rpnotification_step1,edit_rpnotification_step3,edit_rpnotification_step3
        /// This load function is to show all the devices list into datgarid for logged  in users.
        /// This load keeps list of all devices into drop down list.
        /// This load generate auto height for datagrids accross the page.
        /// This load put all the Phone carrier and Pager Carrier list.
        /// This function takes care of Error handling portion if any error occurred.
        /// This load function is to show all the Notification Events list into datgarid for logged  in users.
        /// This load keeps list of all Notificaton Events devices into drop down list for logged in users.
        /// This load generate auto height for datagrids accross the page.
        /// This load put all the findings list into drop down list box.
        /// This load function fill all list of Devices available into list box for logged in users.
        /// This function takes care of Error handling portion if any error occurred.
        /// Summary description for EditRPNofictionStep3 page which is loaded on frame into EditRP.
        /// This class function is to show all the Fill After Hours list into datgarid for logged  in users.
        /// This class keeps list of all After Hours devices into drop down list for logged in users.
        /// This class generate auto height for datagrids accross the page.
        /// This class put all the findings list into drop down list box.
        /// This function takes care of Error handling portion if any error occurred.


        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Session[SessionConstants.LOGGED_USER_ID] == null)
                Response.Redirect(Utils.GetReturnURL("default.aspx", "edit_oc.aspx", this.Page.ClientQueryString));

            registerJavascriptVariables();
            if (Session[SessionConstants.DT_NOTIFICATION] == null)
                dtGridDeviceNotifications = new DataTable();
            else
                dtGridDeviceNotifications = (DataTable)Session[SessionConstants.DT_NOTIFICATION];
            if (Session[SessionConstants.DT_AFTERHOUR] == null)
                dtGridAfterHours = new DataTable();
            else
                dtGridAfterHours = (DataTable)Session[SessionConstants.DT_AFTERHOUR];

            if (Session["DT_IDTYPEINFO"] == null)
                dtIdTypesInfo = new DataTable();
            else
                dtIdTypesInfo = (DataTable)Session["DT_IDTYPEINFO"];

            if (Session["DT_IDTYPES"] == null)
                dtIdTypes = new DataTable();
            else
                dtIdTypes = (DataTable)Session["DT_IDTYPES"];

            if (!Page.IsPostBack)
            {

                if (Request[OCID] != null)
                {
                    ViewState[OCID] = Request[OCID];
                    populateDepartmentAssignments(Convert.ToInt32(Request[OCID]));
                }
                loadDepartment();
                try
                {
                    fillPhysicianInfo();
                    getCarriers();

                    dtGridAfterHours.Rows.Clear();
                    dtGridDeviceNotifications.Rows.Clear();
                    //Step1
                    fillDevices();
                    fillDeviceDDL();
                    fillGroupDDL(cmbGroup,null);
                    fillEventDDL(cmbEvents);
                    fillFindingDDL();
                    //end step1

                    //Step2
                    fillAfterHoursDeviceOptions();
                    fillGroupDDLStep3();
                    fillAfterHoursFindingDDL();
                    fillAfterHoursNotifications();
                    //End Step2
                    resetControls();
                    ScriptManager.RegisterStartupScript(upnlSaveBtn, upnlSaveBtn.GetType(), "HideSaveBtn", "document.getElementById('" + btnAdd.ClientID + "').disabled = 'true';", true);

                    populateIdTypes();
                    BindOCExternalIDsInfo(Convert.ToInt32(ViewState["ReferringPhysicianID"]));
                }
                catch (Exception ex)
                {
                    if (Session[SessionConstants.USER_ID] != null)
                        Tracer.GetLogger().LogExceptionEvent("edit_oc.Page_Load():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                    throw;
                }
            }

            txtPrimaryPhoneAreaCode.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtPrimaryPhonePrefix.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtPrimaryPhoneNNNN.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");

            txtFaxAreaCode.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtFaxPrefix.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtFaxNNNN.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtPassword.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");

            /*SSK  -  2006/09/19 - 1.04 - To prvent user to leave page without saving changes.*/
            addAttributesToSaveChanges();

            Session[SessionConstants.CURRENT_TAB] = "MsgCenter";
            Session[SessionConstants.CURRENT_INNER_TAB] = "EditOC";
            Session[SessionConstants.CURRENT_PAGE] = "edit_oc.aspx?ReferringPhysicianID=" + Request[OCID];

            //btnCancel.Attributes.Add("onclick", "return onCancelClick('" + Session["txtSearch"] + "','" + Session["txtSearchValue"] + "');");
            /*SSK  -  2006/08/24 - Phase 2 Iteration 2 - Help Integration*/
            StringBuilder sbClinicReportParams = new StringBuilder();
            sbClinicReportParams.Append("save_clinician_report.aspx?referringPhysicianID=");
            sbClinicReportParams.Append(ViewState[OCID].ToString());
            sbClinicReportParams.Append("&OrderingClinicianName=");
            sbClinicReportParams.Append(txtFirstName.Text + " " + txtLastName.Text);
            sbClinicReportParams.Append("&Specialty=");
            sbClinicReportParams.Append(txtSpecialty.Text);
            sbClinicReportParams.Append("&PracticeGroup=");
            sbClinicReportParams.Append(txtPracticeGroup.Text);
            if (drpDepartment.SelectedValue != "-1")
            {
                lblStar.InnerText = "*:";
            }
            generateDataGridHeight(grdIdTypeInfo, divExternalIDInformation);
        }
        #endregion

        #region Public and Protected methods and events       

        /// <summary>
        /// This function is to Redirect page into Directory page if  cancel button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            string url = "";
            if (Request["DirectoryMaintenance"] == null)
            {
                url = "./message_center.aspx";
            }
            else
            {
                if (Session["txtSearch"] != null && Convert.ToBoolean(Session["txtSearch"]) == true)
                {
                    if (Session["txtSearchValue"] != null)
                    {
                        url = "./directory_maintenance.aspx?Search=" + Session["txtSearchValue"].ToString();
                    }
                    else
                    {
                        url = "./directory_maintenance.aspx?Search=";
                    }
                }
                else
                {
                    url = "./directory_maintenance.aspx?Search=" + txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim();
                }
            }
            ScriptManager.RegisterClientScriptBlock(upnlSaveBtn, upnlSaveBtn.GetType(), "Navigate", "<script type=\'text/javascript\'>Navigate(\"" + url + "\");</script>", false);

        }

        /// <summary>
        /// Clears clinical team, phone, start date and end date assignment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, System.EventArgs e)
        {
            clearCTData();
        }

        /// <summary>
        /// Adds department assigned to OC.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddDept_Click(object sender, System.EventArgs e)
        {
            string errorMessage = "";
            bool isValidStartDate = false;
            bool isValidEndDate = false;
            int departmentID = 0;
            DateTime departmentStartDate;
            DateTime departmentEndDate;
            bool isUpdated = false;

            if (drpDepartment.SelectedValue != "-1")
            {
                if (txtStartDate.Text.Length == 0)
                {
                    errorMessage += "Please enter start date.\\n";
                }
                else if (!isValidDate(txtStartDate.Text))
                {
                    errorMessage += "Please enter valid start date.\\n";
                }
                else
                {
                    if (DateTime.Parse(txtStartDate.Text) < DateTime.Parse(DateTime.Now.ToShortDateString()) && int.Parse(ViewState["DepartmentAssignID"].ToString()) == 0)
                    {
                        errorMessage += "Start date should be greater than current date.\\n";
                    }
                    else
                    {
                        isValidStartDate = true;
                    }
                }


                if (txtEndDate.Text.Length > 0)
                {
                    if (!isValidDate(txtEndDate.Text))
                        errorMessage += "Please enter valid end date.\\n";
                    else
                        isValidEndDate = true;
                }

                if (isValidStartDate && isValidEndDate)
                {
                    if (DateTime.Parse(txtStartDate.Text) > DateTime.Parse(txtEndDate.Text))
                    {
                        errorMessage += "Start date should be less than end date.\\n";
                    }
                }

            }
            if (errorMessage.Length > 0)
            {
                string showMessage = "alert('" + errorMessage + "');";
                if (drpDepartment.SelectedValue == "-1")
                {
                    showMessage += "enableDateSelection('false');";
                }

                ScriptManager.RegisterStartupScript(upnlCT, upnlCT.GetType(), "Navigate", showMessage, true);
                populateDepartmentAssignments(Convert.ToInt32(ViewState[OCID]));
                return;
            }

            try
            {
                if (int.Parse(drpDepartment.SelectedValue) == -1)
                    departmentID = -1;
                else
                    departmentID = int.Parse(drpDepartment.SelectedValue);

                if (txtStartDate.Text.Length > 0)
                    departmentStartDate = DateTime.Parse(txtStartDate.Text);
                else
                    departmentStartDate = DateTime.Parse(DateTime.Now.ToShortDateString());

                if (txtEndDate.Text.Length > 0)
                    departmentEndDate = DateTime.Parse(txtEndDate.Text).Add(new TimeSpan(23, 59, 59));
                else
                    departmentEndDate = DateTime.MinValue;

                if (btnAddDept.Text == "Add")
                    isUpdated = addDepartmentAssignment(departmentID, Convert.ToInt32(ViewState[OCID]), departmentStartDate, departmentEndDate);
                else
                    isUpdated = updateDepartmentAssignment(Convert.ToInt32(ViewState["DepartmentAssignID"]), departmentID, Convert.ToInt32(ViewState[OCID]), departmentStartDate, departmentEndDate);

                if (isUpdated)
                {
                    if (btnAddDept.Text == "Update")
                    {
                        drpDepartment.Items.Insert(0, "None");
                        drpDepartment.Items[0].Value = "-1";
                    }
                    lblPhone.Text = string.Empty;
                    txtStartDate.Text = string.Empty;
                    txtEndDate.Text = string.Empty;
                    drpDepartment.SelectedValue = "-1";
                    drpDepartment.Enabled = true;
                    txtStartDate.Enabled = false;
                    txtEndDate.Enabled = false;
                    ViewState["DepartmentAssignID"] = 0;
                    btnAddDept.Text = "Add";
                    btnAddDept.Enabled = false;
                    hdnCTChanged.Value = "false";
                    if (textChanged.Value == "false")
                        ScriptManager.RegisterStartupScript(upnlSaveBtn, upnlSaveBtn.GetType(), "HideSaveBtn", "document.getElementById(btnAddClientID).disabled = true;", true);
                }
                populateDepartmentAssignments(Convert.ToInt32(ViewState[OCID]));
                upnlDepartmentGrid.Update();
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("EditRP.btnAddDept_Click:Exception occured for User ID - ", "0", ex.Message, ex.StackTrace), 0);
                throw;
            }
        }

        /// <summary>
        /// This function is to Update Contact Information about logged in users.
        /// This function make use of stored procedure "updateReferringPhysician" for updation of contact information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            string faxNumber = txtFaxAreaCode.Text.Trim() + txtFaxPrefix.Text.Trim() + txtFaxNNNN.Text.Trim();
            string errorMessage = "";
            bool isValidStartDate = false;
            bool isValidEndDate = false;

            if (faxNumber.Length != 10 && txtEmail.Text.Trim().Length < 2)
            {
                errorMessage = "You must enter either an Email Address or a Fax Number for Notifications\\n";
            }
            if (drpDepartment.SelectedValue != "-1")
            {
                if (txtStartDate.Text.Trim().Length == 0)
                {
                    errorMessage += "Please enter start date.\\n";
                }
                else if (!isValidDate(txtStartDate.Text.Trim()))
                {
                    errorMessage += "Please enter valid start date.\\n";
                }
                else
                {
                    if (DateTime.Parse(txtStartDate.Text.Trim()) < DateTime.Parse(DateTime.Now.ToShortDateString()) && int.Parse(ViewState["DepartmentAssignID"].ToString()) == 0)
                    {
                        errorMessage += "Start date should be greater than current date.\\n";
                    }
                    else
                    {
                        isValidStartDate = true;
                    }
                }


                if (txtEndDate.Text.Trim().Length > 0)
                {
                    if (!isValidDate(txtEndDate.Text.Trim()))
                        errorMessage += "Please enter valid end date.\\n";
                    else
                        isValidEndDate = true;
                }

                if (isValidStartDate && isValidEndDate)
                {
                    if (DateTime.Parse(txtStartDate.Text.Trim()) > DateTime.Parse(txtEndDate.Text.Trim()))
                    {
                        errorMessage += "Start date should be less than end date.\\n";
                    }
                }

            }
            if (errorMessage.Length > 0)
            {
                string showMessage = "<script type=\'text/javascript\'>alert('" + errorMessage + "');";
                if (departmentAssignmentID == 0 && drpDepartment.SelectedValue == "-1")
                {
                    showMessage += "enableDateSelection('false');";
                }
                showMessage += "</script>";

                ScriptManager.RegisterClientScriptBlock(upnlSaveBtn, upnlSaveBtn.GetType(), "Navigate", showMessage, false);

                return;
            }
            if (!validateBeforeSave())
            {
                string showMessage = "<script type=\'text/javascript\'>";
                if (departmentAssignmentID == 0 && drpDepartment.SelectedValue == "-1")
                {
                    showMessage += "enableDateSelection('false');";
                }
                showMessage += "</script>";

                ScriptManager.RegisterClientScriptBlock(upnlSaveBtn, upnlSaveBtn.GetType(), "Navigate", showMessage, false);

                return;
            }
            // all other data should be good, updating referring physician...
            OrderingClinician objOC = new OrderingClinician();
            try
            {
                string existingPIN = string.Empty;
                if (ViewState["ExistingPIN"] != null)
                    existingPIN = ViewState["ExistingPIN"].ToString();

                if (objOC.CheckDuplicateOCPIN(txtLoginID.Text.Trim(), txtPassword.Text.Trim(), Convert.ToInt32(ViewState["VOCUserID"].ToString())))
                {
                    ScriptManager.RegisterStartupScript(upnlSaveBtn, upnlSaveBtn.GetType(), "PinError", "alert('This PIN has been already used by another OC.');document.getElementById('" + txtPassword.ClientID + "').value='';", true);
                }
                else if (String.Compare(existingPIN, txtPinForMessage.Text.Trim()) != 0 && txtPinForMessage.Text.Trim().Length > 0 && Utility.CheckDuplicatePINForUser(Convert.ToInt32(ViewState["InstitutionID"].ToString()), txtPinForMessage.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(upnlSaveBtn, upnlSaveBtn.GetType(), "PinError", "alert('This PIN is already in use. Please enter another PIN.');document.getElementById('" + txtPinForMessage.ClientID + "').value='';", true);
                }
                else
                {
                    OrderingClinicianInfo objOCInfo = new OrderingClinicianInfo();
                    objOCInfo.ReferringPhysicianID = int.Parse(ViewState[OCID].ToString());
                    objOCInfo.Active = cbActive.Checked;
                    objOCInfo.FirstName = txtFirstName.Text.Trim();
                    objOCInfo.LastName = txtLastName.Text.Trim();
                    objOCInfo.NickName = txtNickname.Text.Trim();
                    objOCInfo.LoginID = txtLoginID.Text.Trim();
                    objOCInfo.Password = txtPassword.Text.Trim();
                    objOCInfo.PrimaryPhone = txtPrimaryPhoneAreaCode.Text.Trim() + txtPrimaryPhonePrefix.Text.Trim() + txtPrimaryPhoneNNNN.Text.Trim();
                    objOCInfo.PrimaryEmail = txtEmail.Text.Trim();
                    objOCInfo.Fax = txtFaxAreaCode.Text.Trim() + txtFaxPrefix.Text.Trim() + txtFaxNNNN.Text.Trim();
                    objOCInfo.Specialty = txtSpecialty.Text.Trim();
                    objOCInfo.Affiliation = txtHospitalAffiliation.Text.Trim();
                    objOCInfo.PracticeGroup = txtPracticeGroup.Text.Trim();
                    objOCInfo.Address1 = txtAddress1.Text.Trim();
                    objOCInfo.Address2 = txtAddress2.Text.Trim();
                    objOCInfo.Address3 = txtAddress3.Text.Trim();
                    objOCInfo.City = txtCity.Text.Trim();
                    objOCInfo.State = txtState.Text.Trim();
                    objOCInfo.Zip = txtZip.Text.Trim();
                    objOCInfo.AdditionalContName = txtName.Text.Trim();
                    objOCInfo.AdditionalContPhone = txtPhoneCode.Text.Trim() + txtPhonePrefix.Text.Trim() + txtPhoneNumber.Text.Trim();
                    objOCInfo.UpdatedBy = Convert.ToInt32(Session[SessionConstants.USER_ID]);
                    objOCInfo.RadiologyTDR = chkRadiologyTDR.Checked;
                    objOCInfo.LabTDR = chkLabTDR.Checked;
                    objOCInfo.Notes = txtNotes.Text.Trim();
                    objOCInfo.ProfileCompleted = chkProfileCompleted.Checked;
                    objOCInfo.IsResident = false;////chkResident.Checked;
                    objOCInfo.CellPhone = txtCellAreaCode.Text.Trim() + txtCellPrefix.Text.Trim() + txtCellNNNN.Text.Trim();

                    if (int.Parse(drpDepartment.SelectedValue) == -1)
                        objOCInfo.DepartmentID = -1;
                    else
                        objOCInfo.DepartmentID = int.Parse(drpDepartment.SelectedValue);

                    if (txtStartDate.Text.Trim().Length > 0)
                        objOCInfo.DepartmentStartDate = DateTime.Parse(txtStartDate.Text.Trim());
                    else
                        objOCInfo.DepartmentStartDate = DateTime.Parse(DateTime.Now.ToShortDateString());

                    if (txtEndDate.Text.Trim().Length > 0)
                        objOCInfo.DepartmentEndDate = DateTime.Parse(txtEndDate.Text.Trim()).Add(new TimeSpan(23, 59, 59));
                    else
                        objOCInfo.DepartmentEndDate = DateTime.MinValue;

                    if (ViewState["DepartmentAssignID"] != null)
                        objOCInfo.DepartmentAssignID = int.Parse(ViewState["DepartmentAssignID"].ToString());
                    else
                        objOCInfo.DepartmentAssignID = 0;

                    departmentAssignmentID = objOCInfo.DepartmentAssignID;
                    objOCInfo.IsEDDoc = chkEDDoc.Checked;
                    objOCInfo.InstituteID = Convert.ToInt32(ViewState["InstitutionID"].ToString());
                    objOCInfo.VOCUserID = Convert.ToInt32(ViewState["VOCUserID"].ToString());

                    bool pinVisible = pnlMessageRetieve.Visible;

                    string pin = string.Empty;
                    if (pinVisible && txtPinForMessage.Text.Trim().Length > 0)
                        pin = txtPinForMessage.Text.Trim();
                    else if (!pinVisible && existingPIN.Trim().Length > 0)
                        pin = existingPIN.Trim();

                    objOCInfo.PINForMessageRetrieve = pin;

                    DataTable dtblExternalIDInfo = null;
                    if (Session["DT_IDTYPEINFO"] != null)
                    {
                        dtblExternalIDInfo = (DataTable)(Session["DT_IDTYPEINFO"]);
                        dtblExternalIDInfo.TableName = "ExternalIDTable";
                    }
                    int result = objOC.UpdateOrderingClinician(objOCInfo, dtblExternalIDInfo);

                    editDevicesAndAfterHours();

                    if (result < 0)
                    {
                        if (result == -2)
                        {
                            ScriptManager.RegisterClientScriptBlock(upnlSaveBtn, upnlSaveBtn.GetType(), "UpdateException", "<script type=\'text/javascript\'>alert('External ID Information matches with other Ordering Clinician');</script>", false);
                        }
                        else
                        {
                            string message = "";
                            message = @"The entered assignment dates overlaps with following existing assignment.\n";
                            message += objOC.getOverlappedAssignments(objOCInfo.DepartmentAssignID, objOCInfo.DepartmentID, objOCInfo.ReferringPhysicianID, objOCInfo.DepartmentStartDate, objOCInfo.DepartmentEndDate);
                            message += @"You must eliminate the overlap to save the assignment.";
                            ScriptManager.RegisterClientScriptBlock(upnlSaveBtn, upnlSaveBtn.GetType(), "ErrorMsg", "<script type=\'text/javascript\'>alert('" + message + "');</script>", false);
                        }
                    }
                    else
                    {
                        textChanged.Value = "false";
                        string url = "";
                        if (Request["DirectoryMaintenance"] != null)
                        {
                            url = "./directory_maintenance.aspx?Search=" + txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim();
                        }
                        else
                        {
                            url = "./message_center.aspx";
                        }
                        ScriptManager.RegisterClientScriptBlock(upnlSaveBtn, upnlSaveBtn.GetType(), "Navigate", "<script type=\'text/javascript\'>Navigate(\""  + url + "\");</script>", false);
                    }
                }
            }
            catch (Exception ex)
            {
                Tracer.GetLogger().LogExceptionEvent("EditRP.btnAdd_Click():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }

        }

        /// <summary>
        /// This event will hide Login related fields when the EDDoc checkbox is unchecked, and will show these fields when checkbox is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkEDDoc_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEDDoc.Checked)
            {
                pnlOCLogin.Visible = true;
            }
            else
            {
                pnlOCLogin.Visible = false;
            }
        }

        /// <summary>
        /// Bind Data to grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDepartmentAssignment_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            try
            {
                string script = "javascript:";
                DataRowView data = e.Item.DataItem as DataRowView;

                LinkButton lbtnDelete = (LinkButton)e.Item.Cells[7].FindControl("lnkDelete");                


                if (lbtnDelete != null)
                {
                    bool isActiveAssignment = (data["StartDateTime"].ToString() == DateTime.Now.ToString("MM/dd/yyyy"));   
                    lbtnDelete.Attributes.Add("onclick", "javascript:return ConformBeforeCTDelete('"+isActiveAssignment+"');");
                }
                
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    int maxDataDisplayLenght = 20;

                    if (data["DepartmentName"] != System.DBNull.Value)
                    {
                        if (data["DepartmentName"].ToString().Length > maxDataDisplayLenght)
                        {
                            e.Item.Cells[3].Text = data["DepartmentName"].ToString().Substring(0, maxDataDisplayLenght - 3) + "...";
                            e.Item.Cells[3].ToolTip = data["DepartmentName"].ToString();
                        }
                        else
                        {
                            e.Item.Cells[3].Text = data["DepartmentName"].ToString();
                            e.Item.Cells[3].ToolTip = data["DepartmentName"].ToString();
                        }
                    }

                    e.Item.Cells[4].Text = data["StartDateTime"].ToString();
                    e.Item.Cells[5].Text = data["EndDateTime"].ToString();

                    LinkButton lbtnEdit = (e.Item.Cells[6].Controls[0]) as LinkButton;
                    script += "return ChangeFlagforGrid();"; ;
                    lbtnEdit.OnClientClick = script;
                }
            }
            catch (Exception objException)
            {
                if (ViewState["SubscriberID"] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grdDepartmentAssignment_ItemDataBound", ViewState["SubscriberID"].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(ViewState["SubscriberID"]));
                }
                throw objException;
            }
        }

        /// <summary>
        /// Edits the record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDepartmentAssignment_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ListItem li;

            if (drpDepartment.Items[0].Value == "-1")
                drpDepartment.Items.RemoveAt(0);

            li = drpDepartment.Items.FindByText(e.Item.Cells[3].ToolTip);
            drpDepartment.SelectedValue = li.Value;
            txtStartDate.Text = e.Item.Cells[4].Text.ToString();
            txtEndDate.Text = e.Item.Cells[5].Text.ToString();
            ViewState["DepartmentAssignID"] = e.Item.Cells[0].Text.ToString();

            DataTable dtDepartments = (DataTable)ViewState[DEPARTMENT];
            DataRow[] selectRow = dtDepartments.Select("DepartmentID = '" + drpDepartment.SelectedValue + "'");
            if (selectRow[0]["PhoneNumber"] != System.DBNull.Value)
            {
                string phone = Utils.expandPhoneNumber((string)selectRow[0]["PhoneNumber"]);
                if (selectRow[0]["Extn"] != System.DBNull.Value)
                {
                    phone += " Extn." + (string)selectRow[0]["Extn"];
                }
                lblPhone.Text = phone;
            }
            else
                lblPhone.Text = String.Empty;

            dtDepartments = null;

            btnAddDept.Text = "Update";
            btnAddDept.Enabled = true;
            upnlCT.Update();
            populateDepartmentAssignments(Convert.ToInt32(ViewState[OCID]));
            if (txtStartDate.Text == DateTime.Now.ToString("MM/dd/yyyy"))
            {
                drpDepartment.Enabled = false;
                txtStartDate.Enabled = false;
                txtEndDate.Enabled = true;
            }
            else
            {
                drpDepartment.Enabled = true;
                txtStartDate.Enabled = true;
                txtEndDate.Enabled = true;
            }

            if (strUserSettings == "NO")
                e.Item.BackColor = Color.FromName("#ffffcc");
            else
                e.Item.BackColor = Color.FromName("#AAAAAA");
        }

        /// <summary>
        /// Delete current record.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDepartmentAssignment_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                int intGridEditItemIndex = e.Item.ItemIndex;

                int shiftID = int.Parse(grdDepartmentAssignment.Items[intGridEditItemIndex].Cells[0].Text);
                OrderingClinician objOC = new OrderingClinician();
                int result = objOC.DeleteAssignment(shiftID);
                if (result == 0)
                {
                    ScriptManager.RegisterStartupScript(upnlDepartmentGrid, upnlDepartmentGrid.GetType(), "Grid_validation", "alert('You can not delete current Assignment');", true);

                }
                grdDepartmentAssignment.EditItemIndex = -1;

                if (e.Item.Cells[2].Text == drpDepartment.SelectedValue && e.Item.Cells[4].Text.ToString() == txtStartDate.Text)
                {
                    clearCTData();
                    upnlCT.Update();
                }

                populateDepartmentAssignments(Convert.ToInt32(ViewState[OCID]));
            }
            catch (Exception objException)
            {
                if (ViewState["SubscriberID"] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grdDepartmentAssignment_DeleteCommand", ViewState["SubscriberID"].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(ViewState["SubscriberID"]));
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
                OrderingClinician objOrderingClinician = new OrderingClinician();
                txtPassword.Text = objOrderingClinician.GetNewPin(Convert.ToInt32(ViewState[OCID].ToString()));
                textChanged.Value = "true";
                objOrderingClinician = null;
            }
            catch (Exception ex)
            {
                if (ViewState[OCID] != null && ViewState[OCID].ToString().Trim().Length > 0)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("EditRP.btnGeneratePassword_Click:Exception occured for User ID -  ", ViewState[OCID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(ViewState[OCID].ToString()));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("EditRP.btnGeneratePassword_Click:Exception occured for User ID - ", "0", ex.Message, ex.StackTrace), 0);
                }
                ScriptManager.RegisterClientScriptBlock(upnlSaveBtn, upnlSaveBtn.GetType(), "Warning", "<script type=\"text/javascript\">alert('Error Generating Password.');</script>", false);
            }
        }

        /// <summary>
        /// This function is to generate unique PIN randomly for logged in user.
        /// This function calls stored procedure "VOC_CST_getUniquePINForUser"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPinForMessage_Click(object sender, System.EventArgs e)
        {
            try
            {
                txtPinForMessage.Text = Utility.GetNewPinForMessageRetrieve(Convert.ToInt32(ViewState["InstitutionID"].ToString()));
                //txtChanged.Value = "true";
            }
            catch (Exception ex)
            {
                if (ViewState[OCID] != null && ViewState[OCID].ToString().Trim().Length > 0)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("EditRP.btnPinForMessage_Click:Exception occured for User ID -  ", ViewState[OCID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(ViewState[OCID].ToString()));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("EditRP.btnPinForMessage_Click:Exception occured for User ID - ", "0", ex.Message, ex.StackTrace), 0);
                }

                ScriptManager.RegisterClientScriptBlock(upnlSaveBtn, upnlSaveBtn.GetType(), "Warning", "<script type=\"text/javascript\">alert('Error Generating Password.');</script>", false);
            }
            finally
            {
                ScriptManager.RegisterClientScriptBlock(upnlSaveBtn, upnlSaveBtn.GetType(), "Warning", "<script type=\"text/javascript\">document.getElementById(txtPinForMessageClientID).focus();</script>", false);
            }
        }

        /// <summary>
        /// Customize Item Bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCTDevices_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                DataRowView data = (DataRowView)e.Item.DataItem;
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
                {
                    if (e.Item.ItemType != ListItemType.EditItem)
                    {
                        if ((int)data.Row["FindingID"] == -1)
                        {
                            e.Item.Cells[6].Text = "All Findings";
                        }

                    }
                    if (data.Row["GroupID"].ToString() == "" || (int)data.Row["GroupID"] == -1)
                    {
                            e.Item.Cells[1].Text = "All Groups";
                    }

                }

            }
            catch (Exception ex)
            {
                if (Session["SubscriberID"].ToString().Length > 0)
                    Tracer.GetLogger().LogExceptionEvent("dept_devices.dgDevices_ItemDataBound AS-->" + ex.Message + " AT--->" + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));

                throw ex;
            }

        }

        #region Step11
        /// <summary>
        /// Displays or hides the controls to add device as per the device selected in the dropdown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int deviceID;

            try
            {
                if (grdDevices.EditItemIndex == -1)
                {
                    fillGroupDDL(cmbGroup,null);
                    fillFindingDDL();

                    deviceID = int.Parse(cmbDeviceType.SelectedItem.Value);

                    //getDeptNotificationPreferences(Convert.ToInt32(dlistDeptName.SelectedValue));

                    setLabelsAndInputBoxes(deviceID);
                    if (cmbDeviceType.Items.Count > 0)
                    {
                        if (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerTAP || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerTAPA)
                        {
                            txtNumAddress.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes(textChangedClientID);");
                            //txtNumAddress.MaxLength = 6;
                        }
                        else if ((int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerNumSkyTel || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerNumUSA || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerNumRegular || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.pagerPartner))
                        {
                            txtNumAddress.Attributes.Add("onkeyPress", "JavaScript:return PagerValidationWithSpace('" + txtNumAddress.ClientID + "');");
                            //txtNumAddress.MaxLength = 100;
                        }
                        else if (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerAlpha)
                        {
                            txtNumAddress.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes(textChangedClientID);");
                            //txtNumAddress.MaxLength = 100;
                        }
                        else if (int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.EMail)
                        {
                            txtNumAddress.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes(textChangedClientID);");
                            txtNumAddress.MaxLength = 10;
                        }
                        else
                        {
                            txtNumAddress.Attributes.Add("onkeyPress", "return true");
                            txtNumAddress.Attributes.Add("onchange", "return true");
                            txtNumAddress.MaxLength = 100;
                        }
                        if (int.Parse(cmbDeviceType.SelectedItem.Value) == NotificationDevice.OutboundCallAS.GetHashCode())
                        {
                            txtInitialPause.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokesOrDot(textChangedClientID);");
                        }
                        if (int.Parse(cmbDeviceType.SelectedItem.Value) == NotificationDevice.PagerTAP.GetHashCode() || int.Parse(cmbDeviceType.SelectedItem.Value) == NotificationDevice.PagerTAPA.GetHashCode())
                        {
                            txtEmailGateway.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes(textChangedClientID);");
                            //txtEmailGateway.MaxLength = 10;
                        }
                        else
                        {
                            txtEmailGateway.Attributes.Add("onkeypress", "JavaScript:return true;");
                            txtEmailGateway.MaxLength = 100;
                        }
                    }
                    if (deviceID == NotificationDevice.SMS.GetHashCode() || deviceID == NotificationDevice.PagerAlpha.GetHashCode() || deviceID == NotificationDevice.SMS_WebLink.GetHashCode())  // cell or pager.
                        generateGatewayAddress();
                    generateDataGridHeight("DeviceType");
                }
                else
                {
                    generateDataGridHeight("Edit");
                    cmbDeviceType.SelectedValue = "-1";
                }

                if(grdDevices.EditItemIndex != -1)
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
        /// Populates phone number for the selected department
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void drpDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpDepartment.SelectedValue == "-1")
            {
                lblPhone.Text = string.Empty;
                txtStartDate.Text = string.Empty;
                txtEndDate.Text = string.Empty;
                txtStartDate.Enabled = false;
                txtEndDate.Enabled = false;
                btnAddDept.Enabled = false;
            }
            else
            {
                if (!drpDepartment.Enabled && (txtStartDate.Text == DateTime.Now.ToString("MM/dd/yyyy")))
                    txtStartDate.Enabled = false;
                else
                    txtStartDate.Enabled = true;

                txtEndDate.Enabled = true;
                btnAddDept.Enabled = true;
                hdnCTChanged.Value = "true";
                ScriptManager.RegisterStartupScript(upnlSaveBtn, upnlSaveBtn.GetType(), "HideSaveBtn", "document.getElementById(btnAddClientID).disabled = false;", true);
            }

            if (ViewState[DEPARTMENT] != null)
            {
                DataTable dtDepartments = (DataTable)ViewState[DEPARTMENT];
                DataRow[] selectRow = dtDepartments.Select("DepartmentID = '" + drpDepartment.SelectedValue + "'");
                if (selectRow[0]["PhoneNumber"] != System.DBNull.Value)
                {
                    string phone = Utils.expandPhoneNumber((string)selectRow[0]["PhoneNumber"]);
                    if (selectRow[0]["Extn"] != System.DBNull.Value)
                    {
                        phone += " Extn." + (string)selectRow[0]["Extn"];
                    }
                    lblPhone.Text = phone;
                }
                else
                    lblPhone.Text = String.Empty;

                dtDepartments = null;
            }
        }

        /// <summary>
        /// This event handles Group Dropdown Change Event of Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dlistGridGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                generateDataGridHeight("updatedevice");
                int selectedGroupID = Convert.ToInt32((sender as DropDownList).SelectedValue);
                DropDownList dlFindings = (DropDownList)grdDevices.Items[grdDevices.EditItemIndex].Cells[6].FindControl("dlistGridFindings");

                fillGridFindingDDL(selectedGroupID, dlFindings, "0");

                if (grdDevices.EditItemIndex != -1)
                    addLinkToGridInEditMode(grdDevices.Items[grdDevices.EditItemIndex]);     

            }
            finally
            {
            }
        }

        /// <summary>
        /// Fill Findings for selected Group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbGroup.SelectedValue.Length != 0)
                    fillFindingDDL();
                generateDataGridHeight("cmbGroup");
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID].ToString().Length > 0)
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.cmbGroup_SelectedIndexChanged:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
        }
        /// <summary>
        /// This function is to GenerateGateWayAddress for Phone Carriers selected into Device list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCarrier_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cmbCarrier.SelectedItem.Value.Equals("-1"))
            {
                txtEmailGateway.Text = "";
                return;
            }

            generateGatewayAddress();
            generateDataGridHeight("cmbcarrie");
        }
        protected void cmbEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            textChanged.Value = "true";
        }
        /// <summary>
        /// Update Device Information
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDevices_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            textChanged.Value = "true";
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
                DropDownList cmbGrdFindings = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[6].FindControl("dlistGridFindings")));
                DropDownList cmbGrdGroups = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[6].FindControl("dlistGridGroups")));


                int deviceID = (e.Item.Cells[10].Text.Trim().Length > 0) ? Convert.ToInt32(e.Item.Cells[10].Text.Trim()) : 0;

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

                if (deviceName.StartsWith("PagerA_") || deviceName.StartsWith("SMS_") || deviceName.StartsWith("SMSWebLink_"))
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
                string strEvent = (rpNotifyEventID == 0) ? "" : cmbGrdEvents.SelectedItem.Text;

                //Set FindingID property value   
                int findingID = Convert.ToInt32(cmbGrdFindings.SelectedValue);
                string strFinding = (findingID == 0) ? "" : cmbGrdFindings.SelectedItem.Text;

                //Set GroupID property value   
                int groupID = Convert.ToInt32(cmbGrdGroups.SelectedValue);
                string strGroupName = (groupID == 0) ? "" : cmbGrdGroups.SelectedItem.Text;

                //Set DeptDeviceID property value   
                int rowID = Convert.ToInt32(grdDevices.DataKeys[e.Item.ItemIndex]);

                if (gridInitialPauseTxt.Visible == true)
                    initialPause = Convert.ToDecimal(gridInitialPauseTxt.Text.Trim());
                else
                    initialPause = 0;

                if (isAddClicked == "false")
                {
                    string rpDeviceID = e.Item.Cells[16].Text.Trim();
                    DataRow[] editrow = dtGridDeviceNotifications.Select("DeviceName = '" + deviceName + "' AND RowID <>" + rowID +" AND RPDeviceID <>"+ rpDeviceID);

                    if (editrow.GetLength(0) == 0)
                    {
                        DataRow[] otherDeviceRows = dtGridDeviceNotifications.Select("DeviceName = '" + hdnOldDeviceName.Value + "' AND FlagModified <> 'Deleted' AND RowID <>" + rowID + " AND RPDeviceID =" + rpDeviceID);
                        string flagModified = "Changed";
                        foreach (DataRow drow in otherDeviceRows)
                        {
                            drow.BeginEdit();
                            drow["DeviceName"] = deviceName.Trim();
                            drow["DeviceAddress"] = deviceAddress.Trim();
                            drow["Gateway"] = strGridGateway.Trim();
                            drow["Carrier"] = strGridCarrier;
                            drow["InitialPause"] = initialPause;
                            drow["FlagModified"] = flagModified;
                            drow.EndEdit();
                            drow.AcceptChanges();
                        }

                        DataRow[] currentRow = dtGridDeviceNotifications.Select("DeviceName = '" + hdnOldDeviceName.Value + "' AND FlagModified <> 'Deleted' AND RowID =" + rowID + " AND RPDeviceID =" + rpDeviceID);

                        if (currentRow.Length >= 0)
                        {                            
                            currentRow[0].BeginEdit();
                            if (otherDeviceRows.Length > 0 && findingID == 0 && groupID == 0 && rpNotifyEventID == 0)
                            {
                                currentRow[0]["FlagModified"] = "Deleted";
                            }
                            else
                            {
                                currentRow[0]["DeviceName"] = deviceName.Trim();
                                currentRow[0]["DeviceAddress"] = deviceAddress.Trim();
                                currentRow[0]["Gateway"] = strGridGateway.Trim();
                                currentRow[0]["Carrier"] = strGridCarrier;
                                currentRow[0]["EventDescription"] = strEvent;
                                currentRow[0]["RPNotifyEventID"] = rpNotifyEventID;
                                currentRow[0]["FindingDescription"] = strFinding;
                                currentRow[0]["FindingID"] = findingID;
                                currentRow[0]["GroupID"] = groupID;
                                currentRow[0]["GroupName"] = strGroupName;
                                currentRow[0]["InitialPause"] = initialPause;
                                currentRow[0]["FlagModified"] = flagModified;
                            }
                            currentRow[0].EndEdit();
                            currentRow[0].AcceptChanges();
                        }

                        Session[SessionConstants.DT_NOTIFICATION] = dtGridDeviceNotifications;
                        grdDevices.EditItemIndex = -1;
                        dataBindStep1();
                        script = "Device has been updated.";
                        //Update After Hour grid if device name is changed.
                        if (hdnOldDeviceName.Value != deviceName)
                        {
                            DataRow[] drAH = dtGridAfterHours.Select("DeviceName = '" + hdnOldDeviceName.Value + "' AND FlagModified in ('Unchanged', 'Changed')");
                            if (drAH.Length > 0)
                            {
                                int numAH = drAH.Length;
                                for (int countAH = 0; countAH < numAH; countAH++)
                                {
                                    drAH[countAH].BeginEdit();                                    
                                    drAH[countAH]["DeviceName"] = deviceName;
                                    drAH[countAH].EndEdit();
                                    drAH[countAH].AcceptChanges();
                                    Session[SessionConstants.DT_AFTERHOUR] = dtGridAfterHours;
                                }
                            }
                            drAH = null;
                        }
                        hdnOldDeviceName.Value = "";
                        fillAfterHoursDeviceOptions();
                        ViewState[Constants.DEVICE_ADDRESS] = null;
                        ViewState[Constants.EMAIL_GATEWAY] = null;
                        dataBindStep2();
                        upnlStep3.Update();
                    }
                    else
                    {
                        script = "Device Name already exists";
                        addLinkToGridInEditMode(grdDevices.Items[grdDevices.EditItemIndex]);
                  }
                }
                else
                {
                    RowNo++;
                    string flagModified = "UnChanged";
                    string flagRowType = "New";
                    DataRow dtrow = dtGridDeviceNotifications.NewRow();
                    dtrow["DeviceName"] = generateDeviceName(deviceID);
                    dtrow["DeviceAddress"] = deviceAddress.Trim();
                    dtrow["Gateway"] = strGridGateway.Trim();
                    dtrow["Carrier"] = strGridCarrier;
                    dtrow["GroupName"] = strGroupName;
                    dtrow["EventDescription"] = strEvent;
                    dtrow["FindingDescription"] = strFinding;
                    dtrow["InitialPause"] = initialPause;
                    dtrow["DeviceID"] = deviceID;
                    dtrow["GroupID"] = groupID;
                    dtrow["RPNotificationID"] = 0;
                    dtrow["RPNotifyEventID"] = rpNotifyEventID;
                    dtrow["FindingID"] = findingID;
                    dtrow["RowID"] = RowNo;
                    dtrow["FlagRowType"] = flagRowType;
                    dtrow["FlagModified"] = flagModified;
                    dtrow["RPID"] = Convert.ToInt32(ViewState[OCID]);
                    //dtrow["RPDeviceID"] = RowNo;

                    script = "Device has been added.";
                    dtGridDeviceNotifications.Rows.Add(dtrow);

                    Session[SessionConstants.DT_NOTIFICATION] = dtGridDeviceNotifications;
                    
                    grdDevices.EditItemIndex = -1;
                    dataBindStep1();

                    hdnOldDeviceName.Value = "";
                    fillAfterHoursDeviceOptions();
                    ViewState[Constants.DEVICE_ADDRESS] = null;
                    ViewState[Constants.EMAIL_GATEWAY] = null;
                    dataBindStep2();
                    upnlStep3.Update();
                }


                generateDataGridHeight("updatedevice");
                ScriptManager.RegisterStartupScript(upnlHidden, upnlHidden.GetType(), "deviceExists", "alert('" + script + "');SetPostbackVarFalse();", true);
  
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
        /// Customize Item Bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDevices_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblDeviceNumber = (Label)e.Item.FindControl("lblGridDeviceNumber");
                    Label lblEmailGateway = (Label)e.Item.FindControl("lblGridDeviceEmailGateway");

                    if (lblDeviceNumber.Text.Length > 30)
                    {
                        lblDeviceNumber.ToolTip = lblDeviceNumber.Text;
                        lblDeviceNumber.Text = lblDeviceNumber.Text.Substring(0, 30) + "....";
                    }
                    if (lblEmailGateway.Text.Length > 30)
                    {
                        lblEmailGateway.ToolTip = lblEmailGateway.Text;
                        lblEmailGateway.Text = lblEmailGateway.Text.Substring(0, 30) + "....";
                    }

                }
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
                    if ((data.Row["GroupID"].ToString() == "" || (int)data.Row["GroupID"] == -1) && e.Item.ItemType != ListItemType.EditItem)
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
                        if (e.Item.ItemType != ListItemType.EditItem)
                        {
                            gridGroupLabel.Text = data.Row["GroupName"].ToString();
                            gridGroupLabel.ToolTip = data.Row["GroupName"].ToString();
                        }
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
        /// Set the EditItemIndex of grdDevices to the selected index, calls 
        /// editDeviceGrid() method to fill Event, Findings drop downs to make row editable.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDevices_EditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                //dlistDeviceType.SelectedValue = "-1";
                int intGridEditItemIndex = e.Item.ItemIndex;
                string deviceName = ((Label)(grdDevices.Items[intGridEditItemIndex].Cells[1].FindControl("lblGridDeviceNumber"))).Text.Trim();
                string deviceGatway = ((Label)(grdDevices.Items[intGridEditItemIndex].Cells[4].FindControl("lblGridDeviceEmailGateway"))).Text.Trim();
                string strEventText = ((Label)(grdDevices.Items[intGridEditItemIndex].Cells[5].FindControl("lblGridDeviceEvent"))).Text.Trim();
                string deviceType = ((Label)(grdDevices.Items[e.Item.ItemIndex].Cells[0].FindControl("lblGridDeviceType"))).Text.Trim();
                string strFindingText = "";
                if (grdDevices.Items[intGridEditItemIndex].Cells[6].FindControl("lblGridDeviceFinding") != null)
                    strFindingText = ((Label)(grdDevices.Items[intGridEditItemIndex].Cells[6].FindControl("lblGridDeviceFinding"))).Text.Trim();
                string findingID = grdDevices.Items[intGridEditItemIndex].Cells[12].Text.Trim();
                string eventID = grdDevices.Items[intGridEditItemIndex].Cells[13].Text.Trim();
                grdDevices.EditItemIndex = intGridEditItemIndex;
                editDeviceGrid(intGridEditItemIndex, eventID, findingID, deviceName, deviceGatway, int.Parse(grdDevices.Items[intGridEditItemIndex].Cells[11].Text.Trim()));
                hdnOldDeviceName.Value = deviceType;
                resetControls();
                textChanged.Value = "true";
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
        /// Sets the EditItemIndex of grdDevices to -1 and undo the changes of update operation. 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDevices_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                textChanged.Value = "true";
                hdnGridChanged.Value = "false";
                hdnIsAddClicked.Value = "false";
                grdDevices.EditItemIndex = -1;
                dataBindStep1();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.grdDevices_CancelCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
        }

        /// <summary>
        /// Delete Device
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDevices_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                int rpnotificationID;
                textChanged.Value = "true";
                hdnGridChanged.Value = "false";
                string deviceName = "";
                if (grdDevices.EditItemIndex == -1)
                {
                    Label gridDeviceNameLabel;
                    gridDeviceNameLabel = ((Label)(e.Item.Cells[0].FindControl("lblGridDeviceType")));
                    deviceName = gridDeviceNameLabel.Text.Trim();
                    rpnotificationID = Convert.ToInt32(e.Item.Cells[17].Text);
                }
                else
                {
                    TextBox gridDevicenameText;
                    gridDevicenameText = (TextBox)(e.Item.Cells[0].FindControl("txtGridDeviceType"));
                    if (gridDevicenameText == null)
                    {
                        Label gridDeviceNameLabel;
                        gridDeviceNameLabel = ((Label)(e.Item.Cells[0].FindControl("lblGridDeviceType")));
                        deviceName = gridDeviceNameLabel.Text.Trim();
                    }
                    else
                    {
                        deviceName = gridDevicenameText.Text.Trim();
                    }
                    rpnotificationID = Convert.ToInt32(e.Item.Cells[17].Text);                   
                }
                
                DataRow[] checkRow = dtGridAfterHours.Select("DeviceName = '" + deviceName + "' AND FlagModified in ('Unchanged','Changed')");
                if (checkRow.GetLength(0) == 0) //No After Hour Notification
                {
                    //delete the notification
                    deleteDeviceNotification(deviceName,rpnotificationID);
                }
                else
                {
                    //After Hour Notification attached
                    DataRow[] deviceRows = dtGridDeviceNotifications.Select("DeviceName = '" + deviceName + "' AND FlagModified in ('Unchanged','Changed')");
                    if (deviceRows.Length > 1) //More than One Notification attached
                    {
                        //Simply delete the notification
                        deleteDeviceNotification(deviceName,rpnotificationID);
                    }
                    else
                    {
                        //Display error message
                        if (grdDevices.EditItemIndex != -1)
                            addLinkToGridInEditMode(grdDevices.Items[grdDevices.EditItemIndex]);

                        generateDataGridHeight("delete");
                        lblDeviceAlreadyExists.Text = "Warning: This device is associated with notification events (Step 2). You must first delete notification in Step2 before you delete this device.";
                    }
                }
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
        /// This Event is add the device for OC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddDevice_Click(object sender, System.EventArgs e)
        {
            try
            {
                addOCNotificationDevices();
                textChanged.Value = "true";
                hdnGridChanged.Value = "false";

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
                cmbGroup.Visible = isShow;
                lblGroup.Visible = isShow;
                cmbEvents.Visible = isShow;
                lblEvents.Visible = isShow;
                cmbFindings.Visible = isShow;
                lblFindings.Visible = isShow;
                if (cmbEvents.Items.Count > 0)
                    cmbEvents.SelectedIndex = 0;
                cmbGroup.SelectedValue = "-1";
                fillFindingDDL();
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

        /// <summary>
        /// Cancel Editing of the selected record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
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
                //if (e.Item.ItemType == ListItemType.Header)
                //{
                //    if (e.Item.Cells[0].Controls.Count > 0)
                //    {
                //        LinkButton lbtnDirSort = (e.Item.Cells[0].Controls[0]) as LinkButton;
                //        script += "if(confirmOnDataChange()){document.getElementById('" + txtDirectoryName.ClientID + "').value = '';";
                //        script += "__doPostBack('ctl00$ContentPlaceHolder1$grdDirectories$ctl01$ctl00', '');";
                //        script += "} else {return false;}";
                //        lbtnDirSort.OnClientClick = script;
                //    }
                //}

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

        #endregion Step1

        #region Step33
        /// <summary>
        /// Load Findings as per selected group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbStep3Groups_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                fillAfterHoursFindingDDL();
                generateStep3DataGridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.cmbStep3Groups_SelectedIndexChanged:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }
        /// <summary>
        /// This function is to delete records from data grid grdAfterHoursNotifications.
        /// This function calls stored procedure "deleteRPAfterHoursNotification" 
        /// After deletion this function refreshes the page so that updated records can be reached.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdAfterHoursNotifications_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            try
            {
                int rowid = (int.Parse(e.Item.Cells[0].Text));
                DataRow[] deleteRow = dtGridAfterHours.Select("AfterHourRowNo = " + rowid);

                int index = dtGridAfterHours.Rows.IndexOf(deleteRow[0]);

                dtGridAfterHours.Rows[index].BeginEdit();
                dtGridAfterHours.Rows[index]["FlagModified"] = "Deleted";
                dtGridAfterHours.Rows[index].AcceptChanges();
                Session[SessionConstants.DT_AFTERHOUR] = dtGridAfterHours;
                dataBindStep2();
                ScriptManager.RegisterStartupScript(upnlHidden, upnlHidden.GetType(), "Delete", "alert('Device has been deleted');", true);
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.grdAfterHoursNotifications_DeleteCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }

        }

        /// <summary>
        /// This function is to add new records into data grid grdAfterHoursNotifications.
        /// This function calls stored procedure "insertAfterHoursRPNotification"
        /// after insertion this function refreshes itself so that updated records will be shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddStep3_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (cmbAHDevice.Items.Count > 0)
                {
                    int RPDeviceID;
                    DataRow[] drRPDevice = dtGridDeviceNotifications.Select("DeviceName = '" + cmbAHDevice.SelectedItem.Text + "'");
                    if (drRPDevice[0]["FlagRowType"] != "New")
                        RPDeviceID = Convert.ToInt32(drRPDevice[0]["RPDeviceID"]);
                    else
                        RPDeviceID = Convert.ToInt32(drRPDevice[0]["RowID"]);
                    AfterHourRowNo++;
                    DataRow drAHNewRow = dtGridAfterHours.NewRow();
                    drAHNewRow["DeviceName"] = cmbAHDevice.SelectedItem.Text;
                    drAHNewRow["RPDeviceID"] = RPDeviceID;
                    drAHNewRow["FindingDescription"] = cmbAHFindings.SelectedItem.Text;
                    drAHNewRow["FindingID"] = int.Parse(cmbAHFindings.SelectedValue);
                    drAHNewRow["GroupName"] = cmbStep3Groups.SelectedItem.Text;
                    drAHNewRow["GroupID"] = cmbStep3Groups.SelectedValue;
                    drAHNewRow["StartHour"] = int.Parse(cmbAHStartHour.SelectedValue);
                    drAHNewRow["EndHour"] = int.Parse(cmbAHEndHour.SelectedValue);
                    drAHNewRow["AfterHourRowNo"] = AfterHourRowNo;
                    drAHNewRow["FlagRowType"] = "New";
                    drAHNewRow["FlagModified"] = "Unchanged";

                    dtGridAfterHours.Rows.Add(drAHNewRow);
                    Session[SessionConstants.DT_AFTERHOUR] = dtGridAfterHours;
                    dataBindStep2();
                    textChanged.Value = "true";
                    hdnGridChanged.Value = "false";
                    ScriptManager.RegisterStartupScript(upnlHidden, upnlHidden.GetType(), "Alert", "alert('Device has been added');", true);
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.btnAddStep3_Click:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        /// <summary>
        /// This function calls DataBind() function of data grid grdAfterHoursNotifications dynamically.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAfterHoursNotifications_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView data = (DataRowView)e.Item.DataItem;
                    if ((int)data["FindingID"] == -1)
                        e.Item.Cells[3].Text = "All Findings";
                    if (data["GroupID"].ToString().Length == 0 || (int)data["GroupID"] == -1)
                    {
                        e.Item.Cells[2].Text = "All Groups";
                    }

                    if (((int)data["StartHour"]) == 12)
                        e.Item.Cells[4].Text = "12 Noon";
                    else if (((int)data["StartHour"]) == 0)
                        e.Item.Cells[4].Text = "12 Midnight";
                    else if (((int)data["StartHour"]) > 12)
                        e.Item.Cells[4].Text = ((int)data["StartHour"] - 12) + " pm";
                    else
                        e.Item.Cells[4].Text += " am";

                    if (((int)data["EndHour"]) == 12)
                        e.Item.Cells[5].Text = "12 Noon";
                    else if (((int)data["EndHour"]) == 0)
                        e.Item.Cells[5].Text = "12 Midnight";
                    else if (((int)data["EndHour"]) > 12)
                        e.Item.Cells[5].Text = ((int)data["EndHour"] - 12) + " pm";
                    else
                        e.Item.Cells[5].Text += " am";
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.grdAfterHoursNotifications_ItemDataBound:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }

        }
        #endregion Step3

        /// <summary>
        /// EditCommand Events gets fired when edit button in grid is clicked        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdIdTypeInfo_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                grdIdTypeInfo.EditItemIndex = e.Item.ItemIndex;
                dataBindExternalIDs();
                textChanged.Value = "true";
                hdnGridChanged.Value = "false";
            }
            catch (Exception strEx)
            {
                throw strEx;
            }
        }

        /// <summary>
        /// Update event of the grid updates the data in datagrid only
        /// & not in the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdIdTypeInfo_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                TextBox txtIdNumber = (TextBox)e.Item.FindControl("txtIDNumber");
                if (txtIdNumber.Text.Trim().Length == 0)
                {
                    generateDataGridHeight(grdIdTypeInfo, divExternalIDInformation);
                    alertErrorToUserForExternalId("You Must Enter A External ID");
                }
                else
                {
                    dtIdTypesInfo = (DataTable)Session["DT_IDTYPEINFO"];
                    //key of the row being updated is retrieved
                    int externalId = (int)grdIdTypeInfo.DataKeys[e.Item.ItemIndex];
                    //retrieving the row being changed
                    DataRow[] number = dtIdTypesInfo.Select("ExternalRPID = '" + externalId + "'");
                    int introwno = dtIdTypesInfo.Rows.IndexOf(number[0]);
                    if (introwno >= 0)
                    {
                        dtIdTypesInfo.Rows[introwno].BeginEdit();
                        dtIdTypesInfo.Rows[introwno]["ExternalID"] = txtIdNumber.Text;
                        dtIdTypesInfo.Rows[introwno]["FlagModified"] = "Changed";
                        dtIdTypesInfo.Rows[introwno].EndEdit();
                        dtIdTypesInfo.Rows[introwno].AcceptChanges();
                    }
                    Session["DT_IDTYPEINFO"] = dtIdTypesInfo;
                    grdIdTypeInfo.EditItemIndex = -1;
                    dataBindExternalIDs();
                }
            }
            catch (Exception strEx)
            {
                throw strEx;
            }
        }

        /// <summary>
        /// Cancel link Event gets fired when cancel link is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdIdTypeInfo_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                grdIdTypeInfo.EditItemIndex = -1;
                dataBindExternalIDs();
            }
            catch (Exception strEx)
            {
                throw strEx;
            }
        }

        /// <summary>
        /// Deletes the required row from the datagrid by changing "FlagModified" value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdIdTypeInfo_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                int deleteID = Convert.ToInt32(e.Item.Cells[4].Text);
                DataRow[] deleteRow = dtIdTypesInfo.Select("ExternalRPID = '" + deleteID + "'");
                if (deleteRow.Length > 0)
                {
                    int rowId = dtIdTypesInfo.Rows.IndexOf(deleteRow[0]);
                    if (rowId >= 0)
                    {
                        dtIdTypesInfo.Rows[rowId].BeginEdit();
                        dtIdTypesInfo.Rows[rowId]["FlagModified"] = "Deleted";
                        dtIdTypesInfo.Rows[rowId].EndEdit();
                        dtIdTypesInfo.Rows[rowId].AcceptChanges();
                    }
                }
                dataBindExternalIDs();
            }
            catch (Exception strEx)
            {
                throw strEx;
            }
        }

        /// <summary>
        /// This method bind the griddevice grid.
        /// </summary>
        private void dataBindExternalIDs()
        {
            try
            {
                DataView dvIdTypes = new DataView(dtIdTypesInfo);
                //Filter dataview with only Unchanged, Changed devices
                dvIdTypes.RowFilter = "FlagModified in ('Unchanged', 'Changed')"; //Filter Deleted Rows

                grdIdTypeInfo.DataSource = dvIdTypes;
                grdIdTypeInfo.DataBind();
                if (grdIdTypeInfo.Items.Count < 1)
                {
                    lblNoRecordsExtInfo.Visible = true;
                }
                else
                {
                    lblNoRecordsExtInfo.Visible = false;
                }
            }
            catch (Exception strEx)
            {
                throw strEx;
            }
        }

        /// <summary>
        /// Event used to show hide textbox for adding new Id type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlIDType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlIDType.SelectedItem.Text.Trim() == "Other")
                {
                    lblAddIDType.Visible = true;
                    txtAddIDType.Visible = true;
                    btnAddExternalID.Enabled = true;
                    txtUserId.Enabled = true;
                }
                else if (ddlIDType.SelectedItem.Value.Trim() == "-2")
                {
                    lblAddIDType.Visible = false;
                    txtAddIDType.Visible = false;
                    btnAddExternalID.Enabled = false;
                    txtUserId.Enabled = false;
                }
                else
                {
                    lblAddIDType.Visible = false;
                    txtAddIDType.Visible = false;
                    btnAddExternalID.Enabled = true;
                    txtUserId.Enabled = true;
                }
            }
            catch (Exception strEx)
            {
                throw strEx;

            }
        }

        /// <summary>
        /// This event adds new ID type & user Id in the grid & not in database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddExternalID_Click(object sender, EventArgs e)
        {
            try
            {
                string userId = txtUserId.Text.Trim();
                string newIdType = null;

                if (ddlIDType.SelectedItem.Text.Trim() == "Other" && txtAddIDType.Text.Trim().ToLower().StartsWith("other"))
                {
                    generateDataGridHeight(grdIdTypeInfo, divExternalIDInformation);
                    ScriptManager.RegisterStartupScript(UpdatePanelExternalInfo, UpdatePanelExternalInfo.GetType(), "OtherException", "alert(\"Please enter another ID Type which does not start with 'other'\");document.getElementById(txtAddIDTypeClientID).focus();", true);
                }
                else
                {
                    if (validateExternalInfoBeforeSave())
                    {
                        OrderingClinician objAddEditOc = new OrderingClinician();
                        int IDType = Convert.ToInt32(ddlIDType.SelectedItem.Value.ToString());
                        if (ddlIDType.SelectedItem.Text.Trim() == "Other")
                        {
                            newIdType = txtAddIDType.Text.Trim();
                        }

                        DataRow drIDInfo = dtIdTypesInfo.NewRow();

                        if (ddlIDType.SelectedItem.Text.Trim() != "Other")
                            drIDInfo["ExternalIDTypeDescription"] = ddlIDType.SelectedItem.Text.Trim();
                        else
                            drIDInfo["ExternalIDTypeDescription"] = newIdType;

                        drIDInfo["ExternalID"] = userId;
                        drIDInfo["ReferringPhysicianID"] = Convert.ToInt32(ViewState["ReferringPhysicianID"].ToString()); ;//Rp Id needs to be taken from session of OC profile pages.
                        drIDInfo["ExternalIDTypeID"] = IDType;
                        drIDInfo["FlagRowType"] = "New";
                        drIDInfo["FlagModified"] = "Unchanged";

                        //code to populate dropdown with new Id types
                        if (ddlIDType.SelectedItem.Text.Trim() == "Other")
                        {
                            if (ddlIDType.Items.FindByText(newIdType) == null)
                            {
                                DataRow drIdType = dtIdTypes.NewRow();
                                drIdType["ExternalIDTypeDescription"] = newIdType;
                                drIdType["FlagRowType"] = "New";
                                dtIdTypes.Rows.InsertAt(drIdType, dtIdTypes.Rows.Count - 2);
                                Session["DT_IDTYPES"] = dtIdTypes;
                                ddlIDType.DataSource = dtIdTypes;
                                ddlIDType.DataBind();
                            }
                            else
                            {
                                drIDInfo["ExternalIDTypeID"] = Convert.ToInt32(ddlIDType.Items.FindByText(newIdType).Value);
                            }
                        }

                        dtIdTypesInfo.Rows.Add(drIDInfo);
                        Session["DT_IDTYPEINFO"] = dtIdTypesInfo;
                        dataBindExternalIDs();

                        resetControlsForExternalInfo();
                    }
                }
            }
            catch (Exception strEx)
            {
                throw strEx;
            }
        }

        protected void grdIdTypeInfo_OnItemCreated(object source, DataGridItemEventArgs e)
        {
            try
            {
                string script = "javascript:";
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    if (e.Item.Cells[1].Controls[1] is TextBox)
                    {
                        LinkButton lbUpdate = (e.Item.Cells[2].Controls[0]) as LinkButton;
                        LinkButton lbCancel = (e.Item.Cells[2].Controls[2]) as LinkButton;
                        lbUpdate.OnClientClick = script + "return ChangeFlagforGrid();";
                        lbCancel.OnClientClick = script + "return ChangeFlagforGrid();";
                    }
                }
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lbtnEdit = (e.Item.Cells[2].Controls[0]) as LinkButton;
                    script += "return ChangeFlagforGrid();"; ;
                    lbtnEdit.OnClientClick = script;
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("edit_oc.grdIdTypeInfo_OnItemCreated", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add new Department assignment
        /// </summary>
        /// <param name="departmentID"></param>
        /// <param name="referringPhysicianID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private bool addDepartmentAssignment(int departmentID, int referringPhysicianID, DateTime startTime, DateTime endTime)
        {
            OrderingClinician objOC = new OrderingClinician();

            try
            {
                if (objOC.AddNewAssignment(departmentID, referringPhysicianID, startTime, endTime) < 0)
                {
                    string error = "";
                    error = @"The entered assignment dates overlaps with following existing assignment.\n";
                    error += objOC.getOverlappedAssignments(0, departmentID, referringPhysicianID, startTime, endTime);
                    error += @"You must eliminate the overlap to save the assignment.";
                    ScriptManager.RegisterStartupScript(upnlCT, upnlCT.GetType(), "Add_Failed", "alert('" + error + "');", true);
                    return false;
                }
                return true;
            }
            finally
            {
                objOC = null;
            }
        }

        /// <summary>
        /// Update Department Assignment
        /// </summary>
        /// <param name="departmentAssignID"></param>
        /// <param name="departmentID"></param>
        /// <param name="referringPhysicianID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private bool updateDepartmentAssignment(int departmentAssignID, int departmentID, int referringPhysicianID, DateTime startTime, DateTime endTime)
        {
            OrderingClinician objOC = new OrderingClinician();

            try
            {
                if (objOC.UpdateAssignment(departmentAssignID, departmentID, referringPhysicianID, startTime, endTime) < 0)
                {
                    string error = "";
                    error = @"The entered assignment dates overlaps with following existing assignment.\n";
                    error += objOC.getOverlappedAssignments(departmentAssignID, departmentID, referringPhysicianID, startTime, endTime);
                    error += @"You must eliminate the overlap to save the assignment.";

                    ScriptManager.RegisterStartupScript(upnlCT, upnlCT.GetType(), "Update_Failed", "alert('" + error + "');", true);
                    return false;
                }
                return true;
            }
            finally
            {
                objOC = null;
            }
        }

        /// <summary>
        /// Populate all department assignments for given criteria 
        /// </summary>
        /// <param name="referringphysicianID"></param>
        private void populateDepartmentAssignments(int referringphysicianID)
        {
            DataTable dtDepartmentAssignment = null;
            OrderingClinician objOC = new OrderingClinician();

            try
            {
                dtDepartmentAssignment = objOC.getDepartmentAssignmentsForOC(referringphysicianID);

                DataView dvDeptAssigns = dtDepartmentAssignment.DefaultView;
                grdDepartmentAssignment.DataSource = dvDeptAssigns;

                grdDepartmentAssignment.DataSource = dtDepartmentAssignment;
                grdDepartmentAssignment.DataBind();
            }
            finally
            {
                objOC = null;
                dtDepartmentAssignment = null;
                setDatagridHeight();
            }
        }

        /// <summary>
        /// Clears Clinical team data
        /// </summary>
        private void clearCTData()
        {
            if (btnAddDept.Text == "Update")
            {
                drpDepartment.Items.Insert(0, "None");
                drpDepartment.Items[0].Value = "-1";
            }
            lblPhone.Text = string.Empty;
            txtStartDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;
            drpDepartment.Enabled = true;
            drpDepartment.SelectedValue = "-1";
            txtStartDate.Enabled = false;
            txtEndDate.Enabled = false;

            if (btnAddDept.Text == "Update")
                btnAddDept.Text = "Add";

            btnAddDept.Enabled = false;
            hdnCTChanged.Value = "false";
        }

        /// <summary>
        /// This method will set the height of datagrid dynamically accordingly the current rowcount of datagrid,
        /// each time when the page posts back. 
        /// </summary>
        private void setDatagridHeight()
        {
            int devicesGridHeight = 20;
            int rowHeight = 21;
            int headerHeight = 23;
            int maxRows = 1;

            if (grdDepartmentAssignment.Items.Count <= 1)
            {

                if (grdDepartmentAssignment.Items.Count == 0)
                    devicesGridHeight = headerHeight;
                else
                    devicesGridHeight = (grdDepartmentAssignment.Items.Count * rowHeight) + headerHeight;
            }
            else
            {
                devicesGridHeight = (maxRows * rowHeight) + headerHeight;
            }

            devicesGridHeight += 10;

            string script = "";
            script = "<script type=\"text/javascript\">";
            script += "document.getElementById(" + '"' + "DepartmentDiv" + '"' + ").style.height='" + (devicesGridHeight + 1) + "';</script>";

            if (!IsPostBack)
            {
                ScriptManager.RegisterStartupScript(upnlDepartmentGrid, upnlDepartmentGrid.GetType(), "GridHeight1", script, false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(upnlDepartmentGrid, upnlDepartmentGrid.GetType(), "GridHeight", script, false);
            }
        }

        /// <summary>
        /// This function is to Fill information for logged in Ordering Physician.
        /// This function calls stored procedure "getReferringPhysicianByID"
        /// </summary>
        /// <param name="cnx">Connection String</param>
        private void fillPhysicianInfo()
        {
            OrderingClinician objOC;
            try
            {
                int refId = 0;
                if (Request[OCID] != null)
                {
                    ViewState[OCID] = Request[OCID];
                    refId = int.Parse(ViewState[OCID].ToString());

                    lblRPId.Text = refId.ToString();

                    objOC = new OrderingClinician();
                    DataTable dt = objOC.GetReferringPhysicianByID(refId);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow drOC = dt.Rows[0];
                        lblLastUpdated.Text = ((DateTime)drOC["LastUpdated"]).ToShortDateString();
                        string updatedBy = drOC["UpdatedBy"].ToString();
                        if (updatedBy.Length > 0)
                        {
                            lblLastUpdated.Text += " by " + updatedBy;
                        }
                        cbActive.Checked = (bool)drOC["Active"];
                        txtFirstName.Text = drOC["FirstName"].ToString();
                        txtLastName.Text = drOC["LastName"].ToString();
                        ViewState["ClinicianName"] = drOC["FirstName"].ToString() + " " + drOC["LastName"].ToString();
                        txtNickname.Text = drOC["Nickname"].ToString();
                        txtEmail.Text = drOC["PrimaryEmail"].ToString();
                        string primaryPhone = Utils.flattenPhoneNumber(drOC["PrimaryPhone"].ToString());
                        if (primaryPhone.Length == 10)  // only if we have a valid 10 digit phone number stored..
                        {
                            txtPrimaryPhoneAreaCode.Text = primaryPhone.Substring(0, 3);
                            txtPrimaryPhonePrefix.Text = primaryPhone.Substring(3, 3);
                            txtPrimaryPhoneNNNN.Text = primaryPhone.Substring(6, 4);
                        }
                        string fax = Utils.flattenPhoneNumber(drOC["Fax"].ToString());
                        if (fax.Length == 10)  // only if we have a valid 10 digit phone number stored..
                        {
                            txtFaxAreaCode.Text = fax.Substring(0, 3);
                            txtFaxPrefix.Text = fax.Substring(3, 3);
                            txtFaxNNNN.Text = fax.Substring(6, 4);
                        }
                        string cellPhone = Utils.flattenPhoneNumber(drOC["CellPhone"].ToString());
                        if (cellPhone.Length == 10)  // only if we have a valid 10 digit phone number stored..
                        {
                            txtCellAreaCode.Text = cellPhone.Substring(0, 3);
                            txtCellPrefix.Text = cellPhone.Substring(3, 3);
                            txtCellNNNN.Text = cellPhone.Substring(6, 4);
                        }
                        txtAddress1.Text = drOC["Address1"].ToString();
                        txtAddress2.Text = drOC["Address2"].ToString();
                        txtAddress3.Text = drOC["Address3"].ToString();
                        txtCity.Text = drOC["City"].ToString();
                        txtState.Text = drOC["State"].ToString();
                        txtZip.Text = drOC["Zip"].ToString();
                        ViewState["PracticeGroup"] = drOC["PracticeGroup"].ToString();
                        txtPracticeGroup.Text = drOC["PracticeGroup"].ToString();
                        txtHospitalAffiliation.Text = drOC["Affiliation"].ToString();
                        ViewState["Specialty"] = drOC["Specialty"].ToString();
                        txtSpecialty.Text = drOC["Specialty"].ToString();
                        txtName.Text = drOC["AdditionalContName"].ToString();
                        string additionalPhone = Utils.flattenPhoneNumber(drOC["AdditionalContPhone"].ToString());
                        if (additionalPhone.Length == 10)  // only if we have a valid 10 digit phone number stored..
                        {
                            txtPhoneCode.Text = additionalPhone.Substring(0, 3);
                            txtPhonePrefix.Text = additionalPhone.Substring(3, 3);
                            txtPhoneNumber.Text = additionalPhone.Substring(6, 4);
                        }

                        chkRadiologyTDR.Checked = bool.Parse(drOC["RadiologyTDR"].ToString());
                        chkLabTDR.Checked = bool.Parse(drOC["LabTDR"].ToString());
                        txtNotes.Text = drOC["Notes"].ToString();

                        txtStartDate.Text = string.Empty;
                        txtEndDate.Text = string.Empty;
                        if (int.Parse(drOC["DepartmentID"].ToString()) != 0)
                        {
                            drpDepartment.SelectedValue = "-1";
                            drpDepartment.Enabled = true;
                            txtStartDate.Enabled = true;
                            getDeptNotificationPreferences(Convert.ToInt32(drOC["DepartmentID"].ToString()));
                            lblTeamName.Text = " - " + drpDepartment.SelectedItem.Text.ToString();

                        }

                        if (ViewState[DEPARTMENT] != null)
                        {
                            DataTable dtDepartments = (DataTable)ViewState[DEPARTMENT];
                            DataRow[] selectRow = dtDepartments.Select("DepartmentID = '" + drpDepartment.SelectedValue + "'");
                            if (selectRow[0]["PhoneNumber"] != System.DBNull.Value)
                            {
                                string phone = Utils.expandPhoneNumber((string)selectRow[0]["PhoneNumber"]);
                                if (selectRow[0]["Extn"] != System.DBNull.Value)
                                {
                                    phone += " Extn." + (string)selectRow[0]["Extn"];
                                }
                                lblPhone.Text = phone;
                            }
                            dtDepartments = null;
                        }

                        ViewState["DepartmentID"] = drOC["DepartmentID"].ToString();
                        ViewState["VOCUserID"] = drOC["VOCUserID"].ToString();
                        ViewState["InstitutionID"] = drOC["InstitutionID"].ToString();
                        if (drOC["ProfileCompletedOn"] != null)
                        {
                            if (drOC["ProfileCompletedOn"].ToString().Length > 0)
                                chkProfileCompleted.Checked = true;
                        }

                        Institution objInstitution = new Institution();
                        DataTable dtInstInfo = objInstitution.GetInstitutionInfo(Convert.ToInt32(ViewState["InstitutionID"].ToString()));
                        if (dtInstInfo != null)
                        {
                            DataRow drInstInfo = dtInstInfo.Rows[0];
                            if (drInstInfo["RequireVoiceClips"] != System.DBNull.Value && drInstInfo["RequireVoiceClips"].ToString().ToLower() == "true")
                            {
                                pnlEDDoc.Visible = true;
                                chkEDDoc.Checked = Convert.ToBoolean(drOC["EDDoc"]);
                                if (chkEDDoc.Checked)
                                    pnlOCLogin.Visible = true;

                                if (drOC["LoginID"] != System.DBNull.Value)
                                    if (drOC["LoginID"].ToString().Trim() != "0")
                                        txtLoginID.Text = drOC["LoginID"].ToString().Trim();

                                if (drOC["Password"] != null)
                                    if (drOC["LoginID"].ToString().Trim() != "0")
                                        txtPassword.Text = drOC["Password"].ToString().Trim();
                            }
                            else
                            {
                                pnlOCLogin.Visible = false;
                                pnlEDDoc.Visible = false;

                                if (drInstInfo["MessageRetrieveUsingPIN"] != System.DBNull.Value && Convert.ToBoolean(drInstInfo["MessageRetrieveUsingPIN"]) == false)
                                {
                                    fldLoginDetails.Visible = false;
                                }

                            }
                            ViewState["ExistingPIN"] = drOC["PINForMessageRetrieve"].ToString().Trim();
                            if (drInstInfo["MessageRetrieveUsingPIN"] != System.DBNull.Value && Convert.ToBoolean(drInstInfo["MessageRetrieveUsingPIN"]))
                            {
                                pnlMessageRetieve.Visible = true;
                                if (drOC["PINForMessageRetrieve"] != null)
                                {
                                    if (drOC["PINForMessageRetrieve"].ToString().Length > 0)
                                    {
                                        txtPinForMessage.Text = drOC["PINForMessageRetrieve"].ToString().Trim();
                                    }
                                }
                            }
                            else
                            {
                                pnlMessageRetieve.Visible = false;
                            }
                        }

                        dt = null;
                    }
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.fillPhysicianInfo():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
            finally
            {
                objOC = null;
                generateCTDataGridHeight();
            }

        }

        /// <summary>
        /// This function is to get all list of Cell phone carriers into drop down.        
        /// </summary>
        /// <param name="cnx">Connection String</param>
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

            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID].ToString().Length > 0)
                    Tracer.GetLogger().LogExceptionEvent("edit_OC.getCarriers" + ex.Message + "\n" + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
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
        /// To add the attribute to set the flag on change of any control on the page.
        /// </summary>
        private void addAttributesToSaveChanges()
        {
            txtFirstName.Attributes.Add("onkeypress", "JavaScript:UpdateProfile();");
            txtFirstName.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            txtSpecialty.Attributes.Add("onkeypress", "JavaScript:UpdateProfile();");
            txtSpecialty.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");

            txtLastName.Attributes.Add("onkeypress", "JavaScript:UpdateProfile();");
            txtLastName.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtPracticeGroup.Attributes.Add("onkeypress", "JavaScript:UpdateProfile();");
            txtPracticeGroup.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtNickname.Attributes.Add("onkeypress", "JavaScript:UpdateProfile();");
            txtNickname.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtHospitalAffiliation.Attributes.Add("onkeypress", "JavaScript:UpdateProfile();");
            txtHospitalAffiliation.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtPrimaryPhonePrefix.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtPrimaryPhonePrefix.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtPrimaryPhoneAreaCode.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtPrimaryPhoneAreaCode.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtPrimaryPhoneNNNN.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtPrimaryPhoneNNNN.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            cbActive.Attributes.Add("onclick", "JavaScript:UpdateProfile();");
            txtEmail.Attributes.Add("onkeydown", "JavaScript:UpdateProfile();");
            txtEmail.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtFaxPrefix.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtFaxPrefix.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtFaxAreaCode.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtFaxAreaCode.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtFaxNNNN.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtFaxNNNN.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtAddress1.Attributes.Add("onkeypress", "JavaScript:UpdateProfile();");
            txtAddress1.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtAddress2.Attributes.Add("onkeypress", "JavaScript:UpdateProfile();");
            txtAddress2.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtAddress3.Attributes.Add("onkeypress", "JavaScript:UpdateProfile();");
            txtAddress3.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtCity.Attributes.Add("onkeypress", "JavaScript:UpdateProfile();");
            txtCity.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");

            txtState.Attributes.Add("onkeypress", "JavaScript:UpdateProfile();");
            txtState.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtZip.Attributes.Add("onkeypress", "JavaScript:UpdateProfile();");
            txtZip.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
           
            txtName.Attributes.Add("onkeypress", "JavaScript:UpdateProfile();");
            txtName.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");

            txtPhonePrefix.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtPhonePrefix.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");

            txtPhoneCode.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtPhoneCode.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtPhoneNumber.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtPhoneNumber.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");

            //txtLoginID.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            //txtPassword.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            ////chkResident.Attributes.Add("onClick", "JavaScript:UpdateProfile();enbaleDisableDeptCombo();");
            drpDepartment.Attributes.Add("onchange", "JavaScript:enableDateSelection('false');");

            //step1
            lblDeviceAlreadyExists.Text = "";
            /* To prevent user to leave page without saving changes.*/
            btnAddDevice.Attributes.Add("onclick", "return validateDevices('" + cmbDeviceType.ClientID + "','" + txtNumAddress.ClientID + "','" + cmbCarrier.ClientID + "','" + txtEmailGateway.ClientID + "','" + cmbFindings.ClientID + "','" + cmbGroup.ClientID + "','" + txtInitialPause.ClientID + "');");

            txtNumAddress.Attributes.Add("onclick", "RemoveDeviceLabel('" + txtNumAddress.ClientID + "','" + hidDeviceLabel.ClientID + "');");
            txtInitialPause.Attributes.Add("onclick", "RemoveInitialPauseLabel('" + txtInitialPause.ClientID + "','" + hidInitPauseLabel.ClientID + "');");

            //end step1
        }

        /// <summary>
        /// To validate if directly doPostback for Save is called from javascript e.g in case of
        /// Onunload event, if user didn't saved the changes.
        /// </summary>
        /// <returns></returns>
        private bool validateBeforeSave()
        {
            string message = string.Empty;
            bool validated = true;
            if (txtFirstName.Text.Trim().Length == 0)
            {
                message = "You Must Enter A First Name";
                validated = false;
            }
            if (txtLastName.Text.Trim().Length == 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "You Must Enter A Last Name";
                validated = false;
            }

            // Validation for Primary phone
            if (txtPrimaryPhonePrefix.Text.Trim().Length == 0)
            {
                if (message.Length != 0)
                {
                    message += "#";

                }
                message += "You Must Enter A Primary Phone Prefix";
                validated = false;
            }
            else if (txtPrimaryPhonePrefix.Text.Trim().Length != 3)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Primary office Phone Prefix";
                validated = false;
            }
            if (txtPrimaryPhoneAreaCode.Text.Trim().Length == 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "You Must Enter A Primary Office Phone Area Code";
                validated = false;
            }
            else if (txtPrimaryPhoneAreaCode.Text.Trim().Length != 3)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Primary Office Phone Area Code";
                validated = false;
            }
            if (txtPrimaryPhoneNNNN.Text.Trim().Length == 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "You Must Enter A Primary Office Phone Extension";
                validated = false;
            }
            else if (txtPrimaryPhoneNNNN.Text.Trim().Length != 4)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Primary Office Phone Extension";
                validated = false;
            }

            //Validation for additional Phone
            if (txtPhonePrefix.Text.Trim().Length != 3 && txtPhonePrefix.Text.Trim().Length != 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Additional Phone prefix";
                validated = false;
            }
            if (txtPhoneCode.Text.Trim().Length != 3 && txtPhoneCode.Text.Trim().Length != 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Additional Phone Area Code";
                validated = false;
            }
            if (txtPhoneNumber.Text.Trim().Length != 4 && txtPhoneNumber.Text.Trim().Length != 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Additional Phone Extension";
                validated = false;
            }

            //Validation for Fax.
            if (txtFaxPrefix.Text.Trim().Length != 3 && txtFaxPrefix.Text.Trim().Length != 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Fax prefix";
                validated = false;
            }
            if (txtFaxAreaCode.Text.Trim().Length != 3 && txtFaxAreaCode.Text.Trim().Length != 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Fax Area Code";
                validated = false;
            }
            if (txtFaxNNNN.Text.Trim().Length != 4 && txtFaxAreaCode.Text.Trim().Length != 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Fax Extension";
                validated = false;
            }
            if (message.Length != 0)
            {
                alertErrorToUser(message);
            }
            return validated;
        }

        /// <summary>
        /// This method will alert the error passed as parameter to the user.
        /// </summary>
        /// <param name="message"></param>
        private void alertErrorToUser(string message)
        {
            ScriptManager.RegisterStartupScript(upnlSaveBtn, upnlSaveBtn.GetType(), "Validationalert", "<script language=" + '"' + "javascript" + '"' + ">showMessage(" + '"' + message + '"' + ");</script>", false);
        }

        /// <summary>
        /// Register JS variables, client side button click events
        /// </summary>
        private void registerJavascriptVariables()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var textChangedClientID = '" + textChanged.ClientID + "';");
            sbScript.Append("var hdnCTChangedClientID = '" + hdnCTChanged.ClientID + "';");
            sbScript.Append("var txtLastNameClientID = '" + txtLastName.ClientID + "';");
            sbScript.Append("var txtStartDateClientID = '" + txtStartDate.ClientID + "';");
            sbScript.Append("var txtEndDateClientID = '" + txtEndDate.ClientID + "';");
            sbScript.Append("var anchFromDateClientID = '" + anchFromDate.ClientID + "';");
            sbScript.Append("var anchToDateClientID = '" + anchToDate.ClientID + "';");
            sbScript.Append("var drpDepartmentClientID = '" + drpDepartment.ClientID + "';");
            sbScript.Append("var cmbDepartmentClientID = '" + drpDepartment.ClientID + "';");
            sbScript.Append("var lblStarClientID = '" + lblStar.ClientID + "';");
            sbScript.Append("var lblTeamNameClientID = '" + lblTeamName.ClientID + "';");
            sbScript.Append("var lblStarClientID = '" + lblStar.ClientID + "';");
            sbScript.Append("var txtPinForMessageClientID = '" + txtPinForMessage.ClientID + "';");
            sbScript.Append("var txtInitialPauseClientID = '" + txtInitialPause.ClientID + "';");
            sbScript.Append("var hidDeviceLabelClientID = '" + hidDeviceLabel.ClientID + "';");
            sbScript.Append("var hidGatewayLabelClientID = '" + hidGatewayLabel.ClientID + "';");
            sbScript.Append("var txtCellAreaCodeClientID = '" + txtCellAreaCode.ClientID + "';");
            sbScript.Append("var txtCellPrefixClientID = '" + txtCellPrefix.ClientID + "';");
            sbScript.Append("var txtCellNNNNClientID = '" + txtCellNNNN.ClientID + "';");
            sbScript.Append("var hdnGridChangedClientID = '" + hdnGridChanged.ClientID + "';");
            sbScript.Append("var btnAddClientID = '" + btnAdd.ClientID + "';");
            sbScript.Append("var btnAddDeptClientID = '" + btnAddDept.ClientID + "';");
            sbScript.Append("var btnClearClientID = '" + btnClear.ClientID + "';");
            sbScript.Append("var hiddenScrollPos = '" + scrollPos.ClientID + "';");
            sbScript.Append("var hdnIsAddClickedClientID = '" + hdnIsAddClicked.ClientID + "';");
            sbScript.Append("var txtUserIdClientID = '" + txtUserId.ClientID + "';");
            sbScript.Append("var txtAddIDTypeClientID = '" + txtAddIDType.ClientID + "';");

            sbScript.Append("enableDateSelection(); document.getElementById(btnAddDeptClientID).disabled=true;</script>");
            //sbScript.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());

            txtPrimaryPhoneAreaCode.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtPrimaryPhonePrefix.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtPrimaryPhoneNNNN.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");

            txtPhoneCode.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtPhonePrefix.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtPhoneNumber.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");

            txtFaxPrefix.Attributes.Add("onchange", "JavaScript:return isNumericKeyStroke();");
            txtFaxAreaCode.Attributes.Add("onchange", "JavaScript:return isNumericKeyStroke();");
            txtFaxNNNN.Attributes.Add("onchange", "JavaScript:return isNumericKeyStroke();");
            txtCellPrefix.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtCellAreaCode.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtCellNNNN.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");

            txtPrimaryPhoneAreaCode.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryPhonePrefix.ClientID + "').focus()";
            txtPrimaryPhonePrefix.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryPhoneNNNN.ClientID + "').focus()";
            txtPhoneCode.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPhonePrefix.ClientID + "').focus()";
            txtPhonePrefix.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPhoneNumber.ClientID + "').focus()";
            txtFaxAreaCode.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtFaxPrefix.ClientID + "').focus()";
            txtFaxPrefix.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtFaxNNNN.ClientID + "').focus()";
            txtCellAreaCode.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtCellPrefix.ClientID + "').focus();";
            txtCellPrefix.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtCellNNNN.ClientID + "').focus();";

            txtNotes.Attributes.Add("onchange", "JavaScript:UpdateProfile();return CheckMaxLength('" + txtNotes.ClientID + "','" + txtNotes.MaxLength + "');");
            txtNotes.Attributes.Add("onblur", "JavaScript:return CheckMaxLength('" + txtNotes.ClientID + "','" + txtNotes.MaxLength + "');");
            txtNotes.Attributes.Add("onkeyup", "JavaScript:return CheckMaxLength('" + txtNotes.ClientID + "','" + txtNotes.MaxLength + "');");
            txtNotes.Attributes.Add("onkeydown", "JavaScript:return CheckMaxLength('" + txtNotes.ClientID + "','" + txtNotes.MaxLength + "');");
            txtNotes.Attributes.Add("onkeypress", "JavaScript:UpdateProfile();");
            txtNotes.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");

            chkLabTDR.Attributes.Add("onClick", "JavaScript:UpdateProfile();");
            chkRadiologyTDR.Attributes.Add("onClick", "JavaScript:UpdateProfile();");
            chkProfileCompleted.Attributes.Add("onClick", "JavaScript:UpdateProfile();");
            txtStartDate.Attributes.Add("onkeydown", "JavaScript:ctDataChanged();");
            txtStartDate.Attributes.Add("onPaste", "JavaScript:ctDataChanged();");
            txtEndDate.Attributes.Add("onkeydown", "JavaScript:ctDataChanged();");
            txtEndDate.Attributes.Add("onPaste", "JavaScript:ctDataChanged();");

            drpDepartment.Attributes.Add("onClick", "JavaScript:UpdateLabel();");
            txtLoginID.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtLoginID.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtPassword.Attributes.Add("onkeydown", "JavaScript:UpdateProfile();");
            txtPassword.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtPinForMessage.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStroke();");
            txtPinForMessage.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtPinForMessage.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            chkEDDoc.Attributes.Add("onClick", "JavaScript:UpdateProfile();");
            txtCellPrefix.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtCellPrefix.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtCellAreaCode.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtCellAreaCode.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");
            
            txtCellNNNN.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtCellNNNN.Attributes.Add("onPaste", "JavaScript:UpdateProfile();");

            anchFromDate.Attributes.Add("onClick",
             String.Format(@"javascript:calRPStartDate.select(document.all['{0}'],document.all['{1}'],'MM/dd/yyyy');JavaScript:ctDataChanged();",
                                            txtStartDate.ClientID,
                                            anchFromDate.ClientID
                                            ));

            anchToDate.Attributes.Add("onClick",
            String.Format(@"javascript:calRPEndDate.select(document.all['{0}'],document.all['{1}'],'MM/dd/yyyy');JavaScript:ctDataChanged();return false;",
                                           txtEndDate.ClientID,
                                           anchToDate.ClientID
                                           ));
            drpDepartment.Attributes.Add("onClick", "JavaScript:UpdateLabel();");
        }

        /// <summary>
        /// Load Departments in combo
        /// </summary>
        private void loadDepartment()
        {
            OrderingClinician objOC = null;
            DataTable dtDepartments = null;
            DataRow drDepartment = null;
            try
            {
                int ocID = Convert.ToInt32(ViewState[OCID]);
                objOC = new OrderingClinician();
                dtDepartments = objOC.GetDepartments(ocID);
                drDepartment = dtDepartments.NewRow();
                drDepartment[0] = -1;
                drDepartment[1] = "None";
                dtDepartments.Rows.InsertAt(drDepartment, 0);

                drpDepartment.DataSource = dtDepartments;
                ViewState[DEPARTMENT] = dtDepartments;
                drpDepartment.DataTextField = "DepartmentName";
                drpDepartment.DataValueField = "DepartmentID";
                drpDepartment.DataBind();

            }
            catch (Exception ex)
            {
                if (subscriberID.Trim().Length != 0)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_oc:getDepartment :: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
            }
            finally
            {
                objOC = null;
                dtDepartments = null;
                drDepartment = null;
            }
        }

        private bool isValidDate(string date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Populates all the devices and preferances for the selected Department in the Datagrid grdDevices
        /// </summary>
        /// <param name="deptID"></param>
        private void getDeptNotificationPreferences(int deptID)
        {
            OrderingClinician objOC;
            DataSet dsDeptNotification = null;
            try
            {
                objOC = new OrderingClinician();
                dsDeptNotification = objOC.GetNotificationPreferencesForDept(deptID);
                grdCTDevices.DataSource = dsDeptNotification;
                grdCTDevices.DataBind();
                if (grdCTDevices.Items.Count < 1)
                {
                    lblNoRecords.Visible = true;
                    grdCTDevices.SelectedIndex = -1;
                }
                else
                {
                    lblNoRecords.Visible = false;
                }

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.getDeptNotificationPreferences AS-->" + ex.Message + "--> At" + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
            finally
            {

                if (dsDeptNotification != null)
                {
                    dsDeptNotification.Dispose();
                    dsDeptNotification = null;
                }
                objOC = null;
            }
        }

        private void generateCTDataGridHeight()
        {
            int nDevicesGridHeight = 20;
            int gridItemHeight = 21;
            int gridHeaderHeight = 30;
            int maxRows = 4;
            int rowCount = grdCTDevices.Items.Count;

            if (rowCount < maxRows)
            {
                nDevicesGridHeight = (grdCTDevices.Items.Count * gridItemHeight) + gridHeaderHeight;
            }
            else
            {
                nDevicesGridHeight = (maxRows * gridItemHeight) + gridHeaderHeight;
            }

            string script = "<script type=\"text/javascript\">";

            if (rowCount == 0)
            {
                script += "document.getElementById(" + '"' + UnitDeviceDiv.ClientID + '"' + ").style.height=0;";
                script += "document.getElementById(" + '"' + UnitDeviceDiv.ClientID + '"' + ").style.border=0;</script>";
            }
            else
            {
                script += "document.getElementById(" + '"' + UnitDeviceDiv.ClientID + '"' + ").style.height='" + (nDevicesGridHeight + 1) + "';</script>";
            }
            ScriptManager.RegisterStartupScript(upnlEditOC, upnlEditOC.GetType(), "setDataCT", script, false);
        }

        /// <summary>
        /// Delete the Device Notification from Grid
        /// </summary>
        /// <param name="notificationID"></param>
        private void deleteDeviceNotification(string deviceName,int notificationID)
        {
            DataRow[] deleteRow = dtGridDeviceNotifications.Select("DeviceName = '" + deviceName + "' AND RPNotificationID = " + notificationID);
            int rowid = dtGridDeviceNotifications.Rows.IndexOf(deleteRow[0]);
            dtGridDeviceNotifications.Rows[rowid].BeginEdit();
            dtGridDeviceNotifications.Rows[rowid]["FlagModified"] = "Deleted";
            dtGridDeviceNotifications.Rows[rowid].EndEdit();
            dtGridDeviceNotifications.Rows[rowid].AcceptChanges();
            Session[SessionConstants.DT_NOTIFICATION] = dtGridDeviceNotifications;
            grdDevices.EditItemIndex = -1;
            dataBindStep1();
            fillAfterHoursDeviceOptions();
            generateStep3DataGridHeight();
            upnlStep3.Update();
            ScriptManager.RegisterStartupScript(upnlHidden, upnlHidden.GetType(), "Delete", "alert('Device has been deleted');", true);
        }


        /// To validate external information update.
        /// </summary>
        /// <returns></returns>
        private bool validateExternalInfoBeforeSave()
        {
            string message = string.Empty;
            bool validated = true;
            if (txtUserId.Text.Trim().Length == 0)
            {
                message = "You Must Enter A External ID";
                validated = false;
            }
            if (ddlIDType.SelectedItem.Text.Trim() == "Other" && txtAddIDType.Text.Trim().Length == 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "You Must Enter A ID Type";
                validated = false;
            }

            if (message.Length != 0)
            {
                generateDataGridHeight(grdIdTypeInfo, divExternalIDInformation);
                alertErrorToUserForExternalId(message);
            }
            return validated;
        }

        /// <summary>
        /// To reset controls for External Information adds/updates.
        /// </summary>
        /// <returns></returns>
        private void resetControlsForExternalInfo()
        {
            txtUserId.Text = "";
            txtAddIDType.Text = "";
            lblAddIDType.Visible = false;
            txtAddIDType.Visible = false;
            btnAddExternalID.Enabled = false;
            txtUserId.Enabled = false;
            ddlIDType.SelectedIndex = -1;
        }

        /// <summary>
        /// This method will alert the error passed as parameter to the user.
        /// </summary>
        /// <param name="message"></param>
        private void alertErrorToUserForExternalId(string message)
        {
            ScriptManager.RegisterStartupScript(UpdatePanelExternalInfo, UpdatePanelExternalInfo.GetType(), "Validationalert", "showMessage(" + '"' + message + '"' + ");", true);
        }

        /// <summary>
        /// Sets the height(standard for all grids) for the Grid Element.
        /// </summary>
        /// <param name="grdElement"></param>
        /// <param name="divElement"></param>
        private void generateDataGridHeight(DataGrid grdElement, HtmlGenericControl divElement)
        {
            string key = divElement.ClientID + this.UniqueID.Replace(":", "_");
            string script = "<script type=\"text/javascript\">" + Utils.getGridResizeScript(divElement.ClientID, grdElement.ClientID, GRID_ADJUSTMENT_VALUE, GRID_MAX_SIZE) + "</script>";
            ScriptManager.RegisterStartupScript(UpdatePanelExternalInfo, UpdatePanelExternalInfo.GetType(), key, script, false);
        }

        /// <summary>
        /// Method to get the ID type info for a particular RP from database & 
        /// bind it to datagrid
        /// </summary>
        /// <param name="referringPhysicianID"></param>
        private void BindOCExternalIDsInfo(int referringPhysicianID)
        {
            OrderingClinician objAddEditOC = null;
            DataTable dtIdInfo = null;
            try
            {
                objAddEditOC = new OrderingClinician();
                dtIdInfo = new DataTable();

                dtIdInfo = objAddEditOC.GetOCIdTypesInfo(referringPhysicianID);
                dtIdTypesInfo = dtIdInfo.Copy();

                //Autoincrement set to true so that on insertion of new record "ExternalRPID"
                dtIdTypesInfo.Columns["ExternalRPID"].AutoIncrement = true;
                dtIdTypesInfo.Columns["ExternalRPID"].Unique = true;

                //columns added to know which row is updated,deleted or added new to save in the database.
                dtIdTypesInfo.Columns.Add("FlagModified", typeof(string));
                dtIdTypesInfo.Columns.Add("FlagRowType", typeof(string));

                foreach (DataRow dr in dtIdTypesInfo.Rows)
                {
                    dr["FlagModified"] = "Unchanged";
                }
                grdIdTypeInfo.DataSource = dtIdTypesInfo;
                grdIdTypeInfo.DataBind();
                if (grdIdTypeInfo.Items.Count < 1)
                {
                    lblNoRecordsExtInfo.Visible = true;
                }
                else
                {
                    lblNoRecordsExtInfo.Visible = false;
                }

                Session["DT_IDTYPEINFO"] = dtIdTypesInfo;
            }
            catch (Exception strEx)
            {
                throw strEx;
            }
            finally
            {
                generateDataGridHeight(grdIdTypeInfo, divExternalIDInformation);
            }
        }

        /// <summary>
        /// Method to populate Id Types DropDown
        /// </summary>
        private void populateIdTypes()
        {
            OrderingClinician objAddEditOC = null;
            DataTable dtTypes = null;
            try
            {
                objAddEditOC = new OrderingClinician();
                dtTypes = new DataTable();

                dtTypes = objAddEditOC.GetExternalIDTypes();
                DataTable dtIdTypes = dtTypes.Copy();

                //flag added in the datatable to know if the new row for id type has been added
                dtIdTypes.Columns.Add("FlagRowType", typeof(string));
                dtIdTypes.Columns["ExternalIDTypeID"].Unique = true;
                dtIdTypes.Columns["ExternalIDTypeID"].AutoIncrement = true;

                DataRow dr = dtIdTypes.NewRow();
                dr["ExternalIDTypeID"] = "-1";
                dr["ExternalIDTypeDescription"] = "Other";
                dtIdTypes.Rows.Add(dr);

                DataRow drSelectId = dtIdTypes.NewRow();
                drSelectId["ExternalIDTypeID"] = "-2";
                drSelectId["ExternalIDTypeDescription"] = "--Select ID Type--";
                dtIdTypes.Rows.InsertAt(drSelectId, 0);

                ddlIDType.DataTextField = "ExternalIDTypeDescription";
                ddlIDType.DataValueField = "ExternalIDTypeID";
                ddlIDType.DataSource = dtIdTypes;
                ddlIDType.DataBind();

                Session["DT_IDTYPES"] = dtIdTypes;

                lblAddIDType.Visible = false;
                txtAddIDType.Visible = false;
                btnAddExternalID.Enabled = false;
                txtUserId.Enabled = false;
            }
            catch (Exception strEx)
            {
                throw strEx;
            }
        }

        #region Step1_11
        /// <summary>
        /// This function fills the devices into datagrid called dgdevices.
        /// This function calls stored procedure "getRPDevices"
        /// if No records is available then show it in Lable or simply not.
        /// </summary>    
        private void fillDevices()
        {

            try
            {
                OrderingClinician objOC = new OrderingClinician();
                DataTable dtDevice = objOC.GetOCDevice(int.Parse(ViewState[OCID].ToString()));


                dtGridDeviceNotifications = dtDevice.Copy();
                dtGridDeviceNotifications.Columns["FlagRowType"].ReadOnly = false;                
                dtGridDeviceNotifications.Columns["FlagModified"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["GroupName"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["DeviceAddress"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["Gateway"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["Carrier"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["EventDescription"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["RPNotifyEventID"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["FindingDescription"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["FindingID"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["GroupID"].ReadOnly = false;
                dtGridDeviceNotifications.Columns["InitialPause"].ReadOnly = false;

                Session[SessionConstants.DT_NOTIFICATION] = dtGridDeviceNotifications;
                RowNo = dtGridDeviceNotifications.Rows.Count;
                dataBindStep1();

            }
            catch (Exception ex)
            {
                if (ViewState[OCID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_OC.fillDevices():: Exception occured for User ID - " + ViewState[OCID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState[OCID]));
                }
                throw ex;
            }

        }

        /// <summary>
        /// This function fill the list of All Devices they have available
        /// this function  calls stored procedure "getDevices" to get all the list into drop down list box.
        /// </summary>
        private void fillDeviceDDL()
        {
            try
            {
                OrderingClinician objOC = new OrderingClinician();
                DataTable dtDeviceDDL = objOC.GetDevices();
                DataRow dr = dtDeviceDDL.NewRow();
                dr[0] = "-1";
                dr[1] = "-- Select Device To Add";
                dtDeviceDDL.Rows.InsertAt(dr, dtDeviceDDL.Rows.Count);
                cmbDeviceType.DataSource = dtDeviceDDL.DefaultView;
                cmbDeviceType.DataBind();
                cmbDeviceType.Items.FindByValue("-1").Selected = true;
                dtDeviceDDL = null;
                objOC = null;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.fillDeviceDDL():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }
        /// <summary>
        /// This function fills Groups which are maped to selected directory  
        /// into drop down list.
        /// </summary>
        /// <param name="cnx"></param>
        private void fillGroupDDL(DropDownList objDropdown,int? deviceType)
        {
            OrderingClinician objOC = null;
            DataTable dtGroup = null;
            string allGroupName;

            int selectedDeviceType = (deviceType != null) ? (int)deviceType : Convert.ToInt32(cmbDeviceType.SelectedValue);

            try
            {
                objOC = new OrderingClinician();
                if (selectedDeviceType == (int)NotificationDevice.SMS_WebLink)
                {
                    dtGroup = objOC.GetGroupsForOC(int.Parse(ViewState[OCID].ToString()), true);
                    allGroupName = "All Lab Groups";
                }
                else
                {
                    dtGroup = objOC.GetGroupsForOC(int.Parse(ViewState[OCID].ToString()), false);
                    allGroupName = "All Groups";
                }
                dtGroup.DefaultView.Sort = "GroupName";
                objDropdown.DataSource = dtGroup.DefaultView;
                objDropdown.DataBind();
                ListItem objLi = new ListItem();

                objLi.Text = allGroupName;
                objLi.Value = "-1";
                objDropdown.Items.Insert(objDropdown.Items.Count, objLi);
                objDropdown.SelectedValue = "-1";

                if (!objDropdown.ID.ToUpper().Equals("CMBGROUP"))
                {
                    ListItem objLi1 = new ListItem();
                    objLi1.Text = "-- None --";
                    objLi1.Value = "0";
                    objDropdown.Items.Insert(objDropdown.Items.Count, objLi1);
                }  

            }

            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.fillGroupDDL:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                objOC = null;
                dtGroup = null;
            }
        }
        /// <summary>
        /// This function is to fill Notification Events into drop down list.
        /// This function calls stored procedure "getRPNotifyEvents"
        /// </summary>
        /// <param name="cnx">Connection String</param>
        private void fillEventDDL(DropDownList objDropdown)
        {
            try
            {
                OrderingClinician objOC = new OrderingClinician();
                DataTable dtNotifyEvents = objOC.GetOCNotifyEvents();
                objDropdown.DataSource = dtNotifyEvents.DefaultView;
                objDropdown.DataBind();

                if (!objDropdown.ID.ToUpper().Equals("CMBEVENTS"))
                {
                    ListItem objLi1 = new ListItem();
                    objLi1.Text = "-- None --";
                    objLi1.Value = "0";
                    objDropdown.Items.Insert(objDropdown.Items.Count, objLi1);
                }  

                dtNotifyEvents = null;
                objOC = null;

            }

            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.fillEventDDL:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }

        }
        /// <summary>
        /// This function fills Findings into drop down list as par group selection.
        /// This function calls stored procedure "getFindingOptionsForSubscriber"
        /// </summary>
        /// <param name="cnx"></param>
        private void fillFindingDDL()
        {
            try
            {
                OrderingClinician objOC = new OrderingClinician();
                DataTable dtFindings = null;

                if (int.Parse(cmbGroup.SelectedValue) == -1)
                {
                    cmbFindings.Items.Clear();
                    ListItem objLi = new ListItem();
                    objLi.Text = "All Findings";
                    objLi.Value = "-1";
                    cmbFindings.Items.Insert(0, objLi);
                }
                else
                {
                    dtFindings = objOC.GetFindingForOCorGroup(-1, int.Parse(cmbGroup.SelectedValue));

                    dtFindings.DefaultView.Sort = "FindingID";
                    cmbFindings.DataSource = dtFindings.DefaultView;
                    cmbFindings.DataBind();
                    ListItem objLi = new ListItem();
                    objLi.Text = "All Findings";
                    objLi.Value = "-1";
                    cmbFindings.Items.Insert(cmbFindings.Items.Count, objLi);
                    cmbFindings.SelectedValue = "-1";

                    dtFindings = null;
                    objOC = null;
                }
            }

            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.fillFindingDDL:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }

        }
        /// <summary>
        /// This function fills Findings into drop down list as par group selection.
        /// This function calls stored procedure "getFindingOptionsForSubscriber"
        /// </summary>
        /// <param name="cnx"></param>
        private void fillGridFindingDDL(int groupID, DropDownList objFiding, string findingID)
        {
            try
            {
                OrderingClinician objOC = new OrderingClinician();
                DataTable dtFindings = null;
                
                objFiding.Items.Clear();

                if (groupID == -1)
                {                  
                    ListItem objLi = new ListItem();
                    objLi.Text = "All Findings";
                    objLi.Value = "-1";
                    objFiding.Items.Insert(0, objLi);

                    findingID = "-1";                    
                }
                else if(groupID > 0)
                {
                    dtFindings = objOC.GetFindingForOCorGroup(Convert.ToInt32(ViewState[OCID]), groupID);

                    dtFindings.DefaultView.Sort = "FindingID";
                    objFiding.DataSource = dtFindings.DefaultView;
                    objFiding.DataBind();
                    ListItem objLi = new ListItem();
                    objLi.Text = "All Findings";
                    objLi.Value = "-1";
                    objFiding.Items.Insert(objFiding.Items.Count, objLi);                    
                    dtFindings = null;
                    objOC = null;
                }
                
                if(!objFiding.ID.ToUpper().Equals("CMBFINDINGS") && groupID != -1)
                {
                    ListItem objLi1 = new ListItem();
                    objLi1.Text = "-- None --";
                    objLi1.Value = "0";
                    objFiding.Items.Insert(objFiding.Items.Count, objLi1);
                }
                objFiding.SelectedValue = findingID;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.fillFindingDDL:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }

        }
        /// <summary>
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
        /// This method creates a table structure which hold OC Device 
        /// Notification information and After Hours information.
        /// </summary>
        private void CreateTable()
        {
            try
            {
                //if (dtGridDeviceNotifications.Columns.Count <= 0)
                //{
                //    dtGridDeviceNotifications.Rows.Clear();
                //    dtGridDeviceNotifications.Columns.Add("DeviceName", Type.GetType("System.String"));
                //    dtGridDeviceNotifications.Columns.Add("DeviceAddress", Type.GetType("System.String"));
                //    dtGridDeviceNotifications.Columns.Add("Gateway", Type.GetType("System.String"));
                //    dtGridDeviceNotifications.Columns.Add("Carrier", Type.GetType("System.String"));
                //    dtGridDeviceNotifications.Columns.Add("GroupName", Type.GetType("System.String"));
                //    dtGridDeviceNotifications.Columns.Add("EventDescription", Type.GetType("System.String"));
                //    dtGridDeviceNotifications.Columns.Add("FindingDescription", Type.GetType("System.String"));
                //    dtGridDeviceNotifications.Columns.Add("InitialPause", Type.GetType("System.String"));
                //    dtGridDeviceNotifications.Columns.Add("DeviceID", Type.GetType("System.Int32"));
                //    dtGridDeviceNotifications.Columns.Add("GroupID", Type.GetType("System.Int32"));
                //    dtGridDeviceNotifications.Columns.Add("RPNotifyEventID", Type.GetType("System.Int32"));
                //    dtGridDeviceNotifications.Columns.Add("FindingID", Type.GetType("System.Int32"));
                //    dtGridDeviceNotifications.Columns.Add("RowID", Type.GetType("System.Int32"));
                //    dtGridDeviceNotifications.Columns.Add("RPDeviceID", Type.GetType("System.Int32"));
                //    dtGridDeviceNotifications.Columns.Add("FlagRowType", Type.GetType("System.String"));
                //    dtGridDeviceNotifications.Columns.Add("FlagModified", Type.GetType("System.String"));
                //}
                //if (dtGridAfterHours.Columns.Count <= 0)
                //{
                //    dtGridAfterHours.Rows.Clear();
                //    dtGridAfterHours.Columns.Add("DeviceName", Type.GetType("System.String"));
                //    dtGridAfterHours.Columns.Add("GroupName", Type.GetType("System.String"));
                //    dtGridAfterHours.Columns.Add("FindingDescription", Type.GetType("System.String"));
                //    dtGridAfterHours.Columns.Add("StartHour", Type.GetType("System.Int32"));
                //    dtGridAfterHours.Columns.Add("EndHour", Type.GetType("System.Int32"));
                //    dtGridAfterHours.Columns.Add("RPDeviceID", Type.GetType("System.Int32"));
                //    dtGridAfterHours.Columns.Add("GroupID", Type.GetType("System.Int32"));
                //    dtGridAfterHours.Columns.Add("FindingID", Type.GetType("System.Int32"));
                //    dtGridAfterHours.Columns.Add("AfterHourRowNo", Type.GetType("System.Int32"));
                //    dtGridAfterHours.Columns.Add("RPAfterHoursNotificationID", Type.GetType("System.Int32"));
                //    dtGridAfterHoursColumns.Add("FlagRowType", Type.GetType("System.String"));
                //    dtGridAfterHours.Columns.Add("FlagModified", Type.GetType("System.String"));
                //    dtGridAfterHours.Columns.Add("RPDeviceID", Type.GetType("System.Int32"));


                //}
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.CreateTable:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }

        }
        /// <summary>
        /// This function generate GatewayAddress for selected Devices Carriers
        /// If any error occurred during generating GateWayAddress, error log creates
        /// </summary>
        private void generateGatewayAddress()
        {
            try
            {
                int deviceID = int.Parse(cmbDeviceType.SelectedItem.Value);
                DataSet carriers;
                DataRow[] rows;
                IEnumerator e;
                switch (deviceID)
                {
                    case (int)NotificationDevice.SMS: // Cell Phones
                    case (int)NotificationDevice.SMS_WebLink: //sms_weblink
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
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID].ToString().Length > 0)
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.generateGatewayAddress:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }

        }
        /// <summary>
        /// This function is to set Labels and Input boxes for select Devices Carrier from Drop down box list.
        /// Error log is created if any Exception occurred while process.
        /// </summary>
        /// <param name="deviceID">Integer Type</param>
        private void setLabelsAndInputBoxes(int deviceID)
        {
            try
            {
                ListItem li = new ListItem("-- Select Carrier", "-1");
                DataSet ds = new DataSet();
                DataView dv = new DataView();

                cmbGroup.Visible = false;
                lblGroup.Visible = false;
                cmbEvents.Visible = false;
                lblEvents.Visible = false;
                cmbFindings.Visible = false;
                lblFindings.Visible = false;
                if (cmbEvents.Items.Count > 0)
                    cmbEvents.SelectedIndex = 0;
                cmbGroup.SelectedValue = "-1";
                fillFindingDDL();
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
                    case (int)NotificationDevice.SMS_WebLink:
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
                        //cmbFindings.Visible = true;
                        //lblEvents.Visible = true;
                        //cmbEvents.Visible = true;
                        //lblFindings.Visible = true;
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
                        //cmbFindings.Visible = true;
                        //cmbEvents.Visible = true;
                        //lblEvents.Visible = true;
                        //lblFindings.Visible = true;
                        txtInitialPause.Visible = false;
                        txtInitialPause.Text = "";
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
                        //cmbFindings.Visible = true;
                        //cmbEvents.Visible = true;
                        //lblEvents.Visible = true;
                        //lblFindings.Visible = true;
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
                        //cmbFindings.Visible = true;
                        //lblFindings.Visible = true;
                        btnAddDevice.Visible = true;
                       
                        //cmbEvents.Visible = true;
                        //lblEvents.Visible = true;
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
                        //lblFindings.Visible = true;
                        //cmbFindings.Visible = true;
                        //lblEvents.Visible = true;
                        //cmbEvents.Visible = true;
                        txtInitialPause.Visible = false;
                        txtInitialPause.Text = "";
                        lblInitialPause.Visible = false;
                        break;

                    case (int)NotificationDevice.DesktopAlert:  // Desktop Alert
                        txtNumAddress.Text = string.Empty;
                        txtNumAddress.Visible = false;
                        txtNumAddress.Text = "";
                        lblNumAddress.Visible = false;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtEmailGateway.Visible = false;
                        txtEmailGateway.Text = "";
                        lblEmailGateway.Visible = false;
                        btnAddDevice.Visible = true;                       
                        //cmbFindings.Visible = true;
                        //lblFindings.Visible = true;
                        //cmbEvents.Visible = true;
                        //lblEvents.Visible = true;
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
                        //cmbFindings.Visible = true;
                        //lblFindings.Visible = true;
                        //cmbEvents.Visible = true;
                        //lblEvents.Visible = true;
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
                        //cmbFindings.Visible = true;
                        //lblFindings.Visible = true;
                        //cmbEvents.Visible = true;
                        //lblEvents.Visible = true;
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
                        txtNumAddress.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes(textChangedClientID);");
                        txtNumAddress.AutoPostBack = false;
                        lblNumAddress.Visible = true;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtEmailGateway.Text = "Enter TAP 800 number (numbers only)";
                        //txtEmailGateway.MaxLength = 10;
                        txtEmailGateway.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes(textChangedClientID);");
                        txtEmailGateway.Attributes.Add("onclick", "RemoveGatewayLabel('" + txtEmailGateway.ClientID + "','" + hidGatewayLabel.ClientID + "');");
                        txtEmailGateway.Visible = true;
                        lblEmailGateway.Visible = true;
                        lblEmailGateway.Text = "Email Gateway";
                        btnAddDevice.Visible = true;                       
                        //cmbFindings.Visible = true;
                        //lblFindings.Visible = true;
                        //cmbEvents.Visible = true;
                        //lblEvents.Visible = true;
                        txtInitialPause.Visible = false;
                        txtInitialPause.Text = "";
                        lblInitialPause.Visible = false;
                        break;
                    default:
                        txtNumAddress.Visible = false;
                        txtNumAddress.Text = "";
                        lblNumAddress.Visible = false;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtEmailGateway.Visible = false;
                        txtEmailGateway.Text = "";
                        lblEmailGateway.Visible = false;
                        //cmbFindings.Visible = false;
                        //cmbEvents.Visible = false;
                        txtInitialPause.Visible = false;
                        txtInitialPause.Text = "";
                        //lblEvents.Visible = false;
                        //lblFindings.Visible = false;
                        btnAddDevice.Visible = false;                       
                        lblInitialPause.Visible = false;
                        break;

                }

            }

            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.setLabelsAndInputBoxes:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }

        }
        /// <summary>
        /// Populates all the devices and preferences for the selected Department in the Datagrid grdDevices
        /// </summary>
        /// <param name="cnx"></param>
        private void addOCNotificationDevices()
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
                    if (cmbDeviceType.Items.Count > 0 && cmbDeviceType.SelectedItem.Text.Trim().Equals("Email"))
                    {
                        deviceAdd = txtEmailGateway.Text.Trim().Trim();
                    }

                    //Set Gateway property value            
                    if (int.Parse(cmbDeviceType.SelectedValue) == (int)NotificationDevice.PagerTAP || int.Parse(cmbDeviceType.SelectedValue) == (int)NotificationDevice.PagerTAPA) //TAP DEVICE
                    {
                        gateway = txtEmailGateway.Text.Trim();
                    }
                    else if ((!(cmbDeviceType.Items.Count > 0 && cmbDeviceType.SelectedItem.Text.Trim().Equals("Email"))) && txtEmailGateway.Text.Trim().Length > 0)
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

                    RowNo++;
                    string flagModified = "UnChanged";
                    string flagRowType = "New";
                    DataRow dtrow = dtGridDeviceNotifications.NewRow();
                    dtrow["DeviceName"] = generateDeviceName(Convert.ToInt32(cmbDeviceType.SelectedValue));
                    dtrow["DeviceAddress"] = deviceAdd;
                    dtrow["Gateway"] = gateway;
                    dtrow["Carrier"] = carrier;
                    dtrow["GroupName"] = (cmbGroup.Visible) ? cmbGroup.SelectedItem.Text : " ";
                    dtrow["EventDescription"] = (cmbEvents.Visible)? cmbEvents.SelectedItem.Text : " ";
                    dtrow["FindingDescription"] = (cmbFindings.Visible) ? cmbFindings.SelectedItem.Text : " ";
                    dtrow["InitialPause"] = initialPause;
                    dtrow["DeviceID"] = Convert.ToInt32(cmbDeviceType.SelectedValue);
                    dtrow["GroupID"] = (cmbGroup.Visible) ? Convert.ToInt32(cmbGroup.SelectedValue) : 0;
                    dtrow["RPNotificationID"] = 0;
                    dtrow["GroupName"] = (cmbGroup.Visible) ? cmbGroup.SelectedItem.Text : " ";
                    dtrow["RPNotifyEventID"] = (cmbEvents.Visible) ? Convert.ToInt32(cmbEvents.SelectedValue) : 0;
                    dtrow["FindingID"] = (cmbFindings.Visible) ? Convert.ToInt32(cmbFindings.SelectedValue) : 0;
                    dtrow["RowID"] = RowNo;
                    dtrow["FlagRowType"] = flagRowType;
                    dtrow["FlagModified"] = flagModified;
                    dtrow["RPID"] = Convert.ToInt32(ViewState[OCID]);
                    //dtrow["RPDeviceID"] = RowNo;

                    script = "alert('Device has been added')";
                    dtGridDeviceNotifications.Rows.Add(dtrow);

                    Session[SessionConstants.DT_NOTIFICATION] = dtGridDeviceNotifications;
                    dataBindStep1();
                    resetControls();
                    fillAfterHoursDeviceOptions();
                    generateStep3DataGridHeight();
                    upnlStep3.Update();

                    ScriptManager.RegisterStartupScript(upnlHidden, upnlHidden.GetType(), "Alert", script, true);


                }
                else
                {
                    //generateDataGridHeight("Validate");
                }

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID].ToString().Length > 0)
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.addOCNotificationDevices():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
        }
        /// <summary>
        /// This Method generates device name by adding number with sort description
        /// of device.
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
                //if (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.SMS_WebLink)
            //{
            //    bool isLabGroup = true;
            //    if (cmbGroup.SelectedValue != "-1")
            //    {
            //        OrderingClinician objOC = new OrderingClinician();
            //        isLabGroup = objOC.IsLabGroup(Convert.ToInt32(cmbGroup.SelectedValue));
            //        objOC = null;
            //    }
            //    else
            //        isLabGroup = false;

            //    if (!isLabGroup)
            //    {
            //        focus = cmbGroup.ClientID;

            //        StringBuilder acRegScript = new StringBuilder();
            //        acRegScript.Append("<script type=\"text/javascript\">");
            //        acRegScript.AppendFormat("document.getElementById(" + '"' + OCDeviceDiv.ClientID + '"' + ").style.height='" + getDataGridHeight() + "';");
            //        acRegScript.AppendFormat("alert('This device is available only for Lab group');");
            //        acRegScript.Append("document.getElementById('" + focus + "').focus();");
            //        acRegScript.Append("</script>");
            //        ScriptManager.RegisterClientScriptBlock(upnlStep1, upnlStep1.GetType(), "Register3", acRegScript.ToString(), false);
            //        generateDataGridHeight("validatedevice3");
            //        return false;
            //    }

            //}
            //else
            {
                string deviceAdd = txtNumAddress.Text.Trim();
                if (int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.EMail && int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.DesktopAlert)
                {
                    string error = "";

                    if (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.SMS || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.SMS_WebLink || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.Fax)
                    {
                        if (deviceAdd.Length != 10 )
                        {
                            error = "Please enter valid Number\\n";
                            focus = txtNumAddress.ClientID;
                        }
                    }

                    if (int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.PagerAlpha && int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.PagerNumRegular && int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.PagerNumSkyTel && int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.PagerNumUSA && int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.pagerPartner)
                    {
                        if (!Utils.isNumericValue(deviceAdd))
                        {
                            error = "Please enter valid Number\\n";
                            focus = txtNumAddress.ClientID;
                        }
                    }

                    if (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerAlpha || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.SMS || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.SMS_WebLink)
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
        /// Resets all control to their default values.
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

            cmbGroup.Visible = false;
            lblGroup.Visible = false;
            cmbFindings.Visible = false;
            cmbFindings.SelectedValue = "-1";
            lblFindings.Visible = false;
            txtInitialPause.Visible = false;
            lblInitialPause.Visible = false;
            btnAddDevice.Visible = false;
            btnShowHideDetails.Visible = false;
        }
        /// <summary>        
        /// Set the EditItemIndex of grdDevices to the selected index, calls getDeptNotificationPreferences() method
        /// to bind the data grid again, calls fillEventDDL() and fillFindingDDL() methods to fill the "Event" and "Finding"
        /// dropdown in the selected row of data grid in respective columns.        
        /// </summary>
        /// <param name="intGridEditItemIndex"></param>
        /// <param name="strEventText"></param>
        /// <param name="strFindingText"></param>
        private void editDeviceGrid(int intGridEditItemIndex, string strEventID, string findingID, string deviceName, string deviceGatway, int groupID)
        {
            try
            {
                dataBindStep1();

                TextBox tbDevice = ((TextBox)(grdDevices.Items[intGridEditItemIndex].Cells[1].FindControl("txtGridDeviceNumber")));
                TextBox tbEmailGateway = ((TextBox)(grdDevices.Items[intGridEditItemIndex].Cells[4].FindControl("txtGridEmailGateway")));
                DropDownList dlEvent = ((DropDownList)(grdDevices.Items[intGridEditItemIndex].Cells[5].FindControl("dlistGridEvents")));
                DropDownList dlFinding = ((DropDownList)(grdDevices.Items[intGridEditItemIndex].Cells[6].FindControl("dlistGridFindings")));
                DropDownList dlGroups = ((DropDownList)(grdDevices.Items[intGridEditItemIndex].Cells[1].FindControl("dlistGridGroups")));
                TextBox gridInitialPauseTxt = ((TextBox)(grdDevices.Items[intGridEditItemIndex].Cells[7].FindControl("txtGridInitialPause")));

                int deviceType = int.Parse(grdDevices.Items[intGridEditItemIndex].Cells[10].Text);
                
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
                    tbDevice.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes(textChangedClientID);");
                    //tbDevice.MaxLength = 100;
                }
                else if (deviceType != (int)NotificationDevice.EMail)
                {
                    tbDevice.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes(textChangedClientID);");
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
                    txtEmailGateway.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes(textChangedClientID);");
                    //txtEmailGateway.MaxLength = 10;
                }
                else
                {
                    txtEmailGateway.Attributes.Add("onkeypress", "JavaScript:return true;");
                    txtEmailGateway.MaxLength = 100;
                }

                fillEventDDL(dlEvent);
                fillGroupDDL(dlGroups,deviceType);
                fillGridFindingDDL(groupID, dlFinding, findingID);

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
                dlGroups.SelectedValue = groupID.ToString();

                if (tbDevice != null)
                    ScriptManager.GetCurrent(this).SetFocus(tbDevice.ClientID);
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("edit_rp.editDeviceGrid:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
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
            int deviceType = int.Parse(grdDevices.Items[e.Item.ItemIndex].Cells[10].Text.Trim());
            TextBox gridDeviceTypeTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[0].FindControl("txtGridDeviceType")));
            TextBox gridDeviceNumberTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[1].FindControl("txtGridDeviceNumber")));
            TextBox gridEmailGatewayTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[4].FindControl("txtGridEmailGateway")));
            TextBox gridInitialPauseTxt = ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[7].FindControl("txtGridInitialPause")));
            
            DropDownList dlEvent = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[5].FindControl("dlistGridEvents")));
            DropDownList dlFinding = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[6].FindControl("dlistGridFindings")));
            DropDownList dlGroups = ((DropDownList)(grdDevices.Items[e.Item.ItemIndex].Cells[1].FindControl("dlistGridGroups")));
            int selectedEventID = Convert.ToInt32(dlEvent.SelectedValue);
            int selectedGroupID = Convert.ToInt32(dlGroups.SelectedValue);
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
                errorMessage = "Please enter NotificationDevice Type.\\n";

                gridDeviceTypeTxt.Focus();
            }

            //Validation for all Notification details or not
            if (!(selectedEventID == 0 && selectedGroupID == 0 && selectedFindingID == 0))
            {
                if (!((selectedEventID == -1 || selectedEventID > 0) && (selectedGroupID == -1 || selectedGroupID > 0) && (selectedFindingID == -1 || selectedFindingID > 0)))
                {
                    errorMessage += "Either select all notification details (Group, Event and Finding) or none.\\n";
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
                    string strPause = gridInitialPauseTxt.Text.Trim();
                    if (strPause != string.Empty)
                    {
                        if (Utils.isDecimalValue(gridInitialPauseTxt.Text.Trim()))
                        {
                            if (strPause.IndexOf(".") == strPause.LastIndexOf("."))
                            {
                                if (Convert.ToDecimal(strPause) >= 1 && Convert.ToDecimal(strPause) < Convert.ToDecimal("30.99"))
                                {

                                }
                                else
                                {
                                    gridInitialPauseTxt.Focus();
                                    errorMessage += "Initial pause time should be betweeen 1 to 30.99\\n";
                                }
                            }
                            else
                            {
                                if ((!strPause.Contains(".")) && (Convert.ToDecimal(strPause) < 1 && Convert.ToDecimal(strPause) >= 31))
                                {
                                    gridInitialPauseTxt.Focus();
                                    errorMessage += "Initial pause time should be betweeen 1 to 30.99\\n";
                                }
                            }
                        }
                        else
                        {
                            gridInitialPauseTxt.Focus();
                            errorMessage += "Please enter valid initial pause value\\n";
                        }
                    }
                    else
                    {
                        gridInitialPauseTxt.Focus();
                        errorMessage += "Please enter valid initial pause value\\n";
                    }
                    break;
                case (int)NotificationDevice.SMS:
                    if (gridDeviceNumberTxt.Text.Trim().Length != 10)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter valid Number\\n";
                    }
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
                case (int)NotificationDevice.SMS_WebLink:
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
                generateDataGridHeight("dataBind");
                ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "Grid_Alert", "alert('" + errorMessage + "');", true);
                returnVal = false;
            }
            return returnVal;
        }
        /// <summary>
        /// This method bind the grddevice grid.
        /// </summary>
        private void dataBindStep1()
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
        /// This function is to calculate dynamic height for grdDevices datagrid
        /// </summary>
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
        /// This method Add/Edit/Delete OC devices, assigned events to the devices and 
        /// Add/Edit/Delte After Hour Notifications in to the Database.
        /// </summary>
        /// <param name="OCID">Ordering Clinician ID</param>
        private void editDevicesAndAfterHours()
        {
            if (dtGridDeviceNotifications.Rows.Count > 0)
            {
                try
                {
                    int numOfDevices = dtGridDeviceNotifications.Rows.Count;
                    for (int countDevices = 0; countDevices < numOfDevices; countDevices++)
                    {
                        OrderingClinician objOC = new OrderingClinician();
                        OCDeviceInfo objDevice = new OCDeviceInfo();
                        int deviceID = 0;
                        string flageRowType;
                        string flageModified;
                        objDevice.ReferringPhysicianID = Convert.ToInt32(ViewState[OCID]);
                        objDevice.DeviceID = dtGridDeviceNotifications.Rows[countDevices]["DeviceID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["DeviceID"]);
                        objDevice.DeviceAddress = dtGridDeviceNotifications.Rows[countDevices]["DeviceAddress"] == DBNull.Value ? "" : dtGridDeviceNotifications.Rows[countDevices]["DeviceAddress"].ToString();
                        objDevice.DeviceName = dtGridDeviceNotifications.Rows[countDevices]["DeviceName"] == DBNull.Value ? "" : dtGridDeviceNotifications.Rows[countDevices]["DeviceName"].ToString();
                        objDevice.Gateway = dtGridDeviceNotifications.Rows[countDevices]["Gateway"] == DBNull.Value ? "" : dtGridDeviceNotifications.Rows[countDevices]["Gateway"].ToString();
                        objDevice.Carrier = dtGridDeviceNotifications.Rows[countDevices]["Carrier"] == DBNull.Value ? "" : dtGridDeviceNotifications.Rows[countDevices]["Carrier"].ToString();
                        objDevice.InitialPauseTime = dtGridDeviceNotifications.Rows[countDevices]["InitialPause"] == DBNull.Value ? "-1" : dtGridDeviceNotifications.Rows[countDevices]["InitialPause"].ToString();
                        objDevice.GroupID = dtGridDeviceNotifications.Rows[countDevices]["GroupID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["GroupID"]);
                        objDevice.FindingID = dtGridDeviceNotifications.Rows[countDevices]["FindingID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["FindingID"]);
                        objDevice.OCNotifyEventID = dtGridDeviceNotifications.Rows[countDevices]["RPNotifyEventID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["RPNotifyEventID"]);
                        objDevice.OCDeviceID = dtGridDeviceNotifications.Rows[countDevices]["RPDeviceID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["RPDeviceID"]);
                        objDevice.OCNotificationID = dtGridDeviceNotifications.Rows[countDevices]["RPNotificationID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["RPNotificationID"]);

                        if (objDevice.InitialPauseTime == "0")
                            objDevice.InitialPauseTime = "-1";

                        flageRowType = dtGridDeviceNotifications.Rows[countDevices]["FlagRowType"].ToString();
                        flageModified = dtGridDeviceNotifications.Rows[countDevices]["FlagModified"].ToString();

                        if (flageRowType == "New")
                        {
                            if (flageModified != "Deleted")
                            {
                                deviceID = objOC.InsertOCDevice(objDevice, "EDIT");// get new OCdeviceID which is added recently.
                                //select AfterHour with of new data which is not deleted
                                DataRow[] drAfterHour = dtGridAfterHours.Select("DeviceName = '" + objDevice.DeviceName + "' AND FlagModified <> 'Deleted'");
                                if (drAfterHour.GetLength(0) > 0)
                                {
                                    int numofAH = drAfterHour.GetLength(0);
                                    for (int countAH = 0; countAH < numofAH; countAH++)
                                    {
                                        objDevice.StartHour = drAfterHour[countAH]["StartHour"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["StartHour"]);
                                        objDevice.EndHour = drAfterHour[countAH]["EndHour"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["EndHour"]);
                                        objDevice.GroupID = drAfterHour[countAH]["GroupID"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["GroupID"]);
                                        objDevice.FindingID = drAfterHour[countAH]["FindingID"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["FindingID"]);
                                        objDevice.OCDeviceID = deviceID;

                                        objOC.InsertAfterHoursNotifications(objDevice, "EDIT");

                                    }
                                }
                                drAfterHour = null;
                            }
                        }
                        else // If Existing Devices
                        {
                            DataRow[] drAfterHour = dtGridAfterHours.Select("DeviceName = '" + objDevice.DeviceName + "'");
                            int numofAH = 0;

                            if (drAfterHour.GetLength(0) > 0)
                                numofAH = drAfterHour.GetLength(0);

                            if (flageModified == "Deleted") //if device is Deleted then delete AfterHours and Devices 
                            {
                                //First Delete all AfterHourNotification from DB
                                for (int countAH = 0; countAH < numofAH; countAH++)
                                {
                                    int RPNotificationID = Convert.ToInt32(drAfterHour[countAH]["RPAfterHoursNotificationID"]);
                                    objOC.DeleteAfterHoursNotifications(RPNotificationID);
                                }
                                //Second delete Device
                                objOC.DeleteDeviceAndEvent(objDevice.OCDeviceID, objDevice.OCNotificationID);
                            }
                            else if (flageModified == "Changed") // if device is modified
                            {
                                deviceID = objOC.UpdateOCDevice(objDevice); //update deviece & event.

                                for (int countAH = 0; countAH < numofAH; countAH++)
                                {
                                    string ahFlagrowType = drAfterHour[countAH]["FlagRowType"].ToString();
                                    string ahFlagModified = drAfterHour[countAH]["FlagModified"].ToString();

                                    if (ahFlagrowType == "New" && ahFlagModified != "Deleted")
                                    {
                                        objDevice.StartHour = drAfterHour[countAH]["StartHour"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["StartHour"]);
                                        objDevice.EndHour = drAfterHour[countAH]["EndHour"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["EndHour"]);
                                        objDevice.GroupID = drAfterHour[countAH]["GroupID"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["GroupID"]);
                                        objDevice.FindingID = drAfterHour[countAH]["FindingID"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["FindingID"]);

                                        objOC.InsertAfterHoursNotifications(objDevice, "EDIT");

                                    }
                                    else if (ahFlagrowType == "Existing" && ahFlagModified == "Deleted")
                                    {
                                        int RPNotificationID = Convert.ToInt32(drAfterHour[countAH]["RPAfterHoursNotificationID"]);
                                        objOC.DeleteAfterHoursNotifications(RPNotificationID);
                                    }

                                }
                            }
                            else if (flageModified == "Unchanged") // Device is not updated then chek for its afterhours addition and deletion
                            {
                                for (int countAH = 0; countAH < numofAH; countAH++)
                                {
                                    string ahFlagrowType = drAfterHour[countAH]["FlagRowType"].ToString();
                                    string ahFlagModified = drAfterHour[countAH]["FlagModified"].ToString();

                                    if (ahFlagrowType == "New" && ahFlagModified != "Deleted")
                                    {
                                        objDevice.StartHour = drAfterHour[countAH]["StartHour"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["StartHour"]);
                                        objDevice.EndHour = drAfterHour[countAH]["EndHour"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["EndHour"]);
                                        objDevice.GroupID = drAfterHour[countAH]["GroupID"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["GroupID"]);
                                        objDevice.FindingID = drAfterHour[countAH]["FindingID"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["FindingID"]);

                                        objOC.InsertAfterHoursNotifications(objDevice, "EDIT");

                                    }
                                    else if (ahFlagrowType == "Existing" && ahFlagModified == "Deleted")
                                    {
                                        int RPNotificationID = Convert.ToInt32(drAfterHour[countAH]["RPAfterHoursNotificationID"]);
                                        objOC.DeleteAfterHoursNotifications(RPNotificationID);
                                    }

                                }

                            }
                            drAfterHour = null;
                        }
                        objDevice = null;
                        objOC = null;

                    }
                }
                catch (Exception ex)
                {
                    if (Session[SessionConstants.USER_ID] != null)
                    {
                        Tracer.GetLogger().LogExceptionEvent("edit_OC.editDevicesAndAfterHours:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                    }
                    throw ex;
                }

            }
        }

        #endregion Step1_1
        #region Step2_22
        /// <summary>
        /// This function is to fill findings for Fill After Hours into drop down list 
        /// as par group selection.
        /// This function calls stored procedure "getFindingOptionsForSubscriber"
        /// </summary>
        private void fillAfterHoursFindingDDL()
        {
            OrderingClinician objOC = null;
            DataTable dtFindings = null;
            try
            {
                objOC = new OrderingClinician();

                if (int.Parse(cmbStep3Groups.SelectedValue) == -1)
                {
                    cmbAHFindings.Items.Clear();
                    ListItem objLi = new ListItem();
                    objLi.Text = "All Findings";
                    objLi.Value = "-1";
                    cmbAHFindings.Items.Insert(0, objLi);
                }
                else
                {
                    dtFindings = objOC.GetFindingForOCorGroup(-1, int.Parse(cmbStep3Groups.SelectedValue));
                    dtFindings.DefaultView.Sort = "FindingID";
                    cmbAHFindings.DataSource = dtFindings.DefaultView;
                    cmbAHFindings.DataBind();
                    ListItem objLi = new ListItem();
                    objLi.Text = "All Findings";
                    objLi.Value = "-1";
                    cmbAHFindings.Items.Insert(cmbAHFindings.Items.Count, objLi);
                    cmbAHFindings.SelectedValue = "-1";
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.fillAfterHoursFindingDDL:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                dtFindings = null;
                objOC = null;
            }

        }

        /// <summary>
        /// This function is to set dynamic height of data grid for 
        /// After Hours Notification grdAfterHoursNotifications 
        /// </summary>
        private void generateStep3DataGridHeight()
        {
            int nDevicesGridHeight = 20;
            int gridItemHeight = 21;
            int gridHeaderHeight = 26;
            int maxRows = 4;

            if (grdAfterHoursNotifications.Items.Count < maxRows)
            {
                if (grdAfterHoursNotifications.Items.Count <= 1)
                    nDevicesGridHeight = (grdAfterHoursNotifications.Items.Count * gridItemHeight) + gridHeaderHeight + 10;
                else
                    nDevicesGridHeight = (grdAfterHoursNotifications.Items.Count * gridItemHeight) + gridHeaderHeight;
            }
            else
            {
                nDevicesGridHeight = (maxRows * gridItemHeight) + gridHeaderHeight;
            }

            string script = "<script type=\"text/javascript\">";
            script += "document.getElementById(" + '"' + "AfterHoursNotificationsDiv" + '"' + ").style.height='" + (nDevicesGridHeight + 1) + "';</script>";
            ScriptManager.RegisterStartupScript(upnlStep3, upnlStep3.GetType(), "Setdata3", script, false);
        }

        /// <summary>
        /// This function fills Groups which are maped to selected directory  
        /// into drop down list.
        /// </summary>
        private void fillGroupDDLStep3()
        {
            OrderingClinician objOC = null;
            DataTable dtGroup = null;
            try
            {
                objOC = new OrderingClinician();
                dtGroup = objOC.GetGroupsForOC(int.Parse(ViewState[OCID].ToString()), false);
                dtGroup.DefaultView.Sort = "GroupName";
                cmbStep3Groups.DataSource = dtGroup.DefaultView;
                cmbStep3Groups.DataBind();

                ListItem objLi = new ListItem();
                objLi.Text = "All Groups";
                objLi.Value = "-1";
                cmbStep3Groups.Items.Insert(cmbStep3Groups.Items.Count, objLi);
                cmbStep3Groups.SelectedValue = "-1";
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.fillGroupDDLStep3:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                objOC = null;
                dtGroup = null;
            }
        }

        /// <summary>
        /// This function fills device for Fill After Hours  into drop down list box.
        /// </summary>
        private void fillAfterHoursDeviceOptions()
        {
            try
            {
                DataRow[] drAfterHourDevices = dtGridDeviceNotifications.Select("DeviceID in (" + NotificationDevice.EMail.GetHashCode() + "," + NotificationDevice.Fax.GetHashCode() + ") AND FlagModified <> 'Deleted'");
                DataTable dtUniqueRows = GetUniqueRows(drAfterHourDevices);

                cmbAHDevice.DataSource = dtUniqueRows.DefaultView;
                cmbAHDevice.DataBind();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_oc.fillAfterHoursDeviceOptions:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }

        }

        /// <summary>
        /// Get Unique Rows from datatable
        /// </summary>
        /// <param name="dtAfterHoursDevices"></param>
        /// <returns>DataTable</returns>            
        private DataTable GetUniqueRows(DataRow[] drAfterHourDevices)
        {
            DataTable dtUniqueRows = dtGridDeviceNotifications.Clone();

            foreach (DataRow dr in drAfterHourDevices)
            {
                if (dtUniqueRows.Select("DeviceName='" + dr["DeviceName"].ToString() + "'").Length == 0)
                {
                    dtUniqueRows.Rows.Add(dr.ItemArray);
                }
            }

            return dtUniqueRows;
        }

       
        /// <summary>
        /// This method binds the after hours data to the grdAfterHoursNotifications data grid
        /// </summary>
        private void dataBindStep2()
        {

            DataView dvAfterHourDevices = new DataView(dtGridAfterHours);
            //Filter dataview with only Unchanged, Changed devices
            dvAfterHourDevices.RowFilter = "FlagModified in ('Unchanged', 'Changed')"; //Filter Deleted Rows
            dvAfterHourDevices.Sort = "DeviceName, GroupName, FindingDescription ASC";
            grdAfterHoursNotifications.DataSource = dvAfterHourDevices;
            grdAfterHoursNotifications.DataBind();

            if (dvAfterHourDevices.Count > 0)
                lblNoRecordsStep3.Visible = false;
            else
            {
                lblNoRecordsStep3.Visible = true;
                cmbAHFindings.SelectedValue = "-1";
                cmbStep3Groups.SelectedValue = "-1";
                cmbAHStartHour.SelectedItem.Text = "12 noon";
                cmbAHEndHour.SelectedItem.Text = "12 noon";
            }
            generateStep3DataGridHeight();
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
        /// <summary>
        /// This function is to fill datagrid dgAfterHoursNotifications for logged in users.
        /// This function calls stored procedure "getRPAfterHoursNotifications"
        /// if there is no records found then it is shown into label.
        /// </summary>
        /// <param name="cnx">Connection String</param>
        private void fillAfterHoursNotifications()
        {
            OrderingClinician objOC = null;
            try
            {
                objOC = new OrderingClinician();
                DataTable dtNotifications = objOC.GetAfterHoursNotification(int.Parse(ViewState[OCID].ToString()));

                dtGridAfterHours = dtNotifications.Copy();
                dtGridAfterHours.Columns["FlagRowType"].ReadOnly = false;
                dtGridAfterHours.Columns["FlagModified"].ReadOnly = false;
                Session[SessionConstants.DT_AFTERHOUR] = dtGridAfterHours;
                AfterHourRowNo = dtGridAfterHours.Rows.Count;
                dataBindStep2();
                objOC = null;
            }
            catch (Exception ex)
            {
                if (ViewState[OCID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("edit_rpnotification_step1.fillAfterHoursNotifications:: Exception occured for User ID - " + ViewState[OCID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(ViewState[OCID]));
                }
                throw;
            }
        }
        #endregion Step2_22
        #endregion

    }
}

