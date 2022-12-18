using Community.Domain.Models;
using Community.Domain.Models.Abstract;
using Community.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Community.Infrastructure.Services
{
    internal class EmployeeService : UserService<Employee>, IEmployeeService
    {
        public EmployeeService(DataContext dataContext) : base(dataContext) { }

        protected override IQueryable<Employee> CreateQuery(DbSet<Employee> dbSet)
        {
            IQueryable<Employee> query = base.CreateQuery(dbSet)
                .Include(e => e.Roles);

            return query;
        }
    }
}