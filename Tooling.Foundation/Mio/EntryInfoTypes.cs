using System;

namespace Tooling.Mio
{
    [Flags]
    public enum EntryInfoTypes
    {
        Directory = 1,
        File = 2,
        DirectoryOrFile = Directory | File
    }
}