#region File History

/******************************File History***************************
 * File Name        : AgentCallCenter.cs
 * Author           : Suhas Tarihalkar
 * Created Date     : 28 May 2008
 * Purpose          : This class deals with the database operations realted to the call center module.
 *                  : Add, edit, delete Call Center Information for selected institution
 *                  : Add, edit, delete reasons for selected calll center.

 * *********************File Modification History*********************

 * * Date(dd-mm-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 *  07-11-2008	SSK 	Code review fixes Iteration 25
 *  09-23-2008  SSK     Add institutionid for Agent
 *  11-13-2008  Suhas   Remove Agent functionality
 * ------------------------------------------------------------------- 
 */
#endregion

#region Using Block
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.DataAccess;
using System.Collections;
#endregion

namespace Vocada.CSTools.DataAccess
{
    /// <summary>
    /// Performs database operations related to call center
    /// </summary>
    public class AgentCallCenter
    {
        #region Constants
        /// <summary>
        /// This constant stores name of stored procedure Out parameter 
        /// </summary>
        private const string INSTITUTE_ID = "@institutionID";
        /// <summary>
        /// This constant stores name of stored procedure In parameter 
        /// </summary>
        private const string CALLCENTER_NAME = "@callCenterName";
        /// <summary>
        /// This constant stores name of stored procedure In parameter 
        /// </summary>
        private const string CALLCENTER_ID = "@callCenterID";
        /// <summary>
        /// This constant stores name of stored procedure Out parameter 
        /// </summary>
        private const string RETURN_VAL = "@returnVal";
        /// <summary>
        /// This constant stores reason text
        /// </summary>
        private const string REASON_TEXT = "@reasonTypeText";
        /// <summary>
        /// This constant stores reasontype ID
        /// </summary>
        private const string REASON_TYPE_ID = "@reasonTypeID";
        /// <summary>
        /// This constant stores call center name
        /// </summary>
        private const string CALLCENTERNAME = "@callCenterName";
        /// <summary>
        /// This constant stores active state
        /// </summary>
        private const string ISACTIVE = "@isActive";
        /// <summary>
        /// This constant stores contact name
        /// </summary>
        private const string CONTACTNAME = "@contactName";
        /// <summary>
        /// This constant stores contact phone
        /// </summary>
        private const string CONTACTPHONE = "@contactPhone";
        /// <summary>
        /// This constant stores email
        /// </summary>
        private const string EMAIL = "@email";
        /// <summary>
        /// This constant stores pager number
        /// </summary>
        private const string PAGERNUMBER = "@pagerNumber";
        /// <summary>
        /// This constant stores fax
        /// </summary>
        private const string FAX = "@fax";
        /// <summary>
        /// This constant stores alternate contact name
        /// </summary>
        private const string ALTERNATECONTACTNAME = "@alternateContactName";
        /// <summary>
        /// This constant stores alternate phone
        /// </summary>
        private const string ALTERNATEPHONE = "@alternatePhone";
        /// <summary>
        /// This constant stores escalation period 1
        /// </summary>
        private const string ESCALATIONPERIOD1 = "@escalationPeriod1";
        /// <summary>
        /// This constant stores escalation period 2
        /// </summary>
        private const string ESCALATIONPERIOD2 = "@escalationPeriod2";
        /// <summary>
        /// This constant stores Lock message timeout
        /// </summary>
        private const string LOCKEDMESSAGETIMEOUT = "@lockedMessageTimeout";
        /// <summary>
        /// This constant stores autologout
        /// </summary>
        private const string AUTOLOGOUT = "@autoLogout";
        /// <summary>
        /// This constant stores new message alert
        /// </summary>
        private const string NEWMESSAGEALERT = "@newMessageAlert";
        /// <summary>
        /// This constant stores out of compliance alert
        /// </summary>
        private const string OUTOFCOMPLIANCEALERT = "@outOfComplianceAlert";
        /// <summary>
        /// This constant stores message close alert
        /// </summary>
        private const string MESSAGECLOSEDALERT = "@messageClosedAlert";
        /// <summary>
        /// This constant stores escalation 1 alert
        /// </summary>
        private const string ESCALATION1ALERT = "@escalation1Alert";
        /// <summary>
        /// This constant stores escalation 2 alert
        /// </summary>
        private const string ESCALATION2ALERT = "@escalation2Alert";
        /// <summary>
        /// This constant stores confirmation send popup
        /// </summary>
        private const string CONFIRMATIONSENDPOPUP = "@confirmationSendPopup";
        /// <summary>
        /// This constant stores confirmation doc popup
        /// </summary>
        private const string CONFIRMATIONDOCPOPUP = "@confirmationDocPopup";
        /// <summary>
        /// This constant stores confirmation connect popup
        /// </summary>
        private const string CONFIRMATIONCONNECTPOPUP = "@confirmationConnectPopup";
        /// <summary>
        /// This constant stores group IDs collection
        /// </summary>
        private const string GROUPSCOLLECTION = "@groupIDCollection";
        /// <summary>
        /// This constant stores confirmation Manually Closed popup
        /// </summary>
        private const string CONFIRMATIONMANUALLYCLOSEDPOPUP = "@confirmationManuallyClosedPopup";

        private const string USER_ID = "@vocUserID";
        private const string LOGIN_ID = "@loginID";
        private const string PASSWORD = "@password";
        private const string VOC_USER_ID = "@VOCUserID";
        private const string IS_LOGGED_IN = "@IsLoggedIn";
        private const string ROLE_ID = "@roleId";
        private const string STATUS = "@status";
        private const string FIRST_NAME = "@firstName";
        private const string LAST_NAME = "@lastName";
        private const string PHONE = "@phone";
        private const string DEVICE_FOR = "@deviceFor";
        private const string GROUP_ID = "@groupID";
        private const string DEVICE_ID = "@deviceID";
        private const string DEVICE_ADDRESS = "@deviceAddress";
        private const string DEVICE_NAME = "@deviceName";
        private const string GATEWAY = "@gateway";
        private const string CARRIER = "@carrier";
        private const string AGENR_DEVICE_ID = "@agentDeviceID";
        private const string FINDING_ID = "@findingID";
        private const string AGENT_NOTIFICATION_ID = "@agentNotificationID";
        private const string AGENT_DEVICE_ID = "@agentDeviceID";
        private const string INITIAL_PAUSE = "@initialPause";
        private const string AGENT_NOTIFY_EVENT_ID = "@agentNotifyEventID";
        private const string INSERUPDATEFLAG = "@insertUpdateFlag";
        private const string MOBILE_NUMBER = "@mobile";
        
        #endregion

        #region Stored Procedures
        /// <summary>
        /// This constant stores name of stored procedure which will get the list of institution from Database
        /// </summary>
        private const string SP_GET_CALLCENTER_INSTITUTIONS = "dbo.getInstitutions";
        /// <summary>
        /// Get the names of call centers for selected institute.
        /// </summary>
        private const string SP_GET_CALLCENTERS_NAMES = "dbo.VOC_ACC_getCallCenterNames";
        /// <summary>
        /// This constant stores name of stored procedure which will add new units in Database
        /// </summary>
        private const string SP_INSERT_CALLCENTER_NAME = "dbo.VOC_ACC_insertCallCenterName";
        /// <summary>
        /// This constant stores name of stored procedure which will add new units in Database
        /// </summary>
        private const string SP_UPDATE_CALLCENTER_NAME = "dbo.VOC_ACC_updateCallCenterName";
        /// <summary>
        /// Get the all reason for selected call centers.
        /// </summary>
        private const string SP_GET_CALLCENTERS_REASONS = "dbo.VOC_ACC_getCallCenterReasons";
        /// <summary>
        /// Insert call center reason
        /// </summary>
        private const string SP_INSERT_CALLCENTER_REASON = "dbo.VOC_ACC_insertCallCenterReasons";
        /// <summary>
        /// Update call center reason
        /// </summary>
        private const string SP_UPDATE_CALLCENTER_REASON = "dbo.VOC_ACC_updateCallCenterReason";
        /// <summary>
        /// Get all the assigned groups to call center and all those groups not assigned to any call center
        /// </summary>
        private const string SP_GET_CALLCENTERS_GROUPS = "dbo.VOC_ACC_getCallCenterGroups";
        /// <summary>
        /// Get only assigned groups to selected call center
        /// </summary>
        private const string SP_GET_CALLCENTERS_ASSIGNED_GROUPS = "dbo.VOC_ACC_getCallCenterAssignedGroups";
        /// <summary>
        /// Update call center information
        /// </summary>
        private const string SP_UPDATE_CALL_CENTER_INFO = "dbo.VOC_ACC_updateCallCenterInformation";
        /// <summary>
        /// Get the necessary information and preferences for selected call center.
        /// </summary>
        private const string SP_GET_CALLCENTERS_INFO = "dbo.VOC_ACC_getCallCenterInformation";
        /// <summary>
        /// Delete reasons.
        /// </summary>
        private const string SP_DELETE_CALLCENTERS_REASON = "dbo.VOC_ACC_deleteReasons";
        /// <summary>
        /// Get all information of call center agents.
        /// </summary>
        private const string SP_GET_AGENT_INFORMATION = "dbo.VOC_ACC_getAgentInformation";
        /// <summary>
        /// Get Call Center Agent roles.
        /// </summary>
        private const string SP_GET_CALLCENTER_AGENT_ROLES = "dbo.VOC_ACC_getAgentRoles";
        /// <summary>
        /// SP for updating User Information
        /// </summary>
        private const string SP_UPDATE_USER_INFO = "dbo.VOC_ACC_updateUsersInformation";
        /// <summary>
        /// SP for inserting User Information
        /// </summary>
        private const string SP_INSERT_USER_INFO = "dbo.VOC_ACC_insertUsersInformation";
        /// <summary>
        /// SP for getting unique Agent Password
        /// </summary>
        private const string SP_GET_UNIQUE_PASSWORD = "dbo.VOC_ACC_getUniqueAgentPassword";
        /// <summary>
        /// Check for duplicate login ID and Password
        /// </summary>
        private const string SP_CHECK_DUPLICATE_PIN = "dbo.VOC_ACC_checkDuplicatePin";
        /// <summary>
        /// Get all assigned groups to agent.
        /// </summary>
        private const string SP_GET_CALLCENTERS_AGENT_ASSIGNED_GROUPS = "dbo.VOC_ACC_getCCAgentAssignedGroups";
        /// <summary>
        /// SP for getDevices
        /// </summary>
        private const string SP_GET_DEVICES = "dbo.getDevices";
        /// <summary>
        /// SP for getting Agent notifications
        /// </summary>
        private const string SP_GET_AGENT_EVENTS = "dbo.VOC_ACC_getAgentNotifyEvents";
        /// <summary>
        /// Get findings for Groups
        /// </summary>
        private const string SP_GET_FINDINGS_USING_GROUP = "dbo.VOC_ACC_getFindingsUsingGroup";
        /// <summary>
        /// Get Live Agent devices
        /// </summary>
        private const string SP_GET_LIVEAGENT_DEVICE_EVENTS = "dbo.VOC_ACC_getLiveAgentDevices";
        /// <summary>
        /// Insert/Update/Delete Live Agent Notification Device
        /// </summary>
        private const string SP_INSERT_UPDATE_AGENT_DEVICE = "dbo.VOC_ACC_insertUpdateLiveAgentDevices";
        /// <summary>
        /// Remove selected call center user.
        /// </summary>
        private const string SP_REMOVE_CALLCENTER_AGENT = "dbo.VOC_ACC_removeCallCenterAgent";
        /// <summary>
        /// Checks if user logged in to any system.
        /// </summary>
        private const string SP_CHECK_USER_LOGIN = "dbo.VOC_VWACC_CheckUserLogin";
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets Institutution List.
        /// </summary>
        /// <returns></returns>
        public DataTable GetInstitutionList()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtInstitutions = new DataTable();
                SqlDataReader drInstitutions = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_CALLCENTER_INSTITUTIONS);
                dtInstitutions.Load(drInstitutions);
                return dtInstitutions;
            }
        }

        /// <summary>
        /// Gets the call centers.
        /// </summary>
        /// <param name="institutionID">The institution ID.</param>
        /// <returns></returns>
        public DataTable GetCallCenters(int institutionID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtCallCenterData = new DataTable("CallCenters");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(INSTITUTE_ID, institutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drCallCenters = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_CALLCENTERS_NAMES, objSqlParameter);

                dtCallCenterData.Load(drCallCenters);
                drCallCenters.Close();
                return dtCallCenterData;
            }
        }

        /// <summary>
        /// Adds the new name of the call center.
        /// </summary>
        /// <param name="institutionID">The institution ID.</param>
        /// <param name="callCenterName">Name of the call center.</param>
        /// <returns></returns>
        public int AddNewCallCenterName(int institutionID, string callCenterName)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[3];

                objSqlParameter[0] = new SqlParameter(INSTITUTE_ID, SqlDbType.Int);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[0].Value = institutionID;

                objSqlParameter[1] = new SqlParameter(CALLCENTER_NAME, SqlDbType.VarChar);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[1].Value = callCenterName;

                objSqlParameter[2] = new SqlParameter(RETURN_VAL, SqlDbType.Int);
                objSqlParameter[2].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_INSERT_CALLCENTER_NAME, objSqlParameter);
                return int.Parse(objSqlParameter[2].Value.ToString());
            }
        }

        /// <summary>
        /// Updates the name of the call center.
        /// </summary>
        /// <param name="institutionID">The institution ID.</param>
        /// <param name="callCenterID">The call center ID.</param>
        /// <param name="callCenterName">Name of the call center.</param>
        /// <returns></returns>
        public int UpdateCallCenterName(int institutionID, int callCenterID, string callCenterName)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[4];

                objSqlParameter[0] = new SqlParameter(INSTITUTE_ID, SqlDbType.Int);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[0].Value = institutionID;

                objSqlParameter[1] = new SqlParameter(CALLCENTER_ID, SqlDbType.Int);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[1].Value = callCenterID;

                objSqlParameter[2] = new SqlParameter(CALLCENTER_NAME, SqlDbType.VarChar);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[2].Value = callCenterName;

                objSqlParameter[3] = new SqlParameter(RETURN_VAL, SqlDbType.Int);
                objSqlParameter[3].Direction = ParameterDirection.Output;


                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_UPDATE_CALLCENTER_NAME, objSqlParameter);
                return int.Parse(objSqlParameter[3].Value.ToString());
            }
        }

        /// <summary>
        /// Gets the call center reasons.
        /// </summary>
        /// <param name="callCenterID">The call center ID.</param>
        /// <returns></returns>
        public DataTable GetCallCenterReasons(int callCenterID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtCallCenterReasonData = new DataTable("CallCenterReasons");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(CALLCENTER_ID, callCenterID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drCallCentersReason = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_CALLCENTERS_REASONS, objSqlParameter);

                dtCallCenterReasonData.Load(drCallCentersReason);
                drCallCentersReason.Close();
                return dtCallCenterReasonData;
            }
        }

        /// <summary>
        /// Inserts the call center reason.
        /// </summary>
        /// <param name="callCenterID">The call center ID.</param>
        /// <param name="reasonText">The reason text.</param>
        /// <returns></returns>
        public int InsertCallCenterReason(int callCenterID, string reasonText)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[3];

                objSqlParameter[0] = new SqlParameter(CALLCENTER_ID, SqlDbType.Int);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[0].Value = callCenterID;

                objSqlParameter[1] = new SqlParameter(REASON_TEXT, SqlDbType.VarChar);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[1].Value = reasonText;

                objSqlParameter[2] = new SqlParameter(RETURN_VAL, SqlDbType.Int);
                objSqlParameter[2].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_INSERT_CALLCENTER_REASON, objSqlParameter);
                return int.Parse(objSqlParameter[2].Value.ToString());
            }
        }

        /// <summary>
        /// Updates the call center reason.
        /// </summary>
        /// <param name="reasonTypeID">The reason type ID.</param>
        /// <param name="callCenterID">The call center ID.</param>
        /// <param name="callCenterReason">The call center reason.</param>
        /// <returns></returns>
        public int UpdateCallCenterReason(int reasonTypeID, int callCenterID, string callCenterReason)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[4];

                objSqlParameter[0] = new SqlParameter(REASON_TYPE_ID, SqlDbType.Int);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[0].Value = reasonTypeID;

                objSqlParameter[1] = new SqlParameter(CALLCENTER_ID, SqlDbType.Int);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[1].Value = callCenterID;

                objSqlParameter[2] = new SqlParameter(REASON_TEXT, SqlDbType.VarChar);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[2].Value = callCenterReason;

                objSqlParameter[3] = new SqlParameter(RETURN_VAL, SqlDbType.Int);
                objSqlParameter[3].Direction = ParameterDirection.Output;


                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_UPDATE_CALLCENTER_REASON, objSqlParameter);
                return int.Parse(objSqlParameter[3].Value.ToString());
            }
        }

        /// <summary>
        /// Gets all groups.
        /// </summary>
        /// <param name="callCenterID">The call center ID.</param>
        /// <returns></returns>
        public DataTable GetAllGroups(int callCenterID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtCallCenterGroups = new DataTable("CallCenterGroups");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(CALLCENTER_ID, callCenterID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drCallCentersGroups = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_CALLCENTERS_GROUPS, objSqlParameter);

                dtCallCenterGroups.Load(drCallCentersGroups);
                drCallCentersGroups.Close();
                return dtCallCenterGroups;
            }
        }

        /// <summary>
        /// Gets all assigned groups.
        /// </summary>
        /// <param name="callCenterID">The call center ID.</param>
        /// <returns></returns>
        public DataTable GetAllAssignedGroups(int callCenterID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtCallCenterGroups = new DataTable("CallCenterAssignedGroups");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(CALLCENTER_ID, callCenterID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drCallCentersGroups = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_CALLCENTERS_ASSIGNED_GROUPS, objSqlParameter);

                dtCallCenterGroups.Load(drCallCentersGroups);
                drCallCentersGroups.Close();
                return dtCallCenterGroups;
            }
        }

        /// <summary>
        /// Updates the call center information.
        /// </summary>
        /// <param name="objCallCenterInformation">The obj call center information.</param>
        /// <param name="groupIDCollection">The group ID collection.</param>
        /// <returns></returns>
        public int UpdateCallCenterInformation(CallCenterInformation objCallCenterInformation, string groupIDCollection)
        {
            SqlConnection conn = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = conn.BeginTransaction();
            try
            {
                SqlParameter[] arParams = new SqlParameter[25];

                arParams[0] = new SqlParameter(CALLCENTER_ID, SqlDbType.Int, 50);
                arParams[0].Value = objCallCenterInformation.CallCenterID;

                arParams[1] = new SqlParameter(CALLCENTER_NAME, SqlDbType.VarChar, 50);
                arParams[1].Value = objCallCenterInformation.CallCenterName;

                arParams[2] = new SqlParameter(INSTITUTE_ID, SqlDbType.Int, 50);
                arParams[2].Value = objCallCenterInformation.InstitutionID;

                arParams[3] = new SqlParameter(ISACTIVE, SqlDbType.Bit, 1);
                arParams[3].Value = objCallCenterInformation.IsActive;

                arParams[4] = new SqlParameter(CONTACTNAME, SqlDbType.VarChar, 50);
                arParams[4].Value = objCallCenterInformation.ContactName;

                arParams[5] = new SqlParameter(CONTACTPHONE, SqlDbType.VarChar, 10);
                arParams[5].Value = objCallCenterInformation.ContactPhone;

                arParams[6] = new SqlParameter(EMAIL, SqlDbType.VarChar, 50);
                arParams[6].Value = objCallCenterInformation.Email;

                arParams[7] = new SqlParameter(PAGERNUMBER, SqlDbType.VarChar, 10);
                arParams[7].Value = objCallCenterInformation.PagerNumber;

                arParams[8] = new SqlParameter(FAX, SqlDbType.VarChar, 10);
                arParams[8].Value = objCallCenterInformation.Fax;

                arParams[9] = new SqlParameter(ALTERNATECONTACTNAME, SqlDbType.VarChar, 50);
                arParams[9].Value = objCallCenterInformation.AlternateContactName;

                arParams[10] = new SqlParameter(ALTERNATEPHONE, SqlDbType.VarChar, 10);
                arParams[10].Value = objCallCenterInformation.AlternatePhone;

                CallCenterPreferences objCallCenterPreferences = objCallCenterInformation.objCallCenterPreferences;

                arParams[11] = new SqlParameter(ESCALATIONPERIOD1, SqlDbType.Int, 2);
                arParams[11].Value = 0;

                arParams[12] = new SqlParameter(ESCALATIONPERIOD2, SqlDbType.Int, 2);
                arParams[12].Value = 0;

                arParams[13] = new SqlParameter(LOCKEDMESSAGETIMEOUT, SqlDbType.Int, 2);
                if (objCallCenterPreferences.LockedMessageTimeout == null)
                {
                    arParams[13].Value = null;
                }
                else
                {
                    arParams[13].Value = Convert.ToInt32(objCallCenterPreferences.LockedMessageTimeout);
                }

                arParams[14] = new SqlParameter(AUTOLOGOUT, SqlDbType.Int, 2);
                if (objCallCenterPreferences.AutoLogout == null)
                {
                    arParams[14].Value = null;
                }
                else
                {
                    arParams[14].Value = Convert.ToInt32(objCallCenterPreferences.AutoLogout);
                }

                arParams[15] = new SqlParameter(NEWMESSAGEALERT, SqlDbType.Bit, 4);
                arParams[15].Value =0;

                arParams[16] = new SqlParameter(OUTOFCOMPLIANCEALERT, SqlDbType.Bit, 4);
                arParams[16].Value = 0;

                arParams[17] = new SqlParameter(MESSAGECLOSEDALERT, SqlDbType.Bit, 4);
                arParams[17].Value = objCallCenterPreferences.MessageClosedAlert;

                arParams[18] = new SqlParameter(ESCALATION1ALERT, SqlDbType.Bit, 4);
                arParams[18].Value = 0;

                arParams[19] = new SqlParameter(ESCALATION2ALERT, SqlDbType.Bit, 4);
                arParams[19].Value = 0;

                arParams[20] = new SqlParameter(CONFIRMATIONSENDPOPUP, SqlDbType.Bit, 4);
                arParams[20].Value = objCallCenterPreferences.ConfirmationSendPopup;

                arParams[21] = new SqlParameter(CONFIRMATIONDOCPOPUP, SqlDbType.Bit, 4);
                arParams[21].Value = objCallCenterPreferences.ConfirmationDocPopup;

                arParams[22] = new SqlParameter(CONFIRMATIONCONNECTPOPUP, SqlDbType.Bit, 4);
                arParams[22].Value = objCallCenterPreferences.ConfirmationConnectPopup;

                arParams[23] = new SqlParameter(GROUPSCOLLECTION, SqlDbType.VarChar, 8000);
                arParams[23].Value = groupIDCollection;

                arParams[24] = new SqlParameter(CONFIRMATIONMANUALLYCLOSEDPOPUP, SqlDbType.Bit);
                arParams[24].Value = objCallCenterPreferences.ConfirmationManuallyClosedPopup;

                SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, SP_UPDATE_CALL_CENTER_INFO, arParams);

                sqlTransaction.Commit();
                conn.Close();
                return objCallCenterInformation.InstitutionID;
            }
            catch (SqlException sqlError)
            {
                sqlTransaction.Rollback();
                conn.Close();
                throw sqlError;
            }
        }

        /// <summary>
        /// Gets the call center information.
        /// </summary>
        /// <param name="callCenterID">The call center ID.</param>
        /// <returns></returns>
        public DataTable GetCallCenterInformation(int callCenterID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtCallCenterInfo = new DataTable("CallCenterInformation");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(CALLCENTER_ID, callCenterID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drCallCentersInfo = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_CALLCENTERS_INFO, objSqlParameter);

                dtCallCenterInfo.Load(drCallCentersInfo);
                drCallCentersInfo.Close();
                return dtCallCenterInfo;
            }
        }

        /// <summary>
        /// Deletes the reason.
        /// </summary>
        /// <param name="reasonTypeID">The reason type ID.</param>
        public void DeleteReason(int reasonTypeID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(REASON_TYPE_ID, reasonTypeID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_DELETE_CALLCENTERS_REASON, objSqlParameter);
            }
        }

        /// <summary>
        /// Deletes the reason.
        /// </summary>
        /// <param name="reasonTypeID">The reason type ID.</param>
        public bool RemoveAgent(int vocUserID)
        {
            bool isDeleted = false;
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(VOC_USER_ID, vocUserID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                objSqlParameter[1] = new SqlParameter(IS_LOGGED_IN, SqlDbType.Int);
                objSqlParameter[1].Direction = ParameterDirection.Output;

                int deleted = SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_REMOVE_CALLCENTER_AGENT, objSqlParameter);
                if (deleted > 0)
                {
                    isDeleted = true;
                }               
            }
            return isDeleted;
        }

        /// <summary>
        /// Gets the call center agent information.
        /// </summary>
        /// <param name="callCenterID">CallCenter ID</param>
        /// <returns></returns>
        public DataTable GetAgentInformation(int callCenterID)
        {
            SqlDataReader reader = null;
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtUserInfo = new DataTable("AgentInfo");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(CALLCENTER_ID, callCenterID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_AGENT_INFORMATION, objSqlParameter);

                dtUserInfo.Load(reader);
                reader.Close();
                return dtUserInfo;
            }
        }

        /// <summary>
        /// Gets Roles 
        /// </summary>
        /// <returns>DataTable containing Role id and description</returns>
        public DataTable GetAgentRoles()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtRoles = new DataTable();
                SqlDataReader drRoles = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_CALLCENTER_AGENT_ROLES);
                dtRoles.Load(drRoles);
                drRoles.Close();
                return dtRoles;
            }
        }

        /// <summary>
        /// Inserts the user information.
        /// </summary>
        /// <param name="objUserInfo">The obj user info.</param>
        public void InsertUserInformation(AgentInformation objUserInfo, string groupIDCollection)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[12];

                sqlParams[0] = new SqlParameter(ROLE_ID, SqlDbType.Int);
                sqlParams[0].Value = objUserInfo.RoleID;

                sqlParams[1] = new SqlParameter(STATUS, SqlDbType.Bit);
                if (objUserInfo.Status)
                    sqlParams[1].Value = 1;
                else
                    sqlParams[1].Value = 0;

                sqlParams[2] = new SqlParameter(LOGIN_ID, SqlDbType.VarChar);
                sqlParams[2].Value = objUserInfo.LoginID;

                sqlParams[3] = new SqlParameter(PASSWORD, SqlDbType.VarChar);
                sqlParams[3].Value = objUserInfo.Password;

                sqlParams[4] = new SqlParameter(FIRST_NAME, SqlDbType.VarChar);
                sqlParams[4].Value = objUserInfo.FirstName;

                sqlParams[5] = new SqlParameter(LAST_NAME, SqlDbType.VarChar);
                sqlParams[5].Value = objUserInfo.LastName;

                sqlParams[6] = new SqlParameter(EMAIL, SqlDbType.VarChar);
                sqlParams[6].Value = objUserInfo.Email;

                sqlParams[7] = new SqlParameter(PHONE, SqlDbType.VarChar);
                sqlParams[7].Value = objUserInfo.Phone;

                sqlParams[8] = new SqlParameter(MOBILE_NUMBER, SqlDbType.VarChar);
                sqlParams[8].Value = objUserInfo.MobileNumber;

                sqlParams[9] = new SqlParameter(CALLCENTER_ID, SqlDbType.VarChar);
                sqlParams[9].Value = objUserInfo.CallCenterID;

                sqlParams[10] = new SqlParameter(GROUPSCOLLECTION, SqlDbType.VarChar, 8000);
                sqlParams[10].Value = groupIDCollection;

                sqlParams[11] = new SqlParameter(INSTITUTE_ID, SqlDbType.Int);
                sqlParams[11].Value = objUserInfo.InstitutionID;
                SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_INSERT_USER_INFO, sqlParams);
            }
        }
        
        /// <summary>
        /// Updates the user information.
        /// </summary>
        /// <param name="objUserInfo">object containing AgentInformation</param>
        /// <param name="groupIDCollection">groupIDCollection string</param>
        public void UpdateUserInformation(AgentInformation objUserInfo, string groupIDCollection)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[12];

                sqlParams[0] = new SqlParameter(VOC_USER_ID, SqlDbType.Int);
                sqlParams[0].Value = objUserInfo.VOCUserID;

                sqlParams[1] = new SqlParameter(ROLE_ID, SqlDbType.Int);
                sqlParams[1].Value = objUserInfo.RoleID;

                sqlParams[2] = new SqlParameter(STATUS, SqlDbType.Bit);
                if (objUserInfo.Status)
                    sqlParams[2].Value = 1;
                else
                    sqlParams[2].Value = 0;

                sqlParams[3] = new SqlParameter(LOGIN_ID, SqlDbType.VarChar);
                sqlParams[3].Value = objUserInfo.LoginID;

                sqlParams[4] = new SqlParameter(PASSWORD, SqlDbType.VarChar);
                sqlParams[4].Value = objUserInfo.Password;

                sqlParams[5] = new SqlParameter(FIRST_NAME, SqlDbType.VarChar);
                sqlParams[5].Value = objUserInfo.FirstName;

                sqlParams[6] = new SqlParameter(LAST_NAME, SqlDbType.VarChar);
                sqlParams[6].Value = objUserInfo.LastName;

                sqlParams[7] = new SqlParameter(EMAIL, SqlDbType.VarChar);
                sqlParams[7].Value = objUserInfo.Email;

                sqlParams[8] = new SqlParameter(PHONE, SqlDbType.VarChar);
                sqlParams[8].Value = objUserInfo.Phone;

                sqlParams[9] = new SqlParameter(MOBILE_NUMBER, SqlDbType.VarChar);
                sqlParams[9].Value = objUserInfo.MobileNumber;

                sqlParams[10] = new SqlParameter(GROUPSCOLLECTION, SqlDbType.VarChar, 8000);
                sqlParams[10].Value = groupIDCollection;

                sqlParams[11] = new SqlParameter(INSTITUTE_ID, SqlDbType.Int);
                sqlParams[11].Value = objUserInfo.InstitutionID;              
                SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_UPDATE_USER_INFO, sqlParams);
            }
        }

        /// <summary>
        /// Get new Pin number from database for CSTools User
        /// </summary>
        /// <returns></returns>
        public string GetNewPin()
        {
            string pinNumber = "";
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlDataReader newPin = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_UNIQUE_PASSWORD, null);
                if (newPin.Read())
                {
                    pinNumber = newPin["Password"].ToString();
                }
            }
            return pinNumber;
        }

        /// <summary>
        /// Checks for duplicate PIN for Agents
        /// </summary>
        /// <param name="vocUserID">VOCUser ID</param>
        /// <param name="loginID">Login ID</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public bool CheckDuplicatePIN(int vocUserID, string loginID, string password)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] arrParams = new SqlParameter[3];

                arrParams[0] = new SqlParameter(USER_ID, SqlDbType.Int);
                arrParams[0].Value = vocUserID;

                arrParams[1] = new SqlParameter(LOGIN_ID, SqlDbType.VarChar, 15);
                arrParams[1].Value = loginID;

                arrParams[2] = new SqlParameter(PASSWORD, SqlDbType.VarChar, 15);
                arrParams[2].Value = password;

                SqlDataReader duplicateID = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_CHECK_DUPLICATE_PIN, arrParams);
                if (duplicateID.Read())
                {
                    if (Convert.ToInt32(duplicateID["RowCnt"]) > 0)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets all assigned groups to Agent.
        /// </summary>
        /// <param name="agentID">The Agent ID.</param>
        /// <returns></returns>
        public DataTable GetAllAgentAssignedGroups(int agentID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtCallCenterGroups = new DataTable("CallCenterAssignedGroups");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(USER_ID, agentID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drCallCentersGroups = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_CALLCENTERS_AGENT_ASSIGNED_GROUPS, objSqlParameter);

                dtCallCenterGroups.Load(drCallCentersGroups);
                drCallCentersGroups.Close();
                return dtCallCenterGroups;
            }
        }

        /// <summary>
        /// Deletes the reason.
        /// </summary>
        /// <param name="reasonTypeID">The reason type ID.</param>
        public bool CheckUserLogin(int vocUserID)
        {
            bool isLogin = false;
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(VOC_USER_ID, vocUserID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                objSqlParameter[1] = new SqlParameter(IS_LOGGED_IN, SqlDbType.Int);
                objSqlParameter[1].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_CHECK_USER_LOGIN, objSqlParameter);
                if (Convert.ToInt16(objSqlParameter[1].Value.ToString()) > 0)
                {
                    isLogin = true;
                }
            }
            return isLogin;
        }

        #region Live Agent Escalations

        /// <summary>
        /// Gets the agent devices.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAgentDevices()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtOCDevice = new DataTable();
                SqlParameter[] arParams = new SqlParameter[1];
                arParams[0] = new SqlParameter(DEVICE_FOR, "LIVEAGENT");
                arParams[0].Direction = ParameterDirection.Input;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_DEVICES, arParams);
                dtOCDevice.Load(reader);
                reader.Close();
                return dtOCDevice;
            }
        }

        /// <summary>
        /// Gets the agent notify events.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAgentNotifyEvents()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtAgentNotifyEvents = new DataTable();
                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_AGENT_EVENTS);
                dtAgentNotifyEvents.Load(reader);
                reader.Close();
                return dtAgentNotifyEvents;
            }
        }

        /// <summary>
        /// Gets the finding.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <returns></returns>
        public DataTable GetFinding(int groupID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtOCFindings = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[1];

                sqlParams[0] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                sqlParams[0].Value = groupID;

                SqlDataReader drFindings = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_FINDINGS_USING_GROUP, sqlParams);
                dtOCFindings.Load(drFindings);
                drFindings.Close();
                return dtOCFindings;
            }
        }


        /// <summary>
        /// Gets the live agent device.
        /// </summary>
        /// <param name="callCenterID">The call center ID.</param>
        /// <returns></returns>
        public DataTable GetLiveAgentDevice(int callCenterID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtLiveAgentDevice = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[1];

                sqlParams[0] = new SqlParameter(CALLCENTER_ID, SqlDbType.Int);
                sqlParams[0].Value = callCenterID;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_LIVEAGENT_DEVICE_EVENTS, sqlParams);
                dtLiveAgentDevice.Load(reader);
                reader.Close();
                return dtLiveAgentDevice;
            }
        }


        /// <summary>
        /// Inserts the update agent device.
        /// </summary>
        /// <param name="objDevice">The obj device.</param>
        /// <param name="insertUpdateFlag">The insert update flag.</param>
        /// <returns></returns>
        public int InsertUpdateAgentDevice(LiveAgentDevicesInfo objDevice, int insertUpdateFlag)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[13];

                sqlParams[0] = new SqlParameter(CALLCENTER_ID, SqlDbType.Int);
                sqlParams[0].Value = objDevice.CallCenterID;

                sqlParams[1] = new SqlParameter(DEVICE_ID, SqlDbType.Int);
                sqlParams[1].Value = objDevice.DeviceID;

                sqlParams[2] = new SqlParameter(DEVICE_ADDRESS, SqlDbType.VarChar, 100);
                sqlParams[2].Value = objDevice.DeviceAddress;

                sqlParams[3] = new SqlParameter(GATEWAY, SqlDbType.VarChar, 100);
                sqlParams[3].Value = objDevice.Gateway;

                sqlParams[4] = new SqlParameter(CARRIER, SqlDbType.VarChar, 100);
                sqlParams[4].Value = objDevice.Carrier;

                sqlParams[5] = new SqlParameter(INITIAL_PAUSE, SqlDbType.Decimal);
                if (objDevice.InitialPauseTime == "-1" || objDevice.InitialPauseTime == "")
                    sqlParams[5].Value = DBNull.Value;
                else
                    sqlParams[5].Value = objDevice.InitialPauseTime;

                sqlParams[6] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                sqlParams[6].Value = objDevice.GroupID;

                sqlParams[7] = new SqlParameter(AGENT_NOTIFY_EVENT_ID, SqlDbType.Int);
                sqlParams[7].Value = objDevice.AgentNotifyEventID;

                sqlParams[8] = new SqlParameter(FINDING_ID, SqlDbType.Int);
                sqlParams[8].Value = objDevice.FindingID;

                sqlParams[9] = new SqlParameter(DEVICE_NAME, SqlDbType.VarChar, 100);
                sqlParams[9].Value = objDevice.DeviceName;

                sqlParams[10] = new SqlParameter(AGENT_NOTIFICATION_ID, SqlDbType.Int);
                sqlParams[10].Value = objDevice.AgentNotificationID;

                sqlParams[11] = new SqlParameter(AGENT_DEVICE_ID, SqlDbType.Int);
                sqlParams[11].Value = objDevice.AgentDeviceID;

                sqlParams[12] = new SqlParameter(INSERUPDATEFLAG, SqlDbType.Int);
                sqlParams[12].Value = insertUpdateFlag;

                object result = SqlHelper.ExecuteScalar(cnx, CommandType.StoredProcedure, SP_INSERT_UPDATE_AGENT_DEVICE, sqlParams);
                int deviceID = 0;
                if (result != null)
                    deviceID = Convert.ToInt32(result);

                return deviceID;
            }
        }

        
        #endregion
        #endregion
    }
}
