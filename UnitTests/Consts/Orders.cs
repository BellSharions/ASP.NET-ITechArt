using Business.DTO;
using DAL.Entities;
using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Consts
{
    public static class Orders
    {
        public static Order TestOrder1 = new()
        {
            UserId = 1,
            Amount = 4,
            CreationDate = DateTime.Now,
            OrderId = 1,
            OrderList = new List<OrderList>(),
            Status = OrderStatus.Unpaid,
        };
        public static Order TestOrder2 = new()
        {
            UserId = 1,
            Amount = 4,
            CreationDate = DateTime.Now,
            OrderId = 1,
            OrderList = new List<OrderList>(),
            Status = OrderStatus.Unpaid,
        };
        public static Order TestOrder3 = new()
        {
            UserId = 1,
            Amount = 4,
            CreationDate = DateTime.Now,
            OrderId = 1,
            OrderList = new List<OrderList>(),
            Status = OrderStatus.Unpaid,
        };
        public static Order TestOrder4 = new()
        {
            UserId = 1,
            Amount = 4,
            CreationDate = DateTime.Now,
            OrderId = 1,
            OrderList = new List<OrderList>(),
            Status = OrderStatus.Unpaid,
        };
        public static Order TestOrderNoProducts1 = new()
        {
            UserId = 1,
            Amount = 4,
            CreationDate = DateTime.Now,
            OrderId = 1,
            OrderList = new List<OrderList>(),
            Status = OrderStatus.Unpaid,
        };
        public static OrderInfoDto TestOrderInfo1 = new()
        {
            Amount = 4,
            CreationDate = DateTime.Now,
            Status = OrderStatus.Unpaid,
            ProductInfo = new List<ProductInfoDto>
            {
                Products.TestProductInfo1
            }
        };
        public static OrderInfoDto TestOrderInfo2 = new()
        {
            Amount = 4,
            CreationDate = DateTime.Now,
            Status = OrderStatus.Unpaid,
            ProductInfo = new List<ProductInfoDto>
            {
                Products.TestProductInfo1
            }
        };
        public static OrderInfoDto TestOrderInfo3 = new()
        {
            Amount = 4,
            CreationDate = DateTime.Now,
            Status = OrderStatus.Unpaid,
            ProductInfo = new List<ProductInfoDto>
            {
                Products.TestProductInfo2
            }
        };
        public static OrderCreationDto TestOrderCreation1 = new()
        {
            Amount = 4,
            UserId = 1,
            OrderList = new List<OrderList>()
        };
        public static OrderCreationDto TestOrderCreation2 = new()
        {
            Amount = 4,
            UserId = 1,
            OrderList = new List<OrderList>()
        };
        public static OrderItemsDeletionDto TestOrderDeletionInfo1 = new()
        {
            UserId = 1,
            ProductId = new List<int>
            {
                1, 2
            }
        };
        public static OrderList TestOrderList1 = new()
        {
            Amount = 4,
            OrderId = 1,
            ProductId = 2,
            Product = Products.TestProduct1
        };
        public static OrderList TestOrderList2 = new()
        {
            Amount = 4,
            OrderId = 1,
            ProductId = 2,
            Product = Products.TestProduct2
        };

        public static OrderList OrderListTest1 = new()
        {
            ProductId = 1,
            Amount = 2,
            OrderId = 1
        };
        public static OrderList OrderListTest2 = new()
        {
            ProductId = 2,
            Amount = 2,
            OrderId = 1
        };
        public static OrderList OrderListTest3 = new()
        {
            ProductId = 1,
            Amount = 2,
            OrderId = 1
        };
        public static OrderAmountChangeDto OrderAmountTest2 = new()
        {
            ProductId = 2,
            Amount = 2,
            UserId = 1
        };

    }
}
