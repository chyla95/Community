using Community.Domain.Models;

namespace Community.Infrastructure.Services
{
    public interface IRoleService : IService<Role>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<bool> IsNameTakenAsync(string name, int? roleId = null);
    }
}
