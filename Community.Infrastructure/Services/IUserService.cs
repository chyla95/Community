using Community.Domain.Models;

namespace Community.Infrastructure.Services
{
    public interface IUserService<T> : IService<T> where T : User
    {
        Task<T?> GetOneAsync(string email);
        Task<bool> IsEmailTaken(string email, int? userId = null);
    }
}
