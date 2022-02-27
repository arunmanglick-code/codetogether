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
    public partial class EventBubbling : UserControl
    {
        public EventBubbling()
        {
            InitializeComponent();
        }

        protected int eventCounter = 0;

        private void SomethingClicked(object sender, MouseButtonEventArgs e)
        {
            eventCounter++;
            string message = "#" + eventCounter.ToString() + ":\r\n" +
              " Sender: " + sender.ToString() + "\r\n" +
            " Handled: " + e.Handled + "\r\n";
            lstMessages.Items.Add(message);            
        }

        private void cmdClear_Click(object sender, RoutedEventArgs e)
        {
            lstMessages.Items.Clear();
            eventCounter = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            eventCounter++;
            string message = "#" + eventCounter.ToString() + ":\r\n" +
              " Sender: " + sender.ToString() + "\r\n";
            lstMessages.Items.Add(message);
        }
    }
}
