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

public partial class DefaultButtonnDefaultFocus : System.Web.UI.Page
{

    #region Private Variables
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.Form.DefaultButton = btnSave.UniqueID;
            //Page.RegisterHiddenField("__EVENTTARGET", "btnSave");
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            Page.SetFocus(txtChange);
        }
        catch
        {
            throw;
        }
    }

    #endregion

    #region Private Methods

    

    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void Cancel_Click(object sender, EventArgs e)
    {
        lblError.Text = "Cancel";
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        lblError.Text = "Save";
    }
    

    #endregion

}