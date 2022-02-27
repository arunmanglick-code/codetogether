
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Security.Principal;

namespace PasswordValidator
{
    class CustomAuthorizationPolicy: IAuthorizationPolicy
    {
        #region IAuthorizationPolicy Members

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            object objIdentities = evaluationContext.Properties["Identities"];
            IList<IIdentity> identities = objIdentities as IList<IIdentity>;

            if (identities != null && identities.Count > 0)
            {
                IIdentity identity = identities[0] as GenericIdentity;
                if (identity != null)
                {
                    string[] roles = null;

                    if (identity.Name == "Admin")
                        roles = new string[] { "Administrators", "Users" };
                    else if (identity.Name == "User")
                        roles = new string[] { "Users" };
                    else
                        roles = new string[] { "Guests" };

                    evaluationContext.Properties["Principal"] = new GenericPrincipal(identity, roles);
                }
            }

            return true;
        }

        public System.IdentityModel.Claims.ClaimSet Issuer
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IAuthorizationComponent Members

        public string Id
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}
