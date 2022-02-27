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

public partial class Overriding : Arun.Manglick.UI.BasePage
{
    #region Private Variables

    private string pathName1 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"JS\\Overriding.js";

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #endregion

    #region Private Methods
    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void lnkNotePad1_Click(object sender, EventArgs e)
    {
        try
        {
            base.ReadTextStream(pathName1);
            Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

}
