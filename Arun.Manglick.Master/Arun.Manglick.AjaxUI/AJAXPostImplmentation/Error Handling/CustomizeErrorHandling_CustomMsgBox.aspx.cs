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

public partial class AJAXPostImplmentation_Error_Handling_CustomizeErrorHandling_CustomMsgBox : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        int i = 5;
        int j = 0;
        try
        {
            i = i / j;
        }
        catch (Exception ex)
        {
            ex.Data["ExtraInfo"] = "Explicitly raised Test Exception";
            throw ex;
        }
    }
}
