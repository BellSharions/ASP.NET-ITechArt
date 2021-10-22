using Business.Interfaces;
using DAL;
using DAL.Entities;
using DAL.Repository;
using System.Linq;
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
            var ratings = _dbContext.ProductRating.Where(u => u.ProductId == id).Average(u=>u.Rating);
            var test = _dbContext.Products.Where(u => u.Id == id).FirstOrDefault();
            test.TotalRating = (int)ratings;
            await _dbContext.SaveChangesAsync();
        }
    }
}
