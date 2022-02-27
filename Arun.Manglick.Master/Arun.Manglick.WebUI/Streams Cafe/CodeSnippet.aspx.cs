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
using System.IO;
using System.Text;
using Arun.Manglick.BL;
using Arun.Manglick.UI;

public partial class CodeSnippet : BasePage
{
    #region Private Variables

    private string pathName1 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Streams Cafe\\CodeSnippets\\ConversionForStreams.txt";
    private string pathName2 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"TempFolder\\Note.txt";
    private string pathName3 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Streams Cafe\\CodeSnippets\\ReadWrite.txt";
    

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #endregion

    #region Private Methods
        
    #endregion

    #region Public Methods
    #endregion

    #region Control Events
    
    protected void lnkNotePad1_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(pathName1);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));
    }

    protected void lnkNotePad2_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(pathName3);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));
    }

    #endregion
}
