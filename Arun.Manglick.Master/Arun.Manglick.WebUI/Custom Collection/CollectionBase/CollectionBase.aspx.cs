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

public partial class CollectionBase : System.Web.UI.Page
{

    #region Private Variables
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        GridView1.DataSource = GetAllUsersCollection();
        GridView1.DataBind();
    }

    #endregion

    #region Private Methods

    private MyCollectionBase GetAllUsersCollection()
    {
        MyCollectionBase userList = new MyCollectionBase();
        UserI user = new UserI();
        user.Id = "1";
        user.Fname = "AA";
        user.Phone = "12345";
        user.Address = "Address 1";
        user.City = "City 1";
        userList.Add(user);

        user = new UserI();
        user.Id = "2";
        user.Fname = "BB";
        user.Phone = "12345";
        user.Address = "Address 2";
        user.City = "City 2";
        userList.Add(user);

        user = new UserI();
        user.Id = "3";
        user.Fname = "CC";
        user.Phone = "12345";
        user.Address = "Address 3";
        user.City = "City 3";
        userList.Add(user);

        user = new UserI();
        user.Id = "4";
        user.Fname = "DD";
        user.Phone = "12345";
        user.Address = "Address 4";
        user.City = "City 4";
        userList.Add(user);

        user = new UserI();
        user.Id = "5";
        user.Fname = "EE";
        user.Phone = "12345";
        user.Address = "Address 5";
        user.City = "City 5";
        userList.Add(user);

        return userList;
    }

    #endregion

    #region Public Methods
    #endregion

    #region Control Events
    #endregion

}
