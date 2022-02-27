#region File History

/******************************File History***************************
 * File Name        : add_group.aspx.cs
 * Author           : Prerak Shah.
 * Created Date     : 16-07-2007
 * Purpose          : This Class will add new Group.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * ------------------------------------------------------------------- 
 * 03-12-2007P  IAK     Defect 2403 fixed.
 * 18-12-2007   Prerak  iteration 17 Code review fixes review details 1-4 fixed. 
 * 05-05-2008   Suhas    Defect # 2987 - Group Name Validation.
 * 05-07-2008   Suhas    Defect 2979: Auto Tab issue.
 * 12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 * 18 Nov 2008  Raju G Message changed for duplicate validation
 * 26 Nov 2008  Prerak   Defect #4248 Fixed
 * 26 Nov 2008  Raju G   Remove Prerak Changes for #4248
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
    public partial class add_group : System.Web.UI.Page
    {
        #region variables
        private bool isSystemAdmin = true;
        private int instID;
        private const string INSTITUTE_ID = "institutionID";
        #endregion
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
            {
                Response.Redirect(Utils.GetReturnURL("default.aspx","add_group.aspx",this.Page.ClientQueryString)); 
            }
            UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
            if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
            {
                isSystemAdmin = false;
                instID = userInfo.InstitutionID;
            }
            else
                isSystemAdmin = true;

            registerJavascriptVariables();
            try
            {
                if (!IsPostBack)
                {
                    if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
                    {
                        isSystemAdmin = false;
                        hdnIsSystemAdmin.Value = "0";
                        cmbInstitution.Visible = false;
                        lblInstName.Visible = true;
                        lblInstName.Text = userInfo.InstitutionName;
                        //populateDirectories();
                    }
                    else
                    {
                        isSystemAdmin = true;
                        hdnIsSystemAdmin.Value = "1";
                        cmbInstitution.Visible = true; ;
                        lblInstName.Visible = false;
                        fillInstitution();
                        if (Request[INSTITUTE_ID] != null)
                            cmbInstitution.SelectedValue = (Request[INSTITUTE_ID].ToString());
                        instID = Convert.ToInt32(cmbInstitution.SelectedValue);
                        if (cmbInstitution.SelectedValue == "-1")
                        {
                            btnAddGroup.Enabled = false;
                            EnabledControls(false);
                        }
                        else
                        {
                            btnAddGroup.Enabled = true;
                            EnabledControls(true);
                        }
                    }

                    fillPracticeType();
                    populateDirectories(instID);
                    fillTimeZone();
                }
                if (Request[INSTITUTE_ID] != null)
                {
                    ViewState[INSTITUTE_ID] = Request[INSTITUTE_ID];
                    instID = Convert.ToInt32(Request[INSTITUTE_ID]);
                }
                else
                    ViewState[INSTITUTE_ID] = -1;
                Session[SessionConstants.CURRENT_TAB] = "Tools";
                Session[SessionConstants.CURRENT_INNER_TAB] = "AddGroup";
                Session[SessionConstants.CURRENT_PAGE] = "add_group.aspx";
                this.Form.DefaultButton = this.btnAddGroup.UniqueID;
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_group - Page_Load", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            } 
           
        }

        protected void cmbInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            instID = Convert.ToInt32(cmbInstitution.SelectedValue);
            hdnInstitutionVal.Value = cmbInstitution.SelectedValue;
            populateDirectories(instID);
            clearControls();
            if (cmbInstitution.SelectedValue == "-1")
            {
                btnAddGroup.Enabled = false;
                EnabledControls(false);
            }
            else
            {
                btnAddGroup.Enabled = true;
                EnabledControls(true);
            }
            cmbInstitution.Focus();
        }
        protected void btnAddGroup_Click(object sender, EventArgs e)
        {
            if (Session[SessionConstants.USER_ID] != null)
            {   
                string script="<script type=\'text/javascript\'>";
                int groupID = addGroupData();
                if (groupID == -3)
                {
                    ScriptManager.RegisterStartupScript(upnlAddGroup, upnlAddGroup.GetType(), "AddGroup", "alert('Institution name is not selected.');", true);
                }
                else if (groupID == -4)
                {
                    ScriptManager.RegisterStartupScript(upnlAddGroup, upnlAddGroup.GetType(), "AddGroup", "alert('Group Name is already assigned');", true);
                }
                else if (groupID == -1)
                {
                    ScriptManager.RegisterStartupScript(upnlAddGroup, upnlAddGroup.GetType(), "AddGroup", "alert('Group 800 number is already assigned');", true);
                }
                else if (groupID == -2)
                {
                    ScriptManager.RegisterStartupScript(upnlAddGroup, upnlAddGroup.GetType(), "AddGroup", "alert('Ordering Clinician 800 Number is already assigned');", true);
                }
                else
                {
                    ViewState["GroupID"] = groupID;            
                    if (isSystemAdmin)
                     ViewState[INSTITUTE_ID] = cmbInstitution.SelectedValue.ToString();
                     script =  script + "alert('Group added successfully.');";
                     hdnTextChanged.Value = "false";
                    if (chkDefFinding.Checked)
                    {
                        int groupType = rbGroupType.SelectedValue == "1" ? 1 : 0;
                        addDefaultGroupFindings(groupID,groupType);
                    }
                    if (chkOtherFindings.Checked)
                    {
                        script = script + "Navigate('" + ViewState[INSTITUTE_ID].ToString()  + "','AddFindings','" + groupID.ToString() + "');</script>";
                        ScriptManager.RegisterClientScriptBlock(upnlAddGroup, upnlAddGroup.Page.GetType(), "NavigateToFindings", script,false);

                    }
                    else
                    {
                        script = script + "Navigate('" + ViewState[INSTITUTE_ID].ToString() + "','GroupPreferences','" + groupID.ToString() + "');</script>";
                        ScriptManager.RegisterClientScriptBlock(upnlAddGroup, upnlAddGroup.Page.GetType(), "NavigateToGrpPreferences", script,false);

                    }

                }
             }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ViewState["GroupID"] = null;
            ScriptManager.RegisterClientScriptBlock(upnlAddGroup,upnlAddGroup.GetType(),"NavigateToDir", "<script type=\'text/javascript\'>Navigate(" + ViewState[INSTITUTE_ID] + ",'GroupMonitior','0');</script>",false);

        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Register JS variables, client side button click events
        /// </summary>
        private void registerJavascriptVariables()
        {
            string sbScript = "";
            sbScript += "var hdnTextChangedClientID = '" + hdnTextChanged.ClientID + "';";
            ClientScript.RegisterStartupScript(Page.GetType(), "scriptDeviceClientIDs", sbScript, true);

            txtPhone1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtPhone2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtPhone3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtRP800No1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtRP800No2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtRP800No3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtGroup800No1.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtGroup800No2.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtGroup800No3.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtState.Attributes.Add("onKeypress", "JavaScript:isAlphabetKetStroke();");
            txtGroup800No1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtGroup800No2.ClientID + "').focus()";
            txtGroup800No2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtGroup800No3.ClientID + "').focus()";
            //txtGroup800No3.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + txtRP800No1.ClientID + "').focus()";

            txtRP800No1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtRP800No2.ClientID + "').focus()";
            txtRP800No2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtRP800No3.ClientID + "').focus()";
            //txtRP800No3.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + txtAffiliation.ClientID + "').focus()";

            txtPhone1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPhone2.ClientID + "').focus()";
            txtPhone2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPhone3.ClientID + "').focus()";
            //txtPhone3.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + cmbTimeZone.ClientID + "').focus()";

            txtAdd1.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtAdd2.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtAffiliation.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtCity.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtState.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtZip.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtGroupName.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            cmbDirName.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            cmbPractieType.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            cmbTimeZone.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            rbGroupType.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            
             

            StringBuilder sbScript1 = new StringBuilder();
            sbScript1.Append("<script language=JavaScript>");
            sbScript1.Append("var txtGroupNameClientID = '" + txtGroupName.ClientID + "';");
            sbScript1.Append("var cmbDirNameClientID = '" + cmbDirName.ClientID + "';");
            sbScript1.Append("var txtGroup800No1ClientID = '" + txtGroup800No1.ClientID + "';");
            sbScript1.Append("var txtGroup800No2ClientID = '" + txtGroup800No2.ClientID + "';");
            sbScript1.Append("var txtGroup800No3ClientID = '" + txtGroup800No3.ClientID + "';");
            sbScript1.Append("var txtRP800No1ClientID = '" + txtRP800No1.ClientID + "';");
            sbScript1.Append("var txtRP800No2ClientID = '" + txtRP800No2.ClientID + "';");
            sbScript1.Append("var txtRP800No3ClientID = '" + txtRP800No3.ClientID + "';");
            sbScript1.Append("var cmbTimeZoneClientID = '" + cmbTimeZone.ClientID + "';");
            sbScript1.Append("var txtPhone1ClientID = '" + txtPhone1.ClientID + "';");
            sbScript1.Append("var txtPhone2ClientID = '" + txtPhone2.ClientID + "';");
            sbScript1.Append("var txtPhone3ClientID = '" + txtPhone3.ClientID + "';");
            sbScript1.Append("var cmbInstitutionClientID = '" + cmbInstitution.ClientID + "';");
            sbScript1.Append("var hdnInstitutionValClientID = '" + hdnInstitutionVal.ClientID + "';");
            sbScript1.Append("var cmbPractieTypeClientID = '" + cmbPractieType.ClientID + "';");
            sbScript1.Append("var chkOtherFindingsClientID = '" + chkOtherFindings.ClientID + "';");
            sbScript1.Append("var chkDefFindingClientID = '" + chkDefFinding.ClientID + "';");
            sbScript1.Append("var lblDefaultClientID = '" + lblDefault.ClientID + "';");
            sbScript1.Append("var rbGroupTypeClientID = '" + rbGroupType.ClientID + "';");

           
            sbScript1.Append("</script>");
            RegisterStartupScript("scriptClientIDs", sbScript1.ToString());
            btnAddGroup.Attributes.Add("onclick", "return groupCheckRequired('AddGroup');");

            rbGroupType.Attributes.Add("onclick", "JavaScript:return changeTextforDefaultFinding();");

            cmbInstitution.Attributes.Add("onchange", "Javascript:return onComboChange();");
            
        }
        /// <summary>
        /// This method fills the institution combo box
        /// </summary>
        private void fillInstitution()
        {
            DataTable dtInstitution = new DataTable();
            dtInstitution = Utility.GetInstitutionList();
            cmbInstitution.DataSource = dtInstitution;
            cmbInstitution.DataBind();
            
            ListItem li = new ListItem("--Select Institution --", "-1");
            cmbInstitution.Items.Add(li);
            cmbInstitution.Items.FindByValue("-1").Selected = true;
        }
        /// <summary>
        /// This method poputale the directories in directory combo according to institution id
        /// </summary>
        /// <param name="institutionID"></param>
        private void populateDirectories(int institutionID)
        {
            DataTable dtDirectories = new DataTable();
            dtDirectories = Utility.GetDirectories(institutionID);
            cmbDirName.DataSource = dtDirectories;
            cmbDirName.DataBind();

            ListItem li = new ListItem("-- Select Directory --", "-1");
            cmbDirName.Items.Add(li);
            cmbDirName.Items.FindByValue("-1").Selected = true;
        }
        /// <summary>
        /// This Method fill the Practice Type combo box
        /// </summary>
        private void fillPracticeType()
        {
            DataTable dtPracticeType = new DataTable();
            dtPracticeType =  Utility.GetPracticeType();
            cmbPractieType.DataSource = dtPracticeType;
            cmbPractieType.DataBind();

            ListItem li = new ListItem("-- Select Practice Type --", "-1");
            cmbPractieType.Items.Add(li);
            cmbPractieType.Items.FindByValue("-1").Selected = true;
        }
        /// <summary>
        /// This method fill the TimeZone Combo box
        /// </summary>
        private void fillTimeZone()
        {
            DataTable dtTimeZone = new DataTable();
            dtTimeZone = Utility.GetTimeZone();
            cmbTimeZone.DataSource = dtTimeZone;
            cmbTimeZone.DataBind();

            ListItem li = new ListItem("-- Select TimeZone --", "-1");
            cmbTimeZone.Items.Add(li);
            cmbTimeZone.Items.FindByValue("-1").Selected = true;
        }

        /// <summary>
        /// Add new Institution record in database
        /// </summary>
        private int addGroupData()
        {
            Group objGroup ;
            GroupInformation  objGroupInfo ;
            int groupID = 0;
            int duplicate800Num = 0;
            int duplicateName = 0;
            string grpName;
            try
            {
                objGroup = new Group();
                objGroupInfo = new GroupInformation();
                if (isSystemAdmin)
                {
                    if (cmbInstitution.SelectedItem.Value == "-1")
                    {
                        groupID = -3;
                        return groupID;
                    }
                }
                grpName = txtGroupName.Text;
                int institutionID = Convert.ToInt32(cmbInstitution.SelectedItem.Value);
                duplicateName = objGroup.CheckGroupName(grpName, institutionID);
                if (duplicateName == 1)
                {
                    groupID = -4;
                    return groupID;
                }
                string rp800 = txtRP800No1.Text + txtRP800No2.Text + txtRP800No3.Text;
                string gp800 = txtGroup800No1.Text + txtGroup800No2.Text + txtGroup800No3.Text;
                duplicate800Num = objGroup.Check800Numbers(gp800, rp800);
                switch (duplicate800Num)
                {
                    case 1:
                        groupID =  -1;
                        break;
                    case 2:
                        groupID = -2;  
                        break;
                    case 0:
                        {
                            objGroupInfo.GroupName = txtGroupName.Text.Trim();
                            objGroupInfo.Address1 = txtAdd1.Text.Trim();
                            objGroupInfo.Address2 = txtAdd2.Text.Trim();
                            objGroupInfo.City = txtCity.Text.Trim();
                            objGroupInfo.State = txtState.Text.Trim();
                            objGroupInfo.Zip = txtZip.Text.Trim();
                            objGroupInfo.Phone = txtPhone1.Text + txtPhone2.Text + txtPhone3.Text;
                            objGroupInfo.Affiliation = txtAffiliation.Text.Trim();
                            if (isSystemAdmin)
                                instID = Convert.ToInt32(cmbInstitution.SelectedValue);
                            objGroupInfo.InstitutionID = instID;  //Convert.ToInt32(cmbInstitution.SelectedItem.Value);
                            objGroupInfo.PracticeType = Convert.ToInt32(cmbPractieType.SelectedItem.Value);
                            objGroupInfo.DirectoryID = Convert.ToInt32(cmbDirName.SelectedItem.Value);
                            objGroupInfo.Group800Number = txtGroup800No1.Text + txtGroup800No2.Text + txtGroup800No3.Text;
                            objGroupInfo.GroupDID = txtGroup800No1.Text + txtGroup800No2.Text + txtGroup800No3.Text; //GroupDID is same as group 800 number 
                            objGroupInfo.ReferringPhysician800Number = txtRP800No1.Text + txtRP800No2.Text + txtRP800No3.Text;
                            objGroupInfo.ReferringPhysicianDID = txtRP800No1.Text + txtRP800No2.Text + txtRP800No3.Text; //RPDID is same as RP800 Number
                            objGroupInfo.TimeZoneID = Convert.ToInt32(cmbTimeZone.SelectedItem.Value.ToString());
                            objGroupInfo.GroupType = rbGroupType.SelectedValue == "1" ? true : false;
                            //objGroupInfo.GroupGraphicLocation  = txtContact2Title.Text.Trim();
                            //objGroupInfo.GroupVoiceURL  = txtContact2Phone1.Text + txtContact2Phone2.Text + txtContact2Phone3.Text;

                            groupID = objGroup.AddGroup(objGroupInfo);
                            Session[SessionConstants.ALL_GROUPS] = null;

                            break;
                        }
                 }
                return groupID;
            }
            catch (Exception objException)
            {
                if (Session["UserID"] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_group - AddGroupData", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                objGroup  = null;
                objGroupInfo = null;
            }

        }
        /// <summary>
        /// This Method will insert default findings for group according to group type. 
        /// for example for Radiology group it will insert Red, Orange and Yellow findings and 
        /// for Lab group it will insert Red, Orange, Yellow, Positive, Negative, and other findings.
        /// </summary>
        /// <param name="groupID">groupID</param>
        /// <param name="groupType">Group Type</param>
        private void addDefaultGroupFindings(int groupID, int groupType)
        {
           string voiceOverIP = ConfigurationSettings.AppSettings["voiceOverURLIP"];
           //int groupType =  
           Group objGroup ;
           try
           {
              objGroup  = new Group();
              objGroup.AddDefaultGroupFindings(voiceOverIP, groupID, groupType);
           }
            catch (Exception objException)
           {
                if (Session["UserID"] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_group - addDefaultGroupFindings", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                objGroup  = null;
            }
        }
        /// <summary>
        /// This method clear control on UI.
        /// </summary>
        private void clearControls()
        {
            txtAdd1.Text = "";
            txtAdd2.Text = "";
            txtAffiliation.Text = "";
            txtCity.Text = "";
            txtGroup800No1.Text = "";
            txtGroup800No2.Text = "";
            txtGroup800No3.Text = "";
            txtGroupName.Text = "";
            txtPhone1.Text = "";
            txtPhone2.Text = "";
            txtPhone3.Text = "";
            txtRP800No1.Text = "";
            txtRP800No2.Text = "";
            txtRP800No3.Text = "";
            txtState.Text = "";
            txtZip.Text = "";
            chkDefFinding.Checked = false;
            chkOtherFindings.Checked = false;
            cmbTimeZone.SelectedValue = "-1";
            cmbPractieType.SelectedValue = "-1";
 
        }
        /// <summary>
        /// This method Enabled and disabled control as per flag parameter.
        /// if flg = true then control is enabled other wise it is disabled.
        /// </summary>
        /// <param name="flg"></param>
        private void EnabledControls(bool flg)
        {
            txtAdd1.Enabled = flg ;
            txtAdd2.Enabled = flg;
            txtAffiliation.Enabled = flg;
            txtCity.Enabled = flg;
            txtGroup800No1.Enabled = flg;
            txtGroup800No2.Enabled = flg;
            txtGroup800No3.Enabled = flg;
            txtGroupName.Enabled = flg;
            txtPhone1.Enabled = flg;
            txtPhone2.Enabled = flg;
            txtPhone3.Enabled = flg;
            txtRP800No1.Enabled = flg;
            txtRP800No2.Enabled = flg;
            txtRP800No3.Enabled = flg;
            txtState.Enabled = flg;
            txtZip.Enabled = flg;
            chkDefFinding.Enabled = flg;
            chkOtherFindings.Enabled = flg;
            cmbTimeZone.Enabled = flg;
            cmbPractieType.Enabled = flg;
            rbGroupType.Enabled = flg;
            cmbDirName.Enabled = flg;

        }
        #endregion
    }
}
