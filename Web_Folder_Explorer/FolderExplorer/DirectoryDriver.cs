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
    public class DirectoryDriver
    {
        private FolderInfo folderInfo;
        private string currentPath = null;

        public DirectoryDriver(string path)
        {
            folderInfo = new FolderInfo();
            currentPath = string.IsNullOrEmpty(path) ? Directory.GetCurrentDirectory() : path;
            folderInfo.CurrentDirectory = currentPath;
            folderInfo.ErrorMessage = new List<string>();
            folderInfo.CountFiles = new List<string>();
        }

        public FolderInfo GetFolderInfo(bool isCount = false,
            params Func<long, bool>[] conditionsCount)
        {

            // отображение дисков компютера
            if (currentPath == "root")
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                folderInfo.Directoreis = drives.Select(d => d.Name).ToArray();
            }

            if (Directory.Exists(currentPath))
            {
                folderInfo.ParentDirectory = Regex.IsMatch(currentPath, @"^[A-Za-z]+:\\$")
                    ? "root"
                    : Directory.GetParent(currentPath).ToString();

                try
                {
                    folderInfo.Directoreis = Directory.GetDirectories(currentPath);
                    folderInfo.Files = Directory.GetFiles(currentPath);
                }
                    // для случаев отсутствия прав доступа просмотра каталога
                catch (UnauthorizedAccessException exception)
                {
                    folderInfo.ErrorMessage.Add(exception.Message);
                }
                if (isCount)
                    CountFiles(conditionsCount);
            }
            else
            {
                folderInfo.CurrentDirectory = null;
            }
            return folderInfo;
        }

        private void CountFiles(params Func<long, bool>[] conditionsCount)
        {
            var listSizeFile = GetListSizeFile();

            foreach (var condition in conditionsCount)
            {
                var count = listSizeFile.Count(condition);
                folderInfo.CountFiles.Add(count.ToString());
            }
        }

        private List<long> GetListSizeFile()
        {
           List<long> listSizeFile = new List<long>();

            foreach (var dir in TreeWalker(currentPath, Directory.GetDirectories))
            {
                try
                {
                    string[] files = Directory.GetFiles(dir);
                    listSizeFile.AddRange(files.Select(s => new FileInfo(s).Length));
                }
                catch (Exception exception)
                {
                    folderInfo.ErrorMessage.Add(exception.Message);
                }

            }
            return listSizeFile;
        }

        private IEnumerable<T> TreeWalker<T>(T root, Func<T, IEnumerable<T>> next)
        {
            //TODO: Ексепшен - слишком длиное имя каталога
            var q = next(root).SelectMany(n => TreeWalker(n, next));
            return Enumerable.Repeat(root, 1).Concat(q);
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
        public List<string> ErrorMessage { get; set; }

        [DataMember(Name = "parentDirectory")]
        public string ParentDirectory { get; set; }

        [DataMember(Name = "currentDirectory")]
        public string CurrentDirectory { get; set; }

        [DataMember(Name = "countFiles")]
        public List<string> CountFiles { get; set; }
    }

}
