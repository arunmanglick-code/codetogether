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

namespace Arun.Manglick.Silverlight.Views.DataBinding
{
    public partial class Products : Page
    {
        #region Variables

        private ObservableCollection<Product> products;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public Products()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ProductSvcClient proxy = new ProductSvcClient();
            proxy.GetAllProductsCompleted += new EventHandler<GetAllProductsCompletedEventArgs>(proxy_GetSingleProductsCompleted);
            proxy.GetAllProductsAsync();

            //List<BO.Product> lst = new List<BO.Product> {
            //    new BO.Product{ ProductId=1, ModelNumber="AA", ModelName="AAA", UnitCost=12, Description= "AA DESC"},
            //    new BO.Product{ ProductId=1, ModelNumber="BB", ModelName="BBB", UnitCost=12, Description= "BB DESC"},
            //    new BO.Product{ ProductId=1, ModelNumber="CC", ModelName="CCC", UnitCost=12, Description= "CC DESC"}
            //};            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void proxy_GetSingleProductsCompleted(object sender, GetAllProductsCompletedEventArgs e)
        {
            ObservableCollection<Product> res = e.Result;
            gridProductDetails.DataContext = res[0] as Product;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void proxy_GetAllProductsCompleted(object sender, GetAllProductsCompletedEventArgs e)
        {
            products = e.Result;
            lstProducts.ItemsSource = products;
            lstProducts1.ItemsSource = products;
            lstProducts2.ItemsSource = products;
            lstProducts3.ItemsSource = products;
        }

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Product product = (Product)gridProductDetails.DataContext;
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

            MessageBox.Show(e.Error.Exception.Message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductId_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindingExpression expression = txtProductId.GetBindingExpression(TextBox.TextProperty);
            expression.UpdateSource();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            Product product = (Product)gridProductDetails.DataContext;
            product.UnitCost *= 5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetProducts_Click(object sender, RoutedEventArgs e)
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
        private void lstProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridProductDetails.DataContext = lstProducts.SelectedItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstProducts1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridProductDetails.DataContext = lstProducts1.SelectedItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstProducts2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridProductDetails.DataContext = lstProducts2.SelectedItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstProducts3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridProductDetails.DataContext = lstProducts3.SelectedItem;
        }

        #endregion
    }
}