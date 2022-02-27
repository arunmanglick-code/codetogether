using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;


namespace Arun.Manglick.SL3.BL
{
    [DataContract(Name = "Product", Namespace = "http://Arun.Manglick.SL3.BL")]
    public class Product
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

        [DataMember()]
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        [DataMember()]
        public string ModelNumber
        {
            get { return modelNumber; }
            set { modelNumber = value; }
        }

        [DataMember()]
        [StringLength(3, ErrorMessage = "Max Length cap is 3")]
        public string ModelName
        {
            get { return modelName; }
            set { modelName = value; }
        }

        [DataMember()]
        public double UnitCost
        {
            get { return unitCost; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Value Can't be less than 0.");
                else
                    unitCost = value;
            }
        }

        [DataMember()]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }



        #endregion
    }
}
