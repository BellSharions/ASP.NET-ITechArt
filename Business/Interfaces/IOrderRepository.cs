using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<bool> DeleteOrderAsync(int id);

    }
}
