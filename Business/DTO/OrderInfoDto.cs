using DAL.Entities;
using DAL.Enums;
using System;
using System.Collections.Generic;

namespace Business.DTO
{
    public class OrderInfoDto
    {
        public DateTime CreationDate { get; set; }
        public int Amount { get; set; }
        public OrderStatus Status { get; set; }
        public List<ProductInfoDto> ProductInfo { get; set; }
    }
}
