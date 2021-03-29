using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Tooling.Mio
{
    internal static class W32FileHelpers
    {
        [DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int WNetGetConnection(
            [MarshalAs(UnmanagedType.LPTStr)] string localName,
            [MarshalAs(UnmanagedType.LPTStr)] StringBuilder remoteName,
            ref int length);

        public static string GetNetworkPath(string path)
        {
            string disk = Path.GetPathRoot(path)?.TrimEnd('\\', '/');
            if (string.IsNullOrEmpty(disk))
            {
                return path;
            }

            StringBuilder sb = new StringBuilder(512);
            int size = sb.Capacity;
            int error = WNetGetConnection(disk, sb, ref size);
            if (error != 0)
            {
                return path;
            }
            return path.Replace(disk, sb.ToString());
        }
    }
}