using Microsoft.AspNetCore.Identity;
using PrioritiZe.Web.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PrioritiZe.Web.Models.DomainModels
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ProfilePictureSrc { get; set; } = string.Empty;
        public JobTitle JobTitle { get; set; }

        public virtual ICollection<ProjectMember> ProjectMembers { get; set; }

        public virtual ICollection<TaskMember> TaskMembers { get; set; }

        public virtual ICollection<Comment> AuthoredComments { get; set; }
    }
}
