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

public partial class ClientSideFeatures : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        int rowIndex = e.Row.RowIndex;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "ChangeRowColor(this)");
            e.Row.Attributes.Add("onmouseout", "ResetRowColor(this)");
            e.Row.Attributes["ondblclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Edit$" + rowIndex);
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {        
    }
}
