#region File History

/******************************File History***************************
 * File Name        : group_monitor.aspx.cs
 * Author           : Prerak Shah.
 * Created Date     : 19-07-2007
 * Purpose          : This Class will be used for monitor groups.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 14-11-2007  Prerak Shah  Change according to new requirement.
 * 27-11-2007  IAK          Change function to set size of grid.
 * 18-12-2007  Prerak Shah  iteration 17 Code review fixes 
 * 18-01-2008  Prerak Shah  Integration of Messege center sps
 * 12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 * ------------------------------------------------------------------- 
 */
#endregion

#region Using
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
using System.Text;
using Vocada.CSTools.Common;
using Vocada.CSTools.DataAccess;
using Vocada.VoiceLink.Utilities;
#endregion

namespace Vocada.CSTools
{
    public partial class group_monitor : System.Web.UI.Page
    {
        #region Private Constant and Variable
        private const string FLG_DELETE = "FlgDelete";
        private const string INSTITUTE_ID = "InstitutionID";
        public const string SORT_ORDER = "SortOrder";
        private bool isSystemAdmin = true;
        private int instID;
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
            {
                Response.Redirect(Utils.GetReturnURL("default.aspx", "group_monitor.aspx", this.Page.ClientQueryString));
            }
            UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
            if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
            {
                isSystemAdmin = false;
                instID = userInfo.InstitutionID;
            }
            else
                isSystemAdmin = true;
            
            try
            {
                ////if(Session[SessionConstants.INSTITUTION_NAME] != null)
                //if (Request[INSTITUTE_ID] != null)
                //{
                //    ViewState[INSTITUTE_ID] = Request[INSTITUTE_ID].ToString();
                //    //lblInstituteName.Text = Session[SessionConstants.INSTITUTION_NAME].ToString();
                //}
                if (!IsPostBack)
                {
                    Session[SessionConstants.WEEK_NUMBER] = null;
                    Session[SessionConstants.SHOWMESSAGES] = null;
                    Session[SessionConstants.STATUS] = null;
                    Session[SessionConstants.GROUP] = null;
                    if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
                    {
                        isSystemAdmin = false;
                        hdnIsSystemAdmin.Value = "0";
                        cmbInstitutions.Visible = false;
                        lblInstName.Visible = true;
                        lblInstName.Text = " " + userInfo.InstitutionName;
                        //populateDirectories();
                    }
                    else
                    {
                        isSystemAdmin = true;
                        hdnIsSystemAdmin.Value = "1";
                        cmbInstitutions.Visible = true; ;
                        lblInstName.Visible = false;
                        populateInstitutions();
                        if (Request[INSTITUTE_ID] != null)
                        {
                            ViewState[INSTITUTE_ID] = Request[INSTITUTE_ID];
                            instID = Convert.ToInt32(Request[INSTITUTE_ID]);
                            cmbInstitutions.SelectedValue = (instID.ToString());
                        }
                    }
                    ViewState[SORT_ORDER] = "ASC";
                    ViewState[FLG_DELETE] = false;
                    populateGroups();
                }
                Session[SessionConstants.CURRENT_TAB] = "GroupMonitor";
                Session[SessionConstants.CURRENT_INNER_TAB] = "GroupMonitor";
                Session[SessionConstants.CURRENT_PAGE] = "group_monitor.aspx";
                if (userInfo.RoleId == UserRoles.SupportLevel2.GetHashCode())
                {
                    hlinkAddGroup.Visible = false;
                }
                else
                {
                    hlinkAddGroup.NavigateUrl = "./add_group.aspx?institutionID=" + instID;
                }
                Session[SessionConstants.SHOWMESSAGES] = "2";
                //Session[SessionConstants.GROUP] = 
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("group_monitor - page_load", Session["UserID"].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session["UserID"]));
                }
                throw objException;
            }
            
        }
        
        /// <summary>
        /// This event will be get fired when user clicks on the sortable header of the datagrid column to sort ascending or descending
        /// on the basis of the columnname.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgGroups_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            try
            {
                if (Session["DtGroups"] != null)
                {
                    dgGroups.CurrentPageIndex = Convert.ToInt32(hidPageIndex.Value);
                    DataTable dtGroups = Session["DtGroups"] as DataTable;

                    DataView dvsrtGroups = new DataView(dtGroups);

                    if (ViewState[SORT_ORDER].ToString().CompareTo("ASC") == 0)
                        ViewState[SORT_ORDER] = "DESC";
                    else
                        ViewState[SORT_ORDER] = "ASC";

                    dvsrtGroups.Sort = "GroupName " + ViewState[SORT_ORDER].ToString();
                    dgGroups.DataSource = dvsrtGroups;
                    dgGroups.DataBind();
                    dvsrtGroups = null;
                }
                setDatagridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("group_monitor.dgGroups_SortCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
            }
        }

        protected void dgGroups_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                if (string.Compare(e.CommandName, "Delete") == 0)
                {
                    int groupID = int.Parse(e.Item.Cells[0].Text);
                    int retValue = deleteGroup(groupID);
                    if (retValue == -1)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "group_monitor", "alert('We can not delete Group as Messages for Group subscriber is opened.');", true);
                    }
                    else
                    {
                        populateGroups();
                        ClientScript.RegisterStartupScript(this.GetType(), "GroupMonitor", "'Group Deleted Successfully';", true);
                    }
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID].ToString().Length > 0)
                    Tracer.GetLogger().LogExceptionEvent("group_monitor.dgGroups_DelateCommand --> " +  ex.Message + " at " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));

                throw ex;
            }
        }
        protected void cmbInstitutions_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ViewState[SORT_ORDER] = "ASC";
                instID = Convert.ToInt32(cmbInstitutions.SelectedValue);
                hlinkAddGroup.NavigateUrl = "./add_group.aspx?institutionID=" + instID; 
                populateGroups();
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null && Session[SessionConstants.USER_ID].ToString().Length > 0)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - cmbInstitutions_OnSelectedIndexChanged", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("add_directory - cmbInstitutions_OnSelectedIndexChanged", "0", objException.Message, objException.StackTrace), 0);
                }
                throw objException;
            }

        }
        #endregion

        #region Private Methods
        /// <summary>
        /// This method gets all group available on a directory and bound to grid.
        /// </summary>
        private void populateGroups()
        {
            Group objGroup = null;
            DataTable dtGroup = null;
            DataView dvGroup = null;
            try
            {
                objGroup = new Group();
                int institutionID = -1;
                if (isSystemAdmin)
                    institutionID = int.Parse(cmbInstitutions.SelectedValue);
                else
                    institutionID = instID;//Convert.ToInt32(Session[SessionConstants.INSTITUTION_ID]);

                dtGroup = objGroup.GetGroupListByInstitute(institutionID);
                if (dtGroup.Rows.Count > 1)
                    dgGroups.AllowSorting = true;
                else
                    dgGroups.AllowSorting = false;
                dvGroup = dtGroup.DefaultView;
                if (ViewState[SORT_ORDER] == null)
                    ViewState[SORT_ORDER] = "ASC";
                dvGroup.Sort = "GroupName " + ViewState[SORT_ORDER].ToString();
                Session["DtGroups"] = dtGroup;
                dgGroups.DataSource = dvGroup;
                dgGroups.DataBind();
                setDatagridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("group_monitor.populateGroups:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
            finally
            {
                objGroup = null;
                dtGroup = null;
            }
        }
       
        /// <summary>
        /// This method will set the height of datagrid dynamically accordingly the current rowcount of datagrid,
        /// each time when the page posts back. 
        /// </summary>
        private void setDatagridHeight()
        {
            string script = "";
            script += "if(document.getElementById(" + '"' + "divGroups" + '"' + ") != null){document.getElementById(" + '"' + "divGroups" + '"' + ").style.height=setHeightOfGrid('" + dgGroups.ClientID + "','" + 60 + "');}";
            if (!IsPostBack) 
                ScriptManager.RegisterStartupScript(this.Page,Page.GetType(),"PC", script,true);
            else
                ScriptManager.RegisterClientScriptBlock(UpdatePanelAGroupMonitor, UpdatePanelAGroupMonitor.GetType(), "PC", script, true);
         }

        /// <summary>
        /// This function is used to delete group.
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        private int deleteGroup(int groupID)
        {
            Group objGroup = new Group();
            MessageCenter objMessageCenter;
            int retValue = 1;
            try
            {
                objMessageCenter = new MessageCenter();
                int weeknumber = 0;
                DateTime today = DateTime.Today;
                string fromdate = today.AddMonths(-6).ToString();
                fromdate = fromdate.Substring(0, fromdate.Length - 12); 
                int instituteID = -1;

                DataTable dtmsgs = objMessageCenter.GetMessages(groupID, 0, weeknumber, fromdate, instituteID,"BOTH");     
                if (dtmsgs.Rows.Count > 0)
                      retValue = -1;
                if (retValue != -1)
                {
                    objGroup.DeleteGroupInformation(groupID);
                    retValue = 1;
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("group_monitor.DeleteGroup:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
            finally
            {
                objGroup = null;
                objMessageCenter = null;
            }
            return retValue;
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
                cmbInstitutions.Items.Insert(0, new ListItem("-- Select Institution --", "0"));
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null && Session[SessionConstants.USER_ID].ToString().Length > 0)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("group_monitior - populateInstitutions", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                else
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("group_monitior - populateInstitutions", "0", objException.Message, objException.StackTrace), 0);
                }
                throw objException;
            }
            finally
            {
                objInstitution = null;
            }
        }

        #endregion


    }
}