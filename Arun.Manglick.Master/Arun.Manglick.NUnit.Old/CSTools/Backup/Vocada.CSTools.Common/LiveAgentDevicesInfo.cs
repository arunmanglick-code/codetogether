#region File History
/******************************File History***************************
 * File Name        : LiveAgentDeviceInfo.cs
 * Author           : Suhas Tarihalkar  
 * Created Date     : 24-Sept-08
 * Purpose          : Save Live Agent Device Information for Escalations
 *                  : 
 ***********************File Modification History***********************
 *  
 **********************************************************************/

#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace Vocada.CSTools.Common
{
    public class LiveAgentDevicesInfo
    {
        #region Private Variables
        /// <summary>
        /// ReferringPhysicianID for Device
        /// </summary>
        private int callCenterID;

        /// <summary>
        /// deviceID for Device
        /// </summary>
        private int deviceID;

        /// <summary>
        /// Auto Generated Name for device
        /// </summary>
        private string deviceName;

        /// <summary>
        /// Device Number / Address
        /// </summary>
        private string deviceAddress;

        /// <summary>
        /// Email Gateway address for device
        /// </summary>
        private string gateway;

        /// <summary>
        /// Carrier for Cell or Pager
        /// </summary>
        private string carrier;

        /// <summary>
        /// FindingID for that device
        /// </summary>
        private int findingID;

        /// <summary>
        /// OCDeviceID for that device
        /// </summary>
        private int agentDeviceID;

        /// <summary>
        /// OCNotifyEventID for that device
        /// </summary>
        private int agentNotifyEventID;

        /// <summary>
        /// initialPauseTime
        /// </summary>
        private string initialPauseTime;
       
        /// <summary>
        /// GroupID
        /// </summary>
        private int groupID;
        
        /// <summary>
        /// OC Notification ID
        /// </summary>
        private int agentNotificationID;

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or Sets referringPhysicianID for the object
        /// </summary>
        public int CallCenterID
        {
            get
            {
                return callCenterID;
            }
            set
            {
                callCenterID = value;
            }
        }

        /// <summary>
        /// Gets or Sets deviceID for object
        /// </summary>
        public int DeviceID
        {
            get
            {
                return deviceID;
            }
            set
            {
                deviceID = value;
            }
        }

        /// <summary>
        /// Gets or Sets ocDeviceID for object
        /// </summary>
        public int AgentDeviceID
        {
            get
            {
                return agentDeviceID;
            }
            set
            {
                agentDeviceID = value;
            }
        }

        /// <summary>
        /// Gets or Sets Device Number / Address
        /// </summary>
        public string DeviceAddress
        {
            get
            {
                return deviceAddress;
            }
            set
            {
                deviceAddress = value;
            }
        }

        /// <summary>
        /// Gets or Sets Name for object
        /// </summary>
        public string DeviceName
        {
            get
            {
                return deviceName;
            }
            set
            {
                deviceName = value;
            }
        }

        /// <summary>
        /// Gets or Sets Email Gateway address for object
        /// </summary>
        public string Gateway
        {
            get
            {
                return gateway;
            }
            set
            {
                gateway = value;
            }
        }

        /// <summary>
        /// Gets or Sets Carrier for Cell or Pager device object
        /// </summary>
        public string Carrier
        {
            get
            {
                return carrier;
            }
            set
            {
                carrier = value;
            }
        }

        /// <summary>
        /// Gets or Sets FindingID for that object
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
        /// Gets or Sets OCNotifyEventID for that object
        /// </summary>
        public int AgentNotifyEventID
        {
            get
            {
                return agentNotifyEventID;
            }
            set
            {
                agentNotifyEventID = value;
            }
        }

       
        public string InitialPauseTime
        {
            get
            {
                return initialPauseTime;
            }
            set
            {
                initialPauseTime = value;
            }
        }
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
        public int AgentNotificationID
        {
            get
            {
                return agentNotificationID;
            }
            set
            {
                agentNotificationID = value;
            }
        }
        #endregion

    }
}
