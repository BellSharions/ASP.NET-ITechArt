
using System;

namespace Business.DTO
{
    public class ProductInfoDto : IEquatable<ProductInfoDto>
    {
        public string Name { get; set; }
        public string Platform { get; set; }
        public string Genre { get; set; }
        public string Rating { get; set; }
        public string Logo { get; set; }
        public string Background { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string DateCreated { get; set; }
        public int TotalRating { get; set; }

        public bool Equals(ProductInfoDto other)
        {
            if(Background == other.Background &&
                Name == other.Name &&
                Platform == other.Platform &&
                Genre == other.Genre &&
                Rating == other.Rating &&
                Logo == other.Logo &&
                Price == other.Price &&
                Count == other.Count &&
                TotalRating == other.TotalRating)
                return true;
            return false;
        }
    }
}
