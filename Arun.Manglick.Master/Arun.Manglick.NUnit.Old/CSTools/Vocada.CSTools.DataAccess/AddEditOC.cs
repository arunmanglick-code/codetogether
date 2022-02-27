#region File History

/******************************File History***************************
 * File Name        : Vocada.Veriphy.BusinessClasses/AddEditOC.cs
 * Author           : Swapnil K
 * Created Date     : 27-Mar-07
 * Purpose          : To take care all Database transactions for the tab AddEditOC.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification
    04-27-2007   IAK     Support for multiple group to add event
 *                       New Methods: GetGroupsForOC(), GetFindingForOCorGroup()
 *                       Updated Methods: InsertOCNotifications()  
 * 06-21-2007    IAK     Enhnacement 28, 29 30
 * 06-25-2007    IAK     Department and Resident field added, 
 *                       Change Routine: InsertOrderingClinician,  UpdateOrderingClinician
 * 10-15-2007    SSK     Updated InsertOrderingClinician, UpdateOrderingClinician methods to save LoginId and Password for OC. Added method CheckDuplicateOCPIN
 * 12-13-2007    SSK     Update  CheckDuplicateOCPIN, UpdateOrderingClinician, InsertOrderingClinician
 * 03-20-2008    SSK     "PIN for Message Retrieval" changes
 * 03-26-2008    IAK     Added External information while adding new OC
 * ------------------------------------------------------------------- 
 *                
 */
#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Text;
using Vocada.VoiceLink.DataAccess;
using System.Data;
using System.Data.SqlClient;
using Vocada.CSTools.Common;
using Vocada.VoiceLink;
#endregion

namespace Vocada.CSTools.DataAccess
{
    /// <summary>
    /// Class to take care business logic for Add and Edit OC pages
    /// </summary>
    public class AddEditOC
    {
        #region Stored Procedures
        /// <summary>
        /// This constant stores name of stored procedure which will retrive all departments from Database
        /// </summary>
        private const string SP_GET_DEPTS = "dbo.VOC_DEPT_GetDepartments";
        /// <summary>
        /// SP for insertReferringPhysician
        /// </summary>
        private const string SP_INSERT_REFERRING_PHYSICIAN = "dbo.VOC_CST_insertReferringPhysician";

        /// <summary>
        /// SP for updateReferringPhysician
        /// </summary>
        private const string SP_UPDATE_REFERRING_PHYSICIAN = "dbo.VOC_CST_updateOCReferringPhysician";

        /// <summary>
        /// SP for insertRPDevice
        /// </summary>
        private const string SP_INSERT_OC_DEVICE = "dbo.VOC_VL_insertOCDevice";

        /// <summary>
        /// SP for getRPDevices
        /// </summary>
        private const string SP_GET_OC_DEVICE = "dbo.getRPDevices";

        /// <summary>
        /// SP for getRPNotifications
        /// </summary>
        private const string SP_GET_OC_NOTIFICATIONS = "dbo.VOC_VW_getRPNotifications";

        /// <summary>
        /// SP for getDevices
        /// </summary>
        private const string SP_GET_DEVICES = "dbo.getDevices";

        /// <summary>
        /// SP for VOC_VLR_deleteRPDevice
        /// </summary>
        private const string SP_DELETE_OCDEVICE = "dbo.VOC_VLR_deleteRPDevice";

        /// <summary>
        /// SP for deleteRPNotification
        /// </summary>
        private const string SP_DELETE_OCNOTIFICATION = "dbo.deleteRPNotification";

        /// <summary>
        /// SP for updateRPDevice
        /// </summary>
        private const string SP_UPDATE_OC_DEVICE = "dbo.VOC_VL_updateRPDevice";

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
        private const string SP_GET_FINDINGS_USING_OC_GROUP = "dbo.VOC_VW_getFindingsUsingOCGroup";

        /// <summary>
        /// SP for getRPAfterHoursDevices
        /// </summary>
        private const string SP_GET_AFTER_HOURS_NOTIFICATIONS = "dbo.VOC_VW_getRPAfterHoursNotifications";

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
        /// This constant stores name of stored procedure which will Validate for duplicate pin for OC
        /// </summary>
        private const string SP_CHECK_DUPLICATE_OC_PIN = "dbo.VOC_VW_checkDuplicateOCPin";

       
        #endregion

        #region Private Variables
        private const string SUBSCRIBER_ID = "@subscriberID";
        private const string GROUP_ID = "@groupID";
        private const string DIRECTORY_ID = "@directoryID";
        private const string FIRST_NAME = "@firstName";
        private const string LAST_NAME = "@lastName";
        private const string NICK_NAME = "@nickname";
        private const string PRIMARY_PHONE = "@primaryPhone";
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
        private const string REF_ID = "@refId";
        private const string ADDITIONAL_CONT_NAME = "@additionalContName";
        private const string ADDITIONAL_CONT_PHONE = "@additionalContPhone";
        private const string UPDATED_BY = "@updatedBy";
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
        private const string OC_ID = "@ocID";
        private const string RET_VAL = "@returnValue";
        private const string OC_AFTER_NOTIFICATION_ID = "@rpAfterHoursNotificationID";
        private const string OC_NOTIFY_EVENT_ID = "@rpNotifyEventID";
        private const string START_HOUR = "@startHour";
        private const string END_HOUR = "@endHour";
        private const string RESULT = "@result";
        private const string FINDING_NAME = "@findingName";
        private const string RADIOLOGY_TDR = "@radiologyTDR";
        private const string LAB_TDR = "@labTDR";
        private const string NOTES = "@notes";
        private const string IS_RESIDENT = "@isResident";
        private const string DEPARTMENT_ID = "@departmentID";
        private const string PROFILE_UPDATED_ON = "@profileCompletedOn";
        private const string START_DATE = "@startDateTime";
        private const string END_DATE = "@endDateTime";
        private const string DEPT_ASSIGN_ID = "@departmentAssignID";
        private const string INSTITUTE_ID = "@institutionID";        
        private const string LOGIN_ID = "@loginID";
        private const string PASSWORD = "@password";
        private const string USER_ID = "@userID";
        private const string VOC_USER_ID = "@vocUserID";
        private const string INITIAL_PAUSE = "@initialPause";
        private const string ED_DOC = "@edDoc";
        /* Additional fields required by Third party implementations */
        private const string RIS_ID = "@risid";
        private const string LIS_ID = "@lisid";
        private const string MSO_ID = "@msoid";
        private const string NPI = "@npi";
        private const string PIN = "@pin";


        #endregion

        /// <summary>
        /// Inserts the Ordering Clinician details in the database.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public int InsertOrderingClinician(OrderingClinicianInfo objOCInfo)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[36];

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
                if(objOCInfo.DepartmentEndDate == DateTime.MinValue)
                    sqlParams[27].Value = null;
                else
                    sqlParams[27].Value = objOCInfo.DepartmentEndDate;

                sqlParams[28] = new SqlParameter(LOGIN_ID, SqlDbType.VarChar);
                if(objOCInfo.LoginID.Length > 0)
                    sqlParams[28].Value = objOCInfo.LoginID;
                else
                    sqlParams[28].Value = DBNull.Value;


                sqlParams[29] = new SqlParameter(PASSWORD, SqlDbType.VarChar);
                if(objOCInfo.Password.Length > 0)
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

                sqlParams[32] = new SqlParameter(RIS_ID, SqlDbType.VarChar);
                sqlParams[32].Value = objOCInfo.RIS_ID;

                sqlParams[33] = new SqlParameter(LIS_ID, SqlDbType.VarChar);
                sqlParams[33].Value = objOCInfo.LIS_ID;

                sqlParams[34] = new SqlParameter(MSO_ID, SqlDbType.VarChar);
                sqlParams[34].Value = objOCInfo.MSO_ID;

                sqlParams[35] = new SqlParameter(NPI, SqlDbType.VarChar);
                sqlParams[35].Value = objOCInfo.NPI;

                object result = SqlHelper.ExecuteScalar(cnx, CommandType.StoredProcedure, SP_INSERT_REFERRING_PHYSICIAN, sqlParams);
                int ocID = 0;
                if (result != null)
                    ocID = Convert.ToInt32(result);

                return ocID;
            }
        }

        /// <summary>
        /// Updates the Ordering Clinician details in the database.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public int UpdateOrderingClinician(OrderingClinicianInfo objOCInfo)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[40];

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

                sqlParams[30] = new SqlParameter(LOGIN_ID, SqlDbType.VarChar);
                if(objOCInfo.LoginID.Length > 0)
                    sqlParams[30].Value = objOCInfo.LoginID;
                else
                    sqlParams[30].Value = DBNull.Value;

                sqlParams[31] = new SqlParameter(PASSWORD, SqlDbType.VarChar);                

                if(objOCInfo.Password.Length > 0)
                    sqlParams[31].Value = objOCInfo.Password;
                else
                    sqlParams[31].Value = DBNull.Value;

                sqlParams[32] = new SqlParameter(VOC_USER_ID, SqlDbType.Int);
                sqlParams[32].Value = objOCInfo.VOCUserID;

                sqlParams[33] = new SqlParameter(INSTITUTE_ID, SqlDbType.Int);                
                sqlParams[33].Value = objOCInfo.InstituteID;

                sqlParams[34] = new SqlParameter(ED_DOC, SqlDbType.Bit);
                sqlParams[34].Value = objOCInfo.IsEDDoc;

                sqlParams[35] = new SqlParameter(RIS_ID, SqlDbType.VarChar);
                if (objOCInfo.IDRIS.Length > 0)
                    sqlParams[35].Value = objOCInfo.IDRIS;
                else
                    sqlParams[35].Value = DBNull.Value;

                sqlParams[36] = new SqlParameter(LIS_ID, SqlDbType.VarChar);
                if (objOCInfo.IDLIS.Length > 0)
                    sqlParams[36].Value = objOCInfo.IDLIS;
                else
                    sqlParams[36].Value = DBNull.Value;

                sqlParams[37] = new SqlParameter(MSO_ID, SqlDbType.VarChar);
                if (objOCInfo.IDMSO.Length > 0)
                    sqlParams[37].Value = objOCInfo.IDMSO;
                else
                    sqlParams[37].Value = DBNull.Value;

                sqlParams[38] = new SqlParameter(NPI, SqlDbType.VarChar);
                if (objOCInfo.IDNPI.Length > 0)
                    sqlParams[38].Value = objOCInfo.IDNPI;
                else
                    sqlParams[38].Value = DBNull.Value;
                
                sqlParams[39] = new SqlParameter(PIN, SqlDbType.VarChar, 5);
                if (objOCInfo.PINForMessageRetrieve.Length > 0)
                    sqlParams[39].Value = objOCInfo.PINForMessageRetrieve;
                else
                    sqlParams[39].Value = DBNull.Value;

                SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_UPDATE_REFERRING_PHYSICIAN, sqlParams);
                return int.Parse(sqlParams[26].Value.ToString());
            }
        }

        /// <summary>
        /// Get Departments for an institution
        /// </summary>
        /// <param name="institutionID"></param>
        /// <returns></returns>
        public DataTable GetDepartments(int institutionID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtDeptData = new DataTable("Depts");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(INSTITUTE_ID, institutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drDepts = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_DEPTS, objSqlParameter);

                dtDeptData.Load(drDepts);
                drDepts.Close();
                return dtDeptData;
            }
        }


        /// <summary>
        /// Inserts the Ordering Clinician device in the database.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public int InsertOCDevice(OCDeviceInfo objDevice)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[6];

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
                //if (objDevice.InitialPauseTime == "-1")
                    sqlParams[5].Value = DBNull.Value;
                //else
                //    sqlParams[5].Value = objDevice.InitialPauseTime;

                int result = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_INSERT_OC_DEVICE, sqlParams);
                return result;
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

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_OC_DEVICE, sqlParams);
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

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_DEVICES);
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
        public int DeleteDevices(int ocDeviceID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter(OCDEVICE_ID, SqlDbType.Int);
                sqlParams[0].Value = ocDeviceID;

                int result = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_DELETE_OCDEVICE, sqlParams);
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
                SqlParameter[] sqlParams = new SqlParameter[7];

                sqlParams[0] = new SqlParameter(OCDEVICE_ID, SqlDbType.Int);
                sqlParams[0].Value = objDevice.OCDeviceID;

                sqlParams[1] = new SqlParameter(DEVICE_NAME, SqlDbType.VarChar, 50);
                sqlParams[1].Value = objDevice.DeviceName;

                sqlParams[2] = new SqlParameter(DEVICE_ADDRESS, SqlDbType.VarChar, 100);
                sqlParams[2].Value = objDevice.DeviceAddress;

                sqlParams[3] = new SqlParameter(GATEWAY, SqlDbType.VarChar, 100);
                sqlParams[3].Value = objDevice.Gateway;

                //Create parameter for OCID
                sqlParams[4] = new SqlParameter(RP_ID, SqlDbType.Int);
                sqlParams[4].Value = objDevice.ReferringPhysicianID;

                sqlParams[5] = new SqlParameter(INITIAL_PAUSE, SqlDbType.Decimal);
                //if (objDevice.InitialPauseTime == "-1")
                    sqlParams[5].Value = DBNull.Value;
                //else
                //    sqlParams[5].Value = objDevice.InitialPauseTime;

                sqlParams[6] = new SqlParameter(RESULT, SqlDbType.Int);
                sqlParams[6].Direction = ParameterDirection.Output;
                int result = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_UPDATE_OC_DEVICE, sqlParams);
                return result;
            }
        }

        /// <summary>
        /// Deletes selected device notifications for Ordering Clinician.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public int DeleteOCNotifications(int ocNotificationID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter(OCNOTIFICATION_ID, SqlDbType.Int);
                sqlParams[0].Value = ocNotificationID;
                int result = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_DELETE_OCNOTIFICATION, sqlParams);
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

                sqlParams[0] = new SqlParameter(OC_ID, SqlDbType.Int);
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
        public bool InsertOCNotifications(OCDeviceInfo objDevice, int ocID, int groupID, string findingName)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtFindings = null;
                SqlParameter[] sqlParams = null;
                //SqlTransaction sqlTransaction = null;
                try
                {
                    /*sqlParams = new SqlParameter[3];
                    sqlParams[0] = new SqlParameter(OC_ID, SqlDbType.Int);
                    sqlParams[0].Value = ocID;

                    sqlParams[1] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                    sqlParams[1].Value = groupID;

                    sqlParams[2] = new SqlParameter(FINDING_NAME, SqlDbType.VarChar);
                    sqlParams[2].Value = findingName;

                    SqlDataReader drFindings = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_FINDINGS_USING_OGF, sqlParams);
                    dtFindings = new DataTable();
                    dtFindings.Load(drFindings);

                    sqlTransaction = cnx.BeginTransaction(IsolationLevel.ReadCommitted);
                    for (int currentFinding = 0; currentFinding < dtFindings.Rows.Count; currentFinding++)
                    {*/

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
                    //}
                    //sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    //sqlTransaction.Rollback();
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
        public bool InsertAfterHoursNotifications(OCDeviceInfo objDevice, int ocID, int groupID, string findingName)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtFindings = null;
                SqlParameter[] sqlParams = null;
                //SqlTransaction sqlTransaction = null;
                try
                {
                    /*sqlParams = new SqlParameter[3];
                    sqlParams[0] = new SqlParameter(OC_ID, SqlDbType.Int);
                    sqlParams[0].Value = ocID;

                    sqlParams[1] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                    sqlParams[1].Value = groupID;

                    sqlParams[2] = new SqlParameter(FINDING_NAME, SqlDbType.VarChar);
                    sqlParams[2].Value = findingName;

                    SqlDataReader drFindings = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_FINDINGS_USING_OGF, sqlParams);
                    dtFindings = new DataTable();
                    dtFindings.Load(drFindings);

                    sqlTransaction = cnx.BeginTransaction(IsolationLevel.ReadCommitted);
                    for (int currentFinding = 0; currentFinding < dtFindings.Rows.Count; currentFinding++)
                    {*/
                    sqlParams = new SqlParameter[6];

                    sqlParams[0] = new SqlParameter(RP_ID, SqlDbType.Int);
                    sqlParams[0].Value = objDevice.ReferringPhysicianID;

                    sqlParams[1] = new SqlParameter(OCDEVICE_ID, SqlDbType.Int);
                    sqlParams[1].Value = objDevice.OCDeviceID;

                    sqlParams[2] = new SqlParameter(FINDING_ID, SqlDbType.Int);
                    sqlParams[2].Value = objDevice.FindingID;

                    sqlParams[3] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                    sqlParams[3].Value = groupID;

                    sqlParams[4] = new SqlParameter(START_HOUR, SqlDbType.Int);
                    sqlParams[4].Value = objDevice.StartHour;

                    sqlParams[5] = new SqlParameter(END_HOUR, SqlDbType.Int);
                    sqlParams[5].Value = objDevice.EndHour;
                    SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_INSERT_AFTER_HOUR_NOTIFICATIONS, sqlParams);
                    //}
                    //sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    //sqlTransaction.Rollback();
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
        public DataTable GetGroupsForOC(int ocID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtGroup = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[1];

                sqlParams[0] = new SqlParameter(OC_ID, SqlDbType.Int);
                sqlParams[0].Value = ocID;

                SqlDataReader drGroups = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_GROUP_FOR_OC, sqlParams);
                dtGroup.Load(drGroups);
                drGroups.Close();
                return dtGroup;
            }
        }

        /// <summary>
        /// Checks for duplicate OC PIN
        /// </summary>
        /// <param name="nurseIDNumber"></param>
        /// <returns></returns>
        public bool CheckDuplicateOCPIN(int instituteID, string loginID, string password, int userID)
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
                if(duplicateOCID.Read())
                {
                    if(Convert.ToInt32(duplicateOCID["RowCnt"]) > 0)
                        return true;
                }

                return false;
            }
        }
        
    }
}
