#region File History

/******************************File History***************************
 * File Name        : add_nurse_directory.aspx.cs
 * Author           : SSK.
 * Created Date     : 21-07-2007
 * Purpose          : This Class will add new nurse directory for Institution.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 *  26-10-2007 - IAK    - UI Changes /Page navigation etc.
 *  26-10-2007 - IAK    - All Catch Block updated.
 *  26-10-2007 - IAK    - On Edit disable the save button.
 *  30-10-2007 - IAK    - Defects UI Changes Header.
 *  12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 * ------------------------------------------------------------------- 
 */
#endregion

#region Using Block
using System;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.Utilities;
using System.Web.UI;
#endregion

namespace Vocada.CSTools
{
    /// <summary>
    /// Allows user to add / edit nurse directory for the institution
    /// </summary>
    public partial class add_nurse_directory : System.Web.UI.Page
    {
        #region Private Variable
        private bool isSystemAdmin = true;
        private int instID;
        private int userID = 0;
        #endregion

        #region Page Events
        /// <summary>
        /// Loads all the controls with the default values, sets the Session values for the page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
            {
                Response.Redirect(Utils.GetReturnURL("default.aspx", "add_nurse_directory.aspx", this.Page.ClientQueryString));
            }
            Session[SessionConstants.CURRENT_TAB] = MenuTab.TOOLS;
            Session[SessionConstants.CURRENT_INNER_TAB] = MenuInnerTab.ADDNURSEDIR;
            Session[SessionConstants.CURRENT_PAGE] = CSToolsPage.ADDNURSEDIR;
            //Page.SetFocus(btnSave.ClientID);
            //Page.RegisterHiddenField("__EVENTTARGET", btnSave.ClientID);
            Page.SmartNavigation = true;
            try
            {
                UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
                if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
                {
                    isSystemAdmin = false;
                    instID = userInfo.InstitutionID;  
                }
                else
                {
                    isSystemAdmin = true;
                }
                if (!IsPostBack)
                {
                    registerJavaScript();
                    userID = (Session[SessionConstants.USER_ID] != null && Session[SessionConstants.USER_ID].ToString().Length > 0) ? Convert.ToInt32(Session[SessionConstants.USER_ID].ToString()) : 0;

                    Session[SessionConstants.WEEK_NUMBER] = null;
                    Session[SessionConstants.SHOWMESSAGES] = null;
                    Session[SessionConstants.STATUS] = null;
                    Session[SessionConstants.GROUP] = null;

                    ViewState[Utils.SORT_ORDER] = "ASC";
                    if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
                    {
                        isSystemAdmin = false;
                        hdnIsSystemAdmin.Value = "0";
                        cmbInstitution.Visible = false;
                        lblInstName.Visible = true;
                        //lblInstName.Text = " " + Session[SessionConstants.INSTITUTION_NAME].ToString();
                        lblInstName.Text = userInfo.InstitutionName; 
                    }
                    else
                    {
                        isSystemAdmin = true;
                        hdnIsSystemAdmin.Value = "1";
                        cmbInstitution.Visible = true; ;
                        lblInstName.Visible = false;
                        fillInstitution();
                        if (Request["InstitutionID"] != null)
                            cmbInstitution.SelectedValue = (Request["InstitutionID"].ToString()); 

                    }
                    populateNurseDirectories();
                    generateDataGridHeight(true);
                }
                this.Form.DefaultButton = this.btnSave.UniqueID;
               
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_nurse_directory - Page_Load", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Resets the controls, populates the directories for the institution in the Datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtDirectoryName.Text = string.Empty;
                btnSave.Enabled = false;
                txtDirectoryName.Enabled = true;
                grdNurseDirectory.EditItemIndex = -1;
                ViewState[Utils.SORT_ORDER] = "ASC";
                populateNurseDirectories();
                generateDataGridHeight(false);
                hdnInstitutionVal.Value = cmbInstitution.SelectedValue;
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_nurse_directory - cmbInstitution_SelectedIndexChanged", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Sets the EditItemIndex of grdNurseDirectory to -1, load the the grid again.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdNurseDirectory_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                grdNurseDirectory.EditItemIndex = -1;
                txtDirectoryName.Enabled = true;
                
                populateNurseDirectories();
                generateDataGridHeight(false);
                hdnNurseDirectoryDataChanged.Value = "false";
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_nurse_directory - grdNurseDirectory_CancelCommand", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }

        }

        /// <summary>
        /// Set the EditItemIndex of grdNurseDirectory to the selected index, bind the datagrid again, 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdNurseDirectory_EditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                grdNurseDirectory.EditItemIndex = e.Item.ItemIndex;
                populateNurseDirectories();
                generateDataGridHeight(false);
                grdNurseDirectory.Items[e.Item.ItemIndex].Focus();
                txtDirectoryName.Text = "";
                txtDirectoryName.Enabled = false;
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_nurse_directory - grdNurseDirectory_EditCommand", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Reads the value for Direction in viewstate, reverse the order and stores again in viewstate, 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdNurseDirectory_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            try
            {
                grdNurseDirectory.EditItemIndex = -1;
                txtDirectoryName.Enabled = true;
                string sortDirection = string.Empty;
                if (ViewState[Utils.SORT_ORDER] != null)
                    sortDirection = ViewState[Utils.SORT_ORDER].ToString();
                if (sortDirection == "ASC")
                {
                    ViewState[Utils.SORT_ORDER] = "DESC";
                }
                else
                {
                    ViewState[Utils.SORT_ORDER] = "ASC";
                }

                populateNurseDirectories();
                generateDataGridHeight(false);
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_nurse_directory - grdNurseDirectory_SortCommand", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Updates the directory information with the new values, resets the datagrid edit item index to -1 if updated succesfully.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdNurseDirectory_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            Directory objDirectory;
            TextBox dirName = null;
            try
            {
                string msg = string.Empty;
                objDirectory = new Directory();
                int instituteId = -1;
                if (isSystemAdmin)
                    instituteId = Convert.ToInt32(cmbInstitution.SelectedValue);
                else
                    instituteId = instID; //Convert.ToInt32(Session[SessionConstants.INSTITUTION_ID]);

                dirName = ((TextBox)(grdNurseDirectory.Items[e.Item.ItemIndex].Cells[0].FindControl("txtGridDirectoryName")));
                int nurseDirId = Convert.ToInt32(grdNurseDirectory.DataKeys[e.Item.ItemIndex]);
                int result = 0;

                result = objDirectory.UpdateNurseDirectoryForInstitute(instituteId, nurseDirId, hdnGridDirectoryDesc.Value.Trim());
                if (result == -1)
                {
                    msg = "Nurse directory name already exists for this institution, please enter another name.";
                    ScriptManager.RegisterStartupScript(upnlGrid,upnlGrid.GetType(), "UpdateNurseDirectory", "alert('" + msg + "');document.getElementById('" + dirName.ClientID + "').value = '" + hdnGridDirectoryDesc.Value + "';document.getElementById('" + dirName.ClientID + "').focus();", true);
                }

                if (result == 0)
                {
                    grdNurseDirectory.EditItemIndex = -1;
                    hdnNurseDirectoryDataChanged.Value = "false";
                    populateNurseDirectories();
                    generateDataGridHeight(false);
                    txtDirectoryName.Enabled = true;
                }
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_nurse_directory - grdNurseDirectory_UpdateCommand", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
            finally
            {
                objDirectory = null;
                dirName = null;
            }
        }

        /// <summary>
        /// Cancel Editing of the selected record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdNurseDirectory_ItemCreated(object source, DataGridItemEventArgs e)
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
                            lbUpdate.OnClientClick = script + "return setDirectoryDesc('" + txtGridDirectoryName.ClientID + "', '" + (e.Item.ItemIndex + 2) + "');";    //__doPostBack('ctl00$ContentPlaceHolder1$grdNurseDirectory$ctl0" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                            lbCancel.OnClientClick = script + "__doPostBack('ctl00$ContentPlaceHolder1$grdNurseDirectory$ctl0" + (e.Item.ItemIndex + 2) + "$ctl01', '');return false";
                        }
                        else
                        {
                            lbUpdate.OnClientClick = script + "return setDirectoryDesc('" + txtGridDirectoryName.ClientID + "','" + (e.Item.ItemIndex + 2) + "');";    //__doPostBack('ctl00$ContentPlaceHolder1$grdNurseDirectory$ctl" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                            lbCancel.OnClientClick = script + "__doPostBack('ctl00$ContentPlaceHolder1$grdNurseDirectory$ctl" + (e.Item.ItemIndex + 2) + "$ctl01', '');return false";
                        }
                    }
                }
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lbtnEdit = (e.Item.Cells[1].Controls[0]) as LinkButton;
                    script += "if(confirmOnDataChange()){document.getElementById('" + txtDirectoryName.ClientID + "').value = '';";

                    if (e.Item.ItemIndex + 2 < 10)
                    {
                        script += "__doPostBack('ctl00$ContentPlaceHolder1$grdNurseDirectory$ctl0" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                    }
                    else
                    {
                        script += "__doPostBack('ctl00$ContentPlaceHolder1$grdNurseDirectory$ctl" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                    }
                    lbtnEdit.OnClientClick = script + "} else {return false;}";
                }
                if (e.Item.ItemType == ListItemType.Header)
                {
                    if (e.Item.Cells[0].Controls.Count > 0)
                    {
                        LinkButton lbtnDirSort = (e.Item.Cells[0].Controls[0]) as LinkButton;
                        script += "if(confirmOnDataChange()){document.getElementById('" + txtDirectoryName.ClientID + "').value = '';";
                        script += "__doPostBack('ctl00$ContentPlaceHolder1$grdNurseDirectory$ctl01$ctl00', '');";
                        script += "} else {return false;}";
                        lbtnDirSort.OnClientClick = script;
                    }
                }

            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_nurse_directory - grdNurseDirectory_ItemCreated", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        ///  Adds the new nurse directoy with the given values for the selected institution, shows success message popup to user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int instituteId = -1;
            if (isSystemAdmin)
                instituteId = Convert.ToInt32(cmbInstitution.SelectedValue);
            else
                instituteId = instID;//Convert.ToInt32(Session[SessionConstants.INSTITUTION_ID]);
            
            int result = -1;
            string msg = string.Empty;
            Directory objDirectory = new Directory();
            try
            {
                result = objDirectory.InsertNurseDirectoryForInstitute(instituteId, txtDirectoryName.Text.Trim());
                if (result == -1)
                {
                    msg = "Nurse directory name already exists for this institution, please enter another name.";
                    ScriptManager.RegisterStartupScript(InstitutionList,InstitutionList.GetType(), "AddNurseDirectory", "document.getElementById(" + '"' + txtDirectoryName.ClientID + '"' + ").focus();alert('" + msg + "');", true);
                }
                else
                {
                    hdnNurseDirectoryDataChanged.Value = "false";
                    grdNurseDirectory.EditItemIndex = -1;
                    populateNurseDirectories();
                    txtDirectoryName.Text = string.Empty;
                    txtDirectoryName.Focus();
                }
                generateDataGridHeight(false);
                ScriptManager.RegisterStartupScript(InstitutionList, InstitutionList.GetType(), "SetFocusOnTextbox", "document.getElementById(" + '"' + txtDirectoryName.ClientID + '"' + ").focus();", true);
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_nurse_directory - btnSave_Click", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
            finally
            {
                objDirectory = null;
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Fetches all the institution from database and fills it in the Select Institution dropdown.
        /// </summary>
        private void fillInstitution()
        {
            DataTable dtInstitution = null;
            ListItem listItem = null;
            try
            {
                dtInstitution = new DataTable();
                dtInstitution = Utility.GetInstitutionList();
                cmbInstitution.DataSource = dtInstitution;
                cmbInstitution.DataBind();

                listItem = new ListItem("-- Select Institution --", "-1");
                cmbInstitution.Items.Add(listItem);
                cmbInstitution.Items.FindByValue("-1").Selected = true;
            }
            finally
            {
                dtInstitution = null;
                listItem = null;
            }

        }

        /// <summary>
        /// Pupulates all the nurse directories for the institution in the Datagrid.
        /// </summary>
        /// <param name="instituteId"></param>
        private void populateNurseDirectories()
        {
            Directory objDirectory = null;
            DataTable dtNurseDirectory = null;
            DataView dvNurseDirectory = null;
            try
            {
                objDirectory = new Directory();
                dtNurseDirectory = new DataTable();
                int institutionID = -1;
                if (isSystemAdmin)
                    institutionID = Convert.ToInt32(cmbInstitution.SelectedValue);
                else
                    institutionID = instID;//Convert.ToInt32(Session[SessionConstants.INSTITUTION_ID]);

                dtNurseDirectory = objDirectory.GetNurseDirectories(institutionID);
                if (dtNurseDirectory != null)
                {
                    if (dtNurseDirectory.Rows.Count > 1)
                        grdNurseDirectory.AllowSorting = true;
                    else
                        grdNurseDirectory.AllowSorting = false;
                    //Session["NurseDir"] = dtNurseDirectory;
                    dvNurseDirectory = dtNurseDirectory.DefaultView;
                    if (ViewState[Utils.SORT_ORDER] == null)
                        ViewState[Utils.SORT_ORDER] = "ASC";
                    dvNurseDirectory.Sort = "DirectoryDescription " + ViewState[Utils.SORT_ORDER].ToString();
                    grdNurseDirectory.DataSource = dvNurseDirectory;
                    grdNurseDirectory.DataBind();
                }
            }
            finally
            {
                objDirectory = null;
                dtNurseDirectory = null;
                dvNurseDirectory = null;
            }
        }

        /// <summary>
        /// This function is to set dynamic height of datagrid
        /// </summary>
        private void generateDataGridHeight(bool isStartup)
        {
            string script = "<script type=\"text/javascript\">";
            script += "if(document.getElementById(" + '"' + "AddNurseDirDiv" + '"' + ") != null){document.getElementById(" + '"' + "AddNurseDirDiv" + '"' + ").style.height=setHeightOfGrid('" + grdNurseDirectory.ClientID + "','" + 60 + "');}</script>";
            if (isStartup)
                ScriptManager.RegisterStartupScript(upnlGrid, upnlGrid.GetType(), "SetHeight", script, false);
            else
                ScriptManager.RegisterClientScriptBlock(upnlGrid, upnlGrid.GetType(), "SetHeight", script, false);
        }

        /// <summary>
        /// Register JS Variables
        /// </summary>
        private void registerJavaScript()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var cmbInstitutionClientID = '" + cmbInstitution.ClientID + "';");
            sbScript.Append("var hdnInstitutionValClientID = '" + hdnInstitutionVal.ClientID + "';");
            sbScript.Append("var hdnNurseDirectoryDataChangedClientID = '" + hdnNurseDirectoryDataChanged.ClientID + "';");
            sbScript.Append("var hdnGridDirectoryDescClientID = '" + hdnGridDirectoryDesc.ClientID + "';");
            sbScript.Append("var hdnIsSystemAdminClientID = '" + hdnIsSystemAdmin.ClientID + "';");
            sbScript.Append("var txtDirectoryNameClientID = '" + txtDirectoryName.ClientID + "';");
            sbScript.Append("var btnSaveClientID = '" + btnSave.ClientID + "';");
            sbScript.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());

            cmbInstitution.Attributes.Add("onchange", "Javascript:return onComboChange();");
            txtDirectoryName.Attributes.Add("onchange", "Javascript:formDataChange('true');");
            txtDirectoryName.Attributes.Add("onkeypress", "Javascript:isAlphaNumericKeyStroke();");
            txtDirectoryName.Attributes.Add("onpaste", "Javascript:isAlphaNumericKeyStroke();");
            txtDirectoryName.Attributes.Add("onkeyup", "Javascript:isValidDirName();");
                                          
        }
        #endregion

    }
}
