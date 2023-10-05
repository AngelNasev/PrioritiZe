using Microsoft.EntityFrameworkCore;
using PrioritiZe.Web.Data;
using PrioritiZe.Web.Repository.Interface;

namespace PrioritiZe.Web.Repository.Implementation
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Models.DomainModels.Task> tasks;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
            tasks = _context.Set<Models.DomainModels.Task>();
        }

        public async Task<IEnumerable<Models.DomainModels.Task>> GetAll()
        {
            return await tasks.ToListAsync();
        }

        public async Task<IEnumerable<Models.DomainModels.Task>> GetFullTasks()
        {
            return await tasks
                .Include(t => t.TaskMembers)
                .Include("TaskMembers.Member")
                .Include(t => t.Comments)
                .Include("Comments.Author")
                .ToListAsync();
        }

        public async Task<Models.DomainModels.Task> Get(Guid? id)
        {
            return await tasks.SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Models.DomainModels.Task> GetFull(Guid? id)
        {
            return await tasks
                .Include(t => t.TaskMembers)
                .Include("TaskMembers.Member")
                .Include(t => t.Comments)
                .Include("Comments.Author")
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Models.DomainModels.Task> Insert(Models.DomainModels.Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }
            await tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<Models.DomainModels.Task> Update(Models.DomainModels.Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<Models.DomainModels.Task> Delete(Models.DomainModels.Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }
            tasks.Remove(task);
            await _context.SaveChangesAsync();
            return task;
        }
    }
}
