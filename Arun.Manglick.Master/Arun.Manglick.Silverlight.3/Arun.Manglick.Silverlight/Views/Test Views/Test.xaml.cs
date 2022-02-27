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

namespace Arun.Manglick.Silverlight.Views.Test
{
    public partial class Test : UserControl
    {
        public Test()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            busyBee.IsBusy = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            busyBee.IsBusy = false;
        }
    }
}
