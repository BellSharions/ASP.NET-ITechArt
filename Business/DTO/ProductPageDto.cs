using Business.Enums;
using DAL.Entities;
using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
