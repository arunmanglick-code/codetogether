using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Arun.Manglick.Silverlight.Tutorial7
{
	public class Book : INotifyPropertyChanged
	{
	    public event PropertyChangedEventHandler PropertyChanged;

	    public Book()
	   {

	   }

	    public Book(string title, ObservableCollection<Author> authorList, double coverPrice, string isbn10, string publisher, int edition, int printing, string pubYear, double rating)
	   {

		  this.Title = title;

		  this.Authors = new ObservableCollection<Author>();
		  foreach (Author auth in authorList)
		  {
			 this.Authors.Add(auth);
		  }

		  this.CoverPrice = coverPrice;
		  this.ISBN10 = isbn10;
		  this.Publisher = publisher;
		  this.Edition = edition;
		  this.Printing = printing;
		  this.PubYear = pubYear;
		  this.Rating = rating;

	   }

		private string privateTitle;
		private double privateCoverPrice;
		private ObservableCollection<Author> privateAuthors;
		private string privateISBN10;
		private string privatePublisher;
		private int privateEdition;
		private int privatePrinting;
		private string privatePubYear;
	    private double privateRating;


		public string Title
		{
			get
			{
				return privateTitle;
			}
			set
			{
			 privateTitle = value;
			 NotifyPropertyChanged("Title");
		  }
		}
		public string NumAuthors
		{
			get
			{
				return privateAuthors.Count.ToString();
			}
		}
		public ObservableCollection<Author> Authors
		{
			get
			{
				return privateAuthors;
			}
			internal set
			{
			 privateAuthors = value;
			 NotifyPropertyChanged("Authors");
		  }
		}
		public double CoverPrice
		{
			get
			{
				return privateCoverPrice;
			}
			set
			{
			 privateCoverPrice = value;
			 NotifyPropertyChanged("CoverPrice");
		  }
		}
		public string ISBN10
		{
			get
			{
				return privateISBN10;
			}
			set
			{
			 privateISBN10 = value;
			 NotifyPropertyChanged("ISBN10");
		  }
		}
		public string Publisher
		{
			get
			{
				return privatePublisher;
			}
			set
			{
			 privatePublisher = value;
			 NotifyPropertyChanged("Publisher");
		  }
		}
		public int Edition
		{
			get
			{
				return privateEdition;
			}
			set
			{
			 privateEdition = value;
			 NotifyPropertyChanged("Edition");
		  }
		}
		public int Printing
		{
			get
			{
				return privatePrinting;
			}
			set
			{
			 privatePrinting = value;
			 NotifyPropertyChanged("Printing");
		  }
		}
		public string PubYear
		{
			get
			{
				return privatePubYear;
			}
			set
			{
			 privatePubYear = value;
			 NotifyPropertyChanged("PubYear");
		  }
	   }
	    public double Rating
	   {
		  get
		  {
			 return privateRating;
		  }
		  set
		  {
			 privateRating = value;
			 NotifyPropertyChanged("Rating");
		  }
	   }

	    private void NotifyPropertyChanged(string propertyName)
	   {
		  if (PropertyChanged != null)
			  PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
	   }



	}

} //end of root namespace