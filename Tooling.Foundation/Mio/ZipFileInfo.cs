using ICSharpCode.SharpZipLib.Zip;

namespace Tooling.Mio
{
    public class ZipFileInfo : ZipEntryInfo
    {
        public ZipFileInfo(ZipEntry entry, string name) : base(entry, name) { }
    }
}