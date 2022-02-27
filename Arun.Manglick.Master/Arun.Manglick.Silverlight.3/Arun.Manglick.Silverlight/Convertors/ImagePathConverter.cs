using System;
using System.Windows.Browser;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Globalization;

namespace Arun.Manglick.Silverlight.Convertors
{
    public class ImagePathConverter : IValueConverter
    {
        private string rootUri;
        public string RootUri
        {
            get { return rootUri; }
            set { rootUri = value; }
        }

        public ImagePathConverter()
        {        
            string uri = HtmlPage.Document.DocumentUri.ToString();
            rootUri = uri.Remove(uri.LastIndexOf('/'), uri.Length - uri.LastIndexOf('/'));
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
            string imagePath = "../../Images/" + ((string)value).Trim() + ".jpg";
            return imagePath;
            
            //string imagePath = RootUri + "/" + ((string)value).Trim() + ".jpg";
            // Hack for GIFs.(The database expect GIF files, but Silverlight only supports PNG and JPEG.)
            //imagePath = imagePath.ToLower().Replace(".gif", ".png");
            //return new BitmapImage(new Uri(imagePath));
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
            throw new NotSupportedException();
        }
    }

}
