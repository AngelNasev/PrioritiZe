using System.ComponentModel.DataAnnotations;

namespace PrioritiZe.Web.Models.DomainModels
{
    public class Project : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public DateTime DateStarted { get; set; }

        public virtual ICollection<ProjectMember> ProjectMembers { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
