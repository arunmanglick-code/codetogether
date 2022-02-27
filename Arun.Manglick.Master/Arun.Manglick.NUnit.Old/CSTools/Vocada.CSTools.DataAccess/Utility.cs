#region File History
/******************************File History***************************
 * File Name        : Utility.cs
 * Author           : Prerak Shah.
 * Created Date     : 04-07-2007
 * Purpose          : This Class is used to contain common functions.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * ------------------------------------------------------------------- 
 * 03-12-2007       Prerak   Function Added for getOpenConnection for SQL.
 * 03-20-2008       SSK      Added new methods GetNewPinForMessageRetrieve, CheckDuplicatePINForUser
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Vocada.VoiceLink.DataAccess;
using System.Configuration;

namespace Vocada.CSTools.DataAccess
{
    public class Utility
    {
        #region Stored Procedure Constants
        private const string SP_GET_PRACTICETYPES = "dbo.getPracticeTypes";
        /// <summary>
        /// This constant stores name of stored procedure which will get the information of institution from Database
        /// </summary>
        private const string SP_GET_INSTITUTION = "dbo.getInstitutions";
        /// <summary>
        /// This constant stores name of stored procedure which will get the information of TimeZone from Database
        /// </summary>
        private const string SP_GET_TIMEZONE = "dbo.getTimeZones";
        /// <summary>
        /// This constant stores name of stored procedure which will Directories from database
        /// </summary>
        private const string SP_GET_DIRECTORIES = "dbo.getDirectoriesForInstitution";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive Groups for Institution from database
        /// </summary>
        private const string SP_GET_GROUPS_BY_INSTITUTION = "dbo.VOC_CST_getGroupListByInstitute";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive uniqoue PIN for Message
        /// </summary>
        private const string SP_GET_UNIQUE_PIN_FOR_USER = "dbo.VOC_CST_getUniquePINForUser";
        /// <summary>
        /// This constant stores name of stored procedure which will Validate for duplicate pin for OC, Unit and CT
        /// </summary>
        private const string SP_CHECK_DUPLICATE_PIN_FOR_USER = "dbo.VOC_CST_checkDuplicatePinForUser";

        #endregion

        #region Parameter Constants
        /// <summary>
        /// This constant stores name of parameter for Institute ID
        /// </summary>
        private const string INSTITUTE_ID = "@insID";
        /// <summary>
        /// This constant stores name of parameter for Institute ID
        /// </summary>
        private const string INSTITUTION_ID = "@institutionID";

        /// <summary>
        /// This constant stores name of parameter for Directory Name
        /// </summary>
        private const string DIRECTORY_NAME = "@directoryName";

        /// <summary>
        /// This constant stores name of parameter for Directory Name
        /// </summary>
        private const string NURSE_DIRECTORY_ID = "@nurseDirectoryID";

        /// <summary>
        /// This constant stores name of output parameter for Result
        /// </summary>
        private const string RESULT = "@result";

        /// <summary>
        /// This constant stores PIN for user to access the message
        /// </summary>
        private const string PIN = "@pin";
        #endregion

        #region Public Methods
        /// <summary>
        /// Get new Pin number from database
        /// </summary>
        /// <returns></returns>
        public static string GetNewPin(int subscriberID)
        {
            string pinNumber = "";
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlCommand sqlCommand = new SqlCommand("getUniquePassword", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add("@subscriberID", SqlDbType.Int).Value = subscriberID;
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.Read())
                {
                    pinNumber = sqlDataReader["Password"].ToString();

                }
                sqlDataReader.Close();
            }
            return pinNumber;

        }

        /// <summary>
        /// Get new Pin number from database for Message Retrieval
        /// </summary>
        /// <param name="institutionID">int</param>
        /// <returns>string</returns>
        public static string GetNewPinForMessageRetrieve(int institutionID)
        {
            string pinNumber = "";
            SqlDataReader reader = null;
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(INSTITUTION_ID, institutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_UNIQUE_PIN_FOR_USER, objSqlParameter);
                if (reader.Read())
                {
                    pinNumber = reader["PIN"].ToString();

                }
                reader.Close();
            }
            return pinNumber;

        }
        /// <summary>
        /// Get current time accoring to institutation's Zone
        /// </summary>
        /// <param name="institutationID"></param>
        /// <returns></returns>
        public static DateTime GetInstituteZoneTime(int institutationID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                string sqlCommandText = "SELECT Dbo.fnVO_VD_getDateByUsersTime(GETDATE(), " +
                                         "(SELECT TimeZoneID FROM INSTITUTIONS WHERE InstitutionID = " + institutationID + "))";
                SqlCommand sqlCommand = new SqlCommand(sqlCommandText, sqlConnection);
                sqlCommand.CommandType = CommandType.Text;
                return (DateTime)sqlCommand.ExecuteScalar();
            }
        }
    
        /// <summary>
        /// Get application Version Description
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public static DataTable getApplicationVersion(string moduleName)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                string GET_VERSION = "dbo.VOC_VL_getVersion";
                string MODULE_NAME = "@moduleName";
                DataTable dtApplicationInfo = new DataTable();
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(MODULE_NAME, moduleName);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drVersion = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, GET_VERSION, objSqlParameter);
                //Veriphy ReleaseNumber  (VD BuildNum.Veriphy Build Num) 
                //e.g. Veriphy 1.08 (103.100)               
                dtApplicationInfo.Load(drVersion);
                drVersion.Close();
                return dtApplicationInfo;
            }
        }

        public static DataTable GetPracticeType()
        {
            SqlDataReader reader = null;
            DataTable dt = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_PRACTICETYPES);
                dt.Load(reader);
                return dt;
            }
        }
        /// <summary>
        /// This Method returns Data table contaioning Directories of particular Institution.
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns></returns>
        public static DataTable GetDirectories(int InstitutionID)
        {
            SqlDataReader reader = null;
            DataTable dtDirectory = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(INSTITUTE_ID, InstitutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_DIRECTORIES, objSqlParameter);
                dtDirectory.Load(reader);
               
                return dtDirectory;
            }
        }

     
        public static DataTable GetInstitutionList()
        {
            SqlDataReader reader = null;
            DataTable dtInstitution = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_INSTITUTION);
                dtInstitution.Load(reader);
                return dtInstitution;
            }
        }
        /// <summary>
        /// This function Gets all the TimeZones.
        /// </summary>
        /// <param name="subscriberID" type="int"></param>
        public static DataTable GetTimeZone()
        {
            SqlDataReader reader = null;
            DataTable dtTimeZone = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_TIMEZONE);
                dtTimeZone.Load(reader);
                return dtTimeZone;
            }
        }
        /// <summary>
        /// This Method returns Data table contaioning Directories of particular Institution.
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns></returns>
        public static DataTable GetGroups(int InstitutionID)
        {
            SqlDataReader reader = null;
            DataTable dtGroups = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(INSTITUTION_ID, InstitutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;


                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_GROUPS_BY_INSTITUTION, objSqlParameter);
                dtGroups.Load(reader);
                return dtGroups;
            }
        }

        /// <summary>
        /// Checks for duplicate PIN for Message Retrieve
        /// </summary>
        /// <param name="instituteID">int</param>
        /// <param name="pin">int</param>
        /// <returns>bool</returns>
        public static bool CheckDuplicatePINForUser(int instituteID, string pin)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] arrParams = new SqlParameter[2];

                arrParams[0] = new SqlParameter(INSTITUTION_ID, SqlDbType.Int);
                arrParams[0].Value = instituteID;

                arrParams[1] = new SqlParameter(PIN, SqlDbType.VarChar, 5);
                arrParams[1].Value = pin;

                SqlDataReader duplicateUserID = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_CHECK_DUPLICATE_PIN_FOR_USER, arrParams);
                if (duplicateUserID.Read())
                {
                    if (Convert.ToInt32(duplicateUserID["RowCnt"]) > 0)
                        return true;
                }

                return false;
            }
        }

        #endregion
        #region Database connection Functions
        public static SqlConnection getOpenConnection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationSettings.AppSettings["connectionString"];
            conn.Open();
            return conn;
        }

          #endregion Database connection Functions

    }
}
