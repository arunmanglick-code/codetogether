// © 2007 Michele Leroux Bustamante. All rights reserved 
// Book: Learning WCF, O'Reilly
// Book Blog: www.thatindigogirl.com
// Michele's Blog: www.dasblonde.net
// IDesign: www.idesign.net

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace SecurityUtility
{

    public class LogonAPI
    {

        public const int SECURITY_IMPERSONATION_LEVEL_SecurityAnonymous = 0;
        public const int SECURITY_IMPERSONATION_LEVEL_SecurityIdentification = 1;
        public const int SECURITY_IMPERSONATION_LEVEL_SecurityImpersonation = 2;
        public const int SECURITY_IMPERSONATION_LEVEL_SecurityDelegation = 3;

        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_LOGON_NETWORK = 3;
        public const int LOGON32_LOGON_BATCH = 4;
        public const int LOGON32_LOGON_SERVICE = 5;
        public const int LOGON32_LOGON_UNLOCK = 7;
        public const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;
        public const int LOGON32_LOGON_NEW_CREDENTIALS = 9;

        public const int LOGON32_PROVIDER_DEFAULT = 0;
        public const int LOGON32_PROVIDER_WINNT35 = 1;
        public const int LOGON32_PROVIDER_WINNT40 = 2;
        public const int LOGON32_PROVIDER_WINNT50 = 3;

        public const int ERROR_LOGON_FAILURE = 1326;

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool
            LogonUser(string lpszUsername,
            string lpszDomain,
            string lpszPassword, int dwLogonType,
            int dwLogonProvider, ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);
    }

    public class LogonUtil
    {
        public static WindowsPrincipal GetLogonUser(
            string domain, string userName, string password)
        {

            IIdentity user = null;
            WindowsPrincipal principal = null;
            IntPtr refToken = IntPtr.Zero;
            bool loggedIn;

            RevertToSelf();

            loggedIn =
                LogonAPI.LogonUser(userName, domain, password,
                LogonAPI.LOGON32_LOGON_NETWORK_CLEARTEXT,
                LogonAPI.LOGON32_PROVIDER_DEFAULT, ref refToken);

            if (loggedIn == true)
            {
                user = new WindowsIdentity(refToken, "NTLM", WindowsAccountType.Normal, true);
                principal = new WindowsPrincipal(user as WindowsIdentity);
            }

            return principal;
        }

        public static WindowsImpersonationContext ImpersonateWindowsIdentity(WindowsIdentity identity)
        {

            WindowsImpersonationContext impContext =
                WindowsIdentity.Impersonate(identity.Token);

            return impContext;
        }

        public static void RevertToSelf()
        {
            if (!LogonAPI.RevertToSelf())
                throw new Exception("RevertToSelf failed.");
        }

    }

}
