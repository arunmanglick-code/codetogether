using System.Collections.Generic;
using System.ComponentModel;

namespace Arun.Manglick.Silverlight
{
    public class Address : INotifyPropertyChanged
    {
        private string location;
        private string address1;
        private string address2;
        private string city;


        // implement the required event for the interface
        public event PropertyChangedEventHandler PropertyChanged;


        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                NotifyPropertyChanged("Location");
            }       // end set
        }           // end property

        public string Address1
        {
            get { return address1; }
            set
            {
                address1 = value;
                NotifyPropertyChanged("Address1");
            }       // end set

        }

        public string Address2
        {
            get { return address2; }
            set
            {
                address2 = value;
                NotifyPropertyChanged("Address2");
            }       // end set

        }

        public string City
        {
            get { return city; }
            set
            {
                city = value;
                NotifyPropertyChanged("City");
            }       // end set

        }



        // factoring out the call to the event
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }

        }
    }               // end class
}
