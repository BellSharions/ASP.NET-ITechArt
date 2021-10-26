using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using DAL.Entities;
using DAL.Entities.Models;
using DAL.Enums;
using System.Threading.Tasks;

namespace Business.Services
{
    public class OrderService :IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public Task<ServiceResult> BuyAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceResult> ChangeProductInfoAsync(int id, OrderChangeDto info)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceResult> CreateOrderAsync(OrderCreationDto info)
        {
            var order = new Order();
            _mapper.Map(info, order);
            _orderRepository.CreateAsync(order);
        }

        public async Task<ServiceResult> DeleteProduct(int id)
        {
            if(await _orderRepository.DeleteAsync(u => u.OrderId == id))
                return new ServiceResult(ResultType.Success, "Success");
            return new ServiceResult(ResultType.BadRequest, "Invalid Id");
        }

        public async Task<OrderInfoDto> GetOrderInfoByIdAsync(int id)
        {
            var data = await _orderRepository.GetOrderByIdAsync(id);
            var result = new OrderInfoDto();
            _mapper.Map(data, result);
            return result;
        }

        async Task<Order> IOrderService.GetOrderByIdAsync(int id) => await _orderRepository.GetOrderByIdAsync(id);
    }
}
