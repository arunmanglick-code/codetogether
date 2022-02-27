#region File History

/******************************File History***************************

 * File Name        : AuthorizationPolicy.cs
 * Author           : 
 * Created Date     : 
 * Purpose          : This class is used to define the Custom Authorization policy
 *                  : this class sets the CustomPrincipal object as a Generic                    
 *                  : principal object
 * *********************File Modification History*********************
 * Date(mm-dd-yyyy) Developer Reason of Modification
 * 12-23-2009  Raju G     Created
 * ------------------------------------------------------------------- 
 * 
*/

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.IdentityModel.Claims;
using System.Security.Principal;

#endregion

namespace Nuance.Veriphy.Security
{
    /// <summary>
    /// This class defines custom authorization policy to replace 
    /// generic principal object with custom principal object
    /// </summary>
    public class AuthorizationPolicy : IAuthorizationPolicy
    {
        #region Public Methods
        
        /// <summary>
        /// this method override IAuthorization member - Evaluate
        /// this method gets called after the authentication stage
        /// and override generic principal object with VeriphyCustomPrincipal object
        /// </summary>
        /// <param name="evaluationContext"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            // get the authenticated client identity
            IIdentity client = getClientIdentity(evaluationContext);

            // set the custom principal object
            evaluationContext.Properties["Principal"] = new CustomPrincipal(client);

            return true;
        }

        #endregion        

        #region Public Properties
        
        /// <summary>
        /// Override the IAuthorization member Isuser
        /// </summary>
        public System.IdentityModel.Claims.ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        /// <summary>
        /// Override the IAuthorizatiob member Id for 
        /// returning unique ID
        /// </summary>
        public string Id
        {
            get { return id.ToString(); }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the Client Identity
        /// </summary>
        /// <param name="evaluationContext"></param>
        /// <returns></returns>
        private IIdentity getClientIdentity(EvaluationContext evaluationContext)
        {
            object obj;
            if (!evaluationContext.Properties.TryGetValue("Identities", out obj))
                throw new Exception("No Identity found");

            IList<IIdentity> identities = obj as IList<IIdentity>;
            if (identities == null || identities.Count <= 0)
                throw new Exception("No Identity found");

            return identities[0];
        }

        #endregion

        #region Private Members

        Guid id = Guid.NewGuid();

        #endregion
    }
}
