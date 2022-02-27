#region File History

/******************************File History***************************
 * File Name        : UserInfo.cs
 * Author           : Prerak Shah.
 * Created Date     : 22 Auguest 2007
 * Purpose          : This Class provide common properties which specify user information.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * * Date(dd-mm-yyyy) Developer Reason of Modification
 * ------------------------------------------------------------------- 
 *  30-10-2007 Prerak Shah - Added more properties.
    28-01-2008  SBT         Added "SubUserID" property
 *  10-31-2008  SNK         Adde LoginID property. Reason: Remember me functionality
 * ------------------------------------------------------------------- 
 
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Vocada.CSTools.Common
{
    public class UserInfo
    {
        #region private fields
        
        private string userName = "";
        int voc_user_id = 0;
        int user_id = 0;
        private int roleID;
        private string roleDescription = "";
        private string firstName;
        private string lastName;
        private int institutionID;
        private string institutionName;
        private bool allowSystemAdminTab;
        private string loginID;
        
        #endregion 

        #region Properties
        /// <summary>
        /// User ID
        /// </summary>
        public int UserID
        {
            get
            {
                return voc_user_id;
            }
            set
            {
                voc_user_id = value;
            }
        }
        /// <summary>
        /// User Name
        /// </summary>
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }
        /// <summary>
        /// Role Id
        /// </summary>
        public int RoleId
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
        /// Role Description
        /// </summary>
        public string RoleDescription
        {
            get
            {
                return roleDescription;
            }
            set
            {
                roleDescription = value;
            }
        }
        /// <summary>
        /// FirstName
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
        /// Last Name
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
        /// Institution ID
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
        /// Institution Name
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
        /// allowSystemAdmin
        /// </summary>
        public bool AllowSystemAdminTab
        {
            get
            {
                return allowSystemAdminTab;
            }
            set
            {
                allowSystemAdminTab = value;
            }
        }

        /// <summary>
        /// Subscriber User ID
        /// </summary>
        public int SubUserID
        {
            get
            {
                return user_id;
            }
            set
            {
                user_id = value;
            }
        }

        /// <summary>
        /// Gets or Sets the LoginID
        /// </summary>
        public string LoginID
        {
            get { return loginID; }
            set { loginID = value; }
        }
        #endregion
    }
}
