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

namespace Arun.Manglick.CustomControls
{
    [TemplatePart(Name = "LayoutRoot", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "UserIdElement", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PwdElement", Type = typeof(TextBlock))]
    [TemplatePart(Name = "ButtonElement", Type = typeof(Button))]
    public class GenericLoginControl : ContentControl
    {
        #region Controls

        private FrameworkElement _layoutRoot;

        #endregion

        #region Variables
        public event RoutedEventHandler LoginCompleted;
        private string header1;
        #endregion

        #region Constants

        public const string StringLeftListBoxHeader = "Available:";
        public const string StringRightListBoxHeader = "Selected:";
        public const string StringMoveRight = "Right";

        #endregion

        #region .ctor

        public GenericLoginControl()
        {
            DefaultStyleKey = typeof(GenericLoginControl);
        }

        #endregion

        #region Dependency Properties

        #region LeftListBoxHeader

        /// <summary>
        /// Move Left All Button Content
        /// </summary>
        public static readonly DependencyProperty UserIdHeaderProperty = DependencyProperty.Register("UserIdHeader", typeof(string), typeof(GenericLoginControl), null);
        public string UserIdHeader
        {
            get
            {
                return (string)GetValue(UserIdHeaderProperty);
            }
            set
            {
                SetValue(UserIdHeaderProperty, value);
            }
        }

        #endregion

        #region PasswordHeader

        /// <summary>
        /// Move Left All Button Content
        /// </summary>
        public static readonly DependencyProperty PasswordHeaderProperty = DependencyProperty.Register("PasswordHeader", typeof(string), typeof(GenericLoginControl), null);
        public string PasswordHeader
        {
            get
            {
                return (string)GetValue(PasswordHeaderProperty);
            }
            set
            {
                SetValue(PasswordHeaderProperty, value);
            }
        }

        public string PasswordHeader1
        {
            get
            {
                return header1;
            }
            set
            {
                header1 = value;
            }
        }

        #endregion

        #region ButtonContent

        /// <summary>
        /// Move Right Button Content
        /// </summary>
        public static readonly DependencyProperty LoginButtonContentProperty = DependencyProperty.Register("LoginButtonContent", typeof(string), typeof(GenericLoginControl), null);
        public string LoginButtonContent
        {
            get
            {
                return (string)GetValue(LoginButtonContentProperty);
            }
            set
            {
                SetValue(LoginButtonContentProperty, value);
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
            _layoutRoot = base.GetTemplateChild("LayoutRoot") as FrameworkElement;

            #endregion Base Initialize

            if (_layoutRoot != null)
            {
                // Accessing Content Elements
                TextBlock userIdHeaderElement = base.GetTemplateChild("UserIdElement") as TextBlock;
                TextBlock passwordHeaderElement = base.GetTemplateChild("PwdElement") as TextBlock;
                Button loginElement = base.GetTemplateChild("ButtonElement") as Button;

                loginElement.Click += new RoutedEventHandler(loginElement_Click);
            }
        }

        private void loginElement_Click(object sender, RoutedEventArgs e)
        {
            if (null != LoginCompleted)
                LoginCompleted(this, new RoutedEventArgs());
        }

        #endregion

        #endregion
    }
}
