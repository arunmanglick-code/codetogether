/******************************File History***************************
 * File Name        : advance_search.aspx.cs
 * Author           : Prerak Shah.
 * Created Date     : 24-12-2007
 * Purpose          : This Class will provide Message Search Criteria.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 28-12-2007 Prerak - add validation and change UI
 * 31-12-2007 Prerak - Defect Fixes  
 * 07-01-2008 Preak  - Defect 2504; window opening performance improved
 * 10-01-2008 Prerak - Reload Issue of page postback by inplementing Ajax
 * 10-01-2008 Prerak - Remove All Groups options for performace improvement  
 * 06-03-2008 IAK    - If advance search for preselected group, then disable group and msg type combo
 * 04-03-2008 Prerak - Implementing CR #197 Advanced Search for Open Reply and readBack
 * 05-15-2008 NDM    - #3033 Advanced Search for Readbacks Performance Issues
 * 06-12-2008 Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 * 11-20-2008 ZNK    - Perform validation and then Postback on return true & Defect#3338.
 * -------------------------------------------------------------------- 
 */

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
namespace Vocada.CSTools
{
    public partial class advance_search : System.Web.UI.Page
    {
        #region Constants
        private bool isSystemAdmin = true;
        private int instId = -1;
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            registerJavascripts();
            if (!IsPostBack)
            {
                ExpirePageCache();
                Search objSearch = null;
                
                if (Session[SessionConstants.SEARCH_CRITERIA] != null)
                {
                    objSearch = Session[SessionConstants.SEARCH_CRITERIA] as Search;
                    ddlistShowMessages.SelectedIndex = objSearch.GroupType;
                    fillGroups();
                    cmbGroup.SelectedValue = objSearch.GroupID.ToString();
                }
                else
                {
                   fillGroups();
                   DateTime objdt = new DateTime();
                   objdt = DateTime.Today;
                   txtToDate.Text = objdt.ToString().Substring(0, objdt.ToString().Length - 12); 
                   objdt = objdt.AddMonths(-1);
                   txtFromDate.Text = objdt.ToString().Substring(0, objdt.ToString().Length - 12);
                   controlEnabled(false);  
                }
                fillMessageStatus(); 
                fillOC();
                populateSubscribers();
                fillFindings();
                fillNurses();
                fillUnits();
                if (objSearch != null)
                {
                    if (objSearch.OCName.Length > 0)
                    {
                        cmbOC.ClearSelection();
                        cmbOC.Items.FindByText(objSearch.OCName).Selected = true;
                    }
                    if (objSearch.FindingName.Length > 0)
                    {
                        cmbFinding.ClearSelection();
                        cmbFinding.Items.FindByText(objSearch.FindingName).Selected = true;
                    }
                    txtAccession.Text = objSearch.Accession;
                    txtMRN.Text = objSearch.MRN;
                    txtDOB.Text = objSearch.DOB;
                    txtFromDate.Text = objSearch.FromDate;
                    txtToDate.Text = objSearch.ToDate;
                    if (objSearch.NurseName.Length > 0)
                    {
                        cmbNurse.ClearSelection();
                        cmbNurse.Items.FindByText(objSearch.NurseName).Selected = true;
                    }
                    if (objSearch.UnitName.Length > 0)
                    {
                        cmbUnits.ClearSelection();
                        cmbUnits.Items.FindByText(objSearch.UnitName).Selected = true; cmbRC.Text = objSearch.RCName;
                    }
                    cmbMsgStatus.SelectedValue = objSearch.MessageStatus.ToString();

                    if (objSearch.RCName.Length > 0)
                    {
                        cmbRC.ClearSelection();
                        cmbRC.Items.FindByText(objSearch.RCName).Selected = true;
                    }
                    controlEnabled(true);
                    if (cmbGroup.SelectedValue == "-1")
                        cmbFinding.Enabled = false;

                    if (Request["groupID"] != null && Request["isLabGroup"] != null)
                    {
                        ddlistShowMessages.Enabled = false;
                        cmbGroup.Enabled = false;
                    }
                }
                else if (Request["groupID"] != null && Request["isLabGroup"] != null)
                {
                    ddlistShowMessages.SelectedValue = Request["isLabGroup"];
                    ddlistShowMessages_SelectedIndexChanged(sender, e);
                    ddlistShowMessages.Enabled = false;
                    if (cmbGroup.Items.FindByValue(Request["groupID"]) != null)
                    {
                        cmbGroup.SelectedValue = Request["groupID"];
                        cmbGroup_SelectedIndexChanged(sender, e);
                        cmbGroup.Enabled = false;
                    }

                }
            }
            else if (Request["__eventargument"] == "btnSelect_Click")
            {
                btnSelect_Click(this, new EventArgs());
            }
            GroupTypeVisibility();
            ScriptManager.RegisterStartupScript(upnlControls,upnlControls.GetType(), "ControlVisibility", "ControlVisibility();", true);
            
        }

        /// <summary>
        /// This function prevent the page being retrieved from broswer cache
        /// </summary>
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            Search objSearch = new Search();
            
            objSearch.Accession = txtAccession.Text;
            objSearch.DOB = txtDOB.Text;
            if (cmbFinding.SelectedValue != "-1")
                objSearch.FindingName = cmbFinding.SelectedItem.ToString();
            else
                objSearch.FindingName = "";
            objSearch.FromDate = txtFromDate.Text;
            objSearch.GroupID = Convert.ToInt32(cmbGroup.SelectedValue);
            objSearch.GroupType = ddlistShowMessages.SelectedIndex;
            objSearch.MessageStatus = Convert.ToInt32(cmbMsgStatus.SelectedValue);
            objSearch.MRN = txtMRN.Text;
            if (cmbNurse.SelectedValue != "-1")
                objSearch.NurseName = cmbNurse.SelectedItem.ToString();
            else
                objSearch.NurseName = "";
            if (cmbOC.SelectedValue != "-1")
                objSearch.OCName = cmbOC.SelectedItem.ToString();
            else
                objSearch.OCName = "";
            if (cmbRC.SelectedValue != "-1")
                objSearch.RCName = cmbRC.SelectedItem.ToString();
            else
                objSearch.RCName = "";
            objSearch.ToDate = txtToDate.Text;
            if (cmbUnits.SelectedValue != "-1")
                objSearch.UnitName = cmbUnits.SelectedItem.ToString();
            else
                objSearch.UnitName = "";

            Session[SessionConstants.SEARCH_CRITERIA] = objSearch;

            ////refershParent();
            //string sbscript = "window.close();";
            //ScriptManager.RegisterClientScriptBlock(upnlControls, upnlControls.GetType(), "Close", sbscript, true);
              
        }
        protected void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGroup.SelectedValue != "0")
            {
                fillOC();
                populateSubscribers();
                fillFindings();
                if (ddlistShowMessages.SelectedIndex == 1)
                {
                    fillNurses();
                    fillUnits();
                }
                controlEnabled(true);
                if (cmbGroup.SelectedValue == "-1")
                    cmbFinding.Enabled = false;
                //ClientScript.RegisterClientScriptBlock(Page.GetType(), "ControlVisibility", "ControlVisibility();ControlsSetting();", true);

            }
            else
                controlEnabled(false);
            GroupTypeVisibility();
        }
        protected void ddlistShowMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            //btnGo.Enabled = false;
            //Session[SessionConstants.SHOWMESSAGES] = ddlistShowMessages.SelectedIndex.ToString();
            controlEnabled(false);
            fillMessageStatus();
            cmbGroup.Items.Clear();
            fillGroups();
            //GroupTypeVisibility();
        }         
        #endregion

        #region Private Methods
        private void registerJavascripts()
        {
            StringBuilder sbScript = new StringBuilder();

            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var txtAccessionClientID = '" + txtAccession.ClientID + "';");
            sbScript.Append("var txtDOBClientID = '" + txtDOB.ClientID + "';");
            sbScript.Append("var txtFromDateClientID = '" + txtFromDate.ClientID + "';");
            sbScript.Append("var txtMRNClientID = '" + txtMRN.ClientID + "';");
            sbScript.Append("var txtToDateClientID = '" + txtToDate.ClientID + "';");
            sbScript.Append("var cmbFindingClientID = '" + cmbFinding.ClientID + "';");
            sbScript.Append("var cmbNurseClientID = '" + cmbNurse.ClientID + "';");
            sbScript.Append("var cmbOCClientID = '" + cmbOC.ClientID + "';");
            sbScript.Append("var cmbRCClientID = '" + cmbRC.ClientID + "';");
            sbScript.Append("var cmbUnitsClientID = '" + cmbUnits.ClientID + "';");
            sbScript.Append("var lblNurseClientID = '" + lblNurse.ClientID + "';");
            sbScript.Append("var lblUnitClientID = '" + lblUnit.ClientID + "';");
            sbScript.Append("var cmbGroupClientID ='" + cmbGroup.ClientID + "';");
            sbScript.Append("var ddlistShowMessagesClientID = '" + ddlistShowMessages.ClientID + "';");
            sbScript.Append("var btnSelectClientID = '" + btnSelect.ClientID + "';");
            sbScript.Append("</script>");
           
            this.RegisterStartupScript("getClientIDs", sbScript.ToString());

            cmbUnits.Attributes.Add("onChange", "javascript:ControlVisibility();");
            cmbOC.Attributes.Add("onChange", "javascript:ControlVisibility();");
            cmbNurse.Attributes.Add("onChange", "javascript:ControlVisibility();");
            //ddlistShowMessages.Attributes.Add("onChange","javascript:ControlsSetting();");
            //cmbGroup.Attributes.Add("onChange", "javascript:doHourGlass();"); 
            anchFromDate.HRef = String.Format(@"javascript:cal1xx.select(document.all['{0}'],document.all['{1}'],'MM/dd/yyyy')",
                                                             txtFromDate.ClientID,
                                                             anchFromDate.ClientID
                                                             );

            anchToDate.HRef = String.Format(@"javascript:cal2xx.select(document.all['{0}'],document.all['{1}'],'MM/dd/yyyy')",
                                                       txtToDate.ClientID,
                                                       anchToDate.ClientID
                                                       );

            anchSearchDOB.HRef = String.Format(@"javascript:cal3xx.select(document.all['{0}'],document.all['{1}'],'MM/dd/yyyy')",
                                                       txtDOB.ClientID,
                                                       anchSearchDOB.ClientID
                                                       );

        }

        /// <summary>
        /// This method fills the Group Combo as per Institution selected and Group type selected.
        /// </summary>
        /// <param name="institutionID"></param>
        private void fillGroups()
        {
            DataTable dtGroups = new DataTable();
            int groupTypeID = ddlistShowMessages.SelectedIndex;
            int institutionID = -1;
            if (groupTypeID == 0 && ViewState[SessionConstants.RAD_GROUPS] != null)
            {
                dtGroups = (DataTable)ViewState[SessionConstants.RAD_GROUPS];
            }
            else if (groupTypeID == 1 && ViewState[SessionConstants.LAB_GROUPS] != null)
            {
                dtGroups = (DataTable)ViewState[SessionConstants.LAB_GROUPS];
            }          
            else
            {
                MessageCenter objMsgCenter = new MessageCenter();
                if (isSystemAdmin)
                    institutionID = -1;
                else
                    institutionID = instId;
                dtGroups = objMsgCenter.GetGroupsbyGroupType(groupTypeID, institutionID);

                if (groupTypeID == 0)
                {
                    ViewState[SessionConstants.RAD_GROUPS] = dtGroups;
                }
                else if (groupTypeID == 1)
                {
                    ViewState[SessionConstants.LAB_GROUPS] = dtGroups;
                }
                else if (groupTypeID == 2)
                {
                    ViewState[SessionConstants.ALL_GROUPS] = dtGroups;
                }
            }

            cmbGroup.DataSource = dtGroups;
            cmbGroup.DataBind();
            cmbGroup.Items.Insert(0, new ListItem("-- Select Group --", "0"));
            //cmbGroup.Items.Insert(1,new ListItem("All Groups", "-1"));
            cmbGroup.Items.FindByValue("0").Selected = true;
        }
        /// <summary>
        /// This Method Fill the OC combo box as per group selected.
        /// </summary>
        private void fillOC()
        {
            MessageCenter objMsgCenter = null;
            DataTable dtOCs = null;
            try
            {
                if (cmbGroup.SelectedValue != "0")
                {
                    objMsgCenter = new MessageCenter();
                    int groupid = Convert.ToInt32(cmbGroup.SelectedValue);
                    int groupType = ddlistShowMessages.SelectedIndex;
                    dtOCs = objMsgCenter.GetOCbyGroup(groupid, groupType);

                    cmbOC.DataSource = dtOCs;
                    cmbOC.DataBind();
                }
                cmbOC.ClearSelection();
                cmbOC.Items.Insert(0,new ListItem("All OC", "-1"));
                cmbOC.Items.FindByValue("-1").Selected = true;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("advance_search.fillOC", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw;
            }

            finally
            {
                objMsgCenter = null;
                dtOCs = null;
            }

        }
        /// <summary>
        /// Fill Subscriber combo box with selected Group
        /// </summary>
        private void populateSubscribers()
        {
            DataTable dtSubscribers = null;
            MessageCenter objMsgCenter = null;
            int groupID;
            try
            {
                if (cmbGroup.SelectedValue != "0" && cmbGroup.SelectedValue != "0")
                {
                    objMsgCenter = new MessageCenter();

                    //Get list of institutions
                    groupID = int.Parse(cmbGroup.SelectedValue);
                    if (groupID == 0)
                        groupID = -1;
                    int groupType = ddlistShowMessages.SelectedIndex;
                    dtSubscribers = objMsgCenter.GetGroupUsersByGroupType(groupID, groupType);

                    //Bind the datasource to combo
                    DataView dvSubscribers = dtSubscribers.DefaultView;
                    dvSubscribers.Sort = "DisplayName ASC";
                    cmbRC.Items.Clear();
                    cmbRC.DataSource = dvSubscribers;
                    cmbRC.DataTextField = "DisplayName";
                    cmbRC.DataValueField = "SubscriberID";
                    cmbRC.SelectedValue = null;
                    cmbRC.DataBind();
                }
                cmbRC.ClearSelection();
                cmbRC.Items.Insert(0, new ListItem("All Subscriber", "-1"));
                cmbRC.SelectedValue = "-1";
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("vuiErrorReport.populateSubscribers() ", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                dtSubscribers = null;
                objMsgCenter = null;
            }
        }
        /// <summary>
        /// This Method Fill the Nurse combo box as per group selected.
        /// </summary>
        private void fillNurses()
        {
            MessageCenter objMsgCenter = null;
            DataTable dtNurses = null;
            try
            {
                if (ddlistShowMessages.SelectedIndex == 1 && cmbGroup.SelectedValue != "0")
                {
                    objMsgCenter = new MessageCenter();
                    int groupid = Convert.ToInt32(cmbGroup.SelectedValue);
                    dtNurses = objMsgCenter.GetNurses(groupid);

                    cmbNurse.DataSource = dtNurses;
                    cmbNurse.DataBind();
                }
                cmbNurse.ClearSelection();
                cmbNurse.Items.Insert(0,new ListItem("All Nurses", "-1"));
                cmbNurse.Items.FindByValue("-1").Selected = true;

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("advance_search.fillNurse", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw;
            }

            finally
            {
                objMsgCenter = null;
                dtNurses = null;
            }

        }
        /// <summary>
        /// This Method Fill the Units combo box as per group selected.
        /// </summary>
        private void fillUnits()
        {
            MessageCenter objMsgCenter = null;
            DataTable dtUnits = null;
            try
            {
                if (ddlistShowMessages.SelectedIndex == 1 && cmbGroup.SelectedValue != "0")
                {
                    objMsgCenter = new MessageCenter();
                    int groupid = Convert.ToInt32(cmbGroup.SelectedValue);
                    dtUnits = objMsgCenter.GetUnits(groupid);

                    cmbUnits.DataSource = dtUnits;
                    cmbUnits.DataBind();
                }
                cmbUnits.ClearSelection();
                cmbUnits.Items.Insert(0, new ListItem("All Units", "-1"));
                cmbUnits.Items.FindByValue("-1").Selected = true;

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("advance_search.fillUnits", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw;
            }

            finally
            {
                objMsgCenter = null;
                dtUnits = null;
            }

        }
        /// <summary>
        /// This function is to fill Groups Finding into Finding Combo box
        /// </summary>
        private void fillFindings()
        {
            MessageCenter objMsgCenter  = null;
            DataTable dtGroupFindings = null;
            try
            {

                if (cmbGroup.SelectedValue != "0" && cmbGroup.SelectedValue != "-1")
                {
                    int groupID = Convert.ToInt32(cmbGroup.SelectedValue);

                    objMsgCenter = new MessageCenter();
                    dtGroupFindings = objMsgCenter.GetFindingsbyGroupID(groupID);
                    cmbFinding.DataSource = dtGroupFindings.DefaultView;
                    cmbFinding.DataBind();
                }
                cmbFinding.ClearSelection();
                cmbFinding.Items.Insert(0, new ListItem("All Findings", "-1"));
                cmbFinding.Items.FindByValue("-1").Selected = true;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("advance_search.fillFindings", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw;
            }

            finally
            {
                objMsgCenter = null;
                dtGroupFindings = null;
            }
        }
        /// <summary>
        /// Controlled the enable status of Adcaned search control.
        /// </summary>
        /// <param name="flg"></param>
        private void controlEnabled(bool flg)
        {
            cmbOC.Enabled = flg;
            cmbRC.Enabled = flg;
            cmbNurse.Enabled = flg;
            cmbUnits.Enabled = flg;
            cmbFinding.Enabled = flg; 
            txtAccession.Enabled = flg;
            txtDOB.Enabled = flg;
            txtFromDate.Enabled = flg;
            txtMRN.Enabled = flg;
            txtToDate.Enabled = flg;
            btnSelect.Enabled = flg;    
        }

        private void GroupTypeVisibility()
        {
                if (ddlistShowMessages.SelectedIndex == 1)
                {
                    cmbNurse.Enabled = true;
                    lblNurse.Enabled = true;
                    cmbUnits.Enabled = true;
                    lblUnit.Enabled = true;
                }
                else
                {
                    cmbNurse.Enabled = false;
                    lblNurse.Enabled = false;
                    cmbUnits.Enabled = false;
                    lblUnit.Enabled = false; ;
                }
        }
        private void fillMessageStatus()
        {
            if (ddlistShowMessages.SelectedValue == "0")
            {
                cmbMsgStatus.Items.Clear();
                cmbMsgStatus.Items.Add(new ListItem("Open", "0"));
                cmbMsgStatus.Items.Add(new ListItem("Closed", "1"));
                cmbMsgStatus.Items.Add(new ListItem("Documented Message", "2"));
                cmbMsgStatus.Items.Add(new ListItem("Open Replies", "5"));
                cmbMsgStatus.Items.Add(new ListItem("Open ReadBacks", "6"));
                cmbMsgStatus.Items.Add(new ListItem("All", "4"));

            }
            else
            {
                cmbMsgStatus.Items.Clear();
                cmbMsgStatus.Items.Add(new ListItem("Open", "0"));
                cmbMsgStatus.Items.Add(new ListItem("Closed", "1"));
                cmbMsgStatus.Items.Add(new ListItem("Documented Message", "2"));
                cmbMsgStatus.Items.Add(new ListItem("Open Replies", "5"));
                cmbMsgStatus.Items.Add(new ListItem("All", "4"));
            }
        }
        #endregion
    }
}