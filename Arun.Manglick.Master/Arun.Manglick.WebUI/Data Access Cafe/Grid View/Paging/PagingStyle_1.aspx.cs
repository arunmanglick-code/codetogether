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

public partial class Data_Access_Cafe_Grid_View_VariousFeatures : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            foreach (TableCell cell in e.Row.Cells)
            {
                if (cell.Controls.Count != 0)
                {
                    LinkButton lnkButton = cell.Controls[0] as LinkButton;
                    if (lnkButton != null)
                    {
                        lnkButton.ToolTip = "Sort By " + "'" + lnkButton.Text + "'";
                    }
                }
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtTitle.Text != string.Empty)
        {
            GridView1.EmptyDataText = "Sorry";
            srcMovies.FilterExpression = "Title='" + txtTitle.Text + "'";
        }
        else
        {
            srcMovies.FilterExpression = "Title like '%'";
        }
    }    
}
