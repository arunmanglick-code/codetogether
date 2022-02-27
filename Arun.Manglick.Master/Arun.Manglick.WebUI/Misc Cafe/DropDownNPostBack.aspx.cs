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

public partial class DropDownNPostBack : System.Web.UI.Page
{

    #region Private Variables

    private bool mTogglePipedSave = true;

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
      
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

    protected void btnHidden_Click(object sender, EventArgs e)
    {
       
    }

    protected void btnPostback_Click(object sender, EventArgs e)
    {
        
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!mTogglePipedSave)
        {
            return;
        }

        lblError.Text = "Saved Successfully";
    }       

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlStates.Items.Clear();

        switch (ddlCountry.SelectedItem.Text)
        {
            case "America":                
                ddlStates.Items.Add("USA 1");
                ddlStates.Items.Add("USA 2");
                ddlStates.Items.Add("USA 3");
                break;
            case "India":
                ddlStates.Items.Add("India 1");
                ddlStates.Items.Add("India 2");
                ddlStates.Items.Add("India 3");
                break;
            case "Australia":
                ddlStates.Items.Add("Australia 1");
                ddlStates.Items.Add("Australia 2");
                ddlStates.Items.Add("Australia 3");
                break;
        }

        ddlStates.DataBind();
        mTogglePipedSave = false;
    }
    
    #endregion
    
}
