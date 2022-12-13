using Community.Domain.Models.Abstract;

namespace Community.API.Utilities.Accessors
{
    public interface IContextAccessor
    {
        T GetUser<T>() where T : User;
        bool IsUserAuthenticated<T>() where T : User;
    }
}
