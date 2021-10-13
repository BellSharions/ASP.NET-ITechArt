using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class User : IdentityUser<int>
    {
        public string AdressDelivery {  get; set; }
    }
}
