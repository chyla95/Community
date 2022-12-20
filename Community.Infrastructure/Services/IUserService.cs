using Community.Domain.Models.Abstract;

namespace Community.Infrastructure.Services
{
    public interface IUserService<T> : IService<T> where T : User
    {
        Task<T?> GetByEmailAsync(string email);
        Task<bool> IsEmailTakenAsync(string email, int? userId = null);
    }
}
