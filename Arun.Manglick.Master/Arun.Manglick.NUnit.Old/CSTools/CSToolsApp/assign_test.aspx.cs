#region File History

/******************************File History***************************
 * File Name        : assign_test.aspx.cs
 * Author           : Rashmi N
 * Created Date     : August 22, 2007
 * Purpose          : To provide UI to assign test results to group.
 * *********************File Modification History*********************
 * Date(mm-dd-yyyy) Developer Reason of Modification
 * ------------------------------------------------------------------- 
 * 28-11-2007   IAK     Modified function generateDataGridHeight()
 * 12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 * ------------------------------------------------------------------- 
 */
#endregion

#region using
using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Vocada.VoiceLink.DataAccess;
using Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.Utilities;
#endregion

namespace Vocada.CSTools
{
    /// <summary>
    /// The purpose of this class is to store the entered data for Test Result Definitions
    /// in the Veriphy database.
    /// </summary>
    public partial class assign_test : System.Web.UI.Page
    {
        #region Private Members
        int records = 0;
        /// <summary>
        /// Grid Rows Count = 5
        /// </summary>
        private const int GRID_ROWS_COUNT = 5;

        /// <summary>
        /// Grid Row Size = 25
        /// </summary>
        private const int GRID_ROW_SIZE = 70;
        protected int nheight = GRID_ROW_SIZE;
        #endregion

        #region Event Handlers

        /// <summary>
        /// Loads the Controls on the page with their default values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sbScript = new StringBuilder();
            if (Session[SessionConstants.LOGGED_USER_ID] == null)
                Response.Redirect(Utils.GetReturnURL("default.aspx", "test_result_definitions.aspx", this.Page.ClientQueryString));

            try
            {   
                Session[SessionConstants.CURRENT_TAB] = "Tools";
                Session["CurrentInnerTab"] = "TestResult";

                sbScript.Append("<script language=JavaScript>");
                sbScript.Append("var btnSaveClientId = '" + btnSave.ClientID + "';");             
                sbScript.Append("</script>");
                this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());

                if (!IsPostBack)
                {


                    ViewState["groupId"] = (Request.QueryString["groupId"] == null ? 0 : Convert.ToInt32(Request.QueryString["groupId"]));
                    ViewState["instId"] = (Request.QueryString["instId"] == null ? 0 : Convert.ToInt32(Request.QueryString["instId"]));
                    fillTestResults();                    
                    Session["ColumnName"] = null;                                        
                }
            }
            catch (Exception ex)
            { 
                Tracer.GetLogger().LogInfoEvent("test_result_definitions.Page_Load():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, 0);
                throw;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string groupId = "";
            string instId = "-1";

            groupId = ( ViewState["groupId"] == null ? "":  ViewState["groupId"].ToString());
            instId = (ViewState["instId"] == null ? "" : ViewState["instId"].ToString());
            Response.Redirect("test_result_definitions.aspx?groupId=" + groupId + "&instId=" + instId);
        }
        /// <summary>
        /// Adds the Test and its Values to database, calls "VOC_VL_insertTestResults" stored procedure passing the values entered by the user.
        /// Also calls sortGridTestResult() to load the grdTestResults with updated data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            CheckBox chkBox;
            string testIds = "";
            string instId = "-1";
            Control cntrl;
            TestResults testResult;
            int groupId = 0;

            try
            {
                chkBox =  new CheckBox();
                cntrl = new Control();
                testResult = new TestResults();
                groupId = (ViewState["groupId"] == null ? 0 : Convert.ToInt32(ViewState["groupId"]));

                for (int i = 0; i < grdTestResults.Items.Count; i++)
                {
                    cntrl = grdTestResults.Items[i].FindControl("chkTest");

                    if (cntrl != null)
                    {
                        chkBox = (CheckBox)cntrl;

                        if (chkBox.Checked)
                        {
                            testIds += grdTestResults.DataKeys[i] + ":";
                        }
                    }
                }

                if (testIds.Length > 0)
                {
                    testIds = testIds.Remove(testIds.Length - 1, 1);
                    testResult.InsertLabTestForGroup(groupId, testIds);
                }
                instId = (ViewState["instId"] == null ? "" : ViewState["instId"].ToString());
                Response.Redirect("test_result_definitions.aspx?groupId=" + groupId.ToString() + "&instId=" + instId);
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogInfoEvent("test_result_definitions.btnAddDevice_Click:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
            finally
            {
                chkBox = null;
                cntrl = null;
                testResult = null;
            }
        }
   

        /// <summary>
        /// Grid rows are sorted in Ascending/Descending order accordingly for the Sort Expression 
        /// belonging to the Column Header after selecting them.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdTestResults_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            try
            {
                if (Session["Direction"] != null)
                {
                    if (Session["Direction"] == " ASC")
                        Session["Direction"] = " DESC";
                    else
                        Session["Direction"] = " ASC";

                    if (Session["ColumnName"] != null)
                        if(Session["ColumnName"] != e.SortExpression)
                            Session["Direction"] = " ASC";
                }
                else
                    Session["Direction"] = " ASC";
                
                Session["ColumnName"] = e.SortExpression;
                sortGridTestResult();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogInfoEvent("test_result_definitions.grdTestResults_SortCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
        }
        protected void grdTestResults_ItemDataBound(object source, DataGridItemEventArgs e)
        {
            
            
            if (e.Item.ItemType == ListItemType.Header )
            {
                CheckBox chkHeader =(CheckBox) e.Item.FindControl("chkAll");
                if (records == 0)
                    chkHeader.Visible = false;
                else
                    chkHeader.Visible = true ;
            }
        }
        
        #endregion

        #region Private Methods

        /// <summary>
        /// This function fills all the Test Results for Lab in the DataGrid.
        /// This function calls stored procedure "VOC_VL_getTestResults"
        /// </summary>
        private void fillTestResults()
        {
            DataSet dstestresult;
            TestResults testResults;
            try
            {
                
                int groupId = 0;
                groupId = (ViewState["groupId"] == null ? 0 : Convert.ToInt32(ViewState["groupId"]));
                testResults = new TestResults();
                dstestresult = testResults.GetUnassignedLabTests(groupId);

                records = dstestresult.Tables[0].Rows.Count;
                if (records <= 0)
                    lblNorecord.Text = "All tests are already assigned to this group.";
                else
                    lblNorecord.Text = "";

                if (dstestresult.Tables[0].Rows.Count > 1)
                    grdTestResults.AllowSorting = true;
                else
                    grdTestResults.AllowSorting = false;
                grdTestResults.DataSource = dstestresult.Tables[0].DefaultView;
                grdTestResults.DataBind();
                generateDataGridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogInfoEvent("test_result_definitions.fillTestResults():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
            finally
            {
                testResults = null;
                dstestresult = null;
            }
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
                script += "if(document.getElementById('TestResultsDiv') != null){document.getElementById('TestResultsDiv').style.height=setHeightOfGrid('" + grdTestResults.ClientID + "','100');}</script>";
                ScriptManager.RegisterStartupScript(upGridTestResult,upGridTestResult.GetType(), newUid, script,false);
            }
            catch(Exception ex)
            {
                if(Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("test_result_definitions.generateDataGridHeight():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
        }

        /// <summary>
        /// This method is called whenever the Grid columns needs to be arranged in sorted order
        /// after the selection of the Header Text in Column.
        /// </summary>
        private void sortGridTestResult()
        {
            try
            {
                if (Session["ColumnName"] != null)
                {
                    int groupId = 0;
                    groupId = Convert.ToInt32(ViewState["groupId"]);
                    TestResults testResults = new TestResults();
                    DataView dvSortedTestResults = new DataView();
                    dvSortedTestResults = testResults.GetUnassignedLabTests(groupId).Tables[0].DefaultView;
                    dvSortedTestResults.Sort = Session["ColumnName"] + Session["Direction"].ToString();
                    grdTestResults.DataSource = dvSortedTestResults;
                    if (dvSortedTestResults.Count>0)
                           grdTestResults.AllowSorting=true;
                    else
                        grdTestResults.AllowSorting=false ;
                    grdTestResults.DataBind();
                    generateDataGridHeight();
                }
                else
                {
                    fillTestResults();
                }                
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogInfoEvent("test_result_definitions.sortGridTestResult():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
        }

        #endregion
    }
}