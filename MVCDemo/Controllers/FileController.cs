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
        public IActionResult Upload(string url)
        {
            return View();
        }


        [HttpGet]
        public IActionResult Download(string url)
        {
            var net = new System.Net.WebClient();
            try
            {
                var data = net.DownloadData(url);
                var content = new MemoryStream(data);
                var contentType = "APPLICATION/octet-stream";
                return File(content, contentType, url);
            }
            catch(Exception e)
            {
                Debuger.WriteLine($"文件下载失败：{url}，原因：{e.Message}");
                return View();
            }
        }
    }
}
