using DAL.Entities;
using DAL.Interfaces;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRatingRepository : IRepository<ProductRating>
    {
        Task RecalculateRating(int id);
    }
}
