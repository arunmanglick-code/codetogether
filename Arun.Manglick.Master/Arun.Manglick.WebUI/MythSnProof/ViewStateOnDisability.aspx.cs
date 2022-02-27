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

public partial class ViewStateOnDisability : Arun.Manglick.UI.BasePage
{
    
    #region Private Variables
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        //logger.WriteLog("ViewStateOnDisability");
    }

    #endregion

    #region Private Methods
    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void btnMakeInvisble_Click(object sender, EventArgs e)
    {
        lblVisible.Enabled = false;
        ddlVisible.Enabled = false;
        btnVisible.Enabled = false;
        txtVisible.Enabled = false;
    }

    #endregion

}
