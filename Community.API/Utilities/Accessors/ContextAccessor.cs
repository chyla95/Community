using System.Security.Claims;
using Community.Domain.Models.Abstract;

namespace Community.API.Utilities.Accessors
{
    public class ContextAccessor : IContextAccessor
    {
        private readonly HttpContext _httpContext;

        public ContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext? httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null) throw new NullReferenceException(nameof(httpContext));
            _httpContext = httpContext;
        }

        public IEnumerable<Claim> GetAuthorizationClaims()
        {
            ClaimsPrincipal claimsPrincipal = _httpContext.User;
            IEnumerable<Claim> claims = claimsPrincipal.Claims;

            return claims;
        }
        public void SetUser(User user)
        {
            IDictionary<object, object?> contextItems = _httpContext.Items;
            contextItems.Add(Constants.HttpContextItemKeys.USER, user);
        }
        public T GetUser<T>() where T : User
        {
            IDictionary<object, object?> contextItems = _httpContext.Items;
            T? user = (T?)contextItems[Constants.HttpContextItemKeys.USER];
            if(user == null) throw new NullReferenceException(nameof(user));

            return user;
        }
    }
}
