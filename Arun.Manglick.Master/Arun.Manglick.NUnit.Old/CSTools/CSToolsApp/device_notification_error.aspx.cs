#region File History

/******************************File History***************************
 * File Name        : device_notification_error.aspx.cs
 * Author           : Prerak Shah
 * Created Date     : 20-Sep-2007
 * Purpose          : To Display the Report of Device Notification Error.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 28-11-2007   IAK     Modified function generateDataGridHeight()
 * 07-01-2008   IAK     Defect 2455
 * 12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 * ------------------------------------------------------------------- 
 *                          
 */
#endregion

#region using
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
using System.Text;
#endregion using

namespace Vocada.CSTools
{
    public partial class device_notification_error : System.Web.UI.Page
    {
        #region Private Members

        /// <summary>
        /// Grid Rows Count = 5
        /// </summary>
        private const int GRID_ROWS_COUNT = 12;

        /// <summary>
        /// Grid Row Size = 25
        /// </summary>
        private const int GRID_ROW_SIZE = 30;
        protected int nheight = GRID_ROW_SIZE;
        private string groupId = "";
        private string instId = "";
        #endregion Private Members

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
                Response.Redirect(Utils.GetReturnURL("default.aspx", "device_notification_error.aspx", this.Page.ClientQueryString));
            UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
            try
            {
                ScriptManager.RegisterStartupScript(upAddTestResult,upAddTestResult.GetType(),"HideDiv", "<script language=" + '"' + "Javascript" + '"' + ">document.getElementById(" + '"' + "DeviceErrorDiv" + '"' + ").style.border='none'" + ";</script>",false);
                registerJavaScript();

                if (!IsPostBack)
                {
                    Session[SessionConstants.WEEK_NUMBER] = null;
                    Session[SessionConstants.SHOWMESSAGES] = null;
                    Session[SessionConstants.STATUS] = null;
                    Session[SessionConstants.GROUP] = null;

                    if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
                    {
                        hdnIsSystemAdmin.Value = "0";
                        cmbInstitution.Visible = false;
                        lblInstName.Visible = true;
                        lblInstName.Text = userInfo.InstitutionName;
                        fillGroups(userInfo.InstitutionID);
                        //btnOCGrammar.Enabled = false;
                    }
                    else
                    {
                        hdnIsSystemAdmin.Value = "1";
                        cmbInstitution.Visible = true; ;
                        lblInstName.Visible = false;
                        fillInstitition();
                        if (Request["GroupID"] != null && Request["GroupID"].Length > 0)
                        {
                              cmbInstitution.SelectedValue = getInstituteID(int.Parse(Request["GroupID"])).ToString();
                        }
                        fillGroups(Convert.ToInt32(cmbInstitution.SelectedValue));
                        if (Request["GroupID"] != null && Request["GroupID"].Length > 0)
                        {
                            cmbGroup.SelectedValue = Request["GroupID"];
                        }
                    }
                   
                    populateDeviceError();
                    lblRecordCount.Text = "";
                    lblNorecord.Text = "";
                }

                Session[SessionConstants.CURRENT_TAB] = "Tools";
                Session["CurrentInnerTab"] = "DeviceNotificationError";
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("device_notification_error - page_load", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }
        
        protected void cmbInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                fillGroups(Convert.ToInt32(cmbInstitution.SelectedValue));
                if (cmbGroup.SelectedIndex == 0)
                    btnGo.Enabled = false;
                else
                    btnGo.Enabled = true;
                populateDeviceError();
                lblRecordCount.Text = "";
                lblNorecord.Text = "";

                //ClientScript.RegisterStartupScript(this.GetType(), "HideDiv", "<script language=" + '"' + "Javascript" + '"' + ">document.getElementById(" + '"' + "DeviceErrorDiv" + '"' + ").style.border='none'" + ";</script>");
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.LOGGED_USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("device_notification_error - cmbInstitution_SelectedIndexChanged", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {

            }
        }
        
        protected void dgDeviceError_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string deviceAddress;
                string errorDescription;
                string eventDescription;
                DataRowView data = e.Item.DataItem as DataRowView;

                (e.Item.Cells[1].Controls[0] as HyperLink).NavigateUrl = "./message_details.aspx?MessageID=" + data["MessageID"].ToString() + "&IsDeptMsg=" + data["IsDepartmentMessage"].ToString() + "&IsLabMsg=" + data["IsLabMessage"].ToString() + "&returnTo=2";

                deviceAddress = data["NotificationRecipientAddress"].ToString().Trim();
                errorDescription = data["FailureDescription"].ToString().Trim();
                eventDescription = data["EventDescription"].ToString().Trim();
                if (eventDescription.Length > 25)
                {
                    e.Item.Cells[3].Text = eventDescription.Substring(0, 25);
                    e.Item.Cells[3].Text = e.Item.Cells[3].Text + "...";
                    e.Item.Cells[3].ToolTip = eventDescription.ToString();
                }
                if (deviceAddress.Length > 30)
                {
                    e.Item.Cells[4].Text = deviceAddress.Substring(0, 30);
                    e.Item.Cells[4].Text = e.Item.Cells[4].Text + "...";
                    e.Item.Cells[4].ToolTip = deviceAddress.ToString();
                }
                if (errorDescription.Length > 85)
                {
                    e.Item.Cells[5].Text = errorDescription.Substring(0, 85);
                    e.Item.Cells[5].Text = e.Item.Cells[5].Text + "...";
                    e.Item.Cells[5].ToolTip = errorDescription.ToString();
                }

            }
        }
        
        protected void btnGo_Click(object sender, EventArgs e)
        {
            populateDeviceError();
        }
        
        /// <summary>
        /// Grid rows are sorted in Ascending/Descending order accordingly for the Sort Expression 
        /// belonging to the Column Header after selecting them.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgDeviceError_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            try
            {
                dgDeviceError.CurrentPageIndex = Convert.ToInt32(hidPageIndex.Value);
                DataTable dtDeviceError = Session["dtDeviceError"] as DataTable;
                DataView dvsrtDeviceError = new DataView(dtDeviceError);

                if (Session["ColumnName"] == e.SortExpression.ToString() && Session["Direction"] == "ASC")
                {
                    dvsrtDeviceError.Sort = e.SortExpression + " DESC";
                    Session["Direction"] = "DESC";
                }
                else if (Session["ColumnName"] == e.SortExpression.ToString() && Session["Direction"] == "DESC")
                {
                    dvsrtDeviceError.Sort = e.SortExpression + " ASC";
                    Session["Direction"] = "ASC";
                }
                else
                {
                    dvsrtDeviceError.Sort = e.SortExpression + " ASC";
                    Session["Direction"] = "ASC";
                    Session["ColumnName"] = e.SortExpression.ToString();
                }

                dgDeviceError.DataSource = dvsrtDeviceError;
                dgDeviceError.DataBind();

                generateDataGridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("device_notitication_error.dgDeviceError_SortCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
            }
        }
        #endregion Events

        #region Private Methods
        /// <summary>
        /// This Methods Fills the all institutions
        /// </summary>
        private void fillInstitition()
        {
            DataTable dtInstitution = new DataTable();
            dtInstitution = Utility.GetInstitutionList();
            cmbInstitution.DataSource = dtInstitution;
            cmbInstitution.DataBind();

            ListItem li = new ListItem("-- Select Institution --", "0");
            cmbInstitution.Items.Insert(0, li);
            cmbInstitution.Items.FindByValue("0").Selected = true;
        }

        /// <summary>
        /// This Method fill the group combo as per Institution selected.
        /// </summary>
        private void fillGroups(int institutionID)
        {
            DataTable dtGroups = new DataTable();
            Device objDevice = new Device();

            dtGroups = objDevice.GetGroups(institutionID);
            cmbGroup.DataSource = dtGroups;
            cmbGroup.DataBind();

            ListItem li = new ListItem("-- Select Group --", "-1");
            cmbGroup.Items.Insert(0, li);
            cmbGroup.Items.FindByValue("-1").Selected = true;
        }

        /// <summary>
        /// This function is to set dynamic height of datagrid of Test Results
        /// </summary>
        /// <param name="isStartupScript">To register as Startup Script or Client-script block</param>
        private void generateDataGridHeight()
        {
            try
            {
                string newUid = this.UniqueID.Replace(":", "_");
                string script = "<script type=\"text/javascript\">";
                script += "document.getElementById(" + '"' + "DeviceErrorDiv" + '"' + ").style.border='solid 1px #cccccc';";
                script += "if(document.getElementById(" + '"' + "DeviceErrorDiv" + '"' + ") != null){document.getElementById(" + '"' + "DeviceErrorDiv" + '"' + ").style.height=setHeightOfGrid('" + dgDeviceError.ClientID + "','" + 60 + "');}</script>";
                ScriptManager.RegisterStartupScript(upAddTestResult, upAddTestResult.GetType(),newUid, script,false);
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("test_result_definitions.generateDataGridHeight():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
        }

        /// <summary>
        /// Set error message if no record available and set lable for number of records
        /// </summary>
        private void populateDeviceError()
        {
            Device objDevice = null;
            DataTable dtDeviceError = null;
            try
            {
                objDevice = new Device();
                int groupID;

                groupID = Convert.ToInt32(cmbGroup.SelectedValue);

                dtDeviceError = objDevice.GetDeviceNotificationError(groupID);
                dgDeviceError.DataSource = dtDeviceError.DefaultView;
                Session["dtDeviceError"] = dtDeviceError;
                if (dtDeviceError.Rows.Count > 1)
                    dgDeviceError.AllowSorting = true;
                else
                    dgDeviceError.AllowSorting = false;

                int records = dtDeviceError.Rows.Count;
                if (records <= 1)
                    lblRecordCount.Text = records.ToString() + " Record";
                else
                    lblRecordCount.Text = records.ToString() + " Records";
                if (records <= 0)
                    lblNorecord.Text = "No Records available";
                else
                    lblNorecord.Text = "";

                dgDeviceError.DataBind();
                generateDataGridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("device_notification_error.populateDeviceError():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
            finally
            {
                objDevice = null;
                dtDeviceError = null;
            }
        }

        /// <summary>
        /// Register JS Variables
        /// </summary>
        private void registerJavaScript()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var cmbInstitutionsClientID = '" + cmbInstitution.ClientID + "';");
            sbScript.Append("var cmbGroupsClientID = '" + cmbGroup.ClientID + "';");
            sbScript.Append("var btnGoClientID  = '" + btnGo.ClientID + "';");
            sbScript.Append("var hdnIsSystemAdminClientID  = '" + hdnIsSystemAdmin.ClientID + "';");
            //sbScript.Append("var lblRecordCountClientID  = '" + lblRecordCount.ClientID + "';");
            sbScript.Append("enableControls();");
            sbScript.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());

            cmbGroup.Attributes.Add("onchange", "Javascript:enableControls();");
        }

        /// <summary>
        /// Get institute Id from given group id.
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        private int getInstituteID(int GroupID)
        {
            Group objGroup = null;
            DataTable dtGroupInfo = null;
            int institutionID = 0;
            try
            {
                objGroup = new Group();
                dtGroupInfo = objGroup.GetGroupInformationByGroupID(GroupID);
                if (dtGroupInfo.Rows.Count > 0)
                    institutionID = int.Parse(dtGroupInfo.Rows[0]["InstitutionID"].ToString());
                return institutionID;
            }
            finally
            {
                objGroup = null;
                dtGroupInfo = null;
            }
        }
        #endregion
    }
}