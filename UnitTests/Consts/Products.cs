using Business.DTO;
using Business.Enums;
using DAL.Entities;
using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Consts
{
    public static class Products
    {
        public static Product TestProduct1 = new()
        {
            Id = 1,
            Name = "Genshin Impact",
            Platform = AvailablePlatforms.PC,
            TotalRating = 87,
            Genre = AvailableGenres.Adventure,
            Rating = AgeRating.PEGI16,
            Logo = null,
            Background = null,
            Price = 1,
            Count = 150
        };

        public static Product TestProduct2 = new()
        {
            Id = 2,
            Name = "Ultrakill",
            Platform = AvailablePlatforms.PC,
            TotalRating = 99,
            Genre = AvailableGenres.FPS,
            Rating = AgeRating.PEGI18,
            Logo = null,
            Background = null,
            Price = 99,
            Count = 99
        };

        public static TopPlatformDto TopTest1 = new()
        {
            Platform = "PC",
            Count = 5
        };
        public static TopPlatformDto TopTest2 = new()
        {
            Platform = "XBOX",
            Count = 3
        };
        public static TopPlatformDto TopTest3 = new()
        {
            Platform = "PlayStation",
            Count = 1
        };
    }
}
