using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Infrastructure.Persistence.Configs
{
    public class SpendingDetailDBConfiguration : IEntityTypeConfiguration<SpendingDetail>
    {
        public void Configure(EntityTypeBuilder<SpendingDetail> b)
        {
            b.HasKey(d => d.Id);
            b.Property(d => d.ExpenseHeaderId).IsRequired();
            b.Property(d => d.ExpenseTypeId).IsRequired();
            b.Property(d => d.Amount).HasColumnType("decimal(18,2)").IsRequired();

            b.HasOne(d => d.ExpenseType)
             .WithMany()
             .HasForeignKey(d => d.ExpenseTypeId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(d => d.ExpenseHeader)
             .WithMany(h => h.Details)
             .HasForeignKey(d => d.ExpenseHeaderId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
