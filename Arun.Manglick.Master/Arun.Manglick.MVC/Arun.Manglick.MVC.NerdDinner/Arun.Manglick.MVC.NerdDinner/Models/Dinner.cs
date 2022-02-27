using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;

namespace Arun.Manglick.MVC.NerdDinner.Models
{
    public partial class Dinner
    {
        #region Public Methods

        public bool IsAMValid
        {
           get { return (GetRuleViolations().Count() == 0); }
           
        }

        public IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (String.IsNullOrEmpty(Title))
                yield return new RuleViolation("AM Title is required", "Title");

            if (String.IsNullOrEmpty(Description))
                yield return new RuleViolation("AM Description is required", "Description");

            if (String.IsNullOrEmpty(HostedBy))
                yield return new RuleViolation("AM HostedBy is required", "HostedBy");

            if (String.IsNullOrEmpty(Address))
                yield return new RuleViolation("AM Address is required", "Address");

            if (String.IsNullOrEmpty(Country))
                yield return new RuleViolation("AM Country is required", "Address");

            if (String.IsNullOrEmpty(ContactPhone))
                yield return new RuleViolation("AM Phone# is required", "ContactPhone");


            if (!Helpers.PhoneValidator.IsValidNumber(ContactPhone, Country))
                yield return new RuleViolation("AM Phone# does not match country", "ContactPhone");

            yield break;
        }

        #endregion

        #region Private Methods

        partial void OnValidate(ChangeAction action)
        {
            if (!IsAMValid)
            {
                throw new ApplicationException("Rule violations prevent saving");
            }            
        }


        #endregion
    }
}
