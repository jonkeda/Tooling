    using System;

    namespace Foundations.Security
{
    public class Impersonate : IDisposable
    {
        private ImpersonationContext _context;

        public Impersonate(IImpersonationInfo impersonationInfo)
            :this (impersonationInfo.LogonType, impersonationInfo.UserName, impersonationInfo.Domain, impersonationInfo.Password)
        {
        }

        public Impersonate(LogonType logonType, string username, string domain, string password)
        {
            if (logonType == LogonType.None)
            {
                return;
            }
            _context = new ImpersonationContext();
            uint error = _context.ImpersonateValidUser(logonType, username, domain, password);
            if (error != 0)
            {
                throw new Exception($@"Cannot impersonate {domain}\{username} as {logonType} Error: {error}");
            }
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.UndoImpersonation();
                _context = null;
            }
        }
    }
}