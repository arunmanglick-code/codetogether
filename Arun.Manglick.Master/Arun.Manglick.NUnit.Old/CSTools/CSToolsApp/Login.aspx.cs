#region File History

/******************************File History***************************
 * File Name        : login.aspx.cs
 * Author           : Prerak Shah.
 * Created Date     : 17-07-2007
 * Purpose          : This Class will add new Group.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 * 02-25-2008 SSK   Version number on Login page.
 * 02-28-2008 Suhas Setting access rights for the logged user (User Management)
 * 11-07-2008 SNK   Remember me functionality
 */
#endregion
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;
using DataAccess = Vocada.CSTools.DataAccess;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.Utilities;
using Vocada.CSTools.DataAccess;

namespace Vocada.CSTools.UI
{
    /// <summary>
    /// This class will take care of logging in the correct user, redirect him to proper page and store users details
    /// in the session values.
    /// </summary>
    public partial class login : System.Web.UI.Page
    {

        #region Protected Events and methods

        protected string strUserSettings = "NO";
        /// <summary>
        /// This function is to handle weblink part from Veriphy Desktop application.
        /// This function takes radiologistid from application and opens desired link from veriphy desktop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            int userId = 0;
            UserInfo userInfo = new UserInfo();

            if (!IsPostBack)
            {
                HttpCookie cookieLogin;
                if ((cookieLogin = Request.Cookies["RememberMeCSTools"]) != null)
                {
                    if (cookieLogin.Values["RememberMe"] == true.ToString())
                    {
                        txtUsername.Text = cookieLogin.Values["LoginID"];
                        cbRememberMe.Checked = true;
                    }
                }
            }

            if (Request.Cookies[SessionConstants.USER_SETTINGS] != null)
                if (Request.Cookies[SessionConstants.USER_SETTINGS]["LabIsBlackWhite"] != null)
                {
                    strUserSettings = Request.Cookies[SessionConstants.USER_SETTINGS]["LabIsBlackWhite"];
                    Session[SessionConstants.USER_SETTINGS] = strUserSettings;
                }

            if (strUserSettings == "YES")
            {
                rfvUserName.ForeColor = Color.Black;
                rfvPassword.ForeColor = Color.Black;
                lblErrorMessage.ForeColor = Color.Black;
            }

            Session.Clear();

            if (User.Identity.Name.Length > 0)
            {                
                userId = Convert.ToInt32(User.Identity.Name);
                Vocada.CSTools.DataAccess.Login login = new Vocada.CSTools.DataAccess.Login();
                userInfo = login.ValidateLogin(userId);

                if (userInfo.UserID == userId)
                {
                    Session[SessionConstants.LOGGED_USER_ID] = userInfo.UserID;
                    Session[SessionConstants.USER_ID] = userInfo.UserID;
                    Session[SessionConstants.LOGGED_USER_NAME] = userInfo.UserName;
                    Session[SessionConstants.USER_INFO] = userInfo;
                }
            }

            if (Session[SessionConstants.USER_ID] != null)
            {
                FormsAuthentication.RedirectFromLoginPage(userId.ToString(), true);
            }
            else
            {
                txtUsername.Focus();
            }
        }

        ///// <summary>
        ///// This event will be fired when user has entered valid userid and password, it will read the SubscriberId, RoleId and GroupId values from database for that
        ///// user stores it in Session variables.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        protected void btnLogin_Click(object sender, System.EventArgs e)
        {
            string uid = txtUsername.Text;
            string pwd = txtPassword.Text;
            DataAccess.Login objLogin;
            UserInfo userInfo = new UserInfo();

            try
            {
                objLogin = new DataAccess.Login();
                userInfo = objLogin.ValidateLogin(uid, pwd);

                if (userInfo.UserID > 0)
                {


                    Session[SessionConstants.LOGGED_USER_ID] = userInfo.UserID;
                    Session[SessionConstants.USER_ID] = userInfo.UserID;
                    Session[SessionConstants.LOGGED_USER_NAME] = userInfo.UserName;
                    Session[SessionConstants.USER_INFO] = userInfo;
                    setAccessRightsSession(userInfo.RoleId);

                   
                    //If 'Remember Me' is not checked then delete the cookie of previous login (if exists)
                    if (cbRememberMe.Checked != true)
                    {
                        HttpCookie deleteCookie = Request.Cookies["RememberMeCSTools"];
                        if (deleteCookie != null)
                        {
                            deleteCookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(deleteCookie);
                        }
                    }
                    else
                    {                       
                        HttpCookie cookieRememberMe = new HttpCookie("RememberMeCSTools");
                        cookieRememberMe.Values["LoginID"] = userInfo.LoginID.ToString();
                        cookieRememberMe.Values["RememberMe"] = cbRememberMe.Checked.ToString();
                        cookieRememberMe.Expires = DateTime.Now.AddDays(3);
                        Response.Cookies.Add(cookieRememberMe);
                    }
                    FormsAuthentication.RedirectFromLoginPage(userInfo.UserID.ToString(), false);

                }
                else
                {
                    lblErrorMessage.Text = "Invalid Login and Password. Please try again.";
                    lblErrorMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                if (Session[SessionConstants.USER_ID] != null)
                    Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("cs_tool_master.master.Page_Load", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
                throw ex;
            }
            finally
            {
                objLogin = null;
            }


        }

        /// <summary>
        /// Set AccessRole Session Variable.
        /// </summary>
        /// <param name="roleID"></param>
        private void setAccessRightsSession(int roleID)
        {
            RoleAccess roleAccess = null;
            try
            {
                if (roleID == UserRoles.SupportLevel1.GetHashCode() || roleID == UserRoles.SupportLevel2.GetHashCode())
                {
                    roleAccess = new RoleAccess(roleID);
                    Session[SessionConstants.ACCESS_RIGHTS] = roleAccess;
                }
                else
                {
                    Session[SessionConstants.ACCESS_RIGHTS] = null;
                }
            }
            finally
            {
                roleAccess = null;
            }
        }

        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }



        #endregion

    }
}
