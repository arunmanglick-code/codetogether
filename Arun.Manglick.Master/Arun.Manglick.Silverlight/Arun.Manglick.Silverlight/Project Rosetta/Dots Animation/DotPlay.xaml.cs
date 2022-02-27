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
    public partial class DotPlay : UserControl
    {
        public DotPlay()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseMove += new MouseEventHandler(DotPlay_MouseMove);
        }

        void DotPlay_MouseMove(object sender, MouseEventArgs e)
        {
            Dot d = new Dot();
            d.X = e.GetPosition(this).X;
            d.Y = e.GetPosition(this).Y;
            DotPlayGround.Children.Add(d);
        }
    }
}
