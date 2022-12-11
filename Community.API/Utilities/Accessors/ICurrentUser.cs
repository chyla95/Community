using Community.Domain.Models;

namespace Community.API.Utilities.Accessors
{
    public interface ICurrentUser<T> where T : User
    {
        T GetUser();
    }
}
