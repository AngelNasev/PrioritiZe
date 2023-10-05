using Microsoft.EntityFrameworkCore;
using PrioritiZe.Web.Data;
using PrioritiZe.Web.Models.DomainModels;
using PrioritiZe.Web.Repository.Interface;

namespace PrioritiZe.Web.Repository.Implementation
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Project> projects;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
            projects = _context.Set<Project>();
        }

        public async Task<IEnumerable<Project>> GetAll()
        {
            return await projects.ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetFullProjects()
        {
            return await projects
                .Include(p => p.ProjectMembers)
                .Include("ProjectMembers.Member")
                .ToListAsync();
        }

        public async Task<Project> Get(Guid? id)
        {
            return await projects.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project> GetFull(Guid? id)
        {
            return await projects
                .Include(p => p.ProjectMembers)
                .Include("ProjectMembers.Member")
                .Include(p => p.Tasks)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project> Insert(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            await projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<Project> Update(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return project;
        }
        public async Task<Project> Delete(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            projects.Remove(project);
            await _context.SaveChangesAsync();
            return project;
        }
    }
}
