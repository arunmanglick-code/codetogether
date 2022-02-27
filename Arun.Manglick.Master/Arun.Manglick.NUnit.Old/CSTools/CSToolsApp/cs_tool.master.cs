#region File History

/******************************File History***************************
 * File Name        : Vocada.CSTOOLS.CS_TOOLMasterPage.master.cs
 * Author           : Prerak Shah
 * Created Date     : 
 * Purpose          : Populate menu items as per logged user access roles
 *                  : 
 
 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

 * -------------------------------------------------------------------   
 * 12-06-2007 RRD     Changed for Loading icon changes
 * 01-08-2008 Prerak  Clear message center search criteria    
 * 02-25-2008 SSK     Remove all modules build version number other than CSTools
 * 02-28-2008 Suhas   Setting Access rights for the logged User. (User Management)  
 * 13-03-2008 Suhas   Code review fixes
 * 02-06-2008 Suhas   Added Call Center Tab.  
 * 08-08-2008 Prerak  To make session variable null for Add, Edit OC page
 * 11-24-2008 RajuG   #4231 - Call Center Tab access fixed
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
using Vocada.CSTools;
using Vocada.CSTools.Common;
using BitFactory.Logging;
using Vocada.VoiceLink.Utilities;
using Vocada.CSTools.DataAccess;
public partial class cs_tool : System.Web.UI.MasterPage
{
    #region Protected Global Variables
    protected string currentTab = "";
    public string strAccess = "Access Value";
    #endregion
    protected StringBuilder navigationHist = new StringBuilder();

    #region Constants
    private const string MSG_CENTER = "MsgCenter";
    private const string GROUP_MONITOR = "GrpMonitor";
    private const string ADD_SUBSCRIBER = "AddSub";
    private const string NOTIFICATION_ERRORS = "NtfErrors";
    private const string VUI_ERRORS = "VUIError";
    #endregion Constants

    public enum MainMenu
    {
        tcnavMsg = 1,
        tcnavGrpMonitor = 2,
        tcnavSearch = 3,
        tcnavTools = 4,
        tcnavRep = 5,
        tcnavSysAdmin = 6

    }
    public MainMenu MenuID
    {
        set
        {
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[SessionConstants.LOGGED_USER_NAME] != null)
            lblLoggedinUser.Text = Session[SessionConstants.LOGGED_USER_NAME].ToString();
        else
            Page.RegisterStartupScript("SessionTimeout", "<script language='javascript'>window.location.href='login.aspx';</script>");


        Hashtable link;
        Int32 cnt = 0;

        link = (Hashtable)Session["link"];
        if (link == null)
            link = new Hashtable();
        loadJavaScripts();
        if (!IsPostBack)
        {
            string currentPage = "''";

            if (Session[SessionConstants.CURRENT_TAB] != null)
                currentTab = Session[SessionConstants.CURRENT_TAB].ToString();
            if (Session[SessionConstants.CURRENT_PAGE] != null)
                currentPage = Session[SessionConstants.CURRENT_PAGE].ToString();

            if (currentTab != "MsgCenter")
            {
                switch (currentPage)
                {
                    case "user_profile.aspx":
                        break;
                    default:
                        Session[SessionConstants.WEEK_NUMBER] = null;
                        Session[SessionConstants.SHOWMESSAGES] = null;
                        Session[SessionConstants.STATUS] = null;
                        Session[SessionConstants.GROUP] = null;
                        Session[SessionConstants.SEARCH_CRITERIA] = null;
                        break;
                }
            }
            else
            {
                if (currentPage.IndexOf("edit_oc.aspx") == -1)
                {
                    Session[SessionConstants.DT_AFTERHOUR] = null;
                    Session[SessionConstants.DT_NOTIFICATION] = null;
                }
                

            }
            if (currentPage.IndexOf("add_rp.aspx") == -1)
            {
                Session[SessionConstants.DT_ADD_AFTERHOUR] = null;
                Session[SessionConstants.DT_ADD_NOTIFICATION] = null;
            }
        }
    }
    protected override void OnLoad(EventArgs e)
    {
        try
        {
            if (Session[SessionConstants.USER_ID] == null)
            {
                Response.Redirect("login.aspx");
            }
            loadJavaScripts();
            setAccessRights();

            base.OnLoad(e);

        }
        catch (Exception ex)
        {
            if (Session[SessionConstants.USER_ID] != null)
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("cs_tool_master.master.Page_Load", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
            throw ex;
        }
    }
    private void loadJavaScripts()
    {
        currentTab = "''";
        string currentInnerTab = "''";
        string currentPage = "''";
        if (Session[SessionConstants.CURRENT_TAB] != null)
            currentTab = "'" + Session[SessionConstants.CURRENT_TAB].ToString() + "'";
        if (Session[SessionConstants.CURRENT_INNER_TAB] != null)
            currentInnerTab = "'" + Session[SessionConstants.CURRENT_INNER_TAB].ToString() + "'";
        if (Session[SessionConstants.CURRENT_PAGE] != null)
            currentPage = "'" + Session[SessionConstants.CURRENT_PAGE].ToString() + "'";

        imgMsgCenter.Attributes.Add("onLoad", "MM_nbGroup('init'," + currentTab + ",'group1','" + imgMsgCenter.ClientID + "','./img/t_msg_center.gif',1);");
        imgMsgCenter.Attributes.Add("onClick", "setTabChangeVar();");
        lnkMsgCenter.Attributes.Add("onClick", "MM_nbGroup('down'," + currentTab + ",'group1','" + imgMsgCenter.ClientID + "','./img/t_msg_center_o.gif',1);");
        lnkMsgCenter.Attributes.Add("onMouseOver", "MM_nbGroup('over'," + currentTab + ",'" + imgMsgCenter.ClientID + "','./img/t_msg_center_o.gif','./img/t_msg_center_o.gif',1);return true;");
        lnkMsgCenter.Attributes.Add("onMouseOut", "MM_nbGroup('out'," + currentTab + ");return true;");

        imgGroupMonitor.Attributes.Add("onLoad", "MM_nbGroup('init'," + currentTab + ",'group1','" + imgGroupMonitor.ClientID + "','./img/t_grp_monitor.gif',1);");
        imgGroupMonitor.Attributes.Add("onClick", "setTabChangeVar();");
        lnkGroupMonitor.Attributes.Add("onClick", "MM_nbGroup('down'," + currentTab + ",'group1','" + imgGroupMonitor.ClientID + "','./img/t_grp_monitor_o.gif',1);");
        lnkGroupMonitor.Attributes.Add("onMouseOver", "MM_nbGroup('over'," + currentTab + ",'" + imgGroupMonitor.ClientID + "','./img/t_grp_monitor_o.gif','img/t_grp_monitor_o.gif',1);");
        lnkGroupMonitor.Attributes.Add("onMouseOut", "MM_nbGroup('out'," + currentTab + ");");

        imgTools.Attributes.Add("onLoad", "MM_nbGroup('init'," + currentTab + ",'group1','" + imgTools.ClientID + "','img/t_tools.gif',1);");
        imgTools.Attributes.Add("onClick", "setTabChangeVar();");
        lnkTools.Attributes.Add("onClick", "MM_nbGroup('down'," + currentTab + ",'group1','" + imgTools.ClientID + "','img/t_tools_o.gif',1);");
        lnkTools.Attributes.Add("onMouseOver", "MM_nbGroup('over'," + currentTab + ",'" + imgTools.ClientID + "','img/t_tools_o.gif','img/t_tools_o.gif',1);");
        lnkTools.Attributes.Add("onMouseOut", "MM_nbGroup('out'," + currentTab + ");");

        imgSystemAdmin.Attributes.Add("onLoad", "MM_nbGroup('init'," + currentTab + ",'group1','" + imgSystemAdmin.ClientID + "','img/t_system_admin.gif',1);");
        imgSystemAdmin.Attributes.Add("onClick", "setTabChangeVar();");
        lnkSystemAdmin.Attributes.Add("onClick", "MM_nbGroup('down'," + currentTab + ",'group1','" + imgSystemAdmin.ClientID + "','img/t_system_admin_o.gif',1);");
        lnkSystemAdmin.Attributes.Add("onMouseOver", "MM_nbGroup('over'," + currentTab + ",'" + imgSystemAdmin.ClientID + "','img/t_system_admin_o.gif','img/t_system_admin_o.gif',1);return true;");
        lnkSystemAdmin.Attributes.Add("onMouseOut", "MM_nbGroup('out'," + currentTab + ");");

        imgCallCenter.Attributes.Add("onLoad", "MM_nbGroup('init'," + currentTab + ",'group1','" + imgCallCenter.ClientID + "','img/t_call_Center.gif',1);");
        imgCallCenter.Attributes.Add("onClick", "setTabChangeVar();");
        lnkCallCenter.Attributes.Add("onClick", "MM_nbGroup('down'," + currentTab + ",'group1','" + imgCallCenter.ClientID + "','img/t_call_Center_o.gif',1);");
        lnkCallCenter.Attributes.Add("onMouseOver", "MM_nbGroup('over'," + currentTab + ",'" + imgCallCenter.ClientID + "','img/t_system_admin_o.gif','img/t_call_Center_o.gif',1);return true;");
        lnkCallCenter.Attributes.Add("onMouseOut", "MM_nbGroup('out'," + currentTab + ");");

        bdMasterPage.Attributes.Add("onLoad", "MM_preloadImages('img/t_msg_center.gif','img/t_msg_center_o.gif','img/icon_fg_loading_n.gif','img/t_grp_monitor.gif','img/t_grp_monitor_o.gif','img/t_tools.gif','img/t_tools_o.gif','img/t_system_admin.gif','img/btn_logout_over.gif');goforit();");
        bdMasterPage.Attributes.Add("onBeforeUnload", "return SaveChanges(" + currentPage + "," + currentTab + "," + currentInnerTab + ");");
        bdMasterPage.Attributes.Add("onHelp", "window.parent.CallHelp(mapId);return false;");
    }

    /// <summary>
    /// Sets the access rights.
    /// </summary>
    private void setAccessRights()
    {
        UserInfo userInfo = Session[SessionConstants.USER_INFO] as UserInfo;
        try
        {
            if (userInfo == null)
            {
                Response.Redirect("logout.aspx");
            }
            if (Session[SessionConstants.ACCESS_RIGHTS] != null)
            {
                //Disable all the tabs fisrt 
                enableDisableAllTabs(false);
                RoleAccess roleAccess = Session[SessionConstants.ACCESS_RIGHTS] as RoleAccess;
                Task task;
                for (int currentRole = 0; currentRole < roleAccess.Tasks.Items.Count; currentRole++)
                {
                    task = roleAccess.Tasks.Items[currentRole];
                    switch (task.TaskKey)
                    {
                        case MSG_CENTER:
                            tdMessageList.Visible = true;
                            break;
                        case GROUP_MONITOR:
                            tcnavGrpMonitor.Visible = true;
                            break;
                        case ADD_SUBSCRIBER:
                            tcnavTools.Visible = true;
                            mitemAddSubscriber.Visible = true;
                            break;
                        case NOTIFICATION_ERRORS:
                            tcnavTools.Visible = true;
                            mitemDeviceError.Visible = true;
                            break;
                        case VUI_ERRORS:
                            tcnavTools.Visible = true;
                            mitemVUIErrors.Visible = true;
                            break;
                    }
                }
                if (userInfo.RoleId == UserRoles.SupportLevel2.GetHashCode())
                {
                    strAccess = "NO";
                }
            }
            else
            {
                enableDisableAllTabs(true);
                if (userInfo.RoleId == UserRoles.AccountServices.GetHashCode())
                {
                    mitemUserManagement.Visible = false;
                }
                strAccess = "YES";
            }
        }
        catch (Exception ex)
        {
            if (Session[SessionConstants.USER_ID] != null)
                Tracer.GetLogger().LogExceptionEvent(Utils.ConcatenateString("cs_tool_master.master.setAccessRights", Session[SessionConstants.USER_ID].ToString(), ex.Message, ex.StackTrace), Convert.ToInt32(Session[SessionConstants.USER_ID]));
            throw ex;
        }
    }

    /// <summary>
    /// Enables/Disable all tabs.
    /// </summary>
    /// <param name="status">true/false</param>
    private void enableDisableAllTabs(bool status)
    {
        //Message Cener Tab
        tdMessageList.Visible = status;

        //Group Monitor Tab
        tcnavGrpMonitor.Visible = status;

        //Tools Menu 
        tcnavTools.Visible = status;
        //Tools >> subMenus
        mitemAddDirectory.Visible = status;
        mitemAddNurseDir.Visible = status;
        mitemAddGroup.Visible = status;
        mitemAddSubscriber.Visible = status;
        mitemAddFinding.Visible = status;
        mitemOCGrammer.Visible = status;
        mitemOCVoiceover.Visible = status;
        mitemTestResult.Visible = status;
        mitemDeviceError.Visible = status;
        mitemVUIErrors.Visible = status;
        mitemAddOC.Visible = status;

        //System Admin Tab
        tcnavSystemAdmin.Visible = status;
        //Ssytem Admin >> submenus.
        mitemInstitueInfo.Visible = status;
        mitemUserManagement.Visible = status;

        //Hide Call Center tab
        tcnavCallCenter.Visible = status;
        
    }
}
