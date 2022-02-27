using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using CT.SLABB.DX;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public class OrderConfirmationBindableModel : BaseBindableModel, INotifyPropertyChanged
    {
        #region Variables

        public event RoutedEventHandler OCBMServiceCallCompleted;
        public event RoutedEventHandler OrderCompleted;
        private const string ORDER_COMPLETED = "Order already completed";

        #endregion

        #region .ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderConfirmationBindableModel()
        { }
        #endregion

        #region Properties

        #region Other Properties

        /// <summary>
        /// This Holds and Returns PartyName
        /// </summary>
        public string PartyName { get; set; }

        /// <summary>
        /// This Holds and Returns OriginalSearchKey
        /// </summary>
        public string OriginalSearchKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TrackingItemNo { get; set; }

        /// <summary>
        /// This Holds and Returns UserContext
        /// </summary>
        public UserContext UserContext { get; set; }

        /// <summary>
        /// This holds and returns 'true' if there are no records to be submitted else 'false'
        /// </summary>
        public bool NoRecords { get; set; }

        /// <summary>
        /// This holds and returns the submittype-- NameSearchResults or DetailSearchResults
        /// </summary>
        public string SubmitType { get; set; }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Prepare Service Call For Submit Detail Rsult List
        /// </summary>
        /// <param name="processedTaskIds">string</param>
        public void BeginRefresh(string processedTaskIds)
        {
            SubmitDetailResultsRequest submitDetailResultsListRequest = new SubmitDetailResultsRequest();
            SearchCriteria searchCriteria = new SearchCriteria();

            searchCriteria.TrackingNo = this.HeaderInfo.TrackingNo;
            searchCriteria.CountyName = this.HeaderInfo.CountyName;
            searchCriteria.StateName = this.HeaderInfo.StateCode;
            searchCriteria.SearchKey = this.HeaderInfo.SearchKey;
            searchCriteria.FromDate = this.HeaderInfo.FromDate;
            searchCriteria.ToDate = this.HeaderInfo.ToDate;
            searchCriteria.LienType = this.HeaderInfo.LienType;
            searchCriteria.OrderNo = this.HeaderInfo.OrderNo;
            searchCriteria.OrderItemNo = this.HeaderInfo.OrderItemNo;

            searchCriteria.AssignedTo = UserContext.UserId;
            searchCriteria.CountyCode = 0;
            searchCriteria.CreatedBy = UserContext.UserId;
            searchCriteria.FirstName = string.Empty;
            searchCriteria.MiddleName = string.Empty;
            searchCriteria.LastName = string.Empty;
            searchCriteria.PartyName = this.PartyName;
            searchCriteria.SearchCriteriaType = string.Empty;
            searchCriteria.SearchType = string.Empty;
            searchCriteria.StateName = string.Empty;
            searchCriteria.TrackingItemNo = this.TrackingItemNo; 

            submitDetailResultsListRequest.SearchCriteria = searchCriteria;
            submitDetailResultsListRequest.CompletedBy = UserContext.UserId;
            submitDetailResultsListRequest.TaskIDs = processedTaskIds;
            submitDetailResultsListRequest.SubmitType = this.SubmitType;
            submitDetailResultsListRequest.NoRecords = this.NoRecords;

            SubmitDetailResultsProxy submitDetailResultsProxy = new SubmitDetailResultsProxy();
            submitDetailResultsProxy.Invoke(submitDetailResultsListRequest, SubmitDetailResultsServiceCompleted);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Submit Detail Results Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:SubmitDetailResultsResponse</param>
        /// <returns>void</returns>
        private void SubmitDetailResultsServiceCompleted(DxProxy sender, DxCompleteEventArgs<SubmitDetailResultsResponse> args)
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

            if (null != OCBMServiceCallCompleted)
                OCBMServiceCallCompleted(this, new RoutedEventArgs());
        }

        #endregion
    }
}
