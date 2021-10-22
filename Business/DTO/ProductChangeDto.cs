using Business.Enums;
using DAL.Enums;
using Microsoft.AspNetCore.Http;

namespace Business.DTO
{
    public class ProductChangeDto
    {
        public string Name { get; set; }
        public AvailablePlatforms Platform { get; set; }
        public AvailableGenres Genre { get; set; }
        public AgeRating Rating { get; set; }
        public IFormFile Logo { get; set; }
        public IFormFile Background { get; set; }
        public string Price { get; set; }
        public string Count { get; set; }
        public string DateCreated { get; set; }
        public int TotalRating { get; set; }
        public bool IsDeleted { get; set; }
    }
}
