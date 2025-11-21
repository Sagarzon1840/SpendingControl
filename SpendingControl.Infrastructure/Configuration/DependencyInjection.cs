using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpendingControl.Infrastructure.Persistence;

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

            //services.AddScoped<>();

            return services;
        }
    }
}
