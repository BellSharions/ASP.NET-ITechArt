using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ASP_NET.Controllers
{
    public class HomeController : Controller
    {
        //[Role="Admin"]
        //[Authorize]
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
