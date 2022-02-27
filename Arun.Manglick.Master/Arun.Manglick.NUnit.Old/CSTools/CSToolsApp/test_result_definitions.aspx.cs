#region File History

/******************************File History***************************
 * File Name        : test_result_definitions.aspx.cs
 * Author           : Jeeshan K
 * Created Date     : 
 * Purpose          : To provide UI to add/define test results, modify or delete existing tests and their values.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

 * 11-28-2007   IAK     Modified function generateDataGridHeight()
 * 01-16-2008   Prerak  Modified for Displaying alert when Lab test is added or updated
 * 01-24-2008   IAK     Defect 2659 
 * 01-29-2008   ZNK     Defect 2688 -- Commented stmts. for  VoiceOver/Grammar field required value alerts.
 * 01-29-2008   ZNK     Defect 2689 -- Added items tag to Grammar field in btnAdd_Click event.
 * 04-28-2008   Prerak  Defect 3084 -- Fixed
 * 12 Jun 2008  Prerak  Migration of AJAX Atlas to AJAX RTM 1.0
 * ------------------------------------------------------------------- 
 *                          
 */
#endregion

#region using
using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
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
    public partial class test_result_definitions : System.Web.UI.Page
    {
        #region Private Members
       
        /// <summary>
        /// Grid Rows Count = 5
        /// </summary>
        private const int GRID_ROWS_COUNT = 5;
        private bool isSystemAdmin = true;
        private const string SELECTED_GRID_ITEM = "SelectedGridItem";


        /// <summary>
        /// Grid Row Size = 25
        /// </summary>
        private const int GRID_ROW_SIZE = 30;
        protected int nheight = GRID_ROW_SIZE;
        private string groupId = "";
        private string instId = "-1";
        #endregion

        #region Event Handlers

        /// <summary>
        /// Loads the Controls on the page with their default values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if(Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
                Response.Redirect(Utils.GetReturnURL("default.aspx", "test_result_definitions.aspx", this.Page.ClientQueryString));
            try
            {
                registerJavaScript();
                                
                Session[SessionConstants.CURRENT_TAB] = "Tools";
                Session[SessionConstants.CURRENT_INNER_TAB] = "TestResult";
                UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
                if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
                {
                    isSystemAdmin = false;
                    instId = userInfo.InstitutionID.ToString(); 
                }
                else
                    isSystemAdmin = true;
                if (!IsPostBack)
                {
                    Session[SessionConstants.WEEK_NUMBER] = null;
                    Session[SessionConstants.SHOWMESSAGES] = null;
                    Session[SessionConstants.STATUS] = null;
                    Session[SessionConstants.GROUP] = null;

                    groupId = (Request.QueryString["groupId"] == null ? "" : Request.QueryString["groupId"].ToString());
                    instId = (Request.QueryString["instId"] == null ? "" : Request.QueryString["instId"].ToString());
                    if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
                    {
                        isSystemAdmin = false;
                        hdnIsSystemAdmin.Value = "0";
                        cmbInstitution.Visible = false;
                        lblInstName.Visible = true;
                        lblInstName.Text = userInfo.InstitutionName; 
                        instId = userInfo.InstitutionID.ToString();
                        fillGroups(Convert.ToInt32(instId));
                       
                    }
                    else
                    {
                        isSystemAdmin = true;
                        hdnIsSystemAdmin.Value = "1";
                        cmbInstitution.Visible = true; ;
                        lblInstName.Visible = false;
                        fillInstitition();
                    }
                    fillResultTypesDDL();
                    fillMeasurementDDL();
                    fillTestResults();
                    fillLabTestArea();
                    Session["ColumnName"] = null;                    
                    clearControls();
                 }
                 ScriptManager.RegisterStartupScript(upAddTestResult, upAddTestResult.GetType(), "HideOtherText", "<script type=\'text/javascript\'>HideOtherText();</script>", false);

                setAddTestLink();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogInfoEvent("test_result_definitions.Page_Load():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw;
            }
        }

        /// <summary>
        /// Deletes the selected Test Result, calls "VOC_VL_deleteTestResult" stored procedure,
        /// passes the selected Lab Test Id, deletes the Test and fetches the updated records in the grid.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dlistGridResultType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int intGridEditItemIndex = grdTestResults.EditItemIndex;

                if (((DropDownList)(grdTestResults.Items[intGridEditItemIndex].Cells[2].FindControl("dlistGridResultType"))).SelectedIndex != 0)
                {
                    ((TextBox)(grdTestResults.Items[intGridEditItemIndex].Cells[3].FindControl("txtGridLowValue"))).Visible = false;
                    ((TextBox)(grdTestResults.Items[intGridEditItemIndex].Cells[4].FindControl("txtGridHighValue"))).Visible = false;
                    ((DropDownList)(grdTestResults.Items[intGridEditItemIndex].Cells[5].FindControl("dlistGridMeasurement"))).Enabled = false;
                    ((DropDownList)(grdTestResults.Items[intGridEditItemIndex].Cells[6].FindControl("dlistGridDefaultFinding"))).Enabled = false;
                }
                else
                {
                    ((TextBox)(grdTestResults.Items[intGridEditItemIndex].Cells[3].FindControl("txtGridLowValue"))).Visible = true;
                    ((TextBox)(grdTestResults.Items[intGridEditItemIndex].Cells[4].FindControl("txtGridHighValue"))).Visible = true;
                    ((DropDownList)(grdTestResults.Items[intGridEditItemIndex].Cells[5].FindControl("dlistGridMeasurement"))).Enabled = true;
                    ((DropDownList)(grdTestResults.Items[intGridEditItemIndex].Cells[6].FindControl("dlistGridDefaultFinding"))).Enabled = true;
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent("test_result_definitions.dlistGridResultType_SelectedIndexChanged:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw;
            }
        }

        /// <summary>
        /// Adds the Test and its Values to database, calls "VOC_VL_insertTestResults" stored procedure passing the values entered by the user.
        /// Also calls sortGridTestResult() to load the grdTestResults with updated data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            TestAndValues testAndValues;
            TestResults objTestResults;
            bool flgOther = false;
            string fileName = "";
            string script = "";
            try
            {
                if (flupdVoiceOver.FileName.Length == 0)
                {
                    /*generateDataGridHeight();
                    if (ViewState[SELECTED_GRID_ITEM] != null)
                    {
                        script = "setGridSelectedItemColor('" + ViewState[SELECTED_GRID_ITEM].ToString() + "');";
                    }
                    script += "alert(' - You must enter Test VoiceOverURL.');document.getElementById(flupdVoiceOverClientID).focus();";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "NavigateToDir", "<script type=\'text/javascript\'>" + script + "</script>", false);
                    return;*/
                }
                fileName = upload();
                /*if (fileName.Length == 0)
                {
                    generateDataGridHeight();
                    if (ViewState[SELECTED_GRID_ITEM] != null)
                    {
                        script = "setGridSelectedItemColor('" + ViewState[SELECTED_GRID_ITEM].ToString() + "');";
                    }
                    script += "alert(' - File does not exists.');document.getElementById(flupdVoiceOverClientID).focus();";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Invalid File", "<script type=\'text/javascript\'>" + script + "</script>", false);
                    return;
                }*/
                
                
                testAndValues = new TestAndValues();

                testAndValues.FullTestName = txtFullTestName.Text.Trim();
                if (cmbLabTestArea.Text != "Other")
                    testAndValues.TestArea = cmbLabTestArea.Text;
                else
                {
                    testAndValues.TestArea = txtLabTestArea.Text.Trim();
                    flgOther = true;
                }
                testAndValues.ShortTestName = txtShortTestName.Text.Trim();
                if (txtHighestPossibleValue.Text.Trim() == "")
                    testAndValues.HighestValue = -1;
                else
                    testAndValues.HighestValue = Convert.ToSingle(txtHighestPossibleValue.Text.Trim());

                if (txtLowestPossibleValue.Text.ToString().Trim() == "")
                    testAndValues.LowestValue = -1;
                else
                    testAndValues.LowestValue = Convert.ToSingle(txtLowestPossibleValue.Text.Trim());

                testAndValues.ResultTypeID = Convert.ToInt32(cmbResultType.SelectedValue);
                testAndValues.MeasurementID = Convert.ToInt32(cmbMeasurement.SelectedValue);

                string grammar = string.Empty;
                if (txtGrammar.Text.Trim().Length > 0)
                {
                    grammar = txtGrammar.Text.Trim();
                    grammar = grammar.Replace(";", "</item><item>");
                    grammar = grammar.Insert(0, "<item>");
                    grammar = grammar.Insert(grammar.Length, "</item>");
                }
                testAndValues.Grammer = grammar;
                testAndValues.TestVoiceURL = fileName;
                testAndValues.FindingID = 0;
                testAndValues.GroupID = Convert.ToInt32(cmbGroup.SelectedValue);
                testAndValues.IsActive = true;
                testAndValues.TestID = 0;
                // Adds the Test Result details
                objTestResults = new TestResults();
                objTestResults.InsertTestResult(testAndValues);                
                fillTestResults();
                if (flgOther)
                  fillLabTestArea(); 
                sortGridTestResult();
                clearControls();
                hdnTestDataChanged.Value = "false";
                script = "alert('Lab Test added successfully');";
                flupdVoiceOver.Width = 340;
                ScriptManager.RegisterClientScriptBlock(upAddTestResult,upAddTestResult.GetType(), "Suceessful", script, true);
                
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("test_result_definitions.btnAdd_Click():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
            finally
            {
                testAndValues = null;
                objTestResults = null;
            }
        }

        /// <summary>
        /// Clear content to add new test.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //btnEdit.Enabled = false;
            fillTestResults();
            sortGridTestResult();
            clearControls();
            hdnTestDataChanged.Value = "false";
            flupdVoiceOver.Width = 340;
        }

        /// <summary>
        /// Set the EditItemIndex of grdTestResults to the selected index, calls sortGridTestResult() method
        /// to bind the datagrid again, calls fillResultTypesDDL(), fillMeasurementDDL(), and fillFindingsDDL 
        /// methods to fill the "Result Type", "Mesurement", and "Findings" dropdown in the selected row of 
        /// datagrid in respective columns.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdResultDefinitions_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ListItem li;

            ViewState["LabTestId"] = e.Item.Cells[0].Text;
            txtFullTestName.Text  = e.Item.Cells[2].ToolTip.ToString();
            txtShortTestName.Text = e.Item.Cells[3].ToolTip.ToString();
            
           if (e.Item.Cells[5].Text == "Numeric")
            {
                cmbResultType.SelectedValue = "1";
                txtLowestPossibleValue.Enabled = true;
                txtHighestPossibleValue.Enabled = true;
                cmbMeasurement.Enabled = true;
                if (e.Item.Cells[6].Text != "&nbsp;")
                  txtLowestPossibleValue.Text = e.Item.Cells[6].Text;
                if (e.Item.Cells[7].Text != "&nbsp;")
                  txtHighestPossibleValue.Text = e.Item.Cells[7].Text;
               
               if(e.Item.Cells[8].Text == "NA")
                {
                    cmbMeasurement.SelectedValue = "-1";
                }
                else
                {
                    li = cmbMeasurement.Items.FindByText(e.Item.Cells[8].Text);
                    if(li != null)
                    {
                        cmbMeasurement.SelectedValue = li.Value;
                    }
                    else
                    {
                        cmbMeasurement.SelectedValue = "-1";
                    }
                }
                reqValMeasure.Enabled = true;
            }
            else
            {
                li = cmbResultType.Items.FindByText(e.Item.Cells[5].Text);
                if(li != null)
                {
                    cmbResultType.SelectedValue = li.Value;
                }
                else
                {
                    cmbResultType.SelectedValue = "-1";
                }
                txtLowestPossibleValue.Enabled = false;
                txtHighestPossibleValue.Enabled = false;
                cmbMeasurement.Enabled = false;
                txtLowestPossibleValue.Text = "";
                txtHighestPossibleValue.Text = "";
                cmbMeasurement.SelectedValue = "-1";
                reqValMeasure.Enabled = false;
            }

            txtGrammar.Text = e.Item.Cells[9].Text != "&nbsp;" ? e.Item.Cells[9].Text.Substring(0,e.Item.Cells[9].Text.Length-1) : "";
            
            if ((e.Item.Cells[4].Controls[0] as HyperLink).NavigateUrl.Length > 0)
            {
                hlinkPlay.Visible = true;
                flupdVoiceOver.Width = 320;
                hlinkPlay.NavigateUrl = (e.Item.Cells[4].Controls[0] as HyperLink).NavigateUrl;
            }
            else
            {
                hlinkPlay.Visible = false;
                flupdVoiceOver.Width = 340;
            }
            li = cmbLabTestArea.Items.FindByText(e.Item.Cells[10].Text);
            
            if(li != null)
            {
                cmbLabTestArea.SelectedValue = li.Value;
            }
            else
            {
                cmbLabTestArea.SelectedIndex = -1;
            }
            //Page.SetFocus(e.Item.ClientID);
            e.Item.BackColor = Color.FromName("#ffffcc");
            ViewState[SELECTED_GRID_ITEM] = e.Item.ClientID;
            btnAdd.Visible = false;
            btnEdit.Visible = true;
            generateDataGridHeight();
        }

        /// <summary>
        /// Deletes the selected Test Result, calls "VOC_VL_deleteTestResult" stored procedure,
        /// passes the selected Lab Test Id, deletes the Test and fetches the updated records in the grid.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdTestResults_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            TestResults testResults;
            try
            {
                testResults = new TestResults();
                int testID = Convert.ToInt32(e.Item.Cells[0].Text); 

                testResults.DeleteTestResult(testID);
                sortGridTestResult();
                clearControls();
                ScriptManager.RegisterClientScriptBlock(upAddTestResult,upAddTestResult.GetType(), "RemovalSuceessful", "alert('Test Result Removed successfully.')", true);
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("test_result_definitions.grdTestResults_DeleteCommand():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
            finally
            {
                testResults = null;
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

        /// <summary>
        /// Bound data to grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdTestResults_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
           
            DataRowView data = e.Item.DataItem as DataRowView;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string fullName = (string)data["TestDescription"];
                if (fullName.Length > 25)
                {
                    fullName = fullName.Substring(0, 25) + "...";
                    e.Item.Cells[2].ToolTip = (string)data["TestDescription"];
                }
                e.Item.Cells[2].Text = fullName;
                e.Item.Cells[2].ToolTip = (string)data["TestDescription"];

                string shortName = (string)data["TestShortDescription"];
                if (shortName.Length > 25)
                {
                    shortName = shortName.Substring(0, 25) + "...";
                    e.Item.Cells[3].ToolTip = (string)data["TestShortDescription"];
                }
                e.Item.Cells[3].Text = shortName;
                e.Item.Cells[3].ToolTip = (string)data["TestShortDescription"];
 
                string resultype = (string)data["ResultType"];
                if (resultype == "Positive/negative" )
                    e.Item.Cells[5].Text = "Positive/Negative";

                if (e.Item.Cells[6].Text != "&nbsp;")
                {
                    double lval = Convert.ToDouble(e.Item.Cells[6].Text);
                    e.Item.Cells[6].Text = Convert.ToString(Math.Round(lval, 2)); 
                }
                if (e.Item.Cells[7].Text != "&nbsp;")
                {
                    double hval = Convert.ToDouble(e.Item.Cells[7].Text);
                    e.Item.Cells[7].Text = Convert.ToString(Math.Round(hval, 2));
                }
                //DataGridLinkButton linkbutton = e.Item.Cells[11].Controls[1] as DataGridLinkButton;
                //linkbutton.OnClientClick = "return onTestdataChanged();";
            }
        }

        /// <summary>
        /// Cancel Editing of the selected record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdTestResults_OnItemCreated(object source, DataGridItemEventArgs e)
        {
            string editScript = "javascript:";
            string deleteScript = "javascript:";
            LinkButton lbtnEdit = null;
            LinkButton lbtnDelete = null;
            LinkButton lbtnHeader = null;
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    lbtnEdit = (e.Item.Cells[11].Controls[0]) as LinkButton;
                    lbtnDelete = (e.Item.Cells[12].Controls[1]) as LinkButton;

                    editScript += "if(onTestdataChanged()){";
                    deleteScript += "if(ConformBeforeDelete()){";
                    if (e.Item.ItemIndex + 2 < 10)
                    {
                        editScript += "__doPostBack('ctl00$ContentPlaceHolder1$grdTestResults$ctl0" + (e.Item.ItemIndex + 2) + "$ctl01', '');";
                        deleteScript += "__doPostBack('ctl00$ContentPlaceHolder1$grdTestResults$ctl0" + (e.Item.ItemIndex + 2) + "$lbtnDelete', '');";
                    }
                    else
                    {
                        editScript += "__doPostBack('ctl00$ContentPlaceHolder1$grdTestResults$ctl" + (e.Item.ItemIndex + 2) + "$ctl01', '');";
                        deleteScript += "__doPostBack('ctl00$ContentPlaceHolder1$grdTestResults$ctl" + (e.Item.ItemIndex + 2) + "$lbtnDelete', '');";
                    }

                    lbtnEdit.OnClientClick = editScript + "} else {return false;}";
                    lbtnDelete.OnClientClick = deleteScript + "} else {return false;}";
                }
                else if (e.Item.ItemType == ListItemType.Header && e.Item.Cells[2].Controls.Count > 0)
                {
                    ((e.Item.Cells[2].Controls[0]) as LinkButton).OnClientClick += "javascript:if(onTestdataChanged()){__doPostBack('ctl00$ContentPlaceHolder1$grdTestResults$ctl01$ctl00', '');}else{return false;}";
                    ((e.Item.Cells[3].Controls[0]) as LinkButton).OnClientClick += "javascript:if(onTestdataChanged()){__doPostBack('ctl00$ContentPlaceHolder1$grdTestResults$ctl01$ctl01', '');}else{return false;}";
                    ((e.Item.Cells[5].Controls[0]) as LinkButton).OnClientClick += "javascript:if(onTestdataChanged()){__doPostBack('ctl00$ContentPlaceHolder1$grdTestResults$ctl01$ctl02', '');}else{return false;}";
                    ((e.Item.Cells[6].Controls[0]) as LinkButton).OnClientClick += "javascript:if(onTestdataChanged()){__doPostBack('ctl00$ContentPlaceHolder1$grdTestResults$ctl01$ctl03', '');}else{return false;}";
                    ((e.Item.Cells[7].Controls[0]) as LinkButton).OnClientClick += "javascript:if(onTestdataChanged()){__doPostBack('ctl00$ContentPlaceHolder1$grdTestResults$ctl01$ctl04', '');}else{return false;}";
                    ((e.Item.Cells[8].Controls[0]) as LinkButton).OnClientClick += "javascript:if(onTestdataChanged()){__doPostBack('ctl00$ContentPlaceHolder1$grdTestResults$ctl01$ctl05', '');}else{return false;}";
                    ((e.Item.Cells[10].Controls[0]) as LinkButton).OnClientClick += "javascript:if(onTestdataChanged()){__doPostBack('ctl00$ContentPlaceHolder1$grdTestResults$ctl01$ctl06', '');}else{return false;}";
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grammar.grdTestResults_OnItemCreated", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                lbtnEdit = null;
                lbtnDelete = null;
                lbtnHeader = null;
            }
        }

        /// <summary>
        /// update group combo with selected institute 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clearControls();
                fillGroups(Convert.ToInt32(cmbInstitution.SelectedValue));
                if (Convert.ToInt32(cmbInstitution.SelectedValue) != -1)
                {
                    cmbGroup.Enabled = true;
                    // enbaledControl(true);
                }
                else
                {
                    cmbGroup.Enabled = false;
                    // enbaledControl (false);
                    btnImport.Enabled = false;
                }
                hdnTestDataChanged.Value = "false";
                hdnInstitutionVal.Value = cmbInstitution.SelectedValue;
                flupdVoiceOver.Width = 340;
            }
            catch (Exception objException)
            {
                if (Session[SessionConstants.LOGGED_USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("test_result_definitions - cmbGroup_SelectedIndexedChanged", Session[SessionConstants.USER_ID].ToString(), objException.Message, objException.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw objException;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Load the tests in grid 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGroup.SelectedValue != "-1")
            {
                clearControls();
                btnImport.Enabled = true;
                btnAdd.Enabled = true;
                enbaledControl(true);
            }
            else
            {
                enbaledControl(false);
                btnImport.Enabled = false;
                btnAdd.Enabled = false;
            }
            fillTestResults();
            fillLabTestArea();
            hdnTestDataChanged.Value = "false";
            hdnGroupVal.Value = cmbGroup.SelectedValue;
            flupdVoiceOver.Width = 340;
        }

        /// <summary>
        /// Adds the Test and its Values to database, calls "VOC_VL_insertTestResults" stored procedure passing the values entered by the user.
        /// Also calls sortGridTestResult() to load the grdTestResults with updated data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            TestAndValues testAndValues;
            TestResults objTestResults;
            int labTestId = 0;
            string grammar = "";
            bool flgOther = false;
            string fileName = "";
            string script ="";
            try
            {
                if (flupdVoiceOver.FileName.Length != 0)
                {
                    fileName = upload();

                    if (fileName.Length == 0)
                    {
                        generateDataGridHeight();
                        if (ViewState[SELECTED_GRID_ITEM] != null)
                        {
                            script = "setGridSelectedItemColor('" + ViewState[SELECTED_GRID_ITEM].ToString() + "');";
                        }
                        script += "alert(' - File does not exists.');document.getElementById(flupdVoiceOverClientID).focus();";
                        ScriptManager.RegisterClientScriptBlock(upAddTestResult,upAddTestResult.GetType(), "Invalid File", "<script type=\'text/javascript\'>" + script + "</script>", false);
                        return;
                    }
                }
                else
                {
                    /*if (hlinkPlay.NavigateUrl.Length == 0)
                    {
                        generateDataGridHeight();
                        if (ViewState[SELECTED_GRID_ITEM] != null)
                        {
                            script = "setGridSelectedItemColor('" + ViewState[SELECTED_GRID_ITEM].ToString() + "');";
                        }
                        script += "alert(' - You must enter Test VoiceOverURL.');document.getElementById(flupdVoiceOverClientID).focus();";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "NavigateToDir", "<script type=\'text/javascript\'>" + script + "</script>", false);
                        return;
                    }*/
                    fileName = hlinkPlay.NavigateUrl;
                }

                testAndValues = new TestAndValues();
                labTestId = (ViewState["LabTestId"] == null ? 0 : Convert.ToInt32(ViewState["LabTestId"]));
                testAndValues.LabTestID = labTestId;
                testAndValues.FullTestName = txtFullTestName.Text.Trim();
                if (cmbLabTestArea.Text != "Other")
                    testAndValues.TestArea = cmbLabTestArea.Text;
                else
                {
                    testAndValues.TestArea = txtLabTestArea.Text.Trim();
                    flgOther = true;
                }
                testAndValues.ShortTestName = txtShortTestName.Text.Trim();
                if (txtHighestPossibleValue.Text.Trim() != "")
                //    testAndValues.HighestValue = -1;
                //else
                    testAndValues.HighestValue = Convert.ToSingle(txtHighestPossibleValue.Text.Trim());
                if (txtLowestPossibleValue.Text.ToString().Trim() != "")
                //    testAndValues.LowestValue = -1;
                //else
                    testAndValues.LowestValue = Convert.ToSingle(txtLowestPossibleValue.Text.Trim());
                testAndValues.ResultTypeID = Convert.ToInt32(cmbResultType.SelectedValue);
                testAndValues.MeasurementID = Convert.ToInt32(cmbMeasurement.SelectedValue);

                if (txtGrammar.Text.Trim().Length > 0)
                {
                    grammar = txtGrammar.Text.Trim();
                    grammar = grammar.Replace(";", "</item><item>");
                    grammar = grammar.Insert(0, "<item>");
                    grammar = grammar.Insert(grammar.Length, "</item>");
                }
                testAndValues.Grammer = grammar;
                testAndValues.TestVoiceURL = fileName;
                testAndValues.FindingID = 0;
                testAndValues.GroupID = Convert.ToInt32(cmbGroup.SelectedValue);
                testAndValues.IsActive = true;
                testAndValues.TestID = 0;
                // Adds the Test Result details
                objTestResults = new TestResults();
                objTestResults.UpdateTestResult(testAndValues);
                fillTestResults();
                if (flgOther)
                    fillLabTestArea(); 
                sortGridTestResult();

                btnEdit.Visible = false;
                btnAdd.Visible = true;
                clearControls();
                hdnTestDataChanged.Value = "false";
                script = "alert('Lab Test updated successfully');";
                flupdVoiceOver.Width = 340;
                ScriptManager.RegisterClientScriptBlock(upAddTestResult,upAddTestResult.GetType(), "UpdateSuceessful", script, true);
                

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("test_result_definitions.btnAdd_Click():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
            finally
            {
                testAndValues = null;
                objTestResults = null;
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
            TestResults testResults;

            try
            {
                DataSet testData = null;
                testResults = new TestResults();
                int groupID = -1;

                if(cmbGroup.SelectedIndex > -1)
                {
                    groupID = Convert.ToInt32(cmbGroup.SelectedValue);                 
                }
                testData = testResults.GetTestResults(groupID);
                grdTestResults.DataSource = testData;
                
                if (testData.Tables[0].Rows.Count > 1)
                    grdTestResults.AllowSorting = true;
                else
                    grdTestResults.AllowSorting = false;

                grdTestResults.DataBind();
                generateDataGridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("test_result_definitions.fillTestResults():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
            finally
            {
                testResults = null;                
            }
        }

        /// <summary>
        /// This function fills all the Result Types for Lab Test Result in Result TypeRadio Button List.
        /// This function calls stored procedure "VOC_VL_getResultTypes"
        /// </summary>
        private void fillResultTypesDDL()
        {
            TestResults objtestResults ;
            DataTable dtResultType;
            try
            {
                objtestResults = new TestResults();
                dtResultType = objtestResults.GetResultTypes();

                cmbResultType.DataSource = dtResultType;
                cmbResultType.DataBind();

                cmbResultType.Items.FindByText("Positive/negative").Text = "Positive/Negative";
               
                ListItem li = new ListItem("-- Select Result Type--", "-1");
                cmbResultType.Items.Insert(0, li);
                cmbResultType.Items.FindByValue("-1").Selected = true;                
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("test_result_definitions.fillResultTypesDDL():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
            finally
            {
                objtestResults = null;
                dtResultType = null;
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
                script += "if(document.getElementById('TestResultsDiv') != null){document.getElementById('TestResultsDiv').style.height=setHeightOfGrid('" + grdTestResults.ClientID + "','250');}</script>";
                if (!IsPostBack)
                    ScriptManager.RegisterStartupScript(upAddTestResult,upAddTestResult.GetType(),newUid, script,false );
                else
                    ScriptManager.RegisterClientScriptBlock(upAddTestResult,upAddTestResult.GetType(), newUid, script,false );

           }            
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("test_result_definitions.generateDataGridHeight():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
        }

        /// <summary>
        /// This function fills Measurements for Result Type in user into drop down list.
        /// This function calls stored procedure "VOC_VL_getMeasurements"
        /// </summary>
        /// <param name="ddlistMeasurementParam">DropDownList for Measurements</param>
        private void fillMeasurementDDL()
        {
            TestResults objtestResults;
            DataTable dtMeasurement;
            try
            {
                objtestResults = new TestResults();
                dtMeasurement = objtestResults.GetMeasurements();

                cmbMeasurement.DataSource = dtMeasurement;
                cmbMeasurement.DataBind();

                ListItem li = new ListItem("-- Select Measurement --", "-1");
                cmbMeasurement.Items.Insert(0,li);
                cmbMeasurement.Items.FindByValue("-1").Selected = true;                
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("test_result_definitions.fillMeasurementDDL():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
            finally
            {
                objtestResults = null;
            }

        }

        /// <summary>
        /// This method is called whenever the Grid columns needs to be arranged in sorted order
        /// after the selection of the Header Text in Column.
        /// </summary>
        private void sortGridTestResult()
        {
            TestResults testResults;
            try
            {
                if (Session["ColumnName"] != null)
                {
                    testResults = new TestResults();
                    DataView dvSortedTestResults = new DataView();
                    int groupID = Convert.ToInt32(cmbGroup.SelectedValue);
                    dvSortedTestResults = testResults.GetTestResults(groupID).Tables[0].DefaultView;
                    dvSortedTestResults.Sort = Session["ColumnName"] + Session["Direction"].ToString();
                    grdTestResults.DataSource = dvSortedTestResults;
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
                    Tracer.GetLogger().LogExceptionEvent("test_result_definitions.sortGridTestResult():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
            finally
            {
                testResults = null;
                //dvSortedTestResults = null;
            }
        }

        /// <summary>
        /// Fill Institition combo
        /// </summary>
        private void fillInstitition()
        {
            DataTable dtInstitution = new DataTable();
            dtInstitution = Utility.GetInstitutionList();
            cmbInstitution.DataSource = dtInstitution;
            cmbInstitution.DataBind();

            ListItem li = new ListItem("-- Select Institution --", "-1");
            cmbInstitution.Items.Insert(0,li);
            if(instId.Length !=0)
            {
                if(cmbInstitution.Items.FindByValue(instId) != null)
                {
                    cmbInstitution.Items.FindByValue(instId).Selected = true;
                    cmbGroup.Enabled = true;
                    fillGroups(Convert.ToInt32(instId));
                }
                instId = "";
            }
            else
            {
                cmbInstitution.Items.FindByValue("-1").Selected = true;
                enbaledControl(false);
            }
        }

        /// <summary>
        /// Fill Groups combo
        /// </summary>
        /// <param name="institutionID"></param>
        private void fillGroups(int institutionID)
        {
            DataTable dtGroups = new DataTable();
            TestResults objTestResult = new TestResults();

            if(institutionID != -1)
            {
                dtGroups = objTestResult.GetLabGroups(institutionID);
            }
            cmbGroup.DataSource = dtGroups;
            cmbGroup.DataBind();

            ListItem li = new ListItem("-- Select Group --", "-1");
            cmbGroup.Items.Insert(0, li);
            cmbGroup.Items.FindByValue("-1").Selected = true;

            if (dtGroups.Rows.Count > 0)
            {
                if (groupId.Length > 0)
                {
                    if (cmbGroup.Items.FindByValue(groupId) != null)
                    {
                        cmbGroup.ClearSelection();
                        cmbGroup.SelectedValue = groupId;
                    }
                    groupId = "";
                }
                else
                {
                    cmbGroup.SelectedIndex = 0;
                }
                btnAdd.Enabled = true;
                btnImport.Enabled = true;
                enbaledControl(true);
            }
            else
            {
                enbaledControl(false);
                btnImport.Enabled = false;
                btnAdd.Enabled = false ;
            }
            if (cmbGroup.SelectedValue == "-1")
            {
                enbaledControl(false);
                btnImport.Enabled = false;
                btnAdd.Enabled = false;
            }
            fillTestResults();
            fillLabTestArea();
            
        }
        
        /// <summary>
        ///  Fill Lab Test area combo
        /// </summary>
        private void fillLabTestArea()
        {
            DataTable dtLabtestArea = null;
            TestResults objTestResult;
            int groupID = -1;

            try
            {
                if(cmbGroup.SelectedIndex > -1)
                {
                    groupID = Convert.ToInt32(cmbGroup.SelectedValue);
                    objTestResult = new TestResults();
                    dtLabtestArea = objTestResult.GetLabTestArea(groupID);
                }

                cmbLabTestArea.Items.Clear();
                cmbLabTestArea.DataSource = dtLabtestArea;
                cmbLabTestArea.DataBind();

                ListItem li = new ListItem("Other", "Other");
                int index = cmbLabTestArea.Items.Count;
                cmbLabTestArea.Items.Insert(index, li);

                ListItem li1 = new ListItem("-- Select Lab Test Area --", "-1");
                cmbLabTestArea.Items.Insert(0, li1);
                cmbLabTestArea.Items.FindByValue("-1").Selected = true;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("test_result_definitions.fillLabTestArea():: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw;
            }
            finally
            {
                objTestResult = null;
            }
        }

        /// <summary>
        /// Clear controls to add test
        /// </summary>
        private void clearControls ()
        {
             // Clears the textfields after adding the new data.
            ListItem li = new ListItem();
                txtFullTestName.Text = "";
                txtShortTestName.Text = "";
                txtHighestPossibleValue.Text = "";
                txtLowestPossibleValue.Text = "";
                cmbResultType.SelectedIndex = 0;
                cmbMeasurement.SelectedIndex = 0;                
                txtGrammar.Text = "";
                txtLabTestArea.Text = "";
                li = cmbLabTestArea.Items.FindByValue("-1");
                if(li != null)
                {
                    cmbLabTestArea.ClearSelection();
                    cmbLabTestArea.Items.FindByValue("-1").Selected = true;
                }
                ViewState["LabTestId"] = 0;
                btnAdd.Visible = true;
                btnEdit.Visible = false;
                hlinkPlay.Visible = false;
                hdnVoiceOverFile.Value = "";
         }

        /// <summary>
        /// Enable Control if add/ edit flag
        /// </summary>
        /// <param name="flg"></param>
        private void enbaledControl(bool flg)
        {
            txtFullTestName.Enabled = flg;
            txtShortTestName.Enabled = flg;
            cmbResultType.Enabled = flg;
            cmbLabTestArea.Enabled = flg;
            txtGrammar.Enabled = flg;
            flupdVoiceOver.Enabled = flg;
        }

        /// <summary>
        /// Register Java Script
        /// </summary>
        private void registerJavaScript()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var txtLowestPossibleValueClientID = '" + txtLowestPossibleValue.ClientID + "';");
            sbScript.Append("var txtHighestPossibleValueClientID = '" + txtHighestPossibleValue.ClientID + "';");
            sbScript.Append("var txtFullTestNameClientID = '" + txtFullTestName.ClientID + "';");
            sbScript.Append("var txtShortTestNameClientID = '" + txtShortTestName.ClientID + "';");
            sbScript.Append("var cmbLabTestAreaClientID = '" + cmbLabTestArea.ClientID + "';");
            sbScript.Append("var cmbResultTypeClientID = '" + cmbResultType.ClientID + "';");
            sbScript.Append("var cmbMeasurementClientID = '" + cmbMeasurement.ClientID + "';");
            sbScript.Append("var txtGrammarClientID = '" + txtGrammar.ClientID + "';");
            sbScript.Append("var reqValMeasureClientID = '" + reqValMeasure.ClientID + "';");
            sbScript.Append("var txtLabTestAreaClientID = '" + txtLabTestArea.ClientID + "';");
            sbScript.Append("var hdnTestDataChangedClientID = '" + hdnTestDataChanged.ClientID + "';");
            sbScript.Append("var hdnInstitutionValClientID = '" + hdnInstitutionVal.ClientID + "';");
            sbScript.Append("var hdnGroupValClientID = '" + hdnGroupVal.ClientID + "';");
            sbScript.Append("var cmbInstitutionClientID = '" + cmbInstitution.ClientID + "';");
            sbScript.Append("var cmbGroupClientID = '" + cmbGroup.ClientID + "';");
            sbScript.Append("var hdnIsSystemAdminClientID = '" + hdnIsSystemAdmin.ClientID + "';");
            sbScript.Append("var flupdVoiceOverClientID = '" + flupdVoiceOver.ClientID + "';");
            sbScript.Append("var hdnVoiceOverFileClientID = '" + hdnVoiceOverFile.ClientID + "';");
            sbScript.Append("var hlinkPlayClientID = '" + hlinkPlay.ClientID + "';");
                        
            sbScript.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());

            
            cmbLabTestArea.Attributes.Add("onchange", "Javascript:displayLabtestArea();");
            txtLowestPossibleValue.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStrokeOrDecimalpoint();");
            txtHighestPossibleValue.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStrokeOrDecimalpoint();");

            flupdVoiceOver.Attributes.Add("onchange", "JavaScript:testdataChanged();");
            txtFullTestName.Attributes.Add("onchange", "Javascript:testdataChanged();");
            txtShortTestName.Attributes.Add("onchange", "Javascript:testdataChanged();");
            cmbLabTestArea.Attributes.Add("onchange", "Javascript:displayLabtestArea();testdataChanged();");
            cmbResultType.Attributes.Add("onchange", "Javascript:testdataChanged();EnableValControls();");
            cmbMeasurement.Attributes.Add("onchange", "Javascript:testdataChanged();");
            txtLowestPossibleValue.Attributes.Add("onchange", "Javascript:testdataChanged();return isNumericKeyStrokeOrDecimalpoint();");
            txtGrammar.Attributes.Add("onchange", "Javascript:testdataChanged();");
            txtHighestPossibleValue.Attributes.Add("onchange", "Javascript:testdataChanged();return isNumericKeyStrokeOrDecimalpoint();");

            cmbGroup.Attributes.Add("onchange", "Javascript:return onComboChange('false');");
            cmbInstitution.Attributes.Add("onchange", "Javascript:return onComboChange('true');");
            //btnImport.Attributes.Add("onclick", "Javascript:return onTestdataChanged();");

            
            btnCancel.Attributes.Add("onclick", "Javascript:return resetTestDataChanged();");

            this.btnAdd.Attributes.Add("onclick", "javascript:return onclickbutton()");//return false;
            this.btnEdit.Attributes.Add("onclick", "javascript:return onclickbutton(true)");

            txtFullTestName.Attributes.Add("onkeypress", "Javascript:deactiveEnterAction();");
            txtShortTestName.Attributes.Add("onkeypress", "Javascript:deactiveEnterAction();");
            cmbLabTestArea.Attributes.Add("onkeypress", "Javascript:deactiveEnterAction();");
            cmbResultType.Attributes.Add("onkeypress", "Javascript:deactiveEnterAction();");

            txtLowestPossibleValue.Attributes.Add("onkeypress", "Javascript:deactiveEnterAction();");
            txtHighestPossibleValue.Attributes.Add("onkeypress", "Javascript:deactiveEnterAction();");
            cmbMeasurement.Attributes.Add("onkeypress", "Javascript:deactiveEnterAction();");

           
        }

        /// <summary>
        /// Check for Directory and upload file over there
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        private string upload()
        {
            string url = "";
            try
            {
                if (flupdVoiceOver.HasFile)
                {
                    string fileName = flupdVoiceOver.FileName;
                    string directory = ConfigurationSettings.AppSettings["VoiceOverDirectory"];
                    url = ConfigurationSettings.AppSettings["VoiceOverDirectoryBaseURL"];
                    
                    if (!System.IO.Directory.Exists(directory))
                        System.IO.Directory.CreateDirectory(directory);
                    directory += @"\" + fileName;
                    flupdVoiceOver.SaveAs(directory);
                    url +=  fileName;
                }
                return url;
               
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("Add/Edit Finding.upload:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
            return url;
        }

        //set Add Test Link for buttonLab Test Master List
        private void setAddTestLink()
        {
            string groupId = "0";
            string instID = "0";

            if (cmbGroup.SelectedIndex >= 0)
            {
                groupId = cmbGroup.SelectedValue;
                if (isSystemAdmin)
                    instID = cmbInstitution.SelectedValue;
                else
                    instID = instId; //Session[SessionConstants.INSTITUTION_ID].ToString(); 
            }
            btnImport.Attributes.Add("onclick", "Javascript:return redirect('" + groupId + "','" + instID + "');");
        }

        #endregion
    }
}