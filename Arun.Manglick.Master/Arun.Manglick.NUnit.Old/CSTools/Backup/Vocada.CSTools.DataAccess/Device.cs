#region File History

/******************************File History***************************
 * File Name        : Device.cs
 * Author           : Prerak Shah
 * Created Date     : 09-Aug-2007
 * Purpose          : This Class is used to Get Device information.
 *                  : Add, edit, delete device for subscriber/Group
 *                  : Add, edit, delete device notifications for subscriber/Group
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 21-09-2007   Modify to add methods of Device Notification Error
 * 03-12-2007 - Prerak - Call getOpenConnection function from Utility Class.
 * 04-11-2008 - Prerak - GetAllDevices() method modified for CR SMSWithWebLink
 * 05-01-2009  - Raju G - Modified for FR 282
 * 09-01-2009 - GB     - Changes made for FR #282
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
    public class Device
    {

        #region Constant
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string SUBSCRIBER_ID = "@subscriberID";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string CARRIER_ID = "CarrierID";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string GROUP_ID = "@GroupID";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string CARRIER_DESC = "CarrierDescription";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string CARRIER_EMAIL = "CarrierEmail";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string SUBSCRIBER_NOTIFICATION_ID = "@subscriberNotificationID";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string GROUP_NOTIFICATION_EVENT_ID = "@groupNotifyEventID";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string GROUP_NOTIFICATION_ID = "@groupNotificationID";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string SUBSCRIBER_DEVICE_ID = "@subscriberDeviceID";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string GROUP_DEVICE_ID = "@groupDeviceID";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string DEVICE_ID = "@deviceID";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string DEVICE_NAME = "@deviceName";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string DEVICE_ADDRESS = "@deviceAddress";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string GATEWAY = "@gateway";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string CARRIER = "@carrier";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string SUBSCRIBER_NOTIFICATION_EVENT_ID = "@subscriberNotifyEventID";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string NOTIFICATION_ID = "@subscriberAfterHoursNotificationID";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string FINDING_ID = "@findingID";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string START_HOUR = "@startHour";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string END_HOUR = "@endHour";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string EVENT_ID = "@eventID";
        /// <summary>
        /// This constant store the sql parameter name 
        /// </summary>
        private const string RESULT = "@result";
        /// <summary>
        /// This constant store the sql parameter name for Institution
        /// </summary>
        private const string INSTITUTION_ID = "@institutionID";
        /// <summary>
        /// This constant store the sql parameter name for GetAllDevices()
        /// </summary>
        private const string DEVICE_FOR = "@deviceFor";

        /// <summary>
        /// This constant stores name of stored procedure which will retrive list of cell phone Carrier 
        /// </summary>
        private const string SP_GET_CELL_PHONE_CARRIER = "dbo.getCellPhoneCarriers";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive list of devices 
        /// </summary>
        private const string SP_GET_ALL_DEVICES = "dbo.getDevices";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive list of devices for group
        /// </summary>
        private const string SP_GET_GROUP_DEVICES = "dbo.VOC_CST_getGroupDevices";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive list of devices assign to subscriber
        /// </summary>
        private const string SP_GET_SUBSCRIBER_DEVICES = "dbo.getSubscriberDevices";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive all findings for a subscriber
        /// </summary>
        private const string SP_GET_SUBSCRIBER_FINDINGS = "dbo.getFindingOptionsForSubscriber";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive list of notifications assign to subscriber
        /// </summary>
        private const string SP_GET_SUBSCRIBER_NOTIFICATIONS = "dbo.getSubscriberNotifications";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive list of notifications assign to group
        /// </summary>
        private const string SP_GET_GROUP_NOTIFICATIONS = "dbo.getGroupNotifications";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive all type of notification events available for subscriber 
        /// </summary>
        private const string SP_GET_SUBSCRIBER_NOTIFICATION_EVENTS = "dbo.getSubscriberNotifyEvents";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive all type of notification events available for group
        /// </summary>
        private const string SP_GET_GROUP_NOTIFICATION_EVENTS = "dbo.getGroupNotifyEvents";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive afer-hour notification for subscriber 
        /// </summary>
        private const string SP_GET_AH_NOTIFICATIONS = "dbo.getSubscriberAfterHoursNotifications";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive list of pager Carrier 
        /// </summary>
        private const string SP_GET_PAGER_CARRIER = "dbo.getPagerCarriers";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive list of devices and its notifications assigned to subscriber
        /// </summary>
        private const string SP_GET_SUBSCRIBER_DEVICES_NOTIFICATIONS = "dbo.VOC_CST_getUserDeviceAndNotifications";
        /// <summary>
        /// This constant stores name of stored procedure which will delete devices for subscriber
        /// </summary>
        private const string SP_DELETE_DEVICES = "dbo.VOC_VLR_DeleteDevicesWhenRoleChangedToAdmin";
        /// <summary>
        /// This constant stores name of stored procedure which will delete device assigned to subscriber
        /// </summary>
        private const string SP_DELETE_SUBSCRIBER_DEVICES = "dbo.VOC_VLR_deleteSubscriberDevice";
        /// <summary>
        /// This constant stores name of stored procedure which will delete device assigned to group
        /// </summary>
        private const string SP_DELETE_GROUP_DEVICES = "dbo.VOC_CST_deleteGroupDevice";
        /// <summary>
        /// This constant stores name of stored procedure which will delete device notifications
        /// </summary>
        private const string SP_DELETE_DEVICE_NOTIFICATIONS = "dbo.deleteSubscriberNotification";
        /// <summary>
        /// This constant stores name of stored procedure which will delete group notifications
        /// </summary>
        private const string SP_DELETE_GROUP_NOTIFICATIONS = "dbo.deleteGroupNotification";
        /// <summary>
        /// This constant stores name of stored procedure which will delete Subscriber's AfterHours Notification
        /// </summary>
        private const string SP_DELETE_AFTER_HOUR_NOTIFY = "dbo.deleteSubscriberAfterHoursNotification";
        /// <summary>
        /// This constant stores name of stored procedure which will delete device and its notification assigned to subscriber
        /// </summary>
        private const string SP_DELETE_SUBSCRIBER_DEVICES_NOTIFICATIONS = "dbo.VOC_CST_deleteUserDeviceAndNotifications";
        /// <summary>
        /// This constant stores name of stored procedure which will update device information for subscriber
        /// </summary>
        private const string SP_UPDATE_SUBSCRIBER_DEVICES = "dbo.VOC_VL_updateSubscriberDevice";
        /// <summary>
        /// This constant stores name of stored procedure which will update device information for group
        /// </summary>
        private const string SP_UPDATE_GROUP_DEVICE = "dbo.VOC_CST_updateGroupDevice";
        /// <summary>
        /// This constant stores name of stored procedure which will update device and its notification information for subscriber
        /// </summary>
        private const string SP_UPDATE_SUBSCRIBER_DEVICES_NOTIFICATIONS = "dbo.VOC_CST_updateUserDeviceAndNotifications";
        /// <summary>
        /// This constant stores name of stored procedure which will insert new device for subscriber
        /// </summary>
        private const string SP_INSERT_SUBSCRIBER_DEVICE = "dbo.VOC_VL_insertSubscriberDevice";
        /// <summary>
        /// This constant stores name of stored procedure which will insert new device for group
        /// </summary>
        private const string SP_INSERT_GROUP_DEVICE = "dbo.VOC_CST_insertGroupDevice";
        /// <summary>
        /// This constant stores name of stored procedure which will insert new notification for subscriber
        /// </summary>
        private const string SP_INSERT_SUBSCRIBER_NOTIFICATION = "dbo.insertSubscriberNotification";
        /// <summary>
        /// This constant stores name of stored procedure which will insert new notification for Group
        /// </summary>
        private const string SP_INSERT_GROUP_NOTIFICATION = "dbo.insertGroupNotification";
        /// <summary>
        /// This constant stores name of stored procedure which will insert new After-Hour notification for subscriber
        /// </summary>
        private const string SP_INSERT_SUBSCRIBER_AH_NOTIFICATION = "dbo.insertAfterHoursSubscriberNotification";
        /// <summary>
        /// This constant stores name of stored procedure which will insert new device and its notification for subscriber
        /// </summary>
        private const string SP_INSERT_SUBSCRIBER_DEVICE_NOTIFICATIONS = "dbo.VOC_CST_insertUserDeviceAndNotifications";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive all findings for a group
        /// </summary>
        private const string SP_GET_GROUP_FINDINGS = "dbo.VOC_CST_getGroupFindings";
        /// <summary>
        /// This constant stores name of stored procedure which will Delete findings for a Group
        /// </summary>
        private const string SP_DELETE_GROUP_FINDINGS = "dbo.VOC_CST_deleteFindinsByID";
        /// <summary>
        /// This constant stores name of stored procedure which will retrive Groups for Institution from database
        /// </summary>
        private const string SP_GET_GROUPS_BY_INSTITUTION = "dbo.VOC_CST_getGroupListByInstitute";
         /// <summary>
        /// This constant stores name of stored procedure which will retrive Groups for Institution from database
        /// </summary>
        private const string SP_GET_DEVICE_NOTIFICATION_ERROR = "dbo.VOC_CST_getDeviceNotificationError";
 
        
        #endregion Constant

        #region Public Methods

        /// <summary>
        /// Get Cell Phone Carriers
        /// </summary>
        /// <returns></returns>
        public DataTable GetCellPhoneCarriers()
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataSet dsCellPhoneCarriers = new DataSet();
                DataTable dtCellPhoneCarriers = dsCellPhoneCarriers.Tables.Add();
                dtCellPhoneCarriers.Columns.Add(CARRIER_ID);
                dtCellPhoneCarriers.Columns.Add(CARRIER_DESC);
                dtCellPhoneCarriers.Columns.Add(CARRIER_EMAIL);

                SqlDataReader drCellPhoneCarrier = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_CELL_PHONE_CARRIER);
                while (drCellPhoneCarrier.Read())
                {
                    Object[] values = { (int)drCellPhoneCarrier["CellPhoneCarrierID"], (string)drCellPhoneCarrier["CellPhoneCarrierDescription"], (string)drCellPhoneCarrier["CellPhoneCarrierEmail"] };
                    dsCellPhoneCarriers.Tables[0].Rows.Add(values);
                }
                drCellPhoneCarrier.Close();
                return dtCellPhoneCarriers;
            }
        }

        /// <summary>
        /// Get Pager Carriers
        /// </summary>
        /// <returns></returns>
        public DataTable GetPagerCarriers()
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataSet dsPagerCarriers = new DataSet();
                DataTable dtPagerCarriers = dsPagerCarriers.Tables.Add();
                dtPagerCarriers.Columns.Add(CARRIER_ID);
                dtPagerCarriers.Columns.Add(CARRIER_DESC);
                dtPagerCarriers.Columns.Add(CARRIER_EMAIL);

                SqlDataReader drPagerCarrier = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_PAGER_CARRIER);
                while (drPagerCarrier.Read())
                {
                    Object[] values = { (int)drPagerCarrier["PagerCarrierID"], (string)drPagerCarrier["PagerCarrierDescription"], (string)drPagerCarrier["PagerEmail"] };
                    dsPagerCarriers.Tables[0].Rows.Add(values);
                }
                drPagerCarrier.Close();
                return dtPagerCarriers;
            }
        }

        /// <summary>
        /// Get all devices.
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetAllDevices()
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtDevices = new DataTable("Devices");
                SqlParameter[] arParams = new SqlParameter[1];
                arParams[0] = new SqlParameter(DEVICE_FOR, "GROUP");
                arParams[0].Direction = ParameterDirection.Input;

                SqlDataReader drDevices = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_ALL_DEVICES, arParams);
                dtDevices.Load(drDevices);
                drDevices.Close();
                return dtDevices;
            }
        }

        /// <summary>
        /// Get all devices assigned to Group.
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetGroupDevices(int groupID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtDevices = new DataTable("Devices");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drDevices = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUP_DEVICES, objSqlParameter);

                dtDevices.Load(drDevices);
                drDevices.Close();
                return dtDevices;
            }
        }

        /// <summary>
        /// Get all findings for subscriber.
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetFindingsForSubscriber(int subscriberID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtFindings = new DataTable("Findings");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drFindings = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_SUBSCRIBER_FINDINGS, objSqlParameter);

                dtFindings.Load(drFindings);
                drFindings.Close();
                return dtFindings;
            }
        }
        
        /// <summary>
        /// Get all notification event types for subscribers.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllNotificationTypes()
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtNotificationTypes = new DataTable("NotificationTypes");

                SqlDataReader drNotificationTypes = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_SUBSCRIBER_NOTIFICATION_EVENTS);

                dtNotificationTypes.Load(drNotificationTypes);
                drNotificationTypes.Close();
                return dtNotificationTypes;
            }
        }

        /// <summary>
        /// Get all notification event types available for group.
        /// </summary>
        /// <returns></returns>
        public DataTable GetGroupNotificationTypes()
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtNotificationTypes = new DataTable("NotificationTypes");

                SqlDataReader drNotificationTypes = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUP_NOTIFICATION_EVENTS);

                dtNotificationTypes.Load(drNotificationTypes);
                drNotificationTypes.Close();
                return dtNotificationTypes;
            }
        }

        /// <summary>
        /// Get Devices for a given subscriber
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetSubscriberDevices(int subscriberID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtSubscriberDevices = new DataTable("SubscriberDevices");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drSubscriberDevices = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_SUBSCRIBER_DEVICES_NOTIFICATIONS, objSqlParameter);

                dtSubscriberDevices.Load(drSubscriberDevices);
                drSubscriberDevices.Close();
                return dtSubscriberDevices;
            }
        }

        /// <summary>
        /// Get notifications for a given subscriber
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <returns></returns>
        public DataTable GetSubscriberNotifications(int subscriberID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtSubscriberNotifications = new DataTable("SubscriberNotifications");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drSubscriberNotifications = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_SUBSCRIBER_NOTIFICATIONS, objSqlParameter);

                dtSubscriberNotifications.Load(drSubscriberNotifications);
                drSubscriberNotifications.Close();
                return dtSubscriberNotifications;
            }
        }

        /// <summary>
        /// Get notifications for a given group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetGroupNotifications(int groupID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtGroupNotifications = new DataTable("GroupNotifications");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drGroupNotifications = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUP_NOTIFICATIONS, objSqlParameter);

                dtGroupNotifications.Load(drGroupNotifications);
                drGroupNotifications.Close();
                return dtGroupNotifications;
            }
        }

        /// <summary>
        /// Get after-Hour notifications for a given subscriber
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <returns></returns>
        public DataTable GetAHNotificationsForSubscriber(int subscriberID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtSubscriberAHNotifications = new DataTable("SubscriberAHNotifications");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drSubscriberAHNotifications = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_AH_NOTIFICATIONS, objSqlParameter);

                dtSubscriberAHNotifications.Load(drSubscriberAHNotifications);
                drSubscriberAHNotifications.Close();
                return dtSubscriberAHNotifications;
            }
        }

        /// <summary>
        /// Delete Devices for Subscriber
        /// </summary>
        /// <param name="subscriberID"></param>
        public int DeleteDevices(int subscriberID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_DELETE_DEVICES, objSqlParameter);
            }
        }

        /// <summary>
        /// Delete Subscribers-Device Notification
        /// </summary>
        /// <param name="subscriberNotificationID"></param>
        public int DeleteDeviceNotification(int subscriberNotificationID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_NOTIFICATION_ID, subscriberNotificationID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_DELETE_DEVICE_NOTIFICATIONS, objSqlParameter);
            }
        }

        /// <summary>
        /// Delete Group Notification
        /// </summary>
        /// <param name="groupNotificationID"></param>
        public int DeleteGroupNotification(int groupNotificationID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_NOTIFICATION_ID, groupNotificationID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_DELETE_GROUP_NOTIFICATIONS, objSqlParameter);
            }
        }

        /// <summary>
        /// Delete Device assigned to subscriber
        /// </summary>
        /// <param name="subscriberDeviceID"></param>
        /// <param name="subscriberNotificationID"></param>
        public int DeleteSubscriberDevice(int subscriberDeviceID, int subscriberNotificationID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[2];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_NOTIFICATION_ID, subscriberNotificationID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(SUBSCRIBER_DEVICE_ID, subscriberDeviceID);
                objSqlParameter[1].Direction = ParameterDirection.Input;

                return Convert.ToInt32(SqlHelper.ExecuteScalar(sqlConnection, CommandType.StoredProcedure, SP_DELETE_SUBSCRIBER_DEVICES_NOTIFICATIONS, objSqlParameter));
            }
        }

        /// <summary>
        /// Delete Device assigned to group
        /// </summary>
        /// <param name="groupDeviceID"></param>
        /// <summary>
        /// Delete Device assigned to group
        /// </summary>
        /// <param name="groupDeviceID"></param>
        public int DeleteGroupDevice(int groupDeviceID, int groupNotificationID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[2];

                objSqlParameter[0] = new SqlParameter(GROUP_DEVICE_ID, groupDeviceID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(GROUP_NOTIFICATION_ID, groupNotificationID);
                objSqlParameter[1].Direction = ParameterDirection.Input;

                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_DELETE_GROUP_DEVICES, objSqlParameter);
            }
        }

        /// <summary>
        /// Delete After Hour notification
        /// </summary>
        /// <param name="notificationID"></param>
        public void DeleteAfterHoursNotification(int notificationID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(NOTIFICATION_ID, notificationID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_DELETE_AFTER_HOUR_NOTIFY, objSqlParameter);
            }
        }

        /// <summary>
        ///  Update Device information of subscriber
        /// </summary>
        /// <param name="subscriberDeviceID"></param>
        /// <param name="deviceName"></param>
        /// <param name="deviceAddress"></param>
        /// <param name="gateway"></param>
        /// <param name="subscriberID"></param>
        /// <param name="subscriberNotifyEventID"></param>
        /// <param name="findingID"></param>
        /// <param name="subscriberNotificationID"></param>
        /// <returns></returns>
        public int UpdateSubscriberDevice(int subscriberDeviceID, string deviceName, string deviceAddress, string gateway, int subscriberID,
                                            int subscriberNotifyEventID, int findingID, int subscriberNotificationID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[9];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_DEVICE_ID, subscriberDeviceID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(DEVICE_NAME, deviceName);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(DEVICE_ADDRESS, deviceAddress);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[3] = new SqlParameter(GATEWAY, gateway);
                objSqlParameter[3].Direction = ParameterDirection.Input;

                objSqlParameter[4] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[4].Direction = ParameterDirection.Input;

                objSqlParameter[5] = new SqlParameter(SUBSCRIBER_NOTIFICATION_EVENT_ID, subscriberNotifyEventID);
                objSqlParameter[5].Direction = ParameterDirection.Input;

                objSqlParameter[6] = new SqlParameter(FINDING_ID, findingID);
                objSqlParameter[6].Direction = ParameterDirection.Input;

                objSqlParameter[7] = new SqlParameter(SUBSCRIBER_NOTIFICATION_ID, subscriberNotificationID);
                objSqlParameter[7].Direction = ParameterDirection.Input;

                objSqlParameter[8] = new SqlParameter(RESULT, SqlDbType.Int);
                objSqlParameter[8].Direction = ParameterDirection.Output;
                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_UPDATE_SUBSCRIBER_DEVICES_NOTIFICATIONS, objSqlParameter);
            }
        }

        /// <summary>
        ///  Update Device information of group
        /// </summary>
        /// <param name="groupDeviceID"></param>
        /// <param name="deviceName"></param>
        /// <param name="deviceAddress"></param>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public int UpdateGroupDevice(int groupDeviceID, string deviceName, string deviceAddress, string gateway, int groupID, int eventID, int findingID, int groupNotificationID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[9];
                objSqlParameter[0] = new SqlParameter(GROUP_DEVICE_ID, groupDeviceID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(DEVICE_NAME, deviceName);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(DEVICE_ADDRESS, deviceAddress);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[3] = new SqlParameter(GATEWAY, gateway);
                objSqlParameter[3].Direction = ParameterDirection.Input;
                //Create parameter for GroupID
                objSqlParameter[4] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[4].Direction = ParameterDirection.Input;

                objSqlParameter[5] = new SqlParameter(RESULT, SqlDbType.Int);
                objSqlParameter[5].Direction = ParameterDirection.Output;

                objSqlParameter[6] = new SqlParameter(EVENT_ID, eventID);
                objSqlParameter[6].Direction = ParameterDirection.Input;
                objSqlParameter[7] = new SqlParameter(FINDING_ID, findingID);
                objSqlParameter[7].Direction = ParameterDirection.Input;
                objSqlParameter[8] = new SqlParameter(GROUP_NOTIFICATION_ID, groupNotificationID);
                objSqlParameter[8].Direction = ParameterDirection.Input;

                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_UPDATE_GROUP_DEVICE, objSqlParameter);
            }
        }

        /// <summary>
        /// add new device for subscriber
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <param name="deviceID"></param>
        /// <param name="deviceAddress"></param>
        /// <param name="gateway"></param>
        /// <param name="carrier"></param>
        /// <param name="subscriberNotifyEventID"></param>
        /// <param name="findingID"></param>
        /// <returns></returns>       
        public int InsertSubscriberDevice(int subscriberID, int deviceID, string deviceAddress, string gateway, string carrier, int subscriberNotifyEventID, int findingID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[7];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(DEVICE_ID, deviceID);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(DEVICE_ADDRESS, deviceAddress);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[3] = new SqlParameter(GATEWAY, gateway);
                objSqlParameter[3].Direction = ParameterDirection.Input;
                objSqlParameter[4] = new SqlParameter(CARRIER, carrier);
                objSqlParameter[4].Direction = ParameterDirection.Input;
                objSqlParameter[5] = new SqlParameter(SUBSCRIBER_NOTIFICATION_EVENT_ID, subscriberNotifyEventID);
                objSqlParameter[5].Direction = ParameterDirection.Input;
                objSqlParameter[6] = new SqlParameter(FINDING_ID, findingID);
                objSqlParameter[6].Direction = ParameterDirection.Input;

                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_INSERT_SUBSCRIBER_DEVICE_NOTIFICATIONS, objSqlParameter);
            }
        }

        /// <summary>
        /// add new device for Group
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="deviceID"></param>
        /// <param name="deviceAddress"></param>
        /// <param name="gateway"></param>
        /// <param name="carrier"></param>
        /// <returns></returns>
        public int InsertGroupDevice(int groupID, int deviceID, string deviceAddress, string gateway, string carrier, int eventID, int findingID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[7];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(DEVICE_ID, deviceID);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(DEVICE_ADDRESS, deviceAddress);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[3] = new SqlParameter(GATEWAY, gateway);
                objSqlParameter[3].Direction = ParameterDirection.Input;
                objSqlParameter[4] = new SqlParameter(CARRIER, carrier);
                objSqlParameter[4].Direction = ParameterDirection.Input;
                objSqlParameter[5] = new SqlParameter(EVENT_ID, eventID);
                objSqlParameter[5].Direction = ParameterDirection.Input;
                objSqlParameter[6] = new SqlParameter(FINDING_ID, findingID);
                objSqlParameter[6].Direction = ParameterDirection.Input;

                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_INSERT_GROUP_DEVICE, objSqlParameter);
            }
        }

        /// <summary>
        /// add new Notification for subscriber device
        /// </summary>
        /// <param name="subscriberNotifyEventID"></param>
        /// <param name="subscriberDeviceID"></param>
        /// <param name="findingID"></param>
        public int InsertSubscriberNotification(int subscriberNotifyEventID, int subscriberDeviceID, int findingID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[3];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_NOTIFICATION_EVENT_ID, subscriberNotifyEventID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(SUBSCRIBER_DEVICE_ID, subscriberDeviceID);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(FINDING_ID, findingID);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                
                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_INSERT_SUBSCRIBER_NOTIFICATION, objSqlParameter);
            }
        }

        /// <summary>
        /// add new Notification for Group device
        /// </summary>
        /// <param name="GroupNotifyEventID"></param>
        /// <param name="GroupDeviceID"></param>
        /// <param name="findingID"></param>
        public int InsertGroupNotification(int GroupNotifyEventID, int GroupDeviceID, int findingID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[3];
                objSqlParameter[0] = new SqlParameter(GROUP_NOTIFICATION_EVENT_ID, GroupNotifyEventID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(GROUP_DEVICE_ID, GroupDeviceID);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(FINDING_ID, findingID);
                objSqlParameter[2].Direction = ParameterDirection.Input;

                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_INSERT_GROUP_NOTIFICATION, objSqlParameter);
            }
        }

        /// <summary>
        /// Add new  After-Hour Notification for subscriber.
        /// </summary>
        /// <param name="subscriberID"></param>
        /// <param name="subscriberDeviceID"></param>
        /// <param name="findingID"></param>
        /// <param name="startHour"></param>
        /// <param name="endHour"></param>
        /// <returns></returns>
        public int InsertSubscriberAHNotification(int subscriberID, int subscriberDeviceID, int findingID, int startHour, int endHour)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[5];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;
                objSqlParameter[1] = new SqlParameter(SUBSCRIBER_DEVICE_ID, subscriberDeviceID);
                objSqlParameter[1].Direction = ParameterDirection.Input;
                objSqlParameter[2] = new SqlParameter(FINDING_ID, findingID);
                objSqlParameter[2].Direction = ParameterDirection.Input;
                objSqlParameter[3] = new SqlParameter(START_HOUR, startHour);
                objSqlParameter[3].Direction = ParameterDirection.Input;
                objSqlParameter[4] = new SqlParameter(END_HOUR, endHour);
                objSqlParameter[4].Direction = ParameterDirection.Input;

                return SqlHelper.ExecuteNonQuery(sqlConnection, CommandType.StoredProcedure, SP_INSERT_SUBSCRIBER_AH_NOTIFICATION, objSqlParameter);
            }
        }
        /// <summary>
        /// Get all findings for Group.
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetFindingsForGroup(int groupID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtFindings = new DataTable("Findings");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_ID, groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drFindings = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_GROUP_FINDINGS, objSqlParameter);

                dtFindings.Load(drFindings);
                drFindings.Close();
                return dtFindings;
            }
        }
        /// <summary>
        /// This method make a soft delete of Findings in the database.
        /// </summary>
        /// <param name="subscriberID"></param>
        public void DeleteFindig(int findingID)
        {
            SqlConnection sqlConnection = Utility.getOpenConnection();
            SqlTransaction sqlTransaction;
            sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(FINDING_ID, findingID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, SP_DELETE_GROUP_FINDINGS, objSqlParameter);
                sqlTransaction.Commit();
                sqlConnection.Close();

            }
            catch (SqlException sqlError)
            {
                sqlTransaction.Rollback();
                sqlConnection.Close();
                throw sqlError;
            }
        }
        /// <summary>
        /// This Method returns Data table contaioning Directories of particular Institution.
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns></returns>
        public DataTable GetGroups(int InstitutionID)
        {
            SqlDataReader reader = null;
            DataTable dtGroups = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(INSTITUTION_ID, InstitutionID);
                objSqlParameter[0].Direction = ParameterDirection.Input;


                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_GROUPS_BY_INSTITUTION, objSqlParameter);
                dtGroups.Load(reader);
                return dtGroups;
            }
        }
        public DataTable GetDeviceNotificationError(int groupID)
        {
            SqlDataReader reader = null;
            DataTable dtDeviceError = new DataTable();
            using (SqlConnection cnx = Utility.getOpenConnection())
            {
                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(GROUP_ID , groupID);
                objSqlParameter[0].Direction = ParameterDirection.Input;


                reader = SqlHelper.ExecuteReader(cnx, CommandType.StoredProcedure, SP_GET_DEVICE_NOTIFICATION_ERROR, objSqlParameter);
                dtDeviceError.Load(reader);
                return dtDeviceError;
            }
        }

        /// <summary>
        /// Get Devices for a given subscriber
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public DataTable GetSubscriberDevicesForAfterHours(int subscriberID)
        {
            using (SqlConnection sqlConnection = Utility.getOpenConnection())
            {
                DataTable dtSubscriberDevices = new DataTable("SubscriberDevices");

                SqlParameter[] objSqlParameter = new SqlParameter[1];
                objSqlParameter[0] = new SqlParameter(SUBSCRIBER_ID, subscriberID);
                objSqlParameter[0].Direction = ParameterDirection.Input;

                SqlDataReader drSubscriberDevices = SqlHelper.ExecuteReader(sqlConnection, CommandType.StoredProcedure, SP_GET_SUBSCRIBER_DEVICES, objSqlParameter);

                dtSubscriberDevices.Load(drSubscriberDevices);
                drSubscriberDevices.Close();
                return dtSubscriberDevices;
            }
        }

        #endregion Public Methods
    }
}
