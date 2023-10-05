using Microsoft.AspNetCore.Mvc;
using PrioritiZe.Web.Models.DomainModels;
using PrioritiZe.Web.Models.DTO;

namespace PrioritiZe.Web.Services.Interface
{
    public interface IProjectsService
    {
        Task<IEnumerable<Project>> GetAll();
        Task<ProjectDTO> Get(Guid id);
        public Task<Project> Put(Guid id, Project project);
        public Task<Project> Post(CreateProjectDTO projectDto);
        public Task<Project> Delete(Guid id);
        public Task<IEnumerable<ProjectDTO>> GetUserProjects(string id);
    }
}
