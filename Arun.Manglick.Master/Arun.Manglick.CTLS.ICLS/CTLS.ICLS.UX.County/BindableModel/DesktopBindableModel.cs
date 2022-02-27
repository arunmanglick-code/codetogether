using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using CT.SLABB.DX;
using CT.SLABB.Utils;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;


namespace CTLS.ICLS.UX.CountySearch
{
    public class DesktopBindableModel : Bindable
    {
        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public DesktopBindableModel()
        {
            this.__type = "Org";
            this.__searchType = "Lien";
            this.__lienTypeSelected = string.Empty;
            this.__customerSpecialistSelected = string.Empty;
        }
        #endregion

        #region Variables

        private string __type;
        private string __searchType;
        private string __orderNumber;
        private string __orderItemNumber;
        private string __location;
        private string __county;
        private string __fromDate;
        private string __toDate;
        private ComboBoxItem __selectedSearchCriteria = new ComboBoxItem() { Content = "Lien Types" };
        private string __searchKey;
        private string __firstName;
        private string __lastName;
        private string __middleName;
        private string __customerSpecialistSelected;
        private string __lienTypeSelected;
        private Jurisdiction __jurisdiction;
        private int __trackingItemNo;
        private long __trackingNo;
        private SearchCriteria __searchCriteria;

        #endregion

        #region Events

        public event RoutedEventHandler ValidationCompleted;
        public event RoutedEventHandler SearchCompleted;

        #endregion

        #region Properties

        /// <summary>
        /// This Holds and Returns The Type
        /// </summary>
        public string Type
        {
            get
            {
                return __type;
            }
            set
            {
                this.__type = value;
                this.NotifyPropertyChanged("Type");
            }
        }

        /// <summary>
        /// This Holds and Returns The SearchType
        /// </summary>
        public string SearchType
        {
            get
            {
                return __searchType;
            }
            set
            {
                this.__searchType = value;
                this.NotifyPropertyChanged("SearchType");
            }
        }

        /// <summary>
        /// This Holds and Returns The OrderNumber
        /// </summary>
        [Display(Name = "Order #")]
        [CustomValidation(typeof(CustomValidator), "ValidateOrderNumber")]
        public string OrderNumber
        {
            get { return __orderNumber; }
            set
            {
                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = "OrderNumber";
                Validator.ValidateProperty(value, context);

                __orderNumber = value;
                this.NotifyPropertyChanged("OrderNumber");
            }
        }

        /// <summary>
        /// This Holds and Returns The OrderItemNumber 
        /// </summary>        
        [Display(Name = "Order Item #")]
        [CustomValidation(typeof(CustomValidator), "ValidateOrderItemNumber")]
        public string OrderItemNumber
        {
            get { return __orderItemNumber; }
            set
            {
                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = "OrderItemNumber";
                Validator.ValidateProperty(value, context);

                __orderItemNumber = value;
                this.NotifyPropertyChanged("OrderItemNumber");
            }
        }

        /// <summary>
        /// This Holds and Returns The Location 
        /// </summary>      
        [Display(Name = "Location")]
        public string Location
        {
            get { return __location; }
            set
            {
                if (value.Equals(SharedConstants.SELECT_ONE_KEY))
                    throw new Exception("Select Value for State");
                else
                {
                    __location = value;
                    this.NotifyPropertyChanged("Location");
                }
            }
        }

        /// <summary>
        /// This Holds and Returns The County 
        /// </summary>      
        [Display(Name = "County")]
        public string County
        {
            get { return __county; }
            set
            {
                if (value.Equals(SharedConstants.SELECT_ONE_KEY))
                    throw new Exception("Select Value for County");
                else
                {
                    __county = value;
                    this.NotifyPropertyChanged("County");
                }
            }
        }

        /// <summary>
        /// This Holds and Returns The FromDate  
        /// </summary>               
        [Display(Name = "From Date")]
        [CustomValidation(typeof(CustomValidator), "ValidateFromDate")]
        public string FromDate
        {
            get { return __fromDate; }
            set
            {
                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = "FromDate";
                Validator.ValidateProperty(value, context);

                __fromDate = value;
                this.NotifyPropertyChanged("FromDate");
            }
        }

        /// <summary>
        /// This Holds and Returns The ToDate   
        /// </summary>
        [Display(Name = "To Date")]
        [CustomValidation(typeof(CustomValidator), "ValidateToDate")]
        public string ToDate
        {
            get { return __toDate; }
            set
            {
                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = "ToDate";
                Validator.ValidateProperty(value, context);

                __toDate = value;
                this.NotifyPropertyChanged("ToDate");
            }
        }

        /// <summary>
        /// This Holds and Returns The SelectedSearchCriteria 
        /// </summary>              
        public ComboBoxItem SelectedSearchCriteria
        {
            get { return __selectedSearchCriteria; }
            set { __selectedSearchCriteria = value; this.NotifyPropertyChanged("SelectedSearchCriteria"); }
        }

        /// <summary>
        /// This Holds and Returns The SearchKey
        /// </summary>
        [Display(Name = "County Direct Search Key")]
        [CustomValidation(typeof(CustomValidator), "ValidateSearchKey")]
        public string SearchKey
        {
            get { return __searchKey; }
            set
            {
                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = "SearchKey";
                Validator.ValidateProperty(value, context);

                __searchKey = value;
                this.NotifyPropertyChanged("SearchKey");
            }
        }

        /// <summary>
        /// This Holds and Returns The FirstName
        /// </summary>
        [Display(Name = "First Name")]
        [CustomValidation(typeof(CustomValidator), "ValidateFirstName")]
        public string FirstName
        {
            get { return __firstName; }
            set
            {
                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = "FirstName";
                Validator.ValidateProperty(value, context);

                __firstName = value;
                this.NotifyPropertyChanged("FirstName");

            }
        }

        /// <summary>
        /// This Holds and Returns The LastName
        /// </summary>
        [Display(Name = "Last Name")]
        [CustomValidation(typeof(CustomValidator), "ValidateLastName")]
        public string LastName
        {
            get { return __lastName; }
            set
            {
                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = "LastName";
                Validator.ValidateProperty(value, context);

                __lastName = value;
                this.NotifyPropertyChanged("LastName");
            }
        }

        /// <summary>
        /// This Holds and Returns The MiddleName
        /// </summary>3
        [Display(Name = "M. Name")]
        [CustomValidation(typeof(CustomValidator), "ValidateMiddleName")]
        public string MiddleName
        {
            get { return __middleName; }
            set
            {
                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = "MiddleName";
                Validator.ValidateProperty(value, context);

                __middleName = value;
                this.NotifyPropertyChanged("MiddleName");
            }
        }

        /// <summary>
        /// This Holds and Returns The LienTypeSelected
        /// </summary>        
        [Display(Name = "Lien Types")]
        public string LienTypeSelected
        {
            get { return __lienTypeSelected; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Please select lien type.");
                }
                else
                {
                    __lienTypeSelected = value;
                    this.NotifyPropertyChanged("LienTypeSelected");
                }
            }
        }

        /// <summary>
        /// This Holds and Returns The CustomerSpecialistSelected
        /// </summary>        
        public string CustomerSpecialistSelected
        {
            get { return __customerSpecialistSelected; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Please select customer specialist");
                }
                else
                {
                    __customerSpecialistSelected = value;
                    this.NotifyPropertyChanged("CustomerSpecialistSelected");
                }
            }
        }

        /// <summary>
        /// This Holds and Returns The TrackingItemNo
        /// </summary>
        public int TrackingItemNo
        {
            get { return __trackingItemNo; }
            set { __trackingItemNo = value; }
        }

        /// <summary>
        /// This Holds and Returns The TrackingNo
        /// </summary>
        public long TrackingNo
        {
            get { return __trackingNo; }
            set { __trackingNo = value; }
        }

        /// <summary>
        /// This Holds and Returns The CTLS.ICLS.DX.Jurisdiction
        /// </summary>
        public Jurisdiction Jurisdiction
        {
            get { return __jurisdiction; }
            set { __jurisdiction = value; }
        }

        /// <summary>
        /// This Holds and Returns The CTLS.ICLS.DX.SearchCriteria 
        /// </summary>
        public SearchCriteria SearchCriteria
        {
            get { return __searchCriteria; }
            set { __searchCriteria = value; }
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

        #region Public Methods

        /// <summary>
        /// Validates passed OrderNo and OrderItemNo
        /// </summary>
        /// <param name="OrderNo">OrderNo</param>
        /// <param name="OrderItemNo">OrderItemNo</param>
        /// <returns>void</returns>
        public void Validate(long OrderNo, int OrderItemNo)
        {
            ValidateProxy validateProxy = new ValidateProxy();
            ValidateRequest validateRequest = new ValidateRequest();
            validateRequest.OrderNo = OrderNo;
            validateRequest.OrderItemNo = OrderItemNo;
            this.Jurisdiction = null;
            validateProxy.Invoke(validateRequest, ValidateServiceCompleted);
        }

        /// <summary>
        /// Performs search as per the SearchCriteria
        /// </summary>
        /// /// <returns>void</returns>
        public void PerformSearch()
        {
            PerformSearchProxy performSearchProxy = new PerformSearchProxy();
            PerformSearchRequest performSearchRequest = new PerformSearchRequest();

            performSearchRequest.SearchCriteria = this.SearchCriteria;
            performSearchProxy.Invoke(performSearchRequest, PerformSearchServiceCompleted);
        }

        #endregion

        #region Service CallBack handlers

        /// <summary>
        /// Validate State And County
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:ValidateResponse</param>
        /// <returns>void</returns>
        private void ValidateServiceCompleted(DxProxy sender, DxCompleteEventArgs<ValidateResponse> args)
        {

            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "ValidateSVC"));
            }

            this.Jurisdiction = args.Response.jurisdiction;

            if (null != ValidationCompleted)
                ValidationCompleted(this, new RoutedEventArgs());
        }

        /// <summary>
        /// Search Sercvice Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:PerformSearchResponse</param>
        private void PerformSearchServiceCompleted(DxProxy sender, DxCompleteEventArgs<PerformSearchResponse> args)
        {
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "PerformSearchSVC"));
            }

            this.TrackingItemNo = args.Response.TrackingItemNo;
            this.TrackingNo = args.Response.TrackingNo;

            if (this.SearchCompleted != null)
                this.SearchCompleted(this, new RoutedEventArgs());
        }

        #endregion
    }

    public partial class CustomValidator
    {
        #region Variables

        public const string CONST_Reg_Expr_Spl_Chars = @"[^\s\w/]";
        public const string CONST_Reg_Expr_Alpha_Num = "[A-Za-z0-9/]";
        private const string REGEXPR_NUMERIC_VALUE = @"^[0-9]*$";
        private const string REGEXPR_DATE_VALUE = @"^(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$";
        private const string REGEXDOC_SEARCH_KEY = @"\d{4}[-][R][-]\d{1,7}";
        private const string REGEXDOCSEARCH_KEY = @"\d{4}[R]\d{1,7}";
        private const string REGEXCA_DOCSEARCH_KEY = @"\d{1,10}";
        private const string ERROR_MSG01 = "To Date must be 'Greater Than or Equal To' From Date";
        private const string ERROR_MSG02 = "Please re-enter valid {0}.";
        private const string ERROR_MSG03 = "Select Value for SearchInputType.";
        private const string ERROR_MSG04 = "Please enter value for From Date.";
        private const string ERROR_MSG05 = "Please enter value for To Date.";
        private const string ERROR_MSG06 = "You can enter a maximum {0} digits upto {1}.";
        private const string ERROR_MSG07 = "{0} can not be blank and must be positive number.";
        private const string ERROR_MSG08 = "Please enter valid {0}.";
        private const string ERROR_MSG10 = "{0} can not be zero.";
        private const string ERROR_MSG11 = "Only one character is allowed as a middle name to search for this.";
        private const string ERROR_MSG12 = "{0} must have between 1 to 30 characters.";
        private const string ERROR_MSG13 = "Document number not in correct format.";
        private const string ERROR_MSG14 = "Please provide minimum 3 characters in a search key.";
        private const string ERROR_MSG15 = "Document number should be a numeric.";
        private const string ERROR_MSG17 = "Please provide {0}.";
        private const string ORGANIZATION = "Org";
        private const string INDIVIDUAL = "Ind";
        private const string FLCOUNTY = "FL25";
        private const string CACOUNTY = "CA85";
        private const string AZCOUNTY = "AZ13";
        #endregion

        #region Public Methods

        /// <summary>
        /// Validate Order Number for requird and max 10 digits
        /// </summary>
        /// <param name="newValue">object</param>
        /// <param name="context">ValidationContext</param>
        /// <returns>ValidationResult</returns>       
        public static ValidationResult ValidateOrderNumber(object newValue, ValidationContext context)
        {
            string number = String.Empty;
            if (null != newValue)
                if (Regex.IsMatch(newValue.ToString().Trim(), REGEXPR_NUMERIC_VALUE))
                {
                    number = newValue.ToString().Trim();

                    if (String.IsNullOrEmpty(number))
                        return new ValidationResult(string.Format(ERROR_MSG07, context.DisplayName));

                    if (number.Length > 10)
                        return new ValidationResult(string.Format(ERROR_MSG06, context.DisplayName, 10));

                    long checkZero = 0;
                    Int64.TryParse(number, out checkZero);
                    if (checkZero == 0)
                        return new ValidationResult(string.Format(ERROR_MSG10, context.DisplayName));
                }
                else
                    return new ValidationResult(string.Format(ERROR_MSG02, context.DisplayName));
            return ValidationResult.Success;
        }

        /// <summary>
        /// Validate Order ItemNumber  for requird and max 3 digits
        /// </summary>
        /// <param name="newValue">object</param>
        /// <param name="context">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        public static ValidationResult ValidateOrderItemNumber(object newValue, ValidationContext context)
        {
            string number = String.Empty;
            if (null != newValue)
                if (Regex.IsMatch(newValue.ToString().Trim(), REGEXPR_NUMERIC_VALUE))
                {
                    number = newValue.ToString().Trim();

                    if (String.IsNullOrEmpty(number))
                        return new ValidationResult(string.Format(ERROR_MSG07, context.DisplayName));

                    if (number.Length > 3)
                        return new ValidationResult(string.Format(ERROR_MSG06, context.DisplayName, 3));

                    int checkZero = 0;
                    Int32.TryParse(number, out checkZero);
                    if (checkZero == 0)
                        return new ValidationResult(string.Format(ERROR_MSG10, context.DisplayName));
                }
                else
                    return new ValidationResult(string.Format(ERROR_MSG02, context.DisplayName));
            return ValidationResult.Success;
        }

        /// <summary>
        ///  Validate From Date entered Is in Correct <mm/dd/yyyy> Format Or Not
        /// </summary>
        /// <param name="newValue">object</param>
        /// <param name="context">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        public static ValidationResult ValidateFromDate(object newFromValue, ValidationContext context)
        {
            DesktopBindableModel desktopBindableModel = context.ObjectInstance as DesktopBindableModel;
            string tempToDate = desktopBindableModel.TempToDate;
            string tempFromDate = desktopBindableModel.TempFromDate;

            bool presentToDate = !String.IsNullOrEmpty(tempToDate) ? true : false;
            bool validToDate = Regex.IsMatch(tempToDate, REGEXPR_DATE_VALUE);

            if (!String.IsNullOrEmpty(tempFromDate))
            {
                if (!Regex.IsMatch(tempFromDate, REGEXPR_DATE_VALUE)|| !IsconvertToDateTime(tempFromDate))
                    return new ValidationResult(string.Format(ERROR_MSG08, context.DisplayName));
                
            }
            else
            {
                if (presentToDate && validToDate && IsconvertToDateTime(tempToDate))
                    return new ValidationResult(ERROR_MSG04);
            }

            return ValidationResult.Success; // Here Empty Date would also be 'Success'
        }

        /// <summary>
        ///  Validate To Date Entered Is In Correct <mm/dd/yyyy> Format And Also Should Not Greater Than From Date
        /// </summary>
        /// <param name="newValue">object</param>
        /// <param name="context">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        public static ValidationResult ValidateToDate(object newToValue, ValidationContext context)
        {
            DesktopBindableModel desktopBindableModel = context.ObjectInstance as DesktopBindableModel;
            string tempToDate = desktopBindableModel.TempToDate;
            string tempFromDate = desktopBindableModel.TempFromDate;

            bool presentFromDate = !String.IsNullOrEmpty(tempFromDate) ? true : false;
            bool validFromDate = Regex.IsMatch(tempFromDate, REGEXPR_DATE_VALUE);

            if (!String.IsNullOrEmpty(tempToDate))
            {
                if (!Regex.IsMatch(tempToDate, REGEXPR_DATE_VALUE) || !IsconvertToDateTime(newToValue))
                    return new ValidationResult(string.Format(ERROR_MSG08, context.DisplayName));

                if (presentFromDate && validFromDate && IsconvertToDateTime(newToValue) && IsconvertToDateTime(tempFromDate))
                {
                    if (DateTime.Compare(Convert.ToDateTime(newToValue), Convert.ToDateTime(tempFromDate)) < 0) return new ValidationResult(ERROR_MSG01);
                }
            }
            return ValidationResult.Success;
        }

        /// <summary>
        /// Check whether the given string convert to Dte time or not
        /// </summary>
        /// <param name="newToValue">object</param>
        /// <returns>bool</returns>
        public static bool IsconvertToDateTime(object DateValue)
        {
            bool res = true;
            try { DateTime tmpDate = Convert.ToDateTime(DateValue); }
            catch { res = false; }
            return res;
        }
        
        /// <summary>
        ///  Validate MiddleName 
        /// </summary>
        /// <param name="newValue">object</param>
        /// <param name="context">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        public static ValidationResult ValidateMiddleName(object newMiddleNameValue, ValidationContext context)
        {
            DesktopBindableModel desktopBindableModel = context.ObjectInstance as DesktopBindableModel;
            string County = string.Empty;

            if (newMiddleNameValue != null)
            {
                County = desktopBindableModel.Location + desktopBindableModel.County;
                string newMname = newMiddleNameValue.ToString().Trim();

                if (County.Equals(AZCOUNTY) && desktopBindableModel.Type.Equals(INDIVIDUAL) && newMname.Length > 1)
                    return new ValidationResult(ERROR_MSG11);
            }
            return ValidationResult.Success;
        }

        /// <summary>
        ///  Validate ValidateLastName 
        /// </summary>
        /// <param name="newValue">object</param>
        /// <param name="context">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        public static ValidationResult ValidateLastName(object newLastNameValue, ValidationContext context)
        {
            DesktopBindableModel desktopBindableModel = context.ObjectInstance as DesktopBindableModel;
            string County = string.Empty;

            if (newLastNameValue != null)
            {
                County = desktopBindableModel.Location + desktopBindableModel.County;
                string newSearchName = (newLastNameValue.ToString() + " " + desktopBindableModel.FirstName + " " + desktopBindableModel.MiddleName).ToString().Trim();

                if (String.IsNullOrEmpty(newLastNameValue.ToString().Trim()))
                {
                    return new ValidationResult(string.Format(ERROR_MSG17, context.DisplayName));
                }
                if (desktopBindableModel.Type.Equals(INDIVIDUAL))
                {
                    if (!String.IsNullOrEmpty(desktopBindableModel.FirstName))
                    {
                        if (!County.Equals(FLCOUNTY) && string.IsNullOrEmpty(newSearchName) || newSearchName.Length > 30) return new ValidationResult(string.Format(ERROR_MSG12, "Search Key")); // Common to All County
                        else if (County.Equals(FLCOUNTY) && newSearchName.Length < 3) return new ValidationResult(ERROR_MSG14); // Sfecific to FL-Dade
                    }
                }
            }
            return ValidationResult.Success;
        }

        /// <summary>
        ///  Validate Validate FirstName 
        /// </summary>
        /// <param name="newValue">object</param>
        /// <param name="context">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        public static ValidationResult ValidateFirstName(object newFirstNameValue, ValidationContext context)
        {
            DesktopBindableModel desktopBindableModel = context.ObjectInstance as DesktopBindableModel;
            string County = string.Empty;

            if (newFirstNameValue != null)
            {
                string newFirstName = newFirstNameValue.ToString().Trim();
                if (String.IsNullOrEmpty(newFirstName))
                {
                    return new ValidationResult(string.Format(ERROR_MSG17, context.DisplayName));
                }
            }
            return ValidationResult.Success;
        }

        /// <summary>
        ///  Validate SearchKey Depends on County and  search type
        /// </summary>
        /// <param name="newValue">object</param>
        /// <param name="context">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        public static ValidationResult ValidateSearchKey(object newSearchKeyValue, ValidationContext context)
        {
            DesktopBindableModel desktopBindableModel = context.ObjectInstance as DesktopBindableModel;
            string County = string.Empty;
            long result;

            if (newSearchKeyValue != null)
            {
                County = desktopBindableModel.Location + desktopBindableModel.County;
                string newSearchKey = newSearchKeyValue.ToString().Trim();

                if (String.IsNullOrEmpty(newSearchKey))
                    return new ValidationResult(string.Format(ERROR_MSG17, context.DisplayName));

                if ((desktopBindableModel.SelectedSearchCriteria.Content.Equals(SharedConstants.LIEN_TYPES) && desktopBindableModel.Type.Equals(ORGANIZATION)))
                {
                    if (!County.Equals(FLCOUNTY) && string.IsNullOrEmpty(newSearchKey) || newSearchKey.Length > 30) return new ValidationResult(string.Format(ERROR_MSG12, context.DisplayName)); // Common to All County
                    else if (County.Equals(FLCOUNTY) && newSearchKey.Length < 3) return new ValidationResult(ERROR_MSG14); // Sfecific to FL-Dade
                }

                if (desktopBindableModel.SelectedSearchCriteria.Content.Equals(SharedConstants.DOCUMENT_NUMBER))
                {
                    switch (County)
                    {
                        case FLCOUNTY:
                            if (Regex.IsMatch(newSearchKey, REGEXDOC_SEARCH_KEY) || Regex.IsMatch(newSearchKey, REGEXDOCSEARCH_KEY))
                            {
                                int year = 0;
                                long X = 0;
                                string[] keys = newSearchKey.Split('-');
                                if (keys.Length == 3)
                                {
                                    Int32.TryParse(keys[0], out year);
                                    Int64.TryParse(keys[2], out X);
                                }
                                else
                                {
                                    Int32.TryParse(newSearchKey.Substring(0, 4), out year);
                                    Int64.TryParse(newSearchKey.Substring(5, (newSearchKey.Length - 5)), out X);
                                }
                                if (year < 1901) return new ValidationResult(ERROR_MSG13);
                                if (X < 1 || X > 9999999) return new ValidationResult(ERROR_MSG13);
                            }
                            else
                                return new ValidationResult(ERROR_MSG13);
                            break;

                        case CACOUNTY:
                            if (!Regex.IsMatch(newSearchKey, REGEXPR_NUMERIC_VALUE) || !Regex.IsMatch(newSearchKey, REGEXCA_DOCSEARCH_KEY) || newSearchKey.Length > 10)
                                return new ValidationResult(ERROR_MSG15);
                            result = Convert.ToInt64(newSearchKey);
                            if (result < 1 || result > 9999999999)
                                return new ValidationResult(ERROR_MSG15);
                            break;
                        default:
                            if (string.IsNullOrEmpty(newSearchKey) || newSearchKey.Length > 30) return new ValidationResult(string.Format(ERROR_MSG12, context.DisplayName)); // Common to AZ-Maricoppa, CA-SanDiego, FL-Palm Beach County
                            break;
                    }
                }
            }
            return ValidationResult.Success;
        }

        #endregion
    }
}
