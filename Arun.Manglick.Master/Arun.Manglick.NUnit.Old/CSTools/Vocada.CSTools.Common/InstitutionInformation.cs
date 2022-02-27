#region File History

/******************************File History***************************
 * File Name        : InstitutionInformation.cs
 * Author           : IAK.
 * Created Date     : 3 July 07
 * Purpose          : Structure for Institution Information.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * * Date(dd-mm-yyyy) Developer Reason of Modification

 * ------------------------------------------------------------------- 
 *  14-12-2007      IAK      Added BatchMessages Property
 *  03-20-2008      SSK      Added MessageRetrieveUsingPIN Property
 *  30-05-2008      Suhas    Added property EnableCallCenter
 *  09-12-2008      IAK      Added property EnablePromptForPin
 *  11-04-2009      SD       Added property  AmcomPagerURL.
 * ------------------------------------------------------------------- 
 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace Vocada.CSTools.Common
{
    public class InstitutionInformation
    {
        #region private Members
        private int institutionID;
        private string institutionName = string.Empty;
        private string address1 = string.Empty;
        private string address2 = string.Empty;
        private string city = string.Empty;
        private string state = string.Empty;
        private string zip = string.Empty;
        private string mainPhoneNumber = string.Empty;
        private string primaryContactName = string.Empty;
        private string primaryContactTitle = string.Empty;
        private string primaryContactPhone = string.Empty;
        private string primaryContactEmail = string.Empty;
        private string contact1Type = string.Empty;
        private string contact1Name = string.Empty;
        private string contact1Title = string.Empty;
        private string contact1Phone = string.Empty;
        private string contact1Email = string.Empty;
        private string contact2Type = string.Empty;
        private string contact2Name = string.Empty;
        private string contact2Title = string.Empty;
        private string contact2Phone = string.Empty;
        private string contact2Email = string.Empty;
        private string lab800Number = string.Empty;
        private string nurse800Number = string.Empty;
        private string shiftNurse800Number = string.Empty;
        private string amcomPagerURL = string.Empty;
        private int timeZone ;
        private string institutionVoiceOverURL = string.Empty;
        private bool isRequireCallBackVoiceOver = false;
        private bool isReuireNameCapture = false;
        private bool isRequireNameCaptureValidation = false;
        private bool isRequireReadbackMeasurement = false;
        private bool isRequireAcceptanceOutboundCall = false;
        private bool isRequireEDMessage = false;
        private bool isRequireExamDescription = false;
        private bool tabName = false;
        private bool batchMessage = false;
        private bool messageRetrieveUsingPIN = false;
        private bool enableCallCenter = false;
        private bool enablePromptForPin = false;
      

        #endregion private Members

        #region Property

        /// <summary>
        /// Get Institution ID 
        /// </summary>
        public int InstitutionID
        {
            get
            {
                return institutionID;
            }
            set
            {
                institutionID = value;
            }
        }

        /// <summary>
        /// Get Institution Name
        /// </summary>
        public string InstitutionName
        {
            get
            {
                return institutionName;
            }
            set
            {
                institutionName = value;
            }
        }

        /// <summary>
        /// Address Line 1
        /// </summary>
        public string Address1
        {
            get { return address1; }
            set { address1 = value; }
        }
        /// <summary>
        /// Address Line 2
        /// </summary>
        public string Address2
        {
            get { return address2; }
            set { address2 = value; }
        }
        /// <summary>
        /// City
        /// </summary>
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        /// <summary>
        /// State
        /// </summary>
        public string State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// Zip
        /// </summary>
        public string Zip
        {
            get { return zip; }
            set { zip = value; }
        }
        /// <summary>
        /// Main Phone Number
        /// </summary>
        public string MainPhoneNumber
        {
            get { return mainPhoneNumber; }
            set { mainPhoneNumber = value; }
        }

        /// <summary>
        /// primary Contact Name
        /// </summary>
        public string PrimaryContactName
        {
            get { return primaryContactName; }
            set { primaryContactName = value; }
        }
        /// <summary>
        /// primary Contact Title
        /// </summary>
        public string PrimaryContactTitle
        {
            get { return primaryContactTitle; }
            set { primaryContactTitle = value; }
        }
        /// <summary>
        /// primary Contact Phone
        /// </summary>
        public string PrimaryContactPhone
        {
            get { return primaryContactPhone; }
            set { primaryContactPhone = value; }
        }
        /// <summary>
        /// primary Contact Email
        /// </summary>
        public string PrimaryContactEmail
        {
            get { return primaryContactEmail; }
            set { primaryContactEmail = value; }
        }
        /// <summary>
        /// Contact1 Name
        /// </summary>
        public string Contact1Name
        {
            get { return contact1Name; }
            set { contact1Name = value; }
        }
        /// <summary>
        /// Contact1 Title
        /// </summary>
        public string Contact1Title
        {
            get { return contact1Title; }
            set { contact1Title = value; }
        }
        /// <summary>
        /// Contact1 Phone
        /// </summary>
        public string Contact1Phone
        {
            get { return contact1Phone; }
            set { contact1Phone = value; }
        }
        /// <summary>
        /// Contact1 Email
        /// </summary>
        public string Contact1Email
        {
            get { return contact1Email; }
            set { contact1Email = value; }
        }
        /// <summary>
        /// Contact1 Type
        /// </summary>
        public string Contact1Type
        {
            get { return contact1Type; }
            set { contact1Type = value; }
        }
        /// <summary>
        /// Contact2 Name
        /// </summary>
        public string Contact2Name
        {
            get { return contact2Name; }
            set { contact2Name = value; }
        }
        /// <summary>
        /// Contact2 Title
        /// </summary>
        public string Contact2Title
        {
            get { return contact2Title; }
            set { contact2Title = value; }
        }
        /// <summary>
        /// Contact2 Phone
        /// </summary>
        public string Contact2Phone
        {
            get { return contact2Phone; }
            set { contact2Phone = value; }
        }
        /// <summary>
        /// Contact2 Email
        /// </summary>
        public string Contact2Email
        {
            get { return contact2Email; }
            set { contact2Email = value; }
        }
        /// <summary>
        /// Contact2 Type
        /// </summary>
        public string Contact2Type
        {
            get { return contact2Type; }
            set { contact2Type = value; }
        }

        /// <summary>
        /// Get Lab 800 Number
        /// </summary>
        public string Lab800Number
        {
            get
            {
                return lab800Number;
            }
            set
            {
                lab800Number = value;
            }
        }

        /// <summary>
        /// Get Nurse 800 Number
        /// </summary>
        public string Nurse800Number
        {
            get
            {
                return nurse800Number;
            }
            set
            {
                nurse800Number = value;
            }
        }

        /// <summary>
        /// Shift Nurse 800 Number for nursh shift assignment
        /// </summary>
        public string ShiftNurse800Number
        {
            get { return shiftNurse800Number; }
            set { shiftNurse800Number = value; }
        }
        /// <summary>
        /// Timezone
        /// </summary>
        public int TimeZone
        {
            get { return timeZone; }
            set { timeZone = value; }
        }
        ///<summary>
        /// institution Voice Over URL
        ///</summary>
        public string InstitutionVoiceOverURL
        {
            get { return institutionVoiceOverURL; }
            set { institutionVoiceOverURL = value; }
        }

        /// <summary>
        /// AmcomPagerURL
        /// </summary>
        public string AmcomPagerURL
        {
            get { return amcomPagerURL; }
            set { amcomPagerURL = value; }
        }
        
        /// <summary>
        /// Require Call Back Voice Over in VUI Lab
        /// </summary>
        public bool IsRequireCallBackVoiceOver
        {
            get { return isRequireCallBackVoiceOver; }
            set { isRequireCallBackVoiceOver = value; }
        }
        /// <summary>
        /// Require Name capture in VUI Lab
        /// </summary>
        public bool IsRequireNameCapture
        {
            get { return isReuireNameCapture; }
            set { isReuireNameCapture = value; }
        }
        /// <summary>
        /// Require Name capture Validation in VUI Lab
        /// </summary>
        public bool IsRequireNameCaptureValidation
        {
            get { return isRequireNameCaptureValidation; }
            set { isRequireNameCaptureValidation = value; }
        }
        /// <summary>
        /// Require unit measurement readback in VUI Lab
        /// </summary>
        public bool IsRequireReadbackMeasurement
        {
            get { return isRequireReadbackMeasurement; }
            set { isRequireReadbackMeasurement = value; }
        }
        /// <summary>
        /// Require unit measurement readback in VUI Lab
        /// </summary>
        public bool IsRequireAcceptanceOutboundCall
        {
            get { return isRequireAcceptanceOutboundCall; }
            set { isRequireAcceptanceOutboundCall = value; }
        }
        /// <summary>
        /// Require unit measurement readback in VUI Lab
        /// </summary>
        public bool IsRequireEDMessage
        {
            get { return isRequireEDMessage; }
            set { isRequireEDMessage = value; }
        }
        /// <summary>
        /// Require Exam Description 
        /// </summary>
        public bool IsRequireExamDescription
        {
            get { return isRequireExamDescription; }
            set { isRequireExamDescription = value; }
        }
        /// <summary>
        /// Require unit measurement readback in VUI Lab
        /// </summary>
        public bool TabName
        {
            get { return tabName; }
            set { tabName = value; }
        }
        /// <summary>
        /// Batch Message
        /// </summary>
        public bool BatchMessage
        {
            get { return batchMessage; }
            set { batchMessage = value; }
        }
        /// <summary>
        /// Message Retrieve Using PIN
        /// </summary>
        public bool MessageRetrieveUsingPIN
        {
            get { return messageRetrieveUsingPIN; }
            set { messageRetrieveUsingPIN = value; }
        }

        /// <summary>
        /// Message Retrieve Using PIN
        /// </summary>
        public bool EnableCallCenter
        {
            get { return enableCallCenter; }
            set { enableCallCenter = value; }
        }

        /// <summary>
        /// Prompt for PIN for CT Message
        /// </summary>
        public bool EnablePromptForPin
        {
            get { return enablePromptForPin; }
            set { enablePromptForPin = value; }
        }
                             

        #endregion Property
    }
}
