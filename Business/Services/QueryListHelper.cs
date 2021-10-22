using Business.DTO;
using DAL.Entities;
using DAL.Enums;
using System;
using System.Linq;

namespace Business.Services
{
    public static class QueryListHelper
    {
        public static void QueryListAsync(ListProductPageDto info, ref IQueryable<Product> query)
        {
            query = info.PriceSort switch
            {
                Sorting.Asc => query.OrderBy(u => u.Price),
                Sorting.Desc => query.OrderByDescending(u => u.Price),
                Sorting.Ignore => query,
                _ => throw new Exception()
            };
            query = info.RatingSort switch
            {
                Sorting.Asc => query.OrderBy(u => u.TotalRating),
                Sorting.Desc => query.OrderByDescending(u => u.TotalRating),
                Sorting.Ignore => query,
                _ => throw new Exception()
            };
        }
    }
}
