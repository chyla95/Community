using Community.Domain.Models;
using Community.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Community.Infrastructure.Services
{
    internal class Service<T> : IService<T> where T : Entity
    {
        protected readonly DataContext _dataContext;
        protected readonly DbSet<T> _dbSet;

        public Service(DataContext dataContext)
        {
            _dataContext = dataContext;
            _dbSet = dataContext.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetManyAsync()
        {
            IQueryable<T> query = CreateQuery(_dbSet);

            IEnumerable<T> users = await query.ToListAsync();
            return users;
        }
        public virtual async Task<T?> GetAsync(int id)
        {
            IQueryable<T> query = CreateQuery(_dbSet);

            T? user = await query.SingleOrDefaultAsync(e => e.Id == id);
            return user;
        }
        public virtual async Task AddAsync(T user)
        {
            _dbSet.Add(user);
            await _dataContext.SaveChangesAsync();
        }
        public virtual async Task UpdateAsync(T user)
        {
            _dbSet.Update(user);
            await _dataContext.SaveChangesAsync();
        }
        public virtual async Task RemoveAsync(T user)
        {
            _dbSet.Remove(user);
            await _dataContext.SaveChangesAsync();
        }

        protected virtual IQueryable<T> CreateQuery(DbSet<T> dbSet)
        {
            IQueryable<T> query = dbSet;

            return query;
        }
    }
}
