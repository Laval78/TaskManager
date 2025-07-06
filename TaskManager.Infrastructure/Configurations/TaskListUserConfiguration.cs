using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Models;

namespace TaskManager.Infrastructure.Configurations
{
    public class TaskListUserConfiguration : IEntityTypeConfiguration<TaskListUser>
    {
        public void Configure(EntityTypeBuilder<TaskListUser> entity)
        {
            entity.ToTable("TaskListUsers", "TM", e => e.HasComment("Таблиця зв'язку користувачiв та задач"));

            entity.HasKey(tu => tu.Id);

            entity.Property(tu => tu.Id).HasColumnName("ID");
            entity.Property(tu => tu.IdTask).HasColumnName("ID_Task");
            entity.Property(tu => tu.IdUser).HasColumnName("ID_User");

            entity.HasOne(tu => tu.IdTaskListNavigation).WithMany(t => t.TaskListUsers)
                  .HasForeignKey(tu => tu.IdTask)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_TaskListUser_Task");

            entity.HasOne(tu => tu.IdUserNavigation).WithMany(u => u.TaskListUsers)
                  .HasForeignKey(tu => tu.IdUser)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_TaskListUser_User");
        }
    }
}
