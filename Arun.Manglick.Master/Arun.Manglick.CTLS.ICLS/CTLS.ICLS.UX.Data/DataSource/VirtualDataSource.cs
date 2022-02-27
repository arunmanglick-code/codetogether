using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using CT.SLABB.Data;

namespace CTLS.ICLS.UX.Data
{
    [ContentProperty("DataProvider")]
    public class VirtualDataSource : Refreshable
    {
        //Roman Rubin - replaced PagedCollectionView with NEW VirtualPagedCollectionView.
        private VirtualPagedCollectionView __pageableView = null;

        private const int DEF_LoadDelayInMilliSeconds = 100;// 500;
        private const int DEF_RefreshDelayInMilliSeconds = 100; //500;
        private readonly string dataSourceId = System.Guid.NewGuid().ToString("N").ToUpper();

        //...............................................................................
        #region .ctor
        //...............................................................................
        public VirtualDataSource()
        {
            this.LoadDelayInMilliSeconds = DEF_LoadDelayInMilliSeconds;
            this.RefreshDelayInMilliSeconds = DEF_RefreshDelayInMilliSeconds;
           

            this.__pageableView = new VirtualPagedCollectionView();
            __pageableView.OnRefresh += new EventHandler<EventArgs>(PageableView_Refresh);

            this.Loaded += new RoutedEventHandler(DataSource_Loaded);
           
            (__pageableView.SortDescriptions as INotifyCollectionChanged).CollectionChanged += new NotifyCollectionChangedEventHandler(SortDescriptions_CollectionChanged);
            this.FilterDescriptions.CollectionChanged += new NotifyCollectionChangedEventHandler(FilterDescriptions_CollectionChanged);
        }
        #endregion

        //...............................................................................
        #region DataSource_Loaded, SortDescription_CollectionChanged & FilterDescriptions_CollectionChanged
        //...............................................................................
        /// <summary>
        /// Triggered when this DataSource is loaded.
        /// </summary>
        void DataSource_Loaded(object sender, RoutedEventArgs e)
        {
            //// If a DataProvider is available and is configured for AutoLoad.
            //if (this.AutoLoad) {
            //    this.Refresh(this.LoadDelayInMilliSeconds);
          //  }
        }

        /// <summary>
        /// Triggered when the sort description of this data source is modified.
        /// </summary>
        void SortDescriptions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Reset) return;
            // TODO: Is there a situation when we might be sorting even when there was an error in fetch.
            // TODO: ReloadOnSort should this be exposed.
            // TODO: See about away to not go to server if we have all records. //&& this.__pageableView.TotalItemCount > this.MaxRecords
                  //this.Refresh(this.RefreshDelayInMilliSeconds);
            __pageableView.PageIndex = 0;
            __pageableView.Refresh();
        }

        /// <summary>
        /// Triggered when filter description of this data source is modified.
        /// </summary>
        void FilterDescriptions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            __pageableView.PageIndex = 0;
            this.Refresh(this.RefreshDelayInMilliSeconds);
        }

        #endregion

        //...............................................................................
        #region DataProvider :- Dependency Property
        //...............................................................................
        public static readonly DependencyProperty DataProviderProperty = DependencyProperty.Register(
            "DataProvider", typeof(IDataProvider), typeof(VirtualDataSource),
            new PropertyMetadata(whenDataProviderChanged));

        private static void whenDataProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VirtualDataSource me = d as VirtualDataSource;
            if (null == me) return;

            IDataProvider oldProvider = e.OldValue as IDataProvider;
            IDataProvider newProvider = e.NewValue as IDataProvider;

            me.OnDataProviderChanged(oldProvider, newProvider);
        }

        private void OnDataProviderChanged(IDataProvider oldProvider, IDataProvider newProvider)
        {
            if (null != oldProvider)
            {
                oldProvider.PropertyChanged -= OnDataProviderInnerPropertyChanged;
            }

            if (null != newProvider)
            {
                newProvider.PropertyChanged += OnDataProviderInnerPropertyChanged;

            }
        }

        public IDataProvider DataProvider
        {
            get
            {
                return (IDataProvider)this.GetValue(DataProviderProperty);
            }
            set
            {
                this.SetValue(DataProviderProperty, value);
            }
        }

        /// <summary>
        /// This method is triggered when inner properties of DataProvider changes.
        /// This is NOT about DataProvider as a property itself.
        /// </summary>
        private void OnDataProviderInnerPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (null == sender) return;
            if (null == args) return;

            //The VirtualDataSource EXPECTS a VirtualDataProvider
            //VirtualDataProvider will reflect the server's TotalItemCount
            //As well as the sub-set of Items that were brought over.
            VirtualDataProvider provider = sender as VirtualDataProvider;
            if (null == provider) return;

            switch (args.PropertyName)
            {
                case ("Items"):
                    __pageableView.Load(provider.Items);
                    break;
                case ("TotalItemCount"):
                    __pageableView.TotalItemCount = provider.TotalItemCount; //Added By Roman Rubin to support Virtual Data
                    break;
                case ("Status"):
                    ReflectProviderStatus(provider.Status);           
                    break;

            }
        }

        private void ReflectProviderStatus(RefreshableStatus status)
        {
            switch(status) 
            {
                    //RefreshableStatus.Cancelled
                    //
                    //
                    //
                    //RefreshableStatus.Completed;
                    //RefreshableStatus.Failed
                    
                    //RefreshableStatus.Unknown
                    //RefreshableStatus.Waiting
                    //RefreshableStatus.Working

                case (RefreshableStatus.Cancelled):
                    this.IsBusy = false;
                    base.RefreshCancelled();
                    //this.UpdateStatus(RefreshableStatus.Cancelled);
                    break;

                case (RefreshableStatus.Completed):
                    this.IsBusy = false;
                    this.RefreshDone();
                   // this.UpdateStatus(RefreshableStatus.Completed);
                    break;

                case (RefreshableStatus.Failed):
                    this.IsBusy = false;
                    this.RefreshFailed();
                   // this.UpdateStatus(RefreshableStatus.Failed); //?
                    break;

                //case (RefreshableStatus.Unknown):
                //    this.IsBusy = false;
                    
                //    this.UpdateStatus(RefreshableStatus.Unknown);
                //    break;
                //case (RefreshableStatus.Waiting):
                //    this.IsBusy = true;
                  
                //    this.UpdateStatus(RefreshableStatus.Waiting);
                //    break;
                //case (RefreshableStatus.Working):
                //    this.IsBusy = true;
                    
                //    this.UpdateStatus(RefreshableStatus.Working);
                //    break;
            }
        }

        #endregion

        //...............................................................................
        #region AutoLoad, LoadDelayInMilliSeconds, RefreshDelayInMilliSeconds, ReloadOnSort
        //...............................................................................
        //public bool AutoLoad
        //{
        //    get;
        //    set;
        //}

        public int LoadDelayInMilliSeconds
        {
            get;
            private set;
        }

        public int RefreshDelayInMilliSeconds
        {
            get;
            private set;
        }

        //TODO:  Can we be smart enough to reload on sort based on our current items.
        //public bool ReloadOnSort
        //{
        //    get;
        //    set;
        //}

        #endregion

        //...............................................................................
        #region IsBusy :- Dependency property
        //...............................................................................
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy", typeof(bool), typeof(VirtualDataSource), null);

        public bool IsBusy
        {
            get {
                return (bool)this.GetValue(IsBusyProperty);
            }
            private set {
                this.SetValue(IsBusyProperty, value);
            }
        }

        #endregion

        //...............................................................................
        #region Data :- Simple property
        //...............................................................................
        public IEnumerable Data
        {
            get {
                return __pageableView;
            }
        }

        #endregion

        //...............................................................................
        #region Refreshing event
        //...............................................................................
        private EventHandler<CancelEventArgs>   __refreshingHandlers = null;

        public event EventHandler<CancelEventArgs> Refreshing
        {
            add {
                if (null != value) __refreshingHandlers += value;
            }
            remove {
                if ((null != value) && (null != __refreshingHandlers)) __refreshingHandlers -= value;
            }
        }

        protected bool OnRefreshing()
        {
            if (null == __refreshingHandlers) return false;

            EventHandler<CancelEventArgs> handlers = __refreshingHandlers;
            CancelEventArgs args = new CancelEventArgs();
            args.Cancel = false;
            
            try {
                handlers(this, args);
                return args.Cancel;
            }
            catch {
                // Exception ignored by design.
                return false;
            }
        }

        #endregion

        //...............................................................................
        #region Refresh and RefreshNow
        //...............................................................................
       
       
        private bool __waitingToRefresh = false;

        //Added For Virtualizing Data.
        private void PageableView_Refresh(object sender, EventArgs e)
        {
            this.Refresh(this.RefreshDelayInMilliSeconds);
        }
        /// <summary>
        /// Obtains a fresh set of data from supplied DataProvider.
        /// </summary>
        private void Refresh(int delayInMilliSeconds)
        {
            // If already a refresh request is pending...
            if (__waitingToRefresh) return;

            // If there is no data provider, ignore the call.
            if (null == this.DataProvider) return;

            // Consult the Refreshing event.
            bool shouldStop = this.OnRefreshing();
            if (shouldStop) return;

            // If a load delay is suggested...
            if (delayInMilliSeconds > 0)
            {
                // Flag that a timer-wait is already in place...
                __waitingToRefresh = true;

                // Prepare and triger a timer.
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += OnLoadDelayTimerCallback;
                timer.Interval = TimeSpan.FromMilliseconds(delayInMilliSeconds);
                timer.Start();
            }
            else
            {
                base.Refresh();
            }
        }

        private void OnLoadDelayTimerCallback(object sender, EventArgs args)
        {
            if (null == sender) return;
            DispatcherTimer timer = (DispatcherTimer)sender;

            // Stop the timer; Unwire the event handler.
            timer.Stop();
            timer.Tick -= OnLoadDelayTimerCallback;
            timer = null;

            // Clear the flag indicating that we are waiting to refesh.
            __waitingToRefresh = false;

            // Trigger the refresh.
            base.Refresh();
        }
        
        protected override void BeginRefresh()
        {
            // Obtain a local reference to the provider.
            //TODO:  check for a VirtualDataProvider?
            IDataProvider provider = this.DataProvider;
            if (null == provider) return;

            FilterDescriptionCollection searchPredicates = this.FilterDescriptions;
            SortDescriptionCollection sortPredicates = __pageableView.SortDescriptions;
            int startRecord = __pageableView.StartRecord;
            int endRecord = __pageableView.EndRecord;

            this.DataProvider.BeginLoad(this, searchPredicates, sortPredicates, startRecord, endRecord, dataSourceId);
        }
        /// <summary>
        /// Triggers an asynchronous refresh.
        /// Refresh may be triggered immediately or may be delayed as much as RefreshDelayInMilliSeconds.
        /// </summary>
        public override void Refresh()
        {
            // If there is no data provider, ignore the call.
            if (null == this.DataProvider) return;

            this.Refresh(this.RefreshDelayInMilliSeconds);
        }

        #endregion

        //...............................................................................
        #region SortDescriptions & FilterDescriptions
        //...............................................................................
        private FilterDescriptionCollection __filterDescriptions = null;

        public SortDescriptionCollection SortDescriptions
        {
            get
            {
                return __pageableView.SortDescriptions;
            }
        }

        public FilterDescriptionCollection FilterDescriptions
        {

            get
            {
                if (null == __filterDescriptions) __filterDescriptions = new FilterDescriptionCollection();
                return __filterDescriptions;
            }
        }

        #endregion 
    }
}
