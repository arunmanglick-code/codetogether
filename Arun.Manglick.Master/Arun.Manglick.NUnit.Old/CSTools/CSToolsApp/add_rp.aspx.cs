#region File History

/******************************File History***************************
 * File Name        : add_rp.aspx.cs
 * Author           : 
 * Created Date     : 
 * Purpose          : Add new Ordering Clinician.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification
 * 
 * 02-15-2008       Suhas       Code Review Fixes
 * 03-04-2008       Suhas       Added institute and Directory selection for "Add OC" in the tools menu.
 * 03-20-2008       SSK         Added fields for "PIN for Message Retrieval".
 * 03-26-2008       IAK         Added External information section.
 * 03-31-2008       IAK         Defect: 2908
 * 05-05-2008       Suhas       Defect # 3109
 * 05-11-2008       Prerak      Defect #2680 Fixed
 * 05-13-2008       Suhas       Defect #3133 - Fixed
 * 05-21-2008       Suhas       Auto Tab Issue in the Phone fields
 * 12 Jun 2008      Prerak -    Migration of AJAX Atlas to AJAX RTM 1.0
 * 26 Jun 2008      IAK         Defect 3303
 * 04 Aug 2008      Prerak      CR#200 -> OC Profile layout
 * 06 Aug 2008      Prerak      CR -> Cell Phone for OC profile implemented
 * 08 Aug 2008      Prerak      Defect #3583, 3588, 3589 fixed
 * 08 Aug 2008      Prerak      Removed Static datatable
 * 08 Aug 2008      Prerak      Defect #3589 fixed
 * 19 Aug 2008      Prerak      Defect 3636 fixed 
 * 19 Aug 2008       Suhas       FR#271 SMS with Weblink device Group population 
 * 09 Sep 2008      Prerak      Sharepoint defect #558 "The user can't enter the correct email gateway 
 *                              for a clinical team or unit" fixed
 * 31-Oct-2008      Sheetal     Remove Pager validations
 * 11 Nov 2008      Prerak      Change Validation and defect #3622 Fixed
 * 13 Nov 2008      Zeeshan     Defect #3164 - User clicks on Edit for a device at the bottom of the list they are forced to scroll to find the device they selected.
 * 14-Nov-2008      Sheetal     Fixed Defect 3310
 * 17-Nov-2008      IAK         Defect #3113 Fixed.
 * 20-Nov-2008      IAK         Defect #3113, #4165 Fixed: Handled blank values
 * 24-Nov-2008      RG          Defect #4167 fixed
 * 11-25-2008       SD          Defect #4225 fixed
 * 11-28-2008       GB          Added clinical team phone to fix TTP #161
 * 12-09-2008       Prerak      Defect #2679 Fixed
 * 01-05-2009       RG          FR 282 Changes
 * 01-13-2009       RG          Suggestion from Fred
 * 12-18-2008       ZNK         CR-Multiple External ID support.
 * ------------------------------------------------------------------- 
 * 
 */
#endregion

using System;
using System.Text;
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

using Vocada.CSTools.Common;
using Vocada.CSTools.DataAccess;
using Vocada.VoiceLink.Utilities;

namespace Vocada.CSTools
{
    /// <summary>
    /// The purpose of this class is to store the entered data for new ordering clinician
    /// in the Veriphy database.
    /// </summary>	
    public partial class AddRP : System.Web.UI.Page
    {
        #region Page Variables
        private string loggedUserID = string.Empty;
        private const string INSTITUTIONID = "InstitutionID";
        private const string DIRECTORYID = "DirectoryID";
        private const string DEPARTMENT = "Departments";
        private const int GRID_ADJUSTMENT_VALUE = 85;

        //Constants for Toggle Button Name
        private const string SHOWDETAILS_BUTTONNAME = "Assign Event";
        private const string HIDEDETAILS_BUTTONNAME = "Hide Event Details";
        #endregion

        #region Protected Fields
        protected string strUserSettings = "NO";
        protected DataTable dtGridDeviceNotifications ;
        protected DataTable dtGridAfterHours ;
        protected static int RowNo = 1;
        protected static int AfterHourRowNo = 1;
        protected DataTable dtIdTypes = null;
        protected DataTable dtIdTypesInfo = null;
        #endregion

        #region Events

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserSettings"] != null)
                strUserSettings = Session["UserSettings"].ToString();

            if (Session[SessionConstants.USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
                Response.Redirect(Utils.GetReturnURL("default.aspx", "add_rp.aspx", this.Page.ClientQueryString));

            loggedUserID = Session[SessionConstants.USER_ID].ToString();

            if (Session[SessionConstants.DT_ADD_NOTIFICATION] == null)
                dtGridDeviceNotifications = new DataTable();
            else
                dtGridDeviceNotifications = (DataTable)Session[SessionConstants.DT_ADD_NOTIFICATION];
            if (Session[SessionConstants.DT_ADD_AFTERHOUR] == null)
                dtGridAfterHours = new DataTable();
            else
                dtGridAfterHours = (DataTable)Session[SessionConstants.DT_ADD_AFTERHOUR];

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
                try
                {
                    if (Request["DirectoryID"] == null)
                    {
                        tblSelect.Visible = true;
                        tblInformation.Visible = false;
                        populateInstitutions();
                    }
                    else
                    {
                        tblSelect.Visible = false;
                        tblInformation.Visible = true;
                        txtFirstName.Focus();
                        ViewState[INSTITUTIONID] = Convert.ToInt32(Session[SessionConstants.INSTITUTION_ID].ToString());
                        ViewState[DIRECTORYID] = Convert.ToInt32(Request["DirectoryID"].ToString());
                        loadDepartment();
                        loadLoginControls();
                        fillDeviceDDL();
                        fillEventDDL(cmbEvents);
                        fillGroupDDLStep3();
                        fillAfterHoursFindingDDL();
                        lblNoRecordsStep3.Visible = true;
                        dataBindStep1();
                        dataBindStep2();
                        ViewState[DIRECTORYID] = Request["DirectoryID"];
                    }
                    registerJavascriptVariables();
                    CreateTable();
                    getCarriers();
                    resetControls();
                    dtGridDeviceNotifications.Rows.Clear();
                    dtGridAfterHours.Rows.Clear();
                    /* To prevent user to leave page without saving changes.*/
                    btnAddDevice.Attributes.Add("onclick", "return validateDevices('" + cmbDeviceType.ClientID + "','" + txtNumAddress.ClientID + "','" + cmbCarrier.ClientID + "','" + txtEmailGateway.ClientID + "','" + cmbFindings.ClientID + "','" + cmbGroup.ClientID + "','" + txtInitialPause.ClientID + "');");
                    txtNumAddress.Attributes.Add("onclick", "RemoveDeviceLabel('" + txtNumAddress.ClientID + "','" + hidDeviceLabel.ClientID + "');");
                    txtInitialPause.Attributes.Add("onclick", "RemoveInitialPauseLabel('" + txtInitialPause.ClientID + "','" + hidInitPauseLabel.ClientID + "');");

                    populateIdTypes();
                    BindOCExternalIDsInfo(-1);
                }
                catch (Exception ex)
                {
                    if (loggedUserID.Trim().Length > 0)
                    {
                        Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("AddRP.btnAdd_Click:Exception occured for User ID -  ", loggedUserID.ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(loggedUserID));
                    }
                    else
                    {
                        Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("AddRP.btnAdd_Click:Exception occured for User ID - ", "0", ex.Message, ex.StackTrace), 0);
                    }
                }
            }
            lblDeviceAlreadyExists.Text = "";
            Session[SessionConstants.CURRENT_PAGE] = "add_rp.aspx";
            Session[SessionConstants.CURRENT_TAB] = "Tools";
            Session[SessionConstants.CURRENT_INNER_TAB] = "AddOC";
            /* No search alphabet should remain selected after adding new RP*/
            Session["SearchAlphabet"] = "";
            if (drpDepartment.SelectedValue != "-1")
            {
                lblStar.InnerText = "*:";
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

        }

        #endregion

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            string faxNumber = txtFaxAreaCode.Text.Trim() + txtFaxPrefix.Text.Trim() + txtFaxNNNN.Text.Trim();
            string errorMessage = "";
            bool isValidStartDate = false;
            bool isValidEndDate = false;
            string setfocus = "";
            int departmentAssignmentID = 0;

            if (faxNumber.Length != 10 && txtEmail.Text.Trim().Length < 2)
            {
                errorMessage += "You must enter either an Email Address or a Fax Number for Notifications\\n";
                if (setfocus == "")
                    setfocus = txtEmail.ClientID;
            }
            if (faxNumber.Length > 0 && faxNumber.Length < 10)
            {
                errorMessage += "You must enter valid Fax Number for Notifications\\n";
                if (setfocus == "")
                    setfocus = txtFaxAreaCode.ClientID;
            }
            if (Request["DirectoryID"] == null)
            {
                if (drpInstitutions.SelectedValue.Equals("-1"))
                {
                    errorMessage += "You must select Institution\\n";
                    if (setfocus == "")
                    {
                        setfocus = drpInstitutions.ClientID;
                    }
                }
                if (drpDirectories.SelectedValue.Equals("-1"))
                {
                    errorMessage += "You must select Directory\\n";
                    if (setfocus == "")
                    {
                        setfocus = drpDirectories.ClientID;
                    }
                }
            }
            if (drpDepartment.SelectedValue != "-1")
            {
                if (txtStartDate.Text.Trim().Length == 0)
                {
                    errorMessage += "Please enter start date.\\n";
                    if (setfocus == "")
                    {
                        setfocus = txtStartDate.ClientID;
                    }
                }
                else if (!isValidDate(txtStartDate.Text.Trim()))
                {
                    errorMessage += "Please enter valid start date.\\n";
                    if (setfocus == "")
                    {
                        setfocus = txtStartDate.ClientID;
                    }
                }
                else
                {
                    if (DateTime.Parse(txtStartDate.Text.Trim()) < DateTime.Parse(DateTime.Now.ToShortDateString()))
                    {
                        errorMessage += "Start date should be greater than current date.\\n";
                        if (setfocus == "")
                        {
                            setfocus = txtStartDate.ClientID;
                        }
                    }
                    else
                    {
                        isValidStartDate = true;
                    }
                }

                if (txtEndDate.Text.Trim().Length > 0)
                {
                    if (!isValidDate(txtEndDate.Text.Trim()))
                    {
                        errorMessage += "Please enter valid end date.\\n";
                        if (setfocus == "")
                        {
                            setfocus = txtEndDate.ClientID;
                        }
                    }
                    else
                        isValidEndDate = true;
                }

                if (isValidStartDate && isValidEndDate)
                {
                    if (DateTime.Parse(txtStartDate.Text.Trim()) > DateTime.Parse(txtEndDate.Text.Trim()))
                    {
                        errorMessage += "Start date should be less than end date.\\n";
                        if (setfocus == "")
                        {
                            setfocus = txtStartDate.ClientID;
                        }
                    }
                }
            }
            
            if (!validateBeforeSave())
            {
                //string errorMessage = "<script type=\'text/javascript\'>";
                if (departmentAssignmentID == 0 && drpDepartment.SelectedValue == "-1")
                {
                    errorMessage += "enableDateSelection('false');";
                }
               // errorMessage += "</script>";

                ScriptManager.RegisterClientScriptBlock(UpdatePanelMessageList, UpdatePanelMessageList.GetType(), "Navigate", errorMessage, true);

                return;
            }
            if (errorMessage.Length > 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanelMessageList,UpdatePanelMessageList.GetType(),"Error", "<script type=\'text/javascript\'>alert('" + errorMessage + "');document.getElementById('" + setfocus + "').focus();</script>",false);
                return;
            }

            OrderingClinician objOC = new OrderingClinician();
            try
            {
                if (objOC.CheckDuplicateOCPIN(txtLoginID.Text.Trim(), txtPassword.Text.Trim(), 0))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanelMessageList,UpdatePanelMessageList.GetType(), "PinError", "alert('This PIN has been already used by another OC.');document.getElementById('" + txtPassword.ClientID + "').value='';", true);
                }
                else if (txtPinForMessage.Text.Trim().Length > 0 && Utility.CheckDuplicatePINForUser(Convert.ToInt32(ViewState[INSTITUTIONID].ToString()), txtPinForMessage.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanelMessageList,UpdatePanelMessageList.GetType(), "PinError", "alert('This PIN is already in use. Please enter another PIN.');document.getElementById('" + txtPinForMessage.ClientID + "').value='';", true);
                }
                else
                {
                    // all other data should be good, updating referring physician...
                    OrderingClinicianInfo objOCInfo = new OrderingClinicianInfo();
                    objOCInfo.DirectoryID = Convert.ToInt32(ViewState[DIRECTORYID].ToString());
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
                    objOCInfo.AdditionalContPhone = txtAddPhoneAreaCode.Text.Trim() + txtAddPhonePrefix.Text.Trim() + txtAddPhoneNumber.Text.Trim();
                    objOCInfo.UpdatedBy = Convert.ToInt32(Session[SessionConstants.USER_ID].ToString());
                    objOCInfo.RadiologyTDR = chkRadiologyTDR.Checked;
                    objOCInfo.LabTDR = chkLabTDR.Checked;
                    objOCInfo.Notes = txtNotes.Text.Trim();
                    objOCInfo.ProfileCompleted = chkProfileCompleted.Checked;
                    objOCInfo.IsResident = true; ////chkResident.Checked;
                    objOCInfo.IsEDDoc = chkEDDoc.Checked;
                    objOCInfo.CellPhone = txtCellAreaCode.Text.Trim() + txtCellPrefix.Text.Trim()  + txtCellNNNN.Text.Trim();
                    if (txtStartDate.Text.Trim().Length > 0)
                        objOCInfo.DepartmentStartDate = DateTime.Parse(txtStartDate.Text.Trim());
                    else
                        objOCInfo.DepartmentStartDate = DateTime.Parse(DateTime.Now.ToShortDateString());

                    if (txtEndDate.Text.Trim().Length > 0)
                        objOCInfo.DepartmentEndDate = DateTime.Parse(txtEndDate.Text.Trim()).Add(new TimeSpan(23, 59, 59));
                    else
                        objOCInfo.DepartmentEndDate = DateTime.MinValue;

                    if (int.Parse(drpDepartment.SelectedValue) == -1)
                        objOCInfo.DepartmentID = -1;
                    else
                        objOCInfo.DepartmentID = int.Parse(drpDepartment.SelectedValue);

                    string pin = string.Empty;
                    if (pnlMessageRetieve.Visible)
                        if (txtPinForMessage.Text.Trim().Length > 0)
                            pin = txtPinForMessage.Text.Trim();

                    objOCInfo.PINForMessageRetrieve = pin;

                    DataTable dtblExternalIDInfo = null;
                    if (Session["DT_IDTYPEINFO"] != null)
                    {
                        dtblExternalIDInfo = (DataTable)(Session["DT_IDTYPEINFO"]);
                        dtblExternalIDInfo.TableName = "ExternalIDTable";
                    }

                    int refId = objOC.InsertOrderingClinician(objOCInfo, dtblExternalIDInfo);

                    if (refId == -1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanelMessageList, UpdatePanelMessageList.GetType(), "UpdateException", "<script type=\'text/javascript\'>alert('External ID Information matches other Ordering Clinician');</script>", false);
                    }
                    else
                    {
                        addDevicesAndAfterHours(refId);
                        dtGridAfterHours.Rows.Clear();
                        dtGridDeviceNotifications.Rows.Clear();
                        dataBindStep1();
                        upnlStep1.Update();
                        dataBindStep2();
                        upnlStep3.Update();
                        textChanged.Value = "false";
                        if (tblSelect.Visible)
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanelMessageList, UpdatePanelMessageList.GetType(), "RecordUpdate", "<script type=\'text/javascript\'>alert('Ordering Clinician added successfully');</script>", false);
                            clearAllFields();
                        }
                        else
                        {
                            string url = "./directory_maintenance.aspx?DirectoryName=" + Session[SessionConstants.DIRECTORY_NAME].ToString().Replace("'", "#**#%") + "&DirectoryID=" + Session[SessionConstants.DIRECTORY_ID].ToString() + "&RefId=" + refId + "&StartingWith=" + objOCInfo.LastName;
                            ScriptManager.RegisterClientScriptBlock(UpdatePanelMessageList, UpdatePanelMessageList.GetType(), "Nevigate", "<script type=\'text/javascript\'>Navigate('" + url + "');</script>", false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (loggedUserID.Trim().Length > 0)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("AddRP.btnAdd_Click:Exception occured for User ID -  ", loggedUserID.ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(loggedUserID));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("AddRP.btnAdd_Click:Exception occured for User ID - ", "0", ex.Message, ex.StackTrace), 0);
                }
            }
            finally
            {
                objOC = null;
            }
        }

        /// <summary>
        ///  Load Directory Combo with selected institution and clear grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void drpInstitutions_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                populateDirectories(Convert.ToInt32(drpInstitutions.SelectedValue));
                ViewState[INSTITUTIONID] = Convert.ToInt32(drpInstitutions.SelectedValue);
                lblPhone.Text = String.Empty;
                loadDepartment();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_rp.drpInstitutions_SelectedIndexChanged", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
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
        /// Handles the SelectedIndexChanged event of the drpDirectories control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void drpDirectories_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                ViewState[INSTITUTIONID] = Convert.ToInt32(drpInstitutions.SelectedValue);
                ViewState[DIRECTORYID] = Convert.ToInt32(drpDirectories.SelectedValue);
                chkEDDoc.Checked = false;
                lblPhone.Text = String.Empty;
                loadDepartment();
                loadLoginControls();
                tblInformation.Visible = true;
                fillDeviceDDL();
                fillEventDDL(cmbEvents);
                resetControls();
                fillGroupDDLStep3(); 
                fillAfterHoursFindingDDL();
                lblNoRecordsStep3.Visible = true;
                dataBindStep1();
                dataBindStep2();
                txtFirstName.Focus();
                UpdatePanelMessageList.Update();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_rp.drpDirectories_SelectedIndexChanged", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
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
                txtPassword.Text = Utility.GetNewPin((Convert.ToInt32(loggedUserID)));
                txtChanged.Value = "true";
            }
            catch (Exception ex)
            {
                if (loggedUserID.Trim().Length > 0)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("AddRP.btnGeneratePassword_Click:Exception occured for User ID -  ", loggedUserID.ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(loggedUserID));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("AddRP.btnGeneratePassword_Click:Exception occured for User ID - ", "0", ex.Message, ex.StackTrace), 0);
                }

                ScriptManager.RegisterClientScriptBlock(UpdatePanelMessageList,UpdatePanelMessageList.GetType(), "Warning", "<script type=\"text/javascript\">alert('Error Generating Password.');</script>", false);
            }
            finally
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanelMessageList,UpdatePanelMessageList.GetType(), "Warning", "<script type=\"text/javascript\">document.getElementById(txtFirstNameClientID).focus();</script>", false);
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
                txtPinForMessage.Text = Utility.GetNewPinForMessageRetrieve(Convert.ToInt32(ViewState[INSTITUTIONID].ToString()));
                txtChanged.Value = "true";
            }
            catch (Exception ex)
            {
                if (loggedUserID.Trim().Length > 0)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("AddRP.btnPinForMessage_Click:Exception occured for User ID -  ", loggedUserID.ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(loggedUserID));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("AddRP.btnPinForMessage_Click:Exception occured for User ID - ", "0", ex.Message, ex.StackTrace), 0);
                }

                ScriptManager.RegisterClientScriptBlock(UpdatePanelMessageList,UpdatePanelMessageList.GetType(), "Warning", "<script type=\"text/javascript\">alert('Error Generating Password.');</script>", false);
            }
            finally
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanelMessageList,UpdatePanelMessageList.GetType(), "Warning", "<script type=\"text/javascript\">document.getElementById(txtFirstNameClientID).focus();</script>", false);
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
            finally
            {
                generateDataGridHeight(grdIdTypeInfo, divExternalIDInformation);
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
                txtAddIDType.Text = "";
                txtUserId.Text = "";

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
                    ScriptManager.RegisterStartupScript(UpdatePanelMessageList, UpdatePanelMessageList.GetType(), "OtherException", "alert(\"Please enter another ID Type which does not start with 'other'\");", true);
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
                        drIDInfo["ReferringPhysicianID"] = -1; ;//Rp Id needs to be taken from session of OC profile pages.
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
            finally
            {
                generateDataGridHeight(grdIdTypeInfo, divExternalIDInformation);
            }
        }

        #region Step1
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
                           
                        }
                        else if ((int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerNumSkyTel || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerNumUSA || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerNumRegular || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.pagerPartner))
                        {
                            txtNumAddress.Attributes.Add("onkeyPress", "JavaScript:return PagerValidationWithSpace('" + txtNumAddress.ClientID + "');");
                            
                        }
                        else if (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.PagerAlpha)
                        {
                            txtNumAddress.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes(textChangedClientID);");
                            
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


                if (grdDevices.EditItemIndex != -1)
                    addLinkToGridInEditMode(grdDevices.Items[grdDevices.EditItemIndex]);     
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID].ToString().Length > 0)
                    Tracer.GetLogger().LogExceptionEvent("add_rp.cmbDeviceType_SelectedIndexChanged:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
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
                    Tracer.GetLogger().LogExceptionEvent("add_rp.cmbGroup_SelectedIndexChanged:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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

                string deviceName ="";
                string deviceAddress = "";
                string strGridGateway = "";
                string initialPause = "";
                string strGridCarrier = "";
                string script = "";
                //Set DeviceName property value   
                deviceName= ((TextBox)(grdDevices.Items[e.Item.ItemIndex].Cells[0].FindControl("txtGridDeviceType"))).Text.Trim();

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
                 string strEvent = (rpNotifyEventID == 0) ? "" : cmbGrdEvents.SelectedItem.Text.Trim();

                //Set FindingID property value   
                int findingID = Convert.ToInt32(cmbGrdFindings.SelectedValue);
                string strFinding = (findingID == 0) ? "" : cmbGrdFindings.SelectedItem.Text.Trim();

                //Set GroupID property value   
                int groupID = Convert.ToInt32(cmbGrdGroups.SelectedValue);
                string strGroupName = (groupID == 0) ? "" : cmbGrdGroups.SelectedItem.Text;

                //Set DeptDeviceID property value   
                int rowID = Convert.ToInt32(grdDevices.DataKeys[e.Item.ItemIndex]);
                
                if (gridInitialPauseTxt.Visible == true)
                    initialPause = gridInitialPauseTxt.Text.Trim();
                else
                    initialPause = "";

                if (isAddClicked == "false")
                {

                    DataRow[] editrow = dtGridDeviceNotifications.Select("DeviceName = '" + deviceName + "' AND RowID <>" + rowID);

                    if (editrow.GetLength(0) == 0)
                    {
                        DataRow[] number = dtGridDeviceNotifications.Select("DeviceName = '" + hdnOldDeviceName.Value + "'");
                        int introwno = dtGridDeviceNotifications.Rows.IndexOf(number[0]);
                        dtGridDeviceNotifications.Rows[introwno].BeginEdit();
                        dtGridDeviceNotifications.Rows[introwno]["DeviceName"] = deviceName;
                        dtGridDeviceNotifications.Rows[introwno]["DeviceAddress"] = deviceAddress;
                        dtGridDeviceNotifications.Rows[introwno]["Gateway"] = strGridGateway;
                        dtGridDeviceNotifications.Rows[introwno]["Carrier"] = strGridCarrier;
                        dtGridDeviceNotifications.Rows[introwno]["EventDescription"] = strEvent;
                        dtGridDeviceNotifications.Rows[introwno]["RPNotifyEventID"] = rpNotifyEventID;
                        dtGridDeviceNotifications.Rows[introwno]["FindingDescription"] = strFinding;
                        dtGridDeviceNotifications.Rows[introwno]["FindingID"] = findingID;
                        dtGridDeviceNotifications.Rows[introwno]["GroupID"] = groupID;
                        dtGridDeviceNotifications.Rows[introwno]["GroupName"] = strGroupName;
                        dtGridDeviceNotifications.Rows[introwno]["InitialPause"] = initialPause;
                        dtGridDeviceNotifications.Rows[introwno].EndEdit();
                        dtGridDeviceNotifications.Rows[introwno].AcceptChanges();
                        Session[SessionConstants.DT_ADD_NOTIFICATION] = dtGridDeviceNotifications;
                        grdDevices.EditItemIndex = -1;
                        script = "Device has been updated.";
                        grdDevices.DataSource = dtGridDeviceNotifications;
                        grdDevices.DataBind();
                        //Update After Hour grid if device name is changed.
                        if (hdnOldDeviceName.Value != deviceName)
                        {
                            DataRow[] drAH = dtGridAfterHours.Select("DeviceName = '" + hdnOldDeviceName.Value + "'");
                            if (drAH.Length > 0)
                            {
                                int numAH = drAH.Length;
                                for (int countAH = 0; countAH < numAH; countAH++)
                                {
                                    drAH[countAH].BeginEdit();
                                    drAH[countAH]["DeviceName"] = deviceName;
                                    drAH[countAH].EndEdit();
                                    drAH[countAH].AcceptChanges();
                                    Session[SessionConstants.DT_ADD_AFTERHOUR] = dtGridAfterHours;
                                }
                            }
                            drAH = null;
                        }
                        hdnOldDeviceName.Value = "";
                        fillAfterHoursDeviceOptions();

                        dataBindStep2();

                        ViewState[Constants.DEVICE_ADDRESS] = null;
                        ViewState[Constants.EMAIL_GATEWAY] = null;

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
                    dtrow["RPNotifyEventID"] = rpNotifyEventID;
                    dtrow["FindingID"] = findingID;
                    dtrow["RowID"] = RowNo;                                        

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
                ScriptManager.RegisterStartupScript(upnlStep1, upnlStep1.GetType(), "deviceExists", "alert('" + script + "');SetPostbackVarFalse();", true);
                
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("add_rp.grdDevices_UpdateCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
                {
                    DataRowView data = (DataRowView)e.Item.DataItem;
                    Label gridFindingLabel = ((Label)(e.Item.Cells[6].FindControl("lblGridDeviceFinding")));
                    Label gridGroupLabel = ((Label)(e.Item.Cells[1].FindControl("lblGridGroup")));
                    
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

                    e.Item.Attributes.Add("onclick", "return SetPostbackVarTrue();");
                }

                if (e.Item.ItemType == ListItemType.EditItem)
                {                 
                    addLinkToGridInEditMode(e.Item);
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("add_rp.grdDevices_ItemDataBound:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                hdnOldDeviceName.Value = deviceType;
                editDeviceGrid(intGridEditItemIndex, eventID, findingID, deviceName, deviceGatway, int.Parse(grdDevices.Items[intGridEditItemIndex].Cells[11].Text.Trim()));
                resetControls();
                textChanged.Value = "true";
                hdnGridChanged.Value = "false";
                hdnIsAddClicked.Value = "false";
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("add_rp.grdDevices_EditCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                grdDevices.DataSource = dtGridDeviceNotifications;
                grdDevices.DataBind();
                generateDataGridHeight("cancelDevice");
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("add_rp.grdDevices_CancelCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                textChanged.Value = "true";
                hdnGridChanged.Value = "false";
                int rowid = int.Parse(e.Item.Cells[14].Text.Trim());              
                DataRow[] checkRow = dtGridAfterHours.Select("RPDeviceID = " + rowid);
                if (checkRow.GetLength(0) <= 0)
                {
                    DataRow[] deleteRow = dtGridDeviceNotifications.Select("RowID = " + rowid);
                    dtGridDeviceNotifications.Rows.Remove(deleteRow[0]);
                    dtGridDeviceNotifications.AcceptChanges();
                    Session[SessionConstants.DT_ADD_NOTIFICATION] = dtGridDeviceNotifications;
                    grdDevices.EditItemIndex = -1;
                    dataBindStep1();
                    fillAfterHoursDeviceOptions();
                    generateStep3DataGridHeight();
                    upnlStep3.Update();

                }
                else
                {
                    if (grdDevices.EditItemIndex != -1)
                        addLinkToGridInEditMode(grdDevices.Items[grdDevices.EditItemIndex]);     

                    generateDataGridHeight("Delete");
                    lblDeviceAlreadyExists.Text = "Warning: This device is associated with notification events (Step 2). You must first delete notification in Step2 before you delete this device.";
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_rp.grdDevices_DeleteCommand", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
            catch(Exception  ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_rp.btnAddDevice_Click", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                    lbtnEdit.OnClientClick = script ;
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
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_rp.btnAddDevice_Click", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        #endregion Step1
        #region Step2
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
                    Tracer.GetLogger().LogExceptionEvent("add_rp.cmbStep3Groups_SelectedIndexChanged:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                int rowid = (int.Parse(e.Item.Cells[0].Text.Trim()));
                DataRow[] deleteRow = dtGridAfterHours.Select("AfterHourRowNo = " + rowid);
                dtGridAfterHours.Rows.Remove(deleteRow[0]);
                dtGridAfterHours.AcceptChanges();
                Session[SessionConstants.DT_ADD_AFTERHOUR] = dtGridAfterHours;
                dataBindStep2();

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("add_rp.grdAfterHoursNotifications_DeleteCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                    RPDeviceID = Convert.ToInt32(drRPDevice[0]["RowID"]);
                    
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
                    AfterHourRowNo++;
                    dtGridAfterHours.Rows.Add(drAHNewRow);
                    Session[SessionConstants.DT_ADD_AFTERHOUR] = dtGridAfterHours;
                    dataBindStep2();
                    textChanged.Value = "true";
                    hdnGridChanged.Value = "false";
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("add_rp.btnAddStep3_Click:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                    Tracer.GetLogger().LogExceptionEvent("add_rp.grdAfterHoursNotifications_ItemDataBound:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }

        }
        #endregion Step2
        #endregion Events

        #region Private Methods

        /// <summary>
        /// Fill Institution combo box
        /// </summary>
        private void populateInstitutions()
        {
            DataTable dtInstitutions = null;
            DataRow drInstitution = null;
            try
            {
                //Get list of institutions
                dtInstitutions = Utility.GetInstitutionList();

                //Add additional row at top
                drInstitution = dtInstitutions.NewRow();
                drInstitution[1] = "-- Select Institution --";
                drInstitution[0] = -1;
                dtInstitutions.Rows.InsertAt(drInstitution, 0);

                //Bind the data source to combo
                drpInstitutions.DataSource = dtInstitutions;
                drpInstitutions.DataTextField = "InstitutionName";
                drpInstitutions.DataValueField = "InstitutionID";
                drpInstitutions.DataBind();
                drpInstitutions.SelectedValue = "-1";
                populateDirectories(Convert.ToInt32(drpInstitutions.SelectedValue));
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_rp.populateInstitutions", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                dtInstitutions = null;
                drInstitution = null;
            }
        }
        /// <summary>
        /// Fill Directory list in combo box for given institution
        /// </summary>
        /// <param name="institutionID"></param>
        private void populateDirectories(int institutionID)
        {
            DataTable dtDirectories = null;
            DataRow drDirectory = null;
            try
            {
                //Get list of Directories
                dtDirectories = Utility.GetDirectories(institutionID);

                //Add additional row at top
                drDirectory = dtDirectories.NewRow();
                drDirectory[1] = "-- Select Directory --";
                drDirectory[0] = -1;
                dtDirectories.Rows.InsertAt(drDirectory, 0);

                //Bind the data source to combo
                drpDirectories.DataSource = dtDirectories;
                drpDirectories.DataTextField = "DirectoryDescription";
                drpDirectories.DataValueField = "DirectoryID";
                drpDirectories.DataBind();
                drpDirectories.SelectedValue = "-1";
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_rp.populateDirectories", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                dtDirectories = null;
                drDirectory = null;
            }
        }
        /// <summary>
        /// Clears all fields.
        /// </summary>
        private void clearAllFields()
        {
            txtPrimaryPhoneAreaCode.Text = "";
            txtPrimaryPhonePrefix.Text = "";
            txtPrimaryPhoneNNNN.Text = "";

            txtAddPhoneAreaCode.Text = "";
            txtAddPhonePrefix.Text = "";
            txtAddPhoneNumber.Text = "";

            txtFaxPrefix.Text = "";
            txtFaxAreaCode.Text = "";
            txtFaxNNNN.Text = "";

            txtFirstName.Text = "";
            txtSpecialty.Text = "";
            txtLastName.Text = "";
            txtPracticeGroup.Text = "";
            txtNickname.Text = "";
            txtLoginID.Text = "";
            txtPassword.Text = "";
            txtHospitalAffiliation.Text = "";
            txtPrimaryPhonePrefix.Text = "";
            txtPrimaryPhoneAreaCode.Text = "";
            txtPrimaryPhoneNNNN.Text = "";
            txtEmail.Text = "";
            txtFaxPrefix.Text = "";
            txtFaxAreaCode.Text = "";
            txtFaxNNNN.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            lblPhone.Text = "";
            txtZip.Text = "";
            txtName.Text = "";
            txtAddPhonePrefix.Text = "";
            txtAddPhoneAreaCode.Text = "";
            txtAddPhoneNumber.Text = "";
            txtNotes.Text = "";
            txtCellPrefix.Text = "";
            txtCellAreaCode.Text = "";
            txtCellNNNN.Text = "";

            drpDepartment.SelectedIndex = -1;
            chkLabTDR.Checked = false;
            chkRadiologyTDR.Checked = false;
            chkProfileCompleted.Checked = false;
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            chkEDDoc.Checked = false;
            pnlOCLogin.Visible = false;
            txtPinForMessage.Text = "";
        }
        /// <summary>
        /// Register JS variables, client side button click events
        /// </summary>
        private void registerJavascriptVariables()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var textChangedClientID = '" + textChanged.ClientID + "';");
            sbScript.Append("var txtStartDateClientID = '" + txtStartDate.ClientID + "';");
            sbScript.Append("var txtEndDateClientID = '" + txtEndDate.ClientID + "';");
            sbScript.Append("var anchFromDateClientID = '" + anchFromDate.ClientID + "';");
            sbScript.Append("var anchToDateClientID = '" + anchToDate.ClientID + "';");
            sbScript.Append("var cmbDepartmentClientID = '" + drpDepartment.ClientID + "';");
            sbScript.Append("var lblStarClientID = '" + lblStar.ClientID + "';");
            sbScript.Append("var txtFirstNameClientID = '" + txtFirstName.ClientID + "';");
            sbScript.Append("var txtLastNameClientID = '" + txtLastName.ClientID + "';");
            sbScript.Append("var txtNicknameClientID = '" + txtNickname.ClientID + "';");
            sbScript.Append("var txtPrimaryPhoneAreaCodeClientID = '" + txtPrimaryPhoneAreaCode.ClientID + "';");
            sbScript.Append("var txtPrimaryPhonePrefixClientID = '" + txtPrimaryPhonePrefix.ClientID + "';");
            sbScript.Append("var txtPrimaryPhoneNNNNClientID = '" + txtPrimaryPhoneNNNN.ClientID + "';");
            sbScript.Append("var txtCellAreaCodeClientID = '" + txtCellAreaCode.ClientID + "';");
            sbScript.Append("var txtCellPrefixClientID = '" + txtCellPrefix.ClientID + "';");
            sbScript.Append("var txtCellNNNNClientID = '" + txtCellNNNN.ClientID + "';");
            sbScript.Append("var chkEDDocClientID = '" + chkEDDoc.ClientID + "';");
            sbScript.Append("var txtLoginIDClientID = '" + txtLoginID.ClientID + "';");
            sbScript.Append("var txtPasswordClientID = '" + txtPassword.ClientID + "';");
            sbScript.Append("var txtPinForMessageClientID = '" + txtPinForMessage.ClientID + "';");
            sbScript.Append("var txtNameClientID = '" + txtName.ClientID + "';");
            sbScript.Append("var txtAddPhoneAreaCodeClientID = '" + txtAddPhoneAreaCode.ClientID + "';");
            sbScript.Append("var txtAddPhonePrefixClientID = '" + txtAddPhonePrefix.ClientID + "';");
            sbScript.Append("var txtAddPhoneNumberClientID = '" + txtAddPhoneNumber.ClientID + "';");
            sbScript.Append("var txtFaxAreaCodeClientID = '" + txtFaxAreaCode.ClientID + "';");
            sbScript.Append("var txtFaxPrefixClientID = '" + txtFaxPrefix.ClientID + "';");
            sbScript.Append("var txtFaxNNNNClientID = '" + txtFaxNNNN.ClientID + "';");
            sbScript.Append("var txtEmailClientID = '" + txtEmail.ClientID + "';");
            sbScript.Append("var txtSpecialtyClientID = '" + txtSpecialty.ClientID + "';");
            sbScript.Append("var hdnIsAddClickedClientID = '" + hdnIsAddClicked.ClientID + "';");
        
            
            sbScript.Append("var txtHospitalAffiliationClientID = '" + txtHospitalAffiliation.ClientID + "';");

            sbScript.Append("var hdnGridChangedClientID = '" + hdnGridChanged.ClientID + "';");
            sbScript.Append("var hiddenScrollPos = '" + scrollPos.ClientID + "';");
            
            
            sbScript.Append("enableDateSelection();</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());

            txtPrimaryPhoneAreaCode.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtPrimaryPhonePrefix.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtPrimaryPhoneNNNN.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");

            txtAddPhoneAreaCode.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtAddPhonePrefix.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtAddPhoneNumber.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");

            txtFaxPrefix.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtFaxAreaCode.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtFaxNNNN.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            drpDepartment.Attributes.Add("onClick", "JavaScript:UpdateLabel();");
            txtPassword.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
            txtPinForMessage.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
            btnGeneratePassword.Attributes.Add("onClick", "JavaScript:UpdateProfile();");
            txtCellPrefix.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtCellAreaCode.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtCellNNNN.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");

            // To add the attribute to set the flag on change of any control on the page.
            txtFirstName.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtSpecialty.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtLastName.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtPracticeGroup.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtNickname.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtLoginID.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtPassword.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtHospitalAffiliation.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtPrimaryPhonePrefix.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtPrimaryPhoneAreaCode.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtPrimaryPhoneNNNN.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtEmail.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtFaxPrefix.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtFaxAreaCode.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtFaxNNNN.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtAddress1.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtAddress2.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtAddress3.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtCity.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtState.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtZip.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtName.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtAddPhonePrefix.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtAddPhoneAreaCode.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtAddPhoneNumber.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            drpDepartment.Attributes.Add("onchange", "JavaScript:enableDateSelection();UpdateProfile();");
            txtNotes.Attributes.Add("onchange", "JavaScript:UpdateProfile();CheckMaxLength('" + txtNotes.ClientID + "',255);");
            txtNotes.Attributes.Add("onblur", "JavaScript:CheckMaxLength('" + txtNotes.ClientID + "',255);");
            txtNotes.Attributes.Add("onKeyUp", "JavaScript:CheckMaxLength('" + txtNotes.ClientID + "',255);");
            txtCellPrefix.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtCellAreaCode.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtCellNNNN.Attributes.Add("onchange", "JavaScript:UpdateProfile();");

            chkLabTDR.Attributes.Add("onClick", "JavaScript:UpdateProfile();");
            chkRadiologyTDR.Attributes.Add("onClick", "JavaScript:UpdateProfile();");
            chkProfileCompleted.Attributes.Add("onClick", "JavaScript:UpdateProfile();");
            txtStartDate.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            txtEndDate.Attributes.Add("onchange", "JavaScript:UpdateProfile();");
            chkEDDoc.Attributes.Add("onClick", "JavaScript:UpdateProfile();");

            txtPrimaryPhoneAreaCode.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryPhonePrefix.ClientID + "').focus();";
            txtPrimaryPhonePrefix.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryPhoneNNNN.ClientID + "').focus();";
            txtAddPhoneAreaCode.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtAddPhonePrefix.ClientID + "').focus();";
            txtAddPhonePrefix.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtAddPhoneNumber.ClientID + "').focus();";
            txtFaxAreaCode.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtFaxPrefix.ClientID + "').focus();";
            txtFaxPrefix.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtFaxNNNN.ClientID + "').focus();";
            txtCellAreaCode.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtCellPrefix.ClientID + "').focus();";
            txtCellPrefix.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtCellNNNN.ClientID + "').focus();";

            string url = "./add_rp.aspx";
            if (!tblSelect.Visible)
            {
                url = "./directory_maintenance.aspx?DirectoryName=" + Request["DirectoryName"].ToString().Replace("'", "#**#%") + "&DirectoryID=" + ViewState[DIRECTORYID] + "&InstitutionName=" + Request["InstitutionName"].ToString().Replace("'", "#**#%");
            }
            btnCancel.Attributes.Add("onClick", "javascript:Navigate('" + url + "');return false;");
            anchFromDate.Attributes.Add("onClick",
             String.Format(@"javascript:calRPStartDate.select(document.all['{0}'],document.all['{1}'],'MM/dd/yyyy');",
                                            txtStartDate.ClientID,
                                            anchFromDate.ClientID
                                            ));

            anchToDate.Attributes.Add("onClick",
            String.Format(@"javascript:calRPEndDate.select(document.all['{0}'],document.all['{1}'],'MM/dd/yyyy');",
                                           txtEndDate.ClientID,
                                           anchToDate.ClientID
                                           ));

            btnAdd.Attributes.Add("onclick", "javascript: return validation();");
        }
        /// <summary>
        /// Load Departments in combo
        /// </summary>
        private void loadDepartment()
        {
            OrderingClinician  objOC = null;
            DataTable dtDepartments = null;
            DataRow drDepartment = null;
            try
            {
                objOC = new OrderingClinician();
                dtDepartments = objOC.GetDepartmentsByInstitution(Convert.ToInt32(ViewState[INSTITUTIONID]));
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
            finally
            {
                objOC = null;
                dtDepartments = null;
                drDepartment = null;
            }
        }
        /// <summary>
        /// Displays / hides Login details for OC depending upon VoiceClips settings.
        /// </summary>
        private void loadLoginControls()
        {
            Institution objInstitution = null;
            InstitutionInfo objInstInfo = null;
            Group objGroup = null;
            try
            {
                objInstitution = new Institution();
                objInstInfo = objInstitution.GetInstitutionInfo_Obj(Convert.ToInt32(ViewState[INSTITUTIONID].ToString()));
                if (objInstInfo != null)
                {
                    if (objInstInfo.IsRequireVoiceClips)
                    {
                        pnlOCLogin.Visible = false;
                        pnlEDDoc.Visible = true;

                        objGroup = new Group();
                        DataTable dtGroup = objGroup.GetGroupInformation(Convert.ToInt32(loggedUserID));
                        if (dtGroup != null)
                            txtLoginID.Text = "";

                    }
                    else
                    {
                        pnlOCLogin.Visible = false;
                        pnlEDDoc.Visible = false;

                        if (!objInstInfo.MessageRetrieveUsingPIN)
                        {
                            fldLoginDetails.Visible = false;
                        }

                    }
                    if (objInstInfo.MessageRetrieveUsingPIN)
                    {
                        pnlMessageRetieve.Visible = true;
                    }
                    else
                    {
                        pnlMessageRetieve.Visible = false;
                    }


                }
            }
            finally
            {
                objInstInfo = null;
                objInstitution = null;
            }

        }
        /// <summary>
        /// Determines whether [is valid date] [the specified date].
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>true/false</returns>
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
        /// This method will alert the error passed as parameter to the user.
        /// </summary>
        /// <param name="message"></param>
        private void alertErrorToUserForExternalId(string message)
        {
            ScriptManager.RegisterStartupScript(UpdatePanelMessageList, UpdatePanelMessageList.GetType(), "Validationalert", "showMessage(" + '"' + message + '"' + ");", true);
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
            if (txtAddPhonePrefix.Text.Trim().Length != 3 && txtAddPhonePrefix.Text.Trim().Length != 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Additional Phone prefix";
                validated = false;
            }
            if (txtAddPhoneAreaCode.Text.Trim().Length != 3 && txtAddPhoneAreaCode.Text.Trim().Length != 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Additional Phone Area Code";
                validated = false;
            }
            if (txtAddPhoneNumber.Text.Trim().Length != 4 && txtAddPhoneNumber.Text.Trim().Length != 0)
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
            ScriptManager.RegisterStartupScript(UpdatePanelMessageList, UpdatePanelMessageList.GetType(), "Validationalert", "<script language=" + '"' + "javascript" + '"' + ">showMessage(" + '"' + message + '"' + ");</script>", false);
        }

        /// <summary>
        /// This method adds OC devices, assigned events to the devices and 
        /// adds After Hour Notifications in to the Database.
        /// </summary>
        /// <param name="OCID">Ordering Clinician ID</param>
        private void addDevicesAndAfterHours(int OCID)
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
                        objDevice.ReferringPhysicianID = OCID;
                        objDevice.DeviceID = dtGridDeviceNotifications.Rows[countDevices]["DeviceID"] == DBNull.Value ?0 :Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["DeviceID"]);
                        objDevice.DeviceAddress = dtGridDeviceNotifications.Rows[countDevices]["DeviceAddress"] == DBNull.Value ? "" : dtGridDeviceNotifications.Rows[countDevices]["DeviceAddress"].ToString();
                        objDevice.DeviceName = dtGridDeviceNotifications.Rows[countDevices]["DeviceName"] == DBNull.Value ? "" : dtGridDeviceNotifications.Rows[countDevices]["DeviceName"].ToString();
                        objDevice.Gateway = dtGridDeviceNotifications.Rows[countDevices]["Gateway"] == DBNull.Value ? "" : dtGridDeviceNotifications.Rows[countDevices]["Gateway"].ToString();
                        objDevice.Carrier = dtGridDeviceNotifications.Rows[countDevices]["Carrier"] == DBNull.Value ? "" : dtGridDeviceNotifications.Rows[countDevices]["Carrier"].ToString();
                        objDevice.InitialPauseTime = dtGridDeviceNotifications.Rows[countDevices]["InitialPause"] == DBNull.Value ? "-1" : dtGridDeviceNotifications.Rows[countDevices]["InitialPause"].ToString();
                        objDevice.GroupID = dtGridDeviceNotifications.Rows[countDevices]["GroupID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["GroupID"]);
                        objDevice.FindingID = dtGridDeviceNotifications.Rows[countDevices]["FindingID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["FindingID"]);
                        objDevice.OCNotifyEventID = dtGridDeviceNotifications.Rows[countDevices]["RPNotifyEventID"] == DBNull.Value ? 0 : Convert.ToInt32(dtGridDeviceNotifications.Rows[countDevices]["RPNotifyEventID"]);

                        deviceID = objOC.InsertOCDevice(objDevice, "ADD");// get new OCdeviceID which is added recently.
                        DataRow [] drAfterHour = dtGridAfterHours.Select("DeviceName = '" + objDevice.DeviceName + "'");
                        if (drAfterHour.GetLength(0) >0)
                        {   
                            int numofAH = drAfterHour.GetLength(0);
                            for (int countAH = 0;countAH<numofAH ; countAH++)
                            {
                                objDevice.StartHour = drAfterHour[countAH]["StartHour"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["StartHour"]);
                                objDevice.EndHour = drAfterHour[countAH]["EndHour"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["EndHour"]);
                                objDevice.GroupID = drAfterHour[countAH]["GroupID"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["GroupID"]);
                                objDevice.FindingID = drAfterHour[countAH]["FindingID"] == DBNull.Value ? 0 : Convert.ToInt32(drAfterHour[countAH]["FindingID"]);
                                objDevice.OCDeviceID = deviceID;

                                objOC.InsertAfterHoursNotifications(objDevice, "ADD");

                            }
                        }
                        objDevice = null;
                        objOC = null;
                        drAfterHour = null;

                    }
                }
                catch (Exception ex)
                {
                    if (Session[SessionConstants.USER_ID] != null)
                    {
                        Tracer.GetLogger().LogExceptionEvent("add_rp.addDevicesAndAfterHours:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                    }
                    throw ex;
                }
                
            }
        }


        /// <summary>
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
        /// Sets the height(standard for all grids) for the Grid Element.
        /// </summary>
        /// <param name="grdElement"></param>
        /// <param name="divElement"></param>
        private void generateDataGridHeight(DataGrid grdElement, HtmlGenericControl divElement)
        {
            int nDevicesGridHeight = 20;
            int gridItemHeight = 21;
            int gridHeaderHeight = 26;
            int maxRows = 4;
            int rowCount = grdElement.Items.Count;

            if (rowCount < maxRows)
            {
                nDevicesGridHeight = (grdElement.Items.Count * gridItemHeight) + gridHeaderHeight;
            }
            else
            {
                nDevicesGridHeight = (maxRows * gridItemHeight) + gridHeaderHeight;
            }

            string newUid = this.UniqueID.Replace(":", "_");
            string script = "";

            if (rowCount == 0)
            {
                script += "document.getElementById(" + '"' + divElement.ClientID + '"' + ").style.height=0;";
                script += "document.getElementById(" + '"' + divElement.ClientID + '"' + ").style.border=0;";//</script>";
            }
            else
            {
                script += "document.getElementById(" + '"' + divElement.ClientID + '"' + ").style.height='" + (nDevicesGridHeight + 1) + "';";//</script>";

            }

            ScriptManager.RegisterStartupScript(UpdatePanelMessageList, UpdatePanelMessageList.GetType(), newUid, script, true);
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

        #region Step1_1
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
                    Tracer.GetLogger().LogExceptionEvent("add_rp.fillDeviceDDL():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                    dtGroup = objOC.GetGroupsByDirectoryID(Convert.ToInt32(ViewState[DIRECTORYID]), true);
                    allGroupName = "All Lab Groups";
                }
                else
                {
                    dtGroup = objOC.GetGroupsByDirectoryID(Convert.ToInt32(ViewState[DIRECTORYID]), false);
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
                if (Session[SessionConstants.USER_ID]  != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("add_rp.fillGroupDDL:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                    Tracer.GetLogger().LogExceptionEvent("add_rp.fillEventDDL:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                    Tracer.GetLogger().LogExceptionEvent("add_rp.fillFindingDDL:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                else if (groupID > 0)
                {
                    dtFindings = objOC.GetFindingForOCorGroup(-1, groupID);

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

                if (!objFiding.ID.ToUpper().Equals("CMBFINDINGS") && groupID != -1)
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
                    Tracer.GetLogger().LogExceptionEvent("add_rp.fillFindingDDL:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
        /// This method creates a table structure which hold OC Device 
        /// Notification information and After Hours information.
        /// </summary>
        private void CreateTable()
        {
            try
            {
                if (dtGridDeviceNotifications.Columns.Count <= 0)
                {
                    dtGridDeviceNotifications.Rows.Clear();
                    dtGridDeviceNotifications.Columns.Add("DeviceName", Type.GetType("System.String"));
                    dtGridDeviceNotifications.Columns.Add("DeviceAddress", Type.GetType("System.String"));
                    dtGridDeviceNotifications.Columns.Add("Gateway", Type.GetType("System.String"));
                    dtGridDeviceNotifications.Columns.Add("Carrier", Type.GetType("System.String"));
                    dtGridDeviceNotifications.Columns.Add("GroupName", Type.GetType("System.String"));
                    dtGridDeviceNotifications.Columns.Add("EventDescription", Type.GetType("System.String"));
                    dtGridDeviceNotifications.Columns.Add("FindingDescription", Type.GetType("System.String"));
                    dtGridDeviceNotifications.Columns.Add("InitialPause", Type.GetType("System.String"));
                    dtGridDeviceNotifications.Columns.Add("DeviceID", Type.GetType("System.Int32"));
                    dtGridDeviceNotifications.Columns.Add("GroupID", Type.GetType("System.Int32"));
                    dtGridDeviceNotifications.Columns.Add("RPNotifyEventID", Type.GetType("System.Int32"));
                    dtGridDeviceNotifications.Columns.Add("FindingID", Type.GetType("System.Int32"));
                    dtGridDeviceNotifications.Columns.Add("RowID", Type.GetType("System.Int32"));
                    Session[SessionConstants.DT_ADD_NOTIFICATION] = dtGridDeviceNotifications;
                }
                if (dtGridAfterHours.Columns.Count <= 0)
                {
                    dtGridAfterHours.Rows.Clear();
                    dtGridAfterHours.Columns.Add("DeviceName", Type.GetType("System.String"));
                    dtGridAfterHours.Columns.Add("GroupName", Type.GetType("System.String"));
                    dtGridAfterHours.Columns.Add("FindingDescription", Type.GetType("System.String"));
                    dtGridAfterHours.Columns.Add("StartHour", Type.GetType("System.Int32"));
                    dtGridAfterHours.Columns.Add("EndHour", Type.GetType("System.Int32"));
                    dtGridAfterHours.Columns.Add("RPDeviceID", Type.GetType("System.Int32"));
                    dtGridAfterHours.Columns.Add("GroupID", Type.GetType("System.Int32"));
                    dtGridAfterHours.Columns.Add("FindingID", Type.GetType("System.Int32"));
                    dtGridAfterHours.Columns.Add("AfterHourRowNo", Type.GetType("System.Int32"));
                    Session[SessionConstants.DT_ADD_AFTERHOUR] = dtGridAfterHours;
                }
            }
            catch(Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("add_rp.CreateTable:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                    Tracer.GetLogger().LogExceptionEvent("add_rp.generateGatewayAddress:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                        txtInitialPause.Visible = false;
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
                        
                        txtNumAddress.Visible = true;
                        txtNumAddress.Width = Unit.Pixel(175);
                        txtNumAddress.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes(textChangedClientID);");
                        txtNumAddress.AutoPostBack = false;
                        lblNumAddress.Visible = true;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtEmailGateway.Text = "Enter TAP 800 number (numbers only)";
                        
                        txtEmailGateway.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes(textChangedClientID);");
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
                        txtNumAddress.Text = "";
                        lblNumAddress.Visible = false;
                        cmbCarrier.Visible = false;
                        lblCarrier.Visible = false;
                        txtEmailGateway.Visible = false;
                        txtEmailGateway.Text = "";
                        lblEmailGateway.Visible = false;                        
                        txtInitialPause.Visible = false;                        
                        btnAddDevice.Visible = false;
                        lblInitialPause.Visible = false;
                        break;

                }

            }

            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("add_rp.setLabelsAndInputBoxes:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                    string deviceAdd = txtNumAddress.Text;
                    string gateway = "";
                    string carrier = "";
                    string initialPause = "";
                    if (cmbDeviceType.Items.Count > 0 && cmbDeviceType.SelectedItem.Text.Equals("Email"))
                    {
                        deviceAdd = txtEmailGateway.Text.Trim();
                    }

                    //Set Gateway property value            
                    if (int.Parse(cmbDeviceType.SelectedValue) == (int)NotificationDevice.PagerTAP || int.Parse(cmbDeviceType.SelectedValue) == (int)NotificationDevice.PagerTAPA) //TAP DEVICE
                    {
                        gateway = txtEmailGateway.Text.Trim();
                    }
                    else if ((!(cmbDeviceType.Items.Count > 0 && cmbDeviceType.SelectedItem.Text.Equals("Email"))) && txtEmailGateway.Text.Trim().Length > 0)
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
                        initialPause = txtInitialPause.Text.Trim();
                    }
                    else
                    {
                        initialPause = "";
                    }

                    DataRow dtrow = dtGridDeviceNotifications.NewRow();
                    dtrow["DeviceName"] = generateDeviceName(Convert.ToInt32(cmbDeviceType.SelectedValue));
                    dtrow["DeviceAddress"] = deviceAdd;
                    dtrow["Gateway"] = gateway;
                    dtrow["Carrier"] = carrier;
                    dtrow["GroupName"] = (cmbGroup.Visible) ? cmbGroup.SelectedItem.Text : " ";
                    dtrow["EventDescription"] = (cmbEvents.Visible) ? cmbEvents.SelectedItem.Text : " ";
                    dtrow["FindingDescription"] = (cmbFindings.Visible) ? cmbFindings.SelectedItem.Text : " ";
                    dtrow["InitialPause"] = initialPause;
                    dtrow["DeviceID"] = Convert.ToInt32(cmbDeviceType.SelectedValue);
                    dtrow["GroupID"] = (cmbGroup.Visible) ? Convert.ToInt32(cmbGroup.SelectedValue) : 0;                    
                    dtrow["RPNotifyEventID"] = (cmbEvents.Visible) ? Convert.ToInt32(cmbEvents.SelectedValue) : 0;
                    dtrow["FindingID"] = (cmbFindings.Visible) ? Convert.ToInt32(cmbFindings.SelectedValue) : 0;
                    dtrow["RowID"] = RowNo;
                    
                    RowNo++;
                    dtGridDeviceNotifications.Rows.Add(dtrow);

                    Session[SessionConstants.DT_ADD_NOTIFICATION] = dtGridDeviceNotifications;
                    dataBindStep1();
                    resetControls();
                    fillAfterHoursDeviceOptions();
                    generateStep3DataGridHeight();
                    upnlStep3.Update();
                    
                }
                else
                {
                    //generateDataGridHeight("Validate");
                }

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID].ToString().Length > 0)
                    Tracer.GetLogger().LogExceptionEvent("add_rp.addOCNotificationDevices():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                string expression = "DeviceName like '" + deviceShortName + "%'"  ;

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
                
                if ((pin.Trim() == "") || (pin.Trim() == pintext.Trim()))
                {
                    error = "Please enter PIN Number." + @"\n";
                    focus = txtNumAddress.ClientID;
                }
                
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
                    ScriptManager.RegisterClientScriptBlock(upnlStep1,upnlStep1.GetType(), "Register1", acRegScript.ToString(),false);
                    generateDataGridHeight("validatedevice1");
                    return false;
                }
            }
            else //if (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.SMS_WebLink)
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
            //     }
                    
            //}
            //else
            {
                string deviceAdd = txtNumAddress.Text.Trim();
                if (int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.EMail && int.Parse(cmbDeviceType.SelectedItem.Value) != (int)NotificationDevice.DesktopAlert)
                {
                    string error = "";

                    if (int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.Fax || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.SMS || int.Parse(cmbDeviceType.SelectedItem.Value) == (int)NotificationDevice.SMS_WebLink)
                    {
                        if (deviceAdd.Length != 10)
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
                        ScriptManager.RegisterClientScriptBlock(upnlStep1,upnlStep1.GetType(), "Register", acRegScript.ToString(),false);
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
                grdDevices.DataSource = dtGridDeviceNotifications;
                grdDevices.DataBind();
                generateDataGridHeight("EditDevice");

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
                   
                    tbEmailGateway.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
                    
                }
                else if (deviceType == (int)NotificationDevice.PagerNumSkyTel || deviceType == (int)NotificationDevice.PagerNumUSA || deviceType == (int)NotificationDevice.PagerNumRegular || deviceType == (int)NotificationDevice.pagerPartner)
                {
                    tbDevice.Attributes.Add("onkeyPress", "JavaScript:return PagerValidationWithSpace('" + tbDevice.ClientID + "');");
                   
                }
                else if (deviceType == (int)NotificationDevice.PagerAlpha)
                {
                    tbDevice.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes(textChangedClientID);");
                    
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
                    
                }
                else
                {
                    txtEmailGateway.Attributes.Add("onkeypress", "JavaScript:return true;");
                    txtEmailGateway.MaxLength = 100;
                }

                fillEventDDL(dlEvent);
                fillGroupDDL(dlGroups, deviceType);
                fillGridFindingDDL(groupID,dlFinding,findingID);
               
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
                    Tracer.GetLogger().LogExceptionEvent("add_rp.editDeviceGrid:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
            int deviceType = int.Parse(grdDevices.Items[e.Item.ItemIndex].Cells[10].Text);
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
                case (int)NotificationDevice.SMS_WebLink:
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
                    if (!Utils.RegExMatch(gridEmailGatewayTxt.Text.Trim()))
                    {
                        if (errorMessage.Length == 0)
                            gridEmailGatewayTxt.Focus();
                        errorMessage += "Please enter valid E-mail ID.\\n";
                    }
                    if ((Utils.RegExNumericMatch(gridDeviceNumberTxt.Text.Trim())) == false)
                    {
                        if (errorMessage.Length == 0)
                        {
                            gridDeviceNumberTxt.Focus();
                        }
                        errorMessage += "Please enter valid Pager Number\\n";

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
            grdDevices.DataSource = dtGridDeviceNotifications ;
            grdDevices.DataBind();
            generateDataGridHeight("dataBind");
            if (dtGridDeviceNotifications.Rows.Count>0)
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

        #endregion Step1_1
        #region Step2_2
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
                    Tracer.GetLogger().LogExceptionEvent("add_rp.fillAfterHoursFindingDDL:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                dtGroup = objOC.GetGroupsByDirectoryID(Convert.ToInt32(ViewState[DIRECTORYID]), false);
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
                    Tracer.GetLogger().LogExceptionEvent("add_rp.fillGroupDDLStep3:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
                //Create dataview of all devices avaiable.
                DataView dvAfterHourDevices = new DataView(dtGridDeviceNotifications);
                //Filter dataview with Email and Fax devices
                dvAfterHourDevices.RowFilter = "DeviceID in (" + NotificationDevice.EMail.GetHashCode() + "," + NotificationDevice.Fax.GetHashCode() + ")"; //Email and Fax only
                
                cmbAHDevice.DataSource = dvAfterHourDevices;
                cmbAHDevice.DataBind();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("add_rp.fillAfterHoursDeviceOptions:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
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
        /// This method binds the after hours data to the grdAfterHoursNotifications data grid
        /// </summary>
        private void dataBindStep2()
        {
            grdAfterHoursNotifications.DataSource = dtGridAfterHours;
            grdAfterHoursNotifications.DataBind();

            if (dtGridAfterHours.Rows.Count > 0)
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
        #endregion Step2_2
        #endregion Private Methods


    }
}
