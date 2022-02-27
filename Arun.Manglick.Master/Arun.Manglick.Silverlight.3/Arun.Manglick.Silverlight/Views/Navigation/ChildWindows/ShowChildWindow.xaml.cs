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

namespace Arun.Manglick.Silverlight.Views.Navigation.ChildWindows
{
    public partial class ShowChildWindow : UserControl
    {
        private UserInformation childWindow = new UserInformation();
        public ShowChildWindow()
        {
            InitializeComponent();
            childWindow.Closed += childWindow_Closed;
        }
        
        private void cmdEnterInfo_Click(object sender, RoutedEventArgs e)
        {            
            childWindow.Show();
        }
        private void childWindow_Closed(object sender, EventArgs e)
        {
            if (childWindow.DialogResult == true)
            {
                lblInfo.Text = "Welcome to this application, " + childWindow.UserName + ".";
            }
            else
            {
                lblInfo.Text = "Application Closed ";
            }
        }

    }
}
