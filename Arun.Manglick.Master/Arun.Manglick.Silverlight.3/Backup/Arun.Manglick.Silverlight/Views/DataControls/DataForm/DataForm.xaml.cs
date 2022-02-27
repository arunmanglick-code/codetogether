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
using System.Windows.Data;
using Arun.Manglick.Silverlight.ProductServiceReference;
using BO = Arun.Manglick.Silverlight.BO;

namespace Arun.Manglick.Silverlight.Views.DataControls
{
    public partial class DataForm : Page
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public DataForm()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SimpleBindingProducts_Loaded);
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void proxy_GetAllProductsCompleted(object sender, GetAllProductsCompletedEventArgs e)
        {
            ObservableCollection<Product> res = e.Result;
            productDataForm.ItemsSource = res;
        }

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SimpleBindingProducts_Loaded(object sender, RoutedEventArgs e)
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
        private void Grid_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            // After you set ValidatesOnExceptions to true, you also have the option of turning on
            // NotifyOnValidationError. If you do, the data-binding system fires a BindingValidationError
            // event when an error occurs

            //MessageBox.Show(e.Error.Exception.Message);
        }        

        #endregion
    }
}