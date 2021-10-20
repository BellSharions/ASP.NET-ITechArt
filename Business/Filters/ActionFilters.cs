using Business.DTO;
using DAL.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (!Enum.IsDefined(typeof(Sorting), product.PriceSort))
            {
                FilterHelper.BadRequestValue(context, product.PriceSort.ToString());
                return;
            }
            if (!Enum.IsDefined(typeof(Sorting), product.RatingSort))
            {
                FilterHelper.BadRequestValue(context, product.RatingSort.ToString());
                return;
            }
            if (!Enum.IsDefined(typeof(AgeRating), product.AgeRating))
            {
                FilterHelper.BadRequestValue(context, product.AgeRating.ToString());
                return;
            }
            if (!Enum.IsDefined(typeof(AvailableGenres), product.Genre))
            {
                FilterHelper.BadRequestValue(context, product.Genre.ToString());
                return;
            }
        }
    }
}
