#region File History

/******************************File History***************************

 * File Name        : CustomPrincipal.cs
 * Author           : 
 * Created Date     : 
 * Purpose          : This class is used to Provide custom pricipal object
 *                  : to replace the generic principal object. This class 
 *                  : contains additonal member to satisfy the business request
 * *********************File Modification History*********************
 * Date(mm-dd-yyyy) Developer Reason of Modification
 * 12-23-2009  Raju G     Created
 * ------------------------------------------------------------------- 
 * 
*/

#endregion

#region Namespaces

using System.Security.Principal;
using System.Threading;

#endregion

namespace Nuance.Veriphy.Security
{
    /// <summary>
    /// This class provide Custom Principal object to override the
    /// Generic Principal object.This contains the additional properties
    /// to contains the additional user information
    /// </summary>
    public class CustomPrincipal : IPrincipal
    {
        #region Constructor

        /// <summary>
        /// Set the Current Identity from Authorization Policy class
        /// </summary>
        /// <param name="identity"></param>
        public CustomPrincipal(IIdentity identity)
        {
            this.identity = identity;
        }

        #endregion

        #region Properties       

        /// <summary>
        /// Implemented IPrincipal - Identity property
        /// </summary>
        public IIdentity Identity
        {
            get { return identity; }
        }

        //Additonal Custom User Information
        public int VOCUserID { get; set; }
        public int GroupID { get; set; }
        public int InstitutionID { get; set; }
        public int SpecialistID { get; set; }
        public string HL7Version { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Implemented IPrincipal -  IsInRole method
        /// Get/ Set User Information based on Session Key supplied user name token
        /// And also return true/false if role is not matched
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            populateUserInformation();
            return roleName.Equals(role);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get and Set the User Details based on current Session
        /// Key in UserName token
        /// </summary>
        private void populateUserInformation()
        {
            string sessionKey = identity.Name;
            SessionDetailsManager objSessionDetailsManager = new SessionDetailsManager(sessionKey);
            SessionDetails objSessionDetails = objSessionDetailsManager.GetSessionDetails();
            roleName = objSessionDetails.RoleName;
            VOCUserID = objSessionDetails.VOCUserID;
            GroupID = objSessionDetails.GroupID;
            InstitutionID = objSessionDetails.InstitutionID;
            SpecialistID = objSessionDetails.SpecialistID ?? 0;
            HL7Version = objSessionDetails.HL7Version;            
        }

        #endregion

        #region Private Members

        IIdentity identity;
        string roleName;

        #endregion
    }
}