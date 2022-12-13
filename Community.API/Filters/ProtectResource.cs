using Community.API.Utilities.Accessors;
using Community.API.Utilities.Exceptions;
using Community.Domain.Models;
using Community.Domain.Models.Abstract;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Community.API.Filters
{
    public class ProtectResource : IAsyncActionFilter
    {
        private readonly IContextAccessor _contextAccessor;

        public string? Name { get; set; }

        public ProtectResource(IContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!_contextAccessor.IsUserAuthenticated<User>()) throw new HttpUnauthorizedException();

            Employee user = _contextAccessor.GetUser<Employee>();

            await next();
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizationAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable { get; set; }
        public string? Name { get; set; }


        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            ProtectResource? protectResource = serviceProvider.GetService<ProtectResource>();
            if (protectResource == null) throw new NullReferenceException(nameof(protectResource));
            if (string.IsNullOrEmpty(Name)) return protectResource;

            protectResource.Name = Name;
            return protectResource;
        }
    }
}
