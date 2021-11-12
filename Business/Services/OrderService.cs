using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using DAL.Entities;
using DAL.Entities.Models;
using DAL.Enums;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ServiceResult> ChangeOrderAmountAsync(int OrderId, OrderAmountChangeDto info)
        {
            if (info == null)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");

            var foundOrder = await _orderRepository.GetOrderByIdAsync(OrderId);
            if (foundOrder == null)
                return new ServiceResult(ResultType.BadRequest, "No order was found");

            if(foundOrder.OrderList.FirstOrDefault(u => u.ProductId == info.ProductId) == null)
                return new ServiceResult(ResultType.BadRequest, "No product was found");

            foundOrder.OrderList.First(u => u.ProductId == info.ProductId).Amount = info.Amount;
            await _orderRepository.UpdateItemAsync(foundOrder);
            return new ServiceResult(ResultType.Success, "Success");
        }

        public async Task<ServiceResult> CreateOrderAsync(OrderCreationDto info)
        {
            if (info.OrderList == null)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");
            var order = new Order();
            _mapper.Map(info, order);

            var result = await _orderRepository.CreateAsync(order);
            if(result)
                return new ServiceResult(ResultType.Success, "Success");
            return new ServiceResult(ResultType.BadRequest, "Creation was not completed");
        }

        public async Task<ServiceResult> DeleteItems(int id, OrderItemsDeletionDto info)
        {
            var list = info.ProductId;
            if(await _orderRepository.DeleteItemsAsync(id, list))
                return new ServiceResult(ResultType.Success, "Success");
            return new ServiceResult(ResultType.BadRequest, "Invalid Id");
        }

        public async Task<OrderInfoDto> GetOrderInfoByIdAsync(int id)
        {
            var data = await _orderRepository.GetOrderByIdAsync(id);
            if (data == null)
                return null;
            var result = _mapper.Map<Order, OrderInfoDto>(data);
            result.ProductInfo = _mapper.Map<List<Product>, List<ProductInfoDto>>(data.OrderList.Select(t => t.Product).ToList());
            return result;
        }

        public async Task<Order> GetOrderByIdAsync(int id) => await _orderRepository.GetOrderByIdAsync(id);

        public async Task<ServiceResult> BuyAsync(int orderId, int userId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null || userId != order.UserId)
                return new ServiceResult(ResultType.BadRequest, "Invalid Id");

            if(await _orderRepository.BuyAsync(orderId))
                return new ServiceResult(ResultType.Success, "Success");
            return new ServiceResult(ResultType.BadRequest, "Buying action was haulted");
        }
    }
}
