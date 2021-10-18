using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class ListProductPageDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public AvailableGenres Genre {  get; set; }
        public AgeRating AgeRating {  get; set; }
        public Sorting PriceSort {  get; set; }
        public Sorting RatingSort {  get; set; }
    }
}
