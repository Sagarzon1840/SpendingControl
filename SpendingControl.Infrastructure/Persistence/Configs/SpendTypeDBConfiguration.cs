using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Infrastructure.Persistence.Configs
{
    public class SpendTypeDBConfiguration : IEntityTypeConfiguration<SpendType>
    {
        public void Configure(EntityTypeBuilder<SpendType> b)
        {
            b.HasKey(s => s.Id);
            b.Property(s => s.Code).IsRequired();
            b.Property(s => s.UserId).IsRequired();
            b.HasIndex(s => new { s.UserId, s.Code }).IsUnique();
            b.Property(s => s.Name).IsRequired().HasMaxLength(200);
            b.Property(s => s.IsActive).HasDefaultValue(true);
        }
    }
}
