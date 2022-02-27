using System;
using System.Windows;
using System.Windows.Browser;
using CT.SLABB.UX.Controls;
using CTLS.ICLS.UX.Controls;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class FormPreviewNameResultDialog : ICLSDialog
    {
        #region Variables
        private bool __isNoRecordReport = false;
        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public FormPreviewNameResultDialog()
        {
            InitializeComponent();
            this.Loaded += FormPreviewNameResultDialog_Loaded;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or Sets bool value indicating whether to show records in grid or not
        /// </summary>        
        public bool IsNoRecordReport
        {
            get { return __isNoRecordReport; }
            set { __isNoRecordReport = value; }
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
            if (page != null) (page as DesktopPage).HeaderNavVisibilty = Visibility.Visible;
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Loading event of Dialog ICLSDialog
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns> 
        private void FormPreviewNameResultDialog_Loaded(object sender, RoutedEventArgs e)
        {
            grFormPreview.DataContext = countySearchVMC.ViewState.NameResultBindableModel;

            txbNoRecords.Visibility = Visibility.Visible;
            scrlNameResult.Visibility = Visibility.Collapsed;
            grvNameSearchResults.ItemsSource = null;

            if (!IsNoRecordReport)
            {
                grvNameSearchResults.ItemsSource = countySearchVMC.ViewState.NameResultBindableModel.NameList;
                txbNoRecords.Visibility = Visibility.Collapsed;
                scrlNameResult.Visibility = Visibility.Visible;
            }

            SetVisibility();
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
        /// Click event of Export Button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnExport_Click(object sender, MenuItemClickedEventArgs args)
        {
            string trackingNo = countySearchVMC.ViewState.NameResultBindableModel.HeaderInfo.TrackingNo.ToString();
            string uri = URIConstants.URL_ASPX_EXPORT_NAMERESULT + "?" + QueryStringConstants.TRACKING_NO + "=" + trackingNo + "&" + QueryStringConstants.ACTION + "=" + SharedConstants.EXPORT + "&" + QueryStringConstants.NORECORDSREPORT + "=" + this.IsNoRecordReport + "&CreateFile=Y";
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
            string trackingNo = countySearchVMC.ViewState.NameResultBindableModel.HeaderInfo.TrackingNo.ToString();
            string uri = URIConstants.URL_ASPX_EXPORT_NAMERESULT + "?" + QueryStringConstants.TRACKING_NO + "=" + trackingNo + "&" + QueryStringConstants.ACTION + "=" + SharedConstants.PRINT + "&" + QueryStringConstants.NORECORDSREPORT + "=" + this.IsNoRecordReport + "&CreateFile=N";
            HtmlPage.Window.Navigate(new Uri(uri, UriKind.Relative), "_blank");
        }

        #endregion
    }
}
