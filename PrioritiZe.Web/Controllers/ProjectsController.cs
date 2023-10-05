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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _projectsService;
        

        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var projects = await _projectsService.GetAll();

            if (projects == null || !projects.Any())
            {
                return NotFound();
            }

            return Ok(projects);
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(Guid id)
        {
            var project = await _projectsService.Get(id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // PUT: api/Projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(Guid id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            var updatedProject = await _projectsService.Put(id, project);

            if (updatedProject == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Projects
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject([FromForm]CreateProjectDTO projectDto)
        {
            if (projectDto == null)
            {
                return BadRequest("Invalid project data.");
            }

            try
            {
                var newProject = await _projectsService.Post(projectDto);
            }
            catch (Exception ex)
            {
                return Problem("Error creating project: " + ex.Message);
            }
            return NoContent();
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid project id.");
            }

            try
            {
                var deletedProject = await _projectsService.Delete(id);
            }
            catch (Exception ex)
            {
                return Problem("Error deleting project: " + ex.Message);
            }
            return NoContent();
        }

        [HttpGet("UserProjects/{id}")]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetUserProjects(string id)
        {
            var projectsDTO = await _projectsService.GetUserProjects(id);

            return Ok(projectsDTO);
        }
    }
}
