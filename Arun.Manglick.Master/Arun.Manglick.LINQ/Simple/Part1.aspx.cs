using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Part1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Private Variables
    #endregion

    #region Page Events
    #endregion

    #region Private Methods

    private void QueryProduct()
    {
        NorthwindDataContext db = new NorthwindDataContext();
        var products = from p in db.MyProducts
                       where p.Category.CategoryName == "Beverages"
                       select p;

        GridView1.DataSource = products;
        GridView1.DataBind();        
    }

    private void UpdateProduct()
    {
        NorthwindDataContext db = new NorthwindDataContext();
        MyProduct product = db.MyProducts.Single(p => p.ProductName == "Chai");
        //db.MyProducts.Remove(product);

        product.UnitPrice = 99;
        product.UnitsInStock = 5;
        db.SubmitChanges();

        QueryProduct();
    }

    private void InsertProduct()
    {
        NorthwindDataContext db = new NorthwindDataContext();
        Category category = new Category();

        MyProduct product1 = new MyProduct();
        product1.ProductName = "Toy 1";

        MyProduct product2 = new MyProduct();
        product2.ProductName = "Toy 2";

        category.MyProducts.Add(product1);
        category.MyProducts.Add(product2);

        //db.Categories.Add(category);
        db.SubmitChanges();
    }

    private void InsertOrder()
    {
        NorthwindDataContext db = new NorthwindDataContext();
        Customer customer = db.Customers.Single(c => c.CustomerID == "ALFKI");

        Order order = new Order();
        order.OrderDate = DateTime.Now;
        order.RequiredDate = DateTime.Now.AddDays(3);
        order.ShipCity = "Beverly";
        order.ShipPostalCode = "33333";

        customer.Orders.Add(order);
        db.SubmitChanges();
    }

    private void DeleteProduct()
    {
        NorthwindDataContext db = new NorthwindDataContext();
        var MyProduct = from p in db.MyProducts
                      where p.ProductName.Contains("Toy")
                      select p;


        //db.Products.RemoveAll(product);        
        
        db.SubmitChanges();
    }

    private void QueryProductCallSP()
    {
        NorthwindDataContext db = new NorthwindDataContext();
        //var products = db.Ten_Most_Expensive_Products(); // Both works fine
        Ten_Most_Expensive_ProductsResult products = db.Ten_Most_Expensive_Products() as Ten_Most_Expensive_ProductsResult;
        
        GridView1.DataSource = products;
        GridView1.DataBind();    
    }

    private void Paging()
    {
        NorthwindDataContext db = new NorthwindDataContext();
        var products = from p in db.MyProducts
                       select p;

        products = products.Skip(50).Take(10); // Paging
        GridView1.DataSource = products;
        GridView1.DataBind();  
    }

    private void Shaping1()
    {
        NorthwindDataContext db = new NorthwindDataContext();
        var products = from p in db.MyProducts
                       select new
                       {
                           ID = p.ProductID,
                           Name = p.ProductName
                       };

        GridView1.DataSource = products;
        GridView1.DataBind();
    }

    private void Shaping2()
    {
        NorthwindDataContext db = new NorthwindDataContext();
        var products = from p in db.MyProducts
                       select new
                       {
                           ID = p.ProductID,
                           Name = p.ProductName,
                           NumOrders = p.Order_Details.Count,
                           Revenue = p.Order_Details.Sum(o => o.Quantity * o.UnitPrice)
                       };

        GridView1.DataSource = products;
        GridView1.DataBind();
    }

    private void OrderDetailsCount()
    {
        NorthwindDataContext db = new NorthwindDataContext();
        var products = from p in db.MyProducts
                       where p.Order_Details.Count > 5
                       select p;

        GridView1.DataSource = products;
        GridView1.DataBind();
    }

    private void ChangeDates()
    {
        NorthwindDataContext db = new NorthwindDataContext();
        Order order = new Order();
        order.OrderDate = DateTime.Now;
        order.RequiredDate = DateTime.Now.AddDays(-1);

        //db.Orders.Add(order);
        db.SubmitChanges();
    }

    private void QueryProductUsingSQLExpression()
    {
        NorthwindDataContext db = new NorthwindDataContext();
        var products = db.GetProductByCategory(1);

        foreach (MyProduct product in products)
        {
            product.UnitPrice = 6;
        }

        db.SubmitChanges();

        GridView1.DataSource = products;
        GridView1.DataBind();
    }

    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void btnQuery1_Click(object sender, EventArgs e)
    {
        QueryProduct();
    }

    protected void btnQuery2_Click(object sender, EventArgs e)
    {
        QueryProductCallSP();
    }

    protected void btnQuery3_Click(object sender, EventArgs e)
    {
        Paging();
    }

    protected void btnQuery4_Click(object sender, EventArgs e)
    {
        UpdateProduct();
    }

    protected void btnQuery5_Click(object sender, EventArgs e)
    {
        InsertProduct();
    }

    protected void btnQuery6_Click(object sender, EventArgs e)
    {
        DeleteProduct();
    }

    protected void btnQuery7_Click(object sender, EventArgs e)
    {
        Shaping1();
    }

    protected void btnQuery8_Click(object sender, EventArgs e)
    {
        InsertOrder();
    }

    protected void btnQuery9_Click(object sender, EventArgs e)
    {
        QueryProductUsingSQLExpression();
    }

    #endregion

}
