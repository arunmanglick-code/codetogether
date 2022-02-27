using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Arun.Manglick.Silverlight.Convertors
{
    public class PriceToBackGroundConvertor : IValueConverter
    {
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter,CultureInfo culture)
        {
            double price = (double)value;
            if (price >= Double.Parse(parameter.ToString()))
                return HighlightBrush;

            return DefaultBrush;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter,CultureInfo culture)
        {
            string price = value.ToString();

            double result;
            if (Double.TryParse(price, NumberStyles.Any, culture, out result))
            {
                return result;
            }
            return value;
        }
    }
}
