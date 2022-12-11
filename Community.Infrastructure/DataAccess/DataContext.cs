using Community.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Community.Infrastructure.DataAccess
{
    internal class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Staff> Staff { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Customer> Customers { get; set; }
    }
}
// dotnet ef migrations add AddStaffAndRoles --project .\Community.Infrastructure\ -s .\Community.API\
// dotnet ef database update --project .\Community.Infrastructure\ -s .\Community.API\