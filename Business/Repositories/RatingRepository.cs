using Business.Interfaces;
using DAL;
using DAL.Entities;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories
{
    public class RatingRepository : GenericRepository<ApplicationDbContext, ProductRating>, IRatingRepository
    {
        public RatingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task RecalculateRating(int id)
        {
            var ratings = _dbContext.ProductRatings.Where(u => u.ProductId == id).Average(u=>u.Rating);
            _dbContext.Products.Where(u => u.Id == id).First().TotalRating = (int)ratings;
            await _dbContext.SaveChangesAsync();
        }
    }
}
