#region File History

/******************************File History***************************
 * File Name        : message_forward.aspx.cs
 * Author           : 
 * Created Date     : 
 * Purpose          : Gives User Option to Forward Message to new Clinition.
 *                  : 
 *                  :
 * *********************File Modification History*********************
 * Date(mm-dd-yyyy) Developer   Reason of Modification
 * 
 * 02-15-2008      Suhas       Code Review Fixes
 * 03-03-2008      IAK         Message Note: New column createdBySystem added
 * 03-06-2008      IAK         Message Note: insertMessageNote() parameter NoteType passed
 * 03-06-2008      IAK         Error While forwarding message casting of enum was wrong
 * 04-18-2008      IAK         Message Lab Tests: Cansting error for result value
 * 04-25-2008      IAK         Performance Issue
 * 04-29-2008      Suhas       Defect # 3081 - Shift server code to client for btnCancel and lnkBack
 * 08-05-2008      Suhas       Defect #2900 - Fixed.
 * 12 Jun 2008     Prerak      Migration of AJAX Atlas to AJAX RTM 1.0
 * 26 Jun 2008     IAK         Defect 3367
 * 06 Nov 2008     Prerak      Defect #3174 - 'Back to Message' link gives 'Data-Loss Warning' Popup twice, fixed 
 * ------------------------------------------------------------------- 
 * 
 */
#endregion

#region Using Block
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.Handlers;

using Vocada.CSTools.Common;
using Vocada.CSTools.DataAccess;
using Vocada.VoiceLink.Utilities;
using System.Web.UI;
#endregion

namespace Vocada.CSTools
{
    /// <summary>
    /// Class for Message forward screen.
    /// </summary>
    public partial class MessageForward : System.Web.UI.Page
    {
        #region Class Variables
        private DataTable dtMsgDetails = new DataTable();
        private int messageID = 0;
        private int groupID = 0;
        private int timeZoneID = 4;
        private int isLabMessage = 0;
        private int departmentMessage = 0;
        private int instituteID = 0;
        private int directoryID = 0;
        #endregion Class Variables

        #region Constants
        private const string GROUPID = "GroupID";
        private const string DIRECTORYID = "DirectoryID";
        private const string TIMEZONEID = "TimeZoneID";
        private const string INSTITUTIONID = "InstitutionID";
        #endregion Constants

        #region Protected Fields
        protected string strUserSettings = "NO"; //String used in the aspx page 
        protected DataTable dtClinician = null;
        #endregion

        #region Private Methods

        /// <summary>
        /// This function get Message information about selected message
        /// This function calls stored procedure "VOC_VLR_GetMessageDetails"
        /// This function binds data into datagrid dgMessage
        /// </summary>
        /// <param name="cnx"></param>
        /// <param name="messageID"></param>
        private void getMessageDetails()
        {
            MessageDetails objLabDetails = null;
            try
            {
                DataRow dr = null;
                objLabDetails = new MessageDetails();
                dtMsgDetails = objLabDetails.GetMessageDetails(messageID, departmentMessage, isLabMessage);
                if (dtMsgDetails.Rows.Count > 0)
                {
                    dr = dtMsgDetails.Rows[0];
                    groupID = Convert.ToInt32(dr["GroupID"].ToString());
                    instituteID = Convert.ToInt32(dr["institutionID"].ToString());
                    directoryID = Convert.ToInt32(dr["directoryID"].ToString());
                    timeZoneID = Convert.ToInt32(dr["GroupTimeZoneID"].ToString());
                    ViewState[GROUPID] = groupID;
                    ViewState[INSTITUTIONID] = instituteID;
                    ViewState[DIRECTORYID] = directoryID;
                    ViewState[TIMEZONEID] = timeZoneID;

                    int recepientType = -1;
                    if (dr["RecipientTypeID"] != null)
                        recepientType = Convert.ToInt32(dr["RecipientTypeID"]);
                    ViewState["patientVoiceURL"] = dr["PatientVoiceURL"].ToString();
                    ViewState["impressionVoiceURL"] = dr["ImpressionVoiceURL"].ToString();
                    ViewState["findingID"] = Convert.ToInt32(dr["FindingID"].ToString());
                    ViewState["mrn"] = dr["MRN"].ToString();
                    ViewState["RequireReadback"] = dr["RequireReadback"].ToString();
                    if (dr["DOB"] != System.DBNull.Value && Convert.ToDateTime(dr["DOB"]) != DateTime.MinValue)
                        ViewState["dob"] = dr["DOB"].ToString();
                    ViewState["specialistID"] = dr["SpecialistID"].ToString();
                    lblFinding.Text = dr["FindingDescription"].ToString();

                    int subscriberRoleID = Convert.ToInt32(dr["RoleID"]);
                    ViewState["RoleID"] = subscriberRoleID;
                    string roleDesc = " (Lab Technician)";

                    if (subscriberRoleID == UserRole.Radiologists.GetHashCode() )
                    {
                        roleDesc = " (Specialist)";
                    }
                    else if (subscriberRoleID == UserRole.GroupAdmin.GetHashCode())
                    {
                        roleDesc = " (Group Administrator)";
                    }

                    lblFrom.Text = (string)dr["SpecialistDisplayName"] + roleDesc;

                    if (dr["RoomBedID"] == null || dr["RoomBedID"].ToString().Length == 0)
                        ViewState["roomBedID"] = -1;
                    else
                        ViewState["roomBedID"] = Convert.ToInt32(dr["RoomBedID"].ToString());

                    if (dr["ReadOn"] != null)
                    {
                        if (dr["ReadOn"].ToString().Length == 0 || Convert.ToDateTime(dr["ReadOn"].ToString()) == DateTime.MinValue)
                        {
                            chkCloseMessage.Visible = true;
                            lblCloseOrgMsg.Visible = true;
                        }
                        else
                        {
                            chkCloseMessage.Visible = false;
                            lblCloseOrgMsg.Visible = false;
                        }
                    }
                    dr = null;
                }
            }
            finally
            {
                objLabDetails = null;
            }
        }

        /// <summary>
        /// On page load this method fetches Records of all Physicians and loads it in Listbox.
        /// </summary>
        /// <param name="startingWith"></param>
        private void populatePhysicians()
        {
            LabMessageForward objMsgForward = new LabMessageForward();
            dtClinician = objMsgForward.GetPhysiciansForDirectoryID(Convert.ToInt32(ViewState[DIRECTORYID].ToString()));
            ddlRefPhysician.DataSource = dtClinician.DefaultView;
            Session["RecipientInfo"] = dtClinician;
            ddlRefPhysician.DataBind();

            dtClinician = null;
            objMsgForward = null;
        }

        /// <summary>
        /// This function gets Test Results for specified messageID
        /// This function calls stored procedure for "VOC_VL_getMessageTestResults"
        /// </summary>
        private void getMessageTestResults()
        {
            MessageDetails objMsgDetails = new MessageDetails();
            DataTable dtTestReslt = objMsgDetails.GetMessageTestResults(messageID);
            grdTestResults.DataSource = dtTestReslt.DefaultView;
            grdTestResults.DataBind();
            objMsgDetails = null;
            setDatagridHeight();
        }

        /// <summary>
        /// This function is to set dynamic height of datagrid of Message Details, Activity Log and Message Note
        /// </summary>
        private void setDatagridHeight()
        {
            int nTestHeight = 26;

            if (grdTestResults.Items.Count < 5)
            {
                if (grdTestResults.Items.Count == 0)
                    nTestHeight = 26 + 1;
                else
                    nTestHeight = (grdTestResults.Items.Count + 2) * 18;
            }
            else
            {
                nTestHeight = 4 * 19 + 26;
            }

            string uId = this.UniqueID;
            string newUid = uId.Replace(":", "_");
            StringBuilder acScript = new StringBuilder();
            acScript.Append("<script type=\"text/javascript\">");

            if (Convert.ToInt32(ViewState["RoleID"].ToString()) != UserRole.Radiologists.GetHashCode() && Convert.ToInt32(ViewState["RoleID"].ToString()) != UserRole.GroupAdmin.GetHashCode())
            {
                acScript.AppendFormat("document.getElementById(" + '"' + "ForwardTestReultDiv" + '"' + ").style.height='" + (nTestHeight + 1) + "';");
            }
            acScript.Append("</script>");
            ScriptManager.RegisterStartupScript(upnlLabResult, upnlLabResult.GetType(),newUid, acScript.ToString(),false);
        }

        /// <summary>
        /// Populates all the Units for the Group in the dropdownlist 'dlistUnits'.
        /// </summary>
        private void populateUnits()
        {
            DataTable dtUnits = null;
            UnitSetup objUnit = null;
            try
            {
                objUnit = new UnitSetup();
                dtUnits = objUnit.GetUnits(Convert.ToInt32(ViewState[INSTITUTIONID].ToString()), true);
                dtUnits.DefaultView.Sort = "UnitName";
                DataRow drSelect = dtUnits.NewRow();
                drSelect[0] = "-1";
                drSelect[1] = "-- Select Unit --";
                drSelect[2] = true;
                dtUnits.Rows.InsertAt(drSelect, 0);
                dlistUnits.DataSource = dtUnits;
                dlistUnits.DataBind();
                dlistUnits.SelectedValue = "-1";
            }
            finally
            {
                dtUnits = null;
                objUnit = null;
            }
        }

        /// <summary>
        /// Populate Room-Bed Details for selected Unit in 'Room Bed' Drop-down
        /// </summary>
        private void populateUnitRoomBedDetails(int unitID)
        {
            UnitRoomBedSetup objUnitRoomBedSetUp = null;
            DataTable dtBedDetails = null;
            try
            {
                objUnitRoomBedSetUp = new UnitRoomBedSetup();
                if (unitID == 0)
                {
                    dlistRoomBed.Items.Clear();
                }
                else
                {
                    using (dtBedDetails = objUnitRoomBedSetUp.GetUnitRoomBedDetailsForUnit(unitID))
                    {
                        DataRow drSelect = dtBedDetails.NewRow();
                        drSelect[0] = "0";
                        drSelect[1] = "-1";
                        drSelect[2] = "0";
                        drSelect[3] = "0";
                        drSelect[4] = "0";
                        drSelect[5] = "-- Select Room Bed --";
                        dtBedDetails.Rows.InsertAt(drSelect, 0);
                        dlistRoomBed.DataSource = dtBedDetails;
                        dlistRoomBed.DataBind();
                        dlistRoomBed.SelectedValue = "-1";
                        upnlRoomBeds.Update();
                    }
                }
                setDatagridHeight();
            }
            finally
            {
                objUnitRoomBedSetUp = null;
            }
        }
        /// <summary>
        /// Register JS variables, client side button click events
        /// </summary>
        private void registerJavascriptVariables()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var txtNoteClientID = '" + txtNote.ClientID + "';");
            sbScript.Append("var ddlRefPhysicianClientID = '" + ddlRefPhysician.ClientID + "';");
            sbScript.Append("var dlistUnitsClientID = '" + dlistUnits.ClientID + "';");
            sbScript.Append("var hdnReceipientIndexClientID = '" + hdnReceipientIndex.ClientID + "';");
            sbScript.Append("var hdnReceipientNameClientID = '" + hdnReceipientName.ClientID + "';");
            sbScript.Append("var textChangedClientID = '" + textChanged.ClientID + "';");
            sbScript.Append("</script>");
            this.RegisterClientScriptBlock("scriptClientIDs", sbScript.ToString());

            txtNote.Attributes.Add("onchange", "JavaScript:return CheckMaxLength('" + txtNote.ClientID + "',500);");
            txtNote.Attributes.Add("onkeyup", "JavaScript:return CheckMaxLength('" + txtNote.ClientID + "',500);");
            txtNote.Attributes.Add("onkeydown", "JavaScript:return CheckMaxLength('" + txtNote.ClientID + "',500);");
            txtNote.Attributes.Add("onkeypress", "JavaScript:return CheckMaxLength('" + txtNote.ClientID + "',500);");
            txtNote.Attributes.Add("onchange", "JavaScript:UpdateProfile();");

            lnkBack.NavigateUrl = "./message_details.aspx?MessageID=" + Request["MessageID"].ToString() + "&IsDeptMsg=" + Request["IsDeptMsg"] + "&IsLabMsg=" + Request["IsLabMsg"];
            ddlRefPhysician.Attributes.Add("onchange", "JavaScript:setReceipientValue();");
        }
        #endregion

        #region Control's Event
        /// <summary>
        /// Populates and sets all the controls with their default values at the time of Page load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session[SessionConstants.LOGGED_USER_ID] == null)
                    Response.Redirect(Vocada.CSTools.Utils.GetReturnURL("default.aspx", "message_forward.aspx", this.Page.ClientQueryString));

                if (Session["UserSettings"] != null)
                    strUserSettings = Session["UserSettings"].ToString();

                if (Request["MessageID"] == null || Request["MessageID"].Length == 0 || Request["IsDeptMsg"] == null || Request["IsDeptMsg"].Length == 0 || Request["IsLabMsg"] == null || Request["IsLabMsg"].Length == 0)
                {
                    Response.Redirect("./default.aspx");
                }
                else
                {
                    messageID = Convert.ToInt32(Request["MessageID"].ToString());
                    departmentMessage = Convert.ToInt32(Request["IsDeptMsg"].ToString());
                    isLabMessage = Convert.ToInt32(Request["IsLabMsg"].ToString());
                }


                if (!IsPostBack)
                {
                    getMessageDetails();
                    populatePhysicians();
                    populateUnits();

                    if (isLabMessage != 0)
                    {
                        getMessageTestResults();
                    }
                    else
                    {
                        LabTestDiv.Visible = false;
                        dlistUnits.Visible = false;
                        dlistRoomBed.Visible = false;
                        lblUnits.Visible = false;
                        lblRoomBed.Visible = false;
                    }
                    Session[SessionConstants.CURRENT_TAB] = "MsgCenter";
                    Session[SessionConstants.CURRENT_PAGE] = "message_forward.aspx?MessageID=" + Request.QueryString["MessageID"] + "&IsDeptMsg=" + Request["IsDeptMsg"] + "&IsLabMsg=" + Request["IsLabMsg"];
                    Session[SessionConstants.CURRENT_INNER_TAB] = "MessageForward";
                }
                registerJavascriptVariables();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] !=null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageForward.Page_Load:: Exception occured for User ID - " + Convert.ToInt32(Session[SessionConstants.USER_ID]).ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageForward.Page_Load:: Exception occured for User ID - 0 As -->" + ex.Message + " " + ex.StackTrace, 0);
                }
            }
        }

        /// <summary>
        /// Item data bound event to get Details of Message and show Test results if it is new finding and
        /// show result value if result type is Positive/Negative or Reactive/Non-reactive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdTestResults_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
                {
                    DataRowView data = e.Item.DataItem as DataRowView;
                    if (string.Compare(strUserSettings, "YES") == 0)
                    {
                        if (data["NewFinding"] != System.DBNull.Value && Convert.ToBoolean(data["NewFinding"]))
                        {
                            e.Item.Cells[3].Text = "Yes";
                        }
                    }
                    else
                    {
                        if (data["NewFinding"] != System.DBNull.Value && Convert.ToBoolean(data["NewFinding"]))
                        {
                            e.Item.Cells[3].Text = "<image src=img/ic_tick.gif border=0>";
                        }
                    }

                    if (data["ResultTypeID"] != System.DBNull.Value && Convert.ToInt32(data["ResultTypeID"]) == 2)
                    {
                        if (data["ResultLevel"] != System.DBNull.Value && Convert.ToInt32(data["ResultLevel"]) == 1)
                            e.Item.Cells[1].Text = "Positive";
                        else
                            e.Item.Cells[1].Text = "Negative";
                    }
                    else if (data["ResultTypeID"] != System.DBNull.Value && Convert.ToInt32(data["ResultTypeID"]) == 4)
                    {
                        if (data["ResultLevel"] != System.DBNull.Value && Convert.ToInt32(data["ResultLevel"]) == 1)
                            e.Item.Cells[1].Text = "Reactive";
                        else
                            e.Item.Cells[1].Text = "Non-Reactive";
                    }
                    else if (data["ResultTypeID"] != System.DBNull.Value && data["ResultTypeID"].ToString() == "3" && data["ResultLevel"].ToString() == "0")
                        e.Item.Cells[1].Text = " ";
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageForward.grdTestResults_ItemDataBound:: Exception occured for User ID - " + Convert.ToInt32(Session[SessionConstants.USER_ID]).ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageForward.grdTestResults_ItemDataBound:: Exception occured for User ID - 0 As -->" + ex.Message + " " + ex.StackTrace, 0);
                }
            }
        }

        /// <summary>
        /// Gets Details of Message and Forwards it to selected Clinition. Updates the Notification history
        /// for both original and forwarded message. Closes the original message if the checkbox for close message 
        /// is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnForward_Click(object sender, EventArgs e)
        {
            textChanged.Value = "false";
            int orgMessageID = messageID;
            int fwdMessageID = 0;
            int fwdUnitorBedMessageID = 0;
            StringBuilder sbScript = new StringBuilder();
            LabMessageForward objMsgForward = null;
            int roleID = Convert.ToInt32(ViewState["RoleID"].ToString());

            Group group = null;
            UserInfo objUserInfo = null;

            DataTable dtGroupInfo = null;
            try
            {
                setDatagridHeight();
                objUserInfo = Session[SessionConstants.USER_INFO] as UserInfo;

                objMsgForward = new LabMessageForward();
                MessageInfo objMsgInfo = new MessageInfo();
                objMsgInfo.SpecialistID = Convert.ToInt32(ViewState["specialistID"]);

                objMsgInfo.PatientVoiceURL = ViewState["patientVoiceURL"].ToString();
                objMsgInfo.ImpressionVoiceURL = ViewState["impressionVoiceURL"].ToString();
                objMsgInfo.FindingID = Convert.ToInt32(ViewState["findingID"]);
                objMsgInfo.DID = "default";
                objMsgInfo.MRN = ViewState["mrn"].ToString();
                if (ViewState["dob"] != null && ViewState["dob"].ToString().Length != 0)
                    objMsgInfo.DOB = DateTime.Parse(ViewState["dob"].ToString());
                objMsgInfo.Forward = 1;
                objMsgInfo.OriginalMessageID = orgMessageID;
                objMsgInfo.Accession = string.Empty;
                objMsgInfo.SubscriberID = objUserInfo.SubUserID;

                objMsgInfo.RecepientTypeID = MessageInfo.RecepientType.OrderingClinician.GetHashCode();

                if (roleID == 1 || roleID == 2)
                {
                    objMsgInfo.RoomBedID = 0;
                }
                else
                {
                    if (ViewState["roomBedID"] == null)
                        objMsgInfo.RoomBedID = 0;
                    else
                        objMsgInfo.RoomBedID = Convert.ToInt32(ViewState["roomBedID"]);
                }
                objMsgInfo.NeedReadback = bool.Parse(ViewState["RequireReadback"].ToString());
                if (int.Parse(hdnReceipientIndex.Value) > -1)
                {
                    dtClinician = Session["RecipientInfo"] as DataTable;

                    bool isDept = false;
                    if (dtClinician != null && dtClinician.Rows.Count > 0)
                        isDept = Convert.ToBoolean(dtClinician.Rows[int.Parse(hdnReceipientIndex.Value)].ItemArray[12]);

                    if (isDept)
                    {
                        objMsgInfo.RecepientTypeID = MessageInfo.RecepientType.Department.GetHashCode();
                        objMsgInfo.IsFwdToDept = true;
                    }
                    else
                    {
                        objMsgInfo.IsFwdToDept = false;
                    }
                    objMsgInfo.RecepientID = Convert.ToInt32(dtClinician.Rows[int.Parse(hdnReceipientIndex.Value)].ItemArray[1]);
                    fwdMessageID = objMsgForward.ForwardMessage(objMsgInfo, departmentMessage, roleID);
                }

                //send notification.                           
                string sUrl = System.Configuration.ConfigurationManager.AppSettings["VoiceLinkR2.com.vocada.voicelink1.Reference"];
                Vocada.Veriphy.NotifierServiceProxy notifierProxy = new Vocada.Veriphy.NotifierServiceProxy(sUrl);
                int notify = 0;

                if (fwdMessageID > 0)
                {

                    if (roleID == 1 || roleID == 2)
                    {
                        if (objMsgInfo.IsFwdToDept)
                            notify = notifierProxy.NewDepartmentMessage(fwdMessageID);
                        else
                            notify = notifierProxy.NewMessage(fwdMessageID);
                    }
                    else
                    {
                        notify = notifierProxy.NewLabMessage(fwdMessageID);
                    }

                }
                if (roleID != 1 || roleID != 2)
                {
                    if (dlistUnits.SelectedIndex > 0)
                    {
                        if (dlistRoomBed.SelectedIndex > 0)
                        {
                            objMsgInfo.RecepientTypeID = MessageInfo.RecepientType.BedNumber.GetHashCode();
                            objMsgInfo.RecepientID = Convert.ToInt32(dlistRoomBed.SelectedValue);
                            objMsgInfo.RoomBedID = Convert.ToInt32(dlistRoomBed.SelectedValue);
                        }
                        else
                        {
                            objMsgInfo.RecepientTypeID = MessageInfo.RecepientType.UnitName.GetHashCode();
                            objMsgInfo.RecepientID = Convert.ToInt32(dlistUnits.SelectedValue);
                            objMsgInfo.RoomBedID = 0;
                        }

                        fwdUnitorBedMessageID = objMsgForward.ForwardMessage(objMsgInfo, departmentMessage, roleID);

                        notify = notifierProxy.NewLabMessage(fwdUnitorBedMessageID);

                    }
                    dlistUnits.Enabled = false;
                    dlistRoomBed.Enabled = false;
                }

                if (notify == 0)
                {
                    /*SSK  -  2007/07/09 - Use Cc as Backup functionality - 1.Check the ClosePrimaryAndBackupMessages Group Preference setting for the Logged in User Group
                                                                            2.If the Setting is ON call the CloseBackupMessage of MarkAsReceived to close the related message when user checks to close the original message */
                    group = new Group();
                    dtGroupInfo = group.GetGroupInformationByGroupID(Convert.ToInt32(ViewState[GROUPID].ToString()));

                    bool isBackupMessage = false;
                    if (dtGroupInfo.Rows.Count > 0)
                    {
                        isBackupMessage = bool.Parse(dtGroupInfo.Rows[0]["ClosePrimaryAndBackupMessages"].ToString());
                    }
                    string readBy = Session[SessionConstants.LOGGED_USER_NAME].ToString();
                    int backupMessageId = 0;

                    /*SSK  -  2006/11/18 - 1.07 - Close the message i.e Mark it as read if checkbox is checked*/
                    if (chkCloseMessage.Checked)
                    {
                        LabMarkAsReceived objMarkReced = new LabMarkAsReceived();
                        objMarkReced.MarkMessageReceived(orgMessageID, departmentMessage, false, readBy, readBy, txtNote.Text.Trim(), true, isLabMessage, Convert.ToInt32(ViewState[GROUPID].ToString()));
                        if (isBackupMessage)
                            backupMessageId = objMarkReced.CloseBackupMessage(orgMessageID, departmentMessage, false, readBy, readBy, txtNote.Text.Trim(), false, isLabMessage, Convert.ToInt32(ViewState[GROUPID].ToString()));
                    }

                    //If user has entered any note Insert it in Database for both Original as well as forwarded message.

                    if (txtNote.Text.Trim().Length > 0)
                    {
                        MessageDetails objMsgDetails = new MessageDetails();
                        if (!chkCloseMessage.Checked)
                            // Insert Note for Original Message
                            objMsgDetails.InsertMessageNote(orgMessageID, departmentMessage, readBy, txtNote.Text, Convert.ToInt32(ViewState[GROUPID].ToString()), isLabMessage, MessageNoteType.Default);


                        // Insert Note for Forwarded Message
                        if (fwdMessageID > 0)
                            objMsgDetails.InsertMessageNote(fwdMessageID, Convert.ToInt32(objMsgInfo.IsFwdToDept), readBy, txtNote.Text, Convert.ToInt32(ViewState[GROUPID].ToString()), isLabMessage, MessageNoteType.Forwarded);
                        if (fwdUnitorBedMessageID > 0)
                            objMsgDetails.InsertMessageNote(fwdUnitorBedMessageID, 0, readBy, txtNote.Text, Convert.ToInt32(ViewState[GROUPID].ToString()), isLabMessage, MessageNoteType.Forwarded);

                        objMsgDetails = null;
                    }

                    /*SSK  -  2006/11/18 - Ref # - Show successful Message Popup and redirect user to Message Details page*/

                    sbScript.Append("alert('Message Forwarded.');document.getElementById(textChangedClientID).value = 'false';");
                }
                else
                {
                    Tracer.GetLogger().LogInfoEvent(Utils.ConcatenateString("MessageForward.btnForward_Click", Session[SessionConstants.USER_ID].ToString(), "Unable to forward message because notification service failed to send notification.", string.Empty), Convert.ToInt32(Session[SessionConstants.LOGGED_USER_ID]));
                    //revert database changes
                    if (fwdMessageID > 0)
                        objMsgForward.RollbackTransaction(fwdMessageID, roleID);
                    if (fwdUnitorBedMessageID > 0)
                        objMsgForward.RollbackTransaction(fwdUnitorBedMessageID, roleID);
                    sbScript.Append("alert('Unable to forward message to  " + hdnReceipientName.Value + "');");
                }

                sbScript.Append("location.href= 'message_details.aspx?MessageID=" + Request["MessageID"].ToString() + "&IsDeptMsg=" + Request["IsDeptMsg"] + "&IsLabMsg=" + Request["IsLabMsg"] + "';");
                ScriptManager.RegisterStartupScript(uplnButtons, uplnButtons.GetType(), "successFwd", sbScript.ToString(), true);

                
                ddlRefPhysician.Enabled = false;
                txtNote.Enabled = false;
                btnForward.Enabled = false;
                btnCancel.Enabled = false;
                chkCloseMessage.Enabled = false;
            }
            catch (Exception ex)
            {
                if (fwdMessageID > 0)
                {
                    objMsgForward.RollbackTransaction(fwdMessageID, roleID);
                }
                if (fwdUnitorBedMessageID > 0)
                {
                    objMsgForward.RollbackTransaction(fwdUnitorBedMessageID, roleID);
                }
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageForward.btnForward_Click:: Exception occured for User ID - " + Convert.ToInt32(Session[SessionConstants.LOGGED_USER_ID]).ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.LOGGED_USER_ID]));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageForward.btnForward_Click:: Exception occured for User ID - 0 As -->" + ex.Message + " " + ex.StackTrace, 0);
                }

            }
            finally
            {

                dtGroupInfo = null;
                group = null;

                objMsgForward = null;
            }
        }

        /// <summary>
        /// Redirect Page to Message Details Page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(uplnButtons,uplnButtons.GetType(),"Navigate To Message Detail", "<script type=\'text/javascript\'>Navigate('" + Request["MessageID"].ToString() + "', '" + Request["IsDeptMsg"].ToString() + "', '" + Request["IsLabMsg"].ToString() + "');</script>",false);
        }

        
        /// <summary>
        /// Selecting Units from Units Dropdown List for populating Room Bed details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dlistUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateUnitRoomBedDetails(Convert.ToInt32(dlistUnits.SelectedValue));
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageForward.dlistUnits_SelectedIndexChanged:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent("MessageForward.dlistUnits_SelectedIndexChanged:: Exception occured for User ID - 0 As -->" + ex.Message + " " + ex.StackTrace, 0);
                }
            }
        }
        #endregion
    }
}