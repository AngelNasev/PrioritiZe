namespace PrioritiZe.Web.Models.DTO
{
    public class CreateCommentDTO
    {
        public string Content { get; set; }

        public string authorId { get; set; }
        public string taskId { get; set; }
    }
}
