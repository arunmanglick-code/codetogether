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

public partial class UserControls_Cafe_UserControls_Information : System.Web.UI.UserControl
{
    #region Page Events
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #endregion

    #region Public Properties

    public TextBox FirstName
    {
        get
        {
            return txtFirstName;
        }
    }

    public TextBox LastName
    {
        get
        {
            return txtLastName;
        }
    }

    public TextBox Age
    {
        get
        {
            return txtAge;
        }
    }

    #endregion
}
