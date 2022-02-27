using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using CT.SLABB.Data;
using System.Linq;
using CT.SLABB.DX;
using CT.SLABB.UX.Controls;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;
using CTLS.Shared.UX.Controls;
using System.Collections.Generic;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class PortfolioFullView : UserControl
    {
        #region Variables
        private const string NAMERESULTVIEW = "/NameResultView/";
        private const string DETAILRESULTVIEW = "/DetailResultsView/";
        private const string ORDERDETAILSVIEW = "/OrderDetailsView/";
        private const string PORTFOLIOFULLVIEW = "PortfolioFullView";
        private const string TIMEFRAMEFOR = "Week";
        private const string DEAFAULT_TEXT = "Default Text";
        private const char RETRY_TYPE = 'S';
        private const int TRACKING_ITEMNO = 1;
        private string __status = String.Empty;
        private bool isStatusStackVisible = true;
        private bool isSetPortfolioSearchBy1 = false;
        private bool isSetPortfolioSearchBy2 = false;
        private bool goClicked = false;

        private string tempTimeFrameBy = string.Empty;

        OrdersPortfolioBindableModel ordersPortfolioBindableModel;
        LastItemsBindableModel lastItemsBindableModel;
        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public PortfolioFullView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PortfolioFullView_Loaded);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validate Page
        /// </summary>
        /// <returns>bool</returns>
        private bool Validate()
        {
            if (vlsValidationSummary.Errors != null) vlsValidationSummary.Errors.Clear();

            OrdersPortfolioBindableModel OPmodel = dfPortfolioSearch.CurrentItem as OrdersPortfolioBindableModel;

            TextBox txbSearchBy1 = dfPortfolioSearch.FindNameInContent("txbSearchBy1") as TextBox;
            TextBox txbSearchBy2 = dfPortfolioSearch.FindNameInContent("txbSearchBy2") as TextBox;
            CustomDatePicker dtpFromDate = dfPortfolioSearch.FindNameInContent("dtpFromDate") as CustomDatePicker;
            CustomDatePicker dtpToDate = dfPortfolioSearch.FindNameInContent("dtpToDate") as CustomDatePicker;

            OPmodel.TempFromDate = dtpFromDate.DateText;
            OPmodel.TempToDate = dtpToDate.DateText;

            BindingExpression expSearchBy1 = null;
            BindingExpression expSearchBy2 = null;
            BindingExpression expFromDate = null;
            BindingExpression expToDate = null;

            if (txbSearchBy1 != null)
                expSearchBy1 = txbSearchBy1.GetBindingExpression(TextBox.TextProperty);
            if (txbSearchBy2 != null)
                expSearchBy2 = txbSearchBy2.GetBindingExpression(TextBox.TextProperty);
            if (dtpFromDate != null)
                expFromDate = dtpFromDate.GetBindingExpression(CustomDatePicker.TextProperty);
            if (dtpToDate != null)
                expToDate = dtpToDate.GetBindingExpression(CustomDatePicker.TextProperty);

            if (expSearchBy1 != null)
                expSearchBy1.UpdateSource();

            if (expSearchBy2 != null)
                expSearchBy2.UpdateSource();

            if (expFromDate != null)
                expFromDate.UpdateSource();

            if (expToDate != null)
                expToDate.UpdateSource();

            return vlsValidationSummary.Errors.Count > 0 ? false : true;
        }

        /// <summary>
        ///  Set The Preference Level
        /// </summary>
        public void SetPreferenceLevel()
        {
            if (ApplicationContext.ViewState != null)
            {
                rdoMyOrders.Visibility = Visibility.Visible;
                if (ApplicationContext.ViewState.LoggedInUserContext.SecurityRole.Equals(SecurityRole.User))
                {
                    rdoTeamOrders.Visibility = Visibility.Collapsed;
                    rdoAllOrders.Visibility = Visibility.Collapsed;
                }
                else if (ApplicationContext.ViewState.LoggedInUserContext.SecurityRole.Equals(SecurityRole.Manager))
                {
                    rdoTeamOrders.Visibility = Visibility.Visible;
                    rdoAllOrders.Visibility = Visibility.Collapsed;
                }
                else
                {
                    rdoTeamOrders.Visibility = Visibility.Visible;
                    rdoAllOrders.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Build Portfolio Search DTO
        /// </summary>
        private void PortfolioSearch()
        {
            StackPanel stkStatus = dfPortfolioSearch.FindNameInContent("stkStatus") as StackPanel;
            if (Validate())
            {
                string FromDate = string.Empty;
                string ToDate = string.Empty;
                ComboBox ddlsearchBy1 = dfPortfolioSearch.FindNameInContent("ddlsearchBy1") as ComboBox;
                ComboBox ddlsearchBy2 = dfPortfolioSearch.FindNameInContent("ddlsearchBy2") as ComboBox;
                TextBox txbSearchBy1 = dfPortfolioSearch.FindNameInContent("txbSearchBy1") as TextBox;
                TextBox txbSearchBy2 = dfPortfolioSearch.FindNameInContent("txbSearchBy2") as TextBox;
                ComboBox ddlSearchOrderDate = dfPortfolioSearch.FindNameInContent("ddlSearchOrderDate") as ComboBox;
                ComboBox ddlCustomDate = dfPortfolioSearch.FindNameInContent("ddlCustomDate") as ComboBox;
                CustomDatePicker dtpFromDate = (dfPortfolioSearch.FindNameInContent("dtpFromDate") as CustomDatePicker);
                CustomDatePicker dtpToDate = (dfPortfolioSearch.FindNameInContent("dtpToDate") as CustomDatePicker);

                if (ddlsearchBy1 != null) ordersPortfolioBindableModel.SearchBy1 = (ddlsearchBy1.SelectedItem as ComboBoxItem).Content.ToString().Trim();
                if (ddlsearchBy2 != null) ordersPortfolioBindableModel.SearchBy2 = (ddlsearchBy2.SelectedItem as ComboBoxItem).Content.ToString().Trim();
                if (txbSearchBy1 != null) ordersPortfolioBindableModel.Searchfor1 = txbSearchBy1.Text.Trim();
                if (txbSearchBy2 != null) ordersPortfolioBindableModel.Searchfor2 = txbSearchBy2.Text.Trim();
                if (ddlSearchOrderDate != null) ordersPortfolioBindableModel.TimeFrameby = (ddlSearchOrderDate.SelectedItem as ComboBoxItem).Content.ToString().Trim();
                if (ddlCustomDate != null) ordersPortfolioBindableModel.TimeFramefor = (ddlCustomDate.SelectedItem as ComboBoxItem).Tag.ToString().Trim();
                if (dtpFromDate != null) FromDate = dtpFromDate.DateText;
                if (dtpToDate != null) ToDate = dtpToDate.DateText;

                ordersPortfolioBindableModel.TempFromDate = string.IsNullOrEmpty(FromDate) ? null : FromDate;
                ordersPortfolioBindableModel.TempToDate = string.IsNullOrEmpty(ToDate) ? null : ToDate;

                ordersPortfolioBindableModel.FromDate = string.IsNullOrEmpty(FromDate) ? null : FromDate;
                ordersPortfolioBindableModel.ToDate = string.IsNullOrEmpty(ToDate) ? null : ToDate;
                ordersPortfolioBindableModel.TeamId = ApplicationContext.ViewState.LoggedInUserContext.TeamID.ToString().Trim();
                ordersPortfolioBindableModel.UserNo = ApplicationContext.ViewState.LoggedInUserContext.UserId.ToString().Trim();
                lastItemsBindableModel.UserNo = ordersPortfolioBindableModel.UserNo.Trim(); // To Synchronize  data of Last item view and Order portfolio gridview
                ordersPortfolioBindableModel.PreferenceLevel = GetPreference();

                if (stkStatus != null && isStatusStackVisible)
                    ordersPortfolioBindableModel.Status = ((dfPortfolioSearch.FindNameInContent("ddlStatus") as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();
                else
                {
                    ordersPortfolioBindableModel.Status = __status;
                    ColumnVisibility();
                }

                ordersPortfolioBindableModel.StatusEmpty = OrderDetailStatusConstants.SEARCHINPROGRESS;
                ordersPortfolioBindableModel.BeginRefresh();
            }

            refreshIndicator.Status = RefreshableStatus.Completed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddlsearchBy1"></param>
        /// <param name="txbSearchBy1"></param>
        private void SetPortfolioSearch()
        {
            if (this.ApplicationContext.ViewState.PortFolioBindableModel != null)
            {
                ordersPortfolioBindableModel = this.ApplicationContext.ViewState.PortFolioBindableModel as OrdersPortfolioBindableModel;
                dfPortfolioSearch.CurrentItem = ordersPortfolioBindableModel;

                ComboBox ddlsearchBy1 = dfPortfolioSearch.FindNameInContent("ddlsearchBy1") as ComboBox;
                ComboBox ddlsearchBy2 = dfPortfolioSearch.FindNameInContent("ddlsearchBy2") as ComboBox;
                ComboBox ddlSearchOrderDate = dfPortfolioSearch.FindNameInContent("ddlSearchOrderDate") as ComboBox;
                ComboBox ddlCustomDate = dfPortfolioSearch.FindNameInContent("ddlCustomDate") as ComboBox;
                CustomDatePicker dtpFromDate = (dfPortfolioSearch.FindNameInContent("dtpFromDate") as CustomDatePicker);
                CustomDatePicker dtpToDate = (dfPortfolioSearch.FindNameInContent("dtpToDate") as CustomDatePicker);
                ComboBox ddlStatus = dfPortfolioSearch.FindNameInContent("ddlStatus") as ComboBox;
                StackPanel stkStatus = dfPortfolioSearch.FindNameInContent("stkStatus") as StackPanel;

                ordersPortfolioBindableModel = this.ApplicationContext.ViewState.PortFolioBindableModel as OrdersPortfolioBindableModel;
                if (ordersPortfolioBindableModel != null)
                {
                    if (ordersPortfolioBindableModel.TimeFrameby != null && ordersPortfolioBindableModel.Status.Equals(OrderDetailStatusConstants.COMPLETED)) AddRemove_TimeFrameByItem(OrderDetailStatusConstants.COMPLETED);
                    if (ordersPortfolioBindableModel.SearchBy1 != null) ddlsearchBy1.SelectedIndex = GetComboIndex(ddlsearchBy1, ordersPortfolioBindableModel.SearchBy1);
                    if (ordersPortfolioBindableModel.SearchBy2 != null) ddlsearchBy2.SelectedIndex = GetComboIndex(ddlsearchBy2, ordersPortfolioBindableModel.SearchBy2);
                    isSetPortfolioSearchBy1 = true;
                    isSetPortfolioSearchBy2 = true;

                    if (ordersPortfolioBindableModel.TimeFrameby != null) ddlSearchOrderDate.SelectedIndex = ordersPortfolioBindableModel.TimeFrameby.Equals("Search/Order Date") ? 0 : 1;
                    if (ordersPortfolioBindableModel.TimeFramefor != null) ddlCustomDate.SelectedIndex = GetComboIndexByTag(ddlCustomDate, ordersPortfolioBindableModel.TimeFramefor);

                    //if (stkStatus != null && stkStatus.Visibility != Visibility.Collapsed && !isStatusStackVisible && ordersPortfolioBindableModel.Status != null)
                    //    ddlStatus.SelectedIndex = GetComboIndex(ddlStatus, ordersPortfolioBindableModel.Status);
                    //else
                    //    stkStatus.Visibility = Visibility.Collapsed;

                    switch (ordersPortfolioBindableModel.PreferenceLevel)
                    {
                        case "A":
                            rdoAllOrders.IsChecked = true;
                            break;
                        case "M":
                            rdoMyOrders.IsChecked = true;
                            break;
                        case "T":
                            rdoTeamOrders.IsChecked = true;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddlsearchBy1"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        private int GetComboIndex(ComboBox ddlsearchBy1, string searchString)
        {
            int i = 0;
            bool found = false;
            if (ddlsearchBy1 != null)
                foreach (ComboBoxItem item in ddlsearchBy1.Items)
                {
                    if (item.Content.ToString().Equals(searchString))
                    {
                        found = true;
                        break;
                    }
                    i++;
                }

            i = found ? i : 0;
            return i;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddlsearchBy1"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        private ComboBoxItem GetComboItem(ComboBox ddlsearchBy1, string searchString)
        {
            int i = 0;
            bool found = false;
            ComboBoxItem selectedItem = null;
            if (ddlsearchBy1 != null)
                foreach (ComboBoxItem item in ddlsearchBy1.Items)
                {
                    if (item.Content.ToString().Equals(searchString))
                    {
                        found = true;
                        selectedItem = item;
                        break;
                    }
                    i++;
                }

            i = found ? i : 0;
            return selectedItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddlsearchBy1"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        private int GetComboIndexByTag(ComboBox ddlsearchBy1, string searchString)
        {
            int i = 0;
            if (ddlsearchBy1 != null)
                foreach (ComboBoxItem item in ddlsearchBy1.Items)
                {
                    if (item.Tag.ToString().Equals(searchString))
                        break;
                    i++;
                }
            return i;
        }

        /// <summary>
        /// Depends On Status Make Column Visible Invisible
        /// </summary>
        private void ColumnVisibility()
        {
            gvwSearchOrderPortfolio.Columns[0].Width = new DataGridLength(255);
            gvwSearchOrderPortfolio.Columns[1].Width = new DataGridLength(130);
            gvwSearchOrderPortfolio.Columns[4].Width = new DataGridLength(100);
            gvwSearchOrderPortfolio.Columns[5].Width = new DataGridLength(100);
            gvwSearchOrderPortfolio.Columns[6].Width = new DataGridLength(100);
            gvwSearchOrderPortfolio.Columns[7].Width = new DataGridLength(100);

            if (ordersPortfolioBindableModel.Status.Equals(OrderDetailStatusConstants.INPROCESS) || ordersPortfolioBindableModel.Status.Equals(OrderDetailStatusConstants.COMPLETED))
            {
                gvwSearchOrderPortfolio.Columns[2].Width = new DataGridLength(120);
                gvwSearchOrderPortfolio.Columns[3].Width = new DataGridLength(150);
                gvwSearchOrderPortfolio.Columns[4].Width = new DataGridLength(153);
                if (ordersPortfolioBindableModel.Status.Equals(OrderDetailStatusConstants.INPROCESS))
                {
                    gvwSearchOrderPortfolio.Columns[0].Width = new DataGridLength(330);
                    gvwSearchOrderPortfolio.Columns[1].Width = new DataGridLength(200);
                }
                (gvwSearchOrderPortfolio.Columns[5] as DataGridTemplateColumn).Visibility = Visibility.Collapsed;
                (gvwSearchOrderPortfolio.Columns[6] as DataGridTemplateColumn).Visibility = Visibility.Collapsed;
                (gvwSearchOrderPortfolio.Columns[7] as DataGridTemplateColumn).Visibility = Visibility.Collapsed;
                if (ordersPortfolioBindableModel.Status.Equals(OrderDetailStatusConstants.COMPLETED))
                {
                    gvwSearchOrderPortfolio.Columns[7].Width = new DataGridLength(145);
                    (gvwSearchOrderPortfolio.Columns[7] as DataGridTemplateColumn).Visibility = Visibility.Visible;
                }
            }
            else
            {
                (gvwSearchOrderPortfolio.Columns[5] as DataGridTemplateColumn).Visibility = Visibility.Visible;
                (gvwSearchOrderPortfolio.Columns[6] as DataGridTemplateColumn).Visibility = Visibility.Visible;
                (gvwSearchOrderPortfolio.Columns[7] as DataGridTemplateColumn).Visibility = Visibility.Collapsed;
            }

            /* Code for :: After Changing Columns of GridView in portFolio some space remains at left side og GridView, 
              to adjust it so that All gridview columns  can occupy 100% space. */

            /* ASSUMING USER WILL NOT REORDER COLUMNS   */
            /* If user Reordered it then code will fail */
            //int visibleColumnIndex = 0;
            //for (int i = (gvwSearchOrderPortfolio.Columns.Count - 1); i > 0; i--)
            //{
            //    if (gvwSearchOrderPortfolio.Columns[i].Visibility == System.Windows.Visibility.Visible)
            //    {
            //        visibleColumnIndex = i;
            //        break;
            //    }
            //}
            ////var visibleColumns = gvwSearchOrderPortfolio.Columns.Where(c => c.Visibility == System.Windows.Visibility.Visible && c.DisplayIndex != visibleColumnIndex);
            //double width = 0;
            //width = visibleColumns.Select(c => c.ActualWidth).Aggregate((counter, item) => counter += item);
            //if (width < gvwSearchOrderPortfolio.ActualWidth && visibleColumnIndex != 0)
            //    gvwSearchOrderPortfolio.Columns[visibleColumnIndex].Width = new DataGridLength(gvwSearchOrderPortfolio.ActualWidth - width);

        }

        /// <summary>
        /// Display Default Grid View
        /// </summary>
        private void DefaultGrid(string status)
        {
            gvwSearchOrderPortfolio.ItemsSource = null;
            ordersPortfolioBindableModel = new OrdersPortfolioBindableModel();
            ordersPortfolioBindableModel.Status = status;
            grdOrdersPortfolio.DataContext = ordersPortfolioBindableModel;
        }

        /// <summary>
        /// Retriving Preference Level
        /// </summary>
        /// <returns></returns>
        private string GetPreference()
        {
            if (rdoAllOrders.IsChecked == true)
            {
                return "A";
            }
            else if (rdoMyOrders.IsChecked == true)
            {
                return "M";
            }
            else
            {
                return "T";
            }
        }

        /// <summary>
        /// Clear Page Content
        /// </summary>
        private void ClearContent()
        {
            ComboBox temp = null;
            temp = dfPortfolioSearch.FindNameInContent("ddlsearchBy1") as ComboBox;
            if (temp != null)
                temp.SelectedIndex = 0;
            temp = dfPortfolioSearch.FindNameInContent("ddlsearchBy2") as ComboBox;
            if (temp != null)
                temp.SelectedIndex = 0;
            TextBox txbSearchBy1 = (dfPortfolioSearch.FindNameInContent("txbSearchBy1") as TextBox);
            if (txbSearchBy1 != null)
                txbSearchBy1.Text = String.Empty;
            TextBox txbSearchBy2 = (dfPortfolioSearch.FindNameInContent("txbSearchBy2") as TextBox);
            if (txbSearchBy2 != null)
                txbSearchBy2.Text = String.Empty;
            temp = dfPortfolioSearch.FindNameInContent("ddlSearchOrderDate") as ComboBox;
            if (temp != null)
                temp.SelectedIndex = 0;
            temp = dfPortfolioSearch.FindNameInContent("ddlCustomDate") as ComboBox;
            if (temp != null)
                temp.SelectedIndex = 1;

            CustomDatePicker dtpFromDate = (dfPortfolioSearch.FindNameInContent("dtpFromDate") as CustomDatePicker);
            if (dtpFromDate != null)
                dtpFromDate.DateText = String.Empty;

            CustomDatePicker dtpToDate = (dfPortfolioSearch.FindNameInContent("dtpToDate") as CustomDatePicker);
            if (dtpToDate != null)
                dtpToDate.DateText = String.Empty;


            temp = dfPortfolioSearch.FindNameInContent("ddlStatus") as ComboBox;
            if (temp != null)
                temp.SelectedIndex = 0;
        }
        
        /// <summary>
        /// Apply Style To Textblock To Look Like Link
        /// </summary>
        /// <param name="txtBlock"></param>
        private void ConvertToLink(List<TextBlock> txtBlocks)
        {
            byte red = 115; byte green = 169; byte blue = 216;
            txtBlocks.ForEach(delegate(TextBlock txtBlock)
            {
                txtBlock.Cursor = Cursors.Hand;
                txtBlock.TextDecorations = TextDecorations.Underline;
                txtBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, red, green, blue));
            });
        }

        /// <summary>
        /// Add or Remove Item from Search OrderDate Dropdown Depends On Status
        /// </summary>
        /// <param name="status">string</param>
        private void AddRemove_TimeFrameByItem(string status)
        {
            ComboBoxItem cmbItem = new ComboBoxItem();
            cmbItem.Content = "Completed Date";
            DropDownList ddlSearchOrderDate = (dfPortfolioSearch.FindNameInContent("ddlSearchOrderDate") as DropDownList);
            if (ddlSearchOrderDate != null && status.Equals(OrderDetailStatusConstants.COMPLETED) && ddlSearchOrderDate.Items.Count < 2)
                ddlSearchOrderDate.Items.Insert(1, cmbItem);
            if (ddlSearchOrderDate != null && !status.Equals(OrderDetailStatusConstants.COMPLETED) && ddlSearchOrderDate.Items.Count > 1)
                ddlSearchOrderDate.Items.RemoveAt(1);
        }

        /// <summary>
        /// Used to Set the 'SearchBy1' & 'SearchBy2', on 'ddlsearchBy1/2_SelectionChanged' - After coming back on Portfolio page.
        /// </summary>
        private void FuseSetPortfolioSearchBy()
        {
            isSetPortfolioSearchBy1 = false;
            isSetPortfolioSearchBy2 = false;
        }

        /// <summary>
        /// Set Scroll Position On Top
        /// </summary>        
        /// <returns>void</returns>
        private void SetScrollPostionAtTop()
        {
            UxPage page;
            this.FindMyPage(out page);
            (page as PortfolioPage).ScrollToPoint = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReBindGridOnBack()
        {
            grdOrdersPortfolio.DataContext = null;
            ordersPortfolioBindableModel = this.ApplicationContext.ViewState.PortFolioBindableModel as OrdersPortfolioBindableModel;
            ordersPortfolioBindableModel.StatusEmpty = OrderDetailStatusConstants.SEARCHINPROGRESS;

            if (ordersPortfolioBindableModel.UserNo != null)
                ordersPortfolioBindableModel.BeginRefresh();
            else
            {
                ordersPortfolioBindableModel.StatusEmpty = MessageConstants.DEFAULT_SEARCH_GRID_MESSAGE;                
                grdOrdersPortfolio.DataContext = ordersPortfolioBindableModel;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FuseDateValidation(string lastView)
        {
            if (vlsValidationSummary.Errors != null && vlsValidationSummary.Errors.Count > 0)
            {
                bool res = dfPortfolioSearch.CancelEdit();
                vlsValidationSummary.Errors.Clear();
            }
            else
            {
                dfPortfolioSearch.BeginEdit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FuseDateValidation1(string lastView)
        {
            if (vlsValidationSummary.Errors != null) vlsValidationSummary.Errors.Clear();

        }

        #endregion

        #region Event Handlers

        #region Control Event Handlers

        /// <summary>
        /// Mouse Left button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MouseButtonEventArgs</param>
        /// <returns>void</returns>
        private void txbOpen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock txbSender = sender as TextBlock;
            bool visited = false;
            ComboBox ddlStatus = dfPortfolioSearch.FindNameInContent("ddlStatus") as ComboBox;
            StackPanel stkStatus = dfPortfolioSearch.FindNameInContent("stkStatus") as StackPanel;
            FuseDateValidation(String.Empty);
            
            switch (txbSender.Text)
            {
                case OrderDetailStatusConstants.OPEN:
                    if (!txbOpen.Cursor.Equals(Cursors.Arrow))
                    {
                        visited = true;
                        PortFolioGridHeaderMessage.Text = MessageConstants.OPEN_ORDERS;
                        txbOpen.Cursor = Cursors.Arrow;
                        txbOpen.TextDecorations = null;
                        txbOpen.Foreground = new SolidColorBrush(Colors.Black);
                        ConvertToLink(new List<TextBlock> { txbInProcess, txbCompleted, txbFullPortfolioView });

                        isStatusStackVisible = false;
                        if (stkStatus != null) stkStatus.Visibility = Visibility.Collapsed;

                        __status = OrderDetailStatusConstants.OPEN_ERROR;
                        AddRemove_TimeFrameByItem(__status);
                        DefaultGrid(__status);
                    }
                    break;
                case OrderDetailStatusConstants.INPROCESS:
                    if (!txbInProcess.Cursor.Equals(Cursors.Arrow))
                    {
                        visited = true;
                        PortFolioGridHeaderMessage.Text = MessageConstants.INPROCESS_ORDERS;
                        txbInProcess.Cursor = Cursors.Arrow;
                        txbInProcess.TextDecorations = null;
                        txbInProcess.Foreground = new SolidColorBrush(Colors.Black);
                        ConvertToLink(new List<TextBlock> { txbOpen, txbCompleted, txbFullPortfolioView });

                        isStatusStackVisible = false;
                        if (stkStatus != null) stkStatus.Visibility = Visibility.Collapsed;

                        __status = OrderDetailStatusConstants.INPROCESS;
                        AddRemove_TimeFrameByItem(__status);
                        DefaultGrid(__status);
                    }
                    break;

                case OrderDetailStatusConstants.COMPLETED:
                    if (!txbCompleted.Cursor.Equals(Cursors.Arrow))
                    {
                        visited = true;
                        PortFolioGridHeaderMessage.Text = MessageConstants.COMLETED_ORDERS;
                        txbCompleted.Cursor = Cursors.Arrow;
                        txbCompleted.TextDecorations = null;
                        txbCompleted.Foreground = new SolidColorBrush(Colors.Black);
                        ConvertToLink(new List<TextBlock> { txbOpen, txbInProcess, txbFullPortfolioView });

                        isStatusStackVisible = false;
                        if (stkStatus != null) stkStatus.Visibility = Visibility.Collapsed;

                        __status = OrderDetailStatusConstants.COMPLETED;
                        AddRemove_TimeFrameByItem(__status);
                        DefaultGrid(__status);
                    }
                    break;

                default:
                    if (!txbFullPortfolioView.Cursor.Equals(Cursors.Arrow))
                    {
                        visited = true;
                        PortFolioGridHeaderMessage.Text = MessageConstants.FULLSEARCH_ORDERS;
                        txbFullPortfolioView.Cursor = Cursors.Arrow;
                        txbFullPortfolioView.TextDecorations = null;
                        txbFullPortfolioView.Foreground = new SolidColorBrush(Colors.Black);
                        ConvertToLink(new List<TextBlock> { txbOpen, txbInProcess, txbCompleted });

                        isStatusStackVisible = true;
                        if (stkStatus != null) stkStatus.Visibility = Visibility.Visible;

                        __status = DEAFAULT_TEXT;
                        AddRemove_TimeFrameByItem(__status);
                        DefaultGrid(__status);
                    }
                    break;
            }

            if (visited)
            {
                ClearContent();
                ColumnVisibility();
            }

            this.ApplicationContext.ViewState.PortFolioStatus = txbSender.Text;

            ordersPortfolioBindableModel.OPBMServiceCallCompleted -= new RoutedEventHandler(OrdersPortfolioBindableModel_OPBMServiceCallCompleted);
            ordersPortfolioBindableModel.OPBMServiceCallCompleted += new RoutedEventHandler(OrdersPortfolioBindableModel_OPBMServiceCallCompleted);
        }

        /// <summary>
        /// Go Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void GoBtn_Click(object sender, MenuItemClickedEventArgs args)
        {
            goClicked = true;
            refreshIndicator.Status = RefreshableStatus.Waiting;
            this.ApplicationContext.ViewState.PortFolioBindableModel = null;
            PortfolioSearch();
        }

        /// <summary>
        /// Clear Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void clearBtn_Click(object sender, MenuItemClickedEventArgs args)
        {
            ClearContent();
        }

        /// <summary>
        /// Portfolio Grid Loading event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">DataGridRowEventArgs</param>
        /// <returns>void</returns>
        private void gvwSearchOrderPortfolio_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.DataContext is PortfolioResult)
            {
                PortfolioResult portfolioResult = e.Row.DataContext as PortfolioResult;
                if (gvwSearchOrderPortfolio.Columns[6].Visibility != Visibility.Collapsed)
                {
                    if (portfolioResult.Status.Equals(OrderDetailStatusConstants.ERROR))
                    {
                        MenuItem btnerror = (((gvwSearchOrderPortfolio.Columns[6] as DataGridTemplateColumn).GetCellContent(e.Row)) as MenuBar).Items[0] as MenuItem;
                        if (btnerror != null)
                            btnerror.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        MenuItem btnerror = (((gvwSearchOrderPortfolio.Columns[6] as DataGridTemplateColumn).GetCellContent(e.Row)) as MenuBar).Items[0] as MenuItem;
                        if (btnerror != null)
                            btnerror.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        /// <summary>
        /// SelectionChanged Event of SearchBy1 Dropdown
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        /// <returns>void</returns>
        private void ddlsearchBy1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox ddlsearchBy1 = sender as ComboBox;
            OrdersPortfolioBindableModel ordersPortfolioBindableModel = (OrdersPortfolioBindableModel)dfPortfolioSearch.CurrentItem;

            if (this.ApplicationContext.ViewState.PortFolioBindableModel == null)
                ordersPortfolioBindableModel.SearchBy1 = ddlsearchBy1.SelectedItem != null ? (ddlsearchBy1.SelectedItem as ComboBoxItem).Content.ToString() : String.Empty;

            if (isSetPortfolioSearchBy1)
                ordersPortfolioBindableModel.SearchBy1 = ddlsearchBy1.SelectedItem != null ? (ddlsearchBy1.SelectedItem as ComboBoxItem).Content.ToString() : String.Empty;
        }

        /// <summary>
        /// SelectionChanged Event of SearchBy2 Dropdown
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        /// <returns>void</returns>
        private void ddlsearchBy2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox ddlsearchBy2 = sender as ComboBox;
            OrdersPortfolioBindableModel ordersPortfolioBindableModel = (OrdersPortfolioBindableModel)dfPortfolioSearch.CurrentItem;

            if (this.ApplicationContext.ViewState.PortFolioBindableModel == null)
                ordersPortfolioBindableModel.SearchBy2 = ddlsearchBy2.SelectedItem != null ? (ddlsearchBy2.SelectedItem as ComboBoxItem).Content.ToString() : String.Empty;

            if (isSetPortfolioSearchBy2)
                ordersPortfolioBindableModel.SearchBy2 = ddlsearchBy2.SelectedItem != null ? (ddlsearchBy2.SelectedItem as ComboBoxItem).Content.ToString() : String.Empty;
        }

        /// <summary>
        /// SelectionChanged Event of OrderDate Dropdown
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        private void ddlSearchOrderDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox ddlSearchOrderDate = sender as ComboBox;
            OrdersPortfolioBindableModel ordersPortfolioBindableModel = (OrdersPortfolioBindableModel)dfPortfolioSearch.CurrentItem;
            if (this.ApplicationContext.ViewState.PortFolioBindableModel == null)
            {
                ordersPortfolioBindableModel.TimeFrameby = ddlSearchOrderDate.SelectedItem != null ? (ddlSearchOrderDate.SelectedItem as ComboBoxItem).Content.ToString() : String.Empty;
            }

            dfPortfolioSearch.EditEnding += new EventHandler<DataFormEditEndingEventArgs>(dfPortfolioSearch_EditEnding);
        }

        /// <summary>
        /// SelectionChanged Event of CustomDate Dropdown
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        /// <returns>void</returns>
        private void ddlCustomDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox ddlCustomDate = sender as ComboBox;
            OrdersPortfolioBindableModel ordersPortfolioBindableModel = (OrdersPortfolioBindableModel)dfPortfolioSearch.CurrentItem;
            if (this.ApplicationContext.ViewState.PortFolioBindableModel == null)
            {
                ordersPortfolioBindableModel.TimeFramefor = ddlCustomDate.SelectedItem != null ? (ddlCustomDate.SelectedItem as ComboBoxItem).Tag.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Retry Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnRetry_Click(object sender, MenuItemClickedEventArgs args)
        {
            refreshIndicator.Status = RefreshableStatus.Waiting;
            DataGridCell cell = ((sender as MenuItem).Parent as MenuBar).Parent as DataGridCell;
            PortfolioResult rowItem = cell.DataContext as PortfolioResult;
            if (rowItem.Status.Equals(OrderDetailStatusConstants.ERROR))
            {
                RetryProxy retryProxy = new RetryProxy();
                RetryRequest retryRequest = new RetryRequest();
                retryRequest.RetryType = RETRY_TYPE;
                retryRequest.TrackingNo = rowItem.TrackingNo;
                retryRequest.TrackingItemNo = TRACKING_ITEMNO;
                retryProxy.Invoke(retryRequest, RetryServiceCompleted);
            }
        }

        /// <summary>
        /// Navigate To Differnt View(NameResult,Deatilsresult,OrderDetails) Depends on OrderStatus
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void lnkSearch_Click(object sender, RoutedEventArgs e)
        {
            DataGridCell cell = ((sender as HyperlinkButton).Parent as StackPanel).Parent as DataGridCell;
            LastListItem rowItem = cell.DataContext as LastListItem;
            string uri = string.Empty;

            if (rowItem != null)
            {
                if (rowItem.Status.Equals(OrderDetailStatusConstants.INPROCESS))
                {
                    if (rowItem.ActivityType == 15) uri = URIConstants.URL_DESKTOP + "" + NAMERESULTVIEW + "" + rowItem.TrackingNo.ToString();
                    else uri = URIConstants.URL_DESKTOP + "" + DETAILRESULTVIEW + "" + rowItem.TrackingNo.ToString() + "/" + SharedConstants.DEFAULT_TRACKINGITEM_NO + "/" + SharedConstants.DEFAULT_TASK_ID + "/" + "PortfolioFullView";  // Desktop/{View}/{TrackingNo}/{TrackingItemNo}/{TaskId}/{LastView}                    
                }

                if (rowItem.Status.Equals(OrderDetailStatusConstants.COMPLETED)) uri = URIConstants.URL_DESKTOP + "" + ORDERDETAILSVIEW + "" + rowItem.TrackingNo.ToString();
                HtmlPage.Window.Navigate(new Uri(uri, UriKind.Relative), URIConstants.CONST_self);

                ordersPortfolioBindableModel.LastPageIndex = pgrTop.CurrentPageIndex;
                this.ApplicationContext.ViewState.PortFolioBindableModel = ordersPortfolioBindableModel;
                FuseSetPortfolioSearchBy();
                ordersPortfolioBindableModel.OPBMServiceCallCompleted -= new RoutedEventHandler(OrdersPortfolioBindableModel_OPBMServiceCallCompleted);
            }
        }

        /// <summary>
        /// Navigate To Differnt View(NameResult,Deatilsresult,OrderDetails) Depends on OrderStatus
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void lnkSearchKey_Click(object sender, RoutedEventArgs e)
        {
            DataGridCell cell = ((sender as HyperlinkButton).Parent as StackPanel).Parent as DataGridCell;
            PortfolioResult rowItem = cell.DataContext as PortfolioResult;
            string uri = string.Empty;

            if (rowItem != null)
            {
                if (rowItem.Status.Equals(OrderDetailStatusConstants.INPROCESS))
                {
                    if (rowItem.ActivityType == 15) uri = URIConstants.URL_DESKTOP + "" + NAMERESULTVIEW + "" + rowItem.TrackingNo.ToString();
                    else uri = URIConstants.URL_DESKTOP + "" + DETAILRESULTVIEW + "" + rowItem.TrackingNo.ToString() + "/" + SharedConstants.DEFAULT_TRACKINGITEM_NO + "/" + SharedConstants.DEFAULT_TASK_ID + "/" + "" + PORTFOLIOFULLVIEW + "";  // Desktop/{View}/{TrackingNo}/{TrackingItemNo}/{TaskId}/{LastView}
                }

                if (rowItem.Status.Equals(OrderDetailStatusConstants.COMPLETED)) uri = URIConstants.URL_DESKTOP + "" + ORDERDETAILSVIEW + "" + rowItem.TrackingNo.ToString();
                HtmlPage.Window.Navigate(new Uri(uri, UriKind.Relative), URIConstants.CONST_self);

                ordersPortfolioBindableModel.LastPageIndex = pgrTop.CurrentPageIndex;
                this.ApplicationContext.ViewState.PortFolioBindableModel = ordersPortfolioBindableModel;
                FuseSetPortfolioSearchBy();
                ordersPortfolioBindableModel.OPBMServiceCallCompleted -= new RoutedEventHandler(OrdersPortfolioBindableModel_OPBMServiceCallCompleted);
            }
        }

        /// <summary>
        /// Loaded Event of Stackpanel stkStatus
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void stkStatus_Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel stkStaus = sender as StackPanel;

            if (stkStaus != null)
                stkStaus.Visibility = isStatusStackVisible ? Visibility.Visible : Visibility.Collapsed;

            if (this.ApplicationContext.ViewState.PortFolioBindableModel != null)
            {
                StackPanel stkStatus = dfPortfolioSearch.FindNameInContent("stkStatus") as StackPanel;
                ComboBox ddlStatus = dfPortfolioSearch.FindNameInContent("ddlStatus") as ComboBox;
                ordersPortfolioBindableModel = this.ApplicationContext.ViewState.PortFolioBindableModel as OrdersPortfolioBindableModel;

                if (stkStatus != null && stkStatus.Visibility != Visibility.Collapsed && isStatusStackVisible && ordersPortfolioBindableModel.Status != null)
                    ddlStatus.SelectedIndex = GetComboIndex(ddlStatus, ordersPortfolioBindableModel.Status);
                else
                    stkStatus.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dfPortfolioSearch_EditEnding(object sender, DataFormEditEndingEventArgs e)
        {
            if (e.EditAction == DataFormEditAction.Cancel)
            {
                if (vlsValidationSummary.Errors != null && vlsValidationSummary.Errors.Count == 0)
                {
                    e.Cancel = true;
                }
                return;
            }
        }

        #endregion

        #region Other Handlers

        /// <summary>
        /// Prepare PortFolioresult For Full View
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns> 
        private void PortfolioFullView_Loaded(object sender, RoutedEventArgs e)
        {
            if (ApplicationContext.ViewState != null)
            {
                ordersPortfolioBindableModel = new OrdersPortfolioBindableModel();
                lastItemsBindableModel = new LastItemsBindableModel();
                grdOrdersPortfolio.DataContext = ordersPortfolioBindableModel;

                if (!string.IsNullOrEmpty(this.ApplicationContext.ViewState.PortFolioStatus))
                {
                    TextBlock txbStatus = new TextBlock();
                    txbStatus.Text = this.ApplicationContext.ViewState.PortFolioStatus;
                    txbOpen_MouseLeftButtonDown(txbStatus, null);
                    ReBindGridOnBack();
                }
                else
                {
                    this.ApplicationContext.ViewState.PortFolioStatus = DEAFAULT_TEXT;
                }

                SetPreferenceLevel();
                lastItemsBindableModel.TeamId = Convert.ToInt64(ApplicationContext.ViewState.LoggedInUserContext.TeamID);
                lastItemsBindableModel.UserNo = ApplicationContext.ViewState.LoggedInUserContext.UserId.ToString();
                lastItemsBindableModel.LIBMServiceCallCompleted += new RoutedEventHandler(lastItemsBindableModel_LIBMServiceCallCompleted);
                ordersPortfolioBindableModel.OPBMServiceCallCompleted += new RoutedEventHandler(OrdersPortfolioBindableModel_OPBMServiceCallCompleted);
            }
        }

        /// <summary>
        /// Prepare PortFolioresult For Full View
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns> 
        private void dfPortfolioSearch_Loaded(object sender, RoutedEventArgs e)
        {
            dfPortfolioSearch.CurrentItem = new OrdersPortfolioBindableModel();
            SetPortfolioSearch();
        }

        /// <summary>
        /// Last Items Bind Service Call Completed
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns> 
        private void lastItemsBindableModel_LIBMServiceCallCompleted(object sender, RoutedEventArgs e)
        {
            dgLastItems.ItemsSource = lastItemsBindableModel.LastItemList;
        }

        /// <summary>
        /// Orders Portfolio Bindable Service Call Completed
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns> 
        private void OrdersPortfolioBindableModel_OPBMServiceCallCompleted(object sender, RoutedEventArgs e)
        {
            ordersPortfolioBindableModel.StatusEmpty = ordersPortfolioBindableModel.Status;
            grdOrdersPortfolio.DataContext = ordersPortfolioBindableModel;
            pgrTop.ListPageIndex = ordersPortfolioBindableModel.LastPageIndex != -1 ? ordersPortfolioBindableModel.LastPageIndex : 0;
            refreshIndicator.Status = RefreshableStatus.Completed;
        }

        /// <summary>
        /// Retry Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:RetryResponse</param>
        /// <returns>void</returns> 
        private void RetryServiceCompleted(DxProxy sender, DxCompleteEventArgs<RetryResponse> args)
        {
            PortfolioSearch();
        }

        #endregion

        #endregion
    }
}
