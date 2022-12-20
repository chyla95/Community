using Community.Domain.Models;
using Community.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Community.Infrastructure.Services
{
    internal class CustomerService : UserService<Customer>, ICustomerService
    {
        public CustomerService(DataContext dataContext) : base(dataContext) { }

        public async Task ConvertToEmployeeAsync(Customer entityBefore, Employee entityAfter)
        {
            if (entityBefore is Employee) throw new NotSupportedException($"Invalid entity type!");

            _dataContext.Entry(entityBefore).State = EntityState.Deleted;
            _dataContext.Entry(entityAfter).State = EntityState.Added;
            await _dataContext.SaveChangesAsync();
        }

        protected override IQueryable<Customer> CreateQuery(DbSet<Customer> dbSet)
        {
            IQueryable<Customer> query = base.CreateQuery(dbSet);

            return query;
        }
    }
}