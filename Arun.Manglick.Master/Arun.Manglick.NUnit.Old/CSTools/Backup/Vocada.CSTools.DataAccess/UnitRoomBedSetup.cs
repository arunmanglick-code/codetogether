    #region File History

/******************************File History***************************
 * File Name        : UnitRoomBedSetup.cs
 * Author           : 
 * Created Date     : 26 Jan 07
 * Purpose          : Add, Edit, delete Unit room Bed Records in Database.
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
    public class UnitRoomBedSetup
    {

        #region  Constants
        private const string SP_GET_UNIT_ROOM_BED_DETAILS = "dbo.VOC_VL_getLabUnitRoomBedDetails";
        private const string SP_GET_UNIT_ROOM_BED_DETAILS_FOR_INSTITUTION = "dbo.VOC_VL_getLabUnitRoomBedDetailsForInstitution";        
        private const string SP_INSERT_UNIT_ROOM_BED_DETAILS = "dbo.VOC_VL_InsertLabUnitRoomBedDetails";
        private const string SP_DELETE_UNIT_ROOM_BED_DETAILS = "dbo.VOC_VL_DeleteLabUnitRoomBedDetails";
        private const string INSTITUTE_ID = "@InstitutionID";
        private const string ROOM_BED_ID = "@RoomBedID";
        private const string UNIT_ID = "@UnitID";
        private const string ROOM_NAME = "@RoomNumber";
        private const string BED_NAME = "@BedNumber";
        private const string SUCCESS = "@Success";
        #endregion  Constants

        #region Public Methods

        /// <summary>
        /// Get Unit Room Bed Details for given institution
        /// </summary>
        /// <param name="institutionID"></param>
        /// <returns></returns>
        public DataTable GetUnitRoomBedDetailsForInstitution(int institutionID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtUnitRoomBedDetails = new DataTable("UnitRoomBedDetails");
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(INSTITUTE_ID, institutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drUnitRoomBedDetails = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_UNIT_ROOM_BED_DETAILS_FOR_INSTITUTION, objSqlParameter);

                dtUnitRoomBedDetails.Load(drUnitRoomBedDetails);
                drUnitRoomBedDetails.Close();
                return dtUnitRoomBedDetails;
            }
        }

        /// <summary>
        /// Get Room Bed Details for given unit
        /// </summary>
        /// <param name="institutionID"></param>
        /// <returns></returns>
        public DataTable GetUnitRoomBedDetailsForUnit(int unitID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtUnitRoomBedDetails = new DataTable("UnitRoomBedDetails");
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(UNIT_ID, unitID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drUnitRoomBedDetails = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_UNIT_ROOM_BED_DETAILS, objSqlParameter);

                dtUnitRoomBedDetails.Load(drUnitRoomBedDetails);
                drUnitRoomBedDetails.Close();
                return dtUnitRoomBedDetails;
            }
        }

        /// <summary>
        /// Add New Unit Room Bed Detail
        /// </summary>
        /// <param name="unitID"></param>
        /// <param name="roomName"></param>
        /// <param name="bedName"></param>
        /// <returns></returns>
        public int AddNew(int unitID, string roomName, string bedName)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[4];

                objSqlParameter[0] = new SqlParameter(UNIT_ID, SqlDbType.Int);
                objSqlParameter[0].Value = unitID;

                objSqlParameter[1] = new SqlParameter(ROOM_NAME, SqlDbType.NVarChar);
                objSqlParameter[1].Value = roomName;

                objSqlParameter[2] = new SqlParameter(BED_NAME, SqlDbType.NVarChar);
                objSqlParameter[2].Value = bedName;

                objSqlParameter[3] = new SqlParameter();
                objSqlParameter[3].Direction = ParameterDirection.ReturnValue;

                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_INSERT_UNIT_ROOM_BED_DETAILS, objSqlParameter);
            }
        }

        /// <summary>
        /// Delete UnitRoomBed Details for given RoomBedID
        /// </summary>
        /// <param name="roomBedID"></param>
        /// <returns></returns>
        public int Delete(int roomBedID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[2];

                objSqlParameter[0] = new SqlParameter(ROOM_BED_ID, SqlDbType.Int);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[0].Value = roomBedID;
                objSqlParameter[1] = new SqlParameter(SUCCESS, SqlDbType.Int);
                objSqlParameter[1].Direction = ParameterDirection.Output;

                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_DELETE_UNIT_ROOM_BED_DETAILS, objSqlParameter);
            }
        }

        #endregion Public Methods
    }
}
