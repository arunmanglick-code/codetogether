#region File History
/******************************File History***************************
 * File Name        : OrderingClinicianInfo.cs
 * Author           : Mayur P
 * Created Date     : 25-Jul-07
 * Purpose          : To provide all the parameters for Ordering Clinician object.
 *                  : 
 *                  :
 * *********************File Modification History*********************
 *  03-20-2008      SSK      Added PINForMessageRetrieve Property
 *  03-26-2008      IAK      Added external info Property for OrderingClinicianInfo
 *  08-01-2008      Prerak   Added GroupID Property   
 *  08-01-2008      Prerak   Added OCNotificationID Property  
 ***************************************************************************/

#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace Vocada.CSTools.Common
{
    #region Class OrderingClinicianInfo
    public class OrderingClinicianInfo
    {
        #region Private Fields
        /// <summary>
        /// Contains DirectoryID
        /// </summary>
        int directoryID;

        /// <summary>
        /// Contains FirstName
        /// </summary>
        string firstName = string.Empty;

        /// <summary>
        /// Contains LastName
        /// </summary>
        string lastName = string.Empty;

        /// <summary>
        /// Contains NickName string
        /// </summary>
        string nickName = string.Empty;

        /// <summary>
        /// Contains PrimaryPhone
        /// </summary>
        string primaryPhone = string.Empty;

        /// <summary>
        /// Contains Pager
        /// </summary>
        string pager = string.Empty;

        /// <summary>
        /// Contains CellPhone
        /// </summary>
        string cellPhone = string.Empty;

        /// <summary>
        /// Contains AdditionalContName
        /// </summary> 
        string additionalContName = string.Empty;

        /// <summary>
        /// Contains AdditionalContPhone
        /// </summary>
        string additionalContPhone = string.Empty;

        /// <summary>
        /// Contains primaryEmail
        /// </summary>
        string primaryEmail = string.Empty;

        /// <summary>
        /// Contains Fax
        /// </summary> 
        string fax = string.Empty;
               
        /// <summary>
        /// Contains specialty
        /// </summary> 
        string specialty = string.Empty;

        /// <summary>
        /// Contains Affiliation
        /// </summary>
        string affiliation = string.Empty;

        /// <summary>
        /// Contains PracticeGroup
        /// </summary> 
        string practiceGroup = string.Empty;

        /// <summary>
        /// Contains Address1
        /// </summary> 
        string address1 = string.Empty;

        /// <summary>
        /// Contains Address2
        /// </summary> 
        string address2 = string.Empty;

        /// <summary>
        /// Contains Address3
        /// </summary> 
        string address3 = string.Empty;

        /// <summary>
        /// Contains City
        /// </summary>
        string city = string.Empty;

        /// <summary>
        /// Contains State
        /// </summary> 
        string state = string.Empty;

        /// <summary>
        /// Contains Zip
        /// </summary> 
        string zip = string.Empty;


        /// <summary>
        /// Contains UpdatedBy
        /// </summary> 
        int updatedBy;

        /// <summary>
        /// Contains ReferringPhysicianID
        /// </summary> 
        int referringPhysicianID;

        /// <summary>
        /// Contains Active
        /// </summary> 
        bool active;

        /// <summary>
        /// Lab Top Doctor
        /// </summary>
        bool labTDR;

        /// <summary>
        /// Radiology Top Doctor
        /// </summary>
        bool radiologyTDR;


        /// <summary>
        /// additional Notes
        /// </summary>
        string notes = string.Empty;

        /// <summary>
        /// Is Profile Completed
        /// </summary>
        bool profileReturn;
        
        /// <summary>
        /// Is Profile Completed
        /// </summary>
        bool profileCompleted;

        /// <summary>
        /// Is ISResident Doctor
        /// </summary>
        bool isResident;
        
        /// <summary>
        /// RIS ID
        /// </summary>
        string idRIS = string.Empty;

        /// <summary>
        /// LIS ID
        /// </summary>
        string idLIS = string.Empty;

        /// <summary>
        /// MSO ID
        /// </summary>
        string idMSO = string.Empty;

        /// <summary>
        /// NPI
        /// </summary>
        string idNPI = string.Empty;

        /// <summary>
        /// login ID for Nurse
        /// </summary>
        private string loginID;

        /// <summary>
        /// password for nurse
        /// </summary>
        private string password;

        /// <summary>
        /// Is EDDoc
        /// </summary>
        bool isEDDoc;

        /// <summary>
        /// VOCUserID for the OC
        /// </summary>
        int vocUserID;

        /// <summary>
        /// InstituteID for the OC
        /// </summary>
        int instituteID;

        /// <summary>
        /// PIN for Message retrieve for the OC
        /// </summary>
        string pinForMessage;

        /// <summary>
        /// External Information: RIS ID
        /// </summary>
        private string risID;

        /// <summary>
        /// External Information: LIS ID
        /// </summary>
        private string lisID;

        /// <summary>
        /// External Information: MSO ID
        /// </summary>
        private string msoID;

        /// <summary>
        /// External Information: NPI
        /// </summary>
        private string npi;
        
        #endregion

        #region Public Property
        /// <summary>
        /// Gets / Sets DirectoryID
        /// </summary>
        public int DirectoryID
        {
            get
            {
                return directoryID;
            }
            set
            {
                directoryID = value;
            }
        }
        /// <summary>
        /// Gets / Sets FirstName
        /// </summary>
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
            }
        }

        /// <summary>
        /// Gets / Sets LastName
        /// </summary>
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
            }
        }

        /// <summary>
        /// Gets / Sets NickName
        /// </summary>
        public string NickName
        {
            get
            {
                return nickName;
            }
            set
            {
                nickName = value;
            }
        }

        /// <summary>
        /// Gets / Sets PrimaryPhone
        /// </summary>
        public string PrimaryPhone
        {
            get
            {
                return primaryPhone;
            }
            set
            {
                primaryPhone = value;
            }
        }

        /// <summary>
        /// Gets / Sets pager
        /// </summary>
        public string Pager
        {
            get
            {
                return pager;
            }
            set
            {
                pager = value;
            }
        }
        /// <summary>
        /// Gets / Sets cellPhone
        /// </summary>
        public string CellPhone
        {
            get
            {
                return cellPhone;
            }
            set
            {
                cellPhone = value;
            }
        }

        /// <summary>
        /// Gets / Sets AdditionalContName
        /// </summary>
        public string AdditionalContName
        {
            get
            {
                return additionalContName;
            }
            set
            {
                additionalContName = value;
            }
        }

        /// <summary>
        /// Gets / Sets AdditionalContPhone
        /// </summary>
        public string AdditionalContPhone
        {
            get
            {
                return additionalContPhone;
            }
            set
            {
                additionalContPhone = value;
            }
        }

        /// <summary>
        /// Gets / Sets primaryEmail
        /// </summary>
        public string PrimaryEmail
        {
            get
            {
                return primaryEmail;
            }
            set
            {
                primaryEmail = value;
            }
        }

        /// <summary>
        /// Gets / Sets Fax
        /// </summary>
        public string Fax
        {
            get
            {
                return fax;
            }
            set
            {
                fax = value;
            }
        }

        /// <summary>
        /// Gets / Sets Specialty
        /// </summary>
        public string Specialty
        {
            get
            {
                return specialty;
            }
            set
            {
                specialty = value;
            }
        }

        /// <summary>
        /// Gets / Sets Affiliation
        /// </summary>
        public string Affiliation
        {
            get
            {
                return affiliation;
            }
            set
            {
                affiliation = value;
            }
        }

        /// <summary>
        /// Gets / Sets PracticeGroup
        /// </summary>
        public string PracticeGroup
        {
            get
            {
                return practiceGroup;
            }
            set
            {
                practiceGroup = value;
            }
        }

        /// <summary>
        /// Gets / Sets Address1
        /// </summary>
        public string Address1
        {
            get
            {
                return address1;
            }
            set
            {
                address1 = value;
            }
        }

        /// <summary>
        /// Gets / Sets Address2
        /// </summary>
        public string Address2
        {
            get
            {
                return address2;
            }
            set
            {
                address2 = value;
            }
        }

        /// <summary>
        /// Gets / Sets Address3
        /// </summary>
        public string Address3
        {
            get
            {
                return address3;
            }
            set
            {
                address3 = value;
            }
        }

        /// <summary>
        /// Gets / Sets City
        /// </summary>
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }

        /// <summary>
        /// Gets / Sets State
        /// </summary>
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        /// <summary>
        /// Gets / Sets Zip
        /// </summary>
        public string Zip
        {
            get
            {
                return zip;
            }
            set
            {
                zip = value;
            }
        }


         /// <summary>
        /// Gets / Sets UpdatedBy
        /// </summary>
        public int UpdatedBy
        {
            get
            {
                return updatedBy;
            }
            set
            {
                updatedBy = value;
            }
        }

        /// <summary>
        /// Gets / Sets ReferringPhysicianID
        /// </summary>
        public int ReferringPhysicianID
        {
            get
            {
                return referringPhysicianID;
            }
            set
            {
                referringPhysicianID = value;
            }
        }

        /// <summary>
        /// Gets / Sets Active
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
        /// Gets / Sets Lab Top Doctor
        /// </summary>
        public bool LabTDR
        {
            get
            {
                return labTDR;
            }
            set
            {
                labTDR = value;
            }
        }

        /// <summary>
        /// Gets / Sets  Radiology Top Doctor
        /// </summary>
        public bool RadiologyTDR
        {
            get
            {
                return radiologyTDR;
            }
            set
            {
                radiologyTDR = value;
            }
        }

        /// <summary>
        /// Gets / Sets  additional Notes
        /// </summary>
        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = value;
            }
        }

        /// <summary>
        /// Gets / Sets whether profile updation Completed
        /// </summary>
        public bool ProfileReturn
        {
            get
            {
                return profileReturn;
            }
            set
            {
                profileReturn = value;
            }
        }
        /// <summary>
        /// Gets / Sets whether profile updation Completed
        /// </summary>
        public bool ProfileCompleted
        {
            get
            {
                return profileCompleted;
            }
            set
            {
                profileCompleted = value;
            }
        }


        /// <summary>
        /// Gets / Sets whether OC is resident or not
        /// </summary>
        public bool IsResident
        {
            get
            {
                return isResident;
            }
            set
            {
                isResident = value;
            }
        }

        /// <summary>
        /// Gets / Sets RIS ID
        /// </summary>
        public string IDRIS
        {
            get
            {
                return idRIS;
            }
            set
            {
                idRIS = value;
            }
        }

        /// <summary>
        /// Gets / Sets LIS ID
        /// </summary>
        public string IDLIS
        {
            get
            {
                return idLIS;
            }
            set
            {
                idLIS = value;
            }
        }

        /// <summary>
        /// Gets / Sets MSO ID
        /// </summary>
        public string IDMSO
        {
            get
            {
                return idMSO;
            }
            set
            {
                idMSO = value;
            }
        }

        /// <summary>
        /// Gets / Sets NPI
        /// </summary>
        public string IDNPI
        {
            get
            {
                return idNPI;
            }
            set
            {
                idNPI = value;
            }
        }

        /// <summary>
        /// Gets or Sets loginID for that object
        /// </summary>
        public string LoginID
        {
            get
            {
                return loginID;
            }
            set
            {
                loginID = value;
            }
        }

        /// <summary>
        /// Gets or Sets password for that object
        /// </summary>
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        /// <summary>
        /// Gets / Sets whether OC is EDDoc or not
        /// </summary>
        public bool IsEDDoc
        {
            get
            {
                return isEDDoc;
            }
            set
            {
                isEDDoc = value;
            }
        }

        /// <summary>
        /// Gets / Sets VocUserID
        /// </summary>
        public int VOCUserID
        {
            get
            {
                return vocUserID;
            }
            set
            {
                vocUserID = value;
            }
        }

        /// <summary>
        /// Gets / Sets InstituteID
        /// </summary>
        public int InstituteID
        {
            get
            {
                return instituteID;
            }
            set
            {
                instituteID = value;
            }
        }

        /// <summary>
        /// Gets / Sets PIN for Message Retrieve for OC
        /// </summary>
        public string PINForMessageRetrieve
        {
            get
            {
                return pinForMessage;
            }
            set
            {
                pinForMessage = value;
            }
        }

        /// <summary>
        ///  External Information: RIS ID
        /// </summary>
        public string RIS_ID
        {
            get
            {
                return risID;
            }
            set
            {
                risID = value;
            }
        }

        /// <summary>
        /// External Information: LIS ID
        /// </summary>
        public string LIS_ID
        {
            get
            {
                return lisID;
            }
            set
            {
                lisID = value;
            }
        }

        /// <summary>
        /// External Information: MSO ID
        /// </summary>
        public string MSO_ID
        {
            get
            {
                return msoID;
            }
            set
            {
                msoID = value;
            }
        }

        /// <summary>
        /// External Information: NPI
        /// </summary>
        public string NPI
        {
            get
            {
                return npi;
            }
            set
            {
                npi = value;
            }
        }
        #endregion
       
    }
    #endregion

    #region Class OCDeviceInfo
    /// <summary>
    /// Class that contains all private fields and public properties for OCDeviceInfo.
    /// </summary>
    public class OCDeviceInfo
    {
        #region Private Variables

        /// <summary>
        /// ReferringPhysicianID for Device
        /// </summary>
        private int referringPhysicianID;

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
        private int ocDeviceID;

        /// <summary>
        /// OCNotifyEventID for that device
        /// </summary>
        private int ocNotifyEventID;

        /// <summary>
        /// StartHour for After Hours Notifications
        /// </summary>
        private int startHour;

        /// <summary>
        /// EndHour for After Hours Notifications
        /// </summary>
        private int endHour;
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
        private int ocNotificationID;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or Sets referringPhysicianID for the object
        /// </summary>
        public int ReferringPhysicianID
        {
            get
            {
                return referringPhysicianID;
            }
            set
            {
                referringPhysicianID = value;
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
        public int OCDeviceID
        {
            get
            {
                return ocDeviceID;
            }
            set
            {
                ocDeviceID = value;
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
        public int OCNotifyEventID
        {
            get
            {
                return ocNotifyEventID;
            }
            set
            {
                ocNotifyEventID = value;
            }
        }

        /// <summary>
        /// Gets or Sets StartHour for After hours notifications
        /// </summary>
        public int StartHour
        {
            get
            {
                return startHour;
            }
            set
            {
                startHour = value;
            }
        }

        /// <summary>
        /// Gets or Sets EndHour for After hours notifications
        /// </summary>
        public int EndHour
        {
            get
            {
                return endHour;
            }
            set
            {
                endHour = value;
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
        public int OCNotificationID
        {
            get
            {
                return ocNotificationID;
            }
            set
            {
                ocNotificationID = value;
            }
        }
        #endregion

    }
    #endregion
}

