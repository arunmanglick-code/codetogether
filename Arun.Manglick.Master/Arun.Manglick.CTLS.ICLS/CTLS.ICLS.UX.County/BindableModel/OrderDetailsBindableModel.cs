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
    public class OrderDetailsBindableModel : Bindable
    {
        #region Variables

        private char __isCustomReportAvailable;
        private long __trackingNo;
        private string __jurisdiction;
        private long __orderNo;
        private int __orderItemNo;
        private List<OrderDetail> __orderDetail;
        public event RoutedEventHandler ODServiceCallCompleted;
        public event RoutedEventHandler ODServiceCallProcessingCompleted;

        #endregion

        #region Events
        public event RoutedEventHandler OrderReportResultServiceCallCompleted;
        #endregion

        #region .ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDetailsBindableModel()
        { }
        #endregion

        #region Properties

        /// <summary>
        /// This Holds and Returns IsCustomReportAvailable
        /// </summary>
        public char IsCustomReportAvailable
        {
            get
            {
                return this.__isCustomReportAvailable;
            }
            set
            {
                this.__isCustomReportAvailable = value;
                this.NotifyPropertyChanged("IsCustomReportAvailable");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public char IsNoRecordReportAvailable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public char IsOriginalReportAvailable { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public char IsImageAvailable { get; set; }        

        /// <summary>
        /// This Holds and Returns Jurisdiction
        /// </summary>
        public string Jurisdiction
        {
            get
            {
                return this.__jurisdiction;
            }
            set
            {
                this.__jurisdiction = value;
                this.NotifyPropertyChanged("Jurisdiction");
            }
        }

        /// <summary>
        /// This Holds and Returns OrderNo
        /// </summary>
        public long OrderNo
        {
            get
            {
                return this.__orderNo;
            }
            set
            {
                this.__orderNo = value;
                this.NotifyPropertyChanged("OrderNo");
            }
        }

        /// <summary>
        /// This Holds and Returns OrderItemNo
        /// </summary>
        public int OrderItemNo
        {
            get
            {
                return this.__orderItemNo;
            }
            set
            {
                this.__orderItemNo = value;
                this.NotifyPropertyChanged("OrderItemNo");
            }
        }

        /// <summary>
        /// This Holds and Returns TrackingNo
        /// </summary>
        public long TrackingNo
        {
            get
            {
                return this.__trackingNo;
            }
            set
            {
                this.__trackingNo = value;
                this.NotifyPropertyChanged("TrackingNo");
                this.BeginRefresh();
            }
        }

        /// <summary>
        /// This Holds and Returns OrderDetail
        /// </summary>
        public List<OrderDetail> OrderDetail
        {
            get
            {
                return __orderDetail;
            }
            set
            {
                this.__orderDetail = value;
            }
        }

        /// <summary>
        /// This Holds and Returns UserContext
        /// </summary>
        public UserContext UserContext { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare The Service Call For Order Detial
        /// </summary>
        /// <returns>void</returns>
        public void BeginRefresh()
        {
            OrderDetailRequest orderDetailRequest = new OrderDetailRequest();
            orderDetailRequest.TrackingNo = this.__trackingNo;
            OrderDetailsResultListProxy orderDetailsResultListProxy = new OrderDetailsResultListProxy();
            orderDetailsResultListProxy.Invoke(orderDetailRequest, OrderDetailsResultSVCCompleted);
        }

        /// <summary>
        /// Prepare The Service Call For Order Detial
        /// </summary>
        /// <returns>void</returns>
        public void BeginRefresh(long trackingNo)
        {
            OrderDetailRequest orderDetailRequest = new OrderDetailRequest();
            orderDetailRequest.TrackingNo = trackingNo;
            OrderDetailsResultListProxy orderDetailsResultListProxy = new OrderDetailsResultListProxy();
            orderDetailsResultListProxy.Invoke(orderDetailRequest, OrderDetailsResultSVCCompletedCustomReportCheck);
        }

        /// <summary>
        /// 
        /// </summary>
        public void BeginReportResult()
        {
            OrderReportResultProxy orderReportResultProxy = new OrderReportResultProxy();
            OrderReportRequest orderReportRequest = new OrderReportRequest();
            orderReportRequest.TrackingNo = this.TrackingNo;
            orderReportResultProxy.Invoke(orderReportRequest, OrderReportResultSVCCompleted);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// order Detail Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:OrderDetailResponse</param>
        /// <returns>void</returns>
        private void OrderDetailsResultSVCCompleted(DxProxy sender, DxCompleteEventArgs<OrderDetailResponse> args)
        {
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "OrderDetailsResultList.svc"));
            }
            this.OrderNo = args.Response.OrderNo;
            this.OrderItemNo = args.Response.OrderItemNo;
            this.Jurisdiction = args.Response.Jurisdiction;

            this.IsCustomReportAvailable = args.Response.IsCustomReportAvailable;
            this.IsNoRecordReportAvailable = args.Response.IsNoRecordReportAvailable;
            this.IsOriginalReportAvailable = args.Response.IsOriginalReportAvailable;
            this.IsImageAvailable = args.Response.IsImageAvailable;

            this.OrderDetail = args.Response.OrderDetails;
            if (null != ODServiceCallCompleted)
                ODServiceCallCompleted(this, new RoutedEventArgs());
        }

        /// <summary>
        /// order Detail Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:OrderDetailResponse</param>
        /// <returns>void</returns>
        private void OrderDetailsResultSVCCompletedCustomReportCheck(DxProxy sender, DxCompleteEventArgs<OrderDetailResponse> args)
        {
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "OrderDetailsResultList.svc"));
            }

            this.IsCustomReportAvailable = args.Response.IsCustomReportAvailable;
            this.IsNoRecordReportAvailable = args.Response.IsNoRecordReportAvailable;
            this.IsOriginalReportAvailable = args.Response.IsOriginalReportAvailable;
            this.IsImageAvailable = args.Response.IsImageAvailable;

            if (IsCustomReportAvailable.Equals(SharedConstants.NO.ToCharArray()[0]))
                BeginRefresh(args.Response.TrackingNo);

            if (null != ODServiceCallCompleted && IsCustomReportAvailable.Equals(SharedConstants.YES.ToCharArray()[0]))
                ODServiceCallCompleted(this, new RoutedEventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private void OrderReportResultSVCCompleted(DxProxy sender, DxCompleteEventArgs<OrderReportResponse> args)
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

            this.IsCustomReportAvailable = args.Response.CustomReportExists;
            if (null != OrderReportResultServiceCallCompleted)
                OrderReportResultServiceCallCompleted(this, new RoutedEventArgs());
        }

        #endregion
    }
}
