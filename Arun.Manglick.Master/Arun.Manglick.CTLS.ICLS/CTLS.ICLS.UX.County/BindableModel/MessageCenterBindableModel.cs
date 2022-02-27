using System;
using System.Reflection;
using System.Windows;
using CT.SLABB.DX;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public class MessageCenterHeaderInfoBindableModel : BaseBindableModel<HeaderInfo>
    {
        #region .ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public MessageCenterHeaderInfoBindableModel()
        { }

        /// <summary>
        /// Paremetrised Constructor
        /// </summary>
        /// <param name="header">HeaderInfo</param>
        public MessageCenterHeaderInfoBindableModel(HeaderInfo header)
        {
            this.Data = header;
        }

        #endregion

        #region Variables
        public event RoutedEventHandler ServiceCallCompleted;
        #endregion

        #region Properties

        /// <summary>
        /// This Holds and Returns TrackingNo
        /// </summary>
        public long TrackingNo
        {
            get
            {
                return this.Data.TrackingNo;
            }
            set
            {
                this.Data.TrackingNo = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// This Holds and Returns CountyName
        /// </summary>
        public string CountyName
        {
            get
            {
                return this.Data.CountyName;
            }
            set
            {
                this.Data.CountyName = value;
            }
        }

        /// <summary>
        /// This Holds and Returns SearchKey
        /// </summary>
        public string SearchKey
        {
            get
            {
                return this.Data.SearchKey;
            }
            set
            {
                this.Data.SearchKey = value;
            }
        }

        /// <summary>
        /// This Holds and Returns CurrencyDate
        /// </summary>
        public string CurrencyDate
        {
            get
            {
                return this.Data.CurrencyDate;
            }
            set
            {
                this.Data.CurrencyDate = value;
            }
        }

        /// <summary>
        /// This Holds and Returns FromDate
        /// </summary>
        public string FromDate
        {
            get
            {
                return this.Data.FromDate;
            }
            set
            {
                this.Data.FromDate = value;
            }
        }

        /// <summary>
        /// This Holds and Returns ToDate
        /// </summary>
        public string ToDate
        {
            get
            {
                return this.Data.ToDate;
            }
            set
            {
                this.Data.ToDate = value;
            }
        }

        /// <summary>
        /// This Holds and Returns LienType
        /// </summary>
        public string LienType
        {
            get
            {
                return this.Data.LienType;
            }
            set
            {
                this.Data.LienType = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the Current Object
        /// </summary>
        /// <returns>object</returns>
        public override object GetCoreObject()
        {
            return this.Data;
        }

        /// <summary>
        /// Prepare The Service Call For Header Info
        /// </summary>
        /// <returns>void</returns>
        protected override void BeginRefresh()
        {
            HeaderInfoRequest headerInfoRequest = new HeaderInfoRequest();
            headerInfoRequest.TrackingNo = this.TrackingNo;
            HeaderInfoProxy headerInfoProxy = new HeaderInfoProxy();
            headerInfoProxy.Invoke(headerInfoRequest, HeaderInfoSVCCompleted);
        }

        #endregion

        #region Event Handlers
        /// <summary>
        /// Header Info Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:HeaderInfoResponse</param>
        /// <returns>void</returns>
        private void HeaderInfoSVCCompleted(DxProxy sender, DxCompleteEventArgs<HeaderInfoResponse> args)
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
            this.RefreshDone(args.Response.HeaderInfo);
            if (null != ServiceCallCompleted)
                ServiceCallCompleted(this, new RoutedEventArgs());
        }
        #endregion
    }
}
