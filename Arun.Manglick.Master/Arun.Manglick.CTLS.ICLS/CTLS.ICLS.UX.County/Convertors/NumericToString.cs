using System;
using System.Windows.Data;

namespace CTLS.ICLS.UX.CountySearch
{
    public class NumericToString : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Convert Numeric Value To String
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">object</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>object</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = value.ToString();
            return value.ToString().Equals("0") ? string.Empty : result;
        }

        /// <summary>
        /// Convert Back To Original Nuemric Value
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">object</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>object</returns>        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string price = value.ToString();
            int result;

            if (Int32.TryParse(price, out result))
            {
                return result;
            }
            return value;
        }


        #endregion
    }
}
