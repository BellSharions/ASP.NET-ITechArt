using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class OrderList
    {
        public int ProductId {  get; set; }
        public int OrderId {  get; set; }
        public Product Product {  get; set; }
        public Order Order {  get; set; }
        public int Amount {  get; set; }
    }
}
