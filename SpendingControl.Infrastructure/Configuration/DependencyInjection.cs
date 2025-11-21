using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpendingControl.Infrastructure.Persistence;
using SpendingControl.Domain.Interfaces.Repositories;
using SpendingControl.Infrastructure.Repositories;
using SpendingControl.Domain.Interfaces.Services;
using SpendingControl.Application.UseCases;
using SpendingControl.Application.Interfaces;
using SpendingControl.Infrastructure.Services;

namespace SpendingControl.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

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

            // Application services
            services.AddScoped<IUserService, UserService>();

            // Domain services (use infrastructure implementation)
            services.AddScoped<Domain.Interfaces.Services.IExpenseService, Infrastructure.Services.ExpenseService>();

            return services;
        }
    }
}
