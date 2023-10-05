using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrioritiZe.Web.Models.DomainModels;
using PrioritiZe.Web.Models.DTO;
using PrioritiZe.Web.Services.Implementation;
using PrioritiZe.Web.Services.Interface;

namespace PrioritiZe.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _usersService.GetAll();
            if (users == null || !users.Any())
            {
                return NotFound();
            }

            return Ok(users);
        }

        // GET api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _usersService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("UserProfile/{id}")]
        public async Task<ActionResult<UserDetailsDTO>> GetUserProfile(string id)
        {
            var userProfile = await _usersService.GetUserProfile(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return Ok(userProfile);
        }

        [HttpGet("UserTasks/{id}")]
        public async Task<ActionResult<UserTasksDTO>> GetUserTasks(string id)
        {
            var userTasks = await _usersService.GetUserTasks(id);

            if (userTasks == null)
            {
                return NotFound();
            }

            return Ok(userTasks);
        }
    }
}
