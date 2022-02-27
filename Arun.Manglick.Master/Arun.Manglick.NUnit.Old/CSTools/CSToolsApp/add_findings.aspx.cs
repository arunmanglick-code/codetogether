
#region File History

/******************************File History***************************
 * File Name        : add_findings.aspx.cs
 * Author           : Prerak Shah
 * Created Date     : 17-Aug-2007
 * Purpose          : This Class will Add new Findings for Group.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 *  23-11-2007 ZK     - Added DataAccess and common libraries.
 *  23-11-2007 Prerak - Updated for CSTools Part II
 *  26-11-2007 Prerak - Updated for CSTools Part II
 *  29-11-2007 Prerak - Coding
 *  30-10-2007 Prerak - btsave, btncancel cancel click logic change, JS added for these buttons
 *  03-12-2007 Prerak - Active checkbox added link group maintainance is added.
 *  04-12-2007 Prerak - Removed cmbGroup_SelectedIndexChanged event
 *  05-12-2007 Prerak - Modified method addFindingData, active flag default true removed
 *  10-12-2007 Prerak - Update FillGroups methods, added DocumentedOnly check box
 *                      Priority Textbox. 
 *  12-12-2007   IAK  - Added active column
 *  13-12-2007   IAK  - Added chkActive datachange javascript
 *  17-12-2007 Prerak - Change checking duplicate finding order, description and priority     
 *  18-12-2007 Prerak - Defect# 2443  Fixed.
 *  18-12-2007 Prerak - iteration 17 Code review fixes review details 26-35 fixed. 
 *  19-12-2007 IAK    - Code review fixes
 *  17-01-2008 Prerak - Javascript combo box error solved
 *  12 Jun 2008  Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 *  20 Jun 2008  IAK    - Defect 3309
 *  19 Sep 2008  IAK    - CR 265- Default notification for finding
 *  14 Oct 2008 Prerak  - Live Agent "SendToAgent" flag for powerscribe users.  
 *  12 Nov 2008 SNK     - System should not allow user to add special characters in finding description.
 *  12-12-2008 RG       Fixed #3177
 *  22-12-2008  GB      - Added Default field and changed the layout as per TTP #244 and #231. 
 * * ------------------------------------------------------------------- 
 
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
using Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.Utilities;
using System.IO;
using System.Text;
namespace Vocada.CSTools
{
    public partial class add_findings : System.Web.UI.Page
    {
        #region variables
        private bool isSystemAdmin = true;
        private const string INSTITUTE_ID = "institutionID";
        private const string GROUP_ID = "GroupID";
        private const string IS_ADD = "Add";
        private int instID;
        private int groupID;

        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
                Response.Redirect(Utils.GetReturnURL("default.aspx", "add_findings.aspx", this.Page.ClientQueryString));
            UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
            if (ViewState[INSTITUTE_ID] == null)
                ViewState[INSTITUTE_ID] = "-1";
            if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
            {
                isSystemAdmin = false;
                instID = userInfo.InstitutionID;
                ViewState[INSTITUTE_ID] = instID;
            }
            else
                isSystemAdmin = true;

            //this.btnSave.Attributes.Add("onclick", "javascript:if (FindingCheckRequired()){__doPostBack('" + lnkSubmitUpload.ClientID + "','');}else {return false;}");
            registerJavascriptVariables();
            try
            {
                if (!IsPostBack)
                {
                    fillDevices();
                    fillNotificationEvents();
                    checkMode();
                    if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
                    {
                        isSystemAdmin = false;
                        hdnIsSystemAdmin.Value = "0";
                        cmbInstitution.Visible = false;
                        lblInstName.Visible = true;
                        lblInstName.Text = userInfo.InstitutionName;
                        enabledConrol(true);
                    }
                    else
                    {
                        isSystemAdmin = true;
                        hdnIsSystemAdmin.Value = "1";
                        cmbInstitution.Visible = true; ;
                        lblInstName.Visible = false;
                        fillInstitution();
                        if (Request[INSTITUTE_ID] != null)
                        {
                            cmbInstitution.SelectedValue = (Request[INSTITUTE_ID].ToString());
                            ViewState[INSTITUTE_ID] = Request[INSTITUTE_ID];
                        }
                        instID = Convert.ToInt32(cmbInstitution.SelectedValue);
                        hdnInstitutionVal.Value = cmbInstitution.SelectedValue;
                        if (cmbInstitution.SelectedValue == "-1")
                        {
                            btnSave.Enabled = false;
                            enabledConrol(false);
                        }
                        else
                        {
                            btnSave.Enabled = true;
                            enabledConrol(true);
                        }
                    }
                    fillGroups();
                    if (Request[GROUP_ID] != null)
                    {
                        ViewState[GROUP_ID] = Request[GROUP_ID];
                        cmbGroup.SelectedValue = Request[GROUP_ID].ToString();
                    }
                    if (!Convert.ToBoolean(ViewState[IS_ADD]))
                    {
                        fillFinding();
                        //if (isSystemAdmin)
                        //    cmbInstitution.Enabled = false;
                        //cmbGroup.Enabled = false;
                    }
                    

                }

                hlinkGroupMonitor.NavigateUrl = "./group_monitor.aspx?InstitutionID=" + ViewState[INSTITUTE_ID]; ;

                ClientScript.RegisterStartupScript(this.GetType(), "Findings", "enableEmbargo('false');", true);

                Session[SessionConstants.CURRENT_TAB] = "Tools";
                Session[SessionConstants.CURRENT_INNER_TAB] = "AddFindings";
                Session[SessionConstants.CURRENT_PAGE] = "add_findings.aspx";

                //this.Form.DefaultButton = this.btnSave.UniqueID;

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("add_findings.Page_Load:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            bool isNavigatingToFindingNotificationPage = false;
            string instituteID = "-1";

            if (ViewState[INSTITUTE_ID] != null)
                instituteID = ViewState[INSTITUTE_ID].ToString();

            if ((Request["Mode"] != null && Request["Mode"].ToString() == "Edit") || (Request["institutionID"] != null && Request["GroupID"] != null))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanelAddFinding, UpdatePanelAddFinding.GetType(), "NavigateToDir", "<script type=\'text/javascript\'>NavigateFinding('Notifications','" + ViewState[GROUP_ID] + "');</script>", false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanelAddFinding, UpdatePanelAddFinding.GetType(), "NavigateToDir", "<script type=\'text/javascript\'>NavigateFinding('groupMonitor','" + instituteID + "');</script>", false);
            }
        }
        /// <summary>
        /// Event occure when selection of institution is changed. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            instID = Convert.ToInt32(cmbInstitution.SelectedValue);
            hdnInstitutionVal.Value = cmbInstitution.SelectedValue;
            fillGroups();
            clearConrol();
            if (cmbInstitution.SelectedValue == "-1")
            {
                btnSave.Enabled = false;
                enabledConrol(false);
            }
            else
            {
                btnSave.Enabled = true;
                enabledConrol(true);
            }
            ClientScript.RegisterStartupScript(this.GetType(), "Findings", "enableEmbargo('false');", true);
            cmbInstitution.Focus();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (Page.IsValid)
                addFindingData();            
            string script = "";
            if (Request["Mode"] != null && Request["Mode"].ToString() == "Edit")
            {
                script += "hideControl('" + isSystemAdmin + "');";
                script += "alert('Finding updated successfully.');NavigateFinding('Notifications','" + ViewState[GROUP_ID] + "');";
                ScriptManager.RegisterStartupScript(UpdatePanelAddFinding, UpdatePanelAddFinding.GetType(), "EditFindings", script, true);
            }
            else
            {
                script += "document.getElementById('" + hdnTextChanged.ClientID + "').value='false';";
                script += "alert('Finding added successfully.');";
                script += "clearUIControls();";
                ScriptManager.RegisterStartupScript(UpdatePanelAddFinding, UpdatePanelAddFinding.GetType(), "AddFindings", script, true);
                //clearConrol();
            }           
        }
        protected void lnkSubmitUpload_Click(object sender, EventArgs e)
        {


            //    if (Page.IsValid) 
            //        addFindingData();

            //    string script = "<script type=\'text/javascript\'>";
            //    //script += "document.getElementById('" + hdnTextChanged.ClientID + "').value='false';";
            //    if (Convert.ToBoolean(ViewState[IS_ADD]))
            //    {
            //        script += "alert('Finding added successfully.');";
            //        //script += "NavigateFinding('msg_centers');";
            //        script += "</script>";
            //        clearConrol();
            //        if (Page.IsPostBack)
            //            ClientScript.RegisterClientScriptBlock(this.GetType(), "AddFindings", script, false);
            //        else
            //            ClientScript.RegisterStartupScript(this.GetType(), "AddFindings", script, true);
            //        return;
            //    }
            //    else
            //    {
            //        script += "hideControl('" + isSystemAdmin + "');";
            //        script += "alert('Finding updated successfully.');NavigateFinding('Notifications','" + ViewState[GROUP_ID] + "');";
            //        script += "</script>";
            //        ClientScript.RegisterStartupScript(this.GetType(), "EditFindings", script, false );
            //        return;
            //    }
        }
        #endregion Events

        #region Private Methods
        /// <summary>
        /// This function fill the group combo.
        /// </summary>
        private void fillGroups()
        {
            DataTable dtGroups;
            Finding objFinding;
            StringBuilder strHdnvalue = new StringBuilder();

            try
            {

                if (isSystemAdmin)
                    instID = Convert.ToInt32(cmbInstitution.SelectedValue);
                else
                    instID = instID;

                string mode = "Add";
                int findingid = -1;
                objFinding = new Finding();
                if (!Convert.ToBoolean(ViewState[IS_ADD]))
                {
                    mode = "Edit";
                    findingid = Convert.ToInt32(Request["FindingID"]);
                }
                dtGroups = objFinding.GetGroups(instID, findingid, mode);
                cmbGroup.DataSource = dtGroups;
                cmbGroup.DataBind();

                if (dtGroups.Rows.Count > 0)
                {
                    for (int i = 0; i < dtGroups.Rows.Count; i++)
                    {
                        strHdnvalue.Append("#");
                        strHdnvalue.Append(dtGroups.Rows[i][0].ToString());
                        strHdnvalue.Append("#");
                        strHdnvalue.Append(dtGroups.Rows[i][2].ToString());
                        strHdnvalue.Append(dtGroups.Rows[i][3].ToString());
                        strHdnvalue.Append(dtGroups.Rows[i][4].ToString());
                        strHdnvalue.Append("#");
                        strHdnvalue.Append(dtGroups.Rows[i][0].ToString());
                        strHdnvalue.Append("#");
                    }

                    txtGroupCheck.Text = strHdnvalue.ToString();
                    //if (!IsPostBack)
                    //    //ClientScript.RegisterStartupScript(Page.GetType(), "resetHdnValue", "document.getElementById('" + txtGroupCheck.ClientID + "').value='" + strHdnvalue.ToString() + "';", true);
                    //    //ClientScript.RegisterStartupScript(Page.GetType(), "resetHdnValue", "document.getElementById('" + txtGroupCheck.ClientID + "').value='" + strHdnvalue.Length + "';", true);
                    //    Utils.RegisterJS("sas", "document.getElementById('" + txtGroupCheck.ClientID + "').value='" + strHdnvalue.Length + "';", Page);
                    //else
                    //    Utils.RegisterJS("sas", "document.getElementById('" + txtGroupCheck.ClientID + "').value='" + strHdnvalue.Length + "';", Page);
                    //    //ClientScript.RegisterStartupScript(Page.GetType(), "resetHdnValue", "document.getElementById('" + txtGroupCheck.ClientID + "').value='" + strHdnvalue.Length + "';", true);

                    //    //ClientScript.RegisterClientScriptBlock(Page.GetType(), "resetHdnValue1", "document.getElementById('" + txtGroupCheck.ClientID + "').value='" + strHdnvalue.Length + "';", true);
                    //    //ClientScript.RegisterClientScriptBlock(Page.GetType(), "resetHdnValue1", "document.getElementById('" + txtGroupCheck.ClientID + "').value='" + strHdnvalue.ToString() + "';", true);
                    ////txtGroupCheck.Value = strHdnvalue.ToString();
                }

                ListItem li = new ListItem("--Select Group --", "-1");
                cmbGroup.Items.Add(li);
                cmbGroup.Items.FindByValue("-1").Selected = true;
            }
            finally
            {
                dtGroups = null;
                objFinding = null;
                strHdnvalue = null;
            }
        }
        /// <summary>
        /// This Function add-edit findings for a group
        /// </summary>
        private void addFindingData()
        {
            Finding objFinding = null;
            FindingInformation objFindingInfo = null;
            string url = "";
            int findingOrder;
            try
            {

                objFindingInfo = new FindingInformation();

                objFinding = new Finding();

                objFindingInfo.GroupID = Convert.ToInt32(cmbGroup.SelectedValue);
                objFindingInfo.FindingDescription = txtDescription.Text.Trim();
                url = upload(Convert.ToInt32(cmbGroup.SelectedValue));
                if (url != "")
                    objFindingInfo.FindingVoiceOverURL = url;
                if (txtComplianceGoal.Text.Length > 0)
                    objFindingInfo.ComplianceGoal = Convert.ToInt32(txtComplianceGoal.Text);
                if (txtEmbargoStart.Text.Length > 0)
                    objFindingInfo.EmbargoStartHour = Convert.ToInt32(txtEmbargoStart.Text);
                if (txtEmbargoEnd.Text.Length > 0)
                    objFindingInfo.EmbargoEndHour = Convert.ToInt32(txtEmbargoEnd.Text);
                if (txtEndAfter.Text.Length > 0)
                    objFindingInfo.EndAfterMinutes = Convert.ToInt32(txtEndAfter.Text);
                if (txtEscalateEvery.Text.Length > 0)
                    objFindingInfo.EscalateEvery = Convert.ToInt32(txtEscalateEvery.Text);
                if (txtFindingOrder.Text.Length > 0)
                    objFindingInfo.FindingOrder = Convert.ToInt32(txtFindingOrder.Text);
                if (txtPriority.Text.Length > 0)
                    objFindingInfo.Priority = Convert.ToInt32(txtPriority.Text);
                //if (txtEndAt.Text.Length > 0)
                //    objFindingInfo.EndAt = Convert.ToInt32(txtEndAt.Text);
                objFindingInfo.Embargo = chkEmbargo.Checked ? true : false;
                objFindingInfo.RequireReadback = chkRequireReadback.Checked ? true : false;
                objFindingInfo.EmbargoSpanWeekend = chkEmbWeekend.Checked ? true : false;
                objFindingInfo.ContinueToSendPrimary = chkContinue.Checked ? true : false;
                objFindingInfo.StartBackupAt = Convert.ToInt32(cmbStartBackup.SelectedValue);
                objFindingInfo.DocumentedOnly = chkDocumentedOnly.Checked ? true : false;
                objFindingInfo.Active = chkActive.Checked ? true : false;
                objFindingInfo.NotificationDeviceTypeID = int.Parse(cmbDevice.SelectedValue);
                objFindingInfo.NotificationEventTypeID = int.Parse(cmbNotificationEvent.SelectedValue);
                objFindingInfo.IsDefault = chkDefault.Checked ? true : false;
                objFindingInfo.AgentActionTypeID = Convert.ToInt32(rbConnectLive.SelectedValue);

                if (Convert.ToBoolean(ViewState[IS_ADD]))
                {
                    objFinding.AddFindigs(objFindingInfo);
                    hdnTextChanged.Value = "false";
                }
                else
                {
                    //objFindingInfo.IsFindingActive = chkActive.Checked ? true : false;
                    objFindingInfo.FindingID = Convert.ToInt32(Request["FindingID"]);
                    objFinding.UpdateFindigs(objFindingInfo);
                    hdnTextChanged.Value = "false";
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("AddFinding.addFindingData:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
            finally
            {
                objFinding = null;
                objFindingInfo = null;
            }
        }
        /// <summary>
        /// Register JS variables, client side button click events
        /// </summary>
        private void registerJavascriptVariables()
        {
            string sbScript = "";
            sbScript += "var hdnTextChangedClientID = '" + hdnTextChanged.ClientID + "';";
            //ClientScript.RegisterStartupScript(Page.GetType(), "scriptDeviceClientIDs", sbScript, true);
            ScriptManager.RegisterStartupScript(UpdatePanelAddFinding, UpdatePanelAddFinding.GetType(), "scriptDeviceClientIDs", sbScript, true);

            txtEmbargoEnd.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtEmbargoStart.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtEndAfter.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            //txtEndAt.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtFindingOrder.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtEscalateEvery.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtComplianceGoal.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtPriority.Attributes.Add("onKeypress", "JavaScript:return isNumericKeyStroke();");
            txtDescription.Attributes.Add("onKeypress", "JavaScript:return isAlphaNumericKeyStrokeOrUnderscore();");

            cmbGroup.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtDescription.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            flupdVoiceOver.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            rbConnectLive.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            chkContinue.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            chkDocumentedOnly.Attributes.Add("onclick", "JavaScript:return TextChanged();");
            chkActive.Attributes.Add("onclick", "JavaScript:TextChanged(); enableDefault();");
            chkEmbWeekend.Attributes.Add("onclick", "JavaScript:return TextChanged();");

            //this.Form.Attributes.Add("onKeyDown", "JavaScript:return deactiveEnterAction();");

            btnSave.Attributes.Add("onFocus", "JavaScript:focusOnButton(true, true);");
            btnCancel.Attributes.Add("onFocus", "JavaScript:focusOnButton(true, false);");
            cmbNotificationEvent.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            cmbDevice.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            btnSave.Attributes.Add("onBlur", "JavaScript:focusOnButton(false, false);");
            btnCancel.Attributes.Add("onBlur", "JavaScript:focusOnButton(false, false);");
            //this.btnSave.Attributes.Add("onclick", "javascript:if (FindingCheckRequired()){document.getElementById('" + hdnTextChanged.ClientID + "').value = 'false';" + Page.ClientScript.GetPostBackEventReference(lnkSubmitUpload, "") + ";}else {return false;}");
            //this.btnSave.Attributes.Add("onclick", "javascript:if(!isFocusOnSaveButton){" + Page.ClientScript.GetPostBackEventReference(btnCancel, "") + ";return false;}if (FindingCheckRequired()){document.getElementById('" + hdnTextChanged.ClientID + "').value = 'false';" + Page.ClientScript.GetPostBackEventReference(lnkSubmitUpload, "") + ";}else {return false;}");
            this.btnSave.Attributes.Add("onclick", "javascript:if(!isFocusOnSaveButton){" + Page.ClientScript.GetPostBackEventReference(btnCancel, "") + ";return false;}if (FindingCheckRequired()){document.getElementById('" + hdnTextChanged.ClientID + "').value = 'false';return true;}else {return false;}");
            StringBuilder sbScript1 = new StringBuilder();
            sbScript1.Append("<script language=JavaScript>");
            sbScript1.Append("var cmbGroupClientID = '" + cmbGroup.ClientID + "';");
            sbScript1.Append("var txtDescriptionClientID = '" + txtDescription.ClientID + "';");
            sbScript1.Append("var rbConnectLiveClientID = '" + rbConnectLive.ClientID + "';");
            sbScript1.Append("var flupdVoiceOverClientID = '" + flupdVoiceOver.ClientID + "';");
            sbScript1.Append("var cmbInstitutionClientID = '" + cmbInstitution.ClientID + "';");
            sbScript1.Append("var hdnInstitutionValClientID = '" + hdnInstitutionVal.ClientID + "';");
            sbScript1.Append("var chkEmbargoClientID = '" + chkEmbargo.ClientID + "';");
            sbScript1.Append("var chkEmbWeekendClientID = '" + chkEmbWeekend.ClientID + "';");
            sbScript1.Append("var txtEmbargoStartClientID = '" + txtEmbargoStart.ClientID + "';");
            sbScript1.Append("var txtEmbargoEndClientID = '" + txtEmbargoEnd.ClientID + "';");
            sbScript1.Append("var txtFindingOrderClientID = '" + txtFindingOrder.ClientID + "';");
            sbScript1.Append("var txtPriorityClientID = '" + txtPriority.ClientID + "';");
            sbScript1.Append("var txtGroupCheckClientID = '" + txtGroupCheck.ClientID + "';");
            sbScript1.Append("var cmbNotificationEventClientID = '" + cmbNotificationEvent.ClientID + "';");
            sbScript1.Append("var cmbDeviceClientID = '" + cmbDevice.ClientID + "';");
            sbScript1.Append("var txtComplianceGoalClientID = '" + txtComplianceGoal.ClientID + "';");
            sbScript1.Append("var txtEndAfterClientID = '" + txtEndAfter.ClientID + "';");
            sbScript1.Append("var txtEscalateEveryClientID = '" + txtEscalateEvery.ClientID + "';");
            sbScript1.Append("var chkActiveClientID = '" + chkActive.ClientID + "';");
            sbScript1.Append("var chkDocumentedOnlyClientID = '" + chkDocumentedOnly.ClientID + "';");
            sbScript1.Append("var chkContinueClientID = '" + chkContinue.ClientID + "';");
            sbScript1.Append("var chkRequireReadbackClientID = '" + chkRequireReadback.ClientID + "';");
            sbScript1.Append("var cmbStartBackupClientID = '" + cmbStartBackup.ClientID + "';");
            sbScript1.Append("var chkDefaultClientID = '" + chkDefault.ClientID + "';");
            sbScript1.Append("enableConnectLive();");
            sbScript1.Append("enableDefault();");
            
            
            sbScript1.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript1.ToString());

            chkEmbargo.Attributes.Add("onClick", "Javascript:return enableEmbargo('true');");
            chkDocumentedOnly.Attributes.Add("onClick", "Javascript:return enableConnectLive();");
            cmbInstitution.Attributes.Add("onchange", "Javascript:return onComboChange();");

            lnkSubmitUpload.Attributes.Add("onclick", "return checkRequired();");
            sbScript1 = null;
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
        /// This Function is used to set page title as per Add/Edit mode
        /// </summary>
        private void checkMode()
        {
            if (Request["Mode"] != null)
            {
                if (Request["Mode"].ToString() == "Add")
                {
                    ViewState[IS_ADD] = true;
                    lblPageName.Text = "Add Findings";
                    Page.Title = "CSTools: Add Findings";
                    //chkActive.Visible = false;
                    //lblActive.Visible = false;
                }
                else
                {
                    ViewState[IS_ADD] = false;
                    lblPageName.Text = "Edit Findings";
                    Page.Title = "CSTools: Edit Findings";
                    //chkActive.Visible = true;
                    //lblActive.Visible = true;
                }
            }
            else
            {
                ViewState[IS_ADD] = true;
                lblPageName.Text = "Add Findings";
                Page.Title = "CSTools: Add Findings";
                //chkActive.Visible = false;
                //lblActive.Visible = false;
            }
        }
        /// <summary>
        /// This Function is used to populate information of Finding by finding ID
        /// </summary>
        private void fillFinding()
        {
            Finding objFinding = null;
            DataTable dtFinding = null;
            try
            {
                objFinding = new Finding();
                int findingID = Convert.ToInt32(Request["FindingID"]);
                dtFinding = objFinding.GetFindingInfoByFindingID(findingID);

                if (dtFinding.Rows.Count > 0)
                {
                    DataRow drFindinginfo = dtFinding.Rows[0];
                    txtDescription.Text = drFindinginfo["FindingDescription"].ToString();
                    txtComplianceGoal.Text = drFindinginfo["ComplianceGoal"].ToString();
                    txtEmbargoEnd.Text = drFindinginfo["EmbargoEndHour"].ToString();
                    txtEmbargoStart.Text = drFindinginfo["EmbargoStartHour"].ToString();
                    txtEscalateEvery.Text = drFindinginfo["EscalateEvery"].ToString();
                    txtEndAfter.Text = drFindinginfo["EndAfterMinutes"].ToString();
                    txtFindingOrder.Text = drFindinginfo["FindingOrder"].ToString();
                    cmbStartBackup.SelectedValue = drFindinginfo["StartBackupAt"].ToString(); ;
                    chkContinue.Checked = Convert.ToBoolean(drFindinginfo["ContinueToSendPrimary"]);
                    chkEmbargo.Checked = Convert.ToBoolean(drFindinginfo["Embargo"]);
                    chkEmbWeekend.Checked = Convert.ToBoolean(drFindinginfo["EmbargoSpanWeekend"]);
                    chkRequireReadback.Checked = Convert.ToBoolean(drFindinginfo["RequireReadback"]);
                    //chkActive.Checked = Convert.ToBoolean(drFindinginfo["IsFindingActive"]);
                    chkDocumentedOnly.Checked = Convert.ToBoolean(drFindinginfo["IsDocumented"]);
                    txtPriority.Text = drFindinginfo["Priority"].ToString();
                    chkActive.Checked = Convert.ToBoolean(drFindinginfo["IsFindingActive"]);
                    cmbGroup.SelectedValue = drFindinginfo["GroupID"].ToString();
                    cmbGroup.Enabled = false;
                    txtGroupName.Text = cmbGroup.SelectedItem.Text;
                    cmbNotificationEvent.SelectedValue = drFindinginfo["NotificationEventTypeID"].ToString();
                    cmbDevice.SelectedValue = drFindinginfo["NotificationDeviceTypeID"].ToString();
                    chkDefault.Checked = Convert.ToBoolean(drFindinginfo["IsDefault"]);
                    rbConnectLive.SelectedValue = drFindinginfo["AgentActionTypeID"].ToString();
                    //cmbGroup.Visible = false;

                    txtGroupName.Visible = true;
                    ViewState[GROUP_ID] = cmbGroup.SelectedValue;
                    if (drFindinginfo["FindingVoiceOverURL"].ToString().Length > 0)
                    {
                        hlinkPlay.Visible = true;
                        hlinkPlay.NavigateUrl = drFindinginfo["FindingVoiceOverURL"].ToString();
                        flupdVoiceOver.Width = 231;
                    }
                    else
                    {
                        hlinkPlay.Visible = false;
                        flupdVoiceOver.Width = 250;
                    }
                    if (isSystemAdmin)
                    {
                        cmbInstitution.SelectedValue = drFindinginfo["InstitutionID"].ToString();
                        cmbInstitution.Enabled = false;
                        ViewState[INSTITUTE_ID] = cmbInstitution.SelectedValue;
                        hlinkGroupMonitor.NavigateUrl = "./group_monitor.aspx?InstitutionID=" + ViewState[INSTITUTE_ID]; ;
                        txtInstitution.Text = cmbInstitution.SelectedItem.Text;
                        //cmbInstitution.Visible = false;
                        txtInstitution.Visible = true;
                    }
                    enabledConrol(true);
                    cmbGroup.Enabled = false;
                    btnSave.Enabled = true;
                    string script = "hideControl('" + isSystemAdmin + "');";
                    ClientScript.RegisterStartupScript(Page.GetType(), "HideControl", script, true);
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("EditFinding.Fill Finding:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
            finally
            {
                objFinding = null;
                dtFinding = null;
            }
        }
        /// <summary>
        /// Check for Directory and upload file over there
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        private string upload(int groupid)
        {
            string url = "";
            try
            {
                if (flupdVoiceOver.HasFile)
                {
                    string fileName = flupdVoiceOver.FileName;
                    string directory = ConfigurationSettings.AppSettings["FindingsDirectory"];
                    url = ConfigurationSettings.AppSettings["baseURL"];
                    directory += groupid;
                    if (!System.IO.Directory.Exists(directory))
                        System.IO.Directory.CreateDirectory(directory);
                    directory += @"\" + fileName;
                    flupdVoiceOver.SaveAs(directory);
                    url += groupid + @"/" + fileName;
                }
                else if (!Convert.ToBoolean(ViewState[IS_ADD]))
                {
                    if (hlinkPlay.NavigateUrl != "")
                    {
                        url = hlinkPlay.NavigateUrl;
                    }
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("Add/Edit Finding.upload:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
            return url;
        }
        /// <summary>
        /// Clear the Text controls
        /// </summary>
        private void clearConrol()
        {
            txtComplianceGoal.Text = "";
            txtDescription.Text = "";
            txtEmbargoEnd.Text = "";
            txtEmbargoStart.Text = "";
            txtEndAfter.Text = "";
            //txtEndAt.Text = "";
            txtEscalateEvery.Text = "";
            txtFindingOrder.Text = "";
            txtPriority.Text = "";
            chkActive.Checked = false;
            chkDocumentedOnly.Checked = false;
            //txtOTN.Text = "";
            chkContinue.Checked = false;
            chkEmbargo.Checked = false;
            chkEmbWeekend.Checked = false;
            chkRequireReadback.Checked = false;
            cmbStartBackup.SelectedIndex = 0;
            cmbDevice.SelectedIndex = 0;
            cmbNotificationEvent.SelectedIndex = 0;
            chkDefault.Checked = false;
            rbConnectLive.Enabled = true;
            rbConnectLive.SelectedValue = "0";
        }
        /// <summary>
        /// Enabled - Disabled controls as per flag passed
        /// </summary>
        /// <param name="flg"></param>
        private void enabledConrol(bool flg)
        {
            txtComplianceGoal.Enabled = flg;
            txtDescription.Enabled = flg;
            txtEndAfter.Enabled = flg;
            //txtEndAt.Enabled = flg;
            txtEscalateEvery.Enabled = flg;
            txtFindingOrder.Enabled = flg;
            //txtOTN.Enabled = flg;
            chkContinue.Enabled = flg;
            chkEmbargo.Enabled = flg;

            chkRequireReadback.Enabled = flg;
            flupdVoiceOver.Enabled = flg;
            cmbGroup.Enabled = flg;
            cmbStartBackup.Enabled = flg;

            chkActive.Enabled = flg;
            txtPriority.Enabled = flg;
            chkDocumentedOnly.Enabled = flg;

            cmbNotificationEvent.Enabled = flg;
            cmbDevice.Enabled = flg;
            if (!flg)
            {
                //chkEmbWeekend.Enabled = flg;
                txtEmbargoEnd.Enabled = flg;
                txtEmbargoStart.Enabled = flg;
            }
            rbConnectLive.Enabled = flg;
            chkDefault.Disabled = !chkActive.Checked;
        }

        /// <summary>
        /// This function is to fill Notification Events into drop down list 
        /// </summary>
        private void fillNotificationEvents()
        {
            OrderingClinician objOC = null;
            DataTable dtNotifyEvents = null;
            DataRow drNew = null;
            try
            {
                objOC = new OrderingClinician();
                dtNotifyEvents = objOC.GetOCNotifyEvents();
                drNew = dtNotifyEvents.NewRow();
                drNew[0] = 0;
                drNew[1] = "-- Select Notification Event --";
                dtNotifyEvents.Rows.InsertAt(drNew, 0);
                cmbNotificationEvent.DataSource = dtNotifyEvents.DefaultView;
                cmbNotificationEvent.DataBind();
            }
            finally
            {
                dtNotifyEvents = null;
                objOC = null;
                drNew = null;
            }

        }

        /// <summary>
        /// This function is to fill Devices into drop down list 
        /// </summary>
        private void fillDevices()
        {
            Finding objFinding = null;
            DataTable dtFindings = null;
            DataRow drNew = null;
            try
            {
                objFinding = new Finding();
                dtFindings = objFinding.GetFindingDevices();
                drNew = dtFindings.NewRow();
                drNew[0] = 0;
                drNew[1] = "-- Select Device --";
                dtFindings.Rows.InsertAt(drNew, 0);
                cmbDevice.DataSource = dtFindings.DefaultView;
                cmbDevice.DataBind();
            }
            finally
            {
                dtFindings = null;
                objFinding = null;
                drNew = null;
            }

        }
        #endregion Private Methods
    }
}