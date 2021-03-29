using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Foundations.Security
{
    public enum SecurityImpersonationLevel
    {
        SecurityAnonymous,
        SecurityIdentification,
        SecurityImpersonation,
        SecurityDelegation
    }

    // 1008 
    // 1326 bad username or password
    // Error codes
    // https://docs.microsoft.com/en-us/windows/win32/debug/system-error-codes

    // https://forums.asp.net/t/1581190.aspx?Error+An+attempt+was+made+to+reference+a+token+that+does+not+exist+when+impersonating+by+token


    public class ImpersonationContext
    {
        private const int Logon32LogonInteractive = 2;
        private const int Logon32ProviderDefault = 0;

        private WindowsImpersonationContext _impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(string lpszUserName,
            string lpszDomain,
            string lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        public static bool Run(Action action, IImpersonationInfo impersonationInfo)
        {
            return Run(action, impersonationInfo.LogonType, impersonationInfo.UserName, impersonationInfo.Domain,
                impersonationInfo.Password);
        }

        public static bool Run(Action action, LogonType logonType, string username, string domain, string password)
        {
            ImpersonationContext context = new ImpersonationContext();
            try
            {
                uint error = context.ImpersonateValidUser(logonType, username, domain, password);
                if (error == 0)
                {
                    action.Invoke();
                    return true;
                }
            }
            finally
            {
                context.UndoImpersonation();
            }
            return false;
        }

        internal uint ImpersonateValidUser(LogonType logonType, string userName, string domain, string password)
        {
            if (logonType == LogonType.None)
            {
                return 0;
            }
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;
            uint error = 0;
            try
            {
                if (!RevertToSelf())
                {
                    error = GetLastError(1);
                }
                if (error == 0)
                {
                    if (LogonUserA(userName, domain, password, (int) logonType,
                            Logon32ProviderDefault, ref token) == 0)
                    {
                        error = GetLastError(2);
                    }
                }
                if (error == 0)
                {
                    if (DuplicateToken(token,
                            (int)SecurityImpersonationLevel.SecurityImpersonation,
                            ref tokenDuplicate) == 0)
                    {
                        error = GetLastError(3);
                    }
                }
                if (error == 0)
                {
                    _impersonationContext = WindowsIdentity.Impersonate(tokenDuplicate);
                    if (_impersonationContext == null)
                    {
                        error = GetLastError(4);
                    }
                }
            }
            finally
            {
                if (token != IntPtr.Zero)
                {
                    CloseHandle(token);
                }
                if (tokenDuplicate != IntPtr.Zero)
                {
                    CloseHandle(tokenDuplicate);
                }
            }

            return error;
        }

        private static uint GetLastError(uint defaultError)
        {
            uint error = GetLastError();
            if (error == 0)
            {
                return defaultError * 100000;
            }
            return error + defaultError * 100000;
        }

        internal void UndoImpersonation()
        {
            _impersonationContext?.Undo();
            _impersonationContext = null;
        }
    }
}