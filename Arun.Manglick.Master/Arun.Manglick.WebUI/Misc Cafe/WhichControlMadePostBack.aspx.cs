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

public partial class WhichControlMadePostBack : System.Web.UI.Page
{

    #region Private Variables
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = String.Empty;

        try
        {
            lblError.Text =  GetPostBackControl(this).ID.ToString();
        }
        catch (Exception ex)
        {
            lblError.Text = "Error";
        }
    }


    #endregion

    #region Private Methods

    private Control GetPostBackControl(Page page)
    {
        Control control = null;

        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);  // 1st Approach
        }
        else
        {
            foreach (string ctl in page.Request.Form)  // 2nd Approach
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }

    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void btnClose_Click(object sender, EventArgs e)
    {
       
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        
    }   

    #endregion

}
