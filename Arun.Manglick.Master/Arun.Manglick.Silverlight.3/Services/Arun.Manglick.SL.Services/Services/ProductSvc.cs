using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Arun.Manglick.SL3.BL;

namespace Arun.Manglick.SL.Services
{
    [ServiceContract(Namespace = "http://www.ArunManglick.Silverlight3.com/ProductSvc")]
    public interface IProductSvc
    {
        [OperationContract]
        List<Product> GetAllProducts();
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    class ProductSvc : IProductSvc
    {
        #region IProductSvc Members

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAllProducts()
        {
            List<Product> products = ProductFactory.GetProducts();
            return products;
        }

        #endregion
    }
}
