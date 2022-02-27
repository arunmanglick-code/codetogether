#region File History

/******************************File History***************************
 * File Name        : voiceover_utility.aspx.cs
 * Author           : Prerak Shah.
 * Created Date     : 20-08-2007
 * Purpose          : This Class will add voiceover for OC.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 16-10-2007   IAK     Defect 1991 Resolved 
 * 28-11-2007   IAK     Modified function setDatagridHeight()
 * 01-04-2008   IAK     Defect 2803
 * 25-04-2008   PRERAK  Defect 3070, 3074 Fixed
 * 29-04-2008   PRERAK  Defect #3072 Fixed
 * 12 Jun 2008  Prerak  Migration of AJAX Atlas to AJAX RTM 1.0
 * 24-06-2008   NDM     CR#249
 * 25-06-2008   NDM     Performance issue when changing criteria of voiceover.
 * 03-12-2008   GB      Fixed defect #3991
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
using System.Drawing;
using System.Text;
using Vocada.VoiceLink.Utilities;

namespace Vocada.CSTools
{
    public partial class voiceover_utility : System.Web.UI.Page
    {
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_INFO]== null )
            {
                Response.Redirect(Utils.GetReturnURL("default.aspx", "voiceover_utility.aspx", this.Page.ClientQueryString));
            }
            UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo; 
            try
            {
                registerJavaScript();
                ScriptManager.RegisterStartupScript(UpdatePanelAddInstitution,UpdatePanelAddInstitution.GetType(),"HideDiv", "<script language=" + '"' + "Javascript" + '"' + ">document.getElementById(" + '"' + "divOC" + '"' + ").style.border='none'" + ";</script>",false);
                if (!IsPostBack)
                {
                    Session[SessionConstants.WEEK_NUMBER] = null;
                    Session[SessionConstants.SHOWMESSAGES] = null;
                    Session[SessionConstants.STATUS] = null;
                    Session[SessionConstants.GROUP] = null;

                    tblAlphabet.Visible = false;
                    if (userInfo.RoleId  == UserRoles.InstitutionAdmin.GetHashCode())
                    {
                        cmbInstitution.Visible = false;
                        lblInstName.Visible = true;
                        lblInstName.Text = userInfo.InstitutionName;//Session[SessionConstants.INSTITUTION_NAME].ToString();
                        populateDirectories(userInfo.InstitutionID);
                    }
                    else
                    {
                        cmbInstitution.Visible = true;
                        lblInstName.Visible = false;
                        fillInstitition();
                        //if (Session[SessionConstants.INSTITUTION_ID] != null)
                        //{
                        //    cmbInstitution.SelectedValue = (Session[SessionConstants.INSTITUTION_ID].ToString());
                        //    populateDirectories(Convert.ToInt32(Session[SessionConstants.INSTITUTION_ID]));
                        //}

                       
                    }
                    

                    Session["SearchAlphabet"] = "";
                }
                Session[SessionConstants.CURRENT_TAB] = "Tools";
                Session[SessionConstants.CURRENT_INNER_TAB] = "OCVoiceOver";                
                
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("voiceover_utility - page_load", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
        }
        protected void cmbDirectory_SelectedIndexChanged(object sender, EventArgs e)
        {
           try
            {
                resetLinkButtonColor();
                lblRecordCount.Text = "";
                lblNorecord.Text = "";
                dgOC.DataSource = null;
                dgOC.DataBind();
                if (cmbDirectory.SelectedValue != "-1" && cmbInstitution.SelectedValue != "-1")
                    tblAlphabet.Visible = true;
                else
                    tblAlphabet.Visible = false;
                Utils.RegisterJS("HideDiv1", "document.getElementById(" + '"' + "divOC" + '"' + ").style.border='none'" + ";", this.Page);
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.LOGGED_USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("voiceover_utility - cmbDirectory_SelectedIndexedChanged", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            Session["SearchAlphabet"] = "";
        }
        protected void btnOCVoiceOver_Click(object sender, EventArgs e)
        {
            string startWith = "";
            resetLinkButtonColor();
            if (cmbDirectory.SelectedValue != "-1" && cmbInstitution.SelectedValue != "-1")
            {
                populateOCs(startWith);
                tblAlphabet.Visible = true; 
            }
            else
            {
                tblAlphabet.Visible = false;
                lblRecordCount.Text = "";
                lblNorecord.Text = "";
                Utils.RegisterJS("HideDiv1", "document.getElementById(" + '"' + "divOC" + '"' + ").style.border='none'" + ";", this.Page);
            }
            Session["SearchAlphabet"] = "";

        }
        protected void dgOC_SortCommand(object source,DataGridSortCommandEventArgs e )
        {
            try
            {
                dgOC.CurrentPageIndex = Convert.ToInt32(hidPageIndex.Value);
                DataTable dtOC = Session["dtOC"] as DataTable;
                DataView dvsrtOC = new DataView(dtOC);

                if (Session["ColumnName"] == e.SortExpression.ToString() && Session["Direction"] == "ASC")
                {
                    dvsrtOC.Sort = e.SortExpression + " DESC";
                    Session["Direction"] = "DESC";
                }
                else if (Session["ColumnName"] == e.SortExpression.ToString() && Session["Direction"] == "DESC")
                {
                    dvsrtOC.Sort = e.SortExpression + " ASC";
                    Session["Direction"] = "ASC";
                }
                else
                {
                    dvsrtOC.Sort = e.SortExpression + " ASC";
                    Session["Direction"] = "ASC";
                    Session["ColumnName"] = e.SortExpression.ToString();
                }

                dgOC.DataSource = dvsrtOC;
                dgOC.DataBind();

                setDatagridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("SystemAdmin.dgInstitution_SortCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
            }
        }
        protected void dgOC_ItemDataBound(object source, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView data = e.Item.DataItem as DataRowView;
                int iApprove = Convert.ToInt32(data["VoiceOverApproved"]);
                if (iApprove == 0)
                {
                    System.Web.UI.WebControls.Image img1;
                    img1 = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgApproved");
                    img1.ImageUrl = "img/False.GIF";
                    if (data["VoiceOverURL"].ToString() == "")
                    {
                        e.Item.Cells[4].Text = "";
                        e.Item.Cells[3].Text = "";
                        e.Item.Cells[5].Text = "";
                       
                    }
                }
                else
                {
                    System.Web.UI.WebControls.Image img1;
                    img1 = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgApproved");
                    img1.ImageUrl = "img/True.GIF";
                    e.Item.Cells[5].Text = "Approved";
                }
                if (data["VoiceOverURL"].ToString() == "")
                {
                    //((LinkButton)e.Item.Cells[5].FindControl("lbtnDelete")).Enabled=false;
                    e.Item.Cells[5].Text = "";
                    e.Item.Cells[3].Text = "";

                }
            }
        }
        protected void dgOC_DeleteCommand(object source, DataGridCommandEventArgs e)
        {            
            OrderingClinician objOC;
            try
            {
                if (string.Compare(e.CommandName, "Delete") == 0)
                {
                    int ocID = Convert.ToInt32(e.Item.Cells[0].Text);
                    string startWith = "";
                    objOC = new OrderingClinician();
                    objOC.DeleteOCVoiceover(ocID);
                    if (Session["SearchAlphabet"] != null)
                        startWith = Session["SearchAlphabet"].ToString();
                    populateOCs(startWith);
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("voiceover_utility.dgOC_DeleteCommand", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                objOC = null;
            }

        }
        protected void dgOC_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            OrderingClinician objOC;
            try
            {
                if (string.Compare(e.CommandName, "Update") == 0)
                {
                    int ocID = Convert.ToInt32(e.Item.Cells[0].Text);
                    string startWith = "";
                    objOC = new OrderingClinician();
                    objOC.UpdateOCVoiceover(ocID);
                    if (Session["SearchAlphabet"] != null)
                       startWith = Session["SearchAlphabet"].ToString();
                    populateOCs(startWith); 
                }
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("voiceover_utility.dgOC_DeleteCommand", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                objOC = null;
            }

        }
        /// <summary>
        /// This event will be get fired when the user clicks on any link, this method will fetch the records in datagrid
        /// for selected link.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Session["txtSearch"] = false;
            hidAlphabetSelected.Value = (sender as LinkButton).Text;
            Session["SearchAlphabet"] = hidAlphabetSelected.Value;

            resetLinkButtonColor();
            (sender as LinkButton).Font.Bold = true;
            (sender as LinkButton).ForeColor = Color.Brown;

            if (cmbDirectory.SelectedValue != "-1" && cmbInstitution.SelectedValue != "-1")
            {
                populateOCs(hidAlphabetSelected.Value);
            }
            else
            {
                lblRecordCount.Text = "";
                lblNorecord.Text = "";
                Utils.RegisterJS("HideDiv1", "document.getElementById(" + '"' + "divOC" + '"' + ").style.border='none'" + ";", this.Page);
            }

        }
        protected void cmbInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                resetLinkButtonColor(); 
                populateDirectories(Convert.ToInt32(cmbInstitution.SelectedValue));
                dgOC.DataSource = null;
                lblRecordCount.Text = "";
                lblNorecord.Text = "";
                dgOC.DataBind();
                if (cmbDirectory.SelectedValue != "-1" && cmbInstitution.SelectedValue != "-1")
                    tblAlphabet.Visible = true;
                else
                    tblAlphabet.Visible = false;
                Utils.RegisterJS("HideDiv1", "document.getElementById(" + '"' + "divOC" + '"' + ").style.border='none'" + ";", this.Page);
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.LOGGED_USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("voiceover_utility - cmbGroup_SelectedIndexedChanged", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {
                
            }
            Session["SearchAlphabet"] = "";
        }
        protected void rblApprove_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetLinkButtonColor();

            if (cmbDirectory.SelectedValue != "-1" && cmbInstitution.SelectedValue != "-1")
            {
                populateOCs("");
            }
            else
            {
                dgOC.DataSource = null;
                lblRecordCount.Text = "";
                lblNorecord.Text = "";
                dgOC.DataBind();
                Utils.RegisterJS("HideDiv1", "document.getElementById(" + '"' + "divOC" + '"' + ").style.border='none'" + ";", this.Page);
            }
            Session["SearchAlphabet"] = "";

        }
        
        #endregion Events

        #region Private Methods
        /// <summary>
        /// This Method populates Directories of Institution.
        /// </summary>
        /// <param name="institutionID"></param>
        private void populateDirectories(int institutionID)
        {
            DataTable dtDirectories = new DataTable();
            dtDirectories = Utility.GetDirectories(institutionID);
            cmbDirectory.DataSource = dtDirectories;
            cmbDirectory.DataBind();

            ListItem li = new ListItem("-- Select Directory --", "-1");
            cmbDirectory.Items.Add(li);
            cmbDirectory.Items.FindByValue("-1").Selected = true;
            if (institutionID > 0)
            {
                ListItem liAll = new ListItem("All", "0");
                cmbDirectory.Items.Insert(0, liAll);
            }
        }
        /// <summary>
        /// This Method Populate OCs as per character selected from UI
        /// </summary>
        /// <param name="startWith"></param>
        private void populateOCs(string startWith)
        {
            OrderingClinician objOC;
            DataTable dtOC;
            try
            {
                string searchTerm="";
                int directoryID = Convert.ToInt32(cmbDirectory.SelectedValue);
                int institutionID = Convert.ToInt32(cmbInstitution.SelectedValue);
                int iApprove = 2; //ALL (Approved and UnApproved )
                if (rblApprove.Items[0].Selected)
                {
                    iApprove = 1;
                }
                else if (rblApprove.Items[1].Selected)
                {
                    iApprove = 0;
                }
                else
                    iApprove = 2;

                if (startWith.Equals(""))
                    searchTerm = txtSearch.Text;
                else
                    txtSearch.Text = "";

                objOC = new OrderingClinician();
                dtOC = objOC.GetOCforVoiceover(directoryID, iApprove, startWith, searchTerm, institutionID);
                dgOC.DataSource = dtOC.DefaultView  ;
                Session["dtOC"] = dtOC;
                if (dtOC.Rows.Count > 1)
                    dgOC.AllowSorting = true;
                else
                    dgOC.AllowSorting = false;
                dgOC.DataBind();
                setDatagridHeight();
                if (directoryID == 0)
                {
                    dgOC.Columns[1].Visible = true;
                    dgOC.Columns[2].ItemStyle.Width = new Unit("50%");
                    dgOC.Columns[3].ItemStyle.Width = new Unit("10%");
                }
                else
                {
                    dgOC.Columns[1].Visible = false;
                    dgOC.Columns[2].ItemStyle.Width = new Unit("60%");
                }

                int records = dtOC.Rows.Count; 
                if (records <= 1)
                    lblRecordCount.Text = records.ToString() + " Record";
                else
                    lblRecordCount.Text = records.ToString() + " Records";
                if (records <= 0)
                    lblNorecord.Text = "No Records available";
                else
                    lblNorecord.Text = "";

            }
             catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("voiceover_utility.populateOCs:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
            finally
            {
                objOC = null;
                dtOC= null;
            }
        }
        /// <summary>
        /// This method will set the height of datagrid dynamically accordingly the current rowcount of datagrid,
        /// each time when the page posts back. 
        /// </summary>
        private void setDatagridHeight()
        {
            
            string script = "<script type=\"text/javascript\">";
            script += "PageLoad(1);if(document.getElementById(" + '"' + "divOC" + '"' + ") != null){document.getElementById(" + '"' + "divOC" + '"' + ").style.height=setHeightOfGrid('" + dgOC.ClientID + "','" + 40 + "');}";
            script += "document.getElementById(" + '"' + "divOC" + '"' + ").style.border='solid 1';</script>";
            ScriptManager.RegisterStartupScript(UpdatePanelAddInstitution, UpdatePanelAddInstitution.GetType(), "SetHeight", script, false); 
        }
        /// <summary>
        /// Resets the bold font and color of the link buttons for search.
        /// </summary>
        private void resetLinkButtonColor()
        {
            // Change in the Link Button Color Style for Black and White (View Text/Graphics Reason)
            Color objColor = Color.Blue;
            
            aA.Font.Bold = false;
            aA.ForeColor = objColor;
            aB.Font.Bold = false;
            aB.ForeColor = objColor;
            aC.Font.Bold = false;
            aC.ForeColor = objColor;
            aD.Font.Bold = false;
            aD.ForeColor = objColor;
            aE.Font.Bold = false;
            aE.ForeColor = objColor;
            aF.Font.Bold = false;
            aF.ForeColor = objColor;
            aG.Font.Bold = false;
            aG.ForeColor = objColor;
            aH.Font.Bold = false;
            aH.ForeColor = objColor;
            aI.Font.Bold = false;
            aI.ForeColor = objColor;
            aJ.Font.Bold = false;
            aJ.ForeColor = objColor;
            aK.Font.Bold = false;
            aK.ForeColor = objColor;
            aL.Font.Bold = false;
            aL.ForeColor = objColor;
            aM.Font.Bold = false;
            aM.ForeColor = objColor;
            aN.Font.Bold = false;
            aN.ForeColor = objColor;
            aO.Font.Bold = false;
            aO.ForeColor = objColor;
            aP.Font.Bold = false;
            aP.ForeColor = objColor;
            aQ.Font.Bold = false;
            aQ.ForeColor = objColor;
            aR.Font.Bold = false;
            aR.ForeColor = objColor;
            aS.Font.Bold = false;
            aS.ForeColor = objColor;
            aT.Font.Bold = false;
            aT.ForeColor = objColor;
            aU.Font.Bold = false;
            aU.ForeColor = objColor;
            aV.Font.Bold = false;
            aV.ForeColor = objColor;
            aW.Font.Bold = false;
            aW.ForeColor = objColor;
            aX.Font.Bold = false;
            aX.ForeColor = objColor;
            aY.Font.Bold = false;
            aY.ForeColor = objColor;
            aZ.Font.Bold = false;
            aZ.ForeColor = objColor;
        }
        /// <summary>
        /// This method fill the Institution Combo
        /// </summary>
        private void fillInstitition()
        {
            DataTable dtInstitution = new DataTable();
            dtInstitution = Utility.GetInstitutionList();
            cmbInstitution.DataSource = dtInstitution;
            cmbInstitution.DataBind();

            ListItem li = new ListItem("-- Select Institution --", "-1");
            cmbInstitution.Items.Add(li);
            cmbInstitution.Items.FindByValue("-1").Selected = true;

            populateDirectories(Convert.ToInt32(cmbInstitution.SelectedValue));
        }

        /// <summary>
        /// Register JS Variables
        /// </summary>
        private void registerJavaScript()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var cmbInstitutionClientID = '" + cmbInstitution.ClientID + "';");
            sbScript.Append("var cmbDirectoryClientID = '" + cmbDirectory.ClientID + "';");
            sbScript.Append("var dgOCClientID = '" + dgOC.ClientID + "';");
            sbScript.Append("var lblRecordCountClientID = '" + lblRecordCount.ClientID + "';");
            sbScript.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());
        }
        #endregion Private Methods
    }
}