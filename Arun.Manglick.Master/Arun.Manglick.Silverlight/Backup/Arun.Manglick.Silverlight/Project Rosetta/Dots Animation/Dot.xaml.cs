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

namespace Arun.Manglick.Silverlight
{
    public partial class Dot : UserControl
    {
        public Dot()
        {
            InitializeComponent();
        }

        public Double X
        {
            get { return (double)this.GetValue(Canvas.LeftProperty); }
            set { this.SetValue(Canvas.LeftProperty, value); }
        }

        public Double Y
        {
            get { return (double)this.GetValue(Canvas.TopProperty); }
            set { this.SetValue(Canvas.TopProperty, value); }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            sbDrop.Begin();
        }

        private void sbDrop_Completed(object sender, EventArgs e)
        {
            (this.Parent as Canvas).Children.Remove(this);
        }
    }
}
