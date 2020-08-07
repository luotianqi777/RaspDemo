using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class XSSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
