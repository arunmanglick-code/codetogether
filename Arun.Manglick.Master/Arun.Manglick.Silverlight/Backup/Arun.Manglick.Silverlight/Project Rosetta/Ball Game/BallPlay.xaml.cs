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
    public partial class BallPlay : UserControl
    {
        public BallPlay()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseLeftButtonDown += new MouseButtonEventHandler(BallPlay_MouseLeftButtonDown);
        }

        void BallPlay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ball ball = new Ball();
            ball.SetValue(Canvas.LeftProperty, e.GetPosition(this).X);
            ball.SetValue(Canvas.TopProperty, e.GetPosition(this).Y);

            BallGround.Children.Add(ball);
            
        }
    }
}
