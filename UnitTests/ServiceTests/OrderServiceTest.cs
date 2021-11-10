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
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.CorrectOrderWithProducts1.OrderId)).Returns(orderFixture.CorrectOrderWithProducts1);

            //Act
            var result = await orderFixture.orderService.GetOrderByIdAsync(orderFixture.CorrectOrderWithProducts1.OrderId);

            //Assert
            Assert.Equal(orderFixture.CorrectOrderWithProducts1, result);

            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.CorrectOrderWithProducts1.OrderId)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetOrderInfoByIdAsyncPositive_ReturnOrderInfoDto()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(1)).Returns(orderFixture.CorrectOrderWithProducts2);

            //Act
            var result = await orderFixture.orderService.GetOrderInfoByIdAsync(1);

            //Assert
            Assert.Equal(orderFixture.CorrectOrderInfo.Status, result.Status);
            for (var i = 0; i < result.ProductInfo.Count; i++)
                Assert.True(orderFixture.CorrectOrderInfo.ProductInfo[i].Equals(result.ProductInfo[i]));

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
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.CorrectOrderWithProducts2.OrderId)).Returns(orderFixture.CorrectOrderWithProducts2);
            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(orderFixture.CorrectOrderWithProducts2)).Returns(true);

            //Act
            var result = await orderFixture.orderService.ChangeOrderAmountAsync(orderFixture.CorrectOrderWithProducts2.OrderId, orderFixture.OrderAmountTest);

            //Assert
            Assert.Equal(orderFixture.serviceResultOk.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(A<Order>.Ignored)).MustHaveHappenedOnceExactly(); 
        }
        [Fact]
        public async Task ChangeOrderAmountAsyncNegative_NoProducts_ReturnServiceResultBadRequest()
        {

            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.CorrectOrderWithNoProducts.OrderId)).Returns(orderFixture.CorrectOrderWithNoProducts);
            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(orderFixture.CorrectOrderWithNoProducts)).Returns(true);

            //Act
            var result = await orderFixture.orderService.ChangeOrderAmountAsync(orderFixture.CorrectOrderWithNoProducts.OrderId, orderFixture.OrderAmountTest);

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);
            
            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(orderFixture.CorrectOrderWithNoProducts)).MustNotHaveHappened(); 
        }
        [Fact]
        public async Task ChangeOrderAmountAsyncNegative_NoInfo_ReturnServiceResultBadRequest()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.CorrectOrderWithProducts1.OrderId)).Returns(orderFixture.CorrectOrderWithProducts1);
            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(orderFixture.CorrectOrderWithProducts1)).Returns(true);

            //Act
            var result = await orderFixture.orderService.ChangeOrderAmountAsync(orderFixture.CorrectOrderWithProducts1.OrderId, new OrderAmountChangeDto());

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(orderFixture.CorrectOrderWithProducts1)).MustNotHaveHappened(); 
        }
        [Fact]
        public async Task ChangeOrderAmountAsyncNegative_NoOrder_ReturnServiceResultBadRequest()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.CorrectOrderWithProducts1.OrderId)).Returns(orderFixture.NullOrder);
            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(orderFixture.CorrectOrderWithProducts1)).Returns(true);

            //Act
            var result = await orderFixture.orderService.ChangeOrderAmountAsync(orderFixture.CorrectOrderWithProducts1.OrderId, orderFixture.OrderAmountTest);

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.UpdateItemAsync(orderFixture.CorrectOrderWithProducts1)).MustNotHaveHappened(); 
        }
        [Fact]
        public async Task BuyAsyncPositive_ReturnServiceResultWithOk()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.CorrectOrderWithProducts1.OrderId)).Returns(orderFixture.CorrectOrderWithProducts1);
            A.CallTo(() => orderFixture.orderRepository.BuyAsync(orderFixture.CorrectOrderWithProducts1.OrderId)).Returns(true);

            //Act
            var result = await orderFixture.orderService.BuyAsync(orderFixture.CorrectOrderWithProducts1.OrderId, orderFixture.CorrectOrderWithProducts1.UserId);

            //Assert
            Assert.Equal(orderFixture.serviceResultOk.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.BuyAsync(orderFixture.CorrectOrderWithProducts1.OrderId)).MustHaveHappenedOnceExactly(); 
        }
        [Fact]
        public async Task BuyAsyncNegative_InvalidUser_ReturnServiceResultWithBadRequest()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.CorrectOrderWithProducts1.OrderId)).Returns(orderFixture.CorrectOrderWithProducts1);
            A.CallTo(() => orderFixture.orderRepository.BuyAsync(orderFixture.CorrectOrderWithProducts1.OrderId)).Returns(false);

            //Act
            var result = await orderFixture.orderService.BuyAsync(orderFixture.CorrectOrderWithProducts1.OrderId, 2);

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.BuyAsync(orderFixture.CorrectOrderWithProducts1.OrderId)).MustNotHaveHappened(); 
        }
        [Fact]
        public async Task BuyAsyncNegative_BuyingHaulted_ReturnServiceResultWithBadRequest()
        {
            A.CallTo(() => orderFixture.orderRepository.GetOrderByIdAsync(orderFixture.CorrectOrderWithProducts1.OrderId)).Returns(orderFixture.CorrectOrderWithProducts1);
            A.CallTo(() => orderFixture.orderRepository.BuyAsync(orderFixture.CorrectOrderWithProducts1.OrderId)).Returns(false);

            //Act
            var result = await orderFixture.orderService.BuyAsync(orderFixture.CorrectOrderWithProducts1.OrderId, 2);

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.BuyAsync(orderFixture.CorrectOrderWithProducts1.OrderId)).MustNotHaveHappened(); 
        }
        [Fact]
        public async Task DeleteItemsAsyncPositive_ReturnServiceResultWithOk()
        {
            A.CallTo(() => orderFixture.orderRepository.DeleteItemsAsync(orderFixture.CorrectOrderWithProducts1.OrderId, orderFixture.TestOrderDeletionInfo.ProductId)).Returns(true);

            //Act
            var result = await orderFixture.orderService.DeleteItems(orderFixture.CorrectOrderWithProducts1.OrderId, orderFixture.TestOrderDeletionInfo);

            //Assert
            Assert.Equal(orderFixture.serviceResultOk.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.DeleteItemsAsync(orderFixture.CorrectOrderWithProducts1.OrderId, orderFixture.TestOrderDeletionInfo.ProductId)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task DeleteItemsAsyncNegative_InvalidId_ReturnServiceResultWithBadRequest()
        {
            A.CallTo(() => orderFixture.orderRepository.DeleteItemsAsync(orderFixture.CorrectOrderWithProducts1.OrderId, orderFixture.TestOrderDeletionInfo.ProductId)).Returns(true);

            //Act
            var result = await orderFixture.orderService.DeleteItems(2, orderFixture.TestOrderDeletionInfo);

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.DeleteItemsAsync(orderFixture.CorrectOrderWithProducts1.OrderId, orderFixture.TestOrderDeletionInfo.ProductId)).MustNotHaveHappened();
        }
        [Fact]
        public async Task CreateOrderAsyncPositive_ReturnServiceResultWithOk()
        {
            orderFixture.CorrectOrderCreation.OrderList.Add(orderFixture.CorrectOrderList);
            orderFixture.CorrectOrderWithProducts1.OrderList.Add(orderFixture.CorrectOrderList);
            orderFixture.CorrectOrderWithProducts1.CreationDate = DateTime.MinValue;
            orderFixture.CorrectOrderWithProducts1.OrderId = 0;
            A.CallTo(() => orderFixture.orderRepository.CreateAsync(A<Order>.Ignored)).Returns(true);

            //Act
            var result = await orderFixture.orderService.CreateOrderAsync(orderFixture.CorrectOrderCreation);

            //Assert
            Assert.Equal(orderFixture.serviceResultOk.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.CreateAsync(A<Order>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task CreateOrderAsyncNegative_NoInfo_ReturnServiceResultWithBadRequest()
        {
            orderFixture.CorrectOrderCreation.OrderList.Add(orderFixture.CorrectOrderList);
            orderFixture.CorrectOrderWithProducts1.OrderList.Add(orderFixture.CorrectOrderList);
            orderFixture.CorrectOrderWithProducts1.CreationDate = DateTime.MinValue;
            orderFixture.CorrectOrderWithProducts1.OrderId = 0;
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
            orderFixture.CorrectOrderCreation.OrderList.Add(orderFixture.CorrectOrderList);
            orderFixture.CorrectOrderWithProducts1.OrderList.Add(orderFixture.CorrectOrderList);
            A.CallTo(() => orderFixture.orderRepository.CreateAsync(orderFixture.CorrectOrderWithProducts1)).Returns(true);

            //Act
            var result = await orderFixture.orderService.CreateOrderAsync(orderFixture.CorrectOrderCreation);

            //Assert
            Assert.Equal(orderFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => orderFixture.orderRepository.CreateAsync(orderFixture.CorrectOrderWithProducts1)).MustNotHaveHappened();
        }
    }
}
