using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Trail_Cafe_Trial2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (HttpContext.Current.Items["Hello"] != null)
        //{
        //    Label1.Text = HttpContext.Current.Items["Hello"].ToString();
        //}
        //else
        //{
        //    Label1.Text = "No Data";
        //}
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            int? i = null;
            int j = i.GetValueOrDefault();

            Label1.Text = j.ToString();

            //Response.Redirect("http://" + Request.Url.DnsSafeHost + "/Arun.Manglick.AjaxUI/Home.aspx");
            //Response.Redirect(Request.Url.ToString() + "/../../.." + "/Arun.Manglick.AjaxUI/Home.aspx");
            //Response.Redirect(Request.Url.ToString().Replace(Request.RawUrl, "") + "/Arun.Manglick.AjaxUI/Home.aspx");

            //IClass obj1 = new Class1();            x
            //IClass obj2 = new Class2();

            //Class2 obj = obj1 as Class2;
            //obj.name;
            
            //Label1.Text = "success";


            //string str = "43";
            //Int32? ii = null;
            //int i  = -1;
            //int j = -1;
            //bool res = Int32.TryParse(str, out i);

            //if (res)
            //{
            //    Label1.Text = i.ToString();
            //}
            //else
            //{
            //    Label1.Text = j.ToString();
            //}

            //Label1.Text = i.ToString();
            //if (res)
            //{
            //    //i = Convert.ToDecimal(str);
            //}
            //else
            //{
            //    Label1.Text = "Cannot";
            //}

            //string str1 = "abc";
            //string str2 = "Abc";

            //Label1.Text = "Un Equal";
            //if (str1.Equals(str2,StringComparison.InvariantCultureIgnoreCase))
            //{
            //    Label1.Text = "Equal";
            //}

            //string i = "1";
            //if (i == Arun.Manglick.UI.MoveRow.MoveUp.ToString())
            //{
            //    Label1.Text = "Equal";
            //}

            //Label1.Text = Request.Url.AbsolutePath;
            //string str = String.Empty;
            //int j = str.Length;
            //Label1.Text = j.ToString();

            
        }
        catch (Exception ex)
        {

            Label1.Text = ex.Message;
        }

    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        Label1.Text = "Checked";
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        //Label1.Text = "Do it";
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        
       // Label1.Text = "Do it";


    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label1.Text = "Postback occured";
    }
}
