using System;
using System.Windows.Data;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public class PortFolioStatusToGridHeaderMessage  : IValueConverter
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
            string message = value == null ? String.Empty : value.ToString();

            switch (value.ToString())
            {
                case OrderDetailStatusConstants.OPEN:
                case OrderDetailStatusConstants.OPEN_ERROR:
                    message = MessageConstants.OPEN_ORDERS;
                    break;
                case OrderDetailStatusConstants.INPROCESS:
                    message = MessageConstants.INPROCESS_ORDERS;
                    break;
                case OrderDetailStatusConstants.COMPLETED:
                    message = MessageConstants.COMLETED_ORDERS;
                    break;               
                default:
                    message = MessageConstants.FULLSEARCH_ORDERS;
                    break;

            }
            
            return message;
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

    public class PortFolioStatusToEmptyGridMessage : IValueConverter
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
            string message = value == null ? String.Empty : value.ToString();

            switch (value.ToString())
            {
                case OrderDetailStatusConstants.OPEN:                
                case OrderDetailStatusConstants.OPEN_ERROR:
                    message = MessageConstants.OPEN_SEARCH_GRID_MESSAGE;
                    break;
                case OrderDetailStatusConstants.ERROR:
                    message = MessageConstants.ERROR_SEARCH_GRID_MESSAGE;
                    break;
                case OrderDetailStatusConstants.INPROCESS:
                    message = MessageConstants.INPROCESS_SEARCH_GRID_MESSAGE;
                    break;
                case OrderDetailStatusConstants.COMPLETED:
                    message = MessageConstants.COMPLETED_SEARCH_GRID_MESSAGE;
                    break;
                case OrderDetailStatusConstants.ALL:
                    message = MessageConstants.FULL_SEARCH_GRID_MESSAGE;
                    break;
                case OrderDetailStatusConstants.SEARCHINPROGRESS:
                    message = MessageConstants.MSG_SEARCH_INPROGRESS;
                    break;
                default:
                    message = MessageConstants.DEFAULT_SEARCH_GRID_MESSAGE;
                    break;

            }

            return message;
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
