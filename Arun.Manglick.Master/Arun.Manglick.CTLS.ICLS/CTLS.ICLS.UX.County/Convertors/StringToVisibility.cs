using System;
using System.Windows;
using System.Windows.Data;

namespace CTLS.ICLS.UX.CountySearch
{
    /// <summary>
    /// 
    /// </summary>
    public class StringToVisibility : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Convert String To Visibility
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">object</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>object</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString().Equals(parameter))
                return Visibility.Visible;
            return Visibility.Collapsed;

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
            if ((Visibility)value == Visibility.Visible)
                return ParameterString;
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }

    /// <summary>
    /// If String is Empty OR null returns System.Windows.Visibility.Collapsed else System.Windows.Visibility.Visible
    /// </summary>
    public class EmptyStringToVisibility : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// If value is null OR value.ToString() is null OR empty returns System.Windows.Visibility.Collapsed else System.Windows.Visibility.Visible
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">object</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>object</returns> 
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Windows.Visibility visibility = System.Windows.Visibility.Visible;

            if (value == null)
                return System.Windows.Visibility.Collapsed;

            visibility = string.IsNullOrEmpty(value.ToString()) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;

            return visibility;
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">object</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>object</returns> 
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
