#region File History

/******************************File History***************************
 * File Name        : directory_maintenance.aspx.cs
 * Author           : 
 * Created Date     : 
 * Purpose          : 
 *                  : 
 *                  :

 * *********************File Modification History*********************
 *
 * Date(mm-dd-yyyy) Developer Reason of Modification
 *   
 * ------------------------------------------------------------------- 
 *  27-03-2008 - IAK    - Directory and institution name session setting Defect 2959
 *  12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 *  13 Nov 2008 - Prerak - Defect #3637 Performance issue solved. 
 *  18 NOv 2008 - Prerak - Defect #4166 Sorting not working on OC Directory. Fixed 
 * ------------------------------------------------------------------- */
#endregion


using System;
using System.Web;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Data.Common;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;

using Vocada.CSTools.Common;
using Vocada.CSTools.DataAccess;
using Vocada.VoiceLink.Utilities;

namespace Vocada.CSTools
{
    /// <summary>
    /// The purpose of this class is to fetch the directory information and 
    /// populate it in the data grid. This class also provides the links to 
    /// search the directory by name or alphabet.
    /// </summary>
    public partial class DirectoryMaintenance : System.Web.UI.Page
    {
        #region Private Fields
        private DataSet ds;
        private const string DIR_NAME = "DirectoryName";
        private const string INSTI_NAME = "InstitutionName";
        #endregion Private Fields

        #region Protected Fields
        protected string strUserSettings = "NO";
        #endregion

        #region constant
        private const string VL_OC = "OrderingClinician";
        private const string SUBSCRIBER_INFO = "SubscriberInfo";
        #endregion constant

        #region Page_load
        /// <summary>
        /// Page load will handle required settings when the page loads.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (Session[SessionConstants.USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
                    Response.Redirect(Utils.GetReturnURL("default.aspx", "directory_maintenance.aspx", this.Page.ClientQueryString));

                ScriptManager.RegisterStartupScript(UpdatePanelMessageList,UpdatePanelMessageList.GetType(),"HideDiv", "<script language=" + '"' + "Javascript" + '"' + ">document.getElementById(" + '"' + "PhysiciansDiv" + '"' + ").style.border='none'" + ";</script>",false);

                this.Form.DefaultButton = this.btnSearch.UniqueID;


                if (!Page.IsPostBack)
                {
                    if (Request["DirectoryID"] != null)
                    {
                        Session[SessionConstants.DIRECTORY_ID] = Request["DirectoryID"];
                    }
                    if (Request[DIR_NAME] != null)
                    {
                        Session[SessionConstants.DIRECTORY_NAME] = Request[DIR_NAME];
                    }
                    if (Request[INSTI_NAME] != null)
                    {
                        Session[SessionConstants.INSTITUTION_NAME] = Request[INSTI_NAME];
                    }

                    if (Session[SessionConstants.DIRECTORY_NAME] != null && Session[SessionConstants.INSTITUTION_NAME] != null)
                    {
                        lblDirectoryInfoLine.Text = Session[SessionConstants.INSTITUTION_NAME] + ": " + Session[SessionConstants.DIRECTORY_NAME];
                        hlinkUserMaintenance.NavigateUrl += "?DirectoryName=" + Session[SessionConstants.DIRECTORY_NAME].ToString() + "&DirectoryID=" + Session[SessionConstants.DIRECTORY_ID].ToString() + "&InstitutionName=" + Session[SessionConstants.INSTITUTION_NAME].ToString();
                    }
                    else
                    {
                        lblDirectoryInfoLine.Text = "";
                        string url = "./add_directory.aspx";
                        ScriptManager.RegisterClientScriptBlock(UpdatePanelMessageList,UpdatePanelMessageList.GetType(),"Navigate", "<script type=\'text/javascript\'>Navigate('" + url + "');</script>",false );
                        return;
                    }

                    resetLinkButtonColor();

                    LinkButton lnkSearch = (Session["SearchAlphabet"] as LinkButton);
                    if (Request["Search"] == null || Request["Search"] == "")
                    {
                        /* Load only newly added ref. physician.*/
                        if (Request["RefId"] != null)
                        {
                            string refId = Request["RefId"].ToString();
                            if (refId.Length > 0)
                            {
                                populateDirectoryByRefId(Convert.ToInt32(refId));
                            }
                        }
                    }
                    else
                    {
                        string startingWith = Request["Search"].ToString();
                        if (Session["txtSearch"] != null && Convert.ToBoolean(Session["txtSearch"]) == true)
                        {
                            txtSearch.Text = Session["txtSearchValue"].ToString();
                            btnSearch_Click(btnSearch, new EventArgs());
                        }
                        else if (startingWith.Length>0)
                        {
                            txtSearch.Text = startingWith;
                            Session["SearchAlphabet"] = null;
                            lnkSearch = null;
                            btnSearch_Click(btnSearch, e);
                        }
                        if (lnkSearch != null)
                        {
                            ScriptManager.RegisterStartupScript(UpdatePanelMessageList,UpdatePanelMessageList.GetType(), "LinkColor", "<script language=" + '"' + "Javascript" + '"' + ">changeLinkColor('" + lnkSearch.ClientID + "');</script>",false);
                        }
                    }
                   
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(Request["__EVENTTARGET"].ToString(), "dgPhysicians"))
                {
                    dgPhysicians.DataSource = null;
                    dgPhysicians.DataSource = new DataTable();
                    dgPhysicians.DataBind();
                }

                txtSearch.Focus();
                Session["CurrentTab"] = "Directory";
                /* on refresh page should not load records with previously give search criteria*/
                Session["CurrentPage"] = "directory_maintenance.aspx";
                /* Load directory page with no records*/
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.DIRECTORY_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("DirectoryMaintenance.Page_Load:: Exception occured for Directory ID - " + Session[SessionConstants.DIRECTORY_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.DIRECTORY_ID]));
                throw ex;
            }
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// When user enters search criteria into the textbox and click on Search button this method will fire 
        /// "VOC_VLR_getDirectoryPhysiciansByStartingWith" stored procedure passing the directory Id and the search criteria.
        /// If user doesn't enter any search criteria this method will load all the records for that directory.
        /// </summary>
        /// <param name="startingWith"></param>
        private void populatePhysicians(string startingWith)
        {
            Directory objOCDir = new Directory();
            try
            {
                DataTable dt = objOCDir.PopulatePhysiciansByStartingWith(Convert.ToInt32(Session[SessionConstants.DIRECTORY_ID].ToString()), startingWith.Substring(0, 1));
                Session["DirectoryPhysicians"] = dt;
                ds = new DataSet();
                ds.Tables.Add(dt);

                /* Compared string length with 0 instead of comparing it with String.Empty*/
                if (lblTotalRecords.Text.Trim().Length > 0)
                {
                    lblTotalRecords.Text = string.Empty;
                }

                if (Cache.Get("RefPhyDataSet") != null)
                {
                    Cache.Remove("RefPhyDataSet");
                }
                Cache.Add("RefPhyDataSet", ds, null, DateTime.Now.AddHours(1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
                if (ds.Tables[0].Rows.Count > 1)
                    dgPhysicians.AllowSorting = true;
                else
                    dgPhysicians.AllowSorting = false;
                
                dgPhysicians.DataSource = ds.Tables[0].DefaultView;
                dgPhysicians.DataBind();
                setDatagridHeight();
                if (dt.Rows.Count <= 1)
                {
                    lblTotalRecords.Text = dt.Rows.Count.ToString() + " Record";
                }
                else
                {
                    lblTotalRecords.Text = dt.Rows.Count.ToString() + " Records";
                }
                if (dt.Rows.Count == 0)
                    lblNoRecordFound.Text = "No Records available";
                else
                    lblNoRecordFound.Text = "";
                objOCDir = null;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.DIRECTORY_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("DirectoryMaintenance.populatePhysicians:: Exception occured for Directory ID - " + Session[SessionConstants.DIRECTORY_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.DIRECTORY_ID]));
                throw ex;
            }

        }

        /// <summary>
        /// This method will set the height of datagrid dynamically accordingly the current rowcount of datagrid,
        /// each time when the page posts back. 
        /// </summary>
        private void setDatagridHeight()
        {
            string script = "";
            script += "if(document.getElementById(" + '"' + "PhysiciansDiv" + '"' + ") != null){document.getElementById(" + '"' + "PhysiciansDiv" + '"' + ").style.height=setHeightOfGrid('" + dgPhysicians.ClientID + "','" + 40 + "');}";
            script += "document.getElementById(" + '"' + "PhysiciansDiv" + '"' + ").style.border='solid 1'" + ";";
            ScriptManager.RegisterStartupScript(UpdatePanelMessageList, UpdatePanelMessageList.GetType(), "SetHeight", script, true);
        }

        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);

        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgPhysicians.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgPhysicians_ItemDataBound);

        }
        #endregion

        #region Protected Events
        /// <summary>
        /// This event will be get fired when the user clicks on any link, this method will fetch the records in datagrid
        /// for selected link.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Session["txtSearch"] = false;
            txtSearch.Text = "";
            lblSearchResult.Text = string.Empty;
            hidAlphabetSelected.Value = (sender as LinkButton).Text;
            populatePhysicians(hidAlphabetSelected.Value);
            Session["SearchAlphabet"] = sender;

            dgPhysicians.DataBind();
            dgPhysicians.SelectedIndex = 0;
            setDatagridHeight();
            resetLinkButtonColor();
            (sender as LinkButton).Font.Bold = true;
            (sender as LinkButton).ForeColor = Color.Brown;

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
        /// This event will be get fired when user enters search criteria in the textbox and then press the search button.
        /// It will call populatePhysicians method to populate the result in the datagrid for the search criteria entered
        /// by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                Session["txtSearch"] = true;
                Session["txtSearchValue"] = txtSearch.Text;
                Session["SearchAlphabet"] = "";
                resetLinkButtonColor();
                if (txtSearch.Text.Trim().Length == 0)
                {
                    divLinks.Visible = true;
                    populatePhysicians("$");
                    lblSearchResult.Text = string.Empty;
                    txtSearch.Text = string.Empty;
                    return;
                }

                dgPhysicians.CurrentPageIndex = 0;

                Directory objOCDir = new Directory();
                DataTable dt = objOCDir.PopulatePhysiciansBySearchTerm(Convert.ToInt32(Session[SessionConstants.DIRECTORY_ID].ToString()), txtSearch.Text);
                Session["DirectoryPhysicians"] = dt;
                ds = new DataSet();
                ds.Tables.Add(dt);

                if (Cache.Get("RefPhyDataSet") != null)
                {
                    Cache.Remove("RefPhyDataSet");
                }

                /* Compared string length with 0 instead of comparing it with String.Empty */
                if (lblTotalRecords.Text.Trim().Length > 0)
                {
                    lblTotalRecords.Text = string.Empty;
                }
                Cache.Add("RefPhyDataSet", ds, null, DateTime.Now.AddHours(1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
                dgPhysicians.DataSource = ds.Tables[0].DefaultView;
                dgPhysicians.DataBind();
                setDatagridHeight();
                if (dt.Rows.Count <= 1)
                {
                    lblTotalRecords.Text = dt.Rows.Count.ToString() + " Record";
                }
                else
                {
                    lblTotalRecords.Text = dt.Rows.Count.ToString() + " Records";
                }

                if (dt.Rows.Count == 0)
                    lblNoRecordFound.Text = "No Records available";
                else
                    lblNoRecordFound.Text = "";
                lblSearchResult.Text = "Search results for '" + txtSearch.Text + "'";

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.DIRECTORY_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("DirectoryMaintenance.btnSearch_Click:: Exception occured for Directory ID - " + Session[SessionConstants.DIRECTORY_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.DIRECTORY_ID]));
                throw ex;
            }
        }

        /// <summary>
        /// Retrieves the referring physician data from database by referring physician Id and
        /// populates the data in grid.
        /// </summary>
        /// <param name="refId"></param>
        private void populateDirectoryByRefId(int refId)
        {
            try
            {
                Session["txtSearch"] = false;
                Session["txtSearchValue"] = "";
                Session["SearchAlphabet"] = "";
                resetLinkButtonColor();
                divLinks.Visible = true;
                lblSearchResult.Text = string.Empty;
                txtSearch.Text = string.Empty;

                dgPhysicians.CurrentPageIndex = 0;

                int groupId = 0;

                if (Session[SessionConstants.DIRECTORY_ID] != null)
                    groupId = Convert.ToInt32(Session[SessionConstants.DIRECTORY_ID].ToString());

                Directory objOCDir = new Directory();
                DataTable dt = objOCDir.GetReferringPhysicianByID(refId, groupId);

                Session["DirectoryPhysicians"] = dt;
                ds = new DataSet();
                ds.Tables.Add(dt);

                if (Cache.Get("RefPhyDataSet") != null)
                {
                    Cache.Remove("RefPhyDataSet");
                }

                /* Compared string length with 0 instead of comparing it with String.Empty */
                if (lblTotalRecords.Text.Trim().Length > 0)
                {
                    lblTotalRecords.Text = string.Empty;
                }
                Cache.Add("RefPhyDataSet", ds, null, DateTime.Now.AddHours(1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
                dgPhysicians.DataSource = ds.Tables[0].DefaultView;
                dgPhysicians.DataBind();
                setDatagridHeight();
                if (dt.Rows.Count <= 1)
                {
                    lblTotalRecords.Text = dt.Rows.Count.ToString() + " Record";
                }
                else
                {
                    lblTotalRecords.Text = dt.Rows.Count.ToString() + " Records";
                }
                if (dt.Rows.Count == 0)
                    lblNoRecordFound.Text = "No Records available";
                else
                    lblNoRecordFound.Text = "";
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.DIRECTORY_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("DirectoryMaintenance.populateDirectoryByRefId: Exception occured for Directory ID - " + Session[SessionConstants.DIRECTORY_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.DIRECTORY_ID]));
                throw ex;
            }
        }

        #endregion

        #region Grid Events
        /// <summary>
        /// This event will be get fired whenever the datagrid will be bound to dataset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgPhysicians_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
            {
                DataRowView data = e.Item.DataItem as DataRowView;
                if (data["VoiceOverURL"] != System.DBNull.Value)
                {
                    string voiceURL = (string)data["VoiceOverURL"];
                    bool link = voiceURL.StartsWith("http");
                    if (link)
                    {
                        e.Item.Cells[3].Text = "<a href='" + voiceURL + "'><img src='./img/ic_play_msg.gif' border='0'></a>";
                    }
                    else
                    {
                        e.Item.Cells[3].Text = voiceURL;
                    }
                }

                if (data["PrimaryPhone"] != System.DBNull.Value)
                {
                    string phone = Utils.expandPhoneNumber((string)data["PrimaryPhone"]);
                    e.Item.Cells[6].Text = phone;
                }
            }
        }

        /// <summary>
        /// This event will be fired when the user selects to change the page for the recordset more than pagesize.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgPhysicians_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            try
            {
                dgPhysicians.CurrentPageIndex = e.NewPageIndex;
                hidPageIndex.Value = dgPhysicians.CurrentPageIndex.ToString();
                ds = Cache.Get("RefPhyDataSet") as DataSet;
                if (ds == null)
                    populatePhysicians(hidAlphabetSelected.Value);
                else
                {
                    dgPhysicians.DataSource = ds.Tables[0].DefaultView;
                    dgPhysicians.DataBind();
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.DIRECTORY_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("DirectoryMaintenance.dgPhysicians_PageIndexChanged:: Exception occured for Directory ID - " + Session[SessionConstants.DIRECTORY_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.DIRECTORY_ID]));
            }

        }

        /// <summary>
        /// This event will be get fired when user clicks on the sortable header of the datagrid column to sort ascending or descending
        /// on the basis of the columnname.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgPhysicians_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            try
            {
                if (Session["DirectoryPhysicians"] != null)
                {
                    dgPhysicians.CurrentPageIndex = Convert.ToInt32(hidPageIndex.Value);
                    DataTable dtsrtDirectoryPhysicians = (DataTable)Session["DirectoryPhysicians"];

                    DataView dvsrtDirectoryPhysicians = new DataView(dtsrtDirectoryPhysicians);

                    if (Session["ColumnName"] == e.SortExpression.ToString() && Session["Direction"] == "ASC")
                    {
                        dvsrtDirectoryPhysicians.Sort = e.SortExpression + " DESC";
                        Session["Direction"] = "DESC";
                    }
                    else if (Session["ColumnName"] == e.SortExpression.ToString() && Session["Direction"] == "DESC")
                    {
                        dvsrtDirectoryPhysicians.Sort = e.SortExpression + " ASC";
                        Session["Direction"] = "ASC";
                    }
                    else
                    {
                        dvsrtDirectoryPhysicians.Sort = e.SortExpression + " ASC";
                        Session["Direction"] = "ASC";
                        Session["ColumnName"] = e.SortExpression.ToString();
                    }

                    dgPhysicians.DataSource = dvsrtDirectoryPhysicians;
                    dgPhysicians.DataBind();
                }
                setDatagridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.DIRECTORY_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("DirectoryMaintenance.dgPhysicians_SortCommand:: Exception occured for Directory ID - " + Session[SessionConstants.DIRECTORY_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.DIRECTORY_ID]));
            }
        }
        #endregion
    }
}
