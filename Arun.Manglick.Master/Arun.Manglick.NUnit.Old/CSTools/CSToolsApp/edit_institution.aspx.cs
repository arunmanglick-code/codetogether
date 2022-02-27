#region File History

/******************************File History***************************
 * File Name        : edit_institute.aspx.cs
 * Author           : Prerak Shah.
 * Created Date     : 09-07-2007
 * Purpose          : This Class will Edit Institute and its preferences.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification
 * -------------------------------------------------------------------
 * 14-12-2007   IAK     Added BatchMessages Checkbox
 * 03-20-2008   SSK     Added radiobutton for "Allow PIN for Message Retrieval"
 * 05-07-2008   Suhas   Defect 2979: Auto Tab issue.
 * 30-05-2008   Suhas   Added Enable Call Center Flag.
 * 12 Jun 2008  Prerak  Migration of AJAX Atlas to AJAX RTM 1.0
 * 12-Sep-2008  IAK     Added Preference 'Prompt for PIN for CT Message'
 * ------------------------------------------------------------------- 
 */
#endregion
using System;
using System.Data;
using System.Configuration;
using System.Collections;
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

namespace Vocada.CSTools
{
  
    public partial class edit_institution : System.Web.UI.Page
    {
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
             try
            {
                if (Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_ID] == null)
                    Response.Redirect(Utils.GetReturnURL("default.aspx", "edit_institution.aspx", this.Page.ClientQueryString));

                //if (Session["UserSettings"] != null)
                //    strUserSettings = Session["UserSettings"].ToString();
                Page.SmartNavigation = true;
                registerJavascriptVariables();
                if (!Page.IsPostBack)
                {
                    txtInstitutionName.Focus();
                    //ViewState["InstitutionID"] = QueryStringParameter[""]; ;
                    fillTimeZones();
                    PopulateInstitutionInformation();
                }
                if (rblRequireVoiceClips.SelectedValue == "1")
                    pnlTabName.Visible = true;
                Session[SessionConstants.CURRENT_TAB] = "SystemAdmin";
                Session[SessionConstants.CURRENT_INNER_TAB] = "EditInstitution";
                Session[SessionConstants.CURRENT_PAGE] = "edit_institution.aspx";

                this.Form.DefaultButton = this.btnEditInstitution.UniqueID;

            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("edit_institution - Page_Load", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }
        protected void btnEditInstitution_Click(object sender, EventArgs e)
        {
            if (Session[SessionConstants.USER_ID] != null)
            {
                EditInstitutionData();
                hdnTextChanged.Value = "false";
                //ViewState["InstitutionID"] = institutionID;
                ScriptManager.RegisterStartupScript(upnlEditInstitution, upnlEditInstitution.GetType(), "EditInstitution", "alert('Institution information updated successfully.');Navigate();", true);
            }
            
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ViewState["InstitutionID"] = null;
            ScriptManager.RegisterClientScriptBlock(upnlEditInstitution,upnlEditInstitution.GetType(),"NavigateToDir", "<script type=\'text/javascript\'>Navigate();</script>",false);
        }
        /// <summary>
        /// Enable / Disables ConnectED option depending upon the RequireVoiceClips option.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblRequireVoiceClips_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblRequireVoiceClips.SelectedValue == "1")
            {
               pnlTabName.Visible = true;
            }
            else
            {
                pnlTabName.Visible = false;
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// This method is used to fill the Time Zone combo box
        /// </summary>
        /// <param name="cnx"></param>
        private void fillTimeZones()
        {
            Institution objInstitution = new Institution();
            DataTable dtTimeZone = new DataTable();
            dtTimeZone = objInstitution.GetTimeZone();
            drpTimeZone.DataSource = dtTimeZone;
            
            drpTimeZone.DataBind();
                      
            objInstitution = null;
        }
        /// <summary>
        /// Edit Institution record in database
        /// </summary>
        private void EditInstitutionData()
        {
            Institution objInstitution = new Institution();
            InstitutionInformation objInstitutionInfo = new InstitutionInformation();
            //int institutionID;
            try
            {
                objInstitutionInfo.InstitutionID = Convert.ToInt32(txtInstitutionID.Text.Trim());
                objInstitutionInfo.InstitutionName = txtInstitutionName.Text.Trim();
                objInstitutionInfo.Address1 = txtAddress1.Text.Trim();
                objInstitutionInfo.Address2 = txtAddress2.Text.Trim();
                objInstitutionInfo.City = txtCity.Text.Trim();
                objInstitutionInfo.State = txtstate.Text.Trim();
                objInstitutionInfo.Zip = txtZip.Text.Trim();
                objInstitutionInfo.MainPhoneNumber = txtMainNumber1.Text + txtMainNumber2.Text + txtMainNumber3.Text;
                objInstitutionInfo.PrimaryContactName = txtPrimaryName.Text.Trim();
                objInstitutionInfo.PrimaryContactTitle = txtPrimaryTitle.Text.Trim();
                objInstitutionInfo.PrimaryContactPhone = txtPrimaryPhone1.Text + txtPrimaryPhone2.Text + txtPrimaryPhone3.Text;
                objInstitutionInfo.PrimaryContactEmail = txtPrimaryEmail.Text.Trim(); 
                objInstitutionInfo.Contact1Name = txtContact1Name.Text.Trim();
                objInstitutionInfo.Contact1Title = txtContact1Title.Text.Trim();
                objInstitutionInfo.Contact1Phone = txtContact1Phone1.Text + txtContact1Phone2.Text + txtContact1Phone3.Text;
                objInstitutionInfo.Contact1Email = txtContact1email.Text.Trim();
                objInstitutionInfo.Contact2Name = txtContact2Name.Text.Trim();
                objInstitutionInfo.Contact2Title = txtContact2Title.Text.Trim();
                objInstitutionInfo.Contact2Phone = txtContact2Phone1.Text + txtContact2Phone2.Text + txtContact2Phone3.Text;
                objInstitutionInfo.Contact2Email = txtContact2Email.Text.Trim();
                objInstitutionInfo.Lab800Number = txtLab800No1.Text + txtLab800No2.Text + txtLab800No3.Text;
                objInstitutionInfo.ShiftNurse800Number = txtShiftNurce800No1.Text + txtShiftNurce800No2.Text + txtShiftNurce800No3.Text;
                objInstitutionInfo.Nurse800Number = txtNurse800No1.Text + txtNurse800No2.Text + txtNurse800No3.Text;
                objInstitutionInfo.TimeZone = Convert.ToInt32(drpTimeZone.SelectedItem.Value);

                //preferences
                objInstitutionInfo.IsRequireCallBackVoiceOver = rblCallbackVoiceOver.SelectedValue == "1" ? true : false;
                objInstitutionInfo.IsRequireNameCapture = rblNameCapture.SelectedValue == "1" ? true : false;
                objInstitutionInfo.IsRequireNameCaptureValidation = rblNameCaptureValidation.SelectedValue == "1" ? true : false;
                objInstitutionInfo.IsRequireReadbackMeasurement = rblReadback.SelectedValue == "1" ? true : false;
                objInstitutionInfo.IsRequireAcceptanceOutboundCall = rblOutboundCall.SelectedValue == "1" ? true : false;
                objInstitutionInfo.IsRequireEDMessage = rblRequireVoiceClips.SelectedValue == "1" ? true : false;
                objInstitutionInfo.IsRequireExamDescription = rblRequireExamDescription.SelectedValue == "1" ? true : false;
                objInstitutionInfo.TabName = rblConnectED.SelectedValue == "1" ? true : false;
                objInstitutionInfo.BatchMessage= rblBatchMessages.SelectedValue == "1" ? true : false;
                objInstitutionInfo.MessageRetrieveUsingPIN = rblMessageRetrieveUsingPIN.SelectedValue == "1" ? true : false;
                objInstitutionInfo.EnableCallCenter = rblEnableCallCenter.SelectedValue == "1" ? true : false;
                objInstitutionInfo.EnablePromptForPin = rblPinForCT.SelectedValue == "1" ? true : false;
                objInstitution.UpdateInstitution(objInstitutionInfo);
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("edit_institution - EditInstitutionData", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                objInstitution = null;
                objInstitutionInfo = null;
            }
        }

        /// <summary>
        /// This function populates Institution information
        /// </summary>
        private void PopulateInstitutionInformation()
        {
            DataTable dtInstitutionInfo = null;
            Institution objInstitution = new Institution();
            try
            {
                dtInstitutionInfo = new DataTable();
                if (Request["InstitutionID"] != null)
                {
                    dtInstitutionInfo = objInstitution.GetInstitutionInfo(Convert.ToInt32(Request["InstitutionID"].ToString()));
                    if (dtInstitutionInfo.Rows.Count > 0)
                    {
                        DataRow drInstitutionInfo = dtInstitutionInfo.Rows[0];//= new DataRow();
                        //drInstitutionInfo = dtInstitutionInfo.Rows[0];
                        txtInstitutionName.Text = drInstitutionInfo["InstitutionName"].ToString().Trim();
                        txtAddress1.Text = drInstitutionInfo["Address1"].ToString().Trim();
                        txtAddress2.Text = drInstitutionInfo["Address2"].ToString().Trim();
                        txtCity.Text = drInstitutionInfo["City"].ToString().Trim();
                        txtstate.Text = drInstitutionInfo["State"].ToString().Trim();
                        txtZip.Text = drInstitutionInfo["Zip"].ToString().Trim();
                        string mainPhoneNumber = Utils.flattenPhoneNumber(drInstitutionInfo["MainPhoneNumber"].ToString().Trim());
                        if (mainPhoneNumber.Length >= 10)
                        {
                            txtMainNumber1.Text = mainPhoneNumber.Substring(0, 3);
                            txtMainNumber2.Text = mainPhoneNumber.Substring(3, 3);
                            txtMainNumber3.Text = mainPhoneNumber.Substring(6, 4);
                        }
                        txtPrimaryName.Text = drInstitutionInfo["PrimaryContactName"].ToString().Trim();
                        txtPrimaryEmail.Text = drInstitutionInfo["PrimaryContactEmail"].ToString().Trim();
                        txtPrimaryTitle.Text = drInstitutionInfo["PrimaryContactTitle"].ToString().Trim();
                        string primaryPhone = Utils.flattenPhoneNumber(drInstitutionInfo["PrimaryContactPhone"].ToString().Trim());
                        if (primaryPhone.Length >= 10)
                        {
                            txtPrimaryPhone1.Text = primaryPhone.Substring(0, 3);
                            txtPrimaryPhone2.Text = primaryPhone.Substring(3, 3);
                            txtPrimaryPhone3.Text = primaryPhone.Substring(6, 4);
                        }
                        txtContact1email.Text = drInstitutionInfo["Contact1Email"].ToString().Trim();
                        txtContact1Name.Text = drInstitutionInfo["Contact1Name"].ToString().Trim();
                        txtContact1Title.Text = drInstitutionInfo["Contact1Title"].ToString().Trim();
                        string contact1Phone = Utils.flattenPhoneNumber(drInstitutionInfo["Contact1Phone"].ToString().Trim());
                        if (contact1Phone.Length >= 10)
                        {
                            txtContact1Phone1.Text = contact1Phone.Substring(0, 3);
                            txtContact1Phone2.Text = contact1Phone.Substring(3, 3);
                            txtContact1Phone3.Text = contact1Phone.Substring(6, 4);
                        }
                        txtContact2Email.Text = drInstitutionInfo["Contact2Email"].ToString().Trim();
                        txtContact2Name.Text = drInstitutionInfo["Contact2Name"].ToString().Trim();
                        txtContact2Title.Text = drInstitutionInfo["Contact2Title"].ToString().Trim();
                        string contact2Phone = drInstitutionInfo["Contact2Phone"].ToString().Trim();
                        if (contact2Phone.Length >= 10)
                        {
                            txtContact2Phone1.Text = contact2Phone.Substring(0, 3);
                            txtContact2Phone2.Text = contact2Phone.Substring(3, 3);
                            txtContact2Phone3.Text = contact2Phone.Substring(6, 4);
                        }
                        txtInstitutionID.Text = drInstitutionInfo["InstitutionID"].ToString().Trim();
                        string lab800Number = Utils.flattenPhoneNumber(drInstitutionInfo["Lab800Number"].ToString().Trim());
                        if (lab800Number.Length >= 10)
                        {
                            txtLab800No1.Text = lab800Number.Substring(0, 3);
                            txtLab800No2.Text = lab800Number.Substring(3, 3);
                            txtLab800No3.Text = lab800Number.Substring(6, 4);
                        }
                        string nurse800Number = Utils.flattenPhoneNumber(drInstitutionInfo["Nurse800Number"].ToString().Trim());
                        if (nurse800Number.Length >= 10)
                        {
                            txtNurse800No1.Text = nurse800Number.Substring(0, 3);
                            txtNurse800No2.Text = nurse800Number.Substring(3, 3);
                            txtNurse800No3.Text = nurse800Number.Substring(6, 4);
                        }
                        string shiftNurce800Number = Utils.flattenPhoneNumber(drInstitutionInfo["ShiftNurse800Number"].ToString().Trim());
                        if (shiftNurce800Number.Length >= 10)
                        {
                            txtShiftNurce800No1.Text = shiftNurce800Number.Substring(0, 3);
                            txtShiftNurce800No2.Text = shiftNurce800Number.Substring(3, 3);
                            txtShiftNurce800No3.Text = shiftNurce800Number.Substring(6, 4);
                        }
                        drpTimeZone.SelectedValue = drInstitutionInfo["TimeZoneID"].ToString();
                        if (drInstitutionInfo["RequireNameCapture"].ToString().Length > 0)
                            rblNameCapture.SelectedValue = Convert.ToInt32(drInstitutionInfo["RequireNameCapture"]).ToString();
                        else
                            rblNameCapture.SelectedValue = "0";
                        if (drInstitutionInfo["ValidateNameCapture"].ToString().Length > 0)
                            rblNameCaptureValidation.SelectedValue = Convert.ToInt32(drInstitutionInfo["ValidateNameCapture"]).ToString();
                        else
                            rblNameCaptureValidation.SelectedValue = "0";
                        if (drInstitutionInfo["RequireReadbackMeasurement"].ToString().Length > 0)
                            rblReadback.SelectedValue = Convert.ToInt32(drInstitutionInfo["RequireReadbackMeasurement"]).ToString();
                        else
                            rblReadback.SelectedValue = "0";
                        if (drInstitutionInfo["RequireCallBackVoiceOver"].ToString().Length > 0)
                            rblCallbackVoiceOver.SelectedValue = Convert.ToInt32(drInstitutionInfo["RequireCallBackVoiceOver"]).ToString();
                        else
                            rblCallbackVoiceOver.SelectedValue = "0";
                        if (drInstitutionInfo["RequireAcceptanceOutbound"].ToString().Length > 0)
                            rblOutboundCall.SelectedValue = Convert.ToInt32(drInstitutionInfo["RequireAcceptanceOutbound"]).ToString();
                        else
                            rblOutboundCall.SelectedValue = "0";
                        if (drInstitutionInfo["RequireVoiceClips"].ToString().Length > 0)
                            rblRequireVoiceClips.SelectedValue = Convert.ToInt32(drInstitutionInfo["RequireVoiceClips"]).ToString();
                        else
                            rblRequireVoiceClips.SelectedValue = "0";
                        if (drInstitutionInfo["IsConnectED"].ToString().Length > 0)
                            rblConnectED.SelectedValue = Convert.ToInt32(drInstitutionInfo["IsConnectED"]).ToString();
                        else
                            rblConnectED.SelectedValue = "0";
                        if (drInstitutionInfo["BatchMessages"].ToString().Length > 0)
                            rblBatchMessages.SelectedValue = Convert.ToInt32(drInstitutionInfo["BatchMessages"]).ToString();
                        else
                            rblConnectED.SelectedValue = "0";
                        if (drInstitutionInfo["RequireExamDescription"].ToString().Length > 0)
                            rblRequireExamDescription.SelectedValue = Convert.ToInt32(drInstitutionInfo["RequireExamDescription"]).ToString();
                        else
                            rblRequireExamDescription.SelectedValue = "0";
                        
                        if (drInstitutionInfo["MessageRetrieveUsingPIN"].ToString().Length > 0)
                            rblMessageRetrieveUsingPIN.SelectedValue = Convert.ToInt32(drInstitutionInfo["MessageRetrieveUsingPIN"]).ToString();
                        else
                            rblMessageRetrieveUsingPIN.SelectedValue = "0";

                        if (drInstitutionInfo["EnableCallCenter"].ToString().Length > 0)
                            rblEnableCallCenter.SelectedValue = Convert.ToInt32(drInstitutionInfo["EnableCallCenter"]).ToString();
                        else
                            rblEnableCallCenter.SelectedValue = "0";

                        if (drInstitutionInfo["PromptForPin"].ToString().Length > 0)
                            rblPinForCT.SelectedValue = Convert.ToInt32(drInstitutionInfo["PromptForPin"]).ToString();
                        else
                            rblPinForCT.SelectedValue = "0";
                    }
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("edit_institution - PopulateInstitutionData", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
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

            txtInstitutionName.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtAddress1.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtAddress2.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtCity.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtstate.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtZip.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtPrimaryName.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtPrimaryEmail.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtPrimaryTitle.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtContact1Name.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtContact1email.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtContact1Title.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtContact2Name.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtContact2Email.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtContact2Title.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            rblCallbackVoiceOver.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblNameCapture.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblNameCaptureValidation.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblReadback.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblOutboundCall.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            //rblRequireVoiceClips.Attributes.Add("onclick", "JavaScript:return enabledControl();");
            rblConnectED.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblBatchMessages.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblRequireExamDescription.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblMessageRetrieveUsingPIN.Attributes.Add("onclick", "JavaScript:return TextChanged();");

            txtMainNumber1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtMainNumber2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtMainNumber3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtPrimaryPhone1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtPrimaryPhone2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtPrimaryPhone3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtContact1Phone1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtContact1Phone2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtContact1Phone3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtContact2Phone1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtContact2Phone2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtContact2Phone3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtNurse800No1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtNurse800No2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtNurse800No3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtLab800No1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtLab800No2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtLab800No3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtShiftNurce800No1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtShiftNurce800No2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtShiftNurce800No3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtstate.Attributes.Add("onKeypress", "JavaScript:isAlphabetKetStroke();");

            // add Javascript client-side code to move through phone# fields...
            txtMainNumber1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtMainNumber2.ClientID + "').focus()";
            txtMainNumber2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtMainNumber3.ClientID + "').focus()";
            //txtMainNumber3.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + txtPrimaryName.ClientID + "').focus()";

            txtPrimaryPhone1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryPhone2.ClientID + "').focus()";
            txtPrimaryPhone2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryPhone3.ClientID + "').focus()";
           // txtPrimaryPhone3.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + txtPrimaryEmail.ClientID + "').focus()";

            txtContact1Phone1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtContact1Phone2.ClientID + "').focus()";
            txtContact1Phone2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtContact1Phone3.ClientID + "').focus()";
            //txtContact1Phone3.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + txtContact1email.ClientID + "').focus()";

            txtContact2Phone1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtContact2Phone2.ClientID + "').focus()";
            txtContact2Phone2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtContact2Phone3.ClientID + "').focus()";
            //txtContact2Phone3.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + txtContact2Email.ClientID + "').focus()";

            txtNurse800No1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtNurse800No2.ClientID + "').focus()";
            txtNurse800No2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtNurse800No3.ClientID + "').focus()";
            //txtNurse800No3.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + drpTimeZone.ClientID + "').focus()";

            txtLab800No1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtLab800No2.ClientID + "').focus()";
            txtLab800No2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtLab800No3.ClientID + "').focus()";
            //txtLab800No3.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + txtNurse800No1.ClientID + "').focus()";

            txtShiftNurce800No1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtShiftNurce800No2.ClientID + "').focus()";
            txtShiftNurce800No2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtShiftNurce800No3.ClientID + "').focus()";
            //txtShiftNurce800No3.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + rblCallbackVoiceOver.ClientID + "').focus()";

            StringBuilder sbScript1 = new StringBuilder();
            sbScript1.Append("<script language=JavaScript>");
            sbScript1.Append("var txtInstitutionNameClientID = '" + txtInstitutionName.ClientID + "';");
            sbScript1.Append("var cmbTimeZoneClientID = '" + drpTimeZone.ClientID + "';");
            sbScript1.Append("var PrimaryNameClientID = '" + txtPrimaryName.ClientID + "';");
            sbScript1.Append("var txtMainNumber1ClientID = '" + txtMainNumber1.ClientID + "';");
            sbScript1.Append("var txtMainNumber2ClientID = '" + txtMainNumber2.ClientID + "';");
            sbScript1.Append("var txtMainNumber3ClientID = '" + txtMainNumber3.ClientID + "';");
            sbScript1.Append("var txtPrimaryPhone1ClientID = '" + txtPrimaryPhone1.ClientID + "';");
            sbScript1.Append("var txtPrimaryPhone2ClientID = '" + txtPrimaryPhone2.ClientID + "';");
            sbScript1.Append("var txtPrimaryPhone3ClientID = '" + txtPrimaryPhone3.ClientID + "';");
            sbScript1.Append("var txtContact1Phone1ClientID = '" + txtContact1Phone1.ClientID + "';");
            sbScript1.Append("var txtContact1Phone2ClientID = '" + txtContact1Phone2.ClientID + "';");
            sbScript1.Append("var txtContact1Phone3ClientID = '" + txtContact1Phone3.ClientID + "';");
            sbScript1.Append("var txtContact2Phone1ClientID = '" + txtContact2Phone1.ClientID + "';");
            sbScript1.Append("var txtContact2Phone2ClientID = '" + txtContact2Phone2.ClientID + "';");
            sbScript1.Append("var txtContact2Phone3ClientID = '" + txtContact2Phone3.ClientID + "';");
            sbScript1.Append("var txtNurse800No1ClientID = '" + txtNurse800No1.ClientID + "';");
            sbScript1.Append("var txtNurse800No2ClientID = '" + txtNurse800No2.ClientID + "';");
            sbScript1.Append("var txtNurse800No3ClientID = '" + txtNurse800No3.ClientID + "';");
            sbScript1.Append("var txtLab800No1ClientID = '" + txtLab800No1.ClientID + "';");
            sbScript1.Append("var txtLab800No2ClientID = '" + txtLab800No2.ClientID + "';");
            sbScript1.Append("var txtLab800No3ClientID = '" + txtLab800No3.ClientID + "';");
            sbScript1.Append("var txtShiftNurce800No1ClientID = '" + txtShiftNurce800No1.ClientID + "';");
            sbScript1.Append("var txtShiftNurce800No2ClientID = '" + txtShiftNurce800No2.ClientID + "';");
            sbScript1.Append("var txtShiftNurce800No3ClientID = '" + txtShiftNurce800No3.ClientID + "';");
            sbScript1.Append("var lblTabNameClientID = '" + lblTabName.ClientID + "';");
            sbScript1.Append("var rbRequireVoiceClipsClientID = '" + rblRequireVoiceClips.ClientID + "';");
            sbScript1.Append("var rbConnectEDClientID = '" + rblConnectED.ClientID + "';");
            sbScript1.Append("var rbRequireExamDescriptionClientID = '" + rblRequireExamDescription.ClientID + "';");
            sbScript1.Append("var rbMessageRetrieveUsingPINClientID = '" + rblMessageRetrieveUsingPIN.ClientID + "';");
            sbScript1.Append("var txtPrimaryEmailClientID = '" + txtPrimaryEmail.ClientID + "';");
            sbScript1.Append("var txtContact1emailClientID = '" + txtContact1email.ClientID + "';");
            sbScript1.Append("var txtContact2emailClientID = '" + txtContact2Email.ClientID + "';");

            sbScript1.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript1.ToString());
            btnEditInstitution.Attributes.Add("onclick", "return checkRequired();");


        }
        #endregion

    }
}