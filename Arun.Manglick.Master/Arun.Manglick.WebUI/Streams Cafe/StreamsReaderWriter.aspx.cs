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

public partial class TemplatePage : BasePage
{
    #region Private Variables

    private string pathName1 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"XML\\AuditXML.xml";
    private string pathName2 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"TempFolder\\Note.txt";
    

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #endregion

    #region Private Methods

    private void StreamReader(String path)
    {
        StreamReader myReader = new StreamReader(path);
        SessionManager.NotePadText = myReader.ReadToEnd();
        ShowHideError("Success", true);
        myReader.Close();
    }
    private void StreamWriter(String path)
    {
        StreamWriter myWriter = new StreamWriter(pathName2,true);
        String str = Environment.NewLine + "Extra Wriiten Text: " + DateTime.Now.ToLongDateString() + " : " + DateTime.Now.ToLongTimeString();
        myWriter.WriteLine(str);
        
        myWriter.Flush();
        myWriter.Close();
    }

    private void StreamReading(String path)
    {
        Stream myReader = File.Open(path, FileMode.Open, FileAccess.Read);
        byte[] arr = new byte[myReader.Length];
        myReader.Read(arr, 0, arr.Length);
        SessionManager.NotePadText = Encoding.Default.GetString(arr);
        ShowHideError("Success FileStreamReader", true);
        myReader.Close();
    }
    private void StreamWriting(String path)
    {
        Stream myWriter = File.Open(path, FileMode.Append, FileAccess.Write);
        String str = Environment.NewLine + "Extra Wriiten Text: " + DateTime.Now.ToLongDateString() + " : " + DateTime.Now.ToLongTimeString();
        byte[] arr = Encoding.Default.GetBytes(str);
        myWriter.Write(arr, 0, arr.Length);
        myWriter.Close();
    }

    private void FileStreamReader(String path)
    {
        FileStream myReader = new FileStream(path, FileMode.Open,FileAccess.Read);
        byte[] arr = new byte[myReader.Length];
        myReader.Read(arr, 0, arr.Length);
        SessionManager.NotePadText = Encoding.Default.GetString(arr);
        ShowHideError("Success FileStreamReader", true);
        myReader.Close();
    }
    private void FileStreamWriter(String path)
    {
        FileStream myWriter = new FileStream(path, FileMode.Append,FileAccess.Write);
        String str = Environment.NewLine + "Extra Wriiten Text: " + DateTime.Now.ToLongDateString() + " : " + DateTime.Now.ToLongTimeString();
        byte[] arr = Encoding.Default.GetBytes(str);
        myWriter.Write(arr, 0, arr.Length);
        myWriter.Close();
    }

    private void ShowHideError(string errorText, bool show)
    {
        lblError.Text = errorText;
        lblError.Visible = show;
    }

    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void btnStreamReader_Click(object sender, EventArgs e)
    {
        try
        {
            StreamReader(pathName1);
            ShowHideError("Success Read", true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }    
    protected void btnStreamWriter_Click(object sender, EventArgs e)
    {
        try
        {
            StreamWriter(pathName2);
            StreamReader(pathName2);
            ShowHideError("Success Writer", true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void btnFileStreamReader_Click(object sender, EventArgs e)
    {
        try
        {
            FileStreamReader(pathName1);
            ShowHideError("Success FileStreamReader", true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void btnFileStreamWriter_Click(object sender, EventArgs e)
    {
        try
        {
            FileStreamWriter(pathName2);
            FileStreamReader(pathName2);
            ShowHideError("Success FileStreamWriter", true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void btnStreamReading_Click(object sender, EventArgs e)
    {
        try
        {
            StreamReading(pathName1);
            ShowHideError("Success Read", true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void btnStreamWriting_Click(object sender, EventArgs e)
    {
        try
        {
            StreamWriting(pathName2);
            StreamReading(pathName2);
            ShowHideError("Success Writer", true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void lnkNotePad1_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(pathName2);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));

    }

    #endregion
}
