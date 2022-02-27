#region File History

/******************************File History***************************
 * File Name        : institution_information.aspx.cs
 * Author           : Prerak Shah.
 * Created Date     : 5 July 07
 * Purpose          : Display list of all Institution and perform operation on it.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * * Date(dd-mm-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 *  23-07-2007 - Change Data Grid column and added javascript for Navigate pages.
 *  27-11-2007 - Modified funcion setDatagridHeight to set height of grid for diff resolution
 *  28-11-2007   IAK     Modified function setDataGridHeight()
 * 12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
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
    public partial class institution_information : System.Web.UI.Page
    {
        #region Private Fields
        private DataSet ds;
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session[SessionConstants.LOGGED_USER_ID] == null  )
                Response.Redirect(Utils.GetReturnURL("default.aspx", "institution_information.aspx", this.Page.ClientQueryString));//|| Convert.ToInt32(Session[SessionConstants.ROLE_ID])!= UserRoles.SystemAdmin.GetHashCode()

            if (!IsPostBack)
            {
                Session[SessionConstants.WEEK_NUMBER] = null;
                Session[SessionConstants.SHOWMESSAGES] = null;
                Session[SessionConstants.STATUS] = null;
                Session[SessionConstants.GROUP] = null;

                populateInstitution();
                setDatagridHeight();
            }
            setDatagridHeight();
            Session[SessionConstants.CURRENT_TAB] = "SystemAdmin";
            Session[SessionConstants.CURRENT_INNER_TAB] = "Institution Information";
            Session[SessionConstants.CURRENT_PAGE] = "institution_information.aspx";
        }
        /// <summary>
        /// This event will be get fired when user clicks on the sortable header of the datagrid column to sort ascending or descending
        /// on the basis of the columnname.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgInstitution_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
              try
            {
                if (Session["DtInstitutions"] != null)
                {
                    dgInstitution.CurrentPageIndex = Convert.ToInt32(hidPageIndex.Value);
                    DataTable dt = Session["DtInstitutions"] as DataTable;
                    
                    DataView dvsrtInstitutions = new DataView(dt);

                    if(Session["ColumnName"] == e.SortExpression.ToString() && Session["Direction"] == "ASC")
                    {
                        dvsrtInstitutions.Sort = e.SortExpression + " DESC";
                        Session["Direction"] = "DESC";
                    }
                    else if(Session["ColumnName"] == e.SortExpression.ToString() && Session["Direction"] == "DESC")
                    {
                        dvsrtInstitutions.Sort = e.SortExpression + " ASC";
                        Session["Direction"] = "ASC";
                    }
                    else
                    {
                        dvsrtInstitutions.Sort = e.SortExpression + " DESC";
                        Session["Direction"] = "DESC";
                        Session["ColumnName"] = e.SortExpression.ToString();
                    }

                    dgInstitution.DataSource = dvsrtInstitutions;
                    dgInstitution.DataBind();
                }
                setDatagridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("SystemAdmin.dgInstitution_SortCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
            }
        }

        protected void dgInstitution_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            try
            {
                dgInstitution.CurrentPageIndex = e.NewPageIndex;
                hidPageIndex.Value = dgInstitution.CurrentPageIndex.ToString();
                ds = Cache.Get("InstitutionDataSet") as DataSet;
                if (ds == null)
                    populateInstitution();
                else
                {
                    dgInstitution.DataSource = ds.Tables[0].DefaultView;
                    dgInstitution.DataBind();
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("SystemAdmin.dgInstitution_PageIndexChanged:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
            }
        }
        #endregion

        #region Private Methods
        private void populateInstitution()
        {
            Institution objInstituion = new Institution();
            DataTable dtInstitute;
            try
            {
                dtInstitute = objInstituion.GetInstitutionList();
                Session["DtInstitutions"] = dtInstitute;
                ds = new DataSet();
                ds.Tables.Add(dtInstitute);
                if (Cache.Get("InstitutionDataSet") != null)
                {
                    Cache.Remove("InstitutionDataSet");
                }
                Cache.Add("InstitutionDataSet", ds, null, DateTime.Now.AddHours(1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
                dgInstitution.DataSource = ds.Tables[0].DefaultView;
                if (dtInstitute.Rows.Count > 1)
                    dgInstitution.AllowSorting = true;
                else
                    dgInstitution.AllowSorting = false;
                dgInstitution.DataBind();
                setDatagridHeight();
                objInstituion = null;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("SystemAdmin.populateInstitution:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
        }
        /// <summary>
        /// This method will set the height of datagrid dynamically accordingly the current rowcount of datagrid,
        /// each time when the page posts back. 
        /// </summary>
        private void setDatagridHeight()
        {
            string newUid = this.UniqueID.Replace(":", "_");
            string script = "<script type=\"text/javascript\">";
            script += "if(document.getElementById(" + '"' + "phInstitutions" + '"' + ") != null){document.getElementById(" + '"' + "phInstitutions" + '"' + ").style.height=setHeightOfGrid('" + dgInstitution.ClientID + "','" + 40 + "');}</script>";
            ScriptManager.RegisterStartupScript(upnlInstitutionList, upnlInstitutionList.GetType(),newUid, script,false);
        }
        #endregion
    }
}