using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CT.SLABB.Data;
using CT.SLABB.UX.Controls;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;
using CTLS.Shared.UX.Controls;
using System.IO.IsolatedStorage;
using System.Collections.Generic;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class DesktopView : UserControl
    {
        #region Variables

        private const string CONST_STATE_COUNTY_MESSAGE = "State/County not supported by application.";
        private const string CONST_ORDERNO_ORDERITEMNO_MESSAGE = "Invalid Order #/Order Item #.";
        private const string CONST_MESSAGEBOX_MESSAGE = "Your search has been queued.\nPlease note the following details for reference:\n1. Tracking Number: ";
        private const string SEARCH_KEY = "\n2. Search Key: ";
        private const string LOCATION = "\n3. Location: ";
        private const string DOT = ".";
        private const string TITLE_PERFORMSEARCH = "Perform Search ";
        private const string DATE = "Date";
        private const string GRANTOR_GRANTEE_INDEX = "GRANTOR/GRANTEE INDEX";
        private const string LIEN_TYPE = "Lien Types";
        private const string CUSTOMERSPECIALISTSELECTED = "CustomerSpecialistSelected";
        private const string ORGANIZATION = "Org";
        private const string INDIVIDUAL = "Ind";
        private const int DEFAULT_VALUE = -1;        
        #endregion

        #region .ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public DesktopView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DesktopView_Loaded);
            (this.dsOrderItemListData.DataProvider as OrderSearchDataProvider).OnServiceCompleted += new EventHandler(OrderSearchDataProvider_ServiceCallCompleted);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loading Data
        /// </summary>
        private void LoadData()
        {
            refreshIndicator.Status = RefreshableStatus.Working;
            this.dsOrderItemListData.Refresh();
        }

        /// <summary>
        /// Validates Page
        /// </summary>
        /// <param name="full">Bool</param>
        /// <returns>Bool</returns>
        private bool Validate(bool full)
        {
            if (vlsValidationSummary.Errors != null) vlsValidationSummary.Errors.Clear();

            TextBox txtOrderNumber = dfCountySearch.FindNameInContent("txtOrderNumber") as TextBox;
            TextBox txtOrderItemNumber = dfCountySearch.FindNameInContent("txtOrderItemNumber") as TextBox;

            BindingExpression expOrderNumber = txtOrderNumber.GetBindingExpression(TextBox.TextProperty);
            BindingExpression expOrderItemNumber = txtOrderItemNumber.GetBindingExpression(TextBox.TextProperty);

            expOrderNumber.UpdateSource();
            expOrderItemNumber.UpdateSource();

            if (full)
            {
                DesktopBindableModel model = dfCountySearch.CurrentItem as DesktopBindableModel;

                DropDownList ddlSearchCriteria = dfCountySearch.FindNameInContent("ddlSearchCriteria") as DropDownList;
                DropDownList ddlLocation = dfCountySearch.FindNameInContent("ddlLocation") as DropDownList;
                DropDownList ddlCounty = dfCountySearch.FindNameInContent("ddlCounty") as DropDownList;
                CustomDatePicker dtpFromDate = dfCountySearch.FindNameInContent("dtpFromDate") as CustomDatePicker;
                CustomDatePicker dtpToDate = dfCountySearch.FindNameInContent("dtpToDate") as CustomDatePicker;
                TextBox txtCountyDirectSearchKey = dfCountySearch.FindNameInContent("txtCountyDirectSearchKey") as TextBox;

                model.TempFromDate = dtpFromDate.DateText;
                model.TempToDate = dtpToDate.DateText;
                
                TextBox txtLastName = dfCountySearch.FindNameInContent("txtLastName") as TextBox;
                TextBox txtFirstName = dfCountySearch.FindNameInContent("txtFirstName") as TextBox;
                TextBox txtMiddleName = dfCountySearch.FindNameInContent("txtMiddleName") as TextBox;

                ListBox lstLienType = dfCountySearch.FindNameInContent("lstLienTypes") as ListBox;
                ListBox lstCustomerSpecialist = dfCountySearch.FindNameInContent("lstCustomerSpecialist") as ListBox;

                BindingExpression expLocation = ddlLocation.GetBindingExpression(DropDownList.SelectedValueProperty);
                BindingExpression expCounty = ddlCounty.GetBindingExpression(DropDownList.SelectedValueProperty);
                BindingExpression expFromDate = dtpFromDate.GetBindingExpression(CustomDatePicker.TextProperty);
                BindingExpression expToDate = dtpToDate.GetBindingExpression(CustomDatePicker.TextProperty);
                BindingExpression expLastName = txtLastName.GetBindingExpression(TextBox.TextProperty);
                BindingExpression expFirstName = txtFirstName.GetBindingExpression(TextBox.TextProperty);
                BindingExpression expMiddleName = txtMiddleName.GetBindingExpression(TextBox.TextProperty);
                BindingExpression expCountyDirectSearchKey = txtCountyDirectSearchKey.GetBindingExpression(TextBox.TextProperty);

                BindingExpression expSearchCriteria = ddlSearchCriteria.GetBindingExpression(DropDownList.SelectedItemProperty);
                BindingExpression expLienType = lstLienType.GetBindingExpression(ListBox.TagProperty);
                BindingExpression expCustomerSpecialist = lstCustomerSpecialist.GetBindingExpression(ListBox.TagProperty);

                expLocation.UpdateSource();
                expCounty.UpdateSource();
                expFromDate.UpdateSource();
                expToDate.UpdateSource();

                if (ddlSearchCriteria.SelectedIndex == 0)
                {
                    if (lstLienType.Items.Count > 0)
                        expLienType.UpdateSource();
                    if (model.Type.Equals(ORGANIZATION))
                        expCountyDirectSearchKey.UpdateSource();
                    if (model.Type.Equals(INDIVIDUAL))
                    {
                        expMiddleName.UpdateSource();
                        expFirstName.UpdateSource();
                        expLastName.UpdateSource();
                    }
                }
                else
                {
                    expCountyDirectSearchKey.UpdateSource();
                }
                expCustomerSpecialist.UpdateSource();
            }
            return vlsValidationSummary.Errors.Count > 0 ? false : true;
        }

        /// <summary>
        /// Validates Page
        /// </summary>
        /// <param name="full">Bool</param>
        /// <returns>Bool</returns>
        private bool ValidateOnRadio(bool full)
        {
            if (vlsValidationSummary.Errors != null)
                vlsValidationSummary.Errors.Clear();

            TextBox txtOrderNumber = dfCountySearch.FindNameInContent("txtOrderNumber") as TextBox;
            TextBox txtOrderItemNumber = dfCountySearch.FindNameInContent("txtOrderItemNumber") as TextBox;

            BindingExpression expOrderNumber = txtOrderNumber.GetBindingExpression(TextBox.TextProperty);
            BindingExpression expOrderItemNumber = txtOrderItemNumber.GetBindingExpression(TextBox.TextProperty);

            //expOrderNumber.UpdateSource();
            //expOrderItemNumber.UpdateSource();

            if (full)
            {
                DesktopBindableModel model = dfCountySearch.CurrentItem as DesktopBindableModel;

                DropDownList ddlSearchCriteria = dfCountySearch.FindNameInContent("ddlSearchCriteria") as DropDownList;
                DropDownList ddlLocation = dfCountySearch.FindNameInContent("ddlLocation") as DropDownList;
                DropDownList ddlCounty = dfCountySearch.FindNameInContent("ddlCounty") as DropDownList;
                CustomDatePicker dtpFromDate = dfCountySearch.FindNameInContent("dtpFromDate") as CustomDatePicker;
                CustomDatePicker dtpToDate = dfCountySearch.FindNameInContent("dtpToDate") as CustomDatePicker;
                TextBox txtCountyDirectSearchKey = dfCountySearch.FindNameInContent("txtCountyDirectSearchKey") as TextBox;

                model.TempToDate = dtpToDate.Text;

                TextBox txtLastName = dfCountySearch.FindNameInContent("txtLastName") as TextBox;
                TextBox txtFirstName = dfCountySearch.FindNameInContent("txtFirstName") as TextBox;
                TextBox txtMiddleName = dfCountySearch.FindNameInContent("txtMiddleName") as TextBox;

                ListBox lstLienType = dfCountySearch.FindNameInContent("lstLienTypes") as ListBox;
                ListBox lstCustomerSpecialist = dfCountySearch.FindNameInContent("lstCustomerSpecialist") as ListBox;

                BindingExpression expLocation = ddlLocation.GetBindingExpression(DropDownList.SelectedValueProperty);
                BindingExpression expCounty = ddlCounty.GetBindingExpression(DropDownList.SelectedValueProperty);
                BindingExpression expFromDate = dtpFromDate.GetBindingExpression(CustomDatePicker.TextProperty);
                BindingExpression expToDate = dtpToDate.GetBindingExpression(CustomDatePicker.TextProperty);
                BindingExpression expLastName = txtLastName.GetBindingExpression(TextBox.TextProperty);
                BindingExpression expFirstName = txtFirstName.GetBindingExpression(TextBox.TextProperty);
                BindingExpression expMiddleName = txtMiddleName.GetBindingExpression(TextBox.TextProperty);
                BindingExpression expCountyDirectSearchKey = txtCountyDirectSearchKey.GetBindingExpression(TextBox.TextProperty);

                BindingExpression expSearchCriteria = ddlSearchCriteria.GetBindingExpression(DropDownList.SelectedItemProperty);
                BindingExpression expLienType = lstLienType.GetBindingExpression(ListBox.TagProperty);
                BindingExpression expCustomerSpecialist = lstCustomerSpecialist.GetBindingExpression(ListBox.TagProperty);

                //expLocation.UpdateSource();
                //expCounty.UpdateSource();
                //expFromDate.UpdateSource();
                //expToDate.UpdateSource();

                if (ddlSearchCriteria.SelectedIndex == 0)
                {
                    if (lstLienType.Items.Count > 0)
                        //expLienType.UpdateSource();

                        if (model.Type.Equals(ORGANIZATION))
                            //expCountyDirectSearchKey.UpdateSource();

                            if (model.Type.Equals(INDIVIDUAL))
                            {
                                expMiddleName.UpdateSource();
                                expFirstName.UpdateSource();
                                expLastName.UpdateSource();
                            }
                }
                else
                {
                    expCountyDirectSearchKey.UpdateSource();
                }
                //expCustomerSpecialist.UpdateSource();
            }
            return vlsValidationSummary.Errors.Count > 0 ? false : true;
        }

        /// <summary>
        /// Prepares Page Level Information/DTO
        /// </summary>
        /// <returns>SearchCriteria</returns>
        private SearchCriteria GetSearchCriteriaDTO()
        {
            TextBox txtorderNo = dfCountySearch.FindNameInContent("txtOrderNumber") as TextBox;
            TextBox txtorderitemNo = dfCountySearch.FindNameInContent("txtOrderItemNumber") as TextBox;
            TextBox txtLastName = dfCountySearch.FindNameInContent("txtLastName") as TextBox;
            TextBox txtFirstName = dfCountySearch.FindNameInContent("txtFirstName") as TextBox;
            TextBox txtMiddleName = dfCountySearch.FindNameInContent("txtMiddleName") as TextBox;
            TextBox txtCountyDirectSearchKey = dfCountySearch.FindNameInContent("txtCountyDirectSearchKey") as TextBox;
            DropDownList ddlCounty = dfCountySearch.FindNameInContent("ddlCounty") as DropDownList;
            DropDownList ddlLocation = dfCountySearch.FindNameInContent("ddlLocation") as DropDownList;
            DropDownList ddlSearchCriteria = dfCountySearch.FindNameInContent("ddlSearchCriteria") as DropDownList;
            DropDownList ddlTeam = dfCountySearch.FindNameInContent("ddlTeam") as DropDownList;
            CustomDatePicker dtForm = dfCountySearch.FindNameInContent("dtpFromDate") as CustomDatePicker;
            CustomDatePicker dtTo = dfCountySearch.FindNameInContent("dtpToDate") as CustomDatePicker;
            ListBox lstLienType = dfCountySearch.FindNameInContent("lstLienTypes") as ListBox;
            ListBox lstCustomerSpecialist = dfCountySearch.FindNameInContent("lstCustomerSpecialist") as ListBox;
            RadioButton rdoIndividual = dfCountySearch.FindNameInContent("rdoIndividual") as RadioButton;
            RadioButton rdoOrganization = dfCountySearch.FindNameInContent("rdoOrganization") as RadioButton;

            SearchCriteria searchCriteria = new SearchCriteria();

            searchCriteria.OrderNo = Convert.ToInt64(txtorderNo.Text);
            searchCriteria.OrderItemNo = Convert.ToInt64(txtorderitemNo.Text);

            searchCriteria.StateName = (ddlLocation.SelectedItem as LocationListItem).LocationName;
            searchCriteria.StateCode = ddlLocation.SelectedValue.ToString();
            searchCriteria.CountyName = (ddlCounty.SelectedItem as CountyListItem).CountyName;
            searchCriteria.CountyCode = Convert.ToInt32(ddlCounty.SelectedValue);
            searchCriteria.SearchCriteriaType = (ddlSearchCriteria.SelectedItem as ComboBoxItem).Content.ToString();

            searchCriteria.FromDate = string.Empty;
            searchCriteria.ToDate = string.Empty;

            if (searchCriteria.SearchCriteriaType.Equals(LIEN_TYPE))
            {
                searchCriteria.FromDate = dtForm.DateText;
                searchCriteria.ToDate = dtTo.DateText;
            }

            searchCriteria.LienType = GetLienType();

            if (rdoOrganization.IsChecked == true)
            {
                searchCriteria.SearchType = ORGANIZATION;
                searchCriteria.PartyName = txtCountyDirectSearchKey.Text.Trim();
            }
            else
            {
                searchCriteria.SearchType = INDIVIDUAL;
                searchCriteria.FirstName = txtFirstName.Text.Trim();
                searchCriteria.LastName = txtLastName.Text.Trim();
                searchCriteria.MiddleName = txtMiddleName.Text.Trim();
                searchCriteria.PartyName = txtLastName.Text + " " + txtFirstName.Text + " " + txtMiddleName.Text;
            }

            searchCriteria.SearchKey = searchCriteria.PartyName;
            searchCriteria.AssignedTo = (lstCustomerSpecialist.SelectedItem as CommonCustomerSpecialistListItem).CustomerSpecialistId;
            searchCriteria.CreatedBy = ApplicationContext.ViewState.LoggedInUserContext.UserId;

            searchCriteria.TrackingItemNo = DEFAULT_VALUE;
            searchCriteria.TrackingNo = DEFAULT_VALUE;

            return searchCriteria;
        }

        /// <summary>
        /// Builds Lien Type
        /// </summary>
        /// <returns>string</returns>
        private string GetLienType()
        {
            DropDownList ddlSearchCriteria = dfCountySearch.FindNameInContent("ddlSearchCriteria") as DropDownList;
            ListBox lstLienType = dfCountySearch.FindNameInContent("lstLienTypes") as ListBox;
            string lienType = string.Empty;

            if (ddlSearchCriteria.SelectedIndex == 0)
            {
                if (lstLienType.SelectedItem != null) lienType = (lstLienType.SelectedItem as CommonLienTypeListItem).LienType;

                if (lstLienType.SelectedItems.Count > 1)
                {
                    lienType = string.Empty;
                    foreach (CommonLienTypeListItem lstitem in lstLienType.SelectedItems)
                        lienType += lstitem.LienType + ", ";

                    lienType = lienType.Substring(0, lienType.LastIndexOf(","));
                }

                //If 'GRANTOR/GRANTEE INDEX' is selected then ignore all other selected lien types
                if (lienType.ToUpper().Trim().Contains(GRANTOR_GRANTEE_INDEX))
                {
                    lienType = GRANTOR_GRANTEE_INDEX;
                }
            }

            return lienType;
        }

        /// <summary>
        /// Get Team Dropdown Selected Index
        /// </summary>
        /// <param name="ddlTeam">DropDownList</param>
        /// <returns>int</returns>
        private int GetTeamIndex()
        {
            DropDownList ddlTeam = dfCountySearch.FindNameInContent("ddlTeam") as DropDownList;
            int count = ddlTeam.Items.Count;
            for (int i = 0; i < count; i++)
            {
                CommonTeamListItem item = (CommonTeamListItem)ddlTeam.Items[i];
                if (item.TeamId == ApplicationContext.ViewState.LoggedInUserContext.TeamID)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Get CustomerSpecialist Dropdown Selected Index
        /// </summary>
        /// <param name="ddlTeam">ListBox</param>
        /// <returns>int</returns>
        private int GetCustomerSpecialistIndex()
        {
            ListBox lstCustomerSpecialist = dfCountySearch.FindNameInContent("lstCustomerSpecialist") as ListBox;
            int count = lstCustomerSpecialist.Items.Count;
            for (int i = 0; i < count; i++)
            {
                CommonCustomerSpecialistListItem item = (CommonCustomerSpecialistListItem)lstCustomerSpecialist.Items[i];
                if (item.CustomerSpecialistId.ToString() == ApplicationContext.ViewState.LoggedInUserContext.UserId)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Perform Navigation To DetailResultView
        /// </summary>
        /// <param name="trackingNo">Long</param>
        /// <param name="trackingItemNo">Int</param>
        /// <returns>oid</returns>
        private void NavigateToDetailResultView(long trackingNo, int trackingItemNo)
        {
            DropDownList ddlSearchCriteria = dfCountySearch.FindNameInContent("ddlSearchCriteria") as DropDownList;
            string documentNo = (ddlSearchCriteria.SelectedItem as ComboBoxItem).Content.ToString();

            if (documentNo.Equals(SharedConstants.DOCUMENT_NUMBER))
            {
                if (this.countySearchVMC.ViewState.DetailResultBindableModel == null)
                    this.countySearchVMC.ViewState.DetailResultBindableModel = new DetailResultBindableModel();

                ViewDetailsResponse viewDetailsResponse = new ViewDetailsResponse();
                viewDetailsResponse.TrackingNo = trackingNo;
                viewDetailsResponse.TrackingItemNo = trackingItemNo;
                viewDetailsResponse.TaskId = SharedConstants.DEFAULT_TASK_ID;
                this.countySearchVMC.ViewState.DetailResultBindableModel.ViewDetailsResponse = viewDetailsResponse;

                UxViewFrame viewFrame = null;
                if (this.FindMyViewFrame(out viewFrame))
                {
                    viewFrame.Navigate(ViewConstants.DETAILRESULTSVIEW, null);
                }
            }
        }

        /// <summary>
        /// Clears Page 
        /// </summary>
        /// <returns>void</returns>
        private void ClearSearchCriteria()
        {
            ListBox lstLienType = dfCountySearch.FindNameInContent("lstLienTypes") as ListBox;
            if (lstLienType != null)
            {
                lstLienType.Tag = string.Empty;
                lstLienType.SelectedItem = null;
            }

            TextBox txtOrderNumber = dfCountySearch.FindNameInContent("txtOrderNumber") as TextBox;
            if (txtOrderNumber != null)
            {
                txtOrderNumber.Text = string.Empty;
                txtOrderNumber.Focus();
            }

            TextBox txtOrderItemNumber = dfCountySearch.FindNameInContent("txtOrderItemNumber") as TextBox;
            if (txtOrderItemNumber != null)
                txtOrderItemNumber.Text = string.Empty;

            CustomDatePicker dtpFromDate = dfCountySearch.FindNameInContent("dtpFromDate") as CustomDatePicker;
            if (dtpFromDate != null)
                dtpFromDate.Text = string.Empty;

            CustomDatePicker dtpToDate = dfCountySearch.FindNameInContent("dtpToDate") as CustomDatePicker;
            if (dtpToDate != null)
                dtpToDate.Text = string.Empty;

            TextBox txtCountyDirectSearchKey = dfCountySearch.FindNameInContent("txtCountyDirectSearchKey") as TextBox;
            if (txtCountyDirectSearchKey != null)
                txtCountyDirectSearchKey.Text = string.Empty;

            TextBox txtLastName = dfCountySearch.FindNameInContent("txtLastName") as TextBox;
            if (txtLastName != null)
                txtLastName.Text = string.Empty;

            TextBox txtFirstName = dfCountySearch.FindNameInContent("txtFirstName") as TextBox;
            if (txtFirstName != null)
                txtFirstName.Text = string.Empty;

            TextBox txtMiddleName = dfCountySearch.FindNameInContent("txtMiddleName") as TextBox;
            if (txtMiddleName != null)
                txtMiddleName.Text = string.Empty;

            DropDownList ddlLocation = dfCountySearch.FindNameInContent("ddlLocation") as DropDownList;
            if (ddlLocation != null && ddlLocation.Items.Count > 0)
                ddlLocation.SelectedIndex = 0;

            DropDownList ddlCounty = dfCountySearch.FindNameInContent("ddlCounty") as DropDownList;
            if (ddlCounty != null && ddlCounty.Items.Count > 0)
                ddlCounty.SelectedIndex = 0;
        }

        /// <summary>
        /// return Index for County When Validate OrderNumber.,OrderItem Number
        /// </summary>
        /// <param name="ddlCounty">DropDownList</param>
        /// <param name="jurisdiction">Jurisdiction</param>
        /// <returns>int</returns>
        private static int OnValidate_RetriveIndex(DropDownList ddlCounty, Jurisdiction jurisdiction)
        {
            int countyindex = 0;
            foreach (CountyListItem item in ddlCounty.ItemsSource)
            {
                if (item.CountyCode.ToString().Equals(jurisdiction.CountyCode))
                    break;
                countyindex++;
            }
            return countyindex;
        }

        /// <summary>
        /// Set Scroll Position On Top
        /// </summary>        
        /// <returns>void</returns>
        private void SetScrollPostionAtTop()
        {
            UxPage page;
            this.FindMyPage(out page);
            (page as DesktopPage).ScrollToPoint = 0;
        }

        /// <summary>
        /// Make Isolated storage Setting if it is initiazed
        /// </summary>
        private void CreateIsolatedStorage()
        {
            IsolatedStorageSettings isoStorage = IsolatedStorageSettings.ApplicationSettings;

            List<CountySearchInstructionList> searchInstructions = null;
            List<CopyCostInstructionList> copyCostInstructions = null;
            List<string> alerts = null;
            List<CommonLienTypeListItem> lienTypelist = null;
            List<CommonTeamListItem> teams = null;
            List<CommonCustomerSpecialistListItem> customerSpecialists = null;
            List<LocationListItem> locationlist = null;
            List<CountyListItem> countylist = null;

            if (!isoStorage.Contains(SharedConstants.CACHED_SEARCH_INSTRUCTIONS_LIST))
            {
                searchInstructions = ApplicationContext.ViewState.SearchInstructions;
                searchInstructions = searchInstructions != null ? searchInstructions : new List<CountySearchInstructionList>();
                IsolatedStorageSettings.ApplicationSettings.Add(SharedConstants.CACHED_SEARCH_INSTRUCTIONS_LIST, searchInstructions);
            }

            if (!isoStorage.Contains(SharedConstants.CACHED_COPYCOST_LIST))
            {
                copyCostInstructions = ApplicationContext.ViewState.CopyCostInstructions;
                copyCostInstructions = copyCostInstructions != null ? copyCostInstructions : new List<CopyCostInstructionList>();
                isoStorage.Add(SharedConstants.CACHED_COPYCOST_LIST, copyCostInstructions);
            }

            if (!isoStorage.Contains(SharedConstants.CACHED_ALERTS_LIST))
            {
                alerts = ApplicationContext.ViewState.Alerts;
                alerts = alerts != null ? alerts : new List<string>();
                isoStorage.Add(SharedConstants.CACHED_ALERTS_LIST, alerts);
            }            

            if (!isoStorage.Contains(SharedConstants.CACHED_LIENTYPE_LIST))
            {
                lienTypelist = ApplicationContext.ViewState.LienTypeList;
                lienTypelist = lienTypelist != null ? lienTypelist : new List<CommonLienTypeListItem>();
                isoStorage.Add(SharedConstants.CACHED_LIENTYPE_LIST, lienTypelist);
            }
            if (!isoStorage.Contains(SharedConstants.CACHED_CUSTOMERSPECIALIST_LIST))
            {
                customerSpecialists = ApplicationContext.ViewState.CustomerSpecialistList;
                customerSpecialists = customerSpecialists != null ? customerSpecialists : new List<CommonCustomerSpecialistListItem>();
                isoStorage.Add(SharedConstants.CACHED_CUSTOMERSPECIALIST_LIST, customerSpecialists);
            }
            if (!isoStorage.Contains(SharedConstants.CACHED_TEAM_LIST))
            {
                teams = ApplicationContext.ViewState.TeamList;
                teams = teams != null ? teams : new List<CommonTeamListItem>();
                isoStorage.Add(SharedConstants.CACHED_TEAM_LIST, teams);
            }
            if (!isoStorage.Contains(SharedConstants.CACHED_LOCATION_LIST))
            {
                locationlist = ApplicationContext.ViewState.LocationList;
                locationlist = locationlist != null ? locationlist : new List<LocationListItem>();
                locationlist.Insert(0, new LocationListItem { LocationId = SharedConstants.SELECT_ONE_KEY, LocationName = SharedConstants.SELECT_ONE_VALUE });
                isoStorage.Add(SharedConstants.CACHED_LOCATION_LIST, locationlist);
            }

            if (!isoStorage.Contains(SharedConstants.CACHED_COUNTY_LIST))
            {
                countylist = ApplicationContext.ViewState.CountyList;
                countylist = countylist != null ? countylist : new List<CountyListItem>();
                countylist.Insert(0, new CountyListItem { CountyCode = Convert.ToInt16(SharedConstants.SELECT_ONE_KEY), CountyName = SharedConstants.SELECT_ONE_VALUE, StateCode = SharedConstants.SELECT_ONE_KEY });
                isoStorage.Add(SharedConstants.CACHED_COUNTY_LIST, countylist);
            }           
        }

        #endregion

        #region Event Handlers

        #region Control Event Handlers

        /// <summary>
        /// Loading Event of View
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void DesktopView_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = dfCountySearch.FindNameInContent("lblOrderValidationMsg") as TextBlock;
            tb.Visibility = Visibility.Collapsed;
            CreateIsolatedStorage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataForm_Loaded(object sender, RoutedEventArgs e)
        {
            DesktopBindableModel desktopBindableModel = new DesktopBindableModel();
            desktopBindableModel.ValidationCompleted += DesktopView_ValidationCompleted;
            desktopBindableModel.SearchCompleted += DesktopView_SearchCompleted;

            dfCountySearch.CurrentItem = desktopBindableModel;
        }

        /// <summary>
        /// SelectionChanged Event of Location Dropdown
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        /// <returns>void</returns>
        private void ddlLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DropDownList ddlLocation = sender as DropDownList;
            CountyList countyList = dfCountySearch.FindNameInContent("CountyList") as CountyList;

            if (ddlLocation.SelectedValue != null && countyList != null)
            {
                countyList.Location = ddlLocation.SelectedValue.ToString();
                ListBox lstLienType = dfCountySearch.FindNameInContent("lstLienTypes") as ListBox;
                if (lstLienType != null)
                    lstLienType.Tag = string.Empty;

            }
        }

        /// <summary>
        /// SelectionChanged Event of County Dropdown
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        /// <returns>void</returns>
        private void ddlCounty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DropDownList ddlCounty = sender as DropDownList;
            LienTypeList lienTypeList = dfCountySearch.FindNameInContent("LienTypeList") as LienTypeList;
            CountyList countyList = dfCountySearch.FindNameInContent("CountyList") as CountyList;

            if (ddlCounty.SelectedValue != null)
            {
                lienTypeList.StateCode = countyList.Location;
                lienTypeList.CountyCode = Convert.ToInt16(ddlCounty.SelectedValue);

                CountySearchInstructionsList.StateCode = countyList.Location;
                CountySearchInstructionsList.CountyCode = Convert.ToInt16(ddlCounty.SelectedValue);

                CopyCostInstrunctionList.StateCode = countyList.Location;
                CopyCostInstrunctionList.CountyCode = Convert.ToInt16(ddlCounty.SelectedValue);
            }
        }

        /// <summary>
        /// SelectionChanged Event of Team Dropdown
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        /// <returns>void</returns>
        private void ddlTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DropDownList ddlTeam = sender as DropDownList;
            CustomerSpecialistList customerSpecialistList = dfCountySearch.FindNameInContent("CustomerSpecialistList") as CustomerSpecialistList;
            if (ddlTeam.SelectedValue != null && customerSpecialistList != null)
            {
                customerSpecialistList.TeamId = Convert.ToInt32(ddlTeam.SelectedValue);
            }
        }

        /// <summary>
        /// SelectionChanged Event of SearchCriteria Dropdown
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        /// <returns>void</returns>
        private void ddlSearchCriteria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DropDownList ddlSearchCriteria = sender as DropDownList;
            int index = ddlSearchCriteria.SelectedIndex;

            DataField dfldLienTypes = dfCountySearch.FindNameInContent("dfldLienTypes") as DataField;
            StackPanel pnlRadios = dfCountySearch.FindNameInContent("pnlRadios") as StackPanel;
            DataField dfldCountyDirectSearchKey = dfCountySearch.FindNameInContent("dfldCountyDirectSearchKey") as DataField;
            Grid grdNames = dfCountySearch.FindNameInContent("grdNames") as Grid;

            if (index == 0)
            {
                if (dfldLienTypes != null) dfldLienTypes.Visibility = Visibility.Visible;
                if (pnlRadios != null) pnlRadios.Visibility = Visibility.Visible;
                if (dfldCountyDirectSearchKey != null) dfldCountyDirectSearchKey.Visibility = Visibility.Visible;
                if (grdNames != null) grdNames.Visibility = Visibility.Collapsed;
            }
            else if (index == 1)
            {
                if (dfldLienTypes != null) dfldLienTypes.Visibility = Visibility.Collapsed;
                if (pnlRadios != null) pnlRadios.Visibility = Visibility.Collapsed;
                if (grdNames != null) grdNames.Visibility = Visibility.Collapsed;
                if (dfldCountyDirectSearchKey != null) dfldCountyDirectSearchKey.Visibility = Visibility.Visible;
            }
        }
        
        /// <summary>
        /// Validate Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnValidate_Click(object sender, MenuItemClickedEventArgs args)
        {
            try
            {
                long ordrno;
                int ordritemno;

                TextBlock tb = dfCountySearch.FindNameInContent("lblOrderValidationMsg") as TextBlock;
                tb.Visibility = Visibility.Collapsed;

                if (Validate(false))
                {
                    TextBox txtorderNo = dfCountySearch.FindNameInContent("txtOrderNumber") as TextBox;
                    TextBox txtorderitemNo = dfCountySearch.FindNameInContent("txtOrderItemNumber") as TextBox;
                    Int64.TryParse(txtorderNo.Text.Trim(), out ordrno);
                    Int32.TryParse(txtorderitemNo.Text.Trim(), out ordritemno);
                    DesktopBindableModel desktopBindableModel = dfCountySearch.CurrentItem as DesktopBindableModel;
                    desktopBindableModel.Validate(ordrno, ordritemno);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, "btnValidate_Click", this.GetType(), ex);
            }
        }

        /// <summary>
        /// Search Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MenuItemClickedEventArgs</param>
        /// <returns>void</returns>
        private void btnSearch_Click(object sender, MenuItemClickedEventArgs args)
        {
            try
            {
                TextBlock tb = dfCountySearch.FindNameInContent("lblOrderValidationMsg") as TextBlock;
                tb.Visibility = Visibility.Collapsed;

                if (Validate(true))
                {
                    DesktopBindableModel desktopBindableModel = dfCountySearch.CurrentItem as DesktopBindableModel;
                    if (desktopBindableModel != null)
                    {
                        desktopBindableModel.SearchCriteria = GetSearchCriteriaDTO();
                        desktopBindableModel.PerformSearch();
                    }
                }
                SetScrollPostionAtTop();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, "btnSearch_Click", this.GetType(), ex);
            }
        }

        /// <summary>
        /// Refresh Button Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void lnkRefresh_Click(object sender, RoutedEventArgs e)
        {
            refreshIndicator.Status = RefreshableStatus.Working;
            this.dsOrderItemListData.Refresh();
        }

        /// <summary>
        /// Summary Link Click Event Handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MouseButtonEventArgs</param>
        /// <returns>void</returns>
        private void lnkSummary_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TextBlock summaryTextblock = sender as TextBlock;
                if (summaryTextblock != null && summaryTextblock.Tag != null)
                {
                    if (this.countySearchVMC.ViewState.NameResultBindableModel == null)
                        this.countySearchVMC.ViewState.NameResultBindableModel = new NameResultBindableModel();
                    this.countySearchVMC.ViewState.NameResultBindableModel.BeginRefresh((long)summaryTextblock.Tag);
                }
                UxViewFrame viewFrame = null;
                if (this.FindMyViewFrame(out viewFrame))
                {
                    this.countySearchVMC.ViewState.IsNew = true;
                    viewFrame.Navigate(ViewConstants.NAMERESULTVIEW, null);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, "lnkSummary_MouseLeftButtonDown", this.GetType(), ex);
            }
        }

        /// <summary>
        /// SelectionChanged Event of Lien Types List Handle
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        /// <returns>void</returns>
        private void lstLienTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lstLienType = dfCountySearch.FindNameInContent("lstLienTypes") as ListBox;
            lstLienType.Tag = "LienTypeSelected";
        }

        /// <summary>
        /// SelectionChanged Event of CustomerSpecialist List Handle
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        /// <returns>void</returns>
        private void lstCustomerSpecialist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DesktopBindableModel desktopBindableModel = (DesktopBindableModel)dfCountySearch.CurrentItem;
            desktopBindableModel.CustomerSpecialistSelected = CUSTOMERSPECIALISTSELECTED;

            ListBox lstCustomerSpecialist = dfCountySearch.FindNameInContent("lstCustomerSpecialist") as ListBox;
            if (lstCustomerSpecialist.SelectedItem == null) lstCustomerSpecialist.Tag = null;
        }

        /// <summary>
        /// Checked event ofOrganization and Individual Radio buttons
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void rdoType_Checked(object sender, RoutedEventArgs e)
        {
            TextBox txtCountyDirectSearchKey = null;
            TextBox txtLastName = null;
            TextBox txtFirstName = null;
            TextBox txtMiddleName = null;

            RadioButton radioButton = sender as RadioButton;
            if (radioButton == null)
                return;

            if (radioButton.IsChecked.HasValue && radioButton.IsChecked.Value)
            {
                txtLastName = dfCountySearch.FindNameInContent("txtLastName") as TextBox;
                txtFirstName = dfCountySearch.FindNameInContent("txtFirstName") as TextBox;
                txtMiddleName = dfCountySearch.FindNameInContent("txtMiddleName") as TextBox;
                txtCountyDirectSearchKey = dfCountySearch.FindNameInContent("txtCountyDirectSearchKey") as TextBox;

                if (txtLastName != null)
                    txtLastName.Text = string.Empty;
                if (txtFirstName != null)
                    txtFirstName.Text = string.Empty;
                if (txtMiddleName != null)
                    txtMiddleName.Text = string.Empty;
                if (txtCountyDirectSearchKey != null)
                    txtCountyDirectSearchKey.Text = string.Empty;
            }

            // Fuse Validation
            if (radioButton.Content.Equals("Individual"))
            {
                BindingExpression expCountyDirectSearchKey = txtCountyDirectSearchKey.GetBindingExpression(TextBox.TextProperty);
                txtCountyDirectSearchKey.Text = "A";
                expCountyDirectSearchKey.UpdateSource();
                txtCountyDirectSearchKey.Text = "";
            }
            else if (radioButton.Content.Equals("Organization"))
            {
                BindingExpression expLastName = txtLastName.GetBindingExpression(TextBox.TextProperty);
                BindingExpression expFirstName = txtFirstName.GetBindingExpression(TextBox.TextProperty);
                BindingExpression expMiddleName = txtMiddleName.GetBindingExpression(TextBox.TextProperty);

                txtMiddleName.Text = "M";
                txtFirstName.Text = "F";
                txtLastName.Text = "L";

                expMiddleName.UpdateSource();
                expFirstName.UpdateSource();
                expLastName.UpdateSource();

                txtLastName.Text = "";
                txtFirstName.Text = "";
                txtMiddleName.Text = "";
            }
        }

        #endregion

        #region Service CallBack Handlers

        /// <summary>
        /// Getting Dropdowns and List's selected Value
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        /// <returns>void</returns>
        protected void OrderSearchDataProvider_ServiceCallCompleted(object sender, EventArgs e)
        {
            refreshIndicator.Status = RefreshableStatus.Completed;

            DropDownList ddlTeam = dfCountySearch.FindNameInContent("ddlTeam") as DropDownList;
            CustomerSpecialistList customerSpecialistList = dfCountySearch.FindNameInContent("CustomerSpecialistList") as CustomerSpecialistList;
            ListBox lstCustomerSpecialist = dfCountySearch.FindNameInContent("lstCustomerSpecialist") as ListBox;

            customerSpecialistList.TeamId = Convert.ToInt32(ddlTeam.SelectedValue);
            ddlTeam.SelectedIndex = GetTeamIndex();
            lstCustomerSpecialist.SelectedIndex = GetCustomerSpecialistIndex();
        }

        /// <summary>
        /// ValidationCompleted event of a DesktopBindableModel to validates Order #/Order Item #.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        void DesktopView_ValidationCompleted(object sender, RoutedEventArgs e)
        {
            DropDownList ddlCounty = dfCountySearch.FindNameInContent("ddlCounty") as DropDownList;
            DropDownList ddlLocation = dfCountySearch.FindNameInContent("ddlLocation") as DropDownList;
            TextBlock tb = dfCountySearch.FindNameInContent("lblOrderValidationMsg") as TextBlock;

            DesktopBindableModel desktopBindableModel = (dfCountySearch.CurrentItem as DesktopBindableModel);

            if (desktopBindableModel != null &&
                desktopBindableModel.Jurisdiction != null &&
                desktopBindableModel.Jurisdiction.SuccessFlag)
            {
                Jurisdiction jurisdiction = desktopBindableModel.Jurisdiction;
                LocationList l = dfCountySearch.FindNameInContent("LocationList") as LocationList;
                CountyList countyList = dfCountySearch.FindNameInContent("CountyList") as CountyList;

                // check if state and county exists in list.
                int locationCount = l.Data.Count(x => x.LocationId == jurisdiction.StateCode);
                int countyCount = countyList.Data.Where(x => x.StateCode == jurisdiction.StateCode).
                                  Count(y => y.CountyCode == Convert.ToInt16(jurisdiction.CountyCode));
                if (locationCount != 0 && countyCount != 0)
                {
                    ddlLocation.SelectedValue = jurisdiction.StateCode;
                    ddlCounty.SelectedValue = jurisdiction.CountyCode;
                    ddlCounty.SelectedIndex = OnValidate_RetriveIndex(ddlCounty, jurisdiction);
                }
                else
                {
                    tb.Text = CONST_STATE_COUNTY_MESSAGE;
                    tb.Visibility = Visibility.Visible;
                }
            }
            else
            {
                tb.Text = CONST_ORDERNO_ORDERITEMNO_MESSAGE;
                tb.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// SearchCompleted event of a DesktopBindableModel
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        void DesktopView_SearchCompleted(object sender, RoutedEventArgs e)
        {
            DesktopBindableModel desktopBindableModel = dfCountySearch.CurrentItem as DesktopBindableModel;
            if (desktopBindableModel != null && desktopBindableModel.SearchCriteria != null)
            {
                if (desktopBindableModel.SearchCriteria.SearchCriteriaType == LIEN_TYPE)
                {
                    //string message = CONST_MESSAGEBOX_MESSAGE"Your search has been queued.\nPlease note the following details for reference:\n1. Tracking Number: " + desktopBindableModel.TrackingNo.ToString();
                    string message = CONST_MESSAGEBOX_MESSAGE + desktopBindableModel.TrackingNo.ToString();
                    message += SEARCH_KEY + desktopBindableModel.SearchCriteria.SearchKey;
                    message += LOCATION + desktopBindableModel.SearchCriteria.StateName;
                    message += DOT + desktopBindableModel.SearchCriteria.CountyName;
                    MessageBox.Show(message, TITLE_PERFORMSEARCH, MessageBoxButton.OK);
                    ClearSearchCriteria();
                }
                else
                    NavigateToDetailResultView(desktopBindableModel.TrackingNo, desktopBindableModel.TrackingItemNo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerSpecialistList_CustomerSpecialistListCompleted(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        #endregion       

        #endregion
    }
}

