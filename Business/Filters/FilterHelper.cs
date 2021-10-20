using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Filters
{
    public static class FilterHelper
    {
        public static void BadRequestValue(ActionExecutingContext context, string message)
        {
            context.Result = new BadRequestObjectResult($"Invalid value of {message}");
        }
    }
}
