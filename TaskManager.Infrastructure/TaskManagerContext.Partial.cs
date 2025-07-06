using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.Configurations;

namespace TaskManager.Infrastructure
{
    public partial class TaskManagerContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TaskListConfiguration());
            modelBuilder.ApplyConfiguration(new TaskListUserConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
