namespace PrioritiZe.Web.Models.DomainModels
{
    public class ProjectMember : BaseEntity
    {
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public string MemberId { get; set; }
        public virtual User Member { get; set; }
    }
}
