namespace Community.API.Utilities.Wrappers
{
    public class HttpContextWrapper : IHttpContextWrapper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextWrapper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public T? GetFeature<T>()
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) throw new Exception("Could not access HttpContext!");

            T? feature = httpContext.Features.Get<T>();
            return feature;
        }
    }
}
