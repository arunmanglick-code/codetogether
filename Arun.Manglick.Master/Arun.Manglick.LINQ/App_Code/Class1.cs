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
/// Summary description for Class1
/// </summary>
public class Class1
{
    public Class1()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private void temp()
    {
        String message = "Testing";
        String result = message ?? "Null Value";

        int? age = 55;
        int retirement = age ?? 58;
    }
}

public class my : System.Collections.IEnumerable
{
    #region IEnumerable Members

    public System.Collections.IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }

    #endregion
}
