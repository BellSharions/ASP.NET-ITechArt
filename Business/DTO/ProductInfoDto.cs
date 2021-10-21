using Business.Enums;
using DAL.Enums;
using Microsoft.AspNetCore.Http;

namespace Business.DTO
{
    public class ProductInfoDto
    {
        public string Name { get; set; }
        public string Platform { get; set; }
        public string Genre { get; set; }
        public string Rating { get; set; }
        public string Logo { get; set; }
        public string Background { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public string DateCreated { get; set; }
        public int TotalRating { get; set; }
    }
}
