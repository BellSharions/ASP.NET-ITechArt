using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ASP_NET.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "Admin")]
        protected string GetInfo()
        {
            Log.Logger.Information("returning 'Hello world!' from GetInfo() method");
            return "Hello World!";
        }
        protected IActionResult Index()
        {
            return Ok();
            //return View();
        }
    }
}
