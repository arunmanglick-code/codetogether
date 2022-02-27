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

public partial class CustomLinqExpressionSelectingEvent : System.Web.UI.Page
{

    #region Private Variables
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        LinqDataSource1.Selecting +=new EventHandler<LinqDataSourceSelectEventArgs>(LinqDataSource1_Selecting);
    }

    #endregion

    #region Private Methods
    
    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void LinqDataSource1_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
        NorthwindDataContext db = new NorthwindDataContext();
        var products = from p in db.MyProducts
                       where p.Category.CategoryName == "Beverages"
                       select p;

        e.Result = products;
    }

    #endregion

}
