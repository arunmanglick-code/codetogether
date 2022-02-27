using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CT.SLABB.DX;
using CT.SLABB.UX.Controls;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class OrderDetailsView : UserControl
    {
        #region Variables

        private const string COUNTY_DETAIL_SEARCH = "County Direct Detail Search";
        private const string COUNTY_SUMMARY_SEARCH = "County Direct Summary Search";
        private const string COPIES = "Copies (per Pg.)";
        private const char RETRY_ORDERDETAIL = 'I';       

        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDetailsView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(OrderDetailsView_Loaded);          
        }
        #endregion

        #region Methods
        /// <summary>
        /// Refresh Order Details Grid when click Retry or Mark As completed button
        /// </summary>
        /// <param name="rowItem">OrderDetail</param>
        private void OrderDetailRetry_Refresh(OrderDetail rowItem)
        {
            OrderDetailsBindableModel bindable = this.countySearchVMC.ViewState.OrderDetailsBindableModel;
            OrderDetail retryRowItem = bindable.OrderDetail.Where(x => x.TrackingItemNo == rowItem.TrackingItemNo && x.Status == rowItem.Status).FirstOrDefault() as OrderDetail;
            retryRowItem.Status = OrderDetailStatusConstants.INPROCESS;
            retryRowItem.ErrorMessage = string.Empty;
            gvwOrderDetails.ItemsSource = null;
            gvwOrderDetails.ItemsSource = this.countySearchVMC.ViewState.OrderDetailsBindableModel.OrderDetail;
        }
        #endregion

        #region Event Handlers

        #region Control Events

        /// <summary>
        /// Loading Event of OrderDetails Grid
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">DataGridRowEventArgs</param>
        /// <returns>void</returns>
        private void gvwOrderDetails_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            OrderDetailsBindableModel bindable = this.countySearchVMC.ViewState.OrderDetailsBindableModel;
            bool isNoRecordReportAvailable = false;
            bool isOriginalReportAvailable = false;
            bool isImageAvailable = false;

            if (e.Row.DataContext is OrderDetail)
            {
                OrderDetail OrderDetail = e.Row.DataContext as OrderDetail;
                MenuItem btnerror = (((gvwOrderDetails.Columns[6] as DataGridTemplateColumn).GetCellContent(e.Row) as StackPanel).Children[0] as MenuBar).Items[0] as MenuItem;
                MenuItem btnMarkCompleted = (((gvwOrderDetails.Columns[6] as DataGridTemplateColumn).GetCellContent(e.Row) as StackPanel).Children[1] as MenuBar).Items[0] as MenuItem;
                Image imgDocImage = ((gvwOrderDetails.Columns[0] as DataGridTemplateColumn).GetCellContent(e.Row) as HyperlinkButton).Content as Image;

                if (OrderDetail.Status == OrderDetailStatusConstants.COMPLETED)
                {
                    isNoRecordReportAvailable = bindable.IsNoRecordReportAvailable.Equals(SharedConstants.YES.ToCharArray()[0]) ? true : false;
                    isOriginalReportAvailable = bindable.IsOriginalReportAvailable.Equals(SharedConstants.YES.ToCharArray()[0]) ? true : false;
                    isImageAvailable = bindable.IsImageAvailable.Equals(SharedConstants.YES.ToCharArray()[0]) ? true : false;

                    if (OrderDetail.DocumentDetail.Equals(COUNTY_SUMMARY_SEARCH) && isNoRecordReportAvailable)
                        imgDocImage.Source = new BitmapImage(new Uri(SharedConstants.WORDIMAGE_PATH, UriKind.RelativeOrAbsolute));

                    if (OrderDetail.DocumentDetail.Equals(COUNTY_DETAIL_SEARCH) && (isNoRecordReportAvailable || isOriginalReportAvailable))
                        imgDocImage.Source = new BitmapImage(new Uri(SharedConstants.WORDIMAGE_PATH, UriKind.RelativeOrAbsolute));

                    if (OrderDetail.DocumentDetail.Equals(COPIES) && isImageAvailable)
                        imgDocImage.Source = new BitmapImage(new Uri(SharedConstants.PDFIMAGE_PATH, UriKind.RelativeOrAbsolute));

                    imgDocImage.Cursor = Cursors.Hand;
                }
                if (OrderDetail.Status.Equals(OrderDetailStatusConstants.ERROR) && btnerror != null && btnMarkCompleted != null)
                {
                    btnerror.Visibility = Visibility.Visible;
                    ToolTipService.SetToolTip(btnerror, OrderDetail.ErrorMessage);
                    btnMarkCompleted.Visibility = Visibility.Visible;
                }
                else
                {
                    btnerror.Visibility = Visibility.Collapsed;
                    btnMarkCompleted.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// View Image Link Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void lnkViewImage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is HyperlinkButton && (sender as HyperlinkButton).Content is Image && ((sender as HyperlinkButton).Content as Image).Source == null)
                return;

            DataGridCell cell = (sender as HyperlinkButton).Parent as DataGridCell;
            OrderDetail rowItem = cell.DataContext as OrderDetail;

            ViewDetailDocumentProxy viewDetailDocumentProxy = new ViewDetailDocumentProxy();
            ViewDetailDocumentRequest viewDetailDocumentRequest = new ViewDetailDocumentRequest();
                        
            viewDetailDocumentRequest.DocumentType = rowItem.DocumentDetail.Equals(COPIES) ? DetailDocumentType.Image : DetailDocumentType.OriginalReport;
            viewDetailDocumentRequest.TrackingNo = Convert.ToInt64(txbTrackingNo.Text);
            viewDetailDocumentProxy.Invoke(viewDetailDocumentRequest, ViewDetailDocumentServiceCompleted);
        }

        /// <summary>
        /// Retry Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnRetry_Click(object sender, MenuItemClickedEventArgs args)
        {
            DataGridCell cell = (((sender as MenuItem).Parent as MenuBar).Parent as StackPanel).Parent as DataGridCell;
            OrderDetail rowItem = cell.DataContext as OrderDetail;
            TextBlock txbTrackingNo = FindName("txbTrackingNo") as TextBlock;

            RetryProxy retryProxy = new RetryProxy();
            RetryRequest retryRequest = new RetryRequest();
            retryRequest.RetryType = RETRY_ORDERDETAIL;
            retryRequest.TrackingNo = Convert.ToInt64(txbTrackingNo.Text);
            retryRequest.TrackingItemNo = rowItem.TrackingItemNo;

            retryProxy.Invoke(retryRequest, null);// As provided By DX 
            OrderDetailRetry_Refresh(rowItem);
        }        

        /// <summary>
        /// Mark Copleted Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnMarkCompleted_Click(object sender, MenuItemClickedEventArgs args)
        {
            DataGridCell cell = (((sender as MenuItem).Parent as MenuBar).Parent as StackPanel).Parent as DataGridCell;
            OrderDetail rowItem = cell.DataContext as OrderDetail;
            TextBlock txbTrackingNo = FindName("txbTrackingNo") as TextBlock;

            MarkAsCompletedProxy markAsCompletedProxy = new MarkAsCompletedProxy();
            MarkAsCompleteRequest markAsCompleteRequest = new MarkAsCompleteRequest();
            markAsCompleteRequest.TrackingNo = Convert.ToInt64(txbTrackingNo.Text);

            markAsCompletedProxy.Invoke(markAsCompleteRequest, MarkAsCompletedServiceCompleted);
        }

        /// <summary>
        /// View Custom Report Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnViewCustomReport_Click(object sender, MenuItemClickedEventArgs args)
        {
            ViewDetailDocumentProxy viewDetailDocumentProxy = new ViewDetailDocumentProxy();
            ViewDetailDocumentRequest viewDetailDocumentRequest = new ViewDetailDocumentRequest();
            viewDetailDocumentRequest.DocumentType = DetailDocumentType.CustomReport;
            viewDetailDocumentRequest.TrackingNo = Convert.ToInt64(txbTrackingNo.Text);

            viewDetailDocumentProxy.Invoke(viewDetailDocumentRequest, ViewDetailDocumentServiceCompleted);
        }

        /// <summary>
        /// Upload Custom Report Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnUploadCustomReport_Click(object sender, MenuItemClickedEventArgs args)
        {
            OrderDetailsBindableModel orderDetailBindableModel = this.countySearchVMC.ViewState.OrderDetailsBindableModel;
            orderDetailBindableModel.BeginReportResult();
        }

        /// <summary>
        /// Return To Portfolio Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnReturntoPortfolio_Click(object sender, MenuItemClickedEventArgs args)
        {
            HtmlPage.Window.Navigate(new Uri(URIConstants.URL_PORTFOLIO, UriKind.Relative), URIConstants.CONST_self);
        }

        #endregion

        #region Other Handlers

        /// <summary>
        ///  Loading Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns>void</returns>
        private void OrderDetailsView_Loaded(object sender, RoutedEventArgs e)
        {
            UxPage page;
            string trackingNo = string.Empty;
            this.FindMyPage(out page);
            if (page != null && page.NavigationContext.QueryString.ContainsKey(QueryStringConstants.TRACKING_NO))
            {
                page.NavigationContext.QueryString.TryGetValue(QueryStringConstants.TRACKING_NO, out trackingNo);

                if (this.countySearchVMC.ViewState.OrderDetailsBindableModel == null)
                    this.countySearchVMC.ViewState.OrderDetailsBindableModel = new OrderDetailsBindableModel();

                this.countySearchVMC.ViewState.OrderDetailsBindableModel.TrackingNo = Convert.ToInt64(trackingNo);
            }
            
            // -------------------------------------------------------------------------------------------------------

            if (this.countySearchVMC.ViewState.OrderDetailsBindableModel != null)
                this.countySearchVMC.ViewState.OrderDetailsBindableModel.UserContext = ApplicationContext.ViewState.LoggedInUserContext;

            if (!this.countySearchVMC.ViewState.IsNew)
                grdHeaderInfo.DataContext = this.countySearchVMC.ViewState.OrderDetailsBindableModel;

            this.countySearchVMC.ViewState.OrderDetailsBindableModel.ODServiceCallCompleted += new RoutedEventHandler(OrderDetailsBindableModel_ODServiceCallCompleted);
            this.countySearchVMC.ViewState.OrderDetailsBindableModel.OrderReportResultServiceCallCompleted +=new RoutedEventHandler(OrderDetailsBindableModel_OrderReportResultServiceCallCompleted);
        }

        /// <summary>
        ///  Order Details Binadable Service Call Completed
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void OrderDetailsBindableModel_ODServiceCallCompleted(object sender, RoutedEventArgs e)
        {
            grdHeaderInfo.DataContext = null;
            OrderDetailsBindableModel bindable = this.countySearchVMC.ViewState.OrderDetailsBindableModel;

            btnUploadCustomReport.Visibility = bindable.IsCustomReportAvailable.Equals(SharedConstants.NO.ToCharArray()[0]) ? Visibility.Visible : Visibility.Collapsed;
            btnViewCustomReport.Visibility = bindable.IsCustomReportAvailable.Equals(SharedConstants.YES.ToCharArray()[0]) ? Visibility.Visible : Visibility.Collapsed;

            btnUploadCustomReportBottom.Visibility = btnUploadCustomReport.Visibility;
            btnViewCustomReportBottom.Visibility = btnViewCustomReport.Visibility;

            grdHeaderInfo.DataContext = this.countySearchVMC.ViewState.OrderDetailsBindableModel;            
            gvwOrderDetails.ItemsSource = this.countySearchVMC.ViewState.OrderDetailsBindableModel.OrderDetail;
        }

        /// <summary>
        ///  Retry Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args"> DxCompleteEventArgs:RetryResponse</param>
        /// <returns>void</returns>
        private void RetryServiceCompleted(DxProxy sender, DxCompleteEventArgs<RetryResponse> args)
        {
            this.countySearchVMC.ViewState.OrderDetailsBindableModel.BeginRefresh();
        }

        /// <summary>
        /// Mark As Completed Service call completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:MarkAsCompleteResponse</param>
        /// <returns>void</returns>
        private void MarkAsCompletedServiceCompleted(DxProxy sender, DxCompleteEventArgs<MarkAsCompleteResponse> args)
        {
            this.countySearchVMC.ViewState.OrderDetailsBindableModel.BeginRefresh();
        }

        /// <summary>
        /// View DetailDocument Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:ViewDetailDocumentResponse</param>
        /// <returns>void</returns>
        private void ViewDetailDocumentServiceCompleted(DxProxy sender, DxCompleteEventArgs<ViewDetailDocumentResponse> args)
        {
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "ViewDetailDocumentSVC"));
            }

            string DocumentUrl = string.Empty;
            if (args.Response.DetailDocument != null)
                DocumentUrl = args.Response.DetailDocument.DocumentUrl;

            string popupContentUrl = QueryStringConstants.POPUP_CONTENT_URL + "=" + DocumentUrl;
            string uri = URIConstants.URL_ASPX_POPUP + "?" + popupContentUrl;
            HtmlPage.Window.Navigate(new Uri(uri, UriKind.Relative), "_blank");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderDetailsBindableModel_OrderReportResultServiceCallCompleted(object sender, RoutedEventArgs e)
        {
            if (this.countySearchVMC.ViewState.OrderDetailsBindableModel.IsCustomReportAvailable.Equals(SharedConstants.YES.ToCharArray()[0]))
            {
                MessageBox.Show(MessageConstants.MSG_CUSTREPORT_EXISTS, "UPLOAD", MessageBoxButton.OK);
                btnUploadCustomReport.Visibility = Visibility.Collapsed;
                btnUploadCustomReportBottom.Visibility = Visibility.Collapsed;
                btnViewCustomReport.Visibility = Visibility.Visible;
                btnViewCustomReportBottom.Visibility = Visibility.Visible;
            }
            else
            {
                string trackingNo = countySearchVMC.ViewState.OrderDetailsBindableModel.TrackingNo.ToString();
                string uri = URIConstants.URL_ASPX_UPLOAD + "?" + QueryStringConstants.TRACKING_NO + "=" + trackingNo;
                HtmlPage.Window.Navigate(new Uri(uri, UriKind.Relative), "_blank");

                countySearchVMC.ViewState.OrderDetailsBindableModel.BeginRefresh(Convert.ToInt64(trackingNo));
            }
        
        }

        #endregion

        #endregion
    }
}
