
using System;
using System.IdentityModel.Selectors;
using System.Security;

namespace PasswordValidator
{
    public class CustomPasswordValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            // TODO: look up the user in a custom security database
            if (password != "arun123#")
                throw new SecurityException("Access denied.");

            return;
        }
    }
}
