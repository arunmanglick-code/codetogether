using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using CT.SLABB.UX.Controls;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class OrderConfirmationView : UserControl
    {
        #region Variables
        private string status = string.Empty;
        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderConfirmationView()
        {
            InitializeComponent();
            this.Loaded += new System.Windows.RoutedEventHandler(OrderConfirmationView_Loaded);
        }
        #endregion

        #region Properties
        /// <summary>
        /// get and set Status
        /// </summary>
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Sets Image processing message's visibility
        /// </summary>
        /// <param name="flag">If true the visible else collapsed</param>
        ///<returns>void</returns>        
        private void VisbileImageProcessingMessage(bool flag)
        { 
            txbImageProcessingMsg.Visibility = flag ? Visibility.Visible : Visibility.Collapsed;            
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Loading Event of View
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void OrderConfirmationView_Loaded(object sender, RoutedEventArgs e)
        {  
            grdHeaderInfo.DataContext = this.countySearchVMC.ViewState.OrderConfirmationBindableModel;
            if ((this.countySearchVMC != null && this.countySearchVMC.ViewState != null && string.IsNullOrEmpty(this.countySearchVMC.ViewState.OrderConfirmationBindableModel.HeaderInfo.SearchKey)) || (this.countySearchVMC.ViewState.OrderConfirmationBindableModel.SubmitType == "NameSearchResults"))
                VisbileImageProcessingMessage(false);
            else
                VisbileImageProcessingMessage(true);
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

        /// <summary>
        /// View Order Details Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnViewOrderDetails_Click(object sender, MenuItemClickedEventArgs args)
        {
            UxViewFrame viewFrame = null;
            if (this.FindMyViewFrame(out viewFrame))
            {
                if (this.countySearchVMC.ViewState.OrderDetailsBindableModel == null)
                    this.countySearchVMC.ViewState.OrderDetailsBindableModel = new OrderDetailsBindableModel();

                this.countySearchVMC.ViewState.OrderDetailsBindableModel.TrackingNo = this.countySearchVMC.ViewState.OrderConfirmationBindableModel.HeaderInfo.TrackingNo;
                this.countySearchVMC.ViewState.OrderConfirmationBindableModel = null; // Clear ViewState
                viewFrame.Navigate(ViewConstants.ORDERDETAILSVIEW, null);
            }
        }

        /// <summary>
        /// New Search Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnNewSearchbtn_Click(object sender, MenuItemClickedEventArgs args)
        {
            UxViewFrame viewFrame = null;
            if (this.FindMyViewFrame(out viewFrame))
            {
                this.countySearchVMC.ViewState.ClearAll();
                viewFrame.Navigate(ViewConstants.DESKTOPVIEW, null);
            }
        }

        #endregion
    }
}
