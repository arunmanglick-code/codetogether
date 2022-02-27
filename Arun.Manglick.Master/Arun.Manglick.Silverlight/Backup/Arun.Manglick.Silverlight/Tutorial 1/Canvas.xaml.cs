using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Arun.Manglick.Silverlight
{
    public partial class CanvasPage : UserControl
    {
        public CanvasPage()
        {
            InitializeComponent();
        }

        private void BallPlay_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new BallPlay());
        }

        private void DotStarPlay_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new DotStarPlay());
        }

        private void DotPlay_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new DotPlay());
        }

        private void DragFlowerPlay_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new DragFlowerPlay());
        }
    }
}
