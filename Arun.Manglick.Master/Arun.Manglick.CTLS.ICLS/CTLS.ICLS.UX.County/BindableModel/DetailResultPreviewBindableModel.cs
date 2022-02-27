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
    public class DetailResultPreviewBindableModel : BaseBindableModel, INotifyPropertyChanged
    {
        #region Variables

        private string __taskids;
        private List<DetailResultListItem> __detailResultsList;
        private List<DetailResultListItem> __checkedDetailResultsList;
        private List<DetailResultListItem> __previewDetailResultsList;
        private List<DetailsPreviewHeader> __detailsPreview;
        
        #endregion

        #region Events
        public event RoutedEventHandler DRPBMServiceCallCompleted;
        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public DetailResultPreviewBindableModel()
        {
            __detailResultsList = new List<DetailResultListItem>();
            __checkedDetailResultsList = new List<DetailResultListItem>();
            __previewDetailResultsList = new List<DetailResultListItem>();
            __detailsPreview = new List<DetailsPreviewHeader>();

        }
        #endregion

        #region Properties
        
        #region Other Properties

        /// <summary>
        /// 
        /// </summary>
        public int TrackingItemNo { get; set; }

        /// <summary>
        /// This Holds and Returns TaskIds
        /// </summary>
        public string TaskIds
        {
            get { return __taskids; }
            set
            {
                __taskids = value;
                BeginRefresh(__taskids);
            }
        }

        /// <summary>
        /// This Holds and Returns UserContext
        /// </summary>
        public UserContext UserContext { get; set; }

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
        /// This Holds and Returns PreviewDetailResultList
        /// </summary>
        public List<DetailResultListItem> PreviewDetailResultList
        {
            get
            { return __previewDetailResultsList; }

            set
            {
                __previewDetailResultsList = value;
            }
        }

        /// <summary>
        /// This Holds and Returns DetailsPreview
        /// </summary>
        public List<DetailsPreviewHeader> DetailsPreview
        {
            get { return __detailsPreview; }
            set { __detailsPreview = value; }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Prepare Service Call for Detail Results List
        /// </summary>
        /// <param name="taskIds">taskIds</param>
        private void BeginRefresh(string taskIds)
        {
            // Prepare Request             
            DetailResultsListRequest detailResultsListRequest = new DetailResultsListRequest();
            detailResultsListRequest.TrackingNo = this.HeaderInfo.TrackingNo;
            detailResultsListRequest.TrackingItemNo = this.TrackingItemNo;
            detailResultsListRequest.TaskIds = taskIds;

            PreviewDetailResultsProxy previewDetailResultsProxy = new PreviewDetailResultsProxy();
            previewDetailResultsProxy.Invoke(detailResultsListRequest, DetailResultsServiceCompleted);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Detail Result List Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:DetailResultsListResponse</param>
        /// <returns>void</returns>
        private void DetailResultsServiceCompleted(DxProxy sender, DxCompleteEventArgs<DetailResultsListResponse> args)
        {
            string count = string.Empty;
            int skipcount = 0;
            int insertIndex = 0;
            List<DetailResultListItem> tempPreviewDetailResultListDetailResultList = new List<DetailResultListItem>();
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "SummarySearchResultsListSVC"));
            }

            this.DetailResultList.Clear();
            this.DetailsPreview.Clear();
            args.Response.DetailResultsListItems.ForEach(item => this.DetailResultList.Add(item));

            this.DetailResultList = this.DetailResultList.OrderBy(x => x.LienType).ThenBy(x => x.SequenceNumber).ToList();
            PreviewDetailResultList.Clear();
            List<string> lstLienType = DetailResultList.Select(x => x.LienType).Distinct().ToList<string>();

            // To Make Count List Per LienType
            lstLienType.ForEach(delegate(string lienType)
            {
                count = DetailResultList.Where(x => x.LienType == lienType).Select(y => y.FileNumber).Distinct().ToList<string>().Count.ToString();
                DetailsPreview.Add(new DetailsPreviewHeader { LienType = lienType, Count = count });
            });

            // To Make DataGrid Source
            lstLienType.ForEach(delegate(string lienType)
            {
                PreviewDetailResultList.Add(new DetailResultListItem { FileNumber = String.Empty, DocumentType = lienType });
                skipcount = PreviewDetailResultList.Count;

                DetailResultList.ForEach(delegate(DetailResultListItem x)
                {
                    if (x.LienType.Equals(lienType)) tempPreviewDetailResultListDetailResultList.Add(x);
                });

                tempPreviewDetailResultListDetailResultList.ForEach(item => this.PreviewDetailResultList.Add(item));
                tempPreviewDetailResultListDetailResultList.Clear();

                // Below code is to insert blank line between two sets belonging to different 'FileNumber'
                // --------------------------------------------------------------------------------------------
                List<DetailResultListItem> skippedList = PreviewDetailResultList.Skip(skipcount).ToList();
                string fileNumber = skippedList[0].FileNumber;
                string tempFileNumber;
                insertIndex = skipcount;

                skippedList.ForEach(delegate(DetailResultListItem x)
                {
                    tempFileNumber = x.FileNumber;
                    if (!tempFileNumber.Equals(fileNumber))
                    {
                        PreviewDetailResultList.Insert(insertIndex, new DetailResultListItem());
                        fileNumber = tempFileNumber;
                        insertIndex++;
                    }
                    insertIndex++;
                });
                // --------------------------------------------------------------------------------------------
                PreviewDetailResultList.Insert(insertIndex, new DetailResultListItem());
            });

            if (null != DRPBMServiceCallCompleted) DRPBMServiceCallCompleted(this, new RoutedEventArgs());
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

    public class DetailsPreviewHeader
    {
        #region Properties
        public string LienType { get; set; }
        public string Count { get; set; }
        #endregion
    }

}
