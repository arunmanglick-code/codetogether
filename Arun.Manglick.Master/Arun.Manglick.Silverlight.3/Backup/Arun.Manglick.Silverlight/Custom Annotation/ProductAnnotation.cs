using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.DataAnnotations;

namespace Arun.Manglick.Silverlight.Custom_Annotation
{
    public class ProductAnnotation
    {
        public static ValidationResult ValidateProductDescriptionLength(string value,ValidationContext context)
        {
            if (value.Length > 80)
            {
                return new ValidationResult("Maximum Length cap is 80");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
