using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Arun.Manglick.MVC.NerdDinner.Helpers
{
    public class PhoneValidator
    {
        private static IDictionary<string, Regex> countryRegex = new Dictionary<string, Regex>(){
               { "India", new Regex("^[0-9]*$")},
               { "USA", new Regex("^[2-9]\\d{2}-\\d{3}-\\d{4}$")},
               { "UK", new Regex("(^1300\\d{6}$)|(^1800|1900|1902\\d{6}$)|(^0[2|3|7|8]{1}[0-9]{8}$)|(^13\\d{4}$)|(^04\\d{2,3}\\d{6}$)")},
               { "Netherlands", new Regex("(^\\+[0-9]{2}|^\\+[0-9]{2}\\(0\\)|^\\(\\+[0-9]{2}\\)\\(0\\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\\-\\s]{10}$)")},
        };

        public static bool IsValidNumber(string phoneNumber, string country)
        {
            if (!String.IsNullOrEmpty(country) && countryRegex.ContainsKey(country))
            {
                return countryRegex[country].IsMatch(phoneNumber);
            }

            return false;

        }

        public static IEnumerable<string> Countries
        {
            get
            {
                return countryRegex.Keys;
            }
        }

       

    }
}
