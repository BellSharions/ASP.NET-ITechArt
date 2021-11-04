
using System;

namespace Business.DTO
{
    public class UserInfoDto : IEquatable<UserInfoDto>
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AdressDelivery { get; set; }

        public bool Equals(UserInfoDto other)
        {
            if (UserName == other.UserName &&
                PhoneNumber == other.PhoneNumber &&
                Email == other.Email &&
                AdressDelivery == other.AdressDelivery)
                return true;
            return false;
        }
    }
}
