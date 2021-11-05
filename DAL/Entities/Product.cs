using Business.Enums;
using DAL.Enums;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class Product
    {
        public int Id {  get; set; }
        public string Name {  get; set; }
        public AvailablePlatforms Platform { get; set; }
        public AvailableGenres Genre { get; set; }
        public AgeRating Rating { get; set; }
        public string Logo { get; set; }
        public string Background { get; set; } 
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string DateCreated {  get; set; }
        public ICollection<ProductRating> Ratings {  get; set; }
        public int TotalRating {  get; set; }
        public bool IsDeleted {  get; set; }
        public ICollection<OrderList> OrderList { get; set; }
    }
    
}
