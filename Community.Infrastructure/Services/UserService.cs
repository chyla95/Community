using Community.Domain.Models;
using Community.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Community.Infrastructure.Services
{
    internal class UserService<T> : IUserService<T> where T : User
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<T> _dbSet;

        public UserService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _dbSet = dataContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetManyAsync()
        {
            IQueryable<T> query = _dbSet.Select(e => e);

            IEnumerable<T> users = await query.ToListAsync();
            return users;
        }
        public async Task<T?> GetOneAsync(int id)
        {
            IQueryable<T> query = _dbSet.Where(e => e.Id == id);

            T? user = await query.SingleOrDefaultAsync();
            return user;
        }
        public async Task<T?> GetOneAsync(string email)
        {
            IQueryable<T> query = _dbSet.Where(e => e.Email == email);

            T? user = await query.SingleOrDefaultAsync();
            return user;
        }
        public async Task AddOneAsync(T user)
        {
            _dbSet.Add(user);
            await _dataContext.SaveChangesAsync();
        }
        public async Task UpdateOneAsync(T user)
        {
            _dbSet.Update(user);
            await _dataContext.SaveChangesAsync();
        }
        public async Task RemoveOneAsync(T user)
        {
            _dbSet.Remove(user);
            await _dataContext.SaveChangesAsync();
        }
        public async Task<bool> IsEmailTaken(string email, int? userId = null)
        {
            T? user;
            if (userId != null) user = await _dbSet.SingleOrDefaultAsync(u => (u.Email == email) && (u.Id != userId));
            else user = await _dbSet.SingleOrDefaultAsync(u => u.Email == email);

            if (user != null) return true;
            return false;
        }
    }
}
