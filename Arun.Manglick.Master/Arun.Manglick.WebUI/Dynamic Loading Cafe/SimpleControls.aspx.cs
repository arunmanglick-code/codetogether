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

public partial class SimpleControls : System.Web.UI.Page
{
    #region Private Variables
    
    String XmlFileName2 = "~\\XML\\Status.xml";

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lbl = new Label();
        DropDownList dropDown = new DropDownList();
        Button btn = new Button();
        TextBox txt = new TextBox();

        dropDown.CssClass = "inputfield";
        btn.CssClass = "inputfield";
        txt.CssClass = "inputfield";

        btn.Click += new EventHandler(btn_Click);

        PlaceHolderLabel.Controls.Add(lbl);
        PlaceHolderDropDown.Controls.Add(dropDown);
        PlaceHolderButton.Controls.Add(btn);
        PlaceHolderTextBox.Controls.Add(txt);

        if (!IsPostBack)
        {
            lbl.Text = "Dynamically Label";

            dropDown.DataSource = GetDropDownDataTable();
            dropDown.DataTextField = "value";
            dropDown.DataValueField = "key";
            dropDown.DataBind();

            btn.Text = "Dynamically Button";
            txt.Text = "Dynamically TextBox";
        }

    }

    void btn_Click(object sender, EventArgs e)
    {
        lblError.Text = "Done";
    }


    #endregion

    #region Private Methods

    private DataTable GetDropDownDataTable()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath(XmlFileName2));
            DataTable dt = ds.Tables[0];
            ds.Dispose();
            return dt;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region Public Methods
    #endregion

    #region Control Events

  

    #endregion
    
}
