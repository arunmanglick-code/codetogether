using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Browser;

namespace Arun.Manglick.Silverlight.Tutorial7
{
	public class Library : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private string PrivateQuery;
		private string PrivateName;
		private ObservableCollection<Book> PrivateBooks;

		public Library()
		{
		  PrivateName = string.Empty;
		  PrivateBooks = new ObservableCollection<Book>();

		  if (HtmlPage.IsEnabled == false)
		  {
			  GenerateDummyData();
		  }

		}

		public string Query
		{
			get
			{
				return PrivateQuery;
			}
			set
			{
				PrivateQuery = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Query"));
			}
		}
		public string Name
		{
			get
			{
				return PrivateName;
			}
			set
			{
				PrivateName = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Name"));
			}
		}
		public ObservableCollection<Book> Books
		{
		    get
		  {
			 if (PrivateBooks.Count < 1)
			 {
				 GetData();
			 }
			 return PrivateBooks;
		  }
			set
			{
				PrivateBooks = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Books"));
			}
		}

		public void GenerateDummyData()
		{
			Books = new ObservableCollection<Book>();

			ObservableCollection<Author> Authors = new ObservableCollection<Author>();
			Author Author1 = new Author();
			Author Author2 = new Author();
			Author1.Name = "Jesse Liberty";
			Author2.Name = "Mark Twain";
			Authors.Add(Author1);
			Authors.Add(Author2);



		  Book newBook = new Book("Programming Silverlight", Authors, 49.99, "0123456789", "O'Reilly", 1, 1, "2000", 5.0);
		  Books.Add(newBook);


		  Author2.Name = "Cormac McCarthy";
		  newBook = new Book("Programming Without Shoes", Authors, 149.99, "0123456789", "O'Reilly Media", 1, 1, "1955", 4.5);
		  Books.Add(newBook);

		  Author2.Name = "Ian McEwan";
		  newBook = new Book("Programming Without A Spec", Authors, 149.99, "0123456789", "O'Reilly Media", 1, 1, "1955", 4.3);
		  Books.Add(newBook);

		  Author2.Name = "Stephen King";
		  Books.Add(new Book("Programming Without A Conscience", Authors, 149.99, "0123456789", "O'Reilly Media", 1, 1, "1955", 4.1));


		}
	    public void GetData()
	   {

		  Name = "Liberty Books";
		  ObservableCollection<Author> Authors = new ObservableCollection<Author>();
		  Author Author1 = new Author();
		  Author Author2 = new Author();
		  Author Author3 = new Author();

		  Author1.Name = "Jesse Liberty";
		  Author2.Name = "Tim Heurer";
		  Authors.Add(Author1);
		  Authors.Add(Author2);

		  Book newBook = new Book("Programming Silverlight", Authors, 49.99, "TBD", "O'Reilly Media", 1, 1, "2009", 5.0);
		  PrivateBooks.Add(newBook);

		  Authors.Remove(Author2);
		  Author2 = new Author();
		  Author2.Name = "Alex Horovitz";
		  Authors.Add(Author2);

		  newBook = new Book("Programming .NET 3.5", Authors, 49.99, "0-596-51039-X", "O'Reilly Media", 1, 2, "2008", 4.7);
		  PrivateBooks.Add(newBook);

		  Authors.Remove(Author2);
		  Author2 = new Author();
		  Author2.Name = "Dan Hurwitz";
		  Author3.Name = "Brian Macdonald";
		  Authors.Add(Author2);
		  Authors.Add(Author3);

		  newBook = new Book("Learning ASP.NET 3.5", Authors, 44.99, "0-596-51845-5", "O'Reilly Media", 2, 1, "2008", 4.8);
		  PrivateBooks.Add(newBook);

		  Authors.Remove(Author2);
		  Authors.Remove(Author3);
		  Author2 = new Author();
		  Author2.Name = "Donald Xie";
		  Authors.Add(Author2);
		  PrivateBooks.Add(new Book("Programming C# 3.0", Authors, 44.99, "0-596-51845-5", "O'Reilly Media", 5, 2, "2008", 4.3));


		  Authors.Remove(Author2);
		  Author2 = new Author();
		  Author2.Name = "Dan Hurwitz";
		  Authors.Add(Author2);
		  PrivateBooks.Add(new Book("Programming .NET Windows Apps", Authors, 49.95, "0596003218", "O'Reilly Media", 1, 1, "2003", 3.7));


		  Authors.Remove(Author2);
		  Author2 = new Author();
		  Author2.Name = "Brad Jones";
		  Authors.Add(Author2);
		  PrivateBooks.Add(new Book("Teach Yourself C++ in 1 Hour", Authors, 44.99, "0672327112", "Sams", 6, 1, "2008", 2.9));

		  Authors.Remove(Author2);
		  Author2 = new Author();
		  Author2.Name = "David Horvath";
		  Authors.Add(Author2);
		  PrivateBooks.Add(new Book("Teach Yourself C++ in 24 Hours", Authors, 34.99, "0672326817", "Sams", 4, 1, "2004", 2.4));


		  Authors.Remove(Author2);
		  PrivateBooks.Add(new Book("Programming VB.NET", Authors, 39.95, "0596004389 ", "O'Reilly Media", 2, 1, "2003", 3.2));
		  PrivateBooks.Add(new Book("Visual C# 2005 Dev Notebook", Authors, 29.95, "059600799X", "O'Reilly Media", 1, 1, "2005", 3.7));
		  PrivateBooks.Add(new Book("Clouds To Code", Authors, 41.95, "1861000952", "Wrox", 1, 1, "1997", 4.1));
	   }


	}
} //end of root namespace