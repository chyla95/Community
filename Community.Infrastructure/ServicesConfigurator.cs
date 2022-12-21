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
            if (services == null) throw new ArgumentNullException(nameof(services));

            // Database
            services.AddDbContext<DataContext>(options => options.UseSqlServer(databaseConnectionString));

            // Services
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IRoleService, RoleService>();

            return services;
        }
    }
}
