using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class MyBooksCollection : UserControl
    {
        private ObservableCollection<Book> mAllBooks;

        public MyBooksCollection()
        {
            InitializeComponent();
            mAllBooks = new ObservableCollection<Book>();
            mAllBooks.Add(new Book("4458907683", "Training Your Dog", new DateTime(2000, 2, 8), 44.25));
            mAllBooks.Add(new Book("3390092284", "All About Cats", new DateTime(2004, 3, 4), 12.99));
            MyBooks.DataContext = mAllBooks;
        }

        public ObservableCollection<Book> AllBooks
        {
            get
            {
                if (mAllBooks == null)
                {
                    mAllBooks = new ObservableCollection<Book>();
                    mAllBooks.Add(new Book("3390092284", "All About Dogs", new DateTime(2004, 3, 4), 12.99));

                }
                return mAllBooks;
            }
        }

        private void MyBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            BookDetails.DataContext = lb.SelectedItem;
        }
    }
}
