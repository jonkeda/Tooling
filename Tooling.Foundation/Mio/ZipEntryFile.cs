using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ICSharpCode.SharpZipLib.Zip;

namespace Tooling.Mio
{
    public class ZipEntryFile : VirtualFile
    {
        private readonly ZipPath _zipPath;
        private ZipEntry _zipEntry;

        public ZipEntryFile(ZipPath zipPath, ZipEntry zipEntry, string path)
            : base(path, System.IO.Path.GetFileName(path))
        {
            _zipPath = zipPath;
            _zipEntry = zipEntry;
        }

        private ZipEntry GetEntry()
        {
            if (_zipEntry == null)
            {
                return _zipEntry;
            }

            ZipFile file = _zipPath.OpenZipFile();

            _zipEntry = file.GetEntry(Path);

            return _zipEntry;
        }

        public override VirtualPath Directory
        {
            get { return new ZipPath(_zipPath, System.IO.Path.GetDirectoryName(Path)); }
        }

        public override Stream OpenRead()
        {
            ZipFile file = _zipPath.OpenZipFile();
            return file.GetInputStream(GetEntry());
        }

        public override Stream OpenWrite()
        {
            throw new NotImplementedException();
        }

        public override Stream OpenCreate()
        {
            throw new NotImplementedException();
        }

        public override VirtualFile ChangeExtension(string oldExtension, string newExtension)
        {
            string fullname = Path.Replace(oldExtension, newExtension);

            return _zipPath.GetFile(fullname);
        }

        public override void Delete()
        {
            ZipFile file = _zipPath.OpenZipFile();
            file.Delete(Path);
        }

        public override VirtualFile Copy(VirtualPath fromFolder, VirtualPath toFolder, bool overwrite)
        {
            throw new NotImplementedException();
        }

        public override VirtualFile Copy(VirtualFile tofile, bool overwrite)
        {
            throw new NotImplementedException();
        }

        public override VirtualFile GetNetworkPath()
        {
            return this;
        }

        public override bool Exists()
        {
            return GetEntry() != null;
        }

        public override string ReadAllText()
        {
            ZipFile file = _zipPath.OpenZipFile();
            using (Stream s = file.GetInputStream(GetEntry()))
            using (StreamReader sr = new StreamReader(s))
            {
                return sr.ReadToEnd();
            }
        }

        public override BitmapImage ReadImage()
        {
            return null;
        }

        public override void WriteAllText(string contents)
        {
            throw new NotImplementedException();
        }

        public override void StartProcess()
        {
            throw new NotImplementedException();
        }

        public override ImageSource ToImageSource()
        {
            ZipFile file = _zipPath.OpenZipFile();
            return BitmapFrame.Create(
                file.GetInputStream(GetEntry()),
                BitmapCreateOptions.None,
                BitmapCacheOption.Default
            );
        }

        public override VirtualFileKind Kind { get { return VirtualFileKind.Zip; } }

    }
}