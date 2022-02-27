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
    public class CountySearchInstructionsList : Refreshable<List<CountySearchInstructionList>>
    {
        #region Variables

        private List<CountySearchInstructionList> _filteredData;
        private List<CountySearchInstructionList> countySearchInstructionListItems;
        public EventHandler OnServiceCompleted;

        #endregion

        #region DependencyProperty

        public static readonly DependencyProperty StateCodeProperty = DependencyProperty.Register("StateCode", typeof(string), typeof(CountySearchInstructionsList), null);
        public static readonly DependencyProperty countyCodeProperty = DependencyProperty.Register("CountyCode", typeof(int), typeof(CountySearchInstructionsList), null);
       
        #endregion

        #region Method
        /// <summary>
        /// Prepare Service call for County Instructions
        /// </summary>
        protected override void BeginRefresh()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_SEARCH_INSTRUCTIONS_LIST))
            {
                countySearchInstructionListItems = IsolatedStorageSettings.ApplicationSettings[SharedConstants.CACHED_SEARCH_INSTRUCTIONS_LIST] as List<CountySearchInstructionList>;
                this.RefreshDone(countySearchInstructionListItems);
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// StateCode Read Write Property
        /// </summary>
        public string StateCode
        {
            get
            {
                return (string)this.GetValue(StateCodeProperty);
            }
            set
            {
                this.SetValue(StateCodeProperty, value);
            }
        }

        /// <summary>
        /// CountyCode Read Write Property
        /// </summary>
        public int CountyCode
        {
            get
            {
                return (int)this.GetValue(countyCodeProperty);
            }
            set
            {
                this.SetValue(countyCodeProperty, value);
                if (this.Data != null)
                {
                    this.FilteredData = this.Data.Where(t => t.CountyCode == -1).ToList();
                    if (CountyCode.ToString() != SharedConstants.SELECT_ONE_KEY)
                        this.FilteredData = this.Data.Where(t => t.StateCode != null && t.StateCode.Equals(StateCode) && t.CountyCode == value).ToList();

                    this.OnPropertyChanged("FilteredData");
                }
            }
        }

        /// <summary>
        /// FilteredData Read Write Property
        /// </summary>
        public List<CountySearchInstructionList> FilteredData
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
