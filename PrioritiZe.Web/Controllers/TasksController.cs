
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrioritiZe.Web.Models.DomainModels;
using PrioritiZe.Web.Models.DTO;
using PrioritiZe.Web.Models.Enum;
using PrioritiZe.Web.Repository.Interface;
using PrioritiZe.Web.Services.Implementation;
using PrioritiZe.Web.Services.Interface;

namespace PrioritiZe.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;

        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.DomainModels.Task>>> GetTasks()
        {
            var tasks = await _tasksService.GetAll();

            if (tasks == null || !tasks.Any())
            {
                return NotFound();
            }

            return Ok(tasks);
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.DomainModels.Task>> GetTask(Guid id)
        {
            var taskDTO = await _tasksService.Get(id);

            if (taskDTO == null)
            {
                return NotFound();
            }

            return Ok(taskDTO);
        }

        // PUT: api/Tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(Guid id,[FromForm] UpdateStatusDTO updateStatusDto)
        {
            if (id != new Guid(updateStatusDto.taskId))
            {
                return BadRequest();
            }

            var updatedTask = await _tasksService.Put(id, updateStatusDto);

            if (updatedTask == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.DomainModels.Task>> PostTask([FromForm]CreateTaskDTO taskDto)
        {
            if (taskDto == null)
            {
                return BadRequest("Invalid task data.");
            }

            try
            {
                var newTask = await _tasksService.Post(taskDto);

            }
            catch (Exception ex)
            {
                return Problem("Error creating task: " + ex.Message);
            }

            return NoContent();
        }
    }
}
