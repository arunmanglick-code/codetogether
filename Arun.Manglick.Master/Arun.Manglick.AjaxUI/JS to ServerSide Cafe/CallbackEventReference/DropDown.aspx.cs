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

public partial class DropDown : System.Web.UI.Page, ICallbackEventHandler
{

    #region Private Variables

    private String cities;

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RegisterCitiesCallback();
        }
    }

    #endregion

    #region Private Methods

    private void RegisterCitiesCallback()
    {
        ClientScriptManager cm = Page.ClientScript;
        String cbReference = cm.GetCallbackEventReference(this, "MyArgument", "ClientRoutine_WhoReceives_ResultsReturnedByServer", "");
        String callbackScript = "function GetCities(MyArgument) {" + cbReference + "; }";
        cm.RegisterClientScriptBlock(this.GetType(), "GetCities", callbackScript, true);
    }    

    #endregion

    #region Public Methods

    public string GetCallbackResult()
    {
        return cities;
    }
    public void RaiseCallbackEvent(string eventArgument)
    {
        if (!string.IsNullOrEmpty(eventArgument))
        {
            switch (eventArgument)
            {
                case "Maharashtra":
                    cities = "Pune|Mumbai|Lonawala";
                    break;
                case "Rajasthan":
                    cities = "Jaipur|Udaipur|Ajmer";
                    break;
                case "Gujrat":
                    cities = "Baroda|Ahmedabad|Surat";
                    break;
            }
        }
    }

    #endregion

    #region Control Events
    
    #endregion
}
