using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upload(string fileUrl)
        {
            return View();
        }


        [HttpGet]
        public IActionResult Download(string fileUrl)
        {
            var net = new System.Net.WebClient();
            var data = net.DownloadData(fileUrl);
            var content = new MemoryStream(data);
            var contentType = "APPLICATION/octet-stream";
            var fileName = "fileUrl";
            return File(content, contentType, fileName);
        }
    }
}
