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

public partial class TemplatePage : System.Web.UI.Page
{
    #region Private Variables
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        InformationSearch.SearchEvent += new EventHandler(InformationSearch_SearchEvent);  
    }
        
    #endregion

    #region Private Methods
    
    private void BindGrid()
    {
        GridView1.DataSource = GetEmptyDataTable();
        GridView1.DataBind();
    }

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

    #region Public Methods
    #endregion

    #region Control Events

    protected void InformationSearch_SearchEvent(object sender, EventArgs e)
    {
        InformationSearch1.FirstName.Text = "Arun";
        InformationSearch1.LastName.Text = "Manglick";
        InformationSearch1.Age.Text = "30";

        BindGrid();
    }   

    #endregion
}
