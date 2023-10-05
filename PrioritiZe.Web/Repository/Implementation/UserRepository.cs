using Microsoft.EntityFrameworkCore;
using PrioritiZe.Web.Data;
using PrioritiZe.Web.Models.DomainModels;
using PrioritiZe.Web.Repository.Interface;

namespace PrioritiZe.Web.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<User> users;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            users = _context.Set<User>();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetFullUsers()
        {
            return await users
                .Include(u => u.ProjectMembers)
                .Include("ProjectMembers.Project")
                .Include(u => u.TaskMembers)
                .Include("TaskMembers.Task")
                .Include(u => u.AuthoredComments)
                .ToListAsync();
        }
        public async Task<User> Get(string? id)
        {
            return await users.SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetFull(string? id)
        {
            return await users
                .Include(u => u.ProjectMembers)
                .Include("ProjectMembers.Project")
                .Include(u => u.TaskMembers)
                .Include("TaskMembers.Task")
                .Include(u => u.AuthoredComments)
                .SingleOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User> Insert(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            await users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> Update(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> Delete(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
