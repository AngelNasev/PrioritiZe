using Microsoft.AspNetCore.Mvc;
using PrioritiZe.Web.Models.DomainModels;
using PrioritiZe.Web.Models.DTO;

namespace PrioritiZe.Web.Services.Interface
{
    public interface IUsersService
    {
        public Task<IEnumerable<User>> GetAll();
        public Task<User> Get(string id);
        public Task<UserDetailsDTO> GetUserProfile(string id);
        public Task<UserTasksDTO> GetUserTasks(string id);
    }
}
