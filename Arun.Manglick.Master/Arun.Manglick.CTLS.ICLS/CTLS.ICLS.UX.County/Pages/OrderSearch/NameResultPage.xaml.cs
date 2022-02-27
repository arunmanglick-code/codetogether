using System.Windows;
using CTLS.ICLS.UX.Controls;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class NameResultPage : CPPage
    {
        #region .ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public NameResultPage()
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
            this.HeaderNavImageVisibilty = Visibility.Visible;
            this.HeaderNavDetailsVisibilty = Visibility.Visible;
        }
        #endregion
    }
}
