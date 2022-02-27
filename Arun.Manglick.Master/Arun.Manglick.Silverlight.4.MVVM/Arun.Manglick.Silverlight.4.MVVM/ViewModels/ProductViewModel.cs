using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Arun.Manglick.Silverlight.Model;

namespace Arun.Manglick.Silverlight.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        #region Variables

        Product theProduct;
        ObservableCollection<Product> theProducts = new ObservableCollection<Product>();
        public event EventHandler LoadComplete;

        #endregion
        
        #region .ctor
        public ProductViewModel()
        {
        }
        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Product> AllProducts
        {
            get
            {
                return theProducts;
            }
            set
            {
                theProducts = value;
                this.NotifyPropertyChanged("AllProducts");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Product SingleProduct
        {
            get
            {
                if (theProducts.Count > 0)
                    return theProducts[0] as Product;

                return null;
            }
            set
            {
                theProduct = value;
                this.NotifyPropertyChanged("SingleProduct");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Refresh()
        {
            ObservableCollection<Product> lst = GetData();

            AllProducts = lst;
            SingleProduct = theProducts[0] as Product;

            if (LoadComplete != null) LoadComplete(this, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<Product> GetData()
        {
            // This could be a Service Call.
            ObservableCollection<Product> lst = new ObservableCollection<Product> {
                new Product{ ProductId=1, ModelNumber="AA", ModelName="AAA", UnitCost=12, Description= "AA DESC"},
                new Product{ ProductId=1, ModelNumber="BB", ModelName="BBB", UnitCost=12, Description= "BB DESC"},
                new Product{ ProductId=1, ModelNumber="CC", ModelName="CCC", UnitCost=12, Description= "CC DESC"}
            };
            return lst;
        }

        #endregion

        #region Property Changed Implementation

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
