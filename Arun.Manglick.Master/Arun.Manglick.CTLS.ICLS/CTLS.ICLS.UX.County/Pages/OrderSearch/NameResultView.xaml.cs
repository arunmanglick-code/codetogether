using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CT.SLABB.Data;
using CT.SLABB.UX.Controls;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;
using CTLS.Shared.UX.Controls;
using CTLS.ICLS.UX.Controls;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class NameResultView : UserControl
    {
        #region Variables
        FormPreviewNameResultDialog dialog = new FormPreviewNameResultDialog();
        private string status = string.Empty;
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

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public NameResultView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(NameResultView_Loaded);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Enable Perform Search Button
        /// </summary>
        private void EnablePerformSearchButtons()
        {
            if (this.countySearchVMC.ViewState.NameResultBindableModel.NameList == null ||
              this.countySearchVMC.ViewState.NameResultBindableModel.NameList.Count() <= 0)
            {
                btnPerformSearch.IsEnabled = false;
                btnPerformSearchbtm.IsEnabled = false;
            }
            else
            {
                btnPerformSearch.IsEnabled = true;
                btnPerformSearchbtm.IsEnabled = true;
            }

            if (this.countySearchVMC.ViewState.NameResultBindableModel.MoreRecordsAvailable)
                this.lblMoreRecordsAvailable.Visibility =Visibility.Visible;

            if (this.countySearchVMC.ViewState.NameResultBindableModel.SearchedOldTab.Equals(SharedConstants.NO))
                this.lblSearchedOldTab.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Retrun Pipe Delimited Serach Key
        /// </summary>
        /// <returns></returns>
        private string PrepareSearchKey()
        {
            string searchKey = string.Empty;
            if (this.countySearchVMC != null &&
               this.countySearchVMC.ViewState != null &&
               this.countySearchVMC.ViewState.NameResultBindableModel != null &&
               this.countySearchVMC.ViewState.NameResultBindableModel.CheckedNameList != null &&
               this.countySearchVMC.ViewState.NameResultBindableModel.CheckedNameList.Count > 0
               )
            {
                int count = this.countySearchVMC.ViewState.NameResultBindableModel.CheckedNameList.Count;
                int position = 0;

                foreach (NameSearchResult name in this.countySearchVMC.ViewState.NameResultBindableModel.CheckedNameList)
                {
                    position++;
                    searchKey += name.PartyName;

                    // As no need to put pipe at the end of a search key
                    if (position != count)
                        searchKey += "|";
                }
            }

            return searchKey;

        }

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
        /// 
        /// </summary>
        private void RefreshView()
        {
            refreshIndicator.Status = RefreshableStatus.Completed;
            grdHeaderInfo.DataContext = null;
            grdHeaderInfo.DataContext = this.countySearchVMC.ViewState.NameResultBindableModel;
            EnablePerformSearchButtons();
            SetScrollPostionAtTop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        private void DisplayBusyIndication(bool flag)
        {
            busyIndicator.IsBusy = flag;
            EnableBackButtons(!flag);
            btnSubmit.IsEnabled = !flag;
            btnSubmitbtm.IsEnabled = !flag;
            SetScrollPostionAtTop();
        }        

        /// <summary>
        /// Enable Back Button
        /// </summary>
        /// <param name="enable">bool</param> 
        /// <returns>void</returns>
        private void EnableBackButtons(bool enable)
        {
            btnNewSearch.IsEnabled = enable;
            btnNoRecords.IsEnabled = enable;
            btnFormPreview.IsEnabled = enable;
            btnPerformSearch.IsEnabled = enable;

            btnNewSearchbtm.IsEnabled = enable;
            btnNoRecordsbtm.IsEnabled = enable;
            btnFormPreviewbtm.IsEnabled = enable;
            btnPerformSearchbtm.IsEnabled = enable;
        }
        #endregion

        #region Event Handlers

        #region Other Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameResultView_Loaded(object sender, RoutedEventArgs e)
        {
            //refreshIndicator.Status = RefreshableStatus.Working;

            UxPage page;
            string trackingNo = string.Empty;
            this.FindMyPage(out page);
            if (page != null && page.NavigationContext.QueryString.ContainsKey(QueryStringConstants.TRACKING_NO))
            {
                page.NavigationContext.QueryString.TryGetValue(QueryStringConstants.TRACKING_NO, out trackingNo);
                if (page.NavigationContext.QueryString.ContainsKey(QueryStringConstants.STATUS))
                {
                    string status = string.Empty;
                    page.NavigationContext.QueryString.TryGetValue(QueryStringConstants.STATUS, out status);
                    this.Status = status;
                }
                if (this.countySearchVMC.ViewState.NameResultBindableModel == null)
                    this.countySearchVMC.ViewState.NameResultBindableModel = new NameResultBindableModel();
                
                this.countySearchVMC.ViewState.NameResultBindableModel.BeginRefresh(Convert.ToInt64(trackingNo));

                page.NavigationContext.QueryString.Clear();

                // To Maintain Portfolio State
                if (!string.IsNullOrEmpty(this.Status))
                    page.NavigationContext.QueryString.Add(QueryStringConstants.STATUS, this.Status);
            }

            // -------------------------------------------------------------------------------------------------------

            if (this.countySearchVMC.ViewState.NameResultBindableModel != null)
                this.countySearchVMC.ViewState.NameResultBindableModel.UserContext = ApplicationContext.ViewState.LoggedInUserContext;

            if (this.countySearchVMC.ViewState.OrderConfirmationBindableModel == null)
            {
                this.countySearchVMC.ViewState.OrderConfirmationBindableModel = new OrderConfirmationBindableModel();
                this.countySearchVMC.ViewState.OrderConfirmationBindableModel.OCBMServiceCallCompleted += OrderConfirmationBindableModel_OCBMServiceCallCompleted;
                this.countySearchVMC.ViewState.OrderConfirmationBindableModel.OrderCompleted += OrderConfirmationBindableModel_OrderCompleted;
            }
            if (!this.countySearchVMC.ViewState.IsNew)
            {
                RefreshView();
            }

            this.countySearchVMC.ViewState.NameResultBindableModel.BMServiceCallCompleted += NameResultBindableModel_BMServiceCallCompleted;            
        }
        
        /// <summary>
        /// Name Result Service Call Completed
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void NameResultBindableModel_BMServiceCallCompleted(object sender, RoutedEventArgs e)
        {
            RefreshView();            
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

        #endregion

        #region Control Event Handlers

        /// <summary>
        /// perform Serach Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnPerformSearch_Click(object sender,MenuItemClickedEventArgs args)
        {
            string checkedSearchKeys = string.Empty;
            if (grvAcceptedOrders.CheckedItems.Count != 0)
            {
                UxViewFrame viewFrame = null;
                if (this.FindMyViewFrame(out viewFrame))
                {
                    this.countySearchVMC.ViewState.NameResultBindableModel.CheckedNameList.Clear(); // Clearing the Checked List

                    if (grvAcceptedOrders.CheckedItems[0] is NameSearchResult)
                        grvAcceptedOrders.CheckedItems.ForEach(
                            item => this.countySearchVMC.ViewState.NameResultBindableModel.CheckedNameList.Add(item as NameSearchResult)
                            );

                    if (this.countySearchVMC.ViewState.SummarySearchResultBindableModel == null)
                        this.countySearchVMC.ViewState.SummarySearchResultBindableModel = new SummarySearchResultBindableModel();

                    checkedSearchKeys = PrepareSearchKey();

                    //  Passing common Info to SummarySearchResult BindableModel
                    this.countySearchVMC.ViewState.SummarySearchResultBindableModel.HeaderInfo = this.countySearchVMC.ViewState.NameResultBindableModel.HeaderInfo;
                    this.countySearchVMC.ViewState.SummarySearchResultBindableModel.IsNewSummaryResultsList = true;
                    this.countySearchVMC.ViewState.SummarySearchResultBindableModel.SearchKeys = checkedSearchKeys;
                    this.countySearchVMC.ViewState.SummarySearchResultBindableModel.UserContext = this.countySearchVMC.ViewState.NameResultBindableModel.UserContext;

                    this.countySearchVMC.ViewState.SummarySearchResultBindableModel.SummaryResultsList.Clear();
                    this.countySearchVMC.ViewState.SummarySearchResultBindableModel.BeginRefresh(checkedSearchKeys);
                    this.countySearchVMC.ViewState.IsNew = true;
                    viewFrame.Navigate(ViewConstants.SUMMARYSEARCHRESULTVIEW, null);
                }
            }
            else
            {
                MessageBox.Show(MessageConstants.CHECKONE_MESSAGE_PERFORM_SEARCH, "Perform Search", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Name Serach Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnNewSearch_Click(object sender, MenuItemClickedEventArgs args)
        {
            UxViewFrame viewFrame = null;
            if (this.FindMyViewFrame(out viewFrame))
            {
                this.countySearchVMC.ViewState.ClearAll();
                viewFrame.Navigate(ViewConstants.DESKTOPVIEW, null);
            }
        }

        /// <summary>
        /// Form preview Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnFormPreview_Click(object sender, MenuItemClickedEventArgs args)
        {
            if (this.countySearchVMC.ViewState.NameResultBindableModel.NameList.Count > 0)
            {
                dialog.countySearchVMC = countySearchVMC;
                dialog.IsNoRecordReport = false;
                dialog.Show();
            }
            else
            {
                btnNoRecords_Click(sender,args);
            }
        }

        /// <summary>
        /// Loading Grid Row Event Handle
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">DataGridRowEventArgs</param>
        /// <returns>void</returns>
        private void grvAcceptedOrders_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            // To maintain checked state of names
            if (!this.countySearchVMC.ViewState.IsNew &&
                this.countySearchVMC.ViewState.NameResultBindableModel != null &&
                this.countySearchVMC.ViewState.NameResultBindableModel.CheckedNameList != null &&
                this.countySearchVMC.ViewState.NameResultBindableModel.CheckedNameList.Count > 0 &&
                this.countySearchVMC.ViewState.NameResultBindableModel.CheckedNameList.Contains(e.Row.DataContext as NameSearchResult))
            {
                MultiSelectCheckBoxColumn checkBoxColumn = grvAcceptedOrders.Columns[0] as MultiSelectCheckBoxColumn;
                CheckBox checkBox = checkBoxColumn.GetCellContent(e.Row) as CheckBox;
                checkBox.IsChecked = true;
                grvAcceptedOrders.CheckedItems.Add(e.Row.DataContext);                
            }
        }

        /// <summary>
        /// No Records Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnNoRecords_Click(object sender, MenuItemClickedEventArgs args)
        {
            dialog.countySearchVMC = countySearchVMC;
            dialog.IsNoRecordReport = true;
            dialog.Show();
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
            MessageBoxResult mbResult = MessageBoxResult.Cancel;
            mbResult = MessageBox.Show(MessageConstants.MSG_CONFIRM_SUBMIT, "Submit", MessageBoxButton.OKCancel);
            flagSubmit = mbResult == MessageBoxResult.Cancel ? false : true;
            
            if (flagSubmit)
            {
                DisplayBusyIndication(true);             
                string taskId = string.Empty;
                NameResultBindableModel nameResultBindableModel = this.countySearchVMC.ViewState.NameResultBindableModel;
                OrderConfirmationBindableModel orderConfirmationBindableModel = this.countySearchVMC.ViewState.OrderConfirmationBindableModel;

                orderConfirmationBindableModel.HeaderInfo = nameResultBindableModel.HeaderInfo;
                orderConfirmationBindableModel.OriginalSearchKey = this.countySearchVMC.ViewState.NameResultBindableModel.HeaderInfo.SearchKey;
                orderConfirmationBindableModel.TrackingItemNo = 1;//Since here TrackingItem # will always be 1.
                orderConfirmationBindableModel.UserContext = nameResultBindableModel.UserContext;
                if (nameResultBindableModel.NameList.Count > 0)
                    orderConfirmationBindableModel.NoRecords = false;
                else
                    orderConfirmationBindableModel.NoRecords = true;
                orderConfirmationBindableModel.SubmitType = "NameSearchResults";
                orderConfirmationBindableModel.BeginRefresh(taskId);
            }
        }

        #endregion

        #endregion
    }
}
