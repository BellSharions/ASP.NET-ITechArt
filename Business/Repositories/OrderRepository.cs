using Business.Interfaces;
using DAL;
using DAL.Entities;
using DAL.Enums;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Repositories
{
    public class OrderRepository : GenericRepository<ApplicationDbContext, Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> BuyAsync(int id)
        {
            var order = await _dbContext.Orders.AsNoTracking().FirstOrDefaultAsync(u => u.OrderId == id);
            if (order == null && order.Status == OrderStatus.Paid)
                return false;
            order.Status = OrderStatus.Paid;
            _dbContext.Entry(order).Property(u => u.Status).IsModified = true;
            await _dbContext.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteItemsAsync(int id, ICollection<int> products)
        {
            var orders = _dbContext.OrderList.AsNoTracking().Where(x => x.OrderId == id);
            foreach(var order in products)
                _dbContext.OrderList.RemoveRange(orders.Where(u => u.ProductId == order));

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var result = await _dbContext.Orders
                .Include(u => u.OrderList)
                .ThenInclude(u=>u.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.OrderId == id);
            return result;
        }
    }
}
