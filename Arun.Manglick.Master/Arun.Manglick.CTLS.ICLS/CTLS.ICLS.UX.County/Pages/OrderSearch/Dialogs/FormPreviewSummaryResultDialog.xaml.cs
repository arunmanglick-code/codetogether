using System;
using System.Windows;
using System.Windows.Browser;
using CT.SLABB.UX.Controls;
using CTLS.ICLS.UX.Controls;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class FormPreviewSummaryResultDialog : ICLSDialog
    {
        #region Variables
        FrameworkElement activeBlock = null;
        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public FormPreviewSummaryResultDialog()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(FormPreviewSummaryResultView_Loaded);
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Loading event of CountySearchViewModelConnector
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void countySearchVMC_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshView();
        }

        /// <summary>
        /// Sets the Header Navigations visibility
        /// </summary>
        /// <returns>void</returns>
        private void FormPreviewSummaryResultView_Loaded(object sender, RoutedEventArgs e)
        {
            SetVisibility();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void btnClose_Click(object sender, MenuItemClickedEventArgs args)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void btnExport_Click(object sender, MenuItemClickedEventArgs args)
        {
            string trackingNo = QueryStringConstants.TRACKING_NO + "=" + countySearchVMC.ViewState.SummarySearchResultBindableModel.HeaderInfo.TrackingNo.ToString();
            string searchKeys = QueryStringConstants.SEARCH_KEYS + "=" + HttpUtility.UrlEncode(countySearchVMC.ViewState.SummarySearchResultBindableModel.SearchKeys);
            string totalRecords = QueryStringConstants.TOTAL_RECORDS + "=" + countySearchVMC.ViewState.SummarySearchResultBindableModel.SummaryResultsList.Count.ToString();

            string uri = URIConstants.URL_ASPX_EXPORT_SUMMARYRESULTS + "?" + trackingNo + "&" + searchKeys + "&" + totalRecords + "&" + QueryStringConstants.ACTION + "=" + SharedConstants.EXPORT + "&CreateFile=Y";
            HtmlPage.Window.Navigate(new Uri(uri, UriKind.Relative), "_blank");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void btnPrint_Click(object sender, MenuItemClickedEventArgs args)
        {
            string trackingNo = QueryStringConstants.TRACKING_NO + "=" + countySearchVMC.ViewState.SummarySearchResultBindableModel.HeaderInfo.TrackingNo.ToString();
            string searchKeys = QueryStringConstants.SEARCH_KEYS + "=" + HttpUtility.UrlEncode(countySearchVMC.ViewState.SummarySearchResultBindableModel.SearchKeys);
            string totalRecords = QueryStringConstants.TOTAL_RECORDS + "=" + countySearchVMC.ViewState.SummarySearchResultBindableModel.SummaryResultsList.Count.ToString();

            string uri = URIConstants.URL_ASPX_EXPORT_SUMMARYRESULTS + "?" + trackingNo + "&" + searchKeys + "&" + totalRecords + "&" + QueryStringConstants.ACTION + "=" + SharedConstants.PRINT + "&CreateFile=N";
            HtmlPage.Window.Navigate(new Uri(uri, UriKind.Relative), "_blank");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the Header Navigations visibility
        /// </summary>
        /// <returns>void</returns>
        private void SetVisibility()
        {
            UxPage page;
            this.FindMyPage(out page);
            if (page != null) (page as DesktopPage).HeaderNavVisibilty = Visibility.Collapsed;
        }

        /// <summary>
        /// Sets the visibility of blocks according to current StateCode and CountyName
        /// </summary>
        /// <returns>void</returns>
        private void RefreshView()
        {
            grdHeaderInfo.DataContext = this.countySearchVMC.ViewState.SummarySearchResultBindableModel;
            txbTotalRecords.Text = this.countySearchVMC.ViewState.SummarySearchResultBindableModel.SummaryResultsList.Count.ToString();

            string stateName = this.countySearchVMC.ViewState.SummarySearchResultBindableModel.HeaderInfo.StateCode;
            string countyName = this.countySearchVMC.ViewState.SummarySearchResultBindableModel.HeaderInfo.CountyName;

            switch (stateName + ":" + countyName)
            {
                case "AZ:Maricopa":
                    activeBlock = AZMaricopaPreviewSummarySearchResultBlock;
                    CASanDiegoPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    CASantaClaraPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLDadePreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLPalmBeachPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    break;
                case "CA:San Diego":
                    activeBlock = CASanDiegoPreviewSummarySearchResultBlock;
                    AZMaricopaPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    CASantaClaraPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLDadePreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLPalmBeachPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    break;
                case "CA:Santa Clara":
                    activeBlock = CASantaClaraPreviewSummarySearchResultBlock;
                    AZMaricopaPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    CASanDiegoPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLDadePreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLPalmBeachPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    break;
                case "FL:Dade":
                    activeBlock = FLDadePreviewSummarySearchResultBlock;
                    AZMaricopaPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    CASanDiegoPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    CASantaClaraPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLPalmBeachPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    break;
                case "FL:Palm Beach":
                    activeBlock = FLPalmBeachPreviewSummarySearchResultBlock;
                    AZMaricopaPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    CASanDiegoPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    CASantaClaraPreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLDadePreviewSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    break;
            }

            activeBlock.DataContext = this.countySearchVMC.ViewState.SummarySearchResultBindableModel;
            activeBlock.Visibility = Visibility.Visible;
        }

        #endregion       
    }
}
