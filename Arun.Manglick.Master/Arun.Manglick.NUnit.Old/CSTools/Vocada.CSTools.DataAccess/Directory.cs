#region File History

/******************************File History***************************
 * File Name        : Directory.cs
 * Author           : IAK
 * Created Date     : 21 July 07
 * Purpose          : Add, Edit Directory Records in Database.
 *                  : 

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 03-12-2007 - Prerak - Call getOpenConnection function from Utility Class.
 * 13 Nov 2007  Prerak - Defect #3637 Performance issue solved.
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
    public class Directory
    {
        #region  Constants
        /// <summary>
        /// This constant stores name of stored procedure which will retrive all units from Database
        /// </summary>
        private const string SP_GET_DIRS = "dbo.VOC_CST_getDirectoriesForInstitution";
        /// <summary>
        /// This constant stores name of stored procedure which will add new units in Database
        /// </summary>
        private const string SP_INSERT_DIR = "dbo.VOC_CST_insertDirectory";
        /// <summary>
        /// This constant stores name of stored procedure which will update given unit with new values in Database
        /// </summary>
        private const string SP_UPDATE_DIR = "dbo.VOC_CST_updateDirectory";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive Nurse Directories for Institution from database
        /// </summary>
        private const string SP_GET_NURSE_DIRECTORIES = "dbo.VOC_CST_getNurseDirectoriesForInstitution";
        /// <summary>
        /// This constant stores name of stored procedure which will add new Nurse Directory for Institution
        /// </summary>
        private const string SP_ADD_NURSE_DIRECTORIES = "dbo.VOC_CST_insertNurseDirectoryForInstitution";
        /// <summary>
        /// This constant stores name of stored procedure which will update Nurse Directory information of the selected directory
        /// </summary>
        private const string SP_UPDATE_NURSE_DIRECTORIES = "dbo.VOC_CST_updateNurseDirectoryForInstitution";

        /// <summary>
        /// SP for VOC_VLR_getDirectoryPhysiciansByStartingWith
        /// </summary>
        private const string SP_GET_PHYSICIANS_BY_STARTING_WITH = "dbo.VOC_VLR_getDirectoryPhysiciansByStartingWith";

        /// <summary>
        /// SP for VOC_VLR_getDirectoryPhysiciansBySearchTerm
        /// </summary>
        private const string SP_GET_PHYSICIANS_BY_SEARCH_TERM = "dbo.VOC_VLR_getDirectoryPhysiciansBySearchTerm";

        /// <summary>
        /// SP for getReferringPhysicianByID
        /// </summary>
        private const string SP_GET_REFERRING_PHYSICIANS_BY_ID = "dbo.VOC_VLR_getDirectoryPhysiciansByRPID";

        /// <summary>
        /// This constant stores name of stored procedure In parameter 
        /// </summary>
        private const string DIRECTORY_NAME = "@directoryName";
        /// <summary>
        /// This constant stores name of stored procedure Out parameter 
        /// </summary>
        private const string INSTITUTE_ID = "@institutionID";
        /// <summary>
        /// This constant stores name of stored procedure Out parameter 
        /// </summary>
        private const string DIR_ID = "@directoryID";
        /// <summary>
        /// This constant stores name of stored procedure Out parameter 
        /// </summary>
        private const string RETURN_VAL = "@returnVal";
        /// <summary>
        /// This constant stores name of parameter for Directory Name
        /// </summary>
        private const string NURSE_DIRECTORY_ID = "@nurseDirectoryID";
        /// <summary>
        /// This constant stores name of output parameter for Result
        /// </summary>
        private const string RESULT = "@result";        

        #endregion  Constants
        #region Private Variables
        private const string SUBSCRIBER_ID = "@subscriberID";
        private const string DIRECTORY_ID = "@directoryID";
        private const string STARTING_WITH = "@startingWith";
        private const string SEARCH_TERM = "@searchTerm";
        private const string REFERRING_PHYSICIAN_ID = "@referringPhysicianID";
        private const string GROUP_ID = "@groupID";
        private const string DISPLAY_ORDER = "@nameDisplayStyle";
        #endregion

        #region Public Methods

        /// <summary>
        /// Get Directories for a given institution
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetDirectories(int institutionID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtDirectoryData = new DataTable("Directory");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(INSTITUTE_ID, institutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drDirectories = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_DIRS, objSqlParameter);

                dtDirectoryData.Load(drDirectories);
                drDirectories.Close();
                return dtDirectoryData;
            }
        }
        
        /// <summary>
        /// Add new Directory
        /// </summary>
        /// <param name="institutionID"></param>
        /// <param name="DirectoryName"></param>
        /// <returns></returns>
        public int AddNew(int institutionID, string directoryName)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[3];

                objSqlParameter[0] = new SqlParameter(INSTITUTE_ID, SqlDbType.Int);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[0].Value = institutionID;

                objSqlParameter[1] = new SqlParameter(DIRECTORY_NAME, SqlDbType.VarChar);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[1].Value = directoryName;

                objSqlParameter[2] = new SqlParameter(RETURN_VAL, SqlDbType.Int);
                objSqlParameter[2].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_INSERT_DIR, objSqlParameter);
                return int.Parse(objSqlParameter[2].Value.ToString());
            }
        }

        /// <summary>
        /// Update Directory Name
        /// </summary>
        /// <param name="institutionID"></param>
        /// <param name="DirectoryID"></param>
        /// <param name="DirectoryName"></param>
        /// <returns></returns>
        public int Update(int institutionID, int directoryID, string directoryName)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[4];

                objSqlParameter[0] = new SqlParameter(INSTITUTE_ID, SqlDbType.Int);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[0].Value = institutionID;

                objSqlParameter[1] = new SqlParameter(DIR_ID, SqlDbType.Int);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[1].Value = directoryID;

                objSqlParameter[2] = new SqlParameter(DIRECTORY_NAME, SqlDbType.VarChar);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[2].Value = directoryName;

                objSqlParameter[3] = new SqlParameter(RETURN_VAL, SqlDbType.Int);
                objSqlParameter[3].Direction = ParameterDirection.Output;
              

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_UPDATE_DIR, objSqlParameter);
                return int.Parse(objSqlParameter[3].Value.ToString());
            }
        }
        /// <summary>
        /// This Method inserts Nurse Directories for particular Institution.
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns></returns>
        public int InsertNurseDirectoryForInstitute(int institutionID, string directoryName)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[3];
                objSqlParameter[0] = new SqlParameter(INSTITUTE_ID, institutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(DIRECTORY_NAME, directoryName);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(RESULT, SqlDbType.Int);
                objSqlParameter[2].Direction = ParameterDirection.Output;

                int result = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_ADD_NURSE_DIRECTORIES, objSqlParameter);
                return Convert.ToInt32(objSqlParameter[2].Value);
            }
        }
        /// <summary>
        /// This Method updates Nurse Directory information for the selected directory
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns></returns>
        public int UpdateNurseDirectoryForInstitute(int institutionID, int nurseDirectoryId, string directoryName)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[4];
                objSqlParameter[0] = new SqlParameter(INSTITUTE_ID, institutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(NURSE_DIRECTORY_ID, nurseDirectoryId);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(DIRECTORY_NAME, directoryName);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[3] = new SqlParameter(RESULT, SqlDbType.Int);
                objSqlParameter[3].Direction = ParameterDirection.Output;

                int result = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_UPDATE_NURSE_DIRECTORIES, objSqlParameter);
                return Convert.ToInt32(objSqlParameter[3].Value);
            }
        }
        /// <summary>
        /// This Method returns Data table contaioning Nurse Directories of particular Institution.
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns></returns>
        public DataTable GetNurseDirectories(int InstitutionID)
        {
            SqlDataReader reader = null;
            DataTable dtDirectory = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(INSTITUTE_ID, InstitutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;


                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_NURSE_DIRECTORIES, objSqlParameter);
                dtDirectory.Load(reader);
                return dtDirectory;
            }
        }

        #endregion Public Methods
        #region Public Methods for Directory Maintenance

        /// <summary>
        /// This method will call "VOC_VLR_getDirectoryPhysiciansByStartingWith" stored procedure passing directoryID and StartingWith as parameter
        /// it will return Clinicains for given parameters. 
        /// </summary>
        /// <param name="cnx"></param>
        public DataTable PopulatePhysiciansByStartingWith(int directoryID, string startingWith)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataSet dsPhysicians;
                DataTable dtPhysicians = new DataTable();
                SqlParameter[] arSqlParams = new SqlParameter[2];

                arSqlParams[0] = new SqlParameter(DIRECTORY_ID, SqlDbType.Int);
                arSqlParams[0].Value = directoryID;
                arSqlParams[1] = new SqlParameter(STARTING_WITH, SqlDbType.Char, 1);
                arSqlParams[1].Value = startingWith;
                dsPhysicians = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, SP_GET_PHYSICIANS_BY_STARTING_WITH, arSqlParams);
                dtPhysicians = dsPhysicians.Tables[0].Copy();
                return dtPhysicians;
            }
        }

        /// <summary>
        /// This method will call "VOC_VLR_getDirectoryPhysiciansBySearchTerm" stored procedure passing directoryID and Searchterm as parameter
        /// it will return Clinicains for given parameters. 
        /// </summary>
        /// <param name="cnx"></param>
        public DataTable PopulatePhysiciansBySearchTerm(int directoryID, string searchTerm)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtPhysicians = new DataTable();
                DataSet dsPhysicians;
                SqlParameter[] arSqlParams = new SqlParameter[2];

                arSqlParams[0] = new SqlParameter(DIRECTORY_ID, SqlDbType.Int);
                arSqlParams[0].Value = directoryID;
                arSqlParams[1] = new SqlParameter(SEARCH_TERM, SqlDbType.VarChar, searchTerm.Length + 1);
                arSqlParams[1].Value = searchTerm;
                dsPhysicians = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, SP_GET_PHYSICIANS_BY_SEARCH_TERM, arSqlParams);
                dtPhysicians = dsPhysicians.Tables[0].Copy();
                return dtPhysicians;
            }
        }

        /// <summary>
        /// This method will call "getReferringPhysicianByID" stored procedure passing referringPhysicianID as parameter
        /// it will return Clinicains for given parameters. 
        /// </summary>
        /// <param name="cnx"></param>
        public DataTable GetReferringPhysicianByID(int referringPhysicianID, int directoryID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataSet dsPhysicians;
                DataTable dtPhysicians = new DataTable();
                SqlParameter[] arSqlParams = new SqlParameter[2];

                arSqlParams[0] = new SqlParameter(REFERRING_PHYSICIAN_ID, SqlDbType.Int);
                arSqlParams[0].Value = referringPhysicianID;

                arSqlParams[1] = new SqlParameter(DIRECTORY_ID, SqlDbType.Int);
                arSqlParams[1].Value = directoryID;
                dsPhysicians = SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, SP_GET_REFERRING_PHYSICIANS_BY_ID, arSqlParams);
                dtPhysicians = dsPhysicians.Tables[0].Copy();
                return dtPhysicians;
            }
        }

        #endregion
    }
}
