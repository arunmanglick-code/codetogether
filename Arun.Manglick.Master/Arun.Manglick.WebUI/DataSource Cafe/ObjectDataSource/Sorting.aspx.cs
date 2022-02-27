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

public partial class Sorting : System.Web.UI.Page
{
    #region Private Variables

    string FileName = "GridView.xls";

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        //GridView1.DataSource = GetEmptyDataTable();
        //GridView1.DataBind();
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
        //Export(GridView1);
    }

    #endregion
}
