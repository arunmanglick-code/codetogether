using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Arun.Manglick.CustomControls;

namespace Arun.Manglick.Silverlight.Views.Templates_And_CustomControls.CustomControls
{
    public partial class ListMoverControl : UserControl
    {
        public ListMoverControl()
        {
            InitializeComponent();
        }

        private void lstMover_Loaded(object sender, RoutedEventArgs e)
        {
            List<Product> source = new List<Product> {
            new Product{ProductId = 1, ProductName="Closeup ToothPaste"},
            new Product{ProductId = 2, ProductName="Cinthol Soap"},
            new Product{ProductId = 3, ProductName="Lomani Deo"},
            new Product{ProductId = 4, ProductName="Tital Watch"},
            new Product{ProductId = 5, ProductName="Yamaha Bike"},
            };

            Binding binding = new Binding();
            binding.Mode = BindingMode.OneWay;
            binding.Source = source;
            binding.Converter = null;
            binding.Path = new System.Windows.PropertyPath(source);  // Can be skipped

            lstMover.SetBinding(Arun.Manglick.CustomControls.ListMover.ListSourceProperty, binding);
        }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

    }
}
