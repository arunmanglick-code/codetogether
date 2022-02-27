using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Arun.Manglick.Silverlight.Model
{
    public class Product : INotifyPropertyChanged
    {
        #region Private Variables

        private int productId;
        private string modelNumber;
        private string modelName;
        private double unitCost;
        private string description;

        #endregion

        #region Constructor

        public Product()
        {
        }

        public Product(string modelNumber, string modelName, double unitCost, string description)
        {
            ModelNumber = modelNumber;
            ModelName = modelName;
            UnitCost = unitCost;
            Description = description;
        }

        #endregion

        #region Properties

        public int ProductId
        {
            get { return productId; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Product Id - Can't be less than 0.");
                }
                else
                {
                    productId = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ProductId"));
                }
            }
        }
        public string ModelNumber
        {
            get { return modelNumber; }
            set
            {
                modelNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModelNumber"));
            }
        }
        public string ModelName
        {
            get { return modelName; }
            set
            {
                modelName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModelName"));
            }
        }
        public double UnitCost
        {
            get { return unitCost; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Can't be less than 0.");
                }
                else
                {
                    unitCost = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("UnitCost"));
                }
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion
    }
}
