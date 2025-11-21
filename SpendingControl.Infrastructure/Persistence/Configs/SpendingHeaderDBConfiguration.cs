using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Infrastructure.Persistence.Configs
{
    public class SpendingHeaderConfiguration : IEntityTypeConfiguration<SpendingHeader>
    {
        public void Configure(EntityTypeBuilder<SpendingHeader> b)
        {
            b.HasKey(h => h.Id);
            b.Property(h => h.Date).IsRequired();
            b.Property(h => h.MerchantName).HasMaxLength(200);
            b.Property(h => h.Observations).HasMaxLength(1000);
            b.Property(h => h.DocumentType).IsRequired();

            // Skip TotalAmount - Is computed from details in domain
            b.Ignore(h => h.TotalAmount);

            b.Property(h => h.UserId).IsRequired();
            b.HasOne<MonetaryFund>().WithMany().HasForeignKey(h => h.MonetaryFundId).OnDelete(DeleteBehavior.Restrict);

            b.HasMany(h => h.Details)
             .WithOne(d => d.ExpenseHeader)
             .HasForeignKey(d => d.ExpenseHeaderId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
