#region File History

/******************************File History***************************
 * File Name        : Vocada.CSTools.DataAccess/OrderingClinician.cs
 * Author           : Prerak Shah
 * Created Date     : 25-Jul-07
 * Purpose          : To take care all Database transactions for the tab Add/editOC.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 
 * 06-Sep-2007 Prerak Shah - For OC Voiceover Utility page. 
 * 13-Sep-2007 Prerak Shah - For Edit OC page. 
 * 03-Dec-2007 Prerak - Call getOpenConnection function from Utility Class.
 * 17-Jan-2008 Prerak - Added paremeter of InitialPauseTime
 * 03-20-2008  SSK     "PIN for Message Retrieval" changes
 * 11-Apr-2008 Prerak - IsLabGroup(), HasSmsWebLink() Methods added for SMS Web Link implementation 
 * 17-Apr-2008 Prerak - Removed HasSmsWebLink() and  Change IsLabGroup() as there is no more 
 *                      sms web link preferences and we have to display smsweblink all time.
 * 24-06-2008   NDM     CR#249
 * 04-08-2008   Prerak  CR#200 -> OC Profile Layout, Merging AddEdit.cs class into OrderingClinician.cs class.
 * 06-08-2008   Prerak  CR - Cell Phone in OC Profile implemented
 * 08-19-2008   Suhas   FR#271 SMS with Weblink device Group population 
 * 05-01-2009  Raju G   Modified for FR 282
 * 15-01-2009   GB      FR #152 - Clinical team assignment
 * 12-18-2008   ZNK     CR-Multiple External ID support.
 ******************************File History***************************/

#endregion

#region Using
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Vocada.VoiceLink.DataAccess;
using System.Data;
using System.Data.SqlClient;
using Vocada.CSTools.Common;
#endregion

namespace Vocada.CSTools.DataAccess
{
    /// <summary>
    /// Class to take care business logic for Add and Edit OC pages
    /// </summary>
    public class OrderingClinician
    {
        #region Stored Procedures
        /// <summary>
        /// SP for insertReferringPhysician
        /// </summary>
        private const string SP_INSERT_REFERRING_PHYSICIAN = "dbo.VOC_CST_insertReferringPhysician";
        private const string SP_GET_OC_FOR_VOICEOVER = "dbo.VOC_CST_getOcforVoiceOver";
        private const string SP_DELETE_VOICEOVER = "dbo.VOC_CST_deleteOCVoiceOver";
        private const string SP_UPDATE_VOICEOVER = "dbo.VOC_CST_approveOCVoiceOver";
        private const string SP_UPDATE_GRAMMAR = "dbo.VOC_CST_UpdateGrammar";
        private const string SP_GET_GRAMMAR_FOR_DIRECTORY = "dbo.VOC_CST_getGrammerInfoForDirectory";
        private const string SP_GET_UNIQUE_OC_PASSWORD = "dbo.VOC_CST_getUniquePasswordForOC";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive all departments from Database
        /// </summary>
        private const string SP_GET_DEPTS = "dbo.VOC_CST_GetDepartments";
        /// <summary>
        /// SP for getReferringPhysicianByID
        /// </summary>
        private const string SP_GET_REFERRING_PHYSICIANS_BY_ID = "dbo.VOC_CST_getReferringPhysicianByID";
        /// SP for updateReferringPhysician
        /// </summary>
        private const string SP_UPDATE_REFERRING_PHYSICIAN = "dbo.VOC_CST_updateReferringPhysician";

        /// <summary>
        /// SP for insertRPDevice
        /// </summary>
        private const string SP_INSERT_OC_DEVICE = "dbo.VOC_CST_insertOCDeviceAndNotifications";

        /// <summary>
        /// SP for getRPDevices
        /// </summary>
        private const string SP_GET_OC_DEVICE_EVENTS = "dbo.VOC_CST_getOCDevicesAndEvents";

        /// <summary>
        /// SP for getRPNotifications
        /// </summary>
        private const string SP_GET_OC_NOTIFICATIONS = "dbo.VOC_VW_getRPNotifications";

        /// <summary>
        /// SP for getDevices
        /// </summary>
        private const string SP_GET_DEVICES = "dbo.getDevices";

        /// <summary>
        /// SP for delete OC Device and Event Notification
        /// </summary>
        private const string SP_DELETE_OC_DEVICE_EVENT = "dbo.VOC_CST_deleteOCDeviceAndEvent";

        /// <summary>
        /// SP for updateRPDevice
        /// </summary>
        private const string SP_UPDATE_OC_DEVICE_EVENT = "dbo.VOC_CST_updateOCDeviceAndEvent";

        /// <summary>
        /// SP for getRPNotifyEvents
        /// </summary>
        private const string SP_GET_OC_EVENTS = "dbo.getRPNotifyEvents";

        /// <summary>
        /// SP for getFindingOptionsForSubscriber
        /// </summary>
        private const string SP_GET_FINDINGS_FOR_SUBSCRIBER = "dbo.getFindingOptionsForSubscriber";

        /// <summary>
        /// Get list of findings for OC ot group
        /// </summary>
        private const string SP_GET_FINDINGS_USING_OC_GROUP = "dbo.VOC_CST_getFindingsUsingOCGroup";

        /// <summary>
        /// SP for getRPAfterHoursDevices
        /// </summary>
        private const string SP_GET_AFTER_HOURS_NOTIFICATIONS = "dbo.VOC_CST_getOCAfterHoursNotifications";
        
        /// <summary>
        /// SP for deleteRPAfterHoursNotification
        /// </summary>
        private const string SP_DELETE_AFTER_HOURS_NOTIFICATIONS = "dbo.deleteRPAfterHoursNotification";

        /// <summary>
        /// SP for insertRPNotification
        /// </summary>
        private const string SP_INSERT_OC_NOTIFICATIONS = "dbo.VOC_VW_insertRPNotification";

        /// <summary>
        /// SP for insertAfterHoursRPNotification
        /// </summary>
        private const string SP_INSERT_AFTER_HOUR_NOTIFICATIONS = "dbo.VOC_VW_insertAfterHoursRPNotification";

        /// <summary>
        /// SP for getRPAfterHoursDevices
        /// </summary>
        private const string SP_GET_AFTER_HOUR_DEVICES = "dbo.getRPAfterHoursDevices";

        /// <summary>
        /// This SP will return list of groups for given OC
        /// </summary>
        private const string SP_GET_GROUP_FOR_OC = "dbo.VOC_VW_getGroupsForOC";

        /// <summary>
        /// This SP will return list of findings for given OC, Group and findings
        /// </summary>
        private const string SP_GET_FINDINGS_USING_OGF = "dbo.VOC_VW_getFindingsUsingOCGroupFinding";
        /// <summary>
        /// SP for getCellPhoneCarriers
        /// </summary>
        private const string GET_CELL_CARRIER = "dbo.VOC_VL_getCellPhoneCarriers";

        /// <summary>
        /// SP for getPagerCarriers
        /// </summary>
        private const string GET_PAGER_CARRIER = "dbo.VOC_VL_getPagerCarriers";
        /// <summary>
        /// This constant stores name of stored procedure which will delete the department from Database
        /// </summary>
        private const string SP_GET_OVERLAPPED_ASSIGNMENTS = "dbo.VOC_DEPT_getOverlappedAssignments";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive all department assignements for OC
        /// </summary>
        private const string SP_GET_DEPT_ASSIGNMENTSFOROC = "dbo.VOC_DEPT_getDepartmentAssignmentsForOC";
        /// <summary>
        /// This constant stores name of stored procedure which will update the department from Database
        /// </summary>
        private const string SP_UPDATE_DEPT_ASSIGNMENT = "dbo.VOC_DEPT_updateDepartmentAssignment";
        /// <summary>
        /// This constant stores name of stored procedure which will delete the department from Database
        /// </summary>
        private const string SP_DELETE_DEPT_ASSIGNMENT = "dbo.VOC_DEPT_deleteDepartmentAssignments";
        /// <summary>
        /// This constant stores name of stored procedure which will delete the department from Database
        /// </summary>
        private const string SP_INSERT_DEPT_ASSIGNMENT = "dbo.VOC_DEPT_insertDepartmentAssignment";
        /// <summary>
        /// SP for VOC_getDepartmentDevicesAndEvents
        /// </summary>
        private const string GET_DEPT_DEVICES_AND_EVENTS = "dbo.VOC_DEPT_getDepartmentDevicesAndEvents";

        /// <summary>
        /// This constant stores name of stored procedure which will Validate for duplicate pin for OC
        /// </summary>
        private const string SP_CHECK_DUPLICATE_OC_PIN = "dbo.VOC_CST_checkDuplicateOCPin";
        /// <summary>
        /// This constant stores name of stored procedure which will gets grouppreferences
        ///</summary>
        private const string SP_GET_GROUP_PREFERENCES = "dbo.VOC_CST_getGroupPreferences";
        /// <summary>
        /// This constant stores name of stored procedure which will gets groups by derictory id
        ///</summary>
        private const string SP_GET_GROUPS_BY_DIRECTORYID = "dbo.VOC_CST_getGroupsByDirectoryID";
        /// <summary>
        /// This constant stores name of stored procedure which will gets device short descripton
        ///</summary>
        private const string SP_GET_DVICE_SHORT_DESCRIPTION = "dbo.VOC_CST_getDeviceShortDescription";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive all departments from Database
        /// </summary>
        private const string SP_GET_DEPTS_BY_INSTITUTION = "dbo.VOC_DEPT_GetDepartments";

        /// <summary>
        /// SP for inserting IDType & ID info
        /// </summary>
        private const string SP_INSERT_OC_IDTYPE_INFO = "VOC_CST_insertOCIDTypeORIDInfo";

        /// <summary>
        /// SP for get ExternalIDTypes
        /// </summary>
        private const string SP_GET_EXTERNAL_ID_TYPES = "VOC_CST_getExternalIDTypes";

        /// <summary>
        /// SP to get External IDTypes details for particualr OC
        /// </summary>
        private const string SP_GET_OC_EXTERNAL_ID_TYPE_INFO = "VOC_CST_getOCExternalSystemIDInfo";

        #endregion

        #region Private Variables

        private const string DIRECTORY_ID = "@directoryID";
        private const string FIRST_NAME = "@firstName";
        private const string LAST_NAME = "@lastName";
        private const string NICK_NAME = "@nickname";
        private const string PRIMARY_PHONE = "@primaryPhone";
        private const string PAGER = "@pager";
        private const string CELLPHONE = "@cellPhone";
        private const string ADDITIONAL_CONT_NAME = "@additionalContName";
        private const string ADDITIONAL_CONT_PHONE = "@additionalContPhone";
        private const string PRIMARY_EMAIL = "@primaryEmail";
        private const string FAX = "@fax";
        private const string SPECIALTY = "@specialty";
        private const string AFFILIATION = "@affiliation";
        private const string PRACTICE_GROUP = "@practiceGroup";
        private const string ADDRESS1 = "@address1";
        private const string ADDRESS2 = "@address2";
        private const string ADDRESS3 = "@address3";
        private const string CITY = "@city";
        private const string STATE = "@state";
        private const string ZIP = "@zip";
        private const string UPDATED_BY = "@updatedBy";
        private const string RADIOLOGY_TDR = "@RadiologyTDR";
        private const string LAB_TDR = "@LabTDR";
        private const string NOTES = "@Notes";
        private const string IS_RESIDENT = "@IsResident";
        private const string DEPARTMENT_ID = "@DepartmentID";
        private const string PROFILE_UPDATED_ON = "@profileCompletedOn";

        private const string VOICEOVER_APPROVE = "@voiceOverApproved";
        private const string OC_ID = "@ReferringPhysicianID";
        private const string START_WITH = "@startWith";
        private const string SEARCH_TEARM = "@searchTerm ";
        private const string GRAMMAR = "@grammar";

        private const string SUBSCRIBER_ID = "@subscriberID";
        private const string GROUP_ID = "@groupID";
        private const string REF_ID = "@refId";
        private const string DEPTARTMENT_ID = "@departmentID";

        private const string REFERRING_PHYSICIAN_ID = "@referringPhysicianID";
        private const string ACTIVE = "@active";
        private const string DEVICE_ID = "@deviceID";
        private const string DEVICE_ADDRESS = "@deviceAddress";
        private const string DEVICE_NAME = "@deviceName";
        private const string GATEWAY = "@gateway";
        private const string CARRIER = "@carrier";
        private const string OCDEVICE_ID = "@rpDeviceID";
        private const string FINDING_ID = "@findingID";
        private const string OCNOTIFICATION_ID = "@rpNotificationID";
        private const string RP_ID = "@rpID";
        private const string OCID = "@ocID";
        private const string RET_VAL = "@returnValue";
        private const string OC_AFTER_NOTIFICATION_ID = "@rpAfterHoursNotificationID";
        private const string OC_NOTIFY_EVENT_ID = "@rpNotifyEventID";
        private const string START_HOUR = "@startHour";
        private const string END_HOUR = "@endHour";
        private const string RESULT = "@result";
        private const string FINDING_NAME = "@findingName";
        private const string START_DATE = "@startDateTime";
        private const string END_DATE = "@endDateTime";
        private const string DEPT_ASSIGN_ID = "@departmentAssignID";
        private const string LOGIN_ID = "@loginID";
        private const string PASSWORD = "@password";
        private const string USER_ID = "@userID";
        private const string INITIAL_PAUSE = "@initialPause";
        private const string CALL_FROM = "@callFrom";
        private const string ONLY_LAB_GROUP_REQUIRED = "@onlyLabGroups";

        private const string NEW_IDTYPE = "@newIdType";
        private const string EXTERNAL_ID_TYPEID = "@externalIDTypeID";
        private const string EXTERNAL_INFO_TABLE = "@externalInfoTable";
        private const string IS_DELETED = "@isDeleted";
        private const string RETURN_VAL = "@returnVal";
        /// <summary>
        /// This constant stores name of stored procedure In parameter 
        /// </summary>
        private const string REFERRING_ID = "@referringPhysicianID";
        /// <summary>
        /// This constant stores name of stored procedure In parameter 
        /// </summary>
        private const string START_DATETIME = "@startDateTime";
        /// <summary>
        /// This constant stores name of stored procedure In parameter 
        /// </summary>
        private const string END_DATETIME = "@endDateTime";
        /* Additional fields required by Third party implementations */
        private const string RIS_ID = "@risid";
        private const string LIS_ID = "@lisid";
        private const string MSO_ID = "@msoid";
        private const string NPI = "@npi";
        private const string ED_DOC = "@edDoc";
        private const string VOC_USER_ID = "@vocUserID";
        private const string INSTITUTE_ID = "@instituteID";
        private const string INSTITUTION_ID = "@institutionID";

        private const string PIN = "@pin";
        private const string DEVICE_FOR = "@deviceFor";
        

        #endregion

        /// <summary>
        /// Inserts the Ordering Clinician details in the database.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public int InsertOrderingClinician(OrderingClinicianInfo objOCInfo, DataTable dtblExternalIDInfo)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[34];

                sqlParams[0] = new SqlParameter(DIRECTORY_ID, SqlDbType.Int);
                sqlParams[0].Value = objOCInfo.DirectoryID;

                sqlParams[1] = new SqlParameter(FIRST_NAME, SqlDbType.VarChar, 50);
                sqlParams[1].Value = objOCInfo.FirstName;

                sqlParams[2] = new SqlParameter(LAST_NAME, SqlDbType.VarChar, 50);
                sqlParams[2].Value = objOCInfo.LastName;

                sqlParams[3] = new SqlParameter(NICK_NAME, SqlDbType.VarChar, 50);
                sqlParams[3].Value = objOCInfo.NickName;

                sqlParams[4] = new SqlParameter(PRIMARY_PHONE, SqlDbType.VarChar, 15);
                sqlParams[4].Value = objOCInfo.PrimaryPhone;

                sqlParams[5] = new SqlParameter(PRIMARY_EMAIL, SqlDbType.VarChar, 100);
                sqlParams[5].Value = objOCInfo.PrimaryEmail;

                sqlParams[6] = new SqlParameter(FAX, SqlDbType.VarChar, 15);
                sqlParams[6].Value = objOCInfo.Fax;

                sqlParams[7] = new SqlParameter(SPECIALTY, SqlDbType.VarChar, 75);
                sqlParams[7].Value = objOCInfo.Specialty;

                sqlParams[8] = new SqlParameter(AFFILIATION, SqlDbType.VarChar, 100);
                sqlParams[8].Value = objOCInfo.Affiliation;

                sqlParams[9] = new SqlParameter(PRACTICE_GROUP, SqlDbType.VarChar, 100);
                sqlParams[9].Value = objOCInfo.PracticeGroup;

                sqlParams[10] = new SqlParameter(ADDRESS1, SqlDbType.VarChar, 100);
                sqlParams[10].Value = objOCInfo.Address1;

                sqlParams[11] = new SqlParameter(ADDRESS2, SqlDbType.VarChar, 100);
                sqlParams[11].Value = objOCInfo.Address2;

                sqlParams[12] = new SqlParameter(ADDRESS3, SqlDbType.VarChar, 100);
                sqlParams[12].Value = objOCInfo.Address3;

                sqlParams[13] = new SqlParameter(CITY, SqlDbType.VarChar, 50);
                sqlParams[13].Value = objOCInfo.City;

                sqlParams[14] = new SqlParameter(STATE, SqlDbType.VarChar, 50);
                sqlParams[14].Value = objOCInfo.State;

                sqlParams[15] = new SqlParameter(ZIP, SqlDbType.VarChar, 15);
                sqlParams[15].Value = objOCInfo.Zip;

                sqlParams[16] = new SqlParameter(REF_ID, SqlDbType.Int);
                sqlParams[16].Direction = ParameterDirection.ReturnValue;

                sqlParams[17] = new SqlParameter(ADDITIONAL_CONT_NAME, SqlDbType.VarChar, 50);
                sqlParams[17].Value = objOCInfo.AdditionalContName;

                sqlParams[18] = new SqlParameter(ADDITIONAL_CONT_PHONE, SqlDbType.VarChar, 15);
                sqlParams[18].Value = objOCInfo.AdditionalContPhone;

                sqlParams[19] = new SqlParameter(UPDATED_BY, SqlDbType.Int);
                sqlParams[19].Value = objOCInfo.UpdatedBy;

                sqlParams[20] = new SqlParameter(RADIOLOGY_TDR, SqlDbType.Bit);
                sqlParams[20].Value = objOCInfo.RadiologyTDR;

                sqlParams[21] = new SqlParameter(LAB_TDR, SqlDbType.Bit);
                sqlParams[21].Value = objOCInfo.LabTDR;

                sqlParams[22] = new SqlParameter(NOTES, SqlDbType.VarChar);
                sqlParams[22].Value = objOCInfo.Notes;

                sqlParams[23] = new SqlParameter(PROFILE_UPDATED_ON, SqlDbType.DateTime);
                if (objOCInfo.ProfileCompleted)
                    sqlParams[23].Value = DateTime.Now;
                else
                    sqlParams[23].Value = null;

                sqlParams[24] = new SqlParameter(IS_RESIDENT, SqlDbType.Bit);
                sqlParams[24].Value = objOCInfo.IsResident;

                sqlParams[25] = new SqlParameter(DEPARTMENT_ID, SqlDbType.Int);
                sqlParams[25].Value = objOCInfo.DepartmentID;

                sqlParams[26] = new SqlParameter(START_DATE, SqlDbType.DateTime);
                sqlParams[26].Value = objOCInfo.DepartmentStartDate;

                sqlParams[27] = new SqlParameter(END_DATE, SqlDbType.DateTime);
                if (objOCInfo.DepartmentEndDate == DateTime.MinValue)
                    sqlParams[27].Value = null;
                else
                    sqlParams[27].Value = objOCInfo.DepartmentEndDate;

                sqlParams[28] = new SqlParameter(LOGIN_ID, SqlDbType.VarChar);
                if (objOCInfo.LoginID.Length > 0)
                    sqlParams[28].Value = objOCInfo.LoginID;
                else
                    sqlParams[28].Value = DBNull.Value;


                sqlParams[29] = new SqlParameter(PASSWORD, SqlDbType.VarChar);
                if (objOCInfo.Password.Length > 0)
                    sqlParams[29].Value = objOCInfo.Password;
                else
                    sqlParams[29].Value = DBNull.Value;

                sqlParams[30] = new SqlParameter(ED_DOC, SqlDbType.Bit);
                sqlParams[30].Value = objOCInfo.IsEDDoc;

                sqlParams[31] = new SqlParameter(PIN, SqlDbType.VarChar, 5);
                if (objOCInfo.PINForMessageRetrieve.Length > 0)
                    sqlParams[31].Value = objOCInfo.PINForMessageRetrieve;
                else
                    sqlParams[31].Value = DBNull.Value;

                sqlParams[32] = new SqlParameter(EXTERNAL_INFO_TABLE, System.Data.SqlDbType.Xml);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    dtblExternalIDInfo.WriteXml(memoryStream);
                    UTF8Encoding encoding = new UTF8Encoding();
                    sqlParams[32].Value = encoding.GetString(memoryStream.ToArray());
                }

                sqlParams[33] = new SqlParameter(CELLPHONE, SqlDbType.VarChar);
                sqlParams[33].Value = objOCInfo.CellPhone;

                object result = SqlHelper.ExecuteScalar(cnx, CommandType.StoredProcedure, SP_INSERT_REFERRING_PHYSICIAN, sqlParams);
                int ocID = 0;
                if (result != null)
                    ocID = Convert.ToInt32(result);

                return ocID;
            }
        }
        /// <summary>
        /// This Method returns OC List.
        /// </summary>
        /// <param name="directoryID">Directory ID</param>
        /// <param name="iApprove">Approve Voiceover</param>
        /// <param name="startWith">startWith Charector</param>
        /// <returns></returns>
        public DataTable GetOCforVoiceover(int directoryID, int iApprove, string startWith, string searchTerm, int institutionID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtOC = new DataTable();
                SqlParameter[] arSqlParams = new SqlParameter[5];

                arSqlParams[0] = new SqlParameter(DIRECTORY_ID, SqlDbType.Int);
                arSqlParams[0].Value = directoryID;
                arSqlParams[1] = new SqlParameter(VOICEOVER_APPROVE, SqlDbType.Int);
                arSqlParams[1].Value = iApprove;
                arSqlParams[2] = new SqlParameter(START_WITH, SqlDbType.VarChar);
                arSqlParams[2].Value = startWith;
                arSqlParams[3] = new SqlParameter(SEARCH_TEARM, SqlDbType.VarChar);
                arSqlParams[3].Value = searchTerm;
                arSqlParams[4] = new SqlParameter(INSTITUTE_ID, SqlDbType.Int);
                arSqlParams[4].Value = institutionID;

                SqlDataReader drOC = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_OC_FOR_VOICEOVER, arSqlParams);
                dtOC.Load(drOC);
                return dtOC;
            }
        }
        /// <summary>
        /// This Method Delete OC Voiceover from DB.
        /// </summary>
        /// <param name="ocID"></param>
        public void DeleteOCVoiceover(int ocID)
        {
            SqlConnection sqlConnection = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(OC_ID, ocID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, SP_DELETE_VOICEOVER, objSqlParameter);
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
        /// This Method Approve Voiceover in DB
        /// </summary>
        /// <param name="ocID"></param>
        public void UpdateOCVoiceover(int ocID)
        {
            SqlConnection sqlConnection = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(OC_ID, ocID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, SP_UPDATE_VOICEOVER, objSqlParameter);
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
        /// Get Grammmers for OC for given directory
        /// </summary>
        /// <param name="directoryID">Directory ID</param>
        /// <param name="startWith">Last Name of OC Start with</param>
        /// <returns></returns>
        public DataTable GetOCGrammerInfo(int directoryID, string startWith, string searchTerm)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtOCGrammar = new DataTable();
                SqlParameter[] arSqlParams = new SqlParameter[3];

                arSqlParams[0] = new SqlParameter(DIRECTORY_ID, SqlDbType.Int);
                arSqlParams[0].Value = directoryID;
                arSqlParams[1] = new SqlParameter(START_WITH, SqlDbType.VarChar);
                arSqlParams[1].Value = startWith;
                arSqlParams[2] = new SqlParameter(SEARCH_TEARM, SqlDbType.VarChar);
                arSqlParams[2].Value = searchTerm;

                SqlDataReader drOC = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_GRAMMAR_FOR_DIRECTORY, arSqlParams);
                dtOCGrammar.Load(drOC);
                return dtOCGrammar;
            }
        }

        /// <summary>
        /// Update Grammmers of given OC 
        /// </summary>
        /// <param name="ocID">Referring Physician ID</param>
        /// <param name="grammar">grammar</param>
        /// <returns></returns>
        public bool UpdateGrammerInfo(int ocID, string grammar)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtOCGrammar = new DataTable();
                SqlParameter[] arSqlParams = new SqlParameter[2];

                arSqlParams[0] = new SqlParameter(OC_ID, SqlDbType.Int);
                arSqlParams[0].Value = ocID;
                arSqlParams[1] = new SqlParameter(GRAMMAR, SqlDbType.VarChar);
                if (grammar == null)
                    arSqlParams[1].Value = DBNull.Value;
                else
                    arSqlParams[1].Value = grammar;

                int recordUpdated = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_UPDATE_GRAMMAR, arSqlParams);
                if (recordUpdated > 0)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// Get Departments for by OC ID
        /// </summary>
        /// <param name="institutionID"></param>
        /// <returns></returns>
        public DataTable GetDepartments(int ocID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtDeptData = new DataTable("Depts");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(OC_ID, ocID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drDepts = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_DEPTS, objSqlParameter);

                dtDeptData.Load(drDepts);
                drDepts.Close();
                return dtDeptData;
            }
        }
        /// <summary>
        /// This method will call "getReferringPhysicianByID" stored procedure passing referringPhysicianID as parameter
        /// it will return Clinicains for given parameters. 
        /// </summary>
        /// <param name="cnx"></param>
        public DataTable GetReferringPhysicianByID(int referringPhysicianID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtPhysicians = new DataTable();
                SqlParameter[] arSqlParams = new SqlParameter[1];

                arSqlParams[0] = new SqlParameter(OC_ID, SqlDbType.Int);
                arSqlParams[0].Value = referringPhysicianID;
                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_REFERRING_PHYSICIANS_BY_ID, arSqlParams);
                dtPhysicians.Load(reader);
                reader.Close();
                return dtPhysicians;
            }
        }

        /// <summary>
        /// Updates the Ordering Clinician details in the database.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public int UpdateOrderingClinician(OrderingClinicianInfo objOCInfo, DataTable dtblExternalIDInfo)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[38];

                sqlParams[0] = new SqlParameter(REFERRING_PHYSICIAN_ID, SqlDbType.Int);
                sqlParams[0].Value = objOCInfo.ReferringPhysicianID;

                sqlParams[1] = new SqlParameter(ACTIVE, SqlDbType.Bit);
                sqlParams[1].Value = Convert.ToByte(objOCInfo.Active);

                sqlParams[2] = new SqlParameter(FIRST_NAME, SqlDbType.VarChar, 50);
                sqlParams[2].Value = objOCInfo.FirstName;

                sqlParams[3] = new SqlParameter(LAST_NAME, SqlDbType.VarChar, 50);
                sqlParams[3].Value = objOCInfo.LastName;

                sqlParams[4] = new SqlParameter(NICK_NAME, SqlDbType.VarChar, 50);
                sqlParams[4].Value = objOCInfo.NickName;

                sqlParams[5] = new SqlParameter(PRIMARY_PHONE, SqlDbType.VarChar, 15);
                sqlParams[5].Value = objOCInfo.PrimaryPhone;

                sqlParams[6] = new SqlParameter(PRIMARY_EMAIL, SqlDbType.VarChar, 100);
                sqlParams[6].Value = objOCInfo.PrimaryEmail;

                sqlParams[7] = new SqlParameter(FAX, SqlDbType.VarChar, 15);
                sqlParams[7].Value = objOCInfo.Fax;

                sqlParams[8] = new SqlParameter(SPECIALTY, SqlDbType.VarChar, 75);
                sqlParams[8].Value = objOCInfo.Specialty;

                sqlParams[9] = new SqlParameter(AFFILIATION, SqlDbType.VarChar, 100);
                sqlParams[9].Value = objOCInfo.Affiliation;

                sqlParams[10] = new SqlParameter(PRACTICE_GROUP, SqlDbType.VarChar, 100);
                sqlParams[10].Value = objOCInfo.PracticeGroup;

                sqlParams[11] = new SqlParameter(ADDRESS1, SqlDbType.VarChar, 100);
                sqlParams[11].Value = objOCInfo.Address1;

                sqlParams[12] = new SqlParameter(ADDRESS2, SqlDbType.VarChar, 100);
                sqlParams[12].Value = objOCInfo.Address2;

                sqlParams[13] = new SqlParameter(ADDRESS3, SqlDbType.VarChar, 100);
                sqlParams[13].Value = objOCInfo.Address3;

                sqlParams[14] = new SqlParameter(CITY, SqlDbType.VarChar, 50);
                sqlParams[14].Value = objOCInfo.City;

                sqlParams[15] = new SqlParameter(STATE, SqlDbType.VarChar, 50);
                sqlParams[15].Value = objOCInfo.State;

                sqlParams[16] = new SqlParameter(ZIP, SqlDbType.VarChar, 15);
                sqlParams[16].Value = objOCInfo.Zip;

                sqlParams[17] = new SqlParameter(ADDITIONAL_CONT_NAME, SqlDbType.VarChar, 50);
                sqlParams[17].Value = objOCInfo.AdditionalContName;

                sqlParams[18] = new SqlParameter(ADDITIONAL_CONT_PHONE, SqlDbType.VarChar, 15);
                sqlParams[18].Value = objOCInfo.AdditionalContPhone;

                sqlParams[19] = new SqlParameter(UPDATED_BY, SqlDbType.Int);
                sqlParams[19].Value = objOCInfo.UpdatedBy;

                sqlParams[20] = new SqlParameter(RADIOLOGY_TDR, SqlDbType.Bit);
                sqlParams[20].Value = objOCInfo.RadiologyTDR;

                sqlParams[21] = new SqlParameter(LAB_TDR, SqlDbType.Bit);
                sqlParams[21].Value = objOCInfo.LabTDR;

                sqlParams[22] = new SqlParameter(NOTES, SqlDbType.VarChar);
                sqlParams[22].Value = objOCInfo.Notes;

                sqlParams[23] = new SqlParameter(PROFILE_UPDATED_ON, SqlDbType.DateTime);
                if (objOCInfo.ProfileCompleted)
                    sqlParams[23].Value = DateTime.Now;
                else
                    sqlParams[23].Value = null;

                sqlParams[24] = new SqlParameter(IS_RESIDENT, SqlDbType.Bit);
                sqlParams[24].Value = objOCInfo.IsResident;

                sqlParams[25] = new SqlParameter(DEPARTMENT_ID, SqlDbType.Int);
                sqlParams[25].Value = objOCInfo.DepartmentID;

                sqlParams[26] = new SqlParameter(RET_VAL, SqlDbType.Int);
                sqlParams[26].Direction = ParameterDirection.Output;


                sqlParams[27] = new SqlParameter(DEPT_ASSIGN_ID, SqlDbType.Int);
                sqlParams[27].Value = objOCInfo.DepartmentAssignID;

                sqlParams[28] = new SqlParameter(START_DATE, SqlDbType.DateTime);
                sqlParams[28].Value = objOCInfo.DepartmentStartDate;

                sqlParams[29] = new SqlParameter(END_DATE, SqlDbType.DateTime);
                if (objOCInfo.DepartmentEndDate == DateTime.MinValue)
                    sqlParams[29].Value = null;
                else
                    sqlParams[29].Value = objOCInfo.DepartmentEndDate;

                sqlParams[30] = new SqlParameter(EXTERNAL_INFO_TABLE, System.Data.SqlDbType.Xml);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    dtblExternalIDInfo.WriteXml(memoryStream);
                    UTF8Encoding encoding = new UTF8Encoding();
                    sqlParams[30].Value = encoding.GetString(memoryStream.ToArray());
                }

                sqlParams[31] = new SqlParameter(LOGIN_ID, SqlDbType.VarChar);
                if (objOCInfo.LoginID.Length > 0)
                    sqlParams[31].Value = objOCInfo.LoginID;
                else
                    sqlParams[31].Value = DBNull.Value;

                sqlParams[32] = new SqlParameter(PASSWORD, SqlDbType.VarChar);
                if (objOCInfo.Password.Length > 0)
                    sqlParams[32].Value = objOCInfo.Password;
                else
                    sqlParams[32].Value = DBNull.Value;

                sqlParams[33] = new SqlParameter(ED_DOC, SqlDbType.Bit);
                sqlParams[33].Value = objOCInfo.IsEDDoc;

                sqlParams[34] = new SqlParameter(VOC_USER_ID, SqlDbType.Int);
                sqlParams[34].Value = objOCInfo.VOCUserID;

                sqlParams[35] = new SqlParameter(INSTITUTE_ID, SqlDbType.Int);
                sqlParams[35].Value = objOCInfo.InstituteID;

                sqlParams[36] = new SqlParameter(PIN, SqlDbType.VarChar, 5);
                if (objOCInfo.PINForMessageRetrieve.Length > 0)
                    sqlParams[36].Value = objOCInfo.PINForMessageRetrieve;
                else
                    sqlParams[36].Value = DBNull.Value;
                sqlParams[37] = new SqlParameter(CELLPHONE, SqlDbType.VarChar);
                sqlParams[37].Value = objOCInfo.CellPhone;

                SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_UPDATE_REFERRING_PHYSICIAN, sqlParams);
                return int.Parse(sqlParams[26].Value.ToString());
            }
        }
        /// <summary>
        /// Inserts the Ordering Clinician device in the database.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public int InsertOCDevice(OCDeviceInfo objDevice,string callFrom)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[11];

                sqlParams[0] = new SqlParameter(REFERRING_PHYSICIAN_ID, SqlDbType.Int);
                sqlParams[0].Value = objDevice.ReferringPhysicianID;

                sqlParams[1] = new SqlParameter(DEVICE_ID, SqlDbType.Int);
                sqlParams[1].Value = objDevice.DeviceID;

                sqlParams[2] = new SqlParameter(DEVICE_ADDRESS, SqlDbType.VarChar, 100);
                sqlParams[2].Value = objDevice.DeviceAddress;

                sqlParams[3] = new SqlParameter(GATEWAY, SqlDbType.VarChar, 100);
                sqlParams[3].Value = objDevice.Gateway;

                sqlParams[4] = new SqlParameter(CARRIER, SqlDbType.VarChar, 100);
                sqlParams[4].Value = objDevice.Carrier;
                
                sqlParams[5] = new SqlParameter(INITIAL_PAUSE, SqlDbType.Decimal);
                if (objDevice.InitialPauseTime == "-1" ||objDevice.InitialPauseTime == "")
                    sqlParams[5].Value = DBNull.Value;
                else
                    sqlParams[5].Value = objDevice.InitialPauseTime;

                sqlParams[6] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                sqlParams[6].Value = objDevice.GroupID;
                sqlParams[7] = new SqlParameter(OC_NOTIFY_EVENT_ID, SqlDbType.Int);
                sqlParams[7].Value = objDevice.OCNotifyEventID;
                sqlParams[8] = new SqlParameter(FINDING_ID, SqlDbType.Int);
                sqlParams[8].Value = objDevice.FindingID;
                sqlParams[9] = new SqlParameter(DEVICE_NAME, SqlDbType.VarChar, 100);
                sqlParams[9].Value = objDevice.DeviceName;
                sqlParams[10] = new SqlParameter(CALL_FROM, SqlDbType.VarChar, 10);
                sqlParams[10].Value = callFrom;
                

                object  result = SqlHelper.ExecuteScalar(cnx, CommandType.StoredProcedure, SP_INSERT_OC_DEVICE, sqlParams);
                int deviceID = 0;
                if (result != null)
                    deviceID = Convert.ToInt32(result);

                return deviceID;
            }
        }

        /// <summary>
        /// Gets Ordering Clinician device from database into datatable.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public DataTable GetOCDevice(int referringPhysicianID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtOCDevice = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[1];

                sqlParams[0] = new SqlParameter(REFERRING_PHYSICIAN_ID, SqlDbType.Int);
                sqlParams[0].Value = referringPhysicianID;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_OC_DEVICE_EVENTS, sqlParams);
                dtOCDevice.Load(reader);
                reader.Close();
                return dtOCDevice;
            }
        }

        /// <summary>
        /// Gets Ordering Clinician device from database into datatable.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public DataTable GetDevices()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtOCDevice = new DataTable();
                SqlParameter[] arParams = new SqlParameter[1];
                arParams[0] = new SqlParameter(DEVICE_FOR, "OC");
                arParams[0].Direction = ParameterDirection.Input;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_DEVICES, arParams); 
                dtOCDevice.Load(reader);
                reader.Close();
                return dtOCDevice;
            }
        }

        /// <summary>
        /// Deletes selected device for Ordering Clinician.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public int DeleteDeviceAndEvent(int ocDeviceID, int ocNotificationID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[2];
                sqlParams[0] = new SqlParameter(OCDEVICE_ID, SqlDbType.Int);
                sqlParams[0].Value = ocDeviceID;
                sqlParams[1] = new SqlParameter(OCNOTIFICATION_ID, SqlDbType.Int);
                sqlParams[1].Value = ocNotificationID;

                int result = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_DELETE_OC_DEVICE_EVENT, sqlParams);
                return result;
            }
        }

        /// <summary>
        /// Updates the Ordering Clinician device in the database.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public int UpdateOCDevice(OCDeviceInfo objDevice)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[10];

                sqlParams[0] = new SqlParameter(OCDEVICE_ID, SqlDbType.Int);
                sqlParams[0].Value = objDevice.OCDeviceID;

                sqlParams[1] = new SqlParameter(DEVICE_NAME, SqlDbType.VarChar, 50);
                sqlParams[1].Value = objDevice.DeviceName;

                sqlParams[2] = new SqlParameter(DEVICE_ADDRESS, SqlDbType.VarChar, 100);
                sqlParams[2].Value = objDevice.DeviceAddress;

                sqlParams[3] = new SqlParameter(GATEWAY, SqlDbType.VarChar, 100);
                sqlParams[3].Value = objDevice.Gateway;

                sqlParams[4] = new SqlParameter(RP_ID, SqlDbType.Int);
                sqlParams[4].Value = objDevice.ReferringPhysicianID;

                sqlParams[5] = new SqlParameter(INITIAL_PAUSE, SqlDbType.Decimal);
                if (objDevice.InitialPauseTime == "-1")
                    sqlParams[5].Value = DBNull.Value;
                else
                    sqlParams[5].Value = objDevice.InitialPauseTime;

                sqlParams[6] = new SqlParameter(FINDING_ID, SqlDbType.Int);
                sqlParams[6].Value = objDevice.FindingID;

                sqlParams[7] = new SqlParameter(OC_NOTIFY_EVENT_ID, SqlDbType.Int);
                sqlParams[7].Value = objDevice.OCNotifyEventID;

                sqlParams[8] = new SqlParameter(OCNOTIFICATION_ID, SqlDbType.Int);
                sqlParams[8].Value = objDevice.OCNotificationID;

                sqlParams[9] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                sqlParams[9].Value = objDevice.GroupID;

                int result = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_UPDATE_OC_DEVICE_EVENT, sqlParams);
                return result;
            }
        }

        /// <summary>
        /// Deletes selected after hours notifications for Ordering Clinician.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public int DeleteAfterHoursNotifications(int ocAfterHoursNotificationID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter(OC_AFTER_NOTIFICATION_ID, SqlDbType.Int);
                sqlParams[0].Value = ocAfterHoursNotificationID;
                int result = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_DELETE_AFTER_HOURS_NOTIFICATIONS, sqlParams);
                return result;
            }
        }

        /// <summary>
        /// Gets Ordering Clinician notifications from database into datatable.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public DataTable GetOCNotification(int ocID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtOCNotifications = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[1];

                sqlParams[0] = new SqlParameter(RP_ID, SqlDbType.Int);
                sqlParams[0].Value = ocID;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_OC_NOTIFICATIONS, sqlParams);
                dtOCNotifications.Load(reader);
                reader.Close();
                return dtOCNotifications;
            }
        }

        /// <summary>
        /// Gets Ordering Clinician notify events from database into datatable.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public DataTable GetOCNotifyEvents()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtOCNotifyEvents = new DataTable();
                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_OC_EVENTS);
                dtOCNotifyEvents.Load(reader);
                reader.Close();
                return dtOCNotifyEvents;
            }
        }
        /// <summary>
        /// Gets findings for Subscriber.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public DataTable GetFindingForSubscriber(int subscriberID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtOCFindings = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[1];

                sqlParams[0] = new SqlParameter(SUBSCRIBER_ID, SqlDbType.Int);
                sqlParams[0].Value = subscriberID;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_FINDINGS_FOR_SUBSCRIBER, sqlParams);
                dtOCFindings.Load(reader);
                reader.Close();
                return dtOCFindings;
            }
        }

        /// <summary>
        /// Gets findings for OC or group.
        /// </summary>
        /// <param name="ocID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetFindingForOCorGroup(int ocID, int groupID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtOCFindings = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[2];

                sqlParams[0] = new SqlParameter(OCID, SqlDbType.Int);
                sqlParams[0].Value = ocID;
                sqlParams[1] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                sqlParams[1].Value = groupID;

                SqlDataReader drFindings = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_FINDINGS_USING_OC_GROUP, sqlParams);
                dtOCFindings.Load(drFindings);
                drFindings.Close();
                return dtOCFindings;
            }
        }

        /// <summary>
        /// Gets After hours notifications from database into datatable.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public DataTable GetAfterHoursNotification(int ocID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtOCNotifications = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[1];

                sqlParams[0] = new SqlParameter(RP_ID, SqlDbType.Int);
                sqlParams[0].Value = ocID;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_AFTER_HOURS_NOTIFICATIONS, sqlParams);
                dtOCNotifications.Load(reader);
                reader.Close();
                return dtOCNotifications;
            }
        }

        /// <summary>
        /// Inserts the Ordering Clinician Notifications in the database.
        /// </summary>
        /// <param name="objDevice"></param>
        /// <param name="ocID"></param>
        /// <param name="groupID"></param>
        /// <param name="findingName"></param>
        /// <returns></returns>
        public bool InsertOCNotifications(OCDeviceInfo objDevice, int groupID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtFindings = null;
                SqlParameter[] sqlParams = null;
                try
                {
                    sqlParams = new SqlParameter[4];

                    sqlParams[0] = new SqlParameter(OC_NOTIFY_EVENT_ID, SqlDbType.Int);
                    sqlParams[0].Value = objDevice.OCNotifyEventID;

                    sqlParams[1] = new SqlParameter(OCDEVICE_ID, SqlDbType.Int);
                    sqlParams[1].Value = objDevice.OCDeviceID;

                    sqlParams[2] = new SqlParameter(FINDING_ID, SqlDbType.Int);
                    sqlParams[2].Value = objDevice.FindingID;

                    sqlParams[3] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                    sqlParams[3].Value = groupID;

                    SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_INSERT_OC_NOTIFICATIONS, sqlParams);
                }
                catch (Exception ex)
                {
                     throw ex;
                }
                return true;
            }
        }

        /// <summary>
        /// Inserts the after hours notifications in the database.
        /// </summary>
        /// <param name="objDevice"></param>
        /// <param name="ocID"></param>
        /// <param name="groupID"></param>
        /// <param name="findingName"></param>
        /// <returns></returns>
        public bool InsertAfterHoursNotifications(OCDeviceInfo objDevice,  string callFrom)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtFindings = null;
                SqlParameter[] sqlParams = null;
                try
                {
                    sqlParams = new SqlParameter[7];

                    sqlParams[0] = new SqlParameter(RP_ID, SqlDbType.Int);
                    sqlParams[0].Value = objDevice.ReferringPhysicianID;

                    sqlParams[1] = new SqlParameter(OCDEVICE_ID, SqlDbType.Int);
                    sqlParams[1].Value = objDevice.OCDeviceID;

                    sqlParams[2] = new SqlParameter(FINDING_ID, SqlDbType.Int);
                    sqlParams[2].Value = objDevice.FindingID;

                    sqlParams[3] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                    sqlParams[3].Value = objDevice.GroupID;

                    sqlParams[4] = new SqlParameter(START_HOUR, SqlDbType.Int);
                    sqlParams[4].Value = objDevice.StartHour;

                    sqlParams[5] = new SqlParameter(END_HOUR, SqlDbType.Int);
                    sqlParams[5].Value = objDevice.EndHour;

                    sqlParams[6] = new SqlParameter(CALL_FROM, SqlDbType.VarChar,10);
                    sqlParams[6].Value = callFrom;


                    SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_INSERT_AFTER_HOUR_NOTIFICATIONS, sqlParams);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return true;
            }
        }

        /// <summary>
        /// Gets After hours devices from database into datatable.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public DataTable GetAfterHoursDevices(int ocID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtOCNotifications = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[1];

                sqlParams[0] = new SqlParameter(REFERRING_PHYSICIAN_ID, SqlDbType.Int);
                sqlParams[0].Value = ocID;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_AFTER_HOUR_DEVICES, sqlParams);
                dtOCNotifications.Load(reader);
                reader.Close();
                return dtOCNotifications;
            }
        }


        /// <summary>
        /// Gets After hours devices from database into datatable.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public DataTable GetGroupsForOC(int ocID, bool onlyLabGroupsRequired)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtGroup = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[2];

                sqlParams[0] = new SqlParameter(OCID, SqlDbType.Int);
                sqlParams[0].Value = ocID;

                sqlParams[1] = new SqlParameter(ONLY_LAB_GROUP_REQUIRED, SqlDbType.Int);
                sqlParams[1].Value = onlyLabGroupsRequired;

                SqlDataReader drGroups = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_GROUP_FOR_OC, sqlParams);
                dtGroup.Load(drGroups);
                drGroups.Close();
                return dtGroup;
            }
        }
        /// <summary>
        /// This function is to get all list of Cell phone carriers.
        /// This function calls stored procedure "getCellPhoneCarriers"
        /// </summary>
        /// <param name="cnx">Connection String</param>
        public DataSet GetCellPhoneCarriers()
        {
            DataSet dsCellCarrier = null;
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                dsCellCarrier = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, GET_CELL_CARRIER);
                return dsCellCarrier;
            }
        }

        /// <summary>
        /// This function gets all the Pager Carriers available.
        /// This function calls stored procedure "getPagerCarriers"
        /// </summary>
        /// <param name="cnx"></param>
        public DataSet GetPagerCarriers()
        {
            DataSet dsPagerCarrier = null;
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                dsPagerCarrier = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, GET_PAGER_CARRIER);
                return dsPagerCarrier;
            }
        }
        /// <summary>
        /// Get Over lappedAssignments
        /// </summary>
        /// <returns></returns>
        public string getOverlappedAssignments(int departmentAssignID, int departmentID, int referringPhysicianID, DateTime startDateTime, DateTime endDateTime)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[4];

                objSqlParameter[0] = new SqlParameter(REFERRING_ID, SqlDbType.Int);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[0].Value = referringPhysicianID;

                objSqlParameter[1] = new SqlParameter(START_DATETIME, SqlDbType.DateTime);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                if (startDateTime == DateTime.MinValue)
                    objSqlParameter[1].Value = null;
                else
                    objSqlParameter[1].Value = startDateTime;

                objSqlParameter[2] = new SqlParameter(END_DATETIME, SqlDbType.DateTime);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                if (endDateTime == DateTime.MinValue)
                    objSqlParameter[2].Value = null;
                else
                    objSqlParameter[2].Value = endDateTime;

                objSqlParameter[3] = new SqlParameter(DEPT_ASSIGN_ID, SqlDbType.Int);
                objSqlParameter[3].Direction = ParameterDirection.Input;
                objSqlParameter[3].Value = departmentAssignID;

                string overlappedAssignments = "";
                DataSet dsOverlappedAssignment = SqlHelper.ExecuteDataset(sqlConnection, CommandType.StoredProcedure, SP_GET_OVERLAPPED_ASSIGNMENTS, objSqlParameter);

                if (dsOverlappedAssignment != null)
                {

                    for (int assignment = 0; assignment < dsOverlappedAssignment.Tables[0].Rows.Count; assignment++)
                    {
                        overlappedAssignments += " Department: " + dsOverlappedAssignment.Tables[0].Rows[assignment]["DepartmentName"].ToString() +
                            " StartTime: " + dsOverlappedAssignment.Tables[0].Rows[assignment]["StartDateTime"].ToString() +
                            " EndTime: " + dsOverlappedAssignment.Tables[0].Rows[assignment]["EndDateTime"].ToString() + @"\n";
                    }
                }
                return overlappedAssignments;

            }
        }

        /// <summary>
        /// Populates all the devices and preferances for the selected Dept in the Datagrid grdDeptDevices
        /// </summary>
        /// <param name="cnx"></param>
        public DataSet GetNotificationPreferencesForDept(int deptID)
        {
            DataSet dsDeptNotification = null;
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] arParams = new SqlParameter[1];
                arParams[0] = new SqlParameter("@DeptID", SqlDbType.Int);
                arParams[0].Value = deptID;
                dsDeptNotification = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, GET_DEPT_DEVICES_AND_EVENTS, arParams);
                return dsDeptNotification;
            }
        }

        /// <summary>
        /// Get new Pin number from database for OC
        /// </summary>
        /// <returns></returns>
        public string GetNewPin(int referringPhysicianID)
        {
            string pinNumber = "";
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlDataReader sqlDataReader;
                SqlParameter[] arParams = new SqlParameter[1];
                arParams[0] = new SqlParameter(REFERRING_PHYSICIAN_ID, SqlDbType.Int);
                arParams[0].Value = referringPhysicianID;
                sqlDataReader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_UNIQUE_OC_PASSWORD, arParams);
                if (sqlDataReader.Read())
                    pinNumber = sqlDataReader["Password"].ToString();

                sqlDataReader.Close();
            }
            return pinNumber;
        }

        /// <summary>
        /// Checks for duplicate OC PIN
        /// </summary>
        /// <param name="loginID"></param>
        /// <param name="password"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool CheckDuplicateOCPIN(string loginID, string password, int userID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] arrParams = new SqlParameter[3];

                arrParams[0] = new SqlParameter(LOGIN_ID, SqlDbType.VarChar, 10);
                arrParams[0].Value = loginID;

                arrParams[1] = new SqlParameter(PASSWORD, SqlDbType.VarChar, 10);
                arrParams[1].Value = password;

                arrParams[2] = new SqlParameter(USER_ID, SqlDbType.Int);
                arrParams[2].Value = userID;

                SqlDataReader duplicateOCID = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_CHECK_DUPLICATE_OC_PIN, arrParams);
                if (duplicateOCID.Read())
                {
                    if (Convert.ToInt32(duplicateOCID["RowCnt"]) > 0)
                        return true;
                }

                return false;
            }
        }
        /// <summary>
        /// Checks whether selected group is Lab Group or not
        /// </summary>
        /// <param name="groupID">int</param>
        /// <returns></returns>
        public bool IsLabGroup(int groupID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] arrParams = new SqlParameter[1];

                arrParams[0] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                arrParams[0].Value = groupID;

                SqlDataReader groupPref = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_GROUP_PREFERENCES, arrParams);
                if (groupPref.Read())
                {
                    if (Convert.ToBoolean(groupPref["GroupType"]))
                    {
                        groupPref.Close();
                        return true;
                    }
                }
                groupPref.Close();
                return false;
            }
        }
        /// <summary>
        /// Gets group list for a given directory ID
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public DataTable GetGroupsByDirectoryID(int directoryID, bool onlyLabGroupsRequired)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[2];

                sqlParams[0] = new SqlParameter(DIRECTORY_ID, SqlDbType.Int);
                sqlParams[0].Value = directoryID;

                sqlParams[1] = new SqlParameter(ONLY_LAB_GROUP_REQUIRED, SqlDbType.Int);
                sqlParams[1].Value = onlyLabGroupsRequired;

                DataSet dsGroups = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, SP_GET_GROUPS_BY_DIRECTORYID, sqlParams);
                
                return dsGroups.Tables[0];
            }
        }
        /// <summary>
        /// Gets the device short description for a given device id
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public string GetDeviceShortDescription(int deviceID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];

                sqlParams[0] = new SqlParameter(DEVICE_ID, SqlDbType.Int);
                sqlParams[0].Value = deviceID;

                object deviceDesc = SqlHelper.ExecuteScalar(cnx, CommandType.StoredProcedure, SP_GET_DVICE_SHORT_DESCRIPTION, sqlParams);
                string shortDesc = "";
                if (deviceDesc != null)
                {
                   shortDesc =  deviceDesc.ToString();
                }
                return shortDesc;
            }
        }
        /// <summary>
        /// Get Departments for an institution
        /// </summary>
        /// <param name="institutionID"></param>
        /// <returns></returns>
        public DataTable GetDepartmentsByInstitution(int institutionID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtDeptData = new DataTable("Depts");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(INSTITUTION_ID, institutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drDepts = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_DEPTS_BY_INSTITUTION, objSqlParameter);

                dtDeptData.Load(drDepts);
                drDepts.Close();
                return dtDeptData;
            }
        }
        
        /// <summary>
        /// Inserts new ID type or existing ID number corresponding to existing ID type
        /// </summary>
        /// <param name="rpID"></param>
        /// <param name="userId"></param>
        /// <param name="newIdType"></param>
        /// <param name="externalIDTypeID"></param>
        /// <returns></returns>
        public int InsertIdTypeInfo(int rpID, string userId, string newIdType, int externalIDTypeID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[4];

                sqlParams[0] = new SqlParameter(REFERRING_PHYSICIAN_ID, SqlDbType.Int);
                sqlParams[0].Value = rpID;

                sqlParams[1] = new SqlParameter(USER_ID, SqlDbType.VarChar, 255);
                sqlParams[1].Value = userId;

                sqlParams[2] = new SqlParameter(NEW_IDTYPE, SqlDbType.VarChar, 50);
                sqlParams[2].Value = newIdType;

                sqlParams[3] = new SqlParameter(EXTERNAL_ID_TYPEID, SqlDbType.Int);
                sqlParams[3].Value = externalIDTypeID;

                int result = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_INSERT_OC_IDTYPE_INFO, sqlParams);
                return result;
            }
        }

        /// <summary>
        /// Gets External System ID types into datatable.
        /// </summary>
        /// <returns></returns>
        public DataTable GetExternalIDTypes()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtIDTypes = new DataTable();

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_EXTERNAL_ID_TYPES);
                dtIDTypes.Load(reader);
                reader.Close();
                return dtIDTypes;
            }
        }

        /// <summary>
        /// Gets details of external System ID for particular OC
        /// </summary>
        /// <param name="referringPhysicianId"></param>
        /// <returns></returns>
        public DataTable GetOCIdTypesInfo(int referringPhysicianId)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtIDTypeInfo = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[1];

                sqlParams[0] = new SqlParameter(RP_ID, SqlDbType.Int);
                sqlParams[0].Value = referringPhysicianId;
                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_OC_EXTERNAL_ID_TYPE_INFO, sqlParams);
                dtIDTypeInfo.Load(reader);
                reader.Close();
                return dtIDTypeInfo;
            }
        }

        /// <summary>
        /// Get Departments associated with OC
        /// </summary>
        /// <param name="referringphysicianID"></param>
        /// <returns></returns>
        public DataTable getDepartmentAssignmentsForOC(int referringphysicianID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(REFERRING_ID, referringphysicianID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                DataSet departmentAssignemnts = SqlHelper.ExecuteDataset(sqlConnection, CommandType.StoredProcedure, SP_GET_DEPT_ASSIGNMENTSFOROC, objSqlParameter);
                return departmentAssignemnts.Tables[0];
            }
        }

        /// <summary>
        /// Update Department Assignment 
        /// </summary>
        /// <param name="departmentID"></param>
        /// <param name="referringPhysicianID"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public int UpdateAssignment(int departmentAssignID, int departmentID, int referringPhysicianID, DateTime startDateTime, DateTime endDateTime)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[6];

                objSqlParameter[0] = new SqlParameter(DEPT_ASSIGN_ID, SqlDbType.Int);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[0].Value = departmentAssignID;


                objSqlParameter[1] = new SqlParameter(DEPTARTMENT_ID, SqlDbType.Int);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[1].Value = departmentID;

                objSqlParameter[2] = new SqlParameter(REFERRING_ID, SqlDbType.Int);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[2].Value = referringPhysicianID;

                objSqlParameter[3] = new SqlParameter(START_DATETIME, SqlDbType.DateTime);
                objSqlParameter[3].Direction = ParameterDirection.Input;
                if (startDateTime == DateTime.MinValue)
                    objSqlParameter[3].Value = null;
                else
                    objSqlParameter[3].Value = startDateTime;

                objSqlParameter[4] = new SqlParameter(END_DATETIME, SqlDbType.DateTime);
                objSqlParameter[4].Direction = ParameterDirection.Input;
                if (endDateTime == DateTime.MinValue)
                    objSqlParameter[4].Value = null;
                else
                    objSqlParameter[4].Value = endDateTime;

                objSqlParameter[5] = new SqlParameter(RETURN_VAL, SqlDbType.Int);
                objSqlParameter[5].Direction = ParameterDirection.Output;

                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_UPDATE_DEPT_ASSIGNMENT, objSqlParameter);
            }
        }

        /// <summary>
        /// Insert new department assignment
        /// </summary>
        /// <param name="departmentID"></param>
        /// <param name="referringPhysicianID"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public int AddNewAssignment(int departmentID, int referringPhysicianID, DateTime startDateTime, DateTime endDateTime)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[5];

                objSqlParameter[0] = new SqlParameter(DEPTARTMENT_ID, SqlDbType.Int);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[0].Value = departmentID;

                objSqlParameter[1] = new SqlParameter(REFERRING_ID, SqlDbType.Int);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[1].Value = referringPhysicianID;

                objSqlParameter[2] = new SqlParameter(START_DATETIME, SqlDbType.DateTime);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                if (startDateTime == DateTime.MinValue)
                    objSqlParameter[2].Value = null;
                else
                    objSqlParameter[2].Value = startDateTime;
                objSqlParameter[3] = new SqlParameter(END_DATETIME, SqlDbType.DateTime);
                objSqlParameter[3].Direction = ParameterDirection.Input;
                if (endDateTime == DateTime.MinValue)
                    objSqlParameter[3].Value = null;
                else
                    objSqlParameter[3].Value = endDateTime;
                objSqlParameter[4] = new SqlParameter(DEPT_ASSIGN_ID, SqlDbType.Int);
                objSqlParameter[4].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_INSERT_DEPT_ASSIGNMENT, objSqlParameter);
                return int.Parse(objSqlParameter[4].Value.ToString());

            }
        }

        /// <summary>
        /// Delete Assignment
        /// </summary>
        /// <param name="departmentAssignID"></param>
        /// <returns></returns>
        public int DeleteAssignment(int departmentAssignID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[2];

                objSqlParameter[0] = new SqlParameter(DEPT_ASSIGN_ID, SqlDbType.Int);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[0].Value = departmentAssignID;

                objSqlParameter[1] = new SqlParameter(IS_DELETED, SqlDbType.Bit);
                objSqlParameter[1].Direction = ParameterDirection.Output;
                objSqlParameter[1].Value = 0;

                object result = SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_DELETE_DEPT_ASSIGNMENT, objSqlParameter);
                if (bool.Parse(objSqlParameter[1].Value.ToString()) == false)
                    return 0;
                return 1;
            }
        }
    }
}
