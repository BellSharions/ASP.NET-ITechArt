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
    }
}
