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
using Arun.Manglick.UI;

public partial class Trail_Cafe_Trail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Page.Form.DefaultButton = Button2.UniqueID;
        

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {        
        //Page.SetFocus(TextBox2);
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        Class1 obj = new Class1();
        int i = 4;
        int j = 5;

        obj.Swap<int>(ref i, ref j);

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
}
