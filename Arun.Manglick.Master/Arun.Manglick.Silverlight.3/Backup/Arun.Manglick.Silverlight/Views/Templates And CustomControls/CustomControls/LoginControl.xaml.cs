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

namespace Arun.Manglick.Silverlight.Views.Templates_And_CustomControls.CustomControls
{
    public partial class LoginControl : UserControl
    {
        public LoginControl()
        {
            InitializeComponent();
        }

        private void GenericLoginControl_LoginCompleted(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Successful Login");
        }
    }
}
