#region File History

/******************************File History***************************
 * File Name        : default.aspx.cs
 * Author           : Prerak Shah
 * Created Date     : August 01, 2007
 * Purpose          : Default page of CSTools Application.
 * *********************File Modification History*********************
 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 
 * 30-10-2007 Preark Shah - Added access right information.
 * 18-12-2007 IAK         - Change-After login Default navigation to Message center for all users.
 * ------------------------------------------------------------------- 
 *                          
 */
#endregion
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
using System.Text;
using System.Data.SqlClient;
using Vocada.CSTools.Common; 
using Vocada.VoiceLink.Utilities;


namespace Vocada.CSTools
{
    public partial class main : System.Web.UI.Page
    {
        public string strUserSettings = "NO";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo userInfo;
            try
           {               
                int userId = 0;

                if(!IsPostBack && Session[SessionConstants.USER_SETTINGS] == null)
                    if (Request.Cookies[SessionConstants.USER_SETTINGS] != null)
                        if(Request.Cookies[SessionConstants.USER_SETTINGS]["LabIsBlackWhite"] != null)
                        {
                            strUserSettings = Request.Cookies[SessionConstants.USER_SETTINGS]["LabIsBlackWhite"];
                            Session[SessionConstants.USER_SETTINGS] = strUserSettings;
                        }
                               
                Session[SessionConstants.SHOW_All] = null;

                if(User.Identity.Name == null)//Session[SessionConstants.SUBSCRIBER_INFO] == null)
                {
                    Response.Redirect("Logout.aspx");
                }
                else if (Session[SessionConstants.USER_ID] == null)
                {
                    userId  = int.Parse(User.Identity.Name);
                    Vocada.CSTools.DataAccess.Login login = new Vocada.CSTools.DataAccess.Login();
                    userInfo = login.ValidateLogin(userId);

                    if(userInfo.UserID == userId)
                    {
                        
                        Session[SessionConstants.LOGGED_USER_ID] = userInfo.UserID;
                        Session[SessionConstants.USER_ID] = userInfo.UserID;
                        Session[SessionConstants.LOGGED_USER_NAME] = userInfo.UserName;
                        Session[SessionConstants.USER_INFO] = userInfo; 
                    }
                }
              
                if (Request.QueryString["ReturnUrl"] != null)
                {
                    string url = Page.ClientQueryString.Substring(Page.ClientQueryString.IndexOf("=") + 1);
                    url = url.Replace("%3d", "=");
                    url = url.Replace("%3f", "?");
                    Response.Redirect(url,true);
                }
                else
                {
                    if(Session[SessionConstants.USER_ID] == null)
                        Response.Redirect("Login.aspx", true);
                    else
                    {
                        userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
                        if (userInfo.RoleId == UserRoles.SystemAdmin.GetHashCode() || userInfo.RoleId == UserRoles.AccountServices.GetHashCode() || userInfo.RoleId == UserRoles.SupportLevel1.GetHashCode() || userInfo.RoleId == UserRoles.SupportLevel2.GetHashCode())
                            Response.Redirect("message_center.aspx", true);
                        else
                            Response.Redirect("Login.aspx", true);
                    }
                }
             }
             catch (Exception ex)
             {
                 if (Session[SessionConstants.USER_ID] != null)
                 {
                     Tracer.GetLogger().LogInfoEvent("Login.Page_Load:: Exception occured for User ID - " + Session[SessionConstants.USER_ID].ToString() + " As -->" + ex.Message + " " + ex.StackTrace, Convert.ToInt32(Session[SessionConstants.USER_ID]));
                 }
                 throw ex;
              }
              finally
              {
                
              }
         }
         #region Private Methods

         #endregion Private Methods
      }
       
        

       
}