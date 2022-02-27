using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using CT.SLABB.Data;
using CT.SLABB.DX;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;
using System.IO.IsolatedStorage;

namespace CTLS.ICLS.UX.CountySearch
{
    public class CountyList : Refreshable<List<CountyListItem>>, INotifyPropertyChanged
    {
        #region Private Variables

        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(string), typeof(CountyList), null);

        private List<CountyListItem> _filteredData;
        List<CountyListItem> CountyListItems = null;

        #endregion

        #region Constructor
        public CountyList()
        {
            List<CountyListItem> countylst = new List<CountyListItem>() { new CountyListItem { CountyCode = Convert.ToInt16(SharedConstants.SELECT_ONE_KEY), CountyName = SharedConstants.SELECT_ONE_VALUE, StateCode = SharedConstants.SELECT_ONE_KEY } };
            this.FilteredData = countylst;
        }
        #endregion

        #region Method
        /// <summary>
        /// Prepare Service call for County's
        /// </summary>
        protected override void BeginRefresh()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_COUNTY_LIST))
            {
                CountyListItems = IsolatedStorageSettings.ApplicationSettings[SharedConstants.CACHED_COUNTY_LIST] as List<CountyListItem>;
                this.RefreshDone(CountyListItems);
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Location Read Write Property
        /// </summary>
        public string Location
        {
            get
            {
                return (string)this.GetValue(LocationProperty);
            }
            set
            {
                this.SetValue(LocationProperty, value);
                if (this.Data != null)
                {
                    this.FilteredData = this.Data.Where(t => t.StateCode.Equals(value)).ToList();
                    this.OnPropertyChanged("FilteredData");
                }
            }
        }

        /// <summary>
        /// FilteredData Read Write Property
        /// </summary>
        public List<CountyListItem> FilteredData
        {
            get { return _filteredData; }
            set
            {
                _filteredData = value;
                this.NotifyPropertyChanged("FilteredData");
            }
        }

        #endregion

        #region Property Changed Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Proprty Changed Then Notify Event Handler
        /// </summary>
        /// <param name="info">String</param>
        /// <returns>void</returns>
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion               
    }
}
