#region File History

/******************************File History***************************
 * File Name        : grammar.aspx.cs
 * Author           : Prerak Shah.
 * Created Date     : 22 Auguest 2007
 * Purpose          : Shows/Update grammar for all OC's of selected directory.
 *                  : 
 *                  :
 * *********************File Modification History*********************
 * Date(mm-dd-yyyy) Developer Reason of Modification
 * 18-10-2007   IAK     Defect 2128, 2130, 2135, 2136
 * 11-28-2007   IAK     Modified function setDataGridHeight() Hide alphbets when institute or directory is not selected
 * 04-29-2008   Prerak  Defect #3069 Fixed
 * 12 Jun 2008 - Prerak - Migration of AJAX Atlas to AJAX RTM 1.0
 * 28 Nov 2008  Raju G   Focus on Edit - Defect #3101
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
using System.Text;
using System.Drawing;
using Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.Utilities;

namespace Vocada.CSTools
{

    public partial class grammar : System.Web.UI.Page
    {

        #region Class Variables
        private string strUserSettings = "No";
        protected int divHeight = 300;
        protected const string LAST_SELECTED_CHAR = "lastSelectedChar";
        protected const string SEARCH_TEXT = "searchTearm";
        protected string editText = "";
        protected DataTable dtBlank = null;
        protected const string DT = "BlankDataTable";
        #endregion Class Variables

        #region Events

        /// <summary>
        /// Load initial data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session[SessionConstants.LOGGED_USER_ID] == null || Session[SessionConstants.USER_INFO] == null)
                    Response.Redirect(Vocada.CSTools.Utils.GetReturnURL("default.aspx", "grammar.aspx", this.Page.ClientQueryString));

                //Register page level javascript variable

                UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
                registerJavaScript();

                if (!Page.IsPostBack)
                {
                    //Load page data
                    Session[SessionConstants.WEEK_NUMBER] = null;
                    Session[SessionConstants.SHOWMESSAGES] = null;
                    Session[SessionConstants.STATUS] = null;
                    Session[SessionConstants.GROUP] = null;
                    //ScriptManager.RegisterStartupScript(upnlGrammer, upnlGrammer.GetType(), "HideDiv", "<script language=" + '"' + "Javascript" + '"' + ">document.getElementById(" + '"' + "divOCGrammar" + '"' + ").style.border='none'" + ";</script>", false);

                    resetLinkButtonColor();
                    tblAlphabet.Visible = false;
                    if (userInfo.RoleId == UserRoles.InstitutionAdmin.GetHashCode())
                    {
                        hdnIsSystemAdmin.Value = "0";
                        cmbInstitutions.Visible = false;
                        lblInstName.Visible = true;
                        lblInstName.Text = userInfo.InstitutionName;
                        populateDirectories(userInfo.InstitutionID);
                        btnOCGrammar.Enabled = false;
                    }
                    else
                    {
                        hdnIsSystemAdmin.Value = "1";
                        cmbInstitutions.Visible = true;
                        lblInstName.Visible = false;
                        populateInstitutions();
                    }
                }
                Session[SessionConstants.CURRENT_TAB] = "Tools";
                Session[SessionConstants.CURRENT_INNER_TAB] = "OC Grammars";
                                
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("Grammar.Page_Load", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }

        }

        /// <summary>
        ///  Load Directory Combo with selected institution and clear grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbInstitutions_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                resetLinkButtonColor();
                populateDirectories(int.Parse(cmbInstitutions.SelectedValue));
                grdOCGrammar.EditItemIndex = -1;
                grdOCGrammar.DataSource = null;
                grdOCGrammar.DataBind();
                lblRecordCount.Text = "";
                lblNorecord.Text = "";
                hdnInstitutionVal.Value = cmbInstitutions.SelectedValue;
                Utils.RegisterJS("HideDiv1", "document.getElementById(" + '"' + divOCGrammar.ClientID + '"' + ").style.border='none'" + ";", this.Page);
                if (cmbDirectories.SelectedIndex == 0)
                {
                    btnOCGrammar.Enabled = false;
                    tblAlphabet.Visible = false;
                }
                else
                {
                    btnOCGrammar.Enabled = true;
                    tblAlphabet.Visible = true;
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grammar.cmbInstitutions_SelectedIndexChanged", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        /// <summary>
        /// Populate OC Grammmar in grid for selected direcotories
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOCGrammar_Click(object sender, System.EventArgs e)
        {
            try
            {
                resetLinkButtonColor();
                grdOCGrammar.EditItemIndex = -1;
                populateOCGrammer(int.Parse(cmbDirectories.SelectedValue));
                btnOCGrammar.Enabled = true;
                tblAlphabet.Visible = true;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grammar.btnOCGrammar_Click", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        /// <summary>
        /// This event will be get fired when the user clicks on any link, this method will fetch the records in datagrid
        /// for selected link.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void searchText_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LinkButton lnkSearch = null;
            try
            {
                //get control from which get text to search
                lnkSearch = sender as LinkButton;


                //Reset all A-Z butoons with default color
                resetLinkButtonColor();

                //Mark the selected button bold one.
                lnkSearch.Font.Bold = true;
                ViewState[LAST_SELECTED_CHAR] = lnkSearch.Text;
                txtSearch.Text = "";

                if (cmbInstitutions.SelectedValue != "-1" && cmbDirectories.SelectedValue != "-1")
                {
                    //populate list of grammars with OC last name startwith seleceted character
                    grdOCGrammar.EditItemIndex = -1;
                    populateOCGrammer(int.Parse(cmbDirectories.SelectedValue));
                }
                else
                {
                    grdOCGrammar.EditItemIndex = -1;
                    grdOCGrammar.DataSource = null;
                    grdOCGrammar.DataBind();
                    lblRecordCount.Text = "";
                    lblNorecord.Text = "";
                    Utils.RegisterJS("HideDiv1", "document.getElementById(" + '"' + divOCGrammar.ClientID + '"' + ").style.border='none'" + ";", this.Page);
                }



                if (strUserSettings == "YES")
                    lnkSearch.ForeColor = Color.Black;
                else
                    lnkSearch.ForeColor = Color.Brown;
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grammar.searchText_SelectedIndexChanged", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                lnkSearch = null;
            }
        }

        /// <summary>
        /// Change grammer view for user remove item tab, space replace with comma
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdOCGrammar_ItemDataBound(object source, DataGridItemEventArgs e)
        {
            DataRowView data = null;
            string grammar;
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    //Get current data row
                    data = e.Item.DataItem as DataRowView;


                    grammar = Convert.ToString(data["grammar"].ToString().Trim());

                    StringBuilder sbGrammar = new StringBuilder();
                    for (int i = 0; i < grammar.Length; i++)
                        if (char.IsLetterOrDigit(grammar[i]))
                            sbGrammar.Append(grammar[i]);
                        else if (char.ToString(grammar[i]) == ";")
                            sbGrammar.Append(grammar[i]);
                        else if (char.ToString(grammar[i]) == " ")
                            sbGrammar.Append(grammar[i]);
                        else if (char.ToString(grammar[i]) == ".")
                            sbGrammar.Append(grammar[i]);
                        else if (char.ToString(grammar[i]) == "'")
                            sbGrammar.Append(grammar[i]);
                    sbGrammar.Replace("</item><item>", ";");
                    sbGrammar.Replace("<item>", "");
                    sbGrammar.Replace("</item>", "");

                    if (sbGrammar.ToString().Length > 100)
                    {
                        e.Item.Cells[2].Text = sbGrammar.ToString().Substring(0, 100);
                        e.Item.Cells[2].Text = e.Item.Cells[2].Text + "...";
                        e.Item.Cells[2].ToolTip = sbGrammar.ToString();
                    }
                    else
                        e.Item.Cells[2].Text = sbGrammar.ToString();
                    if (e.Item.ItemType == ListItemType.EditItem)
                        editText = sbGrammar.ToString();

                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grammar.grdOCGrammar_ItemDataBound", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                data = null;
            }
        }

        /// <summary>
        /// Cancel Editing of the selected record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdOCGrammar_OnItemCreated(object source, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.Cells[3].Controls.Count > 1)
                {
                    //LinkButton lbUpdate = (e.Item.Cells[3].Controls[0]) as LinkButton;
                    ////lbUpdate.OnClientClick = "return validateGrammar('" + e.Item.Cells[2].Controls[1].ClientID + "','" + (e.Item.ItemIndex + 2) + "');";
                    //lbUpdate.Attributes.Add("onclick", "return validateGrammar('" + e.Item.Cells[2].Controls[1].ClientID + "','" + (e.Item.ItemIndex + 2) + "');");
                    
                    TextBox txtGrammar = (e.Item.Cells[2].Controls[1]) as TextBox;
                    //txtGrammar.MaxLength = 500;
                    txtGrammar.Attributes.Add("onblur", "JavaScript:CheckMaxLenght('" + txtGrammar.ClientID + "',500);");
                    txtGrammar.Attributes.Add("onchange", "Javascript:formDataChange('true');");
                    LinkButton lbUpdate = (e.Item.Cells[3].Controls[0]) as LinkButton;
                    lbUpdate.OnClientClick = "return validateGrammar('" + e.Item.Cells[2].Controls[1].ClientID + "','" + (e.Item.ItemIndex + 2) + "');";
                    LinkButton lbCancel = (e.Item.Cells[3].Controls[2]) as LinkButton;
                    if (e.Item.ItemIndex + 2 < 10)
                    {
                        lbCancel.OnClientClick = "javascript:formDataChange('false');__doPostBack('ctl00$ContentPlaceHolder1$grdOCGrammar$ctl0" + (e.Item.ItemIndex + 2) + "$ctl03', '');return false;";
                        //lbCancel.OnClientClick = "javascript:return formDataChange('false');";

                    }
                    else
                    {
                        lbCancel.OnClientClick = "javascript:formDataChange('false');__doPostBack('ctl00$ContentPlaceHolder1$grdOCGrammar$ctl" + (e.Item.ItemIndex + 2) + "$ctl03', '');return false;";
                        //lbCancel.OnClientClick = "javascript:formDataChange('false');";
                    }
                }
                else if (e.Item.Cells[3].Controls.Count == 1)
                {
                     LinkButton lbtnEdit = (e.Item.Cells[3].Controls[0]) as LinkButton;
                     string script = "javascript:";
                     //string isRecordEditPrv = "false";

                     //if (grdOCGrammar.EditItemIndex != -1 && (grdOCGrammar.EditItemIndex + 1) != e.Item.ItemIndex)
                     //    isRecordEditPrv = "true";

                     //script += "if(conformOnDataChange('" + isRecordEditPrv + "')){";
                     script += "if(confirmOnDataChange()){";
                     if (!Request["__eventtarget"].StartsWith("ctl00$ContentPlaceHolder1$grdOCGrammar"))
                         script += "formDataChange('false');";

                     if (e.Item.ItemIndex + 2 < 10)
                     {
                         script += "__doPostBack('ctl00$ContentPlaceHolder1$grdOCGrammar$ctl0" + (e.Item.ItemIndex + 2) + "$ctl01', '');return false;";
                     }
                     else
                     {
                         script += "__doPostBack('ctl00$ContentPlaceHolder1$grdOCGrammar$ctl" + (e.Item.ItemIndex + 2) + "$ctl01', '');return false;";
                     }
                     lbtnEdit.OnClientClick = script + "} else {return false;}";
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grammar.grdOCGrammar_OnItemCreated", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }


        /// <summary>
        /// Edit the selected record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdOCGrammar_EditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                grdOCGrammar.EditItemIndex = e.Item.ItemIndex;                
                hdnEditedText.Value = "";
                populateOCGrammer(int.Parse(cmbDirectories.SelectedValue));
                //grdOCGrammar.Items[e.Item.ItemIndex].Focus();
                TextBox tbGrammer = ((TextBox)(grdOCGrammar.Items[e.Item.ItemIndex].FindControl("txtGrammar")));
                tbGrammer.Attributes.Add("onkeypress", "JavaScript:return isValidKeyStroke();");

                if(tbGrammer != null)
                    ScriptManager.GetCurrent(this).SetFocus(tbGrammer.ClientID);

                setDatagridHeight();

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grammar.grdOCGrammar_EditCommand", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        /// <summary>
        /// Update the changes for OC grammar.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdOCGrammar_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            string grammar = "";

            int ocID = 0;
            try
            {
                //grammar = ((TextBox)e.Item.Cells[2].Controls[1]).Text;
                //ScriptManager.RegisterStartupScript(upnlGrammer, upnlGrammer.GetType(), "EditRecord", "document.getElementById(" + '"' + hdnGrammarDataChanged.ClientID + '"' + ").value='false';", true);
                       
                grammar = hdnEditedText.Value.Trim();
                //grammar = ((TextBox)e.Item.Cells[2].Controls[1]).Text;
                grammar = grammar.TrimEnd(';');
                grammar = grammar.TrimStart(';');

                if (grammar.Length == 0)
                    grammar = null;

                ocID = int.Parse(grdOCGrammar.DataKeys[e.Item.ItemIndex].ToString());

                if (grammar != null)
                {
                    grammar = grammar.Replace("'", "");
                    int indexSemiCol = 0;
                    int count = System.Text.RegularExpressions.Regex.Matches(grammar, ";").Count;
                    //int count = grammar.Length;
                    for (int i = 0; i < count; i++)
                    {
                        indexSemiCol = grammar.IndexOf(";");
                        if (indexSemiCol != -1)
                        {
                            if (grammar.Substring(indexSemiCol + 1, 1).Equals(";"))
                            {
                                grammar = grammar.Remove(indexSemiCol + 1, 1);
                            }
                            grammar = grammar.Remove(indexSemiCol, 1);
                            if (!grammar.Substring(indexSemiCol - 1, 1).Equals(">"))
                            {
                                grammar = grammar.Insert(indexSemiCol, "</item><item>");
                            }
                        }
                    }

                    grammar = grammar.Insert(0, "<item>");
                    grammar = grammar.Insert(grammar.Length, "</item>");

                }
                if (grammar != null && grammar.Length > 4000)
                {
                    //Page.RegisterStartupScript("Error", "<script language=JavaScript>alert('Grammar text should be less than 200 characters.');</script>");
                    populateOCGrammer(int.Parse(cmbDirectories.SelectedValue));
                }
                else
                {
                    if (updateGrammar(ocID, grammar))
                    {
                        grdOCGrammar.EditItemIndex = -1;
                        //hdnGrammarDataChanged.Value = "false";
                        //Utils.RegisterJS("EditRecord", "document.getElementById(" + '"' + hdnGrammarDataChanged.ClientID + '"' + ").value='false'" + ";", this.Page);
                        //ScriptManager.RegisterStartupScript(upnlGrammer, upnlGrammer.GetType(), "EditRecord", "document.getElementById(" + '"' + hdnGrammarDataChanged.ClientID + '"' + ").value='false';", true);
                        //ScriptManager.RegisterClientScriptBlock(upnlGrammer, upnlGrammer.GetType(), "EditRecord", "document.getElementById(" + '"' + hdnGrammarDataChanged.ClientID + '"' + ").value='false';", true);
                        
                        populateOCGrammer(int.Parse(cmbDirectories.SelectedValue));
                        hdnEditedText.Value = "";
                    }
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grammar.grdOCGrammar_UpdateCommand", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        /// <summary>
        /// Cancel Editing of the selected record
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdOCGrammar_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                grdOCGrammar.EditItemIndex = -1;
                hdnEditedText.Value = "";
                grdOCGrammar.DataBind();
                populateOCGrammer(int.Parse(cmbDirectories.SelectedValue));
                
                
                //grdOCGrammar.EditItemIndex = -1;
                //upnlGrammer.Update();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grammar.grdOCGrammar_CancelCommand", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }

        protected void grdOCGrammar_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            try
            {
                grdOCGrammar.CurrentPageIndex = Convert.ToInt32(hidPageIndex.Value);
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
                grdOCGrammar.EditItemIndex = -1;
                grdOCGrammar.DataSource = dvsrtOC;
                grdOCGrammar.DataBind();

                setDatagridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent("OCGrammar.grdOCGrammar_SortCommand:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
            }
        }

        protected void cmbDirectories_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                resetLinkButtonColor();
                grdOCGrammar.EditItemIndex = -1;
                grdOCGrammar.DataSource = null;
                grdOCGrammar.DataBind();
                lblRecordCount.Text = "";
                lblNorecord.Text = "";
                hdnDirectoryVal.Value = cmbDirectories.SelectedValue;
                Utils.RegisterJS("HideDiv1", "document.getElementById(" + '"' + divOCGrammar.ClientID + '"' + ").style.border='none'" + ";", this.Page);
                if (int.Parse(cmbDirectories.SelectedValue) == -1)
                {
                    btnOCGrammar.Enabled = false;
                    tblAlphabet.Visible = false;
                }
                else
                {
                    btnOCGrammar.Enabled = true;
                    tblAlphabet.Visible = true;
                }

            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grammar.cmbDirectories_SelectedIndexChanged", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
        }


        #endregion Events

        #region Private Methods

        /// <summary>
        /// Fill Institution combo box
        /// </summary>
        private void populateInstitutions()
        {
            DataTable dtInstitutions = null;
            DataRow drInstitution = null;
            try
            {
                //Get list of institutions
                dtInstitutions = Utility.GetInstitutionList();

                //Add additional row at top
                drInstitution = dtInstitutions.NewRow();
                drInstitution[1] = "-- Select Institution --";
                drInstitution[0] = -1;
                dtInstitutions.Rows.InsertAt(drInstitution, 0);

                //Bind the datasource to combo
                cmbInstitutions.DataSource = dtInstitutions;
                cmbInstitutions.DataTextField = "InstitutionName";
                cmbInstitutions.DataValueField = "InstitutionID";
                cmbInstitutions.DataBind();
                cmbInstitutions.SelectedValue = "-1";

                populateDirectories(int.Parse(cmbInstitutions.SelectedValue));
                //this.RegisterStartupScript("scriptClientIDs", "<script language=JavaScript>enableControls();</script>");
                ScriptManager.RegisterStartupScript(upnlGrammer,upnlGrammer.GetType(), "SetControl", "<script language=JavaScript>enableControls();</script>",false);


            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grammar.populateInstitutions", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                dtInstitutions = null;
                drInstitution = null;
            }
        }

        /// <summary>
        /// Fill Directory list in combo box for given institution
        /// </summary>
        /// <param name="institutionID"></param>
        private void populateDirectories(int institutionID)
        {
            DataTable dtDirectories = null;
            DataRow drDirectory = null;
            try
            {
                //Get list of Directories
                dtDirectories = Utility.GetDirectories(institutionID);

                //Add additional row at top
                drDirectory = dtDirectories.NewRow();
                drDirectory[1] = "-- Select Directory --";
                drDirectory[0] = -1;
                dtDirectories.Rows.InsertAt(drDirectory, 0);

                //Bind the datasource to combo
                cmbDirectories.DataSource = dtDirectories;
                cmbDirectories.DataTextField = "DirectoryDescription";
                cmbDirectories.DataValueField = "DirectoryID";
                cmbDirectories.DataBind();
                cmbDirectories.SelectedValue = "-1";
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grammar.populateDirectories", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                dtDirectories = null;
                drDirectory = null;
            }
        }

        /// <summary>
        /// Fill OC Grammers in datagrid for given directory.
        /// </summary>
        /// <param name="directoryID"></param>
        /// <param name="startWith"></param>
        private void populateOCGrammer(int directoryID)
        {
            OrderingClinician objOrderingClinician = null;
            DataView dvOCGrammar = null;
            DataTable dtOCGrammar = null;
            string startWith;
            string searchTerm = "";
            try
            {
                if (ViewState[LAST_SELECTED_CHAR] != null && ViewState[LAST_SELECTED_CHAR].ToString().Length > 0)
                {
                    startWith = ViewState[LAST_SELECTED_CHAR].ToString();
                }
                else
                {
                    startWith = "";
                    ViewState[SEARCH_TEXT] = txtSearch.Text;
                    if (ViewState[SEARCH_TEXT] != null)
                        searchTerm = ViewState[SEARCH_TEXT].ToString();
                }

                //Get list of OC Grammmer
                objOrderingClinician = new OrderingClinician();
                dtOCGrammar = objOrderingClinician.GetOCGrammerInfo(directoryID, startWith, searchTerm);
                Session["dtOC"] = dtOCGrammar;
                int records = dtOCGrammar.Rows.Count;
                dvOCGrammar = dtOCGrammar.DefaultView;

                dtBlank = dtOCGrammar.Clone();
                Session[DT] = dtBlank;


                //Bind the datasource to Grid
                if (Session["Direction"] != null && Session["Direction"].ToString().Length > 0)
                    dvOCGrammar.Sort = "ReferringPhysicianDisplayName " + Session["Direction"].ToString() + ", Grammar ASC";
                grdOCGrammar.DataSource = dvOCGrammar;
                if (dtOCGrammar.Rows.Count > 1)
                    grdOCGrammar.AllowSorting = true;
                else
                    grdOCGrammar.AllowSorting = false;

                grdOCGrammar.DataBind();


                if (records <= 1)
                    lblRecordCount.Text = records.ToString() + " Record";
                else
                    lblRecordCount.Text = records.ToString() + " Records";
                if (records <= 0)
                    lblNorecord.Text = "No Records available";
                else
                    lblNorecord.Text = "";

                setDatagridHeight();
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grammar.populateOCGrammer", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                objOrderingClinician = null;
                dtOCGrammar = null;
                dvOCGrammar = null;
            }
        }

        /// <summary>
        /// Update the grammar of selected OC 
        /// </summary>
        /// <param name="ocID"></param>
        /// <param name="grammar"></param>
        /// <returns>Bool</returns>
        private bool updateGrammar(int ocID, string grammar)
        {
            OrderingClinician objOrderingClinician = null;
            try
            {
                objOrderingClinician = new OrderingClinician();
                return objOrderingClinician.UpdateGrammerInfo(ocID, grammar);
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                {
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("grammar.updateGrammar", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                }
                throw ex;
            }
            finally
            {
                objOrderingClinician = null;
            }
        }

        /// <summary>
        /// Resets the bold font and color of the link buttons for search (A-Z).
        /// </summary>
        private void resetLinkButtonColor()
        {
            // Change in the Link Button Color Style for Black and White (View Text/Graphics Reason)
            Color objColor = Color.Blue;
            updateAlphaDiv();
            ViewState[LAST_SELECTED_CHAR] = null;
            if (strUserSettings == "YES")
                objColor = Color.Black;

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
        /// This method will set the height of datagrid dynamically accordingly the current rowcount of datagrid,
        /// each time when the page posts back. 
        /// </summary>
        private void setDatagridHeight()
        {
            string script = "<script type=\"text/javascript\">";
            script += "PageLoad(1);";
            script += "document.getElementById(" + '"' + divOCGrammar.ClientID + '"' + ").style.border='solid 1px #cccccc';";
            script += "document.getElementById('" + divOCGrammar.ClientID + "').style.height=setHeightOfGrid('" + grdOCGrammar.ClientID + "','" + 40 + "');";
            script += "document.getElementById('" + divOCGrammar.ClientID + "').scrollTop=document.getElementById(hdnDivScrollPosClientID).value;";
            script += "</script>";
            if (!IsPostBack)
            ScriptManager.RegisterStartupScript(upnlGrammer,upnlGrammer.GetType(), "SetHeight", script, false);
            else
            ScriptManager.RegisterClientScriptBlock(upnlGrammer, upnlGrammer.GetType(), "SetHeight2", script, false);
        }

        /// <summary>
        /// Register JS Variables
        /// </summary>
        private void registerJavaScript()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language=JavaScript>");
            sbScript.Append("var cmbInstitutionsClientID = '" + cmbInstitutions.ClientID + "';");
            sbScript.Append("var cmbDirectoriesClientID = '" + cmbDirectories.ClientID + "';");
            sbScript.Append("var hdnEditedTextClientID = '" + hdnEditedText.ClientID + "';");
            sbScript.Append("var hdnGrammarDataChangedClientID = '" + hdnGrammarDataChanged.ClientID + "';");
            sbScript.Append("var hdnInstitutionValClientID = '" + hdnInstitutionVal.ClientID + "';");
            sbScript.Append("var hdnDirectoryValClientID = '" + hdnDirectoryVal.ClientID + "';");
            sbScript.Append("var hdnIsSystemAdminClientID = '" + hdnIsSystemAdmin.ClientID + "';");
            sbScript.Append("var tblAlphabetClientID  = '" + tblAlphabet.ClientID + "';");
            sbScript.Append("var btnOCGrammarClientID  = '" + btnOCGrammar.ClientID + "';");
            sbScript.Append("var hdnDivScrollPosClientID  = '" + hdnDivScrollPos.ClientID + "';");

            sbScript.Append("enableControls();");
            sbScript.Append("</script>");
            this.RegisterStartupScript("scriptClientIDs", sbScript.ToString());

            cmbDirectories.Attributes.Add("onchange", "Javascript:return onComboChange('false');");
            cmbInstitutions.Attributes.Add("onchange", "Javascript:return onComboChange('true');");
            aA.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aB.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aC.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aD.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aE.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aF.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aG.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aH.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aI.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aJ.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aK.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aL.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aM.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aN.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aO.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aP.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aQ.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aR.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aS.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aT.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aU.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aV.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aW.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aX.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aY.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");
            aZ.Attributes.Add("onClick", "Javascript:return confirmOnDataChange();");

            //txtGrammer.Attributes.Add("onkeyPress", "JavaScript:return isNumericKeyStrokes();");
        }

        /// <summary>
        /// Update A-Z char Div 
        /// </summary>
        private void updateAlphaDiv()
        {
            if (hdnUpdateAlpha.Value == "1")
                hdnUpdateAlpha.Value = "0";
            else
                hdnUpdateAlpha.Value = "1";
        }
        /// <summary>
        /// Blank the grid data when user change the status from combo
        /// </summary>
        private void blankGrid()
        {
            //nheight = 40;
            //ViewState["nHeight"] = nheight;
            lblRecordCount.Text = "";
            lblNorecord.Text = "";
            dtBlank = Session[DT] as DataTable;
            grdOCGrammar.AllowSorting = false;
            grdOCGrammar.DataSource = dtBlank;
            grdOCGrammar.DataBind();
        }

        #endregion Private Methods
    }
}