using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace FolderExplorer
{
    public static class DirectoryDriver
    {

        public static FolderInfo GetFolderInfo(string path)
        {

            if (string.IsNullOrEmpty(path))
            {
                path = Directory.GetCurrentDirectory();
            }

            var result = new FolderInfo();

            result.CurrentDirectory = path;

            // отображение дисков компютера
            if (path == "root")
            {

                DriveInfo[] drives = DriveInfo.GetDrives();
                result.Directoreis = drives.Select(d => d.Name).ToArray();

                return result;
            }

            if (Directory.Exists(path))
            {

                result.ParentDirectory = Regex.IsMatch(path, @"^[A-Za-z]+:\\$") 
                    ? "root" : Directory.GetParent(path).ToString();
               
                try
                {
                    result.Directoreis = Directory.GetDirectories(path);
                    result.Files = Directory.GetFiles(path);
                }
                // для случаев отсутствия прав доступа просмотра каталога
                catch (UnauthorizedAccessException exception)
                {
                    result.ErrorMessage = exception.Message;
                }
                return result;
            }
            return null;
        }


    }

    [DataContract]
    public class FolderInfo
    {
        [DataMember(Name = "directoreis")]
        public string[] Directoreis { get; set; }

        [DataMember(Name = "files")]
        public string[] Files { get; set; }

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }

        [DataMember(Name = "parentDirectory")]
        public string ParentDirectory { get; set; }

        [DataMember(Name = "currentDirectory")]
        public string CurrentDirectory { get; set; }
    }

}
