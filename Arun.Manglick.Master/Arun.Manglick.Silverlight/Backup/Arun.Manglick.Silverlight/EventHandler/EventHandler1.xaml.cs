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

namespace Arun.Manglick.WebApp
{
    public partial class EventHandler1 : UserControl
    {
        public EventHandler1()
        {
            InitializeComponent();
        }

        private void Canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas c = sender as Canvas;
            SolidColorBrush newColor = new SolidColorBrush(Color.FromArgb(255, 200, 77, 0));
            c.Background = newColor;
        }
    }
}
