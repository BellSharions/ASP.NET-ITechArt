
using DAL.Entities;
using DAL.Enums;
using System;
using System.Collections.Generic;

namespace Business.DTO
{
    public class OrderCreationDto
    {
        public int Amount { get; set; }
        public Product Product { get; set; }
        public ICollection<OrderList> OrderList { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
