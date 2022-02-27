#region File History

/******************************File History***************************
 * File Name        : Finding.cs
 * Author           : Prerak Shah
 * Created Date     : 23-11-07
 * Purpose          : To Get information related to Findings.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 *   03-12-2007 - Prerak - Call getOpenConnection function from Utility Class.
 *   05-12-2007 - Prerak - IsActiveFinding is removed from insert method
 *   10-12-2007 - Prerak - Method Changed for Get Groups. Priority AND IsDocumented Field Added
 *   12-12-2007 - IAK -    Method Changed AddFindigs, EditFindigs
 *   19-09-2008 - IAK -    CR 265- Default notification for finding
 *                         Method Changed AddFindigs, EditFindigs
 *   15-10-2008 - Prerak - Live Agent "SendToAgent" property added for power 
 *                         scribe user. 
 *   22-12-2008 - GB     - Added fields Default and Connect Live in methods AddFindigs, 
 *                         UpdateFindigs as per TTP #244 and #231. 
 * ------------------------------------------------------------------- 
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
    public class Finding
    {
        #region Constants Parameters
        private const string FINDING_ID = "@findingID";
        private const string GROUP_ID = "@groupID";
        private const string FINDING_DESC = "@findingDescription";
        private const string FINDING_VOICE_OVER = "@findingVoiceOverURL";
        private const string COMPLIANCE_GOAL = "@complianceGoal";
        private const string ESCALATE_EVERY = "@escalateEvery";
        private const string END_AFTER_MINUTES = "@endAfterMinutes";
        private const string END_AT = "@endAt";
        private const string SEND_OTN_AT = "@sendOTNAt";
        private const string START_BACKUP_AT = "@startBackupAt";
        private const string CONTINUE_TO_SEND_PRIMARY = "@continueToSendPrimary";
        private const string EMBARGO = "@embargo";
        private const string EMBORGO_START_HOUR = "@embargoStartHour";
        private const string EMBARGO_END_HOUR = "@embargoEndHour";
        private const string EMBARGO_SPAN_WEEKEND = "@embargoSpanWeekend";
        private const string FINDING_ORDER = "@findingOrder";
        private const string REQUIRE_READBACK = "@requireReadback";
        private const string IS_DOCUMENTED = "@isDocumented";
        private const string INSTITUTION_ID = "@institutionID";
        private const string MODE = "@mode";
        private const string PRIORITY = "@priority";
        private const string ACTIVE = "@isActive";
        private const string NOTIFICATION_DEVICE_TYPE_ID = "@notificationDeviceTypeID";
        private const string NOTIFICATION_EVENT_TYPE_ID = "@notificationEventTypeID";
        private const string ISDEFAULT = "@isDefault";
        private const string AGENT_ACTION_TYPE_ID = "@agentActionTypeID";
        #endregion

        #region Constants Store procedures
        private const string SP_INSERT_FINDING = "dbo.VOC_CST_insertFindings";
        private const string SP_UPDATE_FINDING = "dbo.VOC_CST_updateFindings";
        private const string SP_GET_FINDING = "dbo.VOC_CST_getFindingbyFindingID";
        private const string SP_GET_GROUPS_BY_INSTITUTION = "dbo.VOC_CST_getGroupsForFinding";
        private const string SP_GET_FINDING_DEVICES = "dbo.VOC_CST_getFindingDevices";
        #endregion

        #region  Public Methods
        /// Add Finding information to the database
        /// </summary>
        /// <param name="FindingInformation"></param>
        /// <returns></returns>
        public void AddFindigs(FindingInformation objFindingInfo)
        {
            SqlConnection conn = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = conn.BeginTransaction();
            try
            {
                SqlParameter[] arParams = new SqlParameter[23];

                //Create parameter for Group ID
                arParams[0] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                arParams[0].Value = objFindingInfo.GroupID;

                //Create parameter for FindingDescription
                arParams[1] = new SqlParameter(FINDING_DESC, SqlDbType.VarChar);
                arParams[1].Value = objFindingInfo.FindingDescription;

                //Create parameter for Finding VoiceOver URL
                arParams[2] = new SqlParameter(FINDING_VOICE_OVER, SqlDbType.VarChar);
                arParams[2].Value = objFindingInfo.FindingVoiceOverURL;

                //Create parameter for ComplianceGoal
                arParams[3] = new SqlParameter(COMPLIANCE_GOAL, SqlDbType.Int);
                arParams[3].Value = objFindingInfo.ComplianceGoal;

                //Create parameter for @EscalateEvery
                arParams[4] = new SqlParameter(ESCALATE_EVERY, SqlDbType.Int);
                arParams[4].Value = objFindingInfo.EscalateEvery;

                //Create parameter for EndAfterMinutes
                arParams[5] = new SqlParameter(END_AFTER_MINUTES, SqlDbType.Int);
                arParams[5].Value = objFindingInfo.EndAfterMinutes;

                //Create parameter for EndAt
                arParams[6] = new SqlParameter(END_AT, SqlDbType.Int);
                arParams[6].Value = objFindingInfo.EndAt;

                //Create parameter for SendOTNAt
                arParams[7] = new SqlParameter(SEND_OTN_AT, SqlDbType.Int);
                arParams[7].Value = objFindingInfo.SendOTNAt;

                //Create parameter for StartBackupAt                                    
                arParams[8] = new SqlParameter(START_BACKUP_AT, SqlDbType.Int);
                arParams[8].Value = objFindingInfo.StartBackupAt;

                //Create parameter for ContinueToSendPrimary
                arParams[9] = new SqlParameter(CONTINUE_TO_SEND_PRIMARY, SqlDbType.Bit);
                arParams[9].Value = objFindingInfo.ContinueToSendPrimary;

                //Create parameter for Embargo
                arParams[10] = new SqlParameter(EMBARGO, SqlDbType.Bit);
                arParams[10].Value = objFindingInfo.Embargo;

                //Create parameter for @EmbargoStartHour
                arParams[11] = new SqlParameter(EMBORGO_START_HOUR, SqlDbType.Int);
                arParams[11].Value = objFindingInfo.EmbargoStartHour;

                //Create parameter for @EmbargoEndHour
                arParams[12] = new SqlParameter(EMBARGO_END_HOUR, SqlDbType.Int);
                arParams[12].Value = objFindingInfo.EmbargoEndHour;

                //Create parameter for EmbargoSpanWeekend
                arParams[13] = new SqlParameter(EMBARGO_SPAN_WEEKEND, SqlDbType.Bit);
                arParams[13].Value = objFindingInfo.EmbargoSpanWeekend;

                //Create parameter for @FindingOrder
                arParams[14] = new SqlParameter(FINDING_ORDER, SqlDbType.Int);
                arParams[14].Value = objFindingInfo.FindingOrder;

                //Create parameter for RequireReadback
                arParams[15] = new SqlParameter(REQUIRE_READBACK, SqlDbType.Bit);
                arParams[15].Value = objFindingInfo.RequireReadback;

                //Create parameter for @priority
                arParams[16] = new SqlParameter(PRIORITY, SqlDbType.Int);
                arParams[16].Value = objFindingInfo.Priority;

                //Create parameter for @IsDocumentd
                arParams[17] = new SqlParameter(IS_DOCUMENTED, SqlDbType.Bit);
                arParams[17].Value = objFindingInfo.DocumentedOnly;

                //Create parameter for @isActive
                arParams[18] = new SqlParameter(ACTIVE, SqlDbType.Bit);
                arParams[18].Value = objFindingInfo.Active;

                //Create parameter for @notificationDeviceTypeID
                arParams[19] = new SqlParameter(NOTIFICATION_DEVICE_TYPE_ID, SqlDbType.Int);
                arParams[19].Value = objFindingInfo.NotificationDeviceTypeID;

                //Create parameter for @notificationEventTypeID
                arParams[20] = new SqlParameter(NOTIFICATION_EVENT_TYPE_ID, SqlDbType.Int);
                arParams[20].Value = objFindingInfo.NotificationEventTypeID;

                //Create parameter for @isDefault
                arParams[21] = new SqlParameter(ISDEFAULT, SqlDbType.Bit);
                arParams[21].Value = objFindingInfo.IsDefault;

                //Create parameter for @agentActionTypeID
                arParams[22] = new SqlParameter(AGENT_ACTION_TYPE_ID, SqlDbType.Int);
                arParams[22].Value = objFindingInfo.AgentActionTypeID;
                SqlHelper.ExecuteScalar(sqlTransaction, CommandType.StoredProcedure, SP_INSERT_FINDING, arParams);
                
                sqlTransaction.Commit();
                conn.Close();
               
            }
            catch (SqlException sqlError)
            {
                sqlTransaction.Rollback();
                conn.Close();
                throw;
            }
        }

        /// Update Finding information to the database
        /// </summary>
        /// <param name="FindingInformation"></param>
        /// <returns></returns>
        public void UpdateFindigs(FindingInformation objFindingInfo)
        {
            SqlConnection conn = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = conn.BeginTransaction();
            try
            {

                SqlParameter[] arParams = new SqlParameter[24];

                //Create parameter for Group ID
                arParams[0] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                arParams[0].Value = objFindingInfo.GroupID;

                //Create parameter for FindingDescription
                arParams[1] = new SqlParameter(FINDING_DESC, SqlDbType.VarChar);
                arParams[1].Value = objFindingInfo.FindingDescription;

                //Create parameter for Finding VoiceOver URL
                arParams[2] = new SqlParameter(FINDING_VOICE_OVER, SqlDbType.VarChar);
                arParams[2].Value = objFindingInfo.FindingVoiceOverURL;

                //Create parameter for ComplianceGoal
                arParams[3] = new SqlParameter(COMPLIANCE_GOAL, SqlDbType.Int);
                arParams[3].Value = objFindingInfo.ComplianceGoal;

                //Create parameter for @EscalateEvery
                arParams[4] = new SqlParameter(ESCALATE_EVERY, SqlDbType.Int);
                arParams[4].Value = objFindingInfo.EscalateEvery;

                //Create parameter for EndAfterMinutes
                arParams[5] = new SqlParameter(END_AFTER_MINUTES, SqlDbType.Int);
                arParams[5].Value = objFindingInfo.EndAfterMinutes;

                //Create parameter for EndAt
                arParams[6] = new SqlParameter(END_AT, SqlDbType.Int);
                arParams[6].Value = objFindingInfo.EndAt;

                //Create parameter for SendOTNAt
                arParams[7] = new SqlParameter(SEND_OTN_AT, SqlDbType.Int);
                arParams[7].Value = objFindingInfo.SendOTNAt;

                //Create parameter for StartBackupAt                                    
                arParams[8] = new SqlParameter(START_BACKUP_AT, SqlDbType.Int);
                arParams[8].Value = objFindingInfo.StartBackupAt;

                //Create parameter for ContinueToSendPrimary
                arParams[9] = new SqlParameter(CONTINUE_TO_SEND_PRIMARY, SqlDbType.Bit);
                arParams[9].Value = objFindingInfo.ContinueToSendPrimary;

                //Create parameter for Embargo
                arParams[10] = new SqlParameter(EMBARGO, SqlDbType.Bit);
                arParams[10].Value = objFindingInfo.Embargo;

                //Create parameter for @EmbargoStartHour
                arParams[11] = new SqlParameter(EMBORGO_START_HOUR, SqlDbType.Int);
                arParams[11].Value = objFindingInfo.EmbargoStartHour;

                //Create parameter for @EmbargoEndHour
                arParams[12] = new SqlParameter(EMBARGO_END_HOUR, SqlDbType.Int);
                arParams[12].Value = objFindingInfo.EmbargoEndHour;

                //Create parameter for EmbargoSpanWeekend
                arParams[13] = new SqlParameter(EMBARGO_SPAN_WEEKEND, SqlDbType.Bit);
                arParams[13].Value = objFindingInfo.EmbargoSpanWeekend;

                //Create parameter for @FindingOrder
                arParams[14] = new SqlParameter(FINDING_ORDER, SqlDbType.Int);
                arParams[14].Value = objFindingInfo.FindingOrder;

                //Create parameter for RequireReadback
                arParams[15] = new SqlParameter(REQUIRE_READBACK, SqlDbType.Bit);
                arParams[15].Value = objFindingInfo.RequireReadback;

                //Create parameter for @FindingID
                arParams[16] = new SqlParameter(FINDING_ID, SqlDbType.Int);
                arParams[16].Value = objFindingInfo.FindingID;
                //Create parameter for @priority
                arParams[17] = new SqlParameter(PRIORITY, SqlDbType.Int);
                arParams[17].Value = objFindingInfo.Priority;

                //Create parameter for @IsDocumentd
                arParams[18] = new SqlParameter(IS_DOCUMENTED, SqlDbType.Bit);
                arParams[18].Value = objFindingInfo.DocumentedOnly;

                //Create parameter for @isActive
                arParams[19] = new SqlParameter(ACTIVE, SqlDbType.Bit);
                arParams[19].Value = objFindingInfo.Active;

                //Create parameter for @notificationDeviceTypeID
                arParams[20] = new SqlParameter(NOTIFICATION_DEVICE_TYPE_ID, SqlDbType.Int);
                arParams[20].Value = objFindingInfo.NotificationDeviceTypeID;

                //Create parameter for @notificationEventTypeID
                arParams[21] = new SqlParameter(NOTIFICATION_EVENT_TYPE_ID, SqlDbType.Int);
                arParams[21].Value = objFindingInfo.NotificationEventTypeID;

                //Create parameter for @isDefault
                arParams[22] = new SqlParameter(ISDEFAULT, SqlDbType.Bit);
                arParams[22].Value = objFindingInfo.IsDefault;

                //Create parameter for @agentActionTypeID
                arParams[23] = new SqlParameter(AGENT_ACTION_TYPE_ID, SqlDbType.Int);
                arParams[23].Value = objFindingInfo.AgentActionTypeID;

                SqlHelper.ExecuteScalar(sqlTransaction, CommandType.StoredProcedure, SP_UPDATE_FINDING, arParams);

                sqlTransaction.Commit();
                conn.Close();
                
            }
            catch (SqlException sqlError)
            {
                sqlTransaction.Rollback();
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// Get Finding Information by Finding ID
        /// </summary>
        /// <param name="FindingID"></param>
        /// <returns></returns>
        public DataTable GetFindingInfoByFindingID(int findingID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtGroupInfo = new DataTable("GroupInfo");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(FINDING_ID, findingID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drGroupInfo = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_FINDING, objSqlParameter);

                dtGroupInfo.Load(drGroupInfo);
                drGroupInfo.Close();
                return dtGroupInfo;
            }
        }
        /// <summary>
        /// This Method returns Data table contaioning Groups with FindingOrder and 
        /// FidingDescription of particular Institution.
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns></returns>
        public DataTable GetGroups(int InstitutionID, int findingId, string mode)
        {
            SqlDataReader reader = null;
            DataTable dtGroups = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[3];
                objSqlParameter[0] = new SqlParameter(INSTITUTION_ID, InstitutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(FINDING_ID, findingId);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(MODE, mode);
                objSqlParameter[2].Direction = ParameterDirection.Input;


                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_GROUPS_BY_INSTITUTION, objSqlParameter);
                dtGroups.Load(reader);
                return dtGroups;
            }
        }

        /// <summary>
        /// Gets Finding device types from database into datatable.
        /// </summary>
        public DataTable GetFindingDevices()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtFindingDevices = new DataTable();
                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_FINDING_DEVICES);
                dtFindingDevices.Load(reader);
                reader.Close();
                return dtFindingDevices;
            }
        }
        #endregion


    }
}
