using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Tar;

namespace Tooling.Mio
{
    public class TarPath : VirtualPath<TarEntryFile>, IDisposable
    {
        private readonly string _directory;
        private readonly string _tarFilename;
        private FileStream _inStream;
        private TarArchive _tarFile;
        private readonly object _lock = new object();
        private readonly TarPath _parentTarPath;


        public TarPath(TarPath parentTarPath, string directory)
        {
            _parentTarPath = parentTarPath;
            _tarFilename = parentTarPath._tarFilename;
            _directory = directory?.Replace('\\', '/');
        }

        public TarPath(string tarFilename)
        {
            _tarFilename = tarFilename;
            _name = System.IO.Path.GetFileNameWithoutExtension(tarFilename);
        }


        internal TarArchive OpenTarFile()
        {
            if (_parentTarPath != null)
            {
                return _parentTarPath.OpenTarFile();
            }

            if (_tarFile == null)
            {
                lock (_lock)
                {
                    if (_tarFile == null)
                    {
                        _inStream = File.OpenRead(_tarFilename);

                        _tarFile = TarArchive.CreateInputTarArchive(_inStream);
                    }
                }
            }
            return _tarFile;
        }


        public override VirtualPath ParentPath
        {
            get { throw new NotImplementedException(); }
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
            return File.Exists(_tarFilename);
        }

        public override IEnumerable<VirtualPath> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            using (FileStream fsIn = new FileStream(_tarFilename, FileMode.Open, FileAccess.Read))
            {
                TarInputStream tarIn = new TarInputStream(fsIn);
                TarEntry tarEntry;
                while ((tarEntry = tarIn.GetNextEntry()) != null)
                {
                    if (tarEntry.IsDirectory)
                    {
                        yield return new TarPath(this, tarEntry.File);
                    }
                }
                tarIn.Close();
            }
        }

        public override IEnumerable<VirtualFile> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            using (FileStream fsIn = new FileStream(_tarFilename, FileMode.Open, FileAccess.Read))
            {
                TarInputStream tarIn = new TarInputStream(fsIn);
                TarEntry tarEntry;
                while ((tarEntry = tarIn.GetNextEntry()) != null)
                {
                    if (tarEntry.IsDirectory)
                        continue;
                    yield return new TarEntryFile(this, tarEntry, tarEntry.File, tarIn.Position);
                }
                tarIn.Close();
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
            return new TarEntryFile(this, null, path, -1);
        }

        public override VirtualPath Combine(string path)
        {
            return new TarPath(this, System.IO.Path.GetDirectoryName(Path)); 
        }

        public override VirtualFile CombineToFile(string filename)
        {
            TarArchive file = OpenTarFile();

            string fullFilename = CombinePath(filename);

            // todo
            //var entry = file.GetEntry(fullFilename);

            //return new TarEntryFile(this, entry, fullFilename);

            return null;
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
            if (_tarFile != null)
            {
                lock(_lock)
                {
                    if (_tarFile != null)
                    {
                        _tarFile.Close();
                        _tarFile = null;
                    }
                }
            }

            _inStream?.Close();
        }

        public override VirtualFileKind Kind { get { return VirtualFileKind.Tar; } }

    }
}