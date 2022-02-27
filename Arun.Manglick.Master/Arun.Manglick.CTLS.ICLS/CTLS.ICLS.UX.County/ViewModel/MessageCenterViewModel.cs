using System.Windows;
using CT.SLABB.ClientState;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared;

namespace CTLS.ICLS.UX.CountySearch
{
    public class MessageCenterViewModel : BaseViewModel
    {
        #region Variables

        public event RoutedEventHandler BMServiceCallCompleted;
        private MessageCenterHeaderInfoBindableModel __messageCenterHeaderInfoBM;

        private long __trackingNo;
        private bool __isNew;

        #endregion

        #region .ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public MessageCenterViewModel()
        {
            this.MessageCenterHeaderInfoBM = new MessageCenterHeaderInfoBindableModel(new HeaderInfo());
            this.MessageCenterHeaderInfoBM.ServiceCallCompleted += new RoutedEventHandler(MessageCenterHeaderInfoBM_ServiceCallCompleted);
        }
        #endregion

        #region Properties

        /// <summary>
        /// MessageCenterHeaderInfoBM Read Write Property
        /// </summary>
        public MessageCenterHeaderInfoBindableModel MessageCenterHeaderInfoBM
        {
            get
            {
                return __messageCenterHeaderInfoBM;
            }
            set
            {
                this.__messageCenterHeaderInfoBM = value;
                this.NotifyPropertyChanged("HeaderInfoBM");
            }
        }

        /// <summary>
        /// TrackingNo Read Write Property
        /// </summary>
        public long TrackingNo
        {
            get
            {
                return __trackingNo;
            }
            set
            {
                this.__trackingNo = value;
                this.MessageCenterHeaderInfoBM.TrackingNo = value; //here service will call which will retrive user data
            }
        }

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

        #region Event Handlers

        /// <summary>
        /// Message Center HeaderInfo Service Call Completed
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">RoutedEventArgs</param>
        /// <returns>void</returns>
        private void MessageCenterHeaderInfoBM_ServiceCallCompleted(object sender, RoutedEventArgs e)
        {
            //this.NotifyPropertyChanged("userBM");
            if (null != BMServiceCallCompleted)
                BMServiceCallCompleted(this, new RoutedEventArgs());
        }
        #endregion
    }

    public class MessageCenterViewModelConnector : ViewStateConnector<MessageCenterViewModel>
    {
        #region .ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public MessageCenterViewModelConnector()
        {
        }
        #endregion
    }
}
