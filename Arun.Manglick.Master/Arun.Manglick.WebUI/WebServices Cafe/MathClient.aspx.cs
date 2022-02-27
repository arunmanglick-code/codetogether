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

public partial class MathClient : System.Web.UI.Page
{   

    #region Private Variables
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #endregion

    #region Private Methods

    
    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            Math.Math objMath = new Math.Math();
            int res = objMath.Addition(5, 5);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Result: " + res + "');", true);
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Error: " + ex.Message + "');", true);
        }
    }

    protected void btnSub_Click(object sender, EventArgs e)
    {
        try
        {
            Math.Math objMath = new Math.Math();
            float res = objMath.Subtract(10, 5);

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Result: " + res + "');", true);
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Error: " + ex.Message + "');", true);
        }     
    }

    protected void btnAddAsync_Click(object sender, EventArgs e)
    {
        try
        {
            Math.Math objMath = new Math.Math();
            AsyncCallback async = new AsyncCallback(MyCallBack);
            objMath.BeginAddition(5, 5, async, objMath);
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('Error: " + ex.Message + "');", true);
        }
    }

    private void MyCallBack(IAsyncResult ar)
    {
        Math.Math objMath = ar.AsyncState as Math.Math;        
        int res = objMath.EndAddition(ar);
        Label1.Text = res.ToString();
    }
    

    #endregion

}
