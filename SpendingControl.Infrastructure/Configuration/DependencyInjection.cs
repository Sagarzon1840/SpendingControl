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

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, options =>
                {
                    options.EnableRetryOnFailure(maxRetryCount: 5,maxRetryDelay: TimeSpan.FromSeconds(15), errorNumbersToAdd: null);
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
