using System;
using System.Collections.Generic;
using System.IO;

namespace Tooling.Mio
{
    public class DiskPath : VirtualPath<DiskFile>
    {
        private readonly DiskPath _parentPath;
        private string _baseDirectory;
        private string _name;

        public DiskPath(string baseDirectory)
        {
            BaseDirectory = baseDirectory;
        }

        private DiskPath(DiskPath parentPath, string baseDirectory)
        {
            _parentPath = parentPath;
            BaseDirectory = baseDirectory;
        }

        private string BaseDirectory
        {
            get => _baseDirectory;
            set
            {
                _baseDirectory = value;
                if (string.IsNullOrEmpty(_baseDirectory))
                {
                    _name = null;
                }
                else
                {
                    _name = System.IO.Path.GetFileName(_baseDirectory);
                    if (string.IsNullOrEmpty(_name))
                    {
                        _name = _baseDirectory;
                    }
                }
            }
        }

        public override VirtualPath ParentPath
        {
            get
            {
                if (_parentPath != null)
                {
                    return _parentPath;
                }

                if (string.IsNullOrEmpty(_baseDirectory))
                {
                    return null;
                }

                string parentpath = System.IO.Path.GetDirectoryName(_baseDirectory);
                return new DiskPath(parentpath);
            }
        }

        public override string Path
        {
            get { return _baseDirectory; }
        }

        public override string Name
        {
            get { return _name; }
        }

        public override bool Exists(string path)
        {
            return File.Exists(CombinePath(path));
        }

        public override bool Exists()
        {
            return Directory.Exists(BaseDirectory);
        }

        public override IEnumerable<VirtualPath> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            if (Directory.Exists(CombinePath(path)))
            {
                IEnumerable<string> paths = GetDirectories(path, searchPattern, searchOption);
                if (paths == null)
                {
                    yield break;
                }
                foreach (string directory in paths)
                {
                    yield return new DiskPath(this, directory);
                }
            }
        }

        private IEnumerable<string> GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            try
            {
                return Directory.EnumerateDirectories(CombinePath(path), searchPattern,
                    searchOption);
            }
            catch
            {
                return null;
            }
        }

        public override IEnumerable<VirtualFile> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            foreach (FileData file in FastDirectoryEnumerator.EnumerateFiles(CombinePath(path), searchPattern, searchOption))
            {
                yield return new DiskFile(this, file);
            }
        }

        protected override string CombinePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return BaseDirectory;
            }
            return System.IO.Path.Combine(BaseDirectory, path);
        }

        public override VirtualPath Combine(string path)
        {
            return new DiskPath(CombinePath(path));
        }

        public override VirtualFile CombineToFile(string path)
        {
            return GetFile(CombinePath(path));
        }

        public override VirtualFile CreateFile(string filename)
        {
            return new DiskFile(this, filename);
        }

        public override VirtualPath GetNetworkPath()
        {
            string networkPath = W32FileHelpers.GetNetworkPath(Path);

            return new DiskPath(networkPath);
        }

        public override void Delete(bool recursive)
        {
            Directory.Delete(Path, recursive);
        }

        public override VirtualFile GetFile(string filename)
        {
            string fullName = CombinePath(filename);
            return new DiskFile(this, fullName);
        }

        public override VirtualFileKind Kind { get { return VirtualFileKind.Disk; } }

    }
}