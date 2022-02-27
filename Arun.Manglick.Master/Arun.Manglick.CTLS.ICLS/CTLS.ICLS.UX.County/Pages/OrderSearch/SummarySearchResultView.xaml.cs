using System.Windows;
using System.Windows.Controls;
using CT.SLABB.UX.Controls;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;
using CTLS.Shared.UX.Controls;
using CT.SLABB.Data;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class SummarySearchResultView : UserControl
    {
        #region Variables
        FormPreviewSummaryResultDialog dialog;
        FrameworkElement __activeBlock = null;
        MultiSelectGridView __activeGridView = null;
        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public SummarySearchResultView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SummarySearchResultView_Loaded);            
        }
        #endregion

        #region Properties

        /// <summary>
        /// ActiveBlock Read-Write Property
        /// </summary>
        public FrameworkElement ActiveBlock
        {
            get { return __activeBlock; }
            set { __activeBlock = value; }
        }

        /// <summary>
        /// ActiveGridView Read-Write Property
        /// </summary>
        public MultiSelectGridView ActiveGridView
        {
            get { return __activeGridView; }
            set { __activeGridView = value; }
        }
        #endregion

        #region Methods

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
        /// Build for Search keys And Party Names
        /// </summary>
        /// <param name="searchKey">string</param>
        /// <param name="partyNames">string</param>
        private void PrepareSearchKeyAndPartyNames(out string searchKey, out string partyNames)
        {

            searchKey = string.Empty;
            partyNames = string.Empty;

            if (this.countySearchVMC != null &&
               this.countySearchVMC.ViewState != null &&
               this.countySearchVMC.ViewState.SummarySearchResultBindableModel != null &&
               this.countySearchVMC.ViewState.SummarySearchResultBindableModel.CheckedSummaryResultsList != null &&
               this.countySearchVMC.ViewState.SummarySearchResultBindableModel.CheckedSummaryResultsList.Count > 0
               )
            {
                int count = this.countySearchVMC.ViewState.SummarySearchResultBindableModel.CheckedSummaryResultsList.Count;
                int position = 0;

                foreach (SummaryResults summary in this.countySearchVMC.ViewState.SummarySearchResultBindableModel.CheckedSummaryResultsList)
                {
                    position++;
                    partyNames += summary.FirstParty;
                    searchKey += summary.UniqueKey;

                    // As no need to put pipe at the end of a search key and party names
                    if (position != count)
                    {
                        searchKey += "|";
                        partyNames += "|";
                    }
                }
            }
        }

        /// <summary>
        /// Refresh The Grid View
        /// </summary>
        private void RefreshView()
        {
            grdHeaderInfo.DataContext = null;
            grdHeaderInfo.DataContext = this.countySearchVMC.ViewState.SummarySearchResultBindableModel;

            txbTotalRecords.Text = this.countySearchVMC.ViewState.SummarySearchResultBindableModel.SummaryResultsList.Count.ToString();
            string stateName = this.countySearchVMC.ViewState.SummarySearchResultBindableModel.HeaderInfo.StateCode;
            string countyName = this.countySearchVMC.ViewState.SummarySearchResultBindableModel.HeaderInfo.CountyName;

            switch (stateName + ":" + countyName)
            {
                case "AZ:Maricopa":
                    ActiveBlock = AZMaricopaSummarySearchResultBlock;
                    ActiveGridView = AZMaricopaSummarySearchResultBlock.grvSummaryResults;

                    CASanDiegoSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    CASantaClaraSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLDadeSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLPalmBeachSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    break;
                case "CA:San Diego":
                    ActiveBlock = CASanDiegoSummarySearchResultBlock;
                    ActiveGridView = CASanDiegoSummarySearchResultBlock.grvSummaryResults;

                    AZMaricopaSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    CASantaClaraSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLDadeSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLPalmBeachSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    break;
                case "CA:Santa Clara":
                    ActiveBlock = CASantaClaraSummarySearchResultBlock;
                    ActiveGridView = CASantaClaraSummarySearchResultBlock.grvSummaryResults;

                    AZMaricopaSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    CASanDiegoSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLDadeSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLPalmBeachSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    break;
                case "FL:Dade":
                    ActiveBlock = FLDadeSummarySearchResultBlock;
                    ActiveGridView = FLDadeSummarySearchResultBlock.grvSummaryResults;

                    AZMaricopaSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    CASanDiegoSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    CASantaClaraSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLPalmBeachSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    break;
                case "FL:Palm Beach":
                    ActiveBlock = FLPalmBeachSummarySearchResultBlock;
                    ActiveGridView = FLPalmBeachSummarySearchResultBlock.grvSummaryResults;

                    AZMaricopaSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    CASanDiegoSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    CASantaClaraSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    FLDadeSummarySearchResultBlock.Visibility = Visibility.Collapsed;
                    break;
            }

            ActiveGridView.LoadingRow += ActiveGridView_LoadingRow;
            ActiveBlock.DataContext = this.countySearchVMC.ViewState.SummarySearchResultBindableModel;
            ActiveBlock.Visibility = Visibility.Visible;
            refreshIndicator.Status = RefreshableStatus.Completed;
            SetScrollPostionAtTop();
        }

        /// <summary>
        /// Clear ViewState When Back Button Clicked
        /// </summary>
        private void ClearStateOnBack()
        {
            this.countySearchVMC.ViewState.SummarySearchResultBindableModel = null;
            this.countySearchVMC.ViewState.IsNew = false;
        }

        /// <summary>
        /// Un-register The Service Call.
        /// </summary>
        private void ClearStateOnViewDetail()
        {
            if (this.countySearchVMC.ViewState.SummarySearchResultBindableModel != null)
            {
                this.countySearchVMC.ViewState.SummarySearchResultBindableModel.SSBMServiceCallCompleted1 -= SummarySearchResultBindableModel_SSBMServiceCallCompleted1;
                this.countySearchVMC.ViewState.SummarySearchResultBindableModel.SSBMServiceCallCompleted2 -= SummarySearchResultBindableModel_SSBMServiceCallCompleted2;
            }
        }
        
        #endregion

        #region Event Handlers

        #region Other Handlers

        /// <summary>
        /// Prepare Suammary Search Result View  
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns> 
        private void SummarySearchResultView_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.countySearchVMC.ViewState.SummarySearchResultBindableModel != null)
            {
                this.countySearchVMC.ViewState.SummarySearchResultBindableModel.SSBMServiceCallCompleted1 += SummarySearchResultBindableModel_SSBMServiceCallCompleted1;
                this.countySearchVMC.ViewState.SummarySearchResultBindableModel.SSBMServiceCallCompleted2 += SummarySearchResultBindableModel_SSBMServiceCallCompleted2;
                this.countySearchVMC.ViewState.SummarySearchResultBindableModel.OrderCompleted +=SummarySearchResultBindableModel_OrderCompleted;
            }

            grdHeaderInfo.DataContext = this.countySearchVMC.ViewState.SummarySearchResultBindableModel;
            if (!this.countySearchVMC.ViewState.IsNew)
                RefreshView();
        }

        /// <summary>
        ///  Summary Search Binadable First Service Call Completed
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns> 
        private void SummarySearchResultBindableModel_SSBMServiceCallCompleted1(object sender, RoutedEventArgs e)
        {
            RefreshView();
            SetScrollPostionAtTop();
        }

        /// <summary>
        ///  Summary Search Binadable Second Service Call Completed
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns> 
        private void SummarySearchResultBindableModel_SSBMServiceCallCompleted2(object sender, RoutedEventArgs e)
        {
            if (this.countySearchVMC.ViewState.DetailResultBindableModel == null)
                this.countySearchVMC.ViewState.DetailResultBindableModel = new DetailResultBindableModel();

            DetailResultBindableModel detailResultBindableModel = this.countySearchVMC.ViewState.DetailResultBindableModel;
            SummarySearchResultBindableModel summarySearchResultBindableModel = this.countySearchVMC.ViewState.SummarySearchResultBindableModel;

            //  Passing common Info to DetailResult BindableModel                
            detailResultBindableModel.HeaderInfo = summarySearchResultBindableModel.HeaderInfo;
            detailResultBindableModel.UserContext = summarySearchResultBindableModel.UserContext;
            detailResultBindableModel.ViewDetailsResponse = summarySearchResultBindableModel.ViewDetailsResponse;  // Used for Task Ids            

            UxViewFrame viewFrame = null;
            if (this.FindMyViewFrame(out viewFrame))
            {
                ClearStateOnViewDetail();
                viewFrame.Navigate(ViewConstants.DETAILRESULTSVIEW, null);
            }
        }

        /// <summary>
        ///  Order Completed Service Call Completed
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns> 
        private void SummarySearchResultBindableModel_OrderCompleted(object sender, RoutedEventArgs e)
        {
            UxViewFrame viewFrame = null;
            SummarySearchResultBindableModel summarySearchResultBindableModel = this.countySearchVMC.ViewState.SummarySearchResultBindableModel;
            this.countySearchVMC.ViewState.ClearAll();

            this.countySearchVMC.ViewState.OrderCompletedBindableModel = new OrderCompletedBindableModel();
            this.countySearchVMC.ViewState.OrderCompletedBindableModel.ErrorMessage = summarySearchResultBindableModel.ErrorMessage;
            this.countySearchVMC.ViewState.OrderCompletedBindableModel.OrderCompletedBy = summarySearchResultBindableModel.OrderCompletedBy;
            this.countySearchVMC.ViewState.OrderCompletedBindableModel.OrderCompletedOn = summarySearchResultBindableModel.OrderCompletedOn;
            this.countySearchVMC.ViewState.OrderCompletedBindableModel.HeaderInfo = summarySearchResultBindableModel.HeaderInfo;
            
            if (this.FindMyViewFrame(out viewFrame))                
                viewFrame.Navigate(ViewConstants.ORDERCOMPLETEDVIEW, null);
        }

        #endregion

        #region Control Event Handlers

        /// <summary>
        /// Back Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnBack_Click(object sender, MenuItemClickedEventArgs args)
        {
            UxViewFrame viewFrame = null;
            if (this.FindMyViewFrame(out viewFrame))
            {
                ClearStateOnBack();
                viewFrame.Navigate(ViewConstants.NAMERESULTVIEW, null);
            }
        }

        /// <summary>
        /// Form Preview Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnFormPreview_Click(object sender, MenuItemClickedEventArgs args)
        {
            dialog = new FormPreviewSummaryResultDialog();
            dialog.Show();
            dialog.countySearchVMC = this.countySearchVMC;
        }

        /// <summary>
        /// New Search Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
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
        /// View Detail Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnViewDetail_Click(object sender, MenuItemClickedEventArgs args)
        {
            if (this.ActiveGridView != null && ActiveGridView.CheckedItems.Count > 0)
            {
                this.countySearchVMC.ViewState.SummarySearchResultBindableModel.CheckedSummaryResultsList.Clear(); // Clearing the Checked List

                // To get any checked Items from Active GridView           
                MultiSelectCheckBoxColumn checkBoxColumn = null;
                CheckBox checkBox = null;
                int count = this.countySearchVMC.ViewState.SummarySearchResultBindableModel.SummaryResultsList.Count;
                for (int i = 0; i <= count - 1; i++)
                {
                    ActiveGridView.SelectedIndex = i;
                    checkBoxColumn = ActiveGridView.Columns[0] as MultiSelectCheckBoxColumn;

                    if (checkBoxColumn != null)
                    {
                        checkBox = ActiveGridView.Columns[0].GetCellContent(ActiveGridView.SelectedItem) as CheckBox;
                        if (checkBox != null && checkBox.IsChecked == true)
                            this.countySearchVMC.ViewState.SummarySearchResultBindableModel.CheckedSummaryResultsList.Add(checkBox.Tag as SummaryResults);
                    }
                }

                string searchKey = string.Empty;
                string partyNames = string.Empty;
                PrepareSearchKeyAndPartyNames(out searchKey, out partyNames);
                this.countySearchVMC.ViewState.SummarySearchResultBindableModel.BeginRefresh(searchKey, partyNames);
                this.countySearchVMC.ViewState.SummarySearchResultBindableModel.IsNewSummaryResultsList = false; // Turning it false used for checked state retaination                
            }
            else
            {
                MessageBox.Show(MessageConstants.CHECKONE_MESSAGE_VIEW_DETAIL, "View Detail", MessageBoxButton.OK);
            }

        }

        /// <summary>
        /// Loading Grid Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">DataGridRowEventArgs</param>
        /// <returns>void</returns>
        private void ActiveGridView_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            // To maintain checked state of Summary Results
            if (
                this.countySearchVMC.ViewState.SummarySearchResultBindableModel != null &&
                !this.countySearchVMC.ViewState.SummarySearchResultBindableModel.IsNewSummaryResultsList &&
                this.countySearchVMC.ViewState.SummarySearchResultBindableModel.CheckedSummaryResultsList != null &&
                this.countySearchVMC.ViewState.SummarySearchResultBindableModel.CheckedSummaryResultsList.Count > 0 &&
                this.countySearchVMC.ViewState.SummarySearchResultBindableModel.CheckedSummaryResultsList.Contains(e.Row.DataContext as SummaryResults))
            {
                MultiSelectCheckBoxColumn checkBoxColumn = ActiveGridView.Columns[0] as MultiSelectCheckBoxColumn;
                if (checkBoxColumn == null) return;
                CheckBox checkBox = checkBoxColumn.GetCellContent(e.Row) as CheckBox;
                checkBox.IsChecked = true;
                ActiveGridView.CheckedItems.Add(e.Row.DataContext);
            }
        }

        #endregion

        #endregion
    }
}
