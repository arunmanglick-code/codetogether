#region File History

/******************************File History***************************
 * File Name        : AgentInformation.cs
 * Author           : Suhas Tarihalkar.
 * Created Date     : 10 June 2008
 * Purpose          : This Class provide common properties which specify Call Center user information 
 *                  : 
 *                  :
 * *********************File Modification History*********************
 * * Date(dd-mm-yyyy) Developer Reason of Modification
 * ------------------------------------------------------------------- 
 *
 * ------------------------------------------------------------------- 
 
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Vocada.CSTools.Common
{
    /// <summary>
    /// CSTools User Information class.
    /// </summary>
    public class AgentInformation
    {
        #region private fields
        private int vocUserID;
        private string firstName = "";
        private string lastName = "";
        private string email = "";
        private string phone;
        private int roleID;
        private bool status;
        private string loginID;
        private string password;
        private int callCenterID;
        private int institutionID;
        private string mobileNumber;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the VOC user ID.
        /// </summary>
        /// <value>The VOC user ID.</value>
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
        /// Gets or sets the name of the first.
        /// </summary>
        /// <value>The name of the first.</value>
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
        /// Gets or sets the name of the last.
        /// </summary>
        /// <value>The name of the last.</value>
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
        /// Gets or sets the mobile phone.
        /// </summary>
        /// <value>The Mobile.</value>
        public string MobileNumber
        {
            get
            {
                return mobileNumber;
            }
            set
            {
                mobileNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
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
        /// Gets or sets the role ID.
        /// </summary>
        /// <value>The role ID.</value>
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
        /// Gets or sets a value indicating whether this <see cref="CSTUserInformation"/> is status.
        /// </summary>
        /// <value><c>true</c> if status; otherwise, <c>false</c>.</value>
        public bool Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        /// <summary>
        /// Gets or sets the login ID.
        /// </summary>
        /// <value>The login ID.</value>
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
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
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
        /// Gets or sets the institution ID.
        /// </summary>
        /// <value>The institution ID.</value>
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

        #endregion
    }
}
