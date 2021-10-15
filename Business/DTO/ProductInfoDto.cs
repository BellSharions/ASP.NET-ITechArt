using Business.Enums;
using DAL.Enums;

namespace Business.DTO
{
    public class ProductInfoDto
    {
        public string Name { get; set; }
        public AvailablePlatforms Platform { get; set; }
        public AvailableGenres Genre { get; set; }
        public AgeRating Rating { get; set; }
        public string Logo { get; set; }
        public string Background { get; set; }
        public string Price { get; set; }
        public string Count { get; set; }
        public string DateCreated { get; set; }
        public int TotalRating { get; set; }
    }
}
