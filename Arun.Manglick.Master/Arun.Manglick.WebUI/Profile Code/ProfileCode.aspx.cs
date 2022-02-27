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
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Drawing;
using Arun.Manglick.BL;
using Arun.Manglick.UI;

public partial class ProfileCode : BasePage
{
    #region Private Variables

    private string pathName1 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"TempFolder\\FileName.xml";
    private string pathName2 = AppDomain.CurrentDomain.BaseDirectory.ToString() + "../" + @"JS\\Calendar.js";
    private string pathName3 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"XML\\AuditXML.xml";

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        ShowHideError(String.Empty, false);
    }

    #endregion

    #region Private Methods

    private void NonProfileStringCode()
    {
        int start = Environment.TickCount;
        for (int i = 0; i < 1000; i++)
        {
            string s = "";
            for (int j = 0; j < 100; j++)
            {
                s += "Outer index = ";
                s += i;
                s += " Inner index = ";
                s += j;
                s += " ";
            }
        }

        double totalSeconds = 0.001 * (Environment.TickCount - start);
        ShowHideError(totalSeconds.ToString(), true);
    }
    private void ProfileStringCode()
    {
        int start = Environment.TickCount;
        for (int i = 0; i < 1000; i++)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < 100; j++)
            {
                sb.Append("Outer index = ");
                sb.Append(i);
                sb.Append(" Inner index = ");
                sb.Append(j);
                sb.Append(" ");
            }
            string s = sb.ToString();

        }

        double totalSeconds = 0.001 * (Environment.TickCount - start);
        ShowHideError(totalSeconds.ToString(), true);
    }

    private void NonProfileBrushCode()
    {
        int start = Environment.TickCount;
        for (int i = 0; i < 100 * 1000; i++)
        {
            Brush b = new SolidBrush(Color.Black); // Brush has a finalizer
            string s = new string(' ', i % 37);
        }

        double totalSeconds = 0.001 * (Environment.TickCount - start);
        ShowHideError(totalSeconds.ToString(), true);
    }
    private void ProfileBrushCode()
    {
        int start = Environment.TickCount;
        for (int i = 0; i < 100 * 1000; i++)
        {
            using (Brush b = new SolidBrush(Color.Black))    // Brush has a finalizer
            {
                string s = new string(' ', i % 37);
            }
        }

        double totalSeconds = 0.001 * (Environment.TickCount - start);
        ShowHideError(totalSeconds.ToString(), true);
        //MessageBox.Show("Program ran for {0} seconds: " + totalSeconds.ToString());
    }

    private void NonProfileStreamCode()
    {
        int start = Environment.TickCount;

        string pathName1 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Profile Code\\Demo.dat";
        StreamReader r = new StreamReader(pathName1);
        string line;
        int lineCount = 0;
        int itemCount = 0;
        while ((line = r.ReadLine()) != null)
        {
            lineCount++;
            string[] items = line.Split();
            for (int i = 0; i < items.Length; i++)
            {
                itemCount++;
            }
        }
        r.Close();

        double totalSeconds = 0.001 * (Environment.TickCount - start);
        ShowHideError(totalSeconds.ToString(), true);
    }
    private void ProfileStreamCode()
    {
        
    }
    
    private void ShowHideError(string errorText, bool show)
    {
        lblError.Text = "Time Taken (In Secs): " + errorText;
        lblError.Visible = show;
    }

    #endregion
    
    #region Control Events
    
    protected void btnSerialize_Click(object sender, EventArgs e)
    {
        try
        {
            NonProfileStringCode();
        }
        catch (Exception ex)
        {            
            throw;
        }
    }
    protected void btnDeSerialize_Click(object sender, EventArgs e)
    {
        try
        {
            ProfileStringCode();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void btnNestedSerialize_Click(object sender, EventArgs e)
    {
        NonProfileBrushCode();
    }
    protected void btnNestedDeSerialize_Click(object sender, EventArgs e)
    {
        ProfileBrushCode();
    }

    protected void btnPurchaseOrderSerialize_Click(object sender, EventArgs e)
    {
        NonProfileStreamCode();
    }
    protected void btnPurchaseOrderDeSerialize_Click(object sender, EventArgs e)
    {
        ShowHideError("No Code Present", true);
    }
     

    protected void lnkNotePad1_Click(object sender, EventArgs e)
    {
        //base.ReadTextStream(GetClassFilePath("SerializePurchaseOrderClassXMLAtrribued.cs"));
        //Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));        
    }
    protected void lnkNotePad2_Click(object sender, EventArgs e)
    {
      
    }
    protected void lnkNotePad3_Click(object sender, EventArgs e)
    {
       
    }
    protected void lnkNotePad4_Click(object sender, EventArgs e)
    {
    }
    protected void lnkNotePad_Click(object sender, EventArgs e)
    {
       
    }

    #endregion
}
