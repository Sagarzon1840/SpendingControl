using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Infrastructure.Persistence.Configs
{
    public class BudgetDBConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.UserId).IsRequired();
            b.Property(x => x.SpendTypeId).IsRequired();
            b.Property(x => x.Year).IsRequired();
            b.Property(x => x.Month).IsRequired();
            b.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();

            b.HasIndex(x => new { x.UserId, x.SpendTypeId, x.Year, x.Month }).IsUnique();

            b.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            b.HasOne<SpendType>().WithMany().HasForeignKey(x => x.SpendTypeId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
