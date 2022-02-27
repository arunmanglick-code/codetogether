using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Arun.Manglick.Silverlight.Model;
using Arun.Manglick.Silverlight.ViewModels;

namespace Arun.Manglick.Silverlight.Views
{
    public partial class SimpleBindingProducts : Page
    {
        ProductViewModel viewModel = null;

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public SimpleBindingProducts()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SimpleBindingProducts_Loaded);
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
            loadingBar.Visibility = Visibility.Visible;
            viewModel = Resources["TheProductViewModel"] as ProductViewModel;
            viewModel.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewModel_LoadComplete(object sender, System.EventArgs e)
        {
            if (loadingBar != null) loadingBar.Visibility = Visibility.Collapsed;
        }

        #endregion
    }
}