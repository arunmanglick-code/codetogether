using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arun.Manglick.MVC.NerdDinner.Helpers
{
    public static class ModelStateHelpers
    {
        //Extension Method
        public static void AddRuleViolations(this ModelStateDictionary modelState, IEnumerable<Models.RuleViolation> errors)
        {
            foreach (Models.RuleViolation rule in errors)
            {
                modelState.AddModelError(rule.PropertyName, rule.ErrorMessage);
            }
        }
    }
}
