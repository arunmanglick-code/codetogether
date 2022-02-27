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

namespace Arun.Manglick.WebApp
{
    public partial class Search : UserControl
    {
        public Search()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Search_Loaded);
        }

        void Search_Loaded(object sender, RoutedEventArgs e)
        {
            SearchButton.Click += new RoutedEventHandler(Search_Click);
        }

        void Search_Click(object sender, RoutedEventArgs e)
        {
            ServiceReference1.AMServiceClient webService = new Arun.Manglick.WebApp.ServiceReference1.AMServiceClient();
            
            //webService.GetCustomerByContactNameCompleted += new EventHandler<Arun.Manglick.WebApp.ServiceReference1.GetCustomerByContactNameCompletedEventArgs>(webService_GetCustomerByContactNameCompleted);
            //webService.GetCustomerByContactNameAsync("Ana Trujillo");            
        }

        void webService_GetCustomerByContactNameCompleted(object sender, Arun.Manglick.WebApp.ServiceReference1.GetCustomerByContactNameCompletedEventArgs e)
        {
            theDataGrid.ItemsSource = e.Result;
        }        
    }
}
