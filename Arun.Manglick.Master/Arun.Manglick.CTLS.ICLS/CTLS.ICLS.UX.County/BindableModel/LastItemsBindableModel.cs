using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using CT.SLABB.DX;
using CT.SLABB.Utils;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public class LastItemsBindableModel : Bindable
    {
        #region Variables

        public long __teamId;
        public string __userNo;
        public List<LastListItem> __lastItemList;

        public event RoutedEventHandler LIBMServiceCallCompleted;

        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public LastItemsBindableModel()
        { }
        #endregion

        #region Properties

        /// <summary>
        /// This Holds and Returns LastItemList
        /// </summary>
        public List<LastListItem> LastItemList
        {
            get { return __lastItemList; }
            set
            {
                __lastItemList = value;
                this.NotifyPropertyChanged("LastItemList");
            }
        }

        /// <summary>
        /// This Holds and Returns TeamId
        /// </summary>
        public long TeamId
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
                BeginRefresh();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare Service Call For Last Items List
        /// </summary>        
        public void BeginRefresh()
        {
            // Prepare Request             
            LastItemsRequest lastItemsRequest = new LastItemsRequest();
            lastItemsRequest.TeamID = this.TeamId;
            lastItemsRequest.UserNo = this.UserNo;
            LastItemsListProxy lastItemsListProxy = new LastItemsListProxy();
            lastItemsListProxy.Invoke(lastItemsRequest, LastItemsServiceCompleted);
        }

        #endregion

        #region Event Handlers
        /// <summary>
        /// Last Items List Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:LastItemsListResponse</param>
        /// <returns>void</returns>
        private void LastItemsServiceCompleted(DxProxy sender, DxCompleteEventArgs<LastItemsListResponse> args)
        {
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "PortfolioResultListSvc"));
            }

            this.LastItemList = args.Response.LastItemList;

            if (null != LIBMServiceCallCompleted)
                LIBMServiceCallCompleted(this, new RoutedEventArgs());
        }
        #endregion
    }

}
