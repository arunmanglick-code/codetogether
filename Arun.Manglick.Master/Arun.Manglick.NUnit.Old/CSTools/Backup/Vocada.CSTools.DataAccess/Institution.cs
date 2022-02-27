#region File History

/******************************File History***************************
 * File Name        : Institution.cs
 * Author           : Prerak Shah.
 * Created Date     : 3 July 07
 * Purpose          : Add, Update , Institution Records in Database.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * * Date(dd-mm-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 *  23-10-2007     Prerak - Added more preferences
 *  03-12-2007     Prerak - Call getOpenConnection function from Utility Class.
 *  03-20-2008     SSK     "PIN for Message Retrieval" changes
 *  30-05-2008     Suhas    Added parameter @enableCallCenter  
 *  12-Sep-2008    IAK      Added Preference 'Prompt for PIN for CT Message', changed add/edit method of institutions
 * ------------------------------------------------------------------- 
 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Vocada.CSTools.Common;
using Vocada.VoiceLink.DataAccess;
using System.Collections;



namespace Vocada.CSTools.DataAccess
{
    public class Institution
    {
        
        #region  Constants

        /// <summary>
        /// This constant stores name of stored procedure which will add new institution information in Database
        /// </summary>
        private const string SP_INSERT_INSTITUTION = "dbo.VOC_CST_insertInstitution";
        /// <summary>
        /// This constant stores name of stored procedure which will update the nurse with new values in Database
        /// </summary>
        private const string SP_UPDATE_INSTITUTION = "dbo.VOC_CST_updateInstitution";
         /// <summary>
        /// This constant stores name of stored procedure which will add or update preferences for the institution in Database
        /// </summary>
        private const string SP_UPDATE_INSTITUTION_PREFERENCES = "dbo.VOC_CST_updateInstitutionPreferences";//insertInstitutionPreferences
        /// <summary>
        /// This constant stores name of stored procedure which will get the list of institution from Database
        /// </summary>
        private const string SP_GET_INSTITUTIONS = "dbo.getInstitutions";
        /// <summary>
        /// This constant stores name of stored procedure which will get the information of institution from Database
        /// </summary>
        private const string SP_GET_INSTITUTION_INFO = "dbo.VOC_CST_getInstitutionInfo";
         /// <summary>
        /// This constant store the name of stored procedure which retrive institution information
        /// </summary>
        private const string GET_INSTITUTION_INFO = "dbo.VOC_CST_getInstitutionInfo";
        /// <summary>
        /// This constant stores name of stored procedure which will get the information of TimeZone from Database
        /// </summary>
        private const string SP_GET_TIMEZONE = "dbo.getTimeZones";

        #endregion


        #region  Private Variables
        private const string INSTITUTION_ID = "@institutionID";
        private const string INSTITUION_NAME = "@institutionName";
        private const string ADDRESS1 = "@address1";
        private const string ADDRESS2 = "@address2";
        private const string CITY = "@city";
        private const string STATE = "@state";
        private const string ZIP = "@zip";  
        private const string MAIN_PHONE = "@mainPhone";
        private const string PRIMARY_CONTACT_NAME = "@primaryContactName";
        private const string PRIMARY_CONTACT_TITLE = "@primaryContactTitle";  
        private const string PRIMARY_CONTACT_PHONE = "@primaryContactPhone";
        private const string PRIMARY_CONTACT_EMAIL = "@primaryContactEmail";
        private const string CONTACT1_TYPE = "@contact1Type";
        private const string CONTACT1_NAME = "@contact1Name";
        private const string CONTACT1_TITLE = "@contact1Title";
        private const string CONTACT1_PHONE = "@contact1Phone" ;
        private const string CONTACT1_EMAIL = "@contact1Email"; 
        private const string CONTACT2_TYPE = "@contact2Type";
        private const string CONTACT2_NAME = "@contact2Name";
        private const string CONTACT2_TITLE = "@contact2Title";
        private const string CONTACT2_PHONE = "@contact2Phone";
        private const string CONTACT2_EMAIL = "@contact2Email";
        private const string LAB800NUMBER = "@lab800Number";
        private const string NURSE800NUMBER = "@nurse800Number";
        private const string TIME_ZONE_ID = "@timeZoneID";
        private const string SHIFT_NURSE800NUMBER = "@shiftNurse800Number";
        private const string INSTITUTION_VOICEOVER_URL = "@institutionVoiceOverURL";

        private const string ISREQUIRE_CALLBACK_VOICEOVER = "@requireCallbackVoiceOver";
        private const string ISREQUIRE_NAME_CAPTURE = "@requireNameCapture";
        private const string ISREQUIRE_NAME_CAPTURE_VALIDATION = "@validateNameCapture";
        private const string ISREQUIRE_READBACK_MEASUREMENT = "@requireReadbackMeasurement";
        private const string ISREQUIRE_ACCEPTANCE_OUTBOUNDCALL = "@requireAcceptanceOutboundCall";
        private const string ISREQUIRE_ED_MESSAGE = "@requireEDMessage";
        private const string TAB_NAME = "@tabName";
        private const string BATCH_MSG = "@batchMessages";
        private const string ISREQUIRE_EXAM_DESCRIPTION = "@requireExamDesc";
        private const string MESSAGE_RETRIEVE_USING_PIN = "@messagePin";
        private const string ENABLE_CALL_CENTER = "@enableCallCenter";
        private const string ENABLE_PROMPT_FOR_PIN = "@enablePromptForPin";
        
        #endregion  Constants


        #region PublicMethods
        /// <summary>
        /// Adds Institution information to the database
        /// </summary>
        /// <param name="InstitutionInformation"></param>
        /// <returns></returns>

        public int AddInstitution(InstitutionInformation objInstitutionInfo)
        {
            SqlConnection conn = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = conn.BeginTransaction();
            try
            {
                int institutionID = 0;
                SqlParameter[] arParams = new SqlParameter[39];

                
                //Create parameter for Institution Name
                arParams[0] = new SqlParameter(INSTITUION_NAME, SqlDbType.VarChar);
                arParams[0].Value = objInstitutionInfo.InstitutionName;

                //Create parameter for Address1
                arParams[1] = new SqlParameter(ADDRESS1, SqlDbType.VarChar);
                arParams[1].Value = objInstitutionInfo.Address1;

                //Create parameter for Address2
                arParams[2] = new SqlParameter(ADDRESS2, SqlDbType.VarChar);
                arParams[2].Value = objInstitutionInfo.Address2;

                //Create parameter for CITY
                arParams[3] = new SqlParameter(CITY, SqlDbType.VarChar);
                arParams[3].Value = objInstitutionInfo.City;

                //Create parameter for STATE                                    
                arParams[4] = new SqlParameter(STATE, SqlDbType.VarChar);
                arParams[4].Value = objInstitutionInfo.State;

                //Create parameter for ZIP
                arParams[5] = new SqlParameter(ZIP, SqlDbType.VarChar);
                arParams[5].Value = objInstitutionInfo.Zip;

                //Create parameter for MAIN_PHONE
                arParams[6] = new SqlParameter(MAIN_PHONE, SqlDbType.VarChar);
                arParams[6].Value = objInstitutionInfo.MainPhoneNumber;

                //Create parameter for PRIMARY_CONTACT_NAME
                arParams[7] = new SqlParameter(PRIMARY_CONTACT_NAME, SqlDbType.VarChar);
                arParams[7].Value = objInstitutionInfo.PrimaryContactName;

                //Create parameter for PRIMARY_CONTACT_TITLE
                arParams[8] = new SqlParameter(PRIMARY_CONTACT_TITLE, SqlDbType.VarChar);
                arParams[8].Value = objInstitutionInfo.PrimaryContactTitle ;

                //Create parameter for PRIMARY_CONTACT_PHONE
                arParams[9] = new SqlParameter(PRIMARY_CONTACT_PHONE, SqlDbType.VarChar);
                arParams[9].Value = objInstitutionInfo.PrimaryContactPhone ;

                //Create parameter for PRIMARY_CONTACT_EMAIL
                arParams[10] = new SqlParameter(PRIMARY_CONTACT_EMAIL, SqlDbType.VarChar);
                arParams[10].Value = objInstitutionInfo.PrimaryContactEmail ;

                //Create parameter for CONTACT1_TYPE
                arParams[11] = new SqlParameter(CONTACT1_TYPE, SqlDbType.VarChar);
                arParams[11].Value = objInstitutionInfo.Contact1Type ;

                //Create parameter for CONTACT1_NAME
                arParams[12] = new SqlParameter(CONTACT1_NAME, SqlDbType.VarChar);
                arParams[12].Value = objInstitutionInfo.Contact1Name ;

                //Create parameter for CONTACT1_TITLE
                arParams[13] = new SqlParameter(CONTACT1_TITLE, SqlDbType.VarChar);
                arParams[13].Value = objInstitutionInfo.Contact1Title ;
               
                //Create parameter for CONTACT1_PHONE
                arParams[14] = new SqlParameter(CONTACT1_PHONE, SqlDbType.VarChar);
                arParams[14].Value = objInstitutionInfo.Contact1Phone  ;

                //Create parameter for CONTACT1_Email
                arParams[15] = new SqlParameter(CONTACT1_EMAIL, SqlDbType.VarChar);
                arParams[15].Value = objInstitutionInfo.Contact1Email ;

                 //Create parameter for CONTACT2_TYPE
                arParams[16] = new SqlParameter(CONTACT2_TYPE, SqlDbType.VarChar);
                arParams[16].Value = objInstitutionInfo.Contact2Type ;

                //Create parameter for CONTACT2_NAME
                arParams[17] = new SqlParameter(CONTACT2_NAME, SqlDbType.VarChar);
                arParams[17].Value = objInstitutionInfo.Contact2Name ;

                //Create parameter for CONTACT2_TITLE
                arParams[18] = new SqlParameter(CONTACT2_TITLE, SqlDbType.VarChar);
                arParams[18].Value = objInstitutionInfo.Contact2Title ;
               
                //Create parameter for CONTACT2_PHONE
                arParams[19] = new SqlParameter(CONTACT2_PHONE, SqlDbType.VarChar);
                arParams[19].Value = objInstitutionInfo.Contact2Phone  ;

                //Create parameter for CONTACT2_Email
                arParams[20] = new SqlParameter(CONTACT2_EMAIL, SqlDbType.VarChar);
                arParams[20].Value = objInstitutionInfo.Contact2Email ;

                 //Create parameter for LAB800NUMBER
                arParams[21] = new SqlParameter(LAB800NUMBER, SqlDbType.VarChar);
                arParams[21].Value = objInstitutionInfo.Lab800Number ;

                //Create parameter for NURSE800NUMBER
                arParams[22] = new SqlParameter(NURSE800NUMBER, SqlDbType.VarChar);
                arParams[22].Value = objInstitutionInfo.Nurse800Number ;

                //Create parameter for TIME_ZONE_ID
                arParams[23] = new SqlParameter(TIME_ZONE_ID, SqlDbType.Int, 4);
                arParams[23].Value = objInstitutionInfo.TimeZone;

                //Create parameter for SHIFT_NURSE800NUMBER
                arParams[24] = new SqlParameter(SHIFT_NURSE800NUMBER, SqlDbType.VarChar);
                arParams[24].Value = objInstitutionInfo.ShiftNurse800Number ;
               
               
                //Create parameter for INSTITUTION_VOICEOVER_URL
                arParams[25] = new SqlParameter(INSTITUTION_VOICEOVER_URL, SqlDbType.VarChar);
                arParams[25].Value = objInstitutionInfo.InstitutionVoiceOverURL  ;

                //Create parameter for ISREQUIRE_CALLBACK_VOICEOVER Name
                arParams[26] = new SqlParameter(ISREQUIRE_CALLBACK_VOICEOVER, SqlDbType.Bit);
                arParams[26].Value = objInstitutionInfo.IsRequireCallBackVoiceOver;

                //Create parameter for ISREQUIRE_NAME_CAPTURE
                arParams[27] = new SqlParameter(ISREQUIRE_NAME_CAPTURE, SqlDbType.Bit);
                arParams[27].Value = objInstitutionInfo.IsRequireNameCapture;

                //Create parameter for ISREQUIRE_READBACK_MEASUREMENT
                arParams[28] = new SqlParameter(ISREQUIRE_READBACK_MEASUREMENT, SqlDbType.Bit);
                arParams[28].Value = objInstitutionInfo.IsRequireReadbackMeasurement;

                //Create parameter for ISREQUIRE_ACCEPTANCE_OUTBOUNDCALL Name
                arParams[29] = new SqlParameter(ISREQUIRE_ACCEPTANCE_OUTBOUNDCALL, SqlDbType.Bit);
                arParams[29].Value = objInstitutionInfo.IsRequireAcceptanceOutboundCall;

                //Create parameter for ISREQUIRE_NAME_CAPTURE
                arParams[30] = new SqlParameter(ISREQUIRE_ED_MESSAGE, SqlDbType.Bit);
                arParams[30].Value = objInstitutionInfo.IsRequireEDMessage;

                //Create parameter for TAB_Name
                arParams[31] = new SqlParameter(TAB_NAME, SqlDbType.Bit);
                arParams[31].Value = objInstitutionInfo.TabName;

                //Create parameter for Institution ID
                arParams[32] = new SqlParameter(INSTITUTION_ID, SqlDbType.Int);
                arParams[32].Direction = ParameterDirection.Output; 
                arParams[32].Value = 0;

                //Create parameter for batchMessage
                arParams[33] = new SqlParameter(BATCH_MSG, SqlDbType.Bit);
                arParams[33].Value = objInstitutionInfo.BatchMessage;

                //Create parameter for Require Name Capture Validation
                arParams[34] = new SqlParameter(ISREQUIRE_NAME_CAPTURE_VALIDATION, SqlDbType.Bit);
                arParams[34].Value = objInstitutionInfo.IsRequireNameCaptureValidation;

                //Create parameter for Require Exam Description 
                arParams[35] = new SqlParameter(ISREQUIRE_EXAM_DESCRIPTION, SqlDbType.Bit);
                arParams[35].Value = objInstitutionInfo.IsRequireExamDescription;

                //Create parameter for Message Retrieve Using PIN
                arParams[36] = new SqlParameter(MESSAGE_RETRIEVE_USING_PIN, SqlDbType.Bit);
                arParams[36].Value = objInstitutionInfo.MessageRetrieveUsingPIN;

                //Create parameter for Enable Call Center option
                arParams[37] = new SqlParameter(ENABLE_CALL_CENTER, SqlDbType.Bit);
                arParams[37].Value = objInstitutionInfo.EnableCallCenter;

                //Create parameter for Prompt for PIN for CT Message
                arParams[38] = new SqlParameter(ENABLE_PROMPT_FOR_PIN, SqlDbType.Bit);
                arParams[38].Value = objInstitutionInfo.EnablePromptForPin;

                SqlHelper.ExecuteScalar(sqlTransaction, CommandType.StoredProcedure, SP_INSERT_INSTITUTION, arParams);
                institutionID = Convert.ToInt32(arParams[32].Value);

                sqlTransaction.Commit();
                conn.Close();
                return institutionID;
            }
            catch (SqlException sqlError)
            {
                sqlTransaction.Rollback();
                conn.Close();
                throw;
            }
        }
        /// <summary>
        /// Adds or update Institution preferences to the database
        /// </summary>
        /// <param name="objInstitutionInfo">Object of Institution</param>
        /// <returns></returns>

        public void AddInstitutionPreferances(InstitutionInformation objInstitutionInfo)
        {
            SqlConnection conn = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = conn.BeginTransaction();
            try
            {   
                SqlParameter[] arParams = new SqlParameter[4];

                 //Create parameter for Institution ID
                arParams[0] = new SqlParameter(INSTITUTION_ID, objInstitutionInfo.InstitutionID);
                arParams[0].Direction = ParameterDirection.Input; 
                //arParams[0].Value = objInstitutionInfo.InstitutionID;

                //Create parameter for ISREQUIRE_CALLBACK_VOICEOVER Name
                arParams[1] = new SqlParameter(ISREQUIRE_CALLBACK_VOICEOVER, objInstitutionInfo.IsRequireCallBackVoiceOver);
                //arParams[1].Value = objInstitutionInfo.InstitutionName;
                arParams[1].Direction = ParameterDirection.Input; 
               
                //Create parameter for ISREQUIRE_NAME_CAPTURE
                arParams[2] = new SqlParameter(ISREQUIRE_NAME_CAPTURE, objInstitutionInfo.IsRequireNameCapture);
                //arParams[2].Value = objInstitutionInfo.IsRequireNameCapture ;
                arParams[2].Direction = ParameterDirection.Input; 
               
                //Create parameter for ISREQUIRE_READBACK_MEASUREMENT
                arParams[3] = new SqlParameter(ISREQUIRE_READBACK_MEASUREMENT, objInstitutionInfo.IsRequireReadbackMeasurement);
                //arParams[3].Value = objInstitutionInfo.IsRequireReadbackMeasurement;
                arParams[3].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, SP_UPDATE_INSTITUTION_PREFERENCES, arParams);
                sqlTransaction.Commit();
                conn.Close();

            }
             catch (SqlException sqlError)
            {
                sqlTransaction.Rollback();
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// Adds Update institution information to the database
        /// </summary>
        /// <param name="objInstitutionInfo">Object of Institution</param>
        /// <returns></returns>

        public int UpdateInstitution(InstitutionInformation objInstitutionInfo)
        {
            SqlConnection conn = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = conn.BeginTransaction();
            try
            {
                
                SqlParameter[] arParams = new SqlParameter[39];

                arParams[0] = new SqlParameter(INSTITUTION_ID, SqlDbType.Int, 4);
                arParams[0].Value = objInstitutionInfo.InstitutionID ;

                //Create parameter for Institution Name
                arParams[1] = new SqlParameter(INSTITUION_NAME, SqlDbType.VarChar);
                arParams[1].Value = objInstitutionInfo.InstitutionName;

                //Create parameter for Address1
                arParams[2] = new SqlParameter(ADDRESS1, SqlDbType.VarChar);
                arParams[2].Value = objInstitutionInfo.Address1;

                //Create parameter for Address2
                arParams[3] = new SqlParameter(ADDRESS2, SqlDbType.VarChar);
                arParams[3].Value = objInstitutionInfo.Address2;

                //Create parameter for CITY
                arParams[4] = new SqlParameter(CITY, SqlDbType.VarChar);
                arParams[4].Value = objInstitutionInfo.City;

                //Create parameter for STATE                                    
                arParams[5] = new SqlParameter(STATE, SqlDbType.VarChar);
                arParams[5].Value = objInstitutionInfo.State;

                //Create parameter for ZIP
                arParams[6] = new SqlParameter(ZIP, SqlDbType.VarChar);
                arParams[6].Value = objInstitutionInfo.Zip;

                //Create parameter for MAIN_PHONE
                arParams[7] = new SqlParameter(MAIN_PHONE, SqlDbType.VarChar);
                arParams[7].Value = objInstitutionInfo.MainPhoneNumber;

                //Create parameter for PRIMARY_CONTACT_NAME
                arParams[8] = new SqlParameter(PRIMARY_CONTACT_NAME, SqlDbType.VarChar);
                arParams[8].Value = objInstitutionInfo.PrimaryContactName;

                //Create parameter for PRIMARY_CONTACT_TITLE
                arParams[9] = new SqlParameter(PRIMARY_CONTACT_TITLE, SqlDbType.VarChar);
                arParams[9].Value = objInstitutionInfo.PrimaryContactTitle ;

                //Create parameter for PRIMARY_CONTACT_PHONE
                arParams[10] = new SqlParameter(PRIMARY_CONTACT_PHONE, SqlDbType.VarChar);
                arParams[10].Value = objInstitutionInfo.PrimaryContactPhone ;

                //Create parameter for PRIMARY_CONTACT_EMAIL
                arParams[11] = new SqlParameter(PRIMARY_CONTACT_EMAIL, SqlDbType.VarChar);
                arParams[11].Value = objInstitutionInfo.PrimaryContactEmail ;

                //Create parameter for CONTACT1_TYPE
                arParams[12] = new SqlParameter(CONTACT1_TYPE, SqlDbType.VarChar);
                arParams[12].Value = objInstitutionInfo.Contact1Type ;

                //Create parameter for CONTACT1_NAME
                arParams[13] = new SqlParameter(CONTACT1_NAME, SqlDbType.VarChar);
                arParams[13].Value = objInstitutionInfo.Contact1Name ;

                //Create parameter for CONTACT1_TITLE
                arParams[14] = new SqlParameter(CONTACT1_TITLE, SqlDbType.VarChar);
                arParams[14].Value = objInstitutionInfo.Contact1Title ;
               
                //Create parameter for CONTACT1_PHONE
                arParams[15] = new SqlParameter(CONTACT1_PHONE, SqlDbType.VarChar);
                arParams[15].Value = objInstitutionInfo.Contact1Phone  ;

                //Create parameter for CONTACT1_Email
                arParams[16] = new SqlParameter(CONTACT1_EMAIL, SqlDbType.VarChar);
                arParams[16].Value = objInstitutionInfo.Contact1Email ;

                 //Create parameter for CONTACT2_TYPE
                arParams[17] = new SqlParameter(CONTACT2_TYPE, SqlDbType.VarChar);
                arParams[17].Value = objInstitutionInfo.Contact2Type ;

                //Create parameter for CONTACT2_NAME
                arParams[18] = new SqlParameter(CONTACT2_NAME, SqlDbType.VarChar);
                arParams[18].Value = objInstitutionInfo.Contact2Name ;

                //Create parameter for CONTACT2_TITLE
                arParams[19] = new SqlParameter(CONTACT2_TITLE, SqlDbType.VarChar);
                arParams[19].Value = objInstitutionInfo.Contact2Title ;
               
                //Create parameter for CONTACT2_PHONE
                arParams[20] = new SqlParameter(CONTACT2_PHONE, SqlDbType.VarChar);
                arParams[20].Value = objInstitutionInfo.Contact2Phone  ;

                //Create parameter for CONTACT2_Email
                arParams[21] = new SqlParameter(CONTACT2_EMAIL, SqlDbType.VarChar);
                arParams[21].Value = objInstitutionInfo.Contact2Email ;

                 //Create parameter for LAB800NUMBER
                arParams[22] = new SqlParameter(LAB800NUMBER, SqlDbType.VarChar);
                arParams[22].Value = objInstitutionInfo.Lab800Number ;

                //Create parameter for NURSE800NUMBER
                arParams[23] = new SqlParameter(NURSE800NUMBER, SqlDbType.VarChar);
                arParams[23].Value = objInstitutionInfo.Nurse800Number ;

                //Create parameter for SHIFT_NURSE800NUMBER
                arParams[24] = new SqlParameter(SHIFT_NURSE800NUMBER, SqlDbType.VarChar);
                arParams[24].Value = objInstitutionInfo.ShiftNurse800Number ;
               
                //Create parameter for TIME_ZONE_ID
                arParams[25] = new SqlParameter(TIME_ZONE_ID, SqlDbType.Int,4);
                arParams[25].Value = objInstitutionInfo.TimeZone  ;

                //Create parameter for INSTITUTION_VOICEOVER_URL
                arParams[26] = new SqlParameter(INSTITUTION_VOICEOVER_URL, SqlDbType.VarChar);
                arParams[26].Value = objInstitutionInfo.InstitutionVoiceOverURL;

                //Create parameter for ISREQUIRE_CALLBACK_VOICEOVER Name
                arParams[27] = new SqlParameter(ISREQUIRE_CALLBACK_VOICEOVER, SqlDbType.Bit);
                arParams[27].Value = objInstitutionInfo.IsRequireCallBackVoiceOver;

                //Create parameter for ISREQUIRE_NAME_CAPTURE
                arParams[28] = new SqlParameter(ISREQUIRE_NAME_CAPTURE, SqlDbType.Bit);
                arParams[28].Value = objInstitutionInfo.IsRequireNameCapture;

                //Create parameter for ISREQUIRE_READBACK_MEASUREMENT
                arParams[29] = new SqlParameter(ISREQUIRE_READBACK_MEASUREMENT, SqlDbType.Bit);
                arParams[29].Value = objInstitutionInfo.IsRequireReadbackMeasurement;

                //Create parameter for ISREQUIRE_ACCEPTANCE_OUTBOUNDCALL Name
                arParams[30] = new SqlParameter(ISREQUIRE_ACCEPTANCE_OUTBOUNDCALL, SqlDbType.Bit);
                arParams[30].Value = objInstitutionInfo.IsRequireAcceptanceOutboundCall;

                //Create parameter for ISREQUIRE_NAME_CAPTURE
                arParams[31] = new SqlParameter(ISREQUIRE_ED_MESSAGE, SqlDbType.Bit);
                arParams[31].Value = objInstitutionInfo.IsRequireEDMessage;

                //Create parameter for TAB_Name
                arParams[32] = new SqlParameter(TAB_NAME, SqlDbType.Bit);
                arParams[32].Value = objInstitutionInfo.TabName;

                //Create parameter for batchMessage
                arParams[33] = new SqlParameter(BATCH_MSG, SqlDbType.Bit);
                arParams[33].Value = objInstitutionInfo.BatchMessage;

                //Create parameter for ISREQUIRE_NAME_CAPTURE_VALIDATION
                arParams[34] = new SqlParameter(ISREQUIRE_NAME_CAPTURE_VALIDATION, SqlDbType.Bit);
                arParams[34].Value = objInstitutionInfo.IsRequireNameCaptureValidation;

                //Create parameter for Require Exam Description 
                arParams[35] = new SqlParameter(ISREQUIRE_EXAM_DESCRIPTION, SqlDbType.Bit);
                arParams[35].Value = objInstitutionInfo.IsRequireExamDescription;

                //Create parameter for Message Retrieve Using PIN
                arParams[36] = new SqlParameter(MESSAGE_RETRIEVE_USING_PIN, SqlDbType.Bit);
                arParams[36].Value = objInstitutionInfo.MessageRetrieveUsingPIN;

                //Create parameter for Enable Call Center option
                arParams[37] = new SqlParameter(ENABLE_CALL_CENTER, SqlDbType.Bit);
                arParams[37].Value = objInstitutionInfo.EnableCallCenter;

                //Create parameter for Prompt for PIN for CT Message
                arParams[38] = new SqlParameter(ENABLE_PROMPT_FOR_PIN, SqlDbType.Bit);
                arParams[38].Value = objInstitutionInfo.EnablePromptForPin;

                SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, SP_UPDATE_INSTITUTION, arParams);
               
                sqlTransaction.Commit();
                conn.Close();
                return objInstitutionInfo.InstitutionID ;
            }
            catch (SqlException sqlError)
            {
                sqlTransaction.Rollback();
                conn.Close();
                throw;
            }
        }

        public void UpdateInstitutionPreferances(InstitutionInformation objInstitutionInfo)
        {
            SqlConnection conn = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = conn.BeginTransaction();
            try
            {   
                SqlParameter[] arParams = new SqlParameter[4];

                 //Create parameter for Institution ID
                arParams[0] = new SqlParameter(INSTITUTION_ID, SqlDbType.Int, 4);
                arParams[0].Value = objInstitutionInfo.InstitutionID;

                //Create parameter for ISREQUIRE_CALLBACK_VOICEOVER Name
                arParams[1] = new SqlParameter(ISREQUIRE_CALLBACK_VOICEOVER, SqlDbType.Bit);
                arParams[1].Value = objInstitutionInfo.InstitutionName;

                //Create parameter for ISREQUIRE_NAME_CAPTURE
                arParams[2] = new SqlParameter(ISREQUIRE_NAME_CAPTURE, SqlDbType.Bit);
                arParams[2].Value = objInstitutionInfo.IsRequireNameCapture ;

                //Create parameter for ISREQUIRE_READBACK_MEASUREMENT
                arParams[3] = new SqlParameter(ISREQUIRE_READBACK_MEASUREMENT, SqlDbType.Bit);
                arParams[3].Value = objInstitutionInfo.IsRequireReadbackMeasurement;
                
                SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, SP_UPDATE_INSTITUTION_PREFERENCES, arParams);
                sqlTransaction.Commit();
                conn.Close();

            }
             catch (SqlException sqlError)
            {
                sqlTransaction.Rollback();
                conn.Close();
                throw;
            }
        }
         /// <summary>
        /// Gets Institutution List 
        /// </summary>
        /// <returns></returns>
  
        public DataTable GetInstitutionList()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtInstitutions = new DataTable();
                SqlDataReader drInstitutions = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_INSTITUTIONS);
                dtInstitutions.Load(drInstitutions);
                return dtInstitutions;
            }

        }
         /// <summary>
        /// Gets Institutution List 
        /// </summary>
        /// <returns></returns>
  
        public DataTable GetInstitutionInfo(int institutionID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtInstitutionInfo = new DataTable();
                SqlParameter[] arSqlParams = new SqlParameter[1];

                arSqlParams[0] = new SqlParameter(INSTITUTION_ID, SqlDbType.Int);
                arSqlParams[0].Value = institutionID;
             
                SqlDataReader drInstitutionInfo = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_INSTITUTION_INFO,arSqlParams);
                dtInstitutionInfo.Load(drInstitutionInfo);
                return dtInstitutionInfo;
            }

        }

        /// <summary>
        /// Get Full User Information
        /// </summary>
        /// <param name="subscriberID"></param>
        public InstitutionInfo GetInstitutionInfo_Obj(int institutionID)
        {
            InstitutionInfo objInstitutionInfo = null;
            try
            {
                objInstitutionInfo = new InstitutionInfo();
                using (SqlConnection sqlConnection = Utility.getOpenConnection())
                {
                    SqlParameter[] objSqlParameter = new SqlParameter[1];
                    objSqlParameter[0] = new SqlParameter(INSTITUTION_ID, institutionID);
                    objSqlParameter[0].Direction = ParameterDirection.Input;

                    SqlDataReader drInstitutionInfo = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, GET_INSTITUTION_INFO, objSqlParameter);
                    objInstitutionInfo = new InstitutionInfo();
                    if (drInstitutionInfo.Read())
                    {
                        objInstitutionInfo.InstitutionID = drInstitutionInfo.GetInt32(drInstitutionInfo.GetOrdinal("InstitutionID"));
                        objInstitutionInfo.InstitutionName = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("InstitutionName")) == DBNull.Value ? "" : drInstitutionInfo.GetString(drInstitutionInfo.GetOrdinal("InstitutionName"));
                        objInstitutionInfo.Address1 = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("Address1")) == DBNull.Value ? "" : drInstitutionInfo.GetString(drInstitutionInfo.GetOrdinal("Address1"));
                        objInstitutionInfo.Address2 = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("Address2")) == DBNull.Value ? "" : drInstitutionInfo.GetString(drInstitutionInfo.GetOrdinal("Address2"));

                        objInstitutionInfo.City = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("City")) == DBNull.Value ? "" : drInstitutionInfo.GetString(drInstitutionInfo.GetOrdinal("City"));
                        objInstitutionInfo.State = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("State")) == DBNull.Value ? "" : drInstitutionInfo.GetString(drInstitutionInfo.GetOrdinal("State"));
                        objInstitutionInfo.Zip = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("Zip")) == DBNull.Value ? "" : drInstitutionInfo.GetString(drInstitutionInfo.GetOrdinal("Zip"));

                        objInstitutionInfo.TimeZone = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("TimeZoneDescription")) == DBNull.Value ? "" : drInstitutionInfo.GetString(drInstitutionInfo.GetOrdinal("TimeZoneDescription"));

                        objInstitutionInfo.MainPhoneNumber = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("MainPhoneNumber")) == DBNull.Value ? "" : drInstitutionInfo.GetString(drInstitutionInfo.GetOrdinal("MainPhoneNumber"));
                        objInstitutionInfo.Lab800Number = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("Lab800Number")) == DBNull.Value ? "" : drInstitutionInfo.GetString(drInstitutionInfo.GetOrdinal("Lab800Number"));
                        objInstitutionInfo.Nurse800Number = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("Nurse800Number")) == DBNull.Value ? "" : drInstitutionInfo.GetString(drInstitutionInfo.GetOrdinal("Nurse800Number"));
                        objInstitutionInfo.ShiftNurse800Number = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("ShiftNurse800Number")) == DBNull.Value ? "" : drInstitutionInfo.GetString(drInstitutionInfo.GetOrdinal("ShiftNurse800Number"));

                        objInstitutionInfo.IsRequireCallBackVoiceOver = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("RequireCallBackVoiceOver")) == DBNull.Value ? false : drInstitutionInfo.GetBoolean(drInstitutionInfo.GetOrdinal("RequireCallBackVoiceOver"));
                        objInstitutionInfo.IsRequireNameCapture = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("RequireNameCapture")) == DBNull.Value ? false : drInstitutionInfo.GetBoolean(drInstitutionInfo.GetOrdinal("RequireNameCapture"));
                        objInstitutionInfo.IsRequireNameCaptureValidation = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("ValidateNameCapture")) == DBNull.Value ? false : drInstitutionInfo.GetBoolean(drInstitutionInfo.GetOrdinal("ValidateNameCapture"));
                        objInstitutionInfo.IsRequireReadbackMeasurement = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("RequireReadbackMeasurement")) == DBNull.Value ? false : drInstitutionInfo.GetBoolean(drInstitutionInfo.GetOrdinal("RequireReadbackMeasurement"));
                        objInstitutionInfo.IsRequireAcceptanceOutboundCall = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("RequireAcceptanceOutbound")) == DBNull.Value ? false : drInstitutionInfo.GetBoolean(drInstitutionInfo.GetOrdinal("RequireAcceptanceOutbound"));
                        objInstitutionInfo.IsRequireVoiceClips = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("RequireVoiceClips")) == DBNull.Value ? false : drInstitutionInfo.GetBoolean(drInstitutionInfo.GetOrdinal("RequireVoiceClips"));
                        objInstitutionInfo.IsConnectED = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("IsConnectED")) == DBNull.Value ? false : drInstitutionInfo.GetBoolean(drInstitutionInfo.GetOrdinal("IsConnectED"));
                        objInstitutionInfo.BatchMessage = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("BatchMessages")) == DBNull.Value ? false : drInstitutionInfo.GetBoolean(drInstitutionInfo.GetOrdinal("BatchMessages"));
                        objInstitutionInfo.IsExamDescription = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("RequireExamDescription")) == DBNull.Value ? false : drInstitutionInfo.GetBoolean(drInstitutionInfo.GetOrdinal("RequireExamDescription"));
                        objInstitutionInfo.MessageRetrieveUsingPIN = drInstitutionInfo.GetValue(drInstitutionInfo.GetOrdinal("MessageRetrieveUsingPIN")) == DBNull.Value ? false : drInstitutionInfo.GetBoolean(drInstitutionInfo.GetOrdinal("MessageRetrieveUsingPIN"));
                    }
                }
                return objInstitutionInfo;
            }
            finally
            {
                objInstitutionInfo = null;
            }
        }

        /// <summary>
        /// This function Gets all the TimeZones.
        /// This function calls stored procedure "getTimeZone
        /// </summary>
        /// <param name="subscriberID" type="int"></param>
        public DataTable GetTimeZone()
        {
            SqlDataReader reader = null;
            DataTable dt = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_TIMEZONE);
                if (reader.HasRows)
                {
                    dt.Load(reader); 
                }
                return dt;
            }
        }

        #endregion
    }
    public class InstitutionInfo
    {
        #region private Members
        private int institutionID;
        private string institutionName = string.Empty;
        private string address1 = string.Empty;
        private string address2 = string.Empty;
        private string city = string.Empty;
        private string state = string.Empty;
        private string zip = string.Empty;
        private string mainPhoneNumber = string.Empty;
        private string lab800Number = string.Empty;
        private string nurse800Number = string.Empty;
        private string shiftNurse800Number = string.Empty;
        private string timeZone = string.Empty;
        private bool isRequireCallBackVoiceOver = false;
        private bool isReuireNameCapture = false;
        private bool isRequireNameCaptureValidation = false;
        private bool isRequireReadbackMeasurement = false;
        private bool isRequireAcceptanceOutboundCall = false;
        private bool isRequireVoiceClips = false;
        private bool isConnectED = false;
        private bool batchMessage = false;
        private bool isExamDesc = false;
        private bool messageRetrieve = false;
        private bool enableCallCenter = false;
        #endregion private Members

        #region Property

        /// <summary>
        /// Get Institution ID 
        /// </summary>
        public int InstitutionID
        {
            get
            {
                return institutionID;
            }
            set
            {
                institutionID = value;
            }
        }

        /// <summary>
        /// Get Institution Name
        /// </summary>
        public string InstitutionName
        {
            get
            {
                return institutionName;
            }
            set
            {
                institutionName = value;
            }
        }

        /// <summary>
        /// Address Line 1
        /// </summary>
        public string Address1
        {
            get { return address1; }
            set { address1 = value; }
        }
        /// <summary>
        /// Address Line 2
        /// </summary>
        public string Address2
        {
            get { return address2; }
            set { address2 = value; }
        }
        /// <summary>
        /// City
        /// </summary>
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        /// <summary>
        /// State
        /// </summary>
        public string State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// Zip
        /// </summary>
        public string Zip
        {
            get { return zip; }
            set { zip = value; }
        }
        /// <summary>
        /// Main Phone Number
        /// </summary>
        public string MainPhoneNumber
        {
            get { return mainPhoneNumber; }
            set { mainPhoneNumber = value; }
        }
        /// <summary>
        /// Get Lab 800 Number
        /// </summary>
        public string Lab800Number
        {
            get
            {
                return lab800Number;
            }
            set
            {
                lab800Number = value;
            }
        }

        /// <summary>
        /// Get Nurse 800 Number
        /// </summary>
        public string Nurse800Number
        {
            get
            {
                return nurse800Number;
            }
            set
            {
                nurse800Number = value;
            }
        }

        /// <summary>
        /// Shift Nurse 800 Number for nursh shift assignment
        /// </summary>
        public string ShiftNurse800Number
        {
            get { return shiftNurse800Number; }
            set { shiftNurse800Number = value; }
        }
        /// <summary>
        /// Timezone
        /// </summary>
        public string TimeZone
        {
            get { return timeZone; }
            set { timeZone = value; }
        }
        /// <summary>
        /// Require Call Back Voice Over in VUI Lab
        /// </summary>
        public bool IsRequireCallBackVoiceOver
        {
            get { return isRequireCallBackVoiceOver; }
            set { isRequireCallBackVoiceOver = value; }
        }
        /// <summary>
        /// Require Name capture in VUI Lab 
        /// </summary>
        public bool IsRequireNameCapture
        {
            get { return isReuireNameCapture; }
            set { isReuireNameCapture = value; }
        }
        /// <summary>
        /// Require Name capture validation in VUI Lab 
        /// </summary>
        public bool IsRequireNameCaptureValidation
        {
            get { return isRequireNameCaptureValidation; }
            set { isRequireNameCaptureValidation = value; }
        }
        /// <summary>
        /// Require unit measurement readback in VUI Lab
        /// </summary>
        public bool IsRequireReadbackMeasurement
        {
            get { return isRequireReadbackMeasurement; }
            set { isRequireReadbackMeasurement = value; }
        }
        /// <summary>
        /// Require Acceptance For Outbound Call
        /// </summary>
        public bool IsRequireAcceptanceOutboundCall
        {
            get { return isRequireAcceptanceOutboundCall; }
            set { isRequireAcceptanceOutboundCall = value; }
        }

        /// <summary>
        /// Require VoiceClips Functionality
        /// </summary>
        public bool IsRequireVoiceClips
        {
            get
            {
                return isRequireVoiceClips;
            }
            set
            {
                isRequireVoiceClips = value;
            }
        }

        /// <summary>
        /// Is Connect ED
        /// </summary>
        public bool IsConnectED
        {
            get
            {
                return isConnectED;
            }
            set
            {
                isConnectED = value;
            }
        }
        /// <summary>
        /// Batch Messages
        /// </summary>
        public bool BatchMessage
        {
            get
            {
                return batchMessage;
            }
            set
            {
                batchMessage = value;
            }
        }
        /// <summary>
        /// Show Exam Description Flag
        /// </summary>
        public bool IsExamDescription
        {
            get
            {
                return isExamDesc;
            }
            set
            {
                isExamDesc = value;
            }
        }

        /// <summary>
        /// Message Retrieve Using PIN
        /// </summary>
        public bool MessageRetrieveUsingPIN
        {
            get
            {
                return messageRetrieve;
            }
            set
            {
                messageRetrieve = value;
            }
        }

        /// <summary>
        /// Message Retrieve Using PIN
        /// </summary>
        public bool EnableCallCenter
        {
            get
            {
                return enableCallCenter;
            }
            set
            {
                enableCallCenter = value;
            }
        }

        #endregion Property
    }
}
