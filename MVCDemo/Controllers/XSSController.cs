using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class XSSController : Controller
    {
        [HttpGet]
        public IActionResult Index(string xml)
        {
            ViewData["Xml"] = xml;
            return View();
        }
    }
}
