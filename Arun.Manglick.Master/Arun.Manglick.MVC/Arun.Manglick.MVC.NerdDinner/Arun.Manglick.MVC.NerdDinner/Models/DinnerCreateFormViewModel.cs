using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arun.Manglick.MVC.NerdDinner.Models
{
    public class DinnerCreateFormViewModel
    {
        public Dinner Dinner { get; private set; }
        public SelectList Countries { get; private set; }

        // Constructor
        public DinnerCreateFormViewModel(Dinner dinner)
        {
            this.Dinner = dinner;
            this.Countries = new SelectList(Helpers.PhoneValidator.Countries, dinner.Country);
        }

        public IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (String.IsNullOrEmpty(Dinner.Title))
                yield return new RuleViolation("AM Title is required", "Title");

            if (String.IsNullOrEmpty(Dinner.Description))
                yield return new RuleViolation("AM Description is required", "Description");

            if (String.IsNullOrEmpty(Dinner.HostedBy))
                yield return new RuleViolation("AM HostedBy is required", "HostedBy");

            if (String.IsNullOrEmpty(Dinner.HostedBy))
                yield return new RuleViolation("AM Address is required", "Address");

            if (String.IsNullOrEmpty(Dinner.Country))
                yield return new RuleViolation("AM Country is required", "Address");

            if (String.IsNullOrEmpty(Dinner.ContactPhone))
                yield return new RuleViolation("AM Phone# is required", "ContactPhone");


            if (!Helpers.PhoneValidator.IsValidNumber(Dinner.ContactPhone, Dinner.Country))
                yield return new RuleViolation("AM Phone# does not match country", "ContactPhone");

            yield break;
        }
    }
}
