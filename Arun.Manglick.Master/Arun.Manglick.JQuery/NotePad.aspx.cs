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
using System.Text;

public partial class NotePad : System.Web.UI.Page
{
    #region Private Variables
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(SessionManager.NotePadText))
        {
            if (SessionManager.NotePadText.IndexOf("?xml") !=-1)
            {
                //String str = Server.HtmlEncode(FormatMessage(SessionManager.NotePadText));
                //txtNotePad.Text = str; // Server.HtmlDecode(str);
                txtNotePad.Text = Server.HtmlEncode(SessionManager.NotePadText);
            }
            else
            {
                txtNotePad.Text = Server.HtmlDecode(SessionManager.NotePadText);
            }
            SessionManager.NotePadText = String.Empty;
        }        
    }

    #endregion

    #region Private Methods

    private string FormatMessage(string message)
    {
        StringBuilder msgLog = new StringBuilder();
        string strTemp;
        int startIndex, endIndex;
        startIndex = 0;
        endIndex = message.IndexOf('>') + 1;
        message = message.Substring(endIndex);

       //while (message.Length > 0)
       // {
       //     strTemp = message.Substring(startIndex, endIndex);

       //     msgLog.Append(Environment.NewLine);
       //     msgLog.Append(strTemp);
       //     msgLog.Append(Environment.NewLine);

       //     message = message.Substring(endIndex);
       // }


        return message;
    }

    #endregion

    #region Public Methods
    #endregion

    #region Control Events
    #endregion
}
