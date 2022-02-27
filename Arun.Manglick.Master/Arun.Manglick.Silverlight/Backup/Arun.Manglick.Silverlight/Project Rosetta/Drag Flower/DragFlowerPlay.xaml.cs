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
    public partial class DragFlowerPlay : UserControl
    {
        #region Private Variables

        DragFlower dragPanel;
        double startX = 129;
        double startY = 120;
        int depth = 1;
        bool isDrag = false;

        #endregion

        #region Constructor

        public DragFlowerPlay()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseMove += new MouseEventHandler(Page_MouseMove);
            Random r = new Random();
            for (int i = 0; i < 6; i++)
            {
                DragFlower p = new DragFlower();
                double randomRad = r.Next(360) * (Math.PI / 180);
                p.X = startX + (168 * (i % 3));
                p.Y = startY + (148 * (Math.Floor(i / 3)));
                // each DragFlower will be incharge of loading its own image
                p.setPhoto(5 - i);
                p.MouseLeftButtonDown += new MouseButtonEventHandler(p_MouseLeftButtonDown);
                p.MouseLeftButtonUp += new MouseButtonEventHandler(p_MouseLeftButtonUp);
                LayoutRoot.Children.Add(p);
            }
        }

        void Page_MouseMove(object sender, MouseEventArgs e)
        {
            if ((isDrag == true) && (dragPanel != null))
            {
                dragPanel.targetX = e.GetPosition(this).X;
                dragPanel.targetY = e.GetPosition(this).Y;
            }
        }

        void p_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragPanel = sender as DragFlower;
            dragPanel.isDrag = true;
            dragPanel.SetValue(Canvas.ZIndexProperty, depth++);
            dragPanel.CaptureMouse();
            dragPanel.targetX = e.GetPosition(this).X;
            dragPanel.targetY = e.GetPosition(this).Y;
            double offsetx = dragPanel.X - e.GetPosition(this).X;
            double offsety = dragPanel.Y - e.GetPosition(this).Y;
            dragPanel.offset = new Point(offsetx, offsety);
            isDrag = true;
        }

        void p_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            (sender as DragFlower).ReleaseMouseCapture();
            (sender as DragFlower).isDrag = false;
            isDrag = false;
            dragPanel = null;
        }

        #endregion
    }
}
