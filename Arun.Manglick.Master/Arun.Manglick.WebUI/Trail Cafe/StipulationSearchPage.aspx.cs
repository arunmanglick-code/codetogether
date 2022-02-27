#define MDEPERMISSIONS
//#undef MDEPERMISSIONS

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;


public partial class StipulationSearchPage : System.Web.UI.Page
{
    #region Private Variables


    #endregion

    #region Page Events

    /// <summary>
    /// Page Load event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Dec 24, 2007"></history>
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    
    #endregion

    #region Control Events

    #region Grid Events

    /// <summary>
    /// Disables checkboxs from each row of girdview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Dec 31, 2007"></history>
    protected void gdvStipulationSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }
    
    /// <summary>
    /// GridView DataBound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 13, 2008"></history>
    protected void gdvStipulationSearch_DataBound(object sender, EventArgs e)
    {
        
    }

    /// <summary>
    /// Sorting
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Arun M"></history>
    /// <history date="Feb 20, 2008"></history>
    protected void gdvStipulationSearch_Sorting(object sender, GridViewSortEventArgs e)
    {
        
    }

    /// <summary>
    /// Link button click in the grid view
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Paresh B"></history>
    /// <history date="Feb 27, 2008"></history>
    protected void lbtnName_Click(object sender, EventArgs e)
    {
        
    }

    #endregion

    /// <summary>
    /// Delete Button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Dec 24, 2007"></history>
    protected void btnDelete_Click(object sender, EventArgs e)
    {                
        //#if MDEPERMISSIONS
        //if (base.CheckErrorLabel(lblError, true))
        //{
        //    return;
        //}
        //#endif
        //bool success = true;
        //CheckBox chkDeleteRow = null;

        //try
        //{
        //    foreach (GridViewRow grdRow in gdvStipulationSearch.Rows)
        //    {
        //        chkDeleteRow = (CheckBox)grdRow.FindControl("chkSelect");
        //        if (chkDeleteRow != null)
        //        {
        //            if (chkDeleteRow.Checked)
        //            {
        //                if (!DeleteMessages(grdRow))
        //                {
        //                    success = false;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    lblNoError.Text = "";
        //    if (success)
        //    {
        //        lblNoError.Visible = true;
        //        lblNoError.Text = Resources.Messages.DeleteMessage;
        //        SessionManager.SingleRecord = false;
        //        BindGrid();
        //    }            
        //}
        //catch 
        //{
        //    throw;
        //}
    }

    /// <summary>
    /// ChangeSearch Button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Dec 24, 2007"></history>
    protected void btnChangeSearch_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    gdvStipulationSearch.Visible = false;
        //    SessionManager.SingleRecord = true;
        //    SessionManager.StartIndex = 1;
        //    SessionManager.PageIndex = 0;
        //    mEndIndex = SessionManager.Avarage;
        //    SessionManager.EndIndex = mEndIndex;
        //    //GridViewSortDirection = SortDirection.Descending;
        //    SearchRecords();
        //}
        //catch 
        //{
        //    throw;
        //}
    }

    /// <summary>
    /// Close button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Dec 24, 2007"></history>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    SessionManager.SamePageFlag = false;
        //    base.Close(String.Empty);
        //}
        //catch 
        //{
        //    throw;
        //}
    }

    /// <summary>
    /// New Button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Dec 24, 2007"></history>
    protected void btnNew_Click(object sender, EventArgs e)
    {
//        try
//        {
//            #region Permission
//#if MDEPERMISSIONS
//            if (base.CheckErrorLabel(lblError, false))
//            {
//                return;
//            }
//#endif
//            #endregion

//            RedirectToManagePage();
//        }
//        catch 
//        {
//            throw;
//        }
    }
    
    #endregion
}
