using DAL.Entities;
using DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<bool> BuyAsync(int id);
        Task<bool> DeleteItemsAsync(int id, ICollection<int> products);

    }
}
