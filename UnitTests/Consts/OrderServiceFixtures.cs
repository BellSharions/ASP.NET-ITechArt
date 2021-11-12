using AutoMapper;
using Business;
using Business.DTO;
using Business.Enums;
using Business.Interfaces;
using Business.Services;
using DAL.Entities;
using DAL.Entities.Models;
using DAL.Enums;
using FakeItEasy;
using System;
using System.Collections.Generic;

namespace UnitTests.Consts
{
    public class OrderServiceFixtures
    {
        public Order CorrectOrderWithProducts1 { get; set; }
        public Order CorrectOrderWithProducts2 { get; set; }
        public Order CorrectOrderWithNoProducts { get; set; }
        public Order NullOrder { get; set; }
        public OrderInfoDto CorrectOrderInfo { get; set; }
        public OrderCreationDto CorrectOrderCreation { get; set; }
        public OrderItemsDeletionDto TestOrderDeletionInfo { get; set; }
        public OrderList CorrectOrderInfoWithProduct1 { get; set; }
        public OrderList CorrectOrderInfoWithProduct2 { get; set; }
        public OrderList CorrectOrderList { get; set; }
        public OrderAmountChangeDto OrderAmountTest { get; set; }
        public IOrderRepository orderRepository { get; set; }
        public IMapper mapper { get; set; }
        public OrderService orderService { get; set; }
        public ServiceResult serviceResultOk { get; set; }
        public ServiceResult serviceResultBadRequest { get; set; }
        public ServiceResult serviceResultCreated { get; set; }
        public OrderServiceFixtures()
        {
            CorrectOrderWithProducts1 = new()
            {
                UserId = 1,
                Amount = 4,
                CreationDate = DateTime.Now,
                OrderId = 1,
                OrderList = new List<OrderList>(),
                Status = OrderStatus.Unpaid,
            };
            CorrectOrderWithProducts2 = new()
            {
                UserId = 1,
                Amount = 2,
                CreationDate = DateTime.Now,
                OrderId = 2,
                OrderList = new List<OrderList>(),
                Status = OrderStatus.Unpaid,
            };
            NullOrder = null;
            CorrectOrderWithNoProducts = new()
            {
                UserId = 2,
                Amount = 4,
                CreationDate = DateTime.Now,
                OrderId = 1,
                OrderList = new List<OrderList>(),
                Status = OrderStatus.Unpaid,
            };
            CorrectOrderInfo = new()
            {
                Amount = 4,
                CreationDate = DateTime.Now,
                Status = OrderStatus.Unpaid,
                ProductInfo = new List<ProductInfoDto>
            {
                new()
                {
                    Name = "Ultrakill",
                    Platform = "PC",
                    TotalRating = 99,
                    Genre = "FPS",
                    Rating = "PEGI18",
                    Logo = "",
                    Background = "",
                    Price = 99,
                    Count = 99
                }
            }
            };
            CorrectOrderCreation = new()
            {
                Amount = 4,
                UserId = 1,
                OrderList = new List<OrderList>()
            };
            TestOrderDeletionInfo = new()
            {
                UserId = 1,
                ProductId = new List<int>
            {
                1, 2
            }
            };
            CorrectOrderInfoWithProduct1 = new()
            {
                Amount = 4,
                OrderId = 1,
                ProductId = 2,
                Product = new()
                {
                    Id = 1,
                    Name = "Genshin Impact",
                    Platform = AvailablePlatforms.PC,
                    TotalRating = 87,
                    Genre = AvailableGenres.Adventure,
                    Rating = AgeRating.PEGI16,
                    Logo = "",
                    Background = "",
                    Price = 1,
                    Count = 150
                }
            };
            CorrectOrderInfoWithProduct2 = new()
            {
                Amount = 4,
                OrderId = 1,
                ProductId = 2,
                Product = new()
                {
                    Id = 2,
                    Name = "Ultrakill",
                    Platform = AvailablePlatforms.PC,
                    TotalRating = 99,
                    Genre = AvailableGenres.FPS,
                    Rating = AgeRating.PEGI18,
                    Logo = "",
                    Background = "",
                    Price = 99,
                    Count = 99
                }
            };
            CorrectOrderList = new()
            {
                ProductId = 1,
                Amount = 2,
                OrderId = 1
            };
            OrderAmountTest = new()
            {
                ProductId = 2,
                Amount = 2,
                UserId = 1
            };
            CorrectOrderWithProducts1.OrderList.Add(CorrectOrderInfoWithProduct1);
            CorrectOrderWithProducts2.OrderList.Add(CorrectOrderInfoWithProduct2);

            orderRepository = A.Fake<IOrderRepository>();
            Fake.ClearRecordedCalls(orderRepository);
            mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
            orderService = new OrderService(orderRepository, mapper);
            serviceResultOk = new ServiceResult(ResultType.Success, "Success");
            serviceResultBadRequest = new ServiceResult(ResultType.BadRequest, "Invalid Information");
            serviceResultCreated = new ServiceResult(ResultType.Created, "Created");
        }
    }
}
