using Business.DTO;
using DAL.Entities;
using FakeItEasy;
using System;
using System.Threading.Tasks;
using UnitTests.Consts;
using Xunit;

namespace UnitTests.ServiceTests
{
    public class OrderServiceTest : IClassFixture<OrderServiceFixtures>
    {
        OrderServiceFixtures orderFixture;
        public OrderServiceTest(OrderServiceFixtures fixture)
        {
            this.orderFixture = fixture;
            Fake.ClearRecordedCalls(orderFixture.orderRepository);
        }
        [Fact]
        public async Task GetOrderByIdAsyncPositive_ReturnOrder()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.TestOrderWithProducts1.OrderId)).Returns(orderFixture.TestOrderWithProducts1);

            //Act
            var result = await orderFixture.orderService.GetOrderByIdAsync(orderFixture.TestOrderWithProducts1.OrderId);

            //Assert
            Assert.Equal(orderFixture.TestOrderWithProducts1, result);

            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.TestOrderWithProducts1.OrderId)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetOrderInfoByIdAsyncPositive_ReturnOrderInfoDto()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(1)).Returns(orderFixture.TestOrderWithProducts3);

            //Act
            var result = await orderFixture.orderService.GetOrderInfoByIdAsync(1);

            //Assert
            Assert.Equal(orderFixture.TestOrderInfo3.Status, result.Status);
            for (var i = 0; i < result.ProductInfo.Count; i++)
                Assert.True(orderFixture.TestOrderInfo3.ProductInfo[i].Equals(result.ProductInfo[i]));

            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(1)).MustHaveHappenedOnceExactly(); 
        }
        [Fact]
        public async Task GetOrderInfoByIdAsyncNegative_ReturnNull()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(1)).Returns(orderFixture.NullOrder);

            //Act
            var result = await orderFixture.orderService.GetOrderInfoByIdAsync(1);

            //Assert
            Assert.Null(result);

            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(1)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangeOrderAmountAsyncPositive_ReturnServiceResultOk()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.TestOrderWithProducts2.OrderId)).Returns(orderFixture.TestOrderWithProducts2);
            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(orderFixture.TestOrderWithProducts2)).Returns(true);

            //Act
            var result = await orderFixture.orderService.ChangeOrderAmountAsync(orderFixture.TestOrderWithProducts2.OrderId, orderFixture.OrderAmountTest2);

            //Assert
            Assert.Equal(orderFixture.serviceResultOk.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(A<Order>.Ignored)).MustHaveHappenedOnceExactly(); 
        }
        [Fact]
        public async Task ChangeOrderAmountAsyncNegative_NoProducts_ReturnServiceResultBadRequest()
        {

            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.TestOrderNoProducts1.OrderId)).Returns(orderFixture.TestOrderNoProducts1);
            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(orderFixture.TestOrderNoProducts1)).Returns(true);

            //Act
            var result = await orderFixture.orderService.ChangeOrderAmountAsync(orderFixture.TestOrderNoProducts1.OrderId, orderFixture.OrderAmountTest2);

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);
            
            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(orderFixture.TestOrderNoProducts1)).MustNotHaveHappened(); 
        }
        [Fact]
        public async Task ChangeOrderAmountAsyncNegative_NoInfo_ReturnServiceResultBadRequest()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.TestOrderWithProducts1.OrderId)).Returns(orderFixture.TestOrderWithProducts1);
            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(orderFixture.TestOrderWithProducts1)).Returns(true);

            //Act
            var result = await orderFixture.orderService.ChangeOrderAmountAsync(orderFixture.TestOrderWithProducts1.OrderId, new OrderAmountChangeDto());

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(orderFixture.TestOrderWithProducts1)).MustNotHaveHappened(); 
        }
        [Fact]
        public async Task ChangeOrderAmountAsyncNegative_NoOrder_ReturnServiceResultBadRequest()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.TestOrderWithProducts1.OrderId)).Returns(new Order());
            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(orderFixture.TestOrderWithProducts1)).Returns(true);

            //Act
            var result = await orderFixture.orderService.ChangeOrderAmountAsync(orderFixture.TestOrderWithProducts1.OrderId, orderFixture.OrderAmountTest2);

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(orderFixture.TestOrderWithProducts1)).MustNotHaveHappened(); 
        }
        [Fact]
        public async Task BuyAsyncPositive_ReturnServiceResultWithOk()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.TestOrderWithProducts1.OrderId)).Returns(orderFixture.TestOrderWithProducts1);
            A.CallTo(() => orderFixture.orderRepository.BuyAsync(orderFixture.TestOrderWithProducts1.OrderId)).Returns(true);

            //Act
            var result = await orderFixture.orderService.BuyAsync(orderFixture.TestOrderWithProducts1.OrderId, orderFixture.TestOrderWithProducts1.UserId);

            //Assert
            Assert.Equal(orderFixture.serviceResultOk.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.BuyAsync(orderFixture.TestOrderWithProducts1.OrderId)).MustHaveHappenedOnceExactly(); 
        }
        [Fact]
        public async Task BuyAsyncNegative_InvalidUser_ReturnServiceResultWithBadRequest()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.TestOrderWithProducts1.OrderId)).Returns(orderFixture.TestOrderWithProducts1);
            A.CallTo(() => orderFixture.orderRepository.BuyAsync(orderFixture.TestOrderWithProducts1.OrderId)).Returns(false);

            //Act
            var result = await orderFixture.orderService.BuyAsync(orderFixture.TestOrderWithProducts1.OrderId, 2);

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.BuyAsync(orderFixture.TestOrderWithProducts1.OrderId)).MustNotHaveHappened(); 
        }
        [Fact]
        public async Task BuyAsyncNegative_BuyingHaulted_ReturnServiceResultWithBadRequest()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.TestOrderWithProducts1.OrderId)).Returns(orderFixture.TestOrderWithProducts1);
            A.CallTo(() => orderFixture.orderRepository.BuyAsync(orderFixture.TestOrderWithProducts1.OrderId)).Returns(false);

            //Act
            var result = await orderFixture.orderService.BuyAsync(orderFixture.TestOrderWithProducts1.OrderId, 1);

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.BuyAsync(orderFixture.TestOrderWithProducts1.OrderId)).MustHaveHappenedOnceExactly(); 
        }
        [Fact]
        public async Task DeleteItemsAsyncPositive_ReturnServiceResultWithOk()
        {
            A.CallTo(() => orderFixture.orderRepository.DeleteItemsAsync(orderFixture.TestOrderWithProducts1.OrderId, orderFixture.TestOrderDeletionInfo1.ProductId)).Returns(true);

            //Act
            var result = await orderFixture.orderService.DeleteItems(orderFixture.TestOrderWithProducts1.OrderId, orderFixture.TestOrderDeletionInfo1);

            //Assert
            Assert.Equal(orderFixture.serviceResultOk.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.DeleteItemsAsync(orderFixture.TestOrderWithProducts1.OrderId, orderFixture.TestOrderDeletionInfo1.ProductId)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task DeleteItemsAsyncNegative_InvalidId_ReturnServiceResultWithBadRequest()
        {
            A.CallTo(() => orderFixture.orderRepository.DeleteItemsAsync(orderFixture.TestOrderWithProducts1.OrderId, orderFixture.TestOrderDeletionInfo1.ProductId)).Returns(true);

            //Act
            var result = await orderFixture.orderService.DeleteItems(2, orderFixture.TestOrderDeletionInfo1);

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.DeleteItemsAsync(orderFixture.TestOrderWithProducts1.OrderId, orderFixture.TestOrderDeletionInfo1.ProductId)).MustNotHaveHappened();
        }
        [Fact]
        public async Task CreateOrderAsyncPositive_ReturnServiceResultWithOk()
        {
            orderFixture.TestOrderCreation1.OrderList.Add(orderFixture.OrderListTest1);
            orderFixture.TestOrderWithProducts1.OrderList.Add(orderFixture.OrderListTest1);
            orderFixture.TestOrderWithProducts1.CreationDate = DateTime.MinValue;
            orderFixture.TestOrderWithProducts1.OrderId = 0;
            A.CallTo(() => orderFixture.orderRepository.CreateAsync(A<Order>.Ignored)).Returns(true);

            //Act
            var result = await orderFixture.orderService.CreateOrderAsync(orderFixture.TestOrderCreation1);

            //Assert
            Assert.Equal(orderFixture.serviceResultOk.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.CreateAsync(A<Order>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task CreateOrderAsyncNegative_NoInfo_ReturnServiceResultWithBadRequest()
        {
            orderFixture.TestOrderCreation1.OrderList.Add(orderFixture.OrderListTest1);
            orderFixture.TestOrderWithProducts1.OrderList.Add(orderFixture.OrderListTest1);
            orderFixture.TestOrderWithProducts1.CreationDate = DateTime.MinValue;
            orderFixture.TestOrderWithProducts1.OrderId = 0;
            A.CallTo(() => orderFixture.orderRepository.CreateAsync(A<Order>.Ignored)).Returns(true);

            //Act
            var result = await orderFixture.orderService.CreateOrderAsync(new OrderCreationDto());

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.CreateAsync(A<Order>.Ignored)).MustNotHaveHappened();
        }
        [Fact]
        public async Task CreateOrderAsyncNegative_ResultInNull_ReturnServiceResultWithBadRequest()
        {
            orderFixture.TestOrderCreation1.OrderList.Add(orderFixture.OrderListTest3);
            orderFixture.TestOrderWithProducts1.OrderList.Add(orderFixture.OrderListTest3);
            A.CallTo(() => orderFixture.orderRepository.CreateAsync(orderFixture.TestOrderWithProducts1)).Returns(true);

            //Act
            var result = await orderFixture.orderService.CreateOrderAsync(orderFixture.TestOrderCreation1);

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.CreateAsync(orderFixture.TestOrderWithProducts1)).MustNotHaveHappened();
        }
    }
}
