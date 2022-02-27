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
//using Arun.Manglick.Silverlight.ProductServiceReference;
using BO = Arun.Manglick.Silverlight.BO;
using Arun.Manglick.Silverlight.BO;

namespace Arun.Manglick.Silverlight.Views.DataBinding
{
    public partial class ListBoxBindingValidations : Page
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ListBoxBindingValidations()
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
        //private void proxy_GetSingleProductsCompleted(object sender, GetAllProductsCompletedEventArgs e)
        //{
        //    ObservableCollection<Product> res = e.Result;
        //    gridProductDetails.DataContext = res[0] as Product;
        //    //lstProducts.ItemsSource = res;
        //}

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SimpleBindingProducts_Loaded(object sender, RoutedEventArgs e)
        {
            //ProductSvcClient proxy = new ProductSvcClient();
            //proxy.GetAllProductsCompleted += new EventHandler<GetAllProductsCompletedEventArgs>(proxy_GetSingleProductsCompleted);
            //proxy.GetAllProductsAsync();
            List<Product> lst = new List<Product> {
                new Product{ ProductId=1, ModelNumber="AA", ModelName="AAA", UnitCost=12, Description= "AA DESC"},
                new Product{ ProductId=1, ModelNumber="BB", ModelName="BBB", UnitCost=12, Description= "BB DESC"},
                new Product{ ProductId=1, ModelNumber="CC", ModelName="CCC", UnitCost=12, Description= "CC DESC"}
            };

            gridProductDetails.DataContext = lst[0] as Product;
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
            product.ModelName = "CNN";
            product.ModelNumber = "Test01";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {   
            BindingExpression expression1 = txtModelName.GetBindingExpression(TextBox.TextProperty);
            BindingExpression expression2 = txtDesc.GetBindingExpression(TextBox.TextProperty);
            BindingExpression expression3 = txtUnitCost.GetBindingExpression(TextBox.TextProperty);
            BindingExpression expression4 = lstProducts.GetBindingExpression(ListBox.ItemsSourceProperty);

            expression1.UpdateSource();
            expression2.UpdateSource();
            expression3.UpdateSource();
            expression4.UpdateSource();
        }

        #endregion

        private void lstProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product product = (Product)gridProductDetails.DataContext;
            product.MySelected = true;
        }
    }
}