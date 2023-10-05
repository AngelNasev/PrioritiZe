using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrioritiZe.Web.Models.DomainModels;
using PrioritiZe.Web.Models.DTO;
using PrioritiZe.Web.Repository.Interface;
using PrioritiZe.Web.Services.Interface;

namespace PrioritiZe.Web.Services.Implementation
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IRepository<ProjectMember> _projectMemberRepository;
        private readonly IRepository<Models.DomainModels.Task> _taskRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectsService(IProjectRepository projectRepository, IRepository<ProjectMember> projectMemberRepository, IRepository<Models.DomainModels.Task> taskRepository, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _projectRepository = projectRepository;
            _projectMemberRepository = projectMemberRepository;
            _taskRepository = taskRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IEnumerable<Project>> GetAll()
        {
            return await _projectRepository.GetAll();
        }

        public async Task<ProjectDTO> Get(Guid id)
        {
            var project = await _projectRepository.GetFull(id);
            var request = _httpContextAccessor.HttpContext.Request;

            var projectDto = new ProjectDTO
            {
                Id = project.Id.ToString(),
                Title = project.Title,
                DateStarted = project.DateStarted.ToString("dd MMMM yyyy HH:mm"),
                ProjectMembers = project.ProjectMembers
                        .Where(pm => pm.Member != null)
                        .Select(pm => new UserDTO
                        {
                            Id = pm.Member.Id.ToString(),
                            UserName = pm.Member.UserName,
                            ProfilePictureSrc = string.Format("{0}://{1}{2}/Images/{3}",
                                request.Scheme, request.Host, request.PathBase, pm.Member.ProfilePictureSrc)
                        })
                        .ToList(),
                Tasks = project.Tasks
                    .Where(pt => pt.Project != null)
                    .Select(pt => new BasicTaskDTO
                    {
                        Id = pt.Id.ToString(),
                        Description = pt.Description,
                        Status = pt.Status

                    }).ToList()
            };
            return projectDto;
        }

        public async Task<Project> Put(Guid id, Project project)
        {
            var existingProject = await _projectRepository.Get(id);

            if (existingProject == null)
            {
                return null;
            }

            try
            {
                await _projectRepository.Update(project);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return existingProject;
        }
        public async Task<Project> Post(CreateProjectDTO projectDto)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Title = projectDto.Title,
                DateStarted = DateTime.Now,
            };
            var newProject = await _projectRepository.Insert(project);

            if (projectDto.ProjectMembers != null && projectDto.ProjectMembers.Any())
            {

                foreach (var memberId in projectDto.ProjectMembers)
                {
                    var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == memberId);

                    if (user != null)
                    {
                        var projectMember = new ProjectMember
                        {
                            Id = Guid.NewGuid(),
                            ProjectId = newProject.Id,
                            Project = newProject,
                            MemberId = memberId,
                            Member = user
                        };

                        await _projectMemberRepository.Insert(projectMember);

                    }
                    else
                    {
                        throw new Exception($"User with ID {memberId} not found.");
                    }
                }
            }
            return newProject;
        }

        public async Task<Project> Delete(Guid id)
        {
            var projectToDelete = await _projectRepository.GetFull(id);

            if (projectToDelete == null)
            {
                return null;
            }

            foreach (var projectMember in projectToDelete.ProjectMembers)
            {
                await _projectMemberRepository.Delete(projectMember);
            }

            foreach (var projectTask in projectToDelete.Tasks)
            {
                await _taskRepository.Delete(projectTask);
            }

            return await _projectRepository.Delete(projectToDelete);
        }

        public async Task<IEnumerable<ProjectDTO>> GetUserProjects(string id)
        {
            var projects = await _projectRepository.GetFullProjects();
            var request = _httpContextAccessor.HttpContext.Request;

            var projectsDTO = projects
                .Where(p => p.ProjectMembers.Any(pm => pm.MemberId == id))
                .Select(p => new ProjectDTO
                {
                    Id = p.Id.ToString(),
                    Title = p.Title,
                    DateStarted = p.DateStarted.ToString("dd MMMM yyyy HH:mm"),
                    ProjectMembers = p.ProjectMembers
                        .Where(pm => pm.Member != null)
                        .Select(pm => new UserDTO
                        {
                            Id = pm.Id.ToString(),
                            UserName = pm.Member.UserName,
                            ProfilePictureSrc = string.Format("{0}://{1}{2}/Images/{3}",
                                request.Scheme, request.Host, request.PathBase, pm.Member.ProfilePictureSrc)
                        })
                        .ToList(),
                })
                .ToList();

            return projectsDTO;
        }
    }
}
