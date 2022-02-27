using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Media;
using CT.SLABB.UX.Controls;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Controls;
using CTLS.ICLS.UX.Shared.Utils;
using CTLS.Shared.UX.Controls;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class FormPreviewDetailsResultDialog : ICLSDialog
    {
        #region Variables

        FrameworkElement __activeBlock = null;
        GridView __activeGridView = null;
        string fileNumber = string.Empty;
        string documentType = string.Empty;
        private bool __isNoRecordReport = false;

        #endregion

        #region Properties
        /// <summary>
        /// Get or sets the current active block
        /// </summary>
        /// <returns>FrameworkElement</returns>
        public FrameworkElement ActiveBlock
        {
            get { return __activeBlock; }
            set { __activeBlock = value; }
        }

        /// <summary>
        /// Gets or Sets bool value indicating whether to show records in grid or not
        /// </summary>        
        public bool IsNoRecordReport
        {
            get { return __isNoRecordReport; }
            set { __isNoRecordReport = value; }
        }

        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public FormPreviewDetailsResultDialog()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(FormPreviewDetailsResultView_Loaded);
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Loading event of Dialog ICLSDialog
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns> 
        private void FormPreviewDetailsResultView_Loaded(object sender, RoutedEventArgs e)
        {
            SetVisibility();
        }

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
        /// Click event of Close Button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnClose_Click(object sender, MenuItemClickedEventArgs args)
        {
            this.Close();
        }

        /// <summary>
        /// Row loading event of active GridView
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">DataGridRowEventArgs</param>
        /// <returns>void</returns>
        private void ActiveGridView_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            fileNumber = (e.Row.DataContext as DetailResultListItem).FileNumber;
            documentType = (e.Row.DataContext as DetailResultListItem).DocumentType;
            if (string.IsNullOrEmpty(fileNumber) && (!string.IsNullOrEmpty(documentType)))
            {
                e.Row.Background = new SolidColorBrush(Color.FromArgb(255, 193, 193, 193));
            }
        }

        /// <summary>
        /// Click event of Export Button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnExport_Click(object sender, MenuItemClickedEventArgs args)
        {
            string trackingNo = string.Empty;
            string trackingItemNo = string.Empty;
            string taskIds = string.Empty;

            if (!IsNoRecordReport)
            {
                trackingNo = countySearchVMC.ViewState.DetailResultPreviewBindableModel.HeaderInfo.TrackingNo.ToString();
                trackingItemNo = countySearchVMC.ViewState.DetailResultPreviewBindableModel.TrackingItemNo.ToString();
                taskIds = countySearchVMC.ViewState.DetailResultPreviewBindableModel.TaskIds;
            }
            else
                trackingNo = countySearchVMC.ViewState.DetailResultBindableModel.HeaderInfo.TrackingNo.ToString();

            string uri = URIConstants.URL_ASPX_EXPORT_DETAILS + "?" + QueryStringConstants.TRACKING_NO + "=" + trackingNo + "&" + QueryStringConstants.TRACKING_ITEM_NO + "=" + trackingItemNo + "&" + QueryStringConstants.TASK_ID + "=" + taskIds + "&" + QueryStringConstants.ACTION + "=" + SharedConstants.EXPORT + "&" + QueryStringConstants.NORECORDSREPORT + "=" + this.IsNoRecordReport + "&CreateFile=Y";
            HtmlPage.Window.Navigate(new Uri(uri, UriKind.Relative), "_blank");
        }

        /// <summary>
        /// Click event of Print Button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnPrint_Click(object sender, MenuItemClickedEventArgs args)
        {
            string trackingNo = string.Empty;
            string trackingItemNo = string.Empty;
            string taskIds = string.Empty;
            if (!IsNoRecordReport)
            {
                trackingNo = countySearchVMC.ViewState.DetailResultPreviewBindableModel.HeaderInfo.TrackingNo.ToString();
                trackingItemNo = countySearchVMC.ViewState.DetailResultPreviewBindableModel.TrackingItemNo.ToString();
                taskIds = countySearchVMC.ViewState.DetailResultPreviewBindableModel.TaskIds;
            }
            else
                trackingNo = countySearchVMC.ViewState.DetailResultBindableModel.HeaderInfo.TrackingNo.ToString();

            string uri = URIConstants.URL_ASPX_EXPORT_DETAILS + "?" + QueryStringConstants.TRACKING_NO + "=" + trackingNo + "&" + QueryStringConstants.TRACKING_ITEM_NO + "=" + trackingItemNo + "&" + QueryStringConstants.TASK_ID + "=" + taskIds + "&" + QueryStringConstants.ACTION + "=" + SharedConstants.PRINT + "&" + QueryStringConstants.NORECORDSREPORT + "=" + this.IsNoRecordReport + "&CreateFile=N";
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
            if (IsNoRecordReport)
            {
                grdHeaderInfo.DataContext = this.countySearchVMC.ViewState.DetailResultBindableModel;
                bdrLienType.Visibility = Visibility.Collapsed;
                txbNoRecords.Visibility = Visibility.Visible;
            }
            else
            {
                grdHeaderInfo.DataContext = this.countySearchVMC.ViewState.DetailResultPreviewBindableModel;

                string stateName = this.countySearchVMC.ViewState.DetailResultPreviewBindableModel.HeaderInfo.StateCode;
                string countyName = this.countySearchVMC.ViewState.DetailResultPreviewBindableModel.HeaderInfo.CountyName;

                switch (stateName + ":" + countyName)
                {
                    case "AZ:Maricopa":
                        ActiveBlock = AZMaricopaPreviewDetailRresultsBlock;
                        __activeGridView = this.AZMaricopaPreviewDetailRresultsBlock.grvDetailResults;
                        CASanDiegoPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        CASantaClaraPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        FLDadePreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        FLPalmBeachPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        break;
                    case "CA:San Diego":
                        ActiveBlock = CASanDiegoPreviewDetailRresultsBlock;
                        __activeGridView = this.CASanDiegoPreviewDetailRresultsBlock.grvDetailResults;
                        AZMaricopaPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        CASantaClaraPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        FLDadePreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        FLPalmBeachPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        break;
                    case "CA:Santa Clara":
                        ActiveBlock = CASantaClaraPreviewDetailRresultsBlock;
                        __activeGridView = this.CASantaClaraPreviewDetailRresultsBlock.grvDetailResults;
                        AZMaricopaPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        CASanDiegoPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        FLDadePreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        FLPalmBeachPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        break;
                    case "FL:Dade":
                        ActiveBlock = FLDadePreviewDetailRresultsBlock;
                        __activeGridView = this.FLDadePreviewDetailRresultsBlock.grvDetailResults;
                        AZMaricopaPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        CASanDiegoPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        CASantaClaraPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        FLPalmBeachPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        break;
                    case "FL:Palm Beach":
                        ActiveBlock = FLPalmBeachPreviewDetailRresultsBlock;
                        __activeGridView = this.FLPalmBeachPreviewDetailRresultsBlock.grvDetailResults;
                        AZMaricopaPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        CASanDiegoPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        CASantaClaraPreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        FLDadePreviewDetailRresultsBlock.Visibility = Visibility.Collapsed;
                        break;
                }

                ActiveBlock.DataContext = this.countySearchVMC.ViewState.DetailResultPreviewBindableModel;
                ActiveBlock.Visibility = Visibility.Visible;
                __activeGridView.LoadingRow += new EventHandler<DataGridRowEventArgs>(ActiveGridView_LoadingRow);
                lstLienTypes.ItemsSource = this.countySearchVMC.ViewState.DetailResultPreviewBindableModel.DetailsPreview;
            }
        }

        #endregion
    }
}
