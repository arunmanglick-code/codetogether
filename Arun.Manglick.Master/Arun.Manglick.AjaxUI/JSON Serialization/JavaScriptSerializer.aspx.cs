using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using Arun.Manglick.UI;

public partial class JavaScriptSerializerDemo : System.Web.UI.Page
{

    #region Private Variables

    private JavaScriptSerializer serializer;

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        serializer = new JavaScriptSerializer();
        serializer.RegisterConverters(new JavaScriptConverter[] { new ListItemCollectionConverter() });

        this.SetFocus(TextBox1);
    }

    #endregion

    #region Private Methods
    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void searchButton_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(1000);
        ContactsGrid.SelectedIndex = -1;
        ContactsGrid.PageIndex = 0;
        ScriptManager1.SetFocus(TextBox1);
    }

    protected void saveButton_Click(object sender, EventArgs e)
    {
        SavedState.Text = serializer.Serialize(lstItems.Items);
        recoverButton.Enabled = true;
        Message.Text = "State saved";
    }

    protected void recoverButton_Click(object sender, EventArgs e)
    {
        //Recover the saved items of the ListBox control.

        ListItemCollection recoveredList = serializer.Deserialize<ListItemCollection>(SavedState.Text);
        ListItem[] newListItemArray = new ListItem[recoveredList.Count];
        recoveredList.CopyTo(newListItemArray, 0);

        lstItems.Items.Clear();
        lstItems.Items.AddRange(newListItemArray);
        Message.Text = "Last saved state recovered.";
    }

    protected void clearButton_Click(object sender, EventArgs e)
    {
        // Remove all items from the ListBox control.
        lstItems.Items.Clear();
        Message.Text = "All items removed";
    }

    protected void ContactsGrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = ContactsGrid.SelectedRow;
        string itemId = ContactsGrid.DataKeys[row.RowIndex].Value.ToString();
        ListItem newItem = new ListItem(row.Cells[4].Text, itemId);

        // Check if the item already exists in the ListBox control.
        if (!lstItems.Items.Contains(newItem))
        {
            lstItems.Items.Add(newItem);
            Message.Text = "Item added";
        }
        else
            Message.Text = "Item already exists";
    }

    protected void ContactsGrid_PageIndexChanged(object sender, EventArgs e)
    {
        ContactsGrid.SelectedIndex = -1;
    }


    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        SavedState.Visible = CheckBox1.Checked;
        StateLabel.Visible = CheckBox1.Checked;
    }


    #endregion

}
