using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using CT.SLABB.DX;
using CT.SLABB.Utils;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public class OrdersPortfolioBindableModel : Bindable, IEditableObject
    {
        #region Variables

        private string __searchBy1;
        private string __searchBy2;
        private string __searchfor1;
        private string __searchfor2;
        private string __timeFrameby;
        private string __timeFramefor;
        private string __fromDate;
        private string __toDate;
        private string __status = "Default Text";
        private string __statusEmptyGrid = "Default Text";
        private string __teamId;
        private string __userNo;
        private string __preferenceLevel;
        private List<PortfolioResult> __portfolioResultList;
        private PagedCollectionView __PortfolioResulPagedData;

        public event RoutedEventHandler OPBMServiceCallCompleted;

        #endregion

        #region .ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public OrdersPortfolioBindableModel()
        {
            this.TimeFramefor = "WEEK";
        }

        #endregion

        #region Properties

        /// <summary>
        /// This Holds and Returns SearchBy1
        /// </summary>
        public string SearchBy1
        {
            get
            {
                return __searchBy1;
            }
            set
            {
                this.__searchBy1 = value;
                this.NotifyPropertyChanged("SearchBy1");
            }
        }

        /// <summary>
        /// This Holds and Returns SearchBy2
        /// </summary>
        public string SearchBy2
        {
            get
            {
                return __searchBy2;
            }
            set
            {
                this.__searchBy2 = value;
                this.NotifyPropertyChanged("SearchBy2");
            }
        }

        /// <summary>
        /// This Holds and Returns Searchfor1
        /// </summary>        
        [Display(Name = "Searchfor1")]
        [CustomValidation(typeof(CustomValidator), "ValidateSearchFor1")]
        public string Searchfor1
        {
            get
            {
                return __searchfor1;
            }
            set
            {
                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = "Searchfor1";
                Validator.ValidateProperty(value, context);

                this.__searchfor1 = value;
                this.NotifyPropertyChanged("Searchfor1");
            }
        }

        /// <summary>
        /// This Holds and Returns Searchfor2
        /// </summary>        
        [Display(Name = "Search By2")]
        [CustomValidation(typeof(CustomValidator), "ValidateSearchBy")]
        public string Searchfor2
        {
            get
            {
                return __searchfor2;
            }
            set
            {
                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = "Searchfor2";
                Validator.ValidateProperty(value, context);

                this.__searchfor2 = value;
                this.NotifyPropertyChanged("Searchfor2");
            }
        }

        /// <summary>
        /// This Holds and Returns TimeFrameby
        /// </summary>
        public string TimeFrameby
        {
            get
            {
                return __timeFrameby;
            }
            set
            {
                this.__timeFrameby = value;
                this.NotifyPropertyChanged("TimeFrameby");
            }
        }

        /// <summary>
        /// This Holds and Returns TimeFramefor
        /// </summary>
        public string TimeFramefor
        {
            get
            {
                return __timeFramefor;
            }
            set
            {
                this.__timeFramefor = value;
                this.NotifyPropertyChanged("TimeFramefor");
            }
        }

        /// <summary>
        /// This Holds and Returns FromDate
        /// </summary>
        [Display(Name = "From Date")]
        [CustomValidation(typeof(CustomValidator), "ValidatePortfolioFromDate")]
        public string FromDate
        {
            get { return __fromDate; }
            set
            {
                __fromDate = value;

                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = "FromDate";
                Validator.ValidateProperty(value, context);
                this.NotifyPropertyChanged("FromDate");
            }
        }

        /// <summary>
        /// This Holds and Returns ToDate
        /// </summary>
        [Display(Name = "To Date")]
        [CustomValidation(typeof(CustomValidator), "ValidatePortfolioToDate")]
        public string ToDate
        {
            get { return __toDate; }
            set
            {
                __toDate = value;

                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = "ToDate";
                Validator.ValidateProperty(value, context);
                this.NotifyPropertyChanged("ToDate");
            }
        }

        /// <summary>
        /// This Holds and Returns Status
        /// </summary>
        public string Status
        {
            get
            {
                return __status;
            }
            set
            {
                this.__status = value;
                this.NotifyPropertyChanged("Status");
            }
        }

        /// <summary>
        /// This Holds and Returns StatusEmpty
        /// </summary>
        public string StatusEmpty
        {
            get
            {
                return __statusEmptyGrid;
            }
            set
            {
                this.__statusEmptyGrid = value;
                this.NotifyPropertyChanged("StatusEmpty");
            }
        }

        /// <summary>
        /// This Holds and Returns PortfolioResultList
        /// </summary>
        public List<PortfolioResult> PortfolioResultList
        {
            get { return __portfolioResultList; }
            set
            {
                __portfolioResultList = value;
                if (value != null)
                {
                    this.PortfolioResulPagedData = new PagedCollectionView(value);
                    this.PortfolioResulPagedData.SortDescriptions.Add(new SortDescription("OrderDate", ListSortDirection.Descending));
                }
            }
        }

        /// <summary>
        /// This Holds and Returns PortfolioResulPagedData
        /// </summary>
        public PagedCollectionView PortfolioResulPagedData
        {
            get { return __PortfolioResulPagedData; }
            set
            {
                __PortfolioResulPagedData = value;
                this.NotifyPropertyChanged("PortfolioResulPagedData");
            }
        }

        /// <summary>
        /// This Holds and Returns TeamId
        /// </summary>
        public string TeamId
        {
            get
            {
                return __teamId;
            }
            set
            {
                this.__teamId = value;
                this.NotifyPropertyChanged("TeamId");
            }
        }

        /// <summary>
        /// This Holds and Returns UserNo
        /// </summary>
        public string UserNo
        {
            get
            {
                return __userNo;
            }
            set
            {
                this.__userNo = value;
                this.NotifyPropertyChanged("UserNo");
            }
        }

        /// <summary>
        /// This Holds and Returns PreferenceLevel
        /// </summary>
        public string PreferenceLevel
        {
            get
            {
                return __preferenceLevel;
            }
            set
            {
                this.__preferenceLevel = value;
                this.NotifyPropertyChanged("PreferenceLevel");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int LastPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Required to Validate Empty ToDate
        /// </summary>
        public string TempToDate
        {
            get;
            set;
        }

        /// <summary>
        /// Required to Validate Empty FromDate
        /// </summary>
        public string TempFromDate
        {
            get;
            set;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Prepare The Service Call For Portfolio Result
        /// </summary>
        /// <returns>void</returns>
        public void BeginRefresh()
        {
            // Prepare Request             
            PortfolioResultRequest portfolioResultRequest = new PortfolioResultRequest();
            PortfolioSearchCriteria portfolioSearchCriteria = new PortfolioSearchCriteria();
            portfolioSearchCriteria.SearchBy1 = this.SearchBy1;
            portfolioSearchCriteria.SearchBy2 = this.SearchBy2;
            portfolioSearchCriteria.SearchFor1 = this.Searchfor1;
            portfolioSearchCriteria.SearchFor2 = this.Searchfor2;
            portfolioSearchCriteria.TimeFrameBy = this.TimeFrameby;
            portfolioSearchCriteria.TimeFrameFor = this.TimeFramefor;
            portfolioSearchCriteria.FromDate = this.FromDate;
            portfolioSearchCriteria.ToDate = this.ToDate;
            portfolioSearchCriteria.Status = this.Status;
            portfolioSearchCriteria.TeamId = this.TeamId;
            portfolioSearchCriteria.UserNo = this.UserNo;
            portfolioSearchCriteria.PreferenceLevel = this.PreferenceLevel;

            portfolioResultRequest.SearchCriteria = portfolioSearchCriteria;

            PortfolioResultListProxy portfolioResultListProxy = new PortfolioResultListProxy();
            portfolioResultListProxy.Invoke(portfolioResultRequest, PortfolioResultServiceCompleted);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Portfolio Result Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:PortfolioResultListResponse</param>
        /// <returns>void</returns>
        private void PortfolioResultServiceCompleted(DxProxy sender, DxCompleteEventArgs<PortfolioResultListResponse> args)
        {
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "PortfolioResultList.svc"));
            }

            this.PortfolioResultList = args.Response.PortfolioResultList;

            if (null != OPBMServiceCallCompleted)
                OPBMServiceCallCompleted(this, new RoutedEventArgs());
        }
        #endregion

        #region IEditableObject Interface

        /// <summary>
        /// 
        /// </summary>
        public void BeginEdit()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CancelEdit()
        {
            //throw new NotImplementedException();
            __fromDate = String.Empty;
            __toDate = String.Empty;
            __searchBy1 = String.Empty;
            __searchBy2 = String.Empty;
            __searchfor1 = String.Empty;
            __searchfor2 = String.Empty;            
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndEdit()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }

    public partial class CustomValidator
    {
        #region Variables

        private const string ERROR_MSG09 = "Please select two different search by criteria.";
        private const string ERROR_MSG16 = "You can enter max 30 characters in {0}.";        
        private const string CUSTOM_DATE = "CUSTOM DATE";
        private const string SEARCHFOR1 = "SearchFor1";
        private const string SEARCHFOR2 = "SearchFor2";

        #endregion

        #region Public Methods

        /// <summary>
        /// Validate SearchFor1 Against SearchBy2 
        /// </summary>
        /// <param name="searchfor2">object</param>
        /// <param name="context">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        public static ValidationResult ValidateSearchFor1(object searchfor1, ValidationContext context)
        {
            OrdersPortfolioBindableModel ordersPortfolioBindableModel = context.ObjectInstance as OrdersPortfolioBindableModel;
            string newSearchfor1 = searchfor1.ToString().Trim();
            if (newSearchfor1.Length > 30)
                return new ValidationResult(string.Format(ERROR_MSG16, context.DisplayName, SEARCHFOR1));

            return ValidationResult.Success;
        }

        /// <summary>
        /// Validate SearchBy1 Against SearchBy2 
        /// </summary>
        /// <param name="searchfor2">object</param>
        /// <param name="context">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        public static ValidationResult ValidateSearchBy(object searchfor2, ValidationContext context)
        {
            OrdersPortfolioBindableModel ordersPortfolioBindableModel = context.ObjectInstance as OrdersPortfolioBindableModel;
            string newSearchfor2 = searchfor2.ToString().Trim();

            if (newSearchfor2.Length > 30)
                return new ValidationResult(string.Format(ERROR_MSG16, context.DisplayName,SEARCHFOR2));

            if (!String.IsNullOrEmpty(newSearchfor2) && !String.IsNullOrEmpty(ordersPortfolioBindableModel.Searchfor1))
            {
                if (ordersPortfolioBindableModel.SearchBy1.Equals(ordersPortfolioBindableModel.SearchBy2))
                    return new ValidationResult(string.Format(ERROR_MSG09, context.DisplayName, SEARCHFOR1));
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Validate From Date Is Not Empty
        /// </summary>
        /// <param name="newValue">object</param>
        /// <param name="context">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        public static ValidationResult ValidatePortfolioFromDate(object newToValue, ValidationContext context)
        {
            OrdersPortfolioBindableModel ordersPortfolioBindableModel = context.ObjectInstance as OrdersPortfolioBindableModel;
            string tempToDate = ordersPortfolioBindableModel.TempToDate;
            string tempFromDate = ordersPortfolioBindableModel.TempFromDate;

            if (ordersPortfolioBindableModel.TimeFramefor.Equals(CUSTOM_DATE))
            {
                if (!String.IsNullOrEmpty(tempFromDate))
                    if (!Regex.IsMatch(tempFromDate, REGEXPR_DATE_VALUE) || !IsconvertToDateTime(tempFromDate)) return new ValidationResult(string.Format(ERROR_MSG08, context.DisplayName));

                if (String.IsNullOrEmpty(tempFromDate))
                    return new ValidationResult(ERROR_MSG04);
            }
            return ValidationResult.Success;
        }

        /// <summary>
        /// Validate ToDate Is Not Less Than From Date
        /// </summary>
        /// <param name="newValue">object</param>
        /// <param name="context">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        public static ValidationResult ValidatePortfolioToDate(object newToValue, ValidationContext context)
        {
            OrdersPortfolioBindableModel ordersPortfolioBindableModel = context.ObjectInstance as OrdersPortfolioBindableModel;
            string tempToDate = ordersPortfolioBindableModel.TempToDate;
            string tempFromDate = ordersPortfolioBindableModel.TempFromDate;

            if (ordersPortfolioBindableModel.TimeFramefor.Equals(CUSTOM_DATE))
            {
                if (!String.IsNullOrEmpty(tempToDate))
                    if (!Regex.IsMatch(tempToDate, REGEXPR_DATE_VALUE) || !IsconvertToDateTime(tempToDate)) return new ValidationResult(string.Format(ERROR_MSG08, context.DisplayName));

                if (String.IsNullOrEmpty(tempToDate))
                    return new ValidationResult(ERROR_MSG05);
            }
            if (newToValue != null && newToValue.ToString() != string.Empty)
            {
                if (!String.IsNullOrEmpty(tempFromDate) && IsconvertToDateTime(newToValue) && IsconvertToDateTime(tempFromDate))                    
                {
                    if (DateTime.Compare(Convert.ToDateTime(newToValue), Convert.ToDateTime(tempFromDate)) < 0) return new ValidationResult(ERROR_MSG01); 
                }
            }
            return ValidationResult.Success;
        }

        #endregion
    }
}
