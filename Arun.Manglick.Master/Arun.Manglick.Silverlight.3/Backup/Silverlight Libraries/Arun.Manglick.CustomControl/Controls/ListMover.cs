using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Arun.Manglick.CustomControl
{
    [TemplatePart(Name = "RootElement", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "LeftListBoxHeaderElement", Type = typeof(TextBlock))]
    [TemplatePart(Name = "RightListBoxHeaderElement", Type = typeof(TextBlock))]
    public class ListMover : ContentControl
    {
        #region Controls

        private FrameworkElement _rootElement;
        private ListBox _rightListBoxElement, _leftListBoxElement;
        private Button _moveRightButtonElement;

        #endregion

        #region Constants

        public const string StringLeftListBoxHeader = "Available:";
        public const string StringRightListBoxHeader = "Selected:";
        public const string StringMoveRight = "Right";

        #endregion

        #region .ctor

        public ListMover()
        {
            this.LeftListBoxHeader = ListMover.StringLeftListBoxHeader;
            this.RightListBoxHeader = ListMover.StringRightListBoxHeader;
            this.MoveRightButtonContent = ListMover.StringMoveRight;

            this.DefaultStyleKey = typeof(ListMover);
        }

        #endregion

        #region Dependency Properties

        #region LeftListBoxHeader

        /// <summary>
        /// Move Left All Button Content
        /// </summary>
        public static readonly DependencyProperty LeftListBoxHeaderProperty = DependencyProperty.Register("LeftListBoxHeader", typeof(string), typeof(ListMover), null);
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

        #endregion

        #region RightListBoxHeader

        /// <summary>
        /// Move Left All Button Content
        /// </summary>
        public static readonly DependencyProperty RightListBoxHeaderProperty = DependencyProperty.Register("RightListBoxHeader", typeof(string), typeof(ListMover), null);
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

        #endregion

        #region MoveRightButtonContent

        /// <summary>
        /// Move Right Button Content
        /// </summary>
        public static readonly DependencyProperty MoveRightButtonContentProperty =DependencyProperty.Register("MoveRightButtonContent", typeof(string), typeof(ListMover), null);
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

        #endregion

        #endregion

        #region Methods

        #region Overridden Methods

        /// <summary>
        /// Overriden method to access elements and assign data and events
        /// </summary>
        public override void OnApplyTemplate()
        {
            #region Base Initialize

            base.OnApplyTemplate();
            _rootElement = base.GetTemplateChild("RootElement") as FrameworkElement;

            #endregion Base Initialize

            if (_rootElement != null)
            {
                // Accessing Content Elements
                TextBlock leftListBoxHeaderElement = base.GetTemplateChild("LeftListBoxHeaderElement") as TextBlock;
                TextBlock rightListBoxHeaderElement = base.GetTemplateChild("RightListBoxHeaderElement") as TextBlock;
            }
        }

        #endregion

        #endregion

    }
}
