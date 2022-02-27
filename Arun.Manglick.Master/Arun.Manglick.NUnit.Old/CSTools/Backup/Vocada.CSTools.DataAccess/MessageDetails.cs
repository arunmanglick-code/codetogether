#region File History

/******************************File History***************************
 * File Name        : Vocada.Veriphy.BusinessClasses/LabMessageDetails.cs
 * Author           : Indrajeet K
 * Created Date     : 28-Aug-07
 * Purpose          : To take care all Database transactions for the tab Message Details.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 03-12-2007 - Prerak  - Call getOpenConnection function from Utility Class.
 * 28-12-2007 - Jeeshan - Added MarkReadbackAcceptReject function for Accept/Reject Readback.
 * 03-03-2008 - IAK     - Message Note: New column createdBySystem added
 * 03-06-2008 - IAK     - Message Note: insertMessageNote() parameter NoteType passed
 * 03-06-2008 - IAK     - Message Note: insertMessageNote() parameter NoteType made INT
 * 09-07-2008 - Prerak  - Modified GetNotificationsAvailable() method for implementing CR #258
 * 20-09-2008 - Raju G  - Added function GetActualNotificationMessage() method for Message Details Page
 * ------------------------------------------------------------------- 
 *                          
 */
#endregion

#region Using Block
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Vocada.VoiceLink.DataAccess;
using Vocada.CSTools.Common;
#endregion

namespace Vocada.CSTools.DataAccess
{
    /// <summary>
    /// Business layer class to take care for Message Details Screen functionality.
    /// </summary>
    public class MessageDetails
    {
        #region Stored Procedure
        /// <summary>
        /// SP for VOC_VW_validateUserForMessage
        /// </summary>
        private const string SP_VALIDATE_USER = "dbo.VOC_CST_validateUserForMessage";
        
        /// <summary>
        /// SP for VOC_VW_getMessageDetails
        /// </summary>
        private const string SP_GET_LABMSG_DETAILS = "dbo.VOC_CST_getMessageDetails";

        /// <summary>
        /// SP for VOC_VW_GetForwardMessageDetails
        /// </summary>
        private const string SP_GET_FWD_MSGS = "dbo.VOC_CST_getForwardMessageDetails";

        /// <summary>
        /// SP for VOC_VL_getMessageTestResults
        /// </summary>
        private const string SP_GET_MSG_TEST_RESULTS = "dbo.VOC_VL_getMessageLabResults";

        /// <summary>
        /// SP for VOC_VW_getNotificationHistoryForMessage
        /// </summary>
        private const string SP_GET_LAB_NOTI_HISTORY = "dbo.VOC_CST_getNotificationHistoryForMessage";

        /// <summary>
        /// SP for VOC_VW_getNotificationsForRecipientResend
        /// </summary>
        private const string SP_GET_NOTI_RECEPIENT_RESEND = "dbo.VOC_CST_getNotificationsForRecipientResend";

        /// <summary>
        /// SP for VOC_VW_getMessageNotes
        /// </summary>
        private const string SP_GET_MSG_NOTES = "dbo.VOC_CST_getMessageNotes";

        /// <summary>
        /// SP for Voc_VW_insertMessageNote
        /// </summary>
        private const string SP_INSERT_MSG_NOTE = "dbo.Voc_CST_insertMessageNote";

        /// <summary>
        /// SP for VOC_VLR_markMessageReadback
        /// </summary>
        private const string SP_MARK_ACCEPT_REJECT_READBACK = "dbo.VOC_VLR_markMessageReadback";

        /// <summary>
        /// SP used to get actual notification message
        /// </summary>
        private const string SP_GET_ACTUALNOTIFICATIONMESSAGE = "dbo.VOC_getActualNotificationMessage";

        #endregion

        #region Parameter Constant
        /// <summary>
        /// Parameter for @messageID
        /// </summary>
        private const string MESSAGE_ID = "@messageID";
        /// <summary>
        /// Parameter for @subscriberID
        /// </summary>
        private const string SUBSCRIBER_ID = "@subscriberID";
        /// <summary>
        /// Parameter for @author
        /// </summary>
        private const string AUTHOR_NAME = "@author";
        /// <summary>
        /// Parameter for @note
        /// </summary>
        private const string NOTE_TEXT = "@note";

        /// <summary>
        /// Parameter for @roleId
        /// </summary>
        private const string ROLE_ID = "@roleId";

        /// <summary>
        /// Parameter for @roleId
        /// </summary>
        private const string GROUP_ID = "@groupID";

        /// <summary>
        /// Parameter for @isDeptMsg
        /// </summary>
        private const string IS_DEPT_MSG = "@isDeptMsg";

        /// <summary>
        /// Parameter for @isDeptMsg
        /// </summary>
        private const string IS_LAB_MSG = "@isLabMsg";

        /// <summary>
        /// Parameter for @readbackID
        /// </summary>
        private const string READBACK_ID = "@readbackID";

        /// <summary>
        /// Parameter for @accepted
        /// </summary>
        private const string IS_ACCEPTED = "@accepted";

        /// <summary>
        /// Parameter for @rejected
        /// </summary>
        private const string IS_REJECTED = "@rejected";

        /// <summary>
        /// Parameter for @rejectedText
        /// </summary>
        private const string REJECTED_TEXT = "@rejectedText";

        /// <summary>
        /// Parameter for MSG_NOTE_TYPE
        /// </summary>
        private const string MSG_NOTE_TYPE = "@noteType";

        /// <summary>
        /// Parameter for SP Input NotificationHistoryID
        /// </summary>
        private const string NOTIFICATION_HISTORYID = "@notificationHistoryID";

        /// <summary>
        /// MessageType for sp input
        /// </summary>
        private const string MESSAGE_TYPE = "@messageType";

        #endregion

        #region Public Methods
        /// <summary>
        /// This function Ensure that the messageID is valid for the requesting user/group
        /// This function calls stored procedure "validateUserForMessage"
        /// </summary>
        /// <param name="messageID">Integer Type</param>
        /// <param name="cnx">Connection String</param>
        public bool ValidateUserAgainstMessage(int messageID, int departmentMessage, int isLabMessage)
        {
            bool validUser = false;
            using(SqlConnection cnx = Utility.getOpenConnection()) 
            {   
                SqlParameter[] sqlParams = new SqlParameter[3];
                sqlParams[0] = new SqlParameter(MESSAGE_ID, SqlDbType.Int);
                sqlParams[0].Value = messageID;

                sqlParams[1] = new SqlParameter(IS_LAB_MSG, SqlDbType.Int);
                sqlParams[1].Value = isLabMessage;
                
                sqlParams[2] = new SqlParameter(IS_DEPT_MSG, SqlDbType.Bit);
                sqlParams[2].Value = departmentMessage;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx,CommandType.StoredProcedure,SP_VALIDATE_USER,sqlParams);
                
                if(reader.Read())
                {
                    int messageGroup = 0;
                    int subscriberGroup = -1;

                    if(reader["MessageGroup"] != null)
                        messageGroup = (int)reader["MessageGroup"];

                    if (reader["SubscriberGroup"] != null)
                        subscriberGroup = (int)reader["SubscriberGroup"];

                    if(messageGroup == subscriberGroup)
                        validUser = true;
                }
                reader.Close();
                return validUser;
            }
            
        }

        /// <summary>
        /// This function get Message information about selected message
        /// This function calls stored procedure "VOC_VLR_GetMessageDetails"
        /// This function binds data into datagrid dgMessage
        /// </summary>
        /// <param name="cnx"></param>
        /// <param name="messageID"></param>
        public DataTable GetMessageDetails(int messageID, int departmentMessage, int isLabMessage)
        {
            using(SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtMsgDetails = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[3];
                sqlParams[0] = new SqlParameter(MESSAGE_ID, SqlDbType.Int);
                sqlParams[0].Value = messageID;

                sqlParams[1] = new SqlParameter(IS_LAB_MSG, SqlDbType.Int);
                sqlParams[1].Value = isLabMessage;

                sqlParams[2] = new SqlParameter(IS_DEPT_MSG, SqlDbType.Bit);
                sqlParams[2].Value = departmentMessage;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx,CommandType.StoredProcedure,SP_GET_LABMSG_DETAILS,sqlParams);
                dtMsgDetails.Load(reader);
                reader.Close();
                return dtMsgDetails;

            }            
        }

        /// <summary>
        /// This function get Message information about selected message
        /// This function calls stored procedure "VOC_VLR_GetMessageDetails"
        /// This function binds data into datagrid dgMessage
        /// </summary>
        /// <param name="cnx"></param>
        /// <param name="messageID"></param>
       // public SqlDataReader GetForwardedMessages(int messageID)
        public DataTable GetForwardedMessages(int messageID, int departmentMessage, int isLabMessage)
        {                  
            using(SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtMessages = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[3];
                sqlParams[0] = new SqlParameter(MESSAGE_ID, SqlDbType.Int);
                sqlParams[0].Value = messageID;
                
                sqlParams[1] = new SqlParameter(IS_LAB_MSG, SqlDbType.Int);
                sqlParams[1].Value = isLabMessage;

                sqlParams[2] = new SqlParameter(IS_DEPT_MSG, SqlDbType.Bit);
                sqlParams[2].Value = departmentMessage;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_FWD_MSGS, sqlParams);
                dtMessages.Load(reader);
                reader.Close();
                return dtMessages;
            }            
        }

        /// <summary>
        /// This function gets Test Results for specified messageID
        /// This function calls stored procedure for "VOC_VL_getMessageTestResults"
        /// </summary>
        /// <param name="cnx">Connection String</param>
        /// <param name="messageID">integer Type</param>
        public DataTable GetMessageTestResults(int messageID)
        {
            using(SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter(MESSAGE_ID, SqlDbType.Int);
                sqlParams[0].Value = messageID;
                SqlDataReader reader = SqlHelper.ExecuteReader(cnx,CommandType.StoredProcedure,SP_GET_MSG_TEST_RESULTS,sqlParams);
                DataTable dtTestResult = new DataTable();
                dtTestResult.Load(reader);
                return dtTestResult;
            }
        }

        /// <summary>
        /// This function gets History details for Message Activity Log
        /// This function calls stored procedure for "getNotificationHistoryForMessage"
        /// </summary>
        /// <param name="cnx">Connection String</param>
        /// <param name="messageID">integer Type</param>
        public DataTable GetMessageActivityHistory(int messageID, int departmentMessage, int isLabMessage, int groupID)
        {
            using(SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[4];
                sqlParams[0] = new SqlParameter(MESSAGE_ID, SqlDbType.Int);
                sqlParams[0].Value = messageID;

                sqlParams[1] = new SqlParameter(IS_LAB_MSG, SqlDbType.Int);
                sqlParams[1].Value = isLabMessage;

                sqlParams[2] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                sqlParams[2].Value = groupID;

                sqlParams[3] = new SqlParameter(IS_DEPT_MSG, SqlDbType.Bit);
                sqlParams[3].Value = departmentMessage;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx,CommandType.StoredProcedure,SP_GET_LAB_NOTI_HISTORY,sqlParams);
                DataTable dtNotifyHistory = new DataTable();
                dtNotifyHistory.Load(reader);
                return dtNotifyHistory;
            }
        }

        /// <summary>
        /// This function gets Notification Events related to selected Message
        /// This function calls stored procedure "getNotificationsForRPResend"
        /// </summary>
        /// <param name="cnx">Connection String</param>
        /// <param name="messageID">Integer Type</param>
        public ArrayList GetNotificationsAvailable(int messageID, int departmentMessage, int isLabMessage, int groupID)
        {
            using(SqlConnection cnx = Utility.getOpenConnection())
            {
                ArrayList alLabUnit = new ArrayList();
                LabUnitObject objLabUnit = null;
                SqlParameter[] sqlParams = new SqlParameter[4];
                sqlParams[0] = new SqlParameter(MESSAGE_ID, SqlDbType.Int);
                sqlParams[0].Value = messageID;

                sqlParams[1] = new SqlParameter(IS_LAB_MSG, SqlDbType.Int);
                sqlParams[1].Value = isLabMessage;

                sqlParams[2] = new SqlParameter(IS_DEPT_MSG, SqlDbType.Bit);
                sqlParams[2].Value = departmentMessage;

                sqlParams[3] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                sqlParams[3].Value = groupID;

                string text = string.Empty;
                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_NOTI_RECEPIENT_RESEND, sqlParams);
                while(reader.Read())  // add all secondary notification types.
                {
                    objLabUnit = new LabUnitObject();
                    text = reader["EventDescription"] + " - " + reader["DeviceName"] + " - " + expandPhoneNumber(reader["DeviceAddress"].ToString());                    
                    switch (Convert.ToInt32(reader["OCDevice"]))
                    {
                        case 1: text += " [OC]";
                            break;
                        case 2: text += " [UNIT]";
                            break;
                        case 3: text += " [BED]";
                            break;
                        case 4: text += " [CT]";
                            break;
                    }
                    objLabUnit.FieldID = Convert.ToInt32(reader["RPNotificationID"]);
                    objLabUnit.FieldName = text;
                    if(text.Length > 0)
                    {
                        alLabUnit.Add(objLabUnit);
                    }
                }
                return alLabUnit;
            }
        }

        /// <summary>
        /// This function is get all information about Message Notes history for selected Messages
        /// This function calls stored procedure "getMessageNotes"
        /// </summary>
        /// <param name="cnx"></param>
        /// <param name="messageID"></param>
        public DataTable GetMessageNotes(int messageID, int departmentMessage, int isLabMessage)
        {
            using(SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[3];
                sqlParams[0] = new SqlParameter(MESSAGE_ID, SqlDbType.Int);
                sqlParams[0].Value = messageID;

                sqlParams[1] = new SqlParameter(IS_LAB_MSG, SqlDbType.Int);
                sqlParams[1].Value = isLabMessage;

                sqlParams[2] = new SqlParameter(IS_DEPT_MSG, SqlDbType.Bit);
                sqlParams[2].Value = departmentMessage;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_MSG_NOTES, sqlParams);
                DataTable dtNotes = new DataTable();
                dtNotes.Load(reader);
                return dtNotes;
            }
        }

        /// <summary>
        /// Inserts Note with the given parameters in VOC_VL_MessageNotes table.
        /// </summary>
        /// <param name="messageID"></param>
        /// <param name="author"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public int InsertMessageNote(int messageID, int departmentMessage, string author, string note, int groupID, int isLabMessage, MessageNoteType messageNoteType)
        {
            using(SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[7];
                sqlParams[0] = new SqlParameter(MESSAGE_ID, SqlDbType.Int);
                sqlParams[0].Value = messageID;

                sqlParams[1] = new SqlParameter(AUTHOR_NAME, SqlDbType.VarChar);
                sqlParams[1].Value = author;

                sqlParams[2] = new SqlParameter(NOTE_TEXT, SqlDbType.VarChar);
                sqlParams[2].Value = note;

                sqlParams[3] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                sqlParams[3].Value = groupID;

                sqlParams[4] = new SqlParameter(IS_DEPT_MSG, SqlDbType.Bit);
                sqlParams[4].Value = departmentMessage;

                sqlParams[5] = new SqlParameter(IS_LAB_MSG, SqlDbType.Int);
                sqlParams[5].Value = isLabMessage;

                sqlParams[6] = new SqlParameter(MSG_NOTE_TYPE, SqlDbType.Int);
                sqlParams[6].Value = messageNoteType.GetHashCode();

               int result = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_INSERT_MSG_NOTE, sqlParams);
               return result;
            }

        }

        /// <summary>
        /// This function marks the readback as Accepted/Rejected.
        /// This function calls stored procedure "VOC_VLR_markMessageReadback"
        /// </summary>
        /// <param name="readbackID"></param>
        /// <param name="isAccepted"></param>
        /// <param name="isRejected"></param>
        /// <param name="rejectedText"></param>
        /// <param name="isDeptMsg"></param>
        /// <returns></returns>
        public int MarkReadbackAcceptReject(int readbackID, int isAccepted, int isRejected, string rejectedText, int isDeptMsg)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[5];

                sqlParams[0] = new SqlParameter(READBACK_ID, SqlDbType.Int);
                sqlParams[0].Value = readbackID;

                sqlParams[1] = new SqlParameter(IS_ACCEPTED, SqlDbType.Bit);
                sqlParams[1].Value = isAccepted;

                sqlParams[2] = new SqlParameter(IS_REJECTED, SqlDbType.Bit);
                sqlParams[2].Value = isRejected;

                sqlParams[3] = new SqlParameter(REJECTED_TEXT, SqlDbType.VarChar);
                sqlParams[3].Value = rejectedText;

                sqlParams[4] = new SqlParameter(IS_DEPT_MSG, SqlDbType.Bit);
                sqlParams[4].Value = isDeptMsg;

                int result = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_MARK_ACCEPT_REJECT_READBACK, sqlParams);
                return result;
            }

        }

        /// <summary>
        /// Get Actual message sent to the recipient
        /// </summary>
        /// <param name="notificationHistoryID"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public DataTable GetActualNotificationMessage(int notificationHistoryID,int messageType)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[2];
                sqlParams[0] = new SqlParameter(NOTIFICATION_HISTORYID, SqlDbType.Int);
                sqlParams[0].Value = notificationHistoryID;
                sqlParams[1] = new SqlParameter(MESSAGE_TYPE, SqlDbType.Int);
                sqlParams[1].Value = messageType;

                SqlDataReader drActualNotificationMessaage = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_ACTUALNOTIFICATIONMESSAGE, sqlParams);
                DataTable dtActualNotificationMessaage = new DataTable();
                dtActualNotificationMessaage.Load(drActualNotificationMessaage);
                return dtActualNotificationMessaage;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// This method returns the phonenumber string appending delimiter character after Phone prefix and Phone number.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        private string expandPhoneNumber(string phoneNumber)
        {
            if(phoneNumber.Length < 2)
                return phoneNumber;

            return Regex.Replace(phoneNumber, "(\\d{3})(\\d{3})(\\d{4})", "($1) $2-$3");
        }
        #endregion
    }
}
