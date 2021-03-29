using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Tooling.Foundation.Extensions
{
    public static class Win32Error
    {
        private static string _lastErrorString;

        public static string LastErrorString
        {
            get
            {
                return _lastErrorString;
            }
            set
            {
                _lastErrorString = value;
            }
        }

        static Win32Error()
        {
            _lastErrorString = string.Empty;
        }

        public static void SetLastWin32Error()
        {
            int lastWin32Error = Marshal.GetLastWin32Error();
            if (lastWin32Error == 0)
            {
                _lastErrorString = string.Empty;
                return;
            }
            string str = (new Win32Exception(lastWin32Error)).ToString();
            _lastErrorString = $"Win32 Error Code '{lastWin32Error}' : {str}";
        }
    }
}