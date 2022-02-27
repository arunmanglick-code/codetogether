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
using Arun.Manglick.UI;

public partial class ViewStateOnVisibility : Arun.Manglick.UI.BasePage
{
    
    #region Private Variables
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
         //logger.WriteLog("ViewStateOnVisibility");
    }

    #endregion

    #region Private Methods
    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void btnMakeInvisble_Click(object sender, EventArgs e)
    {
        lblVisible.Visible = false;
        ddlVisible.Visible = false;
        btnVisible.Visible = false;
        txtVisible.Visible = false;
    }

    #endregion

}
