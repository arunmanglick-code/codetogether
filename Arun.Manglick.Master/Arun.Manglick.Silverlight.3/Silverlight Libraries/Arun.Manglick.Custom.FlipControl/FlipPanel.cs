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
using System.Windows.Controls.Primitives;

namespace Arun.Manglick.Custom.FlipControl
{
    [TemplatePart(Name = "FlipButton", Type = typeof(ToggleButton)),
    TemplatePart(Name = "FlipButtonAlternate", Type = typeof(ToggleButton)),
    TemplateVisualState(Name = "Normal", GroupName = "ViewStates"),
    TemplateVisualState(Name = "Flipped", GroupName = "ViewStates")]
    public class FlipPanel : Control
    {
        #region Private Variables

        public static readonly DependencyProperty FrontContentProperty = DependencyProperty.Register("FrontContent", typeof(object), typeof(FlipPanel), null);
        public static readonly DependencyProperty BackContentProperty = DependencyProperty.Register("BackContent", typeof(object), typeof(FlipPanel), null);
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(FlipPanel), null);        
        public static readonly DependencyProperty IsFlippedProperty = DependencyProperty.Register("IsFlipped", typeof(bool), typeof(FlipPanel), null);

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public object FrontContent
        {
            get
            {
                return GetValue(FrontContentProperty);
            }
            set
            {
                SetValue(FrontContentProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object BackContent
        {
            get
            {
                return GetValue(BackContentProperty);
            }
            set
            {
                SetValue(BackContentProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFlipped
        {
            get
            {
                return (bool)GetValue(IsFlippedProperty);
            }
            set
            {
                SetValue(IsFlippedProperty, value);
                ChangeVisualState(true);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public FlipPanel()
        {
            DefaultStyleKey = typeof(FlipPanel);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ToggleButton flipButton = base.GetTemplateChild("FlipButton") as ToggleButton;
            if (flipButton != null) flipButton.Click += flipButton_Click;

            // Allow for two flip buttons if needed (one for each side of the panel).
            // This is an optional design, as the control consumer may use template
            // that places the flip button outside of the panel sides, like the 
            // default template does.
            ToggleButton flipButtonAlternate = base.GetTemplateChild("FlipButtonAlternate") as ToggleButton;
            if (flipButtonAlternate != null)
                flipButtonAlternate.Click += flipButton_Click;

            this.ChangeVisualState(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flipButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsFlipped = !this.IsFlipped;
            ChangeVisualState(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useTransitions"></param>
        private void ChangeVisualState(bool useTransitions)
        {
            if (!this.IsFlipped)
            {                
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Flipped", useTransitions);                
            }
        }

        #endregion
    }

}
