/******************************File History***************************
 * File Name        : FindingInformation.cs
 * Author           : Prerak Shah.
 * Created Date     : 16-07-2007
 * Purpose          : This Class will provide properties of Finding.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 10-12-2007 Prerak - Added Property of Prioriry
 * 12-12-2007 IAK - Added DocumentedOnly property
 * 12-12-2007 IAK - Added Active property
 * 19-09-2008 IAK - CR- 265 -Added property: NotificationEventTypeID, NotificationDeviceTypeID 
 * 14-10-2008 Prerak - Live Agent "SendToAgent" property added for power scribe user.
 * 22-12-2008 GB     - Added fields Default and Connect Live as per TTP #244 and #231. 
 * ------------------------------------------------------------------- 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Vocada.CSTools.Common
{
    public class  FindingInformation
    {
        #region private Members
        private int findingID;
        private int groupID;
        private string findingDescription = string.Empty;
        private string findingVoiceOverURL = string.Empty;
        private int complianceGoal;
        private int escalateEvery;
        private int endAfterMinutes;
        private int endAt;
        private int sendOTNAt ;
        private int startBackupAt;
        private bool continueToSendPrimary;
        private bool embargo;
        private int embargoStartHour;
        private int embargoEndHour;
        private int priority;
        private bool embargoSpanWeekend;
        private int findingOrder;
        private bool requireReadback;
        private bool documentedOnly;
        private bool active;
        private int notificationEventTypeID;
        private int notificationDeviceTypeID;
        private bool isDefault;
        private int agentActionTypeID;
        #endregion

        #region Public Properties
        /// <summary>
        /// Get Finding ID 
        /// </summary>
        public int FindingID
        {
            get
            {
                return findingID;
            }
            set
            {
                findingID = value;
            }
        }
        /// <summary>
        /// Get group ID 
        /// </summary>
        public int GroupID
        {
            get
            {
                return groupID;
            }
            set
            {
                groupID = value;
            }
        }
        /// <summary>
        /// Get FindingDescription 
        /// </summary>
        public string FindingDescription
        {
            get
            {
                return findingDescription;
            }
            set
            {
                findingDescription = value;
            }
        }
        /// <summary>
        /// Get Finding VoiceOver URL 
        /// </summary>
        public string FindingVoiceOverURL
        {
            get
            {
                return findingVoiceOverURL;
            }
            set
            {
                findingVoiceOverURL = value;
            }
        }
        /// <summary>
        /// Get ComplianceGoal
        /// </summary>
        public int ComplianceGoal
        {
            get
            {
                return complianceGoal;
            }
            set
            {
                complianceGoal = value;
            }
        }
        /// <summary>
        /// Get EscalateEvery
        /// </summary>
        public int EscalateEvery
        {
            get
            {
                return escalateEvery;
            }
            set
            {
                escalateEvery = value;
            }
        }
        /// <summary>
        /// Get EndAfterMinutes 
        /// </summary>
        public int EndAfterMinutes
        {
            get
            {
                return endAfterMinutes;
            }
            set
            {
                endAfterMinutes = value;
            }
        }

        /// <summary>
        /// Get EndAt 
        /// </summary>
        public int EndAt
        {
            get
            {
                return endAt;
            }
            set
            {
                endAt = value;
            }
        }

        /// <summary>
        /// Get Send OTN At 
        /// </summary>
        public int SendOTNAt
        {
            get
            {
                return sendOTNAt;
            }
            set
            {
                sendOTNAt = value;
            }
        }

        /// <summary>
        /// Get Start Backup At
        /// </summary>
        public int StartBackupAt
        {
            get
            {
                return startBackupAt;
            }
            set
            {
                startBackupAt = value;
            }
        }

        /// <summary>
        /// Continue To Send Primary
        /// </summary>
        public bool ContinueToSendPrimary
        {
            get
            {
                return continueToSendPrimary;
            }
            set
            {
                continueToSendPrimary = value;
            }
        }
        
        /// <summary>
        /// Embargo
        /// </summary>
        public bool Embargo
        {
            get
            {
                return embargo;
            }
            set
            {
                embargo = value;
            }
        }
       
        /// <summary>
        /// Get Embargo Start Hour
        /// </summary>
        public int EmbargoStartHour
        {
            get
            {
                return embargoStartHour;
            }
            set
            {
                embargoStartHour = value;
            }
        }

        /// <summary>
        /// Get Embargo End Hour
        /// </summary>
        public int EmbargoEndHour
        {
            get
            {
                return embargoEndHour;
            }
            set
            {
                embargoEndHour = value;
            }
        }
        
        /// <summary>
        /// Embargo Span Weekend
        /// </summary>
        public bool EmbargoSpanWeekend
        {
            get
            {
                return embargoSpanWeekend;
            }
            set
            {
                embargoSpanWeekend = value;
            }
        }

        /// <summary>
        /// Get Finding Order
        /// </summary>
        public int FindingOrder
        {
            get
            {
                return findingOrder;
            }
            set
            {
               findingOrder = value;
            }
        }
        
        /// <summary>
        /// Require Readback
        /// </summary>
        public bool RequireReadback
        {
            get
            {
                return requireReadback;
            }
            set
            {
                requireReadback = value;
            }
        }

        /// <summary>
        /// Is true then message created using this finding is documented Only
        /// </summary>
        public bool DocumentedOnly
        {
            get
            {
                return documentedOnly;
            }
            set
            {
                documentedOnly = value;
            }
        }

        /// <summary>
        /// Get Priority
        /// </summary>
        public int Priority
        {
            get
            {
                return priority;
            }
            set
            {
                priority = value;
            }
        }

        /// <summary>
        /// Is finding active or deactive
        /// </summary>
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        /// <summary>
        /// Get Notification Event Type ID
        /// </summary>
        public int NotificationEventTypeID
        {
            get
            {
                return notificationEventTypeID;
            }
            set
            {
                notificationEventTypeID = value;
            }
        }

        /// <summary>
        /// Get Notification Device Type ID
        /// </summary>
        public int NotificationDeviceTypeID
        {
            get
            {
                return notificationDeviceTypeID;
            }
            set
            {
                notificationDeviceTypeID = value;
            }
        }
        /// <summary>
        /// IsDefault
        /// </summary>
        public bool IsDefault
        {
            get
            {
                return isDefault;
            }
            set
            {
                isDefault = value;
            }
        }

        /// <summary>
        /// Agent action Type ID as 0 - None, 1 - Deliver through Veriphy, 2 - Connect live and 3 - Deliver Live.
        /// </summary>
        public int AgentActionTypeID
        {
            get
            {
                return agentActionTypeID;
            }
            set
            {
                agentActionTypeID = value;
            }
        }        
        #endregion
    }
}
