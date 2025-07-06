using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Models;

namespace TaskManager.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users", "TM", e => e.HasComment("Таблиця користувачiв"));

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Id)
                  .HasColumnName("ID");

            entity.Property(u => u.Name).HasMaxLength(100).IsRequired();
        }
    }
}
