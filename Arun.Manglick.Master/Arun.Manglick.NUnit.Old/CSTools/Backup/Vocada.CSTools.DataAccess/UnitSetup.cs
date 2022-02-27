#region File History

/******************************File History***************************
 * File Name        : UnitSetup.cs
 * Author           : 
 * Created Date     : 30 Jan 07
 * Purpose          : Add, Edit , delete Unit Records in Database.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer   Reason of Modification
 * 01-24-2008       Suhas       Copied from Veriphy Web for CR # 126 - Forwarding in CS Tool 
 * ------------------------------------------------------------------- 
 
 */
#endregion


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Vocada.VoiceLink.DataAccess;

namespace Vocada.CSTools.DataAccess
{
    /// <summary>
    /// This Class will manipulate Unit records
    /// </summary>
    public class UnitSetup
    {
        #region  Constants
        /// <summary>
        /// This constant stores name of stored procedure which will retrive all units from Database
        /// </summary>
        private const string SP_GET_UNITS = "dbo.Voc_VL_getNursingUnitsForInstitution";
        /// <summary>
        /// This constant stores name of stored procedure which will add new units in Database
        /// </summary>
        private const string SP_INSERT_UNIT = "dbo.VOC_VL_InsertLabUnit";
        /// <summary>
        /// This constant stores name of stored procedure which will update given unit with new values in Database
        /// </summary>
        private const string SP_DELETE_UNIT = "dbo.VOC_VL_DeleteLabUnit";
        /// <summary>
        /// This constant stores name of stored procedure In parameter 
        /// </summary>
        private const string UNIT_ID = "@UnitID";
        /// <summary>
        /// This constant stores name of stored procedure In parameter 
        /// </summary>
        private const string UNIT_NAME = "@UnitName";
        /// <summary>
        /// This constant stores name of stored procedure Out parameter 
        /// </summary>
        private const string INSTITUTE_ID = "@InstitutionID";
        /// <summary>
        /// This constant stores name of stored procedure Out parameter 
        /// </summary>
        private const string ACTIVE_UNITS = "@NeedActiveUnitsOnly";
        /// <summary>
        /// This constant stores name of stored procedure Out parameter 
        /// </summary>
        private const string PHONE_NUMBER = "@PhoneNumber";
        #endregion  Constants

        #region Public Methods

        /// <summary>
        /// Get Units for a given subscriber
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetUnits(int institutionID, bool needActiveUnitsOnly)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtUnitData = new DataTable("Units");
                
                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(INSTITUTE_ID, institutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                objSqlParameter[1] = new SqlParameter(ACTIVE_UNITS, SqlDbType.Bit);
                if (needActiveUnitsOnly)
                    objSqlParameter[1].Value = 1;
                else
                    objSqlParameter[1].Value = 0;
                objSqlParameter[1].Direction = ParameterDirection.Input;

                SqlDataReader drUnits = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_UNITS, objSqlParameter);

                dtUnitData.Load(drUnits);
                drUnits.Close();
                return dtUnitData;
            }
        }
       

        /// <summary>
        /// Add New unit
        /// </summary>
        /// <param name="institutionID"></param>
        /// <param name="unitName"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public int AddNew(int institutionID, string unitName, string phoneNumber)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[3];

                objSqlParameter[0] = new SqlParameter(INSTITUTE_ID, SqlDbType.Int);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[0].Value = institutionID;

                objSqlParameter[1] = new SqlParameter(UNIT_NAME, SqlDbType.VarChar);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[1].Value = unitName;

                objSqlParameter[2] = new SqlParameter(PHONE_NUMBER, SqlDbType.VarChar);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[2].Value = phoneNumber;
                
                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_INSERT_UNIT, objSqlParameter);
            }
        }

        /// <summary>
        /// Delete Unit.
        /// Return -1 : If delete failed, dependant record associated with the unit
        /// </summary>
        /// <param name="unitID"></param>
        /// <returns></returns>
        public int Delete(int unitID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];

                objSqlParameter[0] = new SqlParameter(UNIT_ID, SqlDbType.Int);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[0].Value = unitID;

                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_DELETE_UNIT, objSqlParameter);
            }
        }

        #endregion Public Methods
    }
}
