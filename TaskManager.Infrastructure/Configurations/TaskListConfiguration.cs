using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Models;

namespace TaskManager.Infrastructure.Configurations
{
    public class TaskListConfiguration : IEntityTypeConfiguration<TaskList>
    {
        public void Configure(EntityTypeBuilder<TaskList> entity)
        {
            entity.ToTable("TaskList", "TM", e => e.HasComment("Таблиця задач"));
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Id).HasColumnName("ID");
            entity.Property(t => t.Title).HasMaxLength(255).IsRequired();
            entity.Property(t => t.DateTimeCreated).IsRequired();
            entity.Property(t => t.WhoCreated).IsRequired();

            entity.HasOne(t => t.WhoCreatedNavigation)
                  .WithMany(u => u.TaskLists)
                  .HasForeignKey(t => t.WhoCreated)
                  .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
