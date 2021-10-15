using DAL.Entities.Roles;
using System.ComponentModel.DataAnnotations;

namespace ASP_NET.Models
{
    public class UserModel //future implementation is required
    {
        public string UserName {  get; set; }
        [Phone]
        public string PhoneNumber {  get; set; }
        public string Email { get; set; }
        public string Password {  get; set; }
        public string AdressDelivery {  get; set; }

    }
}
