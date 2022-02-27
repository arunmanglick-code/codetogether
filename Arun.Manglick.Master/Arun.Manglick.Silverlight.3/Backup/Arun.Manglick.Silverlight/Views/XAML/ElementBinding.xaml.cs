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

namespace Arun.Manglick.Silverlight.Views.XAML
{
    public partial class ElementBinding : UserControl
    {
        public ElementBinding()
        {
            InitializeComponent();
        }


        private void cmd_SetSmall(object sender, RoutedEventArgs e)
        {
            sliderFontSize.Value = 2;
        }
        private void cmd_SetNormal(object sender, RoutedEventArgs e)
        {
            sliderFontSize.Value = this.FontSize;
        }
        private void cmd_SetLarge(object sender, RoutedEventArgs e)
        {
            // Only works in two-way mode.
            lblSampleText.FontSize = 60;
            // With a one-way binding use:
            //sliderFontSize.Value = 30;
        }
    }
}
