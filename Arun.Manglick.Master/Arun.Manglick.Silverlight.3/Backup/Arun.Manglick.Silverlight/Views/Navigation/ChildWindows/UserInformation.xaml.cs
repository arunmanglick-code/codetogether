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
using System.ComponentModel;

namespace Arun.Manglick.Silverlight.Views.Navigation.ChildWindows
{
    public partial class UserInformation : ChildWindow
    {
        bool cancelFlag = false;

        public UserInformation()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public string UserName
        {
            get { return txtFirstName.Text + " " + txtLastName.Text; }
        }

        //protected override void OnClosed(EventArgs e)
        //{
        //    if (!cancelFlag)
        //    {
        //        base.OnClosed(e);
        //    }
        //    else
        //    {
                
        //    }
        //}

        protected override void OnClosing(CancelEventArgs e)
        {
            //EventHandler<CancelEventArgs> closing = this.Closing;
            //if (null != closing)
            //{
            //    closing(this, e);
            //}

            if (cancelFlag)
            {
                base.OnClosing(e);
            }
            else
            {
                e.Cancel = true;
            }
            
        }


    }
}
