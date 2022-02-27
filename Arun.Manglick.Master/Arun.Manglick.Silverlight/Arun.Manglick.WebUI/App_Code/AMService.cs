using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: If you change the class name "AMService" here, you must also update the reference to "AMService" in Web.config.
public class AMService : IAMService
{
    public void DoWork()
    {
    }

    #region IAMService Members

    public List<Customer> GetCustomerByContactName(string lastName)
    {
        //NorthwindDataContext db = new NorthwindDataContext();
        //var products = from p in db.MyProducts
        //               where p.Category.CategoryName == "Beverages"
        //               select p;

        //GridView1.DataSource = products;
        //GridView1.DataBind();   

        DataClassesDataContext db = new DataClassesDataContext();
        var customers = from c in db.Customers
                        where c.ContactName.StartsWith(lastName)
                        select c;

        return customers.ToList();
                        
    }

    #endregion    
}
