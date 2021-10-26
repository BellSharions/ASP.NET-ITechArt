using Business.DTO;
using DAL.Entities;
using DAL.Entities.Models;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResult> CreateOrderAsync(OrderCreationDto info);
        Task<Order> GetOrderByIdAsync(int id);
        Task<OrderInfoDto> GetOrderInfoByIdAsync(int id);
        Task<ServiceResult> DeleteProduct(int id);
        Task<ServiceResult> ChangeProductInfoAsync(int id, OrderChangeDto info);
        Task<ServiceResult> BuyAsync(int id);
    }
}
