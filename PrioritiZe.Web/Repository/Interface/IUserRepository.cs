using PrioritiZe.Web.Models.DomainModels;

namespace PrioritiZe.Web.Repository.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<IEnumerable<User>> GetFullUsers();
        Task<User> Get(string? id);
        Task<User> GetFull(string? id);
        Task<User> Insert(User user);
        Task<User> Update(User user);
        Task<User> Delete(User user);
    }
}
