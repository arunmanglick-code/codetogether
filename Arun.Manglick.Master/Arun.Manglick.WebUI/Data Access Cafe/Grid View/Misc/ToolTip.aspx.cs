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

public partial class ToolTip : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridView1.DataBind();
        }
    }
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        string str1, str2, str3, str4,str5;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            str1 = "'" + DataBinder.Eval(e.Row.DataItem, "Id").ToString() + "'";
            str2 = "'" + DataBinder.Eval(e.Row.DataItem, "Title").ToString() + "'";
            str3 = "'" + DataBinder.Eval(e.Row.DataItem, "Director").ToString() + "'";
            str4 = "'" + DataBinder.Eval(e.Row.DataItem, "DateReleased").ToString() + "'";
            str5 = str1 + "," + str2 + "," + str3 + "," + str4;
            e.Row.Attributes.Add("onmousemove", "ShowToolTip(" + str5 + ")");
            e.Row.Attributes.Add("onmouseout", "HideTooltip()");
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {        
    }
}
