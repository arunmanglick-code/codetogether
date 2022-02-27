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
using System.Windows.Media.Imaging;

namespace Arun.Manglick.Silverlight
{
    public partial class DragFlower : UserControl
    {
        #region Private Variables

        public double targetX = 0;
        public double targetY = 0;
        double vx = 0;
        double vy = 0;
        public bool isDrag = false;
        public Point offset = new Point(0, 0);
        Storyboard sb;

        #endregion

        #region Constructor

        public DragFlower()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Variables

        public double X
        {
            get { return (double)this.GetValue(Canvas.LeftProperty); }
            set { this.SetValue(Canvas.LeftProperty, value); }
        }

        public double Y
        {
            get { return (double)this.GetValue(Canvas.TopProperty); }
            set { this.SetValue(Canvas.TopProperty, value); }
        }

        #endregion
        
        #region Methods

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            sb = new Storyboard();
            sb.Completed += new EventHandler(sb_Completed);
            this.Resources.Add("sb", sb);
            sb.Begin();
        }

        public void setPhoto(int _index)
        {
            string url = "orchid" + _index.ToString() + ".png";
            image.Source = new BitmapImage(new Uri(url, UriKind.Relative));
        }

        void sb_Completed(object sender, EventArgs e)
        {
            if (this.isDrag)
            {
                double oldX = this.X;
                double oldY = this.Y;
                this.X += ((this.targetX + this.offset.X) - this.X) * .2;
                this.Y += ((this.targetY + this.offset.Y) - this.Y) * .2;
                vx = this.X - oldX;
                vy = this.Y - oldY;
            }
            else
            {
                this.X += vx;
                this.Y += vy;
                vx *= .96;
                vy *= .96;
            }
            if (this.X > 600) { this.X = 600; this.vx *= -1; }
            if (this.X < 0) { this.X = 0; this.vx *= -1; }
            if (this.Y > 400) { this.Y = 400; this.vy *= -1; }
            if (this.Y < 0) { this.Y = 0; this.vy *= -1; }
            sb.Begin();
        }

        #endregion
    }
}
