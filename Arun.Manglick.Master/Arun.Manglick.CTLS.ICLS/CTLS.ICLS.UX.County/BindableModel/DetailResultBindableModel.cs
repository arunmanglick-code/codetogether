using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using CT.SLABB.DX;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public class DetailResultBindableModel : BaseBindableModel, INotifyPropertyChanged
    {
        #region Variables

        private bool __firstTimeCall;
        private bool __retryProcessing;
        private bool __cancelProcessing;
        private bool __error;
        private string __sentTaskIds;
        private string __processedTaskIds;
        private string __remainingTaskIds;
        private int __lastProcessedTaskIdCount;
        private ViewDetailsResponse __viewDetailsResponse;
        private List<DetailResultListItem> __detailResultsList;
        private List<DetailResultListItem> __checkedDetailResultsList;

        #endregion

        #region Events

        public event RoutedEventHandler DRBMServiceCallCompleted;
        public event RoutedEventHandler DRBMServiceCallCanceled;
        public event RoutedEventHandler DRBMServiceCallFault;
        public event RoutedEventHandler ProcessingCompleted;
        public event RoutedEventHandler HIRefreshServiceCallCompleted;

        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public DetailResultBindableModel()
        {
            __detailResultsList = new List<DetailResultListItem>();
            __checkedDetailResultsList = new List<DetailResultListItem>();
        }
        #endregion

        #region Properties
        
        #region Other Properties

        /// <summary>
        /// This Holds and Returns TotalTaskIdsCount
        /// </summary>
        public int TotalTaskIdsCount { get; set; }

        /// <summary>
        /// This Holds and Returns ProcessedTaskIdsCount
        /// </summary>
        public int ProcessedTaskIdsCount { get; set; }

        /// <summary>
        /// This Holds and Returns ProcessedTaskIds
        /// </summary>
        public string ProcessedTaskIds { get; set; }

        /// <summary>
        /// This Holds and Returns UserContext
        /// </summary>
        public UserContext UserContext { get; set; }

        /// <summary>
        /// This Holds and Returns RetryProcessing
        /// </summary>
        public bool RetryProcessing
        {
            get { return __retryProcessing; }
            set
            {
                __retryProcessing = value;

                if (value)
                {
                    this.BeginRetryRefresh();
                }
            }
        }

        /// <summary>
        /// This Holds and Returns CancelProcessing
        /// </summary>
        public bool CancelProcessing
        {
            get { return __cancelProcessing; }
            set
            {
                __cancelProcessing = value;
                if (value && ProcessingCompleted != null)
                {
                    ProcessingCompleted(this, new RoutedEventArgs());
                }
            }
        }

        /// <summary>
        /// This Holds and Returns Error
        /// </summary>
        public bool Error
        {
            get { return __error; }
            set
            {
                __error = value;
            }
        }

        /// <summary>
        /// This Holds and Returns ViewDetailsResponse
        /// </summary>
        public ViewDetailsResponse ViewDetailsResponse
        {
            get { return __viewDetailsResponse; }
            set
            {
                __viewDetailsResponse = value;
                this.NotifyPropertyChanged("ViewDetailsResponse");
                BeginingRefresh();
            }
        }

        /// <summary>
        /// This Holds and Returns DetailResultList
        /// </summary>
        public List<DetailResultListItem> DetailResultList
        {
            get { return __detailResultsList; }
            set
            {
                __detailResultsList = value;
                this.NotifyPropertyChanged("DetailResultListList");
            }
        }

        /// <summary>
        /// This Holds and Returns CheckedDetailResultsList
        /// </summary>
        public List<DetailResultListItem> CheckedDetailResultsList
        {
            get
            {
                return __checkedDetailResultsList;
            }
            set
            {
                __checkedDetailResultsList = value;
                this.NotifyPropertyChanged("CheckedDetailResultsList");
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Set Detailresult Bindable Object Propeties
        /// </summary>
        private void BeginingRefresh()
        {
            if (this.HeaderInfo == null)
            {
                BeginHeaderInfoRefresh();
            }
            else
            {
                this.RetryProcessing = false;
                this.CancelProcessing = false;
                this.TotalTaskIdsCount = GetCount(ViewDetailsResponse.TaskId);
                this.ProcessedTaskIdsCount = 0;
                this.__lastProcessedTaskIdCount = 0;
                this.BeginRefresh(ViewDetailsResponse.TaskId);
            }
        }

        /// <summary>
        /// Prepare Service Call For DetailHeader Info
        /// </summary>
        private void BeginHeaderInfoRefresh()
        {
            HeaderInfoRequest headerInfoRequest = new HeaderInfoRequest();
            headerInfoRequest.TrackingNo = ViewDetailsResponse.TrackingNo;
            DetailHeaderInfoProxy headerInfoProxy = new DetailHeaderInfoProxy();
            headerInfoProxy.Invoke(headerInfoRequest, DetailHeaderInfoSVCCompleted);
        }
        
        /// <summary>
        /// Prepare DetailResult Service Call
        /// </summary>
        private void BeginRefresh(string taskIds)
        {
            DetailResultsListRequest detailResultsListRequest = new DetailResultsListRequest();
            detailResultsListRequest.TrackingItemNo = ViewDetailsResponse.TrackingItemNo;
            detailResultsListRequest.TrackingNo = ViewDetailsResponse.TrackingNo;

            detailResultsListRequest.TaskIds = taskIds;
            __sentTaskIds = taskIds;

            RefreshDetailResultsProxy refreshDetailResultsProxy = new RefreshDetailResultsProxy();
            refreshDetailResultsProxy.Invoke(detailResultsListRequest, DetailResultsServiceCompleted);
        }

        /// <summary>
        /// Retry Service Call
        /// </summary>
        private void BeginRetryRefresh()
        {
            RetryRequest retryRequest = new RetryRequest();
            retryRequest.RetryType = SharedConstants.RETRY_DETAILRESULT.ToCharArray()[0];
            retryRequest.TrackingNo = ViewDetailsResponse.TrackingNo;
            retryRequest.TrackingItemNo = ViewDetailsResponse.TrackingItemNo;

            RetryProxy retryProxy = new RetryProxy();
            retryProxy.Invoke(retryRequest, RetryServiceCompleted);
        }

        /// <summary>
        /// Prepare Service Call For DetailHeader Info
        /// </summary>
        public void BeginHeaderInfoReRefresh()
        {
            HeaderInfoRequest headerInfoRequest = new HeaderInfoRequest();
            headerInfoRequest.TrackingNo = ViewDetailsResponse.TrackingNo;
            DetailHeaderInfoProxy headerInfoProxy = new DetailHeaderInfoProxy();
            headerInfoProxy.Invoke(headerInfoRequest, DetailReHeaderInfoSVCCompleted);
        }

        /// <summary>
        /// Split The Task Id Seperated by '|'
        /// </summary>
        /// <param name="taskId">string</param>
        /// <returns>int</returns>
        private int GetCount(string taskId)
        {
            if (string.IsNullOrEmpty(taskId))
                return 0;

            string[] strArry = taskId.Split(new char[] { '|' });
            return strArry.Count();
        }

        /// <summary>
        /// Count The Remaining Task Id's
        /// </summary>
        /// <param name="processedTaskIDs">string</param>
        /// <returns>int</returns>
        private int GetRemainingTaskIdsCount(string processedTaskIDs)
        {
            int count = 0;
            string remainingTaskIds = string.Empty;

            List<string> remainingTaskIdsList = __sentTaskIds.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Except(processedTaskIDs.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
            foreach (string taskId in remainingTaskIdsList)
            {
                remainingTaskIds += taskId + "|";
            }

            remainingTaskIds = (remainingTaskIds.Length > 0) ? remainingTaskIds.Substring(0, remainingTaskIds.LastIndexOf("|")) : remainingTaskIds;
            count = remainingTaskIdsList.Count();

            __remainingTaskIds = remainingTaskIds;
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        public void BeginCancel(long trackingNo, int trackingItemNo)
        {
            OrderCancelRequest orderCancelRequest = new OrderCancelRequest();
            orderCancelRequest.TrackingNo = trackingNo;
            orderCancelRequest.TrackingItemNo = trackingItemNo;
            OrderCancelProxy orderCancelProxy = new OrderCancelProxy();
            orderCancelProxy.Invoke(orderCancelRequest, null);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Detail Header Info Event Handler
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:DetailHeaderInfoResponse</param>
        /// <returns>void</returns>
        private void DetailHeaderInfoSVCCompleted(DxProxy sender, DxCompleteEventArgs<DetailHeaderInfoResponse> args)
        {
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "HeaderInfo.svc"));
            }

            this.HeaderInfo = args.Response.HeaderInfo;
            BeginingRefresh();
        }

        /// <summary>
        /// Detail Header Info Event Handler
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:DetailHeaderInfoResponse</param>
        /// <returns>void</returns>
        private void DetailReHeaderInfoSVCCompleted(DxProxy sender, DxCompleteEventArgs<DetailHeaderInfoResponse> args)
        {
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "HeaderInfo.svc"));
            }

            this.HeaderInfo = args.Response.HeaderInfo;
            if (null != HIRefreshServiceCallCompleted)
                HIRefreshServiceCallCompleted(this, new RoutedEventArgs());
        }

        /// <summary>
        /// Detail Result Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:DetailResultsListResponse</param>
        /// <returns>void</returns>
        private void DetailResultsServiceCompleted(DxProxy sender, DxCompleteEventArgs<DetailResultsListResponse> args)
        {
            if (args.Error != null)
            {
                if (!CancelProcessing) ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                if (!CancelProcessing) throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "SummarySearchResultsListSVC"));
            }

            this.ProcessedTaskIds = args.Response.ProcessedTaskIDs;
            this.ProcessedTaskIdsCount += GetCount(args.Response.ProcessedTaskIDs);
            int remainingTaskIdsCount = GetRemainingTaskIdsCount(args.Response.ProcessedTaskIDs);

            if (!CancelProcessing)
            {
                if (null != DRBMServiceCallCompleted && ProcessedTaskIdsCount > __lastProcessedTaskIdCount)
                {
                    args.Response.DetailResultsListItems.ForEach(item => this.DetailResultList.Add(item));
                    this.DetailResultList = this.DetailResultList.OrderBy(x => x.SequenceNumber).ThenBy(x => x.PartySeqID).ToList(); //To maintain order same as summary results

                    DRBMServiceCallCompleted(this, new RoutedEventArgs());
                    __lastProcessedTaskIdCount = this.ProcessedTaskIdsCount;
                }

                if (ProcessedTaskIdsCount < TotalTaskIdsCount)
                    BeginRefresh(__remainingTaskIds);
            }

            if (this.TotalTaskIdsCount == this.ProcessedTaskIdsCount)
                if (null != ProcessingCompleted)
                    ProcessingCompleted(this, new RoutedEventArgs());
        }

        /// <summary>
        /// Detail Result Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:RetryResponse</param>
        /// <returns>void</returns>
        private void RetryServiceCompleted(DxProxy sender, DxCompleteEventArgs<RetryResponse> args)
        {
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "RetrySVC"));
            }

            if (args.Response.Status)
            {
                BeginingRefresh();
            }
        }

        #endregion

        #region Property Changed Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Proprty Changed Then Notify Event Handler
        /// </summary>
        /// <param name="info">String</param>
        /// <returns>void</returns>
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
