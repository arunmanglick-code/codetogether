using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;


namespace Arun.Manglick.Silverlight
{
    public class LibraryBook : INotifyPropertyChanged
    {
        private string bookTitle;
        private string bookAuthor;
        private int quantityOnHand;
        private bool multipleAuthor;
        private List<string> myChapters;
        
        public event PropertyChangedEventHandler PropertyChanged;

        #region Public Properties

        public string Title
        {
            get { return bookTitle; }
            set
            {
                bookTitle = value;
                NotifyPropertyChanged("Title");
            }       
        }          

        public string Author
        {
            get { return bookAuthor; }
            set
            {
                bookAuthor = value;
                NotifyPropertyChanged("Author");
            }       // end set

        }

        public int QuantityOnHand
        {
            get { return quantityOnHand; }
            set
            {
                quantityOnHand = value;
                NotifyPropertyChanged("QuantityOnHand");
            }       // end set
        }

        public bool MultipleAuthor
        {
            get { return multipleAuthor; }
            set
            {
                multipleAuthor = value;
                NotifyPropertyChanged("MultipleAuthor");
            }       // end set
        }

        public List<string> Chapters
        {
            get { return myChapters; }
            set
            {
                myChapters = value;
                NotifyPropertyChanged("Chapters");
            }
        }
        
        #endregion
        
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }           
}
