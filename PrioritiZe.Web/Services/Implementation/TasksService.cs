using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrioritiZe.Web.Models.DomainModels;
using PrioritiZe.Web.Models.DTO;
using PrioritiZe.Web.Repository.Interface;
using PrioritiZe.Web.Services.Interface;

namespace PrioritiZe.Web.Services.Implementation
{
    public class TasksService : ITasksService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<TaskMember> _taskMemberRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TasksService(ITaskRepository taskRepository, IRepository<Project> projectRepository, IRepository<TaskMember> taskMemberRepository, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _taskMemberRepository = taskMemberRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Models.DomainModels.Task>> GetAll()
        {
            return await _taskRepository.GetAll();
        }

        public async Task<TaskDetailsDTO> Get(Guid id)
        {
            var task = await _taskRepository.GetFull(id);
            var request = _httpContextAccessor.HttpContext.Request;

            var taskDto = new TaskDetailsDTO
            {
                Id = task.Id.ToString(),
                Description = task.Description,
                Status = task.Status,
                CreatedAt = task.CreatedAt.ToString("dd MMMM yyyy HH:mm"),
                UpdatedAt = task.UpdatedAt.ToString("dd MMMM yyyy HH:mm"),
                TaskMembers = task.TaskMembers
                    .Where(tm => tm.Task != null)
                    .Select(m => new UserDTO
                    {
                        Id = m.Member.Id.ToString(),
                        UserName = m.Member.UserName,
                        ProfilePictureSrc = string.Format("{0}://{1}{2}/Images/{3}",
                                request.Scheme, request.Host, request.PathBase, m.Member.ProfilePictureSrc),

                    })
                    .ToList(),
                Comments = task.Comments
                    .Where(tc => tc.Task != null)
                    .OrderByDescending(c => c.Timestamp)
                    .Select(c => new CommentDTO
                    {
                        Id = c.Id.ToString(),
                        Content = c.Content,
                        Timestamp = c.Timestamp.ToString("dd MMMM yyyy HH:mm"),
                        Author = new UserDTO
                        {
                            Id = c.Author.Id.ToString(),
                            UserName = c.Author.UserName,
                            ProfilePictureSrc = string.Format("{0}://{1}{2}/Images/{3}",
                                request.Scheme, request.Host, request.PathBase, c.Author.ProfilePictureSrc),
                        }
                    })
                    .ToList(),
                Creator = new UserDTO
                {
                    Id = task.CreatorId.ToString(),
                    UserName = task.Creator.UserName,
                    ProfilePictureSrc = string.Format("{0}://{1}{2}/Images/{3}",
                                request.Scheme, request.Host, request.PathBase, task.Creator.ProfilePictureSrc)
                },

            };

            return taskDto;
        }

        public async Task<Models.DomainModels.Task> Put(Guid id, UpdateStatusDTO updateStatusDto)
        {
            var existingTask = await _taskRepository.Get(id);

            if (existingTask == null)
            {
                return null;
            }

            Models.Enum.TaskStatus newStatus;
            switch (updateStatusDto.taskStatus)
            {
                case 0:
                    newStatus = Models.Enum.TaskStatus.Pending;
                    break;
                case 1:
                    newStatus = Models.Enum.TaskStatus.InProgress;
                    break;
                case 2:
                    newStatus = Models.Enum.TaskStatus.Completed;
                    break;
                default:
                    return null;
            }

            existingTask.Status = newStatus;
            existingTask.UpdatedAt = DateTime.Now;
            try
            {
                await _taskRepository.Update(existingTask);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return existingTask;
        }

        public async Task<Models.DomainModels.Task> Post(CreateTaskDTO taskDto)
        {
            var project = await _projectRepository.Get(new Guid(taskDto.ProjectId));
            var creator = await _userManager.FindByIdAsync(taskDto.CreatorId);

            var task = new Models.DomainModels.Task
            {
                Id = Guid.NewGuid(),
                Description = taskDto.Description,
                Status = Models.Enum.TaskStatus.Pending,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ProjectId = project.Id,
                Project = project,
                CreatorId = creator.Id,
                Creator = creator,

            };
            var newTask = await _taskRepository.Insert(task);

            if (taskDto.TaskMembers != null && taskDto.TaskMembers.Any())
            {

                foreach (var memberId in taskDto.TaskMembers)
                {
                    var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == memberId);

                    if (user != null)
                    {
                        var taskMember = new TaskMember
                        {
                            Id = Guid.NewGuid(),
                            TaskId = newTask.Id,
                            Task = newTask,
                            MemberId = memberId,
                            Member = user
                        };

                        await _taskMemberRepository.Insert(taskMember);

                    }
                    else
                    {
                        throw new Exception($"User with ID {memberId} not found.");
                    }
                }
            }
            return newTask;
        }
    }
}
