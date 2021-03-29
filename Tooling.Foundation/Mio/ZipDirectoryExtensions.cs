using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;
using Tooling.Extensions;


namespace Tooling.Mio
{
    public static class ZipDirectoryExtensions
    {
        private static readonly char[] _pathSeparators =
            new[] { Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar };

        public static IEnumerable<ZipDirectoryInfo> EnumerateDirectories(
            this ZipFile archive, string path, string searchPattern)
        {
            return archive.EnumerateDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public static IEnumerable<ZipDirectoryInfo> EnumerateDirectories(
            this ZipFile archive, string path, string searchPattern, SearchOption searchOption)
        {
            return archive.EnumerateEntryInfos(path, searchPattern, searchOption, EntryInfoTypes.Directory)
                .Cast<ZipDirectoryInfo>();
        }

        public static IEnumerable<ZipFileInfo> EnumerateFiles(
            this ZipFile archive, string path, string searchPattern)
        {
            return archive.EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public static IEnumerable<ZipFileInfo> EnumerateFiles(
            this ZipFile archive, string path, string searchPattern, SearchOption searchOption)
        {
            return archive.EnumerateEntryInfos(path, searchPattern, searchOption, EntryInfoTypes.File)
                .Cast<ZipFileInfo>();
        }

        public static IEnumerable<ZipEntryInfo> EnumerateEntryInfos(
            this ZipFile archive, string path, string searchPattern, EntryInfoTypes entryInfoTypes)
        {
            return archive.EnumerateEntryInfos(
                path, searchPattern, SearchOption.TopDirectoryOnly, entryInfoTypes);
        }

        public static IEnumerable<ZipEntryInfo> EnumerateEntryInfos(this ZipFile archive,
            string path, string searchPattern, SearchOption searchOption, EntryInfoTypes entryInfoTypes)
        {
            Regex searchPatternRegex = searchPattern.WildcardToRegex(true);

            foreach (ZipEntryInfo info in archive.EnumerateEntryInfos(path, searchPatternRegex, searchOption, entryInfoTypes))
            {
                yield return info;
            }
        }

        public static IEnumerable<ZipEntryInfo> EnumerateEntryInfos(this ZipFile archive,
            string path, Regex searchPatternRegex, SearchOption searchOption, EntryInfoTypes entryInfoTypes)
        {
            if (path == null)

            {
                path = "";
            }

            if (path.Length > 0)
            {
                path = path.Trim(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                path = path + Path.AltDirectorySeparatorChar;
            }

            HashSet<string> found = new HashSet<string>();

            foreach (ZipEntry entry in archive)
            {
                if (path.Length > 0 && !entry.Name.StartsWith(path))
                {
                    continue;
                }

                int nextSeparator = entry.Name.IndexOfAny(_pathSeparators, path.Length);

                if (nextSeparator >= 0)
                {
                    string directoryName = entry.Name.Substring(0, nextSeparator + 1);

                    if (found.Add(directoryName))
                    {
                        if (entryInfoTypes.HasFlag(EntryInfoTypes.Directory))
                        {
                            yield return new ZipDirectoryInfo(entry, directoryName);
                        }

                        if (searchOption == SearchOption.AllDirectories)
                        {
                            foreach (ZipEntryInfo info in
                                archive.EnumerateEntryInfos(
                                    directoryName, searchPatternRegex, searchOption, entryInfoTypes))
                            {
                                yield return info;
                            }
                        }
                    }
                }
                else
                {
                    if (entryInfoTypes.HasFlag(EntryInfoTypes.File))
                    {
                        if (PatternOk(entry.Name, searchPatternRegex))
                        yield return new ZipFileInfo(entry, entry.Name);
                    }
                }
            }
        }

        private static bool PatternOk(string fullname, Regex searchPatternRegex)
        {
            return searchPatternRegex == null
                    || searchPatternRegex.IsMatch(fullname);
        }
    }
}
