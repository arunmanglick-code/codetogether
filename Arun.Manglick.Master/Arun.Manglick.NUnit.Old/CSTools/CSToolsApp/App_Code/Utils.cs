#region File History

/******************************File History***************************
 * File Name        : Utils.cs
 * Author           : 
 * Created Date     : 
 * Purpose          : This is the common Utility class for all pages.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(mm-dd-yyyy) Developer Reason of Modification

 * -------------------------------------------------------------------   
 * 10-05-2006       SSK      validateBeforeSave() added to Validate fields if the page is postback to save unsaved changes.
 *  
 * 10-06-2006       NKV      Added RegisterJS() common method for resgistring Javascript. 
 * 
 * 10-11-2006       IAK      Method added for Concatenating String. This method is used while logging information.
 * 02-19-2007       IAK      Method Added: FullTrim()
 * 02-21-2007       IAK      eventDesc array updated
 * 02-21-2007       IAK      eventDesc array updated
 * 05-28-2008       Prerak   IsDecimal function added
 * 06-20-2008       NDM      Stage column issue.
 * 18-11-2008       SD       Changed regular expression for pager number.
 */
#endregion

#region Using block
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Configuration;
using Vocada.Veriphy;
#endregion

namespace Vocada.CSTools
{
    /// <summary>
    /// his class defines generic methods which are used across the application.
    /// </summary>
    public class Utils
    {
        #region Member Varialbes

        const string Map64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        //Ecryprion and Decryption Parameters
        private const string C_PASS_PHRASE = "x1v1DNetXPCK";
        private const string C_SALT_VALUE = "l7s9t2O56P";
        private const string C_HASH_ALGORITHM = "SHA1";
        private const int C_PASSWORD_ITERATIONS = 2;
        private const string C_INIT_VECTOR = "Y8kj8pl2UGE927Z@";
        private const int C_KEY_SIZE = 128;
        private static StringBuilder message_string;
        private static string[] eventDesc;

        public const string SORT_ORDER = "SortOrder";
        #endregion

        #region Public Methods

        static Utils()
        {
            eventDesc = new string[40];

            eventDesc[0] = "Message Closed";
            eventDesc[1] = "Message Closed";
            eventDesc[2] = "Message Closed";
            eventDesc[3] = "Notifications Embargoed"; //"Message Closed"; the db entry for this is Readback Notifications Embargoed
            eventDesc[4] = "Message Closed";
            eventDesc[5] = "Readback Rejected";
            eventDesc[6] = "Message Forwarded";
            eventDesc[7] = "Notifications Embargoed"; //"Message Declined"; the db entry for this is Decline Notifications Embargoed
            eventDesc[8] = "Message Declined";
            eventDesc[9] = "Message Declined";
            eventDesc[10] = "Readback Created";
            eventDesc[11] = "Readback Created";
            eventDesc[12] = "Reply Created";
            eventDesc[13] = "Reply Created";
            eventDesc[14] = "Compliance";
            eventDesc[15] = "Compliance";
            eventDesc[16] = "Notification Embargoed"; // "Compliance"; the db entry for this is Compliance Notification Embargoed
            eventDesc[17] = "Compliance";
            eventDesc[18] = "Compliance";
            eventDesc[19] = "Fail-Safe";
            eventDesc[20] = "End Escalation";
            eventDesc[21] = "Fail-Safe";
            eventDesc[22] = "Backup";
            eventDesc[23] = "Backup";
            eventDesc[24] = "Backup";
            eventDesc[25] = "Backup";
            eventDesc[26] = "Notifications Embargoed";
            eventDesc[27] = "Message Closed";//"Primary"; the db entry for this is On Received
            eventDesc[28] = "Primary";
            eventDesc[29] = "Primary";
            eventDesc[30] = "Primary";
            eventDesc[31] = "Primary";
            eventDesc[32] = "Primary";
            eventDesc[33] = "Message Created";
            eventDesc[34] = "Direct Comm result verified";
            eventDesc[35] = "Message Closed";
            eventDesc[36] = "Reply Created";
            eventDesc[37] = "Readback Confirmed";
            eventDesc[38] = "Documented";
            eventDesc[39] = "";           
        }

        /// <summary>
        /// Check E-Mail format
        /// </summary>
        /// <param name="strItem"></param>
        /// <returns></returns>
        public static bool RegExMatch(string strItem)
        {
            string strPattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

            System.Text.RegularExpressions.Match objMatch =
                  System.Text.RegularExpressions.Regex.Match(strItem, strPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return objMatch.Success;
        }

        /// <summary>
        /// Check Pager format
        /// </summary>
        /// <param name="strItem"></param>
        /// <returns></returns>
        public static bool RegExNumericMatch(string strItem)
        {
            string strPattern = @"(^([0-9 ]*)([0-9 ])*$)|(^ *$)";

            System.Text.RegularExpressions.Match objMatch =
                  System.Text.RegularExpressions.Regex.Match(strItem, strPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return objMatch.Success;
        }
        


        /// <summary>
        /// Check given value is numeric one.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool isNumericValue(string val)
        {
            try
            {
                long returnVal = long.Parse(val);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }
        /// <summary>
        /// Check given value is decimal one.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool isDecimalValue(string val)
        {
            try
            {
                double returnVal = double.Parse(val);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }
        public static string GetEventDescription(int eventId)
        {
            return eventDesc[eventId];
        }

        public static string flattenPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length < 2)
                return phoneNumber;

            string retNumber = Regex.Replace(phoneNumber, "[-\\(\\) a-zA-Z]", "");
            return retNumber;
        }

        public static string expandPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length < 2)
                return phoneNumber;

            return Regex.Replace(phoneNumber, "(\\d{3})(\\d{3})(\\d{4})", "($1) $2-$3");
        }

        /// <summary>
        /// Returns the ServerTime by timezone id.
        /// </summary>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime GetUsersTimeOnTimeZone(int timeZoneId, DateTime dateToConvert)
        {
            if (dateToConvert != DateTime.MinValue)
            {
                switch (timeZoneId)
                {

                    case 1: //dateToConvert.AddHours(-3);
                        dateToConvert = dateToConvert.AddHours(-3);
                        break;
                    case 2: //dateToConvert.AddHours(-1);
                        dateToConvert = dateToConvert.AddHours(-1);
                        break;
                    case 3: //dateToConvert.AddHours(-2);
                        dateToConvert = dateToConvert.AddHours(-2);
                        break;
                    case 5:
                        dateToConvert = dateToConvert.AddHours(1);
                        break;
                }
            }
            return dateToConvert;
        }

        /*Validation if the page is postback to save unsaved changes.*/
        /// <summary>
        /// To validate if directly doPostback for Save is called from javascript e.g in case of
        /// Onunload event, if user didn't saved the changes.
        /// </summary>
        /// <returns></returns>
        public static string validateBeforeSave(string strFirstName, string strLastName, string strPrimaryPhonePrefix,
                                         string strPrimaryPhoneAreaCode, string strPrimaryPhoneNNNN, string strPhonePrefix, string strPhoneCode,
                                         string strPhoneNumber, string strFaxPrefix, string strFaxAreaCode, string strFaxNNNN, string strEmail)
        {
            string message = string.Empty;

            if (strFirstName.Length == 0)
            {
                message = "You Must Enter A First Name";
            }

            if (strLastName.Length == 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "You Must Enter A Last Name";
            }

            // Validation for Primary phone
            if (strPrimaryPhonePrefix.Length == 0)
            {
                if (message.Length != 0)
                {
                    message += "#";

                }
                message += "You Must Enter A Primary Phone Prefix";
            }
            else if (strPrimaryPhonePrefix.Length != 3)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Primary Phone Prefix";
            }
            if (strPrimaryPhoneAreaCode.Length == 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "You Must Enter A Primary Phone Area Code";
            }
            else if (strPrimaryPhoneAreaCode.Length != 3)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Primary Phone Area Code";
            }
            if (strPrimaryPhoneNNNN.Length == 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "You Must Enter A Primary Phone Extension";
            }
            else if (strPrimaryPhoneNNNN.Length != 4)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Primary Phone Extension";
            }

            //Validation for additional Phone
            if (strPhonePrefix.Length != 3 && strPhonePrefix.Length != 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Additional Phone prefix";
            }
            if (strPhoneCode.Length != 3 && strPhoneCode.Length != 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Additional Phone Area Code";
            }
            if (strPhoneNumber.Length != 4 && strPhoneNumber.Length != 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Additional Phone Extension";
            }

            //Validation for Fax.
            if (strFaxPrefix.Length != 3 && strFaxPrefix.Length != 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Fax prefix";
            }
            if (strFaxAreaCode.Length != 3 && strFaxAreaCode.Length != 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Fax Area Code";
            }
            if (strFaxNNNN.Length != 4 && strFaxAreaCode.Length != 0)
            {
                if (message.Length != 0)
                {
                    message += "#";
                }
                message += "Please enter valid Fax Extension";
            }
            if (strEmail.Length != 0)
            {
                if (!(strEmail.Contains("@") && strEmail.Contains(".")))
                {
                    if (message.Length != 0)
                    {
                        message += "#";
                    }
                    message += "Email format incorrect";
                }
            }
            return message;
        }

        public static void RegisterJS(string keyParam, string scriptParam, System.Web.UI.Page page)
        {
            StringBuilder acScript = new StringBuilder();
            acScript.Append("<script type=\"text/javascript\">");
            acScript.Append(scriptParam);
            acScript.Append("</script>");
            page.RegisterStartupScript(keyParam, acScript.ToString());
        }

        /// <summary>
        /// This method encrypts the string data.
        /// </summary>
        /// <param name="atData">data to encrypt</param>
        /// <returns>encrypted data</returns>
        public string Encrypt(string atData)
        {
            string tRet = string.Empty;
            if ((atData.Trim().Length) > 0)
                tRet = Encode(atData, C_PASS_PHRASE, C_SALT_VALUE, C_HASH_ALGORITHM, C_PASSWORD_ITERATIONS, C_INIT_VECTOR, C_KEY_SIZE);
            else
            {
            }
            return tRet;
        }

        /// <summary>
        /// This method decrypts the encrypted data.
        /// </summary>
        /// <param name="atData">data to decrypt.</param>
        /// <returns>decrypted data in original format.</returns>
        public string Decrypt(string atData)
        {
            string tRet = string.Empty;
            if (atData.Trim().Length > 0)
            {
                atData = atData.Replace("%2f", "/");
                tRet = Decode(atData, C_PASS_PHRASE, C_SALT_VALUE, C_HASH_ALGORITHM, C_PASSWORD_ITERATIONS, C_INIT_VECTOR, C_KEY_SIZE);
            }
            return tRet;
        }

        /// <summary>
        /// This method takes Exception details as string arguments and returns String message
        /// </summary>
        /// <param name="functionName">Name of Page and Function in which error occured in 'Page.Method' Format</param>
        /// <param name="subscriberID">Subscriber ID</param>
        /// <param name="message">Exception Message</param>
        /// <param name="stackTrace">Exception Stack trace</param>
        /// <returns>Concatenated Strings as message to Add to Trace</returns>
        public static string ConcatenateString(string functionName, string subscriberID, string message, string stackTrace)
        {
            message_string = new StringBuilder();
            message_string.Append(functionName);
            message_string.Append(":: Exception occured for User ID - ");
            message_string.Append(subscriberID);
            message_string.Append(" As -->");
            message_string.Append(message);
            message_string.Append(" ");
            message_string.Append(stackTrace);
            return message_string.ToString();
        }

        /// <summary>
        /// This function will return string by replace multiple spaces with single space of given string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string FullTrim(string text)
        {
            text = text.Trim();
            StringBuilder returnText = new StringBuilder(text, text.Length);
            int spaceCount = 0;
            for (int i = 0; i < returnText.Length; i++)
            {
                if (returnText[i] == ' ')
                {
                    if (spaceCount > 0)
                    {
                        returnText = returnText.Remove(i, 1);
                        i--;
                    }
                    else
                    {
                        spaceCount++;
                    }
                }
                else
                {
                    spaceCount = 0;
                }
            }
            return returnText.ToString();
        }

       
        /// <summary>
        /// Create Return URL in case if session variable null
        /// </summary>
        /// <param name="redirectionPageName"></param>
        /// <param name="pageName"></param>
        /// <param name="clientQueryString"></param>
        /// <returns></returns>
        public static string GetReturnURL(string redirectionPageName, string pageName, string clientQueryString)
        {
            StringBuilder sbReturnUrl = null;
            try
            {
                sbReturnUrl = new StringBuilder();
                sbReturnUrl.Append(redirectionPageName);
                sbReturnUrl.Append("?ReturnUrl=");
                sbReturnUrl.Append(pageName);
                if (clientQueryString.Length > 0)
                {
                    sbReturnUrl.Append("?" + clientQueryString);
                }
                return sbReturnUrl.ToString();
            }
            finally
            {
                sbReturnUrl = null;
            }
        }

        /// <summary>
        /// This method inserts the Notification for readback through NotifierService.
        /// </summary>
        /// <param name="readbackID"></param>
        /// <param name="accepted"></param>
        public static int InsertNotificationForReadback(int readbackID, int messageID, bool accepted, int isDeptMsg)
        {
            NotifierServiceProxy notifierSrvProxy = null;
            int retVal = 0;
            try
            {
                String strURL = ConfigurationSettings.AppSettings["VoiceLinkR2.com.vocada.voicelink1.Reference"];
                notifierSrvProxy = new NotifierServiceProxy(strURL);
                if(isDeptMsg == 0)
                    retVal = notifierSrvProxy.MessageReadbackAccepted(readbackID, messageID, accepted);
                else
                    retVal = notifierSrvProxy.DepartmentMessageReadbackAccepted(readbackID, messageID, accepted);
                return retVal;
            }
            catch
            {
                return retVal = 1;
            }
            finally
            {
                if (notifierSrvProxy != null)
                {
                    notifierSrvProxy = null;
                }
            }
        }

        /// <summary>
        /// Get Grid Resize Script
        /// </summary>
        /// <returns></returns>
        public static string getGridResizeScript(string divName, string gridClientID, int adjustmentValue)
        {
            return "if(document.getElementById(" + '"' + divName + '"' + ") != null){document.getElementById(" + '"' + divName + '"' + ").style.height=setHeightOfGrid('" + gridClientID + "','" + adjustmentValue.ToString() + "');}";
        }

        /// <summary>
        /// Get Grid Resize Script
        /// </summary>
        /// <returns></returns>
        public static string getGridResizeScript(string divName, string gridClientID, int adjustmentValue, int gridMaxSize)
        {
            return "if(document.getElementById(" + '"' + divName + '"' + ") != null){document.getElementById(" + '"' + divName + '"' + ").style.height=setHeightOfGrid('" + gridClientID + "','" + adjustmentValue.ToString() + "','" + gridMaxSize.ToString() + "');}";
        }

        #endregion

        #region Private Methods

        #region Encoder

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Encrypts specified plaintext using Rijndael symmetric key algorithm
        /// and returns a base64-encoded result.
        /// </summary>
        /// <param name="atPlainText">Plaintext value to be encrypted.</param>
        /// <param name="atPassPhrase">
        /// Passphrase from which a pseudo-random password will be derived. The
        /// derived password will be used to generate the encryption key.
        /// Passphrase can be any string. In this example we assume that this
        /// passphrase is an ASCII string.
        /// </param>
        /// <param name="atSaltValue">
        /// Salt value used along with passphrase to generate password. Salt can 
        /// be any string. In this example we assume that salt is an ASCII string.
        /// </param>
        /// <param name="atHashAlgorithm">
        /// Hash algorithm used to generate password. Allowed values are: "MD5" and
        /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
        /// </param>
        /// <param name="atPasswordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.
        /// </param>
        /// <param name="atInitVector">
        /// Initialization vector (or IV). This value is required to encrypt the 
        /// first block of plaintext data. For RijndaelManaged class IV must be
        /// exactly 16 ASCII characters long.
        /// </param>
        /// <param name="aiKeySize">
        /// Size of encryption key in bits. Allowed values are: 128, 192, and 256.
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>Encrypted value formatted as a base64-encoded string.</returns>
        /// <remarks>
        /// </remarks>
        /// -----------------------------------------------------------------------------
        private string Encode(string atPlainText, string atPassPhrase, string atSaltValue, string atHashAlgorithm, int atPasswordIterations, string atInitVector, int aiKeySize)
        {
            atPassPhrase = C_PASS_PHRASE;
            atSaltValue = C_SALT_VALUE;
            atHashAlgorithm = C_HASH_ALGORITHM;
            atPasswordIterations = C_PASSWORD_ITERATIONS;
            atInitVector = C_INIT_VECTOR;
            aiKeySize = C_KEY_SIZE;
            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
            // encoding.
            byte[] byInitVectorBytes = Encoding.ASCII.GetBytes(atInitVector);

            byte[] bySaltValueBytes = Encoding.ASCII.GetBytes(atSaltValue);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            byte[] byPlainTextBytes = Encoding.UTF8.GetBytes(atPlainText);

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and 
            // salt value. The password will be created using the specified hash 
            // algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes objPassword = new PasswordDeriveBytes(atPassPhrase, bySaltValueBytes, atHashAlgorithm, atPasswordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] byKeyBytes = objPassword.GetBytes((int)aiKeySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged objSymmetricKey = new RijndaelManaged();
            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            objSymmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform objEncryptor = objSymmetricKey.CreateEncryptor(byKeyBytes, byInitVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream objMemoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objEncryptor, CryptoStreamMode.Write);
            // Start encrypting.
            objCryptoStream.Write(byPlainTextBytes, 0, byPlainTextBytes.Length);

            // Finish encrypting.
            objCryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] byCipherTextBytes = objMemoryStream.ToArray();

            // Close both streams.
            objMemoryStream.Close();
            objCryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            string tCipherText = Convert.ToBase64String(byCipherTextBytes);

            // Return encrypted string.
            return tCipherText;
        }

        #endregion

        #region Decoder

        /// <summary>
        /// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
        /// </summary>
        /// <param name="cipherText">
        /// Base64-formatted ciphertext value.
        /// </param>
        /// <param name="passPhrase">
        /// Passphrase from which a pseudo-random password will be derived. The 
        /// derived password will be used to generate the encryption key. 
        /// Passphrase can be any string. In this example we assume that this 
        /// passphrase is an ASCII string.
        /// </param>
        /// <param name="saltValue">
        /// Salt value used along with passphrase to generate password. Salt can 
        /// be any string. In this example we assume that salt is an ASCII string.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Hash algorithm used to generate password. Allowed values are: "MD5" and
        /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
        /// </param>
        /// <param name="passwordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.
        /// </param>
        /// <param name="initVector">
        /// Initialization vector (or IV). This value is required to encrypt the 
        /// first block of plaintext data. For RijndaelManaged class IV must be 
        /// exactly 16 ASCII characters long.
        /// </param>
        /// <param name="keySize">
        /// Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>
        /// Decrypted string value.
        /// </returns>
        /// <remarks>
        /// Most of the logic in this function is similar to the Encrypt 
        /// logic. In order for decryption to work, all parameters of this function
        /// - except cipherText value - must match the corresponding parameters of 
        /// the Encrypt function which was called to generate the 
        /// ciphertext.
        /// </remarks>
        private string Decode(string atCipherText, string atPassPhrase, string atSaltValue, string atHashAlgorithm, int atPasswordIterations, string atInitVector, int aiKeySize)
        {
            atPassPhrase = C_PASS_PHRASE;
            atSaltValue = C_SALT_VALUE;
            aiKeySize = C_KEY_SIZE;
            atInitVector = C_INIT_VECTOR;
            atPasswordIterations = C_PASSWORD_ITERATIONS;
            atHashAlgorithm = C_HASH_ALGORITHM;
            // Convert strings defining encryption key characteristics into byte
            // arrays. Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] byInitVectorBytes = Encoding.ASCII.GetBytes(atInitVector);

            byte[] bySaltValueBytes = Encoding.ASCII.GetBytes(atSaltValue);

            // Convert our ciphertext into a byte array.
            byte[] byCipherTextBytes = Convert.FromBase64String(atCipherText);

            // First, we must create a password, from which the key will be 
            // derived. This password will be generated from the specified 
            // passphrase and salt value. The password will be created using
            // the specified hash algorithm. Password creation can be done in
            // several iterations.
            PasswordDeriveBytes objPassword = new PasswordDeriveBytes(atPassPhrase, bySaltValueBytes, atHashAlgorithm, atPasswordIterations);
            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] byKeyBytes = objPassword.GetBytes((int)(aiKeySize / 8));

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged objSymmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            objSymmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform objDecryptor = objSymmetricKey.CreateDecryptor(byKeyBytes, byInitVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream objMemoryStream = new MemoryStream(byCipherTextBytes);

            // Define memory stream which will be used to hold encrypted data.
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDecryptor, CryptoStreamMode.Read);

            // Since at this point we don//t know what the size of decrypted data
            // will be, allocate the buffer long enough to hold ciphertext;
            // plaintext is never longer than ciphertext.
            byte[] byPlainTextBytes = new byte[byCipherTextBytes.Length];

            // Start decrypting.
            int iDecryptedByteCount = objCryptoStream.Read(byPlainTextBytes, 0, byPlainTextBytes.Length);

            // Close both streams.
            objMemoryStream.Close();
            objCryptoStream.Close();

            // Convert decrypted data into a string. 
            // Let us assume that the original plaintext string was UTF8-encoded.
            string tplainText = Encoding.UTF8.GetString(byPlainTextBytes, 0, iDecryptedByteCount);

            // Return decrypted string.
            return tplainText;
        }

        #endregion
        #endregion

    }
    /// <summary>
    /// This Enum used to get value for Outstanding message Report Week day 
    /// </summary>
    public enum ReportDay
    {
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Saturday = 64,
        Sunday = 128
    }

    /// <summary>
    /// This Enum used to get Notification Devices ID's
    /// </summary>
    public enum NotificationRecipient : int
    {
        OrderingClinician = 1,
        Group = 2,
        ReportingClinician = 3,
        Unit = 4,
        ClinicalTeam = 5

    }

    /// <summary>
    /// This struct used to load SQL reporting configuration from web config.
    /// </summary>
    public struct SQLReportingSettings
    {
        public string Server;
        public string InternalIP;
        public string UserName;
        public string Password;
        public string Domain;
        public string Protocol;
        public string ReportFolder;
       
    }

    /// <summary>
    /// This class defines Menu Tab constants that need to be accessed accross the application.
    /// </summary>
    public class MenuTab
    {
        #region Member Varialbes
        public const string TOOLS = "Tools";
        #endregion
    }

    /// <summary>
    /// This class defines Menu Inner Tab constants that need to be accessed accross the application.
    /// </summary>
    public class MenuInnerTab
    {
        #region Member Varialbes
        public const string ADDOCDIR = "AddOCDirectory";
        public const string ADDNURSEDIR = "AddNurseDirectory";
        #endregion
    }

    /// <summary>
    /// This class defines CSTools Page constants that need to be accessed accross the application.
    /// </summary>
    public class CSToolsPage
    {
        #region Member Varialbes
        public const string ADDOCDIR = "add_directory.aspx";
        public const string ADDNURSEDIR = "add_nurse_directory.aspx";
        #endregion
    }
}