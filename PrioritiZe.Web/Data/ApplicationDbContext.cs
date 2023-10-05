using Microsoft.EntityFrameworkCore;
using PrioritiZe.Web.Models.DomainModels;

namespace PrioritiZe.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; }
        public virtual DbSet<Models.DomainModels.Task> Tasks { get; set; }
        public virtual DbSet<TaskMember> TaskMembers { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Project>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            
            builder.Entity<Models.DomainModels.Task>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Comment>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Project>()
                .HasMany(p => p.ProjectMembers)
                .WithOne(pm => pm.Project)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectMember>()
                .HasOne<Project>(pm => pm.Project)
                .WithMany(pm => pm.ProjectMembers)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProjectMember>()
                .HasOne<User>(pm => pm.Member)
                .WithMany(pm => pm.ProjectMembers)
                .HasForeignKey(pm => pm.MemberId);

            builder.Entity<Models.DomainModels.Task>()
                .HasOne<Project>(p => p.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Models.DomainModels.Task>()
                .HasMany<TaskMember>(t => t.TaskMembers)
                .WithOne(tm => tm.Task)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Models.DomainModels.Task>()
                .HasMany<Comment>(t => t.Comments)
                .WithOne(c => c.Task)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TaskMember>()
                .HasOne<Models.DomainModels.Task>(tm => tm.Task)
                .WithMany(tm => tm.TaskMembers)
                .HasForeignKey(tm => tm.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TaskMember>()
                .HasOne<User>(tm => tm.Member)
                .WithMany(tm => tm.TaskMembers)
                .HasForeignKey(tm => tm.MemberId);

            builder.Entity<Comment>()
                .HasOne<User>(c => c.Author)
                .WithMany(u => u.AuthoredComments)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne<Models.DomainModels.Task>(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
