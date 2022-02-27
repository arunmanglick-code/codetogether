#region File History
/******************************File History***************************
 * File Name        : NotificationTemplateInfo.cs
 * Author           : Raju Gupta
 * Created Date     : 8 Sep 2008
 * Purpose          : This class provides properties used for Custom Notification Template.
 *                  : 
 *                  :
 * *********************File Modification History*********************
 * Date(mm-dd-yyyy) Developer Reason of Modification
 * 09-26-2008 Raju G Added messageSendType property to handle forwarded/original message sent type
 * ------------------------------------------------------------------- 
 */
#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace Vocada.CSTools.Common
{
    /// <summary>
    /// Summary of class.
    /// </summary>
    public class NotificationTemplateInfo
    {

        #region Private Members

        /// <summary>
        /// Notification Template ID
        /// </summary>
        private int notificationTemplateID;

        /// <summary>
        /// Group ID
        /// </summary>
        private int groupID;

        /// <summary>
        /// Group Name
        /// </summary>
        private string groupName;

        /// <summary>
        /// Recipient ID
        /// </summary>
        private int recipientID;

        /// <summary>
        /// Recipient
        /// </summary>
        private string recipient;

        /// <summary>
        /// Device ID
        /// </summary>
        private int deviceID;

        /// <summary>
        /// Device Description
        /// </summary>
        private string deviceDescription;

        /// <summary>
        /// Event ID
        /// </summary>
        private int eventID;

        /// <summary>
        /// Event Description
        /// </summary>
        private string eventDescription;

        /// <summary>
        /// Subject Text
        /// </summary>
        private string subjectText = string.Empty;

        /// <summary>
        /// BOdy Text
        /// </summary>
        private string bodyText = string.Empty;

        /// <summary>
        /// Fax Template URL
        /// </summary>
        private string faxTemplateURL;

        /// <summary>
        /// Created On
        /// </summary>
        private DateTime createdOn;

        /// <summary>
        /// Last Modified On
        /// </summary>
        private DateTime lastModifiedOn;

        /// <summary>
        /// Message Sent Type - 1. Original Message 2. Forwarded Message
        /// </summary>
        private int messageSendType;

        #endregion

        #region Public Properties

        /// <summary>
        /// Notification Template ID
        /// </summary>
        public int NotificationTemplateID
        {
            get { return notificationTemplateID; }
            set { notificationTemplateID = value; }
        }

        /// <summary>
        /// Group ID
        /// </summary>
        public int GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }

        /// <summary>
        /// Group Name
        /// </summary>
        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }

        /// <summary>
        /// Recipient ID
        /// </summary>
        public int RecipientID
        {
            get { return recipientID; }
            set { recipientID = value; }
        }

        /// <summary>
        /// Recipient
        /// </summary>
        public string Recipient
        {
            get { return recipient; }
            set { recipient = value; }
        }

        /// <summary>
        /// Device ID
        /// </summary>
        public int DeviceID
        {
            get { return deviceID; }
            set { deviceID = value; }
        }

        /// <summary>
        /// Device Description
        /// </summary>
        public string DeviceDescription
        {
            get { return deviceDescription; }
            set { deviceDescription = value; }
        }

        /// <summary>
        /// Event ID
        /// </summary>
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        /// <summary>
        /// Event Description
        /// </summary>
        public string EventDescription
        {
            get { return eventDescription; }
            set { eventDescription = value; }
        }

        /// <summary>
        /// Subject Text
        /// </summary>
        public string SubjectText
        {
            get { return subjectText; }
            set { subjectText = value; }
        }

        /// <summary>
        /// BOdy Text
        /// </summary>
        public string BodyText
        {
            get { return bodyText; }
            set { bodyText = value; }
        }

        /// <summary>
        /// Fax Template URL
        /// </summary>
        public string FaxTemplateURL
        {
            get { return faxTemplateURL; }
            set { faxTemplateURL = value; }
        }

        /// <summary>
        /// Created On
        /// </summary>
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }

        /// <summary>
        /// Last Modified On
        /// </summary>
        public DateTime LastModifiedOn
        {
            get { return lastModifiedOn; }
            set { lastModifiedOn = value; }
        }

        /// <summary>
        /// Message Sent Type - 1. Original Message 2. Forwarded Message
        /// </summary>       
        public int MessageSendType
        {
            get { return messageSendType; }
            set { messageSendType = value; }
        }

        #endregion

    }
}
