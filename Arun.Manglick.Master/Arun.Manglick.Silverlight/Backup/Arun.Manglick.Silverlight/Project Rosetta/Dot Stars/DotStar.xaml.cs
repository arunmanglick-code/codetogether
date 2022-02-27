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
    public partial class DotStar : UserControl
    {
        public DotStar()
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

        public double vx { get; set; }
        public double vy { get; set; }
    }
}
