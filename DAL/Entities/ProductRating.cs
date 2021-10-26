
namespace DAL.Entities
{
    public class ProductRating
    {
        public int ProductId {  get; set; }
        public int UserId {  get; set; }
        public int RatingId {  get; set; }
        public int Rating {  get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
    }
}
