using System.Collections.Generic;
using System.IO;

namespace Tooling.Mio
{
    public class NullPath : VirtualPath<NullFile>
    {
        public static NullPath Default { get; } = new NullPath();

        private NullPath()
        {
        }

        public override VirtualPath ParentPath
        {
            get { return null; }
        }

        public override string Path
        {
            get { return null; }
        }

        public override string Name
        {
            get { return null; }
        }

        public override bool Exists(string path)
        {
            return false;
        }

        public override bool Exists()
        {
            return false;
        }

        public override IEnumerable<VirtualPath> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            yield break;
        }

        public override IEnumerable<VirtualFile> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            yield break;
        }

        protected override string CombinePath(string path)
        {
            return null;
        }

        public override VirtualFile GetFile(string replace)
        {
            return NullFile.Default;
        }

        public override VirtualPath Combine(string path)
        {
            return this;
        }

        public override VirtualFile CombineToFile(string path)
        {
            return NullFile.Default;
        }

        public override VirtualFile CreateFile(string filename)
        {
            return NullFile.Default;
        }

        public override VirtualPath GetNetworkPath()
        {
            return Default;
        }

        public override void Delete(bool recursive)
        {
            
        }

        public override VirtualFileKind Kind { get { return VirtualFileKind.Null; } }

    }
}