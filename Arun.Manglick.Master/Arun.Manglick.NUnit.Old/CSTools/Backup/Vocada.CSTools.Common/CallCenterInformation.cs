#region File History

/******************************File History***************************
 * File Name        : CallCenterInformation.cs
 * Author           : 
 * Created Date     : 
 * Purpose          : 
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * * Date(dd-mm-yyyy)       Developer       Reason of Modification

 * ------------------------------------------------------------------- 
 *  19-09-2008              Suhas           Removeing Alert Functionality
 * ------------------------------------------------------------------- 
 */
#endregion

#region Using Block
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Vocada.CSTools.Common;
using System.Collections;
#endregion

namespace Vocada.CSTools.Common
{
    /// <summary>
    /// Call Center Information Class
    /// </summary>
    public class CallCenterInformation
    {
        #region private Members
        private int callCenterID;
        private int institutionID;
        private string callCenterName = string.Empty;
        private bool isActive = false;
        private string contactName = string.Empty;
        private string contactPhone = string.Empty;
        private string email = string.Empty;
        private string pagerNumber = string.Empty;
        private string fax = string.Empty;
        private string alternateContactName = string.Empty;
        private string alternatePhone = string.Empty;
        private CallCenterPreferences oCallCenterPreferences = new CallCenterPreferences();
        #endregion private Members

        #region Property

        /// <summary>
        /// Gets or sets the obj call center preferences.
        /// </summary>
        /// <value>The obj call center preferences.</value>
        public CallCenterPreferences objCallCenterPreferences
        {
            get { return oCallCenterPreferences; }
            set { oCallCenterPreferences = value; }
        }

        /// <summary>
        /// Gets or sets the call center ID.
        /// </summary>
        /// <value>The call center ID.</value>
        public int CallCenterID
        {
            get { return callCenterID; }
            set { callCenterID = value; }
        }

        /// <summary>
        /// Gets or sets the name of the call center.
        /// </summary>
        /// <value>The name of the call center.</value>
        public string CallCenterName
        {
            get { return callCenterName; }
            set { callCenterName = value; }
        }

       
        /// <summary>
        /// Gets or sets the institution ID.
        /// </summary>
        /// <value>The institution ID.</value>
        public int InstitutionID
        {
            get { return institutionID; }
            set { institutionID = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value></value>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        /// <summary>
        /// Gets or sets the name of the contact.
        /// </summary>
        /// <value>The name of the contact.</value>
        public string ContactName
        {
            get { return contactName; }
            set { contactName = value; }
        }

        /// <summary>
        /// Gets or sets the contact phone.
        /// </summary>
        /// <value>The contact phone.</value>
        public string ContactPhone
        {
            get { return contactPhone; }
            set { contactPhone = value; }
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// Gets or sets the pager number.
        /// </summary>
        /// <value>The pager number.</value>
        public string PagerNumber
        {
            get { return pagerNumber; }
            set { pagerNumber = value; }
        }

        /// <summary>
        /// Gets or sets the fax.
        /// </summary>
        /// <value>The fax.</value>
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        /// <summary>
        /// Gets or sets the name of the alternate contact.
        /// </summary>
        /// <value>The name of the alternate contact.</value>
        public string AlternateContactName
        {
            get { return alternateContactName; }
            set { alternateContactName = value; }
        }

        /// <summary>
        /// Gets or sets the alternate phone.
        /// </summary>
        /// <value>The alternate phone.</value>
        public string AlternatePhone
        {
            get { return alternatePhone; }
            set { alternatePhone = value; }
        }
         
        #endregion Property
    }

    /// <summary>
    /// Call Center Preferences Class
    /// </summary>
    public class CallCenterPreferences
    {
        #region private Members
        //private int escalationPeriod1;
        //private int escalationPeriod2;
        private string lockedMessageTimeout;
        private string autoLogout;
        //private bool newMessageAlert = false;
        //private bool outOfComplianceAlert = false;
        private bool messageClosedAlert = false;
        //private bool escalation1Alert = false;
        //private bool escalation2Alert = false;
        private bool confirmationSendPopup = false;
        private bool confirmationDocPopup = false;
        private bool confirmationConnectPopup = false;
        private bool confirmationManuallyClosedPopup = false;        
        #endregion private Members  

        #region Property
        
        /// <summary>
        /// Gets or sets the escalation period1.
        /// </summary>
        /// <value>The escalation period1.</value>
        /*public int EscalationPeriod1
        {
            get { return escalationPeriod1; }
            set { escalationPeriod1 = value; }
        }*/

        /// <summary>
        /// Gets or sets the escalation period2.
        /// </summary>
        /// <value>The escalation period2.</value>
        /*public int EscalationPeriod2
        {
            get { return escalationPeriod2; }
            set { escalationPeriod2 = value; }
        }*/

        /// <summary>
        /// Gets or sets the locked message timeout.
        /// </summary>
        /// <value>The locked message timeout.</value>
        public string LockedMessageTimeout
        {
            get { return lockedMessageTimeout; }
            set { lockedMessageTimeout = value; }
        }

        /// <summary>
        /// Gets or sets the auto logout.
        /// </summary>
        /// <value>The auto logout.</value>
        public string AutoLogout
        {
            get { return autoLogout; }
            set { autoLogout = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [new message alert].
        /// </summary>
        /// <value></value>
        /*public bool NewMessageAlert
        {
            get { return newMessageAlert; }
            set { newMessageAlert = value; }
        }*/

        /// <summary>
        /// Gets or sets a value indicating whether [out of compliance alert].
        /// </summary>
        /// <value></value>
        /*public bool OutOfComplianceAlert
        {
            get { return outOfComplianceAlert; }
            set { outOfComplianceAlert = value; }
        }*/

        /// <summary>
        /// Gets or sets a value indicating whether [message closed alert].
        /// </summary>
        /// <value></value>
        public bool MessageClosedAlert
        {
            get { return messageClosedAlert; }
            set { messageClosedAlert = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [escalation1 alert].
        /// </summary>
        /// <value></value>
        /*public bool Escalation1Alert
        {
            get { return escalation1Alert; }
            set { escalation1Alert = value; }
        }*/

        /// <summary>
        /// Gets or sets a value indicating whether [escalation2 alert].
        /// </summary>
        /// <value></value>
        /*public bool Escalation2Alert
        {
            get { return escalation2Alert; }
            set { escalation2Alert = value; }
        }*/

        /// <summary>
        /// Gets or sets a value indicating whether [confirmation send popup].
        /// </summary>
        /// <value></value>
        public bool ConfirmationSendPopup
        {
            get { return confirmationSendPopup; }
            set { confirmationSendPopup = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [confirmation doc popup].
        /// </summary>
        /// <value></value>
        public bool ConfirmationDocPopup
        {
            get { return confirmationDocPopup; }
            set { confirmationDocPopup = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [confirmation connect popup].
        /// </summary>
        /// <value></value>
        public bool ConfirmationConnectPopup
        {
            get { return confirmationConnectPopup; }
            set { confirmationConnectPopup = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [confirmation manually closed popup].
        /// </summary>
        /// <value></value>
        public bool ConfirmationManuallyClosedPopup
        {
            get { return confirmationManuallyClosedPopup; }
            set { confirmationManuallyClosedPopup = value; }
        }
        #endregion Property
    }
}
