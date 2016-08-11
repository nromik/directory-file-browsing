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
        public HttpResponseMessage Get([FromUri]string path = "", [FromUri]bool isCountFiles = true )
        {
            var re = Request.RequestUri;
            var result = new DirectoryDriver(path).GetFolderInfo(isCountFiles,
                s => s <= 10 * 1024 * 1024,
                n => 10 * 1024 * 1024 < n && n <= 50 * 1024 * 1024,
                n => 100 * 1024 * 1024 <= n);

            return result != null ? Request.CreateResponse(HttpStatusCode.OK, result) : Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
