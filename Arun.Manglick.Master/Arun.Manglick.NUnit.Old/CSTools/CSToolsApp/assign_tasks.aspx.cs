#region File History

/******************************File History***************************
 * File Name        : assign_tasks.aspx.cs
 * Author           : Prerak Shah.
 * Created Date     : 11-07-2007
 * Purpose          : This Class will display Roles and edit Role.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 30 Oct 2007 Prerak - Add page navigation warnig message.
 * 2-Nov-2007  Prerak - Added anthore select all checkbox for assigned tasks.
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
using Vocada.CSTools.Common;
using Vocada.CSTools.DataAccess;
using System.Text;
using Vocada.VoiceLink.Utilities;
namespace Vocada.CSTools
{
    public partial class assign_tasks : System.Web.UI.Page
    {
        #region Private variables
        private const string COLUMN_NAME = "TaskDescription";
        private const string SORT_DIRECTION = "ASC";
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session[SessionConstants.LOGGED_USER_ID] == null)
            {
                Response.Redirect(Utils.GetReturnURL("default.aspx", "attach_tasks.aspx", this.Page.ClientQueryString));
            }
            if(! IsPostBack)
            {
                fillRoles();
                cmbrole.SelectedValue = Request["RoleID"].ToString();
                hdnRole.Value = cmbrole.SelectedValue; 
                populateTasks();                
            }
            Session[SessionConstants.CURRENT_TAB] = "SystemAdmin";
            Session[SessionConstants.CURRENT_INNER_TAB] = "AssignTasks";
            Session[SessionConstants.CURRENT_PAGE] = "attach_tasks.aspx";

            registerJavascriptVariables();
        }
       

        /// <summary>
        /// This function calls MoveListItems function to transfer list items from Assigned list box to Unssigned Tasks list box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            RoleTask objRoleTask = new RoleTask();
            string taskList = hdTasks.Value;
            int roleId = Convert.ToInt32(cmbrole.SelectedValue);

            try
            {
                objRoleTask.UpdateRoleTask(roleId, taskList);
                hdnDataChanged.Value = "false";
                populateTasks(); 
                ClientScript.RegisterStartupScript(this.GetType(), "Attach Tasks", "alert('Tasks are assigned to " + cmbrole.SelectedItem + " successfully.');Navigate();", true);                
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.LOGGED_USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("attach_task - btnSave_Click", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
            }
        }
        
        /// <summary>
        /// This methode Navigate the page to role_task.aspx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("role_task.aspx", true);
            }
            catch { }
        }

        protected void cmbrole_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnRole.Value = cmbrole.SelectedValue; 
            populateTasks();
            chkAssignedAll.Checked = false;
            chkSelectAllTasks.Checked = false;
           // enableControls();
        }
        #endregion

        #region private methods
        /// <summary>
        /// Populate Units in available list box and assigned list box. 
        /// </summary>
        /// <param name="checkValue"></param>
        private void populateTasks()
        {
            RoleTask  objRoleTask = new RoleTask();
            try
            {
                    DataTable dtAssignTasks = objRoleTask.GetAssignedTask(Convert.ToInt32(cmbrole.SelectedValue));
                    DataView dvAssignedTasks = dtAssignTasks.DefaultView;
                    dvAssignedTasks.Sort = COLUMN_NAME.ToString() + " " + SORT_DIRECTION.ToString();
                    lstbAttachedTasks.DataTextField = "TaskDescription";
                    lstbAttachedTasks.DataValueField = "TaskID";
                    lstbAttachedTasks.DataSource  = dvAssignedTasks;
                    lstbAttachedTasks.DataBind();                    
                    DataTable dtUnAssignedTasks = objRoleTask.GetUnAssignedTasks(Convert.ToInt32(cmbrole.SelectedValue));
                    DataView dvUnAssignedTasks = dtUnAssignedTasks.DefaultView;
                    dvUnAssignedTasks.Sort = COLUMN_NAME.ToString() + " " + SORT_DIRECTION.ToString();
                    lstbAllTasks.DataTextField = "TaskDescription";
                    lstbAllTasks.DataValueField = "TaskID";
                    lstbAllTasks.DataSource = dvUnAssignedTasks;
                    lstbAllTasks.DataBind();
                    
                    btnAdd.Enabled = false;
                    btnRemove.Enabled = false;
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.LOGGED_USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("attach_task - PopulateTasks", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                objRoleTask = null;
            }
             
        }

        private void fillRoles()
        {
            RoleTask objRoleTask = new RoleTask();
            DataTable dtroles = new DataTable();
            dtroles = objRoleTask.GetRoles();
            

            cmbrole.DataSource = dtroles;
            cmbrole.DataTextField = "RoleDescription";
            cmbrole.DataValueField = "RoleID"; 
            cmbrole.DataBind();

        }
        ///// <summary>
        ///// This function moves listbox items from one list box to another list box.
        ///// </summary>
        ///// <param name="fromList"></param>
        ///// <param name="toList"></param>
        ///// <param name="moveAll"></param>
        //private void MoveListItems(ListBox fromList, ListBox toList, bool moveAll)
        //{
        //    RoleTask objRoleTask = new RoleTask();
        //    StringBuilder sbScript = new StringBuilder();
           
        //    try
        //    {
        //        foreach (ListItem objLi in new ArrayList(fromList.Items))
        //        {
        //            if (moveAll)
        //            {
        //                toList.Items.Add(objLi);
        //            }
        //            else
        //            {
        //                if (objLi.Selected == true)
        //                {
        //                    toList.Items.Add(objLi);
        //                    fromList.Items.Remove(objLi);
        //                }
        //            }
        //        }
        //        if (moveAll)
        //        {
        //            fromList.Items.Clear();
        //            toList.SelectedIndex = 0;
        //        }
        //        else
        //        {
        //            toList.ClearSelection();
        //        }
        //        if (fromList.Items.Count > 0)
        //        {
        //            fromList.SelectedIndex = 0;
        //        }
        //        if (lstbAllTasks.Items.Count == 0)
        //        {
        //            btnAdd.Enabled = false;
        //        }
        //        else
        //            btnAdd.Enabled = true;

        //        if (lstbAttachedTasks.Items.Count == 0)
        //        {
        //            btnRemove.Enabled = false;
        //        }
        //        else
        //            btnRemove.Enabled = true;
        //   }
        //    catch (Exception objException)
        //    {
        //        if (Session[SessionConstants.USER_ID] != null)
        //        {
        //            Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("attach_tasks - MoveListItems", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
        //        }
        //        throw objException;
        //    }
        //}

        /// <summary>
        /// Register JS variables, client side button click events
        /// </summary>
        private void registerJavascriptVariables()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("var hdnDataChangedClientID = '" + hdnDataChanged.ClientID + "';");
            sbScript.Append("var btnAddClientID = '" + btnAdd.ClientID + "';");
            sbScript.Append("var btnRemoveClientID = '" + btnRemove.ClientID + "';");
            sbScript.Append("var cmbroleClientID = '" + cmbrole.ClientID + "';");
            sbScript.Append("var hdnRoleClientID = '" + hdnRole.ClientID + "';");
            sbScript.Append("var lstbAllTasksClientID = '" + lstbAllTasks.ClientID + "';");
            sbScript.Append("var lstbAttachedTasksClientID = '" + lstbAttachedTasks.ClientID + "';");
            sbScript.Append("var chkSelectAllTasksClientID = '" + chkSelectAllTasks.ClientID + "';");
            sbScript.Append("var chkAssignedAllClientID = '" + chkAssignedAll.ClientID + "';");
            sbScript.Append("var hdTasksClientID = '" + hdTasks.ClientID + "';");
            
            ClientScript.RegisterStartupScript(Page.GetType(), "scriptDeviceClientIDs", sbScript.ToString(), true);
            chkSelectAllTasks.Attributes.Add("onclick", "SelectAllTasks('" + chkSelectAllTasks.ClientID + "','" + lstbAllTasks.ClientID + "','0','" + chkAssignedAll.ClientID + "','" + lstbAttachedTasks.ClientID + "');");
            chkAssignedAll.Attributes.Add("onclick", "SelectAllTasks('" + chkAssignedAll.ClientID + "','" + lstbAttachedTasks.ClientID + "','1','" + chkSelectAllTasks.ClientID + "','" + lstbAllTasks.ClientID + "');");
            btnAdd.Attributes.Add("onclick", "JavaScript:return btnAdd_Click();");
            btnRemove.Attributes.Add("onclick", "JavaScript:return btnRemove_Click();formDataChange('true');");
            cmbrole.Attributes.Add("onchange", "Javascript:return onComboChange();");
            lstbAttachedTasks.Attributes.Add("onclick", "EnableAddOrRemove('" + lstbAttachedTasks.ClientID + "','" + lstbAllTasks.ClientID + "','" + btnRemove.ClientID + "','" + btnAdd.ClientID + "','" + chkAssignedAll.ClientID + "');");            
            lstbAllTasks.Attributes.Add("onclick", "EnableAddOrRemove('" + lstbAllTasks.ClientID + "','" + lstbAttachedTasks.ClientID + "','" + btnAdd.ClientID + "','" + btnRemove.ClientID + "','" + chkSelectAllTasks.ClientID + "');");

        }
        private void enableControls()
        {
            if (lstbAllTasks.Items.Count > 0)
            {
                btnAdd.Enabled = true;
                chkSelectAllTasks.Enabled = true;
            }
            else
            {
                btnAdd.Enabled = false;
                chkSelectAllTasks.Enabled = false;
            }

            if (lstbAttachedTasks.Items.Count > 0)
            {
                btnRemove.Enabled = true;
                chkAssignedAll.Enabled = true;
            }
            else
            {
                btnRemove.Enabled = false;
                chkAssignedAll.Enabled = false;
            }
        }

        #endregion


}
}
