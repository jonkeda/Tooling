using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tooling.Mio
{
    public class NullFile : VirtualFile
    {
        public static NullFile Default { get; } = new NullFile();

        private NullFile()
        { }

        public override VirtualPath Directory
        {
            get
            {
                return NullPath.Default;
            }
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

        public override string ToString()
        {
            return "(none)";
        }

        public override VirtualFile ChangeExtension(string oldExtension, string newExtension)
        {
            return this;
        }

        public override void Delete()
        {

        }

        public override VirtualFile Copy(VirtualPath fromFolder, VirtualPath toFolder, bool overwrite)
        {
            return this;
        }

        public override VirtualFile Copy(VirtualFile tofile, bool overwrite)
        {
            return this;
        }

        public override VirtualFile GetNetworkPath()
        {
            return this;
        }

        public override bool Exists()
        {
            return false;
        }

        public override string ReadAllText()
        {
            return null;
        }

        public override BitmapImage ReadImage()
        {
            return null;
        }

        public override void WriteAllText(string contents)
        {

        }

        public override void StartProcess()
        {

        }

        public override ImageSource ToImageSource()
        {
            return null;
        }

        public override VirtualFileKind Kind { get { return VirtualFileKind.Null; } }

    }
}