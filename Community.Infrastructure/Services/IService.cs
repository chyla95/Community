using Community.Domain.Models;

namespace Community.Infrastructure.Services
{
    public interface IService <T> where T : Entity
    {
        Task<IEnumerable<T>> GetManyAsync();
        Task<T?> GetOneAsync(int id);
        Task AddOneAsync(T entity);
        Task UpdateOneAsync(T entity);
        Task RemoveOneAsync(T entity);
    }
}
