using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Arun.Manglick.Silverlight
{
    public class Book
    {
        public Book()
        {
        }

        public Book(string isbn, string title, DateTime publishdate, double price)
        {
            this.ISBN = isbn;
            this.Title = title;
            this.PublishDate = publishdate;
            this.Price = price;
        }

        //Define the public properties
        public string ISBN { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public double Price { get; set; }
    }
}
