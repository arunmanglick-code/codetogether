using System.Windows;
using System.Windows.Navigation;
using CTLS.ICLS.UX.Controls;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class PortfolioPage : CPPage
    {
        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public PortfolioPage()
        {
            InitializeComponent();
            RegisterViews();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Register Views
        /// </summary>
        private void RegisterViews()
        {
            this.PortfolioFrame.RegisterView(ViewConstants.PORTFOLIOFULLVIEW, typeof(PortfolioFullView));

            this.HeaderNavImageVisibilty = Visibility.Visible;
            this.HeaderNavDetailsVisibilty = Visibility.Visible;
        }
        #endregion       
    }
}
