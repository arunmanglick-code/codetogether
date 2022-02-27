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

public partial class Simple1 : System.Web.UI.Page
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
    
    #endregion

    #region WebMethods

    [System.Web.Services.WebMethod]
    public static void SaveData()
    {
        try
        {
            // Code to Save the Page Data
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

}
