using DAL.Entities;

namespace Business.DTO
{
    public class OrderChangeDto
    {
        public int Amount { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
    }
}
