using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using CT.SLABB.Data;
using CT.SLABB.DX;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;
using System.IO.IsolatedStorage;

namespace CTLS.ICLS.UX.CountySearch
{
    public class TeamList : Refreshable<List<CommonTeamListItem>>
    {
        #region Variables
        public event RoutedEventHandler TeamListCompleted;
        List<CommonTeamListItem> teamListItem = new List<CommonTeamListItem>();
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Prepare Service call for Team List
        /// </summary>
        protected override void BeginRefresh()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_TEAM_LIST))
            {
                teamListItem = IsolatedStorageSettings.ApplicationSettings[SharedConstants.CACHED_TEAM_LIST] as List<CommonTeamListItem>;
                this.RefreshDone(teamListItem);
            }
        }
        #endregion        
    }
}
