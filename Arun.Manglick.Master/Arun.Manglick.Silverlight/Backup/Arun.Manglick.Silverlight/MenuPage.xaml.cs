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
using Arun.Manglick.Silverlight.Tutorial5;
using Arun.Manglick.Silverlight.Tutorial7;

namespace Arun.Manglick.Silverlight
{
    public partial class MenuPage : UserControl
    {
        #region Page Events

        public MenuPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Control Events

        private void BallPlay_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new BallPlay());
        }
        private void DotStarPlay_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new DotStarPlay());
        }
        private void DotPlay_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new DotPlay());
        }
        private void DragFlowerPlay_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new DragFlowerPlay());
        }

        private void CanvasPage_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new CanvasPage());
        }
        private void DragAndDrop_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new DragAndDrop());
        }
        private void GridPage_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new GridPage());
        }
        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new StackPanel());
        }

        private void StackPanelHorizontal_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new StackPanelHorizontal());
        }
        private void Library_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new Library());
        }
        private void LibraryStyle_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new LibraryStyle());
        }
        private void MyBooksCollection_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new MyBooksCollection());
        }

        private void DiggSampleResearch_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new DiggSample());
        }
        private void DataAccess_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new DBAccessUsingGridViewLINQWCF());
        }
        private void UserControl_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new KeyboardSense());
        }
        private void SkinTemplate_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new LibraryNew());
        }

        private void DataBindingNTemplate_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(new DataBindingNTemplate());
        }

        #endregion

        private void ChatMessenger_Click(object sender, RoutedEventArgs e)
        {
            //App.Navigate(new ChatMessenger());
            //App.Navigate(new ChatApplication.Page());
        }

        
    }
}
