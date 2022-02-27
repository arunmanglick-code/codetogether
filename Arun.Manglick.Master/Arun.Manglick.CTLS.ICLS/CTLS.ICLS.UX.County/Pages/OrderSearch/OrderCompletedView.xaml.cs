using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using CT.SLABB.UX.Controls;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class OrderCompletedView : UserControl
    {
        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderCompletedView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(OrderCompletedView_Loaded);
        }
        #endregion

        #region Event Handlers

        #region Other Handlers
        
        /// <summary>
        /// Binding Data To Grid Layout
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>        
        private void OrderCompletedView_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutRoot.DataContext = this.countySearchVMC.ViewState.OrderCompletedBindableModel;
        }

        #endregion

        #region Control Event Handlers
        
        /// <summary>
        /// View OrderDetails Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnViewOrderDetails_Click(object sender, MenuItemClickedEventArgs args)
        {
            string uri = string.Empty;
            string TrackingNo = this.countySearchVMC.ViewState.OrderCompletedBindableModel.HeaderInfo.TrackingNo.ToString();
            uri = URIConstants.URL_DESKTOP + "/" + ViewConstants.ORDERDETAILSVIEW + "/" + TrackingNo;
            HtmlPage.Window.Navigate(new Uri(uri, UriKind.Relative), URIConstants.CONST_self);
        }

        /// <summary>
        /// Return To Portfolio Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnRetrunToPortfolio_Click(object sender, MenuItemClickedEventArgs args)
        {
            HtmlPage.Window.Navigate(new Uri(URIConstants.URL_PORTFOLIO, UriKind.Relative), URIConstants.CONST_self);
        }

        #endregion

        #endregion
    }
}
