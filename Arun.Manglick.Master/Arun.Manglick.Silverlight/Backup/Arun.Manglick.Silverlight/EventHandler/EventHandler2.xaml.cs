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
using System.Text;


namespace Arun.Manglick.WebApp
{
    public partial class EventHandler2 : UserControl
    {
        public EventHandler2()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(EventHandler2_Loaded);
            
        }

        void EventHandler2_Loaded(object sender, RoutedEventArgs e)
        {
            Canvas1.MouseLeftButtonDown += new MouseButtonEventHandler(Canvas1_MouseLeftButtonDown);
            Canvas1.MouseLeave += new MouseEventHandler(Canvas1_MouseLeave);
            
        }

        void Canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Canvas c = sender as Canvas;
            SolidColorBrush newColor = new SolidColorBrush(Color.FromArgb(255, 100, 37, 0));
            c.Background = newColor;
        }

        void Canvas1_MouseLeave(object sender, MouseEventArgs e)
        {
            Canvas c = sender as Canvas;
            SolidColorBrush newColor = new SolidColorBrush(Color.FromArgb(255, 100, 77, 0));
            c.Background = newColor;
        }

        void Canvas1_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                FrameworkElement fe = e.OriginalSource as FrameworkElement;
                StringBuilder sb = new StringBuilder();
                sb.Append("source: " + fe.Name + "\n");
                sb.Append("relative x/y to source: " + e.GetPosition(fe) + "\n");
                sb.Append("Silverlight content area x/y : " + e.GetPosition(null));
                statusText.Text = sb.ToString();
            }

        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.G)
            {
                //check modifiers for Ctrl
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    statusText.Text = "Beep!";
                }
            }
            else
            {
                WriteText.Text += e.Key.ToString();
            }
        }
    }
}
