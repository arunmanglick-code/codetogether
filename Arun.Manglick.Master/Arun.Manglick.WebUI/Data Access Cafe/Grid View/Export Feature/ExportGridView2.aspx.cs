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
using System.IO;

public partial class Data_Access_Cafe_Grid_View_VariousFeatures : System.Web.UI.Page
{
    #region Private Variables

    string FileName = "GridView.xls";

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        GridView1.DataSource = GetEmptyDataTable();
        GridView1.DataBind();
    }

    #endregion

    #region Private Methods

    private DataTable GetEmptyDataTable()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath("~\\XML\\Table1.xml"));
            DataTable dt = ds.Tables[0];
            ds.Dispose();
            return dt;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    
    private void PrepareGridViewForExport(Control gv)
    {
        LinkButton lb = new LinkButton();
        Literal l = new Literal();
        string name = String.Empty;
        for (int i = 0; i < gv.Controls.Count; i++)
        {
            if (gv.Controls[i].GetType() == typeof(LinkButton))
            {
                l.Text = (gv.Controls[i] as LinkButton).Text;
                gv.Controls.Remove(gv.Controls[i]);
                gv.Controls.AddAt(i, l);
            }
            else if (gv.Controls[i].GetType() == typeof(DropDownList))
            {
                l.Text = (gv.Controls[i] as DropDownList).SelectedItem.Text;
                gv.Controls.Remove(gv.Controls[i]);
                gv.Controls.AddAt(i, l);
            }
            else if (gv.Controls[i].GetType() == typeof(CheckBox))
            {
                l.Text = (gv.Controls[i] as CheckBox).Checked ? "True" : "False";
                gv.Controls.Remove(gv.Controls[i]);
                gv.Controls.AddAt(i, l);
            }
            else if (gv.Controls[i].GetType() == typeof(TextBox))
            {
                l.Text = (gv.Controls[i] as TextBox).Text;
                gv.Controls.Remove(gv.Controls[i]);
                gv.Controls.AddAt(i, l);
            }
            if (gv.Controls[i].HasControls())
            {
                PrepareGridViewForExport(gv.Controls[i]);
            }
        }
    }
    private void Export(GridView gv)
    {
        string attachment = "attachment; filename= " + FileName;
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        gv.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    #endregion

    #region Control Events

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        PrepareGridViewForExport(GridView1);
        Export(GridView1);
    }

    #endregion
}
