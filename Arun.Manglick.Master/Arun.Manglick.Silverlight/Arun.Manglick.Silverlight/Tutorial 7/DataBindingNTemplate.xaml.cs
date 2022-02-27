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

namespace Arun.Manglick.Silverlight.Tutorial7
{
    public partial class DataBindingNTemplate : UserControl
    {
        public DataBindingNTemplate()
        {
            InitializeComponent();
        }

        private void LibraryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book selectedBook = e.AddedItems[0] as Book;
            DetailsGrid.DataContext = selectedBook;
        }

        private void RatingSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider s = sender as Slider;
            SliderValueDisplay.Text = s.Value.ToString("N");
        }
    }
}
