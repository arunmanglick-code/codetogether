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

public partial class AJAXPostImplmentation_Master_Pages_UseMasterSimple : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridView1.DataSource = GetEmptyDataTable();
        //GridView1.DataBind();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        GridView1.DataSource = GetEmptyDataTable();
        GridView1.DataBind();
    }

    private DataTable GetEmptyDataTable()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath("~\\XML\\Table1.xml"));  // ../../XML/Table1.xml
            DataTable dt = ds.Tables[0];
            ds.Dispose();
            return dt;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
