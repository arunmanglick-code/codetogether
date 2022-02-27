using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using CT.SLABB.DX;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public class SummarySearchResultBindableModel : BaseBindableModel, INotifyPropertyChanged
    {
        #region Variables
        
        public event RoutedEventHandler OrderCompleted;
        private long __trackingNo;
        private bool __isNewSummaryResultsList = false;
        private List<SummaryResults> __checkedSummaryResultsList = new List<SummaryResults>();
        private List<SummaryResults> __summaryResultsList = new List<SummaryResults>();
        private const string ORDER_COMPLETED = "Order already completed";

        #endregion

        #region Events

        public event RoutedEventHandler SSBMServiceCallCompleted1;
        public event RoutedEventHandler SSBMServiceCallCompleted2;

        #endregion

        #region .ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public SummarySearchResultBindableModel()
        {
            SummaryResultsList = new List<SummaryResults>();
            IsNewSummaryResultsList = false;
        }
        #endregion

        #region Properties
        
        #region Other Properties

        /// <summary>
        /// 
        /// </summary>
        public string SearchKeys { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ViewDetailsResponse ViewDetailsResponse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNewSummaryResultsList
        {
            get { return __isNewSummaryResultsList; }
            set { __isNewSummaryResultsList = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<SummaryResults> CheckedSummaryResultsList
        {
            get
            {
                return __checkedSummaryResultsList;
            }
            set
            {
                __checkedSummaryResultsList = value;
                this.NotifyPropertyChanged("CheckedSummaryResultsList");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<SummaryResults> SummaryResultsList
        {
            get { return __summaryResultsList; }
            set
            {
                __summaryResultsList = value;
                this.NotifyPropertyChanged("SummaryResultsList");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UserContext UserContext { get; set; }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        ///  Prepare The Service Call For Summary Search Result
        /// </summary>
        /// <param name="searchKey">string</param>
        /// <returns>void</returns>
        public void BeginRefresh(string searchKey)
        {
            SummarySearchResultsRequest summarySearchResultsRequest = new SummarySearchResultsRequest();
            summarySearchResultsRequest.SearchKey = searchKey;
            summarySearchResultsRequest.TrackingNo = (int)this.HeaderInfo.TrackingNo;

            // Call Service
            SummarySearchResultsProxy summarySearchResultsProxy = new SummarySearchResultsProxy();
            IsNewSummaryResultsList = false;
            summarySearchResultsProxy.Invoke(summarySearchResultsRequest, SummarySearchResultsServiceCompleted);
        }
        
        /// <summary>
        /// Prepare The Service Call For View Details
        /// </summary>
        /// <param name="searchKey">string</param>
        /// <param name="partyNames">string</param>
        /// <returns>void</returns>
        public void BeginRefresh(string searchKey, string partyNames)
        {
            ViewDetailsRequest viewDetailsRequest = new ViewDetailsRequest();
            SearchCriteria searchCriteria = new SearchCriteria();

            searchCriteria.CreatedBy = UserContext.UserId;
            searchCriteria.CountyName = this.HeaderInfo.CountyName;
            searchCriteria.FromDate = this.HeaderInfo.FromDate;
            searchCriteria.LienType = this.HeaderInfo.LienType;
            searchCriteria.PartyName = partyNames;
            searchCriteria.SearchKey = searchKey;
            searchCriteria.StateName = this.HeaderInfo.StateCode;
            searchCriteria.TrackingItemNo = -1;    // HARD CODED  ==> As provided by DX Team
            searchCriteria.TrackingNo = this.HeaderInfo.TrackingNo;

            viewDetailsRequest.SearchCriteria = searchCriteria;

            ViewDetailProxy viewDetailProxy = new ViewDetailProxy();
            viewDetailProxy.Invoke(viewDetailsRequest, ViewDetailServiceCompleted);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Summary SearchResults Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:SummarySearchResultsListResponse</param>
        /// <returns>void</returns>
        private void SummarySearchResultsServiceCompleted(DxProxy sender, DxCompleteEventArgs<SummarySearchResultsListResponse> args)
        {
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "SummarySearchResultsListSVC"));
            }

            this.SummaryResultsList = args.Response.SummaryResultsList;
            if (null != SSBMServiceCallCompleted1)
            {
                IsNewSummaryResultsList = true;

                if (CheckedSummaryResultsList == null)
                    CheckedSummaryResultsList = new List<SummaryResults>();

                SSBMServiceCallCompleted1(this, new RoutedEventArgs());
            }

        }

        /// <summary>
        /// View Detail Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:PortfolioResultListResponse</param>
        /// <returns>void</returns>
        private void ViewDetailServiceCompleted(DxProxy sender, DxCompleteEventArgs<ViewDetailsResponse> args)
        {
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "SummarySearchResultsListSVC"));
            }

            if (!String.IsNullOrEmpty(args.Response.ErrorMessage) && args.Response.ErrorMessage.Equals(ORDER_COMPLETED))
            {
                this.ErrorMessage = args.Response.ErrorMessage;
                this.OrderCompletedBy = args.Response.OrderCompletedBy;
                this.OrderCompletedOn = args.Response.OrderCompletedOn;

                if (null != OrderCompleted) OrderCompleted(this, new RoutedEventArgs());
                return;
            }

            if (null != SSBMServiceCallCompleted2)
            {
                this.ViewDetailsResponse = args.Response;
                SSBMServiceCallCompleted2(this, new RoutedEventArgs());
            }
        }

        #endregion

        #region Property Changed Implementation

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
