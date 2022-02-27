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

namespace Arun.Manglick.Silverlight.Views.DependencyAndRoutedEvents.RoutedEvents
{
    public partial class DragCircles : UserControl
    {
        // Keep track of when an ellipse is being dragged.
        private bool isDragging = false;
        private Point mouseOffset;

        public DragCircles()
        {
            InitializeComponent();            
        }

        private void canvas_Click(object sender, MouseButtonEventArgs e)
        {
            // Create an ellipse (unless the user is in the process
            // of dragging another one).
            if (!isDragging)
            {
                // Give the ellipse a 50-pixel diameter and a red fill.
                Ellipse ellipse = new Ellipse();
                ellipse.Fill = new SolidColorBrush(Colors.Red);
                ellipse.Width = 50;
                ellipse.Height = 50;

                // Use the current mouse position for the center of
                // the ellipse.                
                Point point = e.GetPosition(this);
                ellipse.SetValue(Canvas.TopProperty, point.Y - ellipse.Height / 2);
                ellipse.SetValue(Canvas.LeftProperty,point.X - ellipse.Width / 2);

                // Watch for left-button clicks.
                ellipse.MouseLeftButtonDown += ellipse_MouseDown;

                // Add the ellipse to the Canvas.
                parentCanvas.Children.Add(ellipse);
            }
        }
        private void ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Dragging mode begins.
            isDragging = true;
            Ellipse ellipse = (Ellipse)sender;

            // Get the position of the click relative to the ellipse
            // (so the top-left corner of the ellipse is (0,0).
            mouseOffset = e.GetPosition(ellipse);

            // Change the ellipse color.
            ellipse.Fill = new SolidColorBrush(Colors.Green);

            // Watch this ellipse for more mouse events.
            ellipse.MouseMove += ellipse_MouseMove;
            ellipse.MouseLeftButtonUp += ellipse_MouseUp;

            // Capture the mouse. This way you'll keep receiveing
            // the MouseMove event even if the user jerks the mouse
            // off the ellipse.
            ellipse.CaptureMouse();
        }
        private void ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Ellipse ellipse = (Ellipse)sender;

                // Get the position of the ellipse relative to the Canvas.
                Point point = e.GetPosition(parentCanvas);

                // Move the ellipse.
                ellipse.SetValue(Canvas.TopProperty, point.Y - mouseOffset.Y);
                ellipse.SetValue(Canvas.LeftProperty, point.X - mouseOffset.X);                
            }
        }
        private void ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                Ellipse ellipse = (Ellipse)sender;

                // Change the ellipse color.
                ellipse.Fill = new SolidColorBrush(Colors.Orange);

                // Don't watch the mouse events any longer.
                ellipse.MouseMove -= ellipse_MouseMove;
                ellipse.MouseLeftButtonUp -= ellipse_MouseUp;
                ellipse.ReleaseMouseCapture();

                isDragging = false;
            }
        }
    }
}
