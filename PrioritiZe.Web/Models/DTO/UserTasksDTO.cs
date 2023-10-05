namespace PrioritiZe.Web.Models.DTO
{
    public class UserTasksDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public List<BasicTaskDTO> PendingTasks { get; set; }
        public List<BasicTaskDTO> InProgressTasks { get; set; }
        public List<BasicTaskDTO> CompletedTasks { get; set; }
    }
}
