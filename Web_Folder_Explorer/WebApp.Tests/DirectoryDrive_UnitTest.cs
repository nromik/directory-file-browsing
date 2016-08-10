﻿using System;
using System.IO;
using System.Linq;
using FolderExplorer;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace WebApp.Tests
{
    [TestClass]
    public class DirectoryDrive_UnitTest
    {
        [TestMethod]
        public void IsNullOrEmptyPath()
        {
            var folderInfo1 = DirectoryDriver.GetFolderInfo(null);
            var folderInfo2 = DirectoryDriver.GetFolderInfo("");

            Assert.AreEqual(folderInfo1.CurrentDirectory, Directory.GetCurrentDirectory());
            Assert.AreEqual(folderInfo2.CurrentDirectory, Directory.GetCurrentDirectory());
        }

        [TestMethod]
        public void GetDirectoryAndFile()
        {
            var folderInfo = DirectoryDriver.GetFolderInfo("");

            var path = Directory.GetCurrentDirectory();

            Assert.AreEqual(folderInfo.Directoreis.Length, Directory.GetDirectories(path).Length);
            Assert.AreEqual(folderInfo.Files.Length, Directory.GetFiles(path).Length);
        }

        [TestMethod]
        public void PathRoot()
        {
            var folderInfo = DirectoryDriver.GetFolderInfo("root");

            var driverInfo = DriveInfo.GetDrives().Select(d => d.Name).ToArray();

            Assert.AreEqual(folderInfo.Directoreis[0],
                driverInfo[0]);

            Assert.AreEqual(folderInfo.Directoreis.Length,
                driverInfo.Length);

            //для корня дисков отсутствует движение верх
            Assert.IsNull(folderInfo.ParentDirectory);

        }

        [TestMethod]
        public void BedDirectory()
        {
            var folderInfo = DirectoryDriver.GetFolderInfo(@"test:\\test");

            Assert.AreEqual(folderInfo,null);
        }

        [TestMethod]
        public void UnauthorizedAccess()
        {
            var dirSystem = DriveInfo.GetDrives()[0].Name + "System Volume Information";
            var folderInfo = DirectoryDriver.GetFolderInfo(dirSystem);

            Assert.IsNotNull(folderInfo.ErrorMessage);
        }







    }
}
