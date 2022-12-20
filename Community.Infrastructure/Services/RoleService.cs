using Community.Domain.Models;
using Community.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Community.Infrastructure.Services
{
    internal class RoleService : Service<Role>, IRoleService
    {
        public RoleService(DataContext dataContext) : base(dataContext) { }

        public async Task<Role?> GetByNameAsync(string name)
        {
            IQueryable<Role> query = CreateQuery(_dbSet);

            Role? role = await query.SingleOrDefaultAsync(e => e.Name == name);
            return role;
        }
        public async Task<bool> IsNameTakenAsync(string name, int? roleId = null)
        {
            IQueryable<Role> query = CreateQuery(_dbSet);

            Role? role;
            if (roleId != null) role = await query.SingleOrDefaultAsync(u => (u.Name == name) && (u.Id != roleId));
            else role = await query.SingleOrDefaultAsync(u => u.Name == name);

            if (role != null) return true;
            return false;
        }

        protected override IQueryable<Role> CreateQuery(DbSet<Role> dbSet)
        {
            IQueryable<Role> query = base.CreateQuery(dbSet);

            return query;
        }
    }
}