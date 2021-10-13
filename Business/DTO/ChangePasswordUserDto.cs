
namespace Business.DTO
{
    public class ChangePasswordUserDto
    {
        public int Id { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
