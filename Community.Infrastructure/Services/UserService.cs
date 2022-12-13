using Community.Domain.Models.Abstract;
using Community.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Community.Infrastructure.Services
{
    internal class UserService<T> : Service<T>, IUserService<T> where T : User
    {
        public UserService(DataContext dataContext) : base(dataContext) { }

        public async Task<T?> GetAsync(string email)
        {
            IQueryable<T> query = CreateQuery(_dbSet);

            T? user = await query.SingleOrDefaultAsync(e => e.Email == email);
            return user;
        }
        public async Task<bool> IsEmailTaken(string email, int? userId = null)
        {
            IQueryable<T> query = CreateQuery(_dbSet);

            T? user;
            if (userId != null) user = await query.SingleOrDefaultAsync(u => (u.Email == email) && (u.Id != userId));
            else user = await query.SingleOrDefaultAsync(u => u.Email == email);

            if (user != null) return true;
            return false;
        }

        protected override IQueryable<T> CreateQuery(DbSet<T> dbSet)
        {
            IQueryable<T> query = base.CreateQuery(dbSet)
            // Includes for Employee
                .Include("Roles")
                .Include("Roles.Permissions");


            return query;
        }
    }
}