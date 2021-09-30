using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ASP_NET.Controllers
{
    public class HomeController : Controller
    {
        public string GetInfo()
        {
            return "Hello World!";
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
