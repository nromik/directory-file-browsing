using System;
using System.Collections.Generic;
using System.Net;
using FolderExplorer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Controllers;

namespace WebApp.Tests
{
    [TestClass]
    public class DirectoryIntoController_UnitTest
    {
        [TestMethod]
        public void Get()
        {
            var controller = new DirectoryInfoController();
            
            Assert.IsNull(controller.Get("jhfkjf"));
        }
    }
}
