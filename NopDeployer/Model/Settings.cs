using System;
using System.IO;
using System.Xml.Serialization;

namespace NopDeployer.Model
{
    public interface ISettings
    {
        string DeployFolder { get; set; }
        string DeployZipFile { get; set; }
        string AppDataFolder { get; set; }
        string FtpServer { get; set; }
        string FtpUser { get; set; }
        string FtpPassword { get; set; }
    }

    public static class SettingsExtension
    {
        public static void Set(this ISettings to, ISettings from)
        {
            to.DeployFolder = from.DeployFolder;
            to.DeployZipFile = from.DeployZipFile;
            to.AppDataFolder = from.AppDataFolder;
            to.FtpServer = from.FtpServer;
            to.FtpUser = from.FtpUser;
            to.FtpPassword = from.FtpPassword;
        }
    }

    public class Settings : ISettings
    {
        public string DeployFolder
        {
            get;
            set;
        }

        public string DeployZipFile
        {
            get;
            set;
        }

        public string AppDataFolder
        {
            get;
            set;
        }

        public string FtpServer
        {
            get;
            set;
        }

        public string FtpUser
        {
            get;
            set;
        }

        public string FtpPassword
        {
            get;
            set;
        }

        public static string GetPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NopDeployer",
                "Settings.xml");
        }

        public static Settings Load()
        {
            string path = GetPath();
            if (File.Exists(path))
            {
                using (Stream stream = File.OpenRead(path))
                {
                    var xs = new XmlSerializer(typeof(Settings));
                    return xs.Deserialize(stream) as Settings;
                }
            }

            return new Settings
            {
                DeployFolder = @"C:\Azure\deploy",
                DeployZipFile = @"C:\Azure\deploy.zip",
                AppDataFolder = @"C:\Azure\deployApp_Data",
                FtpServer = @"ftp.kozijnenkopen.com",
                FtpUser = "ftpjonkeda"
            };
        }

        public void Save()
        {
            string path = GetPath();

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (StreamWriter stream = File.CreateText(GetPath()))
            {
                var xs = new XmlSerializer(typeof(Settings));
                xs.Serialize(stream, this);
            }
        }

    }
}
