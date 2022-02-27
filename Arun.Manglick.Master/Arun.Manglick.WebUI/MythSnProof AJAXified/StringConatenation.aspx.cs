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
using System.Text;
using RedGate.Profiler;

public partial class TemplatePage : System.Web.UI.Page
{
    private string _text = "The quick brown fox jumps over the lazy dog ";
    private int _iterations = 25000;
    private DateTime dtStart;
    private TimeSpan span;
    private double time1, time2 = 0; 

    protected void Page_Load(object sender, EventArgs e)
    {
        //StringConcat();
    }

    private void StringConcat()
    {
        AddStringsConcat();
        AddStringsBuilder();
    }


    private string AddStringsConcat()
    {
        string s = "";
        for (int i = 0; i < _iterations; i++)
        {
            s = s + _text;
        }
        return s;
    }

    private string AddStringsBuilder()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _iterations; i++)
        {
            sb.Append(_text);
        }
        return sb.ToString();
    }

    private string AddStringsJoin()
    {        
        String[] strArray = new String[_iterations];

        for (int i = 0; i < _iterations; i++)
        {
            strArray[i]= _text;
        }

        return String.Join(String.Empty, strArray);
    }

    protected void btnStringConcat_Click(object sender, EventArgs e)
    {
        dtStart = DateTime.Now;
        AddStringsConcat();
        span = DateTime.Now.Subtract(dtStart);
        time1 = span.TotalMilliseconds;
        time2 = span.TotalMilliseconds / 60;
        string str = "\\n \\n MSecs: " + time1.ToString() + "\\n Secs: " + time2.ToString();
        Response.Write("<script> alert('Time Taken: " + str + "'); </script>");
    }
    protected void btnStringBuilder_Click(object sender, EventArgs e)
    {
        dtStart = DateTime.Now;
        AddStringsBuilder();
        span = DateTime.Now.Subtract(dtStart);
        time1 = span.TotalMilliseconds;
        time2 = span.TotalMilliseconds / 60;
        string str = "\\n \\n MSecs: " + time1.ToString() + "\\n Secs: " + time2.ToString();
        Response.Write("<script> alert('Time Taken: " + str + "'); </script>");
    }
    protected void btnStringJoin_Click(object sender, EventArgs e)
    {
        dtStart = DateTime.Now;
        AddStringsJoin();
        span = DateTime.Now.Subtract(dtStart);
        time1 = span.TotalMilliseconds;
        time2 = span.TotalMilliseconds / 60;
        string str = "\\n \\n MSecs: " + time1.ToString() + "\\n Secs: " + time2.ToString();
        Response.Write("<script> alert('Time Taken: " + str + "'); </script>");
    }
}
