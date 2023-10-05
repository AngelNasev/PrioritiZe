namespace PrioritiZe.Web.Models.DTO
{
    public class BasicTaskDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public Enum.TaskStatus Status { get; set; }
    }
}
