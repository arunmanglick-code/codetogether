using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public delegate void MyDelegate();

public partial class TemplatePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    MyEventClass.MyEvent += new MyDelegate(MyEventClass_MyEvent);
        //}
    }

    void MyEventClass_MyEvent()
    {}

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int i = Convert.ToInt16(lblCount.Text);
        i++;
        lblCount.Text = i.ToString();
    }
}

public class MyEventClass
{
    public static event MyDelegate MyEvent;    
}

