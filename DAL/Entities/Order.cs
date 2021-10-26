using DAL.Enums;
using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public int Amount { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderList> OrderList { get; set; }
        public User User { get; set; }

    }
}
