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

public partial class ShowCornerPopup : System.Web.UI.Page
{
    
    #region Private Variables
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnMakeChange); 
    }   

    #endregion

    #region Private Methods
    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void btnSimplePostback_Click(object sender, EventArgs e)
    {
        PopupWin popupWin1 = new PopupWin();
        popupWin1.Visible = true;
        popupWin1.ShowLink = false;

        
        PlaceHolder1.Controls.Add(popupWin1);       
    }

   
    protected void btnMakeChange_Click(object sender, EventArgs e)
    {
       
    }

    #endregion


}
