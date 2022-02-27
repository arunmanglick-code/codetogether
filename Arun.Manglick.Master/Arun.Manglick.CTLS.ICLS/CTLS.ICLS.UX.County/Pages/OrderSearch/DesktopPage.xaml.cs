using System;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Navigation;
using CTLS.ICLS.UX.Controls;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class DesktopPage : CPPage
    {
        #region .ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public DesktopPage()
        {
            InitializeComponent();
            RegisterViews();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Register all views
        /// </summary>
        private void RegisterViews()
        {
            this.DesktopFrame.RegisterView(ViewConstants.DESKTOPVIEW, typeof(DesktopView));
            this.DesktopFrame.RegisterView(ViewConstants.NAMERESULTVIEW, typeof(NameResultView));
            this.DesktopFrame.RegisterView(ViewConstants.SUMMARYSEARCHRESULTVIEW, typeof(SummarySearchResultView));
            this.DesktopFrame.RegisterView(ViewConstants.DETAILRESULTSVIEW, typeof(DetailResultsView));
            this.DesktopFrame.RegisterView(ViewConstants.ORDERDETAILSVIEW, typeof(OrderDetailsView));
            this.DesktopFrame.RegisterView(ViewConstants.ORDERCONFIRMATIONVIEW, typeof(OrderConfirmationView));
            this.DesktopFrame.RegisterView(ViewConstants.ORDERCOMPLETEDVIEW, typeof(OrderCompletedView));

            this.HeaderNavImageVisibilty = Visibility.Visible;
            this.HeaderNavDetailsVisibilty = Visibility.Visible;
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// navigate To Event Handle
        /// </summary>
        /// <param name="e">NavigationEventArgs</param>    
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string trackingNo = string.Empty;
            string viewName = string.Empty;

            if (this.NavigationContext.QueryString.ContainsKey(QueryStringConstants.TARGET_VIEW) && this.NavigationContext.QueryString.ContainsKey(QueryStringConstants.TRACKING_NO))
            {
                this.NavigationContext.QueryString.TryGetValue(QueryStringConstants.TARGET_VIEW, out viewName);
                this.DesktopFrame.DefaultView = viewName;
            }

            if (IsolatedStorageSettings.SiteSettings.Contains(SharedConstants.ONERROR_BACKTOPAGE))
            {                
                IsolatedStorageSettings.SiteSettings.Remove(SharedConstants.ONERROR_BACKTOPAGE);
                HtmlPage.Window.Navigate(new Uri(URIConstants.URL_PORTFOLIO, UriKind.Relative), URIConstants.CONST_self);
            }
        }

        #endregion
    }
}
