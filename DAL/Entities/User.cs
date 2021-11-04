using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class User : IdentityUser<int>, IEquatable<User>
    {
        public string AdressDelivery {  get; set; }
        public ICollection<ProductRating> Ratings { get; set; }
        public ICollection<Order> Orders { get; set; }

        public bool Equals(User other)
        {
            if (Id == other.Id &&
                UserName == other.UserName &&
                NormalizedUserName == other.NormalizedUserName &&
                Email == other.Email &&
                EmailConfirmed == other.EmailConfirmed &&
                AdressDelivery == other.AdressDelivery &&
                PasswordHash == other.PasswordHash &&
                PhoneNumberConfirmed == other.PhoneNumberConfirmed &&
                PasswordHash == other.PasswordHash &&
                PhoneNumber == other.PhoneNumber)
                return true;
            return false;
        }
    }
}
