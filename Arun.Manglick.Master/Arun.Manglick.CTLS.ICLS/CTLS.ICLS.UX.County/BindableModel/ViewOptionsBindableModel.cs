using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using CT.SLABB.DX;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public class ViewOptionsBindableModel : INotifyPropertyChanged
    {
        #region Variables
        public event RoutedEventHandler VOBMServiceCallCompleted;
        #endregion

        #region .ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public ViewOptionsBindableModel()
        { }
        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public long TrackingNo { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int TrackingItemNo { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string UniqueKey { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string FileNumber { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string CountyName { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string CountyCode { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string StateCode { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public UserContext UserContext { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public ViewImageCost ViewImageCost { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare The Service Call For View Image Cost
        /// </summary>
        /// <returns>void</returns>
        public void BeginRefresh()
        {
            // Prepare Request             
            ViewImageCostRequest viewImageCostRequest = new ViewImageCostRequest();
            viewImageCostRequest.TrackingNo = this.TrackingNo;
            viewImageCostRequest.TrackingItemNo = this.TrackingItemNo;
            viewImageCostRequest.UniqueKey = this.UniqueKey;

            ViewImageCostProxy viewImageCostProxy = new ViewImageCostProxy();
            viewImageCostProxy.Invoke(viewImageCostRequest, ViewImageCostResultsServiceCompleted);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// View Image Cost Results Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:ViewImageCostResponse</param>
        /// <returns>void</returns>
        private void ViewImageCostResultsServiceCompleted(DxProxy sender, DxCompleteEventArgs<ViewImageCostResponse> args)
        {
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "ViewImageCostSVC"));
            }

            this.ViewImageCost = args.Response.ViewImageCost;

            if (null != VOBMServiceCallCompleted)
                VOBMServiceCallCompleted(this, new RoutedEventArgs());
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
