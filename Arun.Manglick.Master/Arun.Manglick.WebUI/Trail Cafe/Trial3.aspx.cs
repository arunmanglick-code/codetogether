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

public partial class Trial3 : System.Web.UI.Page
{
    String XmlFileName2 = "~\\XML\\ListView.xml";

    protected void Page_Load(object sender, EventArgs e)
    {
        TextBox2.TabIndex = -1;

        DataTable dt = GetEmptyDataTable(XmlFileName2);
        CheckBoxList2.DataSource = dt;
        CheckBoxList2.DataBind();

        for (int index = 0; index < dt.Rows.Count; index++)
        {
            bool isGroupAssigned = Convert.ToBoolean(dt.Rows[index]["Active"]); // (dt.Select("GroupID=" + clstGroupList.Items[index].Value).Length > 0);
            CheckBoxList2.Items[index].Selected = isGroupAssigned;
        }
    }


    private DataTable GetEmptyDataTable(string fileName)
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath(fileName));
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
