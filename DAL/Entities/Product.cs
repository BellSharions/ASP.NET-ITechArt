using Business.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Product
    {
        public int Id {  get; set; }
        public string Name {  get; set; }
        public AvailablePlatforms Platform { get; set; }
        public string DateCreated {  get; set; }
        public int TotalRating {  get; set; }
        public Product(string name, AvailablePlatforms platform, string dateCreated, int totalRating)
        {
            Name = name;
            Platform = platform;
            DateCreated = dateCreated;
            TotalRating = totalRating;
        }
    }
    
}
