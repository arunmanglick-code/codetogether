#region File History

/******************************File History***************************
 * File Name        : Subscriber.cs
 * Author           : Prerak Shah.
 * Created Date     : 07-08-2007
 * Purpose          : This Class will provide Add, Update and fetching methos for subscriber in Database.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 
 * ------------------------------------------------------------------- 
 * 03-12-2007   IAK     Method updated AddSubscriber
 * 03-12-2007 - Prerak  Call getOpenConnection function from Utility Class.
 * 03-12-2007 - IAK     Function modified GetUserRoles, UpdateSpecialistInfo.
 * 05-12-2007 - IAK     Function modified GetUserRoles
 *  09-01-2007 - IAK    User Crendtial validation added. UpdateUserInfo method updated
 * ------------------------------------------------------------------- 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Vocada.CSTools.Common;
using System.Data.SqlClient;
using Vocada.VoiceLink.DataAccess;
using System.Data;


namespace Vocada.CSTools.DataAccess
{
    public class Subscriber
    {
        #region Constants Parameter Names
        private const string SUBSCRIBER_ID = "@subscriberId";
        private const string GROUP_ID = "@groupID";
        private const string LOGIN_ID = "@loginID";
        private const string PASSWORD = "@password";
        private const string ROLE_ID = "@roleId";
        private const string ACTIVE = "@isActive";
        private const string FIRST_NAME = "@firstName";
        private const string LAST_NAME = "@lastName";
        private const string NICK_NAME = "@nickName";
        private const string PRIMARY_EMAIL = "@primaryEmail";
        private const string PRIMARY_EMAIL_NOTIFY = "@primaryEmailNotify";
        private const string PRIMARY_PHONE = "@primaryPhone";
        private const string FAX = "@fax";
        private const string FAX_NOTIFY = "@faxNotify";
        private const string LAST_UPDATED = "@lastUpdated";
        private const string CELL_PHONE = "@cellphone";
        private const string CELL_PHONE_CARRIER = "@cellphoneCarrier";
        private const string PAGER = "@pager";
        private const string PAGER_CARRIER = "@pagerCarrier";

        private const string AFFILIATION = "@affiliation";
        private const string SPECIALIST_ID = "@specialistID";
        private const string SPECIALTY = "@specialty";
        private const string VOICE_OVER_URL = "@voiceOverURL";

        private const string REPORT_ON_DAYS = "@reportOnDays";
        private const string REPORT_AT_HOUR = "@reportAtHour";
        private const string REPORT_VIA_EMAIL = "@reportViaEmail";
        private const string REPORT_VIA_FAX = "@reportViaFax";
        private const string NUM_OF_DAYS = "@numberOfDays";
        private const string USER_TYPE = "@userType";

        #endregion

        #region  Constants Store Procedures Name
        /// <summary>
        /// This constant stores name of stored procedure which will Add Subscriber  
        /// </summary>
        private const string SP_INSERT_SUBSCRIBER = "dbo.VOC_CST_insertSubscriber";
        /// <summary>
        /// This constant stores name of stored procedure which will Update Subscriber 
        /// </summary>
        private const string SP_UPDATE_SUBSCRIBER = "dbo.VOC_CST_updateSubscriber";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive Subscriber information from Database
        /// </summary>
        private const string SP_GET_SUBSCRIBER_INFO = "dbo.VOC_CST_getSubscriberInfoBySubscriberID";
        /// <summary>
        /// This constant stores name of stored procedure which will update specialist info in the database
        /// </summary>
        private const string SP_UPDATE_SPECIALIST = "dbo.VOC_CST_updateSpecialistInfo";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive group findings from Database
        /// </summary>
        private const string SP_GENERATE_PIN = "dbo.getUniquePasswordForGroup";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive group users from database
        /// </summary>
        private const string SP_GET_GROUP_USERS = "dbo.VOC_CST_getGroupUsers";
        /// <summary>
        /// This constant stores name of stored procedure which will cell phone carriers from database
        /// </summary>
        private const string SP_GET_CELL_CARRIERS = "dbo.VOC_CST_getCellPhoneCarriers";
        /// <summary>
        /// This constant stores name of stored procedure which will pager carriers from database
        /// </summary>
        private const string SP_GET_PAGER_CARRIERS = "dbo.VOC_CST_getPagerCarriers";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive report setting for user
        /// </summary>
        private const string SP_GET_REPORT_SETTING = "dbo.VOC_CST_getSubscriberReportSettings";
        /// <summary>
        /// This constant stores name of stored procedure which will update Subscriber Information
        /// </summary>
        private const string SP_UPDATE_SUBSCRIBER_INFO = "dbo.VOC_CST_updateSubscriberInformation";
        /// <summary>
        /// This constant stores name of stored procedure which will insert Subscriber Specialist Info in the Database
        /// </summary>
        private const string SP_INSERT_SPECIALIST_INFO = "dbo.VOC_CST_insertSpecialistInfo";
        /// <summary>
        /// This constant stores name of stored procedure which will update Subscriber Specialist Info in the Database
        /// </summary>
        private const string SP_UPDATE_SPECIALIST_INFO = "dbo.VOC_CST_updateSpecialistInfo";
        /// <summary>
        /// This constant stores name of stored procedure which will update Subscriber's Report Settings.
        /// </summary>
        private const string SP_UPDATE_REPORT_SETTINGS = "dbo.VOC_CST_updateSubscriberReportSetting";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive User Configuration Data For Subscriber 
        /// </summary>
        private const string SP_GET_USER_CONFIG_INFO = "dbo.VOC_CST_getUserConfigurationDataForSubscriber";
        /// <summary>
        /// This constant stores name of stored procedure which will insert User Configuration Data For Subscriber 
        /// </summary>
        private const string SP_INSERT_USER_CONFIG_INFO = "dbo.VOC_CST_insertUserConfigurationDataForSubscriber";
        /// <summary>
        /// This constant stores name of stored procedure which will update User Configuration Data For Subscriber 
        /// </summary>
        private const string SP_UPDATE_USER_CONFIG_INFO = "dbo.VOC_CST_updateUserConfigurationDataForSubscriber";
        /// <summary>
        /// This constant stores name of stored procedure which will retrieve roles for user. 
        /// </summary>
        private const string SP_GET_USER_ROLES = "dbo.VOC_CST_getUserRoles";
        /// <summary>
        /// This constant store the name of stored procedure which retrive user information
        /// </summary>
        private const string GET_USER_INFO_BY_LOGIN = "dbo.VOC_CST_getSubscriberInformationByLogin";

        #endregion Constants

        #region Public Methods
        /// <summary>
        /// This method Adds Subscriber information into the database
        /// </summary>
        /// <param name="SubscriberInformation"></param>
        /// <returns></returns>
        public int AddSubscriber(SubscriberInformation objSubscriberInfo)
        {
            SqlConnection conn = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = conn.BeginTransaction();
            try
            {
                int subscriberID = 0;
                SqlParameter[] arParams = new SqlParameter[16];

                //Create parameter for GROUP ID
                arParams[0] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                arParams[0].Value = objSubscriberInfo.GroupID;

                //Create parameter for LOGIN ID
                arParams[1] = new SqlParameter(LOGIN_ID, SqlDbType.VarChar);
                arParams[1].Value = objSubscriberInfo.LoginID;

                //Create parameter for PASSWORD
                arParams[2] = new SqlParameter(PASSWORD, SqlDbType.VarChar);
                arParams[2].Value = objSubscriberInfo.Password;

                //Create parameter for ROLE ID
                arParams[3] = new SqlParameter(ROLE_ID, SqlDbType.Int);
                arParams[3].Value = objSubscriberInfo.RoleID;

                //Create parameter for ACTIVE
                arParams[4] = new SqlParameter(ACTIVE, SqlDbType.Bit);
                arParams[4].Value = objSubscriberInfo.Active;

                //Create parameter for FIRST Name
                arParams[5] = new SqlParameter(FIRST_NAME, SqlDbType.VarChar);
                arParams[5].Value = objSubscriberInfo.FirstName;

                //Create parameter for LAST NAME
                arParams[6] = new SqlParameter(LAST_NAME, SqlDbType.VarChar);
                arParams[6].Value = objSubscriberInfo.LastName;

                //Create parameter for NICK NAME
                arParams[7] = new SqlParameter(NICK_NAME, SqlDbType.VarChar);
                arParams[7].Value = objSubscriberInfo.NickName;

                //Create parameter for PRIMARY EMAIL
                arParams[8] = new SqlParameter(PRIMARY_EMAIL, SqlDbType.VarChar);
                arParams[8].Value = objSubscriberInfo.PrimaryEmail;

                //Create parameter for PRIMARY EMAIL NOTIFY
                arParams[9] = new SqlParameter(PRIMARY_EMAIL_NOTIFY, SqlDbType.Int);
                arParams[9].Value = objSubscriberInfo.PrimaryEmailNotify;

                //Create parameter for PRIMARY PHONE                                    
                arParams[10] = new SqlParameter(PRIMARY_PHONE, SqlDbType.VarChar);
                arParams[10].Value = objSubscriberInfo.PrimaryPhone;

                //Create parameter for FAX
                arParams[11] = new SqlParameter(FAX, SqlDbType.VarChar);
                arParams[11].Value = objSubscriberInfo.Fax;

                //Create parameter for FAX NOTIFY
                arParams[12] = new SqlParameter(FAX_NOTIFY, SqlDbType.Int);
                arParams[12].Value = objSubscriberInfo.FaxNotify;

                arParams[13] = new SqlParameter(SUBSCRIBER_ID, SqlDbType.Int);
                arParams[13].Direction = ParameterDirection.Output;
                arParams[13].Value = subscriberID;

                arParams[14] = new SqlParameter(SPECIALTY, SqlDbType.VarChar);
                arParams[14].Direction = ParameterDirection.Input;
                arParams[14].Value = objSubscriberInfo.Specialty;

                arParams[15] = new SqlParameter(AFFILIATION, SqlDbType.VarChar);
                arParams[15].Direction = ParameterDirection.Input;
                arParams[15].Value = objSubscriberInfo.Affiliation;

                SqlHelper.ExecuteScalar(sqlTransaction, CommandType.StoredProcedure, SP_INSERT_SUBSCRIBER, arParams);
                subscriberID = Convert.ToInt32(arParams[13].Value);

                sqlTransaction.Commit();
                conn.Close();
                return subscriberID;
            }
            catch (SqlException sqlError)
            {
                sqlTransaction.Rollback();
                conn.Close();
                throw;
            }
        }
        /// <summary>
        /// This method Adds Subscriber information into the database
        /// </summary>
        /// <param name="SubscriberInformation"></param>
        /// <returns></returns>
        public int UpdateSpecialist(SubscriberInformation objSubscriberInfo)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlConnection conn = Utility.getOpenConnection();
                SqlTransaction sqlTransaction;
                sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    int specialistID = 0;
                    SqlParameter[] arParams = new SqlParameter[5];

                    //Create parameter for SUBSCRIBER ID
                    arParams[0] = new SqlParameter(SUBSCRIBER_ID, SqlDbType.Int);
                    arParams[0].Value = objSubscriberInfo.SubscriberID;

                    //Create parameter for SPECIALTY
                    arParams[1] = new SqlParameter(SPECIALTY, SqlDbType.VarChar);
                    arParams[1].Value = objSubscriberInfo.Specialty;

                    //Create parameter for AFFILIATION
                    arParams[2] = new SqlParameter(AFFILIATION, SqlDbType.VarChar);
                    arParams[2].Value = objSubscriberInfo.Affiliation;

                    //Create parameter for VOICE OVER URL 
                    arParams[3] = new SqlParameter(VOICE_OVER_URL, SqlDbType.VarChar);
                    arParams[3].Value = objSubscriberInfo.VoiceOverURL;

                    arParams[4] = new SqlParameter(SPECIALIST_ID, SqlDbType.Int);
                    arParams[4].Direction = ParameterDirection.Output;
                    //arParams[4].Value = specialistID;


                    SqlHelper.ExecuteScalar(sqlTransaction, CommandType.StoredProcedure, SP_UPDATE_SPECIALIST, arParams);
                    specialistID = Convert.ToInt32(arParams[4].Value);

                    sqlTransaction.Commit();
                    sqlConnection.Close();
                    return specialistID;
                }
                catch (SqlException sqlError)
                {
                    sqlTransaction.Rollback();
                    sqlConnection.Close();
                    throw;
                }
            }
        }
        /// <summary>
        /// This Method Generates unique Pin/Password for subscriber
        /// </summary>
        /// <param name="groupID">groupID</param>
        /// <returns></returns>
        public string GeneratePin(int groupID)
        {
            string pin = "";
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drSubPin = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GENERATE_PIN, objSqlParameter);

                if (drSubPin.Read())
                {
                    pin = drSubPin["Password"].ToString();
                }
                return pin;
            }
        }
        /// <summary>
        /// This Method Gets all Cell Phone Carriers
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public DataTable GetCellCarriers()
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtCellCarriers = new DataTable("CellPhoneCarriers");

                SqlDataReader drCellCarriers = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_CELL_CARRIERS);

                dtCellCarriers.Load(drCellCarriers);
                drCellCarriers.Close();
                return dtCellCarriers;
            }
        }
        /// <summary>
        /// This Method Gets all Cell Phone Carriers
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public DataTable GetPagerCarriers()
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtPagerCarriers = new DataTable("PagerCarriers");

                SqlDataReader drPagerCarriers = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_PAGER_CARRIERS);

                dtPagerCarriers.Load(drPagerCarriers);
                drPagerCarriers.Close();
                return dtPagerCarriers;
            }
        }

        /// <summary>
        /// Get Different roles for user
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserRoles(bool labUser)
        {
            int userType = 1;
            if (labUser)
                userType = 2;

            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtRoles = new DataTable("Roles");

                SqlParameter[] objSqlParameter = new SqlParameter[1];

                
                objSqlParameter[0] = new SqlParameter(USER_TYPE, userType);
                
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drRoles = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_USER_ROLES, objSqlParameter);

                dtRoles.Load(drRoles);
                drRoles.Close();
                return dtRoles;
            }
        }
        /// <summary>
        /// Get User information for a given subscriber
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetUserInfo(int subscriberID, int roleID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtSubscriberInfo = new DataTable("SubscriberInfo");

                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                objSqlParameter[1] = new SqlParameter(ROLE_ID, roleID);
                objSqlParameter[1].Direction = ParameterDirection.Input;

                SqlDataReader drSubscriberInfo = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_SUBSCRIBER_INFO, objSqlParameter);

                dtSubscriberInfo.Load(drSubscriberInfo);
                drSubscriberInfo.Close();
                return dtSubscriberInfo;
            }
        }
        /// <summary>
        /// Get Report Setting for a given subscriber/User
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetReportSettings(int subscriberID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtReportSettings = new DataTable("ReportSettings");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drReportSettings = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_REPORT_SETTING, objSqlParameter);

                dtReportSettings.Load(drReportSettings);
                drReportSettings.Close();
                return dtReportSettings;
            }
        }
        /// <summary>
        /// Update subscriber information
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public int UpdateUserInfo(int subscriberID, int roleID, bool isActive, string firstName, string nickName, string lastName,
            string loginID, string password, string primaryEmail, string primaryPhone, string fax, string primaryEmailNotify, string FaxNotify)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {

                SqlParameter[] objSqlParameter = new SqlParameter[13];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.InputOutput;
                objSqlParameter[1] = new SqlParameter(ROLE_ID, roleID);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(ACTIVE, isActive);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[3] = new SqlParameter(FIRST_NAME, firstName);
                objSqlParameter[3].Direction = ParameterDirection.Input;
                objSqlParameter[4] = new SqlParameter(NICK_NAME, nickName);
                objSqlParameter[4].Direction = ParameterDirection.Input;
                objSqlParameter[5] = new SqlParameter(LAST_NAME, lastName);
                objSqlParameter[5].Direction = ParameterDirection.Input;
                objSqlParameter[6] = new SqlParameter(LOGIN_ID, loginID);
                objSqlParameter[6].Direction = ParameterDirection.Input;
                objSqlParameter[7] = new SqlParameter(PASSWORD, password);
                objSqlParameter[7].Direction = ParameterDirection.Input;
                objSqlParameter[8] = new SqlParameter(PRIMARY_EMAIL, primaryEmail);
                objSqlParameter[8].Direction = ParameterDirection.Input;
                objSqlParameter[9] = new SqlParameter(PRIMARY_EMAIL_NOTIFY, primaryEmailNotify);
                objSqlParameter[9].Direction = ParameterDirection.Input;
                objSqlParameter[10] = new SqlParameter(PRIMARY_PHONE, primaryPhone);
                objSqlParameter[10].Direction = ParameterDirection.Input;
                objSqlParameter[11] = new SqlParameter(FAX_NOTIFY, FaxNotify);
                objSqlParameter[11].Direction = ParameterDirection.Input;
                objSqlParameter[12] = new SqlParameter(FAX, fax);
                objSqlParameter[12].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_UPDATE_SUBSCRIBER_INFO, objSqlParameter);
                return int.Parse(objSqlParameter[0].Value.ToString());
            }
        }
        /// <summary>
        /// Insert Specialist Information for subscriber
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <param name="specialty"></param>
        /// <param name="affiliation"></param>
        public void InsertSpecialistInfo(int subscriberID, string specialty, string affiliation)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[3];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(SPECIALTY, specialty);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(AFFILIATION, affiliation);
                objSqlParameter[2].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_INSERT_SPECIALIST_INFO, objSqlParameter);
            }
        }
        /// <summary>
        /// Update Specialist Information of subscriber
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <param name="specialty"></param>
        /// <param name="affiliation"></param>
        public void UpdateSpecialistInfo(int SubscriberID, string specialty, string affiliation)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[4];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, SubscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(SPECIALTY, specialty);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(AFFILIATION, affiliation);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[3] = new SqlParameter(SPECIALIST_ID, 0);
                objSqlParameter[3].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_UPDATE_SPECIALIST_INFO, objSqlParameter);
            }
        }
        /// <summary>
        /// Update Report Settings
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <param name="reportOnDays"></param>
        /// <param name="reportAtHour"></param>
        /// <param name="reportViaEmail"></param>
        /// <param name="reportViaFax"></param>
        public void updateReportSetting(int subscriberID, int reportOnDays, int reportAtHour, bool reportViaEmail, bool reportViaFax)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[5];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(REPORT_ON_DAYS, reportOnDays);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(REPORT_AT_HOUR, reportAtHour);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[3] = new SqlParameter(REPORT_VIA_EMAIL, reportViaEmail);
                objSqlParameter[3].Direction = ParameterDirection.Input;
                objSqlParameter[4] = new SqlParameter(REPORT_VIA_FAX, reportViaFax);
                objSqlParameter[4].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_UPDATE_REPORT_SETTINGS, objSqlParameter);
            }
        }
        /// <summary>
        /// Get User Configuration Information for a given subscriber
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <returns></returns>
        public DataTable GetUserConfigurationInfo(int subscriberID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtUserConfigInfo = new DataTable("UserConfigInfo");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drUserConfigInfo = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_USER_CONFIG_INFO, objSqlParameter);

                dtUserConfigInfo.Load(drUserConfigInfo);
                drUserConfigInfo.Close();
                return dtUserConfigInfo;
            }
        }
        /// <summary>
        /// Insert User Configuration Information for a given subscriber
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <returns></returns>
        public void InsertUserConfigurationInfo(int subscriberID, int numOfDays)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(NUM_OF_DAYS, numOfDays);
                objSqlParameter[1].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_INSERT_USER_CONFIG_INFO, objSqlParameter);
            }
        }
        /// <summary>
        /// Update User Configuration Information for a given subscriber
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <param name="numOfDays"></param>
        public void UpdateUserConfigurationInfo(int subscriberID, int numOfDays)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(NUM_OF_DAYS, numOfDays);
                objSqlParameter[1].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_UPDATE_USER_CONFIG_INFO, objSqlParameter);
            }
        }

        /// <summary>
        /// Get Subscriber login information using login id and password
        /// </summary>
        /// <param name="loginID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public SubscriberInformation GetSubscriberInformation(string loginID, string password)
        {
            SubscriberInformation objSubscriberInfo;
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(LOGIN_ID, loginID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                objSqlParameter[1] = new SqlParameter(PASSWORD, password);
                objSqlParameter[1].Direction = ParameterDirection.Input;

                SqlDataReader drUserInfo = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, GET_USER_INFO_BY_LOGIN, objSqlParameter);

                objSubscriberInfo = new SubscriberInformation();
                while (drUserInfo.Read())
                {
                    //objSubscriberInfo.VocUserID = drUserInfo.GetInt32(drUserInfo.GetOrdinal("VOCUserID"));
                    objSubscriberInfo.RoleID = drUserInfo.GetInt32(drUserInfo.GetOrdinal("RoleID"));
                    //objSubscriberInfo.RoleDescription = drUserInfo.GetString(drUserInfo.GetOrdinal("RoleDescription"));
                    objSubscriberInfo.SpecialistID = drUserInfo.GetInt32(drUserInfo.GetOrdinal("SpecialistID"));
                    objSubscriberInfo.SubscriberID = drUserInfo.GetInt32(drUserInfo.GetOrdinal("SubscriberID"));
                    objSubscriberInfo.GroupID = drUserInfo.GetInt32(drUserInfo.GetOrdinal("GroupID"));
                    //objSubscriberInfo.DirectoryID = drUserInfo.GetInt32(drUserInfo.GetOrdinal("DirectoryID"));
                    //objSubscriberInfo.InstitutionID = drUserInfo.GetInt32(drUserInfo.GetOrdinal("InstitutionID"));
                    //objSubscriberInfo.InstitutionName = drUserInfo.GetString(drUserInfo.GetOrdinal("InstitutionName"));
                    objSubscriberInfo.FirstName = drUserInfo.GetString(drUserInfo.GetOrdinal("FirstName"));
                    objSubscriberInfo.LastName = drUserInfo.GetString(drUserInfo.GetOrdinal("LastName"));
                    //objSubscriberInfo.AllowDownload = drUserInfo.GetBoolean(drUserInfo.GetOrdinal("AllowDownload")) ? 1 : 0;
                }
                drUserInfo.Close();
            }
            return objSubscriberInfo;
        }
        #endregion
    }
}
