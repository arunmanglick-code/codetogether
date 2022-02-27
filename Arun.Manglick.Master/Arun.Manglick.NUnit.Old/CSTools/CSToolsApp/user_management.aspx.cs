#region File History

/******************************File History***************************
 * File Name        : user_management.aspx.cs
 * Author           : Suhas Tarihalkar
 * Created Date     : 
 * Purpose          : This class is responsible for the insertion and updation of the current users in the CSTools
 *                  : 
 *                  :
 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification
 * -------------------------------------------------------------------
 * 13-03-2008       Suhas       Code review fixes
 * 02-04-2008       IAK         Password length extended to 20
 * 02-04-2008       IAK         Login ID length extended to 25
 * 05-07-2008       Suhas       Defect 2979: Auto Tab issue.
 * 05-21-2008       Suhas       Validation for phone number added
 * 12 Jun 2008      Prerak      Migration of AJAX Atlas to AJAX RTM 1.0
 * 14 Nov 2008      IAK         Defect #3110
 * ------------------------------------------------------------------- 
 *                          
 */
#endregion

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Text;
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

/// <summary>
/// Class is responsible for the user management for CSTools
/// </summary>
public partial class user_management : System.Web.UI.Page
{
    #region Private Members
    /// <summary>
    /// VOC User ID
    /// </summary>
    private const string VOCUSERID = "VOCUserID";
    /// <summary>
    /// Color code
    /// </summary>
    private const string COLORCODE = "#ffffcc";
    /// <summary>
    /// Current selection in the Datagrid
    /// </summary>
    private const string SELECTED_GRID_ITEM = "SelectedGridItem";
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
            Response.Redirect(Utils.GetReturnURL("default.aspx", "user_management.aspx", this.Page.ClientQueryString));

        try
        {
            Session[SessionConstants.CURRENT_PAGE] = "user_management.aspx";
            Session[SessionConstants.CURRENT_TAB] = "SystemAdmin";
            Session[SessionConstants.CURRENT_INNER_TAB] = "UserManagement";

            if (!Page.IsPostBack)
            {
                registerJavascriptVariables();
                //Fill Current User Information in the grid.
                fillUserInfo();
                //Fill all available roles in the dropdown.
                loadRoles();
                //Clears all the controls
                clearAllFields();

                generateDataGridHeight();
            }
        }
        catch (Exception ex)
        {
            Tracer.GetLogger().LogInfoEvent("user_management.Page_Load():: Exception occured for User ID - 0 As -->" + ex.Message + " " + ex.StackTrace, 0);
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
                lbtnEdit = (e.Item.Cells[6].Controls[0]) as LinkButton;
                editScript += "if(onTestdataChanged()){";

                if (e.Item.ItemIndex + 2 < 10)
                {
                    editScript += "__doPostBack('ctl00$ContentPlaceHolder1$grdUsers$ctl0" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                }
                else
                {
                    editScript += "__doPostBack('ctl00$ContentPlaceHolder1$grdUsers$ctl" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                }
                lbtnEdit.OnClientClick = editScript + "} else {return false;}";
            }
            else if (e.Item.ItemType == ListItemType.Header && e.Item.Cells[1].Controls.Count > 0)
            {
                ((e.Item.Cells[0].Controls[0]) as LinkButton).OnClientClick += "javascript:if(onTestdataChanged()){__doPostBack('ctl00$ContentPlaceHolder1$grdUsers$ctl01$ctl00', '');}else{return false;}";
                ((e.Item.Cells[1].Controls[0]) as LinkButton).OnClientClick += "javascript:if(onTestdataChanged()){__doPostBack('ctl00$ContentPlaceHolder1$grdUsers$ctl01$ctl01', '');}else{return false;}";
                ((e.Item.Cells[2].Controls[0]) as LinkButton).OnClientClick += "javascript:if(onTestdataChanged()){__doPostBack('ctl00$ContentPlaceHolder1$grdUsers$ctl01$ctl02', '');}else{return false;}";
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
    /// Handles the EditCommand event of the grdUsers control.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="e">The instance containing the event data.</param>
    protected void grdUsers_EditCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            generateDataGridHeight();
            clearAllFields();
            txtFirstName.Text = e.Item.Cells[0].Text.Trim() != "&nbsp;" ? e.Item.Cells[0].Text.Trim() : "";
            txtLastName.Text = e.Item.Cells[1].Text.Trim()!= "&nbsp;" ? e.Item.Cells[1].Text.Trim():"";
            txtEmail.Text = e.Item.Cells[3].Text.Trim() != "&nbsp;" ? e.Item.Cells[3].Text.Trim() : "";
            txtLoginID.Text = e.Item.Cells[9].Text.Trim();
            string additionalPhone = Utils.flattenPhoneNumber(e.Item.Cells[4].Text.Trim());
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
            if ((e.Item.Cells[5].Text.Trim()).Equals("Active"))
            {
                chkActive.Checked = true;
            }
            else
            {
                chkActive.Checked = false;
            }
            if (Convert.ToInt32(Session[SessionConstants.USER_ID].ToString()) == Convert.ToInt32(e.Item.Cells[8].Text.Trim()))
            {
                chkActive.Enabled = false;
            }

            cmbRoles.SelectedValue = e.Item.Cells[7].Text.Trim();
            txtPassword.Text = e.Item.Cells[10].Text.Trim();
            ViewState[VOCUSERID] = e.Item.Cells[8].Text.Trim();
            btnAdd.Text = " Save ";
            e.Item.BackColor = Color.FromName(COLORCODE);
            //Set the focus on the selected item on the grid
            ViewState[SELECTED_GRID_ITEM] = e.Item.ItemIndex;
            grdUsers.Items[e.Item.ItemIndex].Focus();
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
        UserManagement objUser = null;
        try
        {
            dvSortedUserInfo = new DataView();
            objUser = new UserManagement();
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

            dvSortedUserInfo = objUser.GetUsersInformation().DefaultView;
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
    /// This function is to generate unique PIN randomly for logged in user.
    /// This function calls stored procedure "getUniquePassword"
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGeneratePassword_Click(object sender, System.EventArgs e)
    {
        UserManagement objUser = null;
        try
        {
            objUser = new UserManagement();
            txtPassword.Text = objUser.GetNewPin("");
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
            Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("AddRP.btnGeneratePassword_Click:Exception occured for User ID - ", "0", ex.Message, ex.StackTrace), 0);
            ScriptManager.RegisterClientScriptBlock(upnlUserManagement, upnlUserManagement.GetType(), "Warning", "<script type=\"text/javascript\">alert('Error Generating Password.');</script>", false);
        }
        finally
        {
            objUser = null;
        }
    }

    /// <summary>
    /// Handles the Click event of the btnAdd control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The instance containing the event data.</param>
    protected void btnAdd_Click(object sender, System.EventArgs e)
    {
        UserManagement objUserManagement = null;
        CSTUserInformation objUserInfo = null;
        string errorMessage = "";
        string setFocus = "";
        string phoneNumber = txtPhoneAreaCode.Text.Trim() + txtPhonePrefix.Text.Trim() + txtPhoneNNNN.Text.Trim();
        try
        {
            if (phoneNumber.Length > 0 && phoneNumber.Length < 10)
            {
                errorMessage = "You must enter valid Phone Number\\n";
                setFocus = txtPhoneAreaCode.ClientID;
                generateDataGridHeight();
                ScriptManager.RegisterClientScriptBlock(upnlUserManagement,upnlUserManagement.GetType(),"Navigate", "<script type=\'text/javascript\'>alert('" + errorMessage + "');document.getElementById('" + setFocus + "').focus();</script>",false);
                generateDataGridHeight();
                return;
            }
            objUserManagement = new UserManagement();
            int vocUserID = 0;
            if (ViewState[VOCUSERID] != null)
            {
                vocUserID = Convert.ToInt32(ViewState[VOCUSERID].ToString());
            }

            if (objUserManagement.CheckDuplicatePIN(vocUserID, txtLoginID.Text.Trim(), txtPassword.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(upnlUserManagement, upnlUserManagement.GetType(), "PinError", "alert('This PIN has been already used by another user.');", true);
                generateDataGridHeight();
            }
            else
            {
                objUserInfo = new CSTUserInformation();
                objUserInfo.FirstName = txtFirstName.Text.Trim();
                objUserInfo.LastName = txtLastName.Text.Trim();
                objUserInfo.Email = txtEmail.Text.Trim();
                objUserInfo.Phone = txtPhoneAreaCode.Text.Trim() + txtPhonePrefix.Text.Trim() + txtPhoneNNNN.Text.Trim();
                objUserInfo.RoleID = Convert.ToInt32(cmbRoles.SelectedValue);
                objUserInfo.Status = chkActive.Checked;
                objUserInfo.LoginID = txtLoginID.Text.Trim();
                objUserInfo.Password = txtPassword.Text.Trim();
                if (vocUserID != 0)
                {
                    objUserInfo.VOCUserID = Convert.ToInt32(ViewState[VOCUSERID].ToString());
                    objUserManagement.UpdateUserInformation(objUserInfo);
                    ScriptManager.RegisterStartupScript(upnlUserManagement, upnlUserManagement.GetType(), "Save", "alert('Record updated successfully.');", true);
                }
                else
                {
                    objUserInfo.VOCUserID = 0;
                    objUserManagement.InsertUserInformation(objUserInfo);
                    ScriptManager.RegisterStartupScript(upnlUserManagement, upnlUserManagement.GetType(), "Add", "alert('Record added successfully.')", true);
                }
                fillUserInfo();
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
            objUserManagement = null;
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
        ViewState[VOCUSERID] = null;
        ViewState[SELECTED_GRID_ITEM] = null;
        hdnTestDataChanged.Value = "false";
        btnAdd.Text = " Add ";
    }

    /// <summary>
    /// Fills the user info.
    /// </summary>
    private void fillUserInfo()
    {
        UserManagement objUser = new UserManagement();
        DataTable dtusers;
        dtusers = objUser.GetUsersInformation();
        if (dtusers.Rows.Count > 1)
            grdUsers.AllowSorting = true;
        else
            grdUsers.AllowSorting = false;
        grdUsers.DataSource = dtusers.DefaultView;
        grdUsers.DataBind();
    }


    /// <summary>
    /// Loads the roles.
    /// </summary>
    private void loadRoles()
    {
        UserManagement objUM = null;
        DataTable dtRoles = null;
        DataRow drRole = null;
        try
        {
            objUM = new UserManagement();
            dtRoles = objUM.GetRoles();
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
            objUM = null;
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
            string script = "<script type=\"text/javascript\">";
            script += "if(document.getElementById('UserInfoDiv') != null){document.getElementById('UserInfoDiv').style.height=setHeightOfGrid('" + grdUsers.ClientID + "','375');}</script>";
            if (!IsPostBack)
                ScriptManager.RegisterStartupScript(upnlUserManagement,upnlUserManagement.GetType(),"setHeight", script,false);
            else
                ScriptManager.RegisterClientScriptBlock(upnlUserManagement, upnlUserManagement.GetType(),"setHeight", script,false);
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
        sbScript.Append("var txtLoginIdClientID = '" + txtLoginID.ClientID + "';");
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

        btnCancel.Attributes.Add("onclick", "Javascript:return resetTestDataChanged();");

        txtPhoneAreaCode.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
        txtPhonePrefix.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
        txtPhoneNNNN.Attributes.Add("onkeypress", "JavaScript:return isNumericKeyStrokes();");
        txtLoginID.Attributes.Add("onkeypress", "JavaScript:if ((window.event.which) ? window.event.which : window.event.keyCode == 32) return false; else return true;");
        txtPassword.Attributes.Add("onkeypress", "JavaScript:if ((window.event.which) ? window.event.which : window.event.keyCode == 32) return false; else return true;");
        txtPhoneAreaCode.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPhonePrefix.ClientID + "').focus()";
        txtPhonePrefix.Attributes["onkeyup"] = "javascript: var keyCode = (window.event.which) ? window.event.which : window.event.keyCode; if(this.value.length>=3 && (keyCode != 9)) document.getElementById('" + txtPhoneNNNN.ClientID + "').focus()";
    }
    #endregion
}
