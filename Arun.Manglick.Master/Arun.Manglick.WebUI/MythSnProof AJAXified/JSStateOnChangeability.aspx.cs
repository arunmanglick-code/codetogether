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

public partial class JSStateOnChangeability : System.Web.UI.Page
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

    protected void btnSimplePostback_Click(object sender, EventArgs e)
    {
        if (hdnField.Value.Length > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "<script type='text/javascript'> alert('Hidden Field Value - " + hdnField.Value + "'); </script>", false);         
        }        
    }

    protected void btnAJAXPostback_Click(object sender, EventArgs e)
    {

    }

    protected void btnMakeChange_Click(object sender, EventArgs e)
    {
        if (hdnField.Value.Length > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "<script type='text/javascript'> alert('Hidden Field Value - " + hdnField.Value + "'); </script>", false);
        } 
    }

    #endregion


}
