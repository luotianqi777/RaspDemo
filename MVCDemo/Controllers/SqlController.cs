using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XMySqlManger;

namespace MVCDemo.Controllers
{
    public class SqlController : Controller
    {
        [HttpGet]
        public IActionResult Index(string id)
        {
            var result = MySqlManger.ExecQuery(id);
            return View();
        }
    }
}
