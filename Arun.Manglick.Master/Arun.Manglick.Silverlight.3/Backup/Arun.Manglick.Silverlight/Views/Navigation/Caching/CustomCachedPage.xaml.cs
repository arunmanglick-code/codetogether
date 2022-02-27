using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace Arun.Manglick.Silverlight.Views.Navigation.Caching
{
    public partial class CustomCachedPage : Page
    {
        public CustomCachedPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Store the text box data.
            CustomCachedPage.TextBoxState = txtCached.Text;
            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Retrieve the text box data.
            if (CustomCachedPage.TextBoxState != null) txtCached.Text = CustomCachedPage.TextBoxState;
            base.OnNavigatedTo(e);
        }

        public static string TextBoxState
        { get; set; }
    }
}
