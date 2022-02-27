using System;

/// <summary>
/// MDE Vehicle Valuation Settings.
/// </summary>
/// <history created="Arun M"></history>
/// <history date="Nov 23, 2007"></history>
public partial class GoBack : System.Web.UI.Page
{   
    #region Page Events

    /// <summary>
    /// Page load event for the Vehicle Valuation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Arun M"></history>
    /// <history date="Nov 23, 2007"></history>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Title = GetLocalResourceObject("PageTitle").ToString();
        }
        catch (Exception ex)
        {
            //
            // TODO: Logging exception code goes here
            //
        }
    }

    #endregion

    #region Control Events

    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            String url = "~/JavaScript Cafe/SaveLooseChangesAfterCrossPostbackToChildPage.aspx";
            Response.Redirect(url, false);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion
}