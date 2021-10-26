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
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult> ChangeOrderAmountAsync(int OrderId, OrderAmountChangeDto info)
        {
            if (info == null)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");

            var foundOrder = await _orderRepository.GetOrderByIdAsync(OrderId);
            if (foundOrder == null || foundOrder.Status == OrderStatus.Paid)
                return new ServiceResult(ResultType.BadRequest, "No order was found");

            if(foundOrder.OrderList.First(u => u.ProductId == info.ProductId) == null)
                return new ServiceResult(ResultType.BadRequest, "No product was found");

            foundOrder.OrderList.First(u => u.ProductId == info.ProductId).Amount = info.Amount;
            await _orderRepository.UpdateItemAsync(foundOrder);
            return new ServiceResult(ResultType.Success, "Success");
        }

        public async Task<ServiceResult> CreateOrderAsync(OrderCreationDto info)
        {
            if (info == null)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");
            var order = new Order();
            _mapper.Map(info, order);

            await _orderRepository.CreateAsync(order);
            return new ServiceResult(ResultType.Success, "Success");
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
            var result = new OrderInfoDto();
            _mapper.Map(data, result);
            var list = new List<ProductInfoDto>();
            var listp = new List<Product>();
            foreach (OrderList item in data.OrderList)
                listp.Add(await _productRepository.GetProductByIdAsync(item.ProductId));
            _mapper.Map(listp, list);
            result.ProductInfo = list;
            return result;
        }

        public async Task<Order> GetOrderByIdAsync(int id) => await _orderRepository.GetOrderByIdAsync(id);

        public async Task<ServiceResult> BuyAsync(int orderId, int userId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (userId != order.UserId)
                return new ServiceResult(ResultType.BadRequest, "Invalid Id");

            if(await _orderRepository.BuyAsync(orderId))
                return new ServiceResult(ResultType.Success, "Success");
            return new ServiceResult(ResultType.BadRequest, "Invalid Id");
        }
    }
}
