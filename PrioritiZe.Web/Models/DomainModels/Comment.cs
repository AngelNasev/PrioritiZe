using System.ComponentModel.DataAnnotations;

namespace PrioritiZe.Web.Models.DomainModels
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public string AuthorId { get; set; }
        public virtual User Author { get; set; }

        public Guid TaskId { get; set; }
        public virtual Task Task { get; set; }
    }
}
