namespace PrioritiZe.Web.Models.DTO
{
    public class CreateProjectDTO
    {
        public string Title { get; set; }
        public DateTime DateStarted { get; set; }
        public virtual List<string> ProjectMembers { get; set; }
    }
}
