#region File History
/******************************File History***************************
 * File Name        : TestAndValues.cs
 * Author           : Swapnil K
 * Created Date     : 15-02-2007
 * Purpose          : Message Details class for Veriphy Lab.
 *                  : 
 *                  :
 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification
 * 
 * 01-28-2008   SBT     Added two properties - IsFwdToDept and IsOrgDeptMsg
 * ------------------------------------------------------------------- 
 */

#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Vocada.CSTools.Common
{
    /// <summary>
    /// The class that holds private variables and their respective proerties for Details of any Message
    /// </summary>
    public class MessageInfo
    {
        #region Private Fields
         /// <summary>
         /// Contains SpecialistID
         /// </summary>
         int specialistID;

        /// <summary>
        /// Contains either OCID or UnitID depending upon RecepientType
        /// </summary>
         int recepientID;

        /// <summary>
        /// Contains PatientVoiceURL
        /// </summary>
         string patientVoiceURL = string.Empty;

        /// <summary>
         /// Contains ImpressionVoiceURL string
        /// </summary>
         string impressionVoiceURL = string.Empty;
         
        /// <summary>
        /// Contains FindingID for message
        /// </summary>
         int findingID;

        /// <summary>
        /// DID for message
        /// </summary>
         string did = string.Empty;
        
        /// <summary>
        /// MRN for Message
        /// </summary> 
        string mrn = string.Empty;
        
        /// <summary>
        /// Contains binary value for forward
        /// </summary>
        int forward = 1;

        /// <summary>
        /// Original MessageID of message
        /// </summary>
         int originalMessageID;
         
        /// <summary>
        /// SubscriberID for Message
        /// </summary>
        int subscriberID;

        /// <summary>
        /// Indicates the recepient type whether it is OC / Unit / Bed
        /// </summary>
         int recepientTypeID;
         
        /// <summary>
        /// Contains Room Bed ID
        /// </summary>
        int roomBedID;
        
        /// <summary>
        /// Contains MessageID
        /// </summary>
        int messageID;

        /// <summary>
        /// Contains Accession
        /// </summary>
        string accession;

        /// <summary>
        /// need Readback
        /// </summary>
        bool needReadback;

        /// <summary>
        /// DOB
        /// </summary>
        DateTime dob;

        /// <summary>
        /// is Forward To Department
        /// </summary>
        bool isFwdToDept;

        /// <summary>
        /// is Original Department Message
        /// </summary>
        bool isOrgDeptMsg;

        #endregion

        #region Public Property
        /// <summary>
        /// Gets / Sets SpecialistID
        /// </summary>
        public int SpecialistID
        {
            get
            {
                return specialistID;
            }
            set
            {
                specialistID = value;
            }
        }
        /// <summary>
        /// Gets / Sets either OCID or UnitID depending upon RecepientType
        /// </summary>
        public int RecepientID
        {
            get
            {
                return recepientID;
            }
            set
            {
                recepientID = value;
            }
        }

        /// <summary>
        /// Gets / Sets PatientVoiceURL
        /// </summary>
        public string PatientVoiceURL
        {
            get
            {
                return patientVoiceURL;
            }
            set
            {
                patientVoiceURL = value;
            }
        }

        /// <summary>
        /// Gets / Sets ImpressionVoiceURL string
        /// </summary>
        public string ImpressionVoiceURL
        {
            get
            {
                return impressionVoiceURL;
            }
            set
            {
                impressionVoiceURL = value;
            }
        }

        /// <summary>
        /// Gets / Sets FindingID for message
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
        /// Gets / Sets DID for message
        /// </summary>
        public string DID
        {
            get
            {
                return did;
            }
            set
            {
                did = value;
            }
        }

        /// <summary>
        /// Gets / Sets MRN for message
        /// </summary>
        public string MRN
        {
            get
            {
                return mrn;
            }
            set
            {
                mrn = value;
            }
        }

        /// <summary>
        /// Gets / Sets binary value for forward
        /// </summary>
        public int Forward
        {
            get
            {
                return forward;
            }
            set
            {
                forward = value;
            }
        }

        /// <summary>
        /// Gets / Sets Original MessageID of message
        /// </summary>
        public int OriginalMessageID
        {
            get
            {
                return originalMessageID;
            }
            set
            {
                originalMessageID = value;
            }
        }

        /// <summary>
        /// Gets / Sets SubscriberID for Message
        /// </summary>
        public int SubscriberID
        {
            get
            {
                return subscriberID;
            }
            set
            {
                subscriberID = value;
            }
        }

        /// <summary>
        /// Gets / Sets the recepient type whether it is OC / Unit / Bed
        /// </summary>
        public int RecepientTypeID
        {
            get
            {
                return recepientTypeID;
            }
            set
            {
                recepientTypeID = value;
            }
        }

        /// <summary>
        /// Gets / Sets Room Bed ID
        /// </summary>
        public int RoomBedID
        {
            get
            {
                return roomBedID;
            }
            set
            {
                roomBedID = value;
            }
        }

        /// <summary>
        /// Gets / Sets MessageID
        /// </summary>
        public int MessageID
        {
            get
            {
                return messageID;
            }
            set
            {
                messageID = value;
            }
        }

        /// <summary>
        /// Gets / Sets Accession for message
        /// </summary>
        public string Accession
        {
            get
            {
                return accession;
            }
            set
            {
                accession = value;
            }
        }

        /// <summary>
        /// Need Readback
        /// </summary>
        public bool NeedReadback
        {
            get
            {
                return needReadback;
            }
            set
            {
                needReadback = value;
            }
        }
        
        /// <summary>
        /// Date of Birth
        /// </summary>
        public DateTime DOB
        {
            get
            {
                return dob;
            }
            set
            {
                dob = value;
            }
        }
        /// <summary>
        /// Gets / Sets value for is Forward To Department
        /// </summary>
        public bool IsFwdToDept
        {
            get
            {
                return isFwdToDept;
            }
            set
            {
                isFwdToDept = value;
            }
        }

        /// <summary>
        /// Gets / Sets value for is Original Department Message
        /// </summary>
        public bool IsOrgDeptMsg
        {
            get
            {
                return isOrgDeptMsg;
            }
            set
            {
                isOrgDeptMsg = value;
            }
        }
        #endregion

        #region Enum
        public enum RecepientType
        {
            OrderingClinician = 1,
            UnitName = 2,
            BedNumber = 3,
            Department = 4
        }
        #endregion

    }


}
