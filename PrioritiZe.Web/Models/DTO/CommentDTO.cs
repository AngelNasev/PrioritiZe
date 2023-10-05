using PrioritiZe.Web.Models.DomainModels;

namespace PrioritiZe.Web.Models.DTO
{
    public class CommentDTO
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string Timestamp { get; set; }

        public UserDTO Author { get; set; }
    }
}
