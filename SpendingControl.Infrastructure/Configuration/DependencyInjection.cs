using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpendingControl.Infrastructure.Persistence;
using SpendingControl.Domain.Interfaces.Repositories;
using SpendingControl.Infrastructure.Repositories;

namespace SpendingControl.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Use configured connection string key 'Default'
            string? connectionString = configuration.GetConnectionString("Default");

            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sql =>
                {
                    sql.EnableRetryOnFailure(
                        maxRetryCount: 8,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
            });


            // Repositories
            services.AddScoped<ISpendTypeRepository, SpendTypeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMonetaryFundRepository, MonetaryFundRepository>();
            services.AddScoped<IBudgetRepository, BudgetRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IDepositRepository, DepositRepository>();

            return services;
        }
    }
}
