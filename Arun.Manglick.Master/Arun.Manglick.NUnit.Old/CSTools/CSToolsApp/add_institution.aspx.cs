#region File History

/******************************File History***************************
 * File Name        : add_institute.aspx.cs
 * Author           : Prerak Shah.
 * Created Date     : 04-07-2007
 * Purpose          : This Class will add new Institute.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification
 * ------------------------------------------------------------------- 
 * 03-12-2007P  IAK     Defect 2403 fixed.
 * 14-12-2007   IAK     Added BatchMessages Radio button
 * 03-20-2008   SSK     Added radiobutton for "Allow PIN for Message Retrieval"
 * 05-07-2008   Suhas   Defect 2979: Auto Tab issue.
 * 30-05-2008   Suhas   Added Enable Call Center Flag
 * 12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
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
    public partial class add_institution : System.Web.UI.Page
    {
        #region Page Variables
        private string userID = string.Empty;
        #endregion Page Variables

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Session[SessionConstants.USER_ID] == null)
                Response.Redirect(Utils.GetReturnURL("default.aspx", "add_institution.aspx", this.Page.ClientQueryString));

                registerJavascriptVariables();
                this.Page.SmartNavigation = true;
                if (!Page.IsPostBack)
                {
                    fillTimeZones();
                    //Page.RegisterStartupScript("Hidecontrol", "<script language=" + '"' + "Javascript" + '"' + ">'enabledControl()';</script>");
                }
                if (rblRequireVoiceClips.SelectedValue == "1")
                    pnlTabName.Visible = true;     
                Session[SessionConstants.CURRENT_TAB] = "SystemAdmin";
                Session[SessionConstants.CURRENT_INNER_TAB] = "AddInstitution";
                Session[SessionConstants.CURRENT_PAGE] = "add_institution.aspx";

                this.Form.DefaultButton = this.btnAddInstitution.UniqueID;


            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_institution - Page_Load", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }


        }
        protected void btnAddInstitution_Click(object sender, EventArgs e)
        {
            if (Session[SessionConstants.USER_ID] != null)
            {
                int institutionID = AddInstitutionData();
                hdnTextChanged.Value="false";
                //ViewState["InstitutionID"] = institutionID;
                Session[SessionConstants.INSTITUTION_ID] = institutionID;
                //ScriptManager.RegisterStartupScript(upnlAddInstitution,upnlAddInstitution.GetType(), "AddInstitution", "alert('Institution added successfully.');Navigate('Add');");
                ScriptManager.RegisterClientScriptBlock(upnlAddInstitution,upnlAddInstitution.GetType(),"NavigateToDir", "<script type=\'text/javascript\'>alert('Institution added successfully.');Navigate('"  + institutionID + "');</script>",false);
                
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ViewState["InstitutionID"] = null;
            ScriptManager.RegisterClientScriptBlock(upnlAddInstitution,upnlAddInstitution.GetType(),"NavigateToDir", "<script type=\'text/javascript\'>Navigate();</script>",false);            
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
            DataTable dtTimeZone ;//= new DataTable();
            dtTimeZone  = objInstitution.GetTimeZone();
            drpTimeZone.DataSource = dtTimeZone;
            //drpTimeZone.DataValueField = dtTimeZone.c
            //drpTimeZone.DataTextField = arrTimeZone[1].ToString();
 
            drpTimeZone.DataBind();

            ListItem li = new ListItem("-- Select TimeZone --", "-1");
            drpTimeZone.Items.Add(li);
            drpTimeZone.Items.FindByValue("-1").Selected = true;
            objInstitution = null;
        }
        /// <summary>
        /// Add new Institution record in database
        /// </summary>
        private int AddInstitutionData()
        {
            Institution objInstitution = new Institution();
            InstitutionInformation objInstitutionInfo = new InstitutionInformation();
            int institutionID;
            try
            {
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
                objInstitutionInfo.TabName = rblConnectED.SelectedValue == "1" ? true : false;
                objInstitutionInfo.BatchMessage = rblBatchMessages.SelectedValue == "1" ? true : false;
                objInstitutionInfo.IsRequireExamDescription = rblRequireExamDescription.SelectedValue == "1" ? true : false;
                objInstitutionInfo.MessageRetrieveUsingPIN = rblMessageRetrieveUsingPIN.SelectedValue == "1" ? true : false;
                objInstitutionInfo.EnableCallCenter = rblEnableCallCenter.SelectedValue == "1" ? true : false;
                objInstitutionInfo.EnablePromptForPin = rblPinForCT.SelectedValue == "1" ? true : false;
                institutionID = objInstitution.AddInstitution(objInstitutionInfo);

                return institutionID;
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_institution - AddInstitutionData", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
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
        /// Register JS variables, client side button click events
        /// </summary>
        private void registerJavascriptVariables()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("var hdnTextChangedClientID = '" + hdnTextChanged.ClientID + "';");
            ClientScript.RegisterStartupScript(Page.GetType(), "scriptDeviceClientIDs", sbScript.ToString(), true);

            //txtInstiturionID.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtInstitutionName.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtInstitutionName.Attributes.Add("onkeypress", "Javascript:isAlphaNumericKeyStroke();");
            txtInstitutionName.Attributes.Add("onpaste", "Javascript:isAlphaNumericKeyStroke();");            
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
            rblConnectED.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblBatchMessages.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblRequireExamDescription.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblMessageRetrieveUsingPIN.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            rblEnableCallCenter.Attributes.Add("onclick", "JavaScript:return TextChanged();");

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

            txtShiftNurce800No1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtShiftNurce800No2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtShiftNurce800No3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");

            txtLab800No1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtLab800No2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtLab800No3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");

            txtstate.Attributes.Add("onKeypress", "JavaScript:isAlphabetKetStroke();");

            // add Javascript client-side code to move through phone# fields...
            txtMainNumber1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtMainNumber2.ClientID + "').focus()";
            txtMainNumber2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtMainNumber3.ClientID + "').focus()";
            //txtMainNumber3.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + txtPrimaryName.ClientID + "').focus()";

            txtPrimaryPhone1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryPhone2.ClientID + "').focus()";
            txtPrimaryPhone2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPrimaryPhone3.ClientID + "').focus()";
            //txtPrimaryPhone3.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + txtPrimaryEmail.ClientID + "').focus()";

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
            sbScript1.Append("var rblEnableCallCenterClientID = '" + rblEnableCallCenter.ClientID + "';"); 
            sbScript1.Append("var txtPrimaryEmailClientID = '" + txtPrimaryEmail.ClientID + "';");
            sbScript1.Append("var txtContact1emailClientID = '" + txtContact1email.ClientID + "';");
            sbScript1.Append("var txtContact2emailClientID = '" + txtContact2Email.ClientID + "';");
            sbScript1.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript1.ToString());
            btnAddInstitution.Attributes.Add("onclick", "return checkRequired();");
              
            //btnRShift.Attributes.Add("onclick", "JavaScript:return UnitsAssignedOrRemoved(true," + lstAvailableUnits.ClientID + ");");
            //btnLShift.Attributes.Add("onclick", "JavaScript:return UnitsAssignedOrRemoved(false," + lstAssignedUnits.ClientID + ");");
        }

        #endregion




    }
}