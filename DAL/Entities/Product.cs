using Business.Enums;

namespace DAL.Entities
{
    public class Product
    {
        public int Id {  get; set; }
        public string Name {  get; set; }
        public AvailablePlatforms Platform { get; set; }
        public string DateCreated {  get; set; }
        public int TotalRating {  get; set; }
    }
    
}
