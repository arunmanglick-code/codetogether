using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CTLS.ICLS.UX.CountySearch
{
    public class StringToBackGroundConvertor : IValueConverter
    {
        #region Properties

        public Brush HighlightBrush
        {
            get;
            set;
        }
        public Brush DefaultBrush
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Convert Depedns String Status To BackGround
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">object</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>object</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fileNumber = (string)value;
            if (String.IsNullOrEmpty(fileNumber))
                return HighlightBrush;

            return DefaultBrush;
        }

        /// <summary>
        /// Convert Back To Origina Value Type
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">object</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>object</returns>        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            return value;
        }
    }
}
