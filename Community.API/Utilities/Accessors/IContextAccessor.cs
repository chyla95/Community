using System.Security.Claims;
using Community.Domain.Models.Abstract;

namespace Community.API.Utilities.Accessors
{
    public interface IContextAccessor
    {
        IEnumerable<Claim> GetAuthorizationClaims();
        void SetUser(User user);
        T GetUser<T>() where T : User;
    }
}
