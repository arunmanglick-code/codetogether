using CTLS.ICLS.UX.Controls;
using System.IO.IsolatedStorage;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public partial class CountySearchMain: CPModule
    {
        #region .ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public CountySearchMain()
        {
            InitializeComponent();
            this.Exit+=new System.EventHandler(CountySearchMain_Exit);
        }
        #endregion
        private void CountySearchMain_Exit(object sender, System.EventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_COPYCOST_LIST))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(SharedConstants.CACHED_COPYCOST_LIST);
            }
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_SEARCH_INSTRUCTIONS_LIST))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(SharedConstants.CACHED_SEARCH_INSTRUCTIONS_LIST);
            }
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_ALERTS_LIST))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(SharedConstants.CACHED_ALERTS_LIST);
            }
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_LOCATION_LIST))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(SharedConstants.CACHED_LOCATION_LIST);
            }
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_COUNTY_LIST))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(SharedConstants.CACHED_COUNTY_LIST);
            }
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_LIENTYPE_LIST))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(SharedConstants.CACHED_LIENTYPE_LIST);
            }
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_TEAM_LIST))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(SharedConstants.CACHED_TEAM_LIST);
            }
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_CUSTOMERSPECIALIST_LIST))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(SharedConstants.CACHED_CUSTOMERSPECIALIST_LIST);
            }
        }

    }
}
