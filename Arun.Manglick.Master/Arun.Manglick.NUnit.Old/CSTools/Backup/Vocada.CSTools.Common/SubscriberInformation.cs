#region File History
/******************************File History***************************
 * File Name        : AddSubscriber.cs
 * Author           : Prerak S
 * Created Date     : 15-06-2007
 * Purpose          : Subscriber.
 *                  : 
 *                  :
 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 07-08-2007 - Specialist Properties Added
 * ------------------------------------------------------------------- 
 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace Vocada.CSTools.Common
{
    /// <summary>
    /// The class that holds private variables and their respective proerties for Details of any Subscrber
    /// </summary>
    public class SubscriberInformation
    {
        #region Private Fields
        /// <summary>
        /// Contains SubscriberID
        /// </summary>
        int subscriberID;

        /// <summary>
        /// Contains GroupID
        /// </summary>
        int groupID;

        /// <summary>
        /// Contains LoginID
        /// </summary>
        string loginID= string.Empty ;

        /// <summary>
        /// Contains Password
        /// </summary>
        string password = string.Empty;

        /// <summary>
        /// Contains RoleID
        /// </summary>
        int roleID;

        /// <summary>
        /// Indicates whether Subscriber is Active or Inactive.
        /// </summary>
        bool active;

        /// <summary>
        /// Contains FirstName 
        /// </summary> 
        string firstName = string.Empty;

        /// <summary>
        /// Contains Nickname
        /// </summary>
        string nickname = string.Empty;

        /// <summary>
        /// Contains LastName
        /// </summary>
        string lastName = string.Empty;

        /// <summary>
        /// PrimaryEmail of Subscriber
        /// </summary>
        string primaryEmail = string.Empty;

        /// <summary>
        /// PrimaryEmailNotify
        /// </summary>
        int primaryEmailNotify;

        /// <summary>
        /// Contains PrimaryPhone number
        /// </summary>
        string primaryPhone = string.Empty;

        /// <summary>
        /// Contains Fax
        /// </summary>
        string fax = string.Empty ;

        /// <summary>
        /// Contains FaxNotify
        /// </summary>
        int faxNotify;

        /// <summary>
        /// Contains LastUpdated date
        /// </summary>
        DateTime lastUpdated;

        /// <summary>
        /// Contains specialistID
        /// </summary>
        int specialistID;

        /// <summary>
        /// Contains VoiceOverURL
        /// </summary>
        string voiceOverURL = string.Empty;

        /// <summary>
        /// Contains Affiliation
        /// </summary>
        string affiliation = string.Empty;

        /// <summary>
        /// Contains Specialty
        /// </summary>
        string specialty = string.Empty;

        #endregion

        #region Public Property
        /// <summary>
        /// Gets / Sets SubscriberID
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
        /// Gets / Sets GroupID
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
        /// Gets / Sets LoginID 
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
        /// Gets / Sets Password
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
        /// Gets / Sets RoleID 
        /// </summary>
        public int RoleID
        {
            get
            {
                return roleID;
            }
            set
            {
                roleID = value;
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
                return nickname;
            }
            set
            {
                nickname = value;
            }
        }

        /// <summary>
        /// Gets / Sets Original MessageID of message
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
        /// Gets / Sets primaryEmailNotify 
        /// </summary>
        public int PrimaryEmailNotify
        {
            get
            {
                return primaryEmailNotify;
            }
            set
            {
                primaryEmailNotify = value;
            }
        }

        /// <summary>
        /// Gets / Sets primaryPhone
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
        /// Gets / Sets MessageID
        /// </summary>
        public int FaxNotify
        {
            get
            {
                return faxNotify;
            }
            set
            {
                faxNotify = value;
            }
        }

        /// <summary>
        /// Gets / Sets lastUpdated
        /// </summary>
        public DateTime LastUpdated
        {
            get
            {
                return lastUpdated;
            }
            set
            {
                lastUpdated = value;
            }
        }

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
        /// Gets / Sets VoiceOverURL
        /// </summary>
        public string VoiceOverURL
        {
            get
            {
                return voiceOverURL;
            }
            set
            {
                voiceOverURL = value;
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

        #endregion

    }
}
