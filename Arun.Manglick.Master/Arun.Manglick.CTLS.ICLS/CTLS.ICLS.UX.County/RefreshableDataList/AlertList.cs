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
    public class AlertList : Refreshable<List<String>>
    {
        #region Method

        /// <summary>
        /// Prepare Service call for Alerts Messages
        /// </summary>
        protected override void BeginRefresh()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_ALERTS_LIST))
            {
                this.Data = IsolatedStorageSettings.ApplicationSettings[SharedConstants.CACHED_ALERTS_LIST] as List<string>;
                this.RefreshDone(this.Data);
            }
        }
        #endregion
    }
}
