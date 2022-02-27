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

public partial class CustomizeErrorHandling_DefaultMsgBox : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Private Variables
    #endregion

    #region Page Events
    #endregion

    #region Private Methods
    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void Button1_Click(object sender, EventArgs e)
    {
        int i = 5;
        int j = 0;
        try
        {
            i = i / j;
        }
        catch (Exception ex)
        {
            ex.Data["ExtraInfo"] = "Explicitly raised Test Exception";
            throw ex;
        }
    }

    protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
    {
        if (e.Exception.Data["ExtraInfo"] != null)
        {
             ScriptManager1.AsyncPostBackErrorMessage = e.Exception.Data["ExtraInfo"].ToString() + "\n\n" + e.Exception.Message;
        }
        else
        {
            ScriptManager1.AsyncPostBackErrorMessage = "An unspecified error occurred.";
        }
    }

    #endregion

}
