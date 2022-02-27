#region File History

/******************************File History***************************
 * File Name        : Vocada.Veriphy.BusinessClasses/LabMessageForward.cs
 * Author           : Swapnil K
 * Created Date     : 15-Feb-07
 * Purpose          : To take care all Database transactions for the tab Message Forward.
 *                  : 
 *                  :

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
    /// Business layer class for Message Forward screen in Veriphy Lab Web
    /// </summary>
    public class LabMessageForward
    {
        #region Stored Procedure
        /// <summary>
        /// SP for getDirectoryInfoForSubscriber
        /// </summary>
        private const string SP_GET_DIRECTORY_ID = "dbo.getDirectoryInfoForSubscriber";

        /// <summary>
        /// SP for VOC_VLR_getClinicianForDirectory
        /// </summary>
        private const string SP_GET_CLINICIAN_FOR_DIRECTORY = "dbo.VOC_VW_getRecipientForDirectory";

        /// <summary>
        /// SP for Voc_VW_insertMessageDetails
        /// </summary>
        private const string SP_INSERT_MSG_DETAILS = "dbo.VOC_CST_insertMessageDetails";

        /// <summary>
        /// SP for VOC_VL_forwardLabTests
        /// </summary>
        private const string SP_FWD_LAB_TESTS = "dbo.VOC_VL_forwardLabTests";
        
        /// <summary>
        /// SP for Voc_VW_rollBackMessage
        /// </summary>
        private const string SP_ROLLBACK = "dbo.VOC_VW_rollbackMessage";
        #endregion

        #region Parameter Constant
        /// <summary>
        /// Parameter for @subscriberID
        /// </summary>
        private const string SUBSCRIBER_ID = "@subscriberID";
        /// <summary>
        /// Parameter for @directoryID
        /// </summary>
        private const string DIRECTORY_ID = "@directoryID";
        /// <summary>
        /// Parameter for @specialistID
        /// </summary>
        private const string SPECIALIST_ID = "@specialistID";
        /// <summary>
        /// Parameter for @recipientID
        /// </summary>
        private const string RECIPIENT_ID = "@recipientID";
        /// <summary>
        /// Parameter for @patientVoiceURL
        /// </summary>
        private const string PATIENT_VOICE_URL = "@patientVoiceURL";        
        /// <summary>
        /// Parameter for @impressionVoiceURL
        /// </summary>
        private const string IMPRESSION_VOICE_URL = "@impressionVoiceURL";
        /// <summary>
        /// Parameter for @findingID
        /// </summary>
        private const string FINDING_ID = "@findingID";
        /// <summary>
        /// Parameter for @did
        /// </summary>
        private const string DID = "@did";
        /// <summary>
        /// Parameter for @mrn
        /// </summary>
        private const string MRN = "@mrn";
        /// <summary>
        /// Parameter for @dob
        /// </summary>
        private const string DOB = "@dob";
        /// <summary>
        /// Parameter for @forward
        /// </summary>
        private const string FORWARD = "@forward";
        /// <summary>
        /// Parameter for @originalMessageID
        /// </summary>
        private const string ORIGINAL_MSG_ID = "@originalMessageID";
        /// <summary>
        /// Parameter for @recipientTypeID
        /// </summary>
        private const string RECIPIENT_TYPE_ID = "@recipientTypeID";
        /// Parameter for @roomBedID
        /// </summary>
        private const string ROOM_BED_ID = "@roomBedID";
        /// <summary>
        /// Parameter for @messageID
        /// </summary>
        private const string MESSAGE_ID = "@messageID";
        /// <summary>
        /// Parameter for @newMessageID
        /// </summary>
        private const string NEW_MESSAGE_ID = "@newMessageID";

        /// <summary>
        /// Parameter for @roleId
        /// </summary>
        private const string ROLE_ID = "@roleId";

        /// <summary>
        /// Parameter for @accession
        /// </summary>
        private const string ACCESSION = "@accession";


        /// <summary>
        /// Parameter for @isDeptMsg
        /// </summary>
        private const string REQ_READBACK = "@requireReadback";

        /// <summary>
        /// Parameter for @isDeptMsg
        /// </summary>
        private const string IS_DEPT_MSG = "@isDeptMsg";

        /// <summary>
        /// Parameter for @isOrgDeptMsg
        /// </summary>
        private const string IS_ORG_DEPT_MSG = "@isOrgDeptMsg";        
        #endregion

        /// <summary>
        /// This method will fire "getDirectoryInfoForSubscriber" stored procedure and will save the resultset
        /// to page variable or viewstate when the page loads for the first time.
        /// </summary>
        public int GetDirectoryID(int subscriberID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter(SUBSCRIBER_ID, SqlDbType.Int);
                sqlParams[0].Value = subscriberID;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_DIRECTORY_ID, sqlParams);
                int directoryID = 0;
                if(reader.Read())
                {
                    directoryID = (int)reader["DirectoryID"];
                }
                reader.Close();
                return directoryID;
            }
        }

        /// <summary>
        /// This method fetches Records of all Physicians and loads it in Datatable.
        /// </summary>
        /// <param name="directoryID"></param>
        /// <returns></returns>
        public DataTable GetPhysiciansForDirectoryID(int directoryID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter(DIRECTORY_ID, SqlDbType.Int);
                sqlParams[0].Value = directoryID;
                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_CLINICIAN_FOR_DIRECTORY, sqlParams);
                DataTable dtClinicians = new DataTable();
                dtClinicians.Columns.Add("RowNumber");
                dtClinicians.Columns["RowNumber"].AutoIncrement = true;
                dtClinicians.Load(reader,System.Data.LoadOption.PreserveChanges);
                reader.Close();
                return dtClinicians;
            }

        }

        /// <summary>
        /// Forwards the message to the selected OC inserts new record in Messages table depending upon role of user and returns the id for new record.
        /// </summary>
        /// <param name="objMsgInfo"></param>
        /// <returns></returns>
        public int ForwardMessage(MessageInfo objMsgInfo, int departmentMessage, int roleID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[19];

                sqlParams[0] = new SqlParameter(SPECIALIST_ID, SqlDbType.Int);
                sqlParams[0].Value = objMsgInfo.SpecialistID;

                sqlParams[1] = new SqlParameter(RECIPIENT_ID, SqlDbType.Int);
                sqlParams[1].Value = objMsgInfo.RecepientID;

                sqlParams[2] = new SqlParameter(PATIENT_VOICE_URL, SqlDbType.VarChar, 150);
                sqlParams[2].Value = objMsgInfo.PatientVoiceURL;

                sqlParams[3] = new SqlParameter(IMPRESSION_VOICE_URL, SqlDbType.VarChar, 150);
                sqlParams[3].Value = objMsgInfo.ImpressionVoiceURL;

                sqlParams[4] = new SqlParameter(FINDING_ID, SqlDbType.Int);
                sqlParams[4].Value = objMsgInfo.FindingID;

                sqlParams[5] = new SqlParameter(DID, SqlDbType.VarChar, 25);
                sqlParams[5].Value = objMsgInfo.DID;

                sqlParams[6] = new SqlParameter(MRN, SqlDbType.VarChar, 50);
                sqlParams[6].Value = objMsgInfo.MRN;

                sqlParams[7] = new SqlParameter(DOB, SqlDbType.DateTime);
                if(objMsgInfo.DOB != DateTime.MinValue)                
                    sqlParams[7].Value = objMsgInfo.DOB;
                else
                    sqlParams[7].Value = null;

                sqlParams[8] = new SqlParameter(FORWARD, SqlDbType.Bit);
                sqlParams[8].Value = Convert.ToByte(objMsgInfo.Forward);

                sqlParams[9] = new SqlParameter(ORIGINAL_MSG_ID, SqlDbType.Int);
                sqlParams[9].Value = objMsgInfo.OriginalMessageID;

                sqlParams[10] = new SqlParameter(ACCESSION, SqlDbType.VarChar,150);
                sqlParams[10].Value = objMsgInfo.Accession;

                sqlParams[11] = new SqlParameter(SUBSCRIBER_ID, SqlDbType.Int);
                sqlParams[11].Value = objMsgInfo.SubscriberID;

                sqlParams[12] = new SqlParameter(RECIPIENT_TYPE_ID, SqlDbType.Int);
                sqlParams[12].Value = objMsgInfo.RecepientTypeID;

                sqlParams[13] = new SqlParameter(ROOM_BED_ID, SqlDbType.Int);
                sqlParams[13].Value = objMsgInfo.RoomBedID;

                sqlParams[14] = new SqlParameter(ROLE_ID, SqlDbType.Int);
                sqlParams[14].Value = roleID;

                sqlParams[15] = new SqlParameter(MESSAGE_ID, SqlDbType.Int);
                sqlParams[15].Direction = ParameterDirection.ReturnValue;

                sqlParams[16] = new SqlParameter(REQ_READBACK, SqlDbType.Bit);
                sqlParams[16].Value = objMsgInfo.NeedReadback;

                sqlParams[17] = new SqlParameter(IS_ORG_DEPT_MSG, SqlDbType.Bit);
                sqlParams[17].Value = departmentMessage;
                
                sqlParams[18] = new SqlParameter(IS_DEPT_MSG, SqlDbType.Bit);
                sqlParams[18].Value = objMsgInfo.IsFwdToDept;

                SqlHelper.ExecuteScalar(cnx, CommandType.StoredProcedure, SP_INSERT_MSG_DETAILS, sqlParams);
                object result = sqlParams[15].Value;
                int fwdMessageID = Convert.ToInt32(result);

                if(roleID != 1 && roleID != 2)
                {
                    sqlParams = new SqlParameter[2];

                    sqlParams[0] = new SqlParameter(MESSAGE_ID, SqlDbType.Int);
                    sqlParams[0].Value = objMsgInfo.OriginalMessageID;

                    sqlParams[1] = new SqlParameter(NEW_MESSAGE_ID, SqlDbType.Int);
                    sqlParams[1].Value = fwdMessageID;
                    SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_FWD_LAB_TESTS, sqlParams);
                }
                return fwdMessageID;
            }
        }

        /// <summary>
        /// This method is used to delete the new message inserted in the Message table. 
        /// This method is called in case notification fails for primary physician or 
        /// there is exception after inserting the message in database.
        /// </summary>
        /// <param name="msgId">Message Id</param>
        /// <returns>void</returns>
        public void RollbackTransaction(int msgId, int roleID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
           {
              SqlParameter[] sqlParams = new SqlParameter[2]; 
              sqlParams[0] = new SqlParameter(MESSAGE_ID, SqlDbType.Int); 
              sqlParams[0].Value = msgId;
              sqlParams[1] = new SqlParameter(ROLE_ID, SqlDbType.Int);
              sqlParams[1].Value = roleID;             
              SqlHelper.ExecuteNonQuery(cnx,CommandType.StoredProcedure,SP_ROLLBACK,sqlParams);                
            }            
        }
    }
}
