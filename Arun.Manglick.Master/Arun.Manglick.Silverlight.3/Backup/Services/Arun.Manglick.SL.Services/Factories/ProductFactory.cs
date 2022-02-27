using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Web;
using Arun.Manglick.SL3.BL;

namespace Arun.Manglick.SL.Services
{
    class ProductFactory
    {
        public static Product GetProduct()
        {
            return null;
        }

        public static List<Product> GetProducts()
        {
            string path = HttpContext.Current.Server.MapPath(@"~\TestData\Products.xml");

            IEnumerable<XElement> elements = XElement.Load(path).Elements("Product");
            List<Product> products = new List<Product>();

            foreach (var element in elements)
            {
                Product product = new Product();

                product.ProductId = Convert.ToInt32(element.Elements("ProductId").First().Value);
                product.ModelName = Convert.ToString(element.Elements("ModelName").First().Value);
                product.ModelNumber = Convert.ToString(element.Elements("ModelNumber").First().Value);
                product.UnitCost = Convert.ToInt32(element.Elements("UnitCost").First().Value);
                product.Description = Convert.ToString(element.Elements("Description").First().Value);

                products.Add(product);
            } 

            return products;
        }
    }
}
