using System.ComponentModel.DataAnnotations;

namespace PrioritiZe.Web.Models.DomainModels
{
    public class Task : BaseEntity
    {
        public string Description { get; set; }
        public Enum.TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public virtual ICollection<TaskMember> TaskMembers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }


        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public string CreatorId { get; set; }
        public virtual User Creator { get; set; }
    }
}
