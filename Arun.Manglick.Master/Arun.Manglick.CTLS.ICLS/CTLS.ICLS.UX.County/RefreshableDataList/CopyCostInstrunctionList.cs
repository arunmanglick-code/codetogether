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
    public class CopyCostInstrunctionList : Refreshable<List<CopyCostInstructionList>>, INotifyPropertyChanged
    {
        #region Private Variables

        public static readonly DependencyProperty StateCodeProperty =
          DependencyProperty.Register("StateCode", typeof(string), typeof(CopyCostInstrunctionList), null);
        public static readonly DependencyProperty CountyCodeProperty =
            DependencyProperty.Register("CountyCode", typeof(int), typeof(CopyCostInstrunctionList), null);
        public static readonly DependencyProperty CostProperty =
            DependencyProperty.Register("Cost", typeof(double), typeof(CopyCostInstrunctionList), null);

        private List<CopyCostInstructionList> _filteredData;
        private List<CopyCostInstructionList> copySearchInstructionListItems;

        #endregion

        #region Method
        /// <summary>
        /// Prepare Service call for CopyCost Instructions
        /// </summary>
        protected override void BeginRefresh()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_COPYCOST_LIST))
            {
                copySearchInstructionListItems = IsolatedStorageSettings.ApplicationSettings[SharedConstants.CACHED_COPYCOST_LIST] as List<CopyCostInstructionList>;
                this.RefreshDone(copySearchInstructionListItems);
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
                return (int)this.GetValue(CountyCodeProperty);
            }
            set
            {
                this.SetValue(CountyCodeProperty, value);
                if (this.Data != null)
                {
                    this.FilteredData = this.Data.Where(t => t.CountyCode == -1).ToList();
                    this.Cost = 0;

                    if (CountyCode.ToString() != SharedConstants.SELECT_ONE_KEY)
                    {
                        this.FilteredData = this.Data.Where(t => t.StateCode.Equals(StateCode) && t.CountyCode == value).ToList();
                        if (this.FilteredData.Count > 0)
                            this.Cost = this.FilteredData.ToList().First().Cost;
                        else
                            this.Cost = 0;
                        this.FilteredData = this.Data.Where(t => t.StateCode.Equals(StateCode) && t.CountyCode == value && t.InstructionText != String.Empty).ToList();
                    }

                    this.OnPropertyChanged("FilteredData");
                    this.OnPropertyChanged("Cost");
                }
            }
        }

        /// <summary>
        /// Cost Read Write Property
        /// </summary>
        public double Cost
        {
            get
            {
                return (double)this.GetValue(CostProperty);
            }
            set
            {
                this.SetValue(CostProperty, value);
            }
        }

        /// <summary>
        /// FilteredData Read Write Property
        /// </summary>
        public List<CopyCostInstructionList> FilteredData
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
