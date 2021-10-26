using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class ProductListFilteredDto
    {
        public List<Product> Products {  get; set;}
        public int Total {  get; set;}
    }
}
