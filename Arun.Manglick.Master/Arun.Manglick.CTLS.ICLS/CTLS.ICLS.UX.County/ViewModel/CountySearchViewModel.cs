using System.Windows;
using CT.SLABB.ClientState;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared;

namespace CTLS.ICLS.UX.CountySearch
{
    public class CountySearchViewModel : BaseViewModel
    {
        #region Variables

        private NameResultBindableModel __nameResultBindableModel;
        private SummarySearchResultBindableModel __summarySearchResultBindableModel;
        private DetailResultBindableModel __detailResultBindableModel;
        private OrderConfirmationBindableModel __orderConfirmationBindableModel;
        private ViewOptionsBindableModel __viewOptionsBindableModel;
        private DetailResultPreviewBindableModel __detailResultPreviewBindableModel;
        private OrderDetailsBindableModel __orderDetailsBindableModel;
        private OrderCompletedBindableModel __orderCompletedBindableModel;
        private long __trackingNo;
        private bool __isNew;

        #endregion

        #region Events

        public event RoutedEventHandler VMRefreshCompleted;

        #endregion

        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public CountySearchViewModel()
        { }
        #endregion

        #region Properties

        /// <summary>
        /// NameResultBindableModel Read Write Property
        /// </summary>
        public NameResultBindableModel NameResultBindableModel
        {
            get
            {
                return __nameResultBindableModel;
            }
            set
            {
                this.__nameResultBindableModel = value;
                this.NotifyPropertyChanged("NameResultBindableModel");
            }
        }

        /// <summary>
        /// SummarySearchResultBindableModel Read Write Property
        /// </summary>
        public SummarySearchResultBindableModel SummarySearchResultBindableModel
        {
            get
            {
                return __summarySearchResultBindableModel;
            }
            set
            {
                this.__summarySearchResultBindableModel = value;
                this.NotifyPropertyChanged("SummarySearchResultBindableModel");
            }
        }

        /// <summary>
        /// DetailResultBindableModel Read Write Property
        /// </summary>
        public DetailResultBindableModel DetailResultBindableModel
        {
            get { return __detailResultBindableModel; }
            set
            {
                __detailResultBindableModel = value;
                this.NotifyPropertyChanged("DetailResultBindableModel");
            }
        }

        /// <summary>
        /// OrderConfirmationBindableModel Read Write Property
        /// </summary>
        public OrderConfirmationBindableModel OrderConfirmationBindableModel
        {
            get { return __orderConfirmationBindableModel; }
            set
            {
                __orderConfirmationBindableModel = value;
                this.NotifyPropertyChanged("OrderConfirmationBindableModel");
            }
        }

        /// <summary>
        /// ViewOptionsBindableModel Read Write Property
        /// </summary>
        public ViewOptionsBindableModel ViewOptionsBindableModel
        {
            get { return __viewOptionsBindableModel; }
            set
            {
                __viewOptionsBindableModel = value;
                this.NotifyPropertyChanged("ViewOptionsBindableModel");
            }
        }

        /// <summary>
        /// DetailResultPreviewBindableModel Read Write Property
        /// </summary>
        public DetailResultPreviewBindableModel DetailResultPreviewBindableModel
        {
            get { return __detailResultPreviewBindableModel; }
            set
            {
                __detailResultPreviewBindableModel = value;
                this.NotifyPropertyChanged("DetailResultPreviewBindableModel");
            }
        }

        /// <summary>
        /// OrderDetailsBindableModel Read Write Property
        /// </summary>
        public OrderDetailsBindableModel OrderDetailsBindableModel
        {
            get
            {
                return __orderDetailsBindableModel;
            }
            set
            {
                this.__orderDetailsBindableModel = value;
                this.NotifyPropertyChanged("OrderDetailsBindableModel");
            }
        }

        /// <summary>
        /// OrderCompletedBindableModel Read Write Property
        /// </summary>
        public OrderCompletedBindableModel OrderCompletedBindableModel
        {
            get
            {
                return __orderCompletedBindableModel;
            }
            set
            {
                this.__orderCompletedBindableModel = value;
                this.NotifyPropertyChanged("OrderCompletedBindableModel");
            }
        }

        ///// <summary>
        ///// TrackingNo Read Write Property
        ///// </summary>
        //public long TrackingNo
        //{
        //    get
        //    {
        //        return __trackingNo;
        //    }
        //    set
        //    {
        //        this.__trackingNo = value;
        //        this.NameResultBindableModel.TrackingNo = value;
        //    }
        //}

        /// <summary>
        /// IsNew Read Write Property
        /// </summary>
        public bool IsNew
        {
            get
            {
                return __isNew;
            }
            set
            {
                this.__isNew = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set All BindableModel To Null When New Search Button Is Clicked
        /// </summary>
        /// <returns>void</returns>
        public void ClearAll()
        {
            NameResultBindableModel = null;
            SummarySearchResultBindableModel = null;
            DetailResultBindableModel = null;
            OrderConfirmationBindableModel = null;
            ViewOptionsBindableModel = null;
            DetailResultPreviewBindableModel = null;
            OrderDetailsBindableModel = null;
        }

        /// <summary>
        /// Set All BindableModel To Null When Submit Button Clicked
        /// </summary>
        /// <returns>void</returns>
        public void ClearAtSubmit()
        {
            NameResultBindableModel = null;
            SummarySearchResultBindableModel = null;
            DetailResultBindableModel = null;
            ViewOptionsBindableModel = null;
            DetailResultPreviewBindableModel = null;
            OrderDetailsBindableModel = null;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Name Result Service Call Completed
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void NameResultBindableModel_ServiceCallCompleted(object sender, RoutedEventArgs e)
        {
            if (null != VMRefreshCompleted)
                VMRefreshCompleted(this, new RoutedEventArgs());
        }

        #endregion
    }

    public class CountySearchViewModelConnector : ViewStateConnector<CountySearchViewModel>
    {
        
    }
}
