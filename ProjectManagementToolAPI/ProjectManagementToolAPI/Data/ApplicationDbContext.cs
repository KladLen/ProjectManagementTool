using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagementToolAPI.Models;

namespace ProjectManagementToolAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProjectModel>().HasOne(p => p.Mananger).WithMany(u => u.Projects).OnDelete(DeleteBehavior.Restrict).HasForeignKey(p => p.ManagerId);
            builder.Entity<TaskModel>().HasOne(t => t.Creator).WithMany(u => u.CreatedTasks).OnDelete(DeleteBehavior.Restrict).HasForeignKey(t => t.CreatorId);
            builder.Entity<TaskModel>().HasOne(t => t.Assignee).WithMany(u => u.AssignedTasks).OnDelete(DeleteBehavior.Restrict).HasForeignKey(t => t.AssigneeId);
        }
    }
}
