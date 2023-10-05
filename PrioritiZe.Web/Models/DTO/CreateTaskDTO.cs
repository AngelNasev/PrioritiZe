namespace PrioritiZe.Web.Models.DTO
{
    public class CreateTaskDTO
    {
        public string Description { get; set; }
        public string CreatorId { get; set; }
        public string ProjectId { get; set; }
        public virtual List<string> TaskMembers { get; set; }
    }
}
