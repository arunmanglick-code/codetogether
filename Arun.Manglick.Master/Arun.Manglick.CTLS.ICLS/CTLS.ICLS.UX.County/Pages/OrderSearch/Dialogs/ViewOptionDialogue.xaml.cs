using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Browser;
using CT.SLABB.DX;
using CT.SLABB.UX.Controls;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Controls;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class ViewOptionDialogue : ICLSDialog
    {
        #region Private Variable
        private string __imageUrl;
        private bool __cancelProcessing;
        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewOptionDialogue()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Calls service to get image URL
        /// </summary>
        /// <returns>void</returns>
        private void GetImageUrl()
        {
            ViewImageProxy viewImageProxy = new ViewImageProxy();
            ViewImageRequest viewImageRequest = new ViewImageRequest();

            viewImageRequest.TrackingNo = this.countySearchVMC.ViewState.ViewOptionsBindableModel.TrackingNo;
            viewImageRequest.FileNumber = this.countySearchVMC.ViewState.ViewOptionsBindableModel.FileNumber;
            viewImageRequest.StateCode = this.countySearchVMC.ViewState.ViewOptionsBindableModel.StateCode;
            viewImageRequest.CountyName = this.countySearchVMC.ViewState.ViewOptionsBindableModel.CountyName;
            viewImageRequest.CountyCode = Convert.ToInt32(this.countySearchVMC.ViewState.ViewOptionsBindableModel.CountyCode);
            viewImageProxy.Invoke(viewImageRequest, viewImageServiceCompleted);
        }

        /// <summary>
        /// Raises the ViewOptionDialogue.Closing event.
        /// </summary>
        /// <param name="e">CancelEventArgs</param>
        /// <returns>void</returns>
        protected override void OnClosing(CancelEventArgs e)
        {
            __cancelProcessing = true;
            base.OnClosing(e);
        }

        #endregion

        #region Event Handlers

        #region Control Events

        /// <summary>
        /// click event of View button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnView_Click(object sender, MenuItemClickedEventArgs e)
        {
            __cancelProcessing = false; // Reset
            GetImageUrl();
            btnView.IsEnabled = false;
            busyIndicator.IsBusy = true;
        }

        /// <summary>
        /// click event of Close button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnClose_Click(object sender, MenuItemClickedEventArgs e)
        {
            __cancelProcessing = true;
            this.Close();
        }

        /// <summary>
        /// Loaded event of countySearchVMC CountySearchViewModelConnector
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void countySearchVMC_Loaded(object sender, RoutedEventArgs e)
        {
            gvwViewOptions.DataContext = null;
            gvwViewOptions.DataContext = this.countySearchVMC.ViewState.ViewOptionsBindableModel;
        }

        #endregion

        #region Other Handlers

        /// <summary>
        /// Asynchronus service ViewImage call completed event
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs<ViewImageResponse></param>
        /// <returns>void</returns>
        private void viewImageServiceCompleted(DxProxy sender, DxCompleteEventArgs<ViewImageResponse> args)
        {
            bool tempCancelProcessing;

            if (args.Error != null)
            {
                busyIndicator.IsBusy = false;
                tempCancelProcessing = __cancelProcessing;
                this.Close();
                if (!tempCancelProcessing) ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                busyIndicator.IsBusy = false;
                tempCancelProcessing = __cancelProcessing;
                this.Close();
                if (!tempCancelProcessing) throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "ViewImageSVC"));
            }

            __imageUrl = args.Response.ImageDetail.ImageUrl;

            busyIndicator.IsBusy = false;
            btnView.IsEnabled = true;
            btnClose.IsEnabled = true;

            if (!__cancelProcessing)
            {
                string popupContentUrl = QueryStringConstants.POPUP_CONTENT_URL + "=" + __imageUrl;
                string uri = URIConstants.URL_ASPX_POPUP + "?" + popupContentUrl;
                HtmlPage.Window.Navigate(new Uri(uri, UriKind.Relative), "_blank");
                this.Close();
            }
        }

        #endregion

        #endregion
    }
}
