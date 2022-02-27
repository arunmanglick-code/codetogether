#region File History
/******************************File History***************************
 * File Name        : Login.cs
 * Author           : Prerak Shah.
 * Created Date     : 22-08-2007
 * Purpose          : This Class is used to contain Login functions.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 03-12-2007 - Prerak - Call getOpenConnection function from Utility Class.
 * 11-07-2008   SNK      Remember Me functionality
 * 13-11-2008   IAK      Defect: 3534 User Name = First Name + Last Name
 * ------------------------------------------------------------------- 
 */

#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Vocada.VoiceLink.DataAccess;
using System.Data;
using Vocada.CSTools.Common;

namespace Vocada.CSTools.DataAccess
{
    public class Login
    {
        #region Stored Procedure Constants
        /// <summary>
        /// This constant stores name of stored procedure which will retrive Groups for Institution from database
        /// </summary>
        private const string SP_VALIDATE_LOGIN = "dbo.VOC_CST_ValidateUserIDPassword";

        
        /// <summary>
        /// 
        /// </summary>
        private const string SP_VALIDATE_USERID = "dbo.VOC_CST_ValidateUserID";

        #endregion Stored Procedure Constants
        #region Parameter Constants
        /// <summary>
        /// This constant stores name of output parameter for userID
        /// </summary>
        private const string LOGIN = "@login";
        /// <summary>
        /// This constant stores name of output parameter for Password
        /// </summary>
        private const string PASSWORD = "@password";

        private const string USER_ID = "@userId";
        
        #endregion Parameter Constants

        #region Public Method
        /// <summary>
        /// Validate Login.
       /// </summary>
       /// <param name="useID">Login ID</param>
       /// <param name="password">Password</param>
       /// <returns></returns>
        public UserInfo ValidateLogin(string useID, string password)
        {
            SqlDataReader reader = null;
            UserInfo userInfo = new UserInfo();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {                
                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(LOGIN, useID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(PASSWORD, password);
                objSqlParameter[1].Direction = ParameterDirection.Input;


                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_VALIDATE_LOGIN, objSqlParameter);
                if (reader.HasRows)
                {
                    reader.Read();
                    userInfo.UserID = reader.GetInt32(reader.GetOrdinal("VOCUserID"));
                    userInfo.RoleId = reader.GetInt32(reader.GetOrdinal("RoleID"));
                    userInfo.InstitutionID = reader.GetInt32(reader.GetOrdinal("InstitutionID"));
                    userInfo.UserName = reader.GetString(reader.GetOrdinal("FirstName")) + " " + reader.GetString(reader.GetOrdinal("LastName"));
                    userInfo.SubUserID = reader.GetInt32(reader.GetOrdinal("UserID"));
                    userInfo.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                    userInfo.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                    userInfo.RoleDescription = reader.GetString(reader.GetOrdinal("RoleDescription"));
                    userInfo.InstitutionName = reader.GetString(reader.GetOrdinal("InstitutionName"));
                    userInfo.AllowSystemAdminTab = reader.GetInt32(reader.GetOrdinal("AllowSystemAdminTab")) == 1 ? true : false;
                    userInfo.LoginID = useID;
                }
                return userInfo;
            }

        }

        public UserInfo ValidateLogin(int userID)
        {
            SqlDataReader reader = null;
            UserInfo userInfo =  new UserInfo();

            using(SqlConnection cnx = Utility.getOpenConnection())
            {                
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(USER_ID, userID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                
                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_VALIDATE_USERID, objSqlParameter);
                if(reader.HasRows)
                {
                    reader.Read();

                    userInfo.UserID = reader.GetInt32(reader.GetOrdinal("VOCUserID"));
                    userInfo.RoleId = reader.GetInt32(reader.GetOrdinal("RoleID"));
                    userInfo.InstitutionID = reader.GetInt32(reader.GetOrdinal("InstitutionID"));
                    userInfo.UserName = reader.GetString(reader.GetOrdinal("FirstName")) + " " + reader.GetString(reader.GetOrdinal("LastName"));
                    userInfo.SubUserID = reader.GetInt32(reader.GetOrdinal("UserID"));
                    userInfo.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                    userInfo.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                    userInfo.RoleDescription = reader.GetString(reader.GetOrdinal("RoleDescription"));
                    userInfo.InstitutionName = reader.GetString(reader.GetOrdinal("InstitutionName"));
                    userInfo.AllowSystemAdminTab = reader.GetInt32(reader.GetOrdinal("AllowSystemAdminTab")) == 1 ? true : false;
                    userInfo.LoginID = reader.GetString(reader.GetOrdinal("LoginID")).Trim();
                }
                return userInfo;
            }
        }
        #endregion Public Methods
    }
}
