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
        //Page.ClientScript.RegisterHiddenField("__EVENTTARGET", "Button2");

//        Page.Form.DefaultButton = Button2.UniqueID;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Label1.Text = "DoPostBack 1";
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Label1.Text = "DoPostBack 2";
    }


    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        TextBox2.Text = "Value";
    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        TextBox2.Text = "Value Check";
    }
}
