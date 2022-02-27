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
    public partial class KeyPressEvents : UserControl
    {
        public KeyPressEvents()
        {
            InitializeComponent();
        }

        private void txt_KeyUp(object sender, KeyEventArgs e)
        {
            string message = "KeyUp " + " Key: " + e.Key;
            lstMessages.Items.Add(message);
        }
        private void txt_KeyDown(object sender, KeyEventArgs e)
        {           
            string message = "KeyDown " + " Key: " + e.Key;
            lstMessages.Items.Add(message);
        }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            string message = "TextChanged";
            lstMessages.Items.Add(message);
        }

        private void cmdClear_Click(object sender, RoutedEventArgs e)
        {
            lstMessages.Items.Clear();
        }

    }
}
