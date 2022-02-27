using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;

namespace CTLS.ICLS.UX.Data
{
    public class VirtualPagedCollectionView : ICollectionView, INotifyPropertyChanged, IPagedCollectionView, INotifyCollectionChanged
    {
        private const int FRONT_CACHE = 3;
        private const int REAR_CACHE = 2;
        private const int MAX_PAGES_IN_CACHE = REAR_CACHE + 1 + FRONT_CACHE;
        private const int DEF_PAGE_SIZE = 20;
        private const int MAX_CACHE_SIZE = 300; // (2+1+3)*50

        private List<object> __internalList = new List<object>();

        //...............................................................................
        #region .ctor
        //...............................................................................
        public VirtualPagedCollectionView()
        {
            
        }

        public VirtualPagedCollectionView(IEnumerable items)
        {
            if (null == items) return;
            Load(items);
        }
        #endregion

        //...............................................................................
        #region Cache Boundary Calculation
        //...............................................................................

        private int __currentCacheStart = 0;  // For Offshore to Validate PR: Default to 1?
        private int __currentCacheEnd = 0;

        public int StartRecord
        {
            get
            {
                // PR: Everytime someone asks for StartRecord, we should nto be calculating it. Only calculate when something happens that changes the StartRec
                int startRec = 0, endRec = -1; // PR: can we remove the default values? 
                __calculateCacheBoundary(out startRec, out endRec);
                __currentCacheStart = startRec;
                return startRec;
            }
        }

        public int EndRecord
        {
            get
            {
                // PR: Everytime someone asks for EndRecord, we should nto be calculating it. Only calculate when something happens that changes the EndRec
                int startRec = 0, endRec = -1; // PR: can we remove the default values?
                __calculateCacheBoundary(out startRec, out endRec);
                __currentCacheEnd = endRec;
                return endRec;
            }
        }

        private int CurrentCacheStartPageIndex
        {
            get
            {
                return (int)Math.Floor(__currentCacheStart / this.PageSize);
            }
        }

        private bool __isPageInCache(int pageIndex)
        {
            int pageStartRec, pageEndRec;
            __calculatePageBoundary(pageIndex, out pageStartRec, out pageEndRec);

            return (
                (pageStartRec >= __currentCacheStart) &&
                (pageStartRec <= __currentCacheEnd) &&
                (pageEndRec >= __currentCacheStart) &&
                (pageEndRec <= __currentCacheEnd)
            );
        }

        private void __calculateCacheBoundary(out int startRec, out int endRec)
        {
            int cacheSize = MAX_PAGES_IN_CACHE * this.PageSize;
            startRec = ((this.PageIndex - REAR_CACHE) * this.PageSize) + 1;
            startRec = (startRec < 1 ? 1 : startRec);
            endRec = startRec + cacheSize - 1;

            if (TotalItemCount > 0 && endRec > TotalItemCount)
            {
                //Ganesh Mohan : Commenting, After first time search or filter the "TotalItemCount" will be different than orginal one
                //So, When you do clear the "TotalItemCount" needs to be get cleared to "-1" but this is not happening and it set the endRec 
                //to new value. Which make to fetch only specified set of records.
                //Confirm with Roman
                //endRec = TotalItemCount;

                // PR: What is this? Why not startRec = endRec - cacheSize + 1;
                startRec = ((this.TotalPageCount - MAX_PAGES_IN_CACHE) * this.PageSize) + 1;
            }

            startRec = (startRec < 1 ? 1 : startRec);
            endRec = (endRec < 1 ? 1 : endRec);

            if (TotalItemCount > 0)
            {
                startRec = (startRec > TotalItemCount ? TotalItemCount : startRec);

                //Ganesh Mohan : Commenting, After first time search or filter the "TotalItemCount" will be different than orginal one
                //So, When you do clear the "TotalItemCount" needs to be get cleared to "-1" but this is not happening and it set the endRec 
                //to new value. Which make to fetch only specified set of records.
                //Confirm with Roman
                //endRec = (endRec > TotalItemCount ? TotalItemCount : endRec);
            }

            endRec = (endRec < startRec ? startRec : endRec);
        }

        private void __calculatePageBoundary(int pageIndex, out int startRec, out int endRec)
        {
            startRec = (pageIndex * this.PageSize) + 1;
            startRec = (startRec < 1 ? 1 : startRec);
            if (TotalItemCount > 0)
            {
                startRec = (startRec > TotalItemCount ? TotalItemCount : startRec);
            }

            endRec = startRec + this.PageSize - 1;
            if (TotalItemCount > 0)
            {
                endRec = (endRec > TotalItemCount ? TotalItemCount : endRec);
            }

            endRec = (endRec < startRec ? startRec : endRec);
        }
        #endregion

        //...............................................................................
        #region IPagedCollectionView
        //...............................................................................
        public bool CanChangePage { get { return true; } }

        private bool __isPageChanging;
        public bool IsPageChanging
        {
            get { return __isPageChanging; }
            private set
            {
                if (__isPageChanging != value)
                {
                    __isPageChanging = value;
                    OnPropertyChanged("IsPageChanging");
                }
            }
        }

        public int ItemCount
        {
            get
            {
                return TotalItemCount;
            }
            set
            {
                TotalItemCount = value;
            }
        }

        private int __pageIndex;
        public int PageIndex
        {
            get { return __pageIndex; }
            internal set { __pageIndex = value; }
        }

        private int __pageSize = DEF_PAGE_SIZE;
        public int PageSize
        {
            get
            {
                return __pageSize;
            }
            set
            {
                if (__pageSize != value && value >= 1)
                {
                    __pageSize = value;
                    OnPropertyChanged("PageSize");

                    // PR: DataSource needs to listen in on PageSize changed. Trigger a refresh when PageSize changes. 
                    // PR: Is this the correct spot to Reset the PageIndex.
                    this.PageIndex = 0;
                    this.Refresh();  //No Matter What recalculate.
                }
            }
        } //RR Validate Page Size Changes.

        private int __totalItemCount = -1;  // PR: Init to -1. 
        public int TotalItemCount
        {
            get
            {
                return __totalItemCount;
            }
            set
            {
                if (__totalItemCount != value)
                {
                    __totalItemCount = value;
                    OnPropertyChanged("TotalItemCount");
                    OnPropertyChanged("ItemCount");
                }
            }
        }

        public event EventHandler<EventArgs> PageChanged;
        protected virtual void OnPageChanged(EventArgs args)
        {
            if (null != PageChanged)
            {
                PageChanged(this, args);
            }
        }

        public event EventHandler<PageChangingEventArgs> PageChanging;
        protected virtual void OnPageChanging(PageChangingEventArgs args)
        {
            if (null != PageChanging)
            {
                PageChanging(this, args);
            }
        }

        public bool MoveToFirstPage()
        {
            return this.MoveToPage(0);
        }

        public bool MoveToLastPage()
        {
            if (this.TotalItemCount < 0) return false;
            if (this.PageSize <= 0) return false;
            return this.MoveToPage(this.TotalPageCount - 1);
            //return (((this.TotalItemCount != -1) && (this.PageSize > 0)) && this.MoveToPage(this.TotalPageCount - 1));
        }

        public bool MoveToPreviousPage()
        {
            return MoveToPage(this.PageIndex - 1);
        }

        public bool MoveToNextPage()
        {
            return MoveToPage(this.PageIndex + 1);
        }

        public bool MoveToPage(int pageIndex)
        {
            // PR: Change checks as below
            if (pageIndex < 0) return false;
            if ((pageIndex >= this.TotalPageCount) || (this.PageIndex == pageIndex)) return false;

            if (this.TotalItemCount < 0) return false;
            if (this.PageSize <= 0) return false;

            //if (pageIndex < -1) return false;
            //if ((pageIndex == -1) && (this.PageSize > 0)) return false;
            //if ((pageIndex >= this.TotalPageCount) || (this.PageIndex == pageIndex)) return false;

            try
            {
                IsPageChanging = true;
                if (null != PageChanging)
                {
                    PageChangingEventArgs args = new PageChangingEventArgs(pageIndex);
                    OnPageChanging(args);
                    if (args.Cancel) return false;
                }

                this.PageIndex = pageIndex; // PR: Already in CompletePageMove. Where should it be? 
                if (!__isPageInCache(pageIndex))
                {
                    __internalList.Clear();
                    Refresh();
                }

                IsPageChanging = false;   // PR: Already in CompletePageMove
                OnPropertyChanged("PageIndex"); // PR: Already in CompletePageMove. Shoukd we not trigger this as soon as we complete the assignment
                OnPageChanged(EventArgs.Empty);
                __completePageMove(pageIndex);  // PR: Why the separate function. It breaks the train of thought
                return true;
            }
            finally
            {
                IsPageChanging = false;
            }
        }

        private void __completePageMove(int pageIndex)
        {
            object currentItem = this.CurrentItem;      // PR: What is the use of this? 
            int currentPosition = this.CurrentPosition;// PR: What is the use of this? 
            bool isCurrentAfterLast = this.IsCurrentAfterLast;// PR: What is the use of this? 
            bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;// PR: What is the use of this? 
            this.PageIndex = pageIndex;

            this.IsPageChanging = false;
            this.OnPropertyChanged("PageIndex");

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

        }
        #endregion

        //...............................................................................
        #region TotalPageCount
        //...............................................................................
         public int TotalPageCount
        {
            get
            {
                if (this.PageSize <= 0) return 0;
                if (this.TotalItemCount < 0) return 0;
                return Math.Max(1, (int)Math.Ceiling(((double)this.TotalItemCount) / ((double)this.PageSize)));
            }
        }

        #endregion

        //...............................................................................
        #region Loading Collections and Internal Copies
        //...............................................................................

        public void Load(IEnumerable items)
        {
            this.SourceCollection = items;
            __copySourceToInternalList(); // PR: Why? 
            __completePageMove(this.PageIndex);  
        }

        public void Clear()
        {
            this.SourceCollection = null; // PR: Why manipulate Source and innercollection? Dont we need to trigger OnCollectionChanged? Should clear set it to null or empty the contents of the Sourcecollection 
        }


        //We need to Delay a Collection Change notification when the Item Count is zero.
        //private bool __waitingToRefresh = false;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
           
            if ((this.CollectionChanged != null) && (((args.Action != NotifyCollectionChangedAction.Add) || (this.PageSize == 0)) /*|| (args.NewStartingIndex < this.Count)*/))
            {
                //Bug fix for No Data 
                //__internalList.Count == 0  was considered but there is a delay during the list copy over that is causing UI flickering
                //TODO: as part of internalList copy optimizations re-visit this to see if there is an opportunity to optimize.
                if (this.SourceCollectionList.Count == 0)
                {
                    this.CollectionChanged(this, args);
                }
                
                if (__internalList.Count > 0)
                    this.CollectionChanged(this, args);
            }
            if (args.Action != NotifyCollectionChangedAction.Replace)
            {
                this.OnPropertyChanged("Count");
            }
        }

        private void __copySourceToInternalList()
        {
            if (null == this.SourceCollection) return; //nothing to copy.

            this.__internalList.Clear();
            
            IEnumerator enumerator = ((IEnumerable)this.SourceCollection).GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (__internalList.Count == MAX_CACHE_SIZE) break;  //if internalList size exceeds MAX_CACHE_SIZE tolerate it and PROTECT the system.
                this.__internalList.Add(enumerator.Current);
            }
        }

        #endregion

        //...............................................................................
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        //...............................................................................
        #region Refreshing, DeferedRefreshing TBD
        //...............................................................................

        public IDisposable DeferRefresh()
        {
            return new DeferRefreshHelper(() => Refresh());
        }

        public event EventHandler<EventArgs> OnRefresh;
        public void Refresh()
        {
            if (null != OnRefresh)
            {
                OnRefresh(this, EventArgs.Empty);
            }
        }

        #region Defer Refresh helper
        private class DeferRefreshHelper : IDisposable
        {
            private Action _callback;

            public DeferRefreshHelper(Action callback)
            {
                _callback = callback;
            }

            public void Dispose()
            {
                //TODO:  In our current DataSource implementation we are carrying the Refresh event upto the DataSource so that
                //we make the DataProvider Pluggable.  Therefore any attempt to DeferRefresh() will cause a double fire.
                //Once when the DeferRefresh is called by the implementation and once when a Sort is changed and causes a collection
                //refresh.

                //_callback();
            }
        }
        #endregion
        #endregion

        //...............................................................................
        #region ICollectionView Members

        public bool CanFilter { get { return false; } }
        public bool CanGroup { get { return false; } }
        public bool CanSort { get { return true; } }

        // PR: Not set anywhere? 
        private object _currentItem;
        public object CurrentItem
        {
            get { return this._currentItem; }
        }

        // PR: Not set anywhere? 
        private int _currentPosition;
        public int CurrentPosition
        {
            get { return this._currentPosition; }
        }

        public bool IsCurrentAfterLast
        {
            get
            {
                if (!this.IsEmpty)
                {
                    //TODO optimize this.
                    return (this.CurrentPosition >= SourceCollectionList.Count);
                }
                return true;
            }
        }

        public bool IsCurrentBeforeFirst
        {
            get
            {
                if (!this.IsEmpty)
                {
                    return (this.CurrentPosition < 0);
                }
                return true;
            }
        }

        public bool MoveCurrentToPrevious()
        {
            throw new NotImplementedException();
        }

        private IEnumerable __sourceCollection;
        public IEnumerable SourceCollection
        {
            get { return __sourceCollection; }
            set
            {
                __sourceCollection = value;
            }
        }

        public List<object> SourceCollectionList
        {
            get
            {
                if (null == __sourceCollection) return new List<object>();
                List<object> data = new List<object>();
                foreach (object t in __sourceCollection)
                {
                    data.Add(t);
                }
                return data;
            }
        }

        public bool Contains(object item)
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty
        {
            get { return null == this.SourceCollection; }
        }
        #endregion

        //...............................................................................
        #region Sorting
        //...............................................................................
        //Are sort descriptions needed since you are going to be doing sorting on the server.
       // private CustomSortDescriptionCollection _sortDescriptions;


        private  SortDescriptionCollection _sortDescriptions;
        public SortDescriptionCollection SortDescriptions
        {
            get
            {
                if (this._sortDescriptions == null)
                {
                    //this.SetSortDescriptions(new CustomSortDescriptionCollection());

                    this._sortDescriptions = new SortDescriptionCollection();
                }
                return this._sortDescriptions;
            }
        }

        //private void SetSortDescriptions(CustomSortDescriptionCollection descriptions)
        //{
        //    if (this._sortDescriptions != null)
        //    {
        //        this._sortDescriptions.MyCollectionChanged -= new NotifyCollectionChangedEventHandler(this.SortDescriptionsChanged);
        //    }
        //    this._sortDescriptions = descriptions;
        //    if (this._sortDescriptions != null)
        //    {
        //        this._sortDescriptions.MyCollectionChanged += new NotifyCollectionChangedEventHandler(this.SortDescriptionsChanged);
        //    }
        //}

        //private void SortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Remove && e.NewStartingIndex == -1 && SortDescriptions.Count > 0)
        //    {
        //        return;
        //    }
        //    if (((e.Action != NotifyCollectionChangedAction.Reset) || (e.NewItems != null)) || (((e.NewStartingIndex != -1) || (e.OldItems != null)) || (e.OldStartingIndex != -1)))
        //    {
        //       // this.Refresh();
        //    }
        //}

        #endregion

        //...............................................................................
        #region NotImplemented TODO TODO TODO
        //........................................................................
        public bool MoveCurrentTo(object item)
        {
            return true;
        }

        public bool MoveCurrentToFirst()
        {
            return true;
        }

        public bool MoveCurrentToLast()
        {
            return true;
        }

        public bool MoveCurrentToNext()
        {
            return true;
        }

        public bool MoveCurrentToPosition(int position)
        {
            return true;
        }

        public Predicate<object> Filter
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ObservableCollection<GroupDescription> GroupDescriptions
        {
            get { return null; }
        }

        public ReadOnlyObservableCollection<object> Groups
        {
            get { return null; }
        }

        public CultureInfo Culture
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler CurrentChanged;

        public event CurrentChangingEventHandler CurrentChanging;


        #endregion

        //...............................................................................
        #region IEnumerable Members

        //ToDo: This gets called for every item. Is there any way to avoid?
        //TODO: RR -  Can we directly use SourceCollection here
        public IEnumerator GetEnumerator()
        {
            List<object> list = new List<object>();

            if (__internalList.Count == 0) return list.GetEnumerator();
            if (this.PageIndex < 0) return list.GetEnumerator();

            // Locate the current page in the cache
            int pageStartItemIndex = this.PageSize * (this.PageIndex - this.CurrentCacheStartPageIndex);
            int pageEndItemIndex = Math.Min(pageStartItemIndex + this.PageSize, this.__internalList.Count);

            for (int i = pageStartItemIndex; i < pageEndItemIndex; i++)
            {
                list.Add(this.__internalList[i]);
            }


            return list.GetEnumerator();
        }
        #endregion

        //...............................................................................
        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion
    }    
}
