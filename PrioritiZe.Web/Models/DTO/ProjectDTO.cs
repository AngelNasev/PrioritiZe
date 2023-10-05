namespace PrioritiZe.Web.Models.DTO
{
    public class ProjectDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string DateStarted { get; set; }
        public List<UserDTO> ProjectMembers { get; set; }
        public List<BasicTaskDTO> Tasks { get; set; }
    }
}
