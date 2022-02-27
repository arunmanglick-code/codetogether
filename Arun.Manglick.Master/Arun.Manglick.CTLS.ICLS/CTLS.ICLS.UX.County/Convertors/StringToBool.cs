using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace CTLS.ICLS.UX.CountySearch
{
    public class StringToBool : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Convert String Status To Boolean
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">object</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>object</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString().Equals(parameter))
                return true;
            return false;

        }

        /// <summary>
        /// Unset The Value If Parameter is Null
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">object</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>object</returns>        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var ParameterString = parameter as string;

            if (ParameterString == null)
                return DependencyProperty.UnsetValue;
            if (System.Convert.ToBoolean(value) == true)
                return ParameterString;
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }

    /// <summary>
    /// Converter to formar Date
    /// </summary>       
    public class StringToDateTime : IValueConverter
    {
        #region IValueConverter Members
        /// <summary>
        /// String To Datetime Converter
        /// </summary>       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                DateTime date = System.Convert.ToDateTime(value);
                return date;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
