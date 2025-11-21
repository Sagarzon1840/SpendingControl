using Microsoft.EntityFrameworkCore;
using SpendingControl.Domain.Entities;
using System;

namespace SpendingControl.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<SpendType> SpendTypes => Set<SpendType>();
        public DbSet<MonetaryFund> MonetaryFunds => Set<MonetaryFund>();
        public DbSet<Budget> Budgets => Set<Budget>();
        public DbSet<SpendingHeader> SpendingHeaders => Set<SpendingHeader>();
        public DbSet<SpendingDetail> SpendingDetails => Set<SpendingDetail>();
        public DbSet<Deposit> Deposits => Set<Deposit>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply all IEntityTypeConfiguration<> from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
