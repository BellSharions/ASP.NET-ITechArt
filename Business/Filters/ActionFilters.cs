using Business.DTO;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Business.Filters
{
    public class ActionFilters : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context){}

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var tryGetProduct = context.ActionArguments.TryGetValue("info", out var dtoParams);

            if (!tryGetProduct)
            {
                FilterHelper.BadRequestValue(context, nameof(ListProductPageDto));
                return;
            }

            var product = (ListProductPageDto)dtoParams;

            if (product.PageSize <= 0)
            {
                FilterHelper.BadRequestValue(context, product.PageSize.ToString());
                return;
            }
            if (product.PageNumber <= 0)
            {
                FilterHelper.BadRequestValue(context, product.PageNumber.ToString());
                return;
            }
        }
    }
}
