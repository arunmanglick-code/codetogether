using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Arun.Manglick.Silverlight.ProductServiceReference;

namespace Arun.Manglick.Silverlight.Views.DataBinding
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
            ProductSvcClient proxy = new ProductSvcClient();
            proxy.GetAllProductsCompleted += new EventHandler<GetAllProductsCompletedEventArgs>(proxy_GetAllProductsCompleted);
            proxy.GetAllProductsAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void proxy_GetAllProductsCompleted(object sender, GetAllProductsCompletedEventArgs e)
        {
            ObservableCollection<Product> res = e.Result;
        }
    }
}