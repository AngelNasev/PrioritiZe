using PrioritiZe.Web.Models.DomainModels;

namespace PrioritiZe.Web.Repository.Interface
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAll();
        Task<IEnumerable<Project>> GetFullProjects();
        Task<Project> Get(Guid? id);
        Task<Project> GetFull(Guid? id);
        Task<Project> Insert(Project project);
        Task<Project> Update(Project project);
        Task<Project> Delete(Project project);
    }
}
