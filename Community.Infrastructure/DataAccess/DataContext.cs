using Community.Domain.Models;
using Community.Domain.Models.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Community.Infrastructure.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Role>()
            //    .HasMany(p => p.Permissions)
            //    .WithOne()
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasMany(p => p.Roles)
                .WithMany();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Role> Roles { get; set; }
    }
}
// dotnet ef migrations add MigrationName --project .\Community.Infrastructure\ -s .\Community.API\
// dotnet ef database update --project .\Community.Infrastructure\ -s .\Community.API\