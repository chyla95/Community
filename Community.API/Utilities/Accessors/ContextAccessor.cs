using Community.API.Utilities.Exceptions;
using Community.Domain.Models;

namespace Community.API.Utilities.Accessors
{
    public class ContextAccessor : IContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public T GetUser<T>() where T : User
        {
            T? user = (T?)GetContextFeature<User>();
            if (user == null) throw new NullReferenceException(nameof(user));

            return user;
        }
        public bool IsUserAuthenticated<T>() where T : User
        {
            T? user;
            try
            {
                user = (T?)GetContextFeature<User>();
            }
            catch
            {
                return false;
            }

            if (user == null) return false;
            return true;
        }

        private T? GetContextFeature<T>() where T : class
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) throw new NullReferenceException(nameof(httpContext));

            T? feature = httpContext.Features.Get<T>();
            return feature;
        }
    }
}
