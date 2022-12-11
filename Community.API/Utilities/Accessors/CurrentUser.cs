using Community.API.Utilities.Wrappers;
using Community.Domain.Models;

namespace Community.API.Utilities.Accessors
{
    public class CurrentUser<T> : ICurrentUser<T> where T : User
    {
        private readonly IHttpContextWrapper _httpContextWrapper;

        public CurrentUser(IHttpContextWrapper httpContextWrapper)
        {
            _httpContextWrapper = httpContextWrapper;
        }

        public T GetUser()
        {
            T? user = (T?)_httpContextWrapper.GetFeature<User>();
            if (user == null) throw new Exception("Could not access current user data!");

            return user;
        }
    }
}
