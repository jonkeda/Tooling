using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace Tooling.Helpers
{
    public static class AssemblyFileVersionHelper
    {
        public static string GetShortVersion(Assembly assembly)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            Version v = new Version(fvi.FileVersion);
            return $"{v.Major}.{v.Minor}";
        }

        public static string GetShortVersion()
        {
            return GetShortVersion(Assembly.GetEntryAssembly());
        }
    }

}
