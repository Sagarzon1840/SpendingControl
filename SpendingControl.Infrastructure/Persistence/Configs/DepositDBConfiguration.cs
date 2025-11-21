using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Infrastructure.Persistence.Configs
{
    public class DepositDBConfiguration : IEntityTypeConfiguration<Deposit>
    {
        public void Configure(EntityTypeBuilder<Deposit> b)
        {
            b.HasKey(d => d.Id);
            b.Property(d => d.Date).IsRequired();
            b.Property(d => d.Amount).HasColumnType("decimal(18,2)").IsRequired();

            b.HasOne<MonetaryFund>().WithMany().HasForeignKey(d => d.FundId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
