using Business.Interfaces;
using DAL;
using DAL.Entities;
using DAL.Repository;
using System.Threading.Tasks;

namespace Business.Repositories
{
    public class OrderRepository : GenericRepository<ApplicationDbContext, Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task<bool> DeleteOrderAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
