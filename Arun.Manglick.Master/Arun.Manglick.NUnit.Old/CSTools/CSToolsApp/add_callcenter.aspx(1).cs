#region File History

/******************************File History***************************
 * File Name        : add_callcenter.aspx.cs
 * Author           : Suhas Tarihalkar.
 * Created Date     : 30 May 07
 * Purpose          : This class is responsible for add/edit new 
 *                    call center Name for selected institution
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * * Date(dd-mm-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 *  12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 *  17 Nov 2008 - Prerak - CR -> "Back to Call center List" link on call center setup page 
 *                         navigate to to add Call center page and populate call center list.  
 *  19 Nov 2008 - Sheetal- Changed Call center to Agent team
 * ------------------------------------------------------------------- 
 */
#endregion

#region Using Block
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.Utilities;
#endregion

namespace Vocada.CSTools
{
    /// <summary>
    /// Class handles the control events 
    /// and responsible for add/edit call centers 
    /// </summary>
    public partial class add_callcenter : System.Web.UI.Page
    {
        #region Private Variable
        /// <summary>
        /// Logged in User ID
        /// </summary>
        private int userID = 0;
        #endregion

        #region Page Events

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
                Response.Redirect(Utils.GetReturnURL("default.aspx", "add_callcenter.aspx", this.Page.ClientQueryString));
            userID = (Session[SessionConstants.USER_ID] != null && Session[SessionConstants.USER_ID].ToString().Length > 0) ? Convert.ToInt32(Session[SessionConstants.USER_ID].ToString()) : 0;
            try
            {
                Session[SessionConstants.CURRENT_PAGE] = "add_callcenter.aspx";
                Session[SessionConstants.CURRENT_TAB] = "CallCenter";
                Session[SessionConstants.CURRENT_INNER_TAB] = "AddCallCenter";
                registerJavascriptVariables();
                if (!Page.IsPostBack)
                {
                    //registerJavascriptVariables();
                    populateInstitutions();
                    if (Request["InstitutionID"] != null)
                        cmbInstitutions.SelectedValue = Request["InstitutionID"].ToString();
                    populateCallCenters();
                    if (Session["Direction"] == null)
                        Session["Direction"] = " ASC";
                }
                setDatagridHeight(false);
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_callcenter - Page_Load", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Handles the OnSelectedIndexChanged event of the cmbInstitutions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void cmbInstitutions_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                grdCallCenters.EditItemIndex = -1;
                btnSave.Enabled = false;
                txtCallCenterName.Text = "";
                populateCallCenters();
                setDatagridHeight(false);
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_callcenter - cmbInstitutions_OnSelectedIndexChanged", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int institutionID;
                institutionID = Convert.ToInt32(cmbInstitutions.SelectedValue);
                saveCallCenter(institutionID, txtCallCenterName.Text);
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_callcenter - btnSave_Click", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Handles the ItemCreated event of the grdCallCenters control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void grdCallCenters_ItemCreated(object source, DataGridItemEventArgs e)
        {
            try
            {
                string script = "javascript:";
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    if (e.Item.Cells[0].Controls[1] is TextBox)
                    {
                        TextBox txtCallCenterName = (e.Item.Cells[0].Controls[1]) as TextBox;
                        txtCallCenterName.EnableViewState = true;
                        txtCallCenterName.Attributes.Add("onchange", "Javascript:formDataChange('true');");

                        LinkButton lbUpdate = (e.Item.Cells[2].Controls[0]) as LinkButton;
                        LinkButton lbCancel = (e.Item.Cells[2].Controls[2]) as LinkButton;
                        if (e.Item.ItemIndex + 2 < 10)
                        {
                            lbUpdate.OnClientClick = script + "return setCallCenterName('" + txtCallCenterName.ClientID + "','" + (e.Item.ItemIndex + 2) + "');"; //__doPostBack('ctl00$ContentPlaceHolder1$grdDirectories$ctl0" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                            lbCancel.OnClientClick = script + "return onGridCancelClick('" + txtCallCenterName.ClientID + "','" + (e.Item.ItemIndex + 2) + "');"; //__doPostBack('ctl00$ContentPlaceHolder1$grdDirectories$ctl0" + (e.Item.ItemIndex + 2) + "$ctl01', '');";
                        }
                        else
                        {
                            lbUpdate.OnClientClick = script + "return setCallCenterName('" + txtCallCenterName.ClientID + "','" + (e.Item.ItemIndex + 2) + "');"; //__doPostBack('ctl00$ContentPlaceHolder1$grdDirectories$ctl" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                            lbCancel.OnClientClick = script + "return onGridCancelClick('" + txtCallCenterName.ClientID + "','" + (e.Item.ItemIndex + 2) + "');"; //"__doPostBack('ctl00$ContentPlaceHolder1$grdDirectories$ctl" + (e.Item.ItemIndex + 2) + "$ctl01', '');";
                        }
                    }
                }
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lbtnEdit = (e.Item.Cells[2].Controls[0]) as LinkButton;
                    script += "if(confirmOnDataChange()){document.getElementById('" + txtCallCenterName.ClientID + "').value = '';";

                    if (e.Item.ItemIndex + 2 < 10)
                    {
                        script += "__doPostBack('ctl00$ContentPlaceHolder1$grdCallCenters$ctl0" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                    }
                    else
                    {
                        script += "__doPostBack('ctl00$ContentPlaceHolder1$grdCallCenters$ctl" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                    }
                    lbtnEdit.OnClientClick = script + "} else {return false;}";
                }
                if (e.Item.ItemType == ListItemType.Header)
                {
                    if (e.Item.Cells[0].Controls.Count > 0)
                    {
                        LinkButton lbtnDirSort = (e.Item.Cells[0].Controls[0]) as LinkButton;
                        script += "if(confirmOnDataChange()){document.getElementById('" + txtCallCenterName.ClientID + "').value = '';";
                        script += "__doPostBack('ctl00$ContentPlaceHolder1$grdCallCenters$ctl01$ctl00', '');";
                        script += "} else {return false;}";
                        lbtnDirSort.OnClientClick = script;
                    }
                }
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_callcenter - grdCallCenters_ItemCreated", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Handles the EditCommand event of the grdCallCenters control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void grdCallCenters_EditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                grdCallCenters.EditItemIndex = e.Item.ItemIndex;
                populateCallCenters();
                setDatagridHeight(false);
                txtCallCenterName.Text = "";
                e.Item.Cells[0].Controls[1].Focus();
                cmbInstitutions.Enabled = false;
                txtCallCenterName.Enabled = false;
                pnlTabName.Enabled = false;
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_callcenter - grdCallCenters_EditCommand", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Handles the CancelCommand event of the grdCallCenters control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void grdCallCenters_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                grdCallCenters.EditItemIndex = -1;
                populateCallCenters();
                setDatagridHeight(false);
                cmbInstitutions.Enabled = true;
                txtCallCenterName.Enabled = true;
                pnlTabName.Enabled = true;
                //hdnCallCenterDataChanged.Value = "false";
                //ScriptManager.RegisterClientScriptBlock(upnlGrid,upnlGrid.GetType(), "ChangeTextValue", "<script type=\'text/javascript\'>document.getElementById('" + hdnCallCenterDataChanged.ClientID  + "').value = 'false';</script>",false);
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_callcenter - grdCallCenters_CancelCommand", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }
        
        /// <summary>
        /// Handles the UpdateCommand event of the grdCallCenters control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void grdCallCenters_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int institutionID;
            try
            {
                int intGridEditItemIndex = e.Item.ItemIndex;
                int callCenterID = Convert.ToInt16(grdCallCenters.DataKeys[intGridEditItemIndex].ToString());
                string callCenterNameClientID = (grdCallCenters.Items[intGridEditItemIndex].FindControl("txtGDCallCenterName") as TextBox).ClientID;
                string callCenterName = hdnGridCCDesc.Value;
                institutionID = Convert.ToInt16(cmbInstitutions.SelectedValue);
                if (updateDirectory(institutionID, callCenterID, callCenterName, callCenterNameClientID))
                {
                    grdCallCenters.EditItemIndex = -1;
                    populateCallCenters();
                    ScriptManager.RegisterClientScriptBlock(upnlGrid,upnlGrid.GetType(), "ChangeTextValue", "<script type=\'text/javascript\'>document.getElementById('" + hdnCallCenterDataChanged.ClientID + "').value = 'false';</script>",false);
                }
                cmbInstitutions.Enabled = true;
                txtCallCenterName.Enabled = true;
                pnlTabName.Enabled = true;
                setDatagridHeight(false);
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_callcenter - grdCallCenters_UpdateCommand", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;
            }
        }

        /// <summary>
        /// Handles the SortCommand event of the grdCallCenters control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void grdCallCenters_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            DataView dvSortedCallCenterInfo = null;
            AgentCallCenter objAgentCallCenter = null;
            int institutionID;
            try
            {
                institutionID = int.Parse(cmbInstitutions.SelectedValue);
                dvSortedCallCenterInfo = new DataView();
                objAgentCallCenter = new AgentCallCenter();
                if (Session["Direction"] != null)
                {
                    if (Session["Direction"].ToString().Equals(" ASC"))
                        Session["Direction"] = " DESC";
                    else
                        Session["Direction"] = " ASC";

                    if (Session["ColumnName"] != null)
                        if (!Session["ColumnName"].ToString().Equals(e.SortExpression))
                            Session["Direction"] = " ASC";
                }
                else
                    Session["Direction"] = " ASC";

                Session["ColumnName"] = e.SortExpression;

                dvSortedCallCenterInfo = objAgentCallCenter.GetCallCenters(institutionID).DefaultView;
                dvSortedCallCenterInfo.Sort = Session["ColumnName"] + Session["Direction"].ToString();

                grdCallCenters.DataSource = dvSortedCallCenterInfo;
                grdCallCenters.DataBind();
                setDatagridHeight(false);
            }
            catch (Exception objException)
            {
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_callcenter - grdCallCenters_SortCommand", userID.ToString(), objException.Message, objException.StackTrace), userID);
                throw objException;

            }
            finally
            {
                dvSortedCallCenterInfo = null;
                objAgentCallCenter = null;
            }
        }

        #endregion

        #region Private Variable


        /// <summary>
        /// Populates the institutions.
        /// </summary>
        private void populateInstitutions()
        {
            AgentCallCenter objAgentCallCenter = null;

            try
            {
                objAgentCallCenter = new AgentCallCenter();
                cmbInstitutions.DataSource = objAgentCallCenter.GetInstitutionList();
                cmbInstitutions.DataTextField = "InstitutionName";
                cmbInstitutions.DataValueField = "InstitutionID";
                cmbInstitutions.DataBind();
                cmbInstitutions.Items.Insert(0, new ListItem("-- Select Institution --", "-1"));
            }
            finally
            {
                objAgentCallCenter = null;
            }
        }

        /// <summary>
        /// Populates the call centers.
        /// </summary>
        private void populateCallCenters()
        {
            AgentCallCenter objAgentCallCenter = null;
            DataView dvCallCenters = null;
            DataTable dtCallCenters = null;
            try
            {
                objAgentCallCenter = new AgentCallCenter();
                int institutionID = -1;

                institutionID = int.Parse(cmbInstitutions.SelectedValue);

                dtCallCenters = objAgentCallCenter.GetCallCenters(institutionID);
                if (dtCallCenters.Rows.Count > 1)
                    grdCallCenters.AllowSorting = true;
                else
                    grdCallCenters.AllowSorting = false;
                dvCallCenters = dtCallCenters.DefaultView;
                grdCallCenters.DataSource = dvCallCenters;
                grdCallCenters.DataBind();
            }
            finally
            {
                objAgentCallCenter = null;
                dtCallCenters = null;
                dvCallCenters = null;
            }
        }

        /// <summary>
        /// Saves the call center.
        /// </summary>
        /// <param name="institutionID">The institution ID.</param>
        /// <param name="callCenterName">Name of the call center.</param>
        private void saveCallCenter(int institutionID, string callCenterName)
        {
            AgentCallCenter objAgentCallCenter;
            try
            {
                objAgentCallCenter = new AgentCallCenter();
                int returnVal = objAgentCallCenter.AddNewCallCenterName(institutionID, callCenterName);
                if (returnVal > 0)
                {
                    ScriptManager.RegisterStartupScript(upnlCallCenter, upnlCallCenter.GetType(), "SaveFailed", "document.getElementById(txtCallCenterNameClientID).focus();alert('Agent Team name already exists for this institution, please enter another name.');", true);
                }
                else
                {
                    txtCallCenterName.Text = "";
                    btnSave.Enabled = false;
                    txtCallCenterName.Focus();
                    grdCallCenters.EditItemIndex = -1;
                    populateCallCenters();
                    ScriptManager.RegisterClientScriptBlock(upnlCallCenter,upnlCallCenter.GetType(),"ChangeTextValue", "<script type=\'text/javascript\'>document.getElementById('" + hdnCallCenterDataChanged.ClientID + "').value = 'false';</script>",false);
                }

                setDatagridHeight(false);
                ClientScript.RegisterStartupScript(this.GetType(), "SetFocusOnTextbox", "document.getElementById(" + '"' + txtCallCenterName.ClientID + '"' + ").focus();", true);
            }
            finally
            {
                objAgentCallCenter = null;
            }
        }

        /// <summary>
        /// Updates the directory.
        /// </summary>
        /// <param name="institutionID">The institution ID.</param>
        /// <param name="callCenterID">The call center ID.</param>
        /// <param name="callCenterName">Name of the call center.</param>
        /// <param name="callCenterTextBoxClientID">The call center text box client ID.</param>
        /// <returns></returns>
        private bool updateDirectory(int institutionID, int callCenterID, string callCenterName, string callCenterTextBoxClientID)
        {
            AgentCallCenter objAgentCallCenter;
            try
            {
                objAgentCallCenter = new AgentCallCenter();
                int returnVal = objAgentCallCenter.UpdateCallCenterName(institutionID, callCenterID, callCenterName);
                if (returnVal > 0)
                {
                    ScriptManager.RegisterStartupScript(upnlGrid,upnlGrid.GetType(), "Save_Failed", "alert('Agent Team directory name already exists for this institution, please enter another name.');document.getElementById('" + callCenterTextBoxClientID + "').focus();", true);
                    return false;
                }
                return true;
            }
            finally
            {
                objAgentCallCenter = null;
            }
        }

        /// <summary>
        /// Registers the javascript variables.
        /// </summary>
        private void registerJavascriptVariables()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var cmbInstitutionsClientID = '" + cmbInstitutions.ClientID + "';");
            sbScript.Append("var hdnCallCenterDataChangedClientID = '" + hdnCallCenterDataChanged.ClientID + "';");
            sbScript.Append("var hdnGridCCDescClientID = '" + hdnGridCCDesc.ClientID + "';");
            sbScript.Append("var hdnInstitutionValClientID = '" + hdnInstitutionVal.ClientID + "';");
            sbScript.Append("var btnSaveClientID = '" + btnSave.ClientID + "';");
            sbScript.Append("var txtCallCenterNameClientID = '" + txtCallCenterName.ClientID + "';");

            sbScript.Append("</script>");
            ClientScript.RegisterStartupScript(Page.GetType(), "scriptClientIDs", sbScript.ToString());

            btnSave.Attributes.Add("onclick", "JavaScript:return onSaveClick();");
            cmbInstitutions.Attributes.Add("onchange", "Javascript:return onComboChange();");
            txtCallCenterName.Attributes.Add("onchange", "Javascript:formDataChange('true');isValidCallCenterName();");
            txtCallCenterName.Attributes.Add("onkeypress", "Javascript:isValidCallCenterName();");
            txtCallCenterName.Attributes.Add("onpaste", "Javascript:isValidCallCenterName();");
            txtCallCenterName.Attributes.Add("onkeyup", "Javascript:isValidCallCenterName();");
            txtCallCenterName.Attributes.Add("onblur", "Javascript:isValidCallCenterName();");
            txtCallCenterName.Attributes.Add("onmouseup", "Javascript:isValidCallCenterName();");
            txtCallCenterName.Attributes.Add("onmousedown", "Javascript:isValidCallCenterName();");
            txtCallCenterName.Attributes.Add("onmouseover", "Javascript:isValidCallCenterName();");
            txtCallCenterName.Attributes.Add("onmouseout", "Javascript:isValidCallCenterName();");
            txtCallCenterName.Attributes.Add("onfocus", "Javascript:isValidCallCenterName();");

        }

        /// <summary>
        /// Sets the height of the datagrid.
        /// </summary>
        /// <param name="startup">if set to <c>true</c> [startup].</param>
        private void setDatagridHeight(bool startup)
        {
            string newUid = this.UniqueID.Replace(":", "_");
            string script = "<script type=\"text/javascript\">";
            script += "if(document.getElementById(" + '"' + "CallCenterDiv" + '"' + ") != null){document.getElementById(" + '"' + "CallCenterDiv" + '"' + ").style.height=setHeightOfGrid('" + grdCallCenters.ClientID + "','" + 60 + "');}</script>";
            ScriptManager.RegisterStartupScript(upnlGrid,upnlGrid.GetType(), newUid, script,false);
                    }
        #endregion
    }
}