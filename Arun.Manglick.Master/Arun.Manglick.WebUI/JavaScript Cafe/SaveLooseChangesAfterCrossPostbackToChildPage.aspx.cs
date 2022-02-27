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
using Arun.Manglick.UI;

public partial class SaveLooseChangesAfterCrossPostbackToChildPage : BasePage
{
    #region Private Variables
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionManager.IsDirty)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UnSaved", "HandleChangeEvent();", true);
        }
    }

    #endregion

    #region Private Methods
    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            SessionManager.IsDirty = false;
            Response.Redirect("~/Home.aspx", false);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            txtChange.Text = String.Empty;
            chkCheckPass.Checked = false;
            SessionManager.IsDirty = false;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void btnSimpleCrossPostBack_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdfDirty.Value.Length > 0)
            {
                SessionManager.IsDirty = true;                
            }
            Response.Redirect("~/GoBack.aspx", false);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "UnSaved", "HandleChangeEvent();", true);
        
    }

    #endregion

}
