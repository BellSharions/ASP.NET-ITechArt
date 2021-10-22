using DAL.Entities;
using System.Collections.Generic;

namespace Business.DTO
{
    public class ProductPageDto
    {
        public int ProductAmount {  get; set; }
        public int PageSize { get; set; }
        public int PageNumber {  get; set; }
        public List<Product> Data {  get; set;}
    }
}
