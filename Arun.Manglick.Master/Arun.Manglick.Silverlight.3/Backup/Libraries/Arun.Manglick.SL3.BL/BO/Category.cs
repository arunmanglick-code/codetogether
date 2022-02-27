using System;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections.Generic;

namespace Arun.Manglick.SL3.BL
{
    [DataContract(Name = "Category", Namespace = "http://Arun.Manglick.SL3.BL")]
    public class Category : INotifyPropertyChanged
    {
        #region Private Variables

        private string categoryName;
        private List<Product> products;

        #endregion

        #region Property

        [DataMember()]
        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CategoryName"));
            }
        }

        [DataMember()]
        public List<Product> Products
        {
            get { return products; }
            set
            {
                products = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Products"));
            }
        }

        #endregion

        #region Constructor

        public Category(string categoryName, List<Product> products)
        {
            CategoryName = categoryName;
            Products = products;
        }

        public Category() { }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion
    }

}
