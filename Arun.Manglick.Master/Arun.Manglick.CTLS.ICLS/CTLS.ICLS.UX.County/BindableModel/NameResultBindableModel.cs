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
    public class NameResultBindableModel : BaseBindableModel, INotifyPropertyChanged
    {
        #region Variables

        public event RoutedEventHandler BMServiceCallCompleted;
        private long __trackingNo;
        private long __orderNo;
        private List<NameSearchResult> __nameList;
        private List<NameSearchResult> __checkedNameList;
        private bool __moreRecordsAvailable;

        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public NameResultBindableModel()
        {
            CheckedNameList = new List<NameSearchResult>();
        }
        #endregion

        #region Properties

        #region Other Properties

        /// <summary>
        /// This Holds and Returns NameList
        /// </summary>
        public List<NameSearchResult> NameList
        {
            get
            {
                return __nameList;
            }
            set
            {
                __nameList = value;
            }
        }

        /// <summary>
        /// This Holds and Returns MoreRecordsAvailable
        /// </summary>
        public bool MoreRecordsAvailable
        {
            get
            {
                return __moreRecordsAvailable;
            }
            set
            {
                __moreRecordsAvailable = value;
            }
        }

        /// <summary>
        /// This Holds and Returns MoreRecordsAvailable
        /// </summary>
        public string SearchedOldTab { get; set; }
        
        /// <summary>
        /// This Holds and Returns CheckedNameList
        /// </summary>
        public List<NameSearchResult> CheckedNameList
        {
            get { return __checkedNameList; }
            set { __checkedNameList = value; }
        }

        /// <summary>
        /// This Holds and Returns UserContext
        /// </summary>
        public UserContext UserContext { get; set; }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Prepare The Service Call For Header Info
        /// </summary>
        /// <returns>void</returns>
        public void BeginRefresh(long trackingNo)
        {
            HeaderInfoRequest headerInfoRequest = new HeaderInfoRequest();
            headerInfoRequest.TrackingNo = trackingNo;
            HeaderInfoProxy headerInfoProxy = new HeaderInfoProxy();
            headerInfoProxy.Invoke(headerInfoRequest, NameResultHeaderInfoSVCCompleted);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Name Result Header Info Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:HeaderInfoResponse</param>
        /// <returns>void</returns>
        private void NameResultHeaderInfoSVCCompleted(DxProxy sender, DxCompleteEventArgs<HeaderInfoResponse> args)
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
            this.NameList = args.Response.NameSearchResults;
            this.MoreRecordsAvailable = args.Response.MoreRecordsAvailable;
            this.SearchedOldTab = args.Response.SearchedOldTab;
            if (null != BMServiceCallCompleted)
                BMServiceCallCompleted(this, new RoutedEventArgs());
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
