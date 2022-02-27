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
    public partial class DragAndDrop : UserControl
    {
        private double beginX;
        private double beginY;
        private bool trackingMouseMove = false;

        public DragAndDrop()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            myButton.Click += new RoutedEventHandler(myButton_Click);
            myButton.MouseLeftButtonDown += new MouseButtonEventHandler(CompositeButton_MouseLeftButtonDown);
            myButton.MouseLeftButtonUp += new MouseButtonEventHandler(CompositeButton_MouseLeftButtonUp);
            myButton.MouseMove += new MouseEventHandler(CompositeButton_MouseMove);

            rushOrder.Unchecked += new RoutedEventHandler(rushOrder_Unchecked);
            rushOrder.Checked += new RoutedEventHandler(rushOrder_Checked);

            CompositeButton.MouseLeftButtonDown += new MouseButtonEventHandler(CompositeButton_MouseLeftButtonDown);
            CompositeButton.MouseLeftButtonUp += new MouseButtonEventHandler(CompositeButton_MouseLeftButtonUp);
            CompositeButton.MouseMove += new MouseEventHandler(CompositeButton_MouseMove);



        }

        void CompositeButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (trackingMouseMove)
            {
                double currentX = e.GetPosition(null).X;
                double currentY = e.GetPosition(null).Y;

                FrameworkElement fe = sender as FrameworkElement;
                double canvLeft = Convert.ToDouble(fe.GetValue(Canvas.LeftProperty));
                double canvTop =  Convert.ToDouble(fe.GetValue(Canvas.TopProperty));

                double newLeft = canvLeft + currentX - beginX;
                double newTop = canvTop + currentY - beginY;

                fe.SetValue(Canvas.LeftProperty, newLeft);
                fe.SetValue(Canvas.TopProperty, newTop);

                beginX = currentX;
                beginY = currentY;
            }
        }

        void CompositeButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            fe.ReleaseMouseCapture();
            trackingMouseMove = false;
        }

        void CompositeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            beginX = e.GetPosition(null).X;
            beginY = e.GetPosition(null).Y;
            trackingMouseMove = true;
            FrameworkElement fe = sender as FrameworkElement;
            if (fe != null)
            {
                fe.CaptureMouse();
            }
        }
        
        void rushOrder_Checked(object sender, RoutedEventArgs e)
        {
            rushOrder.Content = "RUSH!";
        }

        void rushOrder_Unchecked(object sender, RoutedEventArgs e)
        {
            rushOrder.Content = "Rush";

        }

        void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Ouch";
        }
    }
}
