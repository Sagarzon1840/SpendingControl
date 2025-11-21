using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Infrastructure.Persistence.Configs
{
    public class UserDBConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> b)
        {
            b.HasKey(u => u.Id);
            b.Property(u => u.Name).IsRequired().HasMaxLength(100);
            b.Property(u => u.UserName).IsRequired().HasMaxLength(50);
            b.Property(u => u.DateRegistered).IsRequired();
            b.Property(u => u.IsActive).HasDefaultValue(true);
        }
    }
}
