using System.IO;

namespace Tooling.Extensions
{
    public static class FileEx
    {
        public static string ReadAllText(string filename)
        {
            using (FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        public static void Delete(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            File.Delete(path);
        }

        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            CreateDirectoryFromFileName(destFileName);

            File.Copy(sourceFileName, destFileName, overwrite);
        }

        public static void CreateDirectoryFromFileName(string filename)
        {
            string tofolder = Path.GetDirectoryName(filename);
            if (tofolder != null)
            {
                Directory.CreateDirectory(tofolder);
            }
        }

    }
}
