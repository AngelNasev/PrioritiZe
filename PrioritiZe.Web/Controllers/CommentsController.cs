using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrioritiZe.Web.Models.DomainModels;
using PrioritiZe.Web.Models.DTO;
using PrioritiZe.Web.Services.Interface;

namespace PrioritiZe.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CommentsController : ControllerBase
    {

        private readonly ICommentsService _commentService;

        public CommentsController(ICommentsService commentsService)
        {
            _commentService = commentsService;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            var comments = await _commentService.GetAll();

            if (comments == null || !comments.Any())
            {
                return NotFound();
            }

            return Ok(comments);
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(Guid id)
        {
            var comment = await _commentService.Get(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(Guid id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            var updatedComment = await _commentService.Put(id, comment);

            if (updatedComment == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Comments
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment([FromForm]CreateCommentDTO commentDto)
        {
            if (commentDto == null)
            {
                return BadRequest("Invalid comment data.");
            }

            try
            {
                var newComment = await _commentService.Post(commentDto);
            }
            catch (Exception ex)
            {
                return Problem("Error creating task: " + ex.Message);
            }

            return NoContent();
        }
    }
}
