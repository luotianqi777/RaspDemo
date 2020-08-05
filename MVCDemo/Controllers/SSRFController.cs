using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class SSRFController : Controller
    {
        [HttpGet]
        public IActionResult Index(string url)
        {
            ViewData["Result"] = GetUrlData(url);
            return View();
        }

        private string GetUrlData(string url)
        {
            // 转发获取回复
            return url;
        }
    }
}
