using Community.Domain.Models.Abstract;

namespace Community.Infrastructure.Services
{
    public interface IService <T> where T : Entity
    {
        Task<IEnumerable<T>> GetManyAsync();
        Task<T?> GetAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
    }
}
