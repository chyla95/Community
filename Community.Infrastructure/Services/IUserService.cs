using Community.Domain.Models.Abstract;

namespace Community.Infrastructure.Services
{
    public interface IUserService<T> : IService<T> where T : User
    {
        Task<T?> GetAsync(string email);
        Task<bool> IsEmailTaken(string email, int? userId = null);
    }
}
