using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    public class LienTypeList : Refreshable<List<CommonLienTypeListItem>>, INotifyPropertyChanged
    {
        #region Private Varialbes

        private List<CommonLienTypeListItem> _filteredData;
        List<CommonLienTypeListItem> lienTypeListItems = new List<CommonLienTypeListItem>();
        public static readonly DependencyProperty StateCodeProperty = DependencyProperty.Register("StateCode", typeof(string), typeof(LienTypeList), null);
        public static readonly DependencyProperty CountyCodeProperty = DependencyProperty.Register("CountyCode", typeof(int), typeof(LienTypeList), null);
        private string __lienTypeName;

        #endregion

        #region Method
        /// <summary>
        /// Prepare Service call for Lien Type List
        /// </summary>
        protected override void BeginRefresh()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SharedConstants.CACHED_LIENTYPE_LIST))
            {
                lienTypeListItems = IsolatedStorageSettings.ApplicationSettings[SharedConstants.CACHED_LIENTYPE_LIST] as List<CommonLienTypeListItem>;
                this.RefreshDone(lienTypeListItems);
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
                    this.FilteredData = this.Data.Where(t => t.StateCode.Equals(StateCode) && t.CountyCode == value).ToList();
                    this.OnPropertyChanged("FilteredData");
                }
            }
        }

        /// <summary>
        /// FilteredData Read Write Property
        /// </summary>
        [Display(Name = "Lien Types")]
        public List<CommonLienTypeListItem> FilteredData
        {
            get { return _filteredData; }
            set { _filteredData = value; this.NotifyPropertyChanged("FilteredData"); }
        }

        /// <summary>
        /// LienTypeName Read Write Property
        /// </summary>
        [Display(Name = "LienTypeName")]
        [Required(ErrorMessage = "Please enter {0}")]
        public string LienTypeName
        {
            get { return __lienTypeName; }
            set
            {
                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = "LienTypeName";
                Validator.ValidateProperty(value, context);

                __lienTypeName = value;
                this.NotifyPropertyChanged("LienTypeName");
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
