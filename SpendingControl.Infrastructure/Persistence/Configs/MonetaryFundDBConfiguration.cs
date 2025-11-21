using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Infrastructure.Persistence.Configs
{
    public class MonetaryFundDBConfiguration : IEntityTypeConfiguration<MonetaryFund>
    {
        public void Configure(EntityTypeBuilder<MonetaryFund> b)
        {
            b.HasKey(f => f.Id);
            b.Property(f => f.Name).IsRequired().HasMaxLength(100);
            b.Property(f => f.AccountNumberOrDescription).HasMaxLength(200);
            b.Property(f => f.InitialBalance).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
            b.Property(f => f.CurrentBalance).HasColumnType("decimal(18,2)").HasDefaultValue(0m);

            // Owner relationship
            b.Property(f => f.UserId).IsRequired();
            b.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade);
        }
    }
}
