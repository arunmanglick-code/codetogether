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
    public partial class DBAccessUsingGridViewLINQWCF : UserControl
    {
        public DBAccessUsingGridViewLINQWCF()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DBAccessUsingGridViewLINQWCF_Loaded);
        }

        void DBAccessUsingGridViewLINQWCF_Loaded(object sender, RoutedEventArgs e)
        {
            Search.Click += new RoutedEventHandler(Search_Click);
        }

        void Search_Click(object sender, RoutedEventArgs e)
        {
            ServiceReference1.Service1Client webEmployeeService = new Arun.Manglick.Silverlight.ServiceReference1.Service1Client();
            webEmployeeService.GetEmployeeByLastNameCompleted += new EventHandler<Arun.Manglick.Silverlight.ServiceReference1.GetEmployeeByLastNameCompletedEventArgs>(webEmployeeService_GetEmployeeByLastNameCompleted);
            webEmployeeService.GetEmployeeByLastNameAsync(LastName.Text);
        }
               
        void webEmployeeService_GetEmployeeByLastNameCompleted(object sender, Arun.Manglick.Silverlight.ServiceReference1.GetEmployeeByLastNameCompletedEventArgs e)
        {
            theDataGrid.ItemsSource = e.Result;
        }
    }
}
