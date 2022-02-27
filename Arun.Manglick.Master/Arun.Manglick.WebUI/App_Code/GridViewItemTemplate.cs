using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for GridViewItemTemplate
/// </summary>
public class GridViewItemTemplate : ITemplate
{
    #region Private Variables

    private DataControlRowType templateType;
    private string columnName;
    private string controlType;
    private string url;
    private string text;
    private DataTable status; 

    #endregion

    #region Constructor

    public GridViewItemTemplate(DataControlRowType type, string colname, string controlType, string URL, string Text)
    {
        templateType = type;
        columnName = colname;
        this.controlType = controlType;
        url = URL;
        text = Text;
    }
    public GridViewItemTemplate(DataControlRowType type, string colname, string controlType, string URL,DataTable status)
    {
        templateType = type;
        columnName = colname;
        this.controlType = controlType;
        url = URL;
        this.status = status;
    } 

    #endregion

    #region ITemplate Members

    void ITemplate.InstantiateIn(Control container)
    {
        switch (templateType)
        {
            case DataControlRowType.Header:
                Literal lc = new Literal();
                lc.Text = "<b>" + columnName + "</b>";
                container.Controls.Add(lc);
                break;
            case DataControlRowType.DataRow:
                if (controlType.Equals("txt"))
                {
                    TextBox textBox = new TextBox();
                    textBox.ID = "txt" + columnName;
                    textBox.CssClass = "inputfield";
                    textBox.EnableViewState = true;
                    textBox.DataBinding += new EventHandler(textBox_DataBinding);
                    container.Controls.Add(textBox);
                }
                else if (controlType.Equals("ddl"))
                {
                    DropDownList dropDown = new DropDownList();
                    dropDown.ID = "ddl" + columnName;                    
                    dropDown.CssClass = "inputfield";
                    dropDown.Width = 100;
                    dropDown.DataBinding += new EventHandler(dropDown_DataBinding); // Does not work here. The solution is 'GridView1_RowCreated' event
                    dropDown.DataSource = status;
                    dropDown.DataTextField = "value";
                    dropDown.DataValueField = "key";
                    dropDown.DataBind();

                    container.Controls.Add(dropDown);
                }
               break;
            default:
                break;
        }
    }

    void dropDown_DataBinding(object sender, EventArgs e)
    {
        //DropDownList dl = (DropDownList)sender;
        //GridViewRow row = (GridViewRow)dl.NamingContainer;
        //String strTemp = DataBinder.Eval(row.DataItem, "DynamicColumn2").ToString();
        //if (strTemp.Equals("A"))
        //{
        //    dl.SelectedIndex = 0;
        //}
        //else
        //{
        //    dl.SelectedIndex = 1;
        //}
    }

    void textBox_DataBinding(object sender, EventArgs e)
    {
        TextBox tl = (TextBox)sender;        
        GridViewRow row = (GridViewRow)tl.NamingContainer;
        tl.Text = DataBinder.Eval(row.DataItem, "DynamicColumn1").ToString();        
    }

    #endregion
}
