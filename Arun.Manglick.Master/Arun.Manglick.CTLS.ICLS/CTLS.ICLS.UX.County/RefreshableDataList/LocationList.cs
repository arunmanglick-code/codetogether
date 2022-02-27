using System;
using System.Collections.Generic;
using System.Reflection;
using CT.SLABB.Data;
using CT.SLABB.DX;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;
using System.IO.IsolatedStorage;

namespace CTLS.ICLS.UX.CountySearch
{
    public class LocationList : Refreshable<List<LocationListItem>>
    {
        #region Private Variables
        private List<LocationListItem> locationList;

        #endregion

        #region Methods
        /// <summary>
        /// Prepare Service call for Location List
        /// </summary>
        protected override void BeginRefresh()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_LOCATION_LIST))
            {
                locationList = IsolatedStorageSettings.ApplicationSettings[SharedConstants.CACHED_LOCATION_LIST] as List<LocationListItem>;
                this.RefreshDone(locationList);
            }
        }
        #endregion
    }
}
