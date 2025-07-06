using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Models;

namespace TaskManager.Infrastructure
{
    public partial class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<TaskList> TaskLists { get; set; }
        public virtual DbSet<TaskListUser> TaskListUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}