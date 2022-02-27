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

public partial class Timer : System.Web.UI.Page, ICallbackEventHandler
{

    #region Private Variables

    private int countTime;

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RegisterRefreshCallback();
        }
    }

    #endregion

    #region Private Methods

    private void RegisterRefreshCallback()
    {
        ClientScriptManager cm = Page.ClientScript;
        String cbReference = cm.GetCallbackEventReference(this, "arg", "ClientRoutine_ReceivingServerData", "");
        String callbackScript = "function CallServer(arg) {" + cbReference + "; }";
        cm.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
    }    

    #endregion

    #region Public Methods

    public string GetCallbackResult()
    {
        return countTime.ToString();
    }
    public void RaiseCallbackEvent(string eventArgument)
    {
        countTime = Convert.ToInt16(eventArgument);
        countTime--;
    }

    public int GetTimerCount()
    {
        int i = 60 * 60;  // To convert in seconds
        return i;
    }

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
