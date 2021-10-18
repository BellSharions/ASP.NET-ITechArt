using Business.DTO;
using Business.Interfaces;
using DAL;
using DAL.Entities;
using DAL.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
