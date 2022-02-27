using System;
using System.ComponentModel;
using System.Reflection;
using CT.SLABB.Data;
using CT.SLABB.DX;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.ClientConfig;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public class OrderSearchDataProvider : DataProvider
    {
        #region Private Varialbes
        public EventHandler OnServiceCompleted;
        #endregion

        #region .ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSearchDataProvider()
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Preapare Message Center Service Call
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="searchPredicates">FilterDescriptionCollection</param>
        /// <param name="sortPredicates">SortDescriptionCollection</param>
        /// <param name="max">int</param>
        protected override void BeginLoad(object sender, FilterDescriptionCollection searchPredicates, SortDescriptionCollection sortPredicates, int max)
        {
            MessageCenterRequest messageCenterRequest = new MessageCenterRequest();
            messageCenterRequest.UserId = ClientConfigUtil.GetLoggedInUserContext().UserId;
            MessageCenterListProxy proxy = new MessageCenterListProxy();
            proxy.Invoke(messageCenterRequest, MessageCenterSVCCompleted);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Message Center Service Call Completed
        /// </summary>
        /// <param name="sender">DxProxy</param>
        /// <param name="args">DxCompleteEventArgs:MessageCenterListResponse</param>
        /// <returns>void</returns>
        private void MessageCenterSVCCompleted(DxProxy sender, DxCompleteEventArgs<MessageCenterListResponse> args)
        {
            if (args.Error != null)
            {
                ErrorHandler.ExceptionOccured(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, this.GetType(), args.Error);
                return;
            }

            if (args.Response == null)
            {
                throw new Exception(string.Format(MessageConstants.COMMON_EXCEPTION_ERROR_NULL_RESPONSE, "UserListSVC"));
            }

            this.Items = args.Response.MessageCenterList;
            if (OnServiceCompleted != null) OnServiceCompleted(sender, args); // Required to stop Refresh Indicator
        }

        #endregion
    }
}
