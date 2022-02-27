#region File History

/******************************File History***************************
 * File Name        : add_directory.aspx.cs
 * Author           : IAK.
 * Created Date     : 23 July 07
 * Purpose          : Display list of all OC Directories and allow to edit also.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * * Date(dd-mm-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 *  24-07-2007 - Prerak - Change for selecting institution from session.
 *  23-10-2007 - Prerak - Refinement of the page.  
 *  26-10-2007 - IAK    - UI Changes /Page navigation etc.
 *  26-10-2007 - IAK    - All Catch Block updated.
 *  26-10-2007 - IAK    - On Edit disable the save button.
 *  30-10-2007 - IAK    - Defects UI Changes (Header).
 *  31-10-2007 - PCS    - Added for system Admin and Institution Admin Role.
 *  28-11-2007 - IAK    - Modified Function setDatagridHeight
 *  27-03-2008 - IAK    - Pass institutionName for edit directory name  on grid Defect: 2958
 *  12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 * * ------------------------------------------------------------------- 
 
 */
#endregion

#region Using Block
using System;
using System.Text;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.Utilities;
#endregion

namespace Vocada.CSTools
{
    public partial class add_directory : System.Web.UI.Page
    {
        #region Private Variable
        private bool isSystemAdmin = true;
        private int instID;
        private int userID = 0;
        #endregion

        #region Page Events

        /// <summary>
        /// Load the page content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session[SessionConstants.USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
                    Response.Redirect(Utils.GetReturnURL("default.aspx", "add_institution.aspx", this.Page.ClientQueryString));

                Session[SessionConstants.CURRENT_TAB] = MenuTab.TOOLS;
                Session[SessionConstants.CURRENT_INNER_TAB] = MenuInnerTab.ADDOCDIR;
                Session[SessionConstants.CURRENT_PAGE] = CSToolsPage.ADDOCDIR;
                UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
                if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
                {
                    isSystemAdmin = false;
                    instID = userInfo.InstitutionID;  
                }
                else
                    isSystemAdmin = true;
                
                registerJavascriptVariables();
                if (!Page.IsPostBack)
                {
                    Session[SessionConstants.WEEK_NUMBER] = null;
                    Session[SessionConstants.SHOWMESSAGES] = null;
                    Session[SessionConstants.STATUS] = null;
                    Session[SessionConstants.GROUP] = null;                   

                    if (userInfo.RoleId  == UserRoles.InstitutionAdmin.GetHashCode())
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
                    ViewState[Utils.SORT_ORDER] = "ASC";
                    populateDirectories();
                    userID = (Session[SessionConstants.USER_ID] != null && Session[SessionConstants.USER_ID].ToString().Length > 0) ? Convert.ToInt32(Session[SessionConstants.USER_ID].ToString()) : 0;
                    setDatagridHeight(true);

                    if (cmbInstitutions.Visible && Convert.ToInt32(cmbInstitutions.SelectedValue) > 0)
                        Session[SessionConstants.INSTITUTION_ID] = cmbInstitutions.SelectedValue;
                }
               
                this.Form.DefaultButton = this.btnSave.UniqueID;
                this.Page.SmartNavigation = true;
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - Page_Load", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }

        }

        /// <summary>
        /// Save the new OC Directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int institutionID;
                if (isSystemAdmin)
                    institutionID = int.Parse(cmbInstitutions.SelectedValue);
                else
                    institutionID = instID; //Convert.ToInt32(Session[SessionConstants.INSTITUTION_ID]);

                saveDirectory(institutionID, txtDirectoryName.Text);
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - btnSave_Click", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }
       
        /// <summary>
        /// Edit the directory record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDirectories_EditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                hdnMode.Value = "edit";                
                grdDirectories.EditItemIndex = e.Item.ItemIndex;
                e.Item.Cells[0].Controls[1].Focus();
                populateDirectories();
                setDatagridHeight(false);                
                txtDirectoryName.Text = "";
                e.Item.Cells[0].Controls[1].Focus();
                cmbInstitutions.Enabled = false;
                txtDirectoryName.Enabled = false;
                //grdDirectories.Items[e.Item.ItemIndex].Controls[0].Focus();     
                //grdDirectories.Items[e.Item.ItemIndex].Focus(); 
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - grdDirectories_EditCommand", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Update the edited record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDirectories_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                string msg = string.Empty;
                int intGridEditItemIndex = e.Item.ItemIndex;
                int directoryID = int.Parse(grdDirectories.DataKeys[intGridEditItemIndex].ToString());
                string directoryNameClientID = (grdDirectories.Items[intGridEditItemIndex].FindControl("txtGrdDirectoryName") as TextBox).ClientID;
                string directoryName = hdnGridDirectoryDesc.Value;
                int institutionID;
                if (isSystemAdmin)
                    institutionID = int.Parse(cmbInstitutions.SelectedValue);
                else
                    institutionID = instID; //Convert.ToInt32(Session[SessionConstants.INSTITUTION_ID]);

                if (updateDirectory(institutionID, directoryID, directoryName, directoryNameClientID) && !(hdnGridDirectoryDesc.Value.Trim().Length <= 0))
                {
                    grdDirectories.EditItemIndex = -1;
                    populateDirectories();
                    hdnOCDirectoryDataChanged.Value = "false";
                    hdnMode.Value = "";
                    string sb = "document.getElementById('" + hdnOCDirectoryDataChanged.ClientID + "').value = 'false';";
                    ScriptManager.RegisterClientScriptBlock(UpdatePanelDirectoryList, UpdatePanelDirectoryList.GetType(), "Val", sb, true);
                    //Page.RegisterClientScriptBlock("NavigateToDir", "<script type=\'text/javascript\'>document.getElementById(hdnOCDirectoryDataChangedClientID).value = 'false';</script>");
                }
                cmbInstitutions.Enabled = true;
                txtDirectoryName.Enabled = true;
                setDatagridHeight(false);
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - grdDirectories_UpdateCommand", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Cancel the changes made to edited record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDirectories_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                grdDirectories.EditItemIndex = -1;
                hdnMode.Value = "";
                populateDirectories();
                setDatagridHeight(false);
                hdnOCDirectoryDataChanged.Value = "false";
                cmbInstitutions.Enabled = true;
                txtDirectoryName.Enabled = true;
                string sb = "document.getElementById('" + hdnOCDirectoryDataChanged.ClientID + "').value = 'false';";
                //Page.RegisterClientScriptBlock("NavigateToDir", "<script type=\'text/javascript\'>document.getElementById(hdnOCDirectoryDataChangedClientID).value = 'false';</script>");
                ScriptManager.RegisterClientScriptBlock(UpdatePanelDirectoryList,UpdatePanelDirectoryList.GetType(),"Val",sb,true);
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - grdDirectories_CancelCommand", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Sort the grid content on directory name 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDirectories_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            try
            {
                if (ViewState[Utils.SORT_ORDER].ToString().CompareTo("ASC") == 0)
                    ViewState[Utils.SORT_ORDER] = "DESC";
                else
                    ViewState[Utils.SORT_ORDER] = "ASC";
                grdDirectories.EditItemIndex = -1;
                populateDirectories();
                setDatagridHeight(false);                
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - grdDirectories_SortCommand", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Cancel Editing of the selected record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdDirectories_ItemCreated(object source, DataGridItemEventArgs e)
        {
            try
            {
                string script = "javascript:";
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    if (e.Item.Cells[0].Controls[1] is TextBox)
                    {
                        TextBox txtGridDirectoryName = (e.Item.Cells[0].Controls[1]) as TextBox;
                        txtGridDirectoryName.EnableViewState = true;
                        txtGridDirectoryName.Attributes.Add("onblur", "JavaScript:CheckMaxLenght('" + txtGridDirectoryName.ClientID + "',100);");
                        txtGridDirectoryName.Attributes.Add("onchange", "Javascript:formDataChange('true');");

                        txtGridDirectoryName.Attributes.Add("onkeypress", "Javascript:isAlphaNumericKeyStroke();");
                        txtGridDirectoryName.Attributes.Add("onpaste", "Javascript:isAlphaNumericKeyStroke();");                        

                        LinkButton lbUpdate = (e.Item.Cells[1].Controls[0]) as LinkButton;
                        LinkButton lbCancel = (e.Item.Cells[1].Controls[2]) as LinkButton;
                        if (e.Item.ItemIndex + 2 < 10)
                        {
                            lbUpdate.OnClientClick = script + "return setDirectoryDesc('" + txtGridDirectoryName.ClientID + "','" + (e.Item.ItemIndex + 2) + "');"; 
                            lbCancel.OnClientClick = script + "return onGridCancelClick('" + txtGridDirectoryName.ClientID + "','" + (e.Item.ItemIndex + 2) + "');"; 
                        }
                        else
                        {
                            lbUpdate.OnClientClick = script + "return setDirectoryDesc('" + txtGridDirectoryName.ClientID + "','" + (e.Item.ItemIndex + 2) + "');"; 
                            lbCancel.OnClientClick = script + "return onGridCancelClick('" + txtGridDirectoryName.ClientID + "','" + (e.Item.ItemIndex + 2) + "');";
                        }
                    }
                }
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lbtnEdit = (e.Item.Cells[1].Controls[0]) as LinkButton;
                    script += "if(confirmOnDataChange()){document.getElementById('" + txtDirectoryName.ClientID + "').value = '';";

                    if (e.Item.ItemIndex + 2 < 10)
                    {
                        script += "__doPostBack('ctl00$ContentPlaceHolder1$grdDirectories$ctl0" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                    }
                    else
                    {
                        script += "__doPostBack('ctl00$ContentPlaceHolder1$grdDirectories$ctl" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                    }
                    lbtnEdit.OnClientClick = script + "} else {return false;}";
                }
                if (e.Item.ItemType == ListItemType.Header)
                {
                    if (e.Item.Cells[0].Controls.Count > 0)
                    {
                        LinkButton lbtnDirSort = (e.Item.Cells[0].Controls[0]) as LinkButton;
                        script += "if(confirmOnDataChange()){document.getElementById('" + txtDirectoryName.ClientID + "').value = '';";
                        script += "__doPostBack('ctl00$ContentPlaceHolder1$grdDirectories$ctl01$ctl00', '');";
                        script += "} else {return false;}";
                        lbtnDirSort.OnClientClick = script;
                    }
                }

            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - grdDirectories_ItemCreated", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Load the directories for selected institution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbInstitutions_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                grdDirectories.EditItemIndex = -1;
                ViewState[Utils.SORT_ORDER] = "ASC";
                populateDirectories();
                setDatagridHeight(false);
                btnSave.Enabled = false;
                txtDirectoryName.Text = "";
                hdnInstitutionVal.Value = cmbInstitutions.SelectedValue;
                if (cmbInstitutions.SelectedValue == "-1")
                {
                    trGrid.Visible = false;
                }
                else
                {
                    trGrid.Visible = true;
                    Session[SessionConstants.INSTITUTION_ID] = cmbInstitutions.SelectedValue;
                }
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - cmbInstitutions_OnSelectedIndexChanged", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }

        }
        #endregion Events

        #region Private Methods

        /// <summary>
        /// populate Directories in Grid
        /// </summary>
        private void populateDirectories()
        {
            Directory objDirectory = null;
            DataView dvDirectory = null;
            DataTable dtDirectories = null;
            try
            {
                objDirectory = new Directory();
                int institutionID = -1;
                if (isSystemAdmin)
                    institutionID = int.Parse(cmbInstitutions.SelectedValue);
                else
                    institutionID = instID;//Convert.ToInt32(Session[SessionConstants.INSTITUTION_ID]);
                
                dtDirectories = objDirectory.GetDirectories(institutionID);
                if (dtDirectories.Rows.Count > 1)
                    grdDirectories.AllowSorting = true;
                else
                    grdDirectories.AllowSorting = false;
                dvDirectory = dtDirectories.DefaultView;
                if (ViewState[Utils.SORT_ORDER] == null)
                    ViewState[Utils.SORT_ORDER] = "ASC";
                dvDirectory.Sort = "DirectoryDescription " + ViewState[Utils.SORT_ORDER].ToString();
                grdDirectories.DataSource = dvDirectory;
                grdDirectories.DataBind();
            }
            finally
            {
                objDirectory = null;
                dtDirectories = null;
                dvDirectory = null;
            }
        }

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
            finally
            {
                objInstitution = null;
            }
        }

        /// <summary>
        /// Add New Directory
        /// </summary>
        /// <param name="institutionID"></param>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        private void saveDirectory(int institutionID, string directoryName)
        {
            Directory objDirectory;

            try
            {
                objDirectory = new Directory();
                int returnVal = objDirectory.AddNew(institutionID, directoryName);
                if (returnVal > 0)
                {
                    //ClientScript.RegisterStartupScript(Page.GetType(), "Save_Failed", "document.getElementById(txtDirectoryNameClientID).focus();alert('OC directory name already exists for this institution, please enter another name.');", true);
                    ScriptManager.RegisterStartupScript(UpdatePanelDirectoryList,UpdatePanelDirectoryList.GetType(), "Save_Failed", "document.getElementById(txtDirectoryNameClientID).focus();alert('OC directory name already exists for this institution, please enter another name.');", true);
                }
                else
                {
                    txtDirectoryName.Text = "";
                    btnSave.Enabled = false;
                    txtDirectoryName.Focus();
                    grdDirectories.EditItemIndex = -1;
                    populateDirectories();
                    hdnOCDirectoryDataChanged.Value = "false";
                    //Page.RegisterClientScriptBlock("NavigateToDir", "<script type=\'text/javascript\'>document.getElementById(hdnOCDirectoryDataChangedClientID).value = 'false';</script>");
                    ScriptManager.RegisterClientScriptBlock(UpdatePanelDirectoryList,UpdatePanelDirectoryList.GetType(),"Dir", "<script type=\'text/javascript\'>document.getElementById(hdnOCDirectoryDataChangedClientID).value = 'false';</script>",false);
                }
                setDatagridHeight(false);
                //ClientScript.RegisterStartupScript(this.GetType(), "SetFocusOnTextbox", "document.getElementById(" + '"' + txtDirectoryName.ClientID + '"' + ").focus();", true);
                ScriptManager.RegisterStartupScript(UpdatePanelDirectoryList,UpdatePanelDirectoryList.GetType(), "SetFocusOnTextbox", "document.getElementById(" + '"' + txtDirectoryName.ClientID + '"' + ").focus();", true);
            }
            finally
            {
                objDirectory = null;
            }

        }

        /// <summary>
        /// Update Directory
        /// </summary>
        /// <param name="institutionID"></param>
        /// <param name="directoryID"></param>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        private bool updateDirectory(int institutionID, int directoryID, string directoryName, string directoryTextBoxClientID)
        {
            Directory objDirectory;

            try
            {
                objDirectory = new Directory();
                int returnVal = objDirectory.Update(institutionID, directoryID, directoryName);
                if (returnVal > 0)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "Save_Failed", "alert('OC directory name already exists for this institution, please enter another name.');document.getElementById('" + directoryTextBoxClientID + "').value = '" + hdnGridDirectoryDesc.Value + "';document.getElementById('" + directoryTextBoxClientID + "').focus();", true);
                    return false;
                }
                return true;
            }
            finally
            {
                objDirectory = null;
            }

        }

        /// <summary>
        /// Register JS variables, client side button click events
        /// </summary>
        private void registerJavascriptVariables()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var cmbInstitutionsClientID = '" + cmbInstitutions.ClientID + "';");
            sbScript.Append("var txtDirectoryNameClientID = '" + txtDirectoryName.ClientID + "';");
            sbScript.Append("var hdnInstitutionValClientID = '" + hdnInstitutionVal.ClientID + "';");
            sbScript.Append("var hdnOCDirectoryDataChangedClientID = '" + hdnOCDirectoryDataChanged.ClientID + "';");
            sbScript.Append("var hdnGridDirectoryDescClientID = '" + hdnGridDirectoryDesc.ClientID + "';");
            sbScript.Append("var hdnIsSystemAdminClientID = '" + hdnIsSystemAdmin.ClientID + "';");
            sbScript.Append("var btnSaveClientID = '" + btnSave.ClientID + "';");
            sbScript.Append("var hdnModeClientID = '" + hdnMode.ClientID + "';");

            sbScript.Append("</script>");
            ClientScript.RegisterStartupScript(Page.GetType(), "scriptClientIDs", sbScript.ToString());

            btnSave.Attributes.Add("onclick", "JavaScript:return onSaveClick();");

            cmbInstitutions.Attributes.Add("onchange", "Javascript:return onComboChange();");
            txtDirectoryName.Attributes.Add("onchange", "Javascript:formDataChange('true');isValidDirName();");
            txtDirectoryName.Attributes.Add("onkeypress", "Javascript:validateDirName();");
            txtDirectoryName.Attributes.Add("onpaste", "Javascript:validateDirName();");
            txtDirectoryName.Attributes.Add("onkeyup", "Javascript:isValidDirName();");
            txtDirectoryName.Attributes.Add("onblur", "Javascript:isValidDirName();");
            txtDirectoryName.Attributes.Add("onmouseup", "Javascript:isValidDirName();");
            txtDirectoryName.Attributes.Add("onmousedown", "Javascript:isValidDirName();");
            txtDirectoryName.Attributes.Add("onmouseover", "Javascript:isValidDirName();");
            txtDirectoryName.Attributes.Add("onmouseout", "Javascript:isValidDirName();");
            txtDirectoryName.Attributes.Add("onfocus", "Javascript:isValidDirName();");
        }

        /// <summary>
        /// This method will set the height of datagrid dynamically accordingly the current rowcount of datagrid,
        /// each time when the page posts back. 
        /// </summary>
        private void setDatagridHeight(bool startup)
        {
            //string newUid = this.UniqueID.Replace(":", "_");
            string script = "";//"<script type=\"text/javascript\">";
            script += "if(document.getElementById(" + '"' + "DepartmentDiv" + '"' + ") != null){document.getElementById(" + '"' + "DepartmentDiv" + '"' + ").style.height=setHeightOfGrid('" + grdDirectories.ClientID + "','" + 60 + "');}";
            //if(startup)
            //    Page.RegisterStartupScript(newUid, script);
            //else
            //    Page.RegisterClientScriptBlock(newUid, script);
            if (startup)
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "PC", script, true);
            else
                ScriptManager.RegisterClientScriptBlock(UpdatePanelDirectoryList, UpdatePanelDirectoryList.GetType(), "PC", script, true);

        }

        #endregion Methods

    }
}