using System;
using System.IO;
using Tooling.Foundation.UI;
using Tooling.UI;
using System.IO.Compression;
using System.Net;
using NopDeployer.Model;

namespace NopDeployer.ViewModels
{
    public class MainViewModel : ViewModel, ISettings
    {
        public MainViewModel()
        {
            Settings settings = Settings.Load();

            this.Set(settings);
        }

        private string _deployFolder;
        private string _deployZipFile;
        private string _appDataFolder;
        private string _ftpServer;
        private string _ftpUser;
        private string _ftpPassword;
        private string _log;

        public object SendDeploymentCommand
        {
            get { return new TargetCommand(SendDeployment); }
        }

        public string DeployFolder
        {
            get { return _deployFolder; }
            set { SetProperty(ref _deployFolder, value); }
        }

        public string DeployZipFile
        {
            get { return _deployZipFile; }
            set { SetProperty(ref _deployZipFile, value); }
        }

        public string AppDataFolder
        {
            get { return _appDataFolder; }
            set { SetProperty(ref _appDataFolder, value); }
        }

        public string FtpServer
        {
            get { return _ftpServer; }
            set { SetProperty(ref _ftpServer, value); }
        }

        public string FtpUser
        {
            get { return _ftpUser; }
            set { SetProperty(ref _ftpUser, value); }
        }

        public string FtpPassword
        {
            get { return _ftpPassword; }
            set { SetProperty(ref _ftpPassword, value); }
        }

        public string Log
        {
            get { return _log; }
            set { SetProperty(ref _log, value); }
        }

        private void SendDeployment()
        {
            // Delete previous zip
            DeleteZip();

            // copy app data
            CopyAppData();

            // zip folder
            ZipFolder();

            // ftp zip file
            //FtpZip();

            AddLog("Done");
        }

        private void DeleteZip()
        {
            AddLog("Delete zip");
            File.Delete(DeployZipFile);
        }

        private void CopyAppData()
        {
            AddLog("Copy app data");
            DirectoryCopy(AppDataFolder, DeployFolder, true);            
        }

        private void ZipFolder()
        {
            AddLog("Zip folder");
            ZipFile.CreateFromDirectory(DeployFolder, DeployZipFile);
        }

        private void FtpZip()
        {
            AddLog("FTP Zip");
            FtpFile();
        }

        #region Utils

        private void AddLog(string line)
        {
            Log += $"{line}\n";
        }

        private void FtpFile()
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"ftp://{FtpServer}/ftpjonkeda/kozijnenkopen.com/deployNew.zip");
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = true;
            request.UsePassive = true;
            request.KeepAlive = false;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(FtpUser, FtpPassword);

            // Copy the contents of the file to the request stream.
            byte[] fileContents = File.ReadAllBytes(DeployZipFile); 
            //using (StreamReader sourceStream = new StreamReader(DeployZipFile))
            //{
            //    fileContents = File.ReadAllBytes(DeployZipFile); Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            //}

            request.ContentLength = fileContents.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                AddLog($"Upload File Complete, status {response.StatusDescription}");
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, true);
                }
            }
        }


        #endregion

        public void Close()
        {
            Settings settings = new Settings();
            settings.Set(this);
            settings.Save();
        }
    }
}