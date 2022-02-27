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
using System.Security.Cryptography;
using System.IO;

public partial class DSA : System.Web.UI.Page
{
    #region Private Variables
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

    protected void btnSignData_Click(object sender, EventArgs e)
    {
        try
        {
            DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
            byte[] arrDigitalSignature = dsa.SignData(System.Text.Encoding.Default.GetBytes(txtToEncrypt.Text));
            string signature = System.Text.Encoding.Default.GetString(arrDigitalSignature);

            txtDigitalSignature.Text = signature;
            
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    protected void btnVeriphyData_Click(object sender, EventArgs e)
    {
        try
        {
            DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
            bool res = dsa.VerifyData(System.Text.Encoding.Default.GetBytes(txtToEncrypt.Text), System.Text.Encoding.Default.GetBytes(txtDigitalSignature.Text));

            lblSuccess.Text = res ? "Successful" : "Failure";
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
   
    protected void btnClearFields_Click(object sender, EventArgs e)
    {
        try
        {
            txtDigitalSignature.Text = string.Empty;
            
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    
    #endregion
}
