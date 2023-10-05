using Microsoft.AspNetCore.Mvc;
using PrioritiZe.Web.Models.DomainModels;
using PrioritiZe.Web.Models.DTO;

namespace PrioritiZe.Web.Services.Interface
{
    public interface ICommentsService
    {
        Task<IEnumerable<Comment>> GetAll();
        Task<Comment> Get(Guid id);
        Task<Comment> Put(Guid id, Comment comment);
        Task<Comment> Post(CreateCommentDTO commentDto);
    }
}
