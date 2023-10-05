using PrioritiZe.Web.Models;

namespace PrioritiZe.Web.Repository.Interface
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(Guid? id);
        Task<T> Insert(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
    }
}
