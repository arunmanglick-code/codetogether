using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Windows;
using CT.SLABB.Data;
using CT.SLABB.DX;
using CTLS.ICLS.DX;
using CTLS.ICLS.UX.Shared.Utils;

namespace CTLS.ICLS.UX.CountySearch
{
    public class CustomerSpecialistList : Refreshable<List<CommonCustomerSpecialistListItem>>
    {
        #region Variables

        private List<CommonCustomerSpecialistListItem> _filteredData;
        List<CommonCustomerSpecialistListItem> customerListItems = new List<CommonCustomerSpecialistListItem>();

        public static readonly DependencyProperty TeamIdProperty = DependencyProperty.Register("TeamId", typeof(int), typeof(CustomerSpecialistList), null);
        public event RoutedEventHandler CustomerSpecialistListCompleted;

        #endregion

        #region Method
        /// <summary>
        /// Prepare Service call for Customer List
        /// </summary>
        protected override void BeginRefresh()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_CUSTOMERSPECIALIST_LIST))
            {
                customerListItems = IsolatedStorageSettings.ApplicationSettings[SharedConstants.CACHED_CUSTOMERSPECIALIST_LIST] as List<CommonCustomerSpecialistListItem>;                
                this.RefreshDone(customerListItems);
            }            

            if (CustomerSpecialistListCompleted != null)
                CustomerSpecialistListCompleted(null, new RoutedEventArgs()); // To Fire BeginRefresh for OrderSearchDataProvider
        }
        #endregion

        #region Properties

        /// <summary>
        /// TeamId Read Write Property
        /// </summary>
        public int TeamId
        {
            get
            {
                return (int)this.GetValue(TeamIdProperty);
            }
            set
            {
                this.SetValue(TeamIdProperty, value);
                if (this.Data != null)
                {
                    this.FilteredData = this.Data.Where(t => t.TeamId == value).ToList();
                    this.OnPropertyChanged("FilteredData");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// FilteredData Read Write Property
        /// </summary>
        public List<CommonCustomerSpecialistListItem> FilteredData
        {
            get { return _filteredData; }
            set { _filteredData = value; this.NotifyPropertyChanged("FilteredData"); }
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
