using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class UserControls_SearchPagination : System.Web.UI.UserControl
{
    #region Private Variables

    /// <summary>
    /// Delegate to invoke the Parent Page Method
    /// </summary>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    private Delegate mFirstLinkClick;
    private Delegate mPreviousLinkClick;
    private Delegate mNextLinkClick;
    private Delegate mLastLinkClick;
    private Delegate mMenuLinkClick;

    #endregion

    #region Page Events

    /// <summary>
    /// User Control Load Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Set Delegate Function for FirstLink Click
    /// </summary>
    /// <returns>Delegate</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    public Delegate FirstLinkClick
    {
        set
        {
            mFirstLinkClick = value;
        }
    }

    /// <summary>
    /// Set Delegate Function for PreviousLink Click
    /// </summary>
    /// <returns>Delegate</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    public Delegate PreviousLinkClick
    {
        set
        {
            mPreviousLinkClick = value;
        }
    }

    /// <summary>
    /// Set Delegate Function for NextLink Click
    /// </summary>
    /// <returns>Delegate</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    public Delegate NextLinkClick
    {
        set
        {
            mNextLinkClick = value;
        }
    }

    /// <summary>
    /// Set Delegate Function for LastLink Click
    /// </summary>
    /// <returns>Delegate</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    public Delegate LastLinkClick
    {
        set
        {
            mLastLinkClick = value;
        }
    }

    /// <summary>
    /// Set Delegate Function for MenuItem Click
    /// </summary>
    /// <returns>Delegate</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    public Delegate MenuLinkClick
    {
        set
        {
            mMenuLinkClick = value;
        }
    }

    /// <summary>
    /// Get Results Label control
    /// </summary>
    /// <returns>Label</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    public Label ResultsCount
    {
        get
        {
            return lblResults;
        }
    }

    /// <summary>
    /// Get FirstLink LinkButton control
    /// </summary>
    /// <returns>LinkButton</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    public LinkButton FirstLinkButton
    {
        get
        {
            return lbtnFirst;
        }
    }

    /// <summary>
    /// Get NextLink LinkButton control
    /// </summary>
    /// <returns>LinkButton</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    public LinkButton NextLinkButton
    {
        get
        {
            return lbtnNext;
        }
    }

    /// <summary>
    /// Get PreviousLink LinkButton control
    /// </summary>
    /// <returns>LinkButton</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    public LinkButton PreviousLinkButton
    {
        get
        {
            return lbtnPrev;
        }
    }

    /// <summary>
    /// Get LastLink LinkButton control
    /// </summary>
    /// <returns>LinkButton</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    public LinkButton LastLinkButton
    {
        get
        {
            return lbtnLast;
        }
    }

    /// <summary>
    /// Get HtmlTableCell PrevLink LinkButton control
    /// </summary>
    /// <returns>HtmlTableCell</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 26, 2008"></history>
    public HtmlTableCell TDPrevLinkButton
    {
        get
        {
            return tdLinkPrev;
        }
    }

    /// <summary>
    /// Get HtmlTableCell NextLink LinkButton control
    /// </summary>
    /// <returns>HtmlTableCell</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 26, 2008"></history>
    public HtmlTableCell TDNextLinkButton
    {
        get
        {
            return tdLinkNext;
        }
    }

    /// <summary>
    /// Get HtmlTableCell FirstLink LinkButton control
    /// </summary>
    /// <returns>HtmlTableCell</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 26, 2008"></history>
    public HtmlTableCell TDFirstLinkButton
    {
        get
        {
            return tdLinkFirst;
        }
    }

    /// <summary>
    /// Get HtmlTableCell LastLink LinkButton control
    /// </summary>
    /// <returns>HtmlTableCell</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 26, 2008"></history>
    public HtmlTableCell TDLastLinkButton
    {
        get
        {
            return tdLinkLast;
        }
    }

    /// <summary>
    /// Get Menu control
    /// </summary>
    /// <returns>Menu</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    public Menu MenuPagination
    {
        get
        {
            return mnuPageTop;
        }
    }

    #endregion

    #region Control Events

    /// <summary>
    /// Menu Item Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    protected void mnuPageTop_MenuItemClick(object sender, MenuEventArgs e)
    {
        try
        {
            mMenuLinkClick.DynamicInvoke(e);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// First Link Button Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    protected void FirstClick(object sender, EventArgs e)
    {
        try
        {
            mFirstLinkClick.DynamicInvoke();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Previous Link Button Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    protected void PreviousClick(object sender, EventArgs e)
    {
        try
        {
            mPreviousLinkClick.DynamicInvoke();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Next Link Button Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    protected void NextClick(object sender, EventArgs e)
    {
        try
        {
            mNextLinkClick.DynamicInvoke();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Last Link Button Click 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>void</returns>
    /// <history created="Mayur P"></history>
    /// <history date="Feb 18, 2008"></history>
    protected void LastClick(object sender, EventArgs e)
    {
        try
        {
            mLastLinkClick.DynamicInvoke();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion
}
