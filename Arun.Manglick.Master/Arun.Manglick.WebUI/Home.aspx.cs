using System;

/// <summary>
/// MDE Vehicle Valuation Settings.
/// </summary>
/// <history created="Arun M"></history>
/// <history date="Nov 23, 2007"></history>
public partial class Home : System.Web.UI.Page
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
            object arunObj = (object)System.Configuration.ConfigurationSettings.GetConfig("ArunManglick");
            
        }
        catch (Exception ex)
        {
            //
            // TODO: Logging exception code goes here
            //
        }
    }

    #endregion
}