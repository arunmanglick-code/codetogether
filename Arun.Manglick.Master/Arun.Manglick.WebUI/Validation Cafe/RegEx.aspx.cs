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

public partial class RegEx : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //System.Web.Security.SqlMembershipProvider;
        //System.Web.Security.ActiveDirectoryMembershipProvider;
        //System.Web.XmlSiteMapProvider;
        
        //System.Web.Profile.SqlProfileProvider;
        
        //System.Web.Security.SqlRoleProvider;
        //System.Web.UI.WebControls.WebParts.SqlPersonalizationProvider;

    }

    #region Private Variables
    #endregion

    #region Page Events
    #endregion

    #region Private Methods
    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void btnSetRegEx_Click(object sender, EventArgs e)
    {
        revMoveRow.ValidationExpression = txtRegEx.Text;
    }

    protected void btnCheckRegEx_Click(object sender, EventArgs e)
    {
        
    }

    #endregion

}
