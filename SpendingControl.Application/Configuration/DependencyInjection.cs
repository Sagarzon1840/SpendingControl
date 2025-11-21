using Microsoft.Extensions.DependencyInjection;
using SpendingControl.Application.UseCases;
using SpendingControl.Application.Interfaces;

namespace SpendingControl.Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISpendTypeService, SpendTypeService>();
            services.AddScoped<IMonetaryFundService, MonetaryFundService>();
            services.AddScoped<IBudgetService, BudgetService>();
            services.AddScoped<IDepositService, DepositService>();

            return services;
        }
    }
}
