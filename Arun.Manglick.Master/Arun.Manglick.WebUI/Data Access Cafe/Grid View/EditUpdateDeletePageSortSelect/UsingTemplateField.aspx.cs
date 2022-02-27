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

public partial class UsingTemplateField : System.Web.UI.Page
{
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
   
    #endregion
}
