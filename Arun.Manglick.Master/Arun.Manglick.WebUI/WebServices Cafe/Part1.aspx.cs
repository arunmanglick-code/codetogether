using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Arun.Manglick.BL;

public partial class Part1 : System.Web.UI.Page
{   

    #region Private Variables
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #endregion

    #region Private Methods
       
    
    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void btnDataTableReturnType_Click(object sender, EventArgs e)
    {
        PossibleReturnTypes.ReturnType objService = new PossibleReturnTypes.ReturnType();
        GridView1.DataSource = objService.GetEmptyDataTable();
        GridView1.DataBind();
    }

    protected void btnConvertedDataTableReturnType_Click(object sender, EventArgs e)
    {
        PossibleReturnTypes.ReturnType objService = new PossibleReturnTypes.ReturnType();
        GridView1.DataSource = objService.GetEmployeeObject().Profile;
        GridView1.DataBind();
    }

    protected void btnArrayListReturnType_Click(object sender, EventArgs e)
    {
        PossibleReturnTypes.ReturnType objService = new PossibleReturnTypes.ReturnType();
        GridView1.DataSource = objService.GetEmployeeArrayList();
        GridView1.DataBind();       
    }

    protected void btnGenericListReturnType_Click(object sender, EventArgs e)
    {
        PossibleReturnTypes.ReturnType objService = new PossibleReturnTypes.ReturnType();
        GridView1.DataSource = objService.GetEmployeeArrayList();
        GridView1.DataBind();
    }

    protected void btnHashTableReturnType_Click(object sender, EventArgs e)
    {
       // Not Allowed
       // Returning HashTable is not allowed 
       // In fact it does not allow to add Web Reference in the Client Application
                
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Message", "alert('Not Allowed');", true);
    }

    #endregion

}
