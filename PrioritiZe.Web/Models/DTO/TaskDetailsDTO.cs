using PrioritiZe.Web.Models.DomainModels;

namespace PrioritiZe.Web.Models.DTO
{
    public class TaskDetailsDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public Enum.TaskStatus Status { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public List<UserDTO> TaskMembers { get; set; }
        public List<CommentDTO> Comments { get; set; }
        public UserDTO Creator { get; set; }
    }
}
