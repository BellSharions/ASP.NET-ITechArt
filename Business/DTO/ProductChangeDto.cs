using Business.Enums;
using DAL.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class ProductChangeDto
    {
        public int Id { get; set; }
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
