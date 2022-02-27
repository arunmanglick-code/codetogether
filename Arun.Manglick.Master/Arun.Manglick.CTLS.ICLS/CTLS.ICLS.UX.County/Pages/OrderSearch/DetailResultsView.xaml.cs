using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CT.SLABB.UX.Controls;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;
using CTLS.Shared.UX.Controls;
using System.Windows.Browser;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class DetailResultsView : UserControl
    {
        #region Variables

        FormPreviewDetailsResultDialog dialogFP;
        ViewOptionDialogue dialogViewOptions;
        FrameworkElement __activeBlock = null;
        MultiSelectGridView __activeGridView = null;
        bool __headerInfo = false;

        #endregion

        #region Properties

        /// <summary>
        /// This Holds and Returns The ActiveBlock
        /// </summary>
        public FrameworkElement ActiveBlock
        {
            get { return __activeBlock; }
            set { __activeBlock = value; }
        }

        /// <summary>
        /// This Holds and Returns The ActiveGridView
        /// </summary>
        public MultiSelectGridView ActiveGridView
        {
            get { return __activeGridView; }
            set { __activeGridView = value; }
        }
        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public DetailResultsView()
        {
            InitializeComponent();
            this.Loaded += DetailResultsView_Loaded;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        private void SetScrollPostionAtTop()
        {
            UxPage page;
            this.FindMyPage(out page);
            (page as DesktopPage).ScrollToPoint = 0;
        }

        /// <summary>
        /// Refresh The DetailResult View
        /// </summary>
        private void RefreshView()
        {
            DetailResultBindableModel detailResultBindableModel = this.countySearchVMC.ViewState.DetailResultBindableModel;

            // processing Message and disable Back Buttons 
            this.txbProcessingMsg.Text = string.Format("Processed {0} of {1}", detailResultBindableModel.ProcessedTaskIdsCount, detailResultBindableModel.TotalTaskIdsCount);
            this.pnlTotalRecords.Visibility = Visibility.Collapsed;
            pnlProcessingMsg.Visibility = Visibility.Visible;
            pnlProcessingLiveMsg.Visibility = Visibility.Visible;
            this.EnableBackButtons(false);

            #region Blocks Visibility Management

            string stateName = detailResultBindableModel.HeaderInfo.StateCode;
            string countyName = detailResultBindableModel.HeaderInfo.CountyName;

            switch (stateName + ":" + countyName)
            {
                case "AZ:Maricopa":
                    this.ActiveBlock = this.AZMaricopaDetailRresultsBlock;
                    __activeGridView = this.AZMaricopaDetailRresultsBlock.grvDetailResults;
                    this.CASanDiegoDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    this.CASantaClaraDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    this.FLDadeDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    this.FLPalmBeachDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    __activeGridView.LoadingRow += new EventHandler<DataGridRowEventArgs>(ActiveGridView_LoadingRow);
                    break;
                case "CA:San Diego":
                    this.ActiveBlock = this.CASanDiegoDetailRresultsBlock;
                    __activeGridView = this.CASanDiegoDetailRresultsBlock.grvDetailResults;
                    this.AZMaricopaDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    this.CASantaClaraDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    this.FLDadeDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    this.FLPalmBeachDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    break;
                case "CA:Santa Clara":
                    this.ActiveBlock = this.CASantaClaraDetailRresultsBlock;
                    __activeGridView = this.CASantaClaraDetailRresultsBlock.grvDetailResults;
                    this.AZMaricopaDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    this.CASanDiegoDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    this.FLDadeDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    this.FLPalmBeachDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    break;
                case "FL:Dade":
                    this.ActiveBlock = this.FLDadeDetailRresultsBlock;
                    __activeGridView = this.FLDadeDetailRresultsBlock.grvDetailResults;
                    AZMaricopaDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    CASanDiegoDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    CASantaClaraDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    FLPalmBeachDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    __activeGridView.LoadingRow += new EventHandler<DataGridRowEventArgs>(ActiveGridView_LoadingRow);
                    break;
                case "FL:Palm Beach":
                    this.ActiveBlock = this.FLPalmBeachDetailRresultsBlock;
                    __activeGridView = this.FLPalmBeachDetailRresultsBlock.grvDetailResults;
                    this.AZMaricopaDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    this.CASanDiegoDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    this.CASantaClaraDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    this.FLDadeDetailRresultsBlock.Visibility = Visibility.Collapsed;
                    __activeGridView.LoadingRow += new EventHandler<DataGridRowEventArgs>(ActiveGridView_LoadingRow);
                    break;
            }

            this.ActiveBlock.DataContext = null;
            this.ActiveBlock.DataContext = this.countySearchVMC.ViewState.DetailResultBindableModel;
            this.ActiveBlock.Visibility = Visibility.Visible;

            #endregion

            SetScrollPostionAtTop();
        }

        /// <summary>
        /// Enable Back Button
        /// </summary>
        /// <param name="enable">bool</param> 
        /// <returns>void</returns>
        private void EnableBackButtons(bool enable)
        {
            btnBack.IsEnabled = enable;
            btnNewSearch.IsEnabled = enable;
            btnFormPreview.IsEnabled = enable;
            btnSubmit.IsEnabled = enable;

            btnBackBottom.IsEnabled = enable;
            btnNewSearchBottom.IsEnabled = enable;
            btnFormPreviewBottom.IsEnabled = enable;
            btnSubmitBottom.IsEnabled = enable;

            if (this.countySearchVMC.ViewState.DetailResultBindableModel != null &&
                this.countySearchVMC.ViewState.DetailResultBindableModel.DetailResultList != null &&
                this.countySearchVMC.ViewState.DetailResultBindableModel.DetailResultList.Count == 0)
            {
                btnFormPreview.IsEnabled = false;
                btnFormPreviewBottom.IsEnabled = false;
            }
        }

        /// <summary>
        /// Retry Button Visibility
        /// </summary>
        /// <param name="enable">bool</param> 
        /// <returns>void</returns>
        private void RetryCall()
        {
            DetailResultBindableModel detailResultBindableModel = this.countySearchVMC.ViewState.DetailResultBindableModel;
            int count = detailResultBindableModel.DetailResultList.Count(x => x.Status == OrderDetailStatusConstants.ERROR);
            if (count > 0)
            {
                this.pnlError.Visibility = Visibility.Visible;

                if (this.countySearchVMC.ViewState.SummarySearchResultBindableModel == null) // DocumentNumber Search
                {
                    this.btnSubmit.IsEnabled = false;
                    this.btnSubmitBottom.IsEnabled = false;
                    this.btnFormPreview.IsEnabled = false;
                    this.btnFormPreviewBottom.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Prepare Search Key And Party Names
        /// </summary>
        /// <param name="searchKey">string</param>
        /// <param name="partyNames">string</param>
        /// <returns>void</returns>
        private void PrepareSearchKeyAndPartyNames(out string searchKey, out string partyNames)
        {
            searchKey = string.Empty;
            partyNames = string.Empty;

            if (this.countySearchVMC != null &&
               this.countySearchVMC.ViewState != null &&
               this.countySearchVMC.ViewState.DetailResultBindableModel != null &&
               this.countySearchVMC.ViewState.DetailResultBindableModel.CheckedDetailResultsList != null &&
               this.countySearchVMC.ViewState.DetailResultBindableModel.CheckedDetailResultsList.Count > 0
               )
            {
                foreach (DetailResultListItem detail in this.countySearchVMC.ViewState.DetailResultBindableModel.CheckedDetailResultsList)
                {
                    if (!string.IsNullOrEmpty(detail.FileNumber))
                    {
                        searchKey += detail.FileNumber + "|";
                        partyNames += detail.PartyName + "|";
                    }
                }
                if (!string.IsNullOrEmpty(searchKey))
                {
                    searchKey = searchKey.Remove(searchKey.Length - 1);
                    partyNames = partyNames.Remove(partyNames.Length - 1);
                }
            }
        }

        /// <summary>
        /// Returns TrackingItem Number
        /// </summary>
        /// <returns>int</returns>
        private int GetTrackingItemNo()
        {
            UxPage page;
            string trackingItemNo = string.Empty;
            int trackingItemNumber = 2; // HardCoded

            this.FindMyPage(out page);
            DetailResultBindableModel detailResultBindableModel = this.countySearchVMC.ViewState.DetailResultBindableModel;

            if (page != null && page.NavigationContext.QueryString.ContainsKey(QueryStringConstants.TRACKING_ITEM_NO))
            {
                page.NavigationContext.QueryString.TryGetValue(QueryStringConstants.TRACKING_ITEM_NO, out trackingItemNo);
                trackingItemNumber = Convert.ToInt32(trackingItemNo);
            }
            return trackingItemNumber;
        }

        /// <summary>
        /// Returns Last ViewName From Where Navigated
        /// </summary>
        /// <returns>string</returns>
        private string GetViewName()
        {
            string viewName = ViewConstants.SUMMARYSEARCHRESULTVIEW;
            UxPage page;
            this.FindMyPage(out page);

            if (page != null && page.NavigationContext.QueryString.ContainsKey(QueryStringConstants.LAST_VIEW))
                page.NavigationContext.QueryString.TryGetValue(QueryStringConstants.LAST_VIEW, out viewName);

            return viewName;
        }

        /// <summary>
        /// Clears the View State On Back Button Click
        /// </summary>
        /// <returns>void</returns>
        private void ClearStateOnBack()
        {
            this.countySearchVMC.ViewState.DetailResultBindableModel.DetailResultList.Clear();
            this.countySearchVMC.ViewState.DetailResultBindableModel.CheckedDetailResultsList.Clear(); // Clearing the Checked List

            this.countySearchVMC.ViewState.DetailResultBindableModel.DRBMServiceCallCompleted -= DetailResultBindableModel_DRBMServiceCallCompleted;
            this.countySearchVMC.ViewState.DetailResultBindableModel.ProcessingCompleted -= DetailResultBindableModel_ProcessingCompleted;
            this.countySearchVMC.ViewState.DetailResultBindableModel.HIRefreshServiceCallCompleted -= DetailResultBindableModel_HIRefreshServiceCallCompleted;
            this.countySearchVMC.ViewState.OrderConfirmationBindableModel.OCBMServiceCallCompleted -= OrderConfirmationBindableModel_OCBMServiceCallCompleted;
            this.countySearchVMC.ViewState.OrderConfirmationBindableModel.OrderCompleted -= OrderConfirmationBindableModel_OrderCompleted;
            this.countySearchVMC.ViewState.ViewOptionsBindableModel.VOBMServiceCallCompleted -= ViewOptionsBindableModel_VOBMServiceCallCompleted;
            this.countySearchVMC.ViewState.DetailResultPreviewBindableModel.DRPBMServiceCallCompleted -= new RoutedEventHandler(DetailResultPreviewBindableModel_DRPBMServiceCallCompleted);

            this.countySearchVMC.ViewState.OrderConfirmationBindableModel = null;
            this.countySearchVMC.ViewState.ViewOptionsBindableModel = null;
            this.countySearchVMC.ViewState.DetailResultPreviewBindableModel = null;

            this.countySearchVMC.ViewState.IsNew = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReEnableBackButton()
        {
            btnBack.IsEnabled = this.countySearchVMC.ViewState.SummarySearchResultBindableModel == null ? false : true;
            btnBackBottom.IsEnabled = btnBack.IsEnabled;

            UxPage page;
            this.FindMyPage(out page);

            if (page != null && page.NavigationContext.QueryString.ContainsKey(QueryStringConstants.LAST_VIEW))
            {
                btnBack.IsEnabled = true;
                btnBackBottom.IsEnabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshHeaderInfo()
        {
            DetailResultBindableModel detailResultBindableModel = this.countySearchVMC.ViewState.DetailResultBindableModel;
            if (detailResultBindableModel != null && String.IsNullOrEmpty(detailResultBindableModel.HeaderInfo.CurrencyDate))
            {
                detailResultBindableModel.BeginHeaderInfoReRefresh();
            }
        }

        /// <summary>
        /// Handles Visibility of no records buttons
        /// </summary>
        /// <param name="flag">If flag is true then visbile else collapsed</param>
        /// <returns>void</returns>
        private void VisibleNoRecordsButtons(bool flag)
        {
            btnNoRecords.Visibility = flag ? Visibility.Visible : Visibility.Collapsed;
            btnNoRecordsbtm.Visibility = flag ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        private void DisplayBusyIndication(bool flag)
        {
            busyIndicator.IsBusy = flag;
            EnableBackButtons(!flag);
            SetScrollPostionAtTop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        private void EnableGridResults(bool flag)
        {
            if( __activeGridView != null)
            __activeGridView.IsEnabled = flag;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetOnRetry()
        {
            EnableBackButtons(false);
            this.ActiveBlock.DataContext = null;
            this.txbProcessingMsg.Text = string.Empty;

            pnlProcessingMsg.Visibility = Visibility.Collapsed;
            pnlProcessingLiveMsg.Visibility = Visibility.Visible;
            this.pnlError.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearQueryString()
        {
            UxPage page;
            this.FindMyPage(out page);
            page.NavigationContext.QueryString.Clear();
        }

        #endregion

        #region Event Handlers

        #region Other Handlers

        /// <summary>
        /// Detail Result View Loading Event Handle
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void DetailResultsView_Loaded(object sender, RoutedEventArgs e)
        {
            UxPage page;
            string trackingNo = string.Empty;
            string trackingItemNo = string.Empty;
            string taskId = string.Empty;

            this.FindMyPage(out page);
            if (page != null && page.NavigationContext.QueryString.ContainsKey(QueryStringConstants.TRACKING_NO)
                             && page.NavigationContext.QueryString.ContainsKey(QueryStringConstants.TRACKING_ITEM_NO)
                             && page.NavigationContext.QueryString.ContainsKey(QueryStringConstants.TASK_ID))
            {
                page.NavigationContext.QueryString.TryGetValue(QueryStringConstants.TRACKING_NO, out trackingNo);
                page.NavigationContext.QueryString.TryGetValue(QueryStringConstants.TRACKING_ITEM_NO, out trackingItemNo);
                page.NavigationContext.QueryString.TryGetValue(QueryStringConstants.TASK_ID, out taskId);

                if (this.countySearchVMC.ViewState.DetailResultBindableModel == null)
                    this.countySearchVMC.ViewState.DetailResultBindableModel = new DetailResultBindableModel();

                ViewDetailsResponse viewDetailsResponse = new ViewDetailsResponse();
                viewDetailsResponse.TrackingNo = Convert.ToInt64(trackingNo);
                viewDetailsResponse.TrackingItemNo = Convert.ToInt32(trackingItemNo);
                viewDetailsResponse.TaskId = taskId;

                this.countySearchVMC.ViewState.DetailResultBindableModel.ViewDetailsResponse = viewDetailsResponse;
            }

            // -------------------------------------------------------------------------------------------------------

            this.countySearchVMC.ViewState.DetailResultBindableModel.DRBMServiceCallCompleted += DetailResultBindableModel_DRBMServiceCallCompleted;
            this.countySearchVMC.ViewState.DetailResultBindableModel.ProcessingCompleted += DetailResultBindableModel_ProcessingCompleted;
            this.countySearchVMC.ViewState.DetailResultBindableModel.HIRefreshServiceCallCompleted += DetailResultBindableModel_HIRefreshServiceCallCompleted;

            if (this.countySearchVMC.ViewState.OrderConfirmationBindableModel == null)
            {
                this.countySearchVMC.ViewState.OrderConfirmationBindableModel = new OrderConfirmationBindableModel();
                this.countySearchVMC.ViewState.OrderConfirmationBindableModel.OCBMServiceCallCompleted += OrderConfirmationBindableModel_OCBMServiceCallCompleted;
                this.countySearchVMC.ViewState.OrderConfirmationBindableModel.OrderCompleted += OrderConfirmationBindableModel_OrderCompleted;
            }

            if (this.countySearchVMC.ViewState.ViewOptionsBindableModel == null)
            {
                this.countySearchVMC.ViewState.ViewOptionsBindableModel = new ViewOptionsBindableModel();
                this.countySearchVMC.ViewState.ViewOptionsBindableModel.VOBMServiceCallCompleted += ViewOptionsBindableModel_VOBMServiceCallCompleted;
            }

            if (this.countySearchVMC.ViewState.DetailResultPreviewBindableModel == null)
            {
                this.countySearchVMC.ViewState.DetailResultPreviewBindableModel = new DetailResultPreviewBindableModel();
                this.countySearchVMC.ViewState.DetailResultPreviewBindableModel.DRPBMServiceCallCompleted += new RoutedEventHandler(DetailResultPreviewBindableModel_DRPBMServiceCallCompleted);
            }

            if (this.countySearchVMC.ViewState.DetailResultBindableModel.HeaderInfo != null)
            {
                grdHeaderInfo.DataContext = null;
                grdHeaderInfo.DataContext = this.countySearchVMC.ViewState.DetailResultBindableModel;
                __headerInfo = true;
            }
            EnableBackButtons(false);
            this.countySearchVMC.ViewState.DetailResultBindableModel.UserContext = ApplicationContext.ViewState.LoggedInUserContext;
        }

        /// <summary>
        /// Set Detai Result Header Info
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void DetailResultBindableModel_DRBMServiceCallCompleted(object sender, RoutedEventArgs e)
        {
            if (!__headerInfo)
            {
                grdHeaderInfo.DataContext = null;
                grdHeaderInfo.DataContext = this.countySearchVMC.ViewState.DetailResultBindableModel;
            }

            RefreshView();
            EnableGridResults(false);
        }

        /// <summary>
        /// Set Visibility Depends On Its Empty Or Not
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void DetailResultBindableModel_ProcessingCompleted(object sender, RoutedEventArgs e)
        {
            DetailResultBindableModel detailResultBindableModel = this.countySearchVMC.ViewState.DetailResultBindableModel;
            this.txbTotalRecords.Text = detailResultBindableModel.DetailResultList.Count(x => !string.IsNullOrEmpty(x.FileNumber)).ToString();
            this.pnlTotalRecords.Visibility = Visibility.Visible;
            pnlProcessingMsg.Visibility = Visibility.Collapsed;
            pnlProcessingLiveMsg.Visibility = Visibility.Collapsed;
            this.pnlError.Visibility = Visibility.Collapsed;
            detailResultBindableModel.ProcessedTaskIdsCount = 0;  // Fixed - 2 of 1 Problem

            RefreshHeaderInfo();
            EnableBackButtons(true);
            ReEnableBackButton();
            RetryCall();
            EnableNoRecordButton();
            EnableGridResults(true);
            
            int nonErrorCount = this.countySearchVMC.ViewState.DetailResultBindableModel.DetailResultList.Where(x => x.Status != OrderDetailStatusConstants.ERROR).Count();
            if (this.countySearchVMC.ViewState.DetailResultBindableModel.DetailResultList.Count >0 )
            {
                if (nonErrorCount > 0)
                    VisibleSubmitButton(true);
                else
                    VisibleSubmitButton(false);
            }
            else
            {
                string stateName = detailResultBindableModel.HeaderInfo.StateCode;
                string countyName = detailResultBindableModel.HeaderInfo.CountyName;
                switch (stateName + ":" + countyName)
                {
                    case "AZ:Maricopa":
                        this.AZMaricopaDetailRresultsBlock.grvDetailResults.EmptyGridMessage = "Requested Document Number not on file.";
                        break;
                    case "CA:San Diego":
                        this.CASanDiegoDetailRresultsBlock.grvDetailResults.EmptyGridMessage = "Requested Document Number not on file.";
                        break;
                    case "CA:Santa Clara":
                        this.CASantaClaraDetailRresultsBlock.grvDetailResults.EmptyGridMessage = "Requested Document Number not on file.";
                        break;
                    case "FL:Dade":
                        this.FLDadeDetailRresultsBlock.grvDetailResults.EmptyGridMessage = "Requested Document Number not on file.";
                        break;
                    case "FL:Palm Beach":
                        this.FLPalmBeachDetailRresultsBlock.grvDetailResults.EmptyGridMessage = "Requested Document Number not on file.";
                        break;
                }
                VisibleSubmitButton(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        private void VisibleSubmitButton(bool flag)
        {
            btnSubmit.Visibility = flag ? Visibility.Visible : Visibility.Collapsed;
            btnSubmitBottom.Visibility = flag ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnableNoRecordButton()
        {
            if (this.countySearchVMC.ViewState.SummarySearchResultBindableModel == null &&
                this.countySearchVMC.ViewState.DetailResultBindableModel.DetailResultList.Count == 0)
                VisibleNoRecordsButtons(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetailResultBindableModel_HIRefreshServiceCallCompleted(object sender, RoutedEventArgs e)
        {
            grdHeaderInfo.DataContext = null;
            grdHeaderInfo.DataContext = this.countySearchVMC.ViewState.DetailResultBindableModel;
        }

        /// <summary>
        /// Set Order Confiramtion BM And Navigate To OrderConfiramtion View
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void OrderConfirmationBindableModel_OCBMServiceCallCompleted(object sender, RoutedEventArgs e)
        {
            UxViewFrame viewFrame = null;
            this.countySearchVMC.ViewState.ClearAtSubmit();
            DisplayBusyIndication(false);
            if (this.FindMyViewFrame(out viewFrame))
            {
                viewFrame.Navigate(ViewConstants.ORDERCONFIRMATIONVIEW, null);
            }
        }

        /// <summary>
        ///  Order Completed Service Call Completed
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns> 
        private void OrderConfirmationBindableModel_OrderCompleted(object sender, RoutedEventArgs e)
        {
            UxViewFrame viewFrame = null;
            OrderConfirmationBindableModel orderConfirmationBindableModel = this.countySearchVMC.ViewState.OrderConfirmationBindableModel;
            this.countySearchVMC.ViewState.ClearAll();

            this.countySearchVMC.ViewState.OrderCompletedBindableModel = new OrderCompletedBindableModel();
            this.countySearchVMC.ViewState.OrderCompletedBindableModel.ErrorMessage = orderConfirmationBindableModel.ErrorMessage;
            this.countySearchVMC.ViewState.OrderCompletedBindableModel.OrderCompletedBy = orderConfirmationBindableModel.OrderCompletedBy;
            this.countySearchVMC.ViewState.OrderCompletedBindableModel.OrderCompletedOn = orderConfirmationBindableModel.OrderCompletedOn;
            this.countySearchVMC.ViewState.OrderCompletedBindableModel.HeaderInfo = orderConfirmationBindableModel.HeaderInfo;

            if (this.FindMyViewFrame(out viewFrame))
                viewFrame.Navigate(ViewConstants.ORDERCOMPLETEDVIEW, null);
        }

        /// <summary>
        /// Show The View Option Dialog
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void ViewOptionsBindableModel_VOBMServiceCallCompleted(object sender, RoutedEventArgs e)
        {
            dialogViewOptions = new ViewOptionDialogue();
            dialogViewOptions.countySearchVMC = this.countySearchVMC;
            dialogViewOptions.Show();
        }

        /// <summary>
        /// Show The Form Preview Detail Result Dialog
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void DetailResultPreviewBindableModel_DRPBMServiceCallCompleted(object sender, RoutedEventArgs e)
        {
            dialogFP = new FormPreviewDetailsResultDialog();
            dialogFP.countySearchVMC = this.countySearchVMC;
            dialogFP.Show();
        }

        /// <summary>
        /// Grid Row Loding Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">DataGridRowEventArgs</param>
        /// <returns>void</returns>
        private void ActiveGridView_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            bool bShowImage = true;
            string fileNumber = (e.Row.DataContext as DetailResultListItem).FileNumber;
            string error = (e.Row.DataContext as DetailResultListItem).Status;
            string imageAvailable = (e.Row.DataContext as DetailResultListItem).ImageAvailable;
            MultiSelectCheckBoxColumn checkBoxColumn = __activeGridView.Columns[0] as MultiSelectCheckBoxColumn;
            DataGridTemplateColumn imageColumn = __activeGridView.Columns[1] as DataGridTemplateColumn;
            CheckBox checkBox = checkBoxColumn.GetCellContent(e.Row) as CheckBox;
            HyperlinkButton lnkDocImageButton = imageColumn.GetCellContent(e.Row) as HyperlinkButton;
            Image docImage = (imageColumn.GetCellContent(e.Row) as HyperlinkButton).Content as Image;

            if (lnkDocImageButton != null)
            {
                lnkDocImageButton.Click -= new RoutedEventHandler(lnkDocImageButton_Click);
                lnkDocImageButton.Click += new RoutedEventHandler(lnkDocImageButton_Click);
            }

            if (checkBox != null)
            {
                if (string.IsNullOrEmpty(fileNumber) || string.IsNullOrEmpty(imageAvailable))
                    bShowImage = false;
                else if (imageAvailable == "NO" || (!string.IsNullOrEmpty(error) && error == OrderDetailStatusConstants.ERROR))
                    bShowImage = false;

                if (!bShowImage)
                    checkBox.Visibility = docImage.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region Control Event Handlers

        /// <summary>
        /// Back Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnBack_Click(object sender, MenuItemClickedEventArgs args)
        {
            UxViewFrame viewFrame = null;
            if (this.FindMyViewFrame(out viewFrame))
            {
                ClearStateOnBack();
                string viewName = GetViewName();
               
                if (viewName.Equals(ViewConstants.SUMMARYSEARCHRESULTVIEW))
                    viewFrame.Navigate(viewName, null);
                else if (viewName.Equals(ViewConstants.PORTFOLIOFULLVIEW))
                    HtmlPage.Window.Navigate(new Uri(URIConstants.URL_PORTFOLIO, UriKind.Relative), URIConstants.CONST_self);
            }
        }

        /// <summary>
        /// New Search Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnNewSearch_Click(object sender, MenuItemClickedEventArgs args)
        {
            UxViewFrame viewFrame = null;
            if (this.FindMyViewFrame(out viewFrame))
            {
                ClearStateOnBack();
                this.countySearchVMC.ViewState.ClearAll();
                ClearQueryString();
                viewFrame.Navigate("DesktopView", null);
            }
        }

        /// <summary>
        /// Form Preview Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnFormPreview_Click(object sender, CT.SLABB.UX.Controls.MenuItemClickedEventArgs args)
        {
            DetailResultBindableModel detailResultBindableModel = this.countySearchVMC.ViewState.DetailResultBindableModel;
            DetailResultPreviewBindableModel detailResultPreviewBindableModel = this.countySearchVMC.ViewState.DetailResultPreviewBindableModel;

            detailResultPreviewBindableModel.HeaderInfo = detailResultBindableModel.HeaderInfo;
            detailResultPreviewBindableModel.TrackingItemNo = detailResultBindableModel.ViewDetailsResponse.TrackingItemNo; // GetTrackingItemNo();
            int count = this.countySearchVMC.ViewState.DetailResultBindableModel.DetailResultList.Count();
            string processedTaskIds = string.Empty;
            List<string> lstTaskIds = new List<string>();
            for (int i = 0; i <= count - 1; i++)
            {
                if (this.countySearchVMC.ViewState.DetailResultBindableModel.DetailResultList[i].Status != OrderDetailStatusConstants.ERROR)
                {
                    if (!lstTaskIds.Contains(this.countySearchVMC.ViewState.DetailResultBindableModel.DetailResultList[i].TaskID.ToString()))
                    {
                        lstTaskIds.Add(this.countySearchVMC.ViewState.DetailResultBindableModel.DetailResultList[i].TaskID.ToString());
                    }

                }
            }

            processedTaskIds = string.Join("|", lstTaskIds.ToArray());
            detailResultPreviewBindableModel.TaskIds = processedTaskIds;
        }

        /// <summary>
        /// Submit Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnSubmit_Click(object sender, MenuItemClickedEventArgs args)
        {
            bool flagSubmit = true;
            DetailResultBindableModel detailResultBindableModel = this.countySearchVMC.ViewState.DetailResultBindableModel;
            if (detailResultBindableModel.DetailResultList.Count == 0)
            { 
                   MessageBoxResult mbResult = MessageBoxResult.Cancel;
                   mbResult = MessageBox.Show(MessageConstants.MSG_CONFIRM_SUBMIT, "Submit", MessageBoxButton.OKCancel);
                   if (mbResult == MessageBoxResult.Cancel)
                       flagSubmit = false;
                   else
                       flagSubmit = true;    
            }

            if (flagSubmit)
            {
                DisplayBusyIndication(true);

                this.countySearchVMC.ViewState.DetailResultBindableModel.CheckedDetailResultsList.Clear(); // Clearing the Checked List

                if (this.ActiveGridView != null && ActiveGridView.CheckedItems.Count > 0)
                {
                    ActiveGridView.CheckedItems.ForEach(item => this.countySearchVMC.ViewState.DetailResultBindableModel.CheckedDetailResultsList.Add(item as DetailResultListItem));
                }

                string searchKey = string.Empty;
                string partyNames = string.Empty;
                string strProcessedTaskIds = string.Empty;
                PrepareSearchKeyAndPartyNames(out searchKey, out partyNames);
                
                OrderConfirmationBindableModel orderConfirmationBindableModel = this.countySearchVMC.ViewState.OrderConfirmationBindableModel;

                orderConfirmationBindableModel.HeaderInfo = detailResultBindableModel.HeaderInfo;
                orderConfirmationBindableModel.OriginalSearchKey = this.countySearchVMC.ViewState.DetailResultBindableModel.HeaderInfo.SearchKey;
                orderConfirmationBindableModel.PartyName = partyNames;
                orderConfirmationBindableModel.HeaderInfo.SearchKey = searchKey;
                orderConfirmationBindableModel.TrackingItemNo = detailResultBindableModel.ViewDetailsResponse.TrackingItemNo;
                orderConfirmationBindableModel.UserContext = detailResultBindableModel.UserContext;
                if (this.countySearchVMC.ViewState.DetailResultBindableModel.DetailResultList.Count > 0)
                {
                    orderConfirmationBindableModel.NoRecords = false;
                    List<long> processedTaskIds = this.countySearchVMC.ViewState.DetailResultBindableModel.DetailResultList.Where(x => x.Status != OrderDetailStatusConstants.ERROR).Select(x => (long)x.TaskID).Distinct().ToList<long>();
                    int position = 0;
                    int count = processedTaskIds.Count;
                    foreach (long taskId in processedTaskIds)
                    {
                        position++;
                        strProcessedTaskIds += Convert.ToString(taskId);

                        // As no need to put pipe at the end string
                        if (position != count)
                        {
                            strProcessedTaskIds += "|";
                        }
                    }

                        orderConfirmationBindableModel.SubmitType = string.Empty;
                        orderConfirmationBindableModel.NoRecords = false;
                }
                else
                {
                    strProcessedTaskIds = string.Empty;
                    if (this.countySearchVMC.ViewState.SummarySearchResultBindableModel == null)
                    {
                        orderConfirmationBindableModel.SubmitType = "DocumentNumberSearchResults";
                        orderConfirmationBindableModel.NoRecords = true;
                    }
                    else
                    {
                        orderConfirmationBindableModel.SubmitType = "NameSearchResults";
                        orderConfirmationBindableModel.NoRecords = false;
                    }

                }

               
                orderConfirmationBindableModel.BeginRefresh(strProcessedTaskIds);
                ClearQueryString();
            }
        }

        /// <summary>
        /// Cancel Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnCancel_Click(object sender, MenuItemClickedEventArgs args)
        {
            this.countySearchVMC.ViewState.DetailResultBindableModel.CancelProcessing = true;

            long trackingNo = this.countySearchVMC.ViewState.DetailResultBindableModel.HeaderInfo.TrackingNo;
            int trackingItemNo = 1;
            if (this.countySearchVMC.ViewState.SummarySearchResultBindableModel != null)
            {
                trackingItemNo = 2;
            }
            DetailResultBindableModel detailResultBindableModel = this.countySearchVMC.ViewState.DetailResultBindableModel;
            detailResultBindableModel.BeginCancel(trackingNo, trackingItemNo);
        }

        /// <summary>
        /// Retry Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnRetry_Click(object sender, MenuItemClickedEventArgs args)
        {
            ResetOnRetry();
            this.countySearchVMC.ViewState.DetailResultBindableModel.DetailResultList.Clear();
            this.countySearchVMC.ViewState.DetailResultBindableModel.RetryProcessing = true;
        }

        /// <summary>
        /// Document Image Link Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void lnkDocImageButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridCell cell = (((sender as HyperlinkButton).Parent as DataGridCell));
            DetailResultListItem rowItem = cell.DataContext as DetailResultListItem;

            DetailResultBindableModel detailResultBindableModel = this.countySearchVMC.ViewState.DetailResultBindableModel;
            ViewOptionsBindableModel viewOptionsBindableModel = this.countySearchVMC.ViewState.ViewOptionsBindableModel;
            viewOptionsBindableModel.TrackingNo = detailResultBindableModel.HeaderInfo.TrackingNo;
            viewOptionsBindableModel.TrackingItemNo = detailResultBindableModel.ViewDetailsResponse.TrackingItemNo;
            viewOptionsBindableModel.UniqueKey = rowItem.UniqueKey + SharedConstants.CARAT_SIGN + rowItem.LienType; // SummarySearch
            if (viewOptionsBindableModel.TrackingItemNo == 1) viewOptionsBindableModel.UniqueKey = rowItem.UniqueKey; // Document Type
            viewOptionsBindableModel.FileNumber = rowItem.FileNumber;
            viewOptionsBindableModel.StateCode = this.countySearchVMC.ViewState.DetailResultBindableModel.HeaderInfo.StateCode;
            viewOptionsBindableModel.CountyCode = this.countySearchVMC.ViewState.DetailResultBindableModel.HeaderInfo.CountyCode.ToString();
            viewOptionsBindableModel.CountyName = this.countySearchVMC.ViewState.DetailResultBindableModel.HeaderInfo.CountyName;
            viewOptionsBindableModel.BeginRefresh();            
        }

        /// <summary>
        /// No Records Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnNoRecords_Click(object sender, MenuItemClickedEventArgs args)
        {
            dialogFP = new FormPreviewDetailsResultDialog();
            dialogFP.countySearchVMC = countySearchVMC;
            dialogFP.IsNoRecordReport = true;
            dialogFP.Show();
        }

        #endregion

        #endregion
    }
}
