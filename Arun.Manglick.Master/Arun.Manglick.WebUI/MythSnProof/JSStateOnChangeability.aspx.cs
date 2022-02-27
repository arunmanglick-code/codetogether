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

public partial class JSStateOnChangeability : Arun.Manglick.UI.BasePage
{
    
    #region Private Variables
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        //logger.WriteLog("JSStateOnChangeability");
    }

    #endregion

    #region Private Methods
    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void btnSimplePostback_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "a", "<script type='text/javascript'> alert('Hidden Field Value - " + hdnField.Value + "'); </script>");
        Label8.Text = "DropDown's Changed Value - " + ddlChange.SelectedItem.Text;
    }

    protected void btnMakeChange_Click(object sender, EventArgs e)
    {
    }

    #endregion

}
