using Microsoft.AspNetCore.Mvc;
using PrioritiZe.Web.Models.DTO;

namespace PrioritiZe.Web.Services.Interface
{
    public interface ITasksService
    {
        public Task<IEnumerable<Models.DomainModels.Task>> GetAll();
        public Task<TaskDetailsDTO> Get(Guid id);
        public Task<Models.DomainModels.Task> Put(Guid id, UpdateStatusDTO updateStatusDto);
        public Task<Models.DomainModels.Task> Post(CreateTaskDTO taskDto);
    }
}
