#region File History

/******************************File History***************************
 * File Name        : Group.cs
 * Author           : 
 * Created Date     : 28-03-07
 * Purpose          : To Get information related to Group.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 * 07-05-2007   IAK     UpdateGroupPreferences() Method Updated Added parameter ClosePrimaryBackupMessage
 * 07-30-2007   IAK     GetGroupsForInstitution() Method Added 
 * 08-10-2007   IAK     Modified Function GetGroupsForInstitution
 * 09-27-2007   IAK     Added Function GetGroupInformationByGroupID()
 * 12-03-2007 - Prerak  Call getOpenConnection function from Utility Class.
 * 12-04-2007 - Prerak  Added parameter for DirectoryTabOnDesktop on GetGrouppreferences method
 * 12-06-2007 - IAK     Modified UpdateGroupPreferences method remove field archive Messages For Days
 * 12-11-2007 - Prerak  Modified for OC FaxTemplateURL,UnitFaxTemplateURL, and CTFaxTemplate in Group Preferences. 
 * 12-12-2007 - Prerak - Rename CTFaxTemplateURL to GroupTemplateURL, Remove UnitFaxTemplateURL 
 * 13-12-2007 - Prerak - Columns added CTFaxTemplateURL and UnitFaxTemplateURL
 * 17-12-2007 - Prerak - Removed URL word from fax templates
 * 02-06-2008 - IAK -    CR- TAP Pager implementation
 * 03-03-2008 - IAK -    Removed hhtp:// text, this will come from web.config now
 * 04-08-2008 _ Prerak - UpdateGroupPreferences() Updated for SMS with Web Link parameter
 * 04-14-2008 _ Prerak - UpdateGroupPreferences() Updated for ALLow VUI Message Forwarding parameter
 * 04-16-2008 _ Prerak - UpdateGroupPreferences() Updated for MessageForwardingAlert and 
 *                       ForwardedMessageClosedAlert parameter
 * 04-17-2008 - Prerak - UpdateGroupPreferences() remove parameter for SMS with Web Link
 * 05-05-2008   Suhas    Defect # 2987 - Group Name Validation.
 * 30-05-2008   Suhas    Added parameter @allowSendToAgent
 * 14-07-2008   Prerak   UpdateGroupPreferences() for parameters @requirePatientNameInPagerAndSMS and @requirePatientNameInEmail
 * 28 Aug 2008 -IAK    - Added DS option, while updating Group Pref
 * 30-09-2008 - Raju G - Added one more parameter requireFWDLabMsgReadback to Update Group Preferences
 * 18-11-2008 - Raju G - Added output Parameter returnVal in sp VOC_CST_updateGroup - Perform Validation for duplication - Defect 4176
 * ----------------------------------------------------------------------- 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Vocada.VoiceLink.DataAccess;
using Vocada.CSTools.Common;
namespace Vocada.CSTools.DataAccess
{
    public class Group
    {
        #region  Constants Store procedures
        /// <summary>
        /// This constant stores name of stored procedure which will retrive group information of subscriber from Database
        /// </summary>
        private const string SP_GET_GROUP_INFO = "dbo.VOC_VLR_getGroupInfoBySubscriberID";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive group findings from Database
        /// </summary>
        private const string SP_GET_GROUP_FINDINGS = "dbo.getFindingsForGroup";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive group users from database
        /// </summary>
        private const string SP_GET_GROUP_USERS = "dbo.VOC_CST_getGroupUsers";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive groups for given institution from Database
        /// </summary>
        private const string SP_GET_GROUPS_FOR_INSTITUTION = "dbo.VOC_VL_getGroupsForInstitution";
        /// <summary>
        /// This constant stores name of sp which will return group information
        /// </summary>
        private const string SP_GET_GROUP_INFO_BY_GROUP_ID = "dbo.getGroupInfoByGroupID";
        /// <summary>
        /// This constant stores name of sp which will return group Subscribers
        /// </summary>
        private const string SP_GET_GROUP_SUBSCRIBERS = "dbo.VOC_CST_getSubscribersForGroup";
        /// This constant stores name of stored procedure which will get the groups by Institution ID 
        /// </summary>
        private const string SP_GET_GROUP_LIST_BY_INSTITUTE = "dbo.VOC_CST_getGroupListByInstitute";
        /// <summary>
        /// SP for VOC_VLR_getUserConfigurationDataForSubscriber for geting No. of Days
        /// </summary>
        private const string SP_GET_USER_CONFIG_DATA = "dbo.VOC_VLR_getUserConfigurationDataForSubscriber";
        /// <summary>
        /// SP for Voc_Vlr_getMessagesForSubscriber to get open messages of subscriber.
        /// </summary>
        private const string SP_GET_SUBSCRIBER_MSG = "dbo.VOC_VW_getMessagesForSubscriber";
        /// This constant stores name of stored procedure which will update Group Preferences 
        /// </summary>
        private const string SP_DELETE_GROUP = "dbo.VOC_CST_deleteGroup";

        private const string SP_INSERT_GROUP = "dbo.VOC_CST_insertGroup";
        /// <summary>
        /// This constant stores name of stored procedure which will Update Group  
        /// </summary>
        private const string SP_UPDATE_GROUP = "dbo.VOC_CST_updateGroup";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive group information of subscriber from Database
        /// </summary>
        //private const string SP_GET_GROUP_INFO = "dbo.VOC_CST_getGroupInfoByGroupID";//"dbo.getGroupInfoByGroupID";
        /// <summary>
        /// This constant stores name of stored procedure which will Insert default group findings in the database
        /// </summary>
        private const string SP_INSERT_DEFAULT_GROUPFINDINGS = "dbo.VOC_CST_insertDefaultGroupFindings";
        /// <summary>
        /// This constant stores name of stored procedure which will update Group Preferences 
        /// </summary>
        private const string SP_UPDATE_GROUP_PREF = "dbo.VOC_CST_updateGroupPreferences";
        /// <summary>
        /// This constant stores name of stored procedure which will update Group Preferences 
        /// </summary>
        private const string SP_GET_GROUP_PREF = "dbo.VOC_CST_getGroupPreferences";
        /// This constant stores name of stored procedure which will update Group Preferences 
        /// </summary>
        private const string SP_GET_GROUP_LIST = "dbo.VOC_CST_getGroupListByDirectory";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive group infromation by Grpup 800 Number and Referring physician 800 number from database
        /// </summary>
        private const string SP_GET_GROUPINFO_BY800NO = "dbo.getGroupInfoBy800Number";
        /// <summary>
        /// Check for duplicate Group Name
        /// </summary>
        private const string SP_GET_GROUPINFO_BYGRPNAME = "dbo.VOC_CST_getGroupInfoByGroupName";
        
        /// <summary>
        /// SP for VOC_CST_deleteSubscriber to soft delete subscriber.
        /// </summary>
        private const string SP_DELETE_SUBSCRIBER = "dbo.VOC_CST_deleteSubscriber";
        /// <summary>
        /// SP for VOC_CST_insertGroupSupportNote to insert support note for group.
        /// </summary>
        private const string SP_INSERT_GROUP_SUPPORT_NOTE = "dbo.VOC_CST_insertGroupSupportNote";
        /// <summary>
        /// SP for VOC_CST_getGroupSupportNotes to Get support note for group.
        /// </summary>
        private const string SP_GET_GROUP_SUPPORT_NOTE = "dbo.VOC_CST_getGroupSupportNotes";

        private const string SP_UPDATE_FAX_TEMPLATE = "dbo.VOC_CST_updateFaxTemplate";

        #endregion
        #region Constant Parameters
        /// <summary>
        /// This constant stores name of stored procedure In parameter 
        /// </summary>
        private const string GROUP_ID = "@GroupID";
        /// <summary>
        /// This constant stores name of stored procedure In parameter 
        /// </summary>
        private const string SUBSCRIBER_ID = "@subscriberID";
        /// <summary>
        /// This constant stores name of stored procedure In parameter 
        /// </summary>
        private const string INSTITUTION_ID = "@institutionID";
        /// <summary>
        /// This constant stores name of stored procedure In parameter 
        /// </summary>
        private const string ONLY_LAB_GROUP_REQUIRED = "@onlyLabGroups";
        /// <summary>
        /// This constant stores Require name capture for VUI
        /// </summary>
        private const string CLOSE_PRIMARY_BACKUP_MSG = "@ClosePrimaryBackupMessage";
        /// <summary>
        /// This constant stores name of stored procedure In parameter 
        /// </summary>
        private const string ACTIVE_ONLY = "@onlyActive";
        //Add Group Parameter
        private const string DIRECTORY_ID = "@directoryID";
        private const string GROUP_DID = "@groupDID";
        private const string GROUP_800NUMBER = "@group800Number";
        private const string REFERRING_PHYSICIAN_DID = "@referringPhysicianDID";
        private const string REFERRING_PHYSICIAN_800NUMBER = "@referringPhysician800Number";
        private const string GROUP_NAME = "@groupName";
        private const string PRACTICE_TYPE = "@practiceTypeID";
        private const string ADDRESS1 = "@address1";
        private const string ADDRESS2 = "@address2";
        private const string CITY = "@city";
        private const string STATE = "@state";
        private const string ZIP = "@zip";
        private const string PHONE = "@phone";
        private const string AFFILIATION = "@affiliation";
        private const string GROUP_VOICE_URL = "@groupVoiceURL";
        private const string TIMEZONE_ID = "@timeZoneID";
        private const string GROUP_GRAPHICAL_LOCATION = "@groupGraphicLocation";
        //private const string INSTITUTION_ID = "@institutionID";
        private const string GROUP_TYPE = "@groupType";
        private const string CSR_FIRST_NM = "@csrFirstName";
        private const string CSR_MIDDLE_NM = "@csrMiddleName";
        private const string CSR_LAST_NM = "@csrLastName";
        private const string ACTIVE = "@active";
        //Deafult Findings Parameters
        private const string NUMBER_800 = "@number";
        private const string FINDINGS1_DESCRIPTION = "@finding1Description";
        private const string FINDINGS1_VOICE_URL = "@finding1VoiceURL";
        private const string FINDINGS2_DESCRIPTION = "@finding2Description";
        private const string FINDINGS2_VOICE_URL = "@finding2VoiceURL";
        private const string FINDINGS3_DESCRIPTION = "@finding3Description";
        private const string FINDINGS3_VOICE_URL = "@finding3VoiceURL";

        private const string FINDINGS4_DESCRIPTION = "@finding4Description";
        private const string FINDINGS4_VOICE_URL = "@finding4VoiceURL";
        private const string FINDINGS5_DESCRIPTION = "@finding5Description";
        private const string FINDINGS5_VOICE_URL = "@finding5VoiceURL";
        private const string FINDINGS6_DESCRIPTION = "@finding6Description";
        private const string FINDINGS6_VOICE_URL = "@finding6VoiceURL";

        //Group Preferences Peramaters.
        private const string MESSAGE_ACTIVITY_FOR_DAYS = "@MessageActiveForDays";
        private const string ARCHIVE_MSG_FOR_DAYS = "@archiveMessagesForDays";
        private const string OVERDUE_THRESJOLD = "@overdueThreshold";
        private const string REQUIRE_MRN = "@requireMRN";
        private const string REQUIRE_RP_ACCEPTANCE = "@requireRPAcceptance";
        private const string READBACK_ENABLED = "@readbackEnabled";
        private const string REQUIRE_ACCESSION = "@requireAccession";
        private const string REQUIRE_NAME_CAPTURE = "@requireNameCapture";
        private const string ALLOW_DOWNLOAD = "@allowDownload";
        private const string REQUIRE_PATIENT_INITIALS = "@requirePatientInitials";
        private const string REQUIRE_DOB_IDENTIFIER = "@requireDOBIdentifier";
        private const string CLOSE_PRIMARY_AND_BACKUP_MSG = "@closePrimaryAndBackupMessages";
        private const string ALLOW_ALPHANUMARIC_MRN = "@allowAlphaNumericMRN";
        private const string USE_CC_AS_BACKUP = "@useCcAsBackup";
        private const string VUI_ERRORS = "@vuiErrors";
        private const string DIRECTORY_TAB = "@directoryTab";
        private const string FAX_TEMPLATE = "@ocFaxTemplate";
        private const string UNIT_FAX_TEMPLATE = "@unitFaxTemplate";
        private const string CT_FAX_TEMPLATE = "@ctFaxTemplate";
        private const string GROUP_FAX_TEMPLATE = "@groupFaxTemplate";
        private const string TEMPLATE_FOR = "@templatefor";
        private const string ALLOW_VUI_MSG_FORWARDING = "@allowVuiMsgForwaring";
        private const string MSG_FORWARDING_ALERT = "@msgForwardingAlert";
        private const string FORWARDED_MSG_CLOSED_ALERT = "@forwardedMsgClosedAlert";
        private const string ALLOW_SEND_TO_AGENT = "@allowSendToAgent";
        private const string REQUIRE_PATIENT_NAME_PAGER_SMS = "@requirePatientNameInPagerAndSMS";
        private const string REQUIRE_PATIENT_NAME_EMAIL = "@requirePatientNameInEmail";
        private const string ENABLE_DIRECTORY_SYNC = "@enableDirectorySync";

        //Group Maintenance Parameter (Support Note)
        private const string AUTHOR = "@author";
        private const string NOTE = "@note";
        private const string ROLE_ID = "@roleId";
        private const string PAGER_800_NUM = "@tap800Number";

         //Settings for Forwarded Lab Message Readback
        private const string REQUIRE_FWDLABMSG_READBACK = "@requireFWDLabMsgReadback";

        //Added for Output parameter
        private const string RETURN_VAL = "@reutnVal";
        
        #endregion Constants

        #region Public Methods

        /// <summary>
        /// Get subscriber's Group Information. 
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <returns></returns>           
        public DataTable GetGroupInformation(int subscriberID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtGroupInfo = new DataTable("GroupInfo");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drGroupInfo = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUP_INFO, objSqlParameter);

                dtGroupInfo.Load(drGroupInfo);
                drGroupInfo.Close();
                return dtGroupInfo;
            }
        }

        /// <summary>
        /// Get Group list for givens institution. 
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <returns></returns>           
        public DataTable GetGroupsForInstitution(int institutionID, bool onlyLabGroupsRequired)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtGroupInfo = new DataTable("GroupList");

                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(INSTITUTION_ID, institutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                objSqlParameter[1] = new SqlParameter(ONLY_LAB_GROUP_REQUIRED, onlyLabGroupsRequired);
                objSqlParameter[1].Direction = ParameterDirection.Input;

                SqlDataReader drGroupInfo = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUPS_FOR_INSTITUTION, objSqlParameter);

                dtGroupInfo.Load(drGroupInfo);
                drGroupInfo.Close();
                return dtGroupInfo;
            }
        }

        /// <summary>
        /// Get users of a given group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetGroupUsers(int groupID, int activeOnly)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtGroupUsers = new DataTable("GroupUsers");

                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(ACTIVE_ONLY, activeOnly);
                objSqlParameter[1].Direction = ParameterDirection.Input;


                SqlDataReader drGroupUsers = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUP_USERS, objSqlParameter);

                dtGroupUsers.Load(drGroupUsers);
                drGroupUsers.Close();
                return dtGroupUsers;
            }
        }

        /// <summary>
        /// Get all findings for a given group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetGroupFindings(int groupID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtGroupFindings = new DataTable("GroupFindings");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drGroupFindings = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUP_FINDINGS, objSqlParameter);

                dtGroupFindings.Load(drGroupFindings);
                //string dayMode = "am";
                //int hour = 0;
                //dtGroupFindings.Columns["SendOTNAt"].ReadOnly = false;
                //dtGroupFindings.Columns["SendOTNAt"].DataType = System.Type.GetType("System.String");
                //dtGroupFindings.Columns["EmbargoStartHour"].ReadOnly = false;
                //dtGroupFindings.Columns["EmbargoStartHour"].DataType = System.Type.GetType("System.String");
                //dtGroupFindings.Columns["EmbargoEndHour"].ReadOnly = false;
                //dtGroupFindings.Columns["EmbargoEndHour"].DataType = System.Type.GetType("System.String");
                //dtGroupFindings.Columns["EmbargoSpanWeekend"].ReadOnly = false;
                //dtGroupFindings.Columns["EmbargoSpanWeekend"].DataType = System.Type.GetType("System.String");
                //for (int currentfinding = 0; currentfinding < dtGroupFindings.Rows.Count; currentfinding++)
                //{
                //    if ((int)dtGroupFindings.Rows[currentfinding]["SendOTNAt"] == 0)
                //        dtGroupFindings.Rows[currentfinding]["SendOTNAt"] = "-";

                //    if (!(bool)dtGroupFindings.Rows[currentfinding]["Embargo"])
                //    {
                //        dtGroupFindings.Rows[currentfinding]["EmbargoStartHour"] = "-";
                //        dtGroupFindings.Rows[currentfinding]["EmbargoEndHour"] = "-";
                //        dtGroupFindings.Rows[currentfinding]["EmbargoSpanWeekend"] = "-";
                //    }
                //    else
                //    {
                //        hour = (int)dtGroupFindings.Rows[currentfinding]["EmbargoStartHour"];
                //        if (hour > 12)
                //        {
                //            dayMode = "pm";
                //            hour -= 12;
                //        }
                //        else if (hour == 12)
                //        {
                //            dayMode = "noon";
                //        }
                //        dtGroupFindings.Rows[currentfinding]["EmbargoStartHour"] = hour.ToString() + ":00 " + dayMode;

                //        dayMode = "am";
                //        hour = (int)dtGroupFindings.Rows[currentfinding]["EmbargoEndHour"];
                //        if (hour > 12)
                //        {
                //            dayMode = "pm";
                //            hour -= 12;
                //        }
                //        else if (hour == 12)
                //        {
                //            dayMode = "noon";
                //        }
                //        dtGroupFindings.Rows[currentfinding]["EmbargoEndHour"] = hour.ToString() + ":00 " + dayMode;
                //    }
                //}

                drGroupFindings.Close();
                return dtGroupFindings;
            }
        }       

        /// <summary>
        ///  Update Group Preferences
        /// </summary>
        /// <param name="objGroupInfo"></param>
        /// <returns></returns>
        public void UpdateGroupPreferences(GroupInformation objGroupInfo)
        {
            SqlConnection sqlConnection = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                SqlParameter[] objSqlParameter = new SqlParameter[26];

                objSqlParameter[0] = new SqlParameter(GROUP_ID, objGroupInfo.GroupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(MESSAGE_ACTIVITY_FOR_DAYS, objGroupInfo.MessageActiveForDays);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(OVERDUE_THRESJOLD, objGroupInfo.OverdueThreshold);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[3] = new SqlParameter(REQUIRE_MRN, objGroupInfo.RequireMRN);
                objSqlParameter[3].Direction = ParameterDirection.Input;
                objSqlParameter[4] = new SqlParameter(REQUIRE_RP_ACCEPTANCE, objGroupInfo.RequireRPAcceptance);
                objSqlParameter[4].Direction = ParameterDirection.Input;
                objSqlParameter[5] = new SqlParameter(REQUIRE_ACCESSION, objGroupInfo.RequireAccession);
                objSqlParameter[5].Direction = ParameterDirection.Input;
                objSqlParameter[6] = new SqlParameter(REQUIRE_NAME_CAPTURE, objGroupInfo.RequireNameCapture);
                objSqlParameter[6].Direction = ParameterDirection.Input;
                objSqlParameter[7] = new SqlParameter(ALLOW_DOWNLOAD, objGroupInfo.AllowDownload);
                objSqlParameter[7].Direction = ParameterDirection.Input;
                objSqlParameter[8] = new SqlParameter(REQUIRE_PATIENT_INITIALS, objGroupInfo.RequirePatientInitials);
                objSqlParameter[8].Direction = ParameterDirection.Input;
                objSqlParameter[9] = new SqlParameter(REQUIRE_DOB_IDENTIFIER, objGroupInfo.RequireDOBIdentifier);
                objSqlParameter[9].Direction = ParameterDirection.Input;
                objSqlParameter[10] = new SqlParameter(CLOSE_PRIMARY_AND_BACKUP_MSG, objGroupInfo.ClosePrimaryAndBackupMessages);
                objSqlParameter[10].Direction = ParameterDirection.Input;
                objSqlParameter[11] = new SqlParameter(ALLOW_ALPHANUMARIC_MRN, objGroupInfo.AllowAlphanumericMRN);
                objSqlParameter[11].Direction = ParameterDirection.Input;
                objSqlParameter[12] = new SqlParameter(USE_CC_AS_BACKUP, objGroupInfo.UseCcAsBackup);
                objSqlParameter[12].Direction = ParameterDirection.Input;
                objSqlParameter[13] = new SqlParameter(VUI_ERRORS, objGroupInfo.VUIErrors);
                objSqlParameter[13].Direction = ParameterDirection.Input;
                objSqlParameter[14] = new SqlParameter(DIRECTORY_TAB, objGroupInfo.DirectoryTabOnDesktop);
                objSqlParameter[14].Direction = ParameterDirection.Input;
                objSqlParameter[15] = new SqlParameter(FAX_TEMPLATE, objGroupInfo.OCFaxTemplate);
                objSqlParameter[15].Direction = ParameterDirection.Input;
                objSqlParameter[16] = new SqlParameter(GROUP_FAX_TEMPLATE, objGroupInfo.GroupFaxTemplate);
                objSqlParameter[16].Direction = ParameterDirection.Input;
                objSqlParameter[17] = new SqlParameter(CT_FAX_TEMPLATE, objGroupInfo.CTFaxTemplate);
                objSqlParameter[17].Direction = ParameterDirection.Input;
                objSqlParameter[18] = new SqlParameter(UNIT_FAX_TEMPLATE, objGroupInfo.UnitFaxTemplate);
                objSqlParameter[18].Direction = ParameterDirection.Input;
                objSqlParameter[19] = new SqlParameter(PAGER_800_NUM, objGroupInfo.PagerTAP800Number);
                objSqlParameter[19].Direction = ParameterDirection.Input;
                objSqlParameter[20] = new SqlParameter(ALLOW_VUI_MSG_FORWARDING, objGroupInfo.AllowVUIMsgForwarding);
                objSqlParameter[20].Direction = ParameterDirection.Input;
                //objSqlParameter[21] = new SqlParameter(MSG_FORWARDING_ALERT, objGroupInfo.MessageForwardingAlert);
                //objSqlParameter[21].Direction = ParameterDirection.Input;
                //objSqlParameter[22] = new SqlParameter(FORWARDED_MSG_CLOSED_ALERT, objGroupInfo.ForwardedMessageClosedAlert);
                //objSqlParameter[22].Direction = ParameterDirection.Input;
                objSqlParameter[21] = new SqlParameter(ALLOW_SEND_TO_AGENT, objGroupInfo.AllowSendToAgent);
                objSqlParameter[21].Direction = ParameterDirection.Input;
                objSqlParameter[22] = new SqlParameter(REQUIRE_PATIENT_NAME_PAGER_SMS, objGroupInfo.RequirePatientNameInPagerAndSMS);
                objSqlParameter[22].Direction = ParameterDirection.Input;
                objSqlParameter[23] = new SqlParameter(REQUIRE_PATIENT_NAME_EMAIL, objGroupInfo.RequirePatientNameInEmail);
                objSqlParameter[23].Direction = ParameterDirection.Input;
                objSqlParameter[24] = new SqlParameter(ENABLE_DIRECTORY_SYNC, objGroupInfo.EnableDirectorySynchronization);
                objSqlParameter[24].Direction = ParameterDirection.Input;
                objSqlParameter[25] = new SqlParameter(REQUIRE_FWDLABMSG_READBACK, objGroupInfo.RequireReadbackForFwdLabMsg);
                objSqlParameter[25].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, SP_UPDATE_GROUP_PREF, objSqlParameter);
                sqlTransaction.Commit();
                sqlConnection.Close();
            }
            catch (SqlException sqlError)
            {
                sqlTransaction.Rollback();
                sqlConnection.Close();
                throw sqlError;
            }
        }

        /// <summary>
        /// Get Group Information by Group ID
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetGroupInformationByGroupID(int groupID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtGroupInfo = new DataTable("GroupInfo");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drGroupInfo = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUP_INFO_BY_GROUP_ID, objSqlParameter);

                dtGroupInfo.Load(drGroupInfo);
                drGroupInfo.Close();
                return dtGroupInfo;
            }
        }

        /// <summary>
        /// Get subscriber's of Group. 
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>           
        public DataTable GetGroupSubscriber(int groupID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtGroupSubscriberInfo = new DataTable("GroupSubscriber");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drGroupSubscriberInfo = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUP_SUBSCRIBERS, objSqlParameter);

                dtGroupSubscriberInfo.Load(drGroupSubscriberInfo);
                drGroupSubscriberInfo.Close();
                return dtGroupSubscriberInfo;
            }
        }
        /// <summary>
        /// Return Group List by Institution ID from the database
        /// </summary>
        /// <param name="institutionID"></param>
        /// <returns></returns>
        public DataTable GetGroupListByInstitute(int institutionID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtGroupList = new DataTable();

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(INSTITUTION_ID, institutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drGroupList = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUP_LIST_BY_INSTITUTE, objSqlParameter);

                dtGroupList.Load(drGroupList);
                drGroupList.Close();
                return dtGroupList;
            }
        }
        /// <summary>
        /// This method will call "VOC_VLR_getUserConfigurationDataForSubscriber" stored procedure passing subscriberID as parameter
        /// it will return numberOfDays saved for this user in Profile, this value will be saved in the global variable "numberOfDays".
        /// </summary>
        /// <param name="cnx"></param>
        public int GetUserConfigurationData(int subscriberID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] arSqlParams = new SqlParameter[1];
                int numberOfDays = 0;

                arSqlParams[0] = new SqlParameter(SUBSCRIBER_ID, SqlDbType.Int);
                arSqlParams[0].Value = subscriberID;
                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_USER_CONFIG_DATA, arSqlParams);
                if (reader.Read())
                {
                    numberOfDays = (int)reader["numberofDays"];
                }
                else
                {
                    numberOfDays = 1;
                }
                reader.Close();
                return numberOfDays;
            }
        }
        /// <summary>
        /// Delete Group Information by Group ID. 
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>           
        public void DeleteGroupInformation(int groupID)
        {
            SqlConnection sqlConnection = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, SP_DELETE_GROUP, objSqlParameter);
                sqlTransaction.Commit();
                sqlConnection.Close();
            }
            catch (SqlException sqlerror)
            {
                sqlTransaction.Rollback();
                sqlConnection.Close();
                throw sqlerror;
            }

        }
        /// Adds Group information to the database
        /// </summary>
        /// <param name="InstitutionInformation"></param>
        /// <returns></returns>
        public int AddGroup(GroupInformation objGroupInfo)
        {
            SqlConnection conn = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = conn.BeginTransaction();
            try
            {
                int groupID = 0;
                SqlParameter[] arParams = new SqlParameter[20];

                //Create parameter for Directory ID
                arParams[0] = new SqlParameter(DIRECTORY_ID, SqlDbType.Int);
                arParams[0].Value = objGroupInfo.DirectoryID;

                //Create parameter for Group DID
                arParams[1] = new SqlParameter(GROUP_DID, SqlDbType.VarChar);
                arParams[1].Value = objGroupInfo.GroupDID;

                //Create parameter for group 800 Number
                arParams[2] = new SqlParameter(GROUP_800NUMBER, SqlDbType.VarChar);
                arParams[2].Value = objGroupInfo.Group800Number;

                //Create parameter for referringPhysicianDID
                arParams[3] = new SqlParameter(REFERRING_PHYSICIAN_DID, SqlDbType.VarChar);
                arParams[3].Value = objGroupInfo.ReferringPhysicianDID;

                //Create parameter for referringPhysician800Number
                arParams[4] = new SqlParameter(REFERRING_PHYSICIAN_800NUMBER, SqlDbType.VarChar);
                arParams[4].Value = objGroupInfo.ReferringPhysician800Number;

                //Create parameter for Group Name
                arParams[5] = new SqlParameter(GROUP_NAME, SqlDbType.VarChar);
                arParams[5].Value = objGroupInfo.GroupName;

                //Create parameter for @practiceTypeID
                arParams[6] = new SqlParameter(PRACTICE_TYPE, SqlDbType.VarChar);
                arParams[6].Value = objGroupInfo.PracticeType;

                //Create parameter for Address1
                arParams[7] = new SqlParameter(ADDRESS1, SqlDbType.VarChar);
                arParams[7].Value = objGroupInfo.Address1;

                //Create parameter for Address2
                arParams[8] = new SqlParameter(ADDRESS2, SqlDbType.VarChar);
                arParams[8].Value = objGroupInfo.Address2;

                //Create parameter for CITY
                arParams[9] = new SqlParameter(CITY, SqlDbType.VarChar);
                arParams[9].Value = objGroupInfo.City;

                //Create parameter for STATE                                    
                arParams[10] = new SqlParameter(STATE, SqlDbType.VarChar);
                arParams[10].Value = objGroupInfo.State;

                //Create parameter for ZIP
                arParams[11] = new SqlParameter(ZIP, SqlDbType.VarChar);
                arParams[11].Value = objGroupInfo.Zip;

                //Create parameter for PHONE
                arParams[12] = new SqlParameter(PHONE, SqlDbType.VarChar);
                arParams[12].Value = objGroupInfo.Phone;

                //Create parameter for @affiliation
                arParams[13] = new SqlParameter(AFFILIATION, SqlDbType.VarChar);
                arParams[13].Value = objGroupInfo.Affiliation;

                //Create parameter for @groupVoiceURL
                arParams[14] = new SqlParameter(GROUP_VOICE_URL, SqlDbType.VarChar);
                arParams[14].Value = objGroupInfo.GroupVoiceURL;

                //Create parameter for timeZoneID
                arParams[15] = new SqlParameter(TIMEZONE_ID, SqlDbType.Int);
                arParams[15].Value = objGroupInfo.TimeZoneID;

                //Create parameter for @groupGraphicLocation
                arParams[16] = new SqlParameter(GROUP_GRAPHICAL_LOCATION, SqlDbType.VarChar);
                arParams[16].Value = objGroupInfo.GroupGraphicLocation;

                //Create parameter for institutionID
                arParams[17] = new SqlParameter(INSTITUTION_ID, SqlDbType.Int);
                arParams[17].Value = objGroupInfo.InstitutionID;

                //Create parameter for @groupType
                arParams[18] = new SqlParameter(GROUP_TYPE, SqlDbType.Bit);
                arParams[18].Value = objGroupInfo.GroupType;

                arParams[19] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                arParams[19].Direction = ParameterDirection.Output;
                arParams[19].Value = groupID;


                SqlHelper.ExecuteScalar(sqlTransaction, CommandType.StoredProcedure, SP_INSERT_GROUP, arParams);
                groupID = Convert.ToInt32(arParams[19].Value);

                sqlTransaction.Commit();
                conn.Close();
                return groupID;
            }
            catch (SqlException sqlError)
            {
                sqlTransaction.Rollback();
                conn.Close();
                throw;
            }
        }
        /// <summary>
        /// Adds Group information to the database
        /// </summary>
        /// <param name="GroupInformation">Object containing Group Information</param>
        /// <returns></returns>
        public int UpdateGroup(GroupInformation objGroupInfo)
        {
            SqlConnection conn = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = conn.BeginTransaction();
            try
            {

                SqlParameter[] arParams = new SqlParameter[17];

                arParams[0] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                arParams[0].Value = objGroupInfo.GroupID;

                //Create parameter for Directory ID
                arParams[1] = new SqlParameter(DIRECTORY_ID, SqlDbType.Int);
                arParams[1].Value = objGroupInfo.DirectoryID;

                //Create parameter for Group DID
                arParams[2] = new SqlParameter(GROUP_DID, SqlDbType.VarChar);
                arParams[2].Value = objGroupInfo.GroupDID;

                //Create parameter for group 800 Number
                arParams[3] = new SqlParameter(GROUP_800NUMBER, SqlDbType.VarChar);
                arParams[3].Value = objGroupInfo.Group800Number;

                //Create parameter for referringPhysicianDID
                arParams[4] = new SqlParameter(REFERRING_PHYSICIAN_DID, SqlDbType.VarChar);
                arParams[4].Value = objGroupInfo.ReferringPhysicianDID;

                //Create parameter for referringPhysician800Number
                arParams[5] = new SqlParameter(REFERRING_PHYSICIAN_800NUMBER, SqlDbType.VarChar);
                arParams[5].Value = objGroupInfo.ReferringPhysician800Number;

                //Create parameter for Group Name
                arParams[6] = new SqlParameter(GROUP_NAME, SqlDbType.VarChar);
                arParams[6].Value = objGroupInfo.GroupName;

                //Create parameter for @practiceTypeID
                arParams[7] = new SqlParameter(PRACTICE_TYPE, SqlDbType.VarChar);
                arParams[7].Value = objGroupInfo.PracticeType;

                //Create parameter for Address1
                arParams[8] = new SqlParameter(ADDRESS1, SqlDbType.VarChar);
                arParams[8].Value = objGroupInfo.Address1;

                //Create parameter for Address2
                arParams[9] = new SqlParameter(ADDRESS2, SqlDbType.VarChar);
                arParams[9].Value = objGroupInfo.Address2;

                //Create parameter for CITY
                arParams[10] = new SqlParameter(CITY, SqlDbType.VarChar);
                arParams[10].Value = objGroupInfo.City;

                //Create parameter for STATE                                    
                arParams[11] = new SqlParameter(STATE, SqlDbType.VarChar);
                arParams[11].Value = objGroupInfo.State;

                //Create parameter for ZIP
                arParams[12] = new SqlParameter(ZIP, SqlDbType.VarChar);
                arParams[12].Value = objGroupInfo.Zip;

                //Create parameter for PHONE
                arParams[13] = new SqlParameter(PHONE, SqlDbType.VarChar);
                arParams[13].Value = objGroupInfo.Phone;

                //Create parameter for @affiliation
                arParams[14] = new SqlParameter(AFFILIATION, SqlDbType.VarChar);
                arParams[14].Value = objGroupInfo.Affiliation;

                //Create parameter for timeZoneID
                arParams[15] = new SqlParameter(TIMEZONE_ID, SqlDbType.Int);
                arParams[15].Value = objGroupInfo.TimeZoneID;

                arParams[16] = new SqlParameter(RETURN_VAL, SqlDbType.Int);
                arParams[16].Direction = ParameterDirection.Output;
                arParams[16].Value = 0;
          
               SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, SP_UPDATE_GROUP, arParams);

                sqlTransaction.Commit();
                conn.Close();

                return Convert.ToInt32(arParams[16].Value);

            }
            catch (SqlException sqlError)
            {
                sqlTransaction.Rollback();
                conn.Close();
                throw;
            }
        }
        /// <summary>
        /// Return Group List as per directoryID from the database
        /// </summary>
        /// <param name="DirectoryID"></param>
        /// <returns></returns>
        public DataTable GetGroupList(int directoryID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtGroupList = new DataTable();

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(DIRECTORY_ID, directoryID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drGroupList = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUP_LIST, objSqlParameter);

                dtGroupList.Load(drGroupList);
                drGroupList.Close();
                return dtGroupList;
            }
        }
        /// <summary>
        /// This method adds the default findings of group.
        /// </summary>
        /// <param name="voiceOverIP"></param>
        /// <param name="groupID"></param>
        public void AddDefaultGroupFindings(string voiceOverIP, int groupID,int groupType)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[14];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(FINDINGS1_DESCRIPTION, "Red");
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(FINDINGS2_DESCRIPTION, "Orange");
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[3] = new SqlParameter(FINDINGS3_DESCRIPTION, "Yellow");
                objSqlParameter[3].Direction = ParameterDirection.Input;
                objSqlParameter[4] = new SqlParameter(FINDINGS1_VOICE_URL, voiceOverIP + "/message-recordings/red.wav");
                objSqlParameter[4].Direction = ParameterDirection.Input;
                objSqlParameter[5] = new SqlParameter(FINDINGS2_VOICE_URL, voiceOverIP + "/message-recordings/orange.wav");
                objSqlParameter[5].Direction = ParameterDirection.Input;
                objSqlParameter[6] = new SqlParameter(FINDINGS3_VOICE_URL, voiceOverIP + "/message-recordings/yellow.wav");
                objSqlParameter[6].Direction = ParameterDirection.Input;
                objSqlParameter[7] = new SqlParameter(GROUP_TYPE, groupType);
                objSqlParameter[7].Direction = ParameterDirection.Input;
                objSqlParameter[8] = new SqlParameter(FINDINGS4_DESCRIPTION, "Positive");
                objSqlParameter[8].Direction = ParameterDirection.Input;
                objSqlParameter[9] = new SqlParameter(FINDINGS5_DESCRIPTION, "Negative");
                objSqlParameter[9].Direction = ParameterDirection.Input;
                objSqlParameter[10] = new SqlParameter(FINDINGS6_DESCRIPTION, "Other");
                objSqlParameter[10].Direction = ParameterDirection.Input;
                objSqlParameter[11] = new SqlParameter(FINDINGS4_VOICE_URL, voiceOverIP + "/message-recordings/positive.wav");
                objSqlParameter[11].Direction = ParameterDirection.Input;
                objSqlParameter[12] = new SqlParameter(FINDINGS5_VOICE_URL, voiceOverIP + "/message-recordings/negative.wav");
                objSqlParameter[12].Direction = ParameterDirection.Input;
                objSqlParameter[13] = new SqlParameter(FINDINGS6_VOICE_URL, voiceOverIP + "/message-recordings/other.wav");
                objSqlParameter[13].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_INSERT_DEFAULT_GROUPFINDINGS, objSqlParameter);
            }
        }
        /// <summary>
        /// This method is checked for duplicate 800 numbers for group and Refering Physicians.
        /// </summary>
        /// <param name="gp800">Group 800 Number</param>
        /// <param name="rp800">Referring Physician</param>
        /// <returns></returns>
        public int Check800Numbers(string gp800, string rp800)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                int ret = 0;
                SqlDataReader drGroup;
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter("@number", gp800);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                drGroup = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUPINFO_BY800NO, objSqlParameter);

                if (drGroup.Read())
                {
                    drGroup.Close();
                    ret = 1;
                }
                else
                {
                    drGroup.Close();
                    objSqlParameter[0] = new SqlParameter(NUMBER_800, rp800);
                    objSqlParameter[0].Direction = ParameterDirection.Input;
                    drGroup = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUPINFO_BY800NO, objSqlParameter);
                    if (drGroup.Read())
                    {
                        drGroup.Close();
                        ret = 2;
                    }

                }
                return ret;
            }
        }

        /// <summary>
        /// This method is checked for duplicate 800 numbers for group and Refering Physicians.
        /// </summary>
        /// <param name="gp800">Group 800 Number</param>
        /// <param name="rp800">Referring Physician</param>
        /// <returns></returns>
        public int CheckGroupName(string groupName, int institutionID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                int ret = 0;
                SqlDataReader drGroup;
                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(GROUP_NAME, groupName);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                objSqlParameter[1] = new SqlParameter(INSTITUTION_ID, institutionID);
                objSqlParameter[1].Direction = ParameterDirection.Input;

                drGroup = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUPINFO_BYGRPNAME, objSqlParameter);

                if (drGroup.Read())
                {
                    drGroup.Close();
                    ret = 1;
                }
                
                return ret;
            }
        }

        /// <summary>
        /// This method gets Group preferences as per Group ID
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetGroupPreferences(int groupID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtGroupPreferences = new DataTable();

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drGroupPreferences = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUP_PREF, objSqlParameter);
                dtGroupPreferences.Load(drGroupPreferences);
                drGroupPreferences.Close();
                return dtGroupPreferences;

            }
        }
        /// <summary>
        /// This Method is used to get the open messages for the subscriber. 
        /// </summary>
        /// <param name="subscriberID">subscriberID</param>
        /// <param name="roleID">RoleID</param>
        /// <returns></returns>
        public DataTable GetSubscriberMessages(int subscriberID, int roleID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtsubscriberMesgs = new DataTable();

                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                objSqlParameter[1] = new SqlParameter(ROLE_ID, SqlDbType.Int);
                objSqlParameter[1].Value = roleID;

                SqlDataReader drSubscriberMsgs = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_SUBSCRIBER_MSG, objSqlParameter);
                dtsubscriberMesgs.Load(drSubscriberMsgs);
                drSubscriberMsgs.Close();
                return dtsubscriberMesgs;

            }
        }
        /// <summary>
        /// This method make a soft delete of subscriber in the database.
        /// </summary>
        /// <param name="subscriberID"></param>
        public void DeleteSubscriber(int subscriberID)
        {
            SqlConnection sqlConnection = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, SP_DELETE_SUBSCRIBER, objSqlParameter);
                sqlTransaction.Commit();
                sqlConnection.Close();

            }
            catch (SqlException sqlError)
            {
                sqlTransaction.Rollback();
                sqlConnection.Close();
                throw sqlError;
            }
        }
        /// <summary>
        /// This method gets Support Notes of Group 
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetGroupSupportNotes(int groupID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtGrpSupprotNote = new DataTable();

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drGrpSupportNote = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUP_SUPPORT_NOTE, objSqlParameter);
                dtGrpSupprotNote.Load(drGrpSupportNote);
                drGrpSupportNote.Close();
                return dtGrpSupprotNote;

            }
        }
        /// <summary>
        /// This method Inserts Support Note for Group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public void AddGroupSupportNote(int groupID, string author, string note)
        {
            SqlConnection sqlConnection = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                SqlParameter[] objSqlParameter = new SqlParameter[3];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(AUTHOR, author);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(NOTE, note);
                objSqlParameter[2].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, SP_INSERT_GROUP_SUPPORT_NOTE, objSqlParameter);
                sqlTransaction.Commit();
                sqlConnection.Close();

            }
            catch (SqlException sqlError)
            {
                sqlTransaction.Rollback();
                sqlConnection.Close();
                throw sqlError;
            }
        }
        /// <summary>
        /// This method updates DB for using Default template  
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public void UpdateFaxTemplte(int groupID,string templatefor)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(TEMPLATE_FOR, templatefor);
                objSqlParameter[1].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_UPDATE_FAX_TEMPLATE, objSqlParameter);
            }
        }
       #endregion Public Methods
    }
}
