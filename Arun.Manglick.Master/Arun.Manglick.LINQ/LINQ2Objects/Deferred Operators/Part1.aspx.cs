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
using System.Text;

public partial class Part1 : System.Web.UI.Page
{
    #region Private Variables

    string[] presidents = null;
    int[] numbers = null;
    
    string strItems;
    IEnumerable<string> enuItems;
    IEnumerable<char> enuChars;
    IEnumerable<int> enuInts;
    string function;

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        presidents = new string[] { "Adams", "Arthur","Arthur", "Buchanan", "Bush", "Carter", "Cleveland",
                                            "Clinton", "Coolidge", "Eisenhower", "Fillmore", "Ford", "Garfield",
                                            "Grant", "Harding", "Harrison", "Hayes", "Hoover", "Jackson",
                                            "Jefferson", "Johnson", "Kennedy", "Lincoln","Linq", "Madison", "McKinley",
                                            "Monroe", "Nixon", "Pierce", "Polk", "Reagan", "Roosevelt", "Taft",
                                            "Taylor", "Truman", "Tyler", "Van Buren", "Washington", "Wilson"
                                            };

       numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    }

    #endregion

    #region Private Methods

    #region Restriction

    private void Restriction_Where()
    {
        function = "Start With - Lin";
        strItems = presidents.Where(p => p.StartsWith("Lin")).First();
        strItems = presidents.First(p => p.StartsWith("Lin"));

        function = "Start With - A";
        enuItems = presidents.Where(p => p.StartsWith("A"));
        strItems = GetResults(enuItems);

        function = "Words having third character in Lower Case";
        enuItems = presidents.Where(p => Char.IsLower(p[2]));
        strItems = GetResults(enuItems);

        function = "Every other element, the ones with an odd index position";
        enuItems = presidents.Where((p, i) => (i & 1) == 1);
        strItems = GetResults(enuItems);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);

    }
   
    #endregion

    #region Projection

    private void Projection_Select()
    {
        string resultObjectType = "A";

        function = "Select - Length";
        var varItems = presidents.Select(p => new { LastName = p, Length = p.Length });

        function = "Select - New Object";
        enuItems = presidents.Where(p => p.StartsWith("A"));
        varItems = enuItems.Select(p => new { LastName = p, Length = p.Length });

        resultObjectType = "B";
        var varItems1 = presidents.Select((p, i) => new { Index = i, LastName = p });
   
        #region Get Results Funda

        StringBuilder res = new StringBuilder();
        res.Remove(0, res.Length);
        switch (resultObjectType)
        {
            case "A":
                foreach (var item in varItems)
                {
                    res.Append(item.LastName + " is " + item.Length + " characters long").Append("\\n");
                }
                strItems = res.ToString();
                break;
            default:
                res = new StringBuilder();
                foreach (var item in varItems1)
                {
                    res.Append(item.Index + " . " + item.LastName).Append("\\n");
                }
                strItems = res.ToString();
                break;
        }
       
        
        #endregion
        
        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);

    }

    private void Projection_SelectMany()
    {
        function = "Select - Length";
        enuItems = presidents.Where(p => p.StartsWith("A"));
        enuChars = enuItems.SelectMany(p => p.ToCharArray());
        strItems = GetResults(enuChars);                

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    #endregion

    #region Partitioning

    private void Partitioning_Skip()
    {
        function = "Skip first 15 words";
        enuItems = presidents.Where(p => Char.IsLower(p[2])).Skip(15);
        strItems = GetResults(enuItems);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);

    }

    private void Partitioning_SkipWhile()
    {
        function = "Skip While Start With - A";
        enuItems = presidents.Where(p => Char.IsLower(p[2])).SkipWhile(p => p.StartsWith("A"));
        strItems = GetResults(enuItems);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);

    }

    private void Partitioning_Take()
    {
        function = "Take first 15 words";
        enuItems = presidents.Where(p => Char.IsLower(p[2])).Take(15);
        strItems = GetResults(enuItems);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);

    }

    private void Partitioning_TakeWhile()
    {
        function = "Take While Start With - A";
        enuItems = presidents.Where(p => Char.IsLower(p[2])).TakeWhile(p => p.StartsWith("A"));
        strItems = GetResults(enuItems);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);

    }

    #endregion

    #region Concat

    private void Concat()
    {
        function = "Take 5 then Skip 25 words";
        enuItems = presidents.Take(5).Concat(presidents.Skip(25));
        strItems = GetResults(enuItems);

        // An alternative technique for concatenating is to call the SelectMany operator on an array of sequences
        enuItems = new[] { presidents.Take(5), presidents.Skip(25) }.SelectMany(s => s); 
        strItems = GetResults(enuItems);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);

    }

    #endregion

    #region Ordering

    private void OrderBy()
    {
        function = "Order by Length";
        enuItems = presidents.OrderBy(s => s.Length);
        strItems = GetResults(enuItems);

        function = "Order by Length - Descending";
        enuItems = presidents.OrderByDescending(s => s.Length);
        strItems = GetResults(enuItems);

        function = "Order by Words";
        enuItems = presidents.OrderBy(s => s);
        strItems = GetResults(enuItems);

        function = "Order by Length Then Order By Words";
        enuItems = presidents.OrderBy(s => s.Length).OrderBy(s => s);
        strItems = GetResults(enuItems);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    private void ThenBy()
    {
        function = "Order by Length Then Order By Words";
        enuItems = presidents.OrderBy(s => s.Length).ThenBy(s => s); // Same as  OrderBy(s => s.Length);
        strItems = GetResults(enuItems);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    private void Reverse()
    {
        function = "Reverse";
        enuItems = presidents.Reverse();
        strItems = GetResults(enuItems);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    #endregion

    #region Set

    private void Distinct()
    {
        function = "Distinct";
        enuItems = presidents.Where(p => p.StartsWith("A")).Distinct();
        strItems = GetResults(enuItems);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    private void Except()
    {
        function = "Except";
        enuItems = presidents.Take(5).Except(presidents.Take(5));
        strItems = GetResults(enuItems);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    private void Intersect()
    {
        function = "Intersect";
        enuItems = presidents.Take(5).Intersect(presidents.Take(5));
        strItems = GetResults(enuItems);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    private void Union()
    {
        function = "Union";
        enuItems = presidents.Take(5).Union(presidents.Take(5));
        strItems = GetResults(enuItems);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    #endregion

    #region Element

    private void DefaultIfEmpty()
    {
        function = "DefaultIfEmpty";
        strItems = presidents.Where(p => p.StartsWith("Lind")).DefaultIfEmpty().First();

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    private void ElementAt()
    {
        function = "ElementAt - 5";
        strItems = presidents.ElementAt(5);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    private void First()
    {
        function = "First - Start With - Lin";
        strItems = presidents.Where(p => p.StartsWith("Lin")).First();

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    private void Last()
    {
        function = "Last - Start With - Lin";
        strItems = presidents.Where(p => p.StartsWith("Lin")).Last();

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    private void Single()
    {
        function = "Single - Start With - Lin";
        strItems = presidents.Where(p => p.StartsWith("Lin")).Single();

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    #endregion
    
    #region Generation

    private void Empty()
    {
        function = "Empty";
        enuItems = Enumerable.Empty<string>();
        strItems = GetResults(enuItems);

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    private void Range()
    {
        function = "Ranging 1 to 5";
        enuInts = Enumerable.Range(1, 5);
        strItems = GetResults(enuInts);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    private void Repeat()
    {
        function = "Repeat 1 (5 Times)";
        enuInts = Enumerable.Repeat(1, 5);
        strItems = GetResults(enuInts);

        strItems = !String.IsNullOrEmpty(strItems) ? strItems : "No Data Found";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Results", "alert('" + function + "'); alert('" + strItems + "');", true);
    }

    #endregion

    private void UpdateProduct()
    {
        NorthwindDataContext db = new NorthwindDataContext();
        MyProduct product = db.MyProducts.Single(p => p.ProductName == "Chai");
        //db.MyProducts.Remove(product);

        product.UnitPrice = 99;
        product.UnitsInStock = 5;
        db.SubmitChanges();

        
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

    public string GetResults(IEnumerable<string> results)
    {
        StringBuilder res = new StringBuilder();
        foreach (string item in results)
        {
            res.Append(item).Append("\\n");
        }

        return res.ToString();
    }

    public string GetResults(IEnumerable<char> results)
    {
        StringBuilder res = new StringBuilder();
        foreach (char item in results)
        {
            res.Append(item).Append("\\n");
        }

        return res.ToString();
    }

    public string GetResults(IEnumerable<int> results)
    {
        StringBuilder res = new StringBuilder();
        foreach (int item in results)
        {
            res.Append(item).Append("\\n");
        }

        return res.ToString();
    }


    #endregion

    #region Control Events

    protected void btnQuery1_Click(object sender, EventArgs e)
    {
        Restriction_Where();
    }

    protected void btnQuery2_Click(object sender, EventArgs e)
    {
        Projection_Select();
    }
    protected void btnQuery3_Click(object sender, EventArgs e)
    {
        Projection_SelectMany();
    }

    protected void btnQuery4_Click(object sender, EventArgs e)
    {
        Partitioning_Skip();
    }
    protected void btnQuery5_Click(object sender, EventArgs e)
    {
        Partitioning_SkipWhile();
    }
    protected void btnQuery6_Click(object sender, EventArgs e)
    {
        Partitioning_Take();
    }
    protected void btnQuery7_Click(object sender, EventArgs e)
    {
        Partitioning_TakeWhile();
    }

    protected void btnQuery8_Click(object sender, EventArgs e)
    {
        Concat();
    }

    protected void btnQuery9_Click(object sender, EventArgs e)
    {
        OrderBy();
    }
    protected void btnQuery10_Click(object sender, EventArgs e)
    {
        ThenBy();
    }
    protected void btnQuery11_Click(object sender, EventArgs e)
    {
        Reverse();
    }

    protected void Distinct_Click(object sender, EventArgs e)
    {
        Distinct();
    }
    protected void Except_Click(object sender, EventArgs e)
    {
        Except();
    }
    protected void Intersect_Click(object sender, EventArgs e)
    {
        Intersect();
    }
    protected void Union_Click(object sender, EventArgs e)
    {
        Union();
    }

    protected void btnQuery111_Click(object sender, EventArgs e)
    {
        DefaultIfEmpty();
    }
    protected void btnQuery12_Click(object sender, EventArgs e)
    {
        ElementAt();
    }
    protected void btnQuery13_Click(object sender, EventArgs e)
    {
        First();
    }
    protected void btnQuery14_Click(object sender, EventArgs e)
    {
        Last();
    }
    protected void btnQuery15_Click(object sender, EventArgs e)
    {
        Single();
    }

    protected void btnQuery16_Click(object sender, EventArgs e)
    {
        Empty();
    }
    protected void btnQuery17_Click(object sender, EventArgs e)
    {
        Range();
    }
    protected void btnQuery18_Click(object sender, EventArgs e)
    {
        Repeat();
    }

    #endregion

}
