using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FolderExplorer;

namespace WebApp.Controllers
{
    public class DirectoryInfoController : ApiController
    {
        public HttpResponseMessage Get([FromUri]string path = "")
        {
            var result = DirectoryDriver.GetFolderInfo(path);

            return result != null ? Request.CreateResponse(HttpStatusCode.OK, result) : Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
