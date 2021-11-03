using AutoMapper;
using Business;
using Business.DTO;
using Business.Interfaces;
using Business.Services;
using DAL.Entities;
using DAL.Entities.Models;
using DAL.Enums;
using FakeItEasy;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnitTests.Consts;
using Xunit;

namespace UnitTests.ServiceTests
{
    public class OrderServiceTest
    {
        public IOrderRepository orderRepository = A.Fake<IOrderRepository>();
        public IMapper mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
        [Fact]
        public async Task GetOrderByIdAsyncPositive_ReturnOrder()
        {
            var orderService = new OrderService(orderRepository, mapper);

            A.CallTo(() => orderRepository.GetOrderByIdAsync(Orders.TestOrder1.OrderId)).Returns(Orders.TestOrder1);

            //Act
            var result = await orderService.GetOrderByIdAsync(Orders.TestOrder1.OrderId);

            //Assert
            Assert.Equal(Orders.TestOrder1, result);

            A.CallTo(() => orderRepository.GetOrderByIdAsync(Orders.TestOrder1.OrderId)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetOrderInfoByIdAsyncPositive_ReturnOrderInfoDto()
        {
            var orderService = new OrderService(orderRepository, mapper);

            A.CallTo(() => orderRepository.GetOrderByIdAsync(1)).Returns(Orders.TestOrder1);
            Orders.TestOrder1.OrderList.Add(Orders.TestOrderList1);

            //Act
            var result = await orderService.GetOrderInfoByIdAsync(1);

            //Assert
            Assert.Equal(Orders.TestOrderInfo1.Amount, result.Amount);
            Assert.Equal(Orders.TestOrderInfo1.Status, result.Status);
            Assert.Equal(Orders.TestOrderInfo1.ProductInfo.Count, result.ProductInfo.Count);
            for (var i = 0; i < result.ProductInfo.Count; i++)
                Assert.True(Orders.TestOrderInfo1.ProductInfo[i].Equals(result.ProductInfo[i]));

            A.CallTo(() => orderRepository.GetOrderByIdAsync(1)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetOrderInfoByIdAsyncNegative_ReturnNull()
        {
            var orderService = new OrderService(orderRepository, mapper);

            A.CallTo(() => orderRepository.GetOrderByIdAsync(1)).Returns(new Order());
            Orders.TestOrder1.OrderList.Add(Orders.TestOrderList1);

            //Act
            var result = await orderService.GetOrderInfoByIdAsync(1);

            //Assert
            Assert.Null(result);

            A.CallTo(() => orderRepository.GetOrderByIdAsync(1)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangeOrderAmountAsyncPositive_ReturnServiceResultOk()
        {
            var orderService = new OrderService(orderRepository, mapper);
            var serviceResult = new ServiceResult(ResultType.Success, "Success");

            A.CallTo(() => orderRepository.GetOrderByIdAsync(Orders.TestOrder1.OrderId)).Returns(Orders.TestOrder1);
            A.CallTo(() => orderRepository.UpdateItemAsync(Orders.TestOrder1)).Returns(true);
            Orders.TestOrder1.OrderList.Add(Orders.TestOrderList1);

            //Act
            var result = await orderService.ChangeOrderAmountAsync(Orders.TestOrder1.OrderId, Orders.OrderAmountTest2);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => orderRepository.UpdateItemAsync(Orders.TestOrder1)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangeOrderAmountAsyncNegative_NoProducts_ReturnServiceResultBadRequest()
        {
            var orderService = new OrderService(orderRepository, mapper);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "No Product was found");

            A.CallTo(() => orderRepository.GetOrderByIdAsync(Orders.TestOrderNoProducts1.OrderId)).Returns(Orders.TestOrderNoProducts1);
            A.CallTo(() => orderRepository.UpdateItemAsync(Orders.TestOrderNoProducts1)).Returns(true);

            //Act
            var result = await orderService.ChangeOrderAmountAsync(Orders.TestOrderNoProducts1.OrderId, Orders.OrderAmountTest2);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);
            
            A.CallTo(() => orderRepository.UpdateItemAsync(Orders.TestOrderNoProducts1)).MustNotHaveHappened();
        }
        [Fact]
        public async Task ChangeOrderAmountAsyncNegative_NoInfo_ReturnServiceResultBadRequest()
        {
            var orderService = new OrderService(orderRepository, mapper);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid information");

            A.CallTo(() => orderRepository.GetOrderByIdAsync(Orders.TestOrder1.OrderId)).Returns(Orders.TestOrder1);
            A.CallTo(() => orderRepository.UpdateItemAsync(Orders.TestOrder1)).Returns(true);

            //Act
            var result = await orderService.ChangeOrderAmountAsync(Orders.TestOrder1.OrderId, new OrderAmountChangeDto());

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => orderRepository.UpdateItemAsync(Orders.TestOrder1)).MustNotHaveHappened();
        }
        [Fact]
        public async Task ChangeOrderAmountAsyncNegative_NoOrder_ReturnServiceResultBadRequest()
        {
            var orderService = new OrderService(orderRepository, mapper);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid information");

            A.CallTo(() => orderRepository.GetOrderByIdAsync(Orders.TestOrder1.OrderId)).Returns(new Order());
            A.CallTo(() => orderRepository.UpdateItemAsync(Orders.TestOrder1)).Returns(true);

            //Act
            var result = await orderService.ChangeOrderAmountAsync(Orders.TestOrder1.OrderId, Orders.OrderAmountTest2);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => orderRepository.UpdateItemAsync(Orders.TestOrder1)).MustNotHaveHappened();
        }
        [Fact]
        public async Task BuyAsyncPositive_ReturnServiceResultWithOk()
        {
            var orderService = new OrderService(orderRepository, mapper);
            var serviceResult = new ServiceResult(ResultType.Success, "Success");

            A.CallTo(() => orderRepository.GetOrderByIdAsync(Orders.TestOrder1.OrderId)).Returns(Orders.TestOrder1);
            A.CallTo(() => orderRepository.BuyAsync(Orders.TestOrder1.OrderId)).Returns(true);

            //Act
            var result = await orderService.BuyAsync(Orders.TestOrder1.OrderId, Orders.TestOrder1.UserId);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => orderRepository.BuyAsync(Orders.TestOrder1.OrderId)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task BuyAsyncNegative_InvalidUser_ReturnServiceResultWithBadRequest()
        {
            var orderService = new OrderService(orderRepository, mapper);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid Id");

            A.CallTo(() => orderRepository.GetOrderByIdAsync(Orders.TestOrder1.OrderId)).Returns(Orders.TestOrder1);
            A.CallTo(() => orderRepository.BuyAsync(Orders.TestOrder1.OrderId)).Returns(false);

            //Act
            var result = await orderService.BuyAsync(Orders.TestOrder1.OrderId, 2);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => orderRepository.BuyAsync(Orders.TestOrder1.OrderId)).MustNotHaveHappened();
        }
        [Fact]
        public async Task BuyAsyncNegative_BuyingHaulted_ReturnServiceResultWithBadRequest()
        {
            var orderService = new OrderService(orderRepository, mapper);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid Id");

            A.CallTo(() => orderRepository.GetOrderByIdAsync(Orders.TestOrder1.OrderId)).Returns(Orders.TestOrder1);
            A.CallTo(() => orderRepository.BuyAsync(Orders.TestOrder1.OrderId)).Returns(false);

            //Act
            var result = await orderService.BuyAsync(Orders.TestOrder1.OrderId, 1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => orderRepository.BuyAsync(Orders.TestOrder1.OrderId)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task DeleteItemsAsyncPositive_ReturnServiceResultWithOk()
        {
            var orderService = new OrderService(orderRepository, mapper);
            var serviceResult = new ServiceResult(ResultType.Success, "Success");

            A.CallTo(() => orderRepository.DeleteItemsAsync(Orders.TestOrder1.OrderId, Orders.TestOrderDeletionInfo1.ProductId)).Returns(true);

            //Act
            var result = await orderService.DeleteItems(Orders.TestOrder1.OrderId, Orders.TestOrderDeletionInfo1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => orderRepository.DeleteItemsAsync(Orders.TestOrder1.OrderId, Orders.TestOrderDeletionInfo1.ProductId)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task DeleteItemsAsyncNegative_InvalidId_ReturnServiceResultWithBadRequest()
        {
            var orderService = new OrderService(orderRepository, mapper);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid Id");

            A.CallTo(() => orderRepository.DeleteItemsAsync(Orders.TestOrder1.OrderId, Orders.TestOrderDeletionInfo1.ProductId)).Returns(true);

            //Act
            var result = await orderService.DeleteItems(2, Orders.TestOrderDeletionInfo1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => orderRepository.DeleteItemsAsync(Orders.TestOrder1.OrderId, Orders.TestOrderDeletionInfo1.ProductId)).MustNotHaveHappened();
        }
    }
}
