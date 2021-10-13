using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ASP_NET.Models
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class CreateUserModel
    {
        [SwaggerSchema("Specified user name")]
        public string UserName {  get; set; }
        [SwaggerSchema("User phone number")]
        [Phone]
        public string PhoneNumber {  get; set; }
        [SwaggerSchema("User email", Nullable = false)]
        public string Email { get; set; }
        [SwaggerSchema("User unhashed password")]
        public string Password {  get; set; }
        [SwaggerSchema("User address to delivery")]
        public string AdressDelivery {  get; set; }

    }
}
