using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Arun.Manglick.MVC.NerdDinner.Models.EDM
{
    public partial class Dinners : IDataErrorInfo
    {
        private Dictionary<string, string> _errors = new Dictionary<string, string>();
        
        partial void OnContactPhoneChanging(string value)
        {
            if (value.Trim().Length == 0)
            {
                _errors.Add("ContactPhone", "EDM Phone# is required");
            }
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public string this[string columnName]
        {
            get 
            {
                if (_errors.ContainsKey(columnName))
                {
                    return _errors[columnName];
                }

                return string.Empty;
            }
        }

        #endregion
    }
}
