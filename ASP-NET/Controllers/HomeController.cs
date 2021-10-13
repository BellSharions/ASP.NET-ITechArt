using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ASP_NET.Controllers
{
    public class HomeController : Controller
    {
        public string GetInfo()
        {
            Log.Logger.Information("returning 'Hello world!' from GetInfo() method");
            return "Hello World!";
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
