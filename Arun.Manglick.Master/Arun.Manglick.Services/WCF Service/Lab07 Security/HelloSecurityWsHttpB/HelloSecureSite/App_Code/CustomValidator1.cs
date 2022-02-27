using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.IdentityModel.Selectors;

namespace HelloSecurity.Lab7.WsHttp.InternetB
{
    /// <summary>
    /// Summary description for CustomValid
    /// </summary>
    public class CustomValidator : UserNamePasswordValidator
    {
        public CustomValidator()
        {            
        }

        public override void Validate(string userName, string password)
        {
            return;
        }
    }
}
