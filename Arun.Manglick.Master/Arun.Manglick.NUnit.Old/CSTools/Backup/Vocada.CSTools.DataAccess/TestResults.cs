#region File History
/******************************File History***************************
 * File Name        : TestResults.cs
 * Author           : Prerak Shah
 * Created Date     : 23-08-2007
 * Purpose          : Business Class for Test Results Definition.
 *                  : 
 *                  :
 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 *   03-12-2007 - Prerak - Call getOpenConnection function from Utility Class.
 * ------------------------------------------------------------------- 
 
 */

#endregion

#region using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Vocada.VoiceLink.DataAccess;
using Vocada.CSTools.Common ;
#endregion

namespace Vocada.CSTools.DataAccess
{
    public class TestResults
    {
        #region Stored Procedures

        /// <summary>
        /// SP for VOC_VL_getTestResults
        /// </summary>
        private const string GET_TEST_RESULTS = "dbo.VOC_CST_getTestResults";

        /// <summary>
        /// SP for VOC_VL_getResultTypes
        /// </summary>
        private const string GET_RESULT_TYPES = "dbo.VOC_VL_getResultTypes";

        /// <summary>
        /// Sp for getFindingOptionsForSubscriber
        /// </summary>
        private const string GET_FINDINGS_FOR_SUBSCRIBER = "dbo.getFindingOptionsForSubscriber";

        /// <summary>
        /// SP for VOC_VL_getMeasurements
        /// </summary>
        private const string GET_MEASUREMENTS = "dbo.VOC_VL_getMeasurements";

        /// <summary>
        /// SP for VOC_VL_insertTestResults 
        /// </summary>
        private const string INSERT_TEST_RESULT = "dbo.VOC_CST_insertTestResults";

        /// <summary>
        /// SP for VOC_VL_updateTestResults
        /// </summary>
        private const string UPDATE_TEST_RESULT = "dbo.VOC_CST_updateTestResults";

        /// <summary>
        /// SP for VOC_VL_deleteTestResult
        /// </summary>
        private const string DELETE_TEST_RESULT = "dbo.VOC_CST_deleteTestResult";

        /// <summary>
        /// SP For getting unassigned test resuklts to a group.
        /// </summary>
        private const string UNASSIGNED_TEST_RESULTS = "dbo.VOC_CST_GetUnassignedLabTestForGroup";
        /// <summary>
        /// SP for VOC_VL_getLabTestArea
        /// </summary>
        private const string GET_LAB_TEST_AREA = "dbo.VOC_CST_getLabTestArea";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive Groups for 
        /// Institution from database whose GroupType is Lab
        /// </summary>
        private const string SP_GET_GROUPS_BY_INSTITUTION = "dbo.VOC_CST_getLabGroupByInstitute";

        /// <summary>
        /// SP to insert list lab test for group.
        /// </summary>
        private const string INSERT_LAB_TEST_FOR_GROUP = "dbo.VOC_CST_InsertLabTestForGroup";
        #endregion

        #region Stored Procedure Parameters
        /// <summary>
        /// Group ID
        /// </summary>
        private const string GROUP_ID = "@groupID";

        /// <summary>
        /// Lab Test ID
        /// </summary>
        private const string LABTEST_ID = "@LabTestID";

        /// <summary>
        /// Test Description (Full)
        /// </summary>
        private const string TEST_DESCRIPTION_FULL = "@testDescription";

        /// <summary>
        /// Test Description (Short)
        /// </summary>
        private const string TEST_DESCRIPTION_SHORT = "@testShortDescription";

        /// <summary>
        /// Test Area
        /// </summary>
        private const string TEST_AREA = "@testArea";

        /// <summary>
        /// Highest Possible Value
        /// </summary>
        private const string HIGHEST_POSSIBLE_VALUE = "@highLevel";

        /// <summary>
        /// Lowest Possible Value
        /// </summary>
        private const string LOWEST_POSSIBLE_VALUE = "@lowLevel";

        /// <summary>
        /// Result Type ID
        /// </summary>
        private const string RESULT_TYPE_ID = "@ResultTypeID";

        /// <summary>
        /// Measurement ID
        /// </summary>
        private const string MEASUREMENT_ID = "@MeasurementID";

        /// <summary>
        /// Default Finding ID
        /// </summary>
        private const string FINDING_ID = "@FindingID";

        /// <summary>
        /// Default Test Voice URL
        /// </summary>
        private const string TEST_VOICE_URL = "@testVoiceUrl";

        /// <summary>
        /// Default Grammar
        /// </summary>
        private const string GRAMMAR = "@grammar";

        /// <summary>
        /// Default TASK ID
        /// </summary>
        private const string TEST_ID = "@testID";
        /// <summary>
        /// parameter for Institute ID
        /// </summary>
        private const string INSTITUTION_ID = "@institutionID";

        #endregion

        #region Get Result Types

        /// <summary>
        /// This function Gets all the Result Types.
        /// This function calls stored procedure "VOC_VL_getTestResults"
        /// </summary>
        public DataTable GetResultTypes()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlDataReader reader = null;
                DataTable dtResultTypes = new DataTable();
                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, GET_RESULT_TYPES);
                dtResultTypes.Load(reader);
                return dtResultTypes;
            }
            
        }

        #endregion

        #region Get Measurements

        /// <summary>
        /// This function Gets all the Measurements.
        /// This function calls stored procedure "VOC_VL_getMeasurements"
        /// </summary>
        public DataTable GetMeasurements()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlDataReader reader = null;
                DataTable dtMeasurements = new DataTable();
   
                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, GET_MEASUREMENTS);

                dtMeasurements.Load(reader);
                return dtMeasurements;
            }
         }

        #endregion

        #region Get Findings

        /// <summary>
        /// This function Gets all the Findings.
        /// This function calls stored procedure "getFindingOptionsForSubscriber"
        /// </summary>
        /// <param name="subscriberID" type="int"></param>
        public ArrayList GetFindings(int subscriberID)
        {
            SqlDataReader reader = null;
            SqlParameter[] arParms = new SqlParameter[1];
            arParms[0] = new SqlParameter("@subscriberID", SqlDbType.Int);
            arParms[0].Value = subscriberID;

            ArrayList arrFindings = new ArrayList();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, GET_FINDINGS_FOR_SUBSCRIBER, arParms);
                if (reader.HasRows)
                {
                    while (reader.Read())
                        arrFindings.Add(new object[] { reader.GetValue(0), reader.GetValue(1) });
                }
            }
            return arrFindings;
        }

        #endregion

        #region Get Test Results

        /// <summary>
        /// This function Gets all the Test Results.
        /// This function calls stored procedure "VOC_VL_getTestResults"
        /// </summary>
        public DataSet GetTestResults(int groupID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] arParms = new SqlParameter[1];
                // @groupID Input Parameter
                arParms[0] = new SqlParameter(GROUP_ID, groupID);
                arParms[0].Direction = ParameterDirection.Input;  

                return SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, GET_TEST_RESULTS, arParms);
            }
        }

        #endregion

        #region Insert Test Result

        /// <summary>
        /// This function Adds the Test Result.
        /// This function calls stored procedure "VOC_VL_insertTestResults"
        /// </summary>
        /// <param name="testAndValues"></param>
        public int InsertTestResult(TestAndValues testAndValues)
        {
            SqlParameter[] arParms = new SqlParameter[13];
            // @testDescription Input Parameter
            arParms[0] = new SqlParameter(TEST_DESCRIPTION_FULL, SqlDbType.VarChar, 255);
            arParms[0].Value = testAndValues.FullTestName;
            // @testArea Input Parameter
            arParms[1] = new SqlParameter(TEST_AREA, SqlDbType.VarChar, 100);
            arParms[1].Value = testAndValues.TestArea;
            if (testAndValues.TestArea == null)
                arParms[1].Value = DBNull.Value;
            // @testShortDescription Input Parameter
            arParms[2] = new SqlParameter(TEST_DESCRIPTION_SHORT, SqlDbType.VarChar, 100);
            arParms[2].Value = testAndValues.ShortTestName;
            // @highLevel Input Parameter
            arParms[3] = new SqlParameter(HIGHEST_POSSIBLE_VALUE, SqlDbType.Float);
            arParms[3].Value = testAndValues.HighestValue;
            if (testAndValues.HighestValue == -1)
                arParms[3].Value = DBNull.Value;
            // @lowLevel Input Parameter
            arParms[4] = new SqlParameter(LOWEST_POSSIBLE_VALUE, SqlDbType.Float);
            arParms[4].Value = testAndValues.LowestValue;
            if (testAndValues.LowestValue == -1)
                arParms[4].Value = DBNull.Value;
            // @ResultType Input Parameter
            arParms[5] = new SqlParameter(RESULT_TYPE_ID, SqlDbType.Int);
            arParms[5].Value = testAndValues.ResultTypeID;
            // @Measurement Input Parameter
            arParms[6] = new SqlParameter(MEASUREMENT_ID, SqlDbType.Int);
            
            // @DefaultFindingID Input Parameter
            arParms[7] = new SqlParameter(FINDING_ID, SqlDbType.Int);
            arParms[7].Value = testAndValues.FindingID;

            // @groupID Input Parameter
            arParms[8] = new SqlParameter(GROUP_ID, SqlDbType.Int);
            arParms[8].Value = testAndValues.GroupID;

            // @testVoiceUrl Input Parameter
            arParms[9] = new SqlParameter(TEST_VOICE_URL, SqlDbType.VarChar);
            arParms[9].Value = testAndValues.TestVoiceURL;

            // @TestID Input Parameter
            arParms[10] = new SqlParameter(TEST_ID, SqlDbType.Int);
            arParms[10].Value = testAndValues.TestID;

            // @grammar Input Parameter
            arParms[11] = new SqlParameter(GRAMMAR, SqlDbType.VarChar);
            arParms[11].Value = testAndValues.Grammer;
            

            // @LabTestID Output Parameter
            arParms[12] = new SqlParameter(LABTEST_ID, SqlDbType.Int);
            arParms[12].Direction = ParameterDirection.Output;

            if (testAndValues.TestID == 0)
                arParms[10].Value = DBNull.Value;

            if (testAndValues.ResultTypeID != 1)
            {
                arParms[6].Value = DBNull.Value;
            }
            else
            {
                arParms[6].Value = testAndValues.MeasurementID;
                if (testAndValues.MeasurementID == -1)
                    arParms[6].Value = DBNull.Value;
            }

            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                return SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, INSERT_TEST_RESULT, arParms);
            }
        }

        #endregion

        #region Update Test Result

        /// <summary>
        /// This function Updates the Test Result.
        /// This function calls stored procedure "VOC_VL_updateTestResults"
        /// </summary>
        /// <param name="testAndValues"></param>
        public int UpdateTestResult(TestAndValues testAndValues)
        {
            SqlParameter[] arParms = new SqlParameter[10];
            
            // @testDescription Input Parameter
            arParms[0] = new SqlParameter(TEST_DESCRIPTION_FULL, SqlDbType.VarChar, 255);
            arParms[0].Value = testAndValues.FullTestName;
            // @testArea Input Parameter
            arParms[1] = new SqlParameter(TEST_AREA, SqlDbType.VarChar, 100);
            arParms[1].Value = testAndValues.TestArea;
            if (testAndValues.TestArea == null)
                arParms[1].Value = DBNull.Value;
            // @testShortDescription Input Parameter
            arParms[2] = new SqlParameter(TEST_DESCRIPTION_SHORT, SqlDbType.VarChar, 100);
            arParms[2].Value = testAndValues.ShortTestName;
            // @highLevel Input Parameter
            arParms[3] = new SqlParameter(HIGHEST_POSSIBLE_VALUE, SqlDbType.Float);
            arParms[3].Value = testAndValues.HighestValue;
            if (testAndValues.HighestValue == -1)
                arParms[3].Value = DBNull.Value;
            // @lowLevel Input Parameter
            arParms[4] = new SqlParameter(LOWEST_POSSIBLE_VALUE, SqlDbType.Float);
            arParms[4].Value = testAndValues.LowestValue;
            if (testAndValues.LowestValue == -1)
                arParms[4].Value = DBNull.Value;
            // @ResultType Input Parameter
            arParms[5] = new SqlParameter(RESULT_TYPE_ID, SqlDbType.Int);
            arParms[5].Value = testAndValues.ResultTypeID;
            // @Measurement Input Parameter
            arParms[6] = new SqlParameter(MEASUREMENT_ID, SqlDbType.Int);
            // @grammar Input Parameter
            arParms[7] = new SqlParameter(GRAMMAR, SqlDbType.VarChar );
            arParms[7].Value = testAndValues.Grammer ;
            // @LabTestID Input Parameter
            arParms[8] = new SqlParameter(LABTEST_ID, SqlDbType.Int);
            arParms[8].Value = testAndValues.LabTestID;
            // @testVoiceUrl Input Parameter
            arParms[9] = new SqlParameter(TEST_VOICE_URL, SqlDbType.VarChar);
            arParms[9].Value = testAndValues.TestVoiceURL;


            if (testAndValues.ResultTypeID != 1)
            {
                arParms[3].Value = DBNull.Value;
                arParms[4].Value = DBNull.Value;
                arParms[6].Value = DBNull.Value;
            }
            else
            {
                arParms[6].Value = testAndValues.MeasurementID;
            }
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                return SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, UPDATE_TEST_RESULT, arParms);
            }
        }

        #endregion

        #region Delete Test Result

        /// <summary>
        /// This function Deletes the Test Result.
        /// This function calls stored procedure "VOC_VL_deleteTestResult"
        /// </summary>
        /// <param name="labTestID"></param>
        public int DeleteTestResult(int labTestID)
        {
            SqlParameter[] arParms = new SqlParameter[1];
            // @LabTestID Input Parameter - Record to be Deleted
            arParms[0] = new SqlParameter(LABTEST_ID, SqlDbType.Int);
            arParms[0].Value = labTestID;

            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                return SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, DELETE_TEST_RESULT, arParms);
            }
        }

        #endregion

        /// <summary>
        /// Gets a list of all the unassigned lab tests for the group.
        /// </summary>
        /// <returns>Dataset</returns>
        public DataSet GetUnassignedLabTests(int groupId)
        {
            SqlParameter param = new SqlParameter("@groupId",SqlDbType.Int);
            param.Value = groupId;
           
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                return SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, UNASSIGNED_TEST_RESULTS,param);
            }
        }

        /// <summary>
        /// Insert labtest for a selected group from the test master list available in DB
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="testIds">string containing testids separated by ':'</param>
        public void InsertLabTestForGroup(int groupId, string testIds)
        {
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@groupId", SqlDbType.Int);
            param[0].Value = groupId;

            param[1] = new SqlParameter("@testIds", SqlDbType.VarChar);
            param[1].Value = testIds;

            using(SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlHelper.ExecuteDataset(cnx, CommandType.StoredProcedure, INSERT_LAB_TEST_FOR_GROUP, param);
            }
        }

        public DataTable GetLabTestArea(int groupID)
        {
            DataTable dtLabTestArea = new DataTable();
            SqlDataReader reader = null;
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] arParms = new SqlParameter[1];
                // @groupID Input Parameter
                arParms[0] = new SqlParameter(GROUP_ID, groupID);
                arParms[0].Direction = ParameterDirection.Input;

                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, GET_LAB_TEST_AREA, arParms);
                dtLabTestArea.Load(reader);
                return dtLabTestArea;
            }
        }
        /// <summary>
        /// This Method returns Data table contaioning Directories of particular Institution.
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns></returns>
        public DataTable GetLabGroups(int InstitutionID)
        {
            SqlDataReader reader = null;
            DataTable dtGroups = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(INSTITUTION_ID, InstitutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;


                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_GROUPS_BY_INSTITUTION, objSqlParameter);
                if (reader.HasRows)
                {
                    dtGroups.Load(reader);
                }
                return dtGroups;
            }
        }
    }
}
