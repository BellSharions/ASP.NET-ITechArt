﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ProductRating
    {
        public int ProductId {  get; set; }
        public int UserId {  get; set; }
        public int Rating {  get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
    }
}
