using PrioritiZe.Web.Models.Enum;

namespace PrioritiZe.Web.Models.DTO
{
    public class UserDetailsDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureSrc { get; set; }
        public JobTitle JobTitle { get; set; }
        public int NumProjects { get; set; }
        public int NumTasks { get; set; }
        public int NumComments { get; set; }
    }
}
