using System;
using System.IO;
using System.Linq;
using System.Text;
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
        public IActionResult DownloadData(string url)
        {
            try
            {
                var net = new System.Net.WebClient();
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

        [HttpGet]
        public IActionResult OpenRead(string url)
        {
            try
            {
                var data =System.IO.File.OpenRead(url);
                var contentType = "APPLICATION/octet-stream";
                return File(data, contentType, url);
            }
            catch(Exception e)
            {
                Debuger.WriteLine($"文件下载失败：{url}，原因：{e.Message}");
                return View();
            }
        }

        [HttpGet]
        public IActionResult ReadAllBytes(string url)
        {
            try
            {
                var data = System.IO.File.ReadAllBytes(url);
                var contentType = "APPLICATION/octet-stream";
                return File(data, contentType, url);
            }
            catch(Exception e)
            {
                Debuger.WriteLine($"文件下载失败：{url}，原因：{e.Message}");
                return View();
            }
        }

        [HttpGet]
        public IActionResult ReadAllText(string url)
        {
            try
            {
                var data = Encoding.UTF8.GetBytes(System.IO.File.ReadAllText(url));
                var contentType = "APPLICATION/octet-stream";
                return File(data, contentType, url);
            }
            catch(Exception e)
            {
                Debuger.WriteLine($"文件下载失败：{url}，原因：{e.Message}");
                return View();
            }
        }

        [HttpGet]
        public IActionResult Delete(string url)
        {
            try
            {
                System.IO.File.Delete(url);
            }
            catch(Exception e)
            {
                Debuger.WriteLine($"文件删除失败：{url}，原因：{e.Message}");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Upload()
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            foreach(var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string fileName = formFile.FileName;
                    // 读取数据
                    var data = new byte[formFile.Length];
                    formFile.OpenReadStream().ReadAsync(data);
                    // 写入文件
                    System.IO.File.Create(fileName).Close();
                    var fileStream = new StreamWriter(fileName);
                    fileStream.Write(Encoding.UTF8.GetString(data));
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
            return Ok(new { count = files.Count, size });
        }

        [HttpGet]
        public IActionResult Upload(string url)
        {
            _ = url;
            return View();
        }

    }
}
