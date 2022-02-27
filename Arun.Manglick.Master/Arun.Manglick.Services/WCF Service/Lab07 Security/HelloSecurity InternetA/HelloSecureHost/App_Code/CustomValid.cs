using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.IdentityModel.Selectors;

namespace HelloSecurity.Lab7.Internet
{
    /// <summary>
    /// Summary description for CustomValid
    /// </summary>
    public class CustomValid : UserNamePasswordValidator
    {
        public CustomValid()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public override void Validate(string userName, string password)
        {
            return;
        }
    }
}
