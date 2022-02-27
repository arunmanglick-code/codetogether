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

namespace Arun.Manglick.Silverlight.Views.DataControls
{
    public partial class DataFormTemplate : Page
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public DataFormTemplate()
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
        //private void proxy_GetAllProductsCompleted(object sender, GetAllProductsCompletedEventArgs e)
        //{
        //    ObservableCollection<Product> res = e.Result;
        //    productDataForm.ItemsSource = res;
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
            //proxy.GetAllProductsCompleted += new EventHandler<GetAllProductsCompletedEventArgs>(proxy_GetAllProductsCompleted);
            //proxy.GetAllProductsAsync();

            List<BO.Product> lst = new List<BO.Product> {
                new BO.Product{ ProductId=1, ModelNumber="AA", ModelName="AAA", UnitCost=12, Description= "AA DESC"},
                new BO.Product{ ProductId=1, ModelNumber="BB", ModelName="BBB", UnitCost=12, Description= "BB DESC"},
                new BO.Product{ ProductId=1, ModelNumber="CC", ModelName="CCC", UnitCost=12, Description= "CC DESC"}
            };
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productDataForm_AddingNewItem(object sender, DataFormAddingNewItemEventArgs e)
        {
            productDataForm.CurrentItem = null;
            productDataForm.BeginEdit();

            // Edit End will be called when you hit save/cancel
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productDataForm_DeletingItem(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Put the code to perform 'Actual Delete'            
            //MessageBoxResult res= MessageBox.Show("Do you want to delete", "Delete", MessageBoxButton.OKCancel);
            //if (res == MessageBoxResult.Yes)
            //{
            //    e.Cancel = false;
            //}
            //else
            //{
            //    e.Cancel = true;
            //}

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productDataForm_EditEnded(object sender, DataFormEditEndedEventArgs e)
        {
            if (e.EditAction == DataFormEditAction.Commit)
            {                
                productDataForm.CommitEdit();

                // To Perform Actual Save in Database - Do as below.
                Product newProduct = productDataForm.CurrentItem as Product;
                // Put the code to perform 'Actual Save in Database'
                // Reload DataForm - proxy_GetAllProductsCompleted
            }
            else
            {
                productDataForm.CancelEdit();
            }
        }

        private void lstColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBox txtTest = productDataForm.FindNameInContent("txtDesc") as TextBox;
            if (txtTest != null)
                txtTest.Visibility = Visibility.Visible;
        }
        
        #endregion
    }
}