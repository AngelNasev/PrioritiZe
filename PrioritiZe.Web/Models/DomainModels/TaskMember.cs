namespace PrioritiZe.Web.Models.DomainModels
{
    public class TaskMember : BaseEntity
    {
        public Guid TaskId { get; set; }
        public virtual Task Task { get; set; }

        public string MemberId { get; set; }
        public virtual User Member { get; set; }
    }
}
