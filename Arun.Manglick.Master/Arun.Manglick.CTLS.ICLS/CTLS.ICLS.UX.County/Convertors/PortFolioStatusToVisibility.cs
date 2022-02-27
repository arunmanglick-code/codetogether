using System;
using System.Windows.Data;

namespace CTLS.ICLS.UX.CountySearch
{
    public class PortFolioStatusToLinkVisibility : IValueConverter
    {
        /// <summary>
        /// Convert Depedns On Portfolio Status To Visibility
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">object</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>object</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return System.Windows.Visibility.Collapsed;

            if (value.ToString().Equals("In Process") || value.ToString().Equals("Completed"))
                return System.Windows.Visibility.Visible;

            return System.Windows.Visibility.Collapsed;
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
            throw new NotFiniteNumberException();
        }
    }

    public class PortFolioStatusToTextBlockVisibility : IValueConverter
    {
        /// <summary>
        /// Convert To Visiblity Only When Status Is Not 'In Process' and 'Completed'
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">object</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>object</returns>       
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return System.Windows.Visibility.Collapsed;

            if (!value.ToString().Equals("In Process") && !value.ToString().Equals("Completed"))
                return System.Windows.Visibility.Visible;

            return System.Windows.Visibility.Collapsed;
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
            throw new NotFiniteNumberException();
        }
    }
}
