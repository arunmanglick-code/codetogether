#region File History

/******************************File History***************************
 * File Name        : Vocada.Veriphy.BusinessClasses/MessageCenter.cs
 * Author           : Swapnil K
 * Created Date     : 31-Jan-07
 * Purpose          : To take care all Database transactions for the tab Message Center.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 *   28-08-2007 - Prerak - Modify for Institution and group level filter
 *   03-12-2007 - Prerak - Call getOpenConnection function from Utility Class.
 *   18-12-2007 - IAK    - implemented CR- Show'Embargoed' in msg status
 *                         Modified method GetMessagesForBoth, GetMessagesForLab, GetMessagesForRadiology   
 *   26-12-2007 - Prerak - Modified for Advance Search
 *   28-12-2007 - Prerak - Change methods of Advance Search  
 *   31-12-2007 - Prerak - GetFindingsbyGroupID method change for only groupid parameter
 *   18-01-2008 - Prerak - Integration of Messege center sps and Optimize the code
 *   14-02-2008 - IAK    - Implemented Cache for message center - GetMessages() function
 *   03-04-2008 - Prerak - implemented CR#197 Advanced Search for Open Reply and Open ReadBack
 *   30-06-2008 - Suhas  - CR# 256 - Support Monitoring Report Implementation.
 *   20-11-2008 - ZNK    - For selecting Groups after group-type (CStools-Advance Search).
 *   25-11-2008 - Prerak - Defect #4234 -> System is showing supoort monitor messages of another group
 * ------------------------------------------------------------------- 
 *                          
 */
#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.Caching;
using System.Data.SqlClient;
using Vocada.VoiceLink.DataAccess;
using Vocada.CSTools.Common;
#endregion

namespace Vocada.CSTools.DataAccess
{
    /// <summary>
    /// Business layer class for Message Center screen.
    /// </summary>
    public class MessageCenter
    {
        #region Stored Procedures

        /// <summary>
        /// SP for VOC_VLR_getUserConfigurationDataForSubscriber
        /// </summary>
        private const string GET_USER_CONFIG_DATA = "dbo.VOC_VLR_getUserConfigurationDataForSubscriber";

        /// <summary>
        /// SP for getSubscriberInfoBySubscriberID
        /// </summary>
        private const string GET_SUBSCRIBER_INFO = "dbo.getSubscriberInfoBySubscriberID";

        /// <summary>
        /// SP for Voc_Vlr_getMessagesForMC
        /// </summary>
        private const string GET_SUBSCRIBER_MSG = "dbo.VOC_VW_getMessagesForSubscriber";

        /// <summary>
        /// SP for Voc_VW_getRecentMessagesForUnitAdmin
        /// </summary>
        private const string GET_UNIT_ADMIN_MSG = "dbo.Voc_VW_getRecentMessagesForUnitAdmin";

        /// <summary>
        /// SP for Voc_VL_getUnits
        /// </summary>
        private const string GET_UNIT_NAMES = "dbo.Voc_VW_getUnits";

        /// <summary>
        /// SP for voc_vd_getTimeZoneIDBySubscriber
        /// </summary>
        private const string GET_TIMEZONE_ID = "dbo.voc_vd_getTimeZoneIDBySubscriber";
        /// <summary>
        /// SP for VOC_CST_getGroupByGroupType
        /// </summary>
        private const string SP_GET_GROUPS_BY_GROUPTYPE = "dbo.VOC_CST_getGroupByGroupType";
        /// <summary>
        /// SP for VOC_CST_getOCByGroupID
        /// </summary>
        private const string SP_GET_OC_BY_GROUP = "dbo.VOC_CST_getOCByGroupID";
        /// <summary>
        /// SP for VOC_CST_getNurseByGroupID
        /// </summary>
        private const string SP_GET_NURSE_BY_GROUP = "dbo.VOC_CST_getNurseByGroupID";
        /// <summary>
        /// SP for VOC_CST_getUnitsByGroupID
        /// </summary>
        private const string SP_GET_UNIT_BY_GROUP = "dbo.VOC_CST_getUnitsByGroupID";
        /// <summary>
        /// SP for VOC_CST_getFindingsByGroupID
        /// </summary>
        private const string SP_GET_FINDINGS_BY_GROUP = "dbo.VOC_CST_getFindingsByGroupID";
        /// <summary>
        /// SP for VOC_CST_getSearchMsgsForRadiology
        /// </summary>
        private const string SP_SEARCH_RADIOLOGY_MSG = "dbo.VOC_CST_getSearchMsgsForRadiology";
        /// <summary>
        /// SP for VOC_CST_getSearchMsgsForRadiology
        /// </summary>
        private const string SP_SEARCH_LAB_MSG = "dbo.VOC_CST_getSearchMsgsForLab";
        /// <summary>
        /// SP for VOC_CST_getGroupUsersByGroupType
        /// </summary>
        private const string SP_GET_USERS_BY_GROUP = "dbo.VOC_CST_getGroupUsersByGroupType";
        /// <summary>
        /// SP for VOC_CST_getMessagesForRadiology
        /// </summary>
        private const string GET_RADIOLOGY_MSG = "dbo.VOC_CST_getMessagesForRadiology";
        /// <summary>
        /// SP for Voc_CST_getMessagesforLab
        /// </summary>
        private const string GET_LAB_MSG = "dbo.Voc_CST_getMessagesforLab";
        /// <summary>
        /// SP for VOC_CST_getMessagesForBoth
        /// </summary>
        private const string GET_BOTH_MSG = "dbo.VOC_CST_getMessagesForBoth";

        /// <summary>
        /// This SP is used for Support monitoring report.
        /// </summary>
        private const string GET_SUPPORT_MONITORING_REPORT = "dbo.VOC_CST_getMonitoringReportMessages";

        #endregion

        #region Private Variables
        private const int ROLE_SPECIALIST = 1;
        private const string VOC_USER_ID = "@vocUserID";
        private const string SUBSCRIBER_ID = "@subscriberID";
        private const string START_DATE = "@startDt";
        private const string END_DATE = "@endDt";
        private const string ROLE_ID = "@roleId";
        private const string GROUPTYPE_ID = "@groupTypeID";
        private const string INSTITUTION_ID = "@institutionID";
        private const string GROUP_ID = "@groupID";
        private const string WEEK_NUMBER = "@weekNumber";
        private const string FROM_DATE = "@fromDate";
        private const string OC_NAME = "@ocName";
        private const string RC_NAME = "@rcName";
        private const string DOB = "@dob";
        private const string MRN = "@mrn";
        private const string ACCESSION = "@accession";
        private const string FINDING_NAME = "@findingName";
        private const string MSG_STATUS = "@msgStatus";
        private const string NURSE_NAME = "@nurseName";
        private const string UNIT_NAME = "@UnitName";
        private const string GROUP_TYPE = "@groupType";
        private const string REPORTFOR = "@reportFor";
        #endregion

        #region Public Methods
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
                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, GET_USER_CONFIG_DATA, arSqlParams);
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
        /// Returns all the units for the given userID to be displayed in the Units dropdown for Unit Admin.
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <param name="numberOfDays"></param>
        /// <returns></returns>
        public DataTable GetUnitNames(int vocUserID, int roleID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlDataReader reader = null;
                DataTable dtUnits = new DataTable();

                SqlParameter[] arSqlParams = new SqlParameter[2];
                arSqlParams[0] = new SqlParameter(SUBSCRIBER_ID, SqlDbType.Int);
                arSqlParams[0].Value = vocUserID;

                arSqlParams[1] = new SqlParameter(ROLE_ID, SqlDbType.Int);
                arSqlParams[1].Value = roleID;

                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, GET_UNIT_NAMES, arSqlParams);
                dtUnits.Load(reader);
                reader.Close();
                return dtUnits;
            }

        }
        /// <summary>
        /// Returns all the messages for the unit admin or charge nurse within the given date range.
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <param name="numberOfDays"></param>
        /// <returns></returns>
        public DataTable GetMessagesForUnitAdmin(int vocUserId, int numberOfDays, int roleID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlDataReader reader = null;
                DataTable dtMessages = new DataTable();

                SqlParameter[] arSqlParams = new SqlParameter[4];
                arSqlParams[0] = new SqlParameter(SUBSCRIBER_ID, SqlDbType.Int);
                arSqlParams[0].Value = vocUserId;

                arSqlParams[1] = new SqlParameter(START_DATE, SqlDbType.DateTime);
                arSqlParams[1].Value = DateTime.Now.Subtract(TimeSpan.FromDays(numberOfDays));

                arSqlParams[2] = new SqlParameter(END_DATE, SqlDbType.DateTime);
                arSqlParams[2].Value = DateTime.Now;

                arSqlParams[3] = new SqlParameter(ROLE_ID, SqlDbType.Int);
                arSqlParams[3].Value = roleID;

                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, GET_UNIT_ADMIN_MSG, arSqlParams);
                dtMessages.Load(reader);
                reader.Close();
                return dtMessages;
            }

        }
        /// <summary>
        /// Returns all the messages for the subscriber within the given date range.
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <param name="numberOfDays"></param>
        /// <returns></returns>
        public DataTable GetMessagesForSubscriber(int subscriberID, int numberOfDays, int roleID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlDataReader reader = null;
                DataTable dtMessages = new DataTable();

                SqlParameter[] arSqlParams = new SqlParameter[4];
                arSqlParams[0] = new SqlParameter(SUBSCRIBER_ID, SqlDbType.Int);
                arSqlParams[0].Value = subscriberID;

                arSqlParams[1] = new SqlParameter(START_DATE, SqlDbType.DateTime);
                arSqlParams[1].Value = DateTime.Now.Subtract(TimeSpan.FromDays(numberOfDays));

                arSqlParams[2] = new SqlParameter(END_DATE, SqlDbType.DateTime);
                arSqlParams[2].Value = DateTime.Now;

                arSqlParams[3] = new SqlParameter(ROLE_ID, SqlDbType.Int);
                arSqlParams[3].Value = roleID;

                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, GET_SUBSCRIBER_MSG, arSqlParams);
                dtMessages.Load(reader);
                reader.Close();
                return dtMessages;
            }

        }
        /// <summary>
        /// Retrives the Timezone Id for the logged in subscriber group.
        /// </summary>
        /// <param name="subscriberId">SubscriberId</param>
        /// <returns>TimeZone Id</returns>
        public int GetTimeZoneIDForSubscriber(int subscriberID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                int timeZoneId = 0;
                SqlParameter[] arSqlParams = new SqlParameter[1];
                arSqlParams[0] = new SqlParameter(SUBSCRIBER_ID, SqlDbType.Int);
                arSqlParams[0].Value = subscriberID;
                SqlDataReader drTimeZone = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, GET_TIMEZONE_ID, arSqlParams);
                while (drTimeZone.Read())
                {
                    int timeZoneIdOrdinal = drTimeZone.GetOrdinal("TimeZoneID");
                    timeZoneId = drTimeZone.GetValue(timeZoneIdOrdinal) == DBNull.Value ? 0 : drTimeZone.GetInt32(timeZoneIdOrdinal);
                }
                drTimeZone.Close();
                return timeZoneId;
            }
        }
        /// <summary>
        /// This Method returns Data table contaioning groups by group type (Radiology/Lab) for all institution.
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetGroupsbyGroupType(int groupTypeID, int institutionID)
        {
            DataSet objDS = null;
            DataTable dtGroups = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(GROUPTYPE_ID, groupTypeID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(INSTITUTION_ID, institutionID);
                objSqlParameter[1].Direction = ParameterDirection.Input;

                objDS = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, SP_GET_GROUPS_BY_GROUPTYPE, objSqlParameter);
                dtGroups = objDS.Tables[0].Copy();
                objDS = null; 
                return dtGroups;
            }
        }
        /// <summary>
        /// This Method returns Data table contaioning OC by group ID.
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetOCbyGroup(int groupID, int groupType)
        {
            DataSet objDS = null;
            DataTable dtOCs = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(GROUP_TYPE, groupType);
                objSqlParameter[1].Direction = ParameterDirection.Input;

                objDS = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, SP_GET_OC_BY_GROUP, objSqlParameter);
                dtOCs = objDS.Tables[0].Copy();
                objDS = null;
                return dtOCs;
            }
        }
        /// <summary>
        /// This Method returns Data table contaioning Nurses by group ID.
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetNurses(int groupID)
        {
            DataSet objDS = null;
            DataTable dtNurses = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                objDS = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, SP_GET_NURSE_BY_GROUP, objSqlParameter);
                dtNurses = objDS.Tables[0].Copy();
                objDS = null;
                return dtNurses;
            }
        }
        /// <summary>
        /// This Method returns Data table contaioning Units by group ID.
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetUnits(int groupID)
        {
            DataSet objDS = null;
            DataTable dtUnits = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                objDS = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, SP_GET_UNIT_BY_GROUP, objSqlParameter);
                dtUnits = objDS.Tables[0].Copy();
                objDS = null;
                return dtUnits;
            }
        }
        /// <summary>
        /// This Method returns Data table contaioning Findigs by group ID and group type.
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetFindingsbyGroupID(int groupID)
        {
            DataSet objDS = null;
            DataTable dtFindings = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                objDS = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, SP_GET_FINDINGS_BY_GROUP, objSqlParameter);
                dtFindings = objDS.Tables[0].Copy();
                objDS = null;
                return dtFindings;
            }
        }
        /// <summary>
        /// This Method returns Data table contaioning Subscribers by group ID and groupType.
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetGroupUsersByGroupType(int groupID, int groupType)
        {
            DataSet objDS = null;
            DataTable dtFindings = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(GROUP_TYPE, groupType);
                objSqlParameter[1].Direction = ParameterDirection.Input;

                objDS = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, SP_GET_USERS_BY_GROUP, objSqlParameter);
                dtFindings = objDS.Tables[0].Copy();
                objDS = null;
                return dtFindings;
            }
        }
        /// <summary>
        /// Returns all the SEARCHED messages for the RADIOLOGY Groups.
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable SearchMessagesForRadiology(int institutionID, Search objSearch)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataSet objDS = null;
                DataTable dtMessages = null;

                string msgSts;
                if (objSearch.MessageStatus == (int)MessageStatus.Open)
                    msgSts = "OPEN";
                else if (objSearch.MessageStatus == (int)MessageStatus.RecentlyClose)
                    msgSts = "CLOSE";
                else if (objSearch.MessageStatus == (int)MessageStatus.DirectCommunication)
                    msgSts = "DOCUMENTED";
                else if (objSearch.MessageStatus == (int)MessageStatus.OpenReplies)
                    msgSts = "REPLIES";
                else if (objSearch.MessageStatus == (int)MessageStatus.OpenReadBacks)
                    msgSts = "READBACKS";
                else
                    msgSts = "ALL";

                SqlParameter[] objSqlParameter = new SqlParameter[11];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, objSearch.GroupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(INSTITUTION_ID, institutionID);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(START_DATE, objSearch.FromDate);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[3] = new SqlParameter(END_DATE, objSearch.ToDate);
                objSqlParameter[3].Direction = ParameterDirection.Input;
                objSqlParameter[4] = new SqlParameter(OC_NAME, objSearch.OCName);
                objSqlParameter[4].Direction = ParameterDirection.Input;
                objSqlParameter[5] = new SqlParameter(RC_NAME, objSearch.RCName);
                objSqlParameter[5].Direction = ParameterDirection.Input;
                objSqlParameter[6] = new SqlParameter(FINDING_NAME, objSearch.FindingName);
                objSqlParameter[6].Direction = ParameterDirection.Input;
                objSqlParameter[7] = new SqlParameter(DOB, objSearch.DOB);
                objSqlParameter[7].Direction = ParameterDirection.Input;
                objSqlParameter[8] = new SqlParameter(MRN, objSearch.MRN);
                objSqlParameter[8].Direction = ParameterDirection.Input;
                objSqlParameter[9] = new SqlParameter(ACCESSION, objSearch.Accession);
                objSqlParameter[9].Direction = ParameterDirection.Input;

                objSqlParameter[10] = new SqlParameter(MSG_STATUS, msgSts);
                objSqlParameter[10].Direction = ParameterDirection.Input;

                objDS = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, SP_SEARCH_RADIOLOGY_MSG, objSqlParameter);
                dtMessages = objDS.Tables[0].Copy();
                objDS = null;
                return dtMessages;
            }

        }
        /// <summary>
        /// Returns all the SEARCHED messages for the LAB Groups.
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable SearchMessagesForLab(int institutionID, Search objSearch)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataSet objDS = null;
                DataTable dtMessages = null;

                string msgSts;
                if (objSearch.MessageStatus == (int)MessageStatus.Open)
                    msgSts = "OPEN";
                else if (objSearch.MessageStatus == (int)MessageStatus.RecentlyClose)
                    msgSts = "CLOSE";
                else if (objSearch.MessageStatus == (int)MessageStatus.DirectCommunication)
                    msgSts = "DOCUMENTED";
                else if (objSearch.MessageStatus == (int)MessageStatus.OpenReplies)
                    msgSts = "REPLIES";
                else
                    msgSts = "ALL";

                SqlParameter[] objSqlParameter = new SqlParameter[13];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, objSearch.GroupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(INSTITUTION_ID, institutionID);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(START_DATE, objSearch.FromDate);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[3] = new SqlParameter(END_DATE, objSearch.ToDate);
                objSqlParameter[3].Direction = ParameterDirection.Input;
                objSqlParameter[4] = new SqlParameter(OC_NAME, objSearch.OCName);
                objSqlParameter[4].Direction = ParameterDirection.Input;
                objSqlParameter[5] = new SqlParameter(RC_NAME, objSearch.RCName);
                objSqlParameter[5].Direction = ParameterDirection.Input;
                objSqlParameter[6] = new SqlParameter(FINDING_NAME, objSearch.FindingName);
                objSqlParameter[6].Direction = ParameterDirection.Input;
                objSqlParameter[7] = new SqlParameter(DOB, objSearch.DOB);
                objSqlParameter[7].Direction = ParameterDirection.Input;
                objSqlParameter[8] = new SqlParameter(MRN, objSearch.MRN);
                objSqlParameter[8].Direction = ParameterDirection.Input;
                objSqlParameter[9] = new SqlParameter(ACCESSION, objSearch.Accession);
                objSqlParameter[9].Direction = ParameterDirection.Input;
                objSqlParameter[10] = new SqlParameter(MSG_STATUS, msgSts);
                objSqlParameter[10].Direction = ParameterDirection.Input;
                objSqlParameter[11] = new SqlParameter(NURSE_NAME, objSearch.NurseName);
                objSqlParameter[11].Direction = ParameterDirection.Input;
                objSqlParameter[12] = new SqlParameter(UNIT_NAME, objSearch.UnitName);
                objSqlParameter[12].Direction = ParameterDirection.Input;

                objDS = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, SP_SEARCH_LAB_MSG, objSqlParameter);
                dtMessages = objDS.Tables[0].Copy();
                objDS = null;
                return dtMessages;
            }

        }
        /// <summary>
        /// Returns all the messages for Groups.
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetMessages(int groupID, int msgStatus, int weekNumber, string fromDate, int institutionID, string msgFor)
        {
            string cacheKey = "GetMessages:" + groupID + ":" + msgStatus + ":" + weekNumber + ":" + fromDate + ":" + institutionID + ":" + msgFor;
            Cache cache = HttpContext.Current.Cache;
            if (cache[cacheKey] != null) return (DataTable)cache[cacheKey];
            else
            {
                using (SqlConnection cnx = Utility.getOpenConnection())
                {
                    DataSet objDS = null;

                    if (msgStatus == MessageStatus.SupportMonitoringReport.GetHashCode())
                    {
                        SqlParameter[] objSqlParameter = new SqlParameter[2];
                        if (msgFor == "RAD")
                            objSqlParameter[0] = new SqlParameter(REPORTFOR, 0);
                        else if (msgFor == "LAB")
                            objSqlParameter[0] = new SqlParameter(REPORTFOR, 1);
                        objSqlParameter[0].Direction = ParameterDirection.Input;
                        objSqlParameter[1] = new SqlParameter(GROUP_ID, groupID);
                        objSqlParameter[1].Direction = ParameterDirection.Input;
                        objDS = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, GET_SUPPORT_MONITORING_REPORT, objSqlParameter);

                    }
                    else
                    {
                        string msgSts;

                        if (msgStatus == MessageStatus.Open.GetHashCode())
                            msgSts = "OPEN";
                        else if (msgStatus == MessageStatus.RecentlyClose.GetHashCode())
                            msgSts = "CLOSE";
                        else if (msgStatus == MessageStatus.DirectCommunication.GetHashCode())
                            msgSts = "DOCUMENTED";
                        else if (msgStatus == MessageStatus.Embargo.GetHashCode())
                            msgSts = "EMBARGO";
                        else
                            msgSts = "ALL";

                        SqlParameter[] objSqlParameter = new SqlParameter[5];
                        objSqlParameter[0] = new SqlParameter(WEEK_NUMBER, weekNumber);
                        objSqlParameter[0].Direction = ParameterDirection.Input;
                        objSqlParameter[1] = new SqlParameter(GROUP_ID, groupID);
                        objSqlParameter[1].Direction = ParameterDirection.Input;
                        objSqlParameter[2] = new SqlParameter(FROM_DATE, fromDate);
                        objSqlParameter[2].Direction = ParameterDirection.Input;
                        objSqlParameter[3] = new SqlParameter(INSTITUTION_ID, institutionID);
                        objSqlParameter[3].Direction = ParameterDirection.Input;
                        objSqlParameter[4] = new SqlParameter(MSG_STATUS, msgSts);
                        objSqlParameter[4].Direction = ParameterDirection.Input;

                        if (msgFor == "RAD")
                            objDS = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, GET_RADIOLOGY_MSG, objSqlParameter);
                        else if (msgFor == "LAB")
                            objDS = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, GET_LAB_MSG, objSqlParameter);
                        else if (msgFor == "BOTH")
                            objDS = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, GET_BOTH_MSG, objSqlParameter);

                    }

                    cache.Add(cacheKey, objDS.Tables[0], null, DateTime.Now.AddSeconds(60), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    return objDS.Tables[0];
                }
            }
        }
        #endregion
    }
}
