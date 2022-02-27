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

public partial class ViewStateOnChangeability : System.Web.UI.Page
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

    protected void btnMakeInvisble_Click(object sender, EventArgs e)
    {
        lblChange.Text = "Changed";
        ddlChange.SelectedIndex = 1;
        btnChange.Text = "Changed";
        txtChange1.Text = "Changed";
        txtChange2.Text = "Changed";
        txtChangePassword.Text = "Changed";
        hdnField.Value = "Changed";
    }

    protected void btnSimplePostback_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "a", "<script type='text/javascript'> alert('Hidden Field Value - " + hdnField.Value + "'); </script>");
    }


    #endregion
    
}
