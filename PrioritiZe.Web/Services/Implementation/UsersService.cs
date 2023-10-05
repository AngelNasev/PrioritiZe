using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrioritiZe.Web.Models.DomainModels;
using PrioritiZe.Web.Models.DTO;
using PrioritiZe.Web.Repository.Interface;
using PrioritiZe.Web.Services.Interface;

namespace PrioritiZe.Web.Services.Implementation
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersService(UserManager<User> userManager, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();
            var request = _httpContextAccessor.HttpContext.Request;

            var modifiedUsers = new List<User>();

            foreach (var user in users)
            {
                var modifiedUser = new User
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    JobTitle = user.JobTitle,
                    ProfilePictureSrc = string.Format("{0}://{1}{2}/Images/{3}",
                        request.Scheme, request.Host, request.PathBase, user.ProfilePictureSrc)
                };
                modifiedUsers.Add(modifiedUser);
            }
            return modifiedUsers;
        }

        public async Task<User> Get(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var request = _httpContextAccessor.HttpContext.Request;

            if (user == null)
            {
                return null;
            }

            var modifiedUser = new User
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                JobTitle = user.JobTitle,
                ProfilePictureSrc = string.Format("{0}://{1}{2}/Images/{3}",
                        request.Scheme, request.Host, request.PathBase, user.ProfilePictureSrc)
            };

            return modifiedUser;
        }

        public async Task<UserDetailsDTO> GetUserProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var fullUser = await _userRepository.GetFull(id);
            var request = _httpContextAccessor.HttpContext.Request;

            if (user == null || fullUser == null)
            {
                return null;
            }

            var userDetailsDto = new UserDetailsDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePictureSrc = string.Format("{0}://{1}{2}/Images/{3}",
                                request.Scheme, request.Host, request.PathBase, user.ProfilePictureSrc),
                JobTitle = user.JobTitle,
                NumProjects = fullUser.ProjectMembers.Count(),
                NumTasks = fullUser.TaskMembers.Count(),
                NumComments = fullUser.AuthoredComments.Count(),
            };

            return userDetailsDto;
        }

        public async Task<UserTasksDTO> GetUserTasks(string id)
        {
            var fullUser = await _userRepository.GetFull(id);
            if (fullUser == null)
            {
                return null;
            }

            var userTasksDto = new UserTasksDTO
            {
                Id = fullUser.Id,
                UserName = fullUser.UserName,
                PendingTasks = fullUser.TaskMembers
                    .Where(tm => tm.Task.Status == Models.Enum.TaskStatus.Pending)
                    .Select(tm => new BasicTaskDTO
                    {
                        Id = tm.Task.Id.ToString(),
                        Description = tm.Task.Description,
                        Status = tm.Task.Status

                    })
                    .ToList(),
                InProgressTasks = fullUser.TaskMembers
                    .Where(tm => tm.Task.Status == Models.Enum.TaskStatus.InProgress)
                    .Select(tm => new BasicTaskDTO
                    {
                        Id = tm.Task.Id.ToString(),
                        Description = tm.Task.Description,
                        Status = tm.Task.Status

                    })
                    .ToList(),
                CompletedTasks = fullUser.TaskMembers
                    .Where(tm => tm.Task.Status == Models.Enum.TaskStatus.Completed)
                    .Select(tm => new BasicTaskDTO
                    {
                        Id = tm.Task.Id.ToString(),
                        Description = tm.Task.Description,
                        Status = tm.Task.Status

                    })
                    .ToList(),
            };
            return userTasksDto;
        }
    }
}
