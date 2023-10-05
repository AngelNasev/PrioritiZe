using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PrioritiZe.Web.Models.DomainModels;

namespace PrioritiZe.Web.Repository.Interface
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Models.DomainModels.Task>> GetAll();
        Task<IEnumerable<Models.DomainModels.Task>> GetFullTasks();
        Task<Models.DomainModels.Task> Get(Guid? id);
        Task<Models.DomainModels.Task> GetFull(Guid? id);
        Task<Models.DomainModels.Task> Insert(Models.DomainModels.Task task);
        Task<Models.DomainModels.Task> Update(Models.DomainModels.Task task);
        Task<Models.DomainModels.Task> Delete(Models.DomainModels.Task task);
    }
}
