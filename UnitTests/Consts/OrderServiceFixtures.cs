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
        public Order TestOrderWithProducts1 { get; set; }
        public Order TestOrderWithProducts2 { get; set; }
        public Order TestOrderWithProducts3 { get; set; }
        public Order TestOrderNoProducts1 { get; set; }
        public Order NullOrder { get; set; }
        public OrderInfoDto TestOrderInfo1 { get; set; }
        public OrderInfoDto TestOrderInfo2 { get; set; }
        public OrderInfoDto TestOrderInfo3 { get; set; }
        public OrderCreationDto TestOrderCreation1 { get; set; }
        public OrderItemsDeletionDto TestOrderDeletionInfo1 { get; set; }
        public OrderList TestOrderList1 { get; set; }
        public OrderList TestOrderList2 { get; set; }
        public OrderList OrderListTest1 { get; set; }
        public OrderList OrderListTest2 { get; set; }
        public OrderList OrderListTest3 { get; set; }
        public OrderAmountChangeDto OrderAmountTest2 { get; set; }
        public IOrderRepository orderRepository { get; set; }
        public IMapper mapper { get; set; }
        public OrderService orderService { get; set; }
        public ServiceResult serviceResultOk { get; set; }
        public ServiceResult serviceResultBadRequest { get; set; }
        public ServiceResult serviceResultCreated { get; set; }
        public OrderServiceFixtures()
        {
            TestOrderWithProducts1 = new()
            {
                UserId = 1,
                Amount = 4,
                CreationDate = DateTime.Now,
                OrderId = 1,
                OrderList = new List<OrderList>(),
                Status = OrderStatus.Unpaid,
            };
            TestOrderWithProducts2 = new()
            {
                UserId = 1,
                Amount = 2,
                CreationDate = DateTime.Now,
                OrderId = 2,
                OrderList = new List<OrderList>(),
                Status = OrderStatus.Unpaid,
            };
            TestOrderWithProducts3 = new()
            {
                UserId = 1,
                Amount = 4,
                CreationDate = DateTime.Now,
                OrderId = 3,
                OrderList = new List<OrderList>(),
                Status = OrderStatus.Unpaid,
            };
            NullOrder = null;
            TestOrderNoProducts1 = new()
            {
                UserId = 1,
                Amount = 4,
                CreationDate = DateTime.Now,
                OrderId = 1,
                OrderList = new List<OrderList>(),
                Status = OrderStatus.Unpaid,
            };
            TestOrderInfo1 = new()
            {
                Amount = 4,
                CreationDate = DateTime.Now,
                Status = OrderStatus.Unpaid,
                ProductInfo = new List<ProductInfoDto>
                { 
                new()
                {
                    Name = "Genshin Impact",
                    Platform = "PC",
                    TotalRating = 87,
                    Genre = "Adventure",
                    Rating = "PEGI16",
                    Logo = "",
                    Background = "",
                    Price = 1,
                    Count = 150
                }
                }
            };
            TestOrderInfo2 = new()
            {
                Amount = 4,
                CreationDate = DateTime.Now,
                Status = OrderStatus.Unpaid,
                ProductInfo = new List<ProductInfoDto>
            {
                new()
                {
                    Name = "Genshin Impact",
                    Platform = "PC",
                    TotalRating = 87,
                    Genre = "Adventure",
                    Rating = "PEGI16",
                    Logo = "",
                    Background = "",
                    Price = 1,
                    Count = 150
                }
            }
            };
            TestOrderInfo3 = new()
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
            TestOrderCreation1 = new()
            {
                Amount = 4,
                UserId = 1,
                OrderList = new List<OrderList>()
            };
            TestOrderDeletionInfo1 = new()
            {
                UserId = 1,
                ProductId = new List<int>
            {
                1, 2
            }
            };
            TestOrderList1 = new()
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
            TestOrderList2 = new()
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
            OrderListTest1 = new()
            {
                ProductId = 1,
                Amount = 2,
                OrderId = 1
            };
            OrderListTest2 = new()
            {
                ProductId = 2,
                Amount = 2,
                OrderId = 1
            };
            OrderListTest3 = new()
            {
                ProductId = 1,
                Amount = 2,
                OrderId = 1
            };
            OrderAmountTest2 = new()
            {
                ProductId = 2,
                Amount = 2,
                UserId = 1
            };
            TestOrderWithProducts1.OrderList.Add(TestOrderList1);
            TestOrderWithProducts2.OrderList.Add(OrderListTest2);
            TestOrderWithProducts3.OrderList.Add(TestOrderList2);
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
