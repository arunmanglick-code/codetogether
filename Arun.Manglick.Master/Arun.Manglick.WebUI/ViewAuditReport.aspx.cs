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

public partial class AuditReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            BindMasterReport();
        }
        catch (Exception ex)
        {
            string str = ex.Message;
        }
    }
    private void BindMasterReport()
    {
        DataTable dt = Session["AuditTrailData"] as DataTable;
        GridViewMaster.DataSource = dt;
        GridViewMaster.DataBind();
    }    
}
