using System;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for LINQToXML
/// </summary>
public class LINQToXML : System.Collections.IEnumerable
{
    public string CourseID;
    public string Sequence;
    public string YearOfPassing;
    public string Institution;
    public string Course;
    public int Average;
    public List<String> Subjects;

    public LINQToXML()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region IEnumerable Members

    public System.Collections.IEnumerator GetEnumerator()
    {
        return Subjects.GetEnumerator();
    }

    #endregion
}
