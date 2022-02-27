using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Arun.Manglick.CustomControls
{
    [TemplatePart(Name = ListMover.RootElement, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ListMover.RightListBoxElement, Type = typeof(ListBox))]
    [TemplatePart(Name = ListMover.LeftListBoxElement, Type = typeof(ListBox))]
    [TemplatePart(Name = ListMover.MoveLeftButtonElement, Type = typeof(Button))]
    [TemplatePart(Name = ListMover.MoveLeftAllButtonElement, Type = typeof(Button))]
    [TemplatePart(Name = ListMover.MoveRightButtonElement, Type = typeof(Button))]
    [TemplatePart(Name = ListMover.MoveRightAllButtonElement, Type = typeof(Button))]
    [TemplatePart(Name = ListMover.DownLeftButtonElement, Type = typeof(Button))]
    [TemplatePart(Name = ListMover.DownRightButtonElement, Type = typeof(Button))]

    public class ListMover : ContentControl
    {
        #region Constants

        #region Private Constants
        private const string RootElement = "RootElement"; // Holds the Framework element
        private const string RightListBoxElement = "RightListBoxElement"; // Right List Box Element
        private const string LeftListBoxElement = "LeftListBoxElement"; // Left List Box Element
        private const string MoveLeftButtonElement = "MoveLeftButtonElement"; //Button to Move a Item from R to L
        private const string MoveLeftAllButtonElement = "MoveLeftAllButtonElement"; //Button to Move All Item from R to L
        private const string MoveRightButtonElement = "MoveRightButtonElement"; //Button to Move a Item from L to R
        private const string MoveRightAllButtonElement = "MoveRightAllButtonElement"; //Button to Move All Item from L to R

        private const string UpLeftButtonElement = "UpLeftButtonElement";
        private const string UpRightButtonElement = "UpRightButtonElement";

        private const string DownLeftButtonElement = "DownLeftButtonElement";
        private const string DownRightButtonElement = "DownRightButtonElement";

        #endregion

        #region Public Constants [Default Values for ListMover, To be Moved to a General Repository]
        public const string StringMoveRight = ">";
        public const string StringMoveLeft = "<";
        public const string StringMoveAllRight = ">>";
        public const string StringMoveAllLeft = "<<";
        public const string StringLeftListBoxHeader = "Available:";
        public const string StringRightListBoxHeader = "Selected:";
        public const string StringUpLeft = "Up";
        public const string StringUpRight = "Up";
        public const string StringDownLeft = "Down";
        public const string StringDownRight = "Down";
        #endregion

        #endregion

        #region Controls

        private FrameworkElement _rootElement;
        private ListBox _rightListBoxElement, _leftListBoxElement;
        private Button _moveLeftButtonElement, _moveLeftAllButtonElement, _moveRightButtonElement, _moveRightAllButtonElement, _upLeftButtonElement, _upRightButtonElement, _downLeftButtonElement, _downRightButtonElement;
        private string sortpath;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor loads the Default Values
        /// </summary>
        public ListMover()
        {
            this.UpLeftButtonContent = ListMover.StringUpLeft;
            this.UpRightButtonContent = ListMover.StringUpRight;
            this.DownLeftButtonContent = ListMover.StringDownLeft;
            this.DownRightButtonContent = ListMover.StringDownRight;
            this.MoveRightButtonContent = ListMover.StringMoveRight;
            this.MoveLeftButtonContent = ListMover.StringMoveLeft;
            this.MoveRightAllButtonContent = ListMover.StringMoveAllRight;
            this.MoveLeftAllButtonContent = ListMover.StringMoveAllLeft;
            this.LeftListBoxHeader = ListMover.StringLeftListBoxHeader;
            this.RightListBoxHeader = ListMover.StringRightListBoxHeader;
            this.LeftListSelectionMode = SelectionMode.Extended;
            this.RightListSelectionMode = SelectionMode.Extended;
            this.LeftListBoxHeaderTextVisibility = Visibility.Visible;
            this.RightListBoxHeaderTextVisibility = Visibility.Visible;
            this.DefaultStyleKey = typeof(ListMover);
            this.UpDownButtonVisibility = Visibility.Collapsed;
            //this.ApplyTemplate();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// List of Moved Items to Left ListBox
        /// </summary>
        private IList _toListItems;
        public IList ToListItems
        {
            get
            {
                return _toListItems;
            }

            set
            {
                _toListItems = value;
            }
        }

        /// <summary>
        /// List of Moved Items to Right ListBox
        /// </summary>
        private IList _fromListItems;
        public IList FromListItems
        {
            get
            {
                return _fromListItems;
            }

            set
            {
                _fromListItems = value;
            }
        }


        #region Dependency Properties

        /// <summary>
        /// Left List Box ItemTemplate
        /// </summary>
        public static readonly DependencyProperty LeftListBoxItemTemplateProperty =
                   DependencyProperty.Register("LeftListBoxItemTemplate", typeof(DataTemplate), typeof(ListMover), new PropertyMetadata(new PropertyChangedCallback(AssignLeftListBoxItemTemplate)));

        private static void AssignLeftListBoxItemTemplate(DependencyObject dpObj, DependencyPropertyChangedEventArgs change)
        {
            ((ListMover)dpObj).LeftListBoxItemTemplate = (DataTemplate)change.NewValue;
        }

        public DataTemplate LeftListBoxItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(LeftListBoxItemTemplateProperty);
            }
            set
            {
                SetValue(LeftListBoxItemTemplateProperty, value);
            }
        }

        /// <summary>
        /// Right List Box sort flag
        /// </summary>
        public static readonly DependencyProperty RightListBoxSortProperty =
                   DependencyProperty.Register("RightListBoxSort", typeof(bool), typeof(ListMover), new PropertyMetadata(true));

        public bool RightListBoxSort
        {
            get
            {
                return (bool)GetValue(RightListBoxSortProperty);
            }
            set
            {
                SetValue(RightListBoxSortProperty, value);
            }
        }



        /// <summary>
        /// Right List Box ItemTemplate
        /// </summary>
        public static readonly DependencyProperty RightListBoxItemTemplateProperty =
              DependencyProperty.Register("RightListBoxItemTemplate", typeof(DataTemplate), typeof(ListMover), new PropertyMetadata(new PropertyChangedCallback(AssignRightListBoxItemTemplate)));

        private static void AssignRightListBoxItemTemplate(DependencyObject dpObj, DependencyPropertyChangedEventArgs change)
        {
            ((ListMover)dpObj).RightListBoxItemTemplate = (DataTemplate)change.NewValue;
        }

        public DataTemplate RightListBoxItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(RightListBoxItemTemplateProperty);
            }
            set
            {
                SetValue(RightListBoxItemTemplateProperty, value);
            }
        }


        /// <summary>
        /// Display memer path details for From and To ListBox
        /// </summary>
        public static readonly DependencyProperty DisplayMemberPathProperty =
               DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(ListMover), new PropertyMetadata(new PropertyChangedCallback(AssignDisplayMemberPath)));

        private static void AssignDisplayMemberPath(DependencyObject dpObj, DependencyPropertyChangedEventArgs change)
        {
            ((ListMover)dpObj).DisplayMemberPath = (string)change.NewValue;
        }

        public string DisplayMemberPath
        {
            get
            {
                return (string)GetValue(DisplayMemberPathProperty);
            }
            set
            {
                SetValue(DisplayMemberPathProperty, value);
            }
        }


        /// <summary>
        /// Sort Memebr path for both From and To ListBox
        /// </summary>
        public static readonly DependencyProperty SortMemberPathProperty =
               DependencyProperty.Register("SortMemberPath", typeof(string), typeof(ListMover), new PropertyMetadata(new PropertyChangedCallback(AssignSortMemberPath)));

        private static void AssignSortMemberPath(DependencyObject dpObj, DependencyPropertyChangedEventArgs change)
        {
            ((ListMover)dpObj).SortMemberPath = (string)change.NewValue;
        }

        public string SortMemberPath
        {
            get
            {
                return (string)GetValue(SortMemberPathProperty);
            }
            set
            {
                SetValue(SortMemberPathProperty, value);
            }
        }

        private void LoadItems()
        {

            if (ListSource != null && _leftListBoxElement != null)
            {
                //Right List Box and Left List Box shares the same DisplayMemberPath
                _rightListBoxElement.DisplayMemberPath = _leftListBoxElement.DisplayMemberPath = DisplayMemberPath;

                //If SortMemberPath not given by user, DisplayMemberpath will be the default Sort path
                sortpath = string.IsNullOrEmpty(SortMemberPath) ? DisplayMemberPath : SortMemberPath;

                //Initialize the From ListItems and ToListItems to the ListSource Type
                FromListItems = initializeList();
                copyList(FromListItems);
                ToListItems = initializeList();

                //If Default Items needs to be moved to Right List Box [Special condition, only when RightListSource is Set]
                moveDefaultItemstoRightList(ToListItems);
                applyBindings();

                _leftListBoxElement.ItemTemplate = LeftListBoxItemTemplate;
                _rightListBoxElement.ItemTemplate = RightListBoxItemTemplate;
            }
        }


        /// <summary>
        /// Source List Property for From ListBox
        /// </summary>
        public static readonly DependencyProperty ListSourceProperty =
               DependencyProperty.Register("ListSource", typeof(IList), typeof(ListMover), new PropertyMetadata(new PropertyChangedCallback(AssignListSource)));

        private static void AssignListSource(DependencyObject dpObj, DependencyPropertyChangedEventArgs change)
        {
            ((ListMover)dpObj).ListSource = (IList)change.NewValue;
            ((ListMover)dpObj).LoadItems();
        }

        public IList ListSource
        {
            get
            {
                return (IList)GetValue(ListSourceProperty);
            }
            set
            {
                SetValue(ListSourceProperty, value);
            }
        }


        /// <summary>
        /// Source List Property for To ListBox
        /// </summary>
        public static readonly DependencyProperty RightListSourceProperty =
               DependencyProperty.Register("RightListSource", typeof(IList), typeof(ListMover), new PropertyMetadata(new PropertyChangedCallback(AssignRightListSource)));

        private static void AssignRightListSource(DependencyObject dpObj, DependencyPropertyChangedEventArgs change)
        {
            ((ListMover)dpObj).RightListSource = (IList)change.NewValue;
            ((ListMover)dpObj).LoadItems();
        }

        public IList RightListSource
        {
            get
            {
                return (IList)GetValue(RightListSourceProperty);
            }
            set
            {
                SetValue(RightListSourceProperty, value);
            }
        }


        /// <summary>
        /// "Move" Buttons Column Width 
        /// </summary>
        public static readonly DependencyProperty ButtonColumnWidthProperty =
               DependencyProperty.Register("ButtonColumnWidth", typeof(int), typeof(ListMover), null);

        public int ButtonColumnWidth
        {
            get
            {
                return (int)GetValue(ButtonColumnWidthProperty);
            }
            set
            {
                SetValue(ButtonColumnWidthProperty, value);
            }
        }


        /// <summary>
        /// Up Left Button Content
        /// </summary>
        public static readonly DependencyProperty UpLeftButtonContentProperty =
        DependencyProperty.Register("UpLeftButtonContent", typeof(string), typeof(ListMover), null);

        public string UpLeftButtonContent
        {
            get
            {
                return (string)GetValue(UpLeftButtonContentProperty);
            }
            set
            {
                SetValue(UpLeftButtonContentProperty, value);
            }
        }

        /// <summary>
        /// Down Left Button Content
        /// </summary>
        public static readonly DependencyProperty DownLeftButtonContentProperty =
        DependencyProperty.Register("DownLeftButtonContent", typeof(string), typeof(ListMover), null);

        public string DownLeftButtonContent
        {
            get
            {
                return (string)GetValue(DownLeftButtonContentProperty);
            }
            set
            {
                SetValue(DownLeftButtonContentProperty, value);
            }
        }

        /// <summary>
        /// Up Right Button Content
        /// </summary>
        public static readonly DependencyProperty UpRightButtonContentProperty =
        DependencyProperty.Register("UpRightButtonContent", typeof(string), typeof(ListMover), null);

        public string UpRightButtonContent
        {
            get
            {
                return (string)GetValue(UpRightButtonContentProperty);
            }
            set
            {
                SetValue(UpRightButtonContentProperty, value);
            }
        }

        /// <summary>
        /// Down Right Button Content
        /// </summary>
        public static readonly DependencyProperty DownRightButtonContentProperty =
        DependencyProperty.Register("DownRightButtonContent", typeof(string), typeof(ListMover), null);

        public string DownRightButtonContent
        {
            get
            {
                return (string)GetValue(DownRightButtonContentProperty);
            }
            set
            {
                SetValue(DownRightButtonContentProperty, value);
            }
        }

        /// <summary>
        /// Move Right Button Content
        /// </summary>
        public static readonly DependencyProperty MoveRightButtonContentProperty =
        DependencyProperty.Register("MoveRightButtonContent", typeof(string), typeof(ListMover), null);

        public string MoveRightButtonContent
        {
            get
            {
                return (string)GetValue(MoveRightButtonContentProperty);
            }
            set
            {
                SetValue(MoveRightButtonContentProperty, value);
            }
        }

        /// <summary>
        /// Move Left Button Content
        /// </summary>
        public static readonly DependencyProperty MoveLeftButtonContentProperty =
        DependencyProperty.Register("MoveLeftButtonContent", typeof(string), typeof(ListMover), null);

        public string MoveLeftButtonContent
        {
            get
            {
                return (string)GetValue(MoveLeftButtonContentProperty);
            }
            set
            {
                SetValue(MoveLeftButtonContentProperty, value);
            }
        }

        /// <summary>
        /// Move Right All Button Content
        /// </summary>
        public static readonly DependencyProperty MoveRightAllButtonContentProperty =
        DependencyProperty.Register("MoveRightAllButtonContent", typeof(string), typeof(ListMover), null);

        public string MoveRightAllButtonContent
        {
            get
            {
                return (string)GetValue(MoveRightAllButtonContentProperty);
            }
            set
            {
                SetValue(MoveRightAllButtonContentProperty, value);
            }
        }

        /// <summary>
        /// Move Left All Button Content
        /// </summary>
        public static readonly DependencyProperty MoveLeftAllButtonContentProperty =
        DependencyProperty.Register("MoveLeftAllButtonContent", typeof(string), typeof(ListMover), null);

        public string MoveLeftAllButtonContent
        {
            get
            {
                return (string)GetValue(MoveLeftAllButtonContentProperty);
            }
            set
            {
                SetValue(MoveLeftAllButtonContentProperty, value);
            }
        }


        /// <summary>
        /// Move Left All Button Content
        /// </summary>
        public static readonly DependencyProperty LeftListBoxHeaderProperty =
        DependencyProperty.Register("LeftListBoxHeader", typeof(string), typeof(ListMover), null);

        public string LeftListBoxHeader
        {
            get
            {
                return (string)GetValue(LeftListBoxHeaderProperty);
            }
            set
            {
                SetValue(LeftListBoxHeaderProperty, value);
            }
        }


        /// <summary>
        /// Move Left All Button Content
        /// </summary>
        public static readonly DependencyProperty RightListBoxHeaderProperty =
        DependencyProperty.Register("RightListBoxHeader", typeof(string), typeof(ListMover), null);

        public string RightListBoxHeader
        {
            get
            {
                return (string)GetValue(RightListBoxHeaderProperty);
            }
            set
            {
                SetValue(RightListBoxHeaderProperty, value);
            }
        }


        public static readonly DependencyProperty RightListSelectionModeProperty =
        DependencyProperty.Register("RightListSelectionMode", typeof(SelectionMode), typeof(ListMover), null);

        public SelectionMode RightListSelectionMode
        {
            get
            {
                return (SelectionMode)GetValue(RightListSelectionModeProperty);
            }
            set
            {
                SetValue(RightListSelectionModeProperty, value);
            }
        }

        public static readonly DependencyProperty LeftListSelectionModeProperty =
        DependencyProperty.Register("LeftListSelectionMode", typeof(SelectionMode), typeof(ListMover), null);

        public SelectionMode LeftListSelectionMode
        {
            get
            {
                return (SelectionMode)GetValue(LeftListSelectionModeProperty);
            }
            set
            {
                SetValue(LeftListSelectionModeProperty, value);
            }
        }


        public static readonly DependencyProperty LeftListBoxHeaderTextVisibilityProperty =
        DependencyProperty.Register("LeftListBoxHeaderTextVisibility", typeof(Visibility), typeof(ListMover), null);

        public Visibility LeftListBoxHeaderTextVisibility
        {
            get
            {
                return (Visibility)GetValue(LeftListBoxHeaderTextVisibilityProperty);
            }
            set
            {
                SetValue(LeftListBoxHeaderTextVisibilityProperty, value);
            }
        }

        public static readonly DependencyProperty UpDownButtonVisibilityProperty =
       DependencyProperty.Register("UpDownButtonVisibility", typeof(Visibility), typeof(ListMover), null);

        public Visibility UpDownButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(UpDownButtonVisibilityProperty);
            }
            set
            {
                SetValue(UpDownButtonVisibilityProperty, value);
            }
        }

        public static readonly DependencyProperty RightListBoxHeaderTextVisibilityProperty =
        DependencyProperty.Register("RightListBoxHeaderTextVisibility", typeof(Visibility), typeof(ListMover), null);

        public Visibility RightListBoxHeaderTextVisibility
        {
            get
            {
                return (Visibility)GetValue(RightListBoxHeaderTextVisibilityProperty);
            }
            set
            {
                SetValue(RightListBoxHeaderTextVisibilityProperty, value);
            }
        }

        #endregion

        #endregion Properties

        #region Methods

        #region Overridden Methods
        /// <summary>
        /// Overriden method to access elements and assign data and events
        /// </summary>
        public override void OnApplyTemplate()
        {
            #region Base Initialize
            base.OnApplyTemplate();
            _rootElement = GetTemplateChild(ListMover.RootElement) as FrameworkElement;
            #endregion Base Initialize

            if (_rootElement != null)
            {
                // Accessing Content Elements
                _rightListBoxElement = GetTemplateChild(ListMover.RightListBoxElement) as ListBox;
                _leftListBoxElement = GetTemplateChild(ListMover.LeftListBoxElement) as ListBox;
                _moveRightButtonElement = GetTemplateChild(ListMover.MoveRightButtonElement) as Button;
                _moveLeftButtonElement = GetTemplateChild(ListMover.MoveLeftButtonElement) as Button;
                _moveRightAllButtonElement = GetTemplateChild(ListMover.MoveRightAllButtonElement) as Button;
                _moveLeftAllButtonElement = GetTemplateChild(ListMover.MoveLeftAllButtonElement) as Button;

                _upLeftButtonElement = GetTemplateChild(ListMover.UpLeftButtonElement) as Button;
                _upRightButtonElement = GetTemplateChild(ListMover.UpRightButtonElement) as Button;
                _downLeftButtonElement = GetTemplateChild(ListMover.DownLeftButtonElement) as Button;
                _downRightButtonElement = GetTemplateChild(ListMover.DownRightButtonElement) as Button;

                // Content Elements Events

                _moveRightButtonElement.Click += new RoutedEventHandler(MoveRightButtonElement_Click);
                _moveLeftButtonElement.Click += new RoutedEventHandler(MoveLeftButtonElement_Click);
                _moveRightAllButtonElement.Click += new RoutedEventHandler(MoveRightAllButtonElement_Click);
                _moveLeftAllButtonElement.Click += new RoutedEventHandler(MoveLeftAllButtonElement_Click);

                _upLeftButtonElement.Click += new RoutedEventHandler(_upLeftButtonElement_Click);
                _upRightButtonElement.Click += new RoutedEventHandler(_upRightButtonElement_Click);
                _downRightButtonElement.Click += new RoutedEventHandler(_downRightButtonElement_Click);
                _downLeftButtonElement.Click += new RoutedEventHandler(_downLeftButtonElement_Click);

                this.LoadItems();
                // Data Assignations
                ////////if (ListSource != null)
                ////////{
                ////////    //////Right List Box and Left List Box shares the same DisplayMemberPath
                ////////    ////_rightListBoxElement.DisplayMemberPath = _leftListBoxElement.DisplayMemberPath = DisplayMemberPath;

                ////////    //////If SortMemberPath not given by user, DisplayMemberpath will be the default Sort path
                ////////    ////sortpath = string.IsNullOrEmpty(SortMemberPath) ? DisplayMemberPath : SortMemberPath;

                ////////    //////Initialize the From ListItems and ToListItems to the ListSource Type
                ////////    ////FromListItems = initializeList();
                ////////    ////copyList(FromListItems);
                ////////    ////ToListItems = initializeList();

                ////////    //////If Default Items needs to be moved to Right List Box [Special condition, only when RightListSource is Set]
                ////////    ////moveDefaultItemstoRightList(ToListItems);
                ////////    ////applyBindings();
                ////////    this.LoadItems();
                ////////}
            }

        }




        #endregion

        #region Core Control Methods


        void MoveLeftAllButtonElement_Click(object sender, RoutedEventArgs e)
        {
            moveAllItems(_rightListBoxElement, _leftListBoxElement);
        }

        void MoveRightAllButtonElement_Click(object sender, RoutedEventArgs e)
        {
            moveAllItems(_leftListBoxElement, _rightListBoxElement);
        }

        void MoveLeftButtonElement_Click(object sender, RoutedEventArgs e)
        {
            moveItem(_rightListBoxElement);
        }

        void MoveRightButtonElement_Click(object sender, RoutedEventArgs e)
        {
            moveItem(_leftListBoxElement);
        }

        void _upLeftButtonElement_Click(object sender, RoutedEventArgs e)
        {
            moveUpItem(_leftListBoxElement);
        }

        void _upRightButtonElement_Click(object sender, RoutedEventArgs e)
        {
            moveUpItem(_rightListBoxElement);
        }

        void _downLeftButtonElement_Click(object sender, RoutedEventArgs e)
        {
            moveDownItem(_leftListBoxElement);
        }

        void _downRightButtonElement_Click(object sender, RoutedEventArgs e)
        {
            moveDownItem(_rightListBoxElement);
        }


        #endregion Core Control Methods

        #region Private Utility Methods


        /// <summary>
        /// Applies DataBinding to ListBox Elements
        /// </summary>
        private void applyBindings()
        {
            applyDataBinding(_leftListBoxElement, FromListItems, true);
            applyDataBinding(_rightListBoxElement, ToListItems, RightListBoxSort);
        }


        /// <summary>
        /// To Apply DataBinding to ListBox
        /// </summary>
        /// <param name="lstBox"></param>
        /// <param name="listData"></param>
        /// <param name="isSortRequired"></param>
        private void applyDataBinding(ListBox lstBox, IList listData, bool isSortRequired)
        {
            lstBox.ItemsSource = null;
            lstBox.ItemsSource = listData;

            //Sorting Operation done , only when items are added and not when removed from a listbox.
            if (isSortRequired)
            {
                lstBox.ItemsSource = from element in lstBox.Items
                                     orderby (element.GetType().GetProperty(sortpath)).GetValue(element, null).ToString()
                                     select element;
            }
        }

        /// <summary>
        /// To Move Items from Source to Destination ListBox
        /// </summary>
        /// <param name="frmListBox"></param>
        private void moveItem(ListBox frmListBox)
        {
            if (frmListBox.SelectedItems.Count != 0)
            {
                if (frmListBox.Name == ListMover.LeftListBoxElement)
                {
                    foreach (var item in frmListBox.SelectedItems)
                    {
                        ToListItems.Add(item);
                        FromListItems.Remove(item);
                    }
                    applyDataBinding(_leftListBoxElement, FromListItems, true);
                    applyDataBinding(_rightListBoxElement, ToListItems, true);
                }
                else
                {
                    foreach (var item in frmListBox.SelectedItems)
                    {
                        FromListItems.Add(item);
                        ToListItems.Remove(item);
                    }
                    applyDataBinding(_leftListBoxElement, FromListItems, true);
                    applyDataBinding(_rightListBoxElement, ToListItems, true);
                }
                //applyBindings();
            }
        }


        /// <summary>
        /// To Move Items Up to Destination ListBox
        /// </summary>
        /// <param name="frmListBox"></param>
        private void moveUpItem(ListBox frmListBox)
        {
            if (frmListBox.SelectedItems.Count != 0 && frmListBox.SelectedIndex != 0)
            {
                if (frmListBox.Name == ListMover.LeftListBoxElement)
                {
                    foreach (var item in frmListBox.SelectedItems)
                    {
                        int selectedIndex = frmListBox.SelectedIndex;
                        int newIndex = selectedIndex - 1;
                        var PreviousItem = frmListBox.Items[selectedIndex - 1];
                        FromListItems.Remove(PreviousItem);
                        FromListItems.Remove(item);
                        FromListItems.Insert(newIndex, item);
                        FromListItems.Insert(selectedIndex, PreviousItem);
                        _leftListBoxElement.SelectedIndex = newIndex;
                    }
                    applyDataBinding(_leftListBoxElement, FromListItems, false);
                }
                else
                {
                    foreach (var item in frmListBox.SelectedItems)
                    {
                        int selectedIndex = frmListBox.SelectedIndex;
                        int newIndex = selectedIndex - 1;
                        var PreviousItem = frmListBox.Items[selectedIndex - 1];
                        ToListItems.Remove(PreviousItem);
                        ToListItems.Remove(item);
                        ToListItems.Insert(newIndex, item);
                        ToListItems.Insert(selectedIndex, PreviousItem);
                        _rightListBoxElement.SelectedIndex = newIndex;

                    }
                    applyDataBinding(_rightListBoxElement, ToListItems, false);
                }

            }
        }

        /// <summary>
        /// To Move Items Down to Destination ListBox
        /// </summary>
        /// <param name="frmListBox"></param>
        private void moveDownItem(ListBox frmListBox)
        {
            if (frmListBox.SelectedItems.Count != 0 && frmListBox.SelectedIndex + 1 != frmListBox.Items.Count)
            {
                if (frmListBox.Name == ListMover.LeftListBoxElement)
                {
                    foreach (var item in frmListBox.SelectedItems)
                    {
                        int selectedIndex = frmListBox.SelectedIndex;
                        int newIndex = selectedIndex + 1;
                        var NextItem = frmListBox.Items[selectedIndex + 1];
                        FromListItems.Remove(NextItem);
                        FromListItems.Remove(item);
                        FromListItems.Insert(selectedIndex, NextItem);
                        FromListItems.Insert(newIndex, item);
                        _leftListBoxElement.SelectedIndex = newIndex;

                    }
                    applyDataBinding(_leftListBoxElement, FromListItems, false);
                }
                else
                {
                    foreach (var item in frmListBox.SelectedItems)
                    {
                        int selectedIndex = frmListBox.SelectedIndex;
                        int newIndex = selectedIndex + 1;
                        var NextItem = frmListBox.Items[selectedIndex + 1];
                        ToListItems.Remove(NextItem);
                        ToListItems.Remove(item);
                        ToListItems.Insert(selectedIndex, NextItem);
                        ToListItems.Insert(newIndex, item);
                        _rightListBoxElement.SelectedIndex = newIndex;

                    }
                    applyDataBinding(_rightListBoxElement, ToListItems, false);
                }

            }
        }


        /// <summary>
        /// To Move all Items from a listbox to other
        /// </summary>
        /// <param name="frmListBox"></param>
        /// <param name="toListBox"></param>
        private void moveAllItems(ListBox frmListBox, ListBox toListBox)
        {
            applyDataBinding(frmListBox, null, false);
            applyDataBinding(toListBox, (IList)ListSource, true);

            //Clear Exisitng Items
            FromListItems = initializeList();
            ToListItems = initializeList();

            if (frmListBox.Name == ListMover.LeftListBoxElement)
                copyList(ToListItems);
            else
                copyList(FromListItems);
        }

        /// <summary>
        /// Clear all Items in frmListBox and toListBox
        /// </summary>
        /// <param name="frmListBox"></param>
        /// <param name="toListBox"></param>
        private void clearToAndFromList(ListBox frmListBox, ListBox toListBox)
        {
            applyDataBinding(frmListBox, null, false);
            applyDataBinding(toListBox, null, false);
        }

        /// <summary>
        /// To instantiate Lists
        /// </summary>
        /// <returns></returns>
        private IList initializeList()
        {
            if (ListSource != null)
            {
                Type t1 = ListSource.GetType();
                return (IList)Activator.CreateInstance(t1);
            }
            return null;
        }

        /// <summary>
        /// To add listsource to parametrized list
        /// </summary>
        /// <param name="ListItems"></param>
        private void copyList(IList ListItems)
        {
            if (ListSource != null)
            {
                foreach (var item in ListSource)
                {
                    ListItems.Add(item);
                }
            }
        }

        /// <summary>
        /// To Move right List Source elements to Right List [Special condition]
        /// </summary>
        /// <param name="ListItems"></param>
        private void moveDefaultItemstoRightList(IList ListItems)
        {
            if ((RightListSource != null) && (RightListSource.Count > 0))
            {
                foreach (var item in RightListSource)
                {
                    ToListItems.Add(item);
                    FromListItems.Remove(item);
                }
                applyBindings();
            }
        }


        #endregion Private Utility Methods

        # region Public Methods

        public void Clear()
        {
            moveAllItems(_rightListBoxElement, _leftListBoxElement);
        }

        public void ClearToAndFromList()
        {
            this.clearToAndFromList(_rightListBoxElement, _leftListBoxElement);
        }

        # endregion

        #endregion
    }
}
