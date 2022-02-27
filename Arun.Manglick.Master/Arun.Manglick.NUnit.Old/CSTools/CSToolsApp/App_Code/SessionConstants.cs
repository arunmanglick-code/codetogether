#region File History

/******************************File History***************************
 * File Name        : Vocada.Veriphy.SessionConstants.cs
 * Author           : 
 * Created Date     : 
 * Purpose          : List of session varibales names.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification
 * 08-08-08     Prerak     Added Constant for Add, Edit OC page  
 * 10-31-2008   SNK        Added Constant REMEMBER_ME 
 * ------------------------------------------------------------------- 
 *                          
 */
#endregion

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for SessionConstants
/// </summary>
public class SessionConstants
{
    public const string ACCESS_RIGHTS = "CS_AccessRigths";
    public const string USER_INFO = "CS_UserInfo";
    public const string INSTITUTION_ID = "CS_InstitutionID";
    public const string INSTITUTION_NAME = "CS_InstitutionName";
    public const string ROLE_ID = "CS_RoleID";
    public const string ROLE = "CS_Role";
    public const string USER_ID = "CS_UserID";
    public const string LOGGED_USER_ID = "CS_LoggedInUser";
    public const string COPY_RIGHT_TEXT = "CS_CopyRightText";
    public const string CURRENT_PAGE = "CS_CurrentPage";
    public const string CURRENT_TAB = "CS_CurrentTab";  
    public const string CURRENT_INNER_TAB = "CS_CurrentInnerTab";
    public const string SYSTEM_ADMIN = "System Admin";
    public const string INSTITUTE_ADMIN = "Institution Admin";
    public const string LOGGED_USER_NAME = "CS_LoggedInUserName";
    public const string SHOW_All = "CS_ShowAll";
    public const string SORT_ON = "CS_SortOn";    
    public const string USER_SETTINGS = "CS_UserSettings";
    public const string LAB_GROUPS = "Lab_Groups";
    public const string RAD_GROUPS = "Rad_Groups";
    public const string ALL_GROUPS = "All_Groups";
    public const string SHOWMESSAGES = "CS_ShowMsg";
    public const string GROUP = "CS_Group";
    public const string STATUS = "CS_Status";
    public const string WEEK_NUMBER = "CS_WeekNumber";
    public const string FROM_DATE = "CS_FromDate";
    public const string SEARCH_CRITERIA = "CS_Search";
    public const string DIRECTORY_ID = "CS_DirectoryID";
    public const string DIRECTORY_NAME = "CS_DirectoryName";
    public const string DT_NOTIFICATION = "dtNotification";
    public const string DT_AFTERHOUR = "dtAfterHour";
    public const string DT_ADD_NOTIFICATION = "dtAddNotification";
    public const string DT_ADD_AFTERHOUR = "dtAddAfterHour";    


}
