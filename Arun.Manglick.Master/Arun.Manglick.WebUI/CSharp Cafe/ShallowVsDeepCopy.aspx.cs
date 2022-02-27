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
using Arun.Manglick.UI;

public partial class ShallowVsDeepCopy : BasePage
{ 

    #region Private Variables

    private string pathName1 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"App_Code\\ShallowCopy.cs";
    private string pathName2 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"App_Code\\DeepCopy.cs";

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #endregion

    #region Private Methods

    private string GetClassFilePath(string fileName)
    {
        string pathName = AppDomain.CurrentDomain.BaseDirectory.ToString();
        pathName = pathName.Substring(0, pathName.LastIndexOf("Arun.Manglick"));
        pathName = pathName + "//Arun.Manglick.BL" + "//" + fileName;
        return pathName;
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

    protected void lnkNotePad1_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(pathName1);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));
    }
    protected void lnkNotePad2_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(pathName2);
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));
    }

    protected void btnBeforeShallowCopy_Click(object sender, EventArgs e)
    {
        try
        {
            CopyObject clsref = new CopyObject(1000);

            ShallowCopy m1 = new ShallowCopy();
            m1.Age = 25;
            m1.EmployeeName = "Ahmed Eid";
            m1.Salary = clsref;

            ShowHideError("Salary: " + m1.Salary.Salary.ToString(), true);
    
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void btnAfterShallowCopy_Click(object sender, EventArgs e)
    {
        try
        {
            CopyObject clsref = new CopyObject(1000);

            ShallowCopy m1 = new ShallowCopy();
            m1.Age = 25;
            m1.EmployeeName = "Ahmed Eid";
            m1.Salary = clsref;

            ShallowCopy m2 = m1.MakeShallowCopy(m1);
            m2.Salary.Salary = 2000;  // Same Happens if you do it as - clsref.Salary = 2000;


            ShowHideError("Salary: " + m1.Salary.Salary.ToString(), true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void btnBeforeDeepCopy_Click(object sender, EventArgs e)
    {
        try
        {
            CopyObject clsref = new CopyObject(3000);

            DeepCopy m1 = new DeepCopy();
            m1.Age = 25;
            m1.EmployeeName = "Ahmed Eid";
            m1.Salary = clsref;

            ShowHideError("Salary: " + m1.Salary.Salary.ToString(), true);

        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void btnAfterDeepCopy_Click(object sender, EventArgs e)
    {
        try
        {
            CopyObject clsref = new CopyObject(3000);

            DeepCopy m1 = new DeepCopy();
            m1.Age = 25;
            m1.EmployeeName = "Ahmed Eid";
            m1.Salary = clsref;

            //DeepCopy m2 = m1.MakeDeepCopy(m1);  // Non Generic Method Call
            DeepCopy m2 = DeepCopy.MakeDeepCopy<DeepCopy>(m1);

            m2.Salary.Salary = 4000;  // Same Happens if you do it as - clsref.Salary = 2000;


            ShowHideError("Salary: " + m1.Salary.Salary.ToString(), true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

}
