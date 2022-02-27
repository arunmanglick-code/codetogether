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


namespace Arun.Manglick.Silverlight
{
    public partial class LibraryStyle : UserControl
    {
        private LibraryBookStyle b1;                        // now a class member
        private LibraryBookStyle b2;
        private LibraryBookStyle currentBook;

        public LibraryStyle()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Change.Click +=  new RoutedEventHandler(Change_Click);

            b1 = new LibraryBookStyle();
            currentBook = b2 = new LibraryBookStyle();

            InitializeBook1(b1);
            InitializeBook2(b2);

            SetDataSourceComplete();
            //SetDataSourceIndividually();
        }

        private void InitializeBook1(LibraryBookStyle b)
        {
            b.Title = "Bleak House";
            b.Author = "Charles Dickens";
            b.MultipleAuthor = false;
            b.QuantityOnHand = 150;
            b.Chapters = new List<string>() 
            { 
                "In Chancery", 
                "In Fashion", 
                "A Progress", 
                "Telescoopic Philanthropy",
                "A Morning Adventure", 
                "Quite at Home", 
                "The Ghosts Walk", 
                "Covering Sins", 
                "Signs and Tokens", 
                "The Law Writer"
            };
        }
        private void InitializeBook2(LibraryBookStyle b)
        {
            b.Title = "Programming Silverlight";
            b.Author = "Jesse Liberty, Tim Heuer";
            b.MultipleAuthor = true;
            b.QuantityOnHand = 20;
            b.Chapters = new List<string>() { "Introduction", "Controls", "Events", "Styles" };
        }

        private void SetDataSourceComplete()
        {
            LayoutRoot.DataContext = currentBook;
        }
        private void SetDataSourceIndividually()
        {
            Title.DataContext = currentBook;
            Author.DataContext = currentBook;
            Chapters.DataContext = currentBook;
            MultipleAuthor.DataContext = currentBook;
            QuantityOnHand.DataContext = currentBook;
        }  
      
        void Change_Click(object sender, RoutedEventArgs e)
        {
            if (currentBook == b1)
            {
                currentBook = b2;
            }
            else
            {
                currentBook = b1;
            }

            SetDataSourceComplete();
        }

    }
}
