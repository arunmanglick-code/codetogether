#region File History

/******************************File History***************************

 * File Name        : UserValidator.cs
 * Author           : 
 * Created Date     : 
 * Purpose          : This class handles the validation of session key
 *                  : this validator is called for each service request and
 *                  :check whether valid session key is supplied or not
 * *********************File Modification History*********************
 * Date(mm-dd-yyyy) Developer Reason of Modification
 * 12-23-2009  Raju G     Created
 * ------------------------------------------------------------------- 
 * 
*/

#endregion

#region Namespaces

using System;
using System.IdentityModel.Selectors;
using System.ServiceModel;
using System.Configuration;

#endregion

namespace Nuance.Veriphy.Security
{
    /// <summary>
    /// This class is used to handle validation of Secure Session key
    /// supplied in UserName token
    /// </summary>
    public class UserValidator : UserNamePasswordValidator
    {

        #region UserNamePasswordValidator Member

        /// <summary>
        /// Validate the Session key supplied - It should not be expired and 
        /// also it should be exist in database
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public override void Validate(string sessionKey, string password)
        {
            if (string.IsNullOrEmpty(sessionKey)) //if session key is blank
            {
                throw new FaultException(ERR_MSG_BLANKSESSIONKEY);
            }
            else
            {
                EncodeDecode objEncodeDecode = new EncodeDecode();
                string decryptedSessionKey = string.Empty;
                try
                {
                    //try to decrypt session key 
                    decryptedSessionKey = objEncodeDecode.Decrypt(sessionKey);
                }
                catch
                {
                    //Not able to decrypt session key - Invalid session key
                    throw new FaultException(ERR_MSG_INVALIDSESSIONKEY);
                }
                //if session key decrypted successfully
                if (!string.IsNullOrEmpty(decryptedSessionKey))
                {
                    //Get the expiry timestamp from last index of decrypted session key - seperated by sessionKeySeperator
                    string expiryTimeStampString = decryptedSessionKey.Substring(decryptedSessionKey.IndexOf(sessionKeySeperator) + 1);
                    DateTime expiryDateTime;
                    if (DateTime.TryParse(expiryTimeStampString, out expiryDateTime))
                    {
                        //if expiry timestamp is less than current time
                        if (DateTime.Now.CompareTo(expiryDateTime) > 0)
                            throw new FaultException(ERR_MSG_SESSIONKEYEXPIRED);
                        else// check if timestamp exists in database
                        {
                            SessionDetailsManager objSessionDetailsManager = new SessionDetailsManager(sessionKey);
                            //if session key does not exist in database
                            if (objSessionDetailsManager.GetSessionDetails() == null)
                                throw new FaultException(ERR_MSG_INVALIDSESSIONKEY);
                        }
                    }
                    else // if expiry timestamp is not in correct format
                        throw new FaultException(ERR_MSG_INVALIDSESSIONKEY);
                }

            }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Get Session Key Seperator from Config
        /// if not exist in Config then get default value
        /// </summary>   
        private string sessionKeySeperator
        {
            get
            {
                string sessionKeySep = ConfigurationManager.AppSettings[CONFIGKEY_SESSIONKEYSEPERATOR];
                sessionKeySep = (string.IsNullOrEmpty(sessionKeySep)) ? DEFAULT_SESSIONKEYSEPERATOR : sessionKeySep;
                return sessionKeySep;
            }
        }
        #endregion

        #region Constants

        /// <summary>
        /// Constants for error messages
        /// </summary>
        private const string ERR_MSG_BLANKSESSIONKEY = "Blank Session Key.";
        private const string ERR_MSG_INVALIDSESSIONKEY = "Invalid Session Key";
        private const string ERR_MSG_SESSIONKEYEXPIRED = "Session Key Expired";

        /// <summary>
        /// Cosntant for Config Keys
        /// </summary>
        private const string CONFIGKEY_SESSIONKEYSEPERATOR = "SessionKeySeperator";

        /// <summary>
        /// Constant for Default Session Key Seperator
        /// </summary>
        private const string DEFAULT_SESSIONKEYSEPERATOR = "$";

        #endregion
    }
}
