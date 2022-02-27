#region File History

/******************************File History***************************
 * File Name        : Vocada.CSTools.Common/UnitDevice.cs
 * Author           : Indrajeet K
 * Created Date     : 28-Aug-07
 * Purpose          : Contains all DataObjects / Model level classes for the Unit Devices and Preferences screen for Veriphy Lab.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification
 * 07-09-2008 - Prerak - OcDevice Property added for implementing CR #258
 * ------------------------------------------------------------------- 
 *                          
 */
#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace Vocada.CSTools.Common
{
    #region Class UnitDeviceNotification
    /// <summary>
    /// Class that contains all private fields and public properties for Unit Device and its Notification events.
    /// </summary>
    public class UnitDeviceNotification
    {
        #region Private Variables
        /// <summary>
        /// unitID for Device
        /// </summary>
        private int unitID;

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
        /// UnitNotifyEventID for that device
        /// </summary>
        private int unitNotifyEventID;
        
        /// <summary>
        /// unitDeviceID for that device
        /// </summary>
        private int unitDeviceID;
        
        /// <summary>
        /// FindingID for that device
        /// </summary>
        private int findingID;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or Sets unitID for the object
        /// </summary>
        public int UnitID
        {
            get
            {
                return unitID;
            }
            set
            {
                unitID = value;
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
        /// Gets or Sets UnitDeviceID for that object
        /// </summary>
        public int UnitDeviceID
        {
            get
            {
                return unitDeviceID;
            }
            set
            {
                unitDeviceID = value;
            }
        }

        /// <summary>
        /// Gets or Sets UnitNotifyEventID for that object
        /// </summary>
        public int UnitNotifyEventID
        {
            get
            {
                return unitNotifyEventID;
            }
            set
            {
                unitNotifyEventID = value;
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
        #endregion

    }
    #endregion

    #region Class LabUnitObject
    /// <summary>
    /// This class contains fields and properties generically called as FieldID and FieldName to contain the text and Value for any item.
    /// </summary>
    public class LabUnitObject
    {
        #region Private Fields
        /// <summary>
        /// Numeric field containing id for any item
        /// </summary>
        private int fieldID;
        
        /// <summary>
        /// Contains Name or Description of the item
        /// </summary>
        private string fieldName;
        /// <summary>
        /// Contains integer value of OCdevice. 
        /// </summary>
        private bool ocDevice;

        
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or Sets id for that item
        /// </summary>
        public int FieldID
        {
            get
            {
                return fieldID;
            }
            set
            {
                fieldID = value;
            }
        }

        /// <summary>
        /// Gets or Sets Name / Description for that item
        /// </summary>
        public string FieldName
        {
            get
            {
                return fieldName;
            }
            set
            {
                fieldName = value;
            }
        }
        /// <summary>
        /// Gets or Sets OcDevice value
        /// 1=OC device
        /// 2=BED
        /// 3=UNIT
        /// 4=Clinical Team
        /// </summary>
        public bool OcDevice
        {
            get { return ocDevice; }
            set { ocDevice = value; }
        }
        #endregion
    }
    #endregion
}
