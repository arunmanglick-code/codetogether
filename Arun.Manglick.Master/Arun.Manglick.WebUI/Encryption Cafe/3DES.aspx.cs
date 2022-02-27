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

public partial class TemplatePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Private Variables
    #endregion

    #region Page Events
    #endregion

    #region Private Methods

    private string EncryptToBase64String(string stringToEncrypt, string sEncryptionKey, string sIV)
    {
        try
        {
            byte[] inputByteArray = new byte[stringToEncrypt.Length + 1];
            byte[] keyByteArray = new byte[sEncryptionKey.Length + 1];
            byte[] ivByteArray = new byte[sIV.Length + 1];

            //Get the Byte Array for all three parameters
            inputByteArray = System.Text.Encoding.Default.GetBytes(stringToEncrypt);
            keyByteArray = System.Text.Encoding.Default.GetBytes(sEncryptionKey);
            ivByteArray = System.Text.Encoding.Default.GetBytes(sIV);

            //Do the Encryption
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(keyByteArray, ivByteArray), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //Return the Encrypted String
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string DecryptFromBase64String(string stringToDecrypt, string sEncryptionKey, string sIV)
    {
        try
        {
            byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
            byte[] keyByteArray = new byte[sEncryptionKey.Length + 1];
            byte[] ivByteArray = new byte[sIV.Length + 1];

            //Get the Byte Array for all three parameters
            inputByteArray = System.Text.Encoding.Default.GetBytes(stringToDecrypt);
            keyByteArray = System.Text.Encoding.Default.GetBytes(sEncryptionKey);
            ivByteArray = System.Text.Encoding.Default.GetBytes(sIV);

            //Do the Decryption
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(keyByteArray, ivByteArray), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //Return the Decrypted String
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private void FormatKeyIV(string sEncryptionKey)
    {
        try
        {
            byte[] md5key;
            byte[] hashedkey;
            byte[] Key = new byte[25];
            byte[] IV = new byte[9];

            md5key = MD5Encryption(sEncryptionKey);
            hashedkey = MD5SaltedHashEncryption(sEncryptionKey);

            int i;
            for (i = 0; i <= hashedkey.Length - 1; i++)
            {
                Key[i] = hashedkey[i];
            }

            int startcount = hashedkey.Length; // always 128
            int midcount = md5key.Length / 2; // always 64

            for (i = midcount; i <= md5key.Length - 1; i++)
            {
                Key[(startcount + (i - midcount))] = md5key[i];
                IV[(i - midcount)] = md5key[(i - midcount)];
            }

            md5key = null;
            hashedkey = null;

            txtPrivateKey.Text = System.Text.Encoding.Default.GetString(Key);
            txtPrivateIV.Text = System.Text.Encoding.Default.GetString(IV);
        }
        catch (Exception ex)
        {
            throw (ex);
        }
    }
    private byte[] MD5Encryption(string ToEncrypt)
    {
        try
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hashedbytes;

            hashedbytes = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(ToEncrypt));

            md5.Clear();
            md5 = null;

            return hashedbytes;
        }
        catch (Exception ex)
        {
            return System.Text.Encoding.Default.GetBytes(ex.Message);
        }
    }
    private byte[] MD5SaltedHashEncryption(string ToEncrypt)
    {
        try
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hashedbytes;
            byte[] saltedhash;

            hashedbytes = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(ToEncrypt));

            ToEncrypt += System.Text.Encoding.Default.GetString(hashedbytes);
            saltedhash = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(ToEncrypt));

            md5.Clear();
            md5 = null;

            return saltedhash;
        }
        catch (Exception ex)
        {
            return System.Text.Encoding.Default.GetBytes(ex.Message);
        }
    }

    #endregion

    #region Public Methods
    #endregion

    #region Control Events

    protected void btnPrivateKey_Click(object sender, EventArgs e)
    {
        try
        {
            FormatKeyIV(txtKey.Text);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    protected void btnEncrypt_Click(object sender, EventArgs e)
    {
        try
        {
            txtToDecrypt.Text = EncryptToBase64String(txtToEncrypt.Text, txtPrivateKey.Text, txtPrivateIV.Text);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    protected void btnDecrypt_Click(object sender, EventArgs e)
    {
        try
        {
            txtFinal.Text = DecryptFromBase64String(txtToDecrypt.Text, txtPrivateKey.Text, txtPrivateIV.Text);
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
            txtPrivateKey.Text = string.Empty;
            txtPrivateIV.Text = string.Empty;
            txtToDecrypt.Text = string.Empty;
            txtFinal.Text = string.Empty;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    
    #endregion
}
