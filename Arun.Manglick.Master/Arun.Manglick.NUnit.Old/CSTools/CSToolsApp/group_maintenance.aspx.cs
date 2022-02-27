#region File History

/******************************File History***************************
 * File Name        : group_maintenance.aspx.cs
 * Author           : Prerak Shah.
 * Created Date     : 21-07-2007
 * Purpose          : This Class will modified group information, attached 
 *                    notification device to the group, see the subscriber detail 
 *                    and send mail to subscriber, Also add support note for the group. 
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 20-11-2007   Prerak Shah -   Modify for CSTools Part II
 * 06-12-2007   IAK         -   Remove delete link from subscriber grid and added status column.   
 * 12-07-2007   IAK         -   Defect 2403 fixed
 * 18-12-2007   Prerak Shah -   iteration 17 Code review fixes review details 5 fixed. 
 * 02-04-2007   IAK         -   Sort Order For subscriber list.
 * 05-07-2008   Suhas           Defect 2979: Auto Tab issue.
 * 12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 * 18 Nov 2008  Raju G  Defect 4176 fixed - Validation for duplication
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
using Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using System.Text;
using Vocada.VoiceLink.Utilities;
namespace Vocada.CSTools
{
    public partial class group_maintenance : System.Web.UI.Page
    {
        #region Page Variable
        protected int groupID;
        private const string INSTITUTE_ID = "InstID";
        private string userName;

        #endregion

        #region Page Load
        /// <summary>
        /// This method will load all the default fields and there values at the time when the page 
        /// GroupMaintenance loads.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            //((cs_tool)Page.Master).MenuID = cs_tool.MainMenu.tcnavMsg;

            if (Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
            {
                Response.Redirect(Utils.GetReturnURL("default.aspx", "group_maintenance.aspx", Page.ClientQueryString));
            }
            UserInfo userinfo = Session[SessionConstants.USER_INFO] as UserInfo;
            userName = userinfo.UserName;
            registerJavascriptVariables();
            try
            {
                hlinkGroupPreferences.NavigateUrl = "./group_preferences.aspx?GroupID=" + Convert.ToInt32(Request["GroupID"]);
                hlinkFindings.NavigateUrl = "./group_maintenance_findings.aspx?GroupID=" + Convert.ToInt32(Request["GroupID"]);
                
                if (!IsPostBack)
                {
                    groupID = Convert.ToInt32(Request["GroupID"]);
                    ViewState["GroupID"] = groupID;
                    populatePracticeTypes();
                    populateTimeZones();
                    populateGroupMaintenance();
                    txtAuthor.Text = userName;  
                }
                Session[SessionConstants.CURRENT_TAB] = "GroupMonitor";
                Session[SessionConstants.CURRENT_INNER_TAB] = "GroupMaintenance";
                Session[SessionConstants.CURRENT_PAGE] = "group_maintenance.aspx";

            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("group_maintenance - Page_Load", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            updateGroupDetails();
        }

        protected void btnAddNote_Click(object sender, EventArgs e)
        {
            Group objGroup;
            try
            {
                string author = txtAuthor.Text.Trim();
                string note = txtNote.Text.Trim();
                int groupid = Convert.ToInt32(ViewState["GroupID"]);
                objGroup = new Group();
                objGroup.AddGroupSupportNote(groupid, author, note);
                hdnTextChanged.Value = "false";
                populateSupportNotes();
                txtNote.Text = "";
                txtAuthor.Text = userName;
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "AddGroup", "<script type=\'text/javascript\'>document.getElementById(" + hdnTextChanged.ClientID + ").value = 'false';</script>",true);
                
                dgNotes.Focus();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID].ToString().Length != 0)
                {
                    Tracer.GetLogger().LogExceptionEvent("group_maintenance.btnAddNote:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                    throw ex;
                }
            }
            finally
            {
                objGroup = null;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ViewState["GroupID"] = null;
            ScriptManager.RegisterClientScriptBlock(upnlGrpInfo, upnlGrpInfo.GetType(), "NavigateToDir", "<script type=\'text/javascript\'>Navigate(" + ViewState[INSTITUTE_ID] + ");</script>", false);

        }
        protected void dgGroupMembers_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView data = e.Item.DataItem as DataRowView;
                string address = data["PrimaryEmail"].ToString();
                 HyperLink alink;
                alink = (HyperLink)e.Item.FindControl("lnkEmail");
                alink.NavigateUrl = "mailto:" + data["PrimaryEmail"].ToString();
                //alink.OnClientClick = "mailto:" + data["PrimaryEmail"].ToString();
            }
        }
        #endregion

        #region Private Methods
        private void populateGroupMaintenance()
        {
            getGroupInfo();
            fillGroupUsers();
            populateSupportNotes();
            
        }
        /// <summary>
        /// Get the directories for the institution in which this group belongs.
        /// </summary>
        private void getDirectories()
        {
            int institutionID = Convert.ToInt32(ViewState[INSTITUTE_ID]);
            DataTable dtDirectory = new DataTable();
            dtDirectory = Utility.GetDirectories(institutionID);
            cmbDiretories.DataSource = dtDirectory.DefaultView;
            cmbDiretories.DataBind();
        }
        /// <summary>
        /// Populate Practice Type
        /// </summary>
        private void populatePracticeTypes()
        {
            DataTable dtPT = new DataTable();
            dtPT = Utility.GetPracticeType();
            cmbPracticeType.DataSource = dtPT;
            cmbPracticeType.DataBind();
        }
        /// <summary>
        /// Populates TimeZone
        /// </summary>
        private void populateTimeZones()
        {
            DataTable dtTimeZone = new DataTable();
            dtTimeZone = Utility.GetTimeZone();
            cmbTimeZone.DataSource = dtTimeZone;
            cmbTimeZone.DataBind();
        }
        /// <summary>
        /// Register JS variables, client side button click events
        /// </summary>
        private void registerJavascriptVariables()
        {
            string sbScript = "";
            sbScript += "var hdnTextChangedClientID = '" + hdnTextChanged.ClientID + "';";
            ClientScript.RegisterStartupScript(Page.GetType(), "scriptDeviceClientIDs", sbScript.ToString(), true);

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

            txtAddress1.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtAddress2.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtAffiliation.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtAuthor.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtCity.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtNote.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtState.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtZip.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtGroupName.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            cmbDiretories.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            cmbPracticeType.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            cmbTimeZone.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtGroup800No1.Attributes.Add("onchange", "JavaScript:return TextChanged();");
	    txtGroup800No2.Attributes.Add("onchange", "JavaScript:return TextChanged();");
	    txtGroup800No3.Attributes.Add("onchange", "JavaScript:return TextChanged();");
	    txtPhone1.Attributes.Add("onchange", "JavaScript:return TextChanged();");
	    txtPhone2.Attributes.Add("onchange", "JavaScript:return TextChanged();");
	    txtPhone3.Attributes.Add("onchange", "JavaScript:return TextChanged();");
	    txtRP800No1.Attributes.Add("onchange", "JavaScript:return TextChanged();");
	    txtRP800No2.Attributes.Add("onchange", "JavaScript:return TextChanged();");
            txtRP800No3.Attributes.Add("onchange", "JavaScript:return TextChanged();");
 

            StringBuilder sbScript1 = new StringBuilder();
            sbScript1.Append("<script language=JavaScript>");
            sbScript1.Append("var txtAuthorClientID = '" + txtAuthor.ClientID + "';");
            sbScript1.Append("var txtNoteClientID = '" + txtNote.ClientID + "';");
            sbScript1.Append("var txtGroupNameClientID = '" + txtGroupName.ClientID + "';");
            sbScript1.Append("var cmbDirNameClientID = '" + cmbDiretories.ClientID + "';");
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
            sbScript1.Append("var cmbPractieTypeClientID = '" + cmbPracticeType.ClientID + "';");


            sbScript1.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript1.ToString());

            btnAddNote.Attributes.Add("onclick", "return checkNote();");
            btnSave.Attributes.Add("onclick", "return groupCheckRequired('NotAddGroup');");
            txtNote.Attributes.Add("onchange", "JavaScript:return CheckMaxLength('" + txtNote.ClientID + "','" + txtNote.MaxLength + "');");
            txtNote.Attributes.Add("onblur", "JavaScript:return CheckMaxLength('" + txtNote.ClientID + "','" + txtNote.MaxLength + "');");
            txtNote.Attributes.Add("onkeyup", "JavaScript:return CheckMaxLength('" + txtNote.ClientID + "','" + txtNote.MaxLength + "');");
            txtNote.Attributes.Add("onkeydown", "JavaScript:return CheckMaxLength('" + txtNote.ClientID + "','" + txtNote.MaxLength + "');");

        }
        /// <summary>
        /// This method gets the Group Information and fill accordingly.
        /// </summary>
        /// <param name="groupID"></param>
        private void getGroupInfo()
        {
            Group objGroup = new Group();
            DataTable dtGroup;
            try
            {
                dtGroup = objGroup.GetGroupInformationByGroupID(groupID);
                if (dtGroup.Rows.Count > 0)
                {
                    DataRow drGroupinfo = dtGroup.Rows[0];

                    txtGroupName.Text = drGroupinfo["GroupName"].ToString();
                    string grp800No = Utils.flattenPhoneNumber(drGroupinfo["Group800Number"].ToString().Trim());
                    if (grp800No.Length >= 10)
                    {
                        txtGroup800No1.Text = grp800No.Substring(0, 3);
                        txtGroup800No2.Text = grp800No.Substring(3, 3);
                        txtGroup800No3.Text = grp800No.Substring(6, 4);
                    }
                    string rp800No = Utils.flattenPhoneNumber(drGroupinfo["ReferringPhysician800Number"].ToString().Trim());
                    if (rp800No.Length >= 10)
                    {
                        txtRP800No1.Text = rp800No.Substring(0, 3);
                        txtRP800No2.Text = rp800No.Substring(3, 3);
                        txtRP800No3.Text = rp800No.Substring(6, 4);
                    }
                    cmbPracticeType.SelectedValue = drGroupinfo["PracticeType"].ToString();
                    txtAffiliation.Text = drGroupinfo["Affiliation"].ToString().Trim();
                    txtAddress1.Text = drGroupinfo["Address1"].ToString().Trim();
                    txtAddress2.Text = drGroupinfo["Address2"].ToString().Trim();
                    txtCity.Text = drGroupinfo["City"].ToString().Trim();
                    txtState.Text = drGroupinfo["State"].ToString().Trim();
                    txtZip.Text = drGroupinfo["Zip"].ToString().Trim();
                    cmbTimeZone.SelectedValue = drGroupinfo["TimeZoneID"].ToString();
                    string phone = Utils.flattenPhoneNumber(drGroupinfo["Phone"].ToString().Trim());
                    if (phone.Length > 2)
                    {
                        txtPhone1.Text = phone.Substring(0, 3);
                        txtPhone2.Text = phone.Substring(3, 3);
                        txtPhone3.Text = phone.Substring(6, 4);
                    }

                    ViewState[INSTITUTE_ID] = drGroupinfo["InstitutionID"];
                    getDirectories();
                    cmbDiretories.SelectedValue = drGroupinfo["DirectoryID"].ToString();
                    
                    //txtFirstName.Text  = drGroupinfo["CSRFirstName"].ToString();
                    //txtMiddleName.Text   = drGroupinfo["CSRMiddleName"].ToString();
                    //txtLastName.Text  = drGroupinfo["CSRLastName"].ToString();
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("group_maintenance.getGroupInformation:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
            finally
            {
                objGroup = null;
            }

        }
        /// <summary>
        /// This method will be get called when the page loads for the first time, this will fire "getGroupUsers"
        /// stored proc, passing it "GroupId" as parameter, the resultset return by this procedure is bound to
        /// Group Members datagrid.
        /// </summary>
        /// <param name="cnx"></param>
        private void fillGroupUsers()
        {
            Group objGroup = null;
            DataTable dtGroupUsers = null;
            DataView dvgrpusers = null;
            try
            {
                objGroup = new Group();
                int isActiveOnly = 0;
                dtGroupUsers = objGroup.GetGroupUsers(Convert.ToInt32(ViewState["GroupID"]), isActiveOnly);
                dtGroupUsers.Columns["PrimaryPhone"].ReadOnly = false;
                //change format of phone number in (xxx)-xxx-xxxx format
                for (int itemNum = 0; itemNum < dtGroupUsers.Rows.Count; itemNum++)
                    dtGroupUsers.Rows[itemNum]["PrimaryPhone"] = Utils.expandPhoneNumber(dtGroupUsers.Rows[itemNum]["PrimaryPhone"].ToString());
                dvgrpusers = new DataView(dtGroupUsers);
                dgGroupMembers.DataSource = dvgrpusers;
                dgGroupMembers.DataBind();
                setDynamicHeightforDatagrid();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID].ToString().Length != 0)
                {
                    Tracer.GetLogger().LogExceptionEvent("group_maintenance.fillGroupUsers:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                    throw ex;
                }
            }
            finally
            {
                objGroup = null;
                dtGroupUsers = null;
                dvgrpusers = null;
            }
        }
        /// <summary>
        /// This method Gets the suppot notes for the group.
        /// </summary>
        private void populateSupportNotes()
        {

            Group objGroup = null;
            DataTable dtGrpSupportNote = null;
            try
            {
                objGroup = new Group();
                dtGrpSupportNote = objGroup.GetGroupSupportNotes(Convert.ToInt32(ViewState["GroupID"]));

                dgNotes.DataSource = dtGrpSupportNote.DefaultView;
                dgNotes.DataBind();
                setDynamicHeightforNoteDatagrid();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID].ToString().Length != 0)
                {
                    Tracer.GetLogger().LogExceptionEvent("group_maintenance.fillGroupUsers:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                    throw ex;
                }
            }
            finally
            {
                objGroup = null;
            }
        }
        /// <summary>
        /// This method will set the Datagrid height dynamically depending on the
        /// records.
        /// </summary>
        private void setDynamicHeightforDatagrid()
        {
            // To set dynamic height of datagrid
            try
            {
                int gridHeight = 50;
                int headerHeight = 25;
                int rowHeight = 25;
                int maxRows = 7;
                if (dgGroupMembers.Items.Count <= maxRows)
                {
                    if (dgGroupMembers.Items.Count == 0)
                        gridHeight = headerHeight;
                    else
                        gridHeight = (dgGroupMembers.Items.Count * rowHeight) + headerHeight;
                }
                else
                {
                    gridHeight = maxRows * rowHeight;
                }

                string newUid = this.UniqueID.Replace(":", "_");
                string script = "<script type=\"text/javascript\">";
                script += "document.getElementById(" + '"' + divGroupMembers.ClientID + '"' + ").style.height='" + (gridHeight + 1) + "';";
                script += "</script>";
                if (!Page.IsPostBack)
                    ScriptManager.RegisterStartupScript(UpdatePanelAGroupMonitor, UpdatePanelAGroupMonitor.GetType(),newUid, script,false);
                else
                    ScriptManager.RegisterClientScriptBlock(UpdatePanelAGroupMonitor, UpdatePanelAGroupMonitor.GetType(), "divheight", script,false);  
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID].ToString().Length != 0)
                {
                    Tracer.GetLogger().LogExceptionEvent("group_maintenance.setDynamicHeightforDatagrid:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
            }
            finally
            {

            }
            //End of setting dynamic height of datagrid
        }
        /// <summary>
        /// Update Group Details
        /// </summary>
        private void updateGroupDetails()
        {
            if (Session[SessionConstants.USER_ID] != null)
            {
                Group objGroup;
                GroupInformation objGroupInfo = new GroupInformation();
                try
                {
                    objGroupInfo.GroupID = Convert.ToInt32(ViewState["GroupID"]);
                    objGroupInfo.DirectoryID = Convert.ToInt32(cmbDiretories.SelectedValue);
                    objGroupInfo.Affiliation = txtAffiliation.Text.Trim();
                    objGroupInfo.Group800Number = txtGroup800No1.Text + txtGroup800No2.Text + txtGroup800No3.Text;
                    objGroupInfo.GroupDID = txtGroup800No1.Text + txtGroup800No2.Text + txtGroup800No3.Text; //GroupDID is same as group 800 number 
                    objGroupInfo.ReferringPhysician800Number = txtRP800No1.Text + txtRP800No2.Text + txtRP800No3.Text;
                    objGroupInfo.ReferringPhysicianDID = txtRP800No1.Text + txtRP800No2.Text + txtRP800No3.Text; //RPDID is same as RP800 Number
                    objGroupInfo.PracticeType = Convert.ToInt32(cmbPracticeType.SelectedValue);
                    objGroupInfo.Address1 = txtAddress1.Text.Trim();
                    objGroupInfo.Address2 = txtAddress2.Text.Trim();
                    objGroupInfo.City = txtCity.Text.Trim();
                    objGroupInfo.State = txtState.Text.Trim();
                    objGroupInfo.Zip = txtZip.Text.Trim();
                    objGroupInfo.Phone = txtPhone1.Text + txtPhone2.Text + txtPhone3.Text;
                    objGroupInfo.TimeZoneID = Convert.ToInt32(cmbTimeZone.SelectedValue);
                    objGroupInfo.GroupName = txtGroupName.Text.Trim();
                    objGroup = new Group();
                    int retVal = objGroup.UpdateGroup(objGroupInfo);

                    if (retVal > 0)
                    {
                        hdnTextChanged.Value = "false";
                        ScriptManager.RegisterStartupScript(upnlGrpInfo, upnlGrpInfo.GetType(), "group_maintenance", "alert('Group Information updated successfully.');", true);
                    }
                    else
                    {
                        switch (retVal)
                        {
                            case -1: //Group Name duplicate
                                ScriptManager.RegisterStartupScript(upnlGrpInfo, upnlGrpInfo.GetType(), "group_maintenance", "alert('Group Name is already assigned');", true);
                                break;
                            case -2: //Group 800 Number duplicate
                                ScriptManager.RegisterStartupScript(upnlGrpInfo, upnlGrpInfo.GetType(), "group_maintenance", "alert('Group 800 number is already assigned');", true);
                                break;
                            case -3: // RP 800 Number duplicate
                                ScriptManager.RegisterStartupScript(upnlGrpInfo, upnlGrpInfo.GetType(), "group_maintenance", "alert('Ordering Clinician 800 Number is already assigned');", true);
                                break;

                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    if (Session[SessionConstants.USER_ID].ToString().Length != 0)
                    {
                        Tracer.GetLogger().LogExceptionEvent("group_maintenance.updateGroupInfo:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                        throw ex;
                    }
                }
                finally
                {
                    objGroup = null;
                    objGroupInfo = null;
                }
            }
        }

        /// <summary>
        /// This method will set the Datagrid height dynamically depending on the
        /// records.
        /// </summary>
        private void setDynamicHeightforNoteDatagrid()
        {
            // To set dynamic height of datagrid
            try
            {
                int gridHeight = 50;
                int headerHeight = 25;
                int rowHeight = 25;
                int maxRows = 7;
                if (dgNotes.Items.Count <= maxRows)
                {
                    if (dgNotes.Items.Count == 0)
                        gridHeight = headerHeight;
                    else
                        gridHeight = (dgNotes.Items.Count * rowHeight) + headerHeight;
                }
                else
                {
                    gridHeight = maxRows * rowHeight;
                }

                //string newUid = this.UniqueID.Replace(":", "_");
                string script = "<script type=\"text/javascript\">";
                script += "document.getElementById(" + '"' + divSupportNote.ClientID + '"' + ").style.height='" + (gridHeight + 1) + "';";
                script += "document.getElementById(" + '"' + hdnTextChanged.ClientID + '"' + ").value = 'false';";
                script += "</script>";
                if (!Page.IsPostBack)
                    ScriptManager.RegisterStartupScript(upnlSupportNote,upnlSupportNote.GetType(),"divHeight", script,false);
                else
                    ScriptManager.RegisterClientScriptBlock(upnlSupportNote, upnlSupportNote.GetType(), "divheight", script,false);
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID].ToString().Length != 0)
                {
                    Tracer.GetLogger().LogExceptionEvent("group_maintenance.setDynamicHeightforDatagrid:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
            }
            finally
            {

            }
            //End of setting dynamic height of datagrid
        }
        #endregion

    }
}
