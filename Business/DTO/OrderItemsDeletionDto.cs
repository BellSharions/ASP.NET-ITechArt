using System.Collections.Generic;

namespace Business.DTO
{
    public class OrderItemsDeletionDto
    {
        public int UserId {  get; set; }
        public ICollection<int> ProductId {  get; set; }
    }
}
