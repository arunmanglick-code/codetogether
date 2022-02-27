using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class JsonSerializationOnServerCS : System.Web.UI.Page
{
    protected void cblCategories_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<int> selectedCategories = new List<int>();
        foreach (ListItem li in cblCategories.Items)
            if (li.Selected)
                selectedCategories.Add(Convert.ToInt32(li.Value));

        JavaScriptSerializer json = new JavaScriptSerializer();
        autoComplete1.ContextKey = json.Serialize(selectedCategories);
    }
}
