#region File History

/******************************File History***************************
 * File Name        : Vocada.CSTools.DataAccess/UserManagement.cs
 * Author           : Suhas Tarihalkar
 * Created Date     : 21-Feb-08
 * Purpose          : User Management for CSTools Users
 *                  : 
 *                  
 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 *                          
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
    /// <summary>
    /// This class is responsible for insert/update CSTool user information 
    /// </summary>
    public class UserManagement
    {

        #region  Stored Procedures
        /// <summary>
        /// SP for gettting the current users Information
        /// </summary>
        private const string SP_GET_CSTOOLS_USERS = "dbo.VOC_CST_getUsersInformation";
        /// <summary>
        /// SP for getting the CSTools User Roles
        /// </summary>
        private const string SP_GET_CSTOOLS_ROLES = "dbo.VOC_CST_getCSToolsRoles";
        /// <summary>
        /// SP for updating User Information
        /// </summary>
        private const string SP_UPDATE_USER_INFO = "dbo.VOC_CST_updateUsersInformation";
        /// <summary>
        /// SP for inserting User Information
        /// </summary>
        private const string SP_INSERT_USER_INFO = "dbo.VOC_CST_insertUsersInformation";
        /// <summary>
        /// SP for checking duplicate Pin(Password)
        /// </summary>
        private const string SP_CHECK_DUPLICATE_PIN = "dbo.VOC_CST_checkDuplicatePin";
        /// <summary>
        /// SP for getting unique password for user.
        /// </summary>
        private const string SP_GET_UNIQUE_PASSWORD = "dbo.VOC_CST_getUniquePassword";
        #endregion

        #region Private Members
        private const string USER_ID = "@vocUserID";
        private const string LOGIN_ID = "@loginID";
        private const string PASSWORD = "@password";

        private const string VOC_USER_ID = "@VOCUserID";
        private const string ROLE_ID = "@roleId";
        private const string STATUS = "@status";
        private const string FIRST_NAME = "@firstName";
        private const string LAST_NAME = "@lastName";
        private const string EMAIL = "@email";
        private const string PHONE = "@phone";
       
        #endregion Private Members

        #region Public Methods

        /// <summary>
        /// Gets the CSTools users information.
        /// </summary>
        /// <returns></returns>
        public DataTable GetUsersInformation()
        {
            SqlDataReader reader = null;
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtUserInfo = new DataTable("GroupInfo");
                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_CSTOOLS_USERS, null);

                dtUserInfo.Load(reader);
                reader.Close();
                return dtUserInfo;
            }
        }

        /// <summary>
        /// Gets Roles 
        /// </summary>
        /// <returns>DataTable containing Role id and description</returns>
        public DataTable GetRoles()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtRoles = new DataTable();
                SqlDataReader drRoles = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_CSTOOLS_ROLES);
                dtRoles.Load(drRoles);
                return dtRoles;
            }
        }

        /// <summary>
        /// Updates the user information.
        /// </summary>
        /// <returns></returns>
        public void UpdateUserInformation(CSTUserInformation objUserInfo)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[9];

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

                SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_UPDATE_USER_INFO, sqlParams);
            }
        }

        public void InsertUserInformation(CSTUserInformation objUserInfo)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[8];

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

                SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_INSERT_USER_INFO, sqlParams);
            }
        }


        /// <summary>
        /// Checks for duplicate PIN for CSTools Users
        /// </summary>
        /// <param name="nurseIDNumber"></param>
        /// <returns></returns>
        public bool CheckDuplicatePIN(int vocUserID, string loginID, string password)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] arrParams = new SqlParameter[3];

                arrParams[0] = new SqlParameter(USER_ID, SqlDbType.Int);
                arrParams[0].Value = vocUserID;

                arrParams[1] = new SqlParameter(LOGIN_ID, SqlDbType.VarChar, 10);
                arrParams[1].Value = loginID;

                arrParams[2] = new SqlParameter(PASSWORD, SqlDbType.VarChar, 10);
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
        /// Get new Pin number from database for CSTools User
        /// </summary>
        /// <returns></returns>
        public string GetNewPin(string loginID)
        {
            string pinNumber = "";
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] arrParams = new SqlParameter[1];

                arrParams[0] = new SqlParameter(LOGIN_ID, SqlDbType.VarChar,40);
                arrParams[0].Value = loginID;

                SqlDataReader newPin = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_UNIQUE_PASSWORD, arrParams);
                if (newPin.Read())
                {
                    pinNumber = newPin["Password"].ToString();
                }
            }
            return pinNumber;
        }

        #endregion Private Methods
    }
}
