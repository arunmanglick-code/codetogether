using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using CT.ABB.DX;
using CT.SLABB.Data;
using CTLS.ICLS.DX;

namespace CTLS.ICLS.UX.Data
{   
    public abstract class VirtualDataProvider : FrameworkElement, IDataProvider
    {
        RefreshableStatus __myStatus = RefreshableStatus.Unknown;
        IEnumerable __items = null;
                
        #region .ctor
        //...............................................................................
        public VirtualDataProvider()
        {
            //
        }
        #endregion
                
        #region INotifyPropertyChanged Members
        //...............................................................................
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (null == this.PropertyChanged) return;
            if (string.IsNullOrEmpty(propertyName)) return;

            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
            this.PropertyChanged(this, args);
        }

        #endregion
                
        #region VirtualDataProvider Members
        //...............................................................................

        int __totalItemCount = 0;
        public int TotalItemCount
        {
            get
            {
                return __totalItemCount;
            }
            protected set
            {
                this.__totalItemCount = value;
                this.OnPropertyChanged("TotalItemCount");
            }
        }

        #endregion
                
        #region IDataProvider Members
        //...............................................................................
        public RefreshableStatus Status
        {
            get
            {
                return __myStatus;
            }
            protected set
            {
                __myStatus = value;
                this.OnPropertyChanged("Status");

            }
        }

        public IEnumerable Items
        {
            get
            {
                return __items;
            }
            protected set
            {
                this.__items = value;
                this.OnPropertyChanged("Items");
            }
        }

        void IDataProvider.BeginLoad(object sender, FilterDescriptionCollection searchPredicates, SortDescriptionCollection sortPredicates, int maxRecords)
        {
            this.BeginLoad(sender, searchPredicates, sortPredicates, maxRecords);
        }

        void IDataProvider.BeginLoad(object sender, FilterDescriptionCollection searchPredicates, SortDescriptionCollection sortPredicates, int startRecord, int endRecord, string dataSourceId)
        {
            this.BeginLoad(sender, searchPredicates, sortPredicates, startRecord, endRecord, dataSourceId);
        }

        void IDataProvider.CancelLoad()
        {
            this.CancelLoad();
        }

        private void BeginLoad(object sender, FilterDescriptionCollection searchPredicates, SortDescriptionCollection sortPredicates, int maxRecords) { }

        protected abstract void BeginLoad(object sender, FilterDescriptionCollection searchPredicates, SortDescriptionCollection sortPredicates, int startRecord, int endRecord, string dataSourceId);

        protected virtual void CancelLoad()
        {
            // Default implementation doesn't support cancel operation.
            // Hence the method is stubbed.
            // A specific implementation can override this method to support cancelling an operation.
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method transfers the searchPredicates, sortPredicates and maxRecords to properties of the request object
        /// </summary>
        protected void PrepareRequest<T>(T request, FilterDescriptionCollection searchPredicates,
            SortDescriptionCollection sortPredicates, int startRecord, int endRecord, string dataSourceId) where T : IVirtualListRequest
        {
            request.SortDescriptions = sortPredicates;
            request.StartRecord = startRecord;
            request.EndRecord = endRecord;
            request.DataSourceId = dataSourceId;

            if (null == searchPredicates)
                return;

            // Transfer searchPredicates to the request
            // Iterate through the filter descriptions collection
            foreach (FilterDescription fd in searchPredicates)
            {
                // Look for the field in the request 
                FieldInfo fi = typeof(T).GetField(fd.PropertyName, BindingFlags.Public | BindingFlags.Instance);

                // Ignoring filters that are not present in the request object. 
                if (fi == null)
                {
                    continue;
                }

                // Field is present - Transfer the operator, value, value2 from the fd to the field. 
                switch (fi.FieldType.Name)
                {
                    case "TextFilter":
                        TextFilter txtFilter = new TextFilter();
                        txtFilter.Operator = fd.Operator;
                        txtFilter.Value = fd.Value.ToString();
                        fi.SetValue(request, txtFilter);
                        break;

                    case "NumericFilter":
                        NumericFilter numFilter = new NumericFilter();
                        numFilter.Operator = fd.Operator;
                        numFilter.Value = Convert.ToInt64(fd.Value);
                        fi.SetValue(request, numFilter);
                        break;

                    case "NumericRangeFilter":
                        NumericRangeFilter numRangeFilter = new NumericRangeFilter();
                        numRangeFilter.Operator = fd.Operator;
                        numRangeFilter.Value = Convert.ToInt64(fd.Value);
                        numRangeFilter.Value2 = Convert.ToInt64(fd.Value2);
                        fi.SetValue(request, numRangeFilter);
                        break;

                    case "DateFilter":
                        DateFilter dateFilter = new DateFilter();
                        dateFilter.Operator = fd.Operator;
                        dateFilter.Value = Convert.ToDateTime(fd.Value);
                        fi.SetValue(request, dateFilter);
                        break;

                    case "DateRangeFilter":
                        DateRangeFilter dateRangeFilter = new DateRangeFilter();
                        dateRangeFilter.Operator = fd.Operator;
                        dateRangeFilter.Value = Convert.ToDateTime(fd.Value);
                        dateRangeFilter.Value2 = Convert.ToDateTime(fd.Value2);
                        fi.SetValue(request, dateRangeFilter);
                        break;

                    case "BooleanFilter":
                        BooleanFilter boolFilter = new BooleanFilter();
                        boolFilter.Operator = fd.Operator;
                        boolFilter.Value = Convert.ToBoolean(fd.Value);
                        fi.SetValue(request, boolFilter);
                        break;

                    default:
                        // Ignoring if a field of the same name exists in the request object but is not a filter
                        break;
                }

            }
        }

        #endregion
    }
}
