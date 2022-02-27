/******************************File History***************************
 * File Name        : GroupInformation.cs
 * Author           : Prerak Shah.
 * Created Date     : 16-07-2007
 * Purpose          : This Class will provide properties of Group.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 17-07-2007 - Group Preferences property Added  
 * 03-08-2007 - CSRFirstName,CSRMiddleName,CSRLastName,Active properties added
 * 19-11-2007 - Added properties for AllowAlphanumaricMRN and UseCcasBackup
 * 04-12-2007 - Added Properties for DirectoryTabOnDesktop
 * 11-12-2007 - Prerak - Added FaxTemplateURL, UnitFaxTemplateURL, and CTFaxTemplate columns property.
 * 12-12-2007 - Prerak - Rename CTFaxTemplateURL to GroupTemplateURL, Remove UnitFaxTemplateURL
 * 13-12-2007 - Prerak - Columns added CTFaxTemplateURL and UnitFaxTemplateURL
 * 17-12-2007 - Prerak - Removed URL word from fax templates
 * 10-03-2008 - Suhas  - Added property PagerTAP800Number  
 * 08-04-2008 - Prerak - Added Property AllowSmsTextMsgWebLink
 * 14-04-2008 - Prerak - Added Property AllowVUIMsgForwarding
 * 16-04-2008 - Prerak - Added Properties MsgForwardingAlert and ForwardedMsgClosedAlert
 * 17-04-2008 - Prerak - Remove Property AllowSmsTextMsgWebLink
 * 30-05-2008 - Suhas  - Added property AllowSendToAgent
 * 14-07-2008 - Prerak - Added Properties for RequirePatientNameInEmail and RequirePatientNameInPagerAndSMS
 * 28-08-2008 - IAK    - Added DS property
 * 30-09-2008 - Raju G - Added RequireFwdLabMsgReadback property
 * 30-10-2008 - SD     - Removed messageForwardingAlert, forwardedMessageClosedAlert
 * 19-02-2009 - Arun M - Added three fields - persistOrderInfo, allowAccession, ccRecipientType

 * -------------------------------------------------------------------- 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Vocada.CSTools.Common
{
    public class GroupInformation
    {
        #region private Members
        private int groupID;
        private int  directoryID ;
        private string groupDID = string.Empty;
        private string group800Number = string.Empty;
        private string referringPhysicianDID = string.Empty;
        private string referringPhysician800Number = string.Empty;
        private string groupName ;
        private int practiceType ;
        private string address1 = string.Empty;
        private string address2 = string.Empty;
        private string city = string.Empty;
        private string state = string.Empty;
        private string zip = string.Empty;
        private string phone = string.Empty;
        private string affiliation = string.Empty;
        private string groupVoiceURL = string.Empty;
        private int timeZoneID;
        private string groupGraphicLocation = string.Empty;
        private int institutionID ;
        private bool groupType = false ;
        //Group Preferences 
        private int messageActiveForDays;
        private int archiveMessagesForDays;
        private int overdueThreshold;
        private bool requireMRN;
        private bool requireRPAcceptance;
        private bool requireAccession;
        private bool requireNameCapture;
        private bool allowDownload;
        private bool requirePatientInitials;
        private bool requireDOBIdentifier;
        private bool closePrimaryAndBackupMessages;
        private string csrFirstName = string.Empty;
        private string csrMiddleName = string.Empty;
        private string csrLastName = string.Empty;
        private bool isGroupActive = true;
        private bool useCcAsBackup;
        private bool allowAlphanumericMRN;
        private bool vuiErrors;
        private bool directoryTabOnDesktop;
        private string ocFaxTemplate;
        private string unitFaxTemplate;
        private string groupFaxTemplate;
        private string ctFaxTemplate;
        private string pagerTAP800Number;
        private bool allowVuiMsgForwarding;
        //private bool messageForwardingAlert;
        //private bool forwardedMessageClosedAlert;
        private bool allowSendToAgent;
        private bool requirePatientNameInPagerAndSMS;
        private bool requirePatientNameInEmail;
        private bool enableDirectorySynchronization;
        private bool requireReadbackForFwdLabMsg;
        private bool persistOrderInfo;
        private bool allowAccession;
        private int ccRecipientType;
        private int persistOrderForDays;
        
        #endregion

        #region Property

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
        /// Get directory ID 
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
        ///  groupDID 
        /// </summary>
        public string GroupDID
        {
            get { return groupDID; }
            set { groupDID = value; }
        }
        /// <summary>
        /// Group 800 Number 
        /// </summary>
        public string Group800Number
        {
            get 
            { 
                return group800Number; 
            }
            set 
            { 
                group800Number = value; 
            }
        }
        /// <summary>
        /// Referring Physician DID
        /// </summary>
        public string ReferringPhysicianDID
        {
            get 
            { 
                return referringPhysicianDID;
            }
            set 
            { 
                referringPhysicianDID = value; 
            }
        }
        /// <summary>
        ///Referring Physician 800 Number
        /// </summary>
        public string ReferringPhysician800Number
        {
            get 
            { 
                return referringPhysician800Number; 
            }
            set
            { 
                referringPhysician800Number = value; 
            }
        }
        /// <summary>
        /// Get Group Name
        /// </summary>
        public string GroupName
        {
            get
            {
                return groupName;
            }
            set
            {
                groupName = value;
            }
        }
        /// <summary>
        /// PracticeT ype
        /// </summary>
        public int PracticeType
        {
            get
            {
                return practiceType;
            }
            set
            {
                practiceType = value;
            }
        }
        /// <summary>
        /// Address Line 1
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
        /// Address Line 2
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
        /// City
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
        /// State
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
        /// Zip
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
        /// Phone Number
        /// </summary>
        public string Phone
        {
            get 
            { 
                return phone; 
            }
            set 
            { 
                phone = value; 
            }
        }
        /// <summary>
        /// Affiliation
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
        /// groupVoiceURL
        /// </summary>
        public string GroupVoiceURL
        {
            get
            { 
                return groupVoiceURL;
            }
            set
            { 
                groupVoiceURL = value;
            }
        }
        /// <summary>
        /// Timezone ID
        /// </summary>
        public int TimeZoneID
        {
            get  
            { 
                return timeZoneID;
            }
            set
            { 
                timeZoneID = value; 
            }
        }
        /// <summary>
        /// GroupGraphicLocation
        /// </summary>
        public string GroupGraphicLocation
        {
            get 
            { 
                return groupGraphicLocation; 
            }
            set 
            { 
                groupGraphicLocation = value; 
            }
        }
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
        /// Group Type
        /// </summary>
        public bool GroupType
        {
            get
            {
                return groupType;
            }
            set 
            { groupType = value;
            }
        }
        //Group Preferences
        /// <summary>
        /// Nessage Active For Days
        /// </summary>
        public int MessageActiveForDays
        {
            get
            {
                return messageActiveForDays;
            }
            set
            {
                messageActiveForDays = value;
            }
        }
        /// <summary>
        /// Archive Messages For Days
        /// </summary>
        public int ArchiveMessagesForDays
        {
            get
            {
                return archiveMessagesForDays;
            }
            set
            {
                archiveMessagesForDays = value;
            }
        }
        /// <summary>
        /// Overdue Threshold
        /// </summary>
        public int OverdueThreshold
        {
            get
            {
                return overdueThreshold;
            }
            set
            {
                overdueThreshold = value;
            }
        }
        /// <summary>
        /// Require MRN
        /// </summary>
        public bool RequireMRN
        {
            get
            {
                return requireMRN;
            }
            set
            {
                requireMRN = value;
            }
        }
        /// <summary>
        /// Require Referring Phycisian Acceptance
        /// </summary>
        public bool RequireRPAcceptance
        {
            get
            {
                return requireRPAcceptance;
            }
            set
            {
                requireRPAcceptance = value;
            }
        }
        /// <summary>
        /// Require Accession
        /// </summary>
        public bool RequireAccession
        {
            get
            {
                return requireAccession;
            }
            set
            {
                requireAccession = value;
            }
        }
        /// <summary>
        /// Require Name Capture
        /// </summary>
        public bool RequireNameCapture
        {
            get
            {
                return requireNameCapture;
            }
            set
            {
                requireNameCapture = value;
            }
        }
        /// <summary>
        /// Allow Download
        /// </summary>
        public bool AllowDownload
        {
            get
            {
                return allowDownload;
            }
            set
            {
                allowDownload = value;
            }
        }
        /// <summary>
        /// Require Patient Initials
        /// </summary>
        public bool RequirePatientInitials
        {
            get
            {
                return requirePatientInitials;
            }
            set
            {
                requirePatientInitials = value;
            }
        }
        /// <summary>
        /// Require Date Of Birth Identifier
        /// </summary>
        public bool RequireDOBIdentifier
        {
            get
            {
                return requireDOBIdentifier;
            }
            set
            {
                requireDOBIdentifier = value;
            }

        }
        /// <summary>
        /// Close Primary And BackupMessages
        /// </summary>
        public bool ClosePrimaryAndBackupMessages
        {
            get
            {
                return closePrimaryAndBackupMessages;
            }
            set
            {
                closePrimaryAndBackupMessages = value;
            }
        }
        /// <summary>
        /// CSR First Name
        /// </summary>
        public string CSRFirstName
        {
            get
            {
                return csrFirstName;
            }
            set
            {
                csrFirstName = value;
            }
        }
        /// <summary>
        /// CSR Middle Name
        /// </summary>
        public string CSRMiddleName
        {
            get
            {
                return csrMiddleName ;
            }
            set
            {
                csrMiddleName = value;
            }
        }
        /// <summary>
        /// CSR Last Name
        /// </summary>
        public string CSRLastName
        {
            get
            {
                return csrLastName ;
            }
            set
            {
                csrLastName = value;
            }
        }
        /// <summary>
        /// Active Group
        /// </summary>
        public bool IsGroupActive
        {
            get
            {
                return isGroupActive;
            }
            set
            {
                isGroupActive = value;
            }
        }
        /// <summary>
        /// Allow Alphanumeric MRN
        /// </summary>
        public bool AllowAlphanumericMRN
        {
            get
            {
                return allowAlphanumericMRN;
            }
            set
            {
                allowAlphanumericMRN = value;
            }
        }
        /// <summary>
        /// Use Cc As Backup
        /// </summary>
        public bool UseCcAsBackup
        {
            get
            {
                return useCcAsBackup;
            }
            set
            {
                useCcAsBackup = value;
            }
        }
        /// <summary>
        /// VUIErrors
        /// </summary>
        public bool VUIErrors
        {
            get
            {
                return vuiErrors;
            }
            set
            {
                vuiErrors = value;
            }
        }
        /// <summary>
        /// Directory Tab On Desktop
        /// </summary>
        public bool DirectoryTabOnDesktop
        {
            get
            {
                return directoryTabOnDesktop;
            }
            set
            {
                directoryTabOnDesktop = value;
            }
        }
        /// <summary>
        /// OC Fax Template 
        /// </summary>
        public string OCFaxTemplate
        {
            get
            {
                return ocFaxTemplate;
            }
            set
            {
                ocFaxTemplate = value;
            }
        }
        /// <summary>
        /// GROUP Fax Template 
        /// </summary>
        public string GroupFaxTemplate
        {
            get
            {
                return groupFaxTemplate;
            }
            set
            {
                groupFaxTemplate = value;
            }
        }
        /// <summary>
        /// Unit Fax Template 
        /// </summary>
        public string UnitFaxTemplate
        {
            get
            {
                return unitFaxTemplate;
            }
            set
            {
                unitFaxTemplate = value;
            }
        }
        /// <summary>
        /// Clanical Team Fax Template 
        /// </summary>
        public string CTFaxTemplate
        {
            get
            {
                return ctFaxTemplate;
            }
            set
            {
                ctFaxTemplate = value;
            }
        }
        /// <summary>
        /// Pager TAP 800 Number
        /// </summary>
        public string PagerTAP800Number
        {
            get
            {
                return pagerTAP800Number;
            }
            set
            {
                pagerTAP800Number = value;
            }
        }
        /// <summary>
        /// Allow VUI Message Forwarding
        /// </summary>
        public bool AllowVUIMsgForwarding
        {
            get
            {
                return allowVuiMsgForwarding;
            }
            set
            {
                allowVuiMsgForwarding = value;
            }
        }
        /// <summary>
        /// Message Forwarding Alert
        /// </summary>
        //public bool MessageForwardingAlert
        //{
        //    get
        //    {
        //        return messageForwardingAlert;
        //    }
        //    set
        //    {
        //        messageForwardingAlert = value;
        //    }
        //}
        /// <summary>
        /// Forwarded Message Closed Alert
        /// </summary>
        //public bool ForwardedMessageClosedAlert
        //{
        //    get
        //    {
        //        return forwardedMessageClosedAlert;
        //    }
        //    set
        //    {
        //        forwardedMessageClosedAlert = value;
        //    }
        //}

        /// <summary>
        /// Forwarded Message Closed Alert
        /// </summary>
        public bool AllowSendToAgent
        {
            get
            {
                return allowSendToAgent;
            }
            set
            {
                allowSendToAgent = value;
            }
        }
        /// <summary>
        /// Require Patient Name In Pager And SMS
        /// </summary>
        public bool RequirePatientNameInPagerAndSMS
        {
            get
            {
                return requirePatientNameInPagerAndSMS;
            }
            set
            {
                requirePatientNameInPagerAndSMS = value;
            }
        }
        /// <summary>
        /// Require Patient Name In Email 
        /// </summary>
        public bool RequirePatientNameInEmail
        {
            get
            {
                return requirePatientNameInEmail;
            }
            set
            {
                requirePatientNameInEmail = value;
            }
        }

        /// <summary>
        /// Enable Directory Synchronization tab on Veriphy Web site for GA
        /// </summary>
        public bool EnableDirectorySynchronization
        {
            get
            {
                return enableDirectorySynchronization;
            }
            set
            {
                enableDirectorySynchronization = value;
            }
        }

         /// <summary>
        /// Enable / DIsable Require readback for forwarded lab message
        /// </summary>
        public bool RequireReadbackForFwdLabMsg
        {
            get
            {
                return requireReadbackForFwdLabMsg;
            }
            set
            {
                requireReadbackForFwdLabMsg = value;
            }
        }

        /// <summary>
        /// Enable / Disable PersistOrderInfo
        /// </summary>
        public bool PersistOrderInfo
        {
            get { return persistOrderInfo; }
            set { persistOrderInfo = value; }
        }

        /// <summary>
        /// Enable / Disable AllowAccession
        /// </summary>
        public bool AllowAccession
        {
            get { return allowAccession; }
            set { allowAccession = value; }
        }
        
        /// <summary>
        /// CCRecipientType
        /// </summary>
        public int CcRecipientType
        {
            get { return ccRecipientType; }
            set { ccRecipientType = value; }
        }

        /// <summary>
        /// Persist Order For Days
        /// </summary>
        public int PersistOrderForDays
        {
            get { return persistOrderForDays; }
            set { persistOrderForDays = value; }
        }

        #endregion

    }
}
