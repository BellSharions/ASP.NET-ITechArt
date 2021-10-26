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

        
    }
}
