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

public partial class DoPostBack : System.Web.UI.Page
{

    #region Private Variables
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

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtChange.Text = "";
    }

    protected void btnSimple_Click(object sender, EventArgs e)
    {
        txtChange.Text = "Simple";
    }
    
    #endregion

}