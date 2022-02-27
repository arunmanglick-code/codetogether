#region File History
/******************************File History***************************
 * File Name        : NotificationTemplate.cs
 * Author           : Raju Gupta
 * Created Date     : 8 Sep 2008
 * Purpose          : This class is used to provide business logics for 
 *                    Custom Notification Templates
 *                  : 
 *                  :
 * *********************File Modification History*********************
 * Date(mm-dd-yyyy) Developer Reason of Modification
 * 09-29-2008       Prerak      Added MsgSendType parameter in UpdateCustomNotificationTemplate() and 
 *                              AddCustomNotificationTemplate() 
 * 10-04-2008       Prerak      Added OVERWRITE parameter in UpdateCustomNotificationTemplate() 
 * 10-10-2008       Raju G      Added one more parameter @groupID for devices - Issue #3891 (Hide SMS link for Rad Groups)

 * ------------------------------------------------------------------- 
 */
#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Vocada.VoiceLink.DataAccess;
using Vocada.CSTools.Common;
#endregion

namespace Vocada.CSTools.DataAccess
{
    /// <summary>
    /// This class is used to provide business logics for 
    /// Custom Notification Templates.
    /// This class contains methods for fetching recipients, recipient devices, recipient events, 
    /// Notification Templates, Notification template Fields etc.
    /// This class also contains methods for Adding, Updating, deleting 
    /// and checking duplicate for custom notification templates.
    /// 
    /// </summary>
    public class NotificationTemplate
    {
        #region Stored Procedure Constants

        /// <summary>
        /// This constant stores name of stored procedure which will add new Custom Notification information in Database
        /// </summary>
        private const string SP_INSERT_CUSTOM_NOTIFICATION = "dbo.VOC_insertCustomNotificationTemplate";

        /// <summary>
        /// This constant stores name of stored procedure which will add update Custom Notification information in Database
        /// </summary>
        private const string SP_UPDATE_CUSTOM_NOTIFICATION = "dbo.VOC_updateCustomNotificationTemplate";

        /// <summary>
        /// This constant stores name of stored procedure which will delete Custom Notification information in Database
        /// </summary>
        private const string SP_DELETE_CUSTOM_NOTIFICATION = "dbo.VOC_DeleteCustomNotificationTemplate";

        /// <summary>
        /// This constant stores name of stored procedure which will get recipient types from Database
        /// </summary>
        private const string SP_GET_RECIPIENT_TYPES = "dbo.VOC_getNotificationRecipientTypes";

        /// <summary>
        /// This constant stores name of stored procedure which will get custom notification template list from Database
        /// </summary>
        private const string SP_GET_CUSTOM_NOTIFICATION_LIST = "dbo.VOC_getCustomNotificationTemplateList";

        /// <summary>
        /// This constant stores name of stored procedure which will get notification devices for a recipient from Database
        /// </summary>
        private const string SP_GET_DEVICES_BY_RECIPIENT = "dbo.VOC_getNotificationRecipientDevices";

        /// <summary>
        /// This constant stores name of stored procedure which will notification events for a recipient from Database
        /// </summary>
        private const string SP_GET_EVENTS_BY_RECIPIENT = "dbo.VOC_getNotificationRecipientEvents";

        /// <summary>
        /// This constant stores name of stored procedure which will get notifiacation template fields list from Database
        /// </summary>
        private const string SP_GET_TEMPLATE_FIELD_LIST = "dbo.VOC_getNotificationTemplateFields";

        /// <summary>
        /// This constant stores name of stored procedure which will get default notifiacation template from Database
        /// </summary>
        private const string SP_GET_DEFAULT_TEMPLATE = "dbo.VOC_getNotificationDefaultTemplate";

        #endregion

        #region Parameter Constants

        /// <summary>
        /// Stored Procedure Parameters
        /// </summary>
        private const string RECIPIENT_ID = "@recipientID";
        private const string GROUP_ID = "@groupID";
        private const string DEVICE_ID = "@deviceID";
        private const string EVENT_ID = "@eventID";
        private const string NOTIFICATION_TEMPLATE_ID = "@templateID";
        private const string SUBJECT_TEXT = "@subjectText";
        private const string BODY_TEXT = "@bodyText";
        private const string FAX_TEMPLATE_URL = "@faxTemplateURL";
        private const string FAX_TEMPLATE_BACKUPURL = "@faxTemplateBkURL";
        private const string CREATED_ON = "@createON";
        private const string LAST_MODIFIED_ON = "@lastModifiedOn";
        private const string RETURN_VAL = "@reutnVal";
        private const string MSG_SEND_TYPE = "@msgSendType";
        private const string OVERWRITE = "@overwrite";
        #endregion

        #region Public Methods

        /// <summary>
        /// This Method is used to fetch different types of Recipients from data base
        /// </summary>
        /// <returns>Data table containing recipients</returns>
        public DataTable GetNotificationRecipientTypes()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtRecipientTypes = new DataTable();

                SqlDataReader drRecipientTypes = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_RECIPIENT_TYPES);
                dtRecipientTypes.Load(drRecipientTypes);
                return dtRecipientTypes;
            }
        }

        /// <summary>
        /// This method is used to fetch devices for given recipient
        /// </summary>
        /// <param name="recipientID">Recipient ID</param>
        /// <param name="groupID">Group ID</param>
        /// <returns>Data table containing Devices</returns>
        public DataTable GetNotificationRecipientDevices(int recipientID,int groupID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtDevices = new DataTable();
                SqlParameter[] arSqlParams = new SqlParameter[2];

                arSqlParams[0] = new SqlParameter(RECIPIENT_ID, SqlDbType.Int);
                arSqlParams[0].Value = recipientID;
                arSqlParams[1] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                arSqlParams[1].Value = groupID;
                SqlDataReader drDevices = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_DEVICES_BY_RECIPIENT, arSqlParams);
                dtDevices.Load(drDevices);
                return dtDevices;
            }
        }

        /// <summary>
        /// This method is used to fetch events fro the given recipient
        /// </summary>
        /// <param name="recipientID"></param>
        /// <returns>Data table containing Events</returns>
        public DataTable GetNotificationRecipientEvents(int recipientID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtEvents = new DataTable();
                SqlParameter[] arSqlParams = new SqlParameter[1];

                arSqlParams[0] = new SqlParameter(RECIPIENT_ID, SqlDbType.Int);
                arSqlParams[0].Value = recipientID;
                SqlDataReader drEvents = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_EVENTS_BY_RECIPIENT, arSqlParams);
                dtEvents.Load(drEvents);
                return dtEvents;
            }
        }

        /// <summary>
        /// This method is used to fetch custom notification templates for a given group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns>Data table containing Custom Notification Templates</returns>
        public DataTable GetCustomNotificationTemplateList(int groupID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtNotifications = new DataTable();
                SqlParameter[] arSqlParams = new SqlParameter[1];

                arSqlParams[0] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                arSqlParams[0].Value = groupID;
                SqlDataReader drNotifications = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_CUSTOM_NOTIFICATION_LIST, arSqlParams);
                dtNotifications.Load(drNotifications);
                return dtNotifications;
            }
        }

        /// <summary>
        /// This method is used to fetch notification template fields.
        /// </summary>
        /// <param name="groupID">Group ID</param>
        /// <param name="deviceID">Device ID</param>
        /// <returns>Data table containing Notification Template Fields</returns>
        public DataTable GetNotificationTemplateFieldList()
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                DataTable dtNotificationFields = new DataTable();
                SqlDataReader drNotificationFields = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_TEMPLATE_FIELD_LIST);
                dtNotificationFields.Load(drNotificationFields);
                return dtNotificationFields;
            }
        }

        /// <summary>
        /// This method is used to add the custom notification template into database.
        /// </summary>
        /// <param name="objNotificationTemplateInfo">Object of NotificationTemplateInfo</param>
        /// <returns>Integer value of added notification template</returns>
        public int AddCustomNotificationTemplate(NotificationTemplateInfo objNotificationTemplateInfo)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] arSqlParams = new SqlParameter[9];

                arSqlParams[0] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                arSqlParams[0].Value = objNotificationTemplateInfo.GroupID;
                arSqlParams[1] = new SqlParameter(RECIPIENT_ID, SqlDbType.Int);
                arSqlParams[1].Value = objNotificationTemplateInfo.RecipientID;
                arSqlParams[2] = new SqlParameter(DEVICE_ID, SqlDbType.Int);
                arSqlParams[2].Value = objNotificationTemplateInfo.DeviceID;
                arSqlParams[3] = new SqlParameter(EVENT_ID, SqlDbType.Int);
                arSqlParams[3].Value = objNotificationTemplateInfo.EventID;
                arSqlParams[4] = new SqlParameter(SUBJECT_TEXT, SqlDbType.VarChar, 70);
                arSqlParams[4].Value = objNotificationTemplateInfo.SubjectText;
                arSqlParams[5] = new SqlParameter(BODY_TEXT, SqlDbType.VarChar, 8000);
                arSqlParams[5].Value = objNotificationTemplateInfo.BodyText;
                arSqlParams[6] = new SqlParameter(FAX_TEMPLATE_URL, SqlDbType.VarChar, 256);
                arSqlParams[6].Value = objNotificationTemplateInfo.FaxTemplateURL;
                arSqlParams[7] = new SqlParameter(MSG_SEND_TYPE, SqlDbType.Int);
                arSqlParams[7].Value = objNotificationTemplateInfo.MessageSendType;
                arSqlParams[8] = new SqlParameter(RETURN_VAL, SqlDbType.Int);
                arSqlParams[8].Direction = ParameterDirection.Output;
                arSqlParams[8].Value = 0;
                
                SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_INSERT_CUSTOM_NOTIFICATION, arSqlParams);
                return Convert.ToInt32(arSqlParams[8].Value);
            }
        }

        /// <summary>
        /// This method is used to update custom notification template
        /// </summary>
        /// <param name="objNotificationTemplateInfo">Object of NotificationTemplateInfo</param>
        /// <returns>True if updated successfully otherwise False</returns>
        public bool UpdateCustomNotificationTemplate(NotificationTemplateInfo objNotificationTemplateInfo, bool overwrite)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] arSqlParams = new SqlParameter[6];

                arSqlParams[0] = new SqlParameter(NOTIFICATION_TEMPLATE_ID, SqlDbType.Int);
                arSqlParams[0].Value = objNotificationTemplateInfo.NotificationTemplateID;
                arSqlParams[1] = new SqlParameter(SUBJECT_TEXT, SqlDbType.VarChar, 70);
                arSqlParams[1].Value = objNotificationTemplateInfo.SubjectText;
                arSqlParams[2] = new SqlParameter(BODY_TEXT, SqlDbType.VarChar, 8000);
                arSqlParams[2].Value = objNotificationTemplateInfo.BodyText;
                arSqlParams[3] = new SqlParameter(FAX_TEMPLATE_URL, SqlDbType.VarChar, 256);
                arSqlParams[3].Value = objNotificationTemplateInfo.FaxTemplateURL;
                arSqlParams[4] = new SqlParameter(MSG_SEND_TYPE, SqlDbType.Int);
                arSqlParams[4].Value = objNotificationTemplateInfo.MessageSendType;
                arSqlParams[5] = new SqlParameter(OVERWRITE, SqlDbType.Bit);
                arSqlParams[5].Value = overwrite; 

                int rowUpdate = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_UPDATE_CUSTOM_NOTIFICATION, arSqlParams);
                if (rowUpdate > 0)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// This method is used to delete custom notification template from database
        /// </summary>
        /// <param name="notificationTemplateID">NotificationTemplateID</param>
        /// <returns>True if template deleted successfully otherwise False</returns>
        public bool DeleteCustomNotificationTemplate(int notificationTemplateID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] arSqlParams = new SqlParameter[1];

                arSqlParams[0] = new SqlParameter(NOTIFICATION_TEMPLATE_ID, SqlDbType.Int);
                arSqlParams[0].Value = notificationTemplateID;

                int rowUpdate = SqlHelper.ExecuteNonQuery(cnx, CommandType.StoredProcedure, SP_DELETE_CUSTOM_NOTIFICATION, arSqlParams);
                if (rowUpdate > 0)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// This method is used to fetch default notification template
        /// </summary>
        /// <param name="groupID">Group ID</param>
        /// <param name="recipientID">Recipient ID</param>
        /// <param name="deviceID">Device ID</param>
        /// <param name="eventID">Event ID</param>
        /// <returns>Object of NotificationTemplateInfo</returns>
        public NotificationTemplateInfo GetDefaultNotificationTemplate(int groupID, int recipientID, int deviceID, int eventID)
        {
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                NotificationTemplateInfo objNotificationTemplateInfo = new NotificationTemplateInfo();
                SqlParameter[] arSqlParams = new SqlParameter[4];

                arSqlParams[0] = new SqlParameter(RECIPIENT_ID, SqlDbType.Int);
                arSqlParams[0].Value = recipientID;
                arSqlParams[1] = new SqlParameter(DEVICE_ID, SqlDbType.Int);
                arSqlParams[1].Value = deviceID;
                arSqlParams[2] = new SqlParameter(EVENT_ID, SqlDbType.Int);
                arSqlParams[2].Value = eventID;
                arSqlParams[3] = new SqlParameter(GROUP_ID, SqlDbType.Int);
                arSqlParams[3].Value = groupID;

                SqlDataReader drNotifications = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_DEFAULT_TEMPLATE, arSqlParams);
                if (drNotifications.Read())
                {
                    objNotificationTemplateInfo.RecipientID = recipientID;
                    objNotificationTemplateInfo.DeviceID = deviceID;
                    objNotificationTemplateInfo.EventID = eventID;
                    objNotificationTemplateInfo.NotificationTemplateID = Convert.ToInt32(drNotifications["NotificationTemplateID"]);
                    objNotificationTemplateInfo.SubjectText = Convert.ToString(drNotifications["SubjectText"]);
                    objNotificationTemplateInfo.BodyText = Convert.ToString(drNotifications["BodyText"]);
                    objNotificationTemplateInfo.FaxTemplateURL = Convert.ToString(drNotifications["FaxTemplateURL"]);
                }
                return objNotificationTemplateInfo;
            }
        }


        #endregion

    }
}
