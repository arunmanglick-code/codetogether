﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
//using Arun.Manglick.Silverlight.ProductServiceReference;
using Arun.Manglick.SL3.BL;
using System.Collections.Generic;

namespace Arun.Manglick.Silverlight.Views.DataControls
{
    public partial class DataGridRowSorting : Page
    {
        #region Variables

        private ObservableCollection<Product> products;
        private SolidColorBrush highlightBrush = new SolidColorBrush(Colors.LightGray);
        private SolidColorBrush normalBrush = new SolidColorBrush(Colors.White);

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public DataGridRowSorting()
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
        //    //products = e.Result;
        //    //gridProducts.ItemsSource = products;

        //    PagedCollectionView view = new PagedCollectionView(e.Result);
        //    view.SortDescriptions.Add(new SortDescription("ModelName", ListSortDirection.Ascending));
        //    view.SortDescriptions.Add(new SortDescription("UnitCost", ListSortDirection.Ascending));
        //    gridProducts.ItemsSource = view;     
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

            List<Product> lst = new List<Product> {
                new Product{ ProductId=1, ModelNumber="AA", ModelName="AAA", UnitCost=12, Description= "AA DESC"},
                new Product{ ProductId=1, ModelNumber="BB", ModelName="BBB", UnitCost=12, Description= "BB DESC"},
                new Product{ ProductId=1, ModelNumber="CC", ModelName="CCC", UnitCost=12, Description= "CC DESC"}
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
        private void gridProducts_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Product product = (Product)e.Row.DataContext;
            if (product.UnitCost > 300)
                e.Row.Background = highlightBrush;
            else
                e.Row.Background = normalBrush;
        }

        #endregion
    }
}