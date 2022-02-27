using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace Arun.Manglick.Silverlight._4.MVVM
{
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {            
            String absoluteUri = HtmlPage.Document.DocumentUri.AbsoluteUri;
            absoluteUri = absoluteUri.Substring(0, absoluteUri.IndexOf("#"));
            string str1 = absoluteUri + "#/About";

            HtmlPage.Window.Navigate(new Uri(str1, UriKind.RelativeOrAbsolute), "_self");
        }
    }
}