#region File History

/******************************File History***************************
 * File Name        : Role_task.aspx.cs
 * Author           : Prerak Shah.
 * Created Date     : 11-07-2007
 * Purpose          : This Class will display Roles and edit Role.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

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
    public partial class role_task : System.Web.UI.Page
    {
        #region Private Fields
        private const string DIRECTION ="Direction";
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[SessionConstants.LOGGED_USER_ID] == null)
                Response.Redirect(Utils.GetReturnURL("default.aspx", "role_task.aspx", this.Page.ClientQueryString));
            registerJavascriptVariables();
            if (!IsPostBack)
            {
                Session[SessionConstants.WEEK_NUMBER] = null;
                Session[SessionConstants.SHOWMESSAGES] = null;
                Session[SessionConstants.STATUS] = null;
                Session[SessionConstants.GROUP] = null;

                populateRole();

                if (Session[DIRECTION] == null)
                    Session[DIRECTION]  = "ASC";
               
            }
            Session[SessionConstants.CURRENT_TAB] = "SystemAdmin";
            Session[SessionConstants.CURRENT_INNER_TAB] = "RolesandTasks";
            Session[SessionConstants.CURRENT_PAGE] = "role_task.aspx";
            setDatagridHeight();

        }
        /// <summary>
        /// This event save the upated role description to the data base.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgRoles_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            int roleID = Convert.ToInt32(e.Item.Cells[0].Text.ToString());
            string roleDesription = hdnGridRoleDesc.Value; 
            RoleTask objRoleTask = new RoleTask();
            try
            {

                objRoleTask.UpdateRole(roleID, roleDesription);
                dgRoles.EditItemIndex = -1;
                populateRole();
                hdnRoleDataChanged.Value = "false";
                hdnGridRoleDesc.Value = "-1";
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("role_task.dgRoles_UpdateComand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));

            }
        }

        protected void dgRoles_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            dgRoles.EditItemIndex = e.Item.ItemIndex;
            populateRole();           
        }
        protected void dgRoles_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            dgRoles.EditItemIndex = -1;// e.Item.ItemIndex;
            populateRole();
            hdnRoleDataChanged.Value = "false";
            hdnGridRoleDesc.Value = "-1";
        }
       
        /// <summary>
        /// This event will be get fired when user clicks on the sortable header of the datagrid column to sort ascending or descending
        /// on the basis of the columnname.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgRoles_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            try
            {
                if (Session["DtRoles"] != null)
                {   
                    dgRoles.CurrentPageIndex = Convert.ToInt32(hidPageIndex.Value);
                    DataTable dt = Session["DtRoles"] as DataTable;

                    DataView dvsrtRoles = new DataView(dt);

                    if (Session["ColumnName"].ToString()  == e.SortExpression.ToString() && Session[DIRECTION].ToString()  == "ASC")
                    {
                        dvsrtRoles.Sort = e.SortExpression + " DESC";
                        Session[DIRECTION] = "DESC";
                    }
                    else if (Session["ColumnName"].ToString() == e.SortExpression.ToString() && Session[DIRECTION].ToString() == "DESC")
                    {
                        dvsrtRoles.Sort = e.SortExpression + " ASC";
                        Session[DIRECTION] = "ASC";
                    }
                    else
                    {
                        dvsrtRoles.Sort = e.SortExpression + " DESC";
                        Session[DIRECTION] = "DESC";
                        Session["ColumnName"] = e.SortExpression.ToString();
                    }
                    dgRoles.EditItemIndex = -1;
                    dgRoles.DataSource = dvsrtRoles;
                    dgRoles.DataBind();
                }
                setDatagridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("role_task.dgRoles_SortCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
            }
        }
        protected void dgRoles_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView data = e.Item.DataItem as DataRowView;
                string roleid = data["RoleID"].ToString();
               
                HyperLink alink;
                alink = (HyperLink)e.Item.FindControl("lnkRoleDesc");
                alink.NavigateUrl = "./assign_tasks.aspx?RoleID=" + roleid;
            }
        }
        /// <summary>
        /// Cancel Editing of the selected record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgRoles_OnItemCreated(object source, DataGridItemEventArgs e)
        {
            try
            {
                string script = "javascript:";
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    if (e.Item.Cells[1].Controls[1] is TextBox)
                    {
                        TextBox txtRole = (e.Item.Cells[1].Controls[1]) as TextBox;
                        txtRole.EnableViewState = true;
                        txtRole.Attributes.Add("onblur", "JavaScript:CheckMaxLenght('" + txtRole.ClientID + "',100);");
                        txtRole.Attributes.Add("onchange", "Javascript:formDataChange('true');");
                        LinkButton lbUpdate = (e.Item.Cells[2].Controls[0]) as LinkButton;
                        LinkButton lbCancel = (e.Item.Cells[2].Controls[2]) as LinkButton;
                        if (e.Item.ItemIndex + 2 < 10)
                        {
                            lbUpdate.OnClientClick = script + "setRoleDesc('" + txtRole.ClientID + "','" + hdnGridRoleDesc.ClientID + "'); __doPostBack('ctl00$ContentPlaceHolder1$dgRoles$ctl0" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                            lbCancel.OnClientClick = script + "__doPostBack('ctl00$ContentPlaceHolder1$dgRoles$ctl0" + (e.Item.ItemIndex + 2) + "$ctl01', '');";
                        }
                        else
                        {
                            lbUpdate.OnClientClick = script + "setRoleDesc('" + txtRole.ClientID + "','" + hdnGridRoleDesc.ClientID + "'); __doPostBack('ctl00$ContentPlaceHolder1$dgRoles$ctl" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                            lbCancel.OnClientClick = script + "__doPostBack('ctl00$ContentPlaceHolder1$dgRoles$ctl" + (e.Item.ItemIndex + 2) + "$ctl01', '');";
                        }
                    }
                }
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lbtnEdit = (e.Item.Cells[2].Controls[0]) as LinkButton;
                    script += "if(confirmOnDataChange()){";

                    if (e.Item.ItemIndex + 2 < 10)
                    {
                        script += "__doPostBack('ctl00$ContentPlaceHolder1$dgRoles$ctl0" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                    }
                    else
                    {
                        script += "__doPostBack('ctl00$ContentPlaceHolder1$dgRoles$ctl" + (e.Item.ItemIndex + 2) + "$ctl00', '');";
                    }
                    lbtnEdit.OnClientClick = script + "} else {return false;}";
                    
                    //HyperLink alink;
                    //alink = (HyperLink)e.Item.FindControl("lnkRoleDesc");

                    //alink.Attributes.Add("onclick", "Javascript:formDataChange('true');"); 

                }
                if (e.Item.ItemType == ListItemType.Header)
                {
                    if (e.Item.Cells[1].Controls.Count > 0)
                    {
                        LinkButton lbtnDirSort = (e.Item.Cells[1].Controls[0]) as LinkButton;
                        script += "if(confirmOnDataChange()){";
                        script += "__doPostBack('ctl00$ContentPlaceHolder1$dgRoles$ctl01$ctl00', '');";
                        script += "} else {return false;}";
                        lbtnDirSort.OnClientClick = script;
                    }
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("role_task.dgRoles_OnItemCreated", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        #endregion

        #region Private Methods
        private void populateRole()
        {
            RoleTask objRoleTask = new RoleTask();;
            try
            {
               DataTable dtRoles = objRoleTask.GetRoles();
               Session["DtRoles"] = dtRoles;
              
               DataView dvRoles = new DataView(dtRoles);
               dvRoles.Sort = "RoleDescription " + Session[DIRECTION];
               dgRoles.DataSource = dvRoles;
               dgRoles.DataBind();
               setDatagridHeight();
            }
            catch(Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("role_task.populateRoles:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
        }
        /// <summary>
        /// This method will set the height of datagrid dynamically accordingly the current rowcount of datagrid,
        /// each time when the page posts back. 
        /// </summary>
        private void setDatagridHeight()
        {

           int nheight = 25;
            if (dgRoles.Items.Count < 13)
            {
                if (dgRoles.Items.Count == 0)
                {
                    nheight = 35;
                }
                else if (dgRoles.Items.Count < 3)
                {
                    nheight = ((dgRoles.Items.Count) * 25) + 28;
                }
                else
                {
                     nheight = ((dgRoles.Items.Count) * 25) + 25;
                }
            }
            else if (dgRoles.Items.Count > 12)
            {
                nheight = 13 * 25;
            }

            string uId = this.UniqueID;
            string newUid = uId.Replace(":", "_");
            StringBuilder acScript = new StringBuilder();
            acScript.Append("<script type=\"text/javascript\">");
            acScript.AppendFormat("document.getElementById(" + '"' + "phRoles" + '"' + ").style.height='" + (nheight + 1) + "';");
            acScript.Append("</script>");
            Page.RegisterStartupScript(newUid, acScript.ToString());
        }
        /// <summary>
        /// Register JS variables, client side button click events
        /// </summary>
        private void registerJavascriptVariables()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var hdnRolesDataChangedClientID = '" + hdnRoleDataChanged.ClientID + "';");
            sbScript.Append("var hdnEditedTextClientID = '" + hdnEditedText.ClientID + "';");
            sbScript.Append("var hdnGridRoleDescClientID = '" + hdnGridRoleDesc.ClientID + "';");
            
            sbScript.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());
        }

        #endregion
        
}
}