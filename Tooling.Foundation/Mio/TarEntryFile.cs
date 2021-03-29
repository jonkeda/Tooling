using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ICSharpCode.SharpZipLib.Tar;

namespace Tooling.Mio
{
    public class TarEntryFile : VirtualFile
    {
        private readonly TarPath _tarPath;
        private TarEntry _tarEntry;
        private long _streamPosition;

        public TarEntryFile(TarPath tarPath, TarEntry tarEntry, string path, long streamPosition)
            : base(path, System.IO.Path.GetFileName(path))
        {
            _tarPath = tarPath;
            _tarEntry = tarEntry;
            _streamPosition = streamPosition;
        }

        private TarEntry GetEntry()
        {
            if (_tarEntry == null)
            {
                return _tarEntry;
            }

            TarArchive file = _tarPath.OpenTarFile();

            //_tarEntry = file.GetEntry(Path);

            return _tarEntry;
        }

        public override VirtualPath Directory
        {
            get { return new TarPath(_tarPath, System.IO.Path.GetDirectoryName(Path)); }
        }

        public override Stream OpenRead()
        {
            return null;
        }

        public override Stream OpenWrite()
        {
            return null;
        }

        public override Stream OpenCreate()
        {
            return null;
        }

        public override VirtualFile ChangeExtension(string oldExtension, string newExtension)
        {
            string fullname = Path.Replace(oldExtension, newExtension);

            return _tarPath.GetFile(fullname);
        }

        public override void Delete()
        {
            //TarFile file = _tarPath.OpenTarFile();
            //file.Delete(Path);
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
            return null;
//            TarFile file = _tarPath.OpenTarFile();
//            using (Stream s = file.GetInputStream(GetEntry()))
//            using (StreamReader sr = new StreamReader(s))
//            {
//                return sr.ReadToEnd();
//            }
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
            return null;
//            TarFile fi.le = _tarPath.OpenTarFile();
//            return BitmapFrame.Create(
//                file.GetInputStream(GetEntry()),
//                BitmapCreateOptions.None,
//                BitmapCacheOption.Default
           // );
        }

        public override VirtualFileKind Kind { get { return VirtualFileKind.Tar; } }

    }
}