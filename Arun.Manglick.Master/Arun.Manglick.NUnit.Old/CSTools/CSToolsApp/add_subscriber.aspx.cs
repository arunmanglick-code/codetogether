#region File History

/******************************File History***************************
 * File Name        : add_subscriber.aspx.cs
 * Author           : ZNK.
 * Created Date     : 20 November 07
 * Purpose          : Interface to add Subscriber Information.
 *                  : 

 * *********************File Modification History*********************

 * * Date(dd-mm-yyyy) Developer Reason of Modification
**********************************************************************
 * 30-11-2007    IAK     Added javascript variables, UI alignment, and validation 
 * 03-12-2007    IAK     UI Changes and specilaty section move to upper section. 
 * 06-12-2007    IAK     On enter press, btnsave get called defect fixed, Code review changes. 
 * 06-12-2007    IAK     On enter press, btnsave get called defect fixed,
 * 06-12-2007    IAK     On enter press on cancel button, btnsave get called defect fixed,
 * 04-01-2008    IAK     Generate pin button added and validation on login id added
 * 07-01-2008    IAK     Defects solving 2523, 2455, 2526, 2533, 2537, 2538, 2540, 2539, 2541, 2542, 2544, 2545
 * 07-01-2008    IAK     On generate pin the speciality info should not get blank
 * 11-01-2008    IAK     Defect 2558
 * 24-04-2008    Suhas   Defect # 3043, 3046 - Fixed
 * 25-04-2008    Suhas   Defect # 3053 - Fixed
 * 12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
*********************************************************************
 */
#endregion

using System;
using System.Text;
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

namespace Vocada.CSTools
{
    public partial class add_subscriber : System.Web.UI.Page
    {
        #region Constants
        public const string SORT_ORDER = "SortOrder";
        private bool isSystemAdmin = true;
        private int instID;
        #endregion Constants

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session[SessionConstants.USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
                    Response.Redirect(Utils.GetReturnURL("default.aspx", "add_subscriber.aspx", this.Page.ClientQueryString));

                Session[SessionConstants.CURRENT_TAB] = "Tools";
                Session[SessionConstants.CURRENT_INNER_TAB] = "AddSubscriber";
                Session[SessionConstants.CURRENT_PAGE] = "add_subscriber.aspx";

                UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
                if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
                {
                    isSystemAdmin = false;
                    instID = userInfo.InstitutionID;
                    ScriptManager.RegisterStartupScript(upnlInstitutions, upnlInstitutions.GetType(), "showSubScriberInfo", "document.getElementById('" + divSubscriberInfo.ClientID + "').style.visibility = '';", true);
                }
                else
                    isSystemAdmin = true;
             
                registerJavascriptVariables();
                if (!IsPostBack)
                {
                    btnGeneratePin.Enabled = false;
                    if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
                    {
                        isSystemAdmin = false;
                        hdnIsSystemAdmin.Value = "0";
                        cmbInstitutions.Visible = false;
                        lblInstName.Visible = true;
                        lblInstName.Text = " " + userInfo.InstitutionName;
                    }
                    else
                    {
                        isSystemAdmin = true;
                        hdnIsSystemAdmin.Value = "1";
                        cmbInstitutions.Visible = true; ;
                        lblInstName.Visible = false;
                        populateInstitutions();
                        if (Request["InstitutionID"] != null)
                            cmbInstitutions.SelectedValue = (Request["InstitutionID"].ToString());
                    }
                    fillGroups(instID);
                }

                Session["CurrentTab"] = "Tools";
                Session["CurrentInnerTab"] = "Add Subscriber";
                Session["CurrentPage"] = "add_subscriber.aspx";

                if (cmbInstitutions.SelectedValue == "-1")
                    ScriptManager.RegisterStartupScript(upnlInstitutions, upnlInstitutions.GetType(), "AddSpecialist", "document.getElementById('" + divSubscriberInfo.ClientID + "').style.visibility = 'Hidden';", true);
                else
                    ScriptManager.RegisterStartupScript(upnlInstitutions, upnlInstitutions.GetType(), "AddSpecialist", "document.getElementById('" + divSubscriberInfo.ClientID + "').style.visibility = 'visible';", true);

                this.Form.DefaultButton = this.btnSaveSubscriber.UniqueID;
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_subscriber - Page_Load", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        protected void cmbInstitutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                instID = Convert.ToInt32(cmbInstitutions.SelectedValue);
                fillGroups(instID);
                clearControl();
                if (cmbInstitutions.SelectedValue == "-1")
                    ScriptManager.RegisterStartupScript(upnlInstitutions, upnlInstitutions.GetType(), "AddSpecialist", "document.getElementById('" + divSubscriberInfo.ClientID + "').style.visibility = 'Hidden';", true);
                else
                    ScriptManager.RegisterStartupScript(upnlInstitutions, upnlInstitutions.GetType(), "AddSpecialist", "document.getElementById('" + divSubscriberInfo.ClientID + "').style.visibility = 'visible';", true);

            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_subscriber - cmbInstitutions_SelectedIndexChanged", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }

        protected void cmbGroupName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtGroupInfo = null;
            Group objGroup = null;
            try
            {
                dtGroupInfo = new DataTable();
                objGroup = new Group();
                ViewState["GroupID"] = cmbGroupName.SelectedValue;
                dtGroupInfo = objGroup.GetGroupInformationByGroupID(Convert.ToInt32(ViewState["GroupID"].ToString()));
                if (dtGroupInfo.Rows.Count > 0)
                {
                    ViewState["Group800Num"] = dtGroupInfo.Rows[0]["Group800Number"].ToString();
                    ViewState["GroupType"] = dtGroupInfo.Rows[0]["GroupType"].ToString();
                    ViewState["Lab800Num"] = dtGroupInfo.Rows[0]["Lab800Number"].ToString();
                    txtLoginId.Text = ViewState["Group800Num"].ToString();
                    txtPin.Text = "";
                    cmbGroupName.Focus();
                }

                if (int.Parse(cmbGroupName.SelectedValue) == -1)
                    btnGeneratePin.Enabled = false;
                else
                    btnGeneratePin.Enabled = true;
                fillRoles();
                ScriptManager.RegisterStartupScript(upnlInstitutions, upnlInstitutions.GetType(), "AddSpecialist", "document.getElementById('" + divSubscriberInfo.ClientID + "').style.visibility = 'visible';", true);
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_subscriber - cmbGroupName_SelectedIndexChanged", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                dtGroupInfo = null;
                objGroup = null;
            }
        }

        protected void btnSaveSubscriber_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    int subscriberID = addSubscriberData();
                    if (subscriberID > 0)
                    {
                        btnSaveSubscriber.Enabled = false;
                        btnCancel.Enabled  = false;
                        int RoleID = Convert.ToInt32(cmbRole.SelectedValue);
                        clearControl();
                        //fillRoles();
                        cmbGroupName.SelectedValue = "-1";
                        cmbRole.SelectedValue = "-1";

                        ScriptManager.RegisterClientScriptBlock(upnlInstitutions, upnlInstitutions.GetType(), "AddSpecialist", "document.getElementById('" + divSubscriberInfo.ClientID + "').style.visibility = 'visible';document.getElementById('" + txtChanged.ClientID + "').value=false; alert('Subscriber added successfully.');document.getElementById('" + cmbGroupName.ClientID + "').focus();", true);
                    }
                    else
                    {
                        string errorScript = "";
                        if (cmbRole.SelectedValue == "1" || cmbRole.SelectedValue == "4")
                            errorScript += "document.getElementById('" + divSpecialistInfo.ClientID + "').style.display = '';";

                        errorScript += "document.getElementById('" + divSubscriberInfo.ClientID + "').style.visibility = 'visible';";

                        if (subscriberID == -1)
                            errorScript += "alert('- Please enter different login id. This login id is already used in other group.');document.getElementById('" + txtLoginId.ClientID + "').focus();";
                        else
                            errorScript += "alert('- Please enter different PIN,  This PIN  is already used by other subscriber.');document.getElementById('" + txtPin.ClientID + "').focus();";

                        ScriptManager.RegisterClientScriptBlock(upnlInstitutions,upnlInstitutions.GetType(), "errorMessage", errorScript, true);
                    }
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null && Session[SessionConstants.USER_ID].ToString().Length > 0)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - btnSaveSubscriber_Click", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - btnSaveSubscriber_Click", "0", objException.Message, objException.StackTrace), 0);
                }
                throw objException;
            }
        }

        /// <summary>
        /// Generate new pin for subscriber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGeneratePin_Click(object sender, EventArgs e)
        {
            Subscriber objSubscriber = null;
            try
            {
                objSubscriber = new Subscriber();
                txtPin.Text = objSubscriber.GeneratePin(Convert.ToInt32(cmbGroupName.SelectedValue));
                ScriptManager.RegisterClientScriptBlock(upnlInstitutions, upnlInstitutions.GetType(), "AddSpecialist", "document.getElementById('" + divSubscriberInfo.ClientID + "').style.visibility = 'visible';ShowSpecialistInfo(document.getElementById('" + cmbRole.ClientID + "'), false);document.getElementById('" + btnGeneratePin.ClientID + "').focus();", true);

            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null && Session[SessionConstants.USER_ID].ToString().Length > 0)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - btnGeneratePin_Click", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - btnGeneratePin_Click", "0", objException.Message, objException.StackTrace), 0);
                }
                throw objException;
            }
        }


        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
            if (userInfo.RoleId == UserRoles.SupportLevel2.GetHashCode())
            {
                string url = "./add_subscriber.aspx";
                ScriptManager.RegisterClientScriptBlock(upnlInstitutions,upnlInstitutions.GetType(),"Cancel", "<script type=\'text/javascript\'>Navigate('" + url + "');</script>",false);
            }
            else
            {
                string url = "./institution_information.aspx";
                ScriptManager.RegisterClientScriptBlock(upnlInstitutions,upnlInstitutions.GetType(), "Cancel", "<script type=\'text/javascript\'>Navigate('" + url + "');</script>",false);
            }
        }

        #endregion Events

        #region Private Methods
        /// <summary>
        /// populate Institutions in ComboBox
        /// </summary>
        private void populateInstitutions()
        {
            Institution objInstitution = null;

            try
            {
                objInstitution = new Institution();
                cmbInstitutions.DataSource = objInstitution.GetInstitutionList();
                cmbInstitutions.DataTextField = "InstitutionName";
                cmbInstitutions.DataValueField = "InstitutionID";
                cmbInstitutions.DataBind();
                cmbInstitutions.Items.Insert(0, new ListItem("-- Select Institution --", "-1"));
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null && Session[SessionConstants.USER_ID].ToString().Length > 0)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - populateInstitutions", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - populateInstitutions", "0", objException.Message, objException.StackTrace), 0);
                }
                throw objException;
            }
            finally
            {
                objInstitution = null;
            }
        }

        /// <summary>
        /// Gets Groups for selected Institution.
        /// </summary>
        /// <param name="institutionID"></param>
        private void fillGroups(int institutionID)
        {
            DataTable dtGroups = null;
            Group objGroup = null;
            ListItem listItem = null;
            try
            {
                cmbGroupName.Items.Clear();

                dtGroups = new DataTable();
                objGroup = new Group();
                dtGroups = objGroup.GetGroupsForInstitution(institutionID, false);
                cmbGroupName.DataSource = dtGroups;
                cmbGroupName.DataBind();

                listItem = new ListItem("-- Select Group --", "-1");
                cmbGroupName.Items.Insert(0, listItem);
                cmbGroupName.Items.FindByValue("-1").Selected = true;
                if (cmbInstitutions.Visible == true)
                    cmbInstitutions.Focus();
                else
                    cmbGroupName.Focus();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null && Session[SessionConstants.USER_ID].ToString().Length > 0)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - fillGroups", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - fillGroups", "0", objException.Message, objException.StackTrace), 0);
                }
                throw objException;
            }
            finally
            {
                dtGroups = null;
                objGroup = null;
                listItem = null;
            }
        }

        /// <summary>
        /// Fill Roles as per group type (lab/rad)
        /// </summary>
        private void fillRoles()
        {
            RoleTask objRoleTask = null;
            DataTable dtroles = null;
            ListItem listItem = null;
            try
            {
                objRoleTask = new RoleTask();
                dtroles = new DataTable();

                dtroles = objRoleTask.GetRolesForGroup(int.Parse(cmbGroupName.SelectedValue));

                cmbRole.Items.Clear();
                if (cmbGroupName.SelectedValue != "-1")
                {
                    cmbRole.DataSource = dtroles;
                    cmbRole.DataTextField = "RoleDescription";
                    cmbRole.DataValueField = "RoleID";
                    cmbRole.DataBind();
                    if (ViewState["GroupType"] != null)
                    {
                        for (int cnt = 0; cnt < cmbRole.Items.Count; cnt++)
                        {
                            if (!Convert.ToBoolean(ViewState["GroupType"].ToString()))
                            {
                                if (cmbRole.Items[cnt].Value == "4" || cmbRole.Items[cnt].Value == "5" || cmbRole.Items[cnt].Value == "6" || cmbRole.Items[cnt].Value == "7" || cmbRole.Items[cnt].Value == "8")
                                {
                                    cmbRole.Items.Remove(cmbRole.Items[cnt]);
                                    cnt--;
                                }
                            }
                            else
                            {
                                if (cmbRole.Items[cnt].Value == "1" || cmbRole.Items[cnt].Value == "2" || cmbRole.Items[cnt].Value == "3" || cmbRole.Items[cnt].Value == "5" || cmbRole.Items[cnt].Value == "6" || cmbRole.Items[cnt].Value == "7")
                                {
                                    cmbRole.Items.Remove(cmbRole.Items[cnt]);
                                    cnt--;
                                }
                            }
                        }
                    }
                }
                listItem = new ListItem("--Select Role --", "-1");
                cmbRole.Items.Insert(0, listItem);
                cmbRole.Items.FindByValue("-1").Selected = true;
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null && Session[SessionConstants.USER_ID].ToString().Length > 0)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - fillRoles", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - fillRoles", "0", objException.Message, objException.StackTrace), 0);
                }
                throw objException;
            }
            finally
            {
                objRoleTask = null;
                dtroles = null;
                listItem = null;
            }

        }

        /// <summary>
        /// Register JS variables, client side button click events
        /// </summary>
        private void registerJavascriptVariables()
        {
            UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
            txtPin.Attributes.Add("onKeyPress", "JavaScript:deactiveEnterAction();return isNumericKeyStrokes();");
            txtPhone1.Attributes.Add("onKeyPress", "JavaScript:deactiveEnterAction();return isNumericKey();");
            txtPhone2.Attributes.Add("onKeyPress", "JavaScript:deactiveEnterAction();return isNumericKey();");
            txtPhone3.Attributes.Add("onKeyPress", "JavaScript:deactiveEnterAction();return isNumericKey();");
            txtFax1.Attributes.Add("onKeyPress", "JavaScript:deactiveEnterAction();return isNumericKey();");
            txtFax2.Attributes.Add("onKeyPress", "JavaScript:deactiveEnterAction();return isNumericKey();");
            txtFax3.Attributes.Add("onKeyPress", "JavaScript:deactiveEnterAction();return isNumericKey();");
            //if (isSystemAdmin == true)
            //{
            //    btnCancel.Attributes.Add("onclick", "JavaScript:cancelClick('institution_information.aspx');");
            //    btnCancel.Attributes.Add("onkeyPress", "JavaScript:deactiveEnterAction();event.cancel = true;window.event.keyCode = 23;cancelClick('institution_information.aspx');document.getElementById('" + btnCancel.ClientID + "').focus();");
            //}
            //else
            //{
            //    btnCancel.Attributes.Add("onclick", "JavaScript:cancelClick('group_monitor.aspx?InstitutionID=" + instID.ToString() + "');");
            //    btnCancel.Attributes.Add("onkeyPress", "JavaScript:deactiveEnterAction();event.cancel = true;window.event.keyCode = 23;cancelClick('institution_information.aspx');document.getElementById('" + btnCancel.ClientID + "').focus();");
            //}

            //btnCancel.Attributes.Add("onkeyPress", "JavaScript:deactiveEnterAction();event.cancel = true;window.event.keyCode = 23;cancelClick('institution_information.aspx');document.getElementById('" + btnCancel.ClientID + "').focus();");
            if (userInfo.RoleId == UserRoles.SystemAdmin.GetHashCode())
            {
                cmbInstitutions.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            }
            cmbGroupName.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            cmbRole.Attributes.Add("onchange", "JavaScript:ShowSpecialistInfo(this, true);UpdateProfile('" + txtChanged.ClientID + "');");
            txtLoginId.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtPin.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtFirstName.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtLastName.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtPrimaryEmail.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");

            cmbInstitutions.Attributes.Add("onKeyPress", "JavaScript:return deactiveEnterAction();");
            cmbGroupName.Attributes.Add("onKeyPress", "JavaScript:return deactiveEnterAction();");
            cmbRole.Attributes.Add("onKeyPress", "JavaScript:return deactiveEnterAction();");

            txtLoginId.Attributes.Add("onKeyPress", "JavaScript:deactiveEnterAction();return isAlphaNumericKey();");
            txtNickname.Attributes.Add("onKeyPress", "JavaScript:return deactiveEnterAction();");
            txtPrimaryEmail.Attributes.Add("onKeyPress", "JavaScript:return deactiveEnterAction();");
            txtAffilation.Attributes.Add("onKeyPress", "JavaScript:return deactiveEnterAction();");
            txtSpeciality.Attributes.Add("onKeyPress", "JavaScript:return deactiveEnterAction();");

            txtNickname.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");

            txtPhone1.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtPhone2.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtPhone3.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");

            txtFax1.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtFax2.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtFax3.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");

            txtSpeciality.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");
            txtAffilation.Attributes.Add("onchange", "JavaScript:UpdateProfile('" + txtChanged.ClientID + "');");

            btnSaveSubscriber.Attributes.Add("onFocus", "JavaScript:focusOnButton(true, true);");
            btnCancel.Attributes.Add("onFocus", "JavaScript:focusOnButton(true, false);");
            btnSaveSubscriber.Attributes.Add("onBlur", "JavaScript:focusOnButton(false, false);");
            btnCancel.Attributes.Add("onBlur", "JavaScript:focusOnButton(false, false);");
            btnGeneratePin.Attributes.Add("onBlur", "JavaScript:focusOnButton(false, false);");
            btnGeneratePin.Attributes.Add("onBlur", "JavaScript:focusOnButton(false, false);");
            btnGeneratePin.Attributes.Add("onKeyPress", "JavaScript:" + Page.ClientScript.GetPostBackEventReference(btnGeneratePin, "") + ";window.event.keyCode = 23;document.getElementById('" + txtFirstName.ClientID + "').focus();return false;");


            txtFax1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtFax2.ClientID + "').focus()";
            txtFax2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtFax3.ClientID + "').focus()";
            txtFax3.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) {if(document.getElementById(divSpecialistInfoClientID).style.display == 'none')document.getElementById('" + btnSaveSubscriber.ClientID + "').focus(); else document.getElementById('" + txtSpeciality.ClientID + "').focus();}";
            txtFax3.Attributes["onBlur"] = "javascript:  if(document.getElementById(divSpecialistInfoClientID).style.display == 'none')document.getElementById('" + btnSaveSubscriber.ClientID + "').focus(); else document.getElementById('" + txtSpeciality.ClientID + "').focus();";

            txtPhone1.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPhone2.ClientID + "').focus()";
            txtPhone2.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPhone3.ClientID + "').focus()";
            txtPhone3.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=4 && (keyCode != 9)) document.getElementById('" + txtFax1.ClientID + "').focus()";

            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var cmbGroupClientID = '" + cmbGroupName.ClientID + "';");
            sbScript.Append("var cmbRoleClientID = '" + cmbRole.ClientID + "';");
            sbScript.Append("var txtLoginIdClientID = '" + txtLoginId.ClientID + "';");
            sbScript.Append("var txtPinClientID = '" + txtPin.ClientID + "';");
            sbScript.Append("var txtFirstNameClientID = '" + txtFirstName.ClientID + "';");
            sbScript.Append("var txtLastNameClientID = '" + txtLastName.ClientID + "';");
            sbScript.Append("var txtChangedClientID = '" + txtChanged.ClientID + "';");

            sbScript.Append("var txtPhone1ClientID = '" + txtPhone1.ClientID + "';");
            sbScript.Append("var txtPhone2ClientID = '" + txtPhone2.ClientID + "';");
            sbScript.Append("var txtPhone3ClientID = '" + txtPhone3.ClientID + "';");

            sbScript.Append("var txtFax1ClientID = '" + txtFax1.ClientID + "';");
            sbScript.Append("var txtFax2ClientID = '" + txtFax2.ClientID + "';");
            sbScript.Append("var txtFax3ClientID = '" + txtFax3.ClientID + "';");

            sbScript.Append("var divSpecialistInfoClientID = '" + divSpecialistInfo.ClientID + "';");
            sbScript.Append("var txtAffilationClientID = '" + txtAffilation.ClientID + "';");
            sbScript.Append("var txtSpecialityClientID = '" + txtSpeciality.ClientID + "';");
            sbScript.Append("var txtPrimaryEmailClientID = '" + txtPrimaryEmail.ClientID + "';");
            sbScript.Append("var btnGeneratePinClientID = '" + btnGeneratePin.ClientID + "';");
            
            sbScript.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());
            sbScript = null;
        }

        /// <summary>
        /// Add new subscriber record in database
        /// </summary>
        private int addSubscriberData()
        {
            Subscriber objSubscriber = null;
            SubscriberInformation objSubscriberInfo = null;
            int subscriberID = 0;
            try
            {
                objSubscriber = new Subscriber();
                objSubscriberInfo = new SubscriberInformation();

                objSubscriberInfo.SubscriberID = 0;
                objSubscriberInfo.GroupID = Convert.ToInt32(cmbGroupName.SelectedValue);
                objSubscriberInfo.RoleID = Convert.ToInt32(cmbRole.SelectedValue);
                objSubscriberInfo.LoginID = txtLoginId.Text.Trim();
                objSubscriberInfo.Password = txtPin.Text.Trim();
                objSubscriberInfo.FirstName = txtFirstName.Text.Trim();
                objSubscriberInfo.LastName = txtLastName.Text.Trim();
                objSubscriberInfo.PrimaryPhone = txtPhone1.Text + txtPhone2.Text + txtPhone3.Text;
                objSubscriberInfo.NickName = txtNickname.Text.Trim();
                objSubscriberInfo.PrimaryEmail = txtPrimaryEmail.Text.Trim();
                objSubscriberInfo.Fax = txtFax1.Text + txtFax2.Text + txtFax3.Text;
                objSubscriberInfo.Active = true;
                objSubscriberInfo.Affiliation = txtAffilation.Text.Trim();
                objSubscriberInfo.Specialty = txtSpeciality.Text.Trim();
                subscriberID = objSubscriber.AddSubscriber(objSubscriberInfo);

                return subscriberID;
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_subscriber - AddSubscriberData", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                objSubscriber = null;
                objSubscriberInfo = null;
            }
        }

        /// <summary>
        /// Clear control values from Account,User and Specilaty section
        /// </summary>
        private void clearControl()
        {
            txtLoginId.Text = "";
            txtPin.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtNickname.Text = "";
            txtPrimaryEmail.Text = "";
            txtPhone1.Text = "";
            txtPhone2.Text = "";
            txtPhone3.Text = "";
            txtFax1.Text = "";
            txtFax2.Text = "";
            txtFax3.Text = "";
            txtAffilation.Text = "";
            txtSpeciality.Text = "";
            btnSaveSubscriber.Enabled = true;
            btnCancel.Enabled = true;
            btnGeneratePin.Enabled = false;

            cmbRole.Items.Clear();
            ListItem listItem = new ListItem("--Select Role --", "-1");
            cmbRole.Items.Insert(0, listItem);
            cmbRole.Items.FindByValue("-1").Selected = true;
        }

        #endregion Private Methods

    }
}