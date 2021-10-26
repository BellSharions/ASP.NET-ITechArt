
using DAL.Entities;
using DAL.Enums;
using System;

namespace Business.DTO
{
    public class OrderCreationDto
    {
        public DateTime CreationDate { get; set; }
        public int Amount { get; set; }
        public OrderStatus Status { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
    }
}
