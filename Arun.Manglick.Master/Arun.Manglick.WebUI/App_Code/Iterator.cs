using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Arun.Manglick.UI
{
    
    class ProductsIterator
    {
        #region Private Variables

        private List<Product> products;
        private string _name;

        public string CategoryName
        {
            get { return _name; }
            set { _name = value; }
        }
        public int ProductCount
        {
            get
            {
                return products.Count;
            }

        }

        #endregion

        #region Constructor

        public ProductsIterator(string name) : this()
        {
            _name = name;
        }

        public ProductsIterator()
        {
            products = new List<Product>(5);
        }

        #endregion

        #region Methods

        public void AddProduct(Product p)
        {
            products.Add(p);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (Product p in products)
            {
                yield return p;
            }

            // return products.GetEnumerator();  // C# 1.x approach - Where Iterators were not in existence. Also to to this required is to implement the ICollection/IEnumerable interface.
        }

        #endregion
    }

    class Product
    {
        #region Private Variables

        private string _name;
        public string ProductName
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region Constructor

        public Product(string name)
        {
            _name = name;
        }
        public Product() { }

        #endregion

    }

    public class UseIterator
    {
        ProductsIterator productsIterator = null;

        #region Constructor

        public UseIterator()
        {
            MakeList();
        }

        #endregion

        #region Methods

        private void MakeList()
        {
            Product p1 = new Product("Product A");
            Product p2 = new Product("Product B");
            Product p3 = new Product("Product C");
            Product p4 = new Product("Product D");

            productsIterator = new ProductsIterator("Main Category");

            productsIterator.AddProduct(p1);
            productsIterator.AddProduct(p2);
            productsIterator.AddProduct(p3);
            productsIterator.AddProduct(p4);

            
        }

        public void Iterate()
        {            
            foreach (Product p in productsIterator)
            {
                Console.WriteLine(p.ProductName);
            }
        }

        #endregion
    }

}


