using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
