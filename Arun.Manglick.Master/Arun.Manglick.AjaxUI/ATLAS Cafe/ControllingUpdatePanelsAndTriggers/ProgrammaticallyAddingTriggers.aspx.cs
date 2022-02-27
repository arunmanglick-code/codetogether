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

public partial class ProgrammaticallyAddingTriggers : System.Web.UI.Page
{

    #region Private Variables
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {       
       ScriptManager1.RegisterAsyncPostBackControl(Button1);
    }

    #endregion

    #region Private Methods
    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void Button1_Click(object sender, EventArgs e)
    {
        UpdatePanel1.Update();
    }

    #endregion

}
