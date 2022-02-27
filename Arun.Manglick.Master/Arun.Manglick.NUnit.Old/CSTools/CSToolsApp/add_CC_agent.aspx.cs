#region File History

/******************************File History***************************
 * File Name        : add_CC_agent.aspx.cs
 * Author           : Suhas Tarihalkar
 * Created Date     :
 * Date             : 10-June-2008
 * Purpose          : 
 *                  : 
 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification
 * -------------------------------------------------------------------
 * 12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 * 23 Sept 2008  SSK      Add institutionid for Agent
 * * 11-03-2008   RG          Defect #4080 - Fixed
 * 11-13-2008   Suhas       Remove Agent functionality
 * 11-24-2008   RajuG       Performance Improvement - for Edit Command - #4158
 * ------------------------------------------------------------------- 
 *                          
 */
#endregion

#region Using Block
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;

using Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using Vocada.CSTools;
using Vocada.VoiceLink.Utilities;
#endregion

/// <summary>
/// Class is responsible adding and editing Agents
/// </summary>
public partial class add_agent : System.Web.UI.Page
{
    #region Private Members
    /// <summary>
    /// VOC User ID
    /// </summary>
    private const string AGENTID = "CallCenterAgentID";
    /// <summary>
    /// Color code
    /// </summary>
    private const string COLORCODE = "#ffffcc";
    /// <summary>
    /// Current selection in the Datagrid
    /// </summary>
    private const string SELECTED_GRID_ITEM = "SelectedGridItem";

    private int callCenterID = 0;
    private string callCenterName = "";
    private int institutionID = 0;
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
            Response.Redirect(Utils.GetReturnURL("default.aspx", "add_CC_agent.aspx", this.Page.ClientQueryString));

        try
        {
            Session[SessionConstants.CURRENT_PAGE] = "add_CC_agent.aspx";
            Session[SessionConstants.CURRENT_TAB] = "CallCenter";
            Session[SessionConstants.CURRENT_INNER_TAB] = "AddAgent";

            callCenterID = Convert.ToInt32(Request["CallCenterID"].ToString());
            callCenterName = Request["CallCenterName"].ToString();
            institutionID = Convert.ToInt32(Request["InstitutionID"].ToString());
            lblManageUser.Text = callCenterName + " : Current Agents";
            hlinkCallCenterSetup.NavigateUrl = "callCenter_setup.aspx?CallCenterID=" + callCenterID + "&CallCenterName=" + callCenterName + "&InstitutionID=" + institutionID;

            if (!Page.IsPostBack)
            {
                registerJavascriptVariables();
                //Fill Current User Information in the grid.
                fillAgentInfo();
                //Fill all available roles in the dropdown.
                loadRoles();
                //populate groups
                populateGroups();
                //Clears all the controls
                clearAllFields();

                
            }
            generateDataGridHeight();
            
        }
        catch (Exception ex)
        {
            Tracer.GetLogger().LogInfoEvent("add_CC_agent.Page_Load():: Exception occured for User ID - 0 As -->" + ex.Message + " " + ex.StackTrace, 0);
            throw;
        }
    }

    /// <summary>
    /// Handles the OnItemCreated event of the grdUsers control.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="e">The instance containing the event data.</param>
    protected void grdUsers_OnItemCreated(object source, DataGridItemEventArgs e)
    {
        string editScript = "javascript:";
        LinkButton lbtnEdit = null;
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                lbtnEdit = (e.Item.Cells[5].Controls[0]) as LinkButton;
                editScript += "if(onTestdataChanged()){";

                if (e.Item.ItemIndex + 2 < 10)
                {
                    editScript += "return true;";
                }
                else
                {
                    editScript += "return true;";
                }
                lbtnEdit.OnClientClick = editScript + "} else {return false;}";
            }
            else if (e.Item.ItemType == ListItemType.Header && e.Item.Cells[1].Controls.Count > 0)
            {
                ((e.Item.Cells[0].Controls[0]) as LinkButton).OnClientClick += "javascript:if(onTestdataChanged()){return true;}else{return false;}";
                ((e.Item.Cells[1].Controls[0]) as LinkButton).OnClientClick += "javascript:if(onTestdataChanged()){return true;}else{return false;}";
            }
        }
        catch (Exception ex)
        {
            Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grdUsers_OnItemCreated", "0", ex.Message, ex.StackTrace), 0);
            throw ex;
        }
        finally
        {
            lbtnEdit = null;
        }
    }

    /// <summary>
    /// Handles the DeleteCommand event of the grdUsers control.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="e">The instance containing the event data.</param>
    protected void grdUsers_DeleteCommand(object source, DataGridCommandEventArgs e)
    {
        generateDataGridHeight();
        clearAllFields();

        AgentCallCenter objAgentCallCenter = null;
        try
        {
            grdUsers.EditItemIndex = -1;
            objAgentCallCenter = new AgentCallCenter();
            int vocUserID = Convert.ToInt16(e.Item.Cells[8].Text);
            if (objAgentCallCenter.RemoveAgent(vocUserID))
                ScriptManager.RegisterClientScriptBlock(upnlGrdUsers, upnlGrdUsers.GetType(), "RemovalSuceessful", "alert('Agent Removed successfully.')", true);
            else
                ScriptManager.RegisterClientScriptBlock(upnlGrdUsers, upnlGrdUsers.GetType(), "RemovalSuceessful", "alert('Agent already logged in to Veriphy System.')", true);                
        }
        catch (Exception ex)
        {
            Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grdUsers_DeleteCommand", "0", ex.Message, ex.StackTrace), 0);
            throw ex;
        }
        finally
        {
            fillAgentInfo();
            objAgentCallCenter = null;
        }

    }

    /// <summary>
    /// Handles the ItemDataBound event of the grdUsers control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The instance containing the event data.</param>
    protected void grdUsers_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView data = e.Item.DataItem as DataRowView;
            // Reference the Delete button
            ((LinkButton)(e.Item.FindControl("lbtnDelete"))).OnClientClick = "if(onTestdataChanged()){return confirm('Are you sure you want to remove agent?');}";
            e.Item.Cells[3].Text = Utils.expandPhoneNumber(data["Phone"].ToString().Trim());
        }

        

    }

    /// <summary>
    /// Handles the EditCommand event of the grdUsers control.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="e">The instance containing the event data.</param>
    protected void grdUsers_EditCommand(object source, DataGridCommandEventArgs e)
    {
        AgentCallCenter objAgentCallCenter = null;
        try
        {
            //generateDataGridHeight();
            //clearAllFields();
            
            txtFirstName.Text = e.Item.Cells[11].Text.Trim();
            txtLastName.Text = e.Item.Cells[12].Text.Trim();
            txtEmail.Text = e.Item.Cells[2].Text.Trim() != "&nbsp;" ? e.Item.Cells[2].Text.Trim() : "";
            txtLoginID.Text = e.Item.Cells[9].Text.Trim();
            string additionalPhone = Utils.flattenPhoneNumber(e.Item.Cells[3].Text.Trim());
            int userID = Convert.ToInt32(e.Item.Cells[8].Text.Trim());
            string cellPhone = Utils.flattenPhoneNumber(e.Item.Cells[13].Text.Trim());
            if (additionalPhone.Length == 10)  // only if we have a valid 10 digit phone number stored..
            {
                txtPhoneAreaCode.Text = additionalPhone.Substring(0, 3);
                txtPhonePrefix.Text = additionalPhone.Substring(3, 3);
                txtPhoneNNNN.Text = additionalPhone.Substring(6, 4);
            }
            else
            {
                txtPhoneAreaCode.Text = "";
                txtPhonePrefix.Text = "";
                txtPhoneNNNN.Text = "";
            }

            if (cellPhone.Length == 10)  // only if we have a valid 10 digit phone number stored..
            {
                txtmobileArea.Text = cellPhone.Substring(0, 3);
                txtprefix.Text = cellPhone.Substring(3, 3);
                txtMobileNNNN.Text = cellPhone.Substring(6, 4);
            }
            else
            {
                txtmobileArea.Text = "";
                txtprefix.Text = "";
                txtMobileNNNN.Text = "";
            }
            if ((e.Item.Cells[4].Text.Trim()).Equals("Active"))
            {
                chkActive.Checked = true;
            }
            else
            {
                chkActive.Checked = false;
            }

            cmbRoles.SelectedValue = e.Item.Cells[7].Text.Trim();
            txtPassword.Text = e.Item.Cells[10].Text.Trim();
            ViewState[AGENTID] = e.Item.Cells[8].Text.Trim();
            populateGroups();
            btnAdd.Text = " Save ";
            objAgentCallCenter = new AgentCallCenter();
            if (objAgentCallCenter.CheckUserLogin(userID))
            {
                btnAdd.Enabled = false;
                lblLoginUserMessage.Visible = true;
            }
            else
            {
                btnAdd.Enabled = true;
                lblLoginUserMessage.Visible = false;
            }

            e.Item.BackColor = Color.FromName(COLORCODE);
            //Set the focus on the selected item on the grid
            ViewState[SELECTED_GRID_ITEM] = e.Item.ItemIndex;
            grdUsers.Items[e.Item.ItemIndex].Focus();
            upnlAddEdit.Update();
        }
        catch (Exception ex)
        {
            Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grdUsers_EditCommand", "0", ex.Message, ex.StackTrace), 0);
            throw ex;
        }
    }

    /// <summary>
    /// Grid rows are sorted in Ascending/Descending order accordingly for the Sort Expression 
    /// belonging to the Column Header after selecting them.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void grdUsers_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        DataView dvSortedUserInfo = null;
        AgentCallCenter objUser = null;
        try
        {
            dvSortedUserInfo = new DataView();
            objUser = new AgentCallCenter();
            clearAllFields();
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

            dvSortedUserInfo = objUser.GetAgentInformation(callCenterID).DefaultView;
            dvSortedUserInfo.Sort = Session["ColumnName"] + Session["Direction"].ToString();

            grdUsers.DataSource = dvSortedUserInfo;
            grdUsers.DataBind();
            generateDataGridHeight();
        }
        catch (Exception ex)
        {
            Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grdUsers_SortCommand", "0", ex.Message, ex.StackTrace), 0);
            throw ex;

        }
        finally
        {
            dvSortedUserInfo = null;
            objUser = null;
        }
    }

    /// <summary>
    /// This function is to generate unique PIN randomly for Agent.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGeneratePassword_Click(object sender, System.EventArgs e)
    {
        AgentCallCenter objAgentCallCenter = null;
        try
        {
            objAgentCallCenter = new AgentCallCenter();
            txtPassword.Text = objAgentCallCenter.GetNewPin();
            hdnTestDataChanged.Value = "true";
            generateDataGridHeight();
            // Keep the same Row selected 
            if (ViewState[SELECTED_GRID_ITEM] != null)
            {
                int itemIndex = Convert.ToInt32(ViewState[SELECTED_GRID_ITEM].ToString());
                grdUsers.Items[itemIndex].BackColor = Color.FromName(COLORCODE);
                grdUsers.Items[itemIndex].Focus();
            }
        }
        catch (Exception ex)
        {
            Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("AddAgent.btnGeneratePassword_Click:Exception occured for User ID - ", "0", ex.Message, ex.StackTrace), 0);
            ScriptManager.RegisterClientScriptBlock(upnlGrdUsers, upnlGrdUsers.GetType(), "Warning", "<script type=\"text/javascript\">alert('Error Generating Password.');</script>", false);
        }
        finally
        {
            objAgentCallCenter = null;
        }
    }
    /// <summary>
    /// Determines whether [is alpha numeric] [the specified STR to check].
    /// </summary>
    /// <param name="strToCheck">The STR to check.</param>
    /// <returns>
    /// 	<c>true</c> if [is alpha numeric] [the specified STR to check]; otherwise, <c>false</c>.
    /// </returns>
    private bool IsAlphaNumeric(String strToCheck)
    {
        Regex objAlphaPattern = new Regex("[a-zA-Z]");
        Regex objNumericPattern = new Regex("[0-9]");

        if (objAlphaPattern.IsMatch(strToCheck) && objNumericPattern.IsMatch(strToCheck))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Handles the Click event of the btnAdd control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The instance containing the event data.</param>
    protected void btnAdd_Click(object sender, System.EventArgs e)
    {
        AgentCallCenter objAgentCallCenter = null;
        AgentInformation objUserInfo = null;
        string errorMessage = "";
        string setFocus = "";
        string phoneNumber = txtPhoneAreaCode.Text.Trim() + txtPhonePrefix.Text.Trim() + txtPhoneNNNN.Text.Trim();
        string cellNumber = txtmobileArea.Text.Trim() + txtprefix.Text.Trim() + txtMobileNNNN.Text.Trim();

        try
        {
            if (!txtPassword.Text.Trim().Equals(""))
            {
                string password = "";
                password = txtPassword.Text.Trim();

                if (!IsAlphaNumeric(password))
                {
                    errorMessage = "Password must have at least one numeric and one alpha character.";
                    setFocus = txtPassword.ClientID;
                }
            }
            if (phoneNumber.Length > 0 && phoneNumber.Length < 10)
            {
                errorMessage = errorMessage + "You must enter valid Phone Number\\n";
                setFocus = txtPhoneAreaCode.ClientID;
            }

            if (cellNumber.Length > 0 && cellNumber.Length < 10)
            {
                errorMessage = errorMessage + "You must enter valid Mobile Number\\n";
                setFocus = txtmobileArea.ClientID;
            }

            if (errorMessage.Length > 0)
            {
                generateDataGridHeight();
                ScriptManager.RegisterClientScriptBlock(upnlGrdUsers, upnlGrdUsers.GetType(), "Navigate", "<script type=\'text/javascript\'>alert('" + errorMessage + "');document.getElementById('" + setFocus + "').focus();</script>", false);
                generateDataGridHeight();
                return;
            }
            objAgentCallCenter = new AgentCallCenter();
            int vocUserID = 0;
            if (ViewState[AGENTID] != null)
            {
                vocUserID = Convert.ToInt32(ViewState[AGENTID].ToString());
            }

            if ((!txtLoginID.Text.Trim().Equals("") || !txtPassword.Text.Trim().Equals("")) && objAgentCallCenter.CheckDuplicatePIN(vocUserID, txtLoginID.Text.Trim(), txtPassword.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(upnlGrdUsers, upnlGrdUsers.GetType(), "PinError", "alert('These credentials are used by another Agent.');", true);
                generateDataGridHeight();
            }
            else
            {
                objUserInfo = new AgentInformation();
                objUserInfo.FirstName = txtFirstName.Text.Trim();
                objUserInfo.LastName = txtLastName.Text.Trim();
                objUserInfo.Email = txtEmail.Text.Trim();
                objUserInfo.Phone = txtPhoneAreaCode.Text.Trim() + txtPhonePrefix.Text.Trim() + txtPhoneNNNN.Text.Trim();
                objUserInfo.RoleID = Convert.ToInt32(cmbRoles.SelectedValue);
                objUserInfo.Status = chkActive.Checked;
                objUserInfo.LoginID = txtLoginID.Text.Trim();
                objUserInfo.Password = txtPassword.Text.Trim();
                objUserInfo.CallCenterID = callCenterID;
                objUserInfo.InstitutionID = institutionID;
                objUserInfo.MobileNumber = txtmobileArea.Text.Trim() + txtprefix.Text.Trim() + txtMobileNNNN.Text.Trim();
                int itemCount = clstGroupList.Items.Count;
                string groupIDCollection = "";
                if (itemCount > 0)
                {
                    for (int i = 0; i < itemCount; i++)
                    {
                        if (clstGroupList.Items[i].Selected)
                        {
                            groupIDCollection = groupIDCollection + clstGroupList.Items[i].Value + ",";
                        }
                    }
                    if (groupIDCollection.Length <= 0)
                    {
                        groupIDCollection = "-1";
                    }
                    else
                    {
                        groupIDCollection = groupIDCollection.Substring(0, groupIDCollection.Length - 1);
                    }
                }
                if (vocUserID != 0)
                {
                    objUserInfo.VOCUserID = Convert.ToInt32(ViewState[AGENTID].ToString());
                    objAgentCallCenter.UpdateUserInformation(objUserInfo, groupIDCollection);
                    ScriptManager.RegisterStartupScript(upnlGrdUsers, upnlGrdUsers.GetType(), "Save", "alert('Record updated successfully.');", true);
                }
                else
                {
                    objUserInfo.VOCUserID = 0;
                    objAgentCallCenter.InsertUserInformation(objUserInfo, groupIDCollection);
                    ScriptManager.RegisterStartupScript(upnlGrdUsers, upnlGrdUsers.GetType(), "Add", "alert('Record added successfully.')", true);
                }
                fillAgentInfo();
                clearAllFields();
                generateDataGridHeight();
            }
        }
        catch (Exception ex)
        {
            Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("btnAdd_Click", "0", ex.Message, ex.StackTrace), 0);
        }
        finally
        {
            objAgentCallCenter = null;
            objUserInfo = null;
        }
    }

    /// <summary>
    /// Handles the Click event of the btnCancel control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The instance containing the event data.</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearAllFields();
        generateDataGridHeight();
        upnlGrdUsers.Update();
    }

    #endregion

    #region Private Methods
    /// <summary>
    /// Clears all fields.
    /// </summary>
    private void clearAllFields()
    {
        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtEmail.Text = "";
        txtPhoneAreaCode.Text = "";
        txtPhonePrefix.Text = "";
        txtPhoneNNNN.Text = "";
        cmbRoles.SelectedValue = "-1";
        txtLoginID.Text = "";
        txtPassword.Text = "";
        chkActive.Checked = true;
        chkActive.Enabled = true;
        ViewState[AGENTID] = null;
        ViewState[SELECTED_GRID_ITEM] = null;
        hdnTestDataChanged.Value = "false";
        btnAdd.Text = " Add ";
        txtmobileArea.Text = "";
        txtMobileNNNN.Text = "";
        txtprefix.Text = "";
        btnAdd.Enabled = true;
        lblLoginUserMessage.Visible = false;
        populateGroups();
        upnlAddEdit.Update();
    }

    /// <summary>
    /// Fills the user info.
    /// </summary>
    private void fillAgentInfo()
    {
        AgentCallCenter objAgentCallCenter = null;
        DataTable dtUsers = null;
        try
        {
            objAgentCallCenter = new AgentCallCenter();
            dtUsers = objAgentCallCenter.GetAgentInformation(callCenterID);
            if (dtUsers.Rows.Count > 1)
                grdUsers.AllowSorting = true;
            else
                grdUsers.AllowSorting = false;
            grdUsers.DataSource = dtUsers.DefaultView;
            grdUsers.DataBind();
        }
        finally
        {
            objAgentCallCenter = null;
            dtUsers = null;
        }
    }

    /// <summary>
    /// Populates the groups.
    /// </summary>
    private void populateGroups()
    {
        AgentCallCenter objAgentCallCenter = null;
        DataView dvGroups = null;
        DataTable dtGroups = null;
        DataTable dtAgentGroups = null;
        int itemCount = 0;
        try
        {
            objAgentCallCenter = new AgentCallCenter();

            lblNoRecords.Visible = false;

            if (!IsPostBack)
            {
                dtGroups = objAgentCallCenter.GetAllAssignedGroups(callCenterID);
                dvGroups = dtGroups.DefaultView;
                dvGroups.Sort = "GroupName ASC";
                clstGroupList.DataSource = dtGroups;
                clstGroupList.DataTextField = "GroupName";
                clstGroupList.DataValueField = "GroupID";
                clstGroupList.DataBind();
            }
            int groupItemCount = clstGroupList.Items.Count;
            if (groupItemCount > 0)
            {                
                if (ViewState[AGENTID] != null)
                {
                    int vocUserID = Convert.ToInt32(ViewState[AGENTID].ToString());
                    dtAgentGroups = objAgentCallCenter.GetAllAgentAssignedGroups(vocUserID);
                    for (int index = 0; index < groupItemCount && dtAgentGroups.Rows.Count > 0; index++)
                    {
                        bool isGroupAssigned = (dtAgentGroups.Select("GroupID=" + clstGroupList.Items[index].Value).Length > 0);
                        clstGroupList.Items[index].Selected = isGroupAssigned;
                    }
                }
                else
                {
                    for (int index = 0; index < groupItemCount; index++)
                    {
                        clstGroupList.Items[index].Selected = false;
                    }
                }                               
        
            }
            else
            {
                lblNoRecords.Visible = true;
                Div1.Visible = false;
            }
        }
        finally
        {
            objAgentCallCenter = null;
            dtGroups = null;
            dvGroups = null;
            dtAgentGroups = null;
        }
    }

    /// <summary>
    /// Loads the roles.
    /// </summary>
    private void loadRoles()
    {
        AgentCallCenter objAgentCallCenter = null;
        DataTable dtRoles = null;
        DataRow drRole = null;
        try
        {
            objAgentCallCenter = new AgentCallCenter();
            dtRoles = objAgentCallCenter.GetAgentRoles();
            drRole = dtRoles.NewRow();
            drRole[0] = -1;
            drRole[1] = "-- Select Role --";
            dtRoles.Rows.InsertAt(drRole, 0);

            cmbRoles.DataSource = dtRoles;
            cmbRoles.DataTextField = "RoleDescription";
            cmbRoles.DataValueField = "RoleID";
            cmbRoles.DataBind();

        }
        finally
        {
            objAgentCallCenter = null;
            dtRoles = null;
            drRole = null;
        }
    }

    /// <summary>
    /// Generates the height of the data grid.
    /// </summary>
    private void generateDataGridHeight()
    {
        try
        {
            string newUid = this.UniqueID.Replace(":", "_");
            string script = "<script type=\"text/javascript\">";
            script += "if(document.getElementById('UserInfoDiv') != null){document.getElementById('UserInfoDiv').style.height=setHeightOfGrid('" + grdUsers.ClientID + "','375');}</script>";
            if (!IsPostBack)
                ScriptManager.RegisterStartupScript(upnlGrdUsers, upnlGrdUsers.GetType(), newUid, script, false);
            else
                ScriptManager.RegisterClientScriptBlock(upnlGrdUsers, upnlGrdUsers.GetType(), newUid, script, false);
        }
        catch (Exception ex)
        {
            Tracer.GetLogger().LogExceptionEvent("User Management.generateDataGridHeight():: Exception occured for User ID - 0 As -->" + ex.Message + " " + ex.StackTrace, 0);
            throw;
        }
    }

    /// <summary>
    /// Register JS variables, client side button click events
    /// </summary>
    private void registerJavascriptVariables()
    {
        StringBuilder sbScript = new StringBuilder();
        sbScript.Append("<script language=JavaScript>");

        sbScript.Append("var hdnTestDataChangedClientID = '" + hdnTestDataChanged.ClientID + "';");

        sbScript.Append("</script>");
        this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());

        txtFirstName.Attributes.Add("onchange", "JavaScript:testdataChanged();");
        txtLastName.Attributes.Add("onchange", "JavaScript:testdataChanged();");
        txtEmail.Attributes.Add("onchange", "JavaScript:testdataChanged();");
        txtPhoneAreaCode.Attributes.Add("onchange", "JavaScript:testdataChanged();");
        txtPhonePrefix.Attributes.Add("onchange", "JavaScript:testdataChanged();");
        txtPhoneNNNN.Attributes.Add("onchange", "JavaScript:testdataChanged();");
        cmbRoles.Attributes.Add("onchange", "JavaScript:testdataChanged();");
        txtLoginID.Attributes.Add("onchange", "JavaScript:testdataChanged();");
        txtPassword.Attributes.Add("onchange", "JavaScript:testdataChanged();");
        chkActive.Attributes.Add("onchange", "JavaScript:testdataChanged();");

        txtmobileArea.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
        txtprefix.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
        txtMobileNNNN.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");

        btnCancel.Attributes.Add("onclick", "Javascript:return resetTestDataChanged();");

        txtPhoneAreaCode.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
        txtPhonePrefix.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
        txtPhoneNNNN.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
        txtLoginID.Attributes.Add("onkeypress", "JavaScript:if ((window.event.which) ? window.event.which : window.event.keyCode == 32) return false; else return true;");
        txtPassword.Attributes.Add("onkeypress", "JavaScript:if ((window.event.which) ? window.event.which : window.event.keyCode == 32) return false; else return true;");
        txtPhoneAreaCode.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPhonePrefix.ClientID + "').focus()";
        txtPhonePrefix.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPhoneNNNN.ClientID + "').focus()";

        txtmobileArea.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtprefix.ClientID + "').focus()";
        txtprefix.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtMobileNNNN.ClientID + "').focus()";
    }
    #endregion
}
