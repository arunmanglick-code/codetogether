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

public partial class AJAXPostImplmentation_ScriptManagerControl_RegisterDataItem : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterAsyncPostBackControl(Button2);

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ScriptManager1.RegisterDataItem(TextBox1, "Done");
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
    }
}
