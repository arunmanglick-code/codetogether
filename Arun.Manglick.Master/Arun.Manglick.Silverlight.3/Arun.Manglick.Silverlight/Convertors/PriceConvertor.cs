using System;
using System.Globalization;
using System.Windows.Data;

namespace Arun.Manglick.Silverlight.Convertors
{
    public class PriceConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,CultureInfo culture)
        {
            double price = (double)value;
            string newPrice = price.ToString("C", culture);
            return newPrice;
        }

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
