using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class User : IdentityUser<int>
    {
        public string AdressDelivery {  get; set; }
        public ICollection<ProductRating> Ratings { get; set; }
    }
}
