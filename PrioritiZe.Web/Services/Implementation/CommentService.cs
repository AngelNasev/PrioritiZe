using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrioritiZe.Web.Models.DomainModels;
using PrioritiZe.Web.Models.DTO;
using PrioritiZe.Web.Repository.Interface;
using PrioritiZe.Web.Services.Interface;

namespace PrioritiZe.Web.Services.Implementation
{
    public class CommentService : ICommentsService
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly UserManager<User> _userManager;

        public CommentService(IRepository<Comment> commentRepository, ITaskRepository taskRepository, UserManager<User> userManager)
        {
            _commentRepository = commentRepository;
            _taskRepository = taskRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            return await _commentRepository.GetAll();

        }
        public async Task<Comment> Get(Guid id)
        {
            var comment = await _commentRepository.Get(id);

            return comment;
        }

        public async Task<Comment> Put(Guid id, Comment comment)
        {
            var existingComment = await _commentRepository.Get(id);

            if (existingComment == null)
            {
                return null;
            }

            try
            {
                await _commentRepository.Update(comment);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return existingComment;
        }

        public async Task<Comment> Post(CreateCommentDTO commentDto)
        {
            var author = await _userManager.FindByIdAsync(commentDto.authorId);
            var task = await _taskRepository.Get(new Guid(commentDto.taskId));

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Content = commentDto.Content,
                Timestamp = DateTime.Now,
                AuthorId = commentDto.authorId,
                Author = author,
                TaskId = new Guid(commentDto.taskId),
                Task = task,
            };

            return await _commentRepository.Insert(comment);
        }
    }
}
