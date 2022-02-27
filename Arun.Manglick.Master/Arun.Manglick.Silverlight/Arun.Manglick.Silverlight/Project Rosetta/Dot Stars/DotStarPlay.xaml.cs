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
    public partial class DotStarPlay : UserControl
    {
        Storyboard sb;

        public DotStarPlay()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            for (int i = 0; i < 100; i++)
            {
                DotStar d = new DotStar();
                d.X = r.NextDouble() * 600.0;
                d.Y = r.NextDouble() * 300.0;
                d.vx = 0;
                d.vy = 0;
                dots.Children.Add(d);
            }

            sb = new Storyboard();
            sb.Completed += new EventHandler(sb_Completed);
            this.Resources.Add("sb", sb);
            sb.Begin();
        }

        void sb_Completed(object sender, EventArgs e)
        {
            Random r = new Random();
            foreach (DotStar d in dots.Children)
            {
                d.vx += r.NextDouble()- r.NextDouble();
                d.vy += r.NextDouble() - r.NextDouble();
                d.X += d.vx;
                d.Y += d.vy;
                if ((d.X > 600) || (d.X < 0))
                {
                    d.X -= d.vx;
                    d.vx *= -1;
                }
                if ((d.Y > 300) || (d.Y < 0))
                {
                    d.Y -= d.vy;
                    d.vy *= -1;
                }
                d.vx *= .9;
                d.vy *= .9;
            }
            sb.Begin();
        }
    }
}
