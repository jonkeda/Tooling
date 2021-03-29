using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tooling.Mio
{
    public abstract class VirtualFile
    {
        public string Path { get; protected set; }
        public string Name { get; protected set; }
        public DateTime LastWriteTime { get; protected set; }
        public abstract VirtualPath Directory { get; }

        public abstract VirtualFileKind Kind { get; }

        protected VirtualFile()
        { }

        protected VirtualFile(string path, string name)
        {
            Path = path;
            Name = name;
        }

        public abstract Stream OpenRead();

        public abstract Stream OpenCreate();

        public abstract Stream OpenWrite();

        public override string ToString()
        {
            return Path;
        }

        public abstract VirtualFile ChangeExtension(string oldExtension, string newExtension);

        public abstract void Delete();

        public VirtualFile Copy(VirtualPath fromFolder, VirtualPath toFolder)
        {
            return Copy(fromFolder, toFolder, true);
        }
        public abstract VirtualFile Copy(VirtualPath fromFolder, VirtualPath toFolder, bool overwrite);

        public VirtualFile Copy(VirtualFile tofile)
        {
            return Copy(tofile, true);
        }
        public abstract VirtualFile Copy(VirtualFile tofile, bool overwrite);

        public abstract VirtualFile GetNetworkPath();

        public abstract bool Exists();

        public abstract string ReadAllText();

        public abstract BitmapImage ReadImage();

        public abstract void WriteAllText(string contents);

        public abstract void StartProcess();

        public abstract ImageSource ToImageSource();
    }
}