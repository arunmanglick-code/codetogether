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
        String cbReference = cm.GetCallbackEventReference(this, "MyArgument", "ClientRoutine_WhoReceives_ResultsReturnedByServer", "");
        String callbackScript = "function CallServer(MyArgument) {" + cbReference + "; }";
        cm.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

        // ------------------------------------------------------------------------------------------
        // The above will paste below code at Client side
        // ------------------------------------------------------------------------------------------
        // <script type="text/javascript">
        // function CallServer(arg) 
        // {
        //   WebForm_DoCallback('__Page',arg,receiveServerData,"",null,false); 
        // }
        // </script>
        // ------------------------------------------------------------------------------------------

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
}
