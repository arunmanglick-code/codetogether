#region File History

/******************************File History***************************
 * File Name        : Vocada.Veriphy.BusinessClasses/LabMarkAsReceived.cs
 * Author           : Swapnil K
 * Created Date     : 15-Feb-07
 * Purpose          : To take care all Database transactions for the tab Mark As Received screen.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification
 * ------------------------------------------------------------------- 
 *  06-09-2007       SSK         Close the Primary / BackupMessage if one of the Message is closed and Group Setting for Close both Message is ON.
 *  12-03-2007       Prerak      Call getOpenConnection function from Utility Class.
 *  01-11-2008       IAK         Modified MarkMessageReceived readBy value for sp is corrected
 *  03-03-2008       IAK         Message Note: New column createdBySystem added
 *  03-06-2008       IAK         Message Note: insertMessageNote() parameter NoteType passed
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
    /// Business layer class for Mark Message Received screen
    /// </summary>
    public class LabMarkAsReceived
    {
        #region Stored Procedure

        /// <summary>
        /// SP for VOC_VW_getMessageReply
        /// </summary>
        private const string SP_GET_MESSAGE_REPLY = "dbo.VOC_CST_getMessageReply";

        /// <summary>
        /// SP for VOC_VW_markMessageRead
        /// </summary>
        private const string SP_MARK_MSG_RECEIVED = "dbo.VOC_CST_markMessageRead";

        /// <summary>
        /// SP for VOC_VW_markReplyRead
        /// </summary>
        private const string SP_MARK_REPLY_RECEIVED = "dbo.VOC_CST_markReplyRead";

        /// <summary>
        /// SP for VOC_VD_getBackupMessageID
        /// </summary>
        private const string SP_GET_BACKUP_MESSAGE_ID = "dbo.VOC_CST_getBackupMessageID";
        #endregion

        #region Parameter Constant
        /// <summary>
        /// Parameter for @messageReplyID
        /// </summary>
        private const string MESSAGE_REPLY_ID = "@messageReplyID";
        /// <summary>
        /// Parameter for @messageID
        /// </summary>
        private const string MESSAGE_ID = "@messageID";
        /// <summary>
        /// Parameter for @msgClosedWhileForward
        /// </summary>
        private const string MSG_CLOSED_WHILE_FORWARD = "@msgClosedWhileForward";
        /// <summary>
        /// Parameter for @readBy
        /// </summary>
        private const string READ_BY = "@readBy";
        /// <summary>
        /// Parameter for @readComment
        /// </summary>
        private const string READ_COMMENT = "@readComment";

        /// <summary>
        /// Parameter for @groupId
        /// </summary>
        private const string GROUP_ID = "@groupId";

        /// <summary>
        /// Parameter for @roleId
        /// </summary>
        private const string ROLE_ID = "@roleId";

        /// <summary>
        /// Parameter for @roleId
        /// </summary>
        private const string IS_LAB_MSG = "@isLabMsg";

        /// <summary>
        /// Parameter for @isDeptMsg
        /// </summary>
        private const string IS_DEPT_MSG = "@isDeptMsg";
        #endregion

        /// <summary>
        /// This function is called while page load occurs.
        /// This function fills Message Detail Reply information, which Message Reply will be marked as 'Message Read' and set Reason for that Message Reply ID
        /// This function calls stored procedure "getMessageReply"
        /// </summary>
        /// <param name="messageReplyID">Integer Type</param>
        public DataTable GetMessageReplyDetails(int MessageReplyId, int departmentMessage, int isLabMessage)
        {
            using(SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtMsgDetails = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[3];
                sqlParams[0] = new SqlParameter(MESSAGE_REPLY_ID, SqlDbType.Int);
                sqlParams[0].Value = MessageReplyId;

                sqlParams[1] = new SqlParameter(IS_LAB_MSG, SqlDbType.Int);
                sqlParams[1].Value = isLabMessage;

                sqlParams[2] = new SqlParameter(IS_DEPT_MSG, SqlDbType.Bit);
                sqlParams[2].Value = departmentMessage;

                SqlDataReader reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_MESSAGE_REPLY, sqlParams);
                dtMsgDetails.Load(reader);
                reader.Close();
                return dtMsgDetails;
            }
        }


        /// <summary>
        /// This function updates Message Or ReplyMessage with Read details in the respective table depending
        /// upon parameter isReplyOpen, it also inserts entry into VOC_VL_MessageNotes table. 
        /// </summary>
        /// <param name="MessageID"></param>
        /// <returns></returns>
        public int MarkMessageReceived(int MessageID, int departmentMessage, bool isReplyOpen, string userName, string readBy, string comment, bool isForwardClosed, int isLabMessage, int groupID)
        {
            using(SqlConnection cnx = Utility.getOpenConnection())
            {
                string commandText = string.Empty;
                SqlParameter[] sqlParams = null;
                if(!isReplyOpen || isForwardClosed)    // top-level message marked as received
                {
                    sqlParams = new SqlParameter[6];
                    commandText = SP_MARK_MSG_RECEIVED;
                    sqlParams[0] = new SqlParameter(MESSAGE_ID, SqlDbType.Int);
                    sqlParams[0].Value = MessageID;

                    sqlParams[4] = new SqlParameter(IS_LAB_MSG, SqlDbType.Int);
                    sqlParams[4].Value = isLabMessage;

                    
                    sqlParams[5] = new SqlParameter(IS_DEPT_MSG, SqlDbType.Bit);
                    sqlParams[5].Value = departmentMessage;

                    sqlParams[3] = new SqlParameter(MSG_CLOSED_WHILE_FORWARD, SqlDbType.Bit);
                    if(isForwardClosed)
                    {
                        sqlParams[3].Value = 1;
                    }
                    else
                    {
                        sqlParams[3].Value = 0;
                    }
                }
                else
                {
                    sqlParams = new SqlParameter[5];
                    commandText = SP_MARK_REPLY_RECEIVED;
                    sqlParams[0] = new SqlParameter(MESSAGE_REPLY_ID, SqlDbType.Int);
                    sqlParams[0].Value = MessageID;

                    sqlParams[3] = new SqlParameter(IS_LAB_MSG, SqlDbType.Int);
                    sqlParams[3].Value = isLabMessage;

                    sqlParams[4] = new SqlParameter(IS_DEPT_MSG, SqlDbType.Bit);
                    sqlParams[4].Value = departmentMessage;

                }

                sqlParams[1] = new SqlParameter(READ_BY, SqlDbType.VarChar, 50);
                sqlParams[1].Value = readBy;

                sqlParams[2] = new SqlParameter(READ_COMMENT, SqlDbType.VarChar, 255);
                sqlParams[2].Value = comment;

               

                int result = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, commandText, sqlParams);

                if(result > 0)
                {
                    MessageDetails objLabMsgDetails = new MessageDetails();
                    result = objLabMsgDetails.InsertMessageNote(MessageID, departmentMessage, readBy, comment, groupID,  isLabMessage, MessageNoteType.Default);
                }
                return result;
            }
        }

        /// <summary>
        /// Returns Whether the selected message is Primary Message for Department.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        private int isPrimaryDeptMessage(int messageId)
        {
            try
            {
                int backupMessageID = 0;
                int PrimaryDeptMsg = 0;
                using(SqlConnection cnx = Utility.getOpenConnection())
                {
                    string cmdText = "SELECT PrimaryMessageID FROM VOC_Dept_BackupMessages WHERE BackupMessageID =" + messageId;
                    object objBackupMessageID = SqlHelper.ExecuteScalar(cnx, CommandType.Text, cmdText);
                    if(objBackupMessageID != null)
                    {
                        backupMessageID = Convert.ToInt32(objBackupMessageID);
                        if(backupMessageID > 0)
                            PrimaryDeptMsg = 1;
                    }
                }
                return PrimaryDeptMsg;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// This functions gets the Primary  / Backup Message id for the given Message ID.
        /// Calls the MarkMessageReceived method for the backupMessageID and the given parameters.
        /// </summary>
        /// <param name="MessageID"></param>
        /// <param name="isReplyOpen"></param>
        /// <param name="userName"></param>
        /// <param name="readBy"></param>
        /// <param name="comment"></param>
        /// <param name="isForwardClosed"></param>
        /// <param name="isLabMessage"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public int CloseBackupMessage(int MessageID, int departmentMessage, bool isReplyOpen, string userName, string readBy, string comment, bool isForwardClosed, int isLabMessage, int groupID)
        {
            DataTable dt = null;
            DataRow dr = null;
            MessageDetails objMessageDetails = null;
            try
            {
                using(SqlConnection cnx = Utility.getOpenConnection())
                {
                    SqlParameter[] sqlParams = new SqlParameter[3];

                    sqlParams[0] = new SqlParameter(MESSAGE_ID, SqlDbType.Int);
                    sqlParams[0].Value = MessageID;

                    sqlParams[1] = new SqlParameter(IS_LAB_MSG, SqlDbType.Int);
                    sqlParams[1].Value = isLabMessage;

                    sqlParams[2] = new SqlParameter(IS_DEPT_MSG, SqlDbType.Bit);
                    sqlParams[2].Value = departmentMessage;

                    int result = 0;
                    int backupMessageId = 0;
                    object backupMessage = SqlHelper.ExecuteScalar(cnx, CommandType.StoredProcedure, SP_GET_BACKUP_MESSAGE_ID, sqlParams);

                    if(backupMessage != null)
                        backupMessageId = Convert.ToInt32(backupMessage);
                    
                    if(departmentMessage == 1)
                        departmentMessage = 0;

                    if(isLabMessage == 0)
                        departmentMessage = isPrimaryDeptMessage(MessageID);

                    if(backupMessageId > 0)
                    {
                        objMessageDetails = new MessageDetails();
                        dt = objMessageDetails.GetMessageDetails(backupMessageId, departmentMessage, isLabMessage);
                        dr = dt.Rows[0];
                        if(dr["ReadOn"] == System.DBNull.Value)
                        {
                            result = MarkMessageReceived(backupMessageId, departmentMessage, isReplyOpen, userName, readBy, comment, isForwardClosed, isLabMessage, groupID);
                        }
                        else
                        {
                            backupMessageId = 0;
                        }
                    }
                    return backupMessageId;
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                dr = null;
                dt = null;
                objMessageDetails = null;
            }
        }
    }         
     
}
