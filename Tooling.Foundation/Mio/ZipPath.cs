using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Tooling.Mio
{
    public class ZipPath : VirtualPath<ZipEntryFile>, IDisposable
    {
        private readonly string _directory;
        private readonly string _zipFilename;
        private ZipFile _zipFile;
        private readonly object _lock = new object();
        private readonly ZipPath _parentZipPath;


        public ZipPath(ZipPath parentZipPath, string directory)
        {
            _parentZipPath = parentZipPath;
            _zipFilename = parentZipPath._zipFilename;
            _directory = directory?.Replace('\\', '/');
        }

        public ZipPath(string zipFilename)
        {
            _zipFilename = zipFilename;
            _name = System.IO.Path.GetFileNameWithoutExtension(zipFilename);
        }

        public override VirtualPath ParentPath
        {
            get { throw new NotImplementedException(); }
        }


        internal ZipFile OpenZipFile()
        {
            if (_parentZipPath != null)
            {
                return _parentZipPath.OpenZipFile();
            }

            if (_zipFile == null)
            {
                lock (_lock)
                {
                    if (_zipFile == null)
                    {
                        _zipFile = new ZipFile(_zipFilename);
                    }
                }
            }
            return _zipFile;
        }


        public override string Path
        {
            get { return _directory; }
        }

        private readonly string _name;

        public override string Name
        {
            get { return _name; }
        }
        public override bool Exists(string path)
        {
            throw new NotImplementedException();
        }

        public override bool Exists()
        {
            return File.Exists(_zipFilename);
        }

        public override IEnumerable<VirtualPath> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            ZipFile file = OpenZipFile();
            foreach (ZipDirectoryInfo directory in file.EnumerateDirectories(CombinePath(path), searchPattern, searchOption))
            {
                yield return new ZipPath(this, directory.Name);
            }
        }

        public override IEnumerable<VirtualFile> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            ZipFile file = OpenZipFile();
            foreach (ZipFileInfo info in file.EnumerateFiles(CombinePath(path), searchPattern, searchOption))
            {
                yield return new ZipEntryFile(this, info.Entry, info.Name);
            }
        }

        protected override string CombinePath(string path)
        {
            if (string.IsNullOrEmpty(_directory))
            {
                return path?.Replace('\\', '/');
            }

            if (path == null)
            {
                return _directory;
            }
            return System.IO.Path.Combine(_directory, path)?.Replace('\\', '/');
        }

        public override VirtualFile GetFile(string path)
        {
            return new ZipEntryFile(this, null, path);
        }

        public override VirtualPath Combine(string path)
        {
            return new ZipPath(this, System.IO.Path.GetDirectoryName(Path)); 
        }

        public override VirtualFile CombineToFile(string filename)
        {
            ZipFile file = OpenZipFile();

            string fullFilename = CombinePath(filename);

            var entry = file.GetEntry(fullFilename);

            return new ZipEntryFile(this, entry, fullFilename);
        }

        public override VirtualFile CreateFile(string filename)
        {
            return GetFile(CombinePath(filename));
        }

        public override VirtualPath GetNetworkPath()
        {
            return this;
        }

        public override void Delete(bool recursive)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (_zipFile != null)
            {
                lock(_lock)
                {
                    if (_zipFile != null)
                    {
                        _zipFile.Close();
                        _zipFile = null;
                    }
                }
            }
        }

        public override VirtualFileKind Kind { get { return VirtualFileKind.Zip; } }

    }
}