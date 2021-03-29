using System;
using System.Collections.Generic;
using System.IO;

namespace Tooling.Mio
{
    public abstract class VirtualPath
    {
        public static VirtualPath CreateFromPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return NullPath.Default;
            }
            if (path.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                return new ZipPath(path);
            }
            if (path.EndsWith(".tar", StringComparison.OrdinalIgnoreCase))
            {
                return new TarPath(path);
            }
            if (Directory.Exists(path))
            {
                return new DiskPath(path);
            }
            return NullPath.Default;
        }

        public static VirtualPath CreateFromNetworkPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return NullPath.Default;
            }
            if (path.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                return new ZipPath(path);
            }
            if (path.EndsWith(".tar", StringComparison.OrdinalIgnoreCase))
            {
                return new TarPath(path);
            }
            string networkPath = W32FileHelpers.GetNetworkPath(path);
            if (Directory.Exists(networkPath))
            {
                return new DiskPath(networkPath);
            }

            return new DiskPath(path);
        }

        public abstract VirtualPath ParentPath { get; }

        public abstract string Path { get; }
        public abstract string Name { get; }
        public abstract VirtualFileKind Kind { get; }

        public abstract bool Exists(string path);
        public abstract bool Exists();

        public IEnumerable<VirtualPath> EnumerateDirectories()
        {
            return EnumerateDirectories(null, "*");
        }

        public IEnumerable<VirtualPath> EnumerateDirectories(string path)
        {
            return EnumerateDirectories(path, "*");
        }
        public IEnumerable<VirtualPath> EnumerateDirectories(string path, string searchPattern)
        {
            return EnumerateDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
        }
        public abstract IEnumerable<VirtualPath> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);

        public IEnumerable<VirtualFile> EnumerateFiles()
        {
            return EnumerateFiles(null, "*");
        }
        public IEnumerable<VirtualFile> EnumerateFiles(string searchPattern)
        {
            return EnumerateFiles(null, searchPattern);
        }
        public IEnumerable<VirtualFile> EnumerateFiles(string path, string searchPattern)
        {
            return EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
        }
        public IEnumerable<VirtualFile> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return EnumerateFiles(null, searchPattern, searchOption);
        }
        public abstract IEnumerable<VirtualFile> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);

        protected abstract string CombinePath(string path);

        public override string ToString()
        {
            return Path;
        }

        public abstract VirtualFile GetFile(string path);

        public abstract VirtualPath Combine(string path);

        public abstract VirtualFile CombineToFile(string path);

        public abstract VirtualFile CreateFile(string filename);

        public abstract VirtualPath GetNetworkPath();

        public void Delete()
        {
            Delete(false);
        }

        public abstract void Delete(bool recursive);

    }

    
    public abstract class VirtualPath<F> : VirtualPath
        where F : VirtualFile
    {
    }
}