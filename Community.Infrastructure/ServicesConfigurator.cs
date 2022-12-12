using Community.Domain.Models;
using Community.Infrastructure.DataAccess;
using Community.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Community.Infrastructure
{
    public static class ServicesConfigurator
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string databaseConnectionString)
        {
            // Database
            services.AddDbContext<DataContext>(options => options.UseSqlServer(databaseConnectionString));

            // Services
            services.AddScoped<IUserService<User>, UserService<User>>();
            services.AddScoped<IUserService<Staff>, UserService<Staff>>();
            services.AddScoped<IUserService<Customer>, UserService<Customer>>();

            return services;
        }
    }
}
