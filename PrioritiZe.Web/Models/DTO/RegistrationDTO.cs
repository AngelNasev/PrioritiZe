using PrioritiZe.Web.Models.Enum;

namespace PrioritiZe.Web.Models.DTO
{
    public class RegistrationDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureSrc { get; set; }
        public JobTitle JobTitle { get; set; }

        public IFormFile? ProfilePictureFile { get; set; }
    }
}
