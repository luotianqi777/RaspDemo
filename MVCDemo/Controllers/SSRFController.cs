using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class SSRFController : Controller
    {
        [HttpGet]
        public IActionResult Index(string url)
        {

            ViewData["data"] = GetUrlData(url);
            return View();
        }

        private string GetUrlData(string url)
        {
            try
            {
                WebRequest request = WebRequest.CreateHttp(url);
                // request.Method = "GET";
                // request.ContentType = "text/html;charset=UTF-8";
                request.Timeout = 1000*3;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var restream = response.GetResponseStream();
                var stream = new StreamReader(restream);
                // 转发获取回复
                var result = stream.ReadToEnd();
                stream.Close();
                restream.Close();
                return result;
            }
            catch (Exception e)
            {
                Debuger.WriteLine($"内部请求错误：{e.Message}");
                return string.Empty;
            }
        }
    }
}
